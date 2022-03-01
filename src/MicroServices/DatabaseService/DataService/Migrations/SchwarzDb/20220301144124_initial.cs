using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataService.Migrations.SchwarzDb
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "varchar(300)", nullable: false),
                    CommercialRegistrationNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Language = table.Column<string>(type: "varchar(100)", nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", nullable: false),
                    Region = table.Column<string>(type: "varchar(100)", nullable: false),
                    District = table.Column<string>(type: "varchar(100)", nullable: false),
                    PostalCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    City = table.Column<string>(type: "varchar(100)", nullable: false),
                    Street = table.Column<string>(type: "varchar(100)", nullable: false),
                    HouseNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Building = table.Column<string>(type: "varchar(100)", nullable: false),
                    Floor = table.Column<string>(type: "varchar(100)", nullable: false),
                    Room = table.Column<string>(type: "varchar(100)", nullable: false),
                    POBox = table.Column<string>(type: "varchar(100)", nullable: true),
                    PhoneNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    FaxNumber = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Website = table.Column<string>(type: "varchar(200)", nullable: false),
                    VendorType = table.Column<string>(type: "varchar(100)", nullable: false),
                    AccountGroup = table.Column<string>(type: "varchar(200)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(max)", nullable: false),
                    VatNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    DocumentIDs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVendorInitiated = table.Column<bool>(type: "bit", nullable: true),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GivenDocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActualFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalFlows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproverEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverLevel = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    IsDuplicateEntry = table.Column<bool>(type: "bit", nullable: false),
                    LevelApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalFlows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalFlows_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", nullable: false),
                    BankKey = table.Column<string>(type: "varchar(100)", nullable: false),
                    BankName = table.Column<string>(type: "varchar(100)", nullable: false),
                    SwiftCode = table.Column<string>(type: "varchar(100)", nullable: false),
                    BankAccount = table.Column<string>(type: "varchar(100)", nullable: false),
                    AccountHolderName = table.Column<string>(type: "varchar(100)", nullable: false),
                    IBAN = table.Column<string>(type: "varchar(100)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(150)", nullable: false),
                    LastName = table.Column<string>(type: "varchar(150)", nullable: false),
                    Address = table.Column<string>(type: "varchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "varchar(100)", nullable: false),
                    Department = table.Column<string>(type: "varchar(100)", nullable: false),
                    PhoneNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    MobileNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    FaxNo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Language = table.Column<string>(type: "varchar(100)", nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalFlows_CompanyID",
                table: "ApprovalFlows",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CompanyID",
                table: "Banks",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CompanyID",
                table: "Contacts",
                column: "CompanyID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalFlows");

            migrationBuilder.DropTable(
                name: "ApprovalLevels");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "DocumentDetails");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
