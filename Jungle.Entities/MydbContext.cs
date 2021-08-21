using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class MydbContext : DbContext
    {
       

        public MydbContext(DbContextOptions<MydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Gate> Gate { get; set; }
        public virtual DbSet<IdentityProof> IdentityProof { get; set; }
        public virtual DbSet<Parks> Parks { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SafariDetail> SafariDetail { get; set; }
        public virtual DbSet<Tourist> Tourist { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Server=DESKTOP-03SVV1S\\SQLEXPRESS;Database=Mydb;Trusted_Connection=True;");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Pid).HasColumnName("PId");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.TotalCost).HasColumnType("money");

                entity.HasOne(d => d.Gate)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.GateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__GateId__69C6B1F5");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.Pid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__PId__67DE6983");

                entity.HasOne(d => d.Safari)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.SafariId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__SafariI__68D28DBC");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Booking__Vehicle__6ABAD62E");
            });

            modelBuilder.Entity<Gate>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Park)
                    .WithMany(p => p.Gate)
                    .HasForeignKey(d => d.ParkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkID");
            });

            modelBuilder.Entity<IdentityProof>(entity =>
            {
                entity.HasKey(e => e.IdentityId)
                    .HasName("PK__Identity__30667A45EB8BBBA7");

                entity.Property(e => e.IdentityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Parks>(entity =>
            {
                entity.HasKey(e => e.ParkId)
                    .HasName("PK__Parks__7D67D34C70E145E0");

                entity.Property(e => e.Fee).HasColumnType("money");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PayId)
                    .HasName("PK__Payment__EE8FCECF9837069A");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.Park)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.ParkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__ParkId__625A9A57");

                entity.HasOne(d => d.Safari)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.SafariId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__SafariI__634EBE90");

                entity.HasOne(d => d.Tourist)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.TouristId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__Tourist__6166761E");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.VehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__Vehicle__6442E2C9");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<SafariDetail>(entity =>
            {
                entity.HasKey(e => e.SafariId)
                    .HasName("PK__SafariDe__7E867CDA4EE88C11");

                entity.Property(e => e.SafariCost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SafariDate).HasColumnType("date");

                entity.Property(e => e.SafariName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SafariTime)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Park)
                    .WithMany(p => p.SafariDetail)
                    .HasForeignKey(d => d.ParkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SafariDet__ParkI__5DCAEF64");
            });

            modelBuilder.Entity<Tourist>(entity =>
            {
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdentityName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.IdentityNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__1788CC4C28DB4921");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleId__40C49C62");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Vid)
                    .HasName("PK__Vehicle__C5DF235B53B8DC78");

                entity.Property(e => e.Vid).HasColumnName("VId");

                entity.Property(e => e.Capacity).HasMaxLength(20);

                entity.Property(e => e.EntryCost).HasColumnType("money");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Vtype)
                    .IsRequired()
                    .HasColumnName("VType")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
