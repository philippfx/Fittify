"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_1 = require("@angular/platform-browser");
var router_1 = require("@angular/router");
var core_1 = require("@angular/core");
var app_component_1 = require("./app.component");
var forms_1 = require("@angular/forms");
var topnavbar_component_1 = require("./views/topnavbar/topnavbar.component");
var leftsidebar_component_1 = require("./views/leftsidebar/leftsidebar.component");
var footer_component_1 = require("./views/footer/footer/footer.component");
var dashboard_component_1 = require("./views/dashboard/dashboard.component");
var workout_overview_component_1 = require("./views/workout-overview/workout-overview.component");
var http_1 = require("@angular/common/http");
var Gppd_1 = require("./apimodelrepositories/Gppd");
var workout_associated_exercises_component_1 = require("./views/workout-associated-exercises/workout-associated-exercises.component");
var workout_history_component_1 = require("./views/workout-history/workout-history.component");
var workout_history_details_component_1 = require("./views/workout-history-details/workout-history-details.component");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        declarations: [
            topnavbar_component_1.TopNavbarComponent,
            leftsidebar_component_1.LeftSidebarComponent,
            footer_component_1.FooterComponent,
            dashboard_component_1.DashboardComponent,
            workout_overview_component_1.WorkoutOverviewComponent,
            app_component_1.AppComponent,
            workout_associated_exercises_component_1.WorkoutAssociatedExercisesComponent,
            workout_history_component_1.WorkoutHistoryComponent,
            workout_history_details_component_1.WorkoutHistoryDetailsComponent
        ],
        imports: [
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            http_1.HttpClientModule,
            router_1.RouterModule.forRoot([
                { path: 'home', component: dashboard_component_1.DashboardComponent },
                { path: 'workouts/overview', component: workout_overview_component_1.WorkoutOverviewComponent },
                { path: 'workout/:workoutid/associatedexercises', component: workout_associated_exercises_component_1.WorkoutAssociatedExercisesComponent },
                { path: 'workout/:workoutid/workouthistories', component: workout_history_component_1.WorkoutHistoryComponent },
                { path: '', redirectTo: 'welcome', pathMatch: 'full' },
                { path: '**', redirectTo: 'welcome', pathMatch: 'full' }
            ])
        ],
        providers: [
            Gppd_1.GppdRepository
        ],
        bootstrap: [
            app_component_1.AppComponent
        ]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map