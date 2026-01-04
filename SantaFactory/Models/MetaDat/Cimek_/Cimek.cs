using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(CimekMetadata))]
    public partial class Cimek
    {

    }

    public class CimekMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Ország:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Orszag { get; set; }

        [Display(Name = "Város:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Varos { get; set; }

        [Display(Name = "Postakód:")]
        [DataType(DataType.PostalCode)]
        public Nullable<int> PostaKod { get; set; }

        [Display(Name = "Utca:")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Utca { get; set; }

        [Display(Name = "Szám:")]
        [MaxLength(10, ErrorMessage = "Nem lehet hosszabb 10 karakternél!")]
        public string Szam { get; set; }

        [Display(Name = "Egyéb:")]
        [MaxLength(20, ErrorMessage = "Nem lehet hosszabb 20 karakternél!")]
        public string Egyeb { get; set; }

        [Display(Name = "Megjegyzés:")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Megjegyzes { get; set; }
    }
}