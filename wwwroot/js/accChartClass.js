var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
$("#classId").keypress(function (e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
function getInitData() {
    $.ajax({
        type: "GET",
        url: "/AccChart/GetInitData",
        success: function (response) {
            if (response != null) {
                var data = response;
                for (var i = 0; i < data.length; i++) {
                    var option = new Option(data[i].name, data[i].id);
                    $(option).html(data[i].name);
                    $("#ddlClassTypes").append(option);
                }
            }
        }
    });
}
function getAllChartClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/AccChart/GetAllChartClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tAccChartListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tAccChartListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="aCCId${rowTableIdx}">` + items[i].accId + `</td>
                        <td id="classId${rowTableIdx}">` + items[i].classId + `</td>
                        <td id="aCCName${rowTableIdx}">` + items[i].accName + `</td>
                        <td id="aCCCType${rowTableIdx}">` + items[i].acccTypeName + `</td>
                        <td class="text-center">
                             <button id="` + items[i].accId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].accId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tAccChartListbody').on('click', '.remove', function () {

    var accid = parseInt(this.id);
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
                        url: "/AccChart/Delete?id=" + accid,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllChartClass();
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
$('#tAccChartListbody').on('click', '.edit', function () {

    var accId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/AccChart/GetById?id=" + accId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#accId").val(response.accId);
                $("#classId").attr("disabled", true);
                $("#classId").val(response.classId);
                $("#accName").val(response.accName);
                $("#ddlClassTypes").val(response.acccType);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#accId").val(0);
    SaveOrUpdateRequest();
});
$("#classId").blur(function () {
    var classId = $("#classId").val();
    showErrorMessageBelowCtrl('classId', 'Class Id is required', false);
    if (classId == undefined || classId == '') {
        showErrorMessageBelowCtrl('classId', 'Class Id is required', true);
    } else {
        showErrorMessageBelowCtrl('classId', 'Class Id is required', false);
    }
});
$("#accName").blur(function () {
    var accName = $("#accName").val();
    showErrorMessageBelowCtrl('accName', 'Class Name is required', false);
    if (accName == undefined || accName == '') {
        showErrorMessageBelowCtrl('accName', 'Class Name is required', true);
    } else {
        showErrorMessageBelowCtrl('accName', 'Class Name is required', false);
    }
});
$("#ddlClassTypes").change(function () {
    showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', false);
    if (this.value >= 0) {
        showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', false);
    } else {
        showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', true);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('ACCId', $('#accId').val());
    data.append('ClassId', $('#classId').val());
    data.append('ACCName', $('#accName').val());
    data.append('ACCCType', $('#ddlClassTypes').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccChart/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllChartClass();
                $("#classId").val(null);
                $("#accName").val(null);
                $("#ddlClassTypes").val(0);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                $("#classId").attr("disabled", false);

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

    var classId = $("#classId").val();
    var accName = $("#accName").val();
    var classTypes = parseInt($("#ddlClassTypes").children("option:selected").val());

    showErrorMessageBelowCtrl('classId', 'Class Id is required', false);
    showErrorMessageBelowCtrl('accName', 'Class Name is required', false);
    showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', false);

    if (classId == undefined || classId == '') {
        showErrorMessageBelowCtrl('classId', 'Class Id is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#classId").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('classId', 'Class Id is required', false);
    }

    if (accName == undefined || accName == '') {
        showErrorMessageBelowCtrl('accName', 'Class Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#accName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('accName', 'Class Name is required', false);
    }

    if (isNaN(classTypes) || classTypes < 0) {
        showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlClassTypes").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', false);
    }
    return response;
}
$("#cancelButton").click(function () {
    $("#accId").val(0);
    $("#classId").val(null);
    $("#accName").val(null);
    $("#ddlClassTypes").val(0);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#classId").attr("disabled", false);
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});