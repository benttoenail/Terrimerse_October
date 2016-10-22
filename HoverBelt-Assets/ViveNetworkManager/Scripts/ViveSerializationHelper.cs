using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using ViveNetworkManagerProto;
using Google.Protobuf;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Collections.Generic;
using Google.Protobuf.Reflection;

// Stateful class containing static utility methods for protobuf serialization/deserialization/parsing.
public class ViveSerializationHelper : MonoBehaviour {
	private static bool verbose = false;

	private static Dictionary<string,MessageParser> knownParsers = new Dictionary<string,MessageParser>();

	private static BinaryFormatter binaryFormatter = new BinaryFormatter ();


	// Determine whether the specified method has a parameter protobuf in this namespace.
	public static bool ParamProtoExists(string methodName){
		return GetParser (methodName) != null;
	}

	// Get the parser for the specified method's parameter protobuf.
	public static MessageParser GetParser(string methodName){
		if (knownParsers.ContainsKey (methodName)) {
			return knownParsers [methodName];
		} else {
			// Only use reflection if we haven't already found this parser.
			MessageParser parser = FindParser (methodName);

			// Add to dictionary; note that parser may be null.
			// We assume all existing protobuf message types are present at runtime.
			knownParsers.Add (methodName, parser);

			return parser;
		}
	}

	// Find the parser for the specified method's parameter protobuf, using reflection.
	private static MessageParser FindParser(string methodName){
		if(verbose) Debug.Log ("Finding parser for parameter proto of method " + methodName);
		Assembly asm = typeof(CmdGenericParams).Assembly;

		Type myParamType = asm.GetType("ViveNetworkManagerProto."+methodName+"Params");
		if (myParamType == null) {
			if(verbose) Debug.Log ("Could not find type " + "ViveNetworkManagerProto." + methodName + "Params");
			return null;
		}

		MethodInfo parserAccessor = myParamType.GetMethod ("get_Parser");
		if (parserAccessor == null) {
			if(verbose) Debug.Log ("Could not find parser accessor in " + "ViveNetworkManagerProto." + methodName + "Params");
			return null;
		}

		if(verbose) Debug.Log ("Found parser for parameter proto of method " + methodName);
		return parserAccessor.Invoke(null,null) as MessageParser;
	}

	public static ByteString SerializeSpecificParams(string methodName, IMessage paramsProto) {
		using (MemoryStream memoryStream = new MemoryStream ()) {
			paramsProto.WriteTo (memoryStream);
			return ByteString.CopyFrom (memoryStream.ToArray());
		}
	}

	// Convert a byte array to a predefined parameter protobuf.
	public static object[] DeserializeSpecificParams(string methodName, Byte[] bytes){

		MessageParser parser = GetParser (methodName);

		if (parser == null) {
			throw new NullReferenceException ("Parser not found for params of method " + methodName);
		}

		IMessage iMessage = parser.ParseFrom (bytes);

		IList<FieldDescriptor> fields = iMessage.Descriptor.Fields.InDeclarationOrder ();

		object[] paramsList = new object[fields.Count];

		for (int i = 0; i < paramsList.Length; i++) {
			paramsList [i] = fields [i].Accessor.GetValue (iMessage);
		}
		return paramsList;
	}

	// Serialize generic parameters as byte arrays using a BinaryFormatter.
	// Each parameter must have the [Serializable] attribute.
	// Note that the use of BinaryFormatter introduces dependencies between the data and code.
	public static CmdGenericParams SerializeGenericParams(params object[] paramsList)
	{
		CmdGenericParams genericParams = new CmdGenericParams ();
		for (int i = 0; i < paramsList.Length; i++) {

			GenericParam paramElement = new GenericParam ();

			using (MemoryStream memoryStream = new MemoryStream ()) {
				binaryFormatter.Serialize (memoryStream, paramsList [i]);
				paramElement.Bin = ByteString.CopyFrom (memoryStream.ToArray ());
			}

			genericParams.CurrentParam.Add (paramElement);

		}
		return genericParams;
	}

	// Deserialize the byte arrays in a CmdGenericParams object.
	public static object[] DeserializeGenericParams(CmdGenericParams genericParams)
	{
		GenericParam[] protoParList = new GenericParam[genericParams.CurrentParam.Count];
		genericParams.CurrentParam.CopyTo (protoParList,0);

		object[] paramsList = new object[protoParList.Length];

		for (int i = 0; i < protoParList.Length; i++) {
			using (MemoryStream memoryStream = new MemoryStream ()) {
				protoParList [i].Bin.WriteTo (memoryStream);
				memoryStream.Position = 0;
				paramsList [i] = binaryFormatter.Deserialize(memoryStream);
			}
		}

		return paramsList;
	}
}