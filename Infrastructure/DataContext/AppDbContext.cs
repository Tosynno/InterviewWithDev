using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<tbl_RefreshToken> RefreshTokens { get; set; }
        public DbSet<tbl_StaffBioData> tbl_StaffBioDatas { get; set; }
        public DbSet<tbl_Student> tbl_Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<tbl_RefreshToken>()
          .HasOne(p => p.tblStaffBioData)
          .WithMany(b => b.tblRefreshToken).HasForeignKey(v => v.StaffId);
        }
    }
}
