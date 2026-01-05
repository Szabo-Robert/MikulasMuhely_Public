using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class FelhasznaltAnyagokController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index(int? pn)
        {
            //Felhasznalo jogosultsaganak beazonositasa
            var tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                List<FelhasznaltAnyagok> model = new List<FelhasznaltAnyagok>();
                //List<FelhasznaltAnyagok> tempModel = new List<FelhasznaltAnyagok>();

                //Levalogatjuk, hogy a USER-hez melyik feladatok vannak hozzarendelve 
                //es csak ezeknek a felhasznalt anyagjait listazzuk ki
                Feladatok aUserFeladatjai = new Feladatok();
                List<Feladat_FelhaszAnyag_ID> feladatID_FelhasznaltAnyagID_Lista = db.Feladat_FelhaszAnyag_ID.ToList();

                var tempFeladatIDLista = db.Feladat_User_ID.Where(x => x.UserID == tempUser.UserID).ToList();


                foreach (var userID_FeladatID_Item in tempFeladatIDLista)
                {
                    aUserFeladatjai = db.Feladatok.FirstOrDefault(x => x.ID == userID_FeladatID_Item.FeladatID);

                    foreach (var feladatID_FelhasznaltAnyagID_Item in feladatID_FelhasznaltAnyagID_Lista)
                    {
                        if (feladatID_FelhasznaltAnyagID_Item.FeladatID == userID_FeladatID_Item.FeladatID)
                        {
                            FelhasznaltAnyagok felhasznaltAnyagokItem = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == feladatID_FelhasznaltAnyagID_Item.FelhasznaltAnyagID);

                            felhasznaltAnyagokItem.FeladatNeve = aUserFeladatjai.Nev;
                            model.Add(felhasznaltAnyagokItem);

                        }
                    }
                }

                return View(model.OrderByDescending(x => x.FeladatID).ToList().ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Viszonyok/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Viszonyok/Create
        public ActionResult Create(int? id)
        {
            //Felhasznalo jogosultsaganak beazonositasa
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                FelhasznaltAnyagok model = new FelhasznaltAnyagok();
                model.tizFelhasznaltAnyagNeveLista = new FelhasznaltAnyagokNeveLista[10];

                if (id != null)
                {
                    model.FeladatID = (int)id;

                    return View(model);
                }

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
        public ActionResult Create(FelhasznaltAnyagok model)
        {
            //Felhasznalo jogosultsaganak beazonositasa
            var tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                foreach (var item in model.tizFelhasznaltAnyagNeveLista)
                {
                    if (item.felhasznaltAnyagNeve != null)
                    {
                        FelhasznaltAnyagok createModel = new FelhasznaltAnyagok();
                        createModel.Nev = item.felhasznaltAnyagNeve;
                        createModel.UserNev = $"{tempUser.VezetekNev} {tempUser.KeresztNev}";

                        db.FelhasznaltAnyagok.Add(createModel);

                        Feladat_FelhaszAnyag_ID temp = new Feladat_FelhaszAnyag_ID();
                        temp.FeladatID = model.FeladatID;
                        temp.FelhasznaltAnyagID = createModel.ID;

                        db.Feladat_FelhaszAnyag_ID.Add(temp);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Details", "Feladat", new { id = model.FeladatID });
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
            //Felhasznalo jogosultsaganak beazonositasa
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int FelhasznaltAnyagID = int.Parse(IDArray.First());

                var model = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == FelhasznaltAnyagID);

                //erre azert van szukseg, mert ha nincs ProjektID, akkor ne adja a FeladatID-jat ertekul neki,
                //igy a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }
                else
                {
                    var feladatFelhasznaltAnyagIDja = db.Feladat_FelhaszAnyag_ID.FirstOrDefault(x => x.FelhasznaltAnyagID == FelhasznaltAnyagID);
                    model.FeladatID = feladatFelhasznaltAnyagIDja.FeladatID;
                }

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
        public ActionResult Edit(string id, FelhasznaltAnyagok felhasznaltAnyagok)
        {
            //Felhasznalo jogosultsaganak beazonositasa
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int FelhasznaltAnyagID = int.Parse(IDArray.First());

                var model = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == FelhasznaltAnyagID);

                //erre azert van szukseg, mert ha nincs ProjektID, akkor ne adja a FeladatID-jat ertekul neki,
                //igy a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }
                else
                {
                    var feladatFelhasznaltAnyagIDja = db.Feladat_FelhaszAnyag_ID.FirstOrDefault(x => x.FelhasznaltAnyagID == FelhasznaltAnyagID);
                    model.FeladatID = feladatFelhasznaltAnyagIDja.FeladatID;
                }

                model.Nev = felhasznaltAnyagok.Nev;
                model.UserNev = $"{tempUser.VezetekNev} {tempUser.KeresztNev}";

                db.FelhasznaltAnyagok.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "Feladat", new { id = model.FeladatID });
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(felhasznaltAnyagok);
            }
        }

        // GET: Viszonyok/Delete/5
        public ActionResult Delete(string id)
        {
            //Felhasznalo jogosultsaganak beazonositasa
            var tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int felhasznaltAnyagID = int.Parse(IDArray.First());

                var model = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == felhasznaltAnyagID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }

                //var model = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == id);

                var vanKapcsolodoElem = db.Feladatok.Where(x => x.ID == model.FeladatID).FirstOrDefault();

                if (vanKapcsolodoElem.JovahagyvaCB == true)
                {

                    TempData["ErrorMessage"] = "Nem törölhető, mert a feladat már jóvá lett hagyva!";
                    return RedirectToAction("Details", "Feladat", new { @id = model.FeladatID });
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
        public ActionResult Delete(string id, FelhasznaltAnyagok felhasznaltAnyag)
        {
            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int felhasznaltAnyagID = int.Parse(IDArray.First());

                var model = db.FelhasznaltAnyagok.FirstOrDefault(x => x.ID == felhasznaltAnyagID);

                Feladat_FelhaszAnyag_ID deleteModel = db.Feladat_FelhaszAnyag_ID.FirstOrDefault(x => x.FelhasznaltAnyagID == felhasznaltAnyagID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.FeladatID = int.Parse(IDArray.Last());
                }

                db.Feladat_FelhaszAnyag_ID.Remove(deleteModel);

                db.FelhasznaltAnyagok.Remove(model);
                db.SaveChanges();

                return RedirectToAction("Details", "Feladat", new { id = model.FeladatID });
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
    }
}
