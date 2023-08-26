using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Faliush.ContactManager.Infrastructure.Migrations
{
    public partial class AddPhoneNumberToPersonEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "People");
        }
    }
}
