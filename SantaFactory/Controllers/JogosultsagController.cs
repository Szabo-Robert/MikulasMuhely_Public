using System;
using System.Linq;
using System.Web.Mvc;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class JogosultsagController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index()
        {
            //Felhasznalo jogosultsaganak beazonositasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Jogosultsag.ToList();

                return View(model);
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
            //Felhasznalo jogosultsaganak beazonositasa
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
        public ActionResult Create(Jogosultsag model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Jogosultsag.Add(model);
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
            //Felhasznalo jogosultsaganak beazonositasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Jogosultsag.FirstOrDefault(x => x.ID == id);

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
        public ActionResult Edit(int id, Jogosultsag jogosultsag)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Jogosultsag.FirstOrDefault(x => x.ID == id);

                    model.Nev = jogosultsag.Nev;

                    db.Jogosultsag.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(jogosultsag);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(jogosultsag);
            }
        }

        // GET: Viszonyok/Delete/5
        public ActionResult Delete(int id)
        {
            //Felhasznalo jogosultsaganak beazonositasa
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Jogosultsag.FirstOrDefault(x => x.ID == id);

                var vanKapcsolodoElem = db.Users.Where(x => x.JogosultsagID == id).FirstOrDefault();

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
        public ActionResult Delete(int id, Jogosultsag jogosultsag)
        {
            try
            {
                jogosultsag = db.Jogosultsag.FirstOrDefault(x => x.ID == id);

                db.Jogosultsag.Remove(jogosultsag);
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
