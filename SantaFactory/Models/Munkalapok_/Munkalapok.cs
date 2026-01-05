using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SantaFactory.Models
{
    [MetadataType(typeof(MunkalapokMetadata))]
    public partial class Munkalapok
    {
        public List<Feladatok> FeladatokLista { get; set; }
        public List<Users> MunkasokLista { get; set; }

        public List<Koltsegek> KoltsegekLista { get; set; }

        [Required(ErrorMessage = "A mező megadása kötelező")]
        public string KezdesiIdo { get; set; }

        [Required(ErrorMessage = "A mező megadása kötelező")]
        public string BefejezesiIdo { get; set; }

        ///EXCEL tablahoz valtozok

        public string userNev { get; set; }
        public string feladatNev { get; set; }
        public string projNev { get; set; }
        public int koltsegItem { get; set; }

        public int[] honapok { get; set; }
        public List<int> evek { get; set; }

        public int XLS_UserID { get; set; }

        public string XLS_id { get; set; }

        [Display(Name = "Szűrés dátuma")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? XLS_Datum { get; set; }
    }

    public class MunkalapokMetadata
    {

        public int ID { get; set; }

        public int UserID { get; set; }

        [Display(Name = "Bejegyzés dátuma")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? Datum { get; set; }

        [Display(Name = "Megjegyzés")]
        public string Magyjegyzes { get; set; }

        public Nullable<int> Atiranyitott { get; set; }

        [Display(Name = "Jóváhagy")]
        public bool JovahagyvaCB { get; set; }

        [Display(Name = "Feladat")]
        public int FeladatID { get; set; }

        [Display(Name = "M.lap dátuma")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy.MM.dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? MunkaDatuma { get; set; }

        [Display(Name = "Munka idő")]
        public Nullable<decimal> MunkaOra { get; set; }

        [Display(Name = "Megtett KM.")]
        public Nullable<int> MegtettKM { get; set; }

        public bool Megtekintve { get; set; }

        [Display(Name = "K. időpont")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public System.TimeSpan KIdo { get; set; }

        [Display(Name = "B. időpont")]
        [Required(ErrorMessage = "A mező megadása kötelező")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public System.TimeSpan BIdo { get; set; }
    }
}