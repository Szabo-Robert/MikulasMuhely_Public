using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class FeladatTipusokController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index(int? pn)
        {
            //Felhasznalo beazonositasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.FeladatTipusok.ToList();

                return View(model.ToPagedList(pn ?? 1, 10));

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
        public ActionResult Create()
        {
            //Felhasznalo beazonositasa
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

        // POST: Viszonyok/Create
        [HttpPost]
        public ActionResult Create(FeladatTipusok model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.FeladatTipusok.Add(model);
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

        // GET: Viszonyok/Edit/5
        public ActionResult Edit(int id)
        {
            //Felhasznalo beazonositasa
            Users tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.FeladatTipusok.FirstOrDefault(x => x.ID == id);

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
        public ActionResult Edit(int id, FeladatTipusok feladatTipusok)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.FeladatTipusok.FirstOrDefault(x => x.ID == id);

                    model.Nev = feladatTipusok.Nev;

                    db.FeladatTipusok.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(feladatTipusok);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(feladatTipusok);
            }
        }

        // GET: Viszonyok/Delete/5
        public ActionResult Delete(int id)
        {
            //Felhasznalo beazonositasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.FeladatTipusok.FirstOrDefault(x => x.ID == id);

                var vanKapcsolodoElem = db.Feladatok.Where(x => x.FeladatTipusaID == id).FirstOrDefault();

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

        // POST: Viszonyok/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FeladatTipusok feladatTipus)
        {
            try
            {
                feladatTipus = db.FeladatTipusok.FirstOrDefault(x => x.ID == id);

                db.FeladatTipusok.Remove(feladatTipus);
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
    }
}
