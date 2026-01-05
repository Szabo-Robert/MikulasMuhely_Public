using System;
using System.Linq;
using System.Web.Mvc;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class ViszonyokController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index()
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.Viszony.ToList();

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

        // POST: Viszonyok/Create
        [HttpPost]
        public ActionResult Create(Viszony model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Viszony.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(model);
            }
        }

        // GET: Viszonyok/Edit/5
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
                var model = db.Viszony.FirstOrDefault(x => x.ID == id);

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
        public ActionResult Edit(int id, Viszony viszony)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Viszony.FirstOrDefault(x => x.ID == id);

                    model.Nev = viszony.Nev;

                    db.Viszony.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(viszony);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(viszony);
            }
        }

        // GET: Viszonyok/Delete/5
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
                var model = db.Viszony.FirstOrDefault(x => x.ID == id);

                var vanKapcsolodoElem = db.Feladatok.Where(x => x.ViszonyID == id).FirstOrDefault();

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
        public ActionResult Delete(int id, Viszony viszony)
        {
            try
            {
                viszony = db.Viszony.FirstOrDefault(x => x.ID == id);

                db.Viszony.Remove(viszony);
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
