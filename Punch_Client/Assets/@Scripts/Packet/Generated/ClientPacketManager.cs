using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

public enum MsgId
{
	S_Connected = 1,
	C_MakeRoom = 2,
	S_MakeRoom = 3,
	C_EnterRoom = 4,
	S_EnterRoom = 5,
	C_DestroyRoom = 6,
	S_DestroyRoom = 7,
	C_EnterGame = 8,
	S_EnterGame = 9,
	S_LeaveGame = 10,
	S_Spawn = 11,
	S_Despawn = 12,
	C_Move = 13,
	S_Move = 14,
}

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.S_Connected, MakePacket<S_Connected>);
		_handler.Add((ushort)MsgId.S_Connected, PacketHandler.S_ConnectedHandler);		
		_onRecv.Add((ushort)MsgId.S_MakeRoom, MakePacket<S_MakeRoom>);
		_handler.Add((ushort)MsgId.S_MakeRoom, PacketHandler.S_MakeRoomHandler);		
		_onRecv.Add((ushort)MsgId.S_EnterRoom, MakePacket<S_EnterRoom>);
		_handler.Add((ushort)MsgId.S_EnterRoom, PacketHandler.S_EnterRoomHandler);		
		_onRecv.Add((ushort)MsgId.S_DestroyRoom, MakePacket<S_DestroyRoom>);
		_handler.Add((ushort)MsgId.S_DestroyRoom, PacketHandler.S_DestroyRoomHandler);		
		_onRecv.Add((ushort)MsgId.S_EnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)MsgId.S_EnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.S_LeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)MsgId.S_LeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)MsgId.S_Spawn, MakePacket<S_Spawn>);
		_handler.Add((ushort)MsgId.S_Spawn, PacketHandler.S_SpawnHandler);		
		_onRecv.Add((ushort)MsgId.S_Despawn, MakePacket<S_Despawn>);
		_handler.Add((ushort)MsgId.S_Despawn, PacketHandler.S_DespawnHandler);		
		_onRecv.Add((ushort)MsgId.S_Move, MakePacket<S_Move>);
		_handler.Add((ushort)MsgId.S_Move, PacketHandler.S_MoveHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if (CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}