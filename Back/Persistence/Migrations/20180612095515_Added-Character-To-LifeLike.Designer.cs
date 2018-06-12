﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

namespace Persistence.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180612095515_Added-Character-To-LifeLike")]
    partial class AddedCharacterToLifeLike
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Persistence.Entities.LifeLike", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Character");

                    b.Property<string>("ForBirth");

                    b.Property<string>("ForSurvival");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LifeLikes");
                });
#pragma warning restore 612, 618
        }
    }
}
