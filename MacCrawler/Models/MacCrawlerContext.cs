using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MacCrawler.Models
{
    public partial class MacCrawlerContext : DbContext
    {
        public MacCrawlerContext()
        {
        }

        public MacCrawlerContext(DbContextOptions<MacCrawlerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<KnownManufacturer> KnownManufacturer { get; set; }
        public virtual DbSet<KnownModel> KnownModel { get; set; }
        public virtual DbSet<KnownReferenceNumber> KnownReferenceNumber { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<Sheet1> Sheet1 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-1TALNNC\\MSSQLSERVER02;Database=MacCrawler;Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnownManufacturer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsFromExternal).HasDefaultValueSql("((0))");

                entity.Property(e => e.ManufacturerText).HasMaxLength(1000);
            });

            modelBuilder.Entity<KnownModel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsFromExternal).HasDefaultValueSql("((0))");

                entity.Property(e => e.ModelText).HasMaxLength(1000);

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.KnownModel)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK__KnownMode__Manuf__2D27B809");
            });

            modelBuilder.Entity<KnownReferenceNumber>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsFromExternal).HasDefaultValueSql("((0))");

                entity.Property(e => e.ReferenceNumberText).HasMaxLength(1000);

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.KnownReferenceNumber)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK__KnownRefe__Manuf__2E1BDC42");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.KnownReferenceNumber)
                    .HasForeignKey(d => d.ModelId)
                    .HasConstraintName("FK__KnownRefe__Model__2F10007B");
            });

            modelBuilder.Entity<Languages>(entity =>
            {
                entity.HasKey(e => e.IdLanguage);

                entity.Property(e => e.IdLanguage)
                    .HasColumnName("idLanguage")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Sheet1>(entity =>
            {
                entity.HasKey(e => e.Pid)
                    .HasName("PK__Sheet1$__C57755406CAFC81F");

                entity.ToTable("Sheet1$");

                entity.Property(e => e.Pid).HasColumnName("PId");

                entity.Property(e => e.Count).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
