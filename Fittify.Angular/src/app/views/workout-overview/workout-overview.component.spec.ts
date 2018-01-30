/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { WorkoutOverviewComponent } from './workout-overview.component';

let component: WorkoutOverviewComponent;
let fixture: ComponentFixture<WorkoutOverviewComponent>;

describe('workout-overview component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ WorkoutOverviewComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(WorkoutOverviewComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
