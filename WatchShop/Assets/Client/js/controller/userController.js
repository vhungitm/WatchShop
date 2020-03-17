var user = {
    init: function () {

        user.loadProvince();
        user.registerEvent();
    },
    registerEvent: function () {
        $('li').off('click').on('click',function () {
            var id = '';
            if (id != '') {
                user.loadDistrict(parseInt(id));
            }
            else {
                $('#ddlDistrict').html('');
            }
        });
    },
    loadProvince: function () {
        $.ajax({
            url: '/User/LoadProvince',
            type: "POST",
            dataType: "Json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">Chọn tỉnh thành</option>';
                    var html2 = '';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                        html2 += '<li class="option" value="' + item.ID + '">' + item.Name + '</li>';
                    });
                    $(".current:first").html('Chọn tỉnh thành')
                    $('#ddlProvince').html(html);
                    $('.nice-select:first').removeClass().addClass('nice-select form-control').attr('style','width: 360px');
                    $('.nice-select.form-control ul:first').html(html2);
                }
            }
        })
    },
    loadDistrict: function () {
        $.ajax({
            url: '/User/LoadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "Json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">Chọn tỉnh thành</option>';
                    var html2 = '';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                        html2 += '<li class="option" value="' + item.ID + '">' + item.Name + '</li>';
                    });
                    $(".current:second").html('Chọn tỉnh thành')
                    $('#ddlDistrict').html(html);
                    $('.nice-select:second').removeClass().addClass('nice-select form-control').attr('style', 'width: 360px');
                    $('.nice-select.form-control ul:second').html(html2);
                }
            }
        })
    }
}
user.init();
 
