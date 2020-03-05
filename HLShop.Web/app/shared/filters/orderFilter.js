(function (app) {
    app.filter('orderFilter', function () {
        return function (input) {
            if (input == true)
                return 'Đã xử lý';
            else
                return 'Chưa xử lý';
        }
    });
})(angular.module('hlshop.common'));