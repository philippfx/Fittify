import {IUniqueIdentifier} from './IUniqueIdentifier';

export class UniqueIdentifier<TId> implements IUniqueIdentifier<TId>
{
  public id: TId;
}
