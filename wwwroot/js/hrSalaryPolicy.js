var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
$("#srsppercent").keypress(function (e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/HRSalaryPolicy/GetInitData",
        success: function (response) {
            if (response != null) {
                var adduc = response.adduc;
                var perNper = response.perNper;

                for (var i = 0; i < adduc.length; i++) {
                    var option = new Option(adduc[i].name, adduc[i].id);
                    $(option).html(adduc[i].name);
                    $("#ddlAddDeduct").append(option);
                }

                //$('#ddlPerNonPer').append(new Option("none", -1))
                for (var i = 0; i < perNper.length; i++) {
                    var option = new Option(perNper[i].name, perNper[i].id);
                    $(option).html(perNper[i].name);
                    $("#ddlPerNonPer").append(option);
                }
            }
        }
    });
}
function getAllSalaryPolicyClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRSalaryPolicy/GetAllSalaryPolicyClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tSalaryPolicyListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tSalaryPolicyListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrspId${rowTableIdx}">` + items[i].hrspId + `</td>
                        <td id="hrspName${rowTableIdx}">` + items[i].hrspName + `</td>
                        <td id="adducName${rowTableIdx}">` + items[i].adducName + `</td>
                        <td id="perNPerName${rowTableIdx}">` + items[i].perNPerName + `</td>
                       <td id="hrspPercent${rowTableIdx}">` + items[i].hrspPercent + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrspId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrspId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tSalaryPolicyListbody').on('click', '.remove', function () {
    var hrspId = parseInt(this.id);
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
                        url: "/hrsalarypolicy/delete?id=" + hrspId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllSalaryPolicyClass();
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
$('#tSalaryPolicyListbody').on('click', '.edit', function () {

    var hrspId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/hrsalarypolicy/GetById?id=" + hrspId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrspId").val(response.hrspId);
                $("#hrspName").val(response.hrspName);
                $("#ddlAddDeduct").val(response.adduc);
                $("#ddlPerNonPer").val(response.perNPer);
                $("#srsppercent").val(response.hrspPercent);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
                $("#hrspName").attr("disabled", true);
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrspId").val(0);
    SaveOrUpdateRequest();
});

$("#hrspName").blur(function () {
    var hrspName = $("#hrspName").val();
    showErrorMessageBelowCtrl('hrspName', 'Salary Policy Name is required', false);
    if (hrspName == undefined || hrspName == '') {
        showErrorMessageBelowCtrl('hrspName', 'Salary Policy Name is required', true);
    } else {
        showErrorMessageBelowCtrl('hrspName', 'Salary Policy Name is required', false);
    }
});


function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('HRSPId', $('#hrspId').val());
    data.append('HRSPName', $('#hrspName').val());
    data.append('ADDUC', $('#ddlAddDeduct').find(":selected").val());
    data.append('PerNPer', $('#ddlPerNonPer').find(":selected").val());
    data.append('HRSPPercent', $('#srsppercent').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/hrsalarypolicy/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllSalaryPolicyClass();
                $("#hrspName").val(null);
                $("#ddlAddDeduct").val(0);
                $("#ddlPerNonPer").val(0);
                $("#srsppercent").val(null);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                $("#hrspName").attr("disabled", false);
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

function validationCheck() {

    var response = true;

    var hrspName = $("#hrspName").val();
    var srsppercent = $("#srsppercent").val();

    showErrorMessageBelowCtrl('hrspName', 'Salary Policy Name is required', false);
    showErrorMessageBelowCtrl('srsppercent', 'Percentage/NonPercentage  is required', false);


    if (hrspName == undefined || hrspName == '') {
        showErrorMessageBelowCtrl('hrspName', 'Salary Policy Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrspName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrspName', 'Salary Policy Name is required', false);
    }

    if (srsppercent == undefined || srsppercent == '') {
        showErrorMessageBelowCtrl('srsppercent', 'Percentage/NonPercentage is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#srsppercent").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('srsppercent', 'Percentage/NonPercentage is required', false);
    }


    return response;
}
$("#cancelButton").click(function () {
    $("#hrspId").val(0);
    $("#hrspName").val(null);
    $("#ddlAddDeduct").val(0);
    $("#ddlPerNonPer").val(0);
    $("#perNper").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#hrspName").attr("disabled", false);
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});