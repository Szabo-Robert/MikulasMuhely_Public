using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class ElvittEszkozokController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Projekt
        public ActionResult Index(int? pn)
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                var model = db.ElvittEszkozok.OrderBy(x => x.Nev).ToList();

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
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();

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
        public ActionResult Create(ElvittEszkozok model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ElvittEszkozok.Add(model);
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
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();

            try
            {
                var model = db.ElvittEszkozok.FirstOrDefault(x => x.ID == id);

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
        public ActionResult Edit(int id, ElvittEszkozok elvittEszkozok)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.ElvittEszkozok.FirstOrDefault(x => x.ID == id);

                    model.Nev = elvittEszkozok.Nev;

                    db.ElvittEszkozok.Attach(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

                return View(elvittEszkozok);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(elvittEszkozok);
            }
        }

        // GET: Viszonyok/Delete/5
        public ActionResult Delete(int id)
        {
            //A beazonositas vegett lehet erdemes minden INDEX es GET oldal ele
            var tempUser = felhasznaloAzonositas();

            try
            {
                var model = db.ElvittEszkozok.FirstOrDefault(x => x.ID == id);


                var vanKapcsolodoElem = db.Feladat_ElvittEszkoz_ID.Where(x => x.ElvittEszkozID == id).FirstOrDefault();

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
        public ActionResult Delete(int id, ElvittEszkozok elvittEszkozok)
        {
            try
            {
                elvittEszkozok = db.ElvittEszkozok.FirstOrDefault(x => x.ID == id);

                db.ElvittEszkozok.Remove(elvittEszkozok);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
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


            //TempData.Keep();

            return tempUser;
        }

        #endregion
    }
}
