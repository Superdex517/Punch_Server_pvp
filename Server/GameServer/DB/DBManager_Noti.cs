using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public partial class DBManager : JobSerializer
    {
        public static void TestDB()
        {
            HeroDb heroDb = new HeroDb();
            heroDb.AccountDbId = new Random().Next(0, 100);

            using (GameDbContext db = new GameDbContext())
            {
                db.Add(heroDb);
                db.SaveChangesEx();
            }
        }
    }
}
