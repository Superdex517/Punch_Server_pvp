using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Zone
    {
        public float IndexX { get; private set; }
        public float IndexY { get; private set; }
        public float IndexZ { get; private set; }

        public HashSet<Hero> Heroes { get; set; } = new HashSet<Hero>();

        public Zone(float x, float y, float z)
        {
            IndexX = x;
            IndexY = y;
            IndexZ = z;
        }

        public void Remove(BaseObject obj)
        {
            EGameObjectType type = ObjectManager.GetObjectTypeFromId(obj.ObjectId);

            switch (type)
            {
                case EGameObjectType.Hero:
                    Heroes.Remove((Hero)obj);
                    break;
            }
        }
    }
}
