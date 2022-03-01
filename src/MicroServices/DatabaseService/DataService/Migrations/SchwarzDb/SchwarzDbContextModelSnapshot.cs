﻿// <auto-generated />
using System;
using DataService.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataService.Migrations.SchwarzDb
{
    [DbContext(typeof(SchwarzDbContext))]
    partial class SchwarzDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DataService.Entities.ApprovalFlow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<string>("ApproverEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ApproverLevel")
                        .HasColumnType("int");

                    b.Property<int>("CompanyID")
                        .HasColumnType("int");

                    b.Property<bool>("IsDuplicateEntry")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LevelApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.ToTable("ApprovalFlows");
                });

            modelBuilder.Entity("DataService.Entities.ApprovalLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ApprovalLevels");
                });

            modelBuilder.Entity("DataService.Entities.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountHolderName")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BankAccount")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BankKey")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("CompanyID")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("IBAN")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("SwiftCode")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("DataService.Entities.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountGroup")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime?>("ApprovedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Building")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CommercialRegistrationNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("varchar(300)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("DocumentIDs")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FaxNumber")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Floor")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("HouseNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsVendorInitiated")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<string>("POBox")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("VatNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("VendorType")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Website")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DataService.Entities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(max)");

                    b.Property<int>("CompanyID")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FaxNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("MobileNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PhoneNo")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("DataService.Entities.DocumentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ActualFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CertificateNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfExpiry")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfIssue")
                        .HasColumnType("datetime2");

                    b.Property<string>("GivenDocumentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniqueFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DocumentDetails");
                });

            modelBuilder.Entity("DataService.Entities.ApprovalFlow", b =>
                {
                    b.HasOne("DataService.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("DataService.Entities.Bank", b =>
                {
                    b.HasOne("DataService.Entities.Company", "Company")
                        .WithMany("ListOfCompanyBanks")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("DataService.Entities.Contact", b =>
                {
                    b.HasOne("DataService.Entities.Company", "Company")
                        .WithMany("ListOfCompanyContacts")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("DataService.Entities.Company", b =>
                {
                    b.Navigation("ListOfCompanyBanks");

                    b.Navigation("ListOfCompanyContacts");
                });
#pragma warning restore 612, 618
        }
    }
}