async function populateDropdown(sourceSelector, targetSelector, url, placeholderText) {
    // Lưu trữ tham chiếu đến các phần tử DOM
    const $source = $(sourceSelector);
    const $target = $(targetSelector);

    // Kiểm tra sự tồn tại của các phần tử
    if (!$source.length || !$target.length) {
        console.error(`Không tìm thấy phần tử: sourceSelector=${sourceSelector}, targetSelector=${targetSelector}`);
        return;
    }

    try {
        // Xóa nội dung cũ và thêm placeholder
        $target.empty().append(`<option value="">${placeholderText}</option>`);

        // Lấy giá trị từ dropdown nguồn
        const sourceValue = $source.val();
        if (!sourceValue) {
            return; // Không gọi API nếu không có giá trị nguồn
        }

        // Gọi API để lấy dữ liệu
        const response = await fetch(`${url}?sourceId=${encodeURIComponent(sourceValue)}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`Lỗi khi gọi API: ${response.status} - ${response.statusText}`);
        }

        const data = await response.json();

        // Kiểm tra dữ liệu trả về
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xây dựng HTML cho các option
        const options = data.map(item => {
            if (!item || typeof item.Id === 'undefined' || typeof item.Ten === 'undefined') {
                console.warn('Phần tử dữ liệu không hợp lệ:', item);
                return '';
            }
            return `<option value="${item.Id}">${item.Ten}</option>`;
        }).filter(option => option !== '');

        // Thêm tất cả option vào dropdown một lần duy nhất
        $target.append(options.join(''));

        // Kích hoạt sự kiện change (nếu cần)
        $target.trigger('change');
    } catch (error) {
        console.error('Lỗi khi điền dữ liệu vào dropdown:', error);
        // Hiển thị thông báo lỗi cho người dùng (tùy chọn)
        $target.after('<div class="text-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại.</div>');
    }
}
function configureDropdown(sourceSelector, targetSelector, url, placeholderText) {
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
    const $topicDropdown = $("#TopicId");
    const $submenuDropdown = $("#SubMenuId");

    // Kiểm tra sự tồn tại của các phần tử
    if (!$topicDropdown.length || !$submenuDropdown.length) {
        console.error("Không tìm thấy dropdown: #TopicId hoặc #SubMenuId");
        return;
    }

    // Gán sự kiện change cho dropdown #TopicId
    $topicDropdown.on('change', async () => {
        try {
            // Xóa nội dung cũ và thêm placeholder
            $submenuDropdown.empty().append('<option value="">--Chọn Submenu--</option>');

            // Lấy giá trị từ dropdown #TopicId
            const topicId = $topicDropdown.val();
            if (!topicId) {
                return; // Không gọi API nếu không có giá trị
            }

            // Gọi API để lấy danh sách submenu
            const response = await fetch(`../../../CascadingDropdown/GetSubmenusList?sourceId=${encodeURIComponent(topicId)}`, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`Lỗi khi gọi API: ${response.status} - ${response.statusText}`);
            }

            const data = await response.json();

            // Kiểm tra dữ liệu trả về
            if (!Array.isArray(data)) {
                throw new Error('Dữ liệu trả về không phải là mảng');
            }

            // Xây dựng HTML cho các option
            const options = data.map(item => {
                if (!item || typeof item.Id === 'undefined' || typeof item.subMenuName === 'undefined') {
                    console.warn('Phần tử dữ liệu không hợp lệ:', item);
                    return '';
                }
                return `<option value="${item.Id}">${item.subMenuName}</option>`;
            }).filter(option => option !== '');

            // Thêm tất cả option vào dropdown một lần duy nhất
            $submenuDropdown.append(options.join(''));

            // Kích hoạt sự kiện change (nếu cần)
            $submenuDropdown.trigger('change');
        } catch (error) {
            console.error('Lỗi khi điền dữ liệu vào dropdown #SubMenuId:', error);
            // Hiển thị thông báo lỗi cho người dùng (tùy chọn)
            $submenuDropdown.after('<div class="text-danger">Đã xảy ra lỗi khi tải danh sách submenu. Vui lòng thử lại.</div>');
        }
    });

    // Kích hoạt sự kiện change ngay lập tức để điền dữ liệu ban đầu (nếu cần)
    $topicDropdown.trigger('change');


});





