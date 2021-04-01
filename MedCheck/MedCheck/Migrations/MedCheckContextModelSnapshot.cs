﻿// <auto-generated />
using System;
using MedCheck.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedCheck.Migrations
{
    [DbContext(typeof(MedCheckContext))]
    partial class MedCheckContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HospitalMedWorker", b =>
                {
                    b.Property<long>("HospitalsHospitalId")
                        .HasColumnType("bigint");

                    b.Property<long>("MedWorkersMedId")
                        .HasColumnType("bigint");

                    b.HasKey("HospitalsHospitalId", "MedWorkersMedId");

                    b.HasIndex("MedWorkersMedId");

                    b.ToTable("HospitalMedWorker");
                });

            modelBuilder.Entity("MedCheck.Models.Hospital", b =>
                {
                    b.Property<long>("HospitalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SecretCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("HospitalId");

                    b.HasIndex("UserId");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("MedCheck.Models.MedWorker", b =>
                {
                    b.Property<long>("MedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MedId");

                    b.ToTable("MedWorkers");
                });

            modelBuilder.Entity("MedCheck.Models.Speciality", b =>
                {
                    b.Property<long>("SpecialityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SpecialityId");

                    b.ToTable("Specialities");
                });

            modelBuilder.Entity("MedCheck.Models.Stats", b =>
                {
                    b.Property<long>("StatsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("OxygenLevel")
                        .HasColumnType("float");

                    b.Property<double>("Pressure")
                        .HasColumnType("float");

                    b.Property<double>("Pulse")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("StatsId");

                    b.HasIndex("UserId");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("MedCheck.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FamilyID")
                        .HasColumnType("bigint");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MedWorkerSpeciality", b =>
                {
                    b.Property<long>("MedWorkersMedId")
                        .HasColumnType("bigint");

                    b.Property<long>("SpecialitiesSpecialityId")
                        .HasColumnType("bigint");

                    b.HasKey("MedWorkersMedId", "SpecialitiesSpecialityId");

                    b.HasIndex("SpecialitiesSpecialityId");

                    b.ToTable("MedWorkerSpeciality");
                });

            modelBuilder.Entity("MedWorkerUser", b =>
                {
                    b.Property<long>("MedWorkersMedId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersUserId")
                        .HasColumnType("bigint");

                    b.HasKey("MedWorkersMedId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("MedWorkerUser");
                });

            modelBuilder.Entity("HospitalMedWorker", b =>
                {
                    b.HasOne("MedCheck.Models.Hospital", null)
                        .WithMany()
                        .HasForeignKey("HospitalsHospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedCheck.Models.MedWorker", null)
                        .WithMany()
                        .HasForeignKey("MedWorkersMedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedCheck.Models.Hospital", b =>
                {
                    b.HasOne("MedCheck.Models.User", null)
                        .WithMany("Hospitals")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("MedCheck.Models.Stats", b =>
                {
                    b.HasOne("MedCheck.Models.User", "User")
                        .WithMany("Stats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MedWorkerSpeciality", b =>
                {
                    b.HasOne("MedCheck.Models.MedWorker", null)
                        .WithMany()
                        .HasForeignKey("MedWorkersMedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedCheck.Models.Speciality", null)
                        .WithMany()
                        .HasForeignKey("SpecialitiesSpecialityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedWorkerUser", b =>
                {
                    b.HasOne("MedCheck.Models.MedWorker", null)
                        .WithMany()
                        .HasForeignKey("MedWorkersMedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedCheck.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedCheck.Models.User", b =>
                {
                    b.Navigation("Hospitals");

                    b.Navigation("Stats");
                });
#pragma warning restore 612, 618
        }
    }
}