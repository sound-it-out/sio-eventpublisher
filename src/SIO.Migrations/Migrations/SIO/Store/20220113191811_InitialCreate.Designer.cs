﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;

#nullable disable

namespace SIO.Migrations.Migrations.SIO.Store
{
    [DbContext(typeof(SIOStoreDbContext))]
    [Migration("20220113191811_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SIO.Infrastructure.EntityFrameworkCore.Entities.Command", b =>
                {
                    b.Property<long>("SequenceNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SequenceNo"), 1L, 1);

                    b.Property<string>("CorrelationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SequenceNo");

                    b.HasIndex("CorrelationId");

                    b.HasIndex("Id");

                    b.HasIndex("Name");

                    b.HasIndex("Subject");

                    b.HasIndex("UserId");

                    b.ToTable("Command", "log");
                });

            modelBuilder.Entity("SIO.Infrastructure.EntityFrameworkCore.Entities.Event", b =>
                {
                    b.Property<long>("SequenceNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SequenceNo"), 1L, 1);

                    b.Property<string>("Actor")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CausationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CorrelationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("ScheduledPublication")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("StreamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SequenceNo");

                    b.HasIndex("Actor");

                    b.HasIndex("CausationId");

                    b.HasIndex("CorrelationId");

                    b.HasIndex("Id");

                    b.HasIndex("Name");

                    b.HasIndex("StreamId");

                    b.ToTable("Event", "log");
                });

            modelBuilder.Entity("SIO.Infrastructure.EntityFrameworkCore.Entities.Query", b =>
                {
                    b.Property<long>("SequenceNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SequenceNo"), 1L, 1);

                    b.Property<string>("CorrelationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SequenceNo");

                    b.HasIndex("CorrelationId");

                    b.HasIndex("Id");

                    b.HasIndex("Name");

                    b.HasIndex("UserId");

                    b.ToTable("Query", "log");
                });
#pragma warning restore 612, 618
        }
    }
}