using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameLogic : JobSerializer
    {
        public static GameLogic Instance { get; } = new GameLogic();

        public WaitingRoom WaitingRoom = new WaitingRoom();

        public GameLogic()
        {
            if(WaitingRoom == null)
                Console.WriteLine("There is no WaitingRoom");

            Console.WriteLine("WaitingRoom Init");
        }

        public void Update()
        {
            Flush();

            WaitingRoom.Update();
        }
    }
}
