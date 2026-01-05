using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SantaFactory.Models;


namespace SantaFactory.Controllers
{
    public class KoltsegekController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index(int? id, int? pn)
        {
            //Felhasznalo jogosultsaganak meghatarozasa
            Users tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                List<Koltsegek> model = new List<Koltsegek>();

                //Levalogatjuk, hogy a USER-hez melyik feladatok vannak hozzarendelve 
                //es csak ezeknek a koltseget listazzuk ki

                var tempMunkalapokLista = db.Munkalapok.Where(x => x.UserID == tempUser.UserID).ToList();

                foreach (var item in tempMunkalapokLista)
                {
                    List<Koltsegek> koltsegItemLista = db.Koltsegek.Where(x => x.MunkalapID == item.ID).ToList();

                    foreach (var koltsegItem in koltsegItemLista)
                    {
                        koltsegItem.feladatItem = db.Feladatok.FirstOrDefault(x => x.ID == item.FeladatID);
                        koltsegItem.SzamlaDatuma = koltsegItem.Munkalapok.MunkaDatuma;
                        model.Add(koltsegItem);
                    }

                }

                return View(model.OrderByDescending(x => x.FeladatID).ToPagedList(pn ?? 1, 10));
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Viszonyok/Details/5
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: Viszonyok/Create
        public ActionResult Create(int? id)
        {
            //Felhasznalo jogosultsaganak meghatarozasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                if (id != null)
                {
                    Koltsegek model = new Koltsegek();

                    model.MunkalapID = (int)id;

                    return View(model);
                }

                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Viszonyok/Create
        [HttpPost]
        public ActionResult Create(Koltsegek model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Koltsegek.Add(model);
                    db.SaveChanges();

                    ///Betettuk a munkalap ID-t a koltseg melle
                    return RedirectToAction("Details", "Munkalapok", new { @id = model.MunkalapID });
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
            //Felhasznalo jogosultsaganak meghatarozasa
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

                int KoltsegID = int.Parse(IDArray.First());

                var model = db.Koltsegek.FirstOrDefault(x => x.ID == KoltsegID);

                if (IDArray.Count() > 1)
                {
                    model.MunkalapID = int.Parse(IDArray.Last());
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
        public ActionResult Edit(string id, Koltsegek koltsegek)
        {
            try
            {
                //ID atkonvertalasa

                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int KoltsegID = int.Parse(IDArray.First());

                var model = db.Koltsegek.FirstOrDefault(x => x.ID == KoltsegID);

                if (IDArray.Count() > 1)
                {

                    model.MunkalapID = int.Parse(IDArray.Last());

                }

                model.KoltsegNeve = koltsegek.KoltsegNeve;
                model.KoltsegErteke = koltsegek.KoltsegErteke;
                model.KoltsegLeirasa = koltsegek.KoltsegLeirasa;
                model.MunkalapID = koltsegek.MunkalapID;

                db.Koltsegek.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                if (model.MunkalapID != 0)
                {
                    return RedirectToAction("Details", "Munkalapok", new { @id = model.MunkalapID });
                }
                else
                {
                    return RedirectToAction("Index", "Koltsegek");
                }

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Nem törölhető, mert a feladat már jóvá lett hagyva!";
                return View(koltsegek);
            }
        }

        // GET: Viszonyok/Delete/5
        public ActionResult Delete(string id)
        {
            //Felhasznalo jogosultsaganak meghatarozasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {

                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int koltsegID = int.Parse(IDArray.First());

                var model = db.Koltsegek.FirstOrDefault(x => x.ID == koltsegID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {
                    model.MunkalapID = int.Parse(IDArray.Last());
                }

                var vanKapcsolodoElem = db.Munkalapok.Where(x => x.ID == model.MunkalapID).FirstOrDefault();

                if (vanKapcsolodoElem.JovahagyvaCB == true)
                {
                    TempData["ErrorMessage"] = "Nem törölhető, mert a feladat már jóvá lett hagyva!";
                    return RedirectToAction("Details", "Munkalapok", new { @id = model.MunkalapID });
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
        public ActionResult Delete(string id, Koltsegek koltsegek)
        {
            try
            {
                string[] separator = { "_" };

                string[] IDArray = id.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                int koltsegID = int.Parse(IDArray.First());
                int munkalapId = 0;

                koltsegek = db.Koltsegek.FirstOrDefault(x => x.ID == koltsegID);

                //erre azert van szukseg, mert ha nincs FeladatID, akkor 
                //a VISSZA GOMB lenyomasakor a megfelelo helyre ugrik vissza
                if (IDArray.Count() > 1)
                {

                    koltsegek.MunkalapID = int.Parse(IDArray.Last());
                    munkalapId = int.Parse(IDArray.Last());

                }

                db.Koltsegek.Remove(koltsegek);
                db.SaveChanges();

                return RedirectToAction("Details", "Munkalapok", new { id = munkalapId });
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
