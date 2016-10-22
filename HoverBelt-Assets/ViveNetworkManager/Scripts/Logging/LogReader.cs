using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class LogReader : IEnumerable<LogEntry>
{
	public string filename {get; protected set;}

	public abstract DateTime startTime { get; protected set; }

	public LogReader(string _filename)
	{
		filename = _filename;
	}

	public abstract IEnumerator<LogEntry> GetEnumerator ();

	IEnumerator IEnumerable.GetEnumerator () {
		return GetEnumerator ();
	}
}
