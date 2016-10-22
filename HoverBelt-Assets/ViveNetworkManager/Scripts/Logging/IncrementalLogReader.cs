using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ViveNetworkManagerProto;
using System;
using System.IO;
using Google.Protobuf;

// Reads log entries from file one-at-a-time as they are requested.
public class IncrementalLogReader : LogReader {

	public override DateTime startTime { get; protected set; }

	public IncrementalLogReader(string _filename) : base(_filename)
	{
		using (FileStream fileStream = new FileStream (filename, FileMode.Open)) {
			LogEntryProto firstLogEntry = LogEntryProto.Parser.ParseDelimitedFrom (fileStream);
			startTime = DateTime.FromBinary (firstLogEntry.DatetimeBin);
		}
	}

	public override IEnumerator<LogEntry> GetEnumerator() {
		return new IncrementalLogReaderEnumerator (filename);
	}

	private class IncrementalLogReaderEnumerator : IEnumerator<LogEntry>
	{
		private FileStream fileStream;
		private string filename;
		private LogEntry lastRead;

		private bool reachedEndOfFile = false;

		public IncrementalLogReaderEnumerator(string _filename)
		{
			filename = _filename;
			fileStream = new FileStream (filename, FileMode.Open, FileAccess.Read);

			// Note that this starts the stream position after the first entry
			bool logsExist = MoveNext();
			if(!logsExist){
				Debug.Log("Warning: empty file stream");
			}
		}

		object System.Collections.IEnumerator.Current { get { return Current; } }

		public LogEntry Current
		{
			get
			{
				if (reachedEndOfFile)
					throw new InvalidOperationException ();
				return lastRead;
			}
		}

		public bool MoveNext()
		{
			if(!reachedEndOfFile) {
				lastRead = new LogEntry (LogEntryProto.Parser.ParseDelimitedFrom (fileStream));
				reachedEndOfFile = !(fileStream.Position < fileStream.Length);
			}

			return !reachedEndOfFile;
		}

		public void Reset()
		{
			fileStream.Close ();
			fileStream = new FileStream (filename, FileMode.Open, FileAccess.Read);
			MoveNext ();
		}

		public void Dispose()
		{
			fileStream.Close ();
		}
	}
}
