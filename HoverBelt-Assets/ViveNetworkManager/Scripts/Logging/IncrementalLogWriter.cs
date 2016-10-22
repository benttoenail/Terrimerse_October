using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ViveNetworkManagerProto;
using Google.Protobuf;
using System.IO;

// Writes log entries to file one-at-a-time.
public class IncrementalLogWriter : LogWriter {
	
	public override bool isClosed { get; protected set; }
	private FileStream fileStream;

	public IncrementalLogWriter(string _filename) : base(_filename)
	{
		fileStream = new FileStream (filename, FileMode.Create, FileAccess.Write);
		isClosed = false;
	}

	public override void Write(LogEntry entry)
	{
		entry.ToProto ().WriteDelimitedTo (fileStream);
	}

	public override void Close()
	{
		isClosed = true;
		fileStream.Close ();
	}
}
