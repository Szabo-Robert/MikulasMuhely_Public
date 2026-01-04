using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(JogosultsagMetadata))]
    public partial class Jogosultsag
    {

    }

    public class JogosultsagMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Jogosultság:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(20, ErrorMessage = "Nem lehet hosszabb 20 karakternél!")]
        public string Nev { get; set; }
    }
}