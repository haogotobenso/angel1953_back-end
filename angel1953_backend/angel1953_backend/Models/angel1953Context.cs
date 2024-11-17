using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace angel1953_backend.Models;

public partial class angel1953Context : DbContext
{
    public angel1953Context(DbContextOptions<angel1953Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Book { get; set; }
    public virtual DbSet<Bullyinger> Bullyinger { get; set; }
    public virtual DbSet<BullyingerPost> BullyingerPost { get; set; }
    public virtual DbSet<Class> Class { get; set; }
    public virtual DbSet<CrawlerLink> CrawlerLink { get; set; }
    public virtual DbSet<ExternalLinks> ExternalLinks { get; set; }
    public virtual DbSet<Member> Member { get; set; }
    public virtual DbSet<Question> Question { get; set; }
    public virtual DbSet<Recovery> Recovery { get; set; }
    public virtual DbSet<RecoveryRecord> RecoveryRecord { get; set; }
    public virtual DbSet<School> School { get; set; }
    public virtual DbSet<VideoLink> VideoLink { get; set; }
    public virtual DbSet<Todo> Todo {get;set;}
    public virtual DbSet<Scase> Scase {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId);
            entity.Property(e => e.BookId).ValueGeneratedOnAdd();
            entity.Property(e => e.Author).HasMaxLength(50);
            entity.Property(e => e.BookName).HasMaxLength(100);
            entity.Property(e => e.PublicDate).HasColumnType("datetime(6)");
        });

        modelBuilder.Entity<Bullyinger>(entity =>
        {
            entity.HasKey(e => e.BullyingerId);
            entity.Property(e => e.BullyingerId).HasMaxLength(100);
            entity.Property(e => e.Account).HasMaxLength(20);
            entity.Property(e => e.Bullyinger1)
                .HasMaxLength(100)
                .HasColumnName("Bullyinger");
            entity.Property(e => e.FirstDate).HasColumnType("datetime(6)");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Bullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK_Bullyinger_Account");
        });

        modelBuilder.Entity<BullyingerPost>(entity =>
        {
            entity.HasKey(e => e.BPId);
            entity.Property(e => e.BPId).ValueGeneratedOnAdd();
            entity.Property(e => e.BullyingerId).HasMaxLength(100);
            entity.Property(e => e.KeyWord).HasMaxLength(100);
            entity.Property(e => e.PostTime).HasColumnType("datetime(6)");

            entity.HasOne(d => d.Bullyinger).WithMany(p => p.BullyingerPost)
                .HasForeignKey(d => d.BullyingerId)
                .HasConstraintName("FK_BullyingerPost_Bullyinger");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId);
            entity.Property(e => e.ClassId).ValueGeneratedOnAdd();
            entity.Property(e => e.Class1)
                .HasMaxLength(100)
                .HasColumnName("Class");
        });

        modelBuilder.Entity<CrawlerLink>(entity =>
        {
            entity.HasKey(e => e.LinkId);
            entity.Property(e => e.LinkId).ValueGeneratedOnAdd();
            entity.Property(e => e.LinkName).HasMaxLength(200);
        });

        modelBuilder.Entity<ExternalLinks>(entity =>
        {
            entity.HasKey(e => e.LinkId);
            entity.Property(e => e.LinkId).ValueGeneratedOnAdd();
            entity.Property(e => e.LinkTime).HasColumnType("datetime(6)");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Account);
            entity.Property(e => e.Account).HasMaxLength(20);
            entity.Property(e => e.AuthCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasMaxLength(50);
            entity.Property(e => e.TeacherImg).HasColumnType("longblob");

            entity.HasOne(d => d.Class).WithMany(p => p.Member)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Member_Class");

            entity.HasOne(d => d.School).WithMany(p => p.Member)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Member_School");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId);
            entity.Property(e => e.QuestionId).ValueGeneratedOnAdd();
            entity.Property(e => e.Question1).HasColumnName("Question");
        });

        modelBuilder.Entity<Recovery>(entity =>
        {
            entity.HasKey(e => e.RecoveryId);
            entity.Property(e => e.RecoveryId).ValueGeneratedOnAdd();
            entity.Property(e => e.Account).HasMaxLength(20);
            entity.Property(e => e.Time).HasColumnType("datetime(6)");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Recovery)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK_Recovery_Account");
        });

        modelBuilder.Entity<RecoveryRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId);
            entity.Property(e => e.RecordId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Question).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_RecoveryRecord_Question");

            entity.HasOne(d => d.Recovery).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.RecoveryId)
                .HasConstraintName("FK_RecoveryRecord_Recovery");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.SchoolId);
            entity.Property(e => e.SchoolId).ValueGeneratedNever();
            entity.Property(e => e.School1)
                .HasMaxLength(50)
                .HasColumnName("School");
        });

        modelBuilder.Entity<VideoLink>(entity =>
        {
            entity.HasKey(e => e.VideoId);
            entity.Property(e => e.VideoId).ValueGeneratedOnAdd();
            entity.Property(e => e.LinkClick).HasDefaultValue(0);
            entity.Property(e => e.VideoImg).HasColumnType("longblob");
            entity.Property(e => e.VideoLink1).HasColumnName("VideoLink");
            entity.Property(e => e.VideoName).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}