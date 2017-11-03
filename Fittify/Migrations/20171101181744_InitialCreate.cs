using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fittify.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkoutSessionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercises_WorkoutSessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SessionEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkoutSessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSessionHistories_WorkoutSessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    ExecutedOnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: true),
                    PreviousExerciseId = table.Column<int>(type: "int", nullable: true),
                    StandardMachineAdjustable1 = table.Column<int>(type: "int", nullable: true),
                    StandardMachineAdjustable2 = table.Column<int>(type: "int", nullable: true),
                    TotalScoreOfExercise = table.Column<int>(type: "int", nullable: true),
                    WorkoutSessionHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseHistory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseHistory_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseHistory_ExerciseHistory_PreviousExerciseId",
                        column: x => x.PreviousExerciseId,
                        principalTable: "ExerciseHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseHistory_WorkoutSessionHistories_WorkoutSessionHistoryId",
                        column: x => x.WorkoutSessionHistoryId,
                        principalTable: "WorkoutSessionHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardioSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StarTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainingSessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardioSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardioSets_ExerciseHistory_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "ExerciseHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardioSets_WorkoutSessions_TrainingSessionId",
                        column: x => x.TrainingSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightLiftingSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExerciseHistoryId = table.Column<int>(type: "int", nullable: false),
                    MachineAdjustableSetting1 = table.Column<int>(type: "int", nullable: false),
                    MachineAdjustableSetting2 = table.Column<int>(type: "int", nullable: false),
                    MachineAdjustableType1 = table.Column<int>(type: "int", nullable: false),
                    MachineAdjustableType2 = table.Column<int>(type: "int", nullable: false),
                    RepetitionsFull = table.Column<int>(type: "int", nullable: true),
                    RepetitionsReduced = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    WeightBurn = table.Column<int>(type: "int", nullable: true),
                    WeightFull = table.Column<int>(type: "int", nullable: true),
                    WeightReduced = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightLiftingSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightLiftingSets_ExerciseHistory_ExerciseHistoryId",
                        column: x => x.ExerciseHistoryId,
                        principalTable: "ExerciseHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardioSets_ExerciseId",
                table: "CardioSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_CardioSets_TrainingSessionId",
                table: "CardioSets",
                column: "TrainingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistory_CategoryId",
                table: "ExerciseHistory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistory_ExerciseId",
                table: "ExerciseHistory",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistory_PreviousExerciseId",
                table: "ExerciseHistory",
                column: "PreviousExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHistory_WorkoutSessionHistoryId",
                table: "ExerciseHistory",
                column: "WorkoutSessionHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CategoryId",
                table: "Exercises",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_WorkoutSessionId",
                table: "Exercises",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightLiftingSets_ExerciseHistoryId",
                table: "WeightLiftingSets",
                column: "ExerciseHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessionHistories_WorkoutSessionId",
                table: "WorkoutSessionHistories",
                column: "WorkoutSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardioSets");

            migrationBuilder.DropTable(
                name: "WeightLiftingSets");

            migrationBuilder.DropTable(
                name: "ExerciseHistory");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutSessionHistories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");
        }
    }
}
