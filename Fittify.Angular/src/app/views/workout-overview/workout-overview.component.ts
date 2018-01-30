import { Component } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { GppdRepository } from '../../apimodelrepositories/Gppd';
import { WorkoutForGet } from '../../apimodels/get/WorkoutApiModelForGet';
import {ExerciseForGet} from '../../apimodels/get/ExerciseApiModelForGet';

@Component({
   selector: 'ff-workout-overview',
   templateUrl: './workout-overview.component.html',
   styleUrls: ['./workout-overview.component.css']
})

export class WorkoutOverviewComponent {
  private workouts: WorkoutForGet[] = [];
  private exercises: ExerciseForGet[] = [];

  constructor(private gppd: GppdRepository) {
    // gppd.getAllWorkouts().subscribe(data => {
    //   this.workouts = data;
    // });

    gppd.getCollection('http://localhost:52275/api/workouts').subscribe(data => {
      this.workouts = data;
    });

    const be = 'abc';
  }

}
