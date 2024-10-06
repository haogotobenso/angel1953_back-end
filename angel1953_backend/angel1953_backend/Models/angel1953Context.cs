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

    public virtual DbSet<BullyingDetect> BullyingDetect { get; set; }

    public virtual DbSet<Bullyinger> Bullyinger { get; set; }

    public virtual DbSet<Class> Class { get; set; }

    public virtual DbSet<ExternalLinks> ExternalLinks { get; set; }

    public virtual DbSet<Information> Information { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MidSchool> MidSchool { get; set; }

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
        modelBuilder.UseCollation("Chinese_PRC_CI_AS");

        modelBuilder.Entity<BeBullyinger>(entity =>
        {
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.BeBullyinger1)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("BeBullyinger");
            entity.Property(e => e.FBurl)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LinkToCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.BeBullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK_BeBullyinger_Member");
        });

        modelBuilder.Entity<BullyingDetect>(entity =>
        {
            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.Bullyinger).WithMany(p => p.BullyingDetect)
                .HasForeignKey(d => d.BullyingerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BullyingDetect_Bullyinger");
        });

        modelBuilder.Entity<Bullyinger>(entity =>
        {
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Bullyinger1)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("Bullyinger");
            entity.Property(e => e.FBurl)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LinkToCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Bullyinger)
                .HasForeignKey(d => d.Account)
                .HasConstraintName("FK_Bullyinger_Member");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.Property(e => e.Class1)
                .HasMaxLength(25)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("Class");
        });

        modelBuilder.Entity<ExternalLinks>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("PK__External__2D122135DE039159");

            entity.Property(e => e.Link)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LinkTime).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Information>(entity =>
        {
            entity.HasKey(e => e.InformationId).HasName("PK__Informat__C93C35B0EA51F67A");

            entity.Property(e => e.Information1)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
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
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AuthCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TeacherImg).HasColumnType("image");

            entity.HasOne(d => d.Class).WithMany(p => p.Member)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Member_Class");

            entity.HasOne(d => d.MidSchool).WithMany(p => p.Member)
                .HasForeignKey(d => d.MidSchoolId)
                .HasConstraintName("FK_Member_MidSchool");

            entity.HasOne(d => d.School).WithMany(p => p.Member)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Member_School");
        });

        modelBuilder.Entity<MidSchool>(entity =>
        {
            entity.Property(e => e.MidSchoolId).ValueGeneratedNever();
            entity.Property(e => e.MidSchool1)
                .HasMaxLength(50)
                .HasColumnName("MidSchool");
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.PlatformId).HasName("PK__Platform__F559F6FA3F242ED8");

            entity.Property(e => e.PlatformName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.PlatformType).WithMany(p => p.Platform)
                .HasForeignKey(d => d.PlatformTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platform__Platfo__5070F446");

            entity.HasOne(d => d.ReportType).WithMany(p => p.Platform)
                .HasForeignKey(d => d.ReportTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Platform__Report__10566F31");
        });

        modelBuilder.Entity<PlatformType>(entity =>
        {
            entity.HasKey(e => e.PlatformTypeId).HasName("PK__Platform__05A7A707FFC49041");

            entity.Property(e => e.PlatformTypeName)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06FACBD8D5BE0");

            entity.Property(e => e.Answer).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Option1).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Option2).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Option3).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Question1)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("Question");
        });

        modelBuilder.Entity<Recovery>(entity =>
        {
            entity.HasKey(e => e.RecoveryId).HasName("PK__Recovery__EE4C84AC2B6A7647");

            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.AccountNavigation).WithMany(p => p.Recovery)
                .HasForeignKey(d => d.Account)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recovery__Accoun__3D5E1FD2");
        });

        modelBuilder.Entity<RecoveryRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Recovery__FBDF78E99C81D181");

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
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AccountInfo).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Picture).HasColumnType("image");
            entity.Property(e => e.ReportSource)
                .HasMaxLength(20)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ReportTime).HasColumnType("datetime");
            entity.Property(e => e.Solution).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Url)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

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

            entity.HasOne(d => d.Type).WithMany(p => p.Report)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__TypeId__5812160E");
        });

        modelBuilder.Entity<ReportDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__ReportDe__135C316DF0865D3D");

            entity.Property(e => e.Content).UseCollation("SQL_Latin1_General_CP1_CI_AS");
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
            entity.HasKey(e => e.TypeId).HasName("PK__ReportTy__516F03B5FAC0E85A");

            entity.Property(e => e.TypeName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.Property(e => e.SchoolId).ValueGeneratedNever();
            entity.Property(e => e.School1)
                .HasMaxLength(50)
                .HasColumnName("School");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
