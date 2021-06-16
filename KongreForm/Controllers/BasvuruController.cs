using KongreForm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KongreForm.Controllers
{
    public class BasvuruController : Controller
    {
        KongreDbEntities db = new KongreDbEntities();

        public ActionResult Index()
        {
            if (Convert.ToBoolean(Session["yetki"]))
            {
                List<Basvuru> basvurulist = db.Basvurus.OrderByDescending(x => x.BasvuruZaman).ToList();
                List<BasvuruViewModel> kongreVMList = basvurulist.Select(x => new BasvuruViewModel

                {
                    Id = x.Id,
                    BasvuruKodu = x.BasvuruKodu,
                    BasvuruZaman = x.BasvuruZaman,
                    SunumTipi = x.SunumTipi,
                    FKKisiId = x.Kisi.Id,
                    FKKongreId = x.Kongre.Id,
                    KongreAd = x.Kongre.KongreAd,
                    Onay = x.Onay,
                    OnayZaman = x.OnayZaman,
                    Ad = x.Kisi.Ad,
                    Soyad = x.Kisi.Soyad,
                    Egitim = x.Kisi.Egitim,
                    Meslek = x.Kisi.Meslek,
                    Universite = x.Kisi.Universite,
                    AnaDal = x.Kisi.AnaDal,
                    Adres = x.Kisi.Adres,
                    Fakulte = x.Kisi.Fakulte,
                    Bolum = x.Kisi.Bolum,
                    Baslik = x.Baslik,
                    PosterMi = x.PosterMi


                }).ToList();
                return View(kongreVMList);
            }
            else
            {
                var kullanici = Convert.ToInt32(Session["kullaniciid"]);
                Yetki yetki = new Yetki();
                yetki.FKKullaniciId = kullanici;

                List<Basvuru> basvurulist = new List<Basvuru>();

                var kongre = db.Yetkis.Where(x => x.FKKullaniciId == kullanici).Select(x => x.FKKongreId).ToList();
                foreach (var item in kongre)
                {
                    var a= db.Basvurus.Where(x => x.FKKongreId == item).OrderByDescending(x => x.BasvuruZaman).ToList();
                    foreach (var item2 in a)
                    {
                        basvurulist.Add(item2);
                    }
                }
                List<BasvuruViewModel> kongreVMList = basvurulist.Select(x => new BasvuruViewModel

                {
                    Id = x.Id,
                    BasvuruKodu = x.BasvuruKodu,
                    BasvuruZaman = x.BasvuruZaman,
                    SunumTipi = x.SunumTipi,
                    FKKisiId = x.Kisi.Id,
                    FKKongreId = x.Kongre.Id,
                    KongreAd = x.Kongre.KongreAd,
                    Onay = x.Onay,
                    OnayZaman = x.OnayZaman,
                    Ad = x.Kisi.Ad,
                    Soyad = x.Kisi.Soyad,
                    Egitim = x.Kisi.Egitim,
                    Meslek = x.Kisi.Meslek,
                    Universite = x.Kisi.Universite,
                    AnaDal = x.Kisi.AnaDal,
                    Adres = x.Kisi.Adres,
                    Fakulte = x.Kisi.Fakulte,
                    Bolum = x.Kisi.Bolum,
                    Baslik = x.Baslik,
                    PosterMi = x.PosterMi



                }).ToList();

                return View(kongreVMList);
            }
        }



        public ActionResult Details(int? id)
        {
            try
            {

                if (id == null)
                {
                    throw new Exception();
                }

                Basvuru b = db.Basvurus.SingleOrDefault(x => x.Id == id);

                if (b == null)
                {
                    throw new Exception();
                }

                BasvuruViewModel kongreVM = new BasvuruViewModel();

                kongreVM.Id = b.Id;
                kongreVM.Ad = b.Kisi.Ad;
                kongreVM.Soyad = b.Kisi.Soyad;
                kongreVM.Meslek = b.Kisi.Meslek;
                kongreVM.Kurum = b.Kisi.Kurum;
                kongreVM.TcNo = b.Kisi.TcNo;
                kongreVM.Telefon = b.Kisi.Telefon;
                kongreVM.Egitim = b.Kisi.Egitim;
                kongreVM.Email = b.Kisi.Email;
                kongreVM.BasvuruZaman = b.BasvuruZaman;
                kongreVM.BasvuruKodu = b.BasvuruKodu;
                kongreVM.FKKongreId = b.Kongre.Id;
                kongreVM.KongreAd = b.Kongre.KongreAd;
                kongreVM.SunumTipi = b.SunumTipi;
                kongreVM.Universite = b.Kisi.Universite;
                kongreVM.AnaDal = b.Kisi.AnaDal;
                kongreVM.Fakulte = b.Kisi.Fakulte;
                kongreVM.Bolum = b.Kisi.Bolum;
                kongreVM.Baslik = b.Baslik;
                kongreVM.Ozet = b.Ozet;
                kongreVM.PosterMi = b.PosterMi;
                kongreVM.Adres = b.Kisi.Adres;

                int SonBasvuruId = b.Id;


                BasvuruEvrak be2 = db.BasvuruEvraks.SingleOrDefault(x => x.FKBasvuruId == b.Id);

                kongreVM.DosyaTuru = be2.DosyaTuru;
                kongreVM.Dosya = be2.Dosya;
                kongreVM.Zaman = be2.Zaman;
                kongreVM.DosyaAdi = be2.DosyaAdi;


                List<BasvuruDetay> bdetay = new List<BasvuruDetay>();

                bdetay = db.BasvuruDetays.Where(x => x.FKBasvuruId == b.Id).ToList();

                kongreVM.DetayList = bdetay.Select(y => new BasvuruDetayVM()
                {
                    FKBasvuruId = y.FKBasvuruId,
                    FKKullaniciId = y.FKKullaniciId,
                    FKOturumId = y.FKOturumId,
                    Id = y.Id,
                    Onay = y.Onay,
                    OnayZaman = y.OnayZaman,
                    OturumAd = y.Oturum.OturumAd
                }).ToList();


                return View(kongreVM);
            }

            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }

        public ActionResult Onayla(string submitButton, BasvuruViewModel model)
        {

            string[] tokens = submitButton.Split(',');

            var state = tokens[1];

            var button = tokens[0];



            int buttonnum = Int32.Parse(button);

            BasvuruDetay bdetay = new BasvuruDetay();

            var bdetayid = model.DetayList[buttonnum].Id;
            bdetay = db.BasvuruDetays.Where(x => x.Id == bdetayid).FirstOrDefault();

            if (bdetay != null)
            {
                bdetay.Onay = state == "o";
                bdetay.OnayZaman = DateTime.Now;
                bdetay.FKKullaniciId = Convert.ToInt32(Session["kullaniciid"]);
                db.Entry(bdetay).State = EntityState.Modified;
                db.SaveChanges();
            }


            var bdetaylist = db.BasvuruDetays.Where(x => x.Id == bdetayid).Select(x => x.Onay).ToList();
            var basvuru = db.Basvurus.Where(x => x.Id == model.Id).FirstOrDefault();


            if (bdetaylist.All(x => x == false))
            {
                basvuru.Onay = false;
            }
            else if (bdetaylist.All(x => x == null))
            {
                basvuru.Onay = null;
            }
            else if (bdetaylist.Any(x => x == true))
            {
                basvuru.Onay = true;
            }

            db.Entry(basvuru).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Details", new { model.Id });


        }
        [HttpGet]
        [Route("basvurusonuc")]
        public ActionResult Sonuc(int? id)
        {
            KongreDbEntities db = new KongreDbEntities();

            BasvuruViewModel model = new BasvuruViewModel();

            var bdetay = db.BasvuruDetays.Where(x => x.FKBasvuruId == id).Select(y => new BasvuruDetayVM()
            {
                FKBasvuruId = y.FKBasvuruId,
                FKKullaniciId = y.FKKullaniciId,
                FKOturumId = y.FKOturumId,
                Id = y.Id,
                Onay = y.Onay,
                OnayZaman = y.OnayZaman,
                KullaniciAd = y.Kullanici.Ad,
                KullaniciSoyad = y.Kullanici.Soyad,
                OturumAd = y.Oturum.OturumAd


            }).ToList();

            model.DetayList = bdetay;

            try
            {

                if (id == null)
                {
                    throw new Exception();
                }
                Basvuru basvuru = db.Basvurus.Find(id);
                if (basvuru == null)
                {
                    throw new Exception();
                }
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

        }
        public ActionResult Edit(int? id)
        {

            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Basvuru basvuru = db.Basvurus.Find(id);

                BasvuruAndEvrakModel model = new BasvuruAndEvrakModel();

                var kisi = db.Basvurus.Where(x => x.Id == basvuru.Id).Select(x => x.FKKisiId).FirstOrDefault();
                var ad = db.Kisis.Where(x => x.Id == kisi).Select(x => x.Ad).FirstOrDefault();
                var soyad = db.Kisis.Where(x => x.Id == kisi).Select(x => x.Soyad).FirstOrDefault();
                var kongre = db.Basvurus.Where(x => x.Id == basvuru.Id).Select(x => x.FKKongreId).FirstOrDefault();
                var kongreAd = db.Kongres.Where(x => x.Id == kongre).Select(x => x.KongreAd).FirstOrDefault();

                model.Baslik = basvuru.Baslik;
                model.PosterMi = basvuru.PosterMi;
                model.Id = basvuru.Id;
                model.Ad = ad;
                model.Soyad = soyad;
                model.KongreAd = kongreAd;
                       

                var dosya = db.BasvuruEvraks.Where(x => x.FKBasvuruId == basvuru.Id).FirstOrDefault();
                model.Dosya = dosya.Dosya;
                model.DosyaTuru = dosya.DosyaTuru;
                model.Zaman = dosya.Zaman;
                model.DosyaAdi = dosya.DosyaAdi;

                if (basvuru == null)
                {
                    throw new Exception();
                }
               

                return View(model);
            }

            catch
            {
                return RedirectToAction("Error", "Home");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BasvuruAndEvrakModel model, HttpPostedFileBase Dosya)
        {
            var kisisec = db.Basvurus.Where(x => x.Id == model.Id).Select(x => x.FKKisiId).FirstOrDefault();
            var ad = db.Kisis.Where(x => x.Id == kisisec).Select(x => x.Ad).FirstOrDefault();
            var soyad = db.Kisis.Where(x => x.Id == kisisec).Select(x => x.Soyad).FirstOrDefault();
            var kongre = db.Basvurus.Where(x => x.Id == model.Id).Select(x => x.FKKongreId).FirstOrDefault();
            var kongreAd = db.Kongres.Where(x => x.Id == kongre).Select(x => x.KongreAd).FirstOrDefault();

            var evrak = db.BasvuruEvraks.Where(x => x.FKBasvuruId == model.Id).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var a = db.Basvurus.Where(x => x.Id == model.Id).FirstOrDefault();
               
                a.Baslik = model.Baslik;
                a.PosterMi = model.PosterMi;

                db.Entry(a).State = EntityState.Modified;

                var b = db.BasvuruEvraks.Where(x => x.FKBasvuruId == model.Id).FirstOrDefault();
               
                try
                {
                    // if (Dosya.ContentLength > 0 && Dosya.ContentType != null)
                    if (Dosya!= null)
                    {
                        string filename = Path.GetFileName(Dosya.FileName);
                        b.DosyaAdi = filename;
                        FileInfo info = new FileInfo(Dosya.FileName);
                        string uzanti = info.Extension;
                        b.DosyaTuru = uzanti;
                        if (b.DosyaTuru != ".jpg"
                            && b.DosyaTuru != ".pdf"
                            && b.DosyaTuru != ".ppptx"
                            && b.DosyaTuru != ".ppt"
                             && b.DosyaTuru != ".png"
                            && b.DosyaTuru != ".jpeg")
                        {
                            throw new Exception("Sadece pdf, jpeg, png, powerpoint dosyaları yüklenebilir.");
                        }

                        var kisi = db.Basvurus.Where(x => x.Id == model.Id).Select(x => x.FKKisiId).SingleOrDefault();
                        var tel = db.Kisis.Where(x => x.Id == kisi).Select(x => x.Telefon).SingleOrDefault();

                        var kongreid = db.Basvurus.Where(x => x.Id == model.Id).Select(x => x.FKKongreId).FirstOrDefault();


                        string savepath = @"C:\KongreForm\";
                        savepath += "Kisi" + tel + "\\";
                        savepath += "Basvuru" + kongreid + "\\";



                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }


                        string fullpath = Path.Combine(savepath, filename);
                        b.Dosya = Url.Content(Path.Combine(savepath, filename));
                        Dosya.SaveAs(fullpath);
                        db.Entry(b).State = EntityState.Modified;
                    }
                    
                  
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message.ToString();
                    model.KongreAd = kongreAd;
                    model.Ad = ad;
                    model.Soyad = soyad;
                    model.DosyaAdi = evrak.DosyaAdi;

                    return View(model);
                }



            }
            model.KongreAd = kongreAd;
            model.Ad = ad;
            model.Soyad = soyad;
            model.DosyaAdi = evrak.DosyaAdi;
            //TODO DOSYA BÖLÜMÜ GELecek
            return View(model);
        }



    }

}