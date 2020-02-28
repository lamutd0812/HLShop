var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.label);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
              .append("<a>" + item.label + "</a>")
              .appendTo(ul);
            };

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault(); // xóa điều hướng mặc định của thẻ (<a>)
            var productId = parseInt($(this).data('id')); // lấy thuộc tính data-id của nút hiện tại (data-id="@Model.ID")
            $.ajax({
                url: '/Cart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        alert('Thêm sản phẩm vào giỏ hàng thành công!');
                    }
                }
            });
        });

        $('#btnLogout').off('click').on('click', function(e) {
                e.preventDefault(); // xoa su kien cua the hien tai (<a>)
                $('#frmLogout').submit(); 
        });
    }
}
common.init();

