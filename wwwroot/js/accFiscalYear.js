var rowTableIdx = 0;
function getInitData() {
    $.ajax({
        type: "GET",
        url: "/AccFiscalYear/GetInitData",
        success: function (response) {
            if (response != null) {
                var data = response;
                for (var i = 0; i < data.length; i++) {
                    var option = new Option(data[i].name, data[i].id);
                    $(option).html(data[i].name);
                    $("#ddlIsClosed").append(option);
                }
            }
        }
    });
}
function getAll() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/AccFiscalYear/GetAll",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tAccFiscalYearbody').empty();
            for (var i = 0; i < items.length; i++) {
                if (items[i].canClose) {

                    $('#tAccFiscalYearbody').append(
                        `<tr id="Item${++rowTableIdx}">
                        <td hidden id="afyId${rowTableIdx}">` + items[i].afyId + `</td>
                        <td id="afyBeginDateString${rowTableIdx}">` + items[i].afyBeginDateString + `</td>
                        <td id="afyEndDateString${rowTableIdx}">` + items[i].afyEndDateString + `</td>
                        <td id="afyClosedName${rowTableIdx}">` + items[i].afyClosedName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].afyId + `" class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                            <button id="` + items[i].afyId + `" class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                        </td>
                    </tr>`
                    );
                }
                else {

                    $('#tAccFiscalYearbody').append(
                        `<tr id="Item${++rowTableIdx}">
                        <td hidden id="afyId${rowTableIdx}">` + items[i].afyId + `</td>
                        <td id="afyBeginDateString${rowTableIdx}">` + items[i].afyBeginDateString + `</td>
                        <td id="afyEndDateString${rowTableIdx}">` + items[i].afyEndDateString + `</td>
                        <td id="afyClosedName${rowTableIdx}">` + items[i].afyClosedName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].afyId + `" class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                        </td>
                    </tr>`
                    );
                }
            }
        }
    });
}
$('#tAccFiscalYearbody').on('click', '.remove', function () {

    var afyId = parseInt(this.id);
    $.confirm({
        title: 'Are you sure want to delete?',
        content: 'You will not be able to recover this item.',
        type: 'red',
        buttons: {
            yes: {
                btnClass: 'btn-danger',
                keys: ['enter'],
                action: function () {
                    $.ajax({
                        type: "get",
                        url: "/AccFiscalYear/Delete?id=" + afyId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAll();
                            }
                            else {
                                alertify.error(response.message);
                            }
                        }
                    });
                }
            },
            no: function () {
            }
        }
    });
});
$('#tAccFiscalYearbody').on('click', '.edit', function () {

    var accId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/AccFiscalYear/GetById?id=" + accId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#afyId").val(response.afyId);
                $("#afyBeginDate").val(response.afyBeginDate);
                $("#afyBeginDate").hide();
                $("#afyBeginDateString").val(response.afyBeginDateString);
                $("#afyBeginDateString").show();
                $("#afyEndDate").val(response.afyEndDate);
                $("#afyEndDate").hide();
                $("#afyEndDateString").val(response.afyEndDateString);
                $("#afyEndDateString").show();
                $("#ddlIsClosed").val(response.afyClosed);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#afyId").val(0);
    SaveRequest();
});
$("#afyBeginDate").blur(function () {
    var afyBeginDate = $("#afyBeginDate").val();
    showErrorMessageBelowCtrl('afyBeginDate', 'Fiscal Year Begin is required', false);
    if (afyBeginDate == undefined || afyBeginDate == '') {
        showErrorMessageBelowCtrl('afyBeginDate', 'Fiscal Year Begin is required', true);
    } else {
        showErrorMessageBelowCtrl('afyBeginDate', 'Fiscal Year Begin is required', false);
    }
});
$("#afyEndDate").blur(function () {
    var afyEndDate = $("#afyEndDate").val();
    showErrorMessageBelowCtrl('afyEndDate', 'Fiscal Year End is required', false);
    if (afyEndDate == undefined || afyEndDate == '') {
        showErrorMessageBelowCtrl('afyEndDate', 'Fiscal Year End is required', true);
    } else {
        showErrorMessageBelowCtrl('afyEndDate', 'Fiscal Year End is required', false);
    }
});
$("#ddlIsClosed").change(function () {
    showErrorMessageBelowCtrl('ddlIsClosed', 'Is Closed is required', false);
    if (this.value >= 0) {
        showErrorMessageBelowCtrl('ddlIsClosed', 'Is Closed is required', false);
    } else {
        showErrorMessageBelowCtrl('ddlIsClosed', 'Is Closed is required', true);
    }
});

function SaveRequest() {
    var result = validationCheck();
    if (result == false) { return; }

    var afyBeginDate = $("#afyBeginDate").val();
    var afyEndDate = $("#afyEndDate").val();

    if (afyBeginDate > afyEndDate) {
        alertify.error('Fiscal Begin date must be less than fiscal end date');
        return;
    }
    var data = new FormData();
    data.append('AFYId', $('#afyId').val());
    data.append('AFYBeginDate', $('#afyBeginDate').val());
    data.append('AFYEndDate', $('#afyEndDate').val());
    data.append('AFYClosed', $('#ddlIsClosed').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccFiscalYear/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAll();
                resetValue();
            }
            else {
                alertify.error(response.message);
            }
        },
        complete: function () {
            console.log("complete");
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
}
function UpdateRequest() {

    var data = new FormData();

    data.append('AFYId', $('#afyId').val());
    data.append('AFYClosed', $('#ddlIsClosed').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccFiscalYear/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAll();
                resetValue();
            }
            else {
            }
        },
        complete: function () {
            console.log("complete");
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
}
function validationCheck() {

    var response = true;

    var afyBeginDate = $("#afyBeginDate").val();
    var afyEndDate = $("#afyEndDate").val();

    showErrorMessageBelowCtrl('afyBeginDate', 'Fiscal Year Begin is required', false);
    showErrorMessageBelowCtrl('afyEndDate', 'Fiscal Year End is required', false);

    if (afyBeginDate == undefined || afyBeginDate == null || afyBeginDate == '') {
        showErrorMessageBelowCtrl('afyBeginDate', 'Fiscal Year Begin is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#afyBeginDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('afyBeginDate', 'Fiscal Year Begin is required', false);
    }

    if (afyEndDate == undefined || afyEndDate == null || afyEndDate == '') {
        showErrorMessageBelowCtrl('afyEndDate', 'Fiscal Year End is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#afyEndDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('afyEndDate', 'Fiscal Year End is required', false);
    }
    return response;
}

function setDate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $('#afyBeginDate').val(today);
    addOneYear(now);
}
function addOneYear(date) {
    const dateCopy = new Date(date);
    dateCopy.setFullYear(dateCopy.getFullYear() + 1);
    var day = ("0" + dateCopy.getDate()).slice(-2);
    var month = ("0" + (dateCopy.getMonth() + 1)).slice(-2);
    var today = dateCopy.getFullYear() + "-" + (month) + "-" + (day);
    $('#afyEndDate').val(today);
}


$("#cancelButton").click(function () {
    resetValue();
});
$("#updateButton").click(function () {
    UpdateRequest();
});
function resetValue() {
    $("#afyId").val(0);
    $("#afyBeginDate").val(null);
    $("#afyBeginDate").show();
    $("#afyBeginDateString").hide();
    $("#afyEndDate").val(null);
    $("#afyEndDate").show();
    $("#afyEndDateString").hide();
    $("#ddlIsClosed").val(0);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
}