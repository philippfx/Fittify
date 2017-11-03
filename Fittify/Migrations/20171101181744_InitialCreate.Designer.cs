﻿// <auto-generated />
using Fittify.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Fittify.Migrations
{
    [DbContext(typeof(FittifyContext))]
    [Migration("20171101181744_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Fittify.Entities.ExerciseHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryId");

                    b.Property<DateTime>("ExecutedOnDateTime");

                    b.Property<int?>("ExerciseId");

                    b.Property<int?>("PreviousExerciseId");

                    b.Property<int?>("StandardMachineAdjustable1");

                    b.Property<int?>("StandardMachineAdjustable2");

                    b.Property<int?>("TotalScoreOfExercise");

                    b.Property<int?>("WorkoutSessionHistoryId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("PreviousExerciseId");

                    b.HasIndex("WorkoutSessionHistoryId");

                    b.ToTable("ExerciseHistory");
                });

            modelBuilder.Entity("Fittify.Entities.WeightLiftingSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTimeEnd");

                    b.Property<DateTime>("DateTimeStart");

                    b.Property<int>("ExerciseHistoryId");

                    b.Property<int>("MachineAdjustableSetting1");

                    b.Property<int>("MachineAdjustableSetting2");

                    b.Property<int>("MachineAdjustableType1");

                    b.Property<int>("MachineAdjustableType2");

                    b.Property<int?>("RepetitionsFull");

                    b.Property<int?>("RepetitionsReduced");

                    b.Property<int>("Score");

                    b.Property<int?>("WeightBurn");

                    b.Property<int?>("WeightFull");

                    b.Property<int?>("WeightReduced");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseHistoryId");

                    b.ToTable("WeightLiftingSets");
                });

            modelBuilder.Entity("Fittify.Entities.Workout.CardioSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<TimeSpan>("Duration");

                    b.Property<DateTime>("EndTime");

                    b.Property<int>("ExerciseId");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StarTime");

                    b.Property<int>("TrainingSessionId");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("TrainingSessionId");

                    b.ToTable("CardioSets");
                });

            modelBuilder.Entity("Fittify.Entities.Workout.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Fittify.Entities.Workout.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryId");

                    b.Property<string>("Name");

                    b.Property<int?>("WorkoutSessionId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("WorkoutSessionId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Fittify.Entities.Workout.WorkoutSessionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("SessionEnd");

                    b.Property<DateTime>("SessionStart");

                    b.Property<int>("WorkoutSessionId");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutSessionId");

                    b.ToTable("WorkoutSessionHistories");
                });

            modelBuilder.Entity("Fittify.Entities.WorkoutSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("WorkoutSessions");
                });

            modelBuilder.Entity("Fittify.Entities.ExerciseHistory", b =>
                {
                    b.HasOne("Fittify.Entities.Workout.Category")
                        .WithMany("Exercises")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Fittify.Entities.Workout.Exercise", "Exercise")
                        .WithMany("ExerciseHistories")
                        .HasForeignKey("ExerciseId");

                    b.HasOne("Fittify.Entities.ExerciseHistory", "PreviousExercise")
                        .WithMany()
                        .HasForeignKey("PreviousExerciseId");

                    b.HasOne("Fittify.Entities.Workout.WorkoutSessionHistory")
                        .WithMany("ExerciseHistories")
                        .HasForeignKey("WorkoutSessionHistoryId");
                });

            modelBuilder.Entity("Fittify.Entities.WeightLiftingSet", b =>
                {
                    b.HasOne("Fittify.Entities.ExerciseHistory", "ExerciseHistory")
                        .WithMany("WeightLiftingSets")
                        .HasForeignKey("ExerciseHistoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fittify.Entities.Workout.CardioSet", b =>
                {
                    b.HasOne("Fittify.Entities.ExerciseHistory", "Exercise")
                        .WithMany("CardioSets")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fittify.Entities.WorkoutSession", "TrainingSession")
                        .WithMany()
                        .HasForeignKey("TrainingSessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fittify.Entities.Workout.Exercise", b =>
                {
                    b.HasOne("Fittify.Entities.Workout.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fittify.Entities.WorkoutSession")
                        .WithMany("Exercises")
                        .HasForeignKey("WorkoutSessionId");
                });

            modelBuilder.Entity("Fittify.Entities.Workout.WorkoutSessionHistory", b =>
                {
                    b.HasOne("Fittify.Entities.WorkoutSession", "WorkoutSession")
                        .WithMany()
                        .HasForeignKey("WorkoutSessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
