using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoadmapDesigner.Server.Models.Entities;

public partial class RoadmapContext : DbContext
{
    public RoadmapContext()
    {
    }

    public RoadmapContext(DbContextOptions<RoadmapContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DirectionTraining> DirectionTrainings { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<DisciplinesSpecialization> DisciplinesSpecializations { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VersionsDirectionTraining> VersionsDirectionTrainings { get; set; }

    public virtual DbSet<VersionsDirectionTrainingUser> VersionsDirectionTrainingUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DirectionTraining>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("DirectionTrainning_pkey");

            entity.ToTable("DirectionTraining");

            entity.Property(e => e.Code).HasMaxLength(8);
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("Disciplines_pkey");

            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UUID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<DisciplinesSpecialization>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("Disciplines_Specialization_pkey");

            entity.ToTable("Disciplines_Specialization");

            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UUID");
            entity.Property(e => e.DisciplinesUuid).HasColumnName("Disciplines_UUID");
            entity.Property(e => e.SpecializationUuid).HasColumnName("Specialization_UUID");

            entity.HasOne(d => d.DisciplinesUu).WithMany(p => p.DisciplinesSpecializations)
                .HasForeignKey(d => d.DisciplinesUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Disciplines_Specialization_Disciplines_UUID_fkey");

            entity.HasOne(d => d.SpecializationUu).WithMany(p => p.DisciplinesSpecializations)
                .HasForeignKey(d => d.SpecializationUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Disciplines_Specialization_Specialization_UUID_fkey");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("Specialization_pkey");

            entity.ToTable("Specialization");

            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UUID");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.RoadmapJson)
                .HasColumnType("json")
                .HasColumnName("RoadmapJSON");
            entity.Property(e => e.VersionsDirectionTrainningUuid).HasColumnName("VersionsDirectionTrainning_UUID");

            entity.HasOne(d => d.VersionsDirectionTrainningUu).WithMany(p => p.Specializations)
                .HasForeignKey(d => d.VersionsDirectionTrainningUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Specialization_VersionsDirectionTrainning_UUID_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_UserID_PK");

            entity.HasIndex(e => e.Email, "Users_Email_UK").IsUnique();

            entity.HasIndex(e => e.Login, "Users_Login_UK").IsUnique();

            entity.HasIndex(e => e.RoleId, "Users_RoleID_UK");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_UserRoles_RoleID_FK");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("UserRoles_pkey");

            entity.HasIndex(e => e.Role, "UserRoles_Role_key").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Role).HasMaxLength(100);
        });

        modelBuilder.Entity<VersionsDirectionTraining>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("VersionsDirectionTrainning_pkey");

            entity.ToTable("VersionsDirectionTraining");

            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UUID");
            entity.Property(e => e.Code).HasMaxLength(8);
            entity.Property(e => e.FormEducation).HasMaxLength(50);
            entity.Property(e => e.LevelQualification).HasMaxLength(200);
            entity.Property(e => e.TrainingDepartment).HasMaxLength(200);

            entity.HasOne(d => d.CodeNavigation).WithMany(p => p.VersionsDirectionTrainings)
                .HasForeignKey(d => d.Code)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VersionsDirectionTrainning_Code_fkey");
        });

        modelBuilder.Entity<VersionsDirectionTrainingUser>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("VersionsDirectionTrainning_Users_pkey");

            entity.ToTable("VersionsDirectionTraining_Users");

            entity.Property(e => e.Uuid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("UUID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");
            entity.Property(e => e.VersionsDirectionTrainningUuid).HasColumnName("VersionsDirectionTrainning_UUID");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.VersionsDirectionTrainingUsers)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VersionsDirectionTrainning_Users_Users_UserID_fkey");

            entity.HasOne(d => d.VersionsDirectionTrainningUu).WithMany(p => p.VersionsDirectionTrainingUsers)
                .HasForeignKey(d => d.VersionsDirectionTrainningUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("VersionsDirectionTrainning_Us_VersionsDirectionTrainning_U_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
