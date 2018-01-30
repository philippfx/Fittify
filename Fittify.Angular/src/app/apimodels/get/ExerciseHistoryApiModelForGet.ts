import {UniqueIdentifier} from "../../common/UniqueIdentifier";

export class ExerciseHistoryForGet extends UniqueIdentifier<number>
{
  public exerciseId: number;
  public exerciseName: string;
  public workoutHistoryId: number;
  public previousExerciseHistoryId: number;
  public weightLiftingSetIds: number[];
  public cardioSetIds: number[];
}
