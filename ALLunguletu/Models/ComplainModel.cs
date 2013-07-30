using Lugo.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ALLunguletu.Models
{
    public class ComplainContext : DbContext
    {
        public ComplainContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Complain> UserProfiles { get; set; }
    }

    [Table("Complain")]
    public class ComplainModel : Complain
    {
        public int ComplainId { get; set; }
        public int UserId { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public new System.DateTime Data { get; set; }

        [Display(Name="Subiect")]
        [Required(ErrorMessage="Introduceti Subiectul")]
        public new string Subject { get; set; }

        [Display(Name = "Sesizare")]
        [Required(ErrorMessage = "Introduceti Sesizarea")]
        public new string Description { get; set; }
    }
}