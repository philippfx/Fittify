import { Component, OnInit } from '@angular/core';
import { ExerciseForGet } from '../../apimodels/get/ExerciseApiModelForGet';
import { GppdRepository } from '../../apimodelrepositories/Gppd';
import { ActivatedRoute } from '@angular/router';
import {WorkoutForGet} from '../../apimodels/get/WorkoutApiModelForGet';

@Component({
  selector: 'ff-workout-associated-exercises',
  templateUrl: './workout-associated-exercises.component.html',
  styleUrls: ['./workout-associated-exercises.component.css']
})
export class WorkoutAssociatedExercisesComponent {
  private associatedExercises: ExerciseForGet[] = [];
  private allExercises: ExerciseForGet[] = [];
  private currentWorkout: WorkoutForGet[] = [];

  constructor(private gppd: GppdRepository,
              private _route: ActivatedRoute) {
    const param = this._route.snapshot.paramMap.get('workoutid');
    if (param) {
      const workoutId = +param;
      this.gppd.getCollection('http://localhost:52275/api/workouts').subscribe(data => {
        this.currentWorkout = data;
        console.log('currentWorkouts: ' + this.currentWorkout);

        if (this.currentWorkout) {
          this.gppd.getCollection('http://localhost:52275/api/exercises/workout/'
            + this.currentWorkout[0].id).subscribe(data => {
            this.associatedExercises = data;
          });

          this.gppd.getCollection('http://localhost:52275/api/exercises').subscribe(data => {
            this.allExercises = data;
          });
        }
      });
    }
  }
}

