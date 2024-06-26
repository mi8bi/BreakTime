﻿// <auto-generated />
using BreakTimeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BreakTimeApp.Migrations.SelectNotifyDb
{
    [DbContext(typeof(SelectNotifyDbContext))]
    [Migration("20240610021738_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("BreakTimeApp.Models.SelectNotifyDb", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Mode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("SelectNotifyItems");
                });
#pragma warning restore 612, 618
        }
    }
}
