using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class HomeController : Controller
    {
        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        // GET: Home
        //[Authorize]
        public ActionResult Index(int? pn)
        {

            List<Feladatok> userFeladatokListaja = new List<Feladatok>();

            var tempUser = db.Users.FirstOrDefault(x => x.Elerhetosegek.Email == User.Identity.Name);

            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            TempData["Users"] = $"{tempUser.Jogosultsag.Nev}"; //BelepettFelhasznalo;


            var tempFeladatIDLista = db.Feladat_User_ID.Where(x => x.UserID == tempUser.UserID).ToList();

            foreach (var item in tempFeladatIDLista)
            {
                Feladatok tempFeladat = db.Feladatok.FirstOrDefault(x => x.ID == item.FeladatID);
                if (tempFeladat.JovahagyvaCB == false)
                {
                    userFeladatokListaja.Add(tempFeladat);
                }
            }

            //Varhato kezdes utan rendezi sorba
            var datumSzerintRendezettFeladatLista = userFeladatokListaja.OrderBy(x => x.VarhatoKezdes);

            return View(datumSzerintRendezettFeladatLista.ToPagedList(pn ?? 1, 10));
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
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

        // GET: Home/Create
        public ActionResult Create()
        {
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

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
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

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
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

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
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

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
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

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
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
    }
}
