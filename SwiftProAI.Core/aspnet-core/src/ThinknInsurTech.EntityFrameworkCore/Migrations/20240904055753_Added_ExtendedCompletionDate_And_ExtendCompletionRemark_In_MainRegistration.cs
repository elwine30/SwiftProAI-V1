using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThinknInsurTech.Migrations
{
    /// <inheritdoc />
    public partial class Added_ExtendedCompletionDate_And_ExtendCompletionRemark_In_MainRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtendCompletionRemark",
                table: "MainRegistration",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExtendedCompletionDate",
                table: "MainRegistration",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtendCompletionRemark",
                table: "MainRegistration");

            migrationBuilder.DropColumn(
                name: "ExtendedCompletionDate",
                table: "MainRegistration");
        }
    }
}
