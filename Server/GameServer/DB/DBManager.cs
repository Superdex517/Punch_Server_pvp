using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class DBManager : JobSerializer
    {
        public static DBManager Instance { get; } = new DBManager();
    }
}
