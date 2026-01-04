using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(ProjektekAMetadata))]
    public partial class ProjektekA
    {
        public List<FeladatokA> feladatokLista { get; set; }
    }

    public class ProjektekAMetadata
    {
        public int ID { get; set; }

        [Display(Name = "Proj. neve:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Nev { get; set; }

        [Display(Name = "Proj. kódja:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(30, ErrorMessage = "Nem lehet hosszabb 30 karakternél!")]
        public string AzonositoKod { get; set; }

        [Display(Name = "Archiválás")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        public Nullable<bool> ArchiveCB { get; set; }
    }
}