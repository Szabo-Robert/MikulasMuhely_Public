using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SantaFactory.Models
{
    public class FeladatokUsersOsszefugges
    {
        public int UsersID { get; set; }

        public string UsersNeve { get; set; }

        public bool UserKivalasztva { get; set; }

        public int ExUsersFeladatokID { get; set; } //eltaroljuk azokat az ID-kat, ahonnan torolni kell UsersFeladatokhoz rendelt resztvevoket modositasa eseten.
    }
}