

using ADBee.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADBee.Data
{

    public class ADSystemDbContext : DbContext
    {
        public ADSystemDbContext(DbContextOptions<ADSystemDbContext> options) : base(options)
        {

        }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Person>().HasIndex(u => u.Name).IsUnique(); //是否唯一，看你直接是否需要咯
        }

        public DbSet<AdStastic> AdStastics { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
    }
}
