using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Sql.Models;

namespace Scrambles.Models
{
    public class JumperListViewModel
    {
        public IEnumerable<Jumper> Jumpers { get; set; }
        public bool RandomizationLocked { get; set; }
    }
}