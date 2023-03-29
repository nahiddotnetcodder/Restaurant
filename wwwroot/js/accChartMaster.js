var rowTableIdx = 0;
var allBankAccount = [];
function getAllAccountDetails() {
    allBankAccount = [];
    $.ajax({
        type: "GET",
        url: "/AccChartMaster/GetAllAccountDetails",
        success: function (response) {
            if (response != null) {

                allBankAccount = response;
                $('#searchableBankAccount').empty();
                for (var i = 0; i < allBankAccount.length; i++) {
                    $('#searchableBankAccount').append(
                        `<tr id="Item${++rowTableIdx}">
                            <td class="text-center">
                                <a id="` + allBankAccount[i].acmId + `" style='color:blue;cursor:pointer' class="selectAccountItem">select</a>
                            </td>
                            <td class="text-center" id="acmAccCode${rowTableIdx}">` + allBankAccount[i].acmAccCode + `</td>
                            <td class="text-center" id="acmAccName${rowTableIdx}">` + allBankAccount[i].acmAccName + `</td>
                            <td class="text-center" id="actName${rowTableIdx}">` + allBankAccount[i].accChartTypes?.actName + `</td>
                        </tr>`
                    );
                }
            }
        }
    });
}
function getAllAccountIncludeInactiveDetails() {
    allBankAccount = [];
    $.ajax({
        type: "GET",
        url: "/AccChartMaster/GetAllAccountDetailsIncludeInactive",
        success: function (response) {
            if (response != null) {

                allBankAccount = response;
                $('#searchableBankAccount').empty();
                for (var i = 0; i < allBankAccount.length; i++) {
                    $('#searchableBankAccount').append(
                        `<tr id="Item${++rowTableIdx}">
                            <td class="text-center">
                                <a id="` + allBankAccount[i].acmId + `" style='color:blue;cursor:pointer' class="selectAccountItem">select</a>
                            </td>
                            <td class="text-center" id="acmAccCode${rowTableIdx}">` + allBankAccount[i].acmAccCode + `</td>
                            <td class="text-center" id="acmAccName${rowTableIdx}">` + allBankAccount[i].acmAccName + `</td>
                            <td class="text-center" id="actName${rowTableIdx}">` + allBankAccount[i].accChartTypes?.actName + `</td>
                        </tr>`
                    );
                }
            }
        }
    });
}
function getAllAccount() {
    $.ajax({
        type: "GET",
        url: "/AccChartMaster/GetAllAccount",
        success: function (response) {
            if (response != null) {
                var accounts = response.accountsDD;
                var subGroups = response.accChartTypesDD;
                $('#ddlAccounts').empty();
                $('#ddlAccounts').append(new Option("New account", -1))

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlAccounts").append(optgroup);
                }
            }
        }
    });
}
function getAllAccountIncludeInactive() {
    $.ajax({
        type: "GET",
        url: "/AccChartMaster/GetAllIncludeInactive",
        success: function (response) {
            if (response != null) {
                var accounts = response.accountsDD;
                var subGroups = response.accChartTypesDD;

                $('#ddlAccounts').empty();
                $('#ddlAccounts').append(new Option("New account", -1))
                
                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlAccounts").append(optgroup);
                }
            }
        }
    });
}
function getInitData() {
    $.ajax({
        type: "GET",
        url: "/AccChartMaster/GetInitData",
        success: function (response) {
            if (response != null) {

                var statusDD = response.statusDD;
                var accChartTypesDD = response.accChartTypesDD;

                for (var i = 0; i < statusDD.length; i++) {
                    var option = new Option(statusDD[i].name, statusDD[i].id);
                    $(option).html(statusDD[i].name);
                    $("#ddlStatuses").append(option);
                }

                for (var i = 0; i < accChartTypesDD.length; i++) {
                    var option = new Option(accChartTypesDD[i].name, accChartTypesDD[i].id);
                    $(option).html(accChartTypesDD[i].name);
                    $("#ddlAccChartTypes").append(option);
                }
            }
        }
    });
}
$("#addNewButton").click(function () {
    $("#acmId").val(0);
    SaveOrUpdateRequest();
});
function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('ACMId', $('#acmId').val());
    data.append('ACMAccCode', $('#acmAccCode').val());
    data.append('ACMAccName', $('#acmAccName').val());
    data.append('ACTId', $('#ddlAccChartTypes').find(":selected").val());
    data.append('ACMAI', $('#ddlStatuses').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccChartMaster/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllAccount();
                $("#acmId").val(0);
                $("#acmAccCode").val(null);
                $("#acmAccName").val(null);
                $("#switch-id").prop("checked", false);
                $("#ddlStatuses").val(0).change();
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                $("#acmAccCode").attr("disabled", false);
                alertify.notify('Saved Successfully!', 'success');
            }
            else {
                alertify.error('Error in save');
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
    var acmAccCode = $("#acmAccCode").val();
    var acmAccName = $("#acmAccName").val();
    var ddlAccChartTypes = parseInt($("#ddlAccChartTypes").children("option:selected").val());

    showErrorMessageBelowCtrl('acmAccCode', 'Account Code is required', false);
    showErrorMessageBelowCtrl('acmAccName', 'Account Name is required', false);
    showErrorMessageBelowCtrl('ddlAccChartTypes', 'Account Group is required', false);

    if (acmAccCode == undefined || acmAccCode == '') {
        showErrorMessageBelowCtrl('acmAccCode', 'Account Code is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#acmAccCode").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('acmAccCode', 'Account Code is required', false);
    }

    if (acmAccName == undefined || acmAccName == '') {
        showErrorMessageBelowCtrl('acmAccName', 'Account Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#acmAccName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('acmAccName', 'Account Name is required', false);
    }

    if (isNaN(ddlAccChartTypes) || ddlAccChartTypes <= 0) {
        showErrorMessageBelowCtrl('ddlAccChartTypes', 'Account Group is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlAccChartTypes").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlAccChartTypes', 'Account Group is required', false);
    }
    return response;
}
$("#acmAccCode").blur(function () {
    var acmAccCode = $("#acmAccCode").val();
    showErrorMessageBelowCtrl('acmAccCode', 'Account Group is required', false);
    if (acmAccCode == undefined || acmAccCode == '') {
        showErrorMessageBelowCtrl('acmAccCode', 'Account Group is required', true);
    } else {
        showErrorMessageBelowCtrl('acmAccCode', 'Account Group is required', false);
    }
});
$("#acmAccName").blur(function () {
    var acmAccName = $("#acmAccName").val();
    showErrorMessageBelowCtrl('acmAccName', 'Account Name is required', false);
    if (acmAccName == undefined || acmAccName == '') {
        showErrorMessageBelowCtrl('acmAccName', 'Account Name is required', true);
    } else {
        showErrorMessageBelowCtrl('acmAccName', 'Account Name is required', false);
    }
});
$("#ddlAccChartTypes").change(function () {
    showErrorMessageBelowCtrl('ddlAccChartTypes', 'Account Group is required', false);
    if (this.value > 0) {
        showErrorMessageBelowCtrl('ddlAccChartTypes', 'Account Group is required', false);
    } else {
        showErrorMessageBelowCtrl('ddlAccChartTypes', 'Account Group is required', true);
    }
});
$("#ddlAccounts").change(function () {
    if (this.value == -1) {
        $("#acmId").val(0);
        $("#acmAccCode").val(null);
        $("#acmAccName").val(null);
        $("#ddlAccounts").val(-1).change();
        $("#ddlAccChartTypes").val(0).change();
        $("#ddlStatuses").val(0).change();
        $("#addNewItem").show();
        $("#updateExistingItem").hide();
        $("#acmAccCode").attr("disabled", false);
    }
    else {
        $.ajax({
            type: "GET",
            url: "/AccChartMaster/GetById?id=" + this.value,
            success: function (response) {
                if (response != null) {
                    $("#acmId").val(response.acmId);
                    $("#acmAccCode").val(response.acmAccCode);
                    $("#acmAccCode").attr("disabled", true);
                    $("#acmAccName").val(response.acmAccName);
                    $("#ddlAccChartTypes").val(response.actId).change();
                    $("#ddlStatuses").val(response.acmai).change();
                    $("#addNewItem").hide();
                    $("#updateExistingItem").show();
                }
            }
        });
    }
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});
$("#cancelButton").click(function () {
    $("#acmId").val(0);
    $("#acmAccCode").val(null);
    $("#acmAccName").val(null);
    $("#switch-id").prop("checked", false);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#acmAccCode").attr("disabled", false);
    $("#ddlAccounts").val(-1);
    $("#ddlStatuses").val(0).change();
});
$("#switch-id").click(function () {
    if ($(this).is(":checked")) {
        getAllAccountIncludeInactive();
    } else {
        getAllAccount();
    }
});
$('#searchableBankAccount').on('click', '.selectAccountItem', function () {

    var acmId = parseInt(this.id); debugger
    var data = allBankAccount.filter(x => x.acmId == acmId)[0];
    $("#acmId").val(data.acmId);
    $("#acmAccCode").val(data.acmAccCode);
    $("#acmAccCode").attr("disabled", true);
    $("#acmAccName").val(data.acmAccName);
    $("#ddlAccChartTypes").val(data.actId).change();
    $("#ddlAccounts").val(acmId).change();
    $("#ddlStatuses").val(data.acmai).change();
    $("#addNewItem").hide();
    $("#updateExistingItem").show();
    $("#searchModal").modal('hide');
});
$("#searchModalButton").click(function () {
    if ($("#switch-id").is(":checked")) {
        getAllAccountIncludeInactiveDetails();
    } else {
        getAllAccountDetails();
    }
});