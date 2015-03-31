angular.module('ATS',
    [
        'homeController',
        'adminController',
        'directives',
        'ngRoute',
        'LocalStorageModule',
        'authAjax',
        'constants',
        'interceptors',
        'authorization'
    ])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        $locationProvider.html5Mode({ enabled: true });
        $routeProvider
            .when('/', {
                templateUrl: "/assets/app/templates/home.html",
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
            .when('/admin', {
                templateUrl: "/assets/app/templates/admin.html",
                controller: "AdminCtrl"
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
    .run(['authService', function (authService) {
        authService.fillAuthData();
    }])
    .config(function (localStorageServiceProvider, $httpProvider) {
        localStorageServiceProvider.setPrefix('ATS');
        $httpProvider.interceptors.push('authInterceptorService');
    });

