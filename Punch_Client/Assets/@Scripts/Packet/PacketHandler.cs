using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Linq;
//using static Define;

class PacketHandler
{
    ///////////////////////////////////// GameServer - Client /////////////////////////////////////
    public static void S_ConnectedHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("S_Connected");
    }

    public static void S_EnterWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("EnterWaitingRoom");

        S_EnterWaitingRoom enterWaitingRoom = packet as S_EnterWaitingRoom;

        enterWaitingRoom.PlayerCount++;
    }

    public static void S_MakeRoomHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("MakeRoomHandler");

        //S_MakeRoom makeRoom = packet as S_MakeRoom;

        //RoomCard room = Managers.Object.SpawnUI(makeRoom.RoomInfo);
        
        //room.RoomId = 1;
    }

    public static void S_SpawnUIHandler(PacketSession session, IMessage packet)
    {        
        S_SpawnUI spawnPacket = packet as S_SpawnUI;

        foreach (RoomInfo room in spawnPacket.Rooms)
        {
            Managers.Object.SpawnUI(room);
        }
    }


    public static void S_EnterRoomHandler(PacketSession session, IMessage packet)
    {
        S_EnterRoom makeRoom = packet as S_EnterRoom;

        //TODO : player id만 찾아서 2명만 입장

    }    
    
    public static void S_LeaveRoomHandler(PacketSession session, IMessage packet)
    {
        S_LeaveRoom makeRoom = packet as S_LeaveRoom;
    }

    public static void S_DestroyRoomHandler(PacketSession session, IMessage packet)
    {
        S_DestroyRoom destroyRoom = packet as S_DestroyRoom;

    }

    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("EnterGame");

        S_EnterGame enterGamePacket = packet as S_EnterGame;
        MyPlayer myPlayer = Managers.Object.Spawn(enterGamePacket.MyHeroInfo);

        myPlayer.SetInfo(1);
        myPlayer.ObjectState = EObjectState.Idle;
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("LeaveGame");

        Managers.Object.Clear();
    }

    public static void S_DespawnHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("S_DespawnHandler");

        S_Despawn despawnPacket = packet as S_Despawn;
        foreach (int ObjectId in despawnPacket.ObjectIds)
        {
            Managers.Object.Despawn(ObjectId);
        }
    }

    public static void S_SpawnHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("S_SpawnHandler");

        S_Spawn spawnPacket = packet as S_Spawn;

        foreach (HeroInfo obj in spawnPacket.Heroes)
        {
            Managers.Object.Spawn(obj);
        }
    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("S_MoveHandler");

        S_Move movePacket = packet as S_Move;

        GameObject go = Managers.Object.FindById(movePacket.ObjectId);
        if (go == null)
            return;

        BaseObject bo = go.GetComponent<BaseObject>();
        if (bo == null)
            return;

        bo.PosInfo = movePacket.PosInfo;
    }
}
