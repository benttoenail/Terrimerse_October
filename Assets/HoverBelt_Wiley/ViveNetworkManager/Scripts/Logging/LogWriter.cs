using UnityEngine;
using System.Collections;

public abstract class LogWriter {
	public string filename {get; protected set;}

	public abstract bool isClosed { get; protected set; }

	public LogWriter(string _filename)
	{
		filename = _filename;
	}

	public abstract void Write (LogEntry cmd);
	public abstract void Close ();
}
