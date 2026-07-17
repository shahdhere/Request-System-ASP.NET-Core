using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Request = Models.Models.Request;

namespace Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        // الكونستركتور لاستقبال خيارات الكونتكست
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // الـ DbSets تمثل الجداول في الداتابيس
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<IndividualProfile> IndividualProfiles { get; set; } = null!;
        public DbSet<CompanyProfile> CompanyProfiles { get; set; } = null!;
        public DbSet<Request> Requests { get; set; } = null!;
        public DbSet<Attachment> Attachments { get; set; }

        // اذا حبيت تضيف إعدادات إضافية للجداول
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // مثال: تحديد مفتاح أساسي أو علاقات
            modelBuilder.Entity<IndividualProfile>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Request>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<CompanyProfile>()
                .HasKey(r => r.Id);

            // ضبط العلاقة One-to-One بين User و IndividualProfile

            modelBuilder.Entity<User>()
              .HasOne(u => u.IndividualProfile)
              .WithOne(p => p.User)
              .HasForeignKey<IndividualProfile>(p => p.UserId);

            // ضبط العلاقة One-to-One بين User و CompanyProfile
            modelBuilder.Entity<User>()
                .HasOne(u => u.CompanyProfile)
                .WithOne(p => p.User)
                .HasForeignKey<CompanyProfile>(p => p.UserId);

            modelBuilder.Entity<User>()
                    .HasMany(u => u.Requests)
                    .WithOne(r => r.User)
                    .HasForeignKey(r => r.UserId);


        }
    }
}