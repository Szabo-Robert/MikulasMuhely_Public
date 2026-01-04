using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    public class ElvittEszkozokFeladatokOsszefugges
    {
        //Unisol 2018-bol athozva 
        public int EszkozID { get; set; }

        public string EszkozNeve { get; set; }

        public bool EszkozKivalasztva { get; set; }

        public bool EszkozModositva { get; set; }

        public int ExEszkozFeladatID { get; set; } //eltaroljuk azokat az ID-kat, ahonnan torolni kell UsersFeladatokhoz rendelt resztvevoket modositasa eseten.
    }
}