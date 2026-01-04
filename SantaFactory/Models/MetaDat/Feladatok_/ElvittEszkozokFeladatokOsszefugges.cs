using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SantaFactory.Models
{
    public class ElvittEszkozokFeladatokOsszefugges
    {
        public int EszkozID { get; set; }

        public string EszkozNeve { get; set; }

        public bool EszkozKivalasztva { get; set; }

        public bool EszkozModositva { get; set; }

        public int ExEszkozFeladatID { get; set; } //eltaroljuk azokat az ID-kat, ahonnan torolni kell UsersFeladatokhoz rendelt resztvevoket modositasa eseten.
    }
}