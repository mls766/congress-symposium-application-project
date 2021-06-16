using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KongreForm.Models
{
    public partial class BasvuruDVM

    {
        public int Id { get; set; }

        public string OturumAd { get; set; }

        public int FKOturumId { get; set; }

        public string KullaniciAd { get; set; }

        public string KullaniciSoyad { get; set; }
        public Nullable<int> FKKullaniciId { get; set; }
        public int FKBasvuruId { get; set; }
        public Nullable<System.DateTime> OnayZaman { get; set; }
        public Nullable<bool> Onay { get; set; }


    }
    public class BasvuruAndEvrakModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı gereklidir.")]
        public string Baslik { get; set; }


        [Required(ErrorMessage = "Lütfen seçim yapınız.")]
        public bool? PosterMi { get; set; }

        public int FKBasvuruId { get; set; }

        public string DosyaTuru { get; set; }

        public string Dosya { get; set; }

        public DateTime Zaman { get; set; }
        public string DosyaAdi { get; set; }
        public List<BasvuruDetayVM> DetayList { get; set; }

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KongreAd { get; set; }


    }
}