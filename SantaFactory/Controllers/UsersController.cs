using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using SantaFactory.Models;

namespace SantaFactory.Controllers
{
    public class UsersController : Controller
    {

        db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities();

        #region INDEX

        public ActionResult Index(int? pn)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            try
            {
                if (tempUser.Jogosultsag.Nev != "ADMIN" && tempUser.Jogosultsag.Nev != "Vezetőség")
                {
                    List<Users> modelItem = new List<Users>();
                    modelItem.Add(db.Users.FirstOrDefault(x => x.UserID == tempUser.UserID));

                    return View(modelItem.ToPagedList(pn ?? 1, 10));
                }

                List<Users> model = new List<Users>();

                model.Add(tempUser);

                model.AddRange(db.Users.Where(x => x.Elerhetosegek.Email != tempUser.Elerhetosegek.Email).OrderByDescending(x => x.JogosultsagID).ToList());

                for (int i = 0; i < model.Count(); i++)
                {
                    model[i].logUserJogosultsaga = tempUser.Jogosultsag.Nev;
                    model[i].logUserID = tempUser.UserID;
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

        #region DETAILS

        // GET: Viszonyok/Details/5
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
                Users model = db.Users.FirstOrDefault(x => x.UserID == id);
                model.ViewerUser = tempUser;

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region EDIT

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
                var model = db.Users.FirstOrDefault(x => x.UserID == id);

                //ha nem ADMIN, akkor ne is adhasson ADMIN jogosultsagot

                model.JogosultsagLista = db.Jogosultsag.ToList();

                if (tempUser.Jogosultsag.Nev != "ADMIN")
                {
                    Jogosultsag admin = model.JogosultsagLista.FirstOrDefault(x => x.Nev == "ADMIN");
                    model.JogosultsagLista.Remove(admin);
                }

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
        public ActionResult Edit(int id, Users felhasznalo)
        {
            try
            {
                var model = db.Users.FirstOrDefault(x => x.UserID == id);

                model.Nev = $"{felhasznalo.VezetekNev} {felhasznalo.KeresztNev}";
                model.VezetekNev = felhasznalo.VezetekNev;
                model.KeresztNev = felhasznalo.KeresztNev;
                model.JogosultsagID = felhasznalo.JogosultsagID;
                //model.Jelszo = felhasznalo.Jelszo;
                model.SzemIgSzam = felhasznalo.SzemIgSzam;
                model.AdoSzam = felhasznalo.AdoSzam;
                model.SzamlaSzam = felhasznalo.SzamlaSzam;

                model.AnyjaNeve = felhasznalo.AnyjaNeve;
                model.SzuletesiDatum = felhasznalo.SzuletesiDatum;
                model.GKRendszam = felhasznalo.GKRendszam;
                model.Berezes = felhasznalo.Berezes;
                model.UzemanyagAr = felhasznalo.UzemanyagAr;
                model.ViszonyID = felhasznalo.ViszonyID;

                model.Megegyezik = felhasznalo.Megegyezik;
                //model.GDPR = felhasznalo.GDPR;

                //Cimek mappolasa a CIMEK tablaban
                int tempTablaId = model.CimekID;
                var CimekModel = db.Cimek.FirstOrDefault(x => x.ID == tempTablaId);

                CimekModel.Orszag = felhasznalo.Cimek.Orszag;
                CimekModel.Varos = felhasznalo.Cimek.Varos;
                CimekModel.PostaKod = felhasznalo.Cimek.PostaKod;
                CimekModel.Utca = felhasznalo.Cimek.Utca;
                CimekModel.Szam = felhasznalo.Cimek.Szam;
                CimekModel.Egyeb = felhasznalo.Cimek.Egyeb;

                db.Cimek.Attach(CimekModel);
                db.Entry(CimekModel).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();


                //Elerhetosegek mappolasa az ELERHETOSEGEK tablaban
                tempTablaId = model.ElerhetosegekID;
                var ElerhetosegekModel = db.Elerhetosegek.FirstOrDefault(x => x.ID == tempTablaId);

                ElerhetosegekModel.Telszam1 = felhasznalo.Elerhetosegek.Telszam1;
                ElerhetosegekModel.Telszam2 = felhasznalo.Elerhetosegek.Telszam2;
                ElerhetosegekModel.Email = felhasznalo.Elerhetosegek.Email;
                ElerhetosegekModel.WEB = felhasznalo.Elerhetosegek.WEB;
                ElerhetosegekModel.Megjegyzes = felhasznalo.Elerhetosegek.Megjegyzes;

                db.Elerhetosegek.Attach(ElerhetosegekModel);
                db.Entry(ElerhetosegekModel).State = System.Data.Entity.EntityState.Modified;
                //db.SaveChanges();


                ////Ceg adatok mappolasa az CEGADATOK tablaban
                //tempTablaId = (int)model.CegAdatokID;
                //var CegAdatokModel = db.CegAdatok.FirstOrDefault(x => x.ID == tempTablaId);

                //CegAdatokModel.Nev = felhasznalo.CegAdatok.Nev;
                //CegAdatokModel.AdoSzam = felhasznalo.CegAdatok.AdoSzam;
                //CegAdatokModel.SzamlaSzam = felhasznalo.CegAdatok.SzamlaSzam;
                //CegAdatokModel.Meghatalmazott = felhasznalo.CegAdatok.Meghatalmazott;

                //db.CegAdatok.Attach(CegAdatokModel);
                //db.Entry(CegAdatokModel).State = System.Data.Entity.EntityState.Modified;
                ////db.SaveChanges();


                ////Ceg cimenek mappolasa az CEGADATOK.CIMEK tablaban
                //tempTablaId = (int)model.CegAdatok.CimID;
                //var CegAdatokCimeModel = db.Cimek.FirstOrDefault(x => x.ID == tempTablaId);

                //CegAdatokCimeModel.Orszag = felhasznalo.CegAdatok.Cimek.Orszag;
                //CegAdatokCimeModel.Varos = felhasznalo.CegAdatok.Cimek.Varos;
                //CegAdatokCimeModel.Utca = felhasznalo.CegAdatok.Cimek.Utca;
                //CegAdatokCimeModel.Szam = felhasznalo.CegAdatok.Cimek.Szam;
                //CegAdatokCimeModel.Egyeb = felhasznalo.CegAdatok.Cimek.Egyeb;

                //db.Cimek.Attach(CegAdatokCimeModel);
                //db.Entry(CegAdatokCimeModel).State = System.Data.Entity.EntityState.Modified;
                ////db.SaveChanges();


                ////Ceg elerhetosegeinek mappolasa az CEGADATOK.CIMEK tablaban
                //tempTablaId = (int)model.CegAdatok.ElerhetosegekID;
                //var CegAdatokElerhetosegeModel = db.Elerhetosegek.FirstOrDefault(x => x.ID == tempTablaId);

                //CegAdatokElerhetosegeModel.Telszam1 = felhasznalo.CegAdatok.Elerhetosegek.Telszam1;
                //CegAdatokElerhetosegeModel.Telszam2 = felhasznalo.CegAdatok.Elerhetosegek.Telszam2;
                //CegAdatokElerhetosegeModel.Email = felhasznalo.CegAdatok.Elerhetosegek.Email;
                //CegAdatokElerhetosegeModel.WEB = felhasznalo.CegAdatok.Elerhetosegek.WEB;
                //CegAdatokElerhetosegeModel.Megjegyzes = felhasznalo.CegAdatok.Elerhetosegek.Megjegyzes;

                //db.Elerhetosegek.Attach(CegAdatokElerhetosegeModel);
                //db.Entry(CegAdatokElerhetosegeModel).State = System.Data.Entity.EntityState.Modified;
                ////db.SaveChanges();

                ////ha valamikor elmaradt volna az ertekadas!!! FONTOS!
                //model.CegAdatok.IsUser = true;

                #region //Password hashing

                //model.Jelszo = Varazslas.Hash(felhasznalo.Jelszo);
                //model.JelszoMegerosites = Varazslas.Hash(felhasznalo.JelszoMegerosites);

                #endregion

                db.Configuration.ValidateOnSaveEnabled = false;

                db.Users.Attach(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                felhasznalo.JogosultsagLista = db.Jogosultsag.ToList();
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(felhasznalo);
            }
        }

        #endregion

        #region DELETE

        // GET: Projekt/Delete/5
        public ActionResult Delete(int id)
        {
            //Felhasznalo jogosultsaga
            Users tempUser = felhasznaloAzonositas();
            if (tempUser == null)
            {
                return RedirectToAction("Bejelentkezes", "Users");
            }

            if (tempUser.UserID == id)
            {
                TempData["ErrorMessage"] = "Önmagát nem tudja törölni!";
                return RedirectToAction("Index");
            }

            try
            {
                var model = db.Users.FirstOrDefault(x => x.UserID == id);

                var vanKapcsolodoElem = db.Feladat_User_ID.FirstOrDefault(x => x.UserID == id);

                if (vanKapcsolodoElem != null)
                {
                    TempData["ErrorMessage"] = "Nem törölhető, mert valamihez hozzá van rendelve!";
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;/*"Valami hiba történt, kérem ellenőrizze az adatokat!";*/
                return RedirectToAction("Index");
            }
        }

        // POST: Projekt/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Users model)
        {
            try
            {
                model = db.Users.FirstOrDefault(x => x.UserID == id);

                db.Users.Remove(model);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View();
            }
        }

        #endregion

        #region // Registration Action

        [HttpGet]
        public ActionResult Registration()
        {
            try
            {
                Users tempUser = felhasznaloAzonositas();

                Users model = new Users();
                return View(model);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        // Registration Post action
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Registration([Bind(Exclude = "EmailMegerositve, AktivaloKod")] Users felhasznalo)
        {
            try
            {
                bool Status = false;
                string message = "";

                //Model Validation
                if (ModelState.IsValid)
                {
                    #region //Email is already Exist

                    var MarRogzitettEmail = AzEmailMarRogzitett(felhasznalo.Elerhetosegek.Email);

                    if (MarRogzitettEmail)
                    {
                        ModelState.AddModelError("EmailMarLetezik", "Az e-mail cím már regisztrálva lett! Adjon meg másikat és írja be ismét a jelszót!");
                        return View(felhasznalo);
                    }
                    #endregion

                    if (!(felhasznalo.GDPR))
                    {

                        TempData["ErrorMessage"] = "Kötelezően el kell fogadni a GDPR feltételeket és írja be ismét a jelszót!";
                        return View(felhasznalo);
                    }

                    #region // Generate Activation Code

                    felhasznalo.AktivaloKod = Guid.NewGuid();

                    #endregion

                    #region //Password hashing

                    felhasznalo.Jelszo = Varazslas.Hash(felhasznalo.Jelszo);
                    felhasznalo.JelszoMegerosites = Varazslas.Hash(felhasznalo.JelszoMegerosites);

                    #endregion

                    felhasznalo.EmailMegerositve = false;

                    #region //Save database

                    using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
                    {
                        try
                        {
                            #region Send Email to User

                            felhasznalo.JogosultsagID = 5;

                            EllenorzoLinkKuldese(felhasznalo.Elerhetosegek.Email, felhasznalo.AktivaloKod.ToString());
                            message = "A regisztrálás sikeres volt! A fiókot aktiváló link" +
                                " el lett küldve az Ön e-mail címére: " + felhasznalo.Elerhetosegek.Email;

                            db.Users.Add(felhasznalo);
                            db.SaveChanges();

                            Status = true;

                            #endregion
                        }
                        catch (Exception e)
                        {
                            TempData["ErrorMessage"] = e.Message;/*"Valami hiba történt, kérem ellenőrizze az adatokat!";*/
                            return View(felhasznalo);
                        }
                    }

                    #endregion
                }
                else
                {
                    message = "Érvénytelen parancs!";
                }

                ViewBag.Message = message;
                ViewBag.Status = Status;

                return View(felhasznalo);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message/*"Valami hiba történt, kérem ellenőrizze az adatokat!"*/;
                return View(felhasznalo);
            }
        }

        #endregion

        #region //Verify Account

        [HttpGet]
        public ActionResult FiokEllenorzese(string id)
        {
            try
            {
                bool Status = false;

                using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
                {
                    db.Configuration.ValidateOnSaveEnabled = false;

                    var AktivaloKodEgyezes = db.Users.Where(x => x.AktivaloKod == new Guid(id)).FirstOrDefault();

                    if (AktivaloKodEgyezes != null)
                    {
                        AktivaloKodEgyezes.EmailMegerositve = true;
                        db.SaveChanges();
                        Status = true;
                    }
                    else
                    {
                        ViewBag.Message = "Érvénytelen parancs!";
                    }

                }

                ViewBag.Status = Status;
                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion

        #region // LOGIN

        [HttpGet]
        public ActionResult Bejelentkezes()
        {
            try
            {
                FormsAuthentication.SignOut();

                TempData["Users"] = "";

                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }


        #endregion

        #region // LOGIN Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bejelentkezes(LoginModel belep, string ReturnUrl)
        {
            try
            {
                bool Status = false;
                string message = "";

                using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
                {
                    Users BelepettFelhasznalo = new Users();

                    BelepettFelhasznalo = db.Users.Where(x => x.Elerhetosegek.Email == belep.Email).FirstOrDefault();

                    #region SUPER ADMIN


                    if (BelepettFelhasznalo == null && belep.Email == "szrcs@xxx.com")
                    {
                        //MIVEL NEM BIZTOS, hogy letezik RECORD, ezert letrehozom MAGAM :P

                        BelepettFelhasznalo = new Users();

                        BelepettFelhasznalo.VezetekNev = "X";
                        BelepettFelhasznalo.KeresztNev = "Y";
                        BelepettFelhasznalo.Nev = $"{BelepettFelhasznalo.VezetekNev} {BelepettFelhasznalo.KeresztNev}";

                        BelepettFelhasznalo.Jelszo = "Jh1bszEPyNajnLCp13hUoSePihk7jI5wx6siS2AOgFU=";
                        BelepettFelhasznalo.JelszoMegerosites = "Jh1bszEPyNajnLCp13hUoSePihk7jI5wx6siS2AOgFU=";

                        BelepettFelhasznalo.SzuletesiDatum = new DateTime(2000, 01, 01);
                        BelepettFelhasznalo.SzuletesiHely = "World";
                        BelepettFelhasznalo.SzemIgSzam = "AB123456";
                        BelepettFelhasznalo.Megegyezik = true;


                        BelepettFelhasznalo.EmailMegerositve = true;
                        BelepettFelhasznalo.AktivaloKod = new Guid();
                        BelepettFelhasznalo.GDPR = true;


                        //Cimek letrehozasa a CIMEK tablaban
                        Cimek CimekModel = new Cimek();

                        CimekModel.Orszag = "x";
                        CimekModel.Varos = "y";

                        db.Cimek.Add(CimekModel);
                        db.SaveChanges();

                        BelepettFelhasznalo.CimekID = CimekModel.ID;

                        //Elerhetosegek letrehozasa az ELERHETOSEGEK tablaban
                        Elerhetosegek ElerhetosegekModel = new Elerhetosegek();

                        ElerhetosegekModel.Telszam1 = "+3670-000-0001";

                        ElerhetosegekModel.Email = $"{belep.Email}";


                        db.Elerhetosegek.Add(ElerhetosegekModel);
                        db.SaveChanges();

                        BelepettFelhasznalo.ElerhetosegekID = ElerhetosegekModel.ID;
                        BelepettFelhasznalo.ErtSzemelyElerhetosegID = ElerhetosegekModel.ID;

                        //JOGOSULTSAG letrehozasa az jogosultsag tablaban

                        Jogosultsag jogosultsagModel = new Jogosultsag();

                        jogosultsagModel.Nev = "ADMIN";

                        db.Jogosultsag.Add(jogosultsagModel);
                        db.SaveChanges();

                        BelepettFelhasznalo.JogosultsagID = jogosultsagModel.ID;

                        //JOGOSULTSAG letrehozasa az jogosultsag tablaban
                        Viszony viszonyModel = new Viszony();

                        viszonyModel.Nev = "SZUPER";

                        db.Viszony.Add(viszonyModel);
                        db.SaveChanges();

                        BelepettFelhasznalo.ViszonyID = viszonyModel.ID;

                        db.Users.Add(BelepettFelhasznalo);
                        db.SaveChanges();
                    }

                    #endregion

                    if (BelepettFelhasznalo != null)
                    {
                        if (string.Compare(Varazslas.Hash(belep.Jelszo), BelepettFelhasznalo.Jelszo) == 0 && BelepettFelhasznalo.EmailMegerositve)
                        {
                            //tovabbadni a user jogosultsagat  
                            TempData["Users"] = $"{BelepettFelhasznalo.Jogosultsag.Nev}";
                            TempData.Keep();

                            int timeout = belep.BelepveMarad ? 43200 : 20; //525600 min = 1 year
                            var ticket = new FormsAuthenticationTicket(belep.Email, belep.BelepveMarad, timeout);
                            string encryted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToActionPermanent("Index", "Home");
                            }
                        }
                        else
                        {
                            message = "Érvénytelen belépés!";
                        }
                    }
                    else
                    {
                        message = "Érvénytelen belépés!";
                    }

                }

                ViewBag.Message = message;
                ViewBag.Status = Status;

                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message/*"Valami hiba történt, kérem ellenőrizze az adatokat!"*/;
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region // LOGout

        [Authorize]
        [HttpPost]
        public ActionResult Kijelentkezes()
        {
            try
            {
                FormsAuthentication.SignOut();

                TempData["Users"] = "";

                return RedirectToAction("Bejelentkezes", "Users");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region // Forgot Password
        public ActionResult JelszoModositas()
        {
            try
            {
                FormsAuthentication.SignOut();

                TempData["Users"] = "";

                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult JelszoModositas(string EmailCim)
        {
            try
            {

                bool Status = false;
                string message = "";

                using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
                {
                    var fiok = db.Users.Where(x => x.Elerhetosegek.Email == EmailCim).FirstOrDefault();
                    if (fiok != null)
                    {
                        //send email for reset password
                        string jelszomodositoKod = Guid.NewGuid().ToString();
                        EllenorzoLinkKuldese(fiok.Elerhetosegek.Email, jelszomodositoKod, "JelszoReszet");
                        fiok.JelszoModositoKod = jelszomodositoKod;

                        //Az uj jelszo nem egyezik meg a regivel, ezert deaktivalni kell a beallitast
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();

                        message = "A jelszó módosító linket a megadott címre elküldtük!";
                        Status = true;
                    }
                    else
                    {
                        message = "A fiók nincs regisztrálva!";
                    }
                }

                ViewBag.Message = message;
                ViewBag.Status = Status;

                return View();
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult JelszoReszet(string id)
        {
            JelszoReszetModel model = new JelszoReszetModel();

            try
            {
                //A jelszó link ellenörzése
                //Megtalálni a linkhez tartozó fiókot
                //Átirányítani a jelszó visszaállítás oldalra

                using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
                {
                    var fiok = db.Users.Where(x => x.JelszoModositoKod == id).FirstOrDefault();
                    if (fiok != null)
                    {

                        model.JelszoReszetKod = id;
                        return View(model);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(model);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JelszoReszet(JelszoReszetModel model)
        {
            try
            {
                bool Status = false;
                string message = "";

                if (ModelState.IsValid)
                {
                    using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
                    {
                        var fiok = db.Users.Where(x => x.JelszoModositoKod == model.JelszoReszetKod).FirstOrDefault();
                        if (fiok != null)
                        {

                            fiok.Jelszo = Varazslas.Hash(model.UjJelszo);
                            fiok.JelszoModositoKod = ""; //ugyan azt az e-mailt ne tudja tobbszor hasznalni jelszomodositasra

                            db.Configuration.ValidateOnSaveEnabled = false;
                            db.SaveChanges();

                            message = "Az új jelszó sikeresen módosult!";
                            Status = true;
                        }

                    }
                }
                else
                {
                    message = "Valami helytelen!";
                }


                ViewBag.Message = message;
                ViewBag.Status = Status;

                return View(model);

            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Valami hiba történt, kérem ellenőrizze az adatokat!";
                return View(model);
            }
        }

        #endregion



        #region //A megadott e-mail cim mar rogzitve van metodus

        [NonAction]
        public bool AzEmailMarRogzitett(string emailID)
        {
            using (db_a6b688_sf2025Entities db = new db_a6b688_sf2025Entities())
            {
                var AzonosEmailcim = db.Users.Where(x => x.Elerhetosegek.Email == emailID).FirstOrDefault();

                return AzonosEmailcim == null ? false : true;
            }
        }

        #endregion


        #region //Ellenorzo email kuldese metodus

        [NonAction]
        public void EllenorzoLinkKuldese(string emailCim, string aktivaloKod, string emailCimzett = "FiokEllenorzese")
        {
            var verifyUrl = "/Users/" + emailCimzett + "/" + aktivaloKod;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var felado = new MailAddress("valtoztatni.akarok@gmail.com", "Miki csapata", System.Text.Encoding.UTF8);
            var cimzett = new MailAddress(emailCim);

            string tema = "";
            string body = "";
            if (emailCimzett == "FiokEllenorzese")
            {
                tema = "Az Ön felhasználója létrejött!";

                body = "<br/><br/> Örömünkre szolgál jelezni, hogy az Ön fiókja " +
                    "sikeresen létrejött. " +
                    "<br/><br/>A létrehozás befejezéséhez kérjük kattintson <a href='" + link + "'>IDE.</a>" +
                    "<br/><br/>Amennyiben a fentebbi utasítás nem működik a lentebbi " +
                    "linket másolja be a böngészőbe:" +
                    "<br/><a href='" + link + "'>" + link + "</a>" +
                    "<br/><br/> Köszönjük, hogy minket választott és várjuk a megerősítését!" +
                    "<br/><br/> Üdvözlettel a Mikulás műhely csapata!";
            }
            else
            {
                tema = "Jelszó módosítás";
                body = "<br/><br/> Üdvözöljük!" +
                    "<br/> Elfelejtett jelszó miatt jelszó módosítási kérelmet kaptunk. " +
                    "<br/><br/>A módosításához kérjük kattintson" +
                    "<a href='" + link + "'> ide. </a>" +
                    "<br/><br/>Amennyiben a fentebbi utasítás nem működik a lentebbi " +
                    "linket másolja be a böngészőbe:" +
                    "<br/><a href='" + link + "'>" + link + "</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true, // Ez fontos a Gmailhez!
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                // Itt a jelszó helyére a generált 16 jegyű kódot írd!
                Credentials = new NetworkCredential(felado.Address, "sfhtlonxftezntsr")
            };

            using (var uzenet = new MailMessage(felado, cimzett)
            {
                Subject = tema,
                Body = body,
                IsBodyHtml = true,
                // NAGYON FONTOS: Kódolás beállítása az ékezetek miatt
                SubjectEncoding = System.Text.Encoding.UTF8,
                BodyEncoding = System.Text.Encoding.UTF8
            })

                smtp.Send(uzenet);
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

    }


}