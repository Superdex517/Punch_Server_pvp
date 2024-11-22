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

        public Lobby Lobby = new Lobby();

        public GameLogic()
        {
            if(Lobby == null)
                Console.WriteLine("There is no WaitingRoom");

            Console.WriteLine("WaitingRoom Init");
        }

        public void Update()
        {
            Flush();

            Lobby.Update();
        }
    }
}
