﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using fridgeplus_server.Context;

#nullable disable

namespace fridgeplus_server.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    [Migration("20241108153653_SecondMigration")]
    partial class SecondMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("fridgeplus_server.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan?>("Expires")
                        .HasColumnType("time(6)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("fridgeplus_server.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ItemDescription")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ItemExpireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ItemImportDate")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ItemOwner")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ItemQuantity")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.ToTable("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
