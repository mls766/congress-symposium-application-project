using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KongreForm.Models
{
    public class AktifCheck
    {
        public int Id { get; set; }
        public string Icerik { get; set; }
        public bool IsChecked { get; set; }

    }
    public class AktifViewModel
    {

        public int AktifAlanId { get; set; }
        public int FKKongreId { get; set; }
        public List<AktifCheck> AktifItems { get; set; }


    }
}