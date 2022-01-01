using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace quiz_app_dotnet_api.Data.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course_quiz",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_quiz", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "question_quiz",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answer_a = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answer_b = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answer_c = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answer_d = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    correct_answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    point = table.Column<float>(type: "real", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_quiz", x => x.id);
                    table.ForeignKey(
                        name: "FK_question_quiz_course_quiz_course_id",
                        column: x => x.course_id,
                        principalTable: "course_quiz",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disable = table.Column<bool>(type: "bit", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_room_course_quiz_CourseId",
                        column: x => x.CourseId,
                        principalTable: "course_quiz",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "storage_scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scores = table.Column<int>(type: "int", nullable: false),
                    TimeSubmit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseQuizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage_scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_storage_scores_course_quiz_CourseQuizId",
                        column: x => x.CourseQuizId,
                        principalTable: "course_quiz",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_storage_scores_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_question_quiz_course_id",
                table: "question_quiz",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_CourseId",
                table: "room",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_storage_scores_CourseQuizId",
                table: "storage_scores",
                column: "CourseQuizId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_storage_scores_UserId",
                table: "storage_scores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_UserName",
                table: "user",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_quiz");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "storage_scores");

            migrationBuilder.DropTable(
                name: "course_quiz");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
