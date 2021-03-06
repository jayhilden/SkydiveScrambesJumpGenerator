﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sql.Models
{
    public enum UpDownFlag
    {
        [Display(Name = "Up Jumper")]
        UpJumper = 1,
        [Display(Name = "Down Jumper")]
        DownJumper = 2
    }

    public enum JumpGroupFlag
    {
        Left = 1,
        Right = 2
    }

    public class Jumper : ICloneable
    {
        [Key]
        public int JumperID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("# of Jumps")]
        public int NumberOfJumps { get; set; }

        [DisplayName("Is this jumper an organizer?")]
        public bool Organizer { get; set; }

        [DisplayName("Has this jumper paid?")]
        public bool Paid { get; set; }

        public string Comment { get; set; }

        [DisplayName("Randomized into Up/Down jumper group")]
        public UpDownFlag? RandomizedUpDown { get; set; }

        [DisplayName("Jump Group")]
        public JumpGroupFlag? JumpGroup { get; set; }

        public object Clone()
        {
            return AutoMapper.Mapper.Map<Jumper>(this);
        }

        public override string ToString()
        {
            return $"{JumperID}: {FirstName} {LastName} ({NumberOfJumps})";
        }
    }
}
