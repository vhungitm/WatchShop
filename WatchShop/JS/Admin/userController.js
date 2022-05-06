var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.ckb-status').off('click').on('click', function (e) {
            e.preventDefault();
            let id = $(this).data('id');

            $.ajax({
                url: "/Admin/User/ChangeStatus",
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
                    alert.text('Cập nhật trạng thái người dùng thành công!');
                    setTimeout(() => {
                        alert.slideUp();
                    }, 3000);
                }
            });
        });
    }
}
user.init();