$(document).ready(function () {
    //Action Command
    $(".bcommand").click(function (event) {
        var _text = $(this).text();
        var _href = $(this).attr("href");
        var _w = $(this).attr("dialogwidth");
        //$.getScript("../../Scripts/CascadingDropdownAjax.js");
        //$.getScript("../../Content/AdminBinhLT/assets/js/app.js");
        $("#myModal .modal-dialog").width(_w + "px");
        $("#myModal .modal-content").width(_w + "px");
        $("#myModal .modal-title").html(_text + " " + $(".card-header h4").text());
        $("#myModal .modal-body").html("");
    /*    $("#myModal .modal-body").load(_href);*/
        $("#myModal .modal-body").load(_href, function () {
            // Thêm đoạn mã JS để load thư viện khác vào đây
            $.getScript("../../Scripts/CascadingDropdownAjax.js");
            $.getScript("../../Content/AdminBinhLT/assets/js/app.js");
           
        });
        $("#myModal").modal({ backdrop: 'static', keyboard: true });
        event.preventDefault();
    });

    $('.is-invalid-cus').each(function () {
        if ($(this).val() == '') {
            $(this).addClass('is-invalid');
        }
        $(this).on('input', function () {
            if ($(this).val() != '') {
                $(this).removeClass('is-invalid');
            } else {
                $(this).addClass('is-invalid');
            }
        });
    });

    $('.datecus').each(function () {
        var id = $(this).attr('id');
        var cleave = new Cleave('#' + id, {
            date: true,
            delimiter: '-',
            datePattern: ['d', 'm', 'Y']
        });
    });

    $('.textarea-cus').each(function () {
        var id = $(this).attr('id');
        CKEDITOR.replace(id, {
            customConfig: "../../../Content/PluginCK/ckeditor/config.js"
        });
    });  

    //var ckEditor = CKEDITOR.replace('Details',
    //    { customConfig: '../../../Content/PluginCK/ckeditor/config.js', });
});
