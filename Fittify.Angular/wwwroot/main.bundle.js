webpackJsonp(["main"],{

/***/ "../../../../../src/$$_lazy_route_resource lazy recursive":
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncatched exception popping up in devtools
	return Promise.resolve().then(function() {
		throw new Error("Cannot find module '" + req + "'.");
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "../../../../../src/$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "../../../../../src/app/apimodelrepositories/Gppd.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return GppdRepository; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__ = __webpack_require__("../../../../rxjs/_esm5/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_observable_throw__ = __webpack_require__("../../../../rxjs/_esm5/add/observable/throw.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/catch.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_add_operator_do__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/do.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_rxjs_add_operator_map__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/map.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var GppdRepository = (function () {
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
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].throw(errorMessage);
    };
    GppdRepository = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["A" /* Injectable */])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["a" /* HttpClient */]])
    ], GppdRepository);
    return GppdRepository;
}());



/***/ }),

/***/ "../../../../../src/app/app.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/app.component.html":
/***/ (function(module, exports) {

module.exports = "<ff-navbar></ff-navbar>\r\n<div class=\"container-fluid text-center\">\r\n  <div class=\"row content\">\r\n    <!--<ff-leftsidebar></ff-leftsidebar>-->\r\n\r\n    \r\n    <div class=\"container-fluid text-center\">\r\n      <div class=\"row content\">\r\n\r\n        <ff-leftsidebar></ff-leftsidebar>\r\n\r\n        <div class=\"col-sm-10 text-left\">\r\n          <router-outlet></router-outlet>\r\n        </div>\r\n\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n<ff-footer></ff-footer>\r\n"

/***/ }),

/***/ "../../../../../src/app/app.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var AppComponent = (function () {
    function AppComponent() {
    }
    AppComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-root',
            template: __webpack_require__("../../../../../src/app/app.component.html"),
            styles: [__webpack_require__("../../../../../src/app/app.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], AppComponent);
    return AppComponent;
}());



/***/ }),

/***/ "../../../../../src/app/app.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__("../../../platform-browser/esm5/platform-browser.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__views_topnavbar_topnavbar_component__ = __webpack_require__("../../../../../src/app/views/topnavbar/topnavbar.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__views_leftsidebar_leftsidebar_component__ = __webpack_require__("../../../../../src/app/views/leftsidebar/leftsidebar.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__views_footer_footer_footer_component__ = __webpack_require__("../../../../../src/app/views/footer/footer/footer.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__views_dashboard_dashboard_component__ = __webpack_require__("../../../../../src/app/views/dashboard/dashboard.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__views_workout_overview_workout_overview_component__ = __webpack_require__("../../../../../src/app/views/workout-overview/workout-overview.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__apimodelrepositories_Gppd__ = __webpack_require__("../../../../../src/app/apimodelrepositories/Gppd.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__views_workout_associated_exercises_workout_associated_exercises_component__ = __webpack_require__("../../../../../src/app/views/workout-associated-exercises/workout-associated-exercises.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__views_workout_history_workout_history_component__ = __webpack_require__("../../../../../src/app/views/workout-history/workout-history.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__views_workout_history_details_workout_history_details_component__ = __webpack_require__("../../../../../src/app/views/workout-history-details/workout-history-details.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};















var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["I" /* NgModule */])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_5__views_topnavbar_topnavbar_component__["a" /* TopNavbarComponent */],
                __WEBPACK_IMPORTED_MODULE_6__views_leftsidebar_leftsidebar_component__["a" /* LeftSidebarComponent */],
                __WEBPACK_IMPORTED_MODULE_7__views_footer_footer_footer_component__["a" /* FooterComponent */],
                __WEBPACK_IMPORTED_MODULE_8__views_dashboard_dashboard_component__["a" /* DashboardComponent */],
                __WEBPACK_IMPORTED_MODULE_9__views_workout_overview_workout_overview_component__["a" /* WorkoutOverviewComponent */],
                __WEBPACK_IMPORTED_MODULE_3__app_component__["a" /* AppComponent */],
                __WEBPACK_IMPORTED_MODULE_12__views_workout_associated_exercises_workout_associated_exercises_component__["a" /* WorkoutAssociatedExercisesComponent */],
                __WEBPACK_IMPORTED_MODULE_13__views_workout_history_workout_history_component__["a" /* WorkoutHistoryComponent */],
                __WEBPACK_IMPORTED_MODULE_14__views_workout_history_details_workout_history_details_component__["a" /* WorkoutHistoryDetailsComponent */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["a" /* BrowserModule */],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["a" /* FormsModule */],
                __WEBPACK_IMPORTED_MODULE_10__angular_common_http__["b" /* HttpClientModule */],
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["b" /* RouterModule */].forRoot([
                    { path: 'home', component: __WEBPACK_IMPORTED_MODULE_8__views_dashboard_dashboard_component__["a" /* DashboardComponent */] },
                    { path: 'workouts/overview', component: __WEBPACK_IMPORTED_MODULE_9__views_workout_overview_workout_overview_component__["a" /* WorkoutOverviewComponent */] },
                    { path: 'workout/:workoutid/associatedexercises', component: __WEBPACK_IMPORTED_MODULE_12__views_workout_associated_exercises_workout_associated_exercises_component__["a" /* WorkoutAssociatedExercisesComponent */] },
                    { path: 'workout/:workoutid/workouthistories', component: __WEBPACK_IMPORTED_MODULE_13__views_workout_history_workout_history_component__["a" /* WorkoutHistoryComponent */] },
                    { path: '', redirectTo: 'welcome', pathMatch: 'full' },
                    { path: '**', redirectTo: 'welcome', pathMatch: 'full' }
                ])
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_11__apimodelrepositories_Gppd__["a" /* GppdRepository */]
            ],
            bootstrap: [
                __WEBPACK_IMPORTED_MODULE_3__app_component__["a" /* AppComponent */]
            ]
        })
    ], AppModule);
    return AppModule;
}());



/***/ }),

/***/ "../../../../../src/app/views/dashboard/dashboard.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/dashboard/dashboard.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"col-sm-4 well\">\r\n  <div class=\"horizontal-align\">\r\n    <a [routerLink]=\"['/workouts/overview']\">Workout</a>\r\n  </div>\r\n</div>\r\n<div class=\"col-sm-4 well\">\r\n  <div class=\"horizontal-align\">\r\n    <a href=\"#\">Nutrition</a>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/dashboard/dashboard.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var DashboardComponent = (function () {
    function DashboardComponent() {
    }
    DashboardComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-dashboard',
            template: __webpack_require__("../../../../../src/app/views/dashboard/dashboard.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/dashboard/dashboard.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], DashboardComponent);
    return DashboardComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/footer/footer/footer.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/footer/footer/footer.component.html":
/***/ (function(module, exports) {

module.exports = "<footer class=\"navbar navbar-inverse navbar-fixed-bottom\">\r\n  <div class=\"horizontal-align\">\r\n    <p>Fittify 2017</p>\r\n  </div>\r\n</footer>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/footer/footer/footer.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FooterComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var FooterComponent = (function () {
    function FooterComponent() {
    }
    FooterComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-footer',
            template: __webpack_require__("../../../../../src/app/views/footer/footer/footer.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/footer/footer/footer.component.css")]
        })
    ], FooterComponent);
    return FooterComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/leftsidebar/leftsidebar.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/leftsidebar/leftsidebar.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"col-sm-2 sidenav\">\r\n  <p>\r\n    <a href=\"#\">Link</a>\r\n  </p>\r\n  <p>\r\n    <a href=\"#\">Link</a>\r\n  </p>\r\n  <p>\r\n    <a href=\"#\">Link</a>\r\n  </p>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/leftsidebar/leftsidebar.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LeftSidebarComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var LeftSidebarComponent = (function () {
    function LeftSidebarComponent() {
    }
    LeftSidebarComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-leftsidebar',
            template: __webpack_require__("../../../../../src/app/views/leftsidebar/leftsidebar.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/leftsidebar/leftsidebar.component.css")]
        })
    ], LeftSidebarComponent);
    return LeftSidebarComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/topnavbar/topnavbar.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/topnavbar/topnavbar.component.html":
/***/ (function(module, exports) {

module.exports = "<nav class=\"navbar navbar-inverse\">\r\n  <div class=\"container-fluid\">\r\n    <div class=\"navbar-header\">\r\n      <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\"#myNavbar\">\r\n        <span class=\"icon-bar\"></span>\r\n        <span class=\"icon-bar\"></span>\r\n        <span class=\"icon-bar\"></span>\r\n      </button>\r\n      <a class=\"navbar-brand\" href=\"#\">Logo</a>\r\n    </div>\r\n    <div class=\"collapse navbar-collapse\" id=\"myNavbar\">\r\n      <ul class=\"nav navbar-nav\">\r\n        <li class=\"active\"><a [routerLink]=\"['home']\">Home</a></li>\r\n        <li><a [routerLink]=\"['workouts/overview']\">Workout</a></li>\r\n        <li><a href=\"#\">Nutrition</a></li>\r\n        <li><a href=\"#\">About</a></li>\r\n      </ul>\r\n      <ul class=\"nav navbar-nav navbar-right\">\r\n        <li><a href=\"#\"><span class=\"glyphicon glyphicon-log-in\"></span> Login</a></li>\r\n        <li><a href=\"#\"><span class=\"glyphicon glyphicon-user\"></span> Register</a></li>\r\n      </ul>\r\n    </div>\r\n  </div>\r\n</nav>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/topnavbar/topnavbar.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TopNavbarComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var TopNavbarComponent = (function () {
    function TopNavbarComponent() {
    }
    TopNavbarComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-navbar',
            template: __webpack_require__("../../../../../src/app/views/topnavbar/topnavbar.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/topnavbar/topnavbar.component.css")]
        })
    ], TopNavbarComponent);
    return TopNavbarComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/workout-associated-exercises/workout-associated-exercises.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/workout-associated-exercises/workout-associated-exercises.component.html":
/***/ (function(module, exports) {

module.exports = "<h2>Exercises for {{workouts[0].name}}</h2>\r\n<table class=\"table table-striped table-hover\">\r\n  <tr>\r\n    <th>\r\n      Name\r\n    </th>\r\n  </tr>\r\n <tr *ngFor=\"let exercise of associatedExercises\">\r\n    <td>{{exercise.name}}</td>\r\n  </tr>\r\n</table>\r\n<form class=\"form-group\" action=\"~/workout/@Model.Id/associatedexercises\" method=\"post\">\r\n  <input type=\"submit\" value=\"Add\" class=\"btn btn-primary\" />\r\n  <div class=\"col-sm-4\">\r\n    <select class=\"form-control\" name=\"Id\">\r\n      <option>Please select exercise to add</option>\r\n      <option *ngFor=\"let e of allExercises\" value=\"{{e.id}}\">{{e.name}}</option>\r\n    </select>\r\n  </div>\r\n</form>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/workout-associated-exercises/workout-associated-exercises.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkoutAssociatedExercisesComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__apimodelrepositories_Gppd__ = __webpack_require__("../../../../../src/app/apimodelrepositories/Gppd.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



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
    WorkoutAssociatedExercisesComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-workout-associated-exercises',
            template: __webpack_require__("../../../../../src/app/views/workout-associated-exercises/workout-associated-exercises.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/workout-associated-exercises/workout-associated-exercises.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__apimodelrepositories_Gppd__["a" /* GppdRepository */],
            __WEBPACK_IMPORTED_MODULE_2__angular_router__["a" /* ActivatedRoute */]])
    ], WorkoutAssociatedExercisesComponent);
    return WorkoutAssociatedExercisesComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/workout-history-details/workout-history-details.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/workout-history-details/workout-history-details.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\r\n  workout-history-details works!\r\n</p>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/workout-history-details/workout-history-details.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkoutHistoryDetailsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var WorkoutHistoryDetailsComponent = (function () {
    function WorkoutHistoryDetailsComponent() {
    }
    WorkoutHistoryDetailsComponent.prototype.ngOnInit = function () {
    };
    WorkoutHistoryDetailsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-workout-history-details',
            template: __webpack_require__("../../../../../src/app/views/workout-history-details/workout-history-details.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/workout-history-details/workout-history-details.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], WorkoutHistoryDetailsComponent);
    return WorkoutHistoryDetailsComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/workout-history/workout-history.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/workout-history/workout-history.component.html":
/***/ (function(module, exports) {

module.exports = "<h2>Your workout history for {{workouts[0]?.workoutName}}</h2>\r\n<table class=\"table table-striped table-hover\">\r\n  <tr>\r\n    <th>\r\n      DateTime\r\n    </th>\r\n  </tr>\r\n\r\n  <tr *ngFor=\"let workoutHistory of workoutHistories\">\r\n    <td>\r\n      <a href=\"/workout/historyDetails/{{workoutHistory?.id}}\">\r\n        <div *ngIf=\"workoutHistory?.dateTimeStart; else elseNotStartedYet\">\r\n          {{workoutHistory?.dateTimeStart}}\r\n        </div>\r\n        <ng-template #elseNotStartedYet>Not started yet</ng-template>\r\n\r\n\r\n        <div *ngIf=\"workoutHistory?.dateTimeEnd; else elseNotEndedYet\">\r\n          {{workoutHistory?.dateTimeEnd}}\r\n        </div>\r\n        <ng-template #elseNotEndedYet>Not ended yet</ng-template>\r\n\r\n      </a>\r\n    </td>\r\n  </tr>\r\n\r\n\r\n</table>\r\n<form action=\"~/workout/history/new\" method=\"post\">\r\n  <input name=\"workoutId\" type=\"hidden\" value=\"@Model.FirstOrDefault()?.WorkoutId\"/>\r\n  <input type=\"submit\" value=\"Start new workout\" class=\"btn btn-default\"/>\r\n</form>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/workout-history/workout-history.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkoutHistoryComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__apimodelrepositories_Gppd__ = __webpack_require__("../../../../../src/app/apimodelrepositories/Gppd.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var WorkoutHistoryComponent = (function () {
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
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-workout-history',
            template: __webpack_require__("../../../../../src/app/views/workout-history/workout-history.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/workout-history/workout-history.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["a" /* HttpClient */],
            __WEBPACK_IMPORTED_MODULE_2__apimodelrepositories_Gppd__["a" /* GppdRepository */],
            __WEBPACK_IMPORTED_MODULE_3__angular_router__["a" /* ActivatedRoute */]])
    ], WorkoutHistoryComponent);
    return WorkoutHistoryComponent;
}());



/***/ }),

/***/ "../../../../../src/app/views/workout-overview/workout-overview.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "\r\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/views/workout-overview/workout-overview.component.html":
/***/ (function(module, exports) {

module.exports = "<h2>Your workouts</h2>\r\n<table class=\"table table-striped table-hover\">\r\n  <tr>\r\n    <th>\r\n      Name\r\n    </th>\r\n    <th>\r\n      History\r\n    </th>\r\n    <th>\r\n      Start New Session\r\n    </th>\r\n  </tr>\r\n  <tr *ngFor=\"let workout of workouts\">\r\n    <td><a routerLink=\"/workout/{{workout?.id}}/associatedexercises\">{{workout?.name}}</a></td>\r\n    <td><a routerLink=\"/workout/{{workout?.id}}/workouthistories\">Show history</a></td>\r\n    <td>\r\n      <form action=\"/workout/history/new\" method=\"post\">\r\n        <input name=\"workoutId\" type=\"hidden\" value=\"workout.id\"/>\r\n        <input type=\"submit\" value=\"Start\" class=\"btn btn-default\"/>\r\n      </form>\r\n    </td>\r\n  </tr>\r\n\r\n</table>\r\n<form class=\"form-group\" action=\"/workout/new\" method=\"post\">\r\n  <input type=\"text\" name=\"name\"/>\r\n  <input type=\"submit\" value=\"Add Workout\" class=\"btn btn-primary\" />\r\n</form>\r\n"

/***/ }),

/***/ "../../../../../src/app/views/workout-overview/workout-overview.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkoutOverviewComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__apimodelrepositories_Gppd__ = __webpack_require__("../../../../../src/app/apimodelrepositories/Gppd.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var WorkoutOverviewComponent = (function () {
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
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["n" /* Component */])({
            selector: 'ff-workout-overview',
            template: __webpack_require__("../../../../../src/app/views/workout-overview/workout-overview.component.html"),
            styles: [__webpack_require__("../../../../../src/app/views/workout-overview/workout-overview.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__apimodelrepositories_Gppd__["a" /* GppdRepository */]])
    ], WorkoutOverviewComponent);
    return WorkoutOverviewComponent;
}());



/***/ }),

/***/ "../../../../../src/environments/environment.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.
var environment = {
    production: false
};


/***/ }),

/***/ "../../../../../src/main.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__("../../../platform-browser-dynamic/esm5/platform-browser-dynamic.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__("../../../../../src/app/app.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");




if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["_13" /* enableProdMode */])();
}
Object(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */])
    .catch(function (err) { return console.log(err); });


/***/ }),

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__("../../../../../src/main.ts");


/***/ })

},[0]);
//# sourceMappingURL=main.bundle.js.map