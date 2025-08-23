//Summernote JS SmartAdmin 
function initializeSummernote(selector,config = {}) {
    // Cấu hình mặc định cho Summernote
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
    // Kết hợp cấu hình mặc định với cấu hình tùy chỉnh
    const summernoteConfig = { ...defaultConfig, ...config };
    $(selector).summernote(summernoteConfig);
}

// Optional: Auto-initialize if needed
// $(document).ready(() => initializeSummernote());
//End Summernote JS SmartAdmin 
function displayAlert(message) {
    if (!message) return;
    const alertClass = message.includes("thành công") ? "alert-success" : "alert-danger";
    const alertTitle = message.includes("thành công") ? "Thành công!" : "Oh snap!";
    const alertHtml = `
        <div class="alert ${alertClass} alert-dismissible custom-alert alert-show" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true"><i class="fal fa-trash-alt"></i></span>
            </button>
            <strong>${alertTitle}</strong> ${message}
        </div>
    `;
    const $container = $("#alert-container");
    const $alert = $(alertHtml);
    $container.html($alert);
    setTimeout(() => {
        $alert.removeClass("alert-show").addClass("alert-hide").fadeOut(300, () => {
            $alert.remove();
        });
    }, 5000);
    $alert.find(".close").on("click", () => {
        $alert.removeClass("alert-show").addClass("alert-hide").fadeOut(300, () => {
            $alert.remove();
        });
    });
}
// Function to initialize and manage news ticker
function initializeNewsTicker(config) {
    // Initialize news ticker with provided settings
    $('.newstape').newstape(config.newstapeSettings);

    // Manage auto-scroll interval
    let intervalTime = config.defaultInterval;
    let autoScrollInterval;

    function startSetInterval(time) {
        clearInterval(autoScrollInterval); // Prevent duplicate intervals
        autoScrollInterval = setInterval(() => {
            // Assuming newstape plugin handles scrolling internally
            // Add custom scroll logic here if needed
        }, time);
    }

    // Hover event for canvas cover
    $('.canvasCover').hover(
        () => {
            intervalTime = config.hoverInterval;
            startSetInterval(intervalTime);
            // $('.canvasCover').css('overflow-y', 'hidden'); // Uncomment if needed
        },
        () => {
            intervalTime = config.defaultInterval;
            startSetInterval(intervalTime);
            // $('.canvasCover').css('overflow-y', 'scroll'); // Uncomment if needed
        }
    );

    // Start the initial interval
    startSetInterval(intervalTime);
}

// Configuration object for reusability
const tickerConfig = {
    defaultInterval: 50,
    hoverInterval: 50000,
    newstapeSettings: {
        period: 50,
        offset: 1,
        mousewheel: true,
        mousewheelRate: 30,
        dragable: false
    }
};

// Function to initialize email management functionality SmartAdmin
function initializeEmailManager() {
    // Push settings with "false" save to local
    //initApp.pushSettings("nav-function-minify layout-composed", false);
    initApp.pushSettings("layout-composed", false); // loại bỏ thu tự động
    // Store original title
    var title = document.title;

    // Update unread email count and document title
    var newEmailDisplayTab = function () {
        var count = $('.email-list .unread').length;
        var newTitle = title + ' (' + count + ')';
        document.title = newTitle;
        $(".js-unread-emails-count").text(' (' + count + ')');
    };

    // Delete email with animation and AJAX call
    var deleteEmail = function ($elements) {
        if (!$elements.length) {
            alert("Please select at least one email to delete.");
            return;
        }

        // Extract threadIDs from the checkbox IDs (e.g., "msg-{id}" -> extract {id})
        const threadIDs = $elements
            .find('.custom-control-input:checked')
            .map((index, element) => $(element).attr('id').replace('msg-', ''))
            .get()
            .map(id => parseInt(id)); // Convert to integer

        // Determine the context (HopThuDen or HopThuDi)
        const context = $('#mail-context').val(); // Get value from hidden input
        let url = '';   
       if (context === 'HopThuDi') {
            url = '/TransportFiles/TransportFiles/DeleteJqueryHopThuDi';
        } else {           
            url = '/TransportFiles/TransportFiles/DeleteJqueryHopThuDen'; // Default to HopThuDen
        }
        // Send AJAX request to delete emails
        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(threadIDs),
            success: function (response) {
                if (response.success) {
                    displayAlert(response.message);
                } else {
                    console.error("Error processing emails: " + response.message);
                    location.reload(); // Reload on error to restore state
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX error: " + error);
                location.reload(); // Reload on error to restore state
            }
        });
        // Animate removal of selected email items
        $elements.animate(
            {
                height: 'toggle',
                opacity: 'toggle'
            },
            200,
            'easeOutExpo',
            function () {
                $(this).remove();
                newEmailDisplayTab();
            }
        );
      

        // Remove any tooltips to avoid Bootstrap bug
        $('.tooltip').tooltip('dispose');

        // Uncheck master select all if checked
        if ($("#js-msg-select-all").is(":checked")) {
            $("#js-msg-select-all").prop('checked', false).prop('indeterminate', false);
        }
        return this;
    };

    // Select all checkbox functionality with event delegation
    $(document).on("change", "#js-msg-select-all", function (e) {
        const isChecked = this.checked;
        $('.email-list .custom-control-input')
            .prop("checked", isChecked)
            .closest("li")
            .toggleClass("state-selected", isChecked);
        $(this).prop('indeterminate', false); // Reset indeterminate state
    });

    // Individual checkbox change handling with event delegation
    $(document).on("change", '.email-list .custom-control-input', function () {
        const $checkboxes = $('.email-list .custom-control-input');
        const checkedCount = $checkboxes.filter(':checked').length;
        const totalCount = $checkboxes.length;

        if (checkedCount === 0) {
            $("#js-msg-select-all").prop('checked', false).prop('indeterminate', false);
        } else if (checkedCount === totalCount) {
            $("#js-msg-select-all").prop('checked', true).prop('indeterminate', false);
        } else {
            $("#js-msg-select-all").prop('checked', false).prop('indeterminate', true);
        }

        $(this).closest("li").toggleClass("state-selected", this.checked);
    });

    // Delete button triggers with event delegation
    $(document).on('click', '.js-delete-email', function () {
        deleteEmail($(this).closest("li"));
    });

    $("#js-delete-selected").on('click', function () {
        deleteEmail($('.email-list .custom-control-input:checked').closest("li"));
    });

    // Show unread email count (once)
    newEmailDisplayTab();
}
//Modal and select2 SmartAdmin 
function initializeModalAndAlert() {
    // Constants
    const MODAL_SELECTOR = '#myModal';
    const ALERT_CONTAINER = '#alert-container';
    const ALERT_TIMEOUT = 5000;
    const SELECT2_CONFIGS = {
        placeholderMultiple: { placeholder: 'Select State' },
        hideSearch: { minimumResultsForSearch: Infinity },
        maxLength: { maximumSelectionLength: 2, placeholder: 'Select maximum 2 items' },
        placeholder: { placeholder: 'Select a state', allowClear: true },
        icons: {
            minimumResultsForSearch: Infinity,
            templateResult: formatIcon,
            templateSelection: formatIcon,
            escapeMarkup: elm => elm
        },
        ajax: {
            ajax: {
                url: 'https://api.github.com/search/repositories',
                dataType: 'json',
                delay: 250,
                data: params => ({
                    q: params.term,
                    page: params.page
                }),
                processResults: (data, params) => {
                    params.page = params.page || 1;
                    return {
                        results: data.items,
                        pagination: { more: (params.page * 30) < data.total_count }
                    };
                },
                cache: true
            },
            placeholder: 'Search for a repository',
            escapeMarkup: markup => markup,
            minimumInputLength: 1,
            templateResult: formatRepo,
            templateSelection: formatRepoSelection
        }
    };

    // Format icon for select2
    function formatIcon(elm) {
        if (!elm.id) return elm.text;
        return `<i class='${$(elm.element).data('icon')} mr-2'></i>${elm.text}`;
    }

    // Format repository for select2
    function formatRepo(repo) {
        if (repo.loading) return repo.text;
        return `
            <div class='select2-result-repository clearfix d-flex'>
                <div class='select2-result-repository__avatar mr-2'>
                    <img src='${repo.owner.avatar_url}' class='width-2 height-2 mt-1 rounded' />
                </div>
                <div class='select2-result-repository__meta'>
                    <div class='select2-result-repository__title fs-lg fw-500'>${repo.full_name}</div>
                    ${repo.description ? `<div class='select2-result-repository__description fs-xs opacity-80 mb-1'>${repo.description}</div>` : ''}
                    <div class='select2-result-repository__statistics d-flex fs-sm'>
                        <div class='select2-result-repository__forks mr-2'><i class='fal fa-lightbulb'></i> ${repo.forks_count} Forks</div>
                        <div class='select2-result-repository__stargazers mr-2'><i class='fal fa-star'></i> ${repo.stargazers_count} Stars</div>
                        <div class='select2-result-repository__watchers mr-2'><i class='fal fa-eye'></i> ${repo.watchers_count} Watchers</div>
                    </div>
                </div>
            </div>`;
    }

    // Format repository selection
    function formatRepoSelection(repo) {
        return repo.full_name || repo.text;
    }

    // Hàm mã hóa HTML để tránh XSS
    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
    // Initialize modal
    function initModal(element, event) {
        event.preventDefault();
        const $this = $(element);
        const $modal = $(MODAL_SELECTOR);
        // Truyền ViewBag.Title vào JavaScript       
        try {
            const width = $this.attr('dialogwidth');
            const text = $this.text();
            const href = $this.attr('href');
            const modalTitleText = $('.card-header').data('title') || ''; // Lấy ViewBag.Title từ data-title
            $modal.find('.modal-dialog, .modal-content').width(`${width}px`);
            $modal.find('.modal-title').html(`
                        <h2 class="modal-title h2">
                            ${escapeHtml(text)} ${escapeHtml($('.card-header h4').text())} <span class="fw-300"><i>${escapeHtml(modalTitleText)}</i></span>
                        </h2>
                    `);
            $modal.find('.modal-body').empty().load(href, () => {
                $.getScript('../../Content/smartadmin/js/formplugins/select2/select2.bundle.js')
                    .done(() => initializeSelect2())
                    .fail(() => console.error('Failed to load app.js'));
                $.getScript('../../Scripts/CascadingDropdownAjax.js');

            });

            $modal.modal({ backdrop: 'static', keyboard: true });
        } catch (error) {
            console.error('Modal initialization failed:', error);
        }
    }

    // Initialize Select2 components 
    function initializeSelect2() {
        try {
            $('.select2').select2();
            $('.select2-placeholder-multiple').select2(SELECT2_CONFIGS.placeholderMultiple);
            $('.js-hide-search').select2(SELECT2_CONFIGS.hideSearch);
            $('.js-max-length').select2(SELECT2_CONFIGS.maxLength);
            $('.select2-placeholder').select2(SELECT2_CONFIGS.placeholder);
            $('.js-select2-icons').select2(SELECT2_CONFIGS.icons);
            $('.js-data-example-ajax').select2(SELECT2_CONFIGS.ajax);
        } catch (error) {
            console.error('Select2 initialization failed:', error);
        }
    }

    // Show alert
    function showAlert() {
        const message = window.alertMessage || $(ALERT_CONTAINER).data('message');
        if (!message) return;

        const isSuccess = message.includes('thành công');
        const alertClass = isSuccess ? 'alert-success' : 'alert-danger';
        const alertTitle = isSuccess ? 'Thành công!' : 'Oh snap!';

        const alertHtml = `
            <div class="alert ${alertClass} alert-dismissible custom-alert alert-show" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true"><i class="fal fa-trash-alt"></i></span>
                </button>
                <strong>${alertTitle}</strong> ${message}
            </div>`;

        const $container = $(ALERT_CONTAINER).html(alertHtml);

        // Auto-hide alert
        setTimeout(() => {
            const $alert = $container.find('.alert');
            if ($alert.length) {
                $alert.removeClass('alert-show')
                    .addClass('alert-hide')
                    .fadeOut(300, () => $alert.remove());
            }
        }, ALERT_TIMEOUT);

        // Manual close handler
        $container.find('.close').on('click', () => {
            const $alert = $container.find('.alert');
            $alert.removeClass('alert-show')
                .addClass('alert-hide')
                .fadeOut(300, () => $alert.remove());
        });
    }

    // Event bindings
    $('.bcommand').on('click', event => initModal(event.currentTarget, event));

    // Initialize alert
    showAlert();
}


// Hàm khởi tạo DataTables
function initializeDataTables() {
    // Kiểm tra xem phần tử .dt-basic-example có tồn tại không
    if ($('.dt-basic-example').length) {
        $('.dt-basic-example').dataTable({
            responsive: true,
            dom: "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            buttons: [
                {
                    extend: 'csvHtml5',
                    text: 'CSV',
                    titleAttr: 'Generate CSV',
                    className: 'btn-outline-default'
                },
                {
                    extend: 'copyHtml5',
                    text: 'Copy',
                    titleAttr: 'Copy to clipboard',
                    className: 'btn-outline-default'
                },
                {
                    extend: 'print',
                    text: '<i class="fal fa-print"></i>',
                    titleAttr: 'Print Table',
                    className: 'btn-outline-default'
                }
            ],
            responsive: true,    // Bảng co giãn theo kích thước màn hình
            fixedHeader: true,   // Tiêu đề cố định khi cuộn
            colReorder: true     // Cho phép kéo thả cột
        });
    } 
}

jQuery(function ($) {
    // Start initial interval
    initializeModalAndAlert();
    initializeDataTables();
    //initializeEmailManager();
    //initializeNewsTicker(tickerConfig);
    initializeSummernote('.Note');
    const message = window.alertMessage || $("#alert-container").data("message");
    if (message) displayAlert(message);

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
            extraPlugins: 'uploadimage', // Kích hoạt plugin upload hình ảnh
            height: 300,
            // Cấu hình CKFinder
            ////baseHref = '../../../Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
            //filebrowserBrowseUrl: '../../../Content/ckeditor/ckfinder/ckfinder.html',
            //filebrowserImageBrowseUrl: '../../../Content/ckeditor/ckfinder/ckfinder.html?Types=Images',
            //filebrowserUploadUrl: '../../../Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=File',
            //filebrowserImageUploadUrl: '../../../Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
            //uploadUrl: '../../../Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files&responseType=string', // Dùng cho kéo thả
            //customConfig: "../../../Content/ckeditor/config.js"
        });
    });
});
