using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KongreForm.Models;

namespace KongreForm.Controllers
{
    public class AktifController : Controller
    {
        private KongreDbEntities db = new KongreDbEntities();

        // GET: Aktifs
        public ActionResult Index()
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index"); }
            var aktifs = db.Aktifs.Include(a => a.Kongre);
            return View(aktifs.ToList());
        }

        public PartialViewResult GetAktifAlan(int KongreId)
        {

            var aktiflist = new List<AktifCheck>()
            {
                new AktifCheck(){ Id = 1, Icerik = "Ad" , IsChecked = false },
                new AktifCheck(){ Id = 2, Icerik = "Soyad" , IsChecked = false },
                new AktifCheck(){ Id = 3, Icerik = "Email" , IsChecked = false },
                new AktifCheck(){ Id = 4, Icerik = "TcNo" , IsChecked = false },
                new AktifCheck(){ Id = 5, Icerik = "Kurum" , IsChecked = false },
                new AktifCheck(){ Id = 6, Icerik = "Meslek" , IsChecked = false },
                new AktifCheck(){ Id = 7, Icerik = "Egitim" , IsChecked = false },
                new AktifCheck(){ Id = 8, Icerik = "Telefon" , IsChecked = false },
                new AktifCheck(){ Id = 9, Icerik = "Universite" , IsChecked = false },
                new AktifCheck(){ Id = 10, Icerik = "AnaDal" , IsChecked = false },
                new AktifCheck(){ Id = 11, Icerik = "Adres" , IsChecked = false },
                new AktifCheck(){ Id = 12, Icerik = "Fakulte" , IsChecked = false },
                new AktifCheck(){ Id = 13, Icerik = "Bolum" , IsChecked = false },
                new AktifCheck(){ Id = 14, Icerik = "SunumTipi" , IsChecked = false },
                new AktifCheck(){ Id = 15, Icerik = "SunucuMu" , IsChecked = false },
                new AktifCheck(){ Id = 16, Icerik = "SunumBaslik" , IsChecked = false },
                new AktifCheck(){ Id = 17, Icerik = "BildiriBaslik" , IsChecked = false },
                new AktifCheck(){ Id = 18, Icerik = "PosterBaslik" , IsChecked = false },
                new AktifCheck(){ Id = 19, Icerik = "Ozet" , IsChecked = false },
                new AktifCheck(){ Id = 20, Icerik = "PosterMi" , IsChecked = false },
                new AktifCheck(){ Id = 21, Icerik = "Dosya" , IsChecked = false },


            };

            KongreDbEntities db = new KongreDbEntities();

            BasvuruViewModel model = new BasvuruViewModel();

            model.FKKongreId = KongreId;

            var aktifs = db.Aktifs.Where(x => x.FKKongreId == KongreId).Select(x => x.AktifAlanId).ToList();

            List<string> aktifler = new List<string>();

            foreach (var item in aktiflist)
            {
                
                if (aktifs.Contains(item.Id))
                {
                    aktifler.Add(item.Icerik);
                    
                }
            }
            ViewBag.aktiflist = aktifler;


            return PartialView("_GetAktifAlan", ViewBag.aktiflist);
        }

        // GET: Aktifs/Create
        public ActionResult Create()
        {

            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index"); }
            var aktif = new List<AktifCheck>()
            {
                new AktifCheck(){ Id = 1, Icerik = "Ad" , IsChecked = false },
                new AktifCheck(){ Id = 2, Icerik = "Soyad" , IsChecked = false },
                new AktifCheck(){ Id = 3, Icerik = "Email" , IsChecked = false },
                new AktifCheck(){ Id = 4, Icerik = "TcNo" , IsChecked = false },
                new AktifCheck(){ Id = 5, Icerik = "Kurum" , IsChecked = false },
                new AktifCheck(){ Id = 6, Icerik = "Meslek" , IsChecked = false },
                new AktifCheck(){ Id = 7, Icerik = "Egitim" , IsChecked = false },
                new AktifCheck(){ Id = 8, Icerik = "Telefon" , IsChecked = false },
                new AktifCheck(){ Id = 9, Icerik = "Universite" , IsChecked = false },
                new AktifCheck(){ Id = 10, Icerik = "AnaDal" , IsChecked = false },
                new AktifCheck(){ Id = 11, Icerik = "Adres" , IsChecked = false },
                new AktifCheck(){ Id = 12, Icerik = "Fakulte" , IsChecked = false },
                new AktifCheck(){ Id = 13, Icerik = "Bolum" , IsChecked = false },
                new AktifCheck(){ Id = 14, Icerik = "SunumTipi" , IsChecked = false },
                new AktifCheck(){ Id = 15, Icerik = "SunucuMu" , IsChecked = false },
                new AktifCheck(){ Id = 16, Icerik = "SunumBaslik" , IsChecked = false },
                new AktifCheck(){ Id = 17, Icerik = "BildiriBaslik" , IsChecked = false },
                new AktifCheck(){ Id = 18, Icerik = "PosterBaslik" , IsChecked = false },
                new AktifCheck(){ Id = 19, Icerik = "Ozet" , IsChecked = false },
                new AktifCheck(){ Id = 20, Icerik = "PosterMi" , IsChecked = false },
                new AktifCheck(){ Id = 21, Icerik = "Dosya" , IsChecked = false },


            };

            AktifViewModel model = new AktifViewModel();
            model.AktifItems = aktif;



            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd");
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AktifViewModel model)
        {
            if (ModelState.IsValid)
            {


                foreach (var item in model.AktifItems)
                {
                    if (item.IsChecked == true)
                    {
                        Aktif a = new Aktif();
                        a.FKKongreId = model.FKKongreId;
                        a.AktifAlanId = item.Id;
                        db.Aktifs.Add(a);
                        db.SaveChanges();

                    }

                }

                return RedirectToAction("Index");

            }
            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", model.FKKongreId);
            return View(model);
        }

       
        public ActionResult Edit(int? id)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index"); }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aktif aktif = db.Aktifs.Find(id);
            if (aktif == null)
            {
                return HttpNotFound();
            }
            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", aktif.FKKongreId);
            return View(aktif);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FKKongreId,AktifCheck")] Aktif aktif)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aktif).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", aktif.FKKongreId);
            return View(aktif);
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


