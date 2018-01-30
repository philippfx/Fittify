import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkoutAssociatedExercisesComponent } from './workout-associated-exercises.component';

describe('WorkoutAssociatedExercisesComponent', () => {
  let component: WorkoutAssociatedExercisesComponent;
  let fixture: ComponentFixture<WorkoutAssociatedExercisesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkoutAssociatedExercisesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkoutAssociatedExercisesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
