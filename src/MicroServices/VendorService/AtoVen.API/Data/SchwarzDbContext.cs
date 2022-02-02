
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtoVen.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AtoVen.API.Data
{
    public class SchwarzDbContext : DbContext
    { 

        public SchwarzDbContext(DbContextOptions<AtoVenDbContext> options) : base(options)
        {

        }
     

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
