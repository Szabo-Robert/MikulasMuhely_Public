using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    public class FeladatokUsersOsszefugges
    {
        //Unisol 2018-bol athozva
        public int UsersID { get; set; }

        public string UsersNeve { get; set; }

        public bool UserKivalasztva { get; set; }

        public int ExUsersFeladatokID { get; set; } //eltaroljuk azokat az ID-kat, ahonnan torolni kell UsersFeladatokhoz rendelt resztvevoket modositasa eseten.
    }
}