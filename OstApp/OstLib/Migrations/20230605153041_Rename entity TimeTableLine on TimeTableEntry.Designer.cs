﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OstLib;

namespace OstLib.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230605153041_Rename entity TimeTableLine on TimeTableEntry")]
    partial class RenameentityTimeTableLineonTimeTableEntry
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("OstLib.Appointment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClientID")
                        .HasColumnType("integer");

                    b.Property<string>("Complaint")
                        .HasColumnType("text");

                    b.Property<string>("Heal")
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("OstLib.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Anamnez")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Ginekologia")
                        .HasColumnType("text");

                    b.Property<string>("Injury")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Operation")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<string>("YearOfBirth")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("OstLib.TimeTableEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClientID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.ToTable("TimeTableEntry");
                });

            modelBuilder.Entity("OstLib.Appointment", b =>
                {
                    b.HasOne("OstLib.Client", "Client")
                        .WithMany("Appointments")
                        .HasForeignKey("ClientID");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("OstLib.TimeTableEntry", b =>
                {
                    b.HasOne("OstLib.Client", "Client")
                        .WithMany("TimeTableLines")
                        .HasForeignKey("ClientID");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("OstLib.Client", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("TimeTableLines");
                });
#pragma warning restore 612, 618
        }
    }
}
