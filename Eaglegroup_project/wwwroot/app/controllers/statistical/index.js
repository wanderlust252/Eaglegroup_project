var statisticalController = function () {
    this.initialize = function () {

        $(document).ready(function () {
            loadData();
            registerEvents();
        });
    } 
    function registerEvents() {

        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {

            }
        });
        //$('#ddlTaskGroup').chosen({ width: "100%" });
        $('.datep').datepicker({
            todayBtn: "linked",
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: false,
            autoclose: true,
            format: 'dd/mm/yyyy',
            // format: 'yyyy-mm-dd'
        });
        $("#txtBeginDateFilter").on('change', function () {
            $("#txtEndDateFilter").datepicker('show');
        });
        $("#txtEndDateFilter").on('changeDate', function () {
            /// convert date
            var fromD = $("#txtBeginDateFilter").val().split("/")
            fromD = new Date(fromD[2], fromD[1] - 1, fromD[0])
            fromD = (fromD.getMonth() + 1) + "-" + fromD.getDate() + "-" + fromD.getFullYear();
            var toD = $("#txtEndDateFilter").val().split("/")
            toD = new Date(toD[2], toD[1] - 1, toD[0])
            toD = (toD.getMonth() + 1) + "-" + toD.getDate() + "-" + toD.getFullYear();
            const status = $('#ddlTaskGroup').val()
            ///
            //console.log("Statistical?fromDate=" + fromD + "&toDate=" + toD)
            $.ajax({
                type: "GET",
                url: "Statistical/GetStatiscal?fromDate=" + fromD + "&toDate=" + toD + "&status=" + status,
                dataType: "json",
                success: function (response) {
                    $("#txt_income").html(response)
                },
                error: function () {
                    eagle.notify('Có Lỗi Xảy Ra', 'error');
                }
            });
            $.ajax({
                type: "GET",
                url: "Statistical/GetDealRate?fromDate=" + fromD + "&toDate=" + toD + "&status=" + status,
                dataType: "json",
                success: function (response) {
                    $("#txt_tilechot").html(response)
                },
                error: function () {
                    eagle.notify('Có Lỗi Xảy Ra', 'error');
                }
            });
        });

        $("#btn-specifyDate").on('click', function () {
            var fromD = $("#txtSpecifyDate").val().split("/")
            fromD = new Date(fromD[2], fromD[1] - 1, fromD[0])
            fromD = (fromD.getMonth() + 1) + "-" + fromD.getDate() + "-" + fromD.getFullYear();
            const status = $('#ddlTaskGroup').val()
            $.ajax({
                type: "GET",
                url: "Statistical/GetStatiscalBySpecifyDate?specDate=" + fromD + "&status=" + status,
                dataType: "json",
                success: function (response) {
                    $("#txt_income").html(response)
                },
                error: function () {
                    eagle.notify('Có Lỗi Xảy Ra', 'error');
                }
            });
            $.ajax({
                type: "GET",
                url: "Statistical/GetDealRateBySpecifyDate?specDate=" + fromD + "&status=" + status,
                dataType: "json",
                success: function (response) {
                    $("#txt_tilechot").html(response)
                },
                error: function () {
                    eagle.notify('Có Lỗi Xảy Ra', 'error');
                }
            });
        });

        $("#btn-rangeDate").on('click', function () {
            /// convert date
            var fromD = $("#txtBeginDateFilter").val().split("/")
            fromD = new Date(fromD[2], fromD[1] - 1, fromD[0])
            fromD = (fromD.getMonth() + 1) + "-" + fromD.getDate() + "-" + fromD.getFullYear();
            var toD = $("#txtEndDateFilter").val().split("/")
            toD = new Date(toD[2], toD[1] - 1, toD[0])
            toD = (toD.getMonth() + 1) + "-" + toD.getDate() + "-" + toD.getFullYear();
            const status = $('#ddlTaskGroup').val()
            ///
            //console.log("Statistical?fromDate=" + fromD + "&toDate=" + toD)
            $.ajax({
                type: "GET",
                url: "Statistical/GetStatiscal?fromDate=" + fromD + "&toDate=" + toD + "&status=" + status,
                dataType: "json",
                success: function (response) {
                    $("#txt_income").html(response)
                },
                error: function () {
                    eagle.notify('Có Lỗi Xảy Ra', 'error');
                }
            });
            $.ajax({
                type: "GET",
                url: "Statistical/GetDealRate?fromDate=" + fromD + "&toDate=" + toD + "&status=" + status,
                dataType: "json",
                success: function (response) {
                    $("#txt_tilechot").html(response)
                },
                error: function () {
                    eagle.notify('Có Lỗi Xảy Ra', 'error');
                }
            });
        });
    }
    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "Statistical/GetStatiscal",
            dataType: "json",
            success: function (response) {
                $("#txt_income").html(response)
            },
            error: function () {
                eagle.notify('Có Lỗi Xảy Ra', 'error');
            }
        });
    }; 
}