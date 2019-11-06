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

        public virtual DbSet<JobType> JobType { get; set; }
        public virtual DbSet<TicketData> TicketData { get; set; }
        public virtual DbSet<TicketDataLog> TicketDataLog { get; set; }

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
            modelBuilder.Entity<JobType>(entity =>
            {
                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");

                entity.Property(e => e.JobType1)
                    .IsRequired()
                    .HasColumnName("JobType")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TicketData>(entity =>
            {
                entity.HasKey(e => e.EntryId)
                    .HasName("PK__TicketDa__F57BD2D7CD8BE48B");

                entity.Property(e => e.EntryId).HasColumnName("EntryID");

                entity.Property(e => e.Carrier)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments).IsUnicode(false);

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.EntryDate).HasColumnType("date");

                entity.Property(e => e.JobTypeId).HasColumnName("JobTypeID");

                entity.Property(e => e.PalletNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PalletType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StageNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.JobType)
                    .WithMany(p => p.TicketData)
                    .HasForeignKey(d => d.JobTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketDat__JobTy__398D8EEE");
            });

            modelBuilder.Entity<TicketDataLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__TicketDa__5E5499A8F0E756A3");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.ChangeTime).HasColumnType("datetime");

                entity.Property(e => e.DataAction)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Details)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.EntryId).HasColumnName("EntryID");

                entity.HasOne(d => d.Entry)
                    .WithMany(p => p.TicketDataLog)
                    .HasForeignKey(d => d.EntryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TicketDat__Entry__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
