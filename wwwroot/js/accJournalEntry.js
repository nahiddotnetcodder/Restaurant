var rowTableIdx = 0;
var rowJournalTableIdx = 0;
var allAccounts = [];
var itemList = [];
var fiscalBeginYear;
var fiscalEndYear;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
$("#agtDebitAccount").keypress(function (e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
$("#agtCreditAccount").keypress(function (e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});
function getInitData() {
    $.ajax({
        type: "GET",
        url: "/AccJournalEntry/GetInitData",
        success: function (response) {
            if (response != null) {
                fiscalBeginYear = response.fiscalBeginYear;
                fiscalEndYear = response.fiscalEndYear;
                var refNo = response.refNo;
                var refPrefixNo = response.refPrefixNo;
                $("#ajRef").val(refNo);
                $("#ajRefPrefix").val(refPrefixNo);
                var accounts = response.chartMasterDD;
                allAccounts = response.chartMasterDD;
                var subGroups = response.chartTypeDD;
                $('#ddlAGTAccDescription').empty();
                $('#ddlAGTAccDescription').append(new Option("New account", -1))

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlAGTAccDescription").append(optgroup);
                }
            }
        }
    });
}
function getDescriptionDD() {
    $.ajax({
        type: "GET",
        url: "/AccJournalEntry/GetInitData",
        success: function (response) {
            if (response != null) {

                fiscalBeginYear = response.fiscalBeginYear;
                fiscalEndYear = response.fiscalEndYear;
                var accounts = response.chartMasterDD;
                allAccounts = response.chartMasterDD;
                var subGroups = response.chartTypeDD;
                $('#ddlAGTAccDescription').empty();
                $('#ddlAGTAccDescription').append(new Option("New account", -1))

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlAGTAccDescription").append(optgroup);
                }
            }
        }
    });
}
function addItem() {

    var result = validationCheckForItem();
    if (result == false) { return; }

    var descriptionId = $("#ddlAGTAccDescription option:selected").val();
    var selectedAccount = allAccounts.filter(x => x.id == Number(descriptionId))[0];
    let objectItem = {
        agtAccCode: $("#agtAccCode").val(),
        agtAccDescriptionId: descriptionId,
        agtAccDescriptionText: selectedAccount.description,
        agtMemo: $("#agtMemo").val(),
        agtDebitAccount: $('#agtDebitAccount').val(),
        agtCreditAccount: $('#agtCreditAccount').val()
    };

    $('#tGLPostingListbody').append(
        `<tr id="Item${++rowTableIdx}">
            <td hidden id="agtId${rowTableIdx}">` + 0 + `</td>
            <td id="agtAccCode${rowTableIdx}">` + objectItem.agtAccCode + `</td>
            <td hidden id="agtAccDescriptionId${rowTableIdx}">` + objectItem.agtAccDescriptionId + `</td>
            <td id="agtAccDescription${rowTableIdx}">` + objectItem.agtAccDescriptionText + `</td>
            <td id="agtDebitAccount${rowTableIdx}">` + objectItem.agtDebitAccount + `</td>
            <td id="agtCreditAccount${rowTableIdx}">` + objectItem.agtCreditAccount + `</td>
            <td id="agtMemo${rowTableIdx}">` + objectItem.agtMemo + `</td>
            <td class="text-center">
                <button class="btn btn-sm btn-danger remove" type="button"><i class="fa fa-trash" aria-hidden="true"></i></button>
            </td>
        </tr>`
    );
    resetFrom();
}
function validationCheckForItem() {

    var response = true;

    var agtAccCode = $("#agtAccCode").val();
    var ddlAGTAccDescription = parseInt($("#ddlAGTAccDescription option:selected").val());
    var agtMemo = $("#agtMemo").val();
    var agtDebitAccount = $('#agtDebitAccount').val();
    var agtCreditAccount = $('#agtCreditAccount').val();

    showErrorMessageBelowCtrl('agtAccCode', 'Account Code is required', false);
    showErrorMessageBelowCtrl('ddlAGTAccDescription', 'Account Description is required', false);
    showErrorMessageBelowCtrl('agtMemo', 'Memo is required', false);

    if (agtAccCode == undefined || agtAccCode.length <= 0) {
        showErrorMessageBelowCtrl('agtAccCode', 'Account Code is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#agtAccCode").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('agtAccCode', 'Account Code is required', false);
    }
    if (isNaN(ddlAGTAccDescription) || ddlAGTAccDescription <= 0) {
        showErrorMessageBelowCtrl('ddlAGTAccDescription', 'Account Description is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlAGTAccDescription").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlAGTAccDescription', 'Account Description is required', false);
    }
    if (agtMemo == undefined || agtMemo.length <= 0) {
        showErrorMessageBelowCtrl('agtMemo', 'Memo is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#agtMemo").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('agtMemo', 'Memo is required', false);
    }

    if (Number(agtDebitAccount) > 0 && Number(agtCreditAccount) > 0) {
        alertify.error('Only Debit or Credit allowed at a time.');
        response = false;
    }

    if (Number(agtDebitAccount) <= 0 && Number(agtCreditAccount) <= 0) {
        alertify.error('Please Provide a debit or credit amount.');
        response = false;
    }
    return response;
}
function resetFrom() {
    $('#agtAccCode').val("");
    $("#ddlAGTAccDescription").val(-1).change();
    $('#agtMemo').val("");
    $('#agtDebitAccount').val("");
    $('#agtCreditAccount').val("");
}
$('#tGLPostingListbody').on('click', '.remove', function () {
    var child = $(this).closest('tr').nextAll();
    child.each(function () {
        var id = $(this).attr('id');
        var idx = $(this).children('.row-index').children('p');
        var dig = parseInt(id.substring(1));
        idx.html(`Row ${dig - 1}`);
        $(this).attr('id', `Item${dig - 1}`);
    });
    $(this).closest('tr').remove();
    rowTableIdx--;
});
$("#agtAccCode").change(function () {
    showErrorMessageBelowCtrl('agtAccCode', 'Account Code is required', false);
    if (this.value.length > 0) {
        var account = allAccounts.filter(x => x.code == this.value)[0];
        $('#ddlAGTAccDescription').val(account?.id);
        showErrorMessageBelowCtrl('agtAccCode', 'Account Code is required', false);
    } else {
        showErrorMessageBelowCtrl('agtAccCode', 'Account Code is required', true);
    }
});
$("#ddlAGTAccDescription").change(function () {
    showErrorMessageBelowCtrl('ddlAGTAccDescription', 'Account Description is required', false);
    if (this.value > 0) {
        var account = allAccounts.filter(x => x.id == this.value)[0];
        $('#agtAccCode').val(account.code);
        showErrorMessageBelowCtrl('ddlAGTAccDescription', 'Account Description is required', false);
    }
    else {
        $('#agtAccCode').val('');
    }
});
$("#agtMemo").change(function () {
    showErrorMessageBelowCtrl('agtMemo', 'Memo Description is required', false);
    if (this.value.length > 0) {
        showErrorMessageBelowCtrl('agtMemo', 'Memo Description is required', false);
    } else {
        showErrorMessageBelowCtrl('agtMemo', 'Memo is required', true);
    }
});

$("#ajTrDate").change(function () {
    var inputDate = new Date(this.value);
    var todaysDate = new Date();
    if (inputDate.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
        $("#ajTrDate").css("color", "black");
    }
    else {
        $("#ajTrDate").css("color", "red");
    }
});
$("#ajDDate").change(function () {
    var inputDate = new Date(this.value);
    var todaysDate = new Date();
    if (inputDate.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
        $("#ajDDate").css("color", "black");
    }
    else {
        $("#ajDDate").css("color", "red");
    }
});
$("#ajEDate").change(function () {
    var inputDate = new Date(this.value);
    var todaysDate = new Date();
    if (inputDate.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
        $("#ajEDate").css("color", "black");
    }
    else {
        $("#ajEDate").css("color", "red");
    }
});
function SaveRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('Id', 0);
    data.append('AJTrDate', $('#ajTrDate').val());
    data.append('AJDDate', $('#ajDDate').val());
    data.append('AJEDate', $('#ajEDate').val());
    data.append('AJSoRef', $('#ajSoRef').val());
    data.append('AJRefPrefix', $('#ajRefPrefix').val());
    data.append('AJRef', $('#ajRef').val());
    data.append('AJMemo', $('#ajMemo').val());

    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 1; i <= tablelength; i++) {
        itemList.push({
            agtId: $('#agtId' + i).text(),
            agtAccCode: $('#agtAccCode' + i).text(),
            agtAccDescription: $('#agtAccDescription' + i).text(),
            agtDebitAccount: $('#agtDebitAccount' + i).text() ?? 0,
            agtCreditAccount: $('#agtCreditAccount' + i).text() ?? 0,
            agtMemo: $('#agtMemo' + i).text()
        });
    }
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccJournalEntry/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Saved Successfully!', 'success', 1, function () { window.location = "/AccJournalEntry/Journals"; });
            }
            else {
                alertify.error(response.message);
            }
        },
        complete: function () {
            console.log("complete");
        },
        failure: function (response) {
            alertify.error('Something went wrong! please try again.');
        },
        error: function (response) {
            alertify.error('Something went wrong! please try again.');
        }
    });
}
function validationCheck() {

    var response = true;

    var ajTrDate = $("#ajTrDate").val();
    var ajDDate = $("#ajDDate").val();
    var ajEDate = $("#ajEDate").val();
    var ajSoRef = $("#ajSoRef").val();
    var ajMemo = $("#ajMemo").val();

    if (!(ajTrDate >= fiscalBeginYear && ajTrDate <= fiscalEndYear)) {
        alertify.error('Journal date must be between Fiscal Begin and End date.');
        response = false;
        return response;
    }
    if (!(ajDDate >= fiscalBeginYear && ajDDate <= fiscalEndYear)) {
        alertify.error('Document date must be between Fiscal Begin and End date.');
        response = false;
        return response;
    }
    if (!(ajEDate >= fiscalBeginYear && ajEDate <= fiscalEndYear)) {
        alertify.error('Event date must be between Fiscal Begin and End date.');
        response = false;
        return response;
    }

    showErrorMessageBelowCtrl('ajTrDate', 'Journal Date is required', false);
    showErrorMessageBelowCtrl('ajDDate', 'Document Date is required', false);
    showErrorMessageBelowCtrl('ajEDate', 'Event Date is required', false);
    showErrorMessageBelowCtrl('ajSoRef', 'Source Ref is required', false);
    showErrorMessageBelowCtrl('ajMemo', 'Memo is required', false);

    if (ajTrDate == undefined || ajTrDate == '') {
        showErrorMessageBelowCtrl('ajTrDate', 'Journal Date is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ajTrDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ajTrDate', 'Journal Date is required', false);
    }

    if (ajDDate == undefined || ajDDate == '') {
        showErrorMessageBelowCtrl('ajDDate', 'Document Date is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ajDDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ajDDate', 'Document Date is required', false);
    }

    if (ajEDate == undefined || ajEDate == '') {
        showErrorMessageBelowCtrl('ajEDate', 'Event Date is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ajEDate").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ajEDate', 'Event Date is required', false);
    }

    if (ajSoRef == undefined || ajSoRef == '') {
        showErrorMessageBelowCtrl('ajSoRef', 'Source Ref is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ajSoRef").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ajSoRef', 'Source Ref is required', false);
    }

    if (ajMemo == undefined || ajMemo == '') {
        showErrorMessageBelowCtrl('ajMemo', 'Memo is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ajMemo").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ajMemo', 'Memo is required', false);
    }

    var tablelength = $('#tGLPostingListbody tr').length;
    var totalDebitAmount = 0;
    var totalCreditAmount = 0;
    for (var i = 1; i <= tablelength; i++) {
        totalDebitAmount += Number($('#agtDebitAccount' + i).text() ?? 0);
        totalCreditAmount += Number($('#agtCreditAccount' + i).text() ?? 0);
    }
    if (totalDebitAmount != totalCreditAmount) {
        alertify.error("Debit and Credit amount must be same");
        response = false;
    }

    return response;
}
$("#addNewButton").click(function () {
    SaveRequest();
});
function getAllJournalEntry() {
    rowJournalTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/AccJournalEntry/GetAll",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tJournalListbody').empty();
            for (var i = 0; i < items.length; i++) {

                $('#tJournalListbody').append(
                    `<tr id="Item${++rowJournalTableIdx}">
                        <td hidden id="ajId${rowJournalTableIdx}">` + items[i].ajId + `</td>
                        <td id="ajTrDateString${rowJournalTableIdx}">` + items[i].ajTrDateString + `</td>
                        <td id="journalType${rowJournalTableIdx}">` + items[i].journalType + `</td>
                        <td id="ajId${rowJournalTableIdx}">` + items[i].ajId + `</td>
                        <td id="ajRef${rowJournalTableIdx}">` + items[i].ajRef + `</td>
                        <td id="amountString${rowJournalTableIdx}">` + items[i].amountString + `</td>
                        <td id="ajMemo${rowJournalTableIdx}">` + items[i].ajMemo + `</td>
                        <td id="cUser${rowJournalTableIdx}">` + items[i].cUser + `</td>
                        <td class="text-center">
                            <button id="` + items[i].ajId + `" class="btn btn-sm btn-primary view" type="button">View</button>
                        </td>
                        <td class="text-center">
                            <a href='/AccJournalEntry/Edit?id=` + items[i].ajId + `'>Edit</a>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
function setCurrentDate() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $('#ajTrDate').val(today);
    $('#ajDDate').val(today);
    $('#ajEDate').val(today);
}
$('#tJournalListbody').on('click', '.view', function () {
    window.location = '/AccJournalEntry/JournalDetails?id=' + $(this)[0].id;
});
$('#tJournalListbody').on('click', '.edit', function () {
    window.location = '/AccJournalEntry/Edit?id=' + $(this)[0].id;
});
$('.print').click(function () {
    $("#nonPrintArea").hide();
    $("#viewJournalModal").show();
    window.print();
});
function getJournalForEdit() {
    var url = window.location.href;
    var id = url.substring(url.lastIndexOf('=') + 1);

    $.ajax({
        type: "GET",
        url: "/AccJournalEntry/GetById?id=" + id,
        data: "{}",
        success: function (data) {
            let items = data.items;
            $('#tGLPostingListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tGLPostingListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="agtId${rowTableIdx}">` + items[i].agtId + `</td>
                        <td id="agtAccCode${rowTableIdx}">` + items[i].agtAccCode + `</td>
                        <td id="agtAccDescription${rowTableIdx}">` + items[i].agtAccDescription + `</td>
                        <td id="agtDebitAccount${rowTableIdx}">` + items[i].agtDebitAccount + `</td>
                        <td id="agtCreditAccount${rowTableIdx}">` + items[i].agtCreditAccount + `</td>
                        <td id="agtMemo${rowTableIdx}">` + items[i].agtMemo + `</td>
                        <td class="text-center">
                            <button id="` + items[i].agtId + `" class="btn btn-sm btn-danger remove" type="button"><i class="fa fa-trash" aria-hidden="true"></i></button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#updateButton').click(function () {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('AJId', $('#ajId').val());
    data.append('AJTrDate', $('#ajTrDate').val());
    data.append('AJDDate', $('#ajDDate').val());
    data.append('AJEDate', $('#ajEDate').val());
    data.append('AJSoRef', $('#ajSoRef').val());
    data.append('AJRefPrefix', $('#ajRefPrefix').val());
    data.append('AJRef', $('#ajRef').val());
    data.append('AJMemo', $('#ajMemo').val());

    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 1; i <= tablelength; i++) {
        itemList.push({
            agtId: $('#agtId' + i).text(),
            agtAccCode: $('#agtAccCode' + i).text(),
            agtAccDescription: $('#agtAccDescription' + i).text(),
            agtDebitAccount: $('#agtDebitAccount' + i).text() ?? 0,
            agtCreditAccount: $('#agtCreditAccount' + i).text() ?? 0,
            agtMemo: $('#agtMemo' + i).text()
        });
    }
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/AccJournalEntry/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Updated Successfully!', 'success', 1, function () { window.location = "/AccJournalEntry/Journals"; });
            }
            else {
                alertify.error(response.message);
            }
        },
        complete: function () {
            console.log("complete");
        },
        failure: function (response) {
            alertify.error('Something went wrong! please try again.');
        },
        error: function (response) {
            alertify.error('Something went wrong! please try again.');
        }
    });
});