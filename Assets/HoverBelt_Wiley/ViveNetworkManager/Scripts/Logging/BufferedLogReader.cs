using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ViveNetworkManagerProto;
using System;
using System.IO;
using Google.Protobuf;
using System.Threading;

// Reads log entries from an internal buffer, which is populated asynchronously as needed.
// This may or may not be faster than reading from file synchronously during playback as in IncrementalLogReader.
public class BufferedLogReader : LogReader {

	public override DateTime startTime { get; protected set; }

	private int minBufferSize;
	private int maxBufferSize;

	public BufferedLogReader(string _filename, int _minBufferSize = 100, int _maxBufferSize = 500) : base(_filename)
	{
		minBufferSize = _minBufferSize;
		maxBufferSize = _maxBufferSize;

		using (FileStream fileStream = new FileStream (filename, FileMode.Open)) {
			LogEntryProto firstLogEntry = LogEntryProto.Parser.ParseDelimitedFrom (fileStream);
			startTime = DateTime.FromBinary (firstLogEntry.DatetimeBin);
		}
	}

	public override IEnumerator<LogEntry> GetEnumerator() {
		return new BufferedLogReaderEnumerator (filename,minBufferSize,maxBufferSize);
	}

	private class BufferedLogReaderEnumerator : IEnumerator<LogEntry>
	{
		private Queue<LogEntry> buffer;

		private FileStream fileStream;
		private string filename;

		private int minBufferSize;
		private int maxBufferSize;

		private int useCount = 0;
		private bool isEnqueueing = false;
		private bool reachedEndOfFile = false;

		private System.Object enqueueLock = new System.Object();
		private System.Object dequeueLock = new System.Object();

		private LogEntry prevDequeue;

		public BufferedLogReaderEnumerator(string _filename, int _minBufferSize, int _maxBufferSize)
		{
			if(_minBufferSize < 1)
				throw new ArgumentException(String.Format("Minimum buffer size {0} is less than 1",_minBufferSize));

			if(_minBufferSize > _maxBufferSize)
				throw new ArgumentException(String.Format("Minimum buffer size {0} exceeds maximum buffer size {1}",_minBufferSize,_maxBufferSize));
			
			filename = _filename;
			minBufferSize = _minBufferSize;
			maxBufferSize = _maxBufferSize;

			Initialize();
		}

		private void Initialize() {
			fileStream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			buffer = new Queue<LogEntry>();
			PopulateQueue();
		}


		// Fill buffer continuously while it is below max size.
		private void PopulateQueue()
		{
			int currentUseCount = useCount;
			lock (enqueueLock) {
				
				// If the file has been closed since this thread requested the enqueue lock, stop early
				if (currentUseCount != useCount)
					return;
				
				while (buffer.Count < maxBufferSize) {
					if (fileStream.Position >= fileStream.Length) {
						reachedEndOfFile = true;
						break;
					}
					buffer.Enqueue (new LogEntry (LogEntryProto.Parser.ParseDelimitedFrom (fileStream)));
				}
			}
		}


		// Start a new thread to populate the buffer if there is not already one doing so.
		private void PopulateQueueAsync() {

			if (isEnqueueing)
				return;

			isEnqueueing = true;

			// Start thread to do the actual queueing.
			Thread enqueueThread = new Thread (new ThreadStart (() => 
				{
					PopulateQueue();
					isEnqueueing = false;
				}
			));
			enqueueThread.Start ();
		}



		object System.Collections.IEnumerator.Current { get { return Current; } }

		public LogEntry Current
		{
			get
			{
				if(prevDequeue == null)
					throw new InvalidOperationException ();

				return prevDequeue;
			}
		}

		public bool MoveNext()
		{
			lock (dequeueLock) {

				if (buffer.Count == 0) {
					if (reachedEndOfFile) {
						// If buffer is empty and we have hit EOF, we consider the enumeration complete.
						prevDequeue = null;
						return false;
					}

					// If buffer is empty and we haven't hit EOF, fill buffer sychronously before proceeding.
					Debug.Log ("BufferedLogReader buffer is empty; consider increasing minimum size");
					PopulateQueue();
				}
				prevDequeue = buffer.Dequeue ();

				// If buffer is under-full and we haven't hit EOF, start filling buffer asynchronously.
				if (buffer.Count < minBufferSize && !reachedEndOfFile) {
					PopulateQueueAsync ();
				}
				return true;
			}
		}

		public void Reset()
		{
			Dispose ();
			Initialize ();
		}

		public void Dispose()
		{
			lock (enqueueLock) {
				useCount++;
				fileStream.Close ();
			}
		}
	}
}
