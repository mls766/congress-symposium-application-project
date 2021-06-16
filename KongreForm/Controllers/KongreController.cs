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
    public class KongreController : Controller
    {
        private KongreDbEntities db = new KongreDbEntities();

        
        public ActionResult Index()
        {
            if (Convert.ToBoolean(Session["yetki"]))
            {
                return View(db.Kongres.ToList());
            }
            else
            {
                var kullanici = Convert.ToInt32(Session["kullaniciid"]);
                Yetki yetki = new Yetki();
                yetki.FKKullaniciId = kullanici;

                List<Kongre> yetkitumkongre = new List<Kongre>();

                var kongrelist = db.Yetkis.Where(x => x.FKKullaniciId == kullanici).Select(x => x.FKKongreId).ToList();

                foreach (var item in kongrelist)
                {
                    List<Kongre> kongrelist3 = db.Kongres.Where(x => x.Id == item).OrderByDescending(x => x.Id).ToList();
                    foreach (var items in kongrelist3)
                    {
                        yetkitumkongre.Add(items);
                    }

                }


                return View(yetkitumkongre);
            }
               
        }

        public ActionResult Create()
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,KongreAd,Baslangic,Bitis,AktifMi")] Kongre kongre)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index"); }

            if (ModelState.IsValid)
            {
                int sayac = 0;
                var nameexists = db.Oturums.Where(x => x.FKKongreId == kongre.Id).Select(x => x.OturumAd).ToList();

                if (nameexists.Any(x => x == kongre.KongreAd))
                {
                    ModelState.AddModelError("KongreAd", "Bu kongre ismiyle oluşturulmuş kongre bulunmaktadır.");
                    sayac++;
                }


                if (kongre.Baslangic < DateTime.Now)
                {
                    ModelState.AddModelError("Baslangic", "Geçmiş bir tarih girdiniz.");
                    sayac++;
                }
                if (kongre.Bitis < DateTime.Now)
                {
                    ModelState.AddModelError("Bitis", "Geçmiş bir tarih girdiniz.");
                    sayac++;
                }

                int result = DateTime.Compare(kongre.Baslangic, kongre.Bitis);
                if (result > 0 || result == 0)
                {
                    ModelState.AddModelError("Bitis", "Bitiş  başlangıçtan önce olamaz.");
                    sayac++;
                }
                if (sayac>0)
                {
                    return View();
                }
                else if (sayac==0)
                {
                    db.Kongres.Add(kongre);
                    db.SaveChanges();
                    return RedirectToAction("Create", "Oturum");
                }
              
            }

            return View(kongre);
        }



        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Kongre kongre = db.Kongres.Find(id);
                if (kongre == null)
                {
                    throw new Exception();
                }
                return View(kongre);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,KongreAd,Baslangic,Bitis,AktifMi")] Kongre kongre)
        {
            if (ModelState.IsValid)
            {
                int sayac = 0;
                var nameexists = db.Oturums.Where(x => x.FKKongreId == kongre.Id).Select(x => x.OturumAd).ToList();

                if (nameexists.Any(x => x == kongre.KongreAd))
                {
                    ModelState.AddModelError("KongreAd", "Bu kongre ismiyle oluşturulmuş kongre bulunmaktadır.");
                    sayac++;
                }


                if (kongre.Baslangic < DateTime.Now)
                {
                    ModelState.AddModelError("Baslangic", "Geçmiş bir tarih girdiniz.");
                    sayac++;
                }
                if (kongre.Bitis < DateTime.Now)
                {
                    ModelState.AddModelError("Bitis", "Geçmiş bir tarih girdiniz.");
                    sayac++;
                }

                int result = DateTime.Compare(kongre.Baslangic, kongre.Bitis);
                if (result > 0 || result == 0)
                {
                    ModelState.AddModelError("Bitis", "Bitiş  başlangıçtan önce olamaz.");
                    sayac++;
                }
                if (sayac > 0)
                {
                    return View();
                }
                else if (sayac == 0)
                {
                    db.Entry(kongre).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


            }
            return View(kongre);
        }

       
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Kongre kongre = db.Kongres.Find(id);
                if (kongre == null)
                {
                    throw new Exception();
                }
                return View(kongre);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var basvuru = db.Basvurus.Where(x => x.FKKongreId == id).ToList();
            var oturum = db.Oturums.Where(x => x.FKKongreId == id).ToList();
            var yetki = db.Yetkis.Where(x => x.FKKongreId == id).ToList();
            var aktifmi = db.Aktifs.Where(x => x.FKKongreId == id).ToList();
            var kongre2 = db.Kongres.Where(x => x.Id == id).FirstOrDefault();
            List<string> mesajlist = new List<string>();
            mesajlist.Add("Bu kongre silinemez! ");

            if (basvuru.Count > 0)
            {
                mesajlist.Add("Kongreye kayıtlı başvuru bulunmaktadır.");
                
            }
            if (oturum.Count > 0)
            {
                mesajlist.Add("Kongreye kayıtlı oturum bulunmaktadır.");
                
            }
             if (yetki.Count > 0)
            {
                mesajlist.Add("Kongreye kayıtlı yetki bulunmaktadır.");
                
            }
             if (aktifmi.Count > 0)
            {
                mesajlist.Add("Kongreye kayıtlı aktiflik bulunmaktadır.");
            }

            ViewBag.KongreError = String.Join(" ", mesajlist);
            if (aktifmi.Count == 0 && yetki.Count == 0 && oturum.Count == 0 && basvuru.Count == 0)
            {
                Kongre kongre = db.Kongres.Find(id);
                db.Kongres.Remove(kongre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(kongre2);
            }
           

        }

        public PartialViewResult KongreWidget()
        {
            return PartialView(db.Kongres.ToList());
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
