﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SIO.Infrastructure.EntityFrameworkCore.DbContexts;

namespace SIO.Migrations.Migrations.SIO.Projection
{
    [DbContext(typeof(SIOProjectionDbContext))]
    [Migration("20211001143641_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SIO.Domain.EventPublications.Projections.EventPublicationFailure", b =>
                {
                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Subject");

                    b.HasIndex("EventId");

                    b.ToTable("EventPublicationFailure");
                });

            modelBuilder.Entity("SIO.Domain.EventPublications.Projections.EventPublicationQueue", b =>
                {
                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Attempts")
                        .HasColumnType("int");

                    b.Property<string>("CausationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorrelationId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Event")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Subject");

                    b.ToTable("EventPublicationQueue");
                });

            modelBuilder.Entity("SIO.Infrastructure.EntityFrameworkCore.Entities.ProjectionState", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.HasKey("Name");

                    b.ToTable("ProjectionState");
                });
#pragma warning restore 612, 618
        }
    }
}
