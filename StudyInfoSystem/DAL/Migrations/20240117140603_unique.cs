using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_TeacherId",
                table: "SubjectTeachers");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_TeacherId_SubjectId",
                table: "SubjectTeachers",
                columns: new[] { "TeacherId", "SubjectId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubjectTeachers_TeacherId_SubjectId",
                table: "SubjectTeachers");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_TeacherId",
                table: "SubjectTeachers",
                column: "TeacherId");
        }
    }
}
