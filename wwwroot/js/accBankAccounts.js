var rowTableIdx = 0;
var rowJournalTableIdx = 0;
var allAccounts = [];
var itemList = [];

$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/AccBankAccounts/GetInitData",
        success: function (response) {
            if (response != null) {
                var accType = response.accType;
                var accounts = response.chartMasterDD;
                allAccounts = response.chartMasterDD;
                var subGroups = response.chartTypeDD;
                $('#ddlAccCode').empty();
                $('#ddlAccCode').append(new Option("New account", -1))

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlAccCode").append(optgroup);
                }

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlAccCodes").append(optgroup);
                }

                for (var i = 0; i < accType.length; i++) {
                    var option = new Option(accType[i].name, accType[i].id);
                    $(option).html(accType[i].name);
                    $("#ddlAccType").append(option);
                }
            }
        }
    });
}

function getAllClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/AccBankAccounts/GetAll",
        data: "{}",
        success: function (data) {
            let items = data;
            debugger
            $('#tBankAccountListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tBankAccountListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="abaId${rowTableIdx}">` + items[i].abaId + `</td>
                        <td id="ababaName${rowTableIdx}">` + items[i].ababaName + `</td>
                        <td id="abaaTypeName${rowTableIdx}">` + items[i].abaaTypeName + `</td>
                        <td id="ababcCode${rowTableIdx}">` + items[i].ababcCode + `</td>
                        <td id="acmAccCode${rowTableIdx}">` + items[i].acmAccCode + `</td>
                        <td id="ababName${rowTableIdx}">` + items[i].ababName + `</td>
                        <td id="ababaNumber${rowTableIdx}">` + items[i].ababaNumber + `</td>
                        <td id="ababAdd${rowTableIdx}">` + items[i].ababAdd + `</td>
                        <td class="text-center">
                            <button id="` + items[i].abaId + `" class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                            <button id="` + items[i].abaId + `" class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}

$('#tBankAccountListbody').on('click', '.remove', function () {

    var abaId = parseInt(this.id);
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
                        url: "/AccBankAccounts/DeleteItemById?id=" + abaId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllClass();
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

$('#tBankAccountListbody').on('click', '.edit', function () {

    var abaId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/AccBankAccounts/GetById?id=" + abaId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#abaId").val(response.abaId);
                $("#ababaName").val(response.ababaName);
                $("#ddlAccType").val(response.abaaType);
                $("#ddlAccCodes").val(response.ababcCode);
                $("#ddlAccCode").val(response.acmAccCode);
                $("#ababName").val(response.ababName);
                $("#ababaNumber").val(response.ababaNumber);
                $("#ababAdd").val(response.ababAdd);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});

$("#addNewButton").click(function () {
    $("#abaId").val(0);
    SaveRequest();
});


$("#ababaName").blur(function () {
    var ababaName = $("#ababaName").val();
    showErrorMessageBelowCtrl('ababaName', 'Bank Account Name is required', false);
    if (ababaName == undefined || ababaName == '') {
        showErrorMessageBelowCtrl('ababaName', 'Bank Account Name is required', true);
    } else {
        showErrorMessageBelowCtrl('ababaName', 'Bank Account Name is required', false);
    }
});

function SaveRequest() {
    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();
    data.append('ABAId', $('#abaId').val());
    data.append('ABABAName', $('#ababaName').val());
    data.append('ABAAType', $('#ddlAccType').find(":selected").val());
    data.append('ABABCCode', $('#ddlAccCodes').val());
    data.append('ACMAccCode', $('#ddlAccCode').val());
    data.append('ABABName', $('#ababName').val());
    data.append('ABABANumber', $('#ababaNumber').val());
    data.append('ABABAdd', $('#ababAdd').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccBankAccounts/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllClass();
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

    var data = new FormData();
    data.append('ABAId', $('#abaId').val());
    data.append('ABABAName', $('#ababaName').val());
    data.append('ABAAType', $('#ddlAccType').find(":selected").val());
    data.append('ABABCCode', $('#ddlAccCodes').val());
    data.append('ACMAccCode', $('#ddlAccCode').val());
    data.append('ABABName', $('#ababName').val());
    data.append('ABABANumber', $('#ababaNumber').val());
    data.append('ABABAdd', $('#ababAdd').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccBankAccounts/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllClass();
                restValue();
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
    var ababaName = $("#ababaName").val();
    var ddlAccCodes = parseInt($("#ddlAccCodes").children("option:selected").val());
    var ddlAccCode = parseInt($("#ddlAccCode").children("option:selected").val());
    var ababName = $("#ababName").val();
    var ababName = $("#ababaNumber").val();

    showErrorMessageBelowCtrl('ababaName', 'Bank Account Name is required', false);
    showErrorMessageBelowCtrl('ddlAccCodes', 'Bank Code is required', false);
    showErrorMessageBelowCtrl('ddlAccCode', 'Bank Account Code is required', false);
    showErrorMessageBelowCtrl('ababName', 'Bank Name is required', false);
    showErrorMessageBelowCtrl('ababaNumber', 'Bank Account Number is required', false);

    if (ababaNumber == undefined || ababaNumber == '') {
        showErrorMessageBelowCtrl('ababaNumber', 'Bank Account Number is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ababaNumber").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ababaNumber', 'Bank Account Number is required', false);
    }

    if (ababName == undefined || ababName == '') {
        showErrorMessageBelowCtrl('ababName', 'Bank  Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ababName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ababName', 'Bank  Name is required', false);
    }

    if (ababaName == undefined || ababaName == '') {
        showErrorMessageBelowCtrl('ababaName', 'Bank  Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ababaName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ababaName', 'Bank Account Name is required', false);
    }

    if (ddlAccCodes == undefined || ddlAccCodes == '') {
        showErrorMessageBelowCtrl('ddlAccCodes', 'Bank Code is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlAccCodes").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlAccCodes', 'Bank Code is required', false);
    }

    if (ddlAccCode == undefined || ddlAccCode == '') {
        showErrorMessageBelowCtrl('ddlAccCode', 'Bank Account Code is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlAccCode").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlAccCode', 'Bank Account Code is required', false);
    }

    return response;
}


function setCurrentDate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $('#abalrDate').val(today);

}

$("#cancelButton").click(function () {
    restValue()
});
$("#updateButton").click(function () {
    UpdateRequest();
});

function restValue() {
    $("#abaId").val(0);
    $("#ababaName").val(null);
    $("#ddlAccType").val(0);
    $("#ddlAccType").val(-1);
    $("#ddlAccCodes").val(0);
    $("#ddlAccCode").val(0);
    $("#ababName").val(null);
    $("#ababaNumber").val(null);
    $("#ababAdd").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
}
