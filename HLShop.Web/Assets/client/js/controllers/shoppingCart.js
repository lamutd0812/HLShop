var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvents();
    },
    registerEvents: function () {
        $('#btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault(); // xóa điều hướng mặc định của thẻ (<a>)
            var productId = parseInt($(this).data('id')); // lấy thuộc tính data-id của nút hiện tại (data-id="@Model.ID")
            cart.addItem(productId);
        });
        $('.btnDeleteItem').off('click').on('click', function (e) {
           e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });
        $('.txtQuantity').off('keyup').on('keyup', function () {
            var quantity = parseInt($(this).val());
            var productId = parseInt($(this).data('id'));
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
    },
    getTotalAmount: function() {
        var listTextbox = $('.txtQuantity');
        var total = 0;
        $.each(listTextbox, function (i, item) {
            total += parseInt($(this).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },
    addItem: function (productId) {
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
    loadData: function () {
        $.ajax({
            url: '/Cart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function(response) {
                if (response.status) {
                    var template = $('#templateCart').html();
                    var html = '';
                    var data = response.data;
                    $.each(data,
                        function(i, item) {
                            html += Mustache.render(template,
                                {
                                    STT: i+1,
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
                    $('#lblTotalAmount').text(numeral(cart.getTotalAmount()).format('0,0') + '$');
                    cart.registerEvents();
                }
            }
        });
    }
}
cart.init();