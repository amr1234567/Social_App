using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Social_App.API.Migrations
{
    /// <inheritdoc />
    public partial class addUsersConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersConnections",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ConnectionId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersConnections", x => new { x.UserId, x.ConnectionId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersConnections");
        }
    }
}
