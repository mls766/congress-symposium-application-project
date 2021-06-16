using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KongreForm.Models;

namespace KongreForm.Controllers
{
    public class KullaniciController : Controller
    {
        private KongreDbEntities db = new KongreDbEntities();

        public ActionResult Index()
        {
            if (Session["kullaniciid"] == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            return View(db.Kullanicis.ToList());
        }

        public ActionResult Giris()
        {
            return View();
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {


            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        [HttpPost]
        public ActionResult Giris(Kullanici kullanici)
        {

            if (!string.IsNullOrEmpty(kullanici.Email) && !string.IsNullOrEmpty(kullanici.Parola))
            {

                var k = db.Kullanicis.Where(x => x.Email == kullanici.Email).SingleOrDefault();
                if(k != null)
                { 
                    MD5 md5Hash = MD5.Create();
                    var hashed = GetMd5Hash(md5Hash,kullanici.Parola);
                    if (k.Email == kullanici.Email && k.Parola == hashed)

                    {
                        Session["kullaniciid"] = k.Id;
                        Session["Email"] = k.Email;
                        Session["yetki"] = k.AdminMi;
                        Session["kullaniciadi"] = k.Ad + " " + k.Soyad;
                        return RedirectToAction("Index", "Basvuru");
                    }
                    else
                    {
                        ViewBag.uyari = " Kullanıcı adı ya da sifre yanlıs";
                        return View(kullanici);
                    }
                }
                else
                {
                    ViewBag.uyari = " Böyle bir kullanıcı bulunamamaktadır.";
                    return View(kullanici);
                }
            }
            ViewBag.BosAlanUyari = "Lütfen bos alan bırakmayın!";
            return View();
        }

        public ActionResult Cikis()
        {
            Session["kullaniciid"] = null;
            Session["Email"] = null;
            Session.Abandon();
            return RedirectToAction("Giris", "Kullanici");

        }

        public ActionResult Create()
        {
            if (Session["kullaniciid"] == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KullaniciViewModel model)
        {
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            if (ModelState.IsValid)
            {
                try
                {
                    Kullanici k = new Kullanici();
                    k.Email = model.Email;
                    MD5 md5Hash = MD5.Create();
                    k.Parola = GetMd5Hash(md5Hash,model.Parola);
                    k.Ad = model.Ad;
                    k.Soyad = model.Soyad;
                    k.AdminMi = model.AdminMi;

                    var KullaniciAra = db.Kullanicis.Where(x => x.Email == model.Email).SingleOrDefault();
                    if (KullaniciAra != null)
                    {
                        throw new Exception("Mevcut kullanıcı bilgilerini girdiniz.");
                    }
                    db.Kullanicis.Add(k);
                    db.SaveChanges();
                    ModelState.Clear();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    ViewBag.KullaniciMevcutUyari = "Mevcut kullanıcı bilgilerini girdiniz.";
                    return View();
                }
            }
            return View(model);

        }



        public ActionResult Edit(int? id)
        {
            if (Session["kullaniciid"] == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                var k = db.Kullanicis.Where(x => x.Id == id).SingleOrDefault();

                if (k == null)
                {
                    throw new Exception();
                }
                return View(k);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int Id, string Parola, string Ad, string Soyad, bool AdminMi, string Email, Kullanici kullanici)
        {

            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            if (ModelState.IsValid)
            {
                var k = db.Kullanicis.Where(x => x.Id == Id).SingleOrDefault();
                MD5 md5Hash = MD5.Create();
                k.Parola = GetMd5Hash(md5Hash, Parola);
                k.Ad = kullanici.Ad;
                k.Soyad = kullanici.Soyad;
                k.Email = kullanici.Email;
                k.AdminMi = kullanici.AdminMi;
                db.Entry(k).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kullanici);

        }


        // GET: Kullanici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["kullaniciid"] == null)
            {
                return RedirectToAction("Giris", "Kullanici");
            }
            if (!Convert.ToBoolean(Session["yetki"])) { return RedirectToAction("Index", "Basvuru"); }

            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Kullanici kullanici = db.Kullanicis.Find(id);
                if (kullanici == null)
                {
                    throw new Exception();
                }
                return View(kullanici);
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

            Kullanici kullanici = db.Kullanicis.Find(id);
            db.Kullanicis.Remove(kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SifremiUnuttum()
        {

            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(string email)
        {
            var mail = db.Kullanicis.Where(x => x.Email == email).FirstOrDefault();

            if (mail != null)
            {
                Random rnd = new Random();
                string yenisifre = Convert.ToString(rnd.Next());

                Kullanici k = new Kullanici();

                MD5 md5Hash = MD5.Create();
                var hashed = GetMd5Hash(md5Hash, yenisifre);
                mail.Parola = hashed;
                db.SaveChanges();

                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "cukongrebasvuru@gmail.com";
                WebMail.Password = "cukurova.1973";
                WebMail.SmtpPort = 587;
                WebMail.Send(email, "Çukurova Üniversitesi Kongre Yönetim hesabı parola değişikliği",
                    email + " adresli Çukurova Üniversitesi Kongre Yönetim hesabı parolası değişti.Yeni Parolanız : " + yenisifre);
                ViewBag.Uyari = "Şifreniz başarı ile gönderilmiştir.";
            }
            else
            {
                ViewBag.Uyari = "Böyle bir mail adresi bulunmamaktadır.";
            }
            return View();
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
