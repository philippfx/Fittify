"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Gppd_1 = require("../../apimodelrepositories/Gppd");
var router_1 = require("@angular/router");
var WorkoutAssociatedExercisesComponent = (function () {
    function WorkoutAssociatedExercisesComponent(gppd, _route) {
        var _this = this;
        this.gppd = gppd;
        this._route = _route;
        this.associatedExercises = [];
        this.allExercises = [];
        this.currentWorkout = [];
        var param = this._route.snapshot.paramMap.get('workoutid');
        if (param) {
            var workoutId = +param;
            this.gppd.getCollection('http://localhost:52275/api/workouts').subscribe(function (data) {
                _this.currentWorkout = data;
                console.log('currentWorkouts: ' + _this.currentWorkout);
                if (_this.currentWorkout) {
                    _this.gppd.getCollection('http://localhost:52275/api/exercises/workout/'
                        + _this.currentWorkout[0].id).subscribe(function (data) {
                        _this.associatedExercises = data;
                    });
                    _this.gppd.getCollection('http://localhost:52275/api/exercises').subscribe(function (data) {
                        _this.allExercises = data;
                    });
                }
            });
        }
    }
    return WorkoutAssociatedExercisesComponent;
}());
WorkoutAssociatedExercisesComponent = __decorate([
    core_1.Component({
        selector: 'ff-workout-associated-exercises',
        templateUrl: './workout-associated-exercises.component.html',
        styleUrls: ['./workout-associated-exercises.component.css']
    }),
    __metadata("design:paramtypes", [Gppd_1.GppdRepository,
        router_1.ActivatedRoute])
], WorkoutAssociatedExercisesComponent);
exports.WorkoutAssociatedExercisesComponent = WorkoutAssociatedExercisesComponent;
//# sourceMappingURL=workout-associated-exercises.component.js.map