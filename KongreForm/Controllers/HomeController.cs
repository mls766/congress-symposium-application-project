using KongreForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Services;

namespace KongreForm.Controllers
{

    public class HomeController : Controller
    {

        public PartialViewResult GetOturum(int KongreId)
        {


            BasvuruViewModel model = new BasvuruViewModel();


            List<OturumVM> oturums = GetOturumDto(KongreId);

            model.OturumList = oturums;

            return PartialView("_GetOturum", model);
        }

        private static List<OturumVM> GetOturumDto(int KongreId)
        {
            KongreDbEntities db = new KongreDbEntities();
            return db.Oturums.Where(x => x.FKKongreId == KongreId).Select(y => new OturumVM
            {
                FKKongreId = y.FKKongreId,
                Id = y.Id,
                IsChecked = y.IsChecked,
                OturumAd = y.OturumAd,
                OturumBaslangic = y.OturumBaslangic,
                OturumBitis = y.OturumBitis
            }).ToList();
        }



        //[Route("anasayfa")]
        public ActionResult Index()
        {
            KongreDbEntities db = new KongreDbEntities();

            BasvuruViewModel model = new BasvuruViewModel();

            //Oturumu olmayan kongreyi seçme
            var items = db.Kongres.Where(c => db.Oturums.Select(b => b.FKKongreId).Contains(c.Id) && c.AktifMi == true).ToList();
            if (items != null)
            {
                ViewBag.dgr = new SelectList(items, "Id", "KongreAd");
            }


            model.OturumList = new List<OturumVM>();



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BasvuruViewModel model, HttpPostedFileBase Dosya/*, IEnumerable<OturumModelClass> oturummodel*/)
        {
            KongreDbEntities db = new KongreDbEntities();


            var items = db.Kongres.Where(c => db.Oturums.Select(b => b.FKKongreId).Contains(c.Id) && c.AktifMi == true).ToList();

            if (items != null)
            {
                ViewBag.dgr = new SelectList(items, "Id", "KongreAd");
            }

            if (model.OturumList == null)
            {
                ModelState.AddModelError("FKKongreId", "Lütfen en az bir tane oturum seçiniz.");

                return ReturnView(model);
            }
            if (model.OturumList.Count(y => y.IsChecked == true) < 1)
            {
                ModelState.AddModelError("FKKongreId", "Lütfen bir tane oturum seçiniz.");

                return ReturnView(model);
            }


            //Başvuran sunucu ise validation ata
            //if (model.SunucuMu == true)
            //{
            //    if (model.SunumBaslik == null)
            //    {
            //        ModelState.AddModelError("SunumBaslik", "Bu alan doldurulması zorunludur.");
            //    }
            //    if (model.SunumTipi == 0)
            //    {
            //        ModelState.AddModelError("SunumTipi", "Bu alan doldurulması zorunludur.");
            //    }
            //    if (model.Dosya == null)
            //    {
            //        ModelState.AddModelError("Dosya", "Bu alan doldurulması zorunludur.");
            //    }
            //}




            if (ModelState.IsValid)
            {
                Kisi k = new Kisi();
                k.Ad = model.Ad;
                k.Soyad = model.Soyad;
                k.Email = model.Email;
                k.Egitim = model.Egitim;
                k.TcNo = model.TcNo;
                k.Telefon = model.Telefon;
                k.Meslek = model.Meslek;
                k.Kurum = model.Kurum;
                k.Universite = model.Universite;
                k.AnaDal = model.AnaDal;
                k.Adres = model.Adres;
                k.Fakulte = model.Fakulte;
                k.Bolum = model.Bolum;



                var KisiAra = db.Kisis.Where(x => x.Telefon == model.Telefon).SingleOrDefault();
                Basvuru b = new Basvuru();
                if (KisiAra == null)
                {
                    db.Kisis.Add(k);

                    model.FKKisiId = k.Id;
                    b.FKKisiId = model.FKKisiId;
                }
                else
                {
                    model.FKKisiId = KisiAra.Id;
                    b.FKKisiId = model.FKKisiId;
                }

                model.BasvuruKodu = Ifexist();


                b.Id = model.Id;
                b.SunucuMu = model.SunucuMu;
                b.BasvuruKodu = model.BasvuruKodu;
                b.BasvuruZaman = model.BasvuruZaman;
                b.Onay = model.Onay;
                b.OnayZaman = model.OnayZaman;
                b.Baslik = model.Baslik;
                b.SunumTipi = model.SunumTipi;
                b.FKKongreId = model.FKKongreId ?? 0;
                b.BasvuruZaman = DateTime.Now;
                b.BasvuruKodu = model.BasvuruKodu;
                b.Ozet = model.Ozet;
                b.PosterMi = model.PosterMi;

                //var a = db.Basvurus.Where(x => x.FKKisiId == model.FKKisiId).Select(x => x.FKKongreId).FirstOrDefault();



                //if (a == model.FKKongreId)
                //{
                //    ModelState.AddModelError("FKKongreId", "Bu kongreye daha önce basvurdunuz!");
                //    return ReturnView(model);
                //}


                int SonBasvuruId = b.Id;

                BasvuruEvrak be = new BasvuruEvrak();
                be.Id = model.EvrakId;
                be.Dosya = model.Dosya;
                be.DosyaTuru = model.DosyaTuru;
                be.DosyaAdi = model.DosyaAdi;
                be.Zaman = model.Zaman;
                be.Zaman = DateTime.Now;
                be.FKBasvuruId = SonBasvuruId;

                //if(model.SunucuMu  == true)
                //{


                if (Dosya.ContentLength > 0)
                    try
                    {
                        string filename = Path.GetFileName(Dosya.FileName);
                        be.DosyaAdi = filename;
                        FileInfo info = new FileInfo(Dosya.FileName);
                        string uzanti = info.Extension;
                        be.DosyaTuru = uzanti;
                        if (be.DosyaTuru != ".jpg"
                            && be.DosyaTuru != ".pdf"
                            && be.DosyaTuru != ".ppptx"
                            && be.DosyaTuru != ".ppt"
                            && be.DosyaTuru != ".png"
                            && be.DosyaTuru != ".jpeg")
                        {
                            throw new Exception("Sadece pdf, jpeg, png, powerpoint dosyaları yüklenebilir.");
                        }
                        string savepath = @"C:\KongreForm\";
                        savepath += "Kisi" + k.Telefon + "\\";
                        savepath += "Basvuru" + b.FKKongreId + "\\";


                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        string fullpath = Path.Combine(savepath, filename);
                        be.Dosya = Url.Content(Path.Combine(savepath, filename));

                        Dosya.SaveAs(fullpath);
                    }

                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message.ToString();
                        return ReturnView(model);
                    }
                else
                {
                    ViewBag.Message = "Dosya seçimi başarısız.";
                }
                db.BasvuruEvraks.Add(be);

                //}

                //basvuru yapan kişi daha önce başka bir kongreye başvuru yapmış mı?
                var blist = db.Basvurus.Where(x => x.FKKisiId == model.FKKisiId).Select(x => x.Id).ToList();
                List<int> list = new List<int>();

                //Daha önceden başvuru yapılmış oturum var mı sorgusu
                foreach (var item in blist)
                {
                    List<int> bdetlist = db.BasvuruDetays.Where(x => x.FKBasvuruId == item).Select(x => x.FKOturumId).ToList();
                    foreach (var item2 in bdetlist)
                    {
                        list.Add(item2);
                    }
                }

                foreach (var oturum in model.OturumList)
                {
                    if (oturum.IsChecked)
                    {
                        if (list.Any(y => y == oturum.Id))
                        {
                            ViewBag.OturumError = "Bu oturuma daha önce basvurdunuz!";
                            ModelState.AddModelError("FKKongreId", "Aynı oturuma tekrar başvurulamaz!");
                            return ReturnView(model);
                        }
                        else
                        {
                            BasvuruDetay bdetay = new BasvuruDetay();
                            bdetay.FKOturumId = oturum.Id;
                            bdetay.FKBasvuruId = b.Id;
                            db.BasvuruDetays.Add(bdetay);
                        }

                    }

                }



                db.Basvurus.Add(b);

                db.SaveChanges();





                WebMail.SmtpServer = WebConfigurationManager.AppSettings["SmtpServer"].ToString();
                WebMail.EnableSsl = WebConfigurationManager.AppSettings["EnableSsl"].ToString().ToLower().Equals("true");
                WebMail.UserName = WebConfigurationManager.AppSettings["UserName"].ToString();
                WebMail.Password = WebConfigurationManager.AppSettings["Password"].ToString();
                WebMail.SmtpPort = int.Parse(WebConfigurationManager.AppSettings["SmtpPort"]);
                WebMail.Send(b.Kisi.Email, "Çukurova Üniversitesi Kongre Başvuru Formunu doldurdunuz. ",
                    b.BasvuruZaman.ToShortDateString() + " tarihinde saat " + b.BasvuruZaman.ToShortTimeString() + " 'de " + b.Kisi.Email +
                    " adresi üzerinden Çukurova Üniversitesi'nin düzenlediği " + b.FKKongreId +
                    " numaralı kongreye başvurunuz gerçekleşmiştir. Başvuru sonucunuzu Başvuru Kodunuz: "
                    + b.BasvuruKodu + " ile sorgulayabilirsiniz.");

                return RedirectToAction("BasvuruAlindi", new { id = b.Id });

            }
            return ReturnView(model);




        }

        private string Ifexist()
        {
            KongreDbEntities db = new KongreDbEntities();

            var basvurukodu = "A" + (new Random()).Next(1111111, 9999999).ToString();


            if (db.Basvurus.Where(x => x.BasvuruKodu == basvurukodu).FirstOrDefault() != null)
            {
                Ifexist();
            }

            return basvurukodu;
        }


        private ActionResult ReturnView(BasvuruViewModel model)
        {
            if (model.FKKongreId != null)
            {
                if (model.OturumList == null)
                {
                    List<OturumVM> oturums = GetOturumDto(model.FKKongreId ?? 0);
                    model.OturumList = oturums;
                }

            }
            return View(model);
        }

        public ActionResult DownloadFile(int filePath)
        {
            using (KongreDbEntities db = new KongreDbEntities())
            {

                var be = db.BasvuruEvraks.Where(x => x.FKBasvuruId == filePath).FirstOrDefault();

                if (be != null)
                {
                    var fileName = Path.GetFileName(be.Dosya);
                    if (System.IO.File.Exists(be.Dosya))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                        Response.WriteFile(be.Dosya);
                        Response.End();
                    }
                    else
                    {
                        ViewBag.Error = "Dosya bulunamadı";

                        return RedirectToAction("Details", "Basvuru", new { id = filePath });

                    }
                }
                return View();

            }
        }


        [Route("basvurualindi")]
        public ActionResult BasvuruAlindi(int? id)
        {
            KongreDbEntities db = new KongreDbEntities();
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
                return View(basvuru);
            }

            catch
            {
                return RedirectToAction("Error", "Home");
            }


        }


        [Route("basvurusorgu")]
        public ActionResult BasvuruSorgu()
        {
            return View();
        }

        [Route("basvurusorgu")]
        [HttpPost]
        public ActionResult BasvuruSorgu(string email, string basvurukodu)
        {
            KongreDbEntities db = new KongreDbEntities();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(basvurukodu))
                    {
                        var b = db.Basvurus.Where(x => x.BasvuruKodu == basvurukodu && x.Kisi.Email == email).SingleOrDefault();
                        if (b == null)
                        {
                            throw new Exception("Böyle bir başvuru bulunmamaktadır.");
                        }

                        return RedirectToAction("Sonuc", "Basvuru", new { id = b.Id });
                    }
                }

                catch (Exception)
                {

                    ViewBag.ErrorMessage = "Böyle bir başvuru bulunmamaktadır.";
                    return View();
                }
            }
            ModelState.AddModelError("Email", "Lutfen email adresinizi girin");
            ModelState.AddModelError("BasvuruKodu", "Lutfen basvuru kodunuzu girin");
            return View();

        }

        public ActionResult Error()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }

    }
}



//public static string FileUpload(FileUpload fu, int kisi, int basvuru, int dosyaTur, out FileInfo info)
//{

//    try
//    {
//        string filename = Path.GetFileName(Dosya.FileName);
//        be.DosyaAdi = filename;
//        FileInfo info = new FileInfo(Dosya.FileName);
//        string uzanti = info.Extension;
//        be.DosyaTuru = uzanti;
//        if (be.DosyaTuru != ".jpg"
//            && be.DosyaTuru != ".pdf"
//            && be.DosyaTuru != ".ppptx"
//            && be.DosyaTuru != ".ppt"
//            && be.DosyaTuru != ".png"
//            && be.DosyaTuru != ".jpeg")
//        {
//            throw new Exception("Sadece pdf, jpeg, png, powerpoint dosyaları yüklenebilir.");
//        }

//        string path = Server.MapPath("~/Uploads");
//        string fullpath = Path.Combine(path, filename);
//        be.Dosya = Url.Content(Path.Combine("~/Uploads", filename));

//        Dosya.SaveAs(fullpath);
//        be.Dosya = "/Uploads/" + filename;
//    }

//    catch (Exception ex)
//    {
//        ViewBag.Message = ex.Message.ToString();
//        return ReturnView(model);
//    }




//    info = null;
//    var _validFileExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".gif", ".png", ".xls", ".docx", ".doc", ".xlsx", ".pdf", ".zip", ".rar" };
//    if (fu.HasFile)
//    {
//        var f = new FileInfo(fu.FileName);
//        if (!_validFileExtensions.Contains(f.Extension.ToLower()))
//        {

//            return
//                "Hatalı dosya tipi. Geçerli dosya tipleri: .jpg, .jpeg, .bmp, .gif, .png, .xls, .docx, .doc, .xlsx, .pdf, .zip, .rar";
//        }

//        String fileName = fu.FileName;
//        String savePath = @"D:\EnstituData\";
//        savePath += "Kisi" + kisi + "\\";
//        savePath += "Basvuru" + basvuru + "\\";
//        savePath += "Dosya" + dosyaTur + "\\";
//        if (!Directory.Exists(savePath))
//        {
//            Directory.CreateDirectory(savePath);
//        }
//        List<string> silinecekEskiDosyalar = Directory.GetFiles(savePath).ToList();
//        savePath += fileName;
//        try
//        {
//            fu.SaveAs(savePath);
//            if (silinecekEskiDosyalar.Any())
//            {
//                foreach (var file in silinecekEskiDosyalar)
//                {
//                    if (File.Exists(file))
//                        File.Delete(file);
//                }
//            }

//            info = new FileInfo(savePath);
//            return "";
//        }
//        catch (Exception ex)
//        {


//        }