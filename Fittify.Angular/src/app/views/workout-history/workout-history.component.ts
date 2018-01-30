import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {GppdRepository} from '../../apimodelrepositories/Gppd';
import {ActivatedRoute} from '@angular/router';
import {ExerciseForGet} from '../../apimodels/get/ExerciseApiModelForGet';
import {WorkoutHistoryForGet} from '../../apimodels/get/WorkoutHistoryForGet';
import {WorkoutForGet} from '../../apimodels/get/WorkoutApiModelForGet';

@Component({
  selector: 'ff-workout-history',
  templateUrl: './workout-history.component.html',
  styleUrls: ['./workout-history.component.css']
})
export class WorkoutHistoryComponent implements OnInit {
  private workoutHistories: WorkoutHistoryForGet[] = [];
  private workouts: WorkoutForGet[] = [];

  constructor(private _http: HttpClient,
              private _gppd: GppdRepository,
              private _route: ActivatedRoute) { }

  ngOnInit() {
    const param = this._route.snapshot.paramMap.get('workoutid');
    if (param) {
      const workoutId = +param;
      this._gppd.getCollection('http://localhost:52275/api/workouts').subscribe(data => {
        this.workouts = data as WorkoutForGet[];
        if (this.workouts && this.workouts[0]) {
          this._gppd.getCollection('http://localhost:52275/api/workouthistories/workout/'
            + this.workouts[0].id).subscribe(dataH => {
            this.workoutHistories = dataH;
          });
        }
        },
        err => console.log(err)
      );
    }
  }
}
