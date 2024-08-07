using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Project2_TechTrendsAPI_38342626.Models
{
    public partial class NWUTechTrendsContext : DbContext
    {
        public NWUTechTrendsContext()
        {
        }

        public NWUTechTrendsContext(DbContextOptions<NWUTechTrendsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<JobTelemetry> JobTelemetries { get; set; } = null!;
        public virtual DbSet<Process> Processes { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=NWUTechTrends; TrustServerCertificate=true; Data Source=DESKTOP-U96UIA2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client", "Config");

                entity.Property(e => e.ClientId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClientID");

                entity.Property(e => e.DateOnboarded).HasColumnType("datetime");
            });

            modelBuilder.Entity<JobTelemetry>(entity =>
            {
                entity.ToTable("JobTelemetry", "Telemetry");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalInfo).IsUnicode(false);

                entity.Property(e => e.BusinessFunction).IsUnicode(false);

                entity.Property(e => e.EntryDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ExcludeFromTimeSaving).HasDefaultValueSql("((0))");

                entity.Property(e => e.Geography).IsUnicode(false);

                entity.Property(e => e.JobId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JobID");

                entity.Property(e => e.ProccesId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ProccesID");

                entity.Property(e => e.QueueId)
                    .IsUnicode(false)
                    .HasColumnName("QueueID");

                entity.Property(e => e.StepDescription).IsUnicode(false);

                entity.Property(e => e.UniqueReference).IsUnicode(false);

                entity.Property(e => e.UniqueReferenceType).IsUnicode(false);
            });

            modelBuilder.Entity<Process>(entity =>
            {
                entity.ToTable("Process", "Config");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("ProcessID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DateSubmitted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DefaultBusinessFunction)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Unspecified')");

                entity.Property(e => e.DefaultGeography)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Global')");

                entity.Property(e => e.Platform)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessConfigUrl)
                    .IsUnicode(false)
                    .HasColumnName("ProcessConfigURL");

                entity.Property(e => e.ProcessName).IsUnicode(false);

                entity.Property(e => e.ProcessType).IsUnicode(false);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ReportUrl)
                    .IsUnicode(false)
                    .HasColumnName("ReportURL");

                entity.Property(e => e.Submitter).IsUnicode(false);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project", "Config");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("ProjectID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ProjectCreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(dateadd(hour,(2),getdate()))");

                entity.Property(e => e.ProjectDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectStatus)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
