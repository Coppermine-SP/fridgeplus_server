using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fridgeplus_server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expires",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "RecommendedExpirationDays",
                table: "Categories",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendedExpirationDays",
                table: "Categories");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Expires",
                table: "Categories",
                type: "time(6)",
                nullable: true);
        }
    }
}
