using SatendramProperty.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace SatendramProperty.Data
{
    public class AppDBContext: DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<PropertyMaster>().HasNoKey();
            modelBuilder.Entity<UserMaster>().HasNoKey();
            modelBuilder.Entity<RequirmentMaster>().HasNoKey();
            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<PropertyMaster>()
       .Ignore(p => p.PropertyMedia);
        }
		public DbSet<PropertyMaster> PropertyMasters { get; set; }
        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<RequirmentMaster> RequirmentMasters { get; set; }

    }
}
