
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DataService.Entities;

namespace DataService.DataContext
{
    public class SchwarzDbContext : DbContext
    {

        public SchwarzDbContext(DbContextOptions<SchwarzDbContext> options) : base(options)
        {

        }


        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
      

    }
}
