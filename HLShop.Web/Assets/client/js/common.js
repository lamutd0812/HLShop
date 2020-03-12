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

        $('#frmAddToCart').validate({
            rules: {
                quantityToAdd: {
                    required: true,
                    min: 1,
                    max: parseInt($('#maxQuantity').val())
                }
            },
            messages: {
                quantityToAdd: {
                    required: "Phải nhập số lượng.",
                    min: "Số lượng ít nhất là 1",
                    max: "Số sản phẩm còn lại không đủ."
                }
            }
        });

        $('.btnAddToCart').off('click').on('click', function (e) {
            var isValid = $('#frmAddToCart').valid();
            if (isValid) {
                var productId = parseInt($(this).data('id')); // lấy thuộc tính data-id của nút hiện tại (data-id="@Model.ID")
                var quantity = parseInt($('#txtQuantityToAdd').val());
                $.ajax({
                    url: '/Cart/Add',
                    data: {
                        productId: productId,
                        quantity: quantity
                    },
                    type: 'POST',
                    dataType: 'json',
                    success: function (response) {
                        if (response.status) {
                            alert('Thêm sản phẩm vào giỏ hàng thành công!');
                        }
                    }
                });
            }
        });

        $('#btnUpdateAccountInfor').off('click').on('click', function(e) {
            e.preventDefault();
            window.location.href = '/cap-nhat-thong-tin.html';
        });

        $('#btnLogout').off('click').on('click', function(e) {
                e.preventDefault(); // xoa su kien cua the hien tai (<a>)
                $('#frmLogout').submit(); 
        });
    }
}
common.init();

