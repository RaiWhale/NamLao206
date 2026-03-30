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

    // Auto-hide
    setTimeout(() => $alert.fadeOut(300, () => $alert.remove()), 5000);

    // Manual close
    $alert.find(".close").on("click", () => $alert.fadeOut(300, () => $alert.remove()));
}

// ============================================================================
// Summernote Initialization (SmartAdmin style)
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
// NhapBanMu Sections Logic (đã fix event Select2)
// ============================================================================

function updateNhapBanMuSections() {
    const loaiHs = $('#LoaiHs').val() || '';
    const loaiTK = $('#LoaiTK').val() || '';

    $('#loaitk-section, #mua-ngay-section, #mua-thang-section, #ban-section').hide();

    if (loaiHs === '3') {
        $('#loaitk-section').show();
        if (loaiTK === '2') {
            $('#mua-ngay-section').show();
        } else if (loaiTK === '3') {
            $('#mua-thang-section').show();
        }
    } else if (loaiHs === '4') {
        $('#ban-section').show();
    }
}

function attachNhapBanMuEvents() {
    $('#LoaiHs, #LoaiTK')
        .off('change.nbm select2:select.nbm select2:unselect.nbm')  // Cleanup trước
        .on('change.nbm select2:select.nbm select2:unselect.nbm', updateNhapBanMuSections);
}

function cleanupNhapBanMuEvents() {
    $('#LoaiHs, #LoaiTK').off('.nbm');
}

// ============================================================================
// Modal & Select2 Initialization (tối ưu load script + init)
// ============================================================================

const MODAL_SELECTOR = '#myModal';

function initSelect2IfNeeded() {
    const $selects = $('.select2, .js-states, .js-select2-icons, .select2-placeholder, .js-hide-search, .js-max-length, .select2-placeholder-multiple, .js-data-example-ajax');
    if ($selects.length) {
        $selects.each(function () {
            const $el = $(this);
            if (!$el.data('select2')) {  // Tránh init lại nếu đã có
                // Có thể thêm config tùy theo class nếu cần
                if ($el.hasClass('js-hide-search')) $el.select2({ minimumResultsForSearch: Infinity });
                else if ($el.hasClass('js-max-length')) $el.select2({ maximumSelectionLength: 2 });
                else if ($el.hasClass('js-data-example-ajax')) $el.select2({ /* ajax config nếu cần */ });
                else $el.select2();
            }
        });
    }
}

function initModal(element, event) {
    event.preventDefault();
    const $trigger = $(element);
    const $modal = $(MODAL_SELECTOR);

    if (!$modal.length) return console.error('Modal not found:', MODAL_SELECTOR);

    try {
        const dialogWidth = $trigger.attr('dialogwidth') || '800';
        const triggerText = $trigger.text().trim();
        const href = $trigger.attr('href');
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
            if (status !== "success") return console.error('Load partial failed');

            const scripts = [
                '../../Content/smartadmin/js/formplugins/select2/select2.bundle.js',
                '../../Scripts/CascadingDropdownAjax.js'
            ];

            let loaded = 0;
            const total = scripts.length;

            const checkAllLoaded = () => {
                loaded++;
                if (loaded === total) onScriptsReady();
            };

            const onScriptsReady = () => {
                initSelect2IfNeeded();  // Init Select2 sau script load

                if ($('#LoaiHs').length) {
                    setTimeout(() => {
                        updateNhapBanMuSections();
                        attachNhapBanMuEvents();
                    }, 150);  // Đợi Select2 render value
                }
            };

            scripts.forEach(src => $.getScript(src).done(checkAllLoaded).fail(() => {
                console.warn(`Script load fail: ${src}`);
                checkAllLoaded();
            }));
        });

        new bootstrap.Modal($modal[0], { backdrop: 'static', keyboard: true }).show();

        $modal.one('hidden.bs.modal', cleanupNhapBanMuEvents);
    } catch (err) {
        console.error('initModal error:', err);
    }
}

// ============================================================================
// DataTables Initialization
// ============================================================================

function initializeDataTables() {
    if ($('.dt-basic-example').length) {
        $('.dt-basic-example').DataTable({
            responsive: true,
            dom: "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            buttons: [
                { extend: 'csvHtml5', text: 'CSV', className: 'btn-outline-default' },
                { extend: 'copyHtml5', text: 'Copy', className: 'btn-outline-default' },
                { extend: 'print', text: '<i class="fal fa-print"></i>', className: 'btn-outline-default' }
            ],
            fixedHeader: true,
            colReorder: true
        });
    }
}

// ============================================================================
// Document Ready - Central Initialization
// ============================================================================

jQuery(function ($) {
    // Core init
    initializeModalAndAlert();   // Bao gồm bind .bcommand click → initModal
    initializeDataTables();
    initializeSummernote('.Note');

    // Alert từ window hoặc data
    const message = window.alertMessage || $("#alert-container").data("message");
    if (message) displayAlert(message);

    // Input validation visual
    $('.is-invalid-cus').each(function () {
        const $this = $(this);
        const check = () => $this.toggleClass('is-invalid', !$this.val().trim());

        check();
        $this.on('input', check);
    });

    // Uncomment nếu cần
    // initializeEmailManager();
    // initializeNewsTicker(tickerConfig);
});