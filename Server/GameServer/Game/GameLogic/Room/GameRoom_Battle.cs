using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class GameRoom : JobSerializer
    {
        public void HandleMove(Hero hero, C_Move movePacket)
        {
            if ((hero == null))
                return;
            if (hero.State == EObjectState.Dead)
                return;

            PositionInfo movePosInfo = movePacket.PosInfo;
            ObjectInfo info = hero.ObjectInfo;

            info.PosInfo.State = movePosInfo.State;
            info.PosInfo.MoveDir = movePosInfo.MoveDir;
            ApplyMove(hero, new Vector3(movePosInfo.PosX, movePosInfo.PosY, movePosInfo.PosZ), movePosInfo.Dir);

            hero.BroadcastMove();
        }

        public bool ApplyMove(BaseObject obj, Vector3 dest, float dir, bool checkObjects = true, bool collision = true)
        {
            if (obj == null)
                return false;
            if (obj.WaitingRoom.GameRoom == null)
                return false;

            PositionInfo posInfo = obj.PosInfo;

            EGameObjectType type = ObjectManager.GetObjectTypeFromId(obj.ObjectId);
            
            Hero hero = (Hero)obj;

            // 실제 좌표 이동
            posInfo.PosX = dest.X;
            posInfo.PosY = dest.Y;
            posInfo.PosZ = dest.Z;
            posInfo.Dir = dir;

            return true;
        }
    }
}
