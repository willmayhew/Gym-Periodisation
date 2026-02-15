using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymPeriodisation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkoutRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSets_Exercises_ExerciseId",
                table: "WorkoutSets");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSets_Workouts_WorkoutId",
                table: "WorkoutSets");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSets_ExerciseId",
                table: "WorkoutSets");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "WorkoutSets");

            migrationBuilder.RenameColumn(
                name: "WorkoutId",
                table: "WorkoutSets",
                newName: "WorkoutExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutSets_WorkoutId",
                table: "WorkoutSets",
                newName: "IX_WorkoutSets_WorkoutExerciseId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "WorkoutSets",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "WorkoutExercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercise_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutExercise_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercise_ExerciseId",
                table: "WorkoutExercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercise_WorkoutId",
                table: "WorkoutExercise",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSets_WorkoutExercise_WorkoutExerciseId",
                table: "WorkoutSets",
                column: "WorkoutExerciseId",
                principalTable: "WorkoutExercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSets_WorkoutExercise_WorkoutExerciseId",
                table: "WorkoutSets");

            migrationBuilder.DropTable(
                name: "WorkoutExercise");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_Name",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "WorkoutExerciseId",
                table: "WorkoutSets",
                newName: "WorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutSets_WorkoutExerciseId",
                table: "WorkoutSets",
                newName: "IX_WorkoutSets_WorkoutId");

            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "WorkoutSets",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "WorkoutSets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_ExerciseId",
                table: "WorkoutSets",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSets_Exercises_ExerciseId",
                table: "WorkoutSets",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSets_Workouts_WorkoutId",
                table: "WorkoutSets",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
