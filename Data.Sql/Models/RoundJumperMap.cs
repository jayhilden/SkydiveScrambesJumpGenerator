﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sql.Models
{
    public class RoundJumperMap
    {
        public int ID { get; set; }
        public int RoundID { get; set; }
        [ForeignKey("UpJumper1")]
        public int UpJumper1ID { get; set; }

        [ForeignKey("UpJumper2")]
        public int UpJumper2ID { get; set; }

        [ForeignKey("DownJumper1")]
        public int DownJumper1ID { get; set; }

        [ForeignKey("DownJumper2")]
        public int DownJumper2ID { get; set; }

        public virtual Round Round { get; set; }
        
        public virtual Jumper UpJumper1 { get; set; }
        public virtual Jumper UpJumper2 { get; set; }
        public virtual Jumper DownJumper1 { get; set; }
        public virtual Jumper DownJumper2 { get; set; }
    }
}