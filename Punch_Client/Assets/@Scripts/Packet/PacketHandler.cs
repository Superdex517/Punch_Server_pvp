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
    }

    public static void S_EnterLobbyHandler(PacketSession session, IMessage packet)
    {
        S_EnterLobby enterLobbyPacket = packet as S_EnterLobby;

        Managers.Object.EnterLobby(enterLobbyPacket.MyHeroInfo);
    }

    public static void S_LeaveLobbyHandler(PacketSession session, IMessage packet)
    {
        S_LeaveLobby leaveLobby = packet as S_LeaveLobby;

    }

    public static void S_MakeWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        S_MakeWaitingRoom makeWaitingRoomPacket = packet as S_MakeWaitingRoom;
    }

    public static void S_SpawnWaitingRoomUIHandler(PacketSession session, IMessage packet)
    {        
        S_SpawnWaitingRoomUI spawnPacket = packet as S_SpawnWaitingRoomUI;

        foreach (RoomInfo room in spawnPacket.Rooms)
        {
            Managers.Object.SpawnUI(room);
        }
    }

    public static void S_DespawnWaitingRoomUIHandler(PacketSession session, IMessage packet)
    {

        S_DespawnWaitingRoomUI despawnUIPacket = packet as S_DespawnWaitingRoomUI;

        foreach(int roomId in despawnUIPacket.WaitingRoomIds)
        {
            Managers.Object.DespawnUI(roomId);
        }
    }

    public static void S_EnterWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        S_EnterWaitingRoom enterWaitingRoomPakcet = packet as S_EnterWaitingRoom;
        
        Managers.Object.AddWaitingRoom(enterWaitingRoomPakcet);
    }    

    public static void S_ReadyHandler(PacketSession session, IMessage packet)
    {
        S_Ready readyPacket = packet as S_Ready;
        
        //Managers.Object.readyPlayer();
    }    

    public static void S_GameStartHandler(PacketSession session, IMessage packet)
    {
        S_GameStart readyPacket = packet as S_GameStart;

        if (readyPacket.IsStart)
        {
            Managers.Scene.LoadGameScene();
        }
        //Managers.Object.readyPlayer();
    }    
    
    public static void S_LeaveWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        S_LeaveWaitingRoom leavRoomPacket = packet as S_LeaveWaitingRoom;
    }

    public static void S_DestroyWaitingRoomHandler(PacketSession session, IMessage packet)
    {
        S_DestroyWaitingRoom destroyRoom = packet as S_DestroyWaitingRoom;
        //TODO : destroy room
    }

    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;
        MyPlayer myPlayer = Managers.Object.Spawn(enterGamePacket.MyHeroInfo);

        myPlayer.SetInfo(1);
        myPlayer.ObjectState = EObjectState.Idle;
        myPlayer.SceneType = EGameSceneType.Game;

        Debug.Log($"{myPlayer.ObjectId}, {myPlayer.SceneType}");

    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("LeaveGame");

        Managers.Object.Clear();
    }

    public static void S_GameResultHandler(PacketSession session, IMessage packet)
    {
        Debug.Log("Game Result");
        S_GameResult resultPacket = packet as S_GameResult;
        Managers.Room.IsResult = resultPacket.IsGameOver;
        Managers.Room.GameResult();
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
