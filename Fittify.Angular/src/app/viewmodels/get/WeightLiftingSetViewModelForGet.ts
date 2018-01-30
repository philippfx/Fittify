import {UniqueIdentifier} from "../../common/UniqueIdentifier";

export class WeightLiftingSetForGet extends UniqueIdentifier<number>
{
  public weightFull: number;
  public repetitionsFull: number;
  public weightReduced: number;
  public repetitionsReduced: number;
  public weightBurn: number;
  public totalScore: number;
  public exerciseHistoryId: number;
}
