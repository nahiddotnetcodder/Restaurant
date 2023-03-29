var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/HRHolidays/GetInitData",
        success: function (response) {
            if (response != null) {
                var data = response;
                for (var i = 0; i < data.length; i++) {
                    var option = new Option(data[i].name, data[i].id);
                    $(option).html(data[i].name);
                    $("#ddlholidayType").append(option);
                }
            }
        }
    });
}
function getAllHolidayClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRHolidays/GetAllHolidayClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tHolidayListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tHolidayListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrhId${rowTableIdx}">` + items[i].hrhId + `</td>
                        <td id="holidayTypeName${rowTableIdx}">` + items[i].holidayTypeName + `</td>
                        <td id="hrhStartDateString${rowTableIdx}">` + items[i].hrhStartDateString + `</td>
                        <td id="hrhEndDateString${rowTableIdx}">` + items[i].hrhEndDateString + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrhId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrhId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tHolidayListbody').on('click', '.remove', function () {

    var hrhId = parseInt(this.id);
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
                        url: "/HRHolidays/delete?id=" + hrhId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllHolidayClass();
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
$('#tHolidayListbody').on('click', '.edit', function () {

    var hrhId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HRHolidays/GetById?id=" + hrhId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrhId").val(response.hrhId);
                $("#ddlholidayType").val(response.holidayType);
                $("#hrhStartDate").val(response.hrhStartDate);
                $("#hrhStartDate").hide();
                $("#hrhStartDateString").val(response.hrhStartDateString);
                $("#hrhStartDateString").show();
                $("#hrhEndDate").val(response.hrhEndDate);
                $("#hrhEndDate").hide();
                $("#hrhEndDateString").val(response.hrhEndDateString);
                $("#hrhEndDateString").show();
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrhId").val(0);
    SaveRequest();
});

$("#holidayType").change(function () {
    showErrorMessageBelowCtrl('holidayType', 'Holiday Type is required', false);
    if (this.value >= 0) {
        showErrorMessageBelowCtrl('holidayType', 'Holiday Type is required', false);
    } else {
        showErrorMessageBelowCtrl('holidayType', 'Holiday Type is required', true);
    }
});

$("#hrhStartDate").blur(function () {
    var hrhStartDate = $("#hrhStartDate").val();
    showErrorMessageBelowCtrl('hrhStartDate', 'Start Date is required', false);
    if (hrhStartDate == undefined || hrhStartDate == '') {
        showErrorMessageBelowCtrl('hrhStartDate', 'Start Date is required', true);
    } else {
        showErrorMessageBelowCtrl('hrhStartDate', 'Start Date is required', false);
    }
});
$("#hrhEndDate").blur(function () {
    var hrhEndDate = $("#hrhEndDate").val();
    showErrorMessageBelowCtrl('hrhEndDate', 'End Date is required', false);
    if (hrhEndDate == undefined || hrhEndDate == '') {
        showErrorMessageBelowCtrl('hrhEndDate', 'End Date is required', true);
    } else {
        showErrorMessageBelowCtrl('hrhEndDate', 'End Date is required', false);
    }
});

function SaveRequest() {
    var result = validationCheck();
    if (result == false) { return; }

    var hrhStartDate = $("#hrhStartDate").val();
    var hrhEndDate = $("#hrhEndDate").val();

    if (hrhStartDate > hrhEndDate) {
        alertify.error('Start date must be less than End date');
        return
    }
    var data = new FormData();
    data.append('HRHId', $('#hrhId').val());
    data.append('HolidayType', $('#ddlholidayType').find(":selected").val());
    data.append('HRHStartDate', $('#hrhStartDate').val());
    data.append('HRHEndDate', $('#hrhEndDate').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRHolidays/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllHolidayClass();
                resetValue();
                alertify.notify('Saved Successfully!', 'success');
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
    data.append('HRHId', $('#hrhId').val());
    data.append('HolidayType', $('#ddlholidayType').find(":selected").val());
    data.append('HRHStartDate', $('#hrhStartDateString').val());
    data.append('HRHEndDate', $('#hrhEndDateString').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRHolidays/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllHolidayClass();
                resetValue();
                alertify.notify('Updated Successfully!', 'success');
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
function validationCheck() {

    var response = true;

    var hrhStartDate = $("#hrhStartDate").val();
    var hrhEndDate = $("#hrhEndDate").val();

    showErrorMessageBelowCtrl('hrhStartDate', 'Start Date is required', false);
    showErrorMessageBelowCtrl('hrhEndDate', 'End Date  is required', false);


    if (hrhStartDate == undefined || hrhStartDate == '') {
        showErrorMessageBelowCtrl('hrhStartDate', 'Start Date is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrhStartDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrhStartDate', 'Start Date is required', false);
    }


    if (hrhEndDate == undefined || hrhEndDate == '') {
        showErrorMessageBelowCtrl('hrhEndDate', 'End Date is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrhEndDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrhEndDate', 'End Date is required', false);
    }
    
    return response;
}
$("#cancelButton").click(function () {
    resetValue();
});
$("#updateButton").click(function () {
    UpdateRequest();
});

function resetValue() {
    $("#accId").val(0);
    $("#ddlholidayType").val(0);
    $("#hrhStartDate").val(null);
    $("#hrhStartDate").show();
    $("#hrhStartDateString").hide();
    $("#hrhEndDate").val(null);
    $("#hrhEndDate").show();
    $("#hrhEndDateString").hide();
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
}