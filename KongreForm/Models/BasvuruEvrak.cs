//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KongreForm.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BasvuruEvrak
    {
        public int Id { get; set; }
        public int FKBasvuruId { get; set; }
        public string DosyaTuru { get; set; }
        public string Dosya { get; set; }
        public System.DateTime Zaman { get; set; }
        public string DosyaAdi { get; set; }
    
        public virtual Basvuru Basvuru { get; set; }
    }
}