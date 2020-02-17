/// <reference path="../../shared/services/apiservice.js" />

(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productCategoryListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProductCategories = getProductCategories;
        $scope.keyword = '';

        //search
        $scope.search = search;
        function search() {
            getProductCategories();
        }

        // delete 1
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

        //region: delete multiple
        $scope.$watch("productCategories", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        $scope.isAll = false;
        $scope.selectAll = selectAll;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.productCategories, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.productCategories, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.deleteMultiple = deleteMultiple;
        function deleteMultiple() {
            var listId = [];
            $.each($scope.selected, function(i, item) {
                listId.push(item.ID);
            });
            var config = {
                params: {
                    checkedIds: JSON.stringify(listId)
                }
            }
            apiService.del('/api/productcategory/deletemulti', config, function(result) {
                    notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi!');
                    getProductCategories();
                }, function(error) {
                    notificationService.displayError('Xóa không thành công!');
                });
        }
        //end region: delete multiple

        //getProductCategories
        function getProductCategories(page) {
            page = page || 0; // neu page null thi gan = 0
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
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