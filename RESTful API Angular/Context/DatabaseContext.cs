using RESTful_API_Angular.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RESTful_API_Angular.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=AngularApiDB")
        {

        }

        public virtual DbSet<StudentModel> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentModel>()
                        .HasKey(s => s.StudentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}