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

    public virtual DbSet<BeBullyinger> BeBullyinger { get; set; }

    public virtual DbSet<Bullyinger> Bullyinger { get; set; }

    public virtual DbSet<Class> Class { get; set; }

    public virtual DbSet<ExternalLinks> ExternalLinks { get; set; }

    public virtual DbSet<Information> Information { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<Platform> Platform { get; set; }

    public virtual DbSet<PlatformType> PlatformType { get; set; }

    public virtual DbSet<Question> Question { get; set; }

    public virtual DbSet<Recovery> Recovery { get; set; }

    public virtual DbSet<RecoveryRecord> RecoveryRecord { get; set; }

    public virtual DbSet<Report> Report { get; set; }

    public virtual DbSet<ReportDetail> ReportDetail { get; set; }

    public virtual DbSet<ReportType> ReportType { get; set; }

    public virtual DbSet<School> School { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BeBullyinger>(entity =>
        {
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BeBullyinger1)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("BeBullyinger");
            entity.Property(e => e.FBurl).IsUnicode(false);
            entity.Property(e => e.LinkToCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.BeBullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK_BeBullyinger_Member");
        });

        modelBuilder.Entity<Bullyinger>(entity =>
        {
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Bullyinger1)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("Bullyinger");
            entity.Property(e => e.FBurl)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.LinkToCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Bullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK_Bullyinger_Member");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.Property(e => e.Class1)
                .HasMaxLength(25)
                .HasColumnName("Class");
        });

        modelBuilder.Entity<ExternalLinks>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__External__2D122135DE039159");

            entity.Property(e => e.Link)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.LinkTime).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Information>(entity =>
        {
            entity.HasKey(e => e.InformationId).HasName("PK__Informat__C93C35B0A85DE5F0");

            entity.Property(e => e.Information1)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("Information");

            entity.HasOne(d => d.Question).WithMany(p => p.Information)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Informati__Quest__440B1D61");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__Member__B0C3AC4769AECA94");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AuthCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TeacherImg).HasColumnType("image");

            entity.HasOne(d => d.Class).WithMany(p => p.Member)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Member_Class");

            entity.HasOne(d => d.School).WithMany(p => p.Member)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__SchoolId__38996AB5");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId).HasName("PK__Platform__F559F6FA0061683E");

            entity.Property(e => e.PlatformName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.PlatformType).WithMany(p => p.Platform)
                .HasForeignKey(d => d.PlatformTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platform__Platfo__5070F446");

            entity.HasOne(d => d.ReportType).WithMany(p => p.Platform)
                .HasForeignKey(d => d.ReportTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platform__Report__5165187F");
        });

        modelBuilder.Entity<PlatformType>(entity =>
        {
            entity.HasKey(e => e.PlatformTypeId).HasName("PK__Platform__05A7A707FFC49041");

            entity.Property(e => e.PlatformTypeName)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACBD8D5BE0");

            entity.Property(e => e.Answer).IsRequired();
            entity.Property(e => e.Option1).IsRequired();
            entity.Property(e => e.Question1)
                .IsRequired()
                .HasColumnName("Question");
        });

        modelBuilder.Entity<Recovery>(entity =>
        {
            entity.HasKey(e => e.RecoveryId).HasName("PK__Recovery__EE4C84AC2B6A7647");

            entity.Property(e => e.Account)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Recovery)
                .HasForeignKey(d => d.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recovery__Accoun__3D5E1FD2");
        });

        modelBuilder.Entity<RecoveryRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Recovery__FBDF78E9BD9C7E14");

            entity.HasOne(d => d.Question).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecoveryR__Quest__412EB0B6");

            entity.HasOne(d => d.Recovery).WithMany(p => p.RecoveryRecord)
                .HasForeignKey(d => d.RecoveryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RecoveryR__Recov__403A8C7D");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD4805BADB05FD");

            entity.Property(e => e.ReportId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Picture).HasColumnType("image");
            entity.Property(e => e.ReportSource)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.ReportTime).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .IsRequired()
                .IsUnicode(false);

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Report)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK__Report__Account__59063A47");

            entity.HasOne(d => d.Detail).WithMany(p => p.Report)
                .HasForeignKey(d => d.DetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__DetailId__5629CD9C");

            entity.HasOne(d => d.Platform).WithMany(p => p.Report)
                .HasForeignKey(d => d.PlatformId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__Platform__571DF1D5");

            entity.HasOne(d => d.School).WithMany(p => p.Report)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK__Report__SchoolId__59FA5E80");

            entity.HasOne(d => d.Type).WithMany(p => p.Report)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__TypeId__5812160E");
        });

        modelBuilder.Entity<ReportDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__ReportDe__135C316DF0865D3D");

            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.ContentTime).HasColumnType("datetime");

            entity.HasOne(d => d.BeBullyinger).WithMany(p => p.ReportDetail)
                .HasForeignKey(d => d.BeBullyingerId)
                .HasConstraintName("FK_ReportDetail_BeBullyinger");

            entity.HasOne(d => d.Bullyinger).WithMany(p => p.ReportDetail)
                .HasForeignKey(d => d.BullyingerId)
                .HasConstraintName("FK_ReportDetail_Bullyinger");
        });

        modelBuilder.Entity<ReportType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__ReportTy__516F03B5F9A641A7");

            entity.Property(e => e.TypeName)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PK__School__3DA4675BA7B2B194");

            entity.Property(e => e.SchoolName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
