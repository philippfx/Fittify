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
var Rx_1 = require("rxjs/Rx");
require("rxjs/add/observable/throw");
require("rxjs/add/operator/catch");
require("rxjs/add/operator/do");
require("rxjs/add/operator/map");
var GppdRepository = /** @class */ (function () {
    function GppdRepository(_http) {
        this._http = _http;
    }
    GppdRepository.prototype.getAllWorkouts = function () {
        return this._http.get('http://localhost:52275/api/workouts');
    };
    GppdRepository.prototype.getAllExercises = function () {
        return this._http.get('http://localhost:52275/api/exercises');
    };
    GppdRepository.prototype.getCollection = function (uri) {
        var _this = this;
        var response = this._http.get(uri).catch(this.handleError);
        response.subscribe(function (data) {
            _this.extractData = data;
            console.log('Fresh Api Call: ' + _this.extractData);
        }, function (err) { return console.log(err); });
        return response; // this._http.get<any[]>(uri).catch(this.handleError);
    };
    GppdRepository.prototype.getById = function (uri) {
        return this._http.get(uri).catch(this.handleError);
    };
    GppdRepository.prototype.handleError = function (err) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console
        var errorMessage = '';
        if (err.error instanceof Error) {
            // A client-side or network error occurred. Handle it accordingly.
            errorMessage = "An error occurred: " + err.error.message;
        }
        else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            errorMessage = "Server returned code: " + err.status + ", error message is: " + err.message;
        }
        console.error(errorMessage);
        return Rx_1.Observable.throw(errorMessage);
    };
    GppdRepository = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient])
    ], GppdRepository);
    return GppdRepository;
}());
exports.GppdRepository = GppdRepository;
//# sourceMappingURL=Gppd.js.map