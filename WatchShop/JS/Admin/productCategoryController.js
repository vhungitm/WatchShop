var proCategory = {
    init: function () {
        proCategory.registerEvents();
    },
    registerEvents: function () {

        // Status
        $('.ckb-status').off('click').on('click', function (e) {
            e.preventDefault();
            let id = $(this).data('id');

            $.ajax({
                url: "/Admin/ProductCategory/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    let label = $('[for="ckbStatus-' + id + '"]');
                    let btn = $('#ckbStatus-' + id);

                    if (response.status == true) {
                        label.text('Kích hoạt');
                        btn.prop('checked', true);
                    }
                    else {
                        label.text('Khóa');
                        btn.prop('checked', false);
                    }

                    let alert = $('#alert');
                    alert.attr('class', 'alert alert-primary');
                    alert.slideDown();
                    alert.text('Cập nhật trạng thái danh mục sản phẩm thành công!');
                    setTimeout(() => {
                        alert.slideUp();
                    }, 3000);
                }
            });
        });


        // Show On Home
        $('.ckb-show-on-home').off('click').on('click', function (e) {
            e.preventDefault();
            let id = $(this).data('id');

            $.ajax({
                url: "/Admin/ProductCategory/ChangeShowOnHome",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    let label = $('[for="ckbShowOnHome-' + id + '"]');
                    let btn = $('#ckbShowOnHome-' + id);

                    if (response.status == true) {
                        label.text('Kích hoạt');
                        btn.prop('checked', true);
                    }
                    else {
                        label.text('Khóa');
                        btn.prop('checked', false);
                    }
                }
            });
        });
    }
}
proCategory.init();