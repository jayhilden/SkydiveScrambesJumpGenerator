using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrambles.Models
{
    public class ScoresListVM
    {
        public List<ScoresListRow> Scores { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class ScoresListRow
    {
        public int ID { get; set; }

        [Display(Name = "Round Number")]
        public int RoundNumber { get; set; }
        public int? Score { get; set; }
        public string Camera { get; set; }
        [Display(Name = "Jumper 1")]
        public string Jumper1 { get; set; }
        [Display(Name = "Jumper 2")]
        public string Jumper2 { get; set; }
        [Display(Name = "Jumper 3")]
        public string Jumper3 { get; set; }
        [Display(Name = "Jumper 4")]
        public string Jumper4 { get; set; }
        [Display(Name = "Video")]
        public string VideoUrl { get; set; }

    }
}