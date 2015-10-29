using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scrambles.Models
{
    public class ScoresEditModel
    {
        public int RoundJumperMapID { get; set; }
        public int? Score { get; set; }
        public string Camera { get; set; }

        [DisplayName("Jumper 1")]
        public int DownJumper1 { get; set; }
        [DisplayName("Jumper 2")]
        public int DownJumper2 { get; set; }
        [DisplayName("Jumper 3")]
        public int UpJumper1 { get; set; }
        [DisplayName("Jumper 4")]
        public int UpJumper2 { get; set; }
        public List<SelectListItem> DownJumper1List { get; set; }
        public List<SelectListItem> DownJumper2List { get; set; }
        public List<SelectListItem> UpJumper1List { get; set; }
        public List<SelectListItem> UpJumper2List { get; set; }
    }
}