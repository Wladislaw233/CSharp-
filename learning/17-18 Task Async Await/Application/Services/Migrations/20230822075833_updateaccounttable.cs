using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Migrations
{
    /// <inheritdoc />
    public partial class updateaccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_client_id",
                table: "Accounts",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Clients_client_id",
                table: "Accounts",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Clients_client_id",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_client_id",
                table: "Accounts");
        }
    }
}
