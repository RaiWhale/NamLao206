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
    configureDropdown("#CityId", "#DistrictId", "../../../CascadingDropdown/GetDistrictList", "--Chọn Quận/Huyện--");
    configureDropdown("#DistrictId", "#WardId", "../../../CascadingDropdown/GetWardList", "--Chọn Phường/Xã--");
    configureDropdown("#noiCuTruTinhId", "#noiCuTruHuyenId", "../../CascadingDropdown/GetDistrictList", "--Chọn Quận/Huyện--");
    configureDropdown("#noiCuTruHuyenId", "#noiCuTruXaId", "../../CascadingDropdown/GetWardList", "--Chọn Phường/Xã--");
    configureDropdown("#noiOTinhId", "#noiOHuyenId", "../../CascadingDropdown/GetDistrictList", "--Chọn Quận/Huyện--");
    configureDropdown("#noiOHuyenId", "#noiOXaId", "../../CascadingDropdown/GetWardList", "--Chọn Phường/Xã--");
    configureDropdown("#noiDenTinhId1", "#noiDenHuyenId1", "../../CascadingDropdown/GetDistrictList", "--Chọn Quận/Huyện--");
    configureDropdown("#noiDenHuyenId1", "#noiDenXaId1", "../../CascadingDropdown/GetWardList", "--Chọn Phường/Xã--");
    configureDropdown("#noiDenTinhId2", "#noiDenHuyenId2", "../../CascadingDropdown/GetDistrictList", "--Chọn Quận/Huyện--");
    configureDropdown("#noiDenHuyenId2", "#noiDenXaId2", "../../CascadingDropdown/GetWardList", "--Chọn Phường/Xã--");  
    $("#loaiVaiTro").change(function () {
        const selectedValue = $(this).val();
        $('#thongtinbenhnhan').toggle(selectedValue == 2);
        $('#thongtinkhach').toggle(selectedValue == 3);
        $('.benhnhivanguoinha').toggle(selectedValue == 4);
        // Reset required attribute
        $('#tenBn, #soDienThoaiBn').prop("required", selectedValue == 2);
        $('#ghichuKhach').prop("required", selectedValue == 3);
        $('#ngaysinhBn, #tenBenhNhi').prop("required", selectedValue == 4);
        // Reset field values
        $('#ghichuKhach, #soDienThoaiBn, #tenBn, #ngaysinhBn, #tenBenhNhi').val('');
    });
    // Trigger change event on page load
    $("#loaiVaiTro").trigger("change");
    // Hide fields and remove required attributes if no value is selected
    $("#loaiVaiTro").on("focus", function () {
        const selectedValue = $(this).val();
        if (selectedValue == '') {
            $('#thongtinbenhnhan, #thongtinkhach, .benhnhivanguoinha').hide();
            $('#ghichuKhach, #soDienThoaiBn, #tenBn, #ngaysinhBn, #tenBenhNhi').removeAttr('required');
        }
    });



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
    $("#khuphanluongId").empty();
    $('#khuphanluongId').html('<option value="">--Tìm theo khu phân luồng--</option>');
    $("#khuphanluongId").change();
    $.getJSON("../../../CascadingDropdown/GetKhuphanluongIdList", function (data) {
        $.each(data, function (index, row) {
            $("#khuphanluongId").append("<option value='" + row.Id + "'>" + row.TenKhu + "</option>")
        });
    });

    $("#khoaId").empty();
    $('#khoaId').html('<option value="">--Tìm theo khoa--</option>');
    $("#khoaId").change();
    $.getJSON("../../../CascadingDropdown/GetKhoaIdList", function (data) {
        $.each(data, function (index, row) {
            $("#khoaId").append("<option value='" + row.Id + "'>" + row.TenKhoa + "</option>")
        });
    });
});





