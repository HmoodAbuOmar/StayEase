using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StayEase.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcolumntoapllicationuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeResetPassword",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodeResetPasswordExpiration",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeResetPassword",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CodeResetPasswordExpiration",
                table: "Users");
        }
    }
}
