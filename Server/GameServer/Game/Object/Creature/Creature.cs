﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace GameServer
{
    public class Creature : BaseObject
    {
        public CreatureInfo CreatureInfo { get; private set; } = new CreatureInfo();
         
        public Creature()
        {
            CreatureInfo.ObjectInfo = ObjectInfo;
        }
    }
}
