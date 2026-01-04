using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Unisol_2020.Models
{
    [MetadataType(typeof(MunkalapokAMetadata))]
    public partial class MunkalapokA
    {
        public List<FeladatokA> FeladatokLista { get; set; }
        public List<UsersUnisol> MunkasokLista { get; set; }

        public List<Koltsegek> KoltsegekLista { get; set; }

        //public List<int> OrakLista { get; set; }
        //public List<int> PercekLista { get; set; }

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

    public class MunkalapokAMetadata
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
        public int FeladatAID { get; set; }

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

        [Display(Name = "Archív")]
        public Nullable<bool> ArchiveCB { get; set; }
    }
}