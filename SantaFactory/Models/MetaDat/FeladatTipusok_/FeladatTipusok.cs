using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(FeladatTipusokMetadata))]
    public partial class FeladatTipusok
    {

    }

    public class FeladatTipusokMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Fel. típusa:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(50, ErrorMessage = "Nem lehet hosszabb 50 karakternél!")]
        public string Nev { get; set; }
    }
}