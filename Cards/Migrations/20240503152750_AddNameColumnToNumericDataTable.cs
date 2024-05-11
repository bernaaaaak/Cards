using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cards.Migrations
{
    /// <inheritdoc />
    public partial class AddNameColumnToNumericDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "NumericData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "NumericData");
        }
    }
}
