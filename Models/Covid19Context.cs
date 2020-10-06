using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Api.Models
{
    public class Covid19Context : DbContext
    {
        public Covid19Context(DbContextOptions<Covid19Context> options)
           : base(options)
        {
            // Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade
            Database.Migrate(); //we need to import the Microsoft.EntityFrameworkCore.Relational
            //creates tables from models
        }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configures one-to-many relationship
           /* modelBuilder.Entity<Case>()
                .HasRequired<Country>(s => s.Country.id)
                .WithMany(g => g.Case)
                .HasForeignKey<int>(s => s.Country.id); ; */
        }

    }
}
