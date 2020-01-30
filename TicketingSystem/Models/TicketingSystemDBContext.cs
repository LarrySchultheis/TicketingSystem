using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TicketingSystem.Models
{
    public partial class TicketingSystemDBContext : DbContext
    {
        public TicketingSystemDBContext()
        {
        }

        public TicketingSystemDBContext(DbContextOptions<TicketingSystemDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmployeeType> EmployeeType { get; set; }
        public virtual DbSet<JobType> JobType { get; set; }
        public virtual DbSet<TicketData> TicketData { get; set; }
        public virtual DbSet<TicketDataLog> TicketDataLog { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=TicketingSystemDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeType>(entity =>
            {
                entity.Property(e => e.EmployeeTypeId).HasColumnName("EmployeeTypeID");

                entity.Property(e => e.EmployeeType1)
                    .IsRequired()
                    .HasColumnName("EmployeeType")
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<JobType>(entity =>
            {
                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TicketData>(entity =>
            {
                entity.HasKey(e => e.EntryId);

                entity.Property(e => e.EntryId).HasColumnName("EntryID");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EntryAuthorId).HasColumnName("EntryAuthorID");

                entity.Property(e => e.EntryDate).HasColumnType("date");

                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");

                entity.Property(e => e.PalletType)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.StageNum)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.TicketWorkerId).HasColumnName("TicketWorkerID");

                entity.Property(e => e.WorkerName)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.EntryAuthor)
                    .WithMany(p => p.TicketDataEntryAuthor)
                    .HasForeignKey(d => d.EntryAuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketDat__Entry__02FC7413");

                entity.HasOne(d => d.JobType)
                    .WithMany(p => p.TicketData)
                    .HasForeignKey(d => d.JobTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketDat__JobTy__02084FDA");

                entity.HasOne(d => d.TicketWorker)
                    .WithMany(p => p.TicketDataTicketWorker)
                    .HasForeignKey(d => d.TicketWorkerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketDat__Ticke__04E4BC85");
            });

            modelBuilder.Entity<TicketDataLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.ChangeTime).HasColumnType("datetime");

                entity.Property(e => e.DataAction)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Details).IsUnicode(false);

                entity.Property(e => e.EntryId).HasColumnName("EntryID");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Auth0Uid)
                    .HasColumnName("Auth0UID")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FullName)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.ShiftType)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });
        }
    }
}
