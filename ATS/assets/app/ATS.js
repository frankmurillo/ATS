angular.module('ATS',
    [
        'controllers',
        'directives',
        'ngRoute',
        'LocalStorageModule',
        'authAjax',
        'constants',
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $locationProvider.html5Mode({ enabled: true });
        $routeProvider
            .when('/', {
                templateUrl: "assets/app/templates/home.html",
                controller: "HomeCtrl"
            })
            .when('/login', {
                templateUrl: "/assets/app/templates/login.html",
                controller: "LoginCtrl"
            })
            .when('/register', {
                templateUrl: "/assets/app/templates/register.html",
                controller: "RegisterCtrl"
            })
            .when('/consultant', {
                templateUrl: "/assets/app/templates/consultant.html",
                controller: "ConsultantCtrl"
            })
    }])

    /*
     * By doing this there is no need to setup extra code for setting up tokens or 
     * checking the status code, any AngularJS service executes XHR requests will use this interceptor.
     *  Note: this will work if you are using AngularJS service $http or $resource.
     */

    .config(function (localStorageServiceProvider) {
        localStorageServiceProvider.setPrefix('ATS');
    });