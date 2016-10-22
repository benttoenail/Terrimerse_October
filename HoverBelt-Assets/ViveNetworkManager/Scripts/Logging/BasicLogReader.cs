using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ViveNetworkManagerProto;
using System;

// Reads an entire log file into an in-memory list.
public class BasicLogReader : LogReader {
	private List<LogEntry> logList;

	public override DateTime startTime { get; protected set; }

	public BasicLogReader(string _filename) : base(_filename)
	{
		List<LogEntryProto> protoList = ViveUtils.ReadDelimitedProtoFile<LogEntryProto>(LogEntryProto.Parser, filename);
		logList = new List<LogEntry> (protoList.Count);

		for (int i = 0; i < protoList.Count; i++) {
			logList.Add (new LogEntry (protoList [i]));
		}
		startTime = logList [0].datetime;
	}

	public override IEnumerator<LogEntry> GetEnumerator()
	{
		return logList.GetEnumerator();
	}
}
