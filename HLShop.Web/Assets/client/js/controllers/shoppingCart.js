var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvents();
    },
    registerEvents: function () {
        // btnAddToCart : binding trong file common.js để binding đc ở all partial page

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id')); // lấy thuộc tính data-id của nút hiện tại (data-id="@Model.ID")
            cart.deleteItem(productId);
        });

        $('.txtQuantity').off('change').on('change', function () {
            var quantity = parseInt($(this).val());
            var productId = parseInt($(this).data('id'));

            //set min quantity
            var min = parseInt($(this).attr('min'));
            if (quantity < min) {
                quantity = min;
            }

            // update item quantity
            cart.updateItemQuantity(productId, quantity);

            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {
                var amount = quantity * price;
                $('#amount_' + productId).text(numeral(amount).format('0,0') + '$');
            } else {
                $('#amount_' + productId).text('0');
            }

            $('#lblTotalAmount').text(numeral(cart.getTotalAmount()).format('0,0') + '$');
        });

        $('#btnContinueShopping').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });

        $('#btnDeleteAllItem').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAllItem();
            $('#divCheckout').hide();
        });

        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });

        $('#checkUserLoginInfor').off('click').on('click', function (e) {
            if ($(this).prop('checked')) {
                cart.getLoginUser();
            } else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtPhone').val('');
                $('#txtEmail').val('');
            }
        });

        //Create Order Form Validation (using jquery-validation)
        $('#frmPayment').validate({
            rules: {
                name: "required",
                address: "required",
                phone: {
                    required: true,
                    number: true
                },
                email: {
                    required: true,
                    email: true
                }
            },
            message: {
                name: "Tên không được bỏ trống.",
                address: "Địa chỉ không được bỏ trống.",
                phone: {
                    required: "Số điện thoại không được bỏ trống.",
                    number: "Sai định dạng sđt."
                },
                email: {
                    required: "Email không được bỏ trống.",
                    email: "Sai định dạng email."
                }
            }
        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmPayment').valid();
            if (isValid) {
                cart.createOrder();
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/Cart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.Fullname);
                    $('#txtAddress').val(user.Address);
                    $('#txtPhone').val(user.PhoneNumber);
                    $('#txtEmail').val(user.Email);
                }
            }
        });
    },
    createOrder: function () {
        var order = {
            CustomerName : $('#txtName').val(),
            CustomerAddress : $('#txtAddress').val(),
            CustomerMobile : $('#txtPhone').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMessage : $('#txtMessage').val(),
            PaymentMethod : "Thanh toán: COD",
            Status : false
        }
        $.ajax({
            url: '/Cart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    $('#divCheckout').hide();
                    cart.deleteAllItem();
                    // set time out để không gọi ngay cart.loadData(); trong cart.deleteAllItem(); để $('#cartContent') hiển thị đc.
                    setTimeout(function() {
                        $('#cartContent').html('Thanh toán thành công! Chúng tôi sẽ liên hệ với bạn sớm nhất có thế.');
                    }, 2000);
                }
            }
        });
    },
    getTotalAmount: function () {
        var listTextbox = $('.txtQuantity');
        var total = 0;
        $.each(listTextbox, function (i, item) {
            total += parseInt($(this).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    updateItemQuantity: function (productId, quantity) {
        $.ajax({
            url: '/Cart/UpdateQuantity',
            data: {
                productId: productId,
                quantity: quantity
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                }
            }
        });
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/Cart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    alert('Xóa sản phẩm thành công!');
                    cart.loadData();
                }
            }
        });
    },
    deleteAllItem: function () {
        $.ajax({
            url: '/Cart/DeleteAllItem',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    //alert('Xóa giỏ hàng thành công!');
                    cart.loadData();
                }
            }
        });
    },
    loadData: function () {
        $.ajax({
            url: '/Cart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var template = $('#cartTemplate').html();
                    var html = '';
                    var data = response.data;
                    $.each(data,
                        function (i, item) {
                            html += Mustache.render(template,
                                {
                                    STT: i + 1,
                                    ProductId: item.ProductId,
                                    ProductName: item.Product.Name,
                                    Image: item.Product.Image,
                                    Price: item.Product.Price,
                                    PriceF: numeral(item.Product.Price).format('0,0'),
                                    Quantity: item.Quantity,
                                    Amount: numeral(item.Quantity * item.Product.Price).format('0,0') + '$'
                                });
                        });

                    $('#cartBody').html(html);

                    if (html == '') {
                        $('#cartContent').html('Không có sản phẩm nào trong giỏ hàng.');
                    }
                    $('#lblTotalAmount').text(numeral(cart.getTotalAmount()).format('0,0') + '$');
                    cart.registerEvents();
                }
            }
        });
    }
}
cart.init();