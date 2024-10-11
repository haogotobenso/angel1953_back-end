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

    public virtual DbSet<Information> Information { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<Platform> Platform { get; set; }

    public virtual DbSet<PlatformType> PlatformType { get; set; }

    public virtual DbSet<Question> Question { get; set; }

    public virtual DbSet<Recovery> Recovery { get; set; }

    public virtual DbSet<RecoveryRecord> RecoveryRecord { get; set; }

    public virtual DbSet<School> School { get; set; }

    public virtual DbSet<VideoLink> VideoLink { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_PRC_CI_AS");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__3DE0C2078222E0C3");

            entity.Property(e => e.Author).HasMaxLength(40);
            entity.Property(e => e.BookName).HasMaxLength(100);
            entity.Property(e => e.PublicDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Bullyinger>(entity =>
        {
            entity.HasKey(e => e.BullyingerId).HasName("PK__Bullying__1DCD3B6DBF9E874E");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FBurl).IsUnicode(false);
            entity.Property(e => e.FirstDate).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Bullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK__Bullyinge__Accou__7849DB76");
        });

        modelBuilder.Entity<BullyingerPost>(entity =>
        {
            entity.HasKey(e => e.BPId).HasName("PK__Bullying__3876B6AC479D94A3");

            entity.Property(e => e.Bullyinger)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FBurl).IsUnicode(false);
            entity.Property(e => e.KeyWord).HasMaxLength(100);
            entity.Property(e => e.PostTime).HasColumnType("datetime");
            entity.Property(e => e.Posturl).IsUnicode(false);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.Property(e => e.Class1)
                .IsUnicode(false)
                .HasColumnName("Class");
        });

        modelBuilder.Entity<CrawlerLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__CrawlerL__2D1221352D61BC40");

            entity.Property(e => e.FBLink).IsUnicode(false);
            entity.Property(e => e.LinkName).HasMaxLength(50);
        });

        modelBuilder.Entity<ExternalLinks>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__External__2D122135DE039159");

            entity.Property(e => e.Link).IsUnicode(false);
            entity.Property(e => e.LinkTime).HasColumnType("datetime");
            entity.Property(e => e.Title).IsUnicode(false);
        });

        modelBuilder.Entity<Information>(entity =>
        {
            entity.HasKey(e => e.InformationId).HasName("PK__Informat__C93C35B0EA51F67A");

            entity.Property(e => e.Information1)
                .IsUnicode(false)
                .HasColumnName("Information");

            entity.HasOne(d => d.Question).WithMany(p => p.Information)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Informati__Quest__440B1D61");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__Member__B0C3AC47A399B3AA");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AuthCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.FBurl).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.StudentId).IsUnicode(false);
            entity.Property(e => e.TeacherImg).HasColumnType("image");

            entity.HasOne(d => d.Class).WithMany(p => p.Member)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Member__ClassId__625A9A57");

            entity.HasOne(d => d.School).WithMany(p => p.Member)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK__Member__SchoolId__634EBE90");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId).HasName("PK__Platform__F559F6FA3F242ED8");

            entity.Property(e => e.PlatformName).IsUnicode(false);

            entity.HasOne(d => d.PlatformType).WithMany(p => p.Platform)
                .HasForeignKey(d => d.PlatformTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platform__Platfo__5070F446");
        });

        modelBuilder.Entity<PlatformType>(entity =>
        {
            entity.HasKey(e => e.PlatformTypeId).HasName("PK__Platform__05A7A707FFC49041");

            entity.Property(e => e.PlatformTypeName).IsUnicode(false);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACBD8D5BE0");

            entity.Property(e => e.Answer).IsUnicode(false);
            entity.Property(e => e.Question1)
                .IsUnicode(false)
                .HasColumnName("Question");
        });

        modelBuilder.Entity<Recovery>(entity =>
        {
            entity.HasKey(e => e.RecoveryId).HasName("PK__Recovery__EE4C84AC2B6A7647");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<RecoveryRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Recovery__FBDF78E99C81D181");

            entity.Property(e => e.UserAnswer).IsUnicode(false);

            entity.HasOne(d => d.Question).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecoveryR__Quest__412EB0B6");

            entity.HasOne(d => d.Recovery).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.RecoveryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecoveryR__Recov__403A8C7D");
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
            entity.HasKey(e => e.VideoId).HasName("PK__VideoLin__BAE5126A88411C05");

            entity.Property(e => e.VideoImg).HasColumnType("image");
            entity.Property(e => e.VideoLink1)
                .IsUnicode(false)
                .HasColumnName("VideoLink");
            entity.Property(e => e.VideoName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
