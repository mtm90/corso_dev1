﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokerAppMVC.Models;

#nullable disable

namespace PokerAppMVC.Migrations
{
    [DbContext(typeof(PokerDbContext))]
    [Migration("20240927134320_testcommunity")]
    partial class testcommunity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("PokerAppMVC.Models.Hand", b =>
                {
                    b.Property<int>("HandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ComputerHand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ComputerStack")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerHand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerStack")
                        .HasColumnType("INTEGER");

                    b.HasKey("HandId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Hands");
                });

            modelBuilder.Entity("PokerAppMVC.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("PokerAppMVC.Models.Hand", b =>
                {
                    b.HasOne("PokerAppMVC.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
