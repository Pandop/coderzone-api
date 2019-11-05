using Microsoft.EntityFrameworkCore.Migrations;

namespace CoderzoneGrapQLAPI.Migrations
{
    public partial class profileupdatemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Programmers_ProgrammerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Qualifications_Programmers_ProgrammerId",
                table: "Qualifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Programmers_ProgrammerId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperiences_Programmers_ProgrammerId",
                table: "WorkExperiences");

            migrationBuilder.RenameColumn(
                name: "ProgrammerId",
                table: "WorkExperiences",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkExperiences_ProgrammerId",
                table: "WorkExperiences",
                newName: "IX_WorkExperiences_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProgrammerId",
                table: "Skills",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_ProgrammerId",
                table: "Skills",
                newName: "IX_Skills_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProgrammerId",
                table: "Qualifications",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Qualifications_ProgrammerId",
                table: "Qualifications",
                newName: "IX_Qualifications_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProgrammerId",
                table: "Projects",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProgrammerId",
                table: "Projects",
                newName: "IX_Projects_ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Profiles_ProfileId",
                table: "Projects",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Qualifications_Profiles_ProfileId",
                table: "Qualifications",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Profiles_ProfileId",
                table: "Skills",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperiences_Profiles_ProfileId",
                table: "WorkExperiences",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Profiles_ProfileId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Qualifications_Profiles_ProfileId",
                table: "Qualifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Profiles_ProfileId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperiences_Profiles_ProfileId",
                table: "WorkExperiences");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "WorkExperiences",
                newName: "ProgrammerId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkExperiences_ProfileId",
                table: "WorkExperiences",
                newName: "IX_WorkExperiences_ProgrammerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Skills",
                newName: "ProgrammerId");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_ProfileId",
                table: "Skills",
                newName: "IX_Skills_ProgrammerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Qualifications",
                newName: "ProgrammerId");

            migrationBuilder.RenameIndex(
                name: "IX_Qualifications_ProfileId",
                table: "Qualifications",
                newName: "IX_Qualifications_ProgrammerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Projects",
                newName: "ProgrammerId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProfileId",
                table: "Projects",
                newName: "IX_Projects_ProgrammerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Programmers_ProgrammerId",
                table: "Projects",
                column: "ProgrammerId",
                principalTable: "Programmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Qualifications_Programmers_ProgrammerId",
                table: "Qualifications",
                column: "ProgrammerId",
                principalTable: "Programmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Programmers_ProgrammerId",
                table: "Skills",
                column: "ProgrammerId",
                principalTable: "Programmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperiences_Programmers_ProgrammerId",
                table: "WorkExperiences",
                column: "ProgrammerId",
                principalTable: "Programmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
