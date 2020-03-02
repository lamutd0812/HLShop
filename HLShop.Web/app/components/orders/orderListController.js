/// <reference path="../../shared/services/apiservice.js" />

(function (app) {
    app.controller('orderListController', orderListController);

    orderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function orderListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.orders = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getOrders = getOrders;
        $scope.keyword = '';

        //search
        $scope.search = search;
        function search() {
            getOrders();
        }

        //getOrders
        function getOrders(page) {
            page = page || 0; // neu page null thi gan = 0
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 2
                }
            }
            apiService.get('/api/order/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy!');
                } else {
                    notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount + ' bản ghi!');
                }
                $scope.orders = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load orders failed!');
            });
        }

        $scope.isAll = false;
        $scope.selectAll = selectAll;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.orders, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.orders, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }
        
        $scope.getOrders();
    }
})(angular.module('hlshop.orders'));