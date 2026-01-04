using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(FeladatokAMetadata))]
    public partial class FeladatokA
    {

        public List<FeladatTipusok> FeladatTipusokLista { get; set; }
        public List<Viszony> ViszonyLista { get; set; }
        public List<UsersUnisol> UsersLista { get; set; }

        [Display(Name = "Alkalmazott")]
        public string UserNev { get; set; }
        public List<Feladat_User_ID> UsersFeladatokLista { get; set; }
        public List<ProjektekA> ProjektekLista { get; set; }

        [Display(Name = "K. idő")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        public string KezdesiIdo { get; set; }

        //public string FeladatIDProjektID { get; set; }


        //Raktari eszkozokhoz szukseges listak

        public List<ElvittEszkozok> ElvihetoEszkozokLista { get; set; }

        [Display(Name = "Eszközök:")]
        public List<ElvittEszkozokFeladatokOsszefugges> KiirniEszkozokLista { get; set; }

        public List<Feladat_ElvittEszkoz_ID> FeladatokElvittEszkozokIDjaLista { get; set; }


        [Display(Name = "Résztvevő(k):")]
        public List<FeladatokUsersOsszefugges> feladatokUsersLista { get; set; }


        //MUNKALAPOK listaja a feladathoz
        public List<MunkalapokA> MunkalapokListaja { get; set; }
        public List<FelhasznaltAnyagA> FelhasznaltAnyagALista { get; set; }
    }

    public class FeladatokAMetadata
    {
        public int ID { get; set; }

        [Display(Name = "Megnevezés:")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public string Nev { get; set; }

        [Display(Name = "Proj. név:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public Nullable<int> ProjektAID { get; set; }

        [Display(Name = "Fel. típusa:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public int FeladatTipusaID { get; set; }

        [Display(Name = "Rövid leírás:")]
        [MaxLength(500, ErrorMessage = "Nem lehet hosszabb 500 karakternél!")]
        public string FeladatLeirasa { get; set; }

        [Display(Name = "V.Kezdés")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> VarhatoKezdes { get; set; }

        [Display(Name = "V.Befejezés")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> VarhatoBefejezes { get; set; }

        [Display(Name = "Státusz")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        public int ViszonyID { get; set; }

        [Required(ErrorMessage = "A mező megadása kötelező")]
        [Display(Name = "Munkaóra")]
        public Nullable<int> BecsultMunkaOra { get; set; }

        [Display(Name = "Munka leírása:")]
        [MaxLength(1000, ErrorMessage = "Nem lehet hosszabb 1000 karakternél!")]
        public string MunkaLeirasa { get; set; }

        public Nullable<int> Atiranyitott { get; set; }

        public Nullable<int> FeladatFelelosUserID { get; set; }

        [Display(Name = "Jóváhagyva")]
        public bool JovahagyvaCB { get; set; }

        [Display(Name = "K. idő")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "A mező megadása kötelező")]
        public System.TimeSpan Kido { get; set; }

        [Display(Name = "Archív")]
        public Nullable<bool> ArchiveCB { get; set; }

    }
}