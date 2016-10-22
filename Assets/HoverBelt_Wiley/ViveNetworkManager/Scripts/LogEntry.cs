using System;
using UnityEngine.Networking;
using ViveNetworkManagerProto;
using Google.Protobuf;

public class LogEntry
{
	public string gameObjectName;
	public string networkId;
	public string className;
	public string methodName;
	public object[] parameters;
	public DateTime datetime;
	public bool manualParse;

	public LogEntry(NetworkBehaviour obj, string _className, string _methodName, object[] _parameters = null, bool _manualParse = false)
	{
		gameObjectName = obj.name;
		networkId = obj.GetComponent<NetworkIdentity>().netId.ToString();
		className = _className;
		methodName = _methodName;
		parameters = _parameters;
		datetime = System.DateTime.Now;
		manualParse = _manualParse;
	}

	public LogEntry(LogEntryProto proto)
	{
		gameObjectName = proto.GameObjectName;
		networkId = proto.NetworkId;
		className = proto.ClassName;
		methodName = proto.MethodName;

		if(proto.CmdName == LogEntryProto.Types.CmdNameEnum.CmdGeneric)
		{
			parameters = ViveSerializationHelper.DeserializeGenericParams(proto.ParamList);
		}
		else
		{
			// Does SetTransforms need special handling here?
			parameters = ViveSerializationHelper.DeserializeSpecificParams(proto.MethodName, proto.ProtoParam.ToByteArray());
		}

		datetime = DateTime.FromBinary(proto.DatetimeBin);
		manualParse = proto.ManualParse;
	}

	public LogEntryProto ToProto() {
		LogEntryProto newEntry = new LogEntryProto ();

		newEntry.GameObjectName = gameObjectName;
		newEntry.NetworkId = networkId;
		newEntry.ClassName = className;
		newEntry.MethodName = methodName;
		newEntry.DatetimeBin = datetime.ToBinary ();
		newEntry.ManualParse = manualParse;

		if (ViveSerializationHelper.ParamProtoExists(methodName))
		{
			newEntry.CmdName = ViveUtils.EnumFromString<LogEntryProto.Types.CmdNameEnum> (methodName);

			// Assume we have been passed a single parameter containing a protobuf object
			newEntry.ProtoParam = ViveSerializationHelper.SerializeSpecificParams (methodName, parameters[0] as IMessage);
		}
		else
		{
			newEntry.CmdName = LogEntryProto.Types.CmdNameEnum.CmdGeneric;
			newEntry.ParamList = ViveSerializationHelper.SerializeGenericParams (parameters);
		}
		return newEntry;
	}
}