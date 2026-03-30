// ============================================================================
// Utilities & Helpers
// ============================================================================

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}

function displayAlert(message) {
    if (!message) return;

    const isSuccess = message.includes("thành công");
    const alertClass = isSuccess ? "alert-success" : "alert-danger";
    const alertTitle = isSuccess ? "Thành công!" : "Oh snap!";
    const alertHtml = `
        <div class="alert ${alertClass} alert-dismissible custom-alert alert-show" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true"><i class="fal fa-trash-alt"></i></span>
            </button>
            <strong>${alertTitle}</strong> ${message}
        </div>`;

    const $container = $("#alert-container").html(alertHtml);
    const $alert = $container.find(".alert");

    setTimeout(() => $alert.fadeOut(300, () => $alert.remove()), 5000);
    $alert.find(".close").on("click", () => $alert.fadeOut(300, () => $alert.remove()));
}

// ============================================================================
// Summernote
// ============================================================================

function initializeSummernote(selector = '.Note', config = {}) {
    const defaultConfig = {
        height: 200,
        tabsize: 2,
        placeholder: 'Type here...',
        dialogsFade: true,
        toolbar: [
            ['style', ['style']],
            ['font', ['strikethrough', 'superscript', 'subscript']],
            ['font', ['bold', 'italic', 'underline', 'clear']],
            ['fontsize', ['fontsize']],
            ['fontname', ['fontname']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['table', ['table']],
            ['insert', ['link', 'picture', 'video']],
            ['view', ['fullscreen', 'codeview', 'help']]
        ]
    };

    $(selector).summernote({ ...defaultConfig, ...config });
}

// ============================================================================
// NhapBanMu Sections
// ============================================================================

function updateNhapBanMuSections() {
    const loaiHs = $('#LoaiHs').val() || '';
    const loaiTK = $('#LoaiTK').val() || '';

    // Ẩn tất cả bằng class Bootstrap
    $('#loaitk-section, #mua-ngay-section, #mua-thang-section, #ban-section')
        .addClass('d-none');

    if (loaiHs === '3') {
        $('#loaitk-section').removeClass('d-none');

        if (loaiTK === '2') {
            $('#mua-ngay-section').removeClass('d-none');
        } else if (loaiTK === '3') {
            $('#mua-thang-section').removeClass('d-none');
        }
    } else if (loaiHs === '4') {
        $('#ban-section').removeClass('d-none');
    }

    // Force reflow để Bootstrap recalculate grid (rất hiệu quả)
    setTimeout(() => {
        $('.row').css('display', ''); // trigger reflow
    }, 0);
}

function attachNhapBanMuEvents() {
    $('#LoaiHs, #LoaiTK')
        .off('.nbm')
        .on('change.nbm select2:select.nbm select2:unselect.nbm', updateNhapBanMuSections);
}

function cleanupNhapBanMuEvents() {
    $('#LoaiHs, #LoaiTK').off('.nbm');
}

// ============================================================================
// Modal & Select2
// ============================================================================

const MODAL_SELECTOR = '#myModal';

function initSelect2IfNeeded() {
    const $selects = $('.select2, .js-states, .js-select2-icons, .select2-placeholder, .js-hide-search, .js-max-length, .select2-placeholder-multiple, .js-data-example-ajax');

    if (!$selects.length) return;

    $selects.each(function () {
        const $el = $(this);
        if ($el.data('select2')) return;

        let config = { placeholder: 'Chọn...', allowClear: true };

        if ($el.hasClass('js-hide-search')) config.minimumResultsForSearch = Infinity;
        if ($el.hasClass('js-max-length')) config.maximumSelectionLength = 2;
        if ($el.hasClass('select2-placeholder-multiple')) config.placeholder = 'Chọn nhiều...';
        if ($el.hasClass('js-select2-icons')) {
            config.templateResult = formatIcon;
            config.templateSelection = formatIcon;
            config.escapeMarkup = elm => elm;
            config.minimumResultsForSearch = Infinity;
        }
        // Nếu có class ajax → thêm config (hiện tại giữ comment vì chưa dùng thực tế)
        // if ($el.hasClass('js-data-example-ajax')) { config.ajax = { ... } }

        $el.select2(config);
    });
}

// Format cho icons (từ code cũ)
function formatIcon(elm) {
    if (!elm.id) return elm.text;
    return `<i class='${$(elm.element).data('icon') || 'fal fa-question'} mr-2'></i>${elm.text}`;
}

function initModal(element, event) {
    event.preventDefault();
    const $trigger = $(element);
    const $modal = $(MODAL_SELECTOR);

    if (!$modal.length) return console.error('Modal not found:', MODAL_SELECTOR);

    try {
        const dialogWidth = $trigger.attr('dialogwidth') || '800';
        const triggerText = $trigger.text().trim();
        const href = $trigger.attr('href') || '#';
        const cardTitle = $('.card-header').data('title') || '';
        const headerText = $('.card-header h4').text().trim();

        $modal.find('.modal-dialog, .modal-content').css('width', `${dialogWidth}px`);
        $modal.find('.modal-title').html(`
            <h2 class="modal-title h2">
                ${escapeHtml(triggerText)} ${escapeHtml(headerText)}
                <span class="fw-300"><i>${escapeHtml(cardTitle)}</i></span>
            </h2>
        `);

        $modal.find('.modal-body').empty().load(href, (response, status) => {
            if (status !== "success") return console.error('Load partial failed:', href);

            const scripts = [
                '../../Content/smartadmin/js/formplugins/select2/select2.bundle.js',
                '../../Scripts/CascadingDropdownAjax.js'
            ];

            let loaded = 0;
            const total = scripts.length;

            const onAllReady = () => {
                initSelect2IfNeeded();
                if ($('#LoaiHs').length) {
                    setTimeout(() => {
                        updateNhapBanMuSections();
                        attachNhapBanMuEvents();
                    }, 150);
                }
            };

            const check = () => { loaded++; if (loaded === total) onAllReady(); };

            scripts.forEach(src => $.getScript(src).done(check).fail(() => { console.warn(`Script fail: ${src}`); check(); }));
        });

        new bootstrap.Modal($modal[0], { backdrop: 'static', keyboard: true }).show();
        $modal.one('hidden.bs.modal', cleanupNhapBanMuEvents);
    } catch (err) {
        console.error('initModal error:', err);
    }
}

// ============================================================================
// DataTables
// ============================================================================
function initializeDataTables() {
    $('.dt-basic-example').each(function (index) {
        const $table = $(this);

        if ($.fn.DataTable.isDataTable(this)) {
            return;
        }
        $table.DataTable({
            responsive: true,
            destroy: true,
            retrieve: true,
            dom: "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            buttons: [
                { extend: 'csvHtml5', text: 'CSV', className: 'btn-outline-default' },
                { extend: 'copyHtml5', text: 'Copy', className: 'btn-outline-default' },
                { extend: 'print', text: '<i class="fal fa-print"></i>', className: 'btn-outline-default' }
            ],
            fixedHeader: true,
            colReorder: true,
            pageLength: 25,

            // ←←← THÊM PHẦN NÀY ĐỂ SỬA LỖI initApp
            initComplete: function (settings, json) {
              
                // Nếu bạn cần chạy thêm code sau khi table init xong thì viết ở đây
            }
        });
    });
}
// ============================================================================
// Document Ready
// ============================================================================

jQuery(function ($) {
    // Bind modal trigger
    $('.bcommand').on('click', function (e) {
        initModal(this, e);
    });

    initializeDataTables();
    initializeSummernote('.Note');

    const message = window.alertMessage || $("#alert-container").data("message");
    if (message) displayAlert(message);

    $('.is-invalid-cus').each(function () {
        const $el = $(this);
        const check = () => $el.toggleClass('is-invalid', !$el.val().trim());
        check();
        $el.on('input', check);
    });

    // Uncomment nếu cần
    // initializeEmailManager();
    // initializeNewsTicker(tickerConfig);
});