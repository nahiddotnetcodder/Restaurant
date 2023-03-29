var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/HREmpRoaster/GetInitData",
        success: function (response) {
            if (response != null) {
                var shiftType = response.shiftType;
                var empDetails = response.empDetails;

                for (var i = 0; i < shiftType.length; i++) {
                    var option = new Option(shiftType[i].name, shiftType[i].id);
                    $(option).html(shiftType[i].name);
                    $("#ddlshiftType").append(option);
                }

                $('#ddlempName').append(new Option("--Select Employee Name--", -1))
                for (var i = 0; i < empDetails.length; i++) {
                    var option = new Option(empDetails[i].name, empDetails[i].id);
                    $(option).html(empDetails[i].name);
                    $("#ddlempName").append(option);
                }
            }
        }
    });
}
function getAllEmpRoasterClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HREmpRoaster/GetAllEmpRoasterClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tEmpRoasterListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tEmpRoasterListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrerId${rowTableIdx}">` + items[i].hrerId + `</td>
                        <td id="hrEmpDetailsName${rowTableIdx}">` + items[i].hrEmpDetailsName + `</td>
                        <td id="hrerfDateString${rowTableIdx}">` + items[i].hrerfDateString + `</td>
                        <td id="hrertDateString${rowTableIdx}">` + items[i].hrertDateString + `</td>
                        <td id="shiftTypeName{rowTableIdx}">` + items[i].shiftTypeName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrerId + `" class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                            <button id="` + items[i].hrerId + `" class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tEmpRoasterListbody').on('click', '.remove', function () {

    var hrerId = parseInt(this.id);
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
                        type: "GET",
                        url: "/HREmpRoaster/Delete?id=" + hrerId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllEmpRoasterClass();
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
$('#tEmpRoasterListbody').on('click', '.edit', function () {

    var hrerId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HREmpRoaster/GetById?id=" + hrerId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrerId").val(response.hrerId);
                $("#ddlempName").val(response.hredId);
                $("#hrerfDate").val(response.hrerfDate);
                $("#hrerfDate").hide();
                $("#hrerfDateString").val(response.hrerfDateString);
                $("#hrerfDateString").show();
                $("#hrertDate").val(response.hrertDate);
                $("#hrertDate").hide();
                $("#hrertDateString").val(response.hrertDateString);
                $("#hrertDateString").show();
                $("#ddlshiftType").val(response.shiftType);
                $("#ddlempName").attr("disabled", true);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});

$("#addNewButton").click(function () {
    $("#hrerId").val(0);
    SaveRequest();
});

$("#ddlempName").blur(function () {
    var ddlempName = $("#ddlempName").val();
    showErrorMessageBelowCtrl('ddlempName', 'Employee Name is required', false);
    if (ddlempName == undefined || ddlempName == '') {
        showErrorMessageBelowCtrl('ddlempName', 'Employee Name is required', true);
    } else {
        showErrorMessageBelowCtrl('ddlempName', 'Employee Name is required', false);
    }
});
$("#hrerfDate").blur(function () {
    var hrerfDate = $("#hrerfDate").val();
    showErrorMessageBelowCtrl('hrerfDate', 'Date field is required', false);
    if (hrerfDate == undefined || hrerfDate == '') {
        showErrorMessageBelowCtrl('hrerfDate', 'Date field is required', true);
    } else {
        showErrorMessageBelowCtrl('hrerfDate', 'Date field is required', false);
    }
});
$("#ddlshiftType").change(function () {
    showErrorMessageBelowCtrl('ddlshiftType', 'Shirft Type is required', false);
    if (this.value >= 0) {
        showErrorMessageBelowCtrl('ddlshiftType', 'Shirft Type is required', false);
    } else {
        showErrorMessageBelowCtrl('ddlshiftType', 'Shirft Type is required', true);
    }
});

function SaveRequest() {
    var result = validationCheck();
    if (result == false) { return; }

    var hrerfdate = $("#hrerfDate").val();
    var hrertdate = $("#hrertDate").val();

    if (hrerfdate > hrertdate) {
        alertify.error('From date must be less than To date');
        return;
    }

    var data = new FormData();
    data.append('HRERId', $('#hrerId').val());
    data.append('HREDId', $('#ddlempName').find(":selected").val());
    data.append('HRERFDate', $('#hrerfDate').val());
    data.append('HRERTDate', $('#hrertDate').val());
    data.append('ShiftType', $('#ddlshiftType').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HREmpRoaster/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllEmpRoasterClass();
                restValue();
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

    var hrerfdate = $("#hrerfDateString").val();
    var hrertdate = $("#hrertDateString").val();

    if (hrerfdate > hrertdate) {
        alertify.error('From date must be less than To date');
        return;
    }

    var data = new FormData();

    data.append('HRERId', $('#hrerId').val());
    data.append('HREDId', $('#ddlempName').find(":selected").val());
    data.append('HRERFDate', $('#hrerfDateString').val());
    data.append('HRERTDate', $('#hrertDateString').val());
    data.append('ShiftType', $('#ddlshiftType').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HREmpRoaster/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllEmpRoasterClass();
                restValue();
                alertify.notify('Update Successfully!', 'success');
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
    var hrerfDate = $("#hrerfDate").val();
    var hrertDate = $("#hrertDate").val();
    var ddlshiftType = parseInt($("#ddlshiftType").children("option:selected").val());
    var ddlempName = parseInt($("#ddlempName").children("option:selected").val());

    showErrorMessageBelowCtrl('hrerfDate', 'Date Field is required', false);
    showErrorMessageBelowCtrl('hrertDate', 'Date Fielde is required', false);
    showErrorMessageBelowCtrl('ddlshiftType', 'Shift Type is required', false);
    showErrorMessageBelowCtrl('ddlempName', 'Employee Name is required', false);

    if (hrerfDate == undefined || hrerfDate == '') {
        showErrorMessageBelowCtrl('hrerfDate', 'Date Field  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrerfDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrerfDate', 'Date Field Id is required', false);
    }
    if (hrertDate == undefined || hrertDate == '') {
        showErrorMessageBelowCtrl('hrertDate', 'Date Field  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrertDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrertDate', 'Date Field Id is required', false);
    }


    if (isNaN(ddlshiftType) || ddlshiftType < 0) {
        showErrorMessageBelowCtrl('ddlshiftType', 'Shift type is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlshiftType").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlshiftType', 'Shift type is required', false);
    }

    if (isNaN(ddlempName) || ddlempName < 0) {
        showErrorMessageBelowCtrl('ddlempName', 'Employee Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlempName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlempName', 'Employee Name is required', false);
    }
    return response;
}
$("#cancelButton").click(function () {
    restValue()
});
$("#updateButton").click(function () {
    UpdateRequest();
});

function restValue() { 
    $("#hrerId").val(0);
    $("#hrerfDate").val(null);
    $("#hrerfDate").show();
    $("#hrerfDateString").hide();
    $("#hrertDate").val(null);
    $("#hrertDate").show();
    $("#hrertDateString").hide();
    $("#ddlshiftType").val(-1);
    $("#ddlempName").val(0);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#ddlempName").attr("disabled", false);
}
