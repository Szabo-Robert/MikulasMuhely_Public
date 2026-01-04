using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    public partial class LoginModel
    {
        
        public string Email { get; set; }
        
        public string Jelszo { get; set; }

        public bool BelepveMarad { get; set; }
    }
}