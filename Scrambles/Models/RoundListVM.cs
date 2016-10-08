using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Sql.Models;

namespace Scrambles.Models
{
    public class RoundListVM
    {
        public List<Round> Rounds { get; set; }
        public bool IsAdmin { get; set; }
    }
}