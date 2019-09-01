using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gobln.OrderByTest.Model
{
    public class MoonWalker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime LunarEvaDate { get; set; }

        public string Remark { get; set; }

        public string GuiId { get; set; }
    }
}
