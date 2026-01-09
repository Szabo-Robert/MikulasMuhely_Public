using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(ElvittEszkozokMetadata))]
    public partial class ElvittEszkozok
    {

        public List<int> FeladatIDLista { get; set; }

        public bool EszkozModositva { get; set; }

        public bool EszkozKivalasztva { get; set; }
    }

    public class ElvittEszkozokMetadata
    {

        public int ID { get; set; }

        [Display(Name = "Megnevezés:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Nev { get; set; }
    }
}