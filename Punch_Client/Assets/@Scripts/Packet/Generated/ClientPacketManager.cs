using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

public enum MsgId
{
	S_Connected = 1,
	C_EnterLobby = 2,
	S_EnterLobby = 3,
	S_LeaveLobby = 4,
	C_MakeWaitingRoom = 5,
	S_MakeWaitingRoom = 6,
	S_SpawnWaitingRoomUI = 7,
	S_DespawnWaitingRoomUI = 8,
	C_EnterWaitingRoom = 9,
	S_EnterWaitingRoom = 10,
	C_Ready = 11,
	S_Ready = 12,
	S_LeaveWaitingRoom = 13,
	C_DestroyWaitingRoom = 14,
	S_DestroyWaitingRoom = 15,
	C_EnterGame = 16,
	S_EnterGame = 17,
	S_LeaveGame = 18,
	S_Spawn = 19,
	S_Despawn = 20,
	C_Move = 21,
	S_Move = 22,
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
		_onRecv.Add((ushort)MsgId.S_EnterLobby, MakePacket<S_EnterLobby>);
		_handler.Add((ushort)MsgId.S_EnterLobby, PacketHandler.S_EnterLobbyHandler);		
		_onRecv.Add((ushort)MsgId.S_LeaveLobby, MakePacket<S_LeaveLobby>);
		_handler.Add((ushort)MsgId.S_LeaveLobby, PacketHandler.S_LeaveLobbyHandler);		
		_onRecv.Add((ushort)MsgId.S_MakeWaitingRoom, MakePacket<S_MakeWaitingRoom>);
		_handler.Add((ushort)MsgId.S_MakeWaitingRoom, PacketHandler.S_MakeWaitingRoomHandler);		
		_onRecv.Add((ushort)MsgId.S_SpawnWaitingRoomUI, MakePacket<S_SpawnWaitingRoomUI>);
		_handler.Add((ushort)MsgId.S_SpawnWaitingRoomUI, PacketHandler.S_SpawnWaitingRoomUIHandler);		
		_onRecv.Add((ushort)MsgId.S_DespawnWaitingRoomUI, MakePacket<S_DespawnWaitingRoomUI>);
		_handler.Add((ushort)MsgId.S_DespawnWaitingRoomUI, PacketHandler.S_DespawnWaitingRoomUIHandler);		
		_onRecv.Add((ushort)MsgId.S_EnterWaitingRoom, MakePacket<S_EnterWaitingRoom>);
		_handler.Add((ushort)MsgId.S_EnterWaitingRoom, PacketHandler.S_EnterWaitingRoomHandler);		
		_onRecv.Add((ushort)MsgId.S_Ready, MakePacket<S_Ready>);
		_handler.Add((ushort)MsgId.S_Ready, PacketHandler.S_ReadyHandler);		
		_onRecv.Add((ushort)MsgId.S_LeaveWaitingRoom, MakePacket<S_LeaveWaitingRoom>);
		_handler.Add((ushort)MsgId.S_LeaveWaitingRoom, PacketHandler.S_LeaveWaitingRoomHandler);		
		_onRecv.Add((ushort)MsgId.S_DestroyWaitingRoom, MakePacket<S_DestroyWaitingRoom>);
		_handler.Add((ushort)MsgId.S_DestroyWaitingRoom, PacketHandler.S_DestroyWaitingRoomHandler);		
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