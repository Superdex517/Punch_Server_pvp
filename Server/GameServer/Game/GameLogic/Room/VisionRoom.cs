using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class VisionRoom
    {
        public Hero Owner { get; private set; }

        public VisionRoom(Hero owner)
        {
            Owner = owner;
        }
        public HashSet<BaseObject> GatherObjects()
        {
            if (Owner == null || Owner.Room == null)
                return null;

            HashSet<BaseObject> objects = new HashSet<BaseObject>();

            Vector3 cellPos = Owner.Pos;


            return objects;
        }

        public void Update()
        {

        }
    }
}
