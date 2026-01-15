using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using PagedList;
using SantaFactory.Models;


namespace SantaFactory.Controllers
{
    public class FeladatController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        #region INDEX

        // GET: Projekt
        public ActionResult Index(int? pn)
        {

            try
            {
                //List<Feladatok> userFeladatokListaja = new List<Feladatok>();

                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();
                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }
                else if (tempUser.Jogosultsag.Nev == "Alkalmazott")
                {
                    return RedirectToAction("Index", "Home");
                }

                List<Feladatok> model = new List<Feladatok>();

                int tempID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                model = db.Feladatok
                    .Where(x => x.FeladatTipusaID != tempID)
                    .OrderByDescending(x => x.VarhatoBefejezes).ToList();

                List<Feladatok> haveAtiranyitott = model.Where(f => f.Atiranyitott != 0).ToList();

                if (model.Count() != 0 && haveAtiranyitott.Count() != 0)
                {
                    foreach (var item in model)
                    {
                        if (item.Atiranyitott != 0)
                        {
                            item.Atiranyitott = 0;
                            item.KezdesiIdo = "8";

                            db.Feladatok.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                model = model.OrderBy(x => x.JovahagyvaCB).ThenBy(x => x.ID).ToList();
                model.First().LogUserJogosultsaga = tempUser.Jogosultsag.Nev;

                return View(model.ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region TEMP INDEX

        // GET: Projekt
        public ActionResult TempIndex(int? pn) ///a pn= page number :)
        {

            try
            {
                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();
                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }
                else if (tempUser.Jogosultsag.Nev == "Alkalmazott")
                {
                    return RedirectToAction("TempCreate", "Feladat");
                }

                int tempID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                var userTempFeladatokListaja = db.Feladatok
                    .Where(x => x.FeladatTipusaID == tempID)
                    .OrderBy(x => x.VarhatoBefejezes).ToList();

                //Csak a saját feladatát tudja szerkeszteni... de fölösleges
                //foreach (var item in userTempFeladatokListaja)
                //{
                //    if (!item.Elerhetosegek.Email.IsNullOrWhiteSpace() &&
                //        item.Elerhetosegek.Email == User.Identity.Name)
                //    {
                //        item.Sajat = true;
                //    }
                //}


                if (userTempFeladatokListaja.Count() != 0)
                {
                    foreach (var item in userTempFeladatokListaja)
                    {
                        if (item.Atiranyitott != 0)
                        {
                            item.Atiranyitott = 0;
                            item.KezdesiIdo = "8";

                            db.Feladatok.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                return View(userTempFeladatokListaja.ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat! (LEHET NINCS \"IDEIGLENES FELADATTÍPUS\")";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region INDEX Archiv

        // GET: Projekt
        public ActionResult IndexA(int? pn)
        {

            try
            {
                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();
                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }
                else if (tempUser.Jogosultsag.Nev == "Alkalmazott")
                {
                    return RedirectToAction("Index", "Home");
                }

                int tempID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                var userFeladatokListaja = db.FeladatokA
                    .OrderByDescending(x => x.VarhatoBefejezes).ToList();

                return View(userFeladatokListaja.ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Jovahagyott

        // GET: Projekt
        public ActionResult Jovahagyott(int? pn)
        {
            try
            {
                List<Feladatok> userFeladatokListaja = new List<Feladatok>();

                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();

                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }

                IQueryable<Feladat_User_ID> tempFeladatIDLista = db.Feladat_User_ID.Where(x => x.UserID == tempUser.UserID);

                foreach (var item in tempFeladatIDLista)
                {
                    Feladatok tempFeladat = db.Feladatok.FirstOrDefault(x => x.ID == item.FeladatID);

                    if (tempFeladat.JovahagyvaCB == true)
                    {
                        userFeladatokListaja.Add(tempFeladat);
                    }
                }

                if (userFeladatokListaja.Count() != 0)
                {
                    foreach (var item in userFeladatokListaja)
                    {
                        if (item.Atiranyitott != 0)
                        {
                            item.Atiranyitott = 0;
                            item.KezdesiIdo = "8";

                            db.Feladatok.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                return View(userFeladatokListaja.OrderBy(x => x.VarhatoKezdes).ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Nemjovahagyott

        // GET: Projekt
        public ActionResult Nemjovahagyott(int? pn)
        {
            try
            {
                List<Feladatok> userFeladatokListaja = new List<Feladatok>();

                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();

                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }

                var tempFeladatIDLista = db.Feladat_User_ID.Where(x => x.UserID == tempUser.UserID).ToList();

                foreach (var item in tempFeladatIDLista)
                {
                    Feladatok tempFeladat = db.Feladatok.FirstOrDefault(x => x.ID == item.FeladatID);

                    if (tempFeladat.JovahagyvaCB == false)
                    {
                        userFeladatokListaja.Add(tempFeladat);
                    }
                }

                if (userFeladatokListaja.Count() != 0)
                {
                    foreach (var item in userFeladatokListaja)
                    {
                        if (item.Atiranyitott != 0)
                        {
                            item.Atiranyitott = 0;
                            item.KezdesiIdo = "8";

                            db.Feladatok.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                return View(userFeladatokListaja.OrderBy(x => x.VarhatoKezdes).ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion


        #region // DETAILS

        public ActionResult Details(string id)
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int FeladatID = int.Parse(IDArray.First());

                var model = db.Feladatok.FirstOrDefault(x => x.ID == FeladatID);


                //listak lekerdezese
                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ViszonyLista = db.Viszony.ToList();
                model.ProjektekLista = db.Projektek.OrderBy(x => x.AzonositoKod).ToList();

                //FELHASZNALT ANYAGOK lekerdezese

                model.FelhasznaltAnyagokLista = new List<FelhasznaltAnyagok>();

                List<Feladat_FelhaszAnyag_ID> felhasznaltAnyagokIDListaja = db.Feladat_FelhaszAnyag_ID.Where(x => x.FeladatID == FeladatID).ToList();

                foreach (var item in felhasznaltAnyagokIDListaja)
                {
                    model.FelhasznaltAnyagokLista.Add(db.FelhasznaltAnyagok.Where(x => x.ID == item.FelhasznaltAnyagID).FirstOrDefault());
                }


                //userek feltoltese a feladatokba a chack box vegett

                model.UsersLista = db.Users.OrderBy(x => x.VezetekNev).ToList();

                //ezzel vizsgaljuk hogy van e olyan USER, aki reszt vesz ebben a feladatban, es fontos, hogy csak egyszer forduljon elo

                model.UsersFeladatokLista = db.Feladat_User_ID.Where(x => x.FeladatID == FeladatID).ToList();
                model.UsersFeladatokLista = model.UsersFeladatokLista.Distinct().ToList();

                //ESZKOZOK feltoltese a feladatokba a chack box vegett

                model.ElvihetoEszkozokLista = new List<ElvittEszkozok>();

                #region Kivalasztott eszkoz vizsgalaat

                //ezzel vizsgaljuk hogy van e olyan ESZKOZ, aki reszt vesz ebben a feladatban, es fontos, hogy csak egyszer forduljon elo

                List<Feladat_ElvittEszkoz_ID> feladatEszkozokIdLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == FeladatID).ToList();

                foreach (var item in feladatEszkozokIdLista)
                {
                    ElvittEszkozok tempElvittEszkoz = db.ElvittEszkozok.Where(x => x.ID == item.ElvittEszkozID).FirstOrDefault();
                    tempElvittEszkoz.EszkozModositva = (bool)item.Modositott;

                    if (item.Modositott != true)
                    {
                        tempElvittEszkoz.EszkozKivalasztva = true;

                        model.ElvihetoEszkozokLista.Add(tempElvittEszkoz);
                    }
                    else
                    {
                        tempElvittEszkoz.EszkozKivalasztva = (bool)item.KiVoltJelolve;

                        model.ElvihetoEszkozokLista.Add(tempElvittEszkoz);
                    }


                }
                #endregion

                //munkalapok listajanak kigyuhtese
                ///itt nyerem ki, hogy kinek kellenek a munkalapjai, ha ADMIN, vagy VEZETOSEG!!!
                ///DE fontos, hogy csak a feladathoz tartozo munkalapokat legyenek levalasztva

                var aFeladatMunkalapjainakAListaja = db.Munkalapok.Where(x => x.FeladatID == FeladatID).ToList();

                //if nem egyenlo ALKALMAZOTT-ra modositani
                if (tempUser.Jogosultsag.Nev != "Alkalmazott")
                {
                    model.MunkalapokListaja = aFeladatMunkalapjainakAListaja.OrderByDescending(x => x.MunkaDatuma).ToList();
                }
                else
                {
                    model.MunkalapokListaja = aFeladatMunkalapjainakAListaja.Where(x => x.UserID == tempUser.UserID).OrderByDescending(x => x.MunkaDatuma).ToList();
                }

                //ez helyettesti az atiranyitott opciot
                foreach (var item in model.MunkalapokListaja)
                {
                    if (item.Atiranyitott != 1)
                    {
                        item.Atiranyitott = 1;

                        item.KezdesiIdo = item.KIdo.ToString();
                        item.BefejezesiIdo = item.BIdo.ToString();

                        ///itt allitodik, hogy ha azonos a USER, es meg nem nezte meg, akkor kipipalodik

                        db.Munkalapok.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        /////itt allitodik, hogy ha azonos a USER, es meg nem nezte meg, akkor kipipalodik

                    }
                }

                model.UserNev = $"{tempUser.VezetekNev} {tempUser.KeresztNev}";

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region // DETAILS Archiv

        public ActionResult DetailsA(string id)
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int FeladatAID = int.Parse(IDArray.First());

                var model = db.FeladatokA.FirstOrDefault(x => x.ID == FeladatAID);


                //listak lekerdezese
                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ViszonyLista = db.Viszony.ToList();
                model.ProjektekLista = db.ProjektekA.OrderBy(x => x.AzonositoKod).ToList();

                //FELHASZNALT ANYAGOK lekerdezese

                model.FelhasznaltAnyagALista = new List<FelhasznaltAnyagA>();

                model.FelhasznaltAnyagALista = db.FelhasznaltAnyagA.Where(fa => fa.FeladatAID == FeladatAID).ToList();

                //munkalapok listajanak kigyuhtese
                ///itt nyerem ki, hogy kinek kellenek a munkalapjai, ha ADMIN, vagy VEZETOSEG!!!
                ///DE fontos, hogy csak a feladathoz tartozo munkalapokat legyenek levalasztva

                var aFeladatMunkalapjainakAListaja = db.MunkalapokA.Where(x => x.FeladatAID == FeladatAID).ToList();

                //if nem egyenlo ALKALMAZOTT-ra modositani
                if (tempUser.Jogosultsag.Nev != "Alkalmazott")
                {
                    model.MunkalapokListaja = aFeladatMunkalapjainakAListaja.OrderByDescending(x => x.MunkaDatuma).ToList();
                }
                else
                {
                    model.MunkalapokListaja = aFeladatMunkalapjainakAListaja.Where(x => x.UserID == tempUser.UserID).OrderByDescending(x => x.MunkaDatuma).ToList();
                }

                //ez helyettesti az atiranyitott opciot
                foreach (var item in model.MunkalapokListaja)
                {
                    if (item.Atiranyitott != 1)
                    {
                        item.Atiranyitott = 1;

                        item.KezdesiIdo = item.KIdo.ToString();
                        item.BefejezesiIdo = item.BIdo.ToString();

                        if (item.UserID == tempUser.UserID)
                        {
                            item.Megtekintve = true;
                        }

                        db.MunkalapokA.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        if (item.UserID == tempUser.UserID && !(item.Megtekintve))
                        {
                            item.Megtekintve = true;

                            item.KezdesiIdo = item.KIdo.ToString();
                            item.BefejezesiIdo = item.BIdo.ToString();

                            db.MunkalapokA.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                }

                model.UserNev = $"{tempUser.VezetekNev} {tempUser.KeresztNev}";

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Create GET

        // GET: Viszonyok/Create
        public ActionResult Create()
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }
            else if (tempUser.Jogosultsag.Nev.ToLower() != "admin" && tempUser.Jogosultsag.Nev.ToLower() != "vezetőség")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                Feladatok model = new Feladatok();

                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ViszonyLista = db.Viszony.OrderBy(x => x.Nev).ToList();
                model.ProjektekLista = db.Projektek.OrderByDescending(x => x.AzonositoKod).ToList();
                model.UsersFeladatokLista = db.Feladat_User_ID.ToList();

                //Elviheto eszkozok kiirasa
                model.ElvihetoEszkozokLista = db.ElvittEszkozok.OrderBy(x => x.Nev).ToList();

                foreach (var item in model.ElvihetoEszkozokLista)
                {
                    //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                    ElvittEszkozokFeladatokOsszefugges elvittEszkozokFeladatokOsszefuggesItem = new ElvittEszkozokFeladatokOsszefugges();

                    elvittEszkozokFeladatokOsszefuggesItem.EszkozID = item.ID;
                    elvittEszkozokFeladatokOsszefuggesItem.EszkozNeve = item.Nev;

                    //egy belső módosított változó létrehozása
                    if (model.KiirniEszkozokLista == null)
                    {
                        model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>();
                    }

                    model.KiirniEszkozokLista.Add(elvittEszkozokFeladatokOsszefuggesItem);
                }

                //megvedi a CREATE fulet az ERROR-tol, ha ures lenne a lista!!!
                if (model.KiirniEszkozokLista == null)
                {
                    ElvittEszkozokFeladatokOsszefugges TEMP = new ElvittEszkozokFeladatokOsszefugges();

                    TEMP.EszkozID = 0;
                    TEMP.EszkozNeve = "EMPTY";
                    TEMP.EszkozKivalasztva = false;
                    TEMP.ExEszkozFeladatID = 0;

                    model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>();

                    model.KiirniEszkozokLista.Add(TEMP);
                }

                //END

                //userek feltoltese a feladatokba a chack box vegett, DE CSAK AKKOR, ha szabadok
                model.UsersLista = db.Users.OrderBy(x => x.VezetekNev).ToList();

                foreach (var item in model.UsersLista)
                {
                    //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                    FeladatokUsersOsszefugges feladatokUsersOsszefuggesItem = new FeladatokUsersOsszefugges();

                    feladatokUsersOsszefuggesItem.UsersID = item.UserID;
                    feladatokUsersOsszefuggesItem.UsersNeve = $"{item.VezetekNev} {item.KeresztNev}";

                    //egy beelső módosított változó létrehozása
                    if (model.FeladatokUsersLista == null)
                    {
                        model.FeladatokUsersLista = new List<FeladatokUsersOsszefugges>();
                    }

                    model.FeladatokUsersLista.Add(feladatokUsersOsszefuggesItem);
                }

                //erre csak akkor lett volna szukseg, ha feladaton belül lehet ESZKÖZÖKET a feladathoz rendelni
                //...de macerás megoldani, úgyhogy először a feladatot kell létrehozni

                //megvedi a CREATE fulet az ERROR-tol, ha ures lenne a lista!!!
                if (model.FeladatokUsersLista == null)
                {
                    FeladatokUsersOsszefugges TEMP = new FeladatokUsersOsszefugges();

                    TEMP.UsersID = 0;
                    TEMP.UsersNeve = "EMPTY";
                    TEMP.UserKivalasztva = false;
                    TEMP.ExUsersFeladatokID = 0;

                    model.FeladatokUsersLista = new List<FeladatokUsersOsszefugges>();

                    model.FeladatokUsersLista.Add(TEMP);
                }

                ///aki letrehozza a feladatot kezdetben az O elerhetosege kerul be kontakt infonak

                return View(model);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }

        }

        #endregion


        #region // POST: Create

        [HttpPost]
        public ActionResult Create(Feladatok feladat)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    ///Ha EXEPTION lenne, akkor tudja ujratolteni a modelt
                    //Ha EXEPTION lenne, akkor tudja ujratolteni a modelt
                    feladat.FeladatTipusokLista = db.FeladatTipusok.ToList();
                    feladat.ViszonyLista = db.Viszony.OrderBy(x => x.Nev).ToList();
                    feladat.ProjektekLista = db.Projektek.OrderBy(x => x.AzonositoKod).ToList();

                    //FELHASZNALOK nevenek ujrafeltoltese
                    foreach (var nevvelFeltoltendoItem in feladat.FeladatokUsersLista)
                    {
                        nevvelFeltoltendoItem.UsersNeve = $"{db.Users.FirstOrDefault(x => x.UserID == nevvelFeltoltendoItem.UsersID).VezetekNev} " +
                            $"{db.Users.FirstOrDefault(x => x.UserID == nevvelFeltoltendoItem.UsersID).KeresztNev}";
                    }

                    //ESZKOZOK nevenek ujrafeltoltese
                    foreach (var nevvelFeltoltendoItem in feladat.KiirniEszkozokLista)
                    {
                        nevvelFeltoltendoItem.EszkozNeve = $"{db.ElvittEszkozok.FirstOrDefault(x => x.ID == nevvelFeltoltendoItem.EszkozID).Nev}";
                    }

                    //userek feltoltese a feladatokba a chack box vegett, DE CSAK AKKOR, ha szabadok
                    feladat.UsersLista = db.Users.OrderBy(x => x.VezetekNev).ToList();

                    //a viszony erteke a NEM JOVAHAGYOTT
                    Viszony tempViszont = db.Viszony.FirstOrDefault(x => x.Nev == "Nem jóváhagyott");
                    feladat.ViszonyID = tempViszont.ID;

                    //kezdesi ido kinyerese
                    string[] TimeArray = new string[2];


                    TimeArray[0] = feladat.KezdesiIdo.Substring(0, 2);
                    TimeArray[1] = feladat.KezdesiIdo.Substring(3, 2);
                    feladat.Kido = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);

                    int befejezesiIdo = int.Parse(TimeArray[0]) + (int)(feladat.BecsultMunkaOra);
                    TimeSpan feladatBIdo = new TimeSpan((befejezesiIdo == 24) ? 00 : befejezesiIdo, int.Parse(TimeArray[1]), 00);

                    ///ellenorizzuk, hogy van e mar felhasznalo foglalkoztatva ebben az idoszakban
                    List<Munkalapok> meglevoMunkalapok = db.Munkalapok.Where(x => !(x.JovahagyvaCB)).ToList();

                    var napKezdes = (DateTime)(feladat.VarhatoKezdes);
                    var napVege = (DateTime)(feladat.VarhatoBefejezes);
                    var feladatKezdesHonap = (DateTime)(feladat.VarhatoKezdes);
                    var feladatBefejezesHonap = (DateTime)(feladat.VarhatoBefejezes);

                    ///megvizsgaljuk, hogy az adott idoben a kivalasztott szemelyeknek van e mas feladatuk
                    var kivalasztottUserLista = feladat.FeladatokUsersLista.Where(x => x.UserKivalasztva).ToList();

                    for (int i = (napKezdes.Day); i <= napVege.Day; i++)
                    {
                        foreach (var tempMunkalapItem in meglevoMunkalapok)
                        {

                            if ((tempMunkalapItem.MunkaDatuma.Year == feladatKezdesHonap.Year ||
                             tempMunkalapItem.MunkaDatuma.Year == feladatBefejezesHonap.Year) &&
                             tempMunkalapItem.MunkaDatuma.Day == i &&
                            (tempMunkalapItem.MunkaDatuma.Month == feladatKezdesHonap.Month ||
                            tempMunkalapItem.MunkaDatuma.Month == feladatBefejezesHonap.Month))
                            {
                                foreach (var kivalasztottUserItem in kivalasztottUserLista)
                                {
                                    if (tempMunkalapItem.User.UserID == kivalasztottUserItem.UsersID)
                                    {

                                        if ((tempMunkalapItem.KIdo > feladat.Kido && feladatBIdo > tempMunkalapItem.KIdo)
                                            || (tempMunkalapItem.KIdo < feladat.Kido && tempMunkalapItem.BIdo > feladat.Kido))
                                        {
                                            ///név + dátum + feladat ID hozzáadása
                                            TempData["ErrorMessage"] = $"A feladat nem létrehozható, mert " +
                                                    $"{tempMunkalapItem.User.VezetekNev} {tempMunkalapItem.User.KeresztNev} " +
                                                    $"rendelkezik feladattal (ID. {tempMunkalapItem.FeladatID}) " +
                                                    $"a {tempMunkalapItem.Datum.Year}.{tempMunkalapItem.Datum.Month}.{tempMunkalapItem.Datum.Day} dátumon!";


                                            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                                            var tempUser = felhasznaloAzonositas();
                                            if (tempUser == null)
                                            {
                                                return RedirectToAction("Bejelentkezes", "User_");
                                            }
                                            else if (tempUser.Jogosultsag.Nev != "ADMIN" && tempUser.Jogosultsag.Nev != "Vezetőség")
                                            {
                                                return RedirectToAction("Index", "Home");
                                            }

                                            return View(feladat);
                                        }

                                    }
                                }
                            }
                        }
                    }

                    db.Feladatok.Add(feladat);
                    db.SaveChanges();

                    //Kivalasztott ESZKOZ hozzaadasa, vizsgalni kell, hogy csak egyszer adja hoza a listahoz, es ha modosul akkor torolje is ki!
                    if (feladat.KiirniEszkozokLista.Count() != 0)
                    {
                        foreach (var item in feladat.KiirniEszkozokLista)
                        {
                            if (item.EszkozKivalasztva)
                            {
                                Feladat_ElvittEszkoz_ID feladat_ElvittEszkoz_ID = new Feladat_ElvittEszkoz_ID();

                                feladat_ElvittEszkoz_ID.ElvittEszkozID = item.EszkozID;
                                feladat_ElvittEszkoz_ID.FeladatID = feladat.ID;
                                feladat_ElvittEszkoz_ID.KiVoltJelolve = false;
                                feladat_ElvittEszkoz_ID.Modositott = false;

                                db.Feladat_ElvittEszkoz_ID.Add(feladat_ElvittEszkoz_ID);
                                //db.SaveChanges();
                            }
                        }
                    }

                    int LetrehozandoMunkalapokCount = 0;

                    if (kivalasztottUserLista.Count() >= 2)
                    {
                        LetrehozandoMunkalapokCount = ((((TimeSpan)(feladat.VarhatoBefejezes - feladat.VarhatoKezdes)).Days + 1 /*ez a 0. napot mutatja -> AZNAP*/)
                            * kivalasztottUserLista.Count());
                    }
                    else
                    {
                        LetrehozandoMunkalapokCount = (1 + ((TimeSpan)(feladat.VarhatoBefejezes - feladat.VarhatoKezdes)).Days * kivalasztottUserLista.Count());
                    }

                    //Kivalasztott FELHASZNALOK hozzaadasa, vizsgalni kell, hogy csak egyszer adja hoza a listahoz
                    if (LetrehozandoMunkalapokCount > 0)
                    {
                        foreach (var item in kivalasztottUserLista)
                        {
                            Feladat_User_ID userFeladatokID = new Feladat_User_ID();

                            userFeladatokID.UserID = item.UsersID;
                            userFeladatokID.FeladatID = feladat.ID;

                            db.Feladat_User_ID.Add(userFeladatokID);
                            db.SaveChanges();

                            //miutan letrehoztam a feladatot fontos, hogy letrehozzam melle a szukseges MUNKALAPOKAT, de csak akkor, ha van hozza rendelve alkalmazott!
                            if ((feladat.VarhatoKezdes != null || feladat.VarhatoBefejezes != null) && kivalasztottUserLista.Count() != 0)
                            {
                                //kinyerem a ket datum kozti kulonbseget, de az adott napot nem adja hozza igy az utolag kell potolni

                                ///a kulonbsegbol megkapom, hogy hany naprol van szo, de startbol az adott napot hozza kell adni
                                byte letrehozandoMunkalapSzam = (byte)(LetrehozandoMunkalapokCount / kivalasztottUserLista.Count());

                                if (letrehozandoMunkalapSzam > 0)
                                {
                                    for (int i = 0; i < letrehozandoMunkalapSzam; i++)
                                    {
                                        Munkalapok munkalap = new Munkalapok();

                                        ///kotelezoen kitoltendok (not null)
                                        munkalap.KezdesiIdo = $"x";
                                        munkalap.BefejezesiIdo = $"y";
                                        munkalap.UserID = item.UsersID;
                                        munkalap.Datum = DateTime.Now;
                                        munkalap.JovahagyvaCB = false;
                                        munkalap.FeladatID = feladat.ID;
                                        munkalap.Megtekintve = false;

                                        //alap adatok az index lista letrehozazasahoz

                                        //a kulonbsegbol megkapom, hogy hany orat dolgozott
                                        if (feladat.BecsultMunkaOra > 4 && feladat.BecsultMunkaOra <= 10)
                                        {
                                            munkalap.MunkaOra = feladat.BecsultMunkaOra - Convert.ToDecimal(0.50);
                                        }
                                        else if (feladat.BecsultMunkaOra > 10)
                                        {
                                            munkalap.MunkaOra = feladat.BecsultMunkaOra - 1;
                                        }
                                        else
                                        {
                                            munkalap.MunkaOra = feladat.BecsultMunkaOra;
                                        }

                                        ///a letrehozott munkalapokba is keruljon bele a szunet, 
                                        ///es ahany napra van letrehozva munkalap az illetonek, egyezzen a datumozas

                                        munkalap.MunkaDatuma = new DateTime((feladat.VarhatoKezdes.Value).Year,
                                                   (feladat.VarhatoKezdes.Value).Month, (feladat.VarhatoKezdes.Value).Day + i);

                                        munkalap.KIdo = feladat.Kido;

                                        //kezdesi ido atadasa a feladattol

                                        munkalap.BIdo = new TimeSpan((befejezesiIdo == 24) ? 00 : befejezesiIdo, int.Parse(TimeArray[1]), 00);
                                        munkalap.MegtettKM = 0;

                                        db.Munkalapok.Add(munkalap);
                                        db.SaveChanges();
                                    }
                                }

                            }

                        }

                    }

                    return RedirectToAction("Details", "Feladat", new { @id = feladat.ID });
                }

                return View(feladat);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(feladat);
            }
        }

        #endregion


        #region TEMPCreate GET


        // GET: Viszonyok/Create
        public ActionResult TempCreate()
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                Feladatok model = new Feladatok();

                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ViszonyLista = db.Viszony.OrderBy(x => x.Nev).ToList();
                model.ProjektekLista = db.Projektek.OrderByDescending(x => x.AzonositoKod).ToList();

                //Elviheto eszkozok kiirasa
                model.ElvihetoEszkozokLista = db.ElvittEszkozok.OrderBy(x => x.Nev).ToList();

                foreach (var item in model.ElvihetoEszkozokLista)
                {
                    //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                    ElvittEszkozokFeladatokOsszefugges elvittEszkozokFeladatokOsszefuggesItem = new ElvittEszkozokFeladatokOsszefugges();

                    elvittEszkozokFeladatokOsszefuggesItem.EszkozID = item.ID;
                    elvittEszkozokFeladatokOsszefuggesItem.EszkozNeve = item.Nev;

                    //egy belső módosított változó létrehozása
                    if (model.KiirniEszkozokLista == null)
                    {
                        model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>();
                    }

                    model.KiirniEszkozokLista.Add(elvittEszkozokFeladatokOsszefuggesItem);

                }

                //megvedi a CREATE fulet az ERROR-tol, ha ures lenne a lista!!!
                if (model.KiirniEszkozokLista == null)
                {
                    ElvittEszkozokFeladatokOsszefugges TEMP = new ElvittEszkozokFeladatokOsszefugges();

                    TEMP.EszkozID = 0;
                    TEMP.EszkozNeve = "EMPTY";
                    TEMP.EszkozKivalasztva = false;
                    TEMP.ExEszkozFeladatID = 0;

                    model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>();

                    model.KiirniEszkozokLista.Add(TEMP);
                }

                return View(model);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion


        #region // POST: TEMPCreate

        [HttpPost]
        public ActionResult TempCreate(Feladatok feladat)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    //a viszony erteke a NEM JOVAHAGYOTT
                    Viszony tempViszont = db.Viszony.FirstOrDefault(x => x.Nev == "Nem jóváhagyott");
                    feladat.ViszonyID = tempViszont.ID;

                    //kezdesi ido kinyerese
                    string[] TimeArray = new string[2];


                    TimeArray[0] = feladat.KezdesiIdo.Substring(0, 2);
                    TimeArray[1] = feladat.KezdesiIdo.Substring(3, 2);
                    feladat.Kido = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);

                    int befejezesiIdo = int.Parse(feladat.KezdesiIdo.Substring(0, 2)) + (int)feladat.BecsultMunkaOra;
                    TimeSpan feladatBIdo = new TimeSpan(befejezesiIdo, int.Parse(TimeArray[1]), 00);

                    db.Feladatok.Add(feladat);
                    db.SaveChanges();

                    //Kivalasztott ESZKOZ hozzaadasa, vizsgalni kell, hogy csak egyszer adja hoza a listahoz, es ha modosul akkor torolje is ki!
                    var kivalasztottFeladatok = feladat.KiirniEszkozokLista.Where(x => x.EszkozKivalasztva == true).ToList();

                    if (kivalasztottFeladatok.Any())
                    {
                        foreach (var item in kivalasztottFeladatok)
                        {
                            Feladat_ElvittEszkoz_ID feladat_ElvittEszkoz_ID = new Feladat_ElvittEszkoz_ID();

                            feladat_ElvittEszkoz_ID.ElvittEszkozID = item.EszkozID;
                            feladat_ElvittEszkoz_ID.FeladatID = feladat.ID;
                            feladat_ElvittEszkoz_ID.KiVoltJelolve = false;
                            feladat_ElvittEszkoz_ID.Modositott = false;

                            db.Feladat_ElvittEszkoz_ID.Add(feladat_ElvittEszkoz_ID);
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("TempIndex", "Feladat");
                }

                return View(feladat);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(feladat);
            }
        }

        public ActionResult TempCreateFromLetter(Users bekuldottAdatok)
        {
            try
            {

                var osszesAjandek = db.FeladatTipusok.ToList();
                var KivalasztotAjandekokNeve = new List<string>();

                // 1. Gyűjtsük össze a kiválasztott ajándék nevét, majd mindre szánunk 1 munkaórát
                if (bekuldottAdatok.KivalasztotAjandek1 != null)
                    KivalasztotAjandekokNeve.Add(osszesAjandek.First(x => x.ID == bekuldottAdatok.KivalasztotAjandek1).Nev);

                if (bekuldottAdatok.KivalasztotAjandek2 != null)
                    KivalasztotAjandekokNeve.Add(osszesAjandek.First(x => x.ID == bekuldottAdatok.KivalasztotAjandek2).Nev);

                if (bekuldottAdatok.KivalasztotAjandek3 != null)
                    KivalasztotAjandekokNeve.Add(osszesAjandek.First(x => x.ID == bekuldottAdatok.KivalasztotAjandek3).Nev);

                if (KivalasztotAjandekokNeve.Count == 0)
                {
                    KivalasztotAjandekokNeve.Add("Egyedi_ajándék");
                }

                // 2. Minden ajándékhoz hozzunk létre egy feladatot
                foreach (var kivalasztottAjandekNeve in KivalasztotAjandekokNeve)
                {

                    //a viszony erteke a NEM JOVAHAGYOTT
                    int tempViszonyID = db.Viszony.First(x => x.Nev.ToUpper() == "NEM JÓVÁHAGYOTT").ID;
                    int feladatTipusID = db.FeladatTipusok.First(x => x.Nev.ToUpper() == "IDEIGLENES").ID;
                    string projektAzonosito = $"{DateTime.Now.Year.ToString()}_Temp";
                    int ideiglenesProjektId = db.Projektek.Where(x => x.AzonositoKod == projektAzonosito).FirstOrDefault().ID;
                    DateTime AktualisNapMasnapja = new DateTime(DateTime.Now.Year,
                                                                DateTime.Now.Month,
                                                                DateTime.Now.Day + 1,
                                                                DateTime.Now.Hour,
                                                                DateTime.Now.Minute,
                                                                DateTime.Now.Second);

                    var ujIdeiglenesfeladat = new Feladatok();

                    ujIdeiglenesfeladat.Nev = kivalasztottAjandekNeve + " _levél";
                    ujIdeiglenesfeladat.FeladatLeirasa = bekuldottAdatok.LevelUzenete;
                    ujIdeiglenesfeladat.ProjektID = ideiglenesProjektId;
                    ujIdeiglenesfeladat.VarhatoKezdes = AktualisNapMasnapja;
                    ujIdeiglenesfeladat.VarhatoBefejezes = AktualisNapMasnapja;
                    ujIdeiglenesfeladat.KezdesiIdo = $"{AktualisNapMasnapja.Hour}:{AktualisNapMasnapja.Minute}";
                    ujIdeiglenesfeladat.Kido = new TimeSpan(AktualisNapMasnapja.Hour, AktualisNapMasnapja.Minute, 00);
                    ujIdeiglenesfeladat.BecsultMunkaOra = 1;
                    ujIdeiglenesfeladat.ViszonyID = tempViszonyID;
                    ujIdeiglenesfeladat.FeladatTipusaID = feladatTipusID;

                    //Cím
                    ujIdeiglenesfeladat.Cimek = new Cimek()
                    {
                        Orszag = "Északi sark",
                        Varos = "Lapföld",
                        Megjegyzes = "Ideiglenes feladat létrehozása, levélből."
                    };

                    //Elérhetőség
                    ujIdeiglenesfeladat.Elerhetosegek = new Elerhetosegek()
                    {
                        Telszam1 = "+0100000000",
                        Email = "level@amikulasnak.com"
                    };

                    db.Feladatok.Add(ujIdeiglenesfeladat);
                    db.SaveChanges();
                }

                TempData["ErrorMessage"] = "Köszönjük a levelet! A mikulás műhely manói már dolgoznak az ajándékodon!";
                return RedirectToAction("Bejelentkezes", "Users");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region // GET: Edit


        public ActionResult Edit(int id)
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }
            else if (tempUser.Jogosultsag.Nev != "ADMIN" && tempUser.Jogosultsag.Nev != "Vezetőség")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                int FeladatID = id;

                List<Munkalapok> feladatMunkalapjaiLista = db.Munkalapok.Where(x => x.FeladatID == FeladatID).ToList();

                var model = db.Feladatok.FirstOrDefault(x => x.ID == FeladatID);

                model.KezdesiIdo = $"{model.Kido}";
                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ViszonyLista = db.Viszony.OrderBy(x => x.Nev).ToList();
                model.ProjektekLista = db.Projektek.OrderBy(x => x.AzonositoKod).ToList();

                model.Atiranyitott = 1;

                //az osszes user lsitazasa
                model.UsersLista = db.Users.OrderBy(x => x.UserID).ToList();

                //feladatban resztvevo FELHASZNALOK (ID) listaja 
                model.UsersFeladatokLista = db.Feladat_User_ID.Where(x => x.FeladatID == FeladatID).OrderBy(x => x.UserID).ToList();


                //belső módosított változó létrehozása
                model.FeladatokUsersLista = new List<FeladatokUsersOsszefugges>();
                int i = 0;

                foreach (var item in model.UsersLista)
                {
                    if (i < model.UsersFeladatokLista.Count() && item.UserID == model.UsersFeladatokLista[i].UserID)
                    {
                        //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                        FeladatokUsersOsszefugges feladatokUsersOsszefuggesITEM = new FeladatokUsersOsszefugges();

                        feladatokUsersOsszefuggesITEM.UsersID = item.UserID;
                        feladatokUsersOsszefuggesITEM.UsersNeve = $"{item.VezetekNev} {item.KeresztNev}";
                        feladatokUsersOsszefuggesITEM.UserKivalasztva = true;
                        feladatokUsersOsszefuggesITEM.ExUsersFeladatokID = model.UsersFeladatokLista[i].ID;

                        model.FeladatokUsersLista.Add(feladatokUsersOsszefuggesITEM);

                        i++;
                    }
                    else
                    {

                        //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                        FeladatokUsersOsszefugges feladatokUsersOsszefuggesITEM = new FeladatokUsersOsszefugges();

                        feladatokUsersOsszefuggesITEM.UsersID = item.UserID;
                        feladatokUsersOsszefuggesITEM.UsersNeve = $"{item.VezetekNev} {item.KeresztNev}";
                        feladatokUsersOsszefuggesITEM.UserKivalasztva = false;

                        model.FeladatokUsersLista.Add(feladatokUsersOsszefuggesITEM);

                    }

                }

                var temp = model.FeladatokUsersLista.OrderBy(x => x.UsersNeve).ToList();

                model.FeladatokUsersLista = temp;


                //Elviheto ESZKOZOK listaja
                model.ElvihetoEszkozokLista = db.ElvittEszkozok.OrderBy(x => x.Nev).ToList();

                //Azok az ESZKOZOK, amik mar tarsitva vannak a feladathoz.... ha van ilyen

                model.FeladatokElvittEszkozokIDjaLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == FeladatID).OrderBy(x => x.ElvittEszkozID).ToList();

                //belső módosított változó létrehozása
                model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>();
                i = 0;

                foreach (var item in model.ElvihetoEszkozokLista)
                {

                    //ha ki volt valasztva, vagy MODOSITOTT es vmikor ki volt valasztva akkor...
                    if (i < model.FeladatokElvittEszkozokIDjaLista.Count() && item.ID == model.FeladatokElvittEszkozokIDjaLista[i].ElvittEszkozID)
                    {
                        //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                        ElvittEszkozokFeladatokOsszefugges elvittEszkozokFeladatokOsszefuggesItem = new ElvittEszkozokFeladatokOsszefugges();

                        elvittEszkozokFeladatokOsszefuggesItem.EszkozID = item.ID;
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozNeve = item.Nev;
                        elvittEszkozokFeladatokOsszefuggesItem.ExEszkozFeladatID = model.FeladatokElvittEszkozokIDjaLista[i].ID;

                        //a kiirt nev athuzasanak az erdekeben kell
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozKivalasztva = true/*(bool)(model.FeladatokElvittEszkozokIDjaLista[i].KiVoltJelolve)*/;
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozModositva = (bool)(model.FeladatokElvittEszkozokIDjaLista[i].Modositott);


                        model.KiirniEszkozokLista.Add(elvittEszkozokFeladatokOsszefuggesItem);

                        i++;
                    }
                    //ha ki NEM volt valasztva, akkor...
                    else
                    {

                        //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                        ElvittEszkozokFeladatokOsszefugges elvittEszkozokFeladatokOsszefuggesItem = new ElvittEszkozokFeladatokOsszefugges();

                        elvittEszkozokFeladatokOsszefuggesItem.EszkozID = item.ID;
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozNeve = item.Nev;

                        elvittEszkozokFeladatokOsszefuggesItem.EszkozKivalasztva = false;

                        model.KiirniEszkozokLista.Add(elvittEszkozokFeladatokOsszefuggesItem);
                    }

                }
                return View(model);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region // POST: Edit

        [HttpPost]
        public ActionResult Edit(int id, Feladatok feladatok)
        {
            try
            {
                //Ha EXEPTION lenne, akkor tudja ujratolteni a modelt
                feladatok.FeladatTipusokLista = db.FeladatTipusok.ToList();
                feladatok.ViszonyLista = db.Viszony.OrderBy(x => x.Nev).ToList();
                feladatok.ProjektekLista = db.Projektek.OrderBy(x => x.AzonositoKod).ToList();

                //FELHASZNALOK nevenek ujrafeltoltese
                foreach (var nevvelFeltoltendoItem in feladatok.FeladatokUsersLista)
                {
                    nevvelFeltoltendoItem.UsersNeve = $"{db.Users.FirstOrDefault(x => x.UserID == nevvelFeltoltendoItem.UsersID).VezetekNev} " +
                        $"{db.Users.FirstOrDefault(x => x.UserID == nevvelFeltoltendoItem.UsersID).KeresztNev}";
                }

                //ESZKOZOK nevenek ujrafeltoltese
                foreach (var nevvelFeltoltendoItem in feladatok.KiirniEszkozokLista)
                {
                    nevvelFeltoltendoItem.EszkozNeve = $"{db.ElvittEszkozok.FirstOrDefault(x => x.ID == nevvelFeltoltendoItem.EszkozID).Nev}";
                }

                var tempFeladatTipusokID = db.FeladatTipusok.FirstOrDefault(feladat => feladat.Nev.ToUpper() == "IDEIGLENES").ID;

                ///csak akkor valik ervenyesse ha modosult a feladat tipusa TEMP-rol!!!
                if (feladatok.FeladatTipusaID == tempFeladatTipusokID)
                {
                    feladatok.Atiranyitott = 1;
                    TempData["ErrorMessage"] = "A feladat csak akkor módosítható, ha a feladat típusa nem IDEIGLENES !";

                    return View(feladatok);
                }

                #region Már van erre az időpontra más munkalap vizsgálat és IDEIGLENES VOLT ELŐTTE
                var model = db.Feladatok.FirstOrDefault(x => x.ID == id);
                int ideiglenesTipusID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                //kezdesi ido kinyerese
                string[] TimeArray = new string[2];

                TimeArray[0] = feladatok.KezdesiIdo.Substring(0, 2);
                TimeArray[1] = feladatok.KezdesiIdo.Substring(3, 2);
                feladatok.Kido = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);

                int befejezesiIdo = int.Parse(TimeArray[0]) + (int)(feladatok.BecsultMunkaOra);
                TimeSpan feladatBIdo = new TimeSpan((befejezesiIdo == 24) ? 00 : befejezesiIdo, int.Parse(TimeArray[1]), 00);

                var napKezdes = (DateTime)(feladatok.VarhatoKezdes);
                var napVege = (DateTime)(feladatok.VarhatoBefejezes);
                var feladatKezdesHonap = (DateTime)(feladatok.VarhatoKezdes);
                var feladatBefejezesHonap = (DateTime)(feladatok.VarhatoBefejezes);

                //CSAK AKKOR FUSSON BELE, HA ELŐTTE IDEIGLENES VOLT A FELADAT
                if (model.FeladatTipusaID == ideiglenesTipusID ||
                    feladatok.JovahagyvaCB == false)
                {
                    ///ellenorizzuk, hogy van e mar felhasznalo foglalkoztatva ebben az idoszakban
                    IQueryable<Munkalapok> meglevoMunkalapok = db.Munkalapok.Where(x => !(x.JovahagyvaCB));

                    ///megvizsgaljuk, hogy az adott idoben a kivalasztott szemelyeknek van e mas feladatuk
                    var kivalasztottUsersLista = feladatok.FeladatokUsersLista.Where(x => x.UserKivalasztva).ToList();

                    for (int i = (napKezdes.Day); i <= napVege.Day; i++)
                    {
                        foreach (var tempMunkalapItem in meglevoMunkalapok)
                        {

                            if ((tempMunkalapItem.MunkaDatuma.Year == feladatKezdesHonap.Year ||
                                 tempMunkalapItem.MunkaDatuma.Year == feladatBefejezesHonap.Year) &&
                                 tempMunkalapItem.MunkaDatuma.Day == i &&
                                (tempMunkalapItem.MunkaDatuma.Month == feladatKezdesHonap.Month ||
                                tempMunkalapItem.MunkaDatuma.Month == feladatBefejezesHonap.Month))
                            {
                                foreach (var kivalasztottUserItem in kivalasztottUsersLista)
                                {
                                    if (tempMunkalapItem.User.UserID == kivalasztottUserItem.UsersID)
                                    {

                                        if ((tempMunkalapItem.KIdo > feladatok.Kido && feladatBIdo > tempMunkalapItem.KIdo)
                                            || (tempMunkalapItem.KIdo < feladatok.Kido && tempMunkalapItem.BIdo > feladatok.Kido))
                                        {
                                            ///név + dátum + feladat ID hozzáadása
                                            TempData["ErrorMessage"] = $"A feladat nem létrehozható, mert " +
                                                    $"{tempMunkalapItem.User.VezetekNev} {tempMunkalapItem.User.KeresztNev} " +
                                                    $"rendelkezik feladattal (ID. {tempMunkalapItem.FeladatID}) " +
                                                    $"a {tempMunkalapItem.Datum.Year}.{tempMunkalapItem.Datum.Month}.{tempMunkalapItem.Datum.Day} dátumon!";


                                            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                                            var tempUser = felhasznaloAzonositas();
                                            if (tempUser == null)
                                            {
                                                return RedirectToAction("Bejelentkezes", "Users");
                                            }
                                            else if (tempUser.Jogosultsag.Nev != "ADMIN" && tempUser.Jogosultsag.Nev != "Vezetőség")
                                            {
                                                return RedirectToAction("Index", "Home");
                                            }

                                            return View(feladatok);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                #endregion


                if (ModelState.IsValid)
                {
                    ///Elmenti, hogy milyen pozicioban volt a JovahagyvaCB
                    ///meg a munkalapokat erinto adatok
                    bool tempJovahagyvaCB = model.JovahagyvaCB;

                    bool munkaoraModosult = false;
                    bool munkaDatumModosult = false;
                    bool vanKijeloltAlkalmazott = false;

                    ///megnezzuk, hogy az uj kezdes korabbi vagy kesobbi
                    ///mert ha IGEN, akkor a meglevo munkalapok egyszerubb torolni
                    ///es ujjat letrehozni, mint vizsgalni a meglevokben a modositasokat
                    if (model.BecsultMunkaOra != feladatok.BecsultMunkaOra
                        || $"{(model.Kido.ToString()).Substring(0, 2)}:{(model.Kido.ToString()).Substring(3, 2)}" != feladatok.KezdesiIdo)
                    {
                        munkaoraModosult = true;
                    }

                    if (model.VarhatoKezdes != feladatok.VarhatoKezdes || model.VarhatoBefejezes != feladatok.VarhatoBefejezes)
                    {
                        munkaDatumModosult = true;
                    }

                    //kezdesi ido kinyerese
                    if (feladatok.KezdesiIdo != null)
                    {
                        model.Kido = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);
                    }
                    else
                    {
                        feladatok.KezdesiIdo = "8";
                    }

                    model.Atiranyitott = feladatok.Atiranyitott;
                    model.KezdesiIdo = feladatok.KezdesiIdo;
                    model.BecsultMunkaOra = feladatok.BecsultMunkaOra;
                    model.FeladatFelelosUserID = feladatok.FeladatFelelosUserID;
                    model.FeladatLeirasa = feladatok.FeladatLeirasa;

                    model.FeladatTipusaID = feladatok.FeladatTipusaID;
                    model.MunkaLeirasa = feladatok.MunkaLeirasa;
                    model.Nev = feladatok.Nev;
                    model.ProjektID = feladatok.ProjektID;
                    model.VarhatoBefejezes = feladatok.VarhatoBefejezes;
                    model.VarhatoKezdes = feladatok.VarhatoKezdes;
                    model.JovahagyvaCB = feladatok.JovahagyvaCB;

                    if (model.JovahagyvaCB)
                    {
                        Viszony tempViszont = db.Viszony.FirstOrDefault(x => x.Nev == "Jóváhagyott");
                        model.ViszonyID = tempViszont.ID;
                    }
                    else
                    {
                        Viszony tempViszont = db.Viszony.FirstOrDefault(x => x.Nev == "Nem jóváhagyott");
                        model.ViszonyID = tempViszont.ID;
                    }


                    #region Eszkozok

                    model.ElvihetoEszkozokLista = db.ElvittEszkozok.ToList();

                    //Azok az ESZKOZOK, amik mar tarsitva vannak a feladathoz.... ha van ilyen

                    model.FeladatokElvittEszkozokIDjaLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == id).OrderBy(x => x.ElvittEszkozID).ToList();

                    //belső módosított változó létrehozása
                    model.KiirniEszkozokLista = feladatok.KiirniEszkozokLista;
                    //int i = 0;


                    foreach (var item in model.KiirniEszkozokLista)
                    {
                        Feladat_ElvittEszkoz_ID feladat_ElvittEszkoz_ID_Item = new Feladat_ElvittEszkoz_ID();

                        var xxxxx = db.Feladat_ElvittEszkoz_ID.FirstOrDefault(x => x.ID == item.ExEszkozFeladatID);

                        if ((item.ExEszkozFeladatID != 0 || item.EszkozModositva) && (item.EszkozKivalasztva != xxxxx.KiVoltJelolve))
                        {
                            xxxxx.ID = item.ExEszkozFeladatID;
                            xxxxx.ElvittEszkozID = item.EszkozID;
                            xxxxx.FeladatID = id;
                            xxxxx.KiVoltJelolve = item.EszkozKivalasztva;

                            //MunkaLeirasban lett modositva, ATHUZASSAL JELOLES
                            xxxxx.Modositott = true;

                            db.Feladat_ElvittEszkoz_ID.Attach(xxxxx);
                            db.Entry(xxxxx).State = System.Data.Entity.EntityState.Modified;

                            db.SaveChanges();
                        }
                        else if (item.EszkozKivalasztva && item.ExEszkozFeladatID == 0)
                        {

                            feladat_ElvittEszkoz_ID_Item.ElvittEszkozID = item.EszkozID;
                            feladat_ElvittEszkoz_ID_Item.FeladatID = id;
                            feladat_ElvittEszkoz_ID_Item.KiVoltJelolve = true;

                            //MunkaLeirasban lett modositva JELOLES
                            feladat_ElvittEszkoz_ID_Item.Modositott = false;

                            db.Feladat_ElvittEszkoz_ID.Add(feladat_ElvittEszkoz_ID_Item);
                            db.SaveChanges();
                        }

                    }


                    #endregion

                    #region USERS MAPING

                    //az osszes user listazasa
                    model.UsersLista = db.Users.OrderBy(x => x.VezetekNev).ToList();

                    //feladatban resztvevo FELHASZNALOK (ID) listaja 
                    model.UsersFeladatokLista = db.Feladat_User_ID.Where(x => x.FeladatID == id).OrderBy(x => x.UserID).ToList();


                    //belső módosított változó létrehozása
                    model.FeladatokUsersLista = feladatok.FeladatokUsersLista;

                    //Feladathoz rendelt MUNKALAPOK
                    model.MunkalapokListaja = db.Munkalapok.Where(x => x.FeladatID == id).ToList();

                    foreach (var item in model.FeladatokUsersLista)
                    {

                        Feladat_User_ID feladat_User_ID_Item = new Feladat_User_ID();

                        if (item.ExUsersFeladatokID != 0 && !item.UserKivalasztva)
                        {
                            //!!! HA TORLODIK A USER TORLODNEK A MUNKALAPJAI IS !!!!

                            feladat_User_ID_Item.ID = item.ExUsersFeladatokID;

                            var torlendo_Feladat_User_ID_Item = db.Feladat_User_ID.FirstOrDefault(x => x.ID == item.ExUsersFeladatokID);

                            feladat_User_ID_Item.UserID = item.UsersID;
                            feladat_User_ID_Item.FeladatID = id;

                            db.Feladat_User_ID.Remove(torlendo_Feladat_User_ID_Item);

                            //MUNKALAPOK

                            foreach (var munkalapItem in model.MunkalapokListaja)
                            {
                                if (munkalapItem.UserID == feladat_User_ID_Item.UserID && !munkalapItem.Megtekintve)
                                {
                                    db.Munkalapok.Remove(munkalapItem);
                                }
                                else if (munkalapItem.UserID == feladat_User_ID_Item.UserID)
                                {

                                    TempData["ErrorMessage"] = "A törlendő alkalmazott már módosította a MUNKALAPOT !";

                                    return View(feladatok);
                                }
                            }

                            db.SaveChanges();

                        }
                        else if (item.UserKivalasztva && item.ExUsersFeladatokID == 0)
                        {
                            //!!! HA HOZZADUNK USERT, AKKOR MUNKALAPOKAT IS ADUNK HOZZA !!!

                            feladat_User_ID_Item.UserID = item.UsersID;
                            feladat_User_ID_Item.FeladatID = id;

                            db.Feladat_User_ID.Add(feladat_User_ID_Item);
                            db.SaveChanges();

                            //MUNKALAPOK

                            //miutan letrehoztam a feladatot fontos, hogy letrehozzam melle a szukseges MUNKALAPOKAT, de csak akkor, ha van hozza rendelve alkalmazott!
                            if ((model.VarhatoKezdes != null || model.VarhatoBefejezes != null) && model.FeladatokUsersLista.Count() != 0)
                            {
                                vanKijeloltAlkalmazott = true;
                                //kinyerem a ket datum kozti kulonbseget, de az adott napot nem adja hozza igy az utolag kell potolni
                                TimeSpan NapOraPercKulonbseg = (TimeSpan)(model.VarhatoBefejezes - model.VarhatoKezdes);

                                //a kulonbsegbol megkapom, hogy hany naprol van szo, de startbol az adott napot hozza kell adni
                                int LetrehozandoMunkalapokSzama = (1 + NapOraPercKulonbseg.Days);

                                if (LetrehozandoMunkalapokSzama != 0)
                                {
                                    for (int i = 0; i < LetrehozandoMunkalapokSzama / 1; i++)
                                    {
                                        Munkalapok munkalap = new Munkalapok();

                                        //kotelezoen kitoltendok (not null)
                                        munkalap.KezdesiIdo = $"x";
                                        munkalap.BefejezesiIdo = $"y";
                                        munkalap.UserID = item.UsersID;
                                        munkalap.Datum = DateTime.Now;
                                        munkalap.JovahagyvaCB = false;
                                        munkalap.FeladatID = model.ID;
                                        munkalap.Megtekintve = false;

                                        //alap adatok az index lista letrehozazasahoz

                                        //a kulonbsegbol megkapom, hogy hany orat dolgozott
                                        if (model.BecsultMunkaOra > 4 && model.BecsultMunkaOra <= 10)
                                        {
                                            munkalap.MunkaOra = model.BecsultMunkaOra - Convert.ToDecimal(0.50);
                                        }
                                        else if (model.BecsultMunkaOra > 10)
                                        {
                                            munkalap.MunkaOra = model.BecsultMunkaOra - 1;
                                        }
                                        else
                                        {
                                            munkalap.MunkaOra = model.BecsultMunkaOra;
                                        }

                                        munkalap.MunkaDatuma = new DateTime((model.VarhatoKezdes.Value).Year,
                                                (model.VarhatoKezdes.Value).Month, (model.VarhatoKezdes.Value).Day + i);

                                        munkalap.KIdo = model.Kido; /*new TimeSpan(8, 00, 00);*/

                                        ///kezdesi ido atadasa a feladattol
                                        befejezesiIdo = int.Parse(TimeArray[0]) + (int)(model.BecsultMunkaOra);
                                        munkalap.BIdo = new TimeSpan((befejezesiIdo == 24) ? 00 : befejezesiIdo, int.Parse(TimeArray[1]), 00);

                                        db.Munkalapok.Add(munkalap);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        else if (item.UserKivalasztva)
                        {
                            vanKijeloltAlkalmazott = true;
                        }


                    }

                    #endregion

                    #region Cim és elérhetőség

                    model.Cimek.Orszag = feladatok.Cimek.Orszag;
                    model.Cimek.Varos = feladatok.Cimek.Varos;
                    model.Cimek.PostaKod = feladatok.Cimek.PostaKod;
                    model.Cimek.Utca = feladatok.Cimek.Utca;
                    model.Cimek.Szam = feladatok.Cimek.Szam;
                    model.Cimek.Egyeb = feladatok.Cimek.Egyeb;
                    model.Cimek.Megjegyzes = feladatok.Cimek.Megjegyzes;

                    model.Elerhetosegek.Telszam1 = feladatok.Elerhetosegek.Telszam1;
                    model.Elerhetosegek.Telszam2 = feladatok.Elerhetosegek.Telszam2;
                    model.Elerhetosegek.Email = feladatok.Elerhetosegek.Email;
                    model.Elerhetosegek.WEB = feladatok.Elerhetosegek.WEB;
                    model.Elerhetosegek.Megjegyzes = feladatok.Elerhetosegek.Megjegyzes;

                    #endregion

                    #region MUNKALAPOK JovahagyvaCB

                    //Az alapjan alltja be a munkalapok jovahagyasat, mikent a feladat is jova van/nincs hagyva

                    if (model.JovahagyvaCB != tempJovahagyvaCB)
                    {
                        foreach (var itemMunkalap in model.MunkalapokListaja)
                        {
                            Munkalapok munkalapModel = db.Munkalapok.FirstOrDefault(x => x.ID == itemMunkalap.ID);

                            if (munkalapModel != null)
                            {
                                munkalapModel.JovahagyvaCB = model.JovahagyvaCB;

                                itemMunkalap.KezdesiIdo = itemMunkalap.KIdo.ToString();
                                itemMunkalap.BefejezesiIdo = itemMunkalap.BIdo.ToString();

                                db.Munkalapok.Attach(munkalapModel);
                                db.Entry(munkalapModel).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region Ha MODOSUL A MUNKA DATUMA ES IDEJE

                    if (vanKijeloltAlkalmazott && (munkaDatumModosult || munkaoraModosult))
                    {
                        ///ha szukseges torlni vagy letrehozni feladat_User elemet akkor jol jon
                        Feladat_User_ID feladat_User_ID_Item = new Feladat_User_ID();


                        model.FeladatokUsersLista = feladatok.FeladatokUsersLista.Where(x => x.UserKivalasztva).ToList();

                        //inkabb toroljuk, mert sok vele a macera :(
                        var torlendoMunkalapok = db.Munkalapok.Where(x => x.FeladatID == model.ID).Where(x => x.Megtekintve == false).ToList();

                        foreach (var torlendoMunkalapITEM in torlendoMunkalapok)
                        {
                            ///ha a feltetel parametereinek nem felel meg a munkalap, akkor torlesre kerul
                            feladat_User_ID_Item = db.Feladat_User_ID.FirstOrDefault(x => x.FeladatID == id && x.UserID == torlendoMunkalapITEM.UserID);
                            if (feladat_User_ID_Item != null)
                            {
                                db.Feladat_User_ID.Remove(feladat_User_ID_Item);
                            }

                            db.Munkalapok.Remove(torlendoMunkalapITEM);

                            db.SaveChanges();
                        }

                        ///ha a megmaradt munkalapok szama kevesebb, mint a letrehozando munkalapok szama
                        ///akkor letrehozzuk a megfelelo mennyisegu munkalapot
                        ///amikor kevesebb mint 3 nap van akkor +1 letrehozando van

                        int LetrehozandoMunkalapokCount = 0;

                        if (model.FeladatokUsersLista.Count() >= 2)
                        {
                            LetrehozandoMunkalapokCount = ((((TimeSpan)(model.VarhatoBefejezes - model.VarhatoKezdes)).Days + 1 /*ez a 0. napot mutatja -> AZNAP*/)
                                * model.FeladatokUsersLista.Count());
                        }
                        else
                        {
                            LetrehozandoMunkalapokCount = (1 + ((TimeSpan)(model.VarhatoBefejezes - model.VarhatoKezdes)).Days * model.FeladatokUsersLista.Count());
                        }

                        if (LetrehozandoMunkalapokCount /*- kitoroltMunkalapokSzama*/ > 0)
                        {
                            //!!! HA MODOSITOTTUNK USERT, AKKOR FeladatUsert is ADUNK HOZZA !!!
                            List<Feladat_User_ID> temp_Feladat_User_ID_LISTA = new List<Feladat_User_ID>();
                            feladat_User_ID_Item = new Feladat_User_ID();

                            foreach (var item in model.FeladatokUsersLista)
                            {
                                if (temp_Feladat_User_ID_LISTA.Count() != 0)
                                {
                                    foreach (var meglevo_feladat_user_ID_item in temp_Feladat_User_ID_LISTA)
                                    {
                                        if (item.ExUsersFeladatokID != meglevo_feladat_user_ID_item.ID)
                                        {

                                            feladat_User_ID_Item.FeladatID = id;
                                            feladat_User_ID_Item.UserID = item.UsersID;

                                            db.Feladat_User_ID.Add(feladat_User_ID_Item);
                                        }
                                    }
                                }
                                else
                                {
                                    feladat_User_ID_Item.FeladatID = id;
                                    feladat_User_ID_Item.UserID = item.UsersID;

                                    db.Feladat_User_ID.Add(feladat_User_ID_Item);
                                    db.SaveChanges();
                                }

                                //Feladathoz rendelt MUNKALAPOK
                                //MUNKALAPOK
                                //miutan letrehoztam a feladatot fontos, hogy letrehozzam melle a szukseges MUNKALAPOKAT, de csak akkor, ha van hozza rendelve alkalmazott!
                                if ((model.VarhatoKezdes != null || model.VarhatoBefejezes != null) && model.FeladatokUsersLista.Count() != 0)
                                {
                                    ///kinyerem a ket datum kozti kulonbseget, de az adott napot nem adja hozza igy az utolag kell potolni
                                    ///a kulonbsegbol megkapom, hogy hany naprol van szo, de startbol az adott napot hozza kell adni
                                    byte letrehozandoMunkalapSzam = (byte)(LetrehozandoMunkalapokCount / model.FeladatokUsersLista.Count());

                                    if (letrehozandoMunkalapSzam != 0)
                                    {
                                        for (int i = 0; i < letrehozandoMunkalapSzam; i++)
                                        {
                                            Munkalapok munkalap = new Munkalapok();

                                            //kotelezoen kitoltendok (not null)
                                            munkalap.KezdesiIdo = $"x";
                                            munkalap.BefejezesiIdo = $"y";
                                            munkalap.UserID = item.UsersID;
                                            munkalap.Datum = DateTime.Now;
                                            munkalap.JovahagyvaCB = false;
                                            munkalap.FeladatID = model.ID;
                                            munkalap.Megtekintve = false;

                                            //alap adatok az index lista letrehozazasahoz
                                            //a kulonbsegbol megkapom, hogy hany orat dolgozott
                                            if (model.BecsultMunkaOra > 4 && model.BecsultMunkaOra <= 10)
                                            {
                                                munkalap.MunkaOra = model.BecsultMunkaOra - Convert.ToDecimal(0.50);
                                            }
                                            else if (model.BecsultMunkaOra > 10)
                                            {
                                                munkalap.MunkaOra = model.BecsultMunkaOra - 1;
                                            }
                                            else
                                            {
                                                munkalap.MunkaOra = model.BecsultMunkaOra;
                                            }

                                            //a letrehozott munkalapokba is keruljon bele a szunet
                                            munkalap.MunkaDatuma = new DateTime((model.VarhatoKezdes.Value).Year,
                                                    (model.VarhatoKezdes.Value).Month, (model.VarhatoKezdes.Value).Day + i);

                                            munkalap.KIdo = model.Kido;

                                            ///kezdesi ido atadasa a feladattol
                                            befejezesiIdo = int.Parse(TimeArray[0]) + (int)(model.BecsultMunkaOra);
                                            munkalap.BIdo = new TimeSpan((befejezesiIdo == 24) ? 00 : befejezesiIdo, int.Parse(TimeArray[1]), 00);

                                            db.Munkalapok.Add(munkalap);
                                        }
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    #endregion

                    db.Feladatok.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Feladat", new { @id = id });

                }

                return View(feladatok);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(feladatok);
            }
        }

        #endregion

        #region EDIT MUNKA LEIRAS

        public ActionResult EditML(string id)
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                //ProjektID es FeladatID athozatala es kinyerese 
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int FeladatID = int.Parse(IDArray.First());

                var model = db.Feladatok.FirstOrDefault(x => x.ID == FeladatID);

                //Elviheto ESZKOZOK listaja
                model.ElvihetoEszkozokLista = db.ElvittEszkozok.OrderBy(x => x.Nev).ToList();

                //Azok az ESZKOZOK, amik mar tarsitva vannak a feladathoz.... ha van ilyen

                model.FeladatokElvittEszkozokIDjaLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == FeladatID).OrderBy(x => x.ElvittEszkozID).ToList();

                //belső módosított változó létrehozása
                model.KiirniEszkozokLista = new List<ElvittEszkozokFeladatokOsszefugges>();
                int i = 0;

                foreach (var item in model.ElvihetoEszkozokLista)
                {

                    //ha ki volt valasztva, vagy MODOSITOTT es vmikor ki volt valasztva akkor...
                    if (i < model.FeladatokElvittEszkozokIDjaLista.Count() && item.ID == model.FeladatokElvittEszkozokIDjaLista[i].ElvittEszkozID)
                    {
                        //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                        ElvittEszkozokFeladatokOsszefugges elvittEszkozokFeladatokOsszefuggesItem = new ElvittEszkozokFeladatokOsszefugges();

                        elvittEszkozokFeladatokOsszefuggesItem.EszkozID = item.ID;
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozNeve = item.Nev;
                        elvittEszkozokFeladatokOsszefuggesItem.ExEszkozFeladatID = model.FeladatokElvittEszkozokIDjaLista[i].ID;

                        //a kiirt nev athuzasanak az erdekeben kell
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozKivalasztva = true;
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozModositva = (bool)(model.FeladatokElvittEszkozokIDjaLista[i].Modositott);

                        model.KiirniEszkozokLista.Add(elvittEszkozokFeladatokOsszefuggesItem);

                        i++;
                    }
                    //ha ki NEM volt valasztva, akkor...
                    else
                    {
                        //azert kell mindig ujjat letrehozni, mert maskulonben mindig a legutolsora mutat a pointer
                        ElvittEszkozokFeladatokOsszefugges elvittEszkozokFeladatokOsszefuggesItem = new ElvittEszkozokFeladatokOsszefugges();

                        elvittEszkozokFeladatokOsszefuggesItem.EszkozID = item.ID;
                        elvittEszkozokFeladatokOsszefuggesItem.EszkozNeve = item.Nev;

                        elvittEszkozokFeladatokOsszefuggesItem.EszkozKivalasztva = false;

                        model.KiirniEszkozokLista.Add(elvittEszkozokFeladatokOsszefuggesItem);
                    }

                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult EditML(int id, Feladatok feladatok)
        {
            try
            {

                var model = db.Feladatok.FirstOrDefault(x => x.ID == id);

                ///hozzaadando, hog ne legyen Exception
                model.KezdesiIdo = "8";

                //ESZKOZOK nevenek ujrateltoltese
                foreach (var nevvelFeltoltendoItem in feladatok.KiirniEszkozokLista)
                {
                    nevvelFeltoltendoItem.EszkozNeve = $"{db.ElvittEszkozok.FirstOrDefault(x => x.ID == nevvelFeltoltendoItem.EszkozID).Nev}";
                }

                //A LENYEG
                model.MunkaLeirasa = feladatok.MunkaLeirasa;

                //Eszkozok modositasa
                #region // Eszkozok

                model.ElvihetoEszkozokLista = db.ElvittEszkozok.ToList();

                //Azok az ESZKOZOK, amik mar tarsitva vannak a feladathoz.... ha van ilyen

                model.FeladatokElvittEszkozokIDjaLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == id).OrderBy(x => x.ElvittEszkozID).ToList();

                //belső módosított változó létrehozása
                model.KiirniEszkozokLista = feladatok.KiirniEszkozokLista;

                foreach (var item in model.KiirniEszkozokLista)
                {
                    Feladat_ElvittEszkoz_ID feladat_ElvittEszkoz_ID_Item = new Feladat_ElvittEszkoz_ID();

                    var xxxxx = db.Feladat_ElvittEszkoz_ID.FirstOrDefault(x => x.ID == item.ExEszkozFeladatID);

                    if ((item.ExEszkozFeladatID != 0 || item.EszkozModositva) && (item.EszkozKivalasztva != xxxxx.KiVoltJelolve))
                    {

                        xxxxx.ID = item.ExEszkozFeladatID;
                        xxxxx.ElvittEszkozID = item.EszkozID;
                        xxxxx.FeladatID = id;
                        xxxxx.KiVoltJelolve = item.EszkozKivalasztva;

                        //MunkaLeirasban lett modositva, ATHUZASSAL JELOLES
                        xxxxx.Modositott = true;

                        db.Feladat_ElvittEszkoz_ID.Attach(xxxxx);
                        db.Entry(xxxxx).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                    }
                    else if (item.EszkozKivalasztva && item.ExEszkozFeladatID == 0)
                    {

                        feladat_ElvittEszkoz_ID_Item.ElvittEszkozID = item.EszkozID;
                        feladat_ElvittEszkoz_ID_Item.FeladatID = id;
                        feladat_ElvittEszkoz_ID_Item.KiVoltJelolve = true;

                        //MunkaLeirasban lett modositva JELOLES
                        feladat_ElvittEszkoz_ID_Item.Modositott = true;

                        db.Feladat_ElvittEszkoz_ID.Add(feladat_ElvittEszkoz_ID_Item);
                        db.SaveChanges();
                    }
                }

                #endregion

                db.Feladatok.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "Feladat", new { @id = id });

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(feladatok);
            }
        }

        #endregion

        #region // GET:Delete

        public ActionResult Delete(int id)
        {
            try
            {
                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();

                var model = db.Feladatok.FirstOrDefault(x => x.ID == id);
                int feladatTipusID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }
                else if (tempUser.Jogosultsag.Nev != "ADMIN"
                    && tempUser.Jogosultsag.Nev != "Vezetőség"
                    && model.FeladatTipusaID != feladatTipusID)
                {
                    return RedirectToAction("Index", "Home");
                }

                //listak lekerdezese
                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ProjektekLista = db.Projektek.OrderBy(x => x.AzonositoKod).ToList();
                model.MunkalapokListaja = db.Munkalapok.Where(x => x.FeladatID == id).OrderByDescending(x => x.MunkaDatuma).ToList();

                //userek feltoltese a feladatokba a chack box vegett

                model.UsersLista = db.Users.OrderBy(x => x.VezetekNev).ToList();

                //ezzel vizsgaljuk hogy van e olyan USER, aki reszt vesz ebben a feladatban, es fontos, hogy csak egyszer forduljon elo

                model.UsersFeladatokLista = db.Feladat_User_ID.Where(x => x.FeladatID == id).ToList();
                model.UsersFeladatokLista = model.UsersFeladatokLista.Distinct().ToList();

                //ESZKOZOK feltoltese a feladatokba a chack box vegett

                model.ElvihetoEszkozokLista = new List<ElvittEszkozok>();

                //ezzel vizsgaljuk hogy van e olyan USER, aki reszt vesz ebben a feladatban, es fontos, hogy csak egyszer forduljon elo

                List<Feladat_ElvittEszkoz_ID> feladatEszkozokIdLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == id).ToList();

                foreach (var item in feladatEszkozokIdLista)
                {
                    model.ElvihetoEszkozokLista.Add(db.ElvittEszkozok.Where(x => x.ID == item.ElvittEszkozID).FirstOrDefault());
                }

                //FELHASZNALT ANYAGOK lekerdezese

                StringBuilder hibaUzenet = new StringBuilder();

                foreach (var item in model.MunkalapokListaja)
                {
                    ///Ellenorizzuk, hogy a munkalaphoz van e rendelve KOLTSEG
                    ///mert ha igen, akkor FIGYELMEZTETEST kell adni rola

                    item.KoltsegekLista = db.Koltsegek.Where(x => x.MunkalapID == item.ID).ToList();

                    //eloszor a felhasznalora vizsgal ra, hogy ha alkalmazott, akkor
                    if (tempUser.Jogosultsag.Nev != "ADMIN" && tempUser.Jogosultsag.Nev != "Vezetőség")
                    {
                        if (model.JovahagyvaCB == true)
                        {
                            TempData["ErrorMessage"] = "Nem törölhető, mert a feladat már jóvá lett hagyva!";
                            return RedirectToAction("Details", "Feladat", new { @id = model.ID });
                        }
                    }
                    //ha vezetosegi tag, vagy ADMIN akkor
                    else if (item.Megtekintve && tempUser.Jogosultsag.Nev != "Alkalmazott")
                    {
                        TempData["ErrorMessage"] = "Nem törölhető, mert bizonyos munkalap már meg lett tekintve!";
                        return RedirectToAction("Index", "Feladat", new { @id = model.ID });
                    }
                    else if (item.KoltsegekLista.Count != 0)
                    {
                        if (hibaUzenet.Length == 0)
                        {
                            hibaUzenet.Append($"{item.MunkaDatuma.Year}.{item.MunkaDatuma.Month}.{item.MunkaDatuma.Day} / {item.User.VezetekNev} {item.User.KeresztNev}");
                        }
                        else
                        {
                            hibaUzenet.Append($"; {item.MunkaDatuma.Year}.{item.MunkaDatuma.Month}.{item.MunkaDatuma.Day} / {item.User.VezetekNev} {item.User.KeresztNev}");
                        }
                    }
                }

                if (hibaUzenet.Length != 0)
                {
                    TempData["ErrorMessage"] = $"{hibaUzenet} munkalapján van költség!";
                }

                if (model.JovahagyvaCB == true)
                {

                    TempData["ErrorMessage"] = "Nem törölhető, mert a feladat már jóvá lett hagyva!";
                    return RedirectToAction("Index"/*, new { @id = model.ID }*/);
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region // POST: Delete

        [HttpPost]
        public ActionResult Delete(int id, Feladatok feladatok)
        {
            try
            {
                var tempUser = db.Users.Where(x => x.Elerhetosegek.Email == User.Identity.Name).FirstOrDefault();

                var model = db.Feladatok.FirstOrDefault(x => x.ID == id);

                int feladatTempID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                //MUNKALAP - munkalappal valo kapcsolat

                List<Munkalapok> feladat_munkalapok_Lista = db.Munkalapok.Where(x => x.FeladatID == id).ToList();

                foreach (var itemMunkalap in feladat_munkalapok_Lista)
                {
                    var munkalapItem = db.Munkalapok.FirstOrDefault(x => x.ID == itemMunkalap.ID);

                    munkalapItem.KoltsegekLista = db.Koltsegek.Where(x => x.MunkalapID == munkalapItem.ID).ToList();

                    foreach (var koltsegItem in munkalapItem.KoltsegekLista)
                    {
                        db.Koltsegek.Remove(koltsegItem);
                    }

                    db.Munkalapok.Remove(munkalapItem);
                }

                //FELADAT - Felhasznalo kapcsolat
                var feladat_User_ID_Lista = db.Feladat_User_ID.Where(x => x.FeladatID == id).ToList();

                foreach (var item in feladat_User_ID_Lista)
                {
                    var feladat_User_ID_Item = db.Feladat_User_ID.FirstOrDefault(x => x.ID == item.ID);

                    db.Feladat_User_ID.Remove(feladat_User_ID_Item);
                }

                //FELADAT - ELVITT Eszkoz(ok) kapcsolat
                var feladat_ElvittEszkoz_ID_Lista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == id).ToList();

                foreach (var item in feladat_ElvittEszkoz_ID_Lista)
                {
                    var feladat_ElvittEszkoz_ID_Item = db.Feladat_ElvittEszkoz_ID.FirstOrDefault(x => x.ID == item.ID);

                    db.Feladat_ElvittEszkoz_ID.Remove(feladat_ElvittEszkoz_ID_Item);
                }

                //FELADAT - Felhasznalt Anyag(ok) kapcsolat
                var feladat_FelhaszAnyag_ID_Lista = db.Feladat_FelhaszAnyag_ID.Where(x => x.FeladatID == id).ToList();

                foreach (var item in feladat_FelhaszAnyag_ID_Lista)
                {
                    var feladat_FelhaszAnyag_ID_Item = db.Feladat_FelhaszAnyag_ID.FirstOrDefault(x => x.ID == item.ID);
                    db.Feladat_FelhaszAnyag_ID.Remove(feladat_FelhaszAnyag_ID_Item);

                    var felhasznaltAnyag_Item = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == item.FelhasznaltAnyagID);
                    db.FelhasznaltAnyagok.Remove(felhasznaltAnyag_Item);
                }

                db.Feladatok.Remove(model);
                db.SaveChanges();

                if (model.FeladatTipusaID != feladatTempID)
                {
                    return RedirectToAction("Index", "Feladat");
                }
                else
                {
                    return RedirectToAction("TempIndex", "Feladat");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"{e}" /*"Valami hiba történt, kérem ellenőrizze az adatokat!"*/;
                return RedirectToAction("Details", "Feladat", new { id = id });
            }
        }

        #endregion

        #region // GET:TempDelete
        public ActionResult TempDelete(int id)
        {
            try
            {
                //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                var tempUser = felhasznaloAzonositas();

                var model = db.Feladatok.FirstOrDefault(x => x.ID == id);
                int feladatTipusID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }
                else if (tempUser.Jogosultsag.Nev != "ADMIN"
                    && model.FeladatTipusaID != feladatTipusID)
                {
                    return RedirectToAction("Index", "Home");
                }

                //listak lekerdezese
                model.FeladatTipusokLista = db.FeladatTipusok.ToList();
                model.ProjektekLista = db.Projektek.OrderBy(x => x.AzonositoKod).ToList();
                model.MunkalapokListaja = db.Munkalapok.Where(x => x.FeladatID == id).OrderByDescending(x => x.MunkaDatuma).ToList();

                //userek feltoltese a feladatokba a chack box vegett

                //ezzel vizsgaljuk hogy van e olyan USER, aki reszt vesz ebben a feladatban, es fontos, hogy csak egyszer forduljon elo

                model.UsersFeladatokLista = db.Feladat_User_ID.Where(x => x.FeladatID == id).ToList();
                model.UsersFeladatokLista = model.UsersFeladatokLista.Distinct().ToList();

                //ESZKOZOK feltoltese a feladatokba a chack box vegett

                model.ElvihetoEszkozokLista = new List<ElvittEszkozok>();

                //ezzel vizsgaljuk hogy van e olyan USER, aki reszt vesz ebben a feladatban, es fontos, hogy csak egyszer forduljon elo

                List<Feladat_ElvittEszkoz_ID> feladatEszkozokIdLista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == id).ToList();

                foreach (var item in feladatEszkozokIdLista)
                {
                    model.ElvihetoEszkozokLista.Add(db.ElvittEszkozok.Where(x => x.ID == item.ElvittEszkozID).FirstOrDefault());
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region // POST: TempDelete
        [HttpPost]
        public ActionResult TempDelete(int id, Feladatok feladatok)
        {
            try
            {
                var tempUser = db.Users.Where(x => x.Elerhetosegek.Email == User.Identity.Name).FirstOrDefault();

                var model = db.Feladatok.FirstOrDefault(x => x.ID == id);

                int feladatTempID = db.FeladatTipusok.FirstOrDefault(x => x.Nev.ToUpper() == "IDEIGLENES").ID;

                //FELADAT - ELVITT Eszkoz(ok) kapcsolat
                var feladat_ElvittEszkoz_ID_Lista = db.Feladat_ElvittEszkoz_ID.Where(x => x.FeladatID == id).ToList();

                foreach (var item in feladat_ElvittEszkoz_ID_Lista)
                {
                    var feladat_ElvittEszkoz_ID_Item = db.Feladat_ElvittEszkoz_ID.FirstOrDefault(x => x.ID == item.ID);

                    db.Feladat_ElvittEszkoz_ID.Remove(feladat_ElvittEszkoz_ID_Item);
                }

                db.Feladatok.Remove(model);
                db.SaveChanges();

                return RedirectToAction("TempIndex", "Feladat");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"{e}" /*"Valami hiba történt, kérem ellenőrizze az adatokat!"*/;
                return RedirectToAction("Details", "Feladat", new { id = id });
            }
        }

        #endregion

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

            string[] separator = { "_" };

            string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            int userID = int.Parse(IDArray.First());

            int mounth = int.Parse(IDArray.Last());
            int year = 2020;

            List<Munkalapok> model = new List<Munkalapok>();
            List<Munkalapok> tempModel = new List<Munkalapok>();
            List<Munkalapok> tempmunkalapokExcelLista = new List<Munkalapok>();


            ///A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            ///DE ERDEMESEBB LENNE NEM EMBEREKRE LEBONTANI, MERT AKKOR A MUNKATERVEZOHOZ
            ///HASONLOAN LEHETNE MEGCSINALNI, DE EGYELORE ez STORNO!!!

            tempmunkalapokExcelLista = db.Munkalapok.Where(x => x.MunkaDatuma.Month == mounth && x.MunkaDatuma.Year == year && x.UserID == userID).ToList();

            foreach (var item in tempmunkalapokExcelLista)
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

            //osszegek szmaitasa: munkaora total, koltsegek total, KM total
            decimal MunkaoraTotal = 0;
            int KMTotal = 0;
            int KoltsegTotal = 0;

            ///ide mar lehet egybol tenni  a MODELT
            /*tempModel*/
            model = tempmunkalapokExcelLista.Select(x => new Munkalapok
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

            model = model.OrderBy(x => x.userNev).ToList();

            ///idorendbe helyezes
            Munkalapok prevItem = new Munkalapok();

            for (int i = 0; i < model.Count(); i++)
            {
                if (i < model.Count() - 1)
                {
                    if (model[i].UserID == model[i + 1].UserID)
                    {
                        if (model[i].MunkaDatuma > model[i + 1].MunkaDatuma)
                        {
                            prevItem = model[i];
                            model[i] = model[i + 1];
                            model[i + 1] = prevItem;
                        }
                    }
                }
            }


            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
            string[] ABC = new string[] {"C","D","E","F","G","H","I","J","K",
                    "L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                    "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH"
                }; //31 nap van

            ws.Cells["A3"].Value = "Nev";
            ws.Cells["B1"].Value = "Munka dátuma";
            ws.Cells["B3"].Value = "Projekt kódja";
            ws.Cells["B4"].Value = "Feladat neve";
            ws.Cells["B5"].Value = "Munka óra";
            ws.Cells["B6"].Value = "Költségek";
            ws.Cells["B7"].Value = "Megtett KM";


            int columnStart = 0; // az A oszlopot mi adjuk hozza, a tobbi pedig a C-vel kezdodik
            int rowStart = 3; //a 3. sorba irja irja csak a nevet
            var tempDate = model[0].MunkaDatuma;
            var tempUserID = model[0].UserID;
            int MM = 0;

            //fejlec datumanak felepitese
            if (tempDate.Month == 1 || tempDate.Month == 3 || tempDate.Month == 5 || tempDate.Month == 7 ||
                tempDate.Month == 8 || tempDate.Month == 10 || tempDate.Month == 12)
            {
                MM = 31;
                for (int i = 0; i < MM; i++)
                {
                    ws.Cells[string.Format($"{ABC[i]}1")].Value = $"{tempDate.Year}.{tempDate.Month}.{i + 1}";
                }
            }
            else if (tempDate.Month == 2)
            {
                MM = 29;
                for (int i = 0; i < MM; i++)
                {
                    ws.Cells[string.Format($"{ABC[i]}1")].Value = $"{tempDate.Year}.{tempDate.Month}.{i + 1}";
                }
            }
            else
            {
                MM = 30;
                for (int i = 0; i < MM; i++)
                {
                    ws.Cells[string.Format($"{ABC[i]}1")].Value = $"{tempDate.Year}.{tempDate.Month}.{i + 1}";
                }
            }

            ws.Cells[string.Format($"A{rowStart}")].Value = model[0].userNev;


            foreach (var item in model)
            {
                if (tempUserID == item.UserID /*&& tempDate < item.MunkaDatuma*/)
                {
                    //megkeressuk, hogy hanyadik bekezdesben van az o datuma
                    ///erre azert van szukseg, hogy ha letezik mar arra a napra feladat, akkor ezt is adja hozza.
                    ///
                    if (prevItem.MunkaDatuma != item.MunkaDatuma)
                    {

                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.projNev;
                        rowStart++;
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.feladatNev;
                        rowStart++;
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.MunkaOra;
                        MunkaoraTotal += (decimal)item.MunkaOra;
                        rowStart++;
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.koltsegItem;
                        KoltsegTotal += item.koltsegItem;
                        rowStart++;
                        //az EXCAPTION kizarasa vegett
                        if (item.MegtettKM == null)
                        {
                            item.MegtettKM = 0;
                        }
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.MegtettKM;
                        KMTotal += (int)item.MegtettKM;

                        rowStart -= 4;
                    }
                    else ///hozza adjuk az elozo mezo erteket
                    {
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                            ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value + " + " + item.projNev;
                        rowStart++;
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                            ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value + " + " + item.feladatNev;
                        rowStart++;
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                            $"= {(ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value)} + {item.MunkaOra}";
                        MunkaoraTotal += (decimal)item.MunkaOra;
                        rowStart++;
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                            $"= {(ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value)} + {item.koltsegItem}";
                        KoltsegTotal += item.koltsegItem;
                        rowStart++;
                        //az EXCAPTION kizarasa vegett
                        if (item.MegtettKM == null)
                        {
                            item.MegtettKM = 0;
                        }
                        ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                            $"= {(int)(ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value)} + {item.MegtettKM}";
                        KMTotal += (int)item.MegtettKM;

                        rowStart -= 4;
                    }

                    prevItem = item;

                }
                else if (tempUserID != item.UserID)
                {
                    rowStart += 5;

                    ///mivel mas a USER ezert uj sorba irjuk, es modositjuk az eddigi ID-t a mostanira
                    ws.Cells[$"A{rowStart}"].Value = "Nev";
                    ws.Cells[string.Format($"A{rowStart}")].Value = item.userNev;

                    ws.Cells[$"B{rowStart}"].Value = "Projekt kódja";
                    ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.projNev;
                    rowStart++;

                    ws.Cells[$"B{rowStart}"].Value = "Feladat neve";
                    ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.feladatNev;
                    rowStart++;

                    ws.Cells[$"B{rowStart}"].Value = "Munka óra";
                    ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.MunkaOra;
                    MunkaoraTotal += (decimal)item.MunkaOra;
                    rowStart++;

                    ws.Cells[$"B{rowStart}"].Value = "Költségek";
                    ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.koltsegItem;
                    KoltsegTotal += item.koltsegItem;
                    rowStart++;

                    //az EXCAPTION kizarasa vegett
                    if (item.MegtettKM == null)
                    {
                        item.MegtettKM = 0;
                    }

                    ws.Cells[$"B{rowStart}"].Value = "Megtett KM";
                    ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value = item.MegtettKM;
                    KMTotal += (int)item.MegtettKM;

                    tempUserID = item.UserID;
                    rowStart -= 4;

                    prevItem = item;
                }
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.Flush();
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        #endregion
    }
}
