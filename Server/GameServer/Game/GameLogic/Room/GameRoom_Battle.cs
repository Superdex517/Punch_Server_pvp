using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Map.ApplyMove(hero, new System.Numerics.Vector3(movePosInfo.PosX, movePosInfo.PosY, movePosInfo.PosZ));

            hero.BroadcastMove();
        }
    }
}
