using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ViveNetworkManagerProto;

// Stores new log entries in an in-memory list which is written to file on close.
public class BasicLogWriter : LogWriter {
	private List<LogEntry> logList;
	public override bool isClosed { get; protected set; }

	public BasicLogWriter(string _filename) : base(_filename)
	{
		logList = new List<LogEntry> ();
		isClosed = false;
	}

	public override void Write(LogEntry entry)
	{
		logList.Add (entry);
	}

	public override void Close()
	{
		isClosed = true;
		LogEntryProtoList protoList = new LogEntryProtoList ();
		for (int i = 0; i < logList.Count; i++) {
			protoList.LogEntry.Add (logList [i].ToProto ());
		}
		ViveUtils.WriteProtoFile (protoList, filename);
	}
}
