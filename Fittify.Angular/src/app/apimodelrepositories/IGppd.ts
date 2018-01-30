import { Observable } from 'rxjs/Observable';

export interface IGppd<T> {
  get(): T[];
  //getByRangeOfIds(rangeOfIds: string): Observable<T[]>;
  //post(entity: T): Observable<T>;
  //put(entity: T): void;
  //delete(): void;
}
