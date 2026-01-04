using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models
{
    public class JelszoReszetModel
    {
        [Key]
        
        [Display(Name = "Új jelszó:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Új jelszó megadása kötelező")]
        [MinLength(8, ErrorMessage = "Minimum 8 karakter szükséges!")]
        [DataType(DataType.Password)]
        public string UjJelszo { get; set; }

        [Display(Name = "Jelszó megerősítés:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "A jelszó megerősítése kötelező")]
        [MinLength(8, ErrorMessage = "Minimum 8 karakter szükséges!")]
        [DataType(DataType.Password)]
        [Compare("UjJelszo", ErrorMessage = "Hiányos, vagy nem egyező jelszó!")]
        public string JelszoMegerosites { get; set; }

        [Required]
        public string JelszoReszetKod { get; set; }
    }
}