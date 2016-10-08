using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scrambles.Models
{
    public class LoginModel
    {
        [Display(Name = "Admin Password")]
        [Required]
        public string Password { get; set; }
    }
}