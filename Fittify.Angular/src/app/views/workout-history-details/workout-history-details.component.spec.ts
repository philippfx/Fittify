import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkoutHistoryDetailsComponent } from './workout-history-details.component';

describe('WorkoutHistoryDetailsComponent', () => {
  let component: WorkoutHistoryDetailsComponent;
  let fixture: ComponentFixture<WorkoutHistoryDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkoutHistoryDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkoutHistoryDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
