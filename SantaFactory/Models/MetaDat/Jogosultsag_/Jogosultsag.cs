using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
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