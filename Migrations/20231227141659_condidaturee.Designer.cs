﻿// <auto-generated />
using System;
using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace App.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231227141659_condidaturee")]
    partial class condidaturee
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("App.Models.Auth.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("age")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("number")
                        .HasColumnType("int");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("App.Models.Candidature", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"), 1L, 1);

                    b.Property<int?>("OffreId")
                        .HasColumnType("int");

                    b.Property<string>("Statut")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OffreId");

                    b.HasIndex("UserId");

                    b.ToTable("Candidatures");
                });

            modelBuilder.Entity("App.Models.Offre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Competance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remuneration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Responsabilite")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salaire")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Offres");
                });

            modelBuilder.Entity("App.Models.Candidature", b =>
                {
                    b.HasOne("App.Models.Offre", "Offre")
                        .WithMany("Candidatures")
                        .HasForeignKey("OffreId");

                    b.HasOne("App.Models.Auth.User", "User")
                        .WithMany("Candidatures")
                        .HasForeignKey("UserId");

                    b.Navigation("Offre");

                    b.Navigation("User");
                });

            modelBuilder.Entity("App.Models.Offre", b =>
                {
                    b.HasOne("App.Models.Auth.User", "User")
                        .WithMany("Offres")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("App.Models.Auth.User", b =>
                {
                    b.Navigation("Candidatures");

                    b.Navigation("Offres");
                });

            modelBuilder.Entity("App.Models.Offre", b =>
                {
                    b.Navigation("Candidatures");
                });
#pragma warning restore 612, 618
        }
    }
}
