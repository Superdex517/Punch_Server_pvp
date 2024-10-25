using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class MapComponent
    {
        public bool ApplyMove(BaseObject obj, Vector3 dest)
        {
            if (obj == null) return false;
            if (obj.Room == null) return false;
            if (obj.Room.Map != this) return false;

            PositionInfo posInfo = obj.PosInfo;

            posInfo.PosX = dest.X;
            posInfo.PosY = dest.Y;
            posInfo.PosZ = dest.Z;

            return true;
        }

    }

}
