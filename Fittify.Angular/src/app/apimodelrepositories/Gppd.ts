import { IGppd } from './IGppd';
import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import { WorkoutForGet } from '../apimodels/get/WorkoutApiModelForGet';

import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import {ExerciseForGet} from '../apimodels/get/ExerciseApiModelForGet';

@Injectable()
export class GppdRepository  {
  extractData: any;
  private result: WorkoutForGet[];

  constructor(private _http: HttpClient) { }

  getAllWorkouts(): Observable<WorkoutForGet[]> {
    return this._http.get<WorkoutForGet[]>('http://localhost:52275/api/workouts');
  }

  getAllExercises(): Observable<ExerciseForGet[]> {
    return this._http.get<ExerciseForGet[]>('http://localhost:52275/api/exercises');
  }

  getCollection(uri: string): Observable<any[]> {
    const response = this._http.get<any[]>(uri).catch(this.handleError);

    response.subscribe(data => {
      this.extractData = data;
      console.log('Fresh Api Call: ' + this.extractData);
    }, err => console.log(err)
    );

    return response; // this._http.get<any[]>(uri).catch(this.handleError);
  }

  getById(uri: string): Observable<any[]> {
    return this._http.get<any[]>(uri).catch(this.handleError);
  }

  private handleError(err: HttpErrorResponse) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage = '';
    if (err.error instanceof Error) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return Observable.throw(errorMessage);
  }

}

