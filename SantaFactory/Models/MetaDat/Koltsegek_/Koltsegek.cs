using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(KoltsegekMetadata))]
    public partial class Koltsegek
    {
        //public int MunkalapID { get; set; }

        public int FeladatID { get; set; }

        public Feladatok feladatItem { get; set; }

        [Display(Name = "Sz. dátuma")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? SzamlaDatuma { get; set; }
    }

    public class KoltsegekMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Megnevezés:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string KoltsegNeve { get; set; }

        [Display(Name = "Érték:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public int KoltsegErteke { get; set; }

        [Display(Name = "Rövid leírás:")]
        [MaxLength(200, ErrorMessage = "Nem lehet hosszabb 200 karakternél!")]
        public string KoltsegLeirasa { get; set; }

        public Nullable<int> MunkalapID { get; set; }

    }
}