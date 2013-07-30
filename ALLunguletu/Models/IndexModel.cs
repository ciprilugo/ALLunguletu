using DataAnnotationsExtensions;
using Lugo.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace ALLunguletu.Models
{
    public class IndexContext : DbContext
    {
        public IndexContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Index> UserProfiles { get; set; }
    }

    [Table("index")]
    public class IndexModel: Index
    {
        public new System.DateTime Data { get; set; }
        public new DateTime TimeStamp { get; set; }

        [Display(Name="Tip apa")]
        public int WaterId { get; set; }

        [Display(Name="Index vechi")]
        [Required(ErrorMessage = "Introduceti Indexul vechi.")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100.000 .")]
        [DisplayFormat(DataFormatString="{0:n}")]
        public new int IndexOld { get; set; }

        [Display(Name = "Index nou")]
        [Integer(ErrorMessage = "Introduceti valori intre 1 si 100.000 .")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100.000 .")]
        [Required(ErrorMessage = "Introduceti Indexul nou.")]
        public new int IndexNew { get; set; }

        [Display(Name = "Diferenta index ")]
        public new Nullable<int> IndexDiff { get; set; }

        [Display(Name = "Luna citire index")]
        [Required(ErrorMessage = "Introduceti Luna citire Index.")]
        [Range(1,12,ErrorMessage="Introduceti valori intre 1 si 12.")]
        public new int IndexMonth { get; set; }

        [Display(Name = "An citire index")]
        [Required(ErrorMessage = "Introduceti anul citire Index.")]
        [Range(2013, 2020, ErrorMessage = "Introduceti valori intre 2013 si 2020.")]
        public new int IndexYear { get; set; }

        [Display(Name = "Comentarii")]
        public new string Description { get; set; }
    }
}