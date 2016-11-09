using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sql.Models
{
    public class Round
    {
        public int RoundID { get; set; }

        [Display(Name = "Round Number")]
        public int RoundNumber { get; set; }

        [Required]
        [UIHint("FormationImages")]//this is bastadizing the viewmodel and the repo model, in production code this would be broken out
        [MaxLength(100)]
        public string Formations { get; set; }

        public IEnumerable<string> FormationsSplit => Formations.Split(',');
    }
}
