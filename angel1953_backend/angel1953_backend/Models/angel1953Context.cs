using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__3DE0C207AC798BE6");

            entity.Property(e => e.Author).HasMaxLength(50);
            entity.Property(e => e.BookName).HasMaxLength(100);
            entity.Property(e => e.PublicDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Bullyinger>(entity =>
        {
            entity.HasKey(e => e.BullyingerId).HasName("PK__Bullying__1DCD3B6DF43DE381");

            entity.Property(e => e.BullyingerId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Bullyinger1)
                .HasMaxLength(100)
                .HasColumnName("Bullyinger");
            entity.Property(e => e.FBurl).IsUnicode(false);
            entity.Property(e => e.FirstDate).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Bullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK__Bullyinge__Accou__36B12243");
        });

        modelBuilder.Entity<BullyingerPost>(entity =>
        {
            entity.HasKey(e => e.BPId).HasName("PK__Bullying__3876B6ACBBE09D6D");

            entity.Property(e => e.BullyingerId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.KeyWord).HasMaxLength(100);
            entity.Property(e => e.PostTime).HasColumnType("datetime");
            entity.Property(e => e.Posturl).IsUnicode(false);

            entity.HasOne(d => d.Bullyinger).WithMany(p => p.BullyingerPost)
                .HasForeignKey(d => d.BullyingerId)
                .HasConstraintName("FK__Bullyinge__Bully__398D8EEE");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927C059829CC7");

            entity.Property(e => e.Class1)
                .HasMaxLength(100)
                .HasColumnName("Class");
        });

        modelBuilder.Entity<CrawlerLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__CrawlerL__2D122135D05E18B9");

            entity.Property(e => e.FBLink).IsUnicode(false);
            entity.Property(e => e.LinkName).HasMaxLength(200);
        });

        modelBuilder.Entity<ExternalLinks>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__External__2D12213587DC9A44");

            entity.Property(e => e.Link).IsUnicode(false);
            entity.Property(e => e.LinkTime).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__Member__B0C3AC47A2A95B0F");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AuthCode)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FBurl).IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeacherImg).HasColumnType("image");

            entity.HasOne(d => d.Class).WithMany(p => p.Member)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Member__ClassId__267ABA7A");

            entity.HasOne(d => d.School).WithMany(p => p.Member)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Member_SchoolId");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FAC824D2DA8");

            entity.Property(e => e.Question1).HasColumnName("Question");
        });

        modelBuilder.Entity<Recovery>(entity =>
        {
            entity.HasKey(e => e.RecoveryId).HasName("PK__Recovery__EE4C84AC9DCBEF4A");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Recovery)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK__Recovery__Accoun__2B3F6F97");
        });

        modelBuilder.Entity<RecoveryRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Recovery__FBDF78E9930629E0");

            entity.HasOne(d => d.Question).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__RecoveryR__Quest__2F10007B");

            entity.HasOne(d => d.Recovery).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.RecoveryId)
                .HasConstraintName("FK__RecoveryR__Recov__2E1BDC42");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(e => e.SchoolId).ValueGeneratedNever();
            entity.Property(e => e.School1)
                .HasMaxLength(50)
                .HasColumnName("School");
        });

        modelBuilder.Entity<VideoLink>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__VideoLin__BAE5126A1B1677AB");

            entity.Property(e => e.LinkClick).HasDefaultValue(0);
            entity.Property(e => e.VideoImg).HasColumnType("image");
            entity.Property(e => e.VideoLink1)
                .IsUnicode(false)
                .HasColumnName("VideoLink");
            entity.Property(e => e.VideoName).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
