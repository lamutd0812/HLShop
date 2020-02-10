/// <reference path="../../assets/admin/libs/angular/angular.js" />

// khoi tao 1 module
var myApp = angular.module('myModul', []);

//register controller
myApp.controller("schoolController", schoolController);

//register service
myApp.service('Validator', Validator);

// inject
schoolController.$inject = ['$scope', 'Validator'];

// inject $scope object to controller
//myController.$inject = ['$scope'];

//declare a controller
function schoolController($scope, Validator) {
    //$scope.message = "Hello i'm school!";

    $scope.num = 1298;

    Validator.checkNumber($scope.num);

    $scope.checkNumber = function () {
        $scope.message = Validator.checkNumber2($scope.num);
    } 
}

// declare a service
function Validator($window) {
    return {
        checkNumber: checkNumber,
        checkNumber2: checkNumber2
    }

    function checkNumber(input) {
        if (input % 2 == 0) {
            $window.alert(input + ' is even!');
        }
        else {
            $window.alert(input + ' is odd!');
        }
    }

    function checkNumber2(input) {
        if (input % 2 == 0) {
            return (input + ' is even!')
        }
        else {
            return (input + ' is odd!');
        }
    }
}