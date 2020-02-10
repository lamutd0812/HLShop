/// <reference path="../../assets/admin/libs/angular/angular.js" />

// create a module
var myApp = angular.module('myModul', []);

//register controller
myApp.controller("schoolController", schoolController);

//register a custom directive
myApp.directive("hlShopDirective", hlShopDirective);

//register service
myApp.service('validatorService', validatorService);

// inject
schoolController.$inject = ['$scope', 'validatorService'];

//declare a controller
function schoolController($scope, validatorService) {
    $scope.num = 1298;

    $scope.checkNumber = function () {
        $scope.message = validatorService.checkNumber($scope.num);
    } 
}

// declare a service function
function validatorService($window) {
    return {
        checkNumber: checkNumber
    }

    function checkNumber(input) {
        if (input % 2 == 0) {
            return (input + ' is even!')
        }
        else {
            return (input + ' is odd!');
        }
    }
}

// declare a directive function
function hlShopDirective() {
    return {
        restrict:"A",
        templateUrl: "/Scripts/spa/hlShopDirective.html"
    }
}

