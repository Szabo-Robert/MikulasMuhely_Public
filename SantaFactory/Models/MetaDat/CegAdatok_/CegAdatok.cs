using System;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(CegAdatokMetadata))]
    public partial class CegAdatok
    {

    }

    public class CegAdatokMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Cég név:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Nev { get; set; }

        [Display(Name = "Adó szám:")]
        [MaxLength(50, ErrorMessage = "Nem lehet hosszabb 50 karakternél!")]
        public string AdoSzam { get; set; }

        [Display(Name = "Számla szám:")]
        [MaxLength(50, ErrorMessage = "Nem lehet hosszabb 50 karakternél!")]
        public string SzamlaSzam { get; set; }

        [Display(Name = "Meghatalmazott neve:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Meghatalmazott { get; set; }

        public int CimID { get; set; }
        public int ElerhetosegekID { get; set; }

        public Nullable<bool> IsUser { get; set; }
    }
}