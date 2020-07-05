var customerController = function () {
    this.initialize = function () {

        $(document).ready(function () {
            loadData();
            registerEvents();
        });
    }
    var userRole = $('#user_role').val();
    function registerEvents() {
       
        //Init validation
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'vi',
            rules: {

            }
        });

        $('#txtDateSendByCustomer, #txtDeal').datepicker({
            autoclose: true,
            format: 'dd/mm/yyyy'
        });

        $('#txt-search-keyword').keypress(function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });

        $("#btn-search").on('click', function () {
            loadData();
        });

        $("#ddl-show-page").on('change', function () {
            eagle.configs.pageSize = $(this).val();
            eagle.configs.pageIndex = 1;
            loadData(true);
        });

        $("#btn-create").on('click', function () {
            resetFormMaintainance();
            $('#modal-add-edit').modal('show');
        });

        $("#modal-add-edit").on("hidden.bs.modal", function () {
            location.reload();
        });

        $('#customer-status-select').change(function () {
            let selectedStatus = $(this).children('option:selected').val();
            $('#txtCustomerStatus').val(selectedStatus);
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            let that = $(this).data('id');
            $.ajax({
                type: "GET",
                url: "GetById",
                data: { id: that },
                dataType: "json",
                beforeSend: function () {
                    eagle.startLoading();
                },
                success: function (response) {
                    var data = response;
                    $('#hidId').val(data.id);
                    $('#txtFullName').val(data.fullName);
                    $('#txtCreatorId').val(data.creatorId)
                    $('#txtCreatorNote').val(data.creatorNote);
                    $('#txtSaleId').val(data.saleId)
                    $('#txtSaleNote').val(data.saleNote);
                    $('#txtPrice').val(eagle.formatNumber(data.price, 3));
                    $('#txtDeal').val(eagle.dateFormatJson(data.deal));
                    $('#txtDateSendByCustomer').val(eagle.dateFormatJson(data.dateSendByCustomer));
                    $('#txtPhoneNumber').val(data.phoneNumber);
                    $('#ckStatus').prop('checked', data.status === 1);

                    disableFieldEdit(true);
                    $('#modal-add-edit').modal('show');
                    eagle.stopLoading();

                },
                error: function () {
                    eagle.notify('Có lỗi xảy ra', 'error');
                    eagle.stopLoading();
                }
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();

                let id = $('#hidId').val();
                let fullName = $('#txtFullName').val();
                let phoneNumber = $('#txtPhoneNumber').val();
                let creatorNote = $('#txtCreatorNote').val();
                let saleNote = $('#txtSaleNote').val()||"";
                let price = eagle.unformatNumber($('#txtPrice').val());
                let deal = $('#txtDeal').val() ? eagle.dateTimeFormat($('#txtDeal').val()) : null;
                let status = $('#txtCustomerStatus').val() ?? 0;
                let dateSend = eagle.dateTimeFormat($('#txtDateSendByCustomer').val());
                //let staffId = $('#txtStaffId').val();
                //let creatorId = $('#txtCreatorId').val();

                $.ajax({
                    type: "POST",
                    url: "SaveEntity",
                    data: {
                        Id: id,
                        FullName: fullName,
                        PhoneNumber: phoneNumber,
                        CreatorNote: creatorNote,
                        SaleNote: saleNote,
                        Deal: deal,
                        Price: price,
                        DateSendByCustomer: dateSend,
                        Status: status
                        //CreatorId: creatorId,
                        //StaffId: staffId
                    },
                    dataType: "json",
                    beforeSend: function () {
                        eagle.startLoading();
                    },
                    success: function () {
                        eagle.notify('Lưu thành công', 'success');
                        $('#modal-add-edit').modal('hide');
                        resetFormMaintainance();

                        eagle.stopLoading();
                        loadData(true);
                    },
                    error: function (e) {
                        eagle.notify('Có lỗi xảy ra', 'error');
                        console.log("e ", e)
                        eagle.stopLoading();
                    }
                });
            }
            return false;
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            eagle.confirm('Xóa khách hàng này?', function () {
                $.ajax({
                    type: "POST",
                    url: "Delete",
                    data: { id: that },
                    beforeSend: function () {
                        eagle.startLoading();
                    },
                    success: function () {
                        eagle.notify('Xóa bản ghi thành công', 'success');
                        eagle.stopLoading();
                        loadData();
                    },
                    error: function () {
                        eagle.notify('Có lỗi xảy ra', 'error');
                        eagle.stopLoading();
                    }
                });
            });
        });

        $('body').on('click', '#btn-get-customer', function (e) {
            e.preventDefault();
            $.ajax({
                type: "GET",
                url: "GetCustomerForSale",
                dataType: "json",
                beforeSend: function () {
                    eagle.startLoading();
                },
                success: function (response) {
                    if (!response) {
                        eagle.notify('Hết số có thể nhận', 'error');
                    } else {
                        var data = response;
                        $('#hidId').val(data.id);
                        $('#txtPhoneNumber').val(data.phoneNumber);
                        $('#txtFullName').val(data.fullName);
                        $('#txtCreatorNote').val(data.creatorNote);
                        $('#txtPrice').val(eagle.formatNumber(data.price, 3));
                        $('#txtDeal').val(eagle.dateFormatJson(data.deal));//LeHieu
                        $('#txtDateSendByCustomer').val(eagle.dateFormatJson(data.dateSendByCustomer));
                        $('#txtSaleId').val(data.saleId);
                        $('#txtCreatorId').val(data.creatorId)
                        $('#txtDateCreated').val(eagle.dateTimeFormatJson(data.dateCreated)),

                            disableFieldCustomer(true);
                        $('#modal-add-edit').modal({ backdrop: 'static', keyboard: false, show: true });
                    }
                    
                    eagle.stopLoading();

                },
                error: function () {
                    eagle.notify('Có lỗi xảy ra', 'error');
                    eagle.stopLoading();
                }
            });
        });

        $("input[data-type='currency']").on({
            keyup: function () {
                formatCurrency($(this));
            },
            blur: function () {
                formatCurrency($(this), "blur");
            }
        });

    }

    function disableFieldEdit(disabled) {
        if (userRole == 'SaleStaff') {
            $('#txtCreatorNote').prop('disabled', disabled);
            $('#txtPhoneNumber').prop('disabled', disabled);
            $('#txtFullName').prop('disabled', disabled);
        }   
    }

    function disableFieldCustomer(disabled) {
        if (userRole == 'SaleStaff') {
            $('#txtCreatorNote').prop('disabled', disabled);
            $('#txtPhoneNumber').prop('disabled', disabled);
            $('#txtFullName').prop('disabled', disabled);
        }   
    }

    function resetFormMaintainance() {
        disableFieldEdit(false);
        $('#hidId').val('');
    }

    function resetFormGetCustomer() {
        disableFieldCustomer(false);
        $('#hidId').val('');
    }

    function loadData(isPageChanged) {
        $.ajax({
            type: "GET",
            url: "GetAllPaging",
            data: {
                keyword: $('#txt-search-keyword').val(),
                page: eagle.configs.pageIndex,
                pageSize: eagle.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                eagle.startLoading();
            },
            success: function (response) {
                var template = $('#table-template').html();
                var render = "";
                $('#total-customer').html('Tổng số khách hàng có sẵn: ' + response.totalCustomer);
                if (response.rowCount > 0) {
                    $.each(response.results, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.id,
                            FullName: item.fullName,
                            PhoneNumber: item.phoneNumber,
                            CreatorNote: item.creatorNote,
                            SaleNote: item.saleNote,
                            CreatorName: item.creatorName,
                            SaleName:item.saleName,
                            Price: eagle.formatNumber(item.price, 3),
                            Deal: eagle.dateFormatJson(item.deal),
                            DateSendByCustomer: eagle.dateFormatJson(item.dateSendByCustomer),
                            Status: eagle.getCustomerStatus(item.status),

                        });
                    });
                    $("#lbl-total-records").text(response.rowCount);
                    if (render !== undefined) {
                        $('#tbl-content').html(render);

                    }
                    wrapPaging(response.rowCount, function () {
                        loadData();
                    }, isPageChanged);


                }
                else {
                    $('#tbl-content').html('');
                }
                eagle.stopLoading();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / eagle.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                eagle.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
    function formatNumber(n) {
        // format number 1000000 to 1,234,567
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".")
    }


    function formatCurrency(input, blur) {
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position 
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(",") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(",");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = formatNumber(left_side);

            // validate right side
            right_side = formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            if (blur === "blur") {
                right_side += "00";
            }

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            input_val = left_side + "," + right_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = formatNumber(input_val);
            input_val = input_val;

            // final formatting
            if (blur === "blur") {
                input_val += ",00";
            }
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }
}