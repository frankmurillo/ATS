'use strict';
angular.module('authAjax', [])
    .factory('registerFactory', ['$http', '$q', function ($http, $q) {
        return {
            registerUser: function (user) {
                var deferred = $q.defer();
                $http({
                    method: 'POST',
                    url: 'api/Accounts/Register',
                    data: user
                }).success(deferred.resolve).error(deferred.reject);
                return deferred.promise;
            }
        }
    }]);