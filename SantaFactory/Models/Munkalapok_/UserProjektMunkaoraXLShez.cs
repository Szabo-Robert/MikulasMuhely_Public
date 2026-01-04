using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unisol_2020.Models.Munkalapok_
{
    /// <summary>
    /// 2022.02.24 - Munkalap bovitese PROJEKT-el
    /// </summary>
    public class UserProjektMunkaoraXLShez
    {
        /// <summary>
        /// Alkalmazott neve
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Az alkalmazott oszlop jelolese
        /// </summary>
        public int ColumnNr { get; set; } = 0;

        public List<ProjektRow> ProjektekAdatai { get; set; }
            = new List<ProjektRow>();
    }

    /// <summary>
    /// Projekt adatok
    /// </summary>
    public class ProjektRow 
    {
        /// <summary>
        /// Projektnek a kodja
        /// </summary>
        public string ProjKod { get; set; }

        /// <summary>
        /// A projekt soranak a jelolese
        /// </summary>
        public int RowNr { get; set; } = 0;

        /// <summary>
        /// A projektben ledolgozott munkaorak szama
        /// </summary>
        public decimal Munkaora { get; set; }
    }
}