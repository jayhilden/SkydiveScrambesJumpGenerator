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
        public int RoundNumber { get; set; }

        [Required]
        public string Formations { get; set; }
    }
}
