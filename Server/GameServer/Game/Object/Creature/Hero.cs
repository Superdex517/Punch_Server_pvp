using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Hero : Creature
    {
        public ClientSession Session { get; set; }
        public HeroInfo HeroInfo { get; private set; } = new HeroInfo();
        public MyHeroInfo MyHeroInfo { get; private set; } = new MyHeroInfo();

        public string Name
        {
            get { return HeroInfo.Name; }
            set { HeroInfo.Name = value; }
        }

        public Hero()
        {
            MyHeroInfo.HeroInfo = HeroInfo;
            HeroInfo.CreatureInfo = CreatureInfo;

            ObjectType = EGameObjectType.Hero;
        }
    }
}
