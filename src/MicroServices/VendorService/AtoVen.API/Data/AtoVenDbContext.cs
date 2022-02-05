
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtoVen.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AtoVen.API.Controllers.AccountControl.Models;

namespace AtoVen.API.Data
{
    public class AtoVenDbContext: IdentityDbContext<ApplicationUser>
    { 

        public AtoVenDbContext(DbContextOptions<AtoVenDbContext> options) : base(options)
        {

        }
     

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<ApprovalFlow> ApprovalFlows { get; set; }


    }
}
