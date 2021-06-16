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
    public class YetkiController : Controller
    {
        private KongreDbEntities db = new KongreDbEntities();

        public ActionResult Index()
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index","Basvuru"); }
            var yetkis = db.Yetkis.Include(y => y.Kongre).Include(y => y.Kullanici);
            return View(yetkis.ToList());
        }

        public ActionResult Create()
        {

            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd");
            ViewBag.FKKullaniciId = new SelectList(db.Kullanicis, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FKKongreId,FKKullaniciId")] Yetki yetki)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            if (ModelState.IsValid)
            {
                db.Yetkis.Add(yetki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", yetki.FKKongreId);
            ViewBag.FKKullaniciId = new SelectList(db.Kullanicis, "Id", "Email", yetki.FKKullaniciId);
            return View(yetki);
        }

        public ActionResult Edit(int? id)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Yetki yetki = db.Yetkis.Find(id);
                if (yetki == null)
                {
                    throw new Exception();
                }
                ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", yetki.FKKongreId);
                ViewBag.FKKullaniciId = new SelectList(db.Kullanicis, "Id", "Email", yetki.FKKullaniciId);
                return View(yetki);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FKKongreId,FKKullaniciId")] Yetki yetki)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            if (ModelState.IsValid)
            {
                db.Entry(yetki).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FKKongreId = new SelectList(db.Kongres, "Id", "KongreAd", yetki.FKKongreId);
            ViewBag.FKKullaniciId = new SelectList(db.Kullanicis, "Id", "Email", yetki.FKKullaniciId);
            return View(yetki);
        }

        public ActionResult Delete(int? id)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Yetki yetki = db.Yetkis.Find(id);
                if (yetki == null)
                {
                    throw new Exception();
                }
                return View(yetki);
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
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            Yetki yetki = db.Yetkis.Find(id);
            db.Yetkis.Remove(yetki);
            db.SaveChanges();
            return RedirectToAction("Index");
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
