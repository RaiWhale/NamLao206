// Hàm khởi tạo cascading dropdown
function initializeCascadingDropdown(config) {
    const { sourceSelector, targetSelector, targetSelector2, url, placeholderText } = config;

    // Kiểm tra tham số đầu vào
    if (!sourceSelector || !targetSelector || !url || !placeholderText) {
        console.error('Tham số không hợp lệ:', config);
        return;
    }

    // Lưu trữ tham chiếu đến các phần tử DOM
    const $source = $(sourceSelector);
    const $target = $(targetSelector);
    const $target2 = $(targetSelector2);
    // Kiểm tra sự tồn tại của các phần tử
    if (!$source.length || !$target.length) {
        return;
    }

    // Hàm xóa thông báo lỗi cũ
    function clearError() {
        $target.next('.text-danger').remove();
    }

    // Hàm hiển thị thông báo lỗi
    function showError(message) {
        clearError();
        $target.after(`<div class="text-danger">${message}</div>`);
    }

    // Hàm xử lý sự kiện change của dropdown nguồn
    async function handleSourceChange() {
        try {
            // Xóa thông báo lỗi cũ
            clearError();

            // Xóa nội dung cũ và thêm placeholder
            $target.empty().append(`<option value="">${placeholderText}</option>`);
            if ($target2.length) {
                $target2.empty().append(`<option value="">--Chọn Phường/Xã--</option>`);
            }

            // Lấy giá trị từ dropdown nguồn
            const sourceValue = $source.val();
            if (!sourceValue) {
                return; // Không gọi API nếu không có giá trị
            }

            // Gọi API để lấy dữ liệu
            const response = await fetch(`${url}?sourceId=${encodeURIComponent(sourceValue)}`, {
                method: 'GET',
                headers: { 'Accept': 'application/json' }
            });

            if (!response.ok) {
                throw new Error(`Lỗi khi gọi API: ${response.status} - ${response.statusText}`);
            }

            const data = await response.json();
            if (!Array.isArray(data)) {
                throw new Error('Dữ liệu trả về không phải là mảng');
            }

            // Tạo options từ dữ liệu API
            const options = data
                .filter(item => item && typeof item.Id !== 'undefined' && typeof item.Ten !== 'undefined')
                .map(item => `<option value="${item.Id}">${item.Ten}</option>`)
                .join('');

            // Thêm options vào dropdown
            $target.append(options);

            // Kích hoạt sự kiện change
            $target.trigger('change');
        } catch (error) {
            console.error('Lỗi khi điền dữ liệu vào dropdown:', error);
            showError('Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại.');
        }
    }

    // Gán sự kiện change cho dropdown nguồn
    $source.on('change', handleSourceChange);

    // Gọi lần đầu nếu có giá trị ban đầu
    //if ($source.val()) {
    //    handleSourceChange();
    //}
}

// Hàm khởi tạo cascading dropdown cho TopicId và SubMenuId
function initializeSubmenuDropdown() {
    // Tham chiếu DOM
    const selectors = {
        topicDropdown: '#TopicId',
        submenuDropdown: '#SubMenuId',
        errorContainer: 'dropdown-error'
    };

    // Lưu trữ jQuery objects
    const $elements = {
        $topic: $(selectors.topicDropdown),
        $submenu: $(selectors.submenuDropdown)
    };

    // Kiểm tra sự tồn tại của các phần tử
    if (!$elements.$topic.length || !$elements.$submenu.length) {
        return;
    }

    // Hàm xóa thông báo lỗi cũ
    function clearError() {
        $(`#${selectors.errorContainer}`).remove();
    }

    // Hàm hiển thị thông báo lỗi
    function showError(message) {
        clearError();
        $elements.$submenu.after(
            `<div id="${selectors.errorContainer}" class="text-danger">${message}</div>`
        );
    }

    // Hàm lấy danh sách submenu từ API
    async function fetchSubmenus(topicId) {
        // Sử dụng URL động từ Razor (giả sử được truyền qua ViewBag hoặc data attribute)
        const apiUrl = $elements.$topic.data('api-url') || '/CascadingDropdown/GetSubmenusList';
        const url = `${apiUrl}?sourceId=${encodeURIComponent(topicId)}`;

        const response = await fetch(url, {
            method: 'GET',
            headers: { 'Accept': 'application/json' }
        });

        if (!response.ok) {
            throw new Error(`API error: ${response.status} - ${response.statusText}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Invalid data: Expected an array');
        }

        return data;
    }

    // Hàm xử lý sự kiện change của TopicId
    async function handleTopicChange() {
        try {
            // Xóa thông báo lỗi cũ
            clearError();

            // Xóa nội dung cũ và thêm placeholder
            $elements.$submenu.empty().append('<option value="">--Chọn Submenu--</option>');

            // Lấy topicId
            const topicId = $elements.$topic.val();
            if (!topicId) {
                return; // Thoát nếu không có giá trị
            }

            // Gọi API
            const data = await fetchSubmenus(topicId);

            // Tạo options
            const options = data
                .filter(item => item && typeof item.Id !== 'undefined' && typeof item.subMenuName !== 'undefined')
                .map(item => `<option value="${item.Id}">${item.subMenuName}</option>`)
                .join('');

            // Thêm options
            $elements.$submenu.append(options);

            // Kích hoạt sự kiện change
            $elements.$submenu.trigger('change');
        } catch (error) {
            console.error('Error populating SubMenuId dropdown:', error);
            showError('Đã xảy ra lỗi khi tải danh sách submenu. Vui lòng thử lại.');
        }
    }

    // Gắn sự kiện change
    $elements.$topic.on('change', handleTopicChange);

    // Gọi lần đầu nếu có giá trị ban đầu
    if ($elements.$topic.val()) {
        handleTopicChange();
    }
} function configureDropdown(sourceSelector, targetSelector, url, placeholderText) {
    // Kiểm tra tham số đầu vào
    if (!sourceSelector || !targetSelector || !url || !placeholderText) {
        console.error('Tham số không hợp lệ:', { sourceSelector, targetSelector, url, placeholderText });
        return;
    }

    // Kiểm tra sự tồn tại của hàm populateDropdown
    if (typeof populateDropdown !== 'function') {
        console.error('Hàm populateDropdown không được định nghĩa.');
        return;
    }

    // Lưu trữ tham chiếu đến dropdown nguồn
    const $source = $(sourceSelector);

    // Kiểm tra sự tồn tại của dropdown nguồn
    if (!$source.length) {
        console.error(`Không tìm thấy dropdown nguồn: ${sourceSelector}`);
        return;
    }

    // Gán sự kiện change cho dropdown nguồn
    $source.on('change', () => {
        populateDropdown(sourceSelector, targetSelector, url, placeholderText);
    });

    // Kích hoạt sự kiện change ngay lập tức để điền dữ liệu ban đầu (nếu cần)
    $source.trigger('change');
}
// Lưu trữ tham chiếu đến các phần tử DOM để tăng hiệu suất
const $radio1 = $("#customRadio-1");
const $radio2 = $("#customRadio-2");
const $radio3 = $("#customRadio-3");
const $dropdown1 = $("#dropdown1");
const $dropdown2 = $("#dropdown2");
const $dropdown3 = $("#dropdown3");
const $dropdown1Select = $("#lstReceiverUserId");
const $dropdown2Select = $("#lstReceiverRoomId");
const $dropdown3Select = $("#lstReceiverUnitId");
// Xử lý sự kiện change cho các radio buttons
$radio1.add($radio2).add($radio3).on('change', function () {

    // Ẩn tất cả dropdown và giữ trạng thái ban đầu
    $dropdown1.prop('hidden', true);
    $dropdown2.prop('hidden', true);
    $dropdown3.prop('hidden', true);

    // Xử lý dựa trên radio button được chọn
    if ($radio1.is(':checked')) {
        // Radio 1: Hiển thị dropdown 1 (Chọn người gửi), xóa dữ liệu trên dropdown 3
        $dropdown1.prop('hidden', false);
        if ($dropdown3Select.length) {
            $dropdown3Select.val(null).trigger('change'); // Xóa dữ liệu trên dropdown 3 (Select2)
        }
    } else if ($radio2.is(':checked')) {
        // Radio 2: Hiển thị dropdown 1 và 2 (Chọn người gửi và phòng gửi), xóa dữ liệu trên dropdown 3
        $dropdown1.prop('hidden', false);
        $dropdown2.prop('hidden', false);
        if ($dropdown3Select.length) {
            $dropdown3Select.val(null).trigger('change'); // Xóa dữ liệu trên dropdown 3 (Select2)
        }
    } else if ($radio3.is(':checked')) {
        // Radio 3: Hiển thị dropdown 3 (Chọn đơn vị gửi), xóa dữ liệu trên dropdown 1 và 2
        $dropdown3.prop('hidden', false);
        $dropdown1Select.val(null).trigger('change'); // Xóa dữ liệu trên dropdown 1 (Select2)
        $dropdown2Select.val(null).trigger('change'); // Xóa dữ liệu trên dropdown 2 (Select2)
    }
});

$(document).ready(function () {

    $("#dropdown1").prop('hidden', true);
    $("#dropdown2").prop('hidden', true);
    $("#dropdown3").prop('hidden', true);
    // Lưu trữ tham chiếu đến các phần tử DOM
    initializeSubmenuDropdown()
    initializeCascadingDropdown({
        sourceSelector: '#TopicId',
        targetSelector: '#SubMenuId',
        url: '/CascadingDropdown/GetSubmenusList', // Thay bằng URL thực tế
        placeholderText: '-- Chọn Submenu --'
    });
    initializeCascadingDropdown({
        sourceSelector: '#CityId',
        targetSelector: '#DistrictId',
        targetSelector2: '#WardId',
        url: '/CascadingDropdown/GetDistrictList', // Thay bằng URL thực tế
        placeholderText: '--Chọn Quận/Huyện--'
    });
    initializeCascadingDropdown({
        sourceSelector: '#DistrictId',
        targetSelector: '#WardId',
        url: '/CascadingDropdown/GetWardList', // Thay bằng URL thực tế
        placeholderText: '--Chọn Phường/Xã--'
    });
});





