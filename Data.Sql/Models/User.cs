using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sql.Models
{
    public enum UpDownFlag
    {
        UpJumper = 1,
        DownJumper = 2
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int NumberOfJumps { get; set; }

        public bool Organizer { get; set; }

        public bool Paid { get; set; }

        public string Comment { get; set; }

        public UpDownFlag? RandomizedUpDown { get; set; }

        [MaxLength(1)]
        public string RandomizedLetter { get; set; }
    }
}
