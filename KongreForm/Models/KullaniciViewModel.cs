using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KongreForm.Models
{
    public class KullaniciViewModel
    {
       
        [Required(ErrorMessage ="Lütfen email girin")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lütfen parola girin")]
        [StringLength(50)]
        public string Parola { get; set; }

        [Required(ErrorMessage = "Lütfen ad girin")]
        [StringLength(50)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Lütfen soyad girin")]
        [StringLength(50)]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Lütfen seçim yapın")]
        public bool AdminMi { get; set; }
    }
}