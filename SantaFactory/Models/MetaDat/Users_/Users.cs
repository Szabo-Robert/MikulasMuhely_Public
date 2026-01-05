using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{

    [MetadataType(typeof(UsersMetadata))]
    public partial class Users
    {
        public string JelszoMegerosites { get; set; }

        //eltaroljuk azokat az ID-kat, ahonnan torolni kell majd UsersFeladatokhoz rendelt resztvevok modositasa eseten.
        public int UsersFeladatokID { get; set; }

        //Viszonyok listajana eltarolasa
        public List<Viszony> ViszonyokLista { get; set; }

        public List<Jogosultsag> JogosultsagLista { get; set; }

        public Users ViewerUser { get; set; }

        public string Nev { get; set; }

        public string logUserJogosultsaga { get; set; }
        public int logUserID { get; set; }
    }

    public class UsersMetadata
    {
        public int UserID { get; set; }

        [Display(Name = "Vezeték név:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(50, ErrorMessage = "Nem lehet hosszabb 50 karakternél!")]
        public string VezetekNev { get; set; }

        [Display(Name = "Kereszt név:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(50, ErrorMessage = "Nem lehet hosszabb 50 karakternél!")]
        public string KeresztNev { get; set; }

        public int ElerhetosegekID { get; set; }

        public int CimekID { get; set; }

        public int JogosultsagID { get; set; }

        public int ViszonyID { get; set; }

        [Display(Name = "Jelszó:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum 8 karakter szükséges!")]
        public string Jelszo { get; set; }

        [Display(Name = "Jelszó megerősítés:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A jelszó megerősítése kötelező")]
        [DataType(DataType.Password)]
        [Compare("Jelszo", ErrorMessage = "Hiányos, vagy nem egyező jelszó!")]
        public string JelszoMegerosites { get; set; }

        [Display(Name = "Születési dátum:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]

        public System.DateTime SzuletesiDatum { get; set; }

        [Display(Name = "Születési hely:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string SzuletesiHely { get; set; }

        [Display(Name = "Édesanyja neve:")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string AnyjaNeve { get; set; }

        [Display(Name = "Személy. Ig. szám:")]
        [MaxLength(30, ErrorMessage = "Nem lehet hosszabb 30 karakternél!")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public string SzemIgSzam { get; set; }

        [Display(Name = "Számla szám:")]
        [MaxLength(30, ErrorMessage = "Nem lehet hosszabb 30 karakternél!")]
        public string SzamlaSzam { get; set; }

        [Display(Name = "Adó szám:")]
        public string AdoSzam { get; set; }

        public Nullable<int> CegAdatokID { get; set; }

        public bool Megegyezik { get; set; }

        [Display(Name = "Rendszám:")]
        [MaxLength(15, ErrorMessage = "Nem lehet hosszabb 15 karakternél!")]
        public string GKRendszam { get; set; }

        [Display(Name = "Üzemanyag ár:")]
        public Nullable<int> UzemanyagAr { get; set; }

        [Display(Name = "Bérezés:")]
        public Nullable<int> Berezes { get; set; }

        [Display(Name = "Értesitendő neve:")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string ErtSzemelyNeve { get; set; }

        public bool EmailMegerositve { get; set; }

        public System.Guid AktivaloKod { get; set; }

        public Nullable<int> ErtSzemelyElerhetosegID { get; set; }

        [Display(Name = "Bej. marad:")]
        public Nullable<bool> BejelentkezveMarad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public bool GDPR { get; set; }
    }
}