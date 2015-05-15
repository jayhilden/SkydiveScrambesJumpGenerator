﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scrambles.Models
{
    public class DrawSingleJump
    {
        public int RoundNumber { get; set; }
        public DrawJumper Jumper1 { get; set; }
        public DrawJumper Jumper2 { get; set; }
        public DrawJumper Jumper3 { get; set; }
        public DrawJumper Jumper4 { get; set; }
    }

    public class DrawJumper
    {
        public string Name { get; set; }
        public int NumberOfJumps { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, NumberOfJumps);
        }
    }
}