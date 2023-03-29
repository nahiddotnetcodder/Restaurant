var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
$("#actClassId").keypress(function (e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
function getInitData() {
    $.ajax({
        type: "GET",
        url: "/AccChartType/GetInitData",
        success: function (response) {
            if (response != null) {

                var accTypesDD = response.accTypesDD;
                var accChartsDD = response.accChartsDD;
                $('#ddlChartTypes').empty();
                $('#ddlClassTypes').empty();
                for (var i = 0; i < accTypesDD.length; i++) {
                    var option = new Option(accTypesDD[i].name, accTypesDD[i].id);
                    $(option).html(accTypesDD[i].name);
                    $("#ddlChartTypes").append(option);
                }

                $('#ddlClassTypes').append(new Option("None", -1))
                for (var i = 0; i < accChartsDD.length; i++) {
                    var option = new Option(accChartsDD[i].name, accChartsDD[i].id);
                    $(option).html(accChartsDD[i].name);
                    $("#ddlClassTypes").append(option);
                }
            }
        }
    });
}
function getAllChartType() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/AccChartType/GetAllChartType",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tAccChartTypeListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tAccChartTypeListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="actId{rowTableIdx}">` + items[i].actId + `</td>
                        <td id="actClassId${rowTableIdx}">` + items[i].actClassId + `</td>
                        <td id="actName${rowTableIdx}">` + items[i].actName + `</td>
                        <td id="actParentName{rowTableIdx}">` + items[i].actParentName + `</td>
                        <td id="accChartClassName{rowTableIdx}">` + items[i].accChartClassName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].actId + `" class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                            <button id="` + items[i].actId + `" class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tAccChartTypeListbody').on('click', '.remove', function () {

    var actId = parseInt(this.id);
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
                        url: "/AccChartType/Delete?id=" + actId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllChartType();
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
$('#tAccChartTypeListbody').on('click', '.edit', function () {

    var actId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/AccChartType/GetById?id=" + actId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#actId").val(response.actId);
                $("#actClassId").val(response.actClassId);
                $("#actClassId").attr("disabled", true);
                $("#actName").val(response.actName);
                $("#ddlClassTypes").val(response.accId);
                $("#ddlChartTypes").val(response.actParentId);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#actId").val(0);
    SaveOrUpdateRequest();
});
$("#actClassId").blur(function () {
    var actClassId = $("#actClassId").val();
    showErrorMessageBelowCtrl('actClassId', 'Class Id is required', false);
    if (actClassId == undefined || actClassId == '') {
        showErrorMessageBelowCtrl('actClassId', 'Class Id is required', true);
    } else {
        showErrorMessageBelowCtrl('actClassId', 'Class Id is required', false);
    }
});
$("#actName").blur(function () {
    var actName = $("#actName").val();
    showErrorMessageBelowCtrl('actName', 'Class Name is required', false);
    if (actName == undefined || actName == '') {
        showErrorMessageBelowCtrl('actName', 'Class Name is required', true);
    } else {
        showErrorMessageBelowCtrl('actName', 'Class Name is required', false);
    }
});
$("#ddlChartTypes").change(function () {
    showErrorMessageBelowCtrl('ddlChartTypes', 'Class Type is required', false);
    if (this.value >= 0) {
        showErrorMessageBelowCtrl('ddlChartTypes', 'Class Type is required', false);
    } else {
        showErrorMessageBelowCtrl('ddlChartTypes', 'Class Type is required', true);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    var actparentName = $('#ddlChartTypes').find(":selected").text();

    data.append('ACTId', $('#actId').val());
    data.append('ACTClassId', $('#actClassId').val());
    data.append('ACTName', $('#actName').val());
    data.append('ACCId', $('#ddlClassTypes').find(":selected").val());
    data.append('ACTParentId', $('#ddlChartTypes').find(":selected").val());
    data.append('ACTParentName', actparentName);

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccChartType/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllChartType();
                getInitData();
                $("#actClassId").val(null);
                $("#actName").val(null);
                $("#ddlClassTypes").val(-1);
                $("#ddlChartTypes").val(0);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                $("#actClassId").attr("disabled", false);
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
    var actClassId = $("#actClassId").val();
    var actName = $("#actName").val();
    var classTypes = parseInt($("#ddlClassTypes").children("option:selected").val());

    showErrorMessageBelowCtrl('actClassId', 'Id is required', false);
    showErrorMessageBelowCtrl('actName', 'ACT Name is required', false);
    showErrorMessageBelowCtrl('ddlClassTypes', 'Class Type is required', false);

    if (actClassId == undefined || actClassId == '') {
        showErrorMessageBelowCtrl('actClassId', 'Class Id is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#actClassId").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('actClassId', 'Class Id is required', false);
    }

    if (actName == undefined || actName == '') {
        showErrorMessageBelowCtrl('actName', 'Class Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#actName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('actName', 'Class Name is required', false);
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
    $("#actId").val(0);
    $("#actClassId").val(null);
    $("#actName").val(null);
    $("#ddlClassTypes").val(-1);
    $("#ddlChartTypes").val(0);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#actClassId").attr("disabled", false);
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});