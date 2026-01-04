using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(FelhasznaltAnyagAMetadata))]
    public partial class FelhasznaltAnyagA
    {
        [Display(Name = "Feladat:")]
        public string FeladatNeve { get; set; }
    }

    public class FelhasznaltAnyagAMetadata
    {
        public int ID { get; set; }

        [Display(Name = "Megnevezés:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [MaxLength(50, ErrorMessage = "Nem lehet hosszabb 50 karakternél!")]
        public string Nev { get; set; }

        [Display(Name = "Feltöltő:")]
        public string UserNev { get; set; }

        public int FeladatAID { get; set; }
    }
}