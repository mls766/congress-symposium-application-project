using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KongreForm.Models
{
    public class KongreViewModel
    {
        public List<KongreModel> KongreList { get;set; }
        public int SelectedKongre { get; set; }
        public List<OturumModel> OturumList { get; set; }
    }

    public class KongreModel
    {
        [Required]
        public string KongreAd { get; set; }

        public DateTime Baslangic { get; set; }

        public DateTime Bitis { get; set; }

        public bool? AdAktifMi { get; set; }


    }

    public class OturumModel
    {
        public int Id { get; set; }

        public int FKKongreId { get; set; }


        [StringLength(150)]
        public string OturumAd { get; set; }

        public DateTime OturumBaslangic { get; set; }

        public DateTime OturumBitis { get; set; }


    }







}
