using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Upward.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addResumeRelatedAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resume",
                table: "CandidateProfiles",
                newName: "ResumeUrl");

            migrationBuilder.AddColumn<string>(
                name: "ResumePublicId",
                table: "CandidateProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumePublicId",
                table: "CandidateProfiles");

            migrationBuilder.RenameColumn(
                name: "ResumeUrl",
                table: "CandidateProfiles",
                newName: "Resume");
        }
    }
}
