﻿using Company.Fares.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.Fares.DAL.Data.Contexts
{
    //CLR
    public class CompanyDbContext : DbContext
    {

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{


        //    optionsBuilder.UseSqlServer("Server=.;Database=CompanyDb;Trusted_Connection=True;TrustServerCertificate = True");
        //}

        public DbSet<Department>  Departments { get; set; }
        public object departments { get; set; }
    }
}
