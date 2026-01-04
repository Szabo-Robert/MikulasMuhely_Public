using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(ProjektekMetadata))]
    public partial class Projektek
    {
        public List<Feladatok> feladatokLista { get; set; }

        [Display(Name = "Arhíválás")]
        public bool archivumCB { get; set; }
    }

    public class ProjektekMetadata
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

        public Nullable<bool> ArchiveCB { get; set; }
    }
}