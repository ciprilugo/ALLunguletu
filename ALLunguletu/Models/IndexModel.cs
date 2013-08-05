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

        [Display(Name = "Index vechi BaRe")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new int IndexOldBaieRece { get; set; }

        [Display(Name = "Index nou BaRe")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new int IndexNewBaieRece { get; set; }

        [Display(Name = "Diferenta index BaRe")]
        public new  Nullable<int> IndexDiffBaieRece { get; set; }

        [Display(Name = "Index vechi BaCa")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new int IndexOldBaieCalda { get; set; }

        [Display(Name = "Index nou BaCa")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new int IndexNewBaieCalda { get; set; }


        [Display(Name = "Diferenta index BaCa")]
        public new  Nullable<int> IndexDiffBaieCalda { get; set; }

        [Display(Name = "Index vechi BuRe")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new  int IndexOldBucatarieRece { get; set; }

        [Display(Name = "Index nou BuRe")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new  int IndexNewBucatarieRece { get; set; }


        [Display(Name = "Diferenta index BuRe")]
        public new  Nullable<int> IndexDiffBucatarieRece { get; set; }

        [Display(Name = "Index vechi BuCa")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new  int IndexOldBucatarieCalda { get; set; }

        [Display(Name = "Index nou BuCa")]
        [Required(ErrorMessage = "*")]
        [Range(1, 100000, ErrorMessage = "Introduceti valori intre 1 si 100000 .")]
        [DisplayFormat(DataFormatString = "{0:n}")]
        public new  int IndexNewBucatarieCalda { get; set; }


        [Display(Name = "Diferenta index BuCa")]
        public new  Nullable<int> IndexDiffBucatarieCalda { get; set; }


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