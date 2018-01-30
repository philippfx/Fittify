/// <reference path="../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from '@angular/platform-browser';
import { LeftSidebarComponent as LeftsidebarComponent } from './leftsidebar.component';

let component: LeftsidebarComponent;
let fixture: ComponentFixture<LeftsidebarComponent>;

describe('leftsidebar component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ LeftsidebarComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(LeftsidebarComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});
