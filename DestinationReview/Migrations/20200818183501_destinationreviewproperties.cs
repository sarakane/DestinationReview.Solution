using Microsoft.EntityFrameworkCore.Migrations;

namespace DestinationReview.Migrations
{
    public partial class destinationreviewproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ReviewAverage",
                table: "Destinations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewNumber",
                table: "Destinations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewAverage",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "ReviewNumber",
                table: "Destinations");
        }
    }
}
