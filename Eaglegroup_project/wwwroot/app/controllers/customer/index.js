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

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
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
                    $('#txtCreatorNote').val(data.creatorNote);
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
                let saleNote = $('#txtSaleNote').val();
                let price = eagle.unformatNumber($('#txtPrice').val());
                let deal = eagle.dateTimeFormat($('#txtDeal').val());
                let dateSend = eagle.dateTimeFormat($('#txtDateSendByCustomer').val());
                //let staffId = $('#txtStaffId').val();
                //let creatorId = $('#txtCreatorId').val(;

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
                    var data = response;
                    $('#hidId').val(data.id);
                    $('#txtPhoneNumber').val(data.phoneNumber);
                    $('#txtFullName').val(data.fullName);
                    $('#txtCreatorNote').val(data.creatorNote);
                    $('#txtPrice').val(eagle.formatNumber(data.price, 3));
                    $('#txtDeal').val(data.deal);
                    $('#txtSaleId').val(data.saleId); 
                    $('#txtCreatorId').val(data.creatorId)
                    $('#txtDateCreated').val(eagle.dateTimeFormatJson(data.dateCreated)),

                        disableFieldCustomer(true);
                    $('#modal-add-edit').modal('show');
                    eagle.stopLoading();

                },
                error: function () {
                    eagle.notify('Có lỗi xảy ra', 'error');
                    eagle.stopLoading();
                }
            });
        });



    }

    function disableFieldEdit(disabled) {
        if (userRole == 'SaleStaff') {
            $('#txtCreatorNote').prop('disabled', disabled);
        }   
    }

    function disableFieldCustomer(disabled) {

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
                $('#total-customer').val(response.totalCustomer);
                if (response.rowCount > 0) {
                    $.each(response.results, function (i, item) {
                        render += Mustache.render(template, {
                            Id: item.id,
                            FullName: item.fullName,
                            CreatorNote: item.creatorNote,
                            SaleNote: item.saleNote,
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
}