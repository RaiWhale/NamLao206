(function () {
    // ==================== CÁC HÀM TIỆN ÍCH ====================

    /**
     * Hàm khởi tạo cascading dropdown tổng quát
     * @param {Object} config - Cấu hình
     * @param {string} config.sourceSelector - Selector của dropdown nguồn (cha)
     * @param {string} config.targetSelector - Selector của dropdown đích (con)
     * @param {string} [config.targetSelector2] - Selector của dropdown đích cấp 2 (tuỳ chọn)
     * @param {string} config.url - API endpoint (sẽ được gọi với ?sourceId=...)
     * @param {string} config.placeholderText - Text cho option mặc định
     */
    function initializeCascadingDropdown(config) {
        const { sourceSelector, targetSelector, targetSelector2, url, placeholderText } = config;

        if (!sourceSelector || !targetSelector || !url || !placeholderText) {
            console.error('Tham số không hợp lệ:', config);
            return;
        }

        const $source = $(sourceSelector);
        const $target = $(targetSelector);
        const $target2 = targetSelector2 ? $(targetSelector2) : null;

        if (!$source.length || !$target.length) {
            return;
        }

        function clearError() {
            $target.next('.text-danger').remove();
        }

        function showError(message) {
            clearError();
            $target.after(`<div class="text-danger">${message}</div>`);
        }

        async function handleSourceChange() {
            try {
                clearError();

                // Reset dropdown đích
                $target.empty().append(`<option value="">${placeholderText}</option>`);
                if ($target2 && $target2.length) {
                    $target2.empty().append(`<option value="">--Chọn Phường/Xã--</option>`);
                }

                const sourceValue = $source.val();
                if (!sourceValue) return;

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

                const options = data
                    .filter(item => item && typeof item.Id !== 'undefined' && typeof item.Ten !== 'undefined')
                    .map(item => `<option value="${item.Id}">${item.Ten}</option>`)
                    .join('');

                $target.append(options);
                $target.trigger('change');
            } catch (error) {
                console.error('Lỗi khi điền dữ liệu vào dropdown:', error);
                showError('Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại.');
            }
        }

        $source.on('change', handleSourceChange);

        // Nếu dropdown nguồn đã có giá trị ban đầu, gọi luôn để load dữ liệu
        if ($source.val()) {
            handleSourceChange();
        }
    }

    /**
     * Hàm khởi tạo cascading dropdown cho TopicId -> SubMenuId
     * (Sử dụng cấu trúc dữ liệu riêng: item.Id, item.subMenuName)
     */
    function initializeSubmenuDropdown() {
        const selectors = {
            topicDropdown: '#TopicId',
            submenuDropdown: '#SubMenuId',
            errorContainer: 'dropdown-error'
        };

        const $topic = $(selectors.topicDropdown);
        const $submenu = $(selectors.submenuDropdown);

        if (!$topic.length || !$submenu.length) return;

        function clearError() {
            $(`#${selectors.errorContainer}`).remove();
        }

        function showError(message) {
            clearError();
            $submenu.after(`<div id="${selectors.errorContainer}" class="text-danger">${message}</div>`);
        }

        async function fetchSubmenus(topicId) {
            // Lấy URL từ data attribute hoặc dùng mặc định
            const apiUrl = $topic.data('api-url') || '/CascadingDropdown/GetSubmenusList';
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

        async function handleTopicChange() {
            try {
                clearError();
                $submenu.empty().append('<option value="">--Chọn Submenu--</option>');

                const topicId = $topic.val();
                if (!topicId) return;

                const data = await fetchSubmenus(topicId);

                const options = data
                    .filter(item => item && typeof item.Id !== 'undefined' && typeof item.subMenuName !== 'undefined')
                    .map(item => `<option value="${item.Id}">${item.subMenuName}</option>`)
                    .join('');

                $submenu.append(options);
                $submenu.trigger('change');
            } catch (error) {
                console.error('Error populating SubMenuId dropdown:', error);
                showError('Đã xảy ra lỗi khi tải danh sách submenu. Vui lòng thử lại.');
            }
        }

        $topic.on('change', handleTopicChange);

        if ($topic.val()) {
            handleTopicChange();
        }
    }

    /**
     * Hàm cấu hình dropdown (dùng chung, yêu cầu hàm populateDropdown tồn tại)
     * Lưu ý: Hàm này phụ thuộc vào populateDropdown, nếu không dùng thì có thể bỏ qua.
     */
    function configureDropdown(sourceSelector, targetSelector, url, placeholderText) {
        if (!sourceSelector || !targetSelector || !url || !placeholderText) {
            console.error('Tham số không hợp lệ:', { sourceSelector, targetSelector, url, placeholderText });
            return;
        }

        if (typeof populateDropdown !== 'function') {
            console.error('Hàm populateDropdown không được định nghĩa.');
            return;
        }

        const $source = $(sourceSelector);
        if (!$source.length) {
            console.error(`Không tìm thấy dropdown nguồn: ${sourceSelector}`);
            return;
        }

        $source.on('change', () => {
            populateDropdown(sourceSelector, targetSelector, url, placeholderText);
        });

        $source.trigger('change');
    }

    // ==================== XỬ LÝ RADIO BUTTON ====================
    // Lưu tham chiếu đến các phần tử DOM
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
        // Ẩn tất cả dropdown
        $dropdown1.prop('hidden', true);
        $dropdown2.prop('hidden', true);
        $dropdown3.prop('hidden', true);

        if ($radio1.is(':checked')) {
            // Radio 1: Hiển thị dropdown 1, xóa dữ liệu dropdown 3
            $dropdown1.prop('hidden', false);
            if ($dropdown3Select.length) {
                $dropdown3Select.val(null).trigger('change');
            }
        } else if ($radio2.is(':checked')) {
            // Radio 2: Hiển thị dropdown 1 và 2, xóa dữ liệu dropdown 3
            $dropdown1.prop('hidden', false);
            $dropdown2.prop('hidden', false);
            if ($dropdown3Select.length) {
                $dropdown3Select.val(null).trigger('change');
            }
        } else if ($radio3.is(':checked')) {
            // Radio 3: Hiển thị dropdown 3, xóa dữ liệu dropdown 1 và 2
            $dropdown3.prop('hidden', false);
            if ($dropdown1Select.length) {
                $dropdown1Select.val(null).trigger('change');
            }
            if ($dropdown2Select.length) {
                $dropdown2Select.val(null).trigger('change');
            }
        }
    });

    // ==================== KHỞI TẠO KHI TRANG SẴN SÀNG ====================
    $(document).ready(function () {
        // Ẩn các dropdown ban đầu
        $("#dropdown1, #dropdown2, #dropdown3").prop('hidden', true);

        // Khởi tạo cascading dropdown cho CityId -> DistrictId -> WardId
        initializeCascadingDropdown({
            sourceSelector: '#CityId',
            targetSelector: '#DistrictId',
            targetSelector2: '#WardId',
            url: '/CascadingDropdown/GetDistrictList',
            placeholderText: '--Chọn Quận/Huyện--'
        });

        initializeCascadingDropdown({
            sourceSelector: '#DistrictId',
            targetSelector: '#WardId',
            url: '/CascadingDropdown/GetWardList',
            placeholderText: '--Chọn Phường/Xã--'
        });

        // Khởi tạo dropdown cho TopicId -> SubMenuId (dùng hàm riêng)
        initializeSubmenuDropdown();

        // KHÔNG gọi initializeCascadingDropdown cho TopicId nữa để tránh trùng lặp
        // Nếu bạn muốn dùng hàm tổng quát cho TopicId -> SubMenuId,
        // hãy sửa initializeSubmenuDropdown để gọi lại initializeCascadingDropdown hoặc dùng trực tiếp.
    });

})();