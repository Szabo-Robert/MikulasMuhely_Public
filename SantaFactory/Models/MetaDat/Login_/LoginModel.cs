using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(LoginModelMetadata))]
    public partial class LoginModel
    {
        //public List<Feladatok> FeladatokLista{ get; set; }

        //public int UsernekAzIDja { get; set; }
    }

    public class LoginModelMetadata
    {
        [Key]
        [Display(Name = "E-mail cím:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Nem lehet hosszabb 100 karakternél!")]
        public string Email { get; set; }

        [Display(Name = "Jelszó:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Password)]
        public string Jelszo { get; set; }

        [Display(Name = "Belépve marad")]
        public bool BelepveMarad { get; set; }
    }

}