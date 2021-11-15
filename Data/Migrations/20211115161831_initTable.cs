using Microsoft.EntityFrameworkCore.Migrations;

namespace quiz_app_dotnet_api.Data.Migrations
{
    public partial class initTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course_quiz",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_quiz", x => x.id);
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

            migrationBuilder.CreateIndex(
                name: "IX_question_quiz_course_id",
                table: "question_quiz",
                column: "course_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "question_quiz");

            migrationBuilder.DropTable(
                name: "course_quiz");
        }
    }
}
