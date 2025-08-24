
var colors = ["rgba(66,58,255,0.8)", "rgba(100,185,125,0.8)"];
var colors2 = [];
const today = new Date();
const yesterday = new Date(today.setDate(today.getDate() - 1));
// Khai báo today và yesterday
const todayStr = formatDate(today);//new Date().toLocaleDateString('vi-VN'); // Ví dụ: "24/03/2025"
const yesterdayStr = formatDate(yesterday);//new Date(Date.now() - 86400000).toLocaleDateString('vi-VN'); // Ví dụ: "23/03/2025"
function startSetInterval(intervalTime) {
    autoScrollInterval = setInterval(function () {
        if ($('.canvasCover').length > 0 && $('.canvasCover')[0].clientHeight < $('.canvasCover')[0].scrollHeight) {
            var pos = $('.canvasCover').scrollTop();
            $('.canvasCover').scrollTop(pos + 1);
            if ($('.canvasCover')[0].offsetHeight + $('.canvasCover').scrollTop() + 1 >= $('.canvasCover')[0].scrollHeight) {
                $('.canvasCover').scrollTop(0);
            }
        }
    }, intervalTime)
};
// Hàm tính phần trăm thay đổi và trả về thông tin hiển thị
function calculateChange(current, last) {
    const diff = current - last;
    const denominator = last === 0 ? 1 : last;
    const percent = Math.ceil((diff / denominator) * 100);
    const color = diff >= 0 ? 'green' : 'red';
    const sortIcon = diff >= 0 ? 'fa-sort-asc' : 'fa-sort-desc';
    return { percent, color, sortIcon };
}
// Hàm định dạng số tiền thành tiền tệ VND
function formatCurrency(value) {
    return (value ?? 0).toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
}
// Hàm tạo màu ngẫu nhiên (nếu cần)
function randomColorFactor() {
    return Math.round(Math.random() * 255);
}

// Hàm tạo màu ngẫu nhiên (nếu cần)
function randomColorFactor1() {
    return Math.round(Math.random() * 255);
}
// Hàm định dạng ngày thành chuỗi DD/MM/YYYY
function formatDate(date) {
    if (!date || !(date instanceof Date)) return '';
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}
function loadref(time) {
    setTimeout("location.reload(true);", time);
}



//----------CHART Option--------------
var optionsBarChart = {
    scales: {
        xAxes: [{
            ticks: {
                min: 0 // Edit the value according to what you need
            },
            //stacked: true
        }],
        yAxes: [{
            ticks: {
                min: 0
            }
            //stacked: true
        }]
    },
    responsive: true,
    //maintainAspectRatio: true,
    legend: {
        display: false,
    },
    title: {
        display: false,
        text: 'Chart.js Combo Bar Line Chart'
    },
    animation: {
        onComplete: function () {
            var chartInstance = this.chart;
            var ctx = chartInstance.ctx;
            ctx.textAlign = "center";

            Chart.helpers.each(this.data.datasets.forEach(function (dataset, i) {
                var meta = chartInstance.controller.getDatasetMeta(i);
                Chart.helpers.each(meta.data.forEach(function (bar, index) {
                    ctx.fillText(dataset.data[index], bar._model.x + 10, bar._model.y + 5);
                }), this)
            }), this);
        }
    }
};
var dataChartCLS01 = {
    labels: [
        "Red",
        "Blue",
        "Yellow"
    ],
    datasets: [
        {
            data: [300, 50, 100],
            backgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56"
            ],
            hoverBackgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56"
            ]
        }]
};
var optionsChartCLS01 = {
    scales: {
        xAxes: [{
            ticks: {
                min: 0 // Edit the value according to what you need
            },
            //stacked: true
        }],
        yAxes: [{
            ticks: {
                min: 0
            }
            //stacked: true
        }]
    },
    responsive: false,
    //maintainAspectRatio: true,
    legend: {
        display: true,
    },
    title: {
        display: false,
        text: 'Chart.js Combo Bar Line Chart'
    },
    animation: {
        onComplete: function () {
            var chartInstance = this.chart;
            var ctx = chartInstance.ctx;
            ctx.textAlign = "center";

            Chart.helpers.each(this.data.datasets.forEach(function (dataset, i) {
                var meta = chartInstance.controller.getDatasetMeta(i);
                Chart.helpers.each(meta.data.forEach(function (bar, index) {
                    ctx.fillText(dataset.data[index], bar._model.x + 10, bar._model.y - 7);
                }), this)
            }), this);
        }
    }
};
var optionsAverageWaitingTime = {

    scales: {
        xAxes: [{
            ticks: {
                min: 0 // Edit the value according to what you need
            },
            stacked: false
        }],
        yAxes: [{
            ticks: {
                min: 0
            },
            stacked: false
        }]
    },
    responsive: true,
    //maintainAspectRatio: true,
    legend: {
        display: true,
    },
    title: {
        display: false,
        text: 'Chart.js Combo Bar Line Chart'
    },
    animation: {
        onComplete: function () {
            var chartInstance = this.chart;
            var ctx = chartInstance.ctx;
            ctx.textAlign = "center";

            Chart.helpers.each(this.data.datasets.forEach(function (dataset, i) {
                var meta = chartInstance.controller.getDatasetMeta(i);
                Chart.helpers.each(meta.data.forEach(function (bar, index) {
                    ctx.fillText(dataset.data[index], bar._model.x + 10, bar._model.y - 7);
                }), this)
            }), this);
        }
    }
};
//----------CHART Option--------------


//----------DRAW--------------
// Hàm vẽ biểu đồ hình tròn
function drawPieChart(canvasId, counts, labels) {
    // Kiểm tra xem Chart.js đã được tải chưa
    if (typeof Chart === 'undefined') {
        console.error('Chart.js is not loaded. Please include the library.');
        return;
    }

    const config = {
        type: 'pie',
        data: {
            datasets: [{
                data: counts,
                backgroundColor: [
                    '#4e73df', // primary-200
                    '#1cc88a', // success-300
                    '#36b9cc', // info-200
                    '#f6c23e', // warning-400
                    '#e74a3b'  // danger-200
                ],
                label: 'Thống kê số lượng công trình'
            }],
            labels: labels
        },
        options: {
            responsive: true,
            legend: {
                display: true,
                position: 'bottom'
            }
        }
    };

    new Chart($("#pieChart > canvas").get(0).getContext("2d"), config);
}
async function drawChartNhanSu() {
    const $chartNhanSu = $('#nhanSuChart');
    $chartNhanSu.empty(); // Xóa nội dung cũ
    try {
        const response = await fetch(`/TransportFiles/Dashboard/HoatDongNhanSu`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }
        // Xây dựng HTML bằng mảng để tối ưu hiệu suất
        const htmlArray = [];
        data.forEach((item, index) => {
            // Calculate percentages for each category, with checks for division by zero
            const totalPercent = item.Counts > 0 ? Math.ceil((item.LastCounts / item.Counts) * 100) : 0;
            const congTacPercent = item.Counts > 0 ? Math.ceil((item.str4 / item.Counts) * 100) : 0;
            const nghiPhepPercent = item.Counts > 0 ? Math.ceil((item.str3 / item.Counts) * 100) : 0;
            const diHocPercent = item.Counts > 0 ? Math.ceil((item.str5 / item.Counts) * 100) : 0; // Percentage for Đi học
            const lyDoKhacPercent = item.Counts > 0 ? Math.ceil((item.str6 / item.Counts) * 100) : 0; // Percentage for Lý do khác

            htmlArray.push(`
        <div class="col-lg-11 col-xl-11 pl-lg-12">
            <div class="d-flex mt-2">
                Tổng nhân sự
                <span class="d-inline-block ml-auto">${item.LastCounts} / ${item.Counts}</span>
            </div>
            <div class="progress progress-sm mb-3">
                <div class="progress-bar bg-fusion-400" role="progressbar" style="width: ${totalPercent}%;" aria-valuenow="${item.LastCounts}" aria-valuemin="0" aria-valuemax="${item.Counts}"></div>
            </div>
            <div class="d-flex">
                Công tác
                <span class="d-inline-block ml-auto">${item.str4} / ${item.Counts}</span>
            </div>
            <div class="progress progress-sm mb-3">
                <div class="progress-bar bg-success-500" role="progressbar" style="width: ${congTacPercent}%;" aria-valuenow="${item.str4}" aria-valuemin="0" aria-valuemax="${item.Counts}"></div>
            </div>
            <div class="d-flex">
                Nghỉ phép
                <span class="d-inline-block ml-auto">${item.str3} / ${item.Counts}</span>
            </div>
            <div class="progress progress-sm mb-3">
                <div class="progress-bar bg-info-400" role="progressbar" style="width: ${nghiPhepPercent}%;" aria-valuenow="${item.str3}" aria-valuemin="0" aria-valuemax="${item.Counts}"></div>
            </div>
            <div class="d-flex">
                Đi học
                <span class="d-inline-block ml-auto">${item.str5} / ${item.Counts}</span>
            </div>
            <div class="progress progress-sm mb-3">
                <div class="progress-bar bg-primary-300" role="progressbar" style="width: ${diHocPercent}%;" aria-valuenow="${item.str5}" aria-valuemin="0" aria-valuemax="${item.Counts}"></div>
            </div>
            <div class="d-flex">
                Lý do khác
                <span class="d-inline-block ml-auto">${item.str6} / ${item.Counts}</span>
            </div>
            <div class="progress progress-sm mb-g">
                <div class="progress-bar bg-gradient-300" role="progressbar" style="width: ${lyDoKhacPercent}%;" aria-valuenow="${item.str6}" aria-valuemin="0" aria-valuemax="${item.Counts}"></div>
            </div>
        </div>
    `);
        });
        // Append HTML một lần duy nhất
        $chartNhanSu.html(htmlArray.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc tạo biểu đồ:', error);
        $chartNhanSu.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}
// Hàm chính để lấy và hiển thị thông tin bệnh nhân
async function drawChartTinhHinhBenhNhan(today, yesterday) {
    const $topInformation = $('#topInformation');
    $topInformation.empty(); // Xóa nội dung cũ

    // Mảng màu nền và biểu tượng cho các thẻ
    const backgroundColors = ['bg-primary-300', 'bg-warning-400', 'bg-success-200', 'bg-info-200', 'bg-danger-200', 'bg-fusion-200'];
    const icons = ['mb-n1 fa-user mr-n1', 'mb-n1 fa-gem mr-n4', 'mb-n5 fa-lightbulb mr-n6', 'mb-n1 fa-globe mr-n4'];

    // Mảng để lưu dữ liệu cho biểu đồ
    const Arrcounts = [];
    const Arrlabels = [];
    // Initialize formatted values
    let formattedCounts = null;
    let formattedLastCounts = null;
    try {
        // Lấy dữ liệu từ API     
        const response = await fetch(`/TransportFiles/Dashboard/GetTopInformation?_Ngay=${today}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xây dựng HTML bằng mảng để tối ưu hiệu suất
        const htmlArray = [];
        data.forEach((item, index) => {
            // Tính toán phần trăm thay đổi
            const diff = item.Counts - item.LastCounts;
            const counts = item.Counts != null && !isNaN(item.Counts) ? Number(item.Counts) : 0;
            const lastCounts = item.LastCounts != null && !isNaN(item.LastCounts) ? Number(item.LastCounts) : 0;  ;
            const percent = Math.ceil((diff / lastCounts) * 100);
            const color = diff >= 0 ? 'purple' : 'red';
            const sortIcon = diff >= 0 ? 'fa-sort-asc' : 'fa-sort-desc';
            formattedCounts = counts;
            formattedLastCounts = lastCounts;
            if (item.str1 == 'Tien') {
                formattedCounts = counts.toLocaleString('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0
                });
                formattedLastCounts = lastCounts.toLocaleString('vi-VN', {
                    style: 'currency',
                    currency: 'VND',
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 0
                });
            }

            // Xây dựng HTML cho từng thẻ
            htmlArray.push(`
                <div class="col-lg-2 col-sm-2 col-xs-12">
                    <div class="p-3 ${backgroundColors[index] || 'bg-primary-300'} rounded overflow-hidden position-relative text-white mb-g">
                        <div>
                            <h2 class="display-3 d-block l-h-n m-0 fw-500" style="font-size:240%">${item.Titles}</h2>
                            <h2 class="m-0 l-h-n fw-500" style="color:blue;font-size:210%">${formattedCounts}</h2>
                        </div>
                        <span class="count_bottom">        
                            <h4 style="color:black;font-size:15px">
                            <i class="${color}" style="font-size: 24px">
                                <i class="fa ${sortIcon}"></i> ${percent}%
                            </i>So với năm ${item.LastNam} (${formattedLastCounts})</h4>
                        </span>
                        <i class="fal position-absolute pos-right pos-bottom opacity-15 ${icons[index] || 'mb-n1 fa-user mr-n1'}" style="font-size:6rem"></i>
                    </div>
                </div>
            `);

            // Lưu dữ liệu cho biểu đồ
            Arrcounts.push(formattedCounts);
            Arrlabels.push(item.Titles);
        });

        // Append HTML một lần duy nhất
        $topInformation.html(htmlArray.join(''));

        // Vẽ biểu đồ hình tròn  
        drawPieChart('pieChart', Arrcounts, Arrlabels);
   

    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc vẽ biểu đồ:', error);
        $topInformation.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}

async function drawChartGetSoLoiNhuan(today, yesterday) {
    const $getSoLoiNhuan = $('#GetSoLoiNhuan');
    $getSoLoiNhuan.empty(); // Xóa nội dung cũ

    // Mảng màu nền và biểu tượng cho các thẻ
    const backgroundColors = ['bg-primary-300', 'bg-warning-400', 'bg-success-200', 'bg-info-200', 'bg-danger-200', 'bg-fusion-200'];
    const icons = ['mb-n1 fa-user mr-n1', 'mb-n1 fa-gem mr-n4', 'mb-n5 fa-lightbulb mr-n6', 'mb-n1 fa-globe mr-n4'];

    // Mảng để lưu dữ liệu cho biểu đồ
    const counts = [];
    const labels = [];

    try {
        // Lấy dữ liệu từ API     
        const response = await fetch(`/TransportFiles/Dashboard/GetSoLoiNhuan?_Ngay=${today}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xây dựng HTML bằng mảng để tối ưu hiệu suất
        const htmlArray = [];
        data.forEach((item, index) => {
            // Tính toán phần trăm thay đổi
            const diff = item.Counts - item.LastCounts;
            const lastCount = item.LastCounts === 0 ? 1 : item.LastCounts;
            const percent = Math.ceil((diff / lastCount) * 100);
            const color = diff >= 0 ? 'purple' : 'red';
            const sortIcon = diff >= 0 ? 'fa-sort-asc' : 'fa-sort-desc';

            // Xây dựng HTML cho từng thẻ
            htmlArray.push(`
                <div class="col-lg-2 col-sm-2 col-xs-12">
                    <div class="p-3 ${backgroundColors[index] || 'bg-primary-300'} rounded overflow-hidden position-relative text-white mb-g">
                        <div>
                            <h2 class="display-3 d-block l-h-n m-0 fw-500" style="font-size:240%">${item.Titles}</h2>
                            <h2 class="m-0 l-h-n fw-500" style="color:blue;font-size:210%">${item.Counts}</h2>
                        </div>
                        <span class="count_bottom">        
                            <h4 style="color:black;font-size:15px">
                            <i class="${color}" style="font-size: 24px">
                                <i class="fa ${sortIcon}"></i> ${percent}%
                            </i>So với năm ${item.LastNam} (${item.LastCounts})</h4>
                        </span>
                        <i class="fal position-absolute pos-right pos-bottom opacity-15 ${icons[index] || 'mb-n1 fa-user mr-n1'}" style="font-size:6rem"></i>
                    </div>
                </div>
            `);

            // Lưu dữ liệu cho biểu đồ
            counts.push(item.Counts);
            labels.push(item.Titles);
        });

        // Append HTML một lần duy nhất
        $getSoLoiNhuan.html(htmlArray.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc vẽ biểu đồ:', error);
        $getSoLoiNhuan.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}
// Hàm vẽ biểu đồ thanh ngang
function drawBarChartKhamBenh(canvasId, labels, bhytData, vpData) {
    // Kiểm tra xem Chart.js đã được tải chưa
    if (typeof Chart === 'undefined') {
        console.error('Chart.js is not loaded. Please include the library.');
        return;
    }
    const ctx = document.getElementById(canvasId).getContext('2d');
    // Xóa biểu đồ cũ nếu tồn tại
    //if (window.myBar && typeof window.myBar.destroy === 'function') {
    //    window.myBar.destroy();
    //}
    // Tạo biểu đồ mới
    window.myBar = new Chart(ctx, {
        type: 'horizontalBar', // Sử dụng 'bar' và xoay ngang bằng options
        data: {
            labels: labels, // Tên các phòng ban
            datasets: [
                {
                    label: 'BHYT',
                    data: bhytData,
                    backgroundColor: 'rgba(255, 0, 0, 0.8)', // Màu đỏ cho BHYT
                    borderWidth: 1
                },
                {
                    label: 'Viện phí',
                    data: vpData,
                    backgroundColor: 'rgba(151, 187, 205, 0.8)', // Màu xanh nhạt cho VP
                    borderWidth: 1
                }
            ]
        },
        options: optionsAverageWaitingTime
    });  
}

// Hàm chính để lấy và hiển thị số lượng khám bệnh
async function drawChartKhamBenh() {
    const totalElement = document.getElementById('tongcong');
    if (!totalElement) {
        console.error('Element with ID "tongcong" not found.');
        return;
    }

    // Khởi tạo các mảng dữ liệu
    const departments = [];
    const vpCounts = [];
    const bhytCounts = [];
    let totalCount = 0;

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetSoLuongKhamBenh', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xử lý dữ liệu
        data.forEach(item => {
            departments.push(item.Titles);
            const vpCount = parseInt(item.Counts ?? 0);
            const bhytCount = parseInt(item.LastCounts ?? 0);
            vpCounts.push(vpCount);
            bhytCounts.push(bhytCount);
            totalCount += vpCount + bhytCount;
        });

        // Cập nhật tổng số
        totalElement.textContent = totalCount;

        // Vẽ biểu đồ
        drawBarChartKhamBenh('canvasSoLuongKhamBenh', departments, bhytCounts, vpCounts);
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc vẽ biểu đồ:', error);
        totalElement.textContent = 'Lỗi';
        totalElement.classList.add('text-danger');
    }
}

// Hàm vẽ biểu đồ thanh ngang
//function drawBarChartKhamBenhBHYT(canvasId, labels, data, colors) {
//    // Kiểm tra xem Chart.js đã được tải chưa
//    if (typeof Chart === 'undefined') {
//        console.error('Chart.js is not loaded. Please include the library.');
//        return;
//    }

//    const ctx = document.getElementById(canvasId).getContext('2d');

//    // Xóa biểu đồ cũ nếu tồn tại
//    if (window.myBar && typeof window.myBar.destroy === 'function') {
//        window.myBar.destroy();
//    }

//    // Tạo biểu đồ mới
//    window.myBar = new Chart(ctx, {
//        type: 'bar', // Sử dụng 'bar' và xoay ngang bằng options
//        data: {
//            labels: labels, // Tên các phòng ban
//            datasets: [
//                {
//                    label: 'Số lượng khám BHYT',
//                    data: data,
//                    backgroundColor: colors, // Màu ngẫu nhiên cho từng thanh
//                    borderWidth: 1
//                }
//            ]
//        },
//        options: optionsAverageWaitingTime
//    });
//}

// Hàm chính để lấy và hiển thị số lượng khám bệnh BHYT
//async function drawChartKhamBenhBhyt() {
//    const totalElement = document.getElementById('tongcongBHYT');
//    if (!totalElement) {
//        console.error('Element with ID "tongcongBHYT" not found.');
//        return;
//    }

//    // Khởi tạo các mảng dữ liệu
//    const departments = [];
//    const bhytCounts = [];
//    const colors = [];
//    let totalBhytCount = 0;

//    try {
//        // Lấy dữ liệu từ API
//        const response = await fetch('/Home/GetSoLuongKhamBenhBHYT', {
//            method: 'GET',
//            headers: {
//                'Accept': 'application/json'
//            }
//        });

//        if (!response.ok) {
//            throw new Error(`HTTP error! Status: ${response.status}`);
//        }

//        const data = await response.json();
//        if (!Array.isArray(data)) {
//            throw new Error('Dữ liệu trả về không phải là mảng');
//        }

//        // Xử lý dữ liệu
//        data.forEach(item => {
//            departments.push(item.Titles);
//            const count = parseInt(item.Counts ?? 0);
//            bhytCounts.push(count);
//            totalBhytCount += count;
//            // Tạo màu ngẫu nhiên cho từng thanh
//            colors.push(`rgba(${randomColorFactor()}, ${randomColorFactor()}, ${randomColorFactor()}, 0.7)`);
//        });

//        // Cập nhật tổng số
//        totalElement.textContent = totalBhytCount;

//        // Vẽ biểu đồ
//        drawBarChartKhamBenhBHYT('canvasSoLuongKhamBenhBHYT', departments, bhytCounts, colors);
//    } catch (error) {
//        console.error('Lỗi khi lấy dữ liệu hoặc vẽ biểu đồ:', error);
//        totalElement.textContent = 'Lỗi';
//        totalElement.classList.add('text-danger');
//    }
//}
//function drawChartKhamBenhDV() {
//    var phongBans = [];
//    var soLuongsDV = [];
  

//    var d_tongcongDV = 0;
//    $.ajax({
//        /* url: '/Home/GetSoLuongKhamBenhVP?_Ngay=' + strdate,*/
//        url: '/Home/GetSoLuongKhamBenhVP',
//        method: 'GET',
//        dataType: 'json',
//        success: function (result) {
//            // Khai báo biến tick và gán cho nó giá trị trả về của phương thức map         
//            var info = result;
//            for (var i in info) {
//                phongBans.push(info[i].Titles);
//                soLuongsDV.push(info[i].Counts == null ? "0" : info[i].Counts);
//                colors.push('rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',.7)');
//                colors2.push('rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',.7)');
//                d_tongcongDV += parseInt(info[i].Counts == null ? "0" : info[i].Counts);
//            }
//            document.getElementById("tongcongDV").innerHTML = d_tongcongDV;
//            const ctxDV = document.getElementById("canvasSoLuongKhamBenhDV");
//            //canvasHeight = info.length * 30;
//            //if (info.length > 10) {
//            //    ctxDV.height = canvasHeight;
//            //} else {
//            //    ctxDV.height = 200;
//            //}
//            window.myBar = new Chart(ctxDV, {
//                type: 'horizontalBar',
//                data: {
//                    labels: phongBans, //["January", "February", "March", "April", "May", "June", "July"],
//                    datasets: [
//                        {
//                            label: "Screening",
//                            backgroundColor: colors,
//                            data: soLuongsDV//[65, 59, 80, 81, 56, 55, 40]
//                        }
//                    ]
//                },
//                options: optionsBarChart
//            });
//        },
//        error: function (xhr, status, error) {
//            // Xử lý lỗi nếu có
//            console.log(error);
//        }
//    });
//}

// Hàm chính để lấy và hiển thị bảng nhập/xuất khoa
async function drawTableNhapXuatKhoa(yesterday) {
    const $topNhapXuatVien = $('#topNhapXuatVien');

    // Kiểm tra phần tử tồn tại
    if (!$topNhapXuatVien.length) {
        console.error('Element with ID "topNhapXuatVien" not found.');
        return;
    }

    $topNhapXuatVien.empty(); // Xóa nội dung cũ

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetNhapXuatKhoa', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xây dựng bảng HTML bằng mảng để tối ưu hiệu suất
        const tableRows = [];
        tableRows.push(`
            <div class="table-responsive">
                <table class="table table-bordered table-hover table-striped w-100" style="font-size: calc(0.3vw + 10px)">
                    <thead class="bg-warning-200">
                        <tr>
                            <th>Khoa</th>
                            <th>Nhập Khoa</th>
                            <th>Xuất khoa</th>
                        </tr>
                    </thead>
                    <tbody>
        `);

        // Duyệt qua dữ liệu để tạo các hàng
        data.forEach(item => {
            // Tính toán cho nhập khoa
            const nhapChange = calculateChange(item.Counts ?? 0, item.LastCounts ?? 0);
            // Tính toán cho xuất khoa
            const xuatChange = calculateChange(item.CountsXV ?? 0, item.LastCountsXV ?? 0);

            tableRows.push(`
                <tr>
                    <td rowspan="2" class="x_title" style="vertical-align:middle;word-wrap: break-word;font-size: 13px;font-weight: 400;text-transform: uppercase;">
                        ${item.Titles}
                    </td>
                    <td align="center" width="175" class="count">${item.Counts ?? 0}</td>
                    <td align="center" width="175" class="count">${item.CountsXV ?? 0}</td>
                </tr>
                <tr>
                    <td align="center" width="175" class="count_bottom">
                        <i class="${nhapChange.color}">
                            <i class="fa ${nhapChange.sortIcon}"></i> ${nhapChange.percent}%
                        </i> Ngày ${yesterday} (${item.LastCounts ?? 0})
                    </td>
                    <td align="center" width="175" class="count_bottom">
                        <i class="${xuatChange.color}">
                            <i class="fa ${xuatChange.sortIcon}"></i> ${xuatChange.percent}%
                        </i> Ngày ${yesterday} (${item.LastCountsXV ?? 0})
                    </td>
                </tr>
            `);
        });

        tableRows.push(`
                    </tbody>
                </table>
            </div>
        `);

        // Append HTML một lần duy nhất
        $topNhapXuatVien.html(tableRows.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc tạo bảng:', error);
        $topNhapXuatVien.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}
// Hàm chính để lấy và hiển thị danh sách chỉ tiêu
async function drawChartDanhSachChiTieu() {
    const $listChiTieu = $('#lstChiTieu');

    // Kiểm tra phần tử tồn tại
    if (!$listChiTieu.length) {
        console.error('Element with ID "lstChiTieu" not found.');
        return;
    }

    $listChiTieu.empty(); // Xóa nội dung cũ

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetlstChiTieu', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Nhóm dữ liệu theo khoa
        const groupedData = data.reduce((acc, item) => {
            const khoa = item.KHOA || 'Không xác định';
            if (!acc[khoa]) {
                acc[khoa] = [];
            }
            acc[khoa].push(item);
            return acc;
        }, {});

        // Xây dựng HTML bằng mảng để tối ưu hiệu suất
        const htmlArray = [];

        // Duyệt qua từng khoa
        for (const [khoa, items] of Object.entries(groupedData)) {
            htmlArray.push(`
                <h1><span style="color: red; font-weight: bold;">${khoa}</span></h1>
                <table class="table table-bordered table-hover table-striped w-100 dt-basic-example" style="font-size: 120%">
                    <thead>
                        <tr>
                            <th>Tên Dịch Vụ</th>
                            <th>Số BN BHYT</th>
                            <th>Số BN Viện Phí</th>
                            <th>Tổng / Chỉ Tiêu</th>
                        </tr>
                    </thead>
                    <tbody>
            `);

            // Duyệt qua từng chỉ tiêu trong khoa
            items.forEach(item => {
                const soBnBhyt = item.SOBN_BH ?? 0;
                const soBnVp = item.SOBN_VP ?? 0;
                const total = soBnBhyt + soBnVp;
                const chiTieuGiao = item.CHITIEU_GIAO ?? 0;

                const status = calculateStatus(total, chiTieuGiao);

                htmlArray.push(`
                    <tr>
                        <td width="600px">${item.TENDICHVU || 'Không xác định'}</td>
                        <td width="100px">${soBnBhyt}</td>
                        <td width="100px">${soBnVp}</td>
                        <td>
                            ${total} / ${chiTieuGiao}
                            <span class="count_bottom">
                                <i class="${status.color}" style="font-size: 24px">
                                    <i class="fa ${status.sortIcon}"></i> ${status.percent}%
                                </i>
                            </span>
                        </td>
                    </tr>
                `);
            });

            htmlArray.push(`
                    </tbody>
                </table>
            `);
        }

        // Append HTML một lần duy nhất
        $listChiTieu.html(htmlArray.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc tạo bảng:', error);
        $listChiTieu.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}
// Hàm chính để lấy và hiển thị bảng số tiền thu chi
async function drawGetSoTienThuChi(today, yesterday) {
    const $listSoTienThuChi = $('#GetSoTienThuChi');

    // Kiểm tra phần tử tồn tại
    if (!$listSoTienThuChi.length) {
        console.error('Element with ID "GetSoTienThuChi" not found.');
        return;
    }

    $listSoTienThuChi.empty(); // Xóa nội dung cũ

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetSoTienThuChi', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Tính toán rowspan cho từng loại
        const rowspanData = data.reduce((acc, item) => {
            const type = item.LOAI || 'Không xác định';
            acc[type] = (acc[type] || 0) + 1;
            return acc;
        }, {});

        // Xây dựng bảng HTML bằng mảng để tối ưu hiệu suất
        const tableRows = [];
        tableRows.push(`
            <div class="table-responsive">
                <table class="table table-bordered table-hover table-striped w-100" style="font-size: calc(0.3vw + 10px)">
                    <thead class="bg-warning-200">
                        <tr>
                            <th>Loại</th>
                            <th>Đối tượng</th>
                            <th>${today}</th> 
                            <th>${yesterday}</th>
                            <th>Chuyển khoản</th>
                            <th>Tiền mặt</th>
                        </tr>
                    </thead>
                    <tbody>
        `);

        // Duyệt qua dữ liệu để tạo các hàng
        let previousType = null;
        data.forEach(item => {
            const type = item.LOAI || 'Không xác định';
            const isNewType = type !== previousType;

            tableRows.push('<tr>');
            if (isNewType) {
                tableRows.push(`<td rowspan="${rowspanData[type]}">${type}</td>`);
                previousType = type;
            }

            tableRows.push(`
                <td>${item.DOITUONG || 'Không xác định'}</td>
                <td>${formatCurrency(item.todaySQL)}</td>
                <td>${formatCurrency(item.yesterdaySQL)}</td>
                <td>${formatCurrency(item.CK)}</td>
                <td>${formatCurrency(item.TM)}</td>
            `);
            tableRows.push('</tr>');
        });

        tableRows.push(`
                    </tbody>
                </table>
            </div>
        `);

        // Append HTML một lần duy nhất
        $listSoTienThuChi.html(tableRows.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc tạo bảng:', error);
        $listSoTienThuChi.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}

// Hàm chính để lấy và hiển thị số liệu Đề án 06
async function drawGetSoLieuDeAn06() {
    const $listSoLieuDeAn06 = $('#GetSoLieuDeAn06');

    // Kiểm tra phần tử tồn tại
    if (!$listSoLieuDeAn06.length) {
        console.error('Element with ID "GetSoLieuDeAn06" not found.');
        return;
    }

    $listSoLieuDeAn06.empty(); // Xóa nội dung cũ

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetSoLieuDeAn06', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xây dựng bảng HTML bằng mảng để tối ưu hiệu suất
        const tableRows = [];
        tableRows.push(`
            <div class="table-responsive">
                <table class="table table-bordered table-hover table-striped w-100" style="font-size: calc(0.3vw + 10px)">
                    <thead class="bg-warning-200 text-center">
                        <tr>
                            <th rowspan="1"></th>
                            <th colspan="5">Khám chữa bệnh bằng thẻ CCCD gắn chip và VNeID</th>
                            <th colspan="3">Thanh toán trực tuyến không dùng tiền mặt</th>
                            <th rowspan="1">Liên thông dữ liệu</th>
                        </tr>
                        <tr>
                            <th>TT</th>
                            <th>Số lượt KCB</th>
                            <th>Số lượt KCB BHYT</th>
                            <th>Số lượt KCB bằng thẻ CCCD</th>
                            <th>% lượt khám BHYT / Tổng số lượt khám</th>
                            <th>% lượt khám bằng thẻ CCCD / Tổng số lượt KCB BHYT</th>
                            <th>Số lượt thanh toán trực tuyến</th>
                            <th>Giá trị thanh toán trực tuyến</th>
                            <th>Tổng giá trị thanh toán</th>
                            <th>Số lượng giấy báo tử</th>
                        </tr>
                    </thead>
                    <tbody>
        `);

        // Duyệt qua dữ liệu để tạo các hàng
        data.forEach((item, index) => {
            tableRows.push(`
                <tr class="text-center">
                    <td>${index + 1}</td>
                    <td>${item.TONG_KCB ?? 0}</td>
                    <td>${item.TONGKCB_BHYT ?? 0}</td>
                    <td>${item.SOLUOT_KCB_CCCD ?? 0}</td>
                    <td>${item.TL_BHYT_TONGKHAM ?? 0}</td>
                    <td>${item.TL_CCCD_BHYT ?? 0}</td>
                    <td>${item.SL_TTTT ?? 0}</td>
                    <td>${formatCurrency(item.GT_TTTT)}</td>
                    <td>${formatCurrency(item.TONG_TT)}</td>
                    <td>${item.SL_GBT ?? 0}</td>
                </tr>
            `);
        });

        tableRows.push(`
                    </tbody>
                </table>
            </div>
        `);

        // Append HTML một lần duy nhất
        $listSoLieuDeAn06.html(tableRows.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc tạo bảng:', error);
        $listSoLieuDeAn06.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}

// Hàm vẽ biểu đồ thanh ngang
function drawBarChartTinhHinhCLS(canvasId, labels, todayData, yesterdayData, todayLabel, yesterdayLabel) {
    // Kiểm tra xem Chart.js đã được tải chưa
    if (typeof Chart === 'undefined') {
        console.error('Chart.js is not loaded. Please include the library.');
        return;
    }

    const ctx = document.getElementById(canvasId).getContext('2d');

    // Xóa biểu đồ cũ nếu tồn tại
    //if (window.myBar && typeof window.myBar.destroy === 'function') {
    //    window.myBar.destroy();
    //}

    // Tạo biểu đồ mới
    window.myBar = new Chart(ctx, {
        type: 'horizontalBar', // Sử dụng 'bar' và xoay ngang bằng options
        data: {
            labels: labels, // Tên các phòng ban
            datasets: [
                {
                    label: todayLabel,
                    data: todayData,
                    backgroundColor: 'rgba(255, 99, 132, 0.8)', // Màu đỏ cho today
                    borderWidth: 1
                },
                {
                    label: yesterdayLabel,
                    data: yesterdayData,
                    backgroundColor: 'rgba(75, 192, 192, 0.8)', // Màu xanh lam cho yesterday
                    borderWidth: 1
                }
            ]
        },
        options: optionsAverageWaitingTime
    });
}

// Hàm chính để lấy và hiển thị số liệu CLS
async function drawChartTinhHinhCLS(today, yesterday) {
    // Khởi tạo các mảng dữ liệu
    const departments = [];
    const counts = [];
    const lastCounts = [];

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetSoLuongCLS', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xử lý dữ liệu
        data.forEach(item => {
            departments.push(item.Titles || 'Không xác định');
            counts.push(item.Counts ?? 0);
            lastCounts.push(item.LastCounts ?? 0);
        });

        // Vẽ biểu đồ
        drawBarChartTinhHinhCLS('chartCLSTotal', departments, counts, lastCounts, today, yesterday);
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc vẽ biểu đồ:', error);
        const $chartContainer = document.getElementById('chartCLSTotal')?.parentElement;
        if ($chartContainer) {
            $chartContainer.innerHTML = '<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>';
        }
    }
}

// Hàm vẽ biểu đồ thanh ngang
function drawBarChartThoiGianChoKhamBenh(canvasId, labels, todayData, yesterdayData, todayLabel, yesterdayLabel) {
    // Kiểm tra xem Chart.js đã được tải chưa
    if (typeof Chart === 'undefined') {
        console.error('Chart.js is not loaded. Please include the library.');
        return;
    }

    const ctx = document.getElementById(canvasId).getContext('2d');

    // Xóa biểu đồ cũ nếu tồn tại
    //if (window.myBar && typeof window.myBar.destroy === 'function') {
    //    window.myBar.destroy();
    //}

    // Tạo biểu đồ mới
    window.myBar = new Chart(ctx, {
        type: 'horizontalBar', // Sử dụng 'bar' và xoay ngang bằng options
        data: {
            labels: labels, // Tên các phòng ban
            datasets: [
                {
                    label: todayLabel,
                    data: todayData,
                    backgroundColor: 'rgba(54, 162, 235, 0.8)', // Màu xanh dương cho today
                    borderWidth: 1
                },
                {
                    label: yesterdayLabel,
                    data: yesterdayData,
                    backgroundColor: 'rgba(153, 102, 255, 0.8)', // Màu tím cho yesterday
                    borderWidth: 1
                }
            ]
        },
        options: optionsAverageWaitingTime
    });
}

// Hàm chính để lấy và hiển thị thời gian chờ khám bệnh
async function drawChartThoiGianChoKhamBenh(today, yesterday) {
    // Khởi tạo các mảng dữ liệu
    const departments = [];
    const waitingMinutes = [];
    const lastWaitingMinutes = [];

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/getThoiGianChoKhamBenh', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xử lý dữ liệu
        data.forEach(item => {
            departments.push(item.Titles || 'Không xác định');
            waitingMinutes.push(parseInt(item.Counts ?? 0));
            lastWaitingMinutes.push(parseInt(item.LastCounts ?? 0));
        });

        // Vẽ biểu đồ
        drawBarChartThoiGianChoKhamBenh('chartAverageWaitingTime', departments, waitingMinutes, lastWaitingMinutes, today, yesterday);
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc vẽ biểu đồ:', error);
        const $chartContainer = document.getElementById('chartAverageWaitingTime')?.parentElement;
        if ($chartContainer) {
            $chartContainer.innerHTML = '<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>';
        }
    }
}
// Hàm chính để lấy và hiển thị tình hình toa thuốc
async function drawChartTinhHinhToaThuoc(yesterday) {
    const $listToaThuoc = $('#chartTinhHinhToaThuoc');

    // Kiểm tra phần tử tồn tại
    if (!$listToaThuoc.length) {
        console.error('Element with ID "chartTinhHinhToaThuoc" not found.');
        return;
    }

    $listToaThuoc.empty(); // Xóa nội dung cũ

    // Mảng màu nền và biểu tượng cho các thẻ
    const backgroundColors = ['bg-primary-300', 'bg-warning-400', 'bg-success-200', 'bg-info-200', 'bg-danger-200', 'bg-fusion-200'];
    const icons = ['mb-n1 fa-user mr-n1', 'mb-n1 fa-gem mr-n4', 'mb-n5 fa-lightbulb mr-n6', 'mb-n1 fa-globe mr-n4'];

    try {
        // Lấy dữ liệu từ API
        const response = await fetch('/Home/GetSoLuongToaThuoc', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        if (!Array.isArray(data)) {
            throw new Error('Dữ liệu trả về không phải là mảng');
        }

        // Xây dựng HTML bằng mảng để tối ưu hiệu suất
        const htmlArray = [];
        data.forEach((item, index) => {
            // Tính toán phần trăm thay đổi
            const change = calculateChange(item.Counts ?? 0, item.LastCounts ?? 0);

            htmlArray.push(`
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="p-3 rounded overflow-hidden position-relative text-white mb-g ${backgroundColors[index] || 'bg-primary-300'}">
                        <div>
                            <h2 class="display-3 d-block l-h-n m-0 fw-500" style="font-size:200%">${item.Titles || 'Không xác định'}</h2>
                            <h2 class="m-0 l-h-n fw-500" style="color:blue;font-size:210%">${item.Counts ?? 0}</h2>
                        </div>
                        <span class="count_bottom">
                            <i class="${change.color}" style="font-size: 24px">
                                <i class="fa ${change.sortIcon}"></i> ${change.percent}%
                            </i>
                            <h4 style="color:black;font-size:15px">Ngày ${yesterday} (${item.LastCounts ?? 0})</h4>
                        </span>
                        <i class="fal position-absolute pos-right pos-bottom opacity-15 ${icons[index] || 'mb-n1 fa-user mr-n1'}" style="font-size:6rem"></i>
                    </div>
                </div>
            `);
        });

        // Append HTML một lần duy nhất
        $listToaThuoc.html(htmlArray.join(''));
    } catch (error) {
        console.error('Lỗi khi lấy dữ liệu hoặc hiển thị:', error);
        $listToaThuoc.html('<div class="alert alert-danger">Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.</div>');
    }
}
//----------DRAW--------------


async function drawLineChartTangTruongLoiNhuan(canvasId = 'myLineChart') {
    const chartCanvas = document.getElementById(canvasId);
    if (!chartCanvas) {
        console.error(`Không tìm thấy canvas với ID: ${canvasId}`);
        return;
    }

    const ctx = chartCanvas.getContext('2d');
    window.chartInstances = window.chartInstances || {};

    try {
        const response = await fetch('/TransportFiles/Dashboard/TangTruongLoiNhuan', {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`Lỗi API: ${response.status} - ${response.statusText}`);
        }

        const rawData = await response.json();
        if (!rawData) {
            throw new Error('Dữ liệu trả về là null hoặc undefined');
        }

        const data = Array.isArray(rawData)
            ? rawData
            : rawData?.data && Array.isArray(rawData.data)
                ? rawData.data
                : null;

        if (!data || !Array.isArray(data)) {
            throw new Error('Dữ liệu không phải là mảng hợp lệ');
        }

        if (data.length === 0) {
            chartCanvas.parentElement.innerHTML = `
                <div class="alert alert-info" role="alert">
                    Không có dữ liệu để hiển thị.
                </div>`;
            return;
        }

        // Trích xuất dữ liệu
        const thoiGians = data.map(item => 'Tháng ' + (item.Thang || 'Không xác định'));
        const counts = data.map(item => {
            const value = parseFloat(item.Counts);
            return isNaN(value) ? 0 : value;
        });

        if (!thoiGians.length || !counts.length) {
            throw new Error('Dữ liệu sau khi xử lý rỗng');
        }

        // Hủy biểu đồ cũ nếu tồn tại
        if (window.chartInstances[canvasId] instanceof Chart) {
            window.chartInstances[canvasId].destroy();
            delete window.chartInstances[canvasId];
        }
     
        // Tạo hàm định dạng tiền VNĐ
        const formatVND = (value) => {
            return value.toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND',
                minimumFractionDigits: 0,
                maximumFractionDigits: 0
            });
        };

        // Tính toán gợi ý cho trục Y
        const maxCount = Math.max(...counts);
        const minCount = Math.min(...counts);
        const range = maxCount - minCount;
        const suggestedMax = maxCount + (range * 0.2);
        const stepSize = Math.ceil(range / 5 / 1000000) * 1000000 || 1000000;

        // Tạo cấu hình biểu đồ
        window.chartInstances[canvasId] = new Chart(ctx, {
            type: 'line',
            data: {
                labels: thoiGians,
                datasets: [{
                    label: 'Tăng trưởng lợi nhuận',
                    data: counts,
                    borderColor: '#FF6384',
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderWidth: 2,
                    pointRadius: 5,
                    pointBackgroundColor: '#FF6384',
                    pointBorderColor: '#FFFFFF',
                    pointBorderWidth: 2,
                    fill: true,
                    tension: 0.3
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'top',
                        labels: {
                            font: {
                                size: 14,
                                family: "'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', sans-serif"
                            }
                        }
                    },
                    title: {
                        display: true,
                        text: 'Biểu đồ tăng trưởng lợi nhuận',
                        font: {
                            size: 16,
                            weight: 'bold'
                        },
                        padding: {
                            top: 10,
                            bottom: 20
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(0, 0, 0, 0.7)',
                        padding: 12,
                        titleFont: {
                            size: 14
                        },
                        bodyFont: {
                            size: 14
                        },
                        callbacks: {
                            label: function (context) {
                                return `${context.dataset.label}: ${formatVND(context.parsed.y)}`;
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Tháng',
                            font: {
                                size: 14,
                                weight: 'bold'
                            }
                        },
                        ticks: {
                            maxRotation: 45,
                            minRotation: 45,
                            font: {
                                size: 12
                            }
                        },
                        grid: {
                            display: false
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: 'Số tiền (VNĐ)',
                            font: {
                                size: 14,
                                weight: 'bold'
                            }
                        },
                        beginAtZero: false,
                        min: Math.max(0, minCount - (range * 0.1)),
                        suggestedMax: suggestedMax,
                        ticks: {
                            padding: 10,
                            font: {
                                size: 12
                            },
                            callback: function (value) {
                                return formatVND(value);
                            }
                        },
                        grid: {
                            color: 'rgba(0, 0, 0, 0.05)'
                        }
                    }
                },
                interaction: {
                    intersect: false,
                    mode: 'index'
                },
                animation: {
                    duration: 1000,
                    easing: 'easeOutQuart'
                },
                elements: {
                    line: {
                        cubicInterpolationMode: 'monotone'
                    }
                }
            }
        });

    } catch (error) {
        console.error(`Lỗi khi vẽ biểu đồ ${canvasId}:`, error);
        chartCanvas.parentElement.innerHTML = `
            <div class="alert alert-danger" role="alert">
                Không thể tải biểu đồ: ${error.message || 'Lỗi không xác định'}
                <br>Vui lòng kiểm tra console để biết chi tiết.
            </div>`;
    }
}
function drawCharttinhhinhPhieuThuocNoiTru() {
    var InformationNT = $('#InformationNT');
    InformationNT.html('');
    var color = 'green';
    var sort = 'fa-sort-asc';
    var html = '';
    var tens = [];
    var fal = ['mb-n1 fa-user mr-n1', 'mb-n1 fa-gem mr-n4', 'mb-n5 fa-lightbulb mr-n6', 'mb-n1 fa-globe mr-n4'];
    var tens = [];
    var soLuongs = [];
    var soLuongPhats = [];
    var soLuongChuaPhats = [];
    var LastCounts = [];
    var LastLuongPhatsCounts = [];
    var LastLuongChuaPhatsCounts = [];

    $.ajax({
        /*      url: '/Home/gettinhhinhPhieuThuocNoiTru?_Ngay=' + strdate,*/
        url: '/Home/gettinhhinhPhieuThuocNoiTru',
        method: 'GET',
        dataType: 'json',
        success: function (result) {
            // Khai báo biến tick và gán cho nó giá trị trả về của phương thức map          
            var info = result;//JSON.parse(result);
            for (var i in info) {
                tens.push(info[i].TEN);
                soLuongs.push(info[i].SL);
                soLuongPhats.push(info[i].SL_PHAT);
                soLuongChuaPhats.push(parseInt(info[i].SL) - parseInt(info[i].SL_PHAT));
                LastCounts.push(info[i].SL_LAST);
                LastLuongPhatsCounts.push(info[i].SL_PHAT_LAST);
                LastLuongChuaPhatsCounts.push(parseInt(info[i].SL_LAST) - parseInt(info[i].SL_PHAT_LAST));
                colors.push('rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',.7)');
                colors2.push('rgba(' + randomColorFactor() + ',' + randomColorFactor1() + ',' + randomColorFactor() + ',.7)');

                var diff = 0;
                if (info[i].SL < info[i].SL_LAST) {
                    color = 'red';
                    sort = 'fa-sort-desc';
                    diff = info[i].SL_LAST - info[i].SL;
                }
                else {
                    color = 'purple';
                    sort = 'fa-sort-asc';
                    diff = info[i].SL - info[i].SL_LAST;
                }
                var lc = 0
                if (info[i].SL_LAST == 0) {
                    lc = 1
                } else {
                    lc = info[i].SL_LAST
                }
                var percent = Math.ceil((diff / lc) * 100);
                html = '';
                html += '<div class="col-lg-6 col-md-6 col-xs-6" style="min-width: 150px;">';
                html += ' <div class="p-3 bg-info-300 rounded overflow-hidden position-relative text-white mb-g rgba(75, 192, 192,0.5)">';
                html += ' <div class="">'
                html += '    <h2 class="display-3 d-block l-h-n m-0 fw-500" style="font-size:130%">' + info[i].TEN + '</h2><h2 class="m-0 l-h-n fw-500" style="color:blue;font-size:150%">' + info[i].SL + '</h2></div>';
                html += '    <span class="count_bottom"><i class="' + color + '" style="font-size: 24px"><i class="fa ' + sort + '"></i>' + percent + '% </i><h4 style="color:black;font-size:15px">Ngày ' + yesterday + ' (' + info[i].SL_LAST + ')</h4></span>';
                html += ' <i class="fal position-absolute pos-right pos-bottom opacity-15 ' + fal[i] + ' " style="font-size:6rem"></i></div></div>'
                html += '</div>';
                InformationNT.append(html);
            }
            const ctx = document.getElementById("charttinhhinhPhieuThuocNoiTru");
            window.myBar = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: tens,
                    datasets: [
                        {
                            label: "Đã phát:" + today,//"Today",
                            type: "bar",
                            stack: "1",
                            backgroundColor: "rgba(75, 192, 192, 0.5)",// "rgba(0,187,255,0.8)",//colors,
                            fill: false,
                            data: soLuongPhats
                        },
                        {
                            label: "Chưa phát:" + today,//"Today",
                            type: "bar",
                            stack: "1",
                            backgroundColor: "rgba(230, 230, 230,0.5)",// "rgba(0,187,255,0.8)",//colors,
                            fill: false,
                            data: soLuongChuaPhats
                        },
                        {
                            label: "Đã phát:" + yesterday,//"Yesterday", 
                            type: "bar",
                            stack: "2",
                            backgroundColor: "rgba(54, 162, 235, 0.5)",// "rgba(151,187,12,0.8)",
                            fill: true,
                            data: LastLuongPhatsCounts //[65, 59, 80, 81, 56, 55, 40]
                        },
                        {
                            label: "Chưa phát:" + yesterday,//"Yesterday", 
                            type: "bar",
                            stack: "2",
                            backgroundColor: "rgba(200, 200, 200,0.8)",// "rgba(151,187,12,0.8)",
                            fill: true,
                            data: LastLuongChuaPhatsCounts //[65, 59, 80, 81, 56, 55, 40]
                        }
                    ],
                    yAxes: [{
                        name: "1",
                        scalePositionLeft: false,
                        scaleFontColor: "rgba(151,137,200,0.8)"
                    }, {
                        name: "2",
                        scalePositionLeft: true,
                        scaleFontColor: "rgba(151,187,205,0.8)"
                    }]
                },
                options: {
                    responsive: false,
                    legend: {
                        position: 'top' // place legend on the right side of chart
                    },
                    scales: {
                        xAxes: [{
                            stacked: true
                        }],
                        yAxes: [{
                            stacked: true
                        }]
                    },
                    animation: {
                        onComplete: function () {
                            var chartInstance = this.chart;
                            var ctx = chartInstance.ctx;
                            ctx.textAlign = "center";
                            Chart.helpers.each(this.data.datasets.forEach(function (dataset, i) {
                                var meta = chartInstance.controller.getDatasetMeta(i);
                                Chart.helpers.each(meta.data.forEach(function (bar, index) {
                                    ctx.fillText(dataset.data[index], bar._model.x + (i % 2 == 0 ? -10 : 10), bar._model.y);
                                }), this)
                            }), this);
                        }
                    }
                }
            });
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi nếu có
            console.log(error);
        }
    });
};


// Hàm gọi tất cả các hàm vẽ biểu đồ và bảng
async function refreshData(todayStr, yesterdayStr) {
    try {
        // Gọi các hàm bất đồng bộ song song để tăng hiệu suất
        await Promise.all([
            drawChartTinhHinhBenhNhan(todayStr, yesterdayStr)
            //drawChartKhamBenh(),
            //drawGetSoTienThuChi(todayStr, yesterdayStr),
            //drawGetSoLieuDeAn06(),
            //drawChartTinhHinhCLS(todayStr, yesterdayStr),
            //drawChartThoiGianChoKhamBenh(todayStr, yesterdayStr),
            //drawChartTinhHinhToaThuoc(yesterdayStr),
            //drawTableNhapXuatKhoa(yesterdayStr),
            , drawLineChartTangTruongLoiNhuan()
            , drawChartNhanSu()
           // ,drawChartGetSoLoiNhuan()
        ]);
 
        console.log('Dữ liệu đã được làm mới thành công.');
    } catch (error) {
        console.error('Lỗi khi làm mới dữ liệu:', error);
        // Hiển thị thông báo lỗi cho người dùng (tùy chọn)
        const $errorContainer = document.createElement('div');
        $errorContainer.className = 'alert alert-danger';
        $errorContainer.textContent = 'Đã xảy ra lỗi khi làm mới dữ liệu. Vui lòng thử lại sau.';
        document.body.prepend($errorContainer);
    }
}

// Hàm khởi tạo và làm mới dữ liệu định kỳ
const initializeAndRefreshData = (intervalTimeGetData = 60000) => {
    // Kiểm tra các thư viện cần thiết
    if (typeof jQuery === 'undefined') {
        console.error('jQuery is not loaded. Please include the library.');
        return;
    }
    if (typeof Chart === 'undefined') {
        console.error('Chart.js is not loaded. Please include the library.');
        return;
    }

    // Khai báo todayStr và yesterdayStr
    const todayDate = new Date();
    const yesterdayDate = new Date(todayDate.getTime() - 86400000);
    const todayStr = formatDate(todayDate);
    const yesterdayStr = formatDate(yesterdayDate);

    // Gọi lần đầu tiên
    refreshData(todayStr, yesterdayStr);

    // Thiết lập interval để làm mới dữ liệu
    const intervalId = setInterval(() => {
        refreshData(todayStr, yesterdayStr);
    }, intervalTimeGetData);

    // Trả về hàm để dừng interval (dùng khi cần)
    return () => clearInterval(intervalId);
};


// Gọi hàm khi tài liệu sẵn sàng
document.addEventListener('DOMContentLoaded', () => {
    // Gọi hàm khởi tạo và nhận hàm dừng interval
    const stopInterval = initializeAndRefreshData(60000); // Mặc định 60 giây

    // Ví dụ: Dừng interval khi người dùng rời khỏi trang (tùy chọn)
    window.addEventListener('beforeunload', () => {
        stopInterval();
    });
});


