(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    productCategoryEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];

    //note: $state: là đối tượng của ui-router dùng để route
    function productCategoryEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true
        }

        // event for submit button ("Lưu")
        $scope.updateProductCategory = updateProductCategory;
        function updateProductCategory() {
            apiService.put('/api/productcategory/update', $scope.productCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật!');
                    $state.go('product_categories');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công!');
                });
        }

        // auto insert SEO tittle
        $scope.getSeoTittle = getSeoTittle;
        function getSeoTittle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        function loadProductCategoryDetail() {
            apiService.get('/api/productcategory/getbyid/' + $stateParams.id, null,
                function(result) {
                    $scope.productCategory = result.data;
                },
                function(error) {
                    notificationService.displayError(error.data);
                });
        }

        function loadParentCategories() {
            apiService.get('/api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = result.data;
            }, function () {
                console.log('Cannot get list parent!');
            });
        }

        loadParentCategories();
        loadProductCategoryDetail();
    }
})(angular.module('hlshop.product_categories'));