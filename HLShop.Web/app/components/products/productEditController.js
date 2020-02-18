(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', 'commonService', '$stateParams'];

    //note: $state: là đối tượng của ui-router dùng để route
    function productEditController($scope, apiService, notificationService, $state, commonService, $stateParams) {
        $scope.product = {};
        $scope.moreImages = [];

        //cau hinh ck editor
        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        }

        //load product detail infor
        function loadProductDetail() {
            apiService.get('/api/product/getbyid/' + $stateParams.id, null,
                function (result) {
                    $scope.product = result.data;
                    $scope.moreImages = JSON.parse($scope.product.MoreImages); // convert JSON to array
                },
                function (error) {
                    notificationService.displayError(error.data);
                });
        }

        // event for submit button ("Lưu")
        $scope.updateProduct = updateProduct;
        function updateProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages); 
            apiService.put('/api/product/update', $scope.product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật!');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công!');
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
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function() {
                    $scope.product.Image = fileUrl; 
                });
            }
            finder.popup();
        }

        //Choose more images
        $scope.chooseMoreImages = chooseMoreImages;
        function chooseMoreImages() {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                });
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
        loadProductDetail();
    }
})(angular.module('hlshop.products'));