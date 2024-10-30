﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PROG5_NinjaManager;

#nullable disable

namespace PROG5_NinjaManager.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PROG5_NinjaManager.Ninja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ninjas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Shadow",
                            SkillLevel = 5
                        },
                        new
                        {
                            Id = 2,
                            Name = "Blade",
                            SkillLevel = 7
                        });
                });

            modelBuilder.Entity("PROG5_NinjaManager.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Damage")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NinjaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NinjaId");

                    b.ToTable("Weapons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Damage = 10,
                            Name = "Katana",
                            NinjaId = 1
                        },
                        new
                        {
                            Id = 2,
                            Damage = 5,
                            Name = "Shuriken",
                            NinjaId = 2
                        });
                });

            modelBuilder.Entity("PROG5_NinjaManager.Weapon", b =>
                {
                    b.HasOne("PROG5_NinjaManager.Ninja", "Ninja")
                        .WithMany("Weapons")
                        .HasForeignKey("NinjaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ninja");
                });

            modelBuilder.Entity("PROG5_NinjaManager.Ninja", b =>
                {
                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}
