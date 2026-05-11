using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nipton.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class PrerequisitionAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "CourseStudents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SubjectPrerequisites",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    PrerequisiteSubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectPrerequisites", x => new { x.SubjectId, x.PrerequisiteSubjectId });
                    table.ForeignKey(
                        name: "FK_SubjectPrerequisites_Subjects_PrerequisiteSubjectId",
                        column: x => x.PrerequisiteSubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectPrerequisites_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectPrerequisites_PrerequisiteSubjectId",
                table: "SubjectPrerequisites",
                column: "PrerequisiteSubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectPrerequisites");

            migrationBuilder.DropColumn(
                name: "Passed",
                table: "CourseStudents");
        }
    }
}
