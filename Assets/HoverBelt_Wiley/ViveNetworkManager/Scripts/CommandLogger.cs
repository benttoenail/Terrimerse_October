using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using ViveNetworkManagerProto;

// Server only helper class for recording commands.
using Google.Protobuf;


public class CommandLogger : MonoBehaviour {

	private static LogWriter currentWriter;

	public static bool isLogging {
		get { return currentWriter != null && !currentWriter.isClosed; }
	}
			
	public static string filename { get; private set; }

	// Main function to log commands. Send player object and all the parameters of the calling function.
	// Details are found via reflection
	public static void Log(NetworkBehaviour obj, params object[] list)
	{
		if (!isLogging) return;

		//Note, add a check if the param list matches the calling function

		// Use reflection to fill the missing details.
		Log(new LogEntry(obj, ViveUtils.GetClassName(1), ViveUtils.GetMethodName(1), list));
	}

	// Log an entry directly. Shouldn't be used unles implementing a custom protobuf. (See LOG_TRANSFORMS in ViveAvatar)
	public static void Log(LogEntry cmd)
	{
		if (!isLogging) return;

		currentWriter.Write(cmd);
	}

	public static void StartLogging(string _filename)
	{
		// TODO Prevent sharing violation
		currentWriter = new IncrementalLogWriter (_filename);
	}

	public static void Save()
	{
		if (!isLogging) return;

		currentWriter.Close ();

		Debug.Log("Command Log saved to " + filename);
	}

	public static LogReader Load(string filename)
	{
		// Prevent sharing violation
		if (currentWriter != null && filename == currentWriter.filename && !currentWriter.isClosed)
			throw new InvalidOperationException ("File \""+filename+"\" is currently being written to");
		
		return new BufferedLogReader (filename);
	}
}
