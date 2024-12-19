using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class ObjectManager : Singleton<ObjectManager>
    {
        object _lock = new object();
        Dictionary<int, Hero> _player = new Dictionary<int, Hero>();

        Dictionary<int, Hero> _heroes = new Dictionary<int, Hero> ();
        Dictionary<int, BaseObject> _gameobjects = new Dictionary<int, BaseObject> ();
        int _counter = 0;

        Dictionary<int, WaitingRoom> _rooms = new Dictionary<int, WaitingRoom> ();
        int _roomCounter = 0;

        public T SpawnPlayer<T>(int templateId = 0) where T : BaseObject, new()
        {
            T obj = new T();

            lock (_lock)
            {
                obj.ObjectId = GenerateId(obj.ObjectType, templateId);

                if (obj.ObjectType == EGameObjectType.Hero)
                {
                    _player.Add(obj.ObjectId, obj as Hero);
                }
            }

            return obj;
        }

        public T SpawnRoom<T>(int templateId = 0) where T : WaitingRoom, new()
        {
            T room = new T();

            lock (_lock)
            {
                room.WaitingRoomId = GenerateRoomId(room.UIType, templateId);

                _rooms.Add(room.WaitingRoomId, room as WaitingRoom);
            }

            return room;
        }

        public T Spawn<T>(int id, int templateId = 0) where T : BaseObject, new()
        {
            T obj = new T();

            lock (_lock)
            {
                obj.ObjectId = id;

                _heroes.Add(obj.ObjectId, obj as Hero);

                //Console.WriteLine($"spawn heros ++ : {_heroes.Count}, {obj.ObjectId}");
            }

            return obj;
        }

        int GenerateId(EGameObjectType type, int templateId)
        {
            lock (_lock)
            {
                return ((int)type << 28) | (templateId << 20) | (_counter++);
            }
        }

        int GenerateRoomId(EGameUIType type, int templateId)
        {
            lock (_lock)
            {
                return ((int)type << 28) | (templateId << 20) | (_roomCounter++);
            }
        }

        public static EGameObjectType GetObjectTypeFromId(int id)
        {
            int type = (id >> 28) & 0x0F;
            return (EGameObjectType)type;
        }

        public static int GetTemplateIdFromId(int id)
        {
            int templateId = (id >> 20) & 0xFF;
            return templateId;
        }

        public bool Remove(int objectId)
        {
            EGameObjectType objectType = GetObjectTypeFromId(objectId);

            lock (_lock)
            {
                if (objectType == EGameObjectType.Hero)
                    return _heroes.Remove(objectId);
            }

            return false;
        }

        public Hero Find(int objectId)
        {
            EGameObjectType objectType = GetObjectTypeFromId(objectId);

            lock (_lock)
            {
                if (objectType == EGameObjectType.Hero)
                {
                    if (_heroes.TryGetValue(objectId, out Hero hero))
                        return hero;
                }
            }

            return null;
        }

        public Hero FindInLobby(int objectId)
        {
            EGameObjectType objectType = GetObjectTypeFromId(objectId);

            lock (_lock)
            {
                if (objectType == EGameObjectType.Hero)
                {
                    if (_player.TryGetValue(objectId, out Hero hero))
                        return hero;
                }
            }

            return null;
        }

        public T Find<T>(int objectId) where T : BaseObject, new()
        {
            if (_gameobjects.TryGetValue(objectId, out BaseObject bo))
            {
                return bo as T;
            }

            return null;
        }
    }


}
