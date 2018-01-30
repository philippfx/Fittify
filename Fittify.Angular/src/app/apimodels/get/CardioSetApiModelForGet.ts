import {UniqueIdentifier} from "../../common/UniqueIdentifier";

export class CardioSetForGet extends UniqueIdentifier<number>
{
  public dateTimeStart: Date;
  public dateTimeEnd: Date;
  public exerciseHistoryId: number;
}
