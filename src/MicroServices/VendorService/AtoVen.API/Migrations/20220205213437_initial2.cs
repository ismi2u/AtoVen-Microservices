using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtoVen.API.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "ApprovalFlows",
                newName: "IsLevelApproved");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsLevelApproved",
                table: "ApprovalFlows",
                newName: "IsApproved");
        }
    }
}
