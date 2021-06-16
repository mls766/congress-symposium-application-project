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
    public class OturumController : Controller
    {
        private KongreDbEntities db = new KongreDbEntities();

        public ActionResult Index()
        {
            if (Convert.ToBoolean(Session["yetki"]))
            {
                var oturums = db.Oturums.Include(o => o.Kongre);
                return View(oturums.ToList());
            }
            else
            {
                var kullanici = Convert.ToInt32(Session["kullaniciid"]);
                Yetki yetki = new Yetki();
                yetki.FKKullaniciId = kullanici;

                var kongrelist = db.Yetkis.Where(x => x.FKKullaniciId == kullanici).Select(x => x.FKKongreId).ToList();
                List<Oturum> olist = new List<Oturum>();
                foreach (var item in kongrelist)
                {
                    var a = db.Oturums.Where(x => x.FKKongreId == item).ToList();
                    foreach (var item2 in a)
                    {
                        olist.Add(item2);
                    }

                }


                return View(olist);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oturum oturum = db.Oturums.Find(id);
            if (oturum == null)
            {
                return HttpNotFound();
            }
            return View(oturum);
        }

        public ActionResult Create()
        {
            //Admin etkisindeyse tüm kongrelere oturum ekleme
            if (Convert.ToBoolean(Session["yetki"]))
            {
                ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd");
                return View(/*oturums.ToList()*/);
            }
            //yetkili olduğu kongrelere oturum ekleme
            else
            {

                var kullanici = Convert.ToInt32(Session["kullaniciid"]);
                Yetki yetki = new Yetki();
                yetki.FKKullaniciId = kullanici;

                var kongreIdlist = db.Yetkis.Where(x => x.FKKullaniciId == kullanici).Select(x => x.FKKongreId).ToList();
                List<Kongre> kongreList = new List<Kongre>();
                if (kongreIdlist != null)
                {
                    foreach (var item in kongreIdlist)
                    {
                        var a = db.Kongres.Where(x => x.Id == item).ToList();
                        foreach (var item2 in a)
                        {
                            kongreList.Add(item2);
                        }

                    }
                    ViewBag.FKKongreId = new SelectList(kongreList, "Id", "KongreAd");
                    return View();
                }

            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FKKongreId,OturumAd,OturumBaslangic,OturumBitis")] Oturum oturum)
        {
            if (ModelState.IsValid)
            {
                var nameexists = db.Oturums.Where(x => x.FKKongreId == oturum.FKKongreId).Select(x => x.OturumAd).ToList();

                int sayac = 0;
                if (nameexists.Any(x => x == oturum.OturumAd))
                {
                    ModelState.AddModelError("OturumAd", "Bu kongre ismiyle oluşturulmuş oturum bulunmaktadır.");
                    sayac++;
                }


                var kongreid = db.Kongres.Where(x => x.Id == oturum.FKKongreId).FirstOrDefault();

                int rbaslangic = DateTime.Compare(kongreid.Baslangic, oturum.OturumBaslangic);
                if (rbaslangic > 0)
                {
                    ModelState.AddModelError("OturumBaslangic", "Oturum baslangıc zamanı kongrenin baslangıc zamanından once olamaz.");
                    sayac++;
                }

                int rbitis = DateTime.Compare(kongreid.Bitis, oturum.OturumBitis);
                if (rbitis < 0)
                {
                    ModelState.AddModelError("OturumBitis", "Oturum bitis zamanı kongrenin bitis zamanından sonra olamaz.");
                    sayac++;
                }
                int result = DateTime.Compare(oturum.OturumBaslangic, oturum.OturumBitis);
                if (result > 0 || result == 0)
                {
                    ModelState.AddModelError("OturumBitis", "Oturun bitişi oturum başlangıcından önce olamaz.");
                    sayac++;
                }
                int result2 = DateTime.Compare(oturum.OturumBaslangic, kongreid.Bitis);
                if (result2 > 0 || result == 0)
                {
                    ModelState.AddModelError("OturumBaslangic", "Oturum başlangıcı kongre bitişinden sonra olamaz.");
                    sayac++;
                }

                if (sayac > 0)
                {
                    ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", oturum.FKKongreId);

                    return View(oturum);
                }
                else if (sayac == 0)
                {
                    db.Oturums.Add(oturum);
                    db.SaveChanges();
                    TempData["mesaj"] = "Oturum başarıyla oluşturuldu!";
                    ModelState.Clear();

                    return RedirectToAction("Create");
                }

            }

            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", oturum.FKKongreId);
            return View(oturum);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oturum oturum = db.Oturums.Find(id);
            if (oturum == null)
            {
                return HttpNotFound();
            }

            var oturumId = db.Oturums.Where(x => x.Id == id).Select(x => x.FKKongreId).FirstOrDefault();
            var kongrelist = db.Kongres.Where(x => x.Id == oturumId).ToList();


            if (kongrelist != null)
            {
                ViewBag.FKKongreId = new SelectList(kongrelist, "Id", "KongreAd");
            }

            return View(oturum);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FKKongreId,OturumAd,OturumBaslangic,OturumBitis")] Oturum oturum)
        {
            var oturumId = db.Oturums.Where(x => x.Id == oturum.Id).Select(x => x.FKKongreId).FirstOrDefault();
            var kongrelist = db.Kongres.Where(x => x.Id == oturumId).ToList();

            if (ModelState.IsValid)
            {
                int sayac = 0;
                var kongreid = db.Kongres.Where(x => x.Id == oturum.FKKongreId).FirstOrDefault();

                int rbaslangic = DateTime.Compare(kongreid.Baslangic, oturum.OturumBaslangic);
                if (rbaslangic > 0)
                {
                    ModelState.AddModelError("OturumBaslangic", "Oturum başlangıc zamanı kongrenin baslangıc zamanından once olamaz.");
                    sayac++;
                }

                int rbitis = DateTime.Compare(kongreid.Bitis, oturum.OturumBitis);
                if (rbitis < 0)
                {
                    ModelState.AddModelError("OturumBitis", "Oturum bitis zamanı kongrenin bitis zamanından sonra olamaz.");
                    sayac++;
                }
                int result = DateTime.Compare(oturum.OturumBaslangic, oturum.OturumBitis);
                if (result > 0 || result == 0)
                {
                    ModelState.AddModelError("OturumBitis", "Bitiş  başlangıçtan önce olamaz.");
                    sayac++;
                }

                if (sayac > 0)
                {
                    ViewBag.FKKongreId = new SelectList(kongrelist, "Id", "KongreAd", oturum.FKKongreId);
                    return View(oturum);
                }
                else if (sayac == 0)
                {
                    db.Entry(oturum).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.FKKongreId = new SelectList(kongrelist, "Id", "KongreAd", oturum.FKKongreId);
            return View(oturum);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Oturum oturum = db.Oturums.Find(id);
            if (oturum == null)
            {
                return HttpNotFound();
            }
            return View(oturum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Oturum oturum = db.Oturums.Find(id);

            var basvuru = db.BasvuruDetays.Where(x => x.FKOturumId == id).ToList();
          
            if (basvuru.Count > 0)
            {
               ViewBag.OturumError = "Bu oturum silinemez. Oturuma kayıtlı başvuru bulunmaktadır.";
                return View(oturum);
                    
            }
            else
            {
                db.Oturums.Remove(oturum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           
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
