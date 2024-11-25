using Cinemachine;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public MyPlayer MyPlayer { get; set; }
    public WaitingRoomCard RoomObject { get; set; }

    List<int> _players = new List<int>();
    Dictionary<int, GameObject> _waitingRooms = new Dictionary<int, GameObject>();
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public ObjectManager()
    {
    }

    #region root
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public Transform RoomRoot { get { return GetRootTransform("@Rooms"); } }
    public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform ProjectileRoot { get { return GetRootTransform("@Projectiles"); } }
    public Transform EnvRoot { get { return GetRootTransform("@Envs"); } }
    public Transform EffectRoot { get { return GetRootTransform("@Effects"); } }
    public Transform NpcRoot { get { return GetRootTransform("@Npc"); } }
    public Transform ItemHolderRoot { get { return GetRootTransform("@ItemHolders"); } }
    #endregion

    public WaitingRoomCard SpawnUI(RoomInfo roomInfo)
    {
        if (roomInfo == null)
            return null;

        GameObject go = Managers.Resource.Instantiate("RoomCard");

        go.name = "Room" + roomInfo.RoomId;
        go.transform.parent = RoomRoot;
        _waitingRooms.Add(roomInfo.RoomId, go);

        RoomObject = Utils.GetOrAddComponent<WaitingRoomCard>(go);
        RoomObject.RoomInfo = roomInfo;
        RoomObject.UpdateRoomTitle(roomInfo.RoomId.ToString());
        RoomObject.MaxPlayer = 1;

        return RoomObject;
    }

    public void EnterLobby(MyHeroInfo myHeroInfo)
    {
        HeroInfo info = myHeroInfo.HeroInfo;
        if (info == null || info.CreatureInfo == null || info.CreatureInfo.ObjectInfo == null)
            return;

        ObjectInfo objectInfo = info.CreatureInfo.ObjectInfo;

        _players.Add(objectInfo.ObjectId);
        Managers.MyPlayer.MyHeroInfo = myHeroInfo;

        Debug.Log($"{Managers.MyPlayer.MyHeroInfo.HeroInfo.CreatureInfo.ObjectInfo}");

        return;
    }

    public void AddPlayer(S_EnterWaitingRoom myHeroInfo)
    {
        HeroInfo info = myHeroInfo.MyHeroInfo.HeroInfo;
        if (info == null || info.CreatureInfo == null || info.CreatureInfo.ObjectInfo == null)
            return;
        
        ObjectInfo objectInfo = info.CreatureInfo.ObjectInfo;

        Managers.MyPlayer.RoomInfo = myHeroInfo.RoomInfo;
        Managers.MyPlayer.MyHeroInfo = myHeroInfo.MyHeroInfo;

        Debug.Log($"{Managers.MyPlayer.MyHeroInfo.Scene}, {Managers.MyPlayer.RoomInfo.RoomId}, {Managers.MyPlayer.RoomInfo.RoomName}");

        return;
    }

    public void readyPlayer(bool isReady)
    {
        S_Ready readyPacket = new S_Ready();
        //readyPacket.MyHeroInfo.IsReady = true;
    }


    // spawn은 내 아이디값을 find해서 가져오는걸로 수정
    public MyPlayer Spawn(MyHeroInfo myHeroInfo)
    {
        HeroInfo info = myHeroInfo.HeroInfo;
        if (info == null || info.CreatureInfo == null || info.CreatureInfo.ObjectInfo == null)
            return null;
        ObjectInfo objectInfo = info.CreatureInfo.ObjectInfo;
        if (MyPlayer != null && MyPlayer.ObjectId == objectInfo.ObjectId)
            return null;
        if (_objects.ContainsKey(objectInfo.ObjectId))
            return null;
        EGameObjectType objectType = Utils.GetObjectTypeFromId(objectInfo.ObjectId);
        if (objectType != EGameObjectType.Hero)
            return null;

        GameObject go = Managers.Resource.Instantiate("FightPlayer"); // TEMP		
        go.name = info.Name;
        go.transform.parent = HeroRoot;
        _objects.Add(objectInfo.ObjectId, go);

        Debug.Log(MyPlayer.SceneType);

        MyPlayer = Utils.GetOrAddComponent<MyPlayer>(go);
        MyPlayer.ObjectId = objectInfo.ObjectId;
        MyPlayer.PosInfo = objectInfo.PosInfo;
        MyPlayer.freeLookCam = Utils.FindAndGetComponent<CinemachineFreeLook>("ThirdPersonCamera");
        //MyPlayer.cc = Utils.GetOrAddComponent<CharacterController>(go);
        MyPlayer.cam = Camera.main.transform;

        //MyHero.SyncWorldPosWithCellPos();

        return MyPlayer;
    }

    public Player Spawn(HeroInfo info)
    {
        if (info == null || info.CreatureInfo == null || info.CreatureInfo.ObjectInfo == null)
            return null;
        ObjectInfo objectInfo = info.CreatureInfo.ObjectInfo;
        if (MyPlayer.ObjectId == objectInfo.ObjectId)
            return null;
        if (_objects.ContainsKey(objectInfo.ObjectId))
            return null;
        EGameObjectType objectType = Utils.GetObjectTypeFromId(objectInfo.ObjectId);
        if (objectType != EGameObjectType.Hero)
            return null;

        GameObject go = Managers.Resource.Instantiate("FightPlayer"); // TEMP
        go.name = info.Name;
        go.transform.parent = HeroRoot;
        _objects.Add(objectInfo.ObjectId, go);

        Player player = Utils.GetOrAddComponent<Player>(go);
        //player.cc = Utils.GetOrAddComponent<CharacterController>(go);
        player.ObjectId = objectInfo.ObjectId;
        player.PosInfo = objectInfo.PosInfo;
        player.SetInfo(1);

        //hero.SyncWorldPosWithCellPos();

        return player;
    }

    public void DespawnUI(int roomId)
    {
        if (RoomObject != null && RoomObject.RoomInfo.RoomId == roomId)
            return;

        if (_waitingRooms.ContainsKey(roomId) == false)
            return;

        GameObject go = FindById(roomId);
        if (go == null)
            return;

        WaitingRoomCard room = go.GetComponent<WaitingRoomCard>();
        if (room != null)
        {

        }

        _waitingRooms.Remove(roomId);
        Managers.Resource.Destroy(go);
    }

    public void Despawn(int objectId)
    {
        if (MyPlayer != null && MyPlayer.ObjectId == objectId)
            return;
        if (_objects.ContainsKey(objectId) == false)
            return;

        GameObject go = FindById(objectId);
        if (go == null)
            return;

        BaseObject bo = go.GetComponent<BaseObject>();
        if (bo != null)
        {

        }

        _objects.Remove(objectId);
        Managers.Resource.Destroy(go);
    }

    public GameObject FindById(int id)
    {
        GameObject go = null;
        _objects.TryGetValue(id, out go);
        return go;
    }

    public GameObject FindCreature(Vector3Int cellPos)
    {
        foreach (GameObject obj in _objects.Values)
        {
            Creature creature = obj.GetComponent<Creature>();
            if (creature == null)
                continue;

            //if (creature.CellPos == cellPos)
            //	return obj;
        }

        return null;
    }

    public GameObject Find(Func<GameObject, bool> condition)
    {
        foreach (GameObject obj in _objects.Values)
        {
            if (condition.Invoke(obj))
                return obj;
        }

        return null;
    }

    public List<T> FindAllComponents<T>(Func<T, bool> condition) where T : UnityEngine.Component
    {
        List<T> ret = new List<T>();

        foreach (GameObject obj in _objects.Values)
        {
            T t = Utils.FindChild<T>(obj, recursive: true);
            if (t != null && condition.Invoke(t))
                ret.Add(t);
        }

        return ret;
    }

    public void Clear()
    {
        foreach (GameObject obj in _objects.Values)
            Managers.Resource.Destroy(obj);

        _objects.Clear();
        MyPlayer = null;
    }
}
