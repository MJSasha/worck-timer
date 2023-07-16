﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkTimer.Api;

#nullable disable

namespace WorckTimer.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230716102951_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WorkTimer.Common.Models.WorkPeriod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("EndAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("WorkPeriods");
                });
#pragma warning restore 612, 618
        }
    }
}
