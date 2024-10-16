﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TropicalExpress.Infrastructure;

#nullable disable

namespace TropicalExpress.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241015230016_PrimitiveTypes")]
    partial class PrimitiveTypes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TropicalExpress.Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("Fruit", "TropicalExpress.Domain.Order.Fruit#Fruit", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FruitType")
                                .IsRequired()
                                .HasColumnType("varchar(50)")
                                .HasColumnName("FruitType");

                            b1.Property<string>("WeightData")
                                .IsRequired()
                                .HasColumnType("varchar(100)")
                                .HasColumnName("WeightData");
                        });

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
