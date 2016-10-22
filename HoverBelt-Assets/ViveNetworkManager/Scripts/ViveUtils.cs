using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Google.Protobuf;

// Stateless class containing static utility methods.
using System.Collections.Generic;


public class ViveUtils : MonoBehaviour {

    /// <summary>
    /// Serializes an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializableObject"></param>
    /// <param name="fileName"></param>
    static public void SerializeObject<T>(T serializableObject, string fileName)
    {
        if (serializableObject == null) { return; }

        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(fileName);
                stream.Close();
            }
        }
        catch (Exception ex)
        {
            //Log exception here
            UnityEngine.Debug.Log("error " + ex);
        }
    }


    /// <summary>
    /// Deserializes an xml file into an object list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    static public T DeSerializeObject<T>(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) { return default(T); }

        T objectOut = default(T);

        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(T);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    objectOut = (T)serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }
        }
        catch (Exception ex)
        {
            //Log exception here
            UnityEngine.Debug.Log(ex);
        }

        return objectOut;
    }

    // Initilize an empty array of type T
    static public T[] InitializeArray<T>(int length) where T : new()
    {
        T[] array = new T[length];
        for (int i = 0; i < length; ++i)
        {
            array[i] = new T();
        }

        return array;
    }

    // Parse enum from String representation
    public static T EnumFromString<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value);
    }

    // Reflection helper. 
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetMethodName(int stackCount = 0)
    {
        StackTrace st = new StackTrace();
        StackFrame sf = st.GetFrame(1 + stackCount);

        return sf.GetMethod().Name;
    }

    // Reflection helper. 
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetClassName(int stackCount=0)
    {
        StackTrace st = new StackTrace();
        StackFrame sf = st.GetFrame(1 + stackCount);

        return sf.GetMethod().DeclaringType.Name;

    }

	// Write a single protobuf message to a file.
	public static void WriteProtoFile(Google.Protobuf.IMessage proto, string filename)
	{
		using (FileStream stream = new FileStream(filename,FileMode.Create, FileAccess.Write)) {
			proto.WriteTo (stream);
		}
	}

	// Read a single protobuf message from file using its type's parser.
	public static T ReadProtoFile<T>(MessageParser<T> parser, string filename) where T : IMessage<T>
	{
		using (FileStream stream = new FileStream (filename, FileMode.Open, FileAccess.Read)) {
			return parser.ParseFrom (stream);
		}
	}

	// Write a list of protobuf messages to file in delimited form.
	public static void WriteDelimitedProtoFile<T>(List<IMessage> protoList, string filename) {
		using (FileStream stream = new FileStream (filename, FileMode.Create, FileAccess.Write)) {
			foreach (IMessage proto in protoList) {
				proto.WriteDelimitedTo (stream);
			}
		}
	}

	// Read a list of delimited protobuf messages from file using their type's parser.
	public static List<T> ReadDelimitedProtoFile<T>(MessageParser<T> parser, string filename) where T : IMessage<T>
	{
		List<T> list = new List<T> ();
		using (FileStream stream = new FileStream (filename, FileMode.Open, FileAccess.Read)) {
			while (stream.Position < stream.Length) {
				list.Add (parser.ParseDelimitedFrom (stream));
			}
		}
		return list;
	}
}

// Extend enums to have a Next() function
public static class Extensions
{

    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }
}
