import { IGppd } from './IGppd';
import { WorkoutForGet } from '../apimodels/get/WorkoutApiModelForGet';
import { GppdRepository } from './Gppd';
import { Http } from '@angular/http';

export class WorkoutGppd {

    constructor(private _gppdFactory: GppdRepository) {

    }

    //workouts: WorkoutForGet[] = this._gppdFactory.getGeneric(WorkoutForGet);
}
