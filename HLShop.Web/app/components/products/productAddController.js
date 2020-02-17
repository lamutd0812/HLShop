(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService'];

    //note: $state: là đối tượng của ui-router dùng để route
    function productAddController($scope, apiService, notificationService, $state, commonService) {
        $scope.product = {
            CreatedDate: new Date(),
            Status: true
        }

        //cau hinh ck editor
        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        }

        // event for submit button ("Lưu")
        $scope.addProduct = addProduct;
        function addProduct() {
            apiService.post('/api/product/create', $scope.product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được thêm mới!');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Thêm mới không thành công!');
                });
        }

        // auto insert SEO tittle
        $scope.getSeoTittle = getSeoTittle;
        function getSeoTittle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        //Choose image
        $scope.chooseImage = chooseImage;
        function chooseImage() {
            var finder = new CKFinder();
            finder.selectActionFunction = function(fileUrl) {
                $scope.product.Image = fileUrl;
            }
            finder.popup();
        }

        function loadProductCategories() {
            apiService.get('/api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                console.log('Cannot get list category!');
            });
        }

        loadProductCategories();
    }
})(angular.module('hlshop.products'));