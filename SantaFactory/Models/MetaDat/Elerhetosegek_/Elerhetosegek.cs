using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(ElerhetosegekMetadata))]
    public partial class Elerhetosegek
    {

    }

    public class ElerhetosegekMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Telsz.1:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20, ErrorMessage = "Nem lehet hosszabb 20 karakternél!")]
        public string Telszam1 { get; set; }

        [Display(Name = "Telsz.2:")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(20, ErrorMessage = "Nem lehet hosszabb 20 karakternél!")]
        public string Telszam2 { get; set; }

        [Display(Name = "E-mail:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Email { get; set; }

        [Display(Name = "Web URL:")]
        [DataType(DataType.Url)]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string WEB { get; set; }

        [Display(Name = "Megjegyzés:")]
        [MaxLength(200, ErrorMessage = "Nem lehet hosszabb 200 karakternél!")]
        public string Megjegyzes { get; set; }
    }
}