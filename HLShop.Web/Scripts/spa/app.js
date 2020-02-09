/// <reference path="../../assets/admin/libs/angular/angular.js" />

// khoi tao 1 module
var myApp = angular.module('myModul', []);

//register controller
myApp.controller("studentController", studentController);
myApp.controller("teacherController", teacherController);
myApp.controller("schoolController", schoolController);

// inject $scope object to controller
//myController.$inject = ['$scope'];

//controller cho scope lồng nhau
function schoolController($scope) {
    $scope.message = "Hello i'm school!"
}

//declare a controller
function studentController ($scope) {
    $scope.message = "Hello i'm student!";
}

function teacherController($scope) {
    $scope.message = "Hello i'm teacher!";
}