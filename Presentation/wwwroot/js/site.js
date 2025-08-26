// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function ($) {
    const nf = new Intl.NumberFormat('tr-TR', { style: 'currency', currency: 'TRY' });

    // Minicart render
    function renderMiniCart(data) {
        const items = data.items || [];
        let html = '';

        if (items.length === 0) {
            html = '<li class="minicart-product"><p>Sepetiniz boş.</p></li>';
        } else {
            html = items.map(item => `
                <li class="minicart-product" data-productid="${item.productId}">
                    <a class="product-item_remove" href="javascript:void(0)" data-productid="${item.productId}">
                        <i class="ion-android-close"></i>
                    </a>
                    <div class="product-item_img">
                        <img src="/images/${item.imageUrl}" alt="${item.title}">
                    </div>
                    <div class="product-item_content">
                        <a class="product-item_title" href="/Product/ProductDetails/${item.productId}">${item.title}</a>
                        <span class="product-item_quantity">${item.quantity} x ${nf.format(item.price)}</span>
                    </div>
                </li>
            `).join('');
        }

        $('#minicartItems').html(html);
        $('#minicartTotal').text(nf.format(data.total || 0));
        $('.item-count').text(data.cartItemCount || 0);
    }

    // Minicart yükle
    function loadMiniCart() {
        $.get('/Cart/GetCart', renderMiniCart);
    }

    $(function () {
        // Sayfa yüklendiğinde minicart doldur
        loadMiniCart();

        // Sepete ekle
        $(document).off('click', '.add-to-cart-btn');
        $(document).on('click', '.add-to-cart-btn', function (e) {
            e.preventDefault();
            var productId = $(this).data('productid');

            // Eğer ProductDetails sayfasındaysak quantity inputu var
            var quantityInput = $('.cart-plus-minus-box');
            var quantity = quantityInput.length ? parseInt(quantityInput.val()) || 1 : 1;

            $.post('/Cart/AddToCart', { productId: productId, quantity: quantity }, function (res) {
                if (res.success) {
                    loadMiniCart();
                } else {
                    alert(res.message || 'Bir hata oluştu.');
                }
            });
        });

        // Minicart'tan ürün sil
        $(document).off('click', '.product-item_remove');
        $(document).on('click', '.product-item_remove', function (e) {
            e.preventDefault();
            var productId = $(this).data('productid');
            var token = $('#af-form input[name="__RequestVerificationToken"]').val();

            $.post('/Cart/Remove', { productId: productId, __RequestVerificationToken: token }, function (res) {
                if (res.success) {
                    loadMiniCart();
                } else {
                    alert(res.message || 'Bir hata oluştu.');
                }
            });
        });

        // Sepette miktar güncelleme (UpdateQuantity)
        $(document).off('change', '.cart-quantity-input');
        $(document).on('change', '.cart-quantity-input', function () {
            var productId = $(this).data('productid');
            var quantity = parseInt($(this).val()) || 1;

            $.post('/Cart/UpdateQuantity', { productId: productId, quantity: quantity }, function (res) {
                if (res.success) {
                    loadMiniCart();
                } else {
                    alert(res.message || 'Bir hata oluştu.');
                }
            });
        });
    });
})(jQuery);


