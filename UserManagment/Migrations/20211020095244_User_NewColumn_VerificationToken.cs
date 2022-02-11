using Microsoft.EntityFrameworkCore.Migrations;

namespace Manager.Migrations
{
    public partial class User_NewColumn_VerificationToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerifcationToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifcationToken",
                table: "Users");
        }
    }
}
