var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/HRWeekend/GetInitData",
        success: function (response) {
            if (response != null) {
                var data = response;
                for (var i = 0; i < data.length; i++) {
                    var option = new Option(data[i].name, data[i].id);
                    $(option).html(data[i].name);
                    $("#ddlweekend").append(option);
                }
            }
        }
    });
}
function getAllWeekdayClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRWeekend/GetAllWeekdayClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tWeekendListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tWeekendListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hRWId${rowTableIdx}">` + items[i].hrwId + `</td>
                        <td id="weekday${rowTableIdx}">` + items[i].weekdayName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrwId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrwId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}

$('#tWeekendListbody').on('click', '.remove', function () {

    var hrwid = parseInt(this.id);
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
                        url: "/HRWeekend/delete?id=" + hrwid,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllWeekdayClass();
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
$('#tWeekendListbody').on('click', '.edit', function () {

    var hrwId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HRWeekend/GetById?id=" + hrwId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrwId").val(response.hrwId);
                $("#ddlweekend").val(response.weekday);
                $("#ddlweekend").attr("disabled", true);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrwId").val(0);
    SaveOrUpdateRequest();
});
$("#hrwId").blur(function () {
    var classId = $("#hrwId").val();
    showErrorMessageBelowCtrl('hrwId', 'Weekend is required', false);
    if (classId == undefined || classId == '') {
        showErrorMessageBelowCtrl('hrwId', 'Weekend Id is required', true);
    } else {
        showErrorMessageBelowCtrl('hrwId', 'Weekend Id is required', false);
    }
});

$("#ddlweekend").change(function () {
    showErrorMessageBelowCtrl('ddlweekend', 'Weekend Type is required', false);
    if (this.value >= 0) {
        showErrorMessageBelowCtrl('ddlweekend', 'Weekend Type is required', false);
    } else {
        showErrorMessageBelowCtrl('ddlweekend', 'Weekend Type is required', true);
    }
});

function SaveOrUpdateRequest() {

    var data = new FormData();

    data.append('HRWId', $('#hrwId').val());
    data.append('Weekday', $('#ddlweekend').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRWeekend/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllWeekdayClass();
                $("#hrwId").val(null);
                $("#ddlweekend").val(0);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                $("#ddlweekend").attr("disabled", false);
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

$("#cancelButton").click(function () {
    $("#hrwId").val(0);
    $("#ddlweekend").val(0);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#ddlweekend").attr("disabled", false);
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});