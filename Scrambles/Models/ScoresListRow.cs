using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrambles.Models
{
    public class ScoresListRow
    {
        public int ID { get; set; }

        [Display(Name = "Round Number")]
        public int RoundNumber { get; set; }
        public int? Score { get; set; }
        public string Camera { get; set; }
        public string Jumper1 { get; set; }
        public string Jumper2 { get; set; }
        public string Jumper3 { get; set; }
        public string Jumper4 { get; set; }
        [Display(Name = "Video")]
        public string VideoUrl { get; set; }

    }
}