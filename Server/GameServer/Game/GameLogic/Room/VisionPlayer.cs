using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class VisionPlayerComponent
    {
        public Hero Owner { get; private set; }
        public HashSet<BaseObject> PreviousObjects { get; private set; } = new HashSet<BaseObject>();
        public IJob UpdateJob;

        public VisionPlayerComponent(Hero owner)
        {
            Owner = owner;
        }

        public HashSet<BaseObject> GatherObjects()
        {
            if (Owner == null || Owner.Room == null)
                return null;

            HashSet<BaseObject> objects = new HashSet<BaseObject>();
            
            Vector3 cellPos = Owner.Pos;

            foreach (Hero hero in GameLogic.Instance.Lobby.FindWaitingRoom(1).GameRoom._heroes.Values)
            {
                objects.Add(hero);
            }

            return objects;
        }

        public void Update()
        {
            if (Owner == null || Owner.Room == null)
                return;

            HashSet<BaseObject> currentObjects = GatherObjects();

            // 기존엔 없었는데 새로 생긴 애들 Spawn 처리
            List<BaseObject> added = currentObjects.Except(PreviousObjects).ToList();
            if (added.Count > 0)
            {
                S_Spawn spawnPacket = new S_Spawn();

                foreach (BaseObject obj in added)
                {
                    if (obj.ObjectType == EGameObjectType.Hero)
                    {
                        Hero player = (Hero)obj;
                        HeroInfo info = new HeroInfo(); // TODO CHECK
                        info.MergeFrom(player.HeroInfo);
                        spawnPacket.Heroes.Add(info);
                    }
                }

                Owner.Session?.Send(spawnPacket);
            }

            // 기존엔 있었는데 사라진 애들 Despawn 처리
            List<BaseObject> removed = PreviousObjects.Except(currentObjects).ToList();
            if (removed.Count > 0)
            {
                S_Despawn despawnPacket = new S_Despawn();

                foreach (BaseObject obj in removed)
                {
                    despawnPacket.ObjectIds.Add(obj.ObjectId);
                }

                Owner.Session?.Send(despawnPacket);
            }

            // 교체
            PreviousObjects = currentObjects;

            UpdateJob = Owner.Room.PushAfter(100, Update);
        }

        public void Clear()
        {
            PreviousObjects.Clear();
        }
    }
}
