import {UniqueIdentifier} from '../../common/UniqueIdentifier';

export class WorkoutForGet extends UniqueIdentifier<number>
{
  public name: string;
  public categoryId: number;
  public rangeOfExerciseIds: string;
  public rangeOfWorkoutHistoryIds: string;
}
