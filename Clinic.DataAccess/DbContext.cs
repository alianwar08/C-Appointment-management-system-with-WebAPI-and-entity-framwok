using Clinic.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.DataAccess
{
    public class ClinicDB : DbContext
    {
        string conStr = @"server=localhost;database=ClinicDB;uid=root;password=root;";

        public DbSet<Patient> Patient { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        //public ClinicDB(DbContextOptions<ClinicDB> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(conStr, ServerVersion.AutoDetect(conStr));
        }

    }
}


