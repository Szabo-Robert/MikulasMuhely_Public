using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(ViszonyMetadata))]
    public partial class Viszony
    {

    }

    public  class ViszonyMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Állapot:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(30, ErrorMessage = "Nem lehet hosszabb 30 karakternél!")]
        public string Nev { get; set; }
    }
}