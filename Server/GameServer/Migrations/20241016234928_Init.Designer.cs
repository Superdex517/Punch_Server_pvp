﻿// <auto-generated />
using System;
using GameServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameServer.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20241016234928_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GameServer.Game.HeroDb", b =>
                {
                    b.Property<int>("HeroDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HeroDbId"));

                    b.Property<long>("AccountDbId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("HeroDbId");

                    b.HasIndex("AccountDbId");

                    b.ToTable("Heroes");
                });

            modelBuilder.Entity("GameServer.Game.ItemDb", b =>
                {
                    b.Property<long>("ItemDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ItemDbId"));

                    b.Property<long>("AccountDbId")
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("EnchantCount")
                        .HasColumnType("int");

                    b.Property<int>("EquipSlot")
                        .HasColumnType("int");

                    b.Property<int>("OwnerDbId")
                        .HasColumnType("int");

                    b.Property<int>("TemplateId")
                        .HasColumnType("int");

                    b.HasKey("ItemDbId");

                    b.HasIndex("AccountDbId");

                    b.HasIndex("OwnerDbId");

                    b.ToTable("Item");
                });
#pragma warning restore 612, 618
        }
    }
}
