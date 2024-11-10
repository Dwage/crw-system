﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarWorkshopAppASP.Migrations
{
    [DbContext(typeof(CarWorkshopContext))]
    [Migration("20241008205959_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Car", b =>
                {
                    b.Property<int>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CarId"));

                    b.Property<string>("BodyNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("EngineNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FactoryNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("ModelId")
                        .HasColumnType("integer");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("CarId");

                    b.HasIndex("ModelId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarModel", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ModelId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("ModelId");

                    b.ToTable("CarModels");
                });

            modelBuilder.Entity("CarRepair", b =>
                {
                    b.Property<int>("RepairId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RepairId"));

                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MalfunctionId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int?>("WorkshopId")
                        .HasColumnType("integer");

                    b.HasKey("RepairId");

                    b.HasIndex("CarId");

                    b.HasIndex("MalfunctionId");

                    b.HasIndex("TeamId");

                    b.HasIndex("WorkshopId");

                    b.ToTable("CarRepairs");
                });

            modelBuilder.Entity("Malfunction", b =>
                {
                    b.Property<int>("MalfunctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MalfunctionId"));

                    b.Property<decimal>("LaborCost")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("MalfunctionId");

                    b.ToTable("Malfunctions");
                });

            modelBuilder.Entity("SparePart", b =>
                {
                    b.Property<int>("PartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PartId"));

                    b.Property<int>("MalfunctionId")
                        .HasColumnType("integer");

                    b.Property<int>("ModelId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("PartId");

                    b.HasIndex("MalfunctionId");

                    b.HasIndex("ModelId");

                    b.ToTable("SpareParts");
                });

            modelBuilder.Entity("Staff", b =>
                {
                    b.Property<string>("PersonInn")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("Salary")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int>("WorkshopId")
                        .HasColumnType("integer");

                    b.HasKey("PersonInn");

                    b.HasIndex("TeamId");

                    b.HasIndex("WorkshopId");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TeamId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Workshop", b =>
                {
                    b.Property<int>("WorkshopId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WorkshopId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("WorkshopId");

                    b.ToTable("Workshops");
                });

            modelBuilder.Entity("Car", b =>
                {
                    b.HasOne("CarModel", "CarModel")
                        .WithMany("Cars")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarModel");
                });

            modelBuilder.Entity("CarRepair", b =>
                {
                    b.HasOne("Car", "Car")
                        .WithMany("CarRepairs")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Malfunction", "Malfunction")
                        .WithMany("CarRepairs")
                        .HasForeignKey("MalfunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Team", "Team")
                        .WithMany("CarRepairs")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Workshop", null)
                        .WithMany("CarRepairs")
                        .HasForeignKey("WorkshopId");

                    b.Navigation("Car");

                    b.Navigation("Malfunction");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("SparePart", b =>
                {
                    b.HasOne("Malfunction", "Malfunction")
                        .WithMany("SpareParts")
                        .HasForeignKey("MalfunctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarModel", "CarModel")
                        .WithMany("SpareParts")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarModel");

                    b.Navigation("Malfunction");
                });

            modelBuilder.Entity("Staff", b =>
                {
                    b.HasOne("Team", "Team")
                        .WithMany("StaffMembers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Workshop", "Workshop")
                        .WithMany("StaffMembers")
                        .HasForeignKey("WorkshopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("Workshop");
                });

            modelBuilder.Entity("Car", b =>
                {
                    b.Navigation("CarRepairs");
                });

            modelBuilder.Entity("CarModel", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("SpareParts");
                });

            modelBuilder.Entity("Malfunction", b =>
                {
                    b.Navigation("CarRepairs");

                    b.Navigation("SpareParts");
                });

            modelBuilder.Entity("Team", b =>
                {
                    b.Navigation("CarRepairs");

                    b.Navigation("StaffMembers");
                });

            modelBuilder.Entity("Workshop", b =>
                {
                    b.Navigation("CarRepairs");

                    b.Navigation("StaffMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
