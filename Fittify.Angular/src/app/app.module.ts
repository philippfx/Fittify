import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import {TopNavbarComponent} from './views/topnavbar/topnavbar.component';
import {LeftSidebarComponent} from './views/leftsidebar/leftsidebar.component';
import {FooterComponent} from './views/footer/footer/footer.component';
import {DashboardComponent} from './views/dashboard/dashboard.component';
import {WorkoutOverviewComponent} from './views/workout-overview/workout-overview.component';
import { HttpClientModule } from '@angular/common/http';
import { GppdRepository } from './apimodelrepositories/Gppd';
import { WorkoutAssociatedExercisesComponent } from './views/workout-associated-exercises/workout-associated-exercises.component';
import { WorkoutHistoryComponent } from './views/workout-history/workout-history.component';
import { WorkoutHistoryDetailsComponent } from './views/workout-history-details/workout-history-details.component';

@NgModule({
  declarations:
  [
    TopNavbarComponent,
    LeftSidebarComponent,
    FooterComponent,
    DashboardComponent,
    WorkoutOverviewComponent,
    AppComponent,
    WorkoutAssociatedExercisesComponent,
    WorkoutHistoryComponent,
    WorkoutHistoryDetailsComponent
  ],
  imports:
  [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'home', component: DashboardComponent},
      { path: 'workouts/overview', component: WorkoutOverviewComponent },
      { path: 'workout/:workoutid/associatedexercises', component: WorkoutAssociatedExercisesComponent },
      { path: 'workout/:workoutid/workouthistories', component: WorkoutHistoryComponent },
      { path: '', redirectTo: 'welcome', pathMatch: 'full' },
      { path: '**', redirectTo: 'welcome', pathMatch: 'full' }
    ])
  ],
  providers: [
    GppdRepository
  ],
  bootstrap:
  [
    AppComponent
  ]
})
export class AppModule { }
