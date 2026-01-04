using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    public class FelhasznaltAnyagokNeveLista
    {
        [Display(Name = "")]
        public string felhasznaltAnyagNeve { get; set; }

        //public List<Feladatok> FeladatokLista { get; set; }

    }
}