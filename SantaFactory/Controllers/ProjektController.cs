using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using PagedList;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class ProjektController : Controller
    {

        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index(int? pn)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Projektek.OrderByDescending(x => x.AzonositoKod).ToList();

                return View(model.ToPagedList(pn ?? 1, 10));

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #region IndexA

        public ActionResult IndexA(int? pn)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.ProjektekA.OrderByDescending(x => x.AzonositoKod).ToList();

                return View(model.ToPagedList(pn ?? 1, 10));

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        // GET: Projekt/Details/5
        public ActionResult Details(int id)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                Projektek model = db.Projektek.FirstOrDefault(p => p.ID == id);

                model.feladatokLista = db.Feladatok.Where(f => f.ProjektID == id).ToList();

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #region DetailsA

        // GET: Projekt/DetailsA
        public ActionResult DetailsA(int id)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                ProjektekA model = db.ProjektekA.FirstOrDefault(p => p.ID == id);

                model.feladatokLista = db.FeladatokA.Where(f => f.ProjektAID == id).ToList();

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        // GET: Projekt/Create
        public ActionResult Create()
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Projekt/Create
        [HttpPost]
        public ActionResult Create(Projektek model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Projektek.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(model);
            }
        }

        // GET: Projekt/Edit/5
        public ActionResult Edit(int id)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Projektek.FirstOrDefault(x => x.ID == id);

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        // POST: Projekt/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Projektek projekt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Projektek.FirstOrDefault(x => x.ID == id);

                    model.Nev = projekt.Nev;
                    model.AzonositoKod = projekt.AzonositoKod;

                    if (projekt.archivumCB == false)
                    {
                        projekt.ArchiveCB = projekt.archivumCB;
                        db.Projektek.Attach(model);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ///Ha archiv a projekt, akkor a hozza tartozo FELADATOK es MUNKALAPOKAT is
                        ///archivalni kell. Ez mind itt torteni meg.
                        ProjektekA archivModel = new ProjektekA
                        {
                            ID = model.ID,
                            Nev = model.Nev,
                            AzonositoKod = model.AzonositoKod,
                            ArchiveCB = projekt.archivumCB,
                        };

                        ///miutan athelyezodott a PROJEKT az ARCHIV reszbe
                        ///az elozo PROJEKT tablabol torlesre kerul
                        db.ProjektekA.Add(archivModel);
                        db.Projektek.Remove(model);

                        ///kigyujtjuk azokat a feladatokat, ami ehhez a projekthez tartozik
                        IQueryable<Feladatok> projektFeladatjaiLista = db.Feladatok.Where(f => f.ProjektID == model.ID);
                        var nemJovahagyottFeladat = projektFeladatjaiLista.Where(f => f.JovahagyvaCB == false);

                        if (projektFeladatjaiLista.Any() && !nemJovahagyottFeladat.Any())
                        {
                            foreach (var feladatItem in projektFeladatjaiLista)
                            {
                                FeladatokA archivFeladat = new FeladatokA
                                {
                                    ArchiveCB = true,
                                    Atiranyitott = feladatItem.Atiranyitott,
                                    ID = feladatItem.ID,
                                    Nev = feladatItem.Nev,
                                    ProjektAID = feladatItem.ProjektID,
                                    FeladatTipusaID = feladatItem.FeladatTipusaID,
                                    FeladatLeirasa = feladatItem.FeladatLeirasa,
                                    VarhatoKezdes = feladatItem.VarhatoKezdes,
                                    VarhatoBefejezes = feladatItem.VarhatoBefejezes,
                                    ViszonyID = feladatItem.ViszonyID,
                                    BecsultMunkaOra = feladatItem.BecsultMunkaOra,
                                    MunkaLeirasa = feladatItem.MunkaLeirasa,
                                    FeladatFelelosUserID = feladatItem.FeladatFelelosUserID,
                                    JovahagyvaCB = feladatItem.JovahagyvaCB,
                                    Kido = feladatItem.Kido,
                                    CimekID = feladatItem.CimekID,
                                    ElerhetosegekID = feladatItem.ElerhetosegekID,
                                    KezdesiIdo = $"{feladatItem.Kido}"
                                };

                                ///miutan athelyezodott a FELADAT az ARCHIV reszbe
                                ///az elozo FELADAT tablabol torlesre kerul
                                db.FeladatokA.Add(archivFeladat);
                                db.Feladatok.Remove(feladatItem);

                                ///kigyujtjuk azokat a munkalapokat, amik a feladathoz tartoznak
                                IQueryable<Munkalapok> feladatMunkalajaiLista = db.Munkalapok.Where(m => m.FeladatID == feladatItem.ID && m.JovahagyvaCB);
                                var nemJovahagyottMunkalap = feladatMunkalajaiLista.Where(m => m.JovahagyvaCB == false);

                                if (feladatMunkalajaiLista.Any() && !nemJovahagyottMunkalap.Any())
                                {
                                    foreach (var munkalapItem in feladatMunkalajaiLista)
                                    {
                                        MunkalapokA munkalapA = new MunkalapokA
                                        {
                                            ArchiveCB = true,
                                            Atiranyitott = munkalapItem.Atiranyitott,
                                            FeladatAID = munkalapItem.FeladatID,
                                            BefejezesiIdo = $"{munkalapItem.BIdo}",
                                            BIdo = munkalapItem.BIdo,
                                            Datum = munkalapItem.Datum,
                                            evek = munkalapItem.evek,
                                            feladatNev = munkalapItem.feladatNev,
                                            honapok = munkalapItem.honapok,
                                            ID = munkalapItem.ID,
                                            JovahagyvaCB = munkalapItem.JovahagyvaCB,
                                            KIdo = munkalapItem.KIdo,
                                            KezdesiIdo = $"{munkalapItem.KIdo}",
                                            KoltsegekLista = munkalapItem.KoltsegekLista,
                                            koltsegItem = munkalapItem.koltsegItem,
                                            Magyjegyzes = munkalapItem.Magyjegyzes,
                                            Megtekintve = munkalapItem.Megtekintve,
                                            MegtettKM = munkalapItem.MegtettKM,
                                            MunkaDatuma = munkalapItem.MunkaDatuma,
                                            MunkaOra = munkalapItem.MunkaOra,
                                            MunkasokLista = munkalapItem.MunkasokLista,
                                            projNev = munkalapItem.projNev,
                                            UserID = munkalapItem.UserID,
                                            userNev = munkalapItem.userNev

                                        };

                                        db.MunkalapokA.Add(munkalapA);
                                        db.Munkalapok.Remove(munkalapItem);

                                        #region Munkalaphoz tartozo kapcsolatok torlese

                                        var vanKapcsolodoKoltseg = db.Koltsegek.Where(x => x.MunkalapID == munkalapItem.ID).ToList();

                                        foreach (var item in vanKapcsolodoKoltseg)
                                        {
                                            db.Koltsegek.Remove(item);
                                        }

                                        ///megvizsgaljuk, hogy az-az alkalmazott, akinek toroljuk a munkalapjat
                                        ///szerepel e masik munkalapon, mert ha nem, akkor ki kell vegyuk a kapcsolatat
                                        ///a feladattal, hiszen megszunnek a munkalapjai

                                        var feladatUserMunkalapLista = db.Munkalapok.Where(x => x.FeladatID == munkalapItem.FeladatID && x.UserID == munkalapItem.UserID).ToList();

                                        if (feladatUserMunkalapLista.Count() <= 1)
                                        {
                                            var feladatUserIDItem = db.Feladat_User_ID.FirstOrDefault(x => x.FeladatID == munkalapItem.FeladatID && x.UserID == munkalapItem.UserID);

                                            db.Feladat_User_ID.Remove(feladatUserIDItem);
                                        }

                                        #endregion
                                    }
                                }
                                else
                                {
                                    TempData["ErrorMessage"] = "Nem ARCHÍVÁLHATÓ, mert nincs hozzá rendelve munkalap, vagy valamelyik munkalap nincs jóváhagyva!";
                                    return View(model);
                                }

                                ///kigyujtjuk azokat a felhasznált anyagokat, amik a feladathoz tartoznak
                                IQueryable<Feladat_FelhaszAnyag_ID> feladat_felhasznaltAnyagokKapcsolotablaIDs = db.Feladat_FelhaszAnyag_ID.Where(m => m.FeladatID == feladatItem.ID);

                                if (feladat_felhasznaltAnyagokKapcsolotablaIDs.Any())
                                {
                                    foreach (var feladat_felhasznaltAnyagokKapcsolotablaItem in feladat_felhasznaltAnyagokKapcsolotablaIDs)
                                    {
                                        FelhasznaltAnyagok felhasznaltAnyagItem =
                                            db.FelhasznaltAnyagok.Where(m => m.ID == feladat_felhasznaltAnyagokKapcsolotablaItem.FelhasznaltAnyagID).FirstOrDefault();

                                        FelhasznaltAnyagA felhasznaltAnyagA = new FelhasznaltAnyagA
                                        {
                                            FeladatAID = feladat_felhasznaltAnyagokKapcsolotablaItem.FeladatID,
                                            Nev = felhasznaltAnyagItem.Nev,
                                            UserNev = felhasznaltAnyagItem.UserNev
                                        };

                                        db.FelhasznaltAnyagA.Add(felhasznaltAnyagA);

                                        db.FelhasznaltAnyagok.Remove(felhasznaltAnyagItem);

                                    }

                                }

                                #region Feladat Kapcsolatok Torlese

                                //FELADAT - Felhasznalo kapcsolat
                                var feladat_User_ID_Lista = db.Feladat_User_ID.Where(x => x.FeladatID == feladatItem.ID).ToList();

                                foreach (var item in feladat_User_ID_Lista)
                                {
                                    var feladat_User_ID_Item = db.Feladat_User_ID.FirstOrDefault(x => x.ID == item.ID);

                                    db.Feladat_User_ID.Remove(feladat_User_ID_Item);
                                }

                                //FELADAT - ELVITT Eszkoz(ok) kapcsolat
                                var feladat_ElvittEszkoz_ID_Lista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == feladatItem.ID).ToList();

                                foreach (var item in feladat_ElvittEszkoz_ID_Lista)
                                {
                                    var feladat_ElvittEszkoz_ID_Item = db.Feladat_ElvittEszkoz_ID.FirstOrDefault(x => x.ID == item.ID);

                                    db.Feladat_ElvittEszkoz_ID.Remove(feladat_ElvittEszkoz_ID_Item);
                                }

                                //FELADAT - Felhasznalt Anyag(ok) kapcsolotábla kapcsolat
                                var feladat_FelhaszAnyag_ID_Lista = db.Feladat_FelhaszAnyag_ID.Where(x => x.FeladatID == feladatItem.ID).ToList();

                                foreach (var item in feladat_FelhaszAnyag_ID_Lista)
                                {
                                    var feladat_FelhaszAnyag_ID_Item = db.Feladat_FelhaszAnyag_ID.FirstOrDefault(x => x.ID == item.ID);
                                    db.Feladat_FelhaszAnyag_ID.Remove(feladat_FelhaszAnyag_ID_Item);

                                }

                                #endregion
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Nem ARCHÍVÁLHATÓ, mert nincs hozzá rendelve feladat, vagy valamelyik feladat nincs jóváhagyva!";
                            return View(model);
                        }
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                return View(projekt);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(projekt);
            }
        }

        // GET: Projekt/Delete/5
        public ActionResult Delete(int id)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Projektek.FirstOrDefault(x => x.ID == id);

                var vanKapcsolodoElem = db.Feladatok.Where(x => x.ProjektID == id).FirstOrDefault();

                if (vanKapcsolodoElem != null)
                {
                    TempData["ErrorMessage"] = "Nem törölhető, mert valamihez hozzá van rendelve!";
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        // POST: Projekt/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Projektek projekt)
        {
            try
            {
                projekt = db.Projektek.FirstOrDefault(x => x.ID == id);

                db.Projektek.Remove(projekt);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View();
            }
        }

        #region //Felhasznalo beazonositasanak metodusa

        [NonAction]
        public Users felhasznaloAzonositas()
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            Users tempUser = db.Users.Where(x => x.Elerhetosegek.Email == User.Identity.Name).FirstOrDefault();

            if (tempUser != null)
            {
                TempData["Users"] = $"{tempUser.Jogosultsag.Nev}"; //BelepettFelhasznalo;
            }
            else
            {
                TempData["Users"] = "";
            }

            return tempUser;
        }

        #endregion

        #region ExportToExcel function

        public void ExportToExcel(string id)
        {
            try
            {

                int XLSprojektID = int.Parse(id);

                ///Szukseg van az osszes feladatra, ami ehhez a projekthez tartozik

                List<Feladatok> aProjektFeladataiLista = db.Feladatok.Where(x => x.ProjektID == XLSprojektID && x.JovahagyvaCB).ToList();

                if (aProjektFeladataiLista.Count() == 0)
                {
                    TempData["ErrorMessage"] = "A projekt nem tartalmaz jóváhagyott feladatot!";

                    RedirectToAction("Index", "Projekt").ExecuteResult(this.ControllerContext);

                }
                else
                {

                    List<Munkalapok> model = new List<Munkalapok>();
                    List<Munkalapok> tempModel = new List<Munkalapok>();
                    List<Munkalapok> tempmunkalapokExcelLista = new List<Munkalapok>();

                    ///Feladatokhoz tartozo munkalapok leszurese
                    foreach (var item in aProjektFeladataiLista)
                    {
                        tempmunkalapokExcelLista = db.Munkalapok.Where(x => x.FeladatID == item.ID).ToList();

                        foreach (var feladatMunkalapja in tempmunkalapokExcelLista)
                        {
                            tempModel.Add(feladatMunkalapja);
                        }
                    }


                    ///A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                    ///DE ERDEMESEBB LENNE NEM EMBEREKRE LEBONTANI, MERT AKKOR A MUNKATERVEZOHOZ
                    ///HASONLOAN LEHETNE MEGCSINALNI, DE EGYELORE ez STORNO!!!
                    foreach (var item in tempModel/*tempmunkalapokExcelLista*/)
                    {
                        item.KoltsegekLista = db.Koltsegek.Where(x => x.MunkalapID == item.ID).ToList();
                        if (item.KoltsegekLista.Count() != 0)
                        {
                            foreach (var koltsegItem in item.KoltsegekLista)
                            {
                                item.koltsegItem += koltsegItem.KoltsegErteke;
                            }
                        }
                    }

                    //osszegek szsmitasa: munkaora total, koltsegek total, KM total
                    decimal MunkaoraTotal = 0;
                    int KMTotal = 0;
                    int KoltsegTotal = 0;

                    ///gruppolja a felhasznalokat, mejd idorendbe rendezi a munkalapokat
                    var testModel = tempModel.GroupBy(x => x.UserID).ToList();

                    tempModel.Clear();

                    foreach (var item in testModel)
                    {

                        tempModel.AddRange(item.OrderBy(x => x.MunkaDatuma).ToList());
                    }

                    ///ide mar lehet egybol tenni  a MODELT
                    model = tempModel./*tempmunkalapokExcelLista.Where(x => x.Megtekintve).OrderBy(x => x.JovahagyvaCB).ToList();*/Select(x => new Munkalapok
                    {
                        projNev = x.Feladatok.Projektek.AzonositoKod,
                        feladatNev = x.Feladatok.Nev,
                        userNev = $"{x.User.VezetekNev} {x.User.KeresztNev}",
                        MunkaDatuma = x.MunkaDatuma,
                        MunkaOra = x.MunkaOra,
                        koltsegItem = x.koltsegItem,
                        MegtettKM = x.MegtettKM,
                        UserID = x.UserID
                    }).ToList();

                    Munkalapok prevItem = new Munkalapok();

                    ExcelPackage pck = new ExcelPackage();
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                    string[] ABC = new string[] {"C","D","E","F","G","H","I","J","K",
                    "L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                    "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH"
                }; //31 nap van

                    ws.Cells["A3"].Value = "Nev";
                    ws.Cells["B1"].Value = "Projekt kódja";
                    ws.Cells["B3"].Value = "Munka dátuma";
                    ws.Cells["B4"].Value = "Feladat neve";
                    ws.Cells["B5"].Value = "Munka óra";
                    ws.Cells["B6"].Value = "Költségek";
                    ws.Cells["B7"].Value = "Megtett KM";


                    int columnStart = -1; // az A oszlopot mi adjuk hozza, a tobbi pedig a C-vel kezdodik
                    int rowStart = 3; //a 3. sorba irja irja csak a nevet
                    var tempDate = model[0].MunkaDatuma;
                    var tempUserID = model[0].UserID;

                    ///fejlecbe megadjuk melyik PROJEKTROL van szo
                    ws.Cells[string.Format($"C1")].Value = $"{model[0].projNev}";

                    ws.Cells[string.Format($"A{rowStart}")].Value = model[0].userNev;


                    foreach (var item in model)
                    {
                        if (tempUserID == item.UserID)
                        {
                            ///megkeressuk, hogy hanyadik bekezdesben van az o datuma
                            ///erre azert van szukseg, hogy ha letezik mar arra a napra feladat, akkor ezt is adja hozza.
                            if (prevItem.MunkaDatuma != item.MunkaDatuma)
                            {
                                columnStart++;

                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value =
                                    $"{item.MunkaDatuma.Year}.{item.MunkaDatuma.Month}.{item.MunkaDatuma.Day}";
                                rowStart++;
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.feladatNev;
                                rowStart++;
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.MunkaOra;
                                MunkaoraTotal += (decimal)item.MunkaOra;
                                rowStart++;
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.koltsegItem;
                                KoltsegTotal += item.koltsegItem;
                                rowStart++;
                                ///az EXCAPTION kizarasa vegett
                                if (item.MegtettKM == null)
                                {
                                    item.MegtettKM = 0;
                                }
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.MegtettKM;
                                KMTotal += (int)item.MegtettKM;
                                rowStart -= 4;

                            }
                            else ///hozza adjuk az elozo mezo erteket
                            {

                                rowStart++;
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value =
                                    ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value + " + " + item.feladatNev;
                                rowStart++;
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value =
                                    $"= {(ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value)} + {item.MunkaOra}".Replace("= =", "=");
                                MunkaoraTotal += (decimal)item.MunkaOra;
                                rowStart++;
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value =
                                    $"= {(ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value)} + {item.koltsegItem}".Replace("= =", "=");
                                KoltsegTotal += item.koltsegItem;
                                rowStart++;
                                //az EXCAPTION kizarasa vegett
                                if (item.MegtettKM == null)
                                {
                                    item.MegtettKM = 0;
                                }
                                ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value =
                                    $"= {(ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value)} + {item.MegtettKM}".Replace("= =", "=");
                                KMTotal += (int)item.MegtettKM;

                                rowStart -= 4;
                            }

                            prevItem = item;

                        }
                        else if (tempUserID != item.UserID)
                        {
                            ///elozo user osszegzesenek kiirasa
                            ws.Cells[string.Format($"A{rowStart + 1}")].Value =
                                $"TOTÁL m.o.: {MunkaoraTotal}";
                            ws.Cells[string.Format($"A{rowStart + 2}")].Value =
                                    $"TOTÁL költ.: {KoltsegTotal}";
                            ws.Cells[string.Format($"A{rowStart + 3}")].Value =
                                    $"TOTÁL km.: {KMTotal}";

                            MunkaoraTotal = 0;
                            KMTotal = 0;
                            KoltsegTotal = 0;

                            columnStart = 0;
                            rowStart += 5;

                            ///mivel mas a USER ezert uj sorba irjuk, es modositjuk az eddigi ID-t a mostanira
                            ws.Cells[$"A{rowStart}"].Value = "Nev";
                            ws.Cells[string.Format($"A{rowStart}")].Value = item.userNev;

                            ws.Cells[$"B{rowStart}"].Value = "Munka dátuma";
                            ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value =
                                $"{item.MunkaDatuma.Year}.{item.MunkaDatuma.Month}.{item.MunkaDatuma.Day}";
                            rowStart++;

                            ws.Cells[$"B{rowStart}"].Value = "Feladat neve";
                            ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.feladatNev;
                            rowStart++;

                            ws.Cells[$"B{rowStart}"].Value = "Munka óra";
                            ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.MunkaOra;
                            MunkaoraTotal += (decimal)item.MunkaOra;
                            rowStart++;

                            ws.Cells[$"B{rowStart}"].Value = "Költségek";
                            ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.koltsegItem;
                            KoltsegTotal += item.koltsegItem;
                            rowStart++;

                            //az EXCAPTION kizarasa vegett
                            if (item.MegtettKM == null)
                            {
                                item.MegtettKM = 0;
                            }

                            ws.Cells[$"B{rowStart}"].Value = "Megtett KM";
                            ws.Cells[string.Format($"{ABC[columnStart]}{rowStart}")].Value = item.MegtettKM;
                            KMTotal += (int)item.MegtettKM;

                            tempUserID = item.UserID;
                            rowStart -= 4;

                            prevItem = item;
                        }


                    }

                    ///az utolso user osszegzesenek kiirasa
                    ws.Cells[string.Format($"A{rowStart + 1}")].Value =
                        $"TOTÁL m.o.: {MunkaoraTotal}";
                    ws.Cells[string.Format($"A{rowStart + 2}")].Value =
                            $"TOTÁL költ.: {KoltsegTotal}";
                    ws.Cells[string.Format($"A{rowStart + 3}")].Value =
                            $"TOTÁL km.: {KMTotal}";

                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
                    Response.Flush();
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
            }

        }

        #endregion
    }
}
