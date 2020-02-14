﻿/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function (app) {
    app.factory('apiService', apiService);

    apiService().$inject = ['$http', 'notificationService'];

    function apiService($http, notificationService) {
        return {
            get: get,
            post: post
        }

        function get(url, params, success, failure) {
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                failure(error);
            });
        }

        function post(url, data, success) {
            $http.post(url, data).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                }
                else if (failure != null) {
                    failure(error);
                }

            });
        }
    }
})(angular.module('hlshop.common'));

// mapping: namespace thuộc module hlshop.common