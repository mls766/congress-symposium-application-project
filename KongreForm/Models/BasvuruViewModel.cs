using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KongreForm.Models
{
    public partial class OturumVM
    {
        public int Id { get; set; }
      
        public int FKKongreId { get; set; }
        public string OturumAd { get; set; }
        public System.DateTime OturumBaslangic { get; set; }
        public System.DateTime OturumBitis { get; set; }
      
        [Required(ErrorMessage ="Oturum seçin")]
        public bool IsChecked { get; set; }


    }
    public partial class BasvuruDetayVM
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
    public class BasvuruViewModel
    {

       
        public bool? SunucuMu { get; set; }


        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [StringLength(30)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        [StringLength(50)]
        public string Soyad { get; set; }


        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi girin.")]
        public string Email { get; set; }

        [RegularExpression(@"^[1-9]{1}[0-9]{9}[02468]{1}$", ErrorMessage ="Lütfen geçerli bir kimlik numarası adresi girin.")]
        [StringLength(11,MinimumLength =11, ErrorMessage =" Tc kimlik numarası 11 haneli olmalıdır.")]
        public string TcNo { get; set; }

        [StringLength(50,ErrorMessage = "Karakter asimi yaptiniz")]
        public string Kurum { get; set; }


        [StringLength(50, ErrorMessage = "Karakter asimi yaptiniz")]

        public string Meslek { get; set; }


        public int? Egitim { get; set; }

        [Required]
        [Phone(ErrorMessage = "Lütfen telefon numaranızı giriniz")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage ="Lütfen geçerli bir telefon numarası girin. (5xxxxxxxxx)")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Üniversite alanı gereklidir.")]
        [StringLength(50, ErrorMessage = "Karakter asimi yaptiniz")]

        public string Universite { get; set; }

        [StringLength(50, ErrorMessage = "Karakter asimi yaptiniz")]

        public string AnaDal { get; set; }
        [Required(ErrorMessage = "Adres alanı gereklidir.")]

        public string Adres { get; set; }


        [Required(ErrorMessage = "Fakülte alanı gereklidir.")]
        [StringLength(50, ErrorMessage = "Karakter asimi yaptiniz")]
        public string Fakulte { get; set; }


        [Required(ErrorMessage = "Bölüm alanı gereklidir.")]
        [StringLength(50, ErrorMessage = "Karakter asimi yaptiniz")]

        public string Bolum { get; set; }


        public int Id { get; set; }

        public int FKKisiId { get; set; }

        [Required(ErrorMessage = "Lütfen sunum yapmak istediğiniz kongreyi seçin")]
        public int? FKKongreId { get; set; }

        public bool? Onay { get; set; }

        public DateTime? OnayZaman { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BasvuruZaman { get; set; }

        public string BasvuruKodu { get; set; }


        public int? SunumTipi { get; set; }


        [Required(ErrorMessage = "Başlık alanı gereklidir.")]
        public string Baslik { get; set; }


        public string Ozet { get; set; }


        [Required(ErrorMessage = "Lütfen seçim yapınız.")]
        public bool? PosterMi { get; set; }



        public int EvrakId { get; set; }
        public int FKBasvuruId { get; set; }

        public string DosyaTuru { get; set; }

        [Required(ErrorMessage = "Dosya alanı gereklidir.")]
        public string Dosya { get; set; }

        public DateTime Zaman { get; set; }



        [StringLength(50)]
        public string DosyaAdi { get; set; }

        public string KongreAd { get; set; }


        [Required(ErrorMessage = "Oturum")]

        public List<OturumVM> OturumList { get; set; }

        public List<BasvuruDetayVM> DetayList { get; set; }
    }

   
}