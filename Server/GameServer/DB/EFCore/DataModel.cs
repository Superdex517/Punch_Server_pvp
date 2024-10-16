using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Game
{
    public class HeroDb
    {
        // Convention : [클래스]Id 으로 명명하면 PK
        public int HeroDbId { get; set; }
        public long AccountDbId { get; set; }
        public DateTime CreateDate { get; private set; }
    }

    [Table("Item")]
    public class ItemDb
    {
        // Convention : [클래스]Id 으로 명명하면 PK
        public long ItemDbId { get; set; }
        public int TemplateId { get; set; }
        public int EquipSlot { get; set; }
        public long AccountDbId { get; set; }
        public int Count { get; set; }
        public int OwnerDbId { get; set; }
        public int EnchantCount { get; set; }
    }
}
