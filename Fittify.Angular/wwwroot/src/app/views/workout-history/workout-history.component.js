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
var http_1 = require("@angular/common/http");
var Gppd_1 = require("../../apimodelrepositories/Gppd");
var router_1 = require("@angular/router");
var WorkoutHistoryComponent = /** @class */ (function () {
    function WorkoutHistoryComponent(_http, _gppd, _route) {
        this._http = _http;
        this._gppd = _gppd;
        this._route = _route;
        this.workoutHistories = [];
        this.workouts = [];
    }
    WorkoutHistoryComponent.prototype.ngOnInit = function () {
        var _this = this;
        var param = this._route.snapshot.paramMap.get('workoutid');
        if (param) {
            var workoutId = +param;
            this._gppd.getCollection('http://localhost:52275/api/workouts').subscribe(function (data) {
                _this.workouts = data;
                if (_this.workouts && _this.workouts[0]) {
                    _this._gppd.getCollection('http://localhost:52275/api/workouthistories/workout/'
                        + _this.workouts[0].id).subscribe(function (dataH) {
                        _this.workoutHistories = dataH;
                    });
                }
            }, function (err) { return console.log(err); });
        }
    };
    WorkoutHistoryComponent = __decorate([
        core_1.Component({
            selector: 'ff-workout-history',
            templateUrl: './workout-history.component.html',
            styleUrls: ['./workout-history.component.css']
        }),
        __metadata("design:paramtypes", [http_1.HttpClient,
            Gppd_1.GppdRepository,
            router_1.ActivatedRoute])
    ], WorkoutHistoryComponent);
    return WorkoutHistoryComponent;
}());
exports.WorkoutHistoryComponent = WorkoutHistoryComponent;
//# sourceMappingURL=workout-history.component.js.map