(function (app) {
    app.controller('loginController', ['$scope', 'loginService', '$injector', 'notificationService',
        function ($scope, loginService, $injector, notificationService) {

            $scope.loginData = {
                userName: "",
                password: ""
            };

            $scope.loginSubmit = function () {
                loginService.login($scope.loginData.userName, $scope.loginData.password).then(function (response) {
                    if (response != null && response.data.error != undefined) {
                        notificationService.displayError("Đăng nhập không đúng!");
                    }
                    else {
                        // 1 cách để tránh lỗi liên kết vòng khi sd $state: $injector
                        var stateService = $injector.get('$state');
                        stateService.go('home');
                    }
                });
            }
        }]);
})(angular.module('hlshop'));