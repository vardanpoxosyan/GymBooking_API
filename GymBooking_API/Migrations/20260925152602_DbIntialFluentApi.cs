using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBooking_API.Migrations
{
    /// <inheritdoc />
    public partial class DbIntialFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Packages_PackageId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_PackageId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PackagId",
                table: "CartItems",
                column: "PackagId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Packages_PackagId",
                table: "CartItems",
                column: "PackagId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Packages_PackagId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_PackagId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_PackageId",
                table: "CartItems",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Packages_PackageId",
                table: "CartItems",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
