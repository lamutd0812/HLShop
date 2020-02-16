/// <reference path="../../shared/services/apiservice.js" />

(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function productCategoryListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProductCategories = getProductCategories;
        $scope.keyword = '';

        $scope.search = search;
        function search() {
            getProductCategories();
        }

        $scope.deleteProductCategory = deleteProductCategory;
        function deleteProductCategory(id) {
            $ngBootbox.confirm('Bạn có chắc chắn muốn xóa danh mục sản phẩm này không?')
                .then(function() {
                    var config = {
                        params: {
                            id: id
                        }
                    }
                    apiService.del('/api/productcategory/delete', config, function() {
                        notificationService.displaySuccess('Xóa thành công!');
                        getProductCategories();
                    }, function() {
                        notificationService.displayError('Xóa không thành công!');
                    });
                }); 
        }

        function getProductCategories(page) {
            page = page || 0; // neu page null thi gan = 0
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 3
                }
            }
            apiService.get('/api/productcategory/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy!');
                } else {
                    notificationService.displaySuccess('Đã tìm thấy ' + result.data.TotalCount +' bản ghi!');
                }
                $scope.productCategories = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load product category failed!');
            });
        }

        $scope.getProductCategories();
    }
})(angular.module('hlshop.product_categories'));