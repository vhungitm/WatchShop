var feedback = {
    init: function () {
        feedback.registerEvents();
    },
    registerEvents: function () {
        $('.ckb-status').off('click').on('click', function (e) {
            e.preventDefault();
            let id = $(this).data('id');

            $.ajax({
                url: "/Admin/Feedback/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    let label = $('[for="ckbStatus-' + id + '"]');
                    let btn = $('#ckbStatus-' + id);

                    if (response.status == true) {
                        label.text('Đã xem');
                        btn.prop('checked', true);
                    }
                    else {
                        label.text('Chưa xem');
                        btn.prop('checked', false);
                    }

                    let alert = $('#alert');
                    alert.attr('class', 'alert alert-primary');
                    alert.slideDown();
                    alert.text('Cập nhật trạng thái feedback thành công!');
                    setTimeout(() => {
                        alert.slideUp();
                    }, 3000);
                }
            });
        });
    }
}
feedback.init();