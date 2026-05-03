using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerGuidancePlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureToResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "ResumeData",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "ResumeData");
        }
    }
}
