function populateDropdown(sourceId, targetId, url, optionText) {
    $(targetId).empty();
    $(targetId).html('<option value="">' + optionText + '</option>');
    $(targetId).change();
    $.getJSON(url, { sourceId: $(sourceId).val() }, function (data) {
        $.each(data, function (index, row) {
            $(targetId).append("<option value='" + row.Id + "'>" + row.Ten + "</option>");
        });
    });
}
function configureDropdown(sourceId, targetId, url, optionText) {
    $(sourceId).change(function () {
        populateDropdown(sourceId, targetId, url, optionText);
    });
}

$(document).ready(function () { 
    $("#TopicId").change(function () {
        $("#SubMenuId").empty();
        $('#SubMenuId').html('<option value="">--Chọn Submenu--</option>');
        $("#SubMenuId").change();
        $.getJSON("../../../CascadingDropdown/GetSubmenusList", { sourceId: $("#TopicId").val() }, function (data) {
            $.each(data, function (index, row) {
                $("#SubMenuId").append("<option value='" + row.Id + "'>" + row.subMenuName + "</option>")
            });
        });
    })
});





