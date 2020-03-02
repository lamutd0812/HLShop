/// <reference path="../../../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('hlshop.orders', ['hlshop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('orders', {
            url: "/orders",
            parent: "base",
            templateUrl: "/app/components/orders/orderListView.html",
            controller: "orderListController"
        }).state('order_details', {
            url: "/order_details/:id",
            parent: "base",
            templateUrl: "/app/components/orders/orderDetailView.html",
            controller: "orderDetailController"
        });
    }
})();