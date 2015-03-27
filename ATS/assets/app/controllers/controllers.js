//(function () {
//    'use strict';
//}());
angular.module('controllers', [])
     .run(['localStorageService', function (localStorageService) {
         //localStorageService.clearAll();
         //authService.fillAuthData();
         
     }])
    .controller('HomeCtrl', ['$scope', '$location', function ($scope, $location) {
    }])
    .controller('LoginCtrl', ['$scope', '$location', function ($scope, $location) {
        $scope.loginData = {
            email: "",
            password: ""
        };

        $scope.message = "";

        $scope.login = function () {
            
        }
        
        $scope.goToRegistrationPage = function () {
            $location.path('/register');
        }
    }])
    .controller('RegisterCtrl', ['$scope', '$location', '$timeout', '$http', 'registrationConst', function ($scope, $location, $timeout, $http, registrationConst) {
        $scope.savedSuccessfully = false;
        $scope.message = "";
        $scope.registration = {
            email: "",
            Username: "",
            FirstName: "",
            LastName: "",
            Password: "",
            ConfirmPassword: ""
        };

        $scope.register = function () {
            $http({
                method: 'POST',
                url: 'api/Accounts/Create',
                data: $scope.registration
            }).success(function (response) {
                console.log(response);
            })
        }

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/');
            }, registrationConst.REDIRECT_LOGIN_TIME);
        }
    }])

   
;
