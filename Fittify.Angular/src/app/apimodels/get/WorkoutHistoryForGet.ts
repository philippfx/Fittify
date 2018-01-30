import {UniqueIdentifier} from "../../common/UniqueIdentifier";

export class WorkoutHistoryForGet extends UniqueIdentifier<number>
{
  public dateTimeStart: Date;
  public dateTimeEnd: Date;
  public workoutId: number;
  public workoutName: string;
  public exerciseHistoryIds: number[];
}
