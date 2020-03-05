/// <reference path="../../shared/services/apiservice.js" />

(function (app) {
    app.controller('orderDetailController', orderDetailController);

    orderDetailController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', '$stateParams'];

    function orderDetailController($scope, apiService, notificationService, $ngBootbox, $filter, $stateParams) {
        $scope.orderDetails = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getOrderDetails = getOrderDetails;

        //getOrderDetails
        function getOrderDetails(page) {
            page = page || 0; // neu page null thi gan = 0
            var config = {
                params: {
                    page: page,
                    pageSize: 10
                }
            }
            apiService.get('/api/orderdetail/getbyorderid/' + $stateParams.id , config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy!');
                } else {
                    notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount + ' bản ghi!');
                }
                $scope.orderDetails = result.data.Items;
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
                angular.forEach($scope.orderDetails, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.orderDetails, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.totalAmount = function() {
            var total = 0;
            for (var i = 0; i < $scope.orderDetails.length; i++) {
                total += $scope.orderDetails[i].ProductPrice * $scope.orderDetails[i].Quantity;
            }
            return total;
        }
        

        $scope.getOrderDetails();
    }
})(angular.module('hlshop.orders'));