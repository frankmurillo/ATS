angular.module('authorization', [])
    .factory('authService', ['$http', '$q', 'localStorageService', 'host', function ($http, $q, localStorageService, host) {

        var serviceBase = host.LOCAL_HOST;
        var authServiceFactory = {};

        var _authentication = {
            isAuth: false,
            userName: ""
        };

        var _saveRegistration = function (registration) {

            _logOut();

            return $http.post(serviceBase + '/api/accounts/register', registration).then(function (response) {
                return response;
            });

        };

        var _login = function (loginData) {
            var grant_type = "grant_type=password&username=" + loginData.email + "&password=" + loginData.password;
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: serviceBase + '/oauth/token', 
                data: grant_type,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).success(function (response) {
                console.log(response);
                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });

                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;

                deferred.resolve(response);

            }).error(function (err, status) {
                _logOut();
                deferred.reject(err);
            });

            return deferred.promise;

        };

        var _logOut = function () {

            localStorageService.remove('authorizationData');

            _authentication.isAuth = false;
            _authentication.userName = "";

        };

        var _fillAuthData = function () {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                _authentication.isAuth = true;
                _authentication.userName = authData.userName;
            }

        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.fillAuthData = _fillAuthData;
        authServiceFactory.authentication = _authentication;

        return authServiceFactory;
    }]);