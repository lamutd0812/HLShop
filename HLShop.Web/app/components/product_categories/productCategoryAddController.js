(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    productCategoryAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state'];

    //note: $state: là đối tượng của ui-router dùng để route
    function productCategoryAddController($scope, apiService, notificationService, $state) {
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true,
            Name: "Danh mục 1"
        }

        // event for submit button ("Lưu")
        $scope.addProductCategory = addProductCategory;
        function addProductCategory() {
            apiService.post('/api/productcategory/create', $scope.productCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                    $state.go('product_categories');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công.');
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
    }
})(angular.module('hlshop.product_categories'));