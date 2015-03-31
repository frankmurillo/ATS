//(function () {
//    'use strict';
//}());
angular.module('homeController', [])
     .run(['localStorageService', function (localStorageService) {
         //localStorageService.clearAll();
         //authService.fillAuthData();
         
     }])
    .controller('HomeCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
        $scope.logOut = function () {
            authService.logOut();
            $location.path('/');
        }

        $scope.authentication = authService.authentication;
    }])
    .controller('LoginCtrl', ['$scope', '$location', 'authService', function ($scope, $location, authService) {
        $scope.loginData = {
            email: "",
            password: ""
        };

        $scope.message = "";

        $scope.login = function () {
            authService.login($scope.loginData).then(
            function (response) {
                console.log(response);
                $location.path('/admin');
            },
            function (err) {
            $scope.message = err.error_description;
            });
        };
       
    }])
    .controller('RegisterCtrl', ['$scope', '$location', '$timeout', '$http', 'registrationConst', 'authService', function ($scope, $location, $timeout, $http, registrationConst, authService) {
        $scope.savedSuccessfully = false;
        $scope.message = "";
        $scope.registration = {
            email: "",
            username: "",
            firstName: "",
            lastName: "",
            roleName: "",
            password: "",
            confirmPassword: ""
        };

        $scope.register = function () {
            authService.saveRegistration($scope.registration).then(function (response) {

                $scope.savedSuccessfully = true;
                $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                startTimer();

            },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                $scope.message = "Failed to register user due to:" + errors.join(' ');
            });
        };

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/');
            }, registrationConst.REDIRECT_LOGIN_TIME);
        }
    }])
   
;
