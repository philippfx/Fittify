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
var WorkoutOverviewComponent = /** @class */ (function () {
    function WorkoutOverviewComponent(gppd) {
        // gppd.getAllWorkouts().subscribe(data => {
        //   this.workouts = data;
        // });
        var _this = this;
        this.gppd = gppd;
        this.workouts = [];
        this.exercises = [];
        gppd.getCollection('http://localhost:52275/api/workouts').subscribe(function (data) {
            _this.workouts = data;
        });
        var be = 'abc';
    }
    WorkoutOverviewComponent = __decorate([
        core_1.Component({
            selector: 'ff-workout-overview',
            templateUrl: './workout-overview.component.html',
            styleUrls: ['./workout-overview.component.css']
        }),
        __metadata("design:paramtypes", [Gppd_1.GppdRepository])
    ], WorkoutOverviewComponent);
    return WorkoutOverviewComponent;
}());
exports.WorkoutOverviewComponent = WorkoutOverviewComponent;
//# sourceMappingURL=workout-overview.component.js.map