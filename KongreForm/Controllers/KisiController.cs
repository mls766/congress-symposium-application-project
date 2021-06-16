using KongreForm.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KongreForm.Controllers
{
    public class KisiController : Controller
    {
        KongreDbEntities db = new KongreDbEntities();
   
       
        public ActionResult Index()
        {

            if (Convert.ToBoolean(Session["yetki"]))
            {
                return View(db.Kisis.OrderByDescending(x => x.Id).ToList());
            }
            else
            {
                var kullanici = Convert.ToInt32(Session["kullaniciid"]);
                Yetki yetki = new Yetki();
                yetki.FKKullaniciId = kullanici;

                var kongrelist = db.Yetkis.Where(x => x.FKKullaniciId == kullanici).Select(x => x.FKKongreId).ToList();
                List<int> kisiIdler = new List<int>();

                foreach (var item in kongrelist)
                {
                    var kisilist = db.Basvurus.Where(x => x.FKKongreId == item).Select(x => x.FKKisiId).ToList();
                    foreach (var item2 in kisilist)
                    {
                        kisiIdler.Add(item2);
                    }

                }
                List<Kisi> kisi =new List<Kisi>() ;
                
                foreach (var item in kisiIdler)
                {
                    var eklenecek = db.Kisis.FirstOrDefault(x => x.Id == item);
                    kisi.Add(eklenecek);
                }



                return View(kisi);
            }
        }


        public ActionResult Basvurular(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception();
                }
                Kisi k = db.Kisis.SingleOrDefault(x => x.Id == id);
                if (k == null)
                {
                    throw new Exception();
                }

                if (Convert.ToBoolean(Session["yetki"]))
                {
                
                    List<Basvuru> basvurulist = db.Basvurus.Where(x => x.FKKisiId == id).ToList();

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
                        Ozet= x.Ozet,
                        PosterMi = x.PosterMi,
                        Baslik = x.Baslik


                    }).ToList();

                    return View(kongreVMList);
                }
                else
                {
                    var kullanici = Convert.ToInt32(Session["kullaniciid"]);
                    Yetki yetki = new Yetki();
                    yetki.FKKullaniciId = kullanici;

                    var kongrelist = db.Yetkis.Where(x => x.FKKullaniciId == kullanici).Select(x => x.FKKongreId).ToList();
                    List<Basvuru> basvurulist = new List<Basvuru>();
                    foreach (var item in kongrelist)
                    {
                        var a = db.Basvurus.Where(x => x.FKKisiId == id && x.FKKongreId == item).ToList();
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
                        Ozet = x.Ozet,
                        PosterMi = x.PosterMi,
                        Baslik = x.Baslik


                    }).ToList();

                    return View(kongreVMList);
                }
  
            }

            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
            

            
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kisi kisi = db.Kisis.Find(id);
            if (kisi == null)
            {
                return HttpNotFound();
            }
            return View(kisi);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ad,Soyad,Email,TcNo,Kurum,Meslek,Egitim,Telefon,Universite,AnaDal,Adres,Fakulte,Bolum")] Kisi kisi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kisi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kisi);
        }


    }
}
