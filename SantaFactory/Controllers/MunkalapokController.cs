using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OfficeOpenXml;
using PagedList;
using SantaFactory.Models;
using SantaFactory.Models.Munkalapok_;

namespace SantaFactory.Controllers
{
    public class MunkalapokController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        public class testData
        {
            public int ID { get; set; }
            public string Nev { get; set; }
        }

        // GET: MunkalapIndex
        public ActionResult Index(string test, int? pn)
        {
            try
            {
                ///DB direkt meghívása
                //List<testData> tesztMilan = db.Database.SqlQuery<testData>("Select").ToList();

                if (test != null && test != "")
                {
                    ExportToExcel(test);
                }
                else if (test != null)
                {
                    TempData["ErrorMessage"] = "Nem volt kiválasztva év, vagy hónap!";
                }
                //itt nyerem ki, hogy kinek kellenek a munkalapjai
                Users tempUser = felhasznaloAzonositas();

                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }

                List<Munkalapok> model = new List<Munkalapok>();

                List<Users> MunkasokLista = db.Users.OrderBy(x => x.VezetekNev).ToList();
                foreach (var item in MunkasokLista)
                {
                    item.Nev = $"{item.VezetekNev} {item.KeresztNev}";
                }

                //Amig nem tudni, hogy mik az elvarasok, kiirja az osszeset
                //utana majd szurjon ra a megfelelo modon
                IQueryable<Munkalapok> tempModel;
                List<int> tempMunkalapokEvek = new List<int>();

                if (tempUser.Jogosultsag.Nev != "Alkalmazott")
                {
                    tempModel = db.Munkalapok;
                }
                else
                {
                    tempModel = db.Munkalapok.Where(x => x.UserID == tempUser.UserID);
                }

                if (!tempModel.Any())
                {
                    TempData["ErrorMessage"] = "Nincs kilistázható munkalap!";
                    return RedirectToAction("Index", "Home");
                }

                model = tempModel.Where(x => x.JovahagyvaCB/*Megtekintve*/).OrderByDescending(x => x.MunkaDatuma).ToList();

                if (!model.Any())
                {
                    TempData["ErrorMessage"] = "Nincs kilistázható munkalap!";
                    return RedirectToAction("Index", "Home");
                }

                ///levalogatjuk a munkalapokon szereplo eveket
                ///kezdjuk az elso munkalapnak az evevel, 
                ///majd a legutolsonak az eveig feltoltjuk
                ///es kikuszoboljuk az EXCEPTION-t, ha nincs munkalap

                for (int i = model.LastOrDefault().MunkaDatuma.Year; i <= model.FirstOrDefault().MunkaDatuma.Year; i++)
                {
                    tempMunkalapokEvek.Add(i);
                }

                ///ez helyettesti az atiranyitott opciot
                ///es itt toltjuk fel a honapok DropD. listat

                foreach (var item in model)
                {
                    item.MunkasokLista = MunkasokLista;
                    item.honapok = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    item.evek = tempMunkalapokEvek;

                    item.Atiranyitott = 0;

                    item.KezdesiIdo = item.KIdo.ToString();
                    item.BefejezesiIdo = item.BIdo.ToString();
                }

                return View(model.ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #region IndexA

        // GET: MunkalapIndex
        public ActionResult IndexA(string test, int? pn)
        {
            try
            {
                if (test != null && test != "")
                {
                    ExportToExcel(test);
                }
                else if (test != null)
                {
                    TempData["ErrorMessage"] = "Nem volt kiválasztva év, vagy hónap!";
                }
                //itt nyerem ki, hogy kinek kellenek a munkalapjai
                Users tempUser = felhasznaloAzonositas();

                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }

                List<MunkalapokA> model = new List<MunkalapokA>();

                List<Users> MunkasokLista = db.Users.OrderBy(x => x.VezetekNev).ToList();
                for (int i = 0; i < MunkasokLista.Count(); i++)
                {
                    MunkasokLista[i].Nev = $"{MunkasokLista[i].VezetekNev} {MunkasokLista[i].KeresztNev}";
                }

                //Amig nem tudni, hogy mik az elvarasok, kiirja az osszeset
                //utana majd szurjon ra a megfelelo modon
                IQueryable<MunkalapokA> tempModel;
                List<int> tempMunkalapokEvek = new List<int>();

                if (tempUser.Jogosultsag.Nev != "Alkalmazott")
                {
                    tempModel = db.MunkalapokA;
                }
                else
                {
                    tempModel = db.MunkalapokA.Where(x => x.UserID == tempUser.UserID);
                }

                if (tempModel.Count() == 0)
                {
                    TempData["ErrorMessage"] = "Nincs kilistázható munkalap!";
                    return RedirectToAction("Index", "Home");
                }

                model = tempModel.Where(x => x.JovahagyvaCB).OrderByDescending(x => x.MunkaDatuma).ToList();

                ///levalogatjuk a munkalapokon szereplo eveket
                ///kezdjuk az elso munkalapnak az evevel, 
                ///majd a legutolsonak az eveig feltoltjuk
                ///es kikuszoboljuk az EXCEPTION-t, ha nincs munkalap

                for (int i = model.LastOrDefault().MunkaDatuma.Year; i <= model.FirstOrDefault().MunkaDatuma.Year; i++)
                {
                    tempMunkalapokEvek.Add(i);
                }

                ///ez helyettesti az atiranyitott opciot
                ///es itt toltjuk fel a honapok DropD. listat

                foreach (var item in model)
                {
                    item.MunkasokLista = MunkasokLista;
                    item.honapok = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    item.evek = tempMunkalapokEvek;

                    item.Atiranyitott = 0;

                    item.KezdesiIdo = item.KIdo.ToString();
                    item.BefejezesiIdo = item.BIdo.ToString();
                }

                return View(model.ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion


        // GET: Viszonyok/Details/5
        public ActionResult Details(string id)
        {
            //itt nyerem ki, hogy kinek kellenek a munkalapjai
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                //ID atkonvertalasa
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int MunkalapID = int.Parse(IDArray.First());

                var model = db.Munkalapok.FirstOrDefault(x => x.ID == MunkalapID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }

                model.MunkasokLista = db.Users.ToList();
                model.FeladatokLista = db.Feladatok.ToList();

                //KOLTSEGEK lekerdezese

                model.KoltsegekLista = new List<Koltsegek>();

                List<Koltsegek> tempKoltsegLista = db.Koltsegek.Where(x => x.MunkalapID == model.ID).ToList();

                model.KoltsegekLista = tempKoltsegLista;

                ///mar nincs koltseg_munkalapokID tabla hasznalat, igy ezek nem kellenek

                model.KezdesiIdo = model.KIdo.ToString();
                model.BefejezesiIdo = model.BIdo.ToString();

                //mivel valaki megnezi ezert modositani kell az alapotat majd elmenteni
                /////itt allitodik, hogy ha azonos a USER, es meg nem nezte meg, akkor kipipalodik

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #region Details ARCHIV

        // GET: Viszonyok/Details/5
        public ActionResult DetailsA(string id)
        {
            //itt nyerem ki, hogy kinek kellenek a munkalapjai
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                //ID atkonvertalasa
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int MunkalapID = int.Parse(IDArray.First());

                var model = db.MunkalapokA.FirstOrDefault(x => x.ID == MunkalapID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatAID = int.Parse(IDArray.Last());
                }

                model.MunkasokLista = db.Users.ToList();
                model.FeladatokLista = db.FeladatokA.ToList();

                //KOLTSEGEK lekerdezese

                model.KoltsegekLista = new List<Koltsegek>();

                List<Koltsegek> tempKoltsegLista = db.Koltsegek.Where(x => x.MunkalapID == model.ID).ToList();

                model.KoltsegekLista = tempKoltsegLista;

                model.KezdesiIdo = model.KIdo.ToString();
                model.BefejezesiIdo = model.BIdo.ToString();

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        // GET: Viszonyok/Create
        public ActionResult Create(string id)
        {
            try
            {
                Munkalapok model = new Munkalapok();

                //itt nyerem ki, hogy kinek kellenek a munkalapjai
                Users tempUser = felhasznaloAzonositas();
                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }

                model.UserID = tempUser.UserID;

                model.MunkasokLista = db.Users.OrderBy(x => x.VezetekNev).ToList();

                foreach (var item in model.MunkasokLista)
                {
                    item.Nev = $"{item.VezetekNev} {item.KeresztNev}";
                }

                model.FeladatokLista = db.Feladatok.ToList();

                if (id != null)
                {
                    string[] separator = { "_" };

                    string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    int FeladatID = int.Parse(IDArray.First());

                    if (id != null)
                    {
                        model.FeladatID = FeladatID;
                        model.Atiranyitott = 1;
                    }
                    else
                    {
                        model.Atiranyitott = 0;
                    }

                    model.MunkaDatuma = DateTime.Today.Date;
                    return View(model);
                }
                //model.Atiranyitott = 0;
                model.MunkaDatuma = DateTime.Today.Date;

                return View(model);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Viszonyok/Create
        [HttpPost]
        public ActionResult Create(Munkalapok model)
        {
            //itt nyerem ki, hogy kinek kellenek a munkalapjai
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                //erre azert van szukseg, hogy arra a napra parhuzamosan tobb feladatot ugyan annak a szemlynek ne adjon
                List<Munkalapok> munkalapokLista = new List<Munkalapok>();

                munkalapokLista = db.Munkalapok.Where(x => x.MunkaDatuma == model.MunkaDatuma).Where(x => x.UserID == model.UserID).ToList();

                //Munka kezdesenek es befejezesenek kinyerese 

                string[] TimeArray = new string[2];

                TimeArray[0] = model.KezdesiIdo.Substring(0, 2);
                TimeArray[1] = model.KezdesiIdo.Substring(3, 2);
                model.KIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);
                ///Hogy le legyen kerulve az azonos idopontok hibaja letrehozunk egy k.ido+1 b.ido-1 perc valtozot
                ///es ezeket vizsgaljuk, hogy elternek e a mar letrehozott munkalapoktol
                var modelTempKIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]) + 1, 00);

                TimeArray[0] = model.BefejezesiIdo.Substring(0, 2);
                TimeArray[1] = model.BefejezesiIdo.Substring(3, 2);
                model.BIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);

                var modelTempBIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]) - 1, 00);

                //time vege

                foreach (var azonosMunkalap in munkalapokLista)
                {
                    if ((modelTempKIdo > azonosMunkalap.KIdo && azonosMunkalap.BIdo > modelTempKIdo)
                                            || (azonosMunkalap.KIdo < modelTempBIdo && azonosMunkalap.BIdo > modelTempBIdo))
                    {
                        TempData["ErrorMessage"] = "A kijelölt munkásnak már van munkalapja ebben az időpontban!";

                        model.MunkasokLista = db.Users.OrderBy(x => x.VezetekNev).ToList();
                        foreach (var item in model.MunkasokLista)
                        {
                            item.Nev = $"{item.VezetekNev} {item.KeresztNev}";
                        }

                        return View(model);
                    }
                }

                //miutan letrehoztam a feladatot fontos, hogy letrehozzam melle a szukseges MUNKALAPOKAT, de csak akkor, ha van hozza rendelve alkalmazott!
                if ((model.KIdo != null || model.BIdo != null) && model.UserID != null || model.FeladatID != null)
                {
                    //kinyerem a ket datum kozti kulonbseget, de az adott napot nem adja hozza igy az utolag kell potolni
                    TimeSpan OraPercKulonbseg = new TimeSpan();
                    if (model.BIdo > model.KIdo)
                    {
                        OraPercKulonbseg = (TimeSpan)(model.BIdo - model.KIdo);
                    }
                    else
                    {
                        OraPercKulonbseg = new TimeSpan(int.Parse(TimeArray[0]) + 24, int.Parse(TimeArray[1]), 00);
                        OraPercKulonbseg -= (TimeSpan)(model.KIdo);
                    }

                    double munkaIdo = new double();

                    if (OraPercKulonbseg.Minutes <= 15)
                    {
                        munkaIdo = 0;
                    }
                    else if (OraPercKulonbseg.Minutes > 15 && OraPercKulonbseg.Minutes < 30)
                    {
                        munkaIdo = 0.25;
                    }
                    else if (OraPercKulonbseg.Minutes >= 30 && OraPercKulonbseg.Minutes < 45)
                    {
                        munkaIdo = 0.50;
                    }
                    else
                    {
                        munkaIdo = 0.75;
                    }

                    //a kulonbsegbol megkapom, hogy hany orat dolgozott
                    model.MunkaOra = OraPercKulonbseg.Hours + Convert.ToDecimal(munkaIdo);

                    if (model.MunkaOra > 4 && model.MunkaOra <= 10)
                    {
                        model.MunkaOra = model.MunkaOra - Convert.ToDecimal(0.50);
                    }
                    else if (model.MunkaOra > 10)
                    {
                        model.MunkaOra = model.MunkaOra - 1;
                    }

                    //alap adatok az index lista letrehozazasahoz

                    #region Munkalap_ feladat-User kapcsolat
                    ///megvizsgaljuk, hogy az-az alkalmazott, akinek hozzadjuk a munkalapjat
                    ///szerepel e masik munkalapon, mert ha nem, akkor hozza kell adjuk a kapcsolatat
                    ///a feladattal, hiszen letrehozunk neki munkalapot

                    var feladatUserLista = db.Feladat_User_ID.Where(x => x.FeladatID == model.FeladatID && x.UserID == model.UserID).ToList();

                    if (feladatUserLista.Count() < 1)
                    {
                        Feladat_User_ID feladat_User_ID_ITEM = new Feladat_User_ID();

                        feladat_User_ID_ITEM.FeladatID = model.FeladatID;
                        feladat_User_ID_ITEM.UserID = model.UserID;

                        db.Feladat_User_ID.Add(feladat_User_ID_ITEM);
                    }

                    #endregion

                    //kotelezoen kitoltendok (not null)
                    model.Datum = DateTime.Now;
                    model.JovahagyvaCB = false;

                    ///Ha a USER sajat maganak hoz letre munkalapot, akkor automatikusan MEGTEKINTETTE
                    ///ha mas hozza neki letre, akkor pedig a modositasnal lesz megtekintve. 

                    bool letrehozEsMunkavegzoAzonos = db.Users.Where(item => item.UserID == model.UserID).FirstOrDefault().Elerhetosegek1.Email
                                                == tempUser.Elerhetosegek1.Email;

                    if (letrehozEsMunkavegzoAzonos)
                    {
                        model.Megtekintve = true;
                    }
                    else
                    {
                        model.Megtekintve = false;
                    }


                    db.Munkalapok.Add(model);
                    db.SaveChanges();

                    ///legyen hozzaadva KOLTSEG, 
                    ///ezert a modositas utan bele megyunk a munkalap reszleteibe,
                    ///ha viszont mas hozza letre a munkalapot, akkor a feladatra megyen vissza
                    if (letrehozEsMunkavegzoAzonos)
                    {
                        return RedirectToAction("Details", "Munkalapok", new { id = $"{model.ID}_{model.FeladatID}" });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Feladat", new { id = model.FeladatID });
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(model);
            }
        }

        // GET: Viszonyok/Edit/5
        public ActionResult Edit(string id)
        {
            //itt nyerem ki, hogy kinek kellenek a munkalapjai
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                //ID atkonvertalasa
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int MunkalapID = int.Parse(IDArray.First());

                var model = db.Munkalapok.FirstOrDefault(x => x.ID == MunkalapID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.Atiranyitott = 1;
                }
                else
                {
                    model.Atiranyitott = 0;
                }

                model.FeladatokLista = db.Feladatok.ToList();

                model.KezdesiIdo = $"{model.KIdo}";
                model.BefejezesiIdo = $"{model.BIdo}";

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        // POST: Viszonyok/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Munkalapok munkalapok)
        {
            //itt nyerem ki, hogy kinek kellenek a munkalapjai
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }
            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int MunkalapID = int.Parse(IDArray.First());

                var model = db.Munkalapok.FirstOrDefault(x => x.ID == MunkalapID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.Atiranyitott = 1;
                }
                else
                {
                    model.Atiranyitott = 0;
                }

                model.JovahagyvaCB = munkalapok.JovahagyvaCB;
                model.Magyjegyzes = munkalapok.Magyjegyzes;
                if (munkalapok.MegtettKM != null)
                {
                    model.MegtettKM = munkalapok.MegtettKM;
                }
                else
                {
                    model.MegtettKM = 0;
                }


                ///ha a felhasznalo maganak szerkeszti a munkalapjat, akkor az mar megtekintve lesz
                ///ha viszont mas szerkeszti, akkor az nem szamit megtekintesnek
                if (tempUser.UserID == model.UserID)
                {
                    model.Megtekintve = true;
                }
                else
                {
                    model.Megtekintve = false;
                }

                //erre azert van szukseg, hogy arra a napra parhuzamosan tobb feladatot ugyan annak a szemlynek ne adjon
                List<Munkalapok> munkalapokLista = new List<Munkalapok>();

                munkalapokLista = db.Munkalapok.Where(x => x.MunkaDatuma == munkalapok.MunkaDatuma && x.UserID == model.UserID && x.ID != model.ID).ToList();

                ///Munka kezdesenek es befejezesenek kinyerese 
                ///es vizsgaljuk, hog yerre az idopontra van e mar munkalap letrehozva

                string[] TimeArray = new string[2];

                TimeArray[0] = munkalapok.KezdesiIdo.Substring(0, 2);
                TimeArray[1] = munkalapok.KezdesiIdo.Substring(3, 2);
                model.KIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);
                ///Hogy le legyen kerulve az azonos idopontok hibaja letrehozunk egy k.ido+1 b.ido-1 perc valtozot
                ///es ezeket vizsgaljuk, hogy elternek e a mar letrehozott munkalapoktol
                var modelTempKIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]) + 1, 00);


                TimeArray[0] = munkalapok.BefejezesiIdo.Substring(0, 2);
                TimeArray[1] = munkalapok.BefejezesiIdo.Substring(3, 2);
                model.BIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]), 00);

                var modelTempBIdo = new TimeSpan(int.Parse(TimeArray[0]), int.Parse(TimeArray[1]) - 1, 00);

                foreach (var azonosMunkalap in munkalapokLista)
                {
                    if ((modelTempKIdo > azonosMunkalap.KIdo && azonosMunkalap.BIdo > modelTempKIdo)
                                            || (azonosMunkalap.KIdo < modelTempBIdo && azonosMunkalap.BIdo > modelTempBIdo))
                    {
                        TempData["ErrorMessage"] = "A kijelölt munkásnak már van munkalapja ebben az időpontban!";

                        munkalapok.MunkasokLista = db.Users.OrderBy(x => x.VezetekNev).ToList();
                        foreach (var item in munkalapok.MunkasokLista)
                        {
                            item.Nev = $"{item.VezetekNev} {item.KeresztNev}";
                        }

                        return View(munkalapok);
                    }
                }

                //time vege

                ///miutan letrehoztam a feladatot fontos, hogy letrehozzam melle a szukseges MUNKALAPOKAT, de csak akkor, ha van hozza rendelve alkalmazott!
                ///kinyerem a ket datum kozti kulonbseget, de az adott napot nem adja hozza igy az utolag kell potolni
                ///ha pedig atnyulik az ejszakaba (ejfel utan), akkor 24orat hozza kell adni, hogy valos munkaidot kapjunk
                TimeSpan OraPercKulonbseg = new TimeSpan();
                if (model.BIdo > model.KIdo)
                {
                    OraPercKulonbseg = (TimeSpan)(model.BIdo - model.KIdo);
                }
                else
                {
                    OraPercKulonbseg = new TimeSpan(int.Parse(TimeArray[0]) + 24, int.Parse(TimeArray[1]), 00);
                    OraPercKulonbseg -= (TimeSpan)(model.KIdo);
                }

                double munkaIdo = new double();

                if (OraPercKulonbseg.Minutes < 15)
                {
                    munkaIdo = 0;
                }
                else if (OraPercKulonbseg.Minutes >= 15 && OraPercKulonbseg.Minutes < 30)
                {
                    munkaIdo = 0.25;
                }
                else if (OraPercKulonbseg.Minutes >= 30 && OraPercKulonbseg.Minutes < 45)
                {
                    munkaIdo = 0.50;
                }
                else
                {
                    munkaIdo = 0.75;
                }


                //a kulonbsegbol megkapom, hogy hany orat dolgozott
                model.MunkaOra = OraPercKulonbseg.Hours + Convert.ToDecimal(munkaIdo);

                if (model.MunkaOra > 4 && model.MunkaOra <= 10)
                {
                    model.MunkaOra = model.MunkaOra - Convert.ToDecimal(0.50);
                }
                else if (model.MunkaOra > 10)
                {
                    model.MunkaOra = model.MunkaOra - 1;
                }

                //kotelezoen kitoltendok (not null)
                model.Datum = DateTime.Now;
                model.JovahagyvaCB = false;

                model.MunkaDatuma = munkalapok.MunkaDatuma;
                model.KezdesiIdo = $"{model.KIdo}";
                model.BefejezesiIdo = $"{model.BIdo}";

                db.Munkalapok.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();



                ///legyen hozzaadva KOLTSEG, 
                ///ezert a modositas utan bele megyunk a munkalap reszleteibe,
                ///ha viszont mas hozza letre a munkalapot, akkor a feladatra megyen vissza
                if (model.Megtekintve)
                {
                    return RedirectToAction("Details", "Munkalapok", new { id = $"{model.ID}_{model.FeladatID}" });
                }
                else if (model.Atiranyitott == 1)
                {
                    return RedirectToAction("Details", "Feladat", new { id = model.FeladatID });
                }
                else
                {
                    return RedirectToAction("Index", "Munkalapok");
                }
            }
            catch (Exception e)
            {
                munkalapok.MunkasokLista = db.Users.ToList();
                foreach (var item in munkalapok.MunkasokLista)
                {
                    item.Nev = $"{item.VezetekNev} {item.KeresztNev}";
                }
                munkalapok.FeladatokLista = db.Feladatok.ToList();
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(munkalapok);
            }
        }

        // GET: Viszonyok/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                //itt nyerem ki, hogy kinek kellenek a munkalapjai
                Users tempUser = felhasznaloAzonositas();
                if (tempUser == null)
                {
                    return RedirectToAction("Bejelentkezes", "Users");
                }

                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int munkalapID = int.Parse(IDArray.First());

                var model = db.Munkalapok.FirstOrDefault(x => x.ID == munkalapID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }

                //eloszor a felhasznalora vizsgal ra, hogy ha alkalmazott, akkor
                if ((tempUser.Jogosultsag.Nev != "ADMIN" && tempUser.Jogosultsag.Nev != "Vezetőség") || tempUser.UserID == model.UserID)
                {
                    if (model.JovahagyvaCB)
                    {
                        TempData["ErrorMessage"] = "Nem törölhető, mert a munkalap már jóvá lett hagyva!";
                        return RedirectToAction("Details", "Feladat", new { @id = model.FeladatID });
                    }
                }
                //ha vezetosegi tag, akkor
                else if (model.Megtekintve && tempUser.Jogosultsag.Nev == "Vezetőség")
                {
                    TempData["ErrorMessage"] = "Nem törölhető, mert a munkalap már módosítva lett!";
                    return RedirectToAction("Details", "Munkalapok", new { @id = model.ID });
                }
                //ha ADMIN, akkor
                else if (tempUser.Jogosultsag.Nev == "ADMIN")
                {
                    model.JovahagyvaCB = false;
                    model.Megtekintve = false;

                    model.KezdesiIdo = model.KIdo.ToString();
                    model.BefejezesiIdo = model.BIdo.ToString();

                    db.Munkalapok.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index");
            }
        }

        // POST: Viszonyok/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Munkalapok munkalap)
        {
            try
            {
                var tempUser = db.Users.Where(x => x.Elerhetosegek.Email == User.Identity.Name).FirstOrDefault();

                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int munkalapID = int.Parse(IDArray.First());

                var model = db.Munkalapok.FirstOrDefault(x => x.ID == munkalapID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }

                var vanKapcsolodoKoltseg = db.Koltsegek.Where(x => x.MunkalapID == munkalapID).ToList();

                foreach (var item in vanKapcsolodoKoltseg)
                {
                    db.Koltsegek.Remove(item);
                }

                #region Munkalap_ feladat-User kapcsolat

                ///megvizsgaljuk, hogy az-az alkalmazott, akinek toroljuk a munkalapjat
                ///szerepel e masik munkalapon, mert ha nem, akkor ki kell vegyuk a kapcsolatat
                ///a feladattal, hiszen megszunnek a munkalapjai
                var feladatUserMunkalapLista = db.Munkalapok.Where(x => x.FeladatID == model.FeladatID && x.UserID == model.UserID).ToList();

                if (feladatUserMunkalapLista.Count() <= 1)
                {
                    var feladatUserIDItem = db.Feladat_User_ID.FirstOrDefault(x => x.FeladatID == model.FeladatID && x.UserID == model.UserID);
                    db.Feladat_User_ID.Remove(feladatUserIDItem);
                }

                #endregion

                db.Munkalapok.Remove(model);
                db.SaveChanges();

                return RedirectToAction("Details", "Feladat", new { id = model.FeladatID });
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int munkalapID = int.Parse(IDArray.First());

                return View(db.Munkalapok.FirstOrDefault(x => x.ID == munkalapID));
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
                //ID atkonvertalasa

                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int year = int.Parse(IDArray.First());

                int mounth = int.Parse(IDArray.Last());

                List<Munkalapok> model = new List<Munkalapok>();
                List<Munkalapok> tempModel = new List<Munkalapok>();
                List<Munkalapok> tempmunkalapokExcelLista = new List<Munkalapok>();


                ///A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
                ///DE ERDEMESEBB LENNE NEM EMBEREKRE LEBONTANI, MERT AKKOR A MUNKATERVEZOHOZ
                ///HASONLOAN LEHETNE MEGCSINALNI, DE EGYELORE ez STORNO!!!
                Users tempUser = db.Users.Where(x => x.Elerhetosegek.Email == User.Identity.Name).FirstOrDefault();

                if (tempUser.Jogosultsag.Nev != "Alkalmazott")
                {
                    tempmunkalapokExcelLista =
                        db.Munkalapok.
                        Where(x => x.MunkaDatuma.Month == mounth && x.MunkaDatuma.Year == year).ToList();
                }
                else
                {
                    tempmunkalapokExcelLista = db.Munkalapok.Where(x => x.MunkaDatuma.Month == mounth && x.MunkaDatuma.Year == year && x.UserID == tempUser.UserID).OrderBy(x => x.MunkaDatuma).ToList();
                }

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

                if (tempmunkalapokExcelLista.Count() == 0)
                {
                    TempData["ErrorMessage"] = "Nincs kilistázható munkalap!";

                    RedirectToAction("Index", "Munkalapok").ExecuteResult(this.ControllerContext);
                }
                else
                {
                    //osszegek szamitasa: munkaora total, koltsegek total, KM total
                    decimal MunkaoraTotal = 0;
                    int KMTotal = 0;
                    int KoltsegTotal = 0;

                    ///ide mar lehet egybol tenni  a MODELT
                    /*tempModel*/
                    model = tempmunkalapokExcelLista./*Where(x => x.Megtekintve).OrderBy(x => x.JovahagyvaCB).ToList();*/Select(x => new Munkalapok
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

                    model = model.OrderBy(x => x.userNev).ThenBy(x => x.MunkaDatuma).ToList();

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


                    //int columnStart = 0; // az A oszlopot mi adjuk hozza, a tobbi pedig a C-vel kezdodik
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

                    Munkalapok prevItem = new Munkalapok();

                    ///Bovitett XLS adatainak megadasa User.Projekt.Munkaora
                    UserProjektMunkaoraXLShez UpmXlsItem = new UserProjektMunkaoraXLShez()
                    {
                        UserName = model[0].userNev
                    };

                    ///erre azert van szukseg, hogy tartsuk nyilvan a projektek-row osszefuggeseket
                    List<ProjektRow> projektAdatokLista = new List<ProjektRow>();

                    ///ezzel tartjuk szamon, hogy hanyadik sorba kerul vegul majd a kiiras,
                    ///azert indul egytol, mert a 0. helyen a "tabla" fejlece lesz
                    int projektRowNumerator = 1;

                    ///Ebbe a listaba elmentujuk, hogy ki hany orat dolgozik osszesen adott honapba,
                    ///es hogy mennyit dolgoznak osszesen adott projekten
                    List<decimal> totalAlkalmazottMunkaora = new List<decimal>();
                    List<ProjektRow> totalProjektMunkaOra = new List<ProjektRow>();

                    ///vegleges megjelenitendo lista
                    List<UserProjektMunkaoraXLShez> UpmXls = new List<UserProjektMunkaoraXLShez>();

                    foreach (var item in model)
                    {
                        bool azonosProjekt = false;

                        if (tempUserID == item.UserID /*&& tempDate < item.MunkaDatuma*/)
                        {
                            //megkeressuk, hogy hanyadik bekezdesben van az o datuma
                            ///erre azert van szukseg, hogy ha letezik mar arra a napra feladat, akkor ezt is adja hozza.
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

                                ///XLS bovitesi adatok
                                for (int i = 0; i < projektAdatokLista.Count; i++)
                                {
                                    if (projektAdatokLista[i].ProjKod == item.projNev
                                        && UpmXlsItem.ProjektekAdatai.Any(x => x.ProjKod == item.projNev))
                                    {
                                        var temp = UpmXlsItem.ProjektekAdatai.Where(x => x.ProjKod == item.projNev).First();
                                        temp.Munkaora = (decimal)(temp.Munkaora + item.MunkaOra);
                                        azonosProjekt = true;
                                        break;
                                    }
                                    else if (projektAdatokLista[i].ProjKod == item.projNev)
                                    {
                                        UpmXlsItem.ProjektekAdatai.Add(new ProjektRow
                                        {
                                            ProjKod = projektAdatokLista[i].ProjKod,
                                            RowNr = projektAdatokLista[i].RowNr,
                                            Munkaora = 0
                                        });

                                        var temp = UpmXlsItem.ProjektekAdatai.Where(x => x.ProjKod == item.projNev).First();
                                        temp.Munkaora = (decimal)(temp.Munkaora + item.MunkaOra);
                                        azonosProjekt = true;
                                        break;
                                    }
                                }

                                if (!azonosProjekt)
                                {
                                    ProjektRow projektItem = new ProjektRow();

                                    projektItem.ProjKod = item.projNev;
                                    projektItem.Munkaora = (decimal)(projektItem.Munkaora + item.MunkaOra);
                                    projektItem.RowNr = projektRowNumerator++;

                                    projektAdatokLista.Add(projektItem);

                                    UpmXlsItem.ProjektekAdatai.Add(projektItem);

                                    totalProjektMunkaOra.Add(new ProjektRow
                                    {
                                        Munkaora = 0,
                                        ProjKod = projektItem.ProjKod,
                                        RowNr = projektItem.RowNr
                                    });
                                }


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
                                    $"= {(ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value)} + {item.MunkaOra}".Replace("= =", "=");
                                MunkaoraTotal += (decimal)item.MunkaOra;
                                rowStart++;
                                ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                                    $"= {(ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value)} + {item.koltsegItem}".Replace("= =", "=");
                                KoltsegTotal += item.koltsegItem;
                                rowStart++;
                                //az EXCAPTION kizarasa vegett
                                if (item.MegtettKM == null)
                                {
                                    item.MegtettKM = 0;
                                }
                                ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value =
                                    $"= {(ws.Cells[string.Format($"{ABC[item.MunkaDatuma.Day - 1]}{rowStart}")].Value)} + {item.MegtettKM}".Replace("= =", "=");
                                KMTotal += (int)item.MegtettKM;

                                rowStart -= 4;

                                ///XLS bovitesi adatok
                                for (int i = 0; i < projektAdatokLista.Count; i++)
                                {
                                    if (projektAdatokLista[i].ProjKod == item.projNev
                                        && UpmXlsItem.ProjektekAdatai.Any(x => x.ProjKod == item.projNev))
                                    {
                                        var temp = UpmXlsItem.ProjektekAdatai.Where(x => x.ProjKod == item.projNev).First();
                                        temp.Munkaora = (decimal)(temp.Munkaora + item.MunkaOra);
                                        azonosProjekt = true;
                                        break;
                                    }
                                    else if (projektAdatokLista[i].ProjKod == item.projNev)
                                    {
                                        UpmXlsItem.ProjektekAdatai.Add(new ProjektRow
                                        {
                                            ProjKod = projektAdatokLista[i].ProjKod,
                                            RowNr = projektAdatokLista[i].RowNr,
                                            Munkaora = 0
                                        });

                                        var temp = UpmXlsItem.ProjektekAdatai.Where(x => x.ProjKod == item.projNev).First();
                                        temp.Munkaora = (decimal)(temp.Munkaora + item.MunkaOra);
                                        azonosProjekt = true;
                                        break;
                                    }
                                }

                                if (!azonosProjekt)
                                {
                                    ProjektRow projektItem = new ProjektRow();
                                    projektItem.ProjKod = item.projNev;
                                    projektItem.Munkaora = (decimal)(projektItem.Munkaora + item.MunkaOra);
                                    projektItem.RowNr = projektRowNumerator++;

                                    projektAdatokLista.Add(projektItem);

                                    UpmXlsItem.ProjektekAdatai.Add(projektItem);

                                    totalProjektMunkaOra.Add(new ProjektRow
                                    {
                                        Munkaora = 0,
                                        ProjKod = projektItem.ProjKod,
                                        RowNr = projektItem.RowNr
                                    });
                                }

                            }

                            prevItem = item;

                        }
                        else if (tempUserID != item.UserID)
                        {
                            ///elozo user osszegzesenek kiirasa
                            ws.Cells[string.Format($"A{rowStart + 1}")].Value =
                               $"TOTÁL m.ó.: {MunkaoraTotal}";

                            ///az XLS bovitesnel a legvegere is kibiggyesszuk az ossz munkaorajat az alkalmazottnak
                            ///ezt ebbe a valtozoba taroljuk el ideiglenesen
                            totalAlkalmazottMunkaora.Add(MunkaoraTotal);

                            ws.Cells[string.Format($"A{rowStart + 2}")].Value =
                                    $"TOTÁL költ.: {KoltsegTotal}";
                            ws.Cells[string.Format($"A{rowStart + 3}")].Value =
                                    $"TOTÁL km.: {KMTotal}";

                            MunkaoraTotal = 0;
                            KMTotal = 0;
                            KoltsegTotal = 0;

                            rowStart += 5;

                            ///XLS bovitese, mivel mas user lesz ezert az elozo adatait toroljuk
                            UpmXls.Add(new UserProjektMunkaoraXLShez
                            {
                                ColumnNr = UpmXlsItem.ColumnNr,
                                UserName = UpmXlsItem.UserName,
                                ProjektekAdatai = new List<ProjektRow>(UpmXlsItem.ProjektekAdatai.Select(x => new ProjektRow()
                                {
                                    Munkaora = x.Munkaora,
                                    ProjKod = x.ProjKod,
                                    RowNr = x.RowNr
                                }))
                            });

                            UpmXlsItem.ProjektekAdatai.Clear();
                            UpmXlsItem.UserName = item.userNev;
                            UpmXlsItem.ColumnNr = UpmXlsItem.ColumnNr + 1;


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

                            ///XLS bovitesi adatok
                            for (int i = 0; i < projektAdatokLista.Count; i++)
                            {
                                if (projektAdatokLista[i].ProjKod == item.projNev
                                        && UpmXlsItem.ProjektekAdatai.Any(x => x.ProjKod == item.projNev))
                                {
                                    var temp = UpmXlsItem.ProjektekAdatai.Where(x => x.ProjKod == item.projNev).First();
                                    temp.Munkaora = (decimal)(temp.Munkaora + item.MunkaOra);
                                    azonosProjekt = true;
                                    break;
                                }
                                else if (projektAdatokLista[i].ProjKod == item.projNev)
                                {
                                    UpmXlsItem.ProjektekAdatai.Add(new ProjektRow
                                    {
                                        ProjKod = projektAdatokLista[i].ProjKod,
                                        RowNr = projektAdatokLista[i].RowNr,
                                        Munkaora = 0
                                    });

                                    var temp = UpmXlsItem.ProjektekAdatai.Where(x => x.ProjKod == item.projNev).First();
                                    temp.Munkaora = (decimal)(temp.Munkaora + item.MunkaOra);
                                    azonosProjekt = true;
                                    break;
                                }
                            }

                            if (!azonosProjekt)
                            {
                                ProjektRow projektItem = new ProjektRow();
                                projektItem.ProjKod = item.projNev;
                                projektItem.Munkaora = (decimal)(projektItem.Munkaora + item.MunkaOra);
                                projektItem.RowNr = projektRowNumerator++;

                                projektAdatokLista.Add(projektItem);

                                UpmXlsItem.ProjektekAdatai.Add(projektItem);

                                totalProjektMunkaOra.Add(new ProjektRow
                                {
                                    Munkaora = 0,
                                    ProjKod = projektItem.ProjKod,
                                    RowNr = projektItem.RowNr
                                });
                            }

                        }

                    }

                    ///XLS bovitese, az UTOLSO USER-t itt adjuk hozza
                    UpmXls.Add(new UserProjektMunkaoraXLShez
                    {
                        ColumnNr = UpmXlsItem.ColumnNr,
                        UserName = UpmXlsItem.UserName,
                        ProjektekAdatai = new List<ProjektRow>(UpmXlsItem.ProjektekAdatai.Select(x => new ProjektRow()
                        {
                            Munkaora = x.Munkaora,
                            ProjKod = x.ProjKod,
                            RowNr = x.RowNr
                        }))
                    });

                    ///az utolso user osszegzesenek kiirasa
                    ws.Cells[string.Format($"A{rowStart + 1}")].Value =
                                $"TOTÁL m.ó.: {MunkaoraTotal}";
                    ///az XLS bovitesnel a legvegere is kibiggyesszuk az ossz munkaorajat az alkalmazottnak
                    ///ezt ebbe a valtozoba taroljuk el ideiglenesen
                    totalAlkalmazottMunkaora.Add(MunkaoraTotal);

                    ws.Cells[string.Format($"A{rowStart + 2}")].Value =
                            $"TOTÁL költ.: {KoltsegTotal}";
                    ws.Cells[string.Format($"A{rowStart + 3}")].Value =
                            $"TOTÁL km.: {KMTotal}";


                    //IDE JÖN AZ ÚJ RÉSZ - Érdemes új osztályt létrehozni, ami a felhasználót veszi alapúl ... amit össze kell majd fűzni a projektekkel :( 
                    //itt is felhasznaljuk az ABC valtozot az az oszlopok jelolesere... pontosabban az egy INT, hogy az ABC vector hanyadik elemerol van szo
                    int rowFejlec = rowStart + 8;

                    ///tabla megrajzolasa
                    ws.Cells[($"A{rowFejlec}")].Value = $"PROJEKTEK/ALKALMAZOTTAK";
                    ws.Cells[($"B{rowFejlec}")].Value = $"TOTAL proj. m.ó.";

                    int legutolsoRowErtek = projektRowNumerator + rowFejlec;

                    int alkalmazottLepegeto = 0;

                    foreach (var xlsItem in UpmXls)
                    {
                        ///projekten dolgoztt alkalmazott nevenek kiiratasa
                        ws.Cells[string.Format($"{ABC[xlsItem.ColumnNr]}{rowFejlec}")].Value = $"{xlsItem.UserName}";

                        for (int i = 0; i < xlsItem.ProjektekAdatai.Count; i++)
                        {
                            ///projekt nevenek kiiratasa
                            ws.Cells[($"A{xlsItem.ProjektekAdatai[i].RowNr + rowFejlec}")].Value = $"{xlsItem.ProjektekAdatai[i].ProjKod}";

                            ///projekten resztvett orak szamanak kiiratasa
                            ws.Cells[string.Format($"{ABC[xlsItem.ColumnNr]}{xlsItem.ProjektekAdatai[i].RowNr + rowFejlec}")].Value =
                            $"{xlsItem.ProjektekAdatai[i].Munkaora}";

                            ///a projekten osszesen ledolgozott munka orak szama
                            for (int j = 0; j < totalProjektMunkaOra.Count; j++)
                            {
                                if (totalProjektMunkaOra[j].ProjKod == xlsItem.ProjektekAdatai[i].ProjKod)
                                {
                                    var tempTotalMO = totalProjektMunkaOra.Where(x => x.ProjKod == xlsItem.ProjektekAdatai[i].ProjKod).First();
                                    tempTotalMO.Munkaora = (decimal)(tempTotalMO.Munkaora + xlsItem.ProjektekAdatai[i].Munkaora);
                                    break;
                                }
                            }
                        }

                        ///A USER-hez tartozo total munkaorak szama
                        ws.Cells[string.Format($"{ABC[xlsItem.ColumnNr]}{legutolsoRowErtek}")].Value =
                            $"{totalAlkalmazottMunkaora[alkalmazottLepegeto++]}";
                    }

                    for (int k = 0; k < totalProjektMunkaOra.Count; k++)
                    {
                        ///a projekten osszesen ledolgozott munka orak szamanak kiiratasa
                        ws.Cells[($"B{totalProjektMunkaOra[k].RowNr + rowFejlec}")].Value = $"{totalProjektMunkaOra[k].Munkaora}";
                    }


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
