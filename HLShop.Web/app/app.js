/// <reference path="../assets/admin/libs/angular/angular.js" />

// cấu hình routing cho app chính hlshop
(function () {
    angular.module('hlshop', ['hlshop.products', 'hlshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            templateUrl: "/app/components/home/homeView.html",
            controller: "homeController"
        });
        $urlRouterProvider.otherwise('/admin');
    }
})();
