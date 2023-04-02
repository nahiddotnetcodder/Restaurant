var rowIdx = 0;
var rowTableIdx = 0;
var rowJournalTableIdx = 0;
var allAccounts = [];
var itemList = [];
var fiscalBeginYear;
var fiscalEndYear;
var accCode = '';
var accDesc = 0;
var debit = 0;
var credit = 0;
var memo = '';
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
    for (var i = 0; i < tablelength - 1; i++) {
        itemList.push({
            agtId: Number($('#agtId' + i).val()) ?? 0,
            agtAccCode: $('#agtAccCode' + i).val(),
            agtAccDescriptionId: $('#ddlAGTAccDescription' + i).find(":selected").val(),
            agtAccDescription: $('#ddlAGTAccDescription' + i).find(":selected").text(),
            agtDebitAccount: $('#agtDebitAccount' + i).val() ?? 0,
            agtCreditAccount: $('#agtCreditAccount' + i).val() ?? 0,
            agtMemo: $('#agtMemo' + i).val()
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
    for (var i = 0; i < tablelength - 1; i++) {
        totalDebitAmount += Number($('#agtDebitAccount' + i).val() ?? 0);
        totalCreditAmount += Number($('#agtCreditAccount' + i).val() ?? 0);
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
            allAccounts = data.allAccounts;
            let items = data.items;
            rowIdx = 0;
            $('#tGLPostingListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tGLPostingListbody').append(`
                <tr id="R${rowIdx}">
                    <td hidden>
                        <input type="text" class="form-control-sm" id="agtId${rowIdx}" />
                    </td>
                    <td>
                        <input type="text" class="form-control-sm" id="agtAccCode${rowIdx}" />
                    </td>
                    <td>
                        <select class="form-control-sm accountChangesFromRow autoSuggestionSelect" id="ddlAGTAccDescription${rowIdx}"><option selected>` + items[i].agtAccDescriptionId + `</option></select>
                    </td>
                    <td>
                        <input type="text" class="form-control-sm numbersOnly" placeholder="0.00" id="agtDebitAccount${rowIdx}" />
                    </td>
                    <td>
                        <input type="text" class="form-control-sm numbersOnly" placeholder="0.00" id="agtCreditAccount${rowIdx}" />
                    </td>
                    <td>
                        <input type="text" class="form-control-sm" id="agtMemo${rowIdx}" />
                    </td>
                    <td class="text-center">
                        <div id="addItem${rowIdx}" class="row">
                            <a style="cursor:pointer" class="btn btn-primary add"><i class="bi bi-plus-circle"></i>add</a>
                        </div>
                        <div id="updateItem${rowIdx}" class="row">
                            <button style='margin-left:2px' class="btn btn-primary edit" type="button"><i class="fa fa-edit" aria-hidden="true"></i>upd</button>
                            <button style='margin-left:2px' class="btn btn-danger remove" type="button"><i class="fa fa-trash" aria-hidden="true"></i>del</button>
                        </div>
                        <div id="saveAsUpdatedItem${rowIdx}" class="row">
                            <button style='margin-left:2px' class="btn btn-primary sav" type="button"><i class="fa fa-edit" aria-hidden="true"></i>sav</button>
                            <button style='margin-left:2px' class="btn btn-danger can" type="button"><i class="fa fa-trash" aria-hidden="true"></i>can</button>
                        </div>
                    </td>
                </tr>`);
                getAllAccountFromDB(rowIdx);
                //$('.autoSuggestionSelect').css('width', '100%');
                //$(".autoSuggestionSelect").select2({});

                $("#agtId" + rowIdx).val(items[i].agtId);
                $("#agtAccCode" + rowIdx).val(items[i].agtAccCode);
                $("#ddlAGTAccDescription" + rowIdx).val(items[i].agtAccDescriptionId);
                $("#ddlAGTAccDescription" + rowIdx).change();
                $("#agtDebitAccount" + rowIdx).val(items[i].agtDebitAccount);
                $("#agtCreditAccount" + rowIdx).val(items[i].agtCreditAccount);
                $("#agtMemo" + rowIdx).val(items[i].agtMemo);

                $("#agtAccCode" + rowIdx).prop('disabled', true);
                $("#ddlAGTAccDescription" + rowIdx).prop('disabled', true);
                $("#agtDebitAccount" + rowIdx).prop('disabled', true);
                $("#agtCreditAccount" + rowIdx).prop('disabled', true);
                $("#agtMemo" + rowIdx).prop('disabled', true);

                $("#addItem" + rowIdx).hide();
                $("#updateItem" + rowIdx).show();
                $("#saveAsUpdatedItem" + rowIdx).hide();

                rowIdx += 1;
            }
            addFirstRow(rowIdx);
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
    for (var i = 0; i < tablelength - 1; i++) {
        itemList.push({
            agtId: Number($('#agtId' + i).val()) ?? 0,
            agtAccCode: $('#agtAccCode' + i).val(),
            agtAccDescriptionId: $('#ddlAGTAccDescription' + i).find(":selected").val(),
            agtAccDescription: $('#ddlAGTAccDescription' + i).find(":selected").text(),
            agtDebitAccount: $('#agtDebitAccount' + i).val() ?? 0,
            agtCreditAccount: $('#agtCreditAccount' + i).val() ?? 0,
            agtMemo: $('#agtMemo' + i).val()
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
$("#ajTrDate").change(function (e) {

    var inputDate = new Date($('#ajTrDate').val());
    var todaysDate = new Date();

    if (inputDate.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
        $("#ajTrDate").css("color", "black");
    } else {
        $("#ajTrDate").css("color", "red");
    }
});
$("#ajDDate").change(function (e) {

    var inputDate = new Date($('#ajDDate').val());
    var todaysDate = new Date();

    if (inputDate.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
        $("#ajDDate").css("color", "black");
    } else {
        $("#ajDDate").css("color", "red");
    }
});
$("#ajEDate").change(function (e) {

    var inputDate = new Date($('#ajEDate').val());
    var todaysDate = new Date();

    if (inputDate.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
        $("#ajEDate").css("color", "black");
    } else {
        $("#ajEDate").css("color", "red");
    }
});

function getAllAccountFromDB(parameter) {
    $.ajax({
        type: "GET",
        url: "/AccJournalEntry/GetAllAccount",
        data: "{}",
        success: function (response) {
            var accounts = response.chartMasterDD;
            var subGroups = response.chartTypeDD; debugger

            var o = new Option("Select Account", "-1");
            $(o).html("Please Select Account");
            $("#ddlAGTAccDescription" + parameter).append(o);

            for (var i = 0; i < subGroups.length; i++) {
                var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                for (var j = 0; j < data.length; j++) {
                    optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                }
                optgroup += "</optgroup>";
                $("#ddlAGTAccDescription" + parameter).append(optgroup);
            }
        }
    });
}
$('#tGLPostingListbody').on('change',
    '.accountChangesFromRow',
    function () {
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(20);
        var value = $("#ddlAGTAccDescription" + rowTrackId).val();
        if (value != null) {
            var account = allAccounts.filter(x => x.id == value)[0];

            if (account != undefined) {
                $('#agtAccCode' + rowTrackId).val(account.code);
            }
            else {
                $('#agtAccCode' + rowTrackId).val();
            }
        }
    });
$('#tGLPostingListbody').on('change',
    '.codeChangesFromRow',
    function () {
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(10);
        var value = $("#agtAccCode" + rowTrackId).val();
        if (value != null) {
            var account = allAccounts.filter(x => x.code == value)[0];
            if (account != undefined) {
                $('#ddlAGTAccDescription' + rowTrackId).val(account.id);
                $('#ddlAGTAccDescription' + rowTrackId).change();
            }
            else {
                $('#ddlAGTAccDescription' + rowTrackId).val(-1);
                $('#ddlAGTAccDescription' + rowTrackId).change();
            }
        }
    });
$('#tGLPostingListbody').on('click', '.add', function () {

    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));
    var agtDebitAccount = $("#agtDebitAccount" + rowIdx).val();
    var agtCreditAccount = $("#agtCreditAccount" + rowIdx).val();
    if (Number(agtDebitAccount) > 0 && Number(agtCreditAccount) > 0) {
        alertify.error('Only Debit or Credit allowed at a time.');
        response = false;
    }
    else if (Number(agtDebitAccount) <= 0 && Number(agtCreditAccount) <= 0) {
        alertify.error('Please Provide a debit or credit amount.');
        response = false;
    }
    else {
        rowIdx += 1;
        $('#tGLPostingListbody').append(`
            <tr id="R${rowIdx}">
                <td hidden>
                    <input type="text" class="form-control" id="agtId${rowIdx}" />
                </td>
                <td>
                    <input type="text" class="form-control codeChangesFromRow" id="agtAccCode${rowIdx}" />
                </td>
                <td>
                    <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlAGTAccDescription${rowIdx}"></select>
                </td>
                <td>
                    <input type="text" class="form-control numbersOnly" placeholder="0.00" id="agtDebitAccount${rowIdx}" />
                </td>
                <td>
                    <input type="text" class="form-control numbersOnly" placeholder="0.00" id="agtCreditAccount${rowIdx}" />
                </td>
                <td>
                    <input type="text" class="form-control" id="agtMemo${rowIdx}" />
                </td>
                <td class="text-center">
                    <div id="addItem${rowIdx}" class="row">
                        <a style="cursor:pointer" class="btn btn-primary add"><i class="bi bi-plus-circle"></i>add</a>
                    </div>
                    <div id="updateItem${rowIdx}" class="row">
                        <button style='margin-left:2px' class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                        <button style='margin-left:2px' class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                    </div>
                    <div id="saveAsUpdatedItem${rowIdx}" class="row">
                       <button style='margin-left:2px' class="btn btn-sm  sav" type="button"><i class="fa-solid fa-check text-success" aria-hidden="true" title="save"></i></button>
                       <button style='margin-left:2px' class="btn btn-sm  can" type="button"><i class="fa-solid fa-xmark text-success" aria-hidden="true" title="cancel"></i></button>
                    </div>
                </td>
            </tr>`);
        getAllAccountFromDB(rowIdx);
       // $('.autoSuggestionSelect').css('width', '100%');
       // $(".autoSuggestionSelect").select2({});
        $("#updateItem" + rowIdx).hide();
        $("#saveAsUpdatedItem" + rowIdx).hide();
        var prevRowIdx = rowIdx - 1;
        $("#addItem" + prevRowIdx).hide();
        $("#agtAccCode" + prevRowIdx).prop('disabled', true);
        $("#ddlAGTAccDescription" + prevRowIdx).prop('disabled', true);
        $("#agtDebitAccount" + prevRowIdx).prop('disabled', true);
        $("#agtCreditAccount" + prevRowIdx).prop('disabled', true);
        $("#agtMemo" + prevRowIdx).prop('disabled', true);
        $("#updateItem" + prevRowIdx).show();
        $("#saveAsUpdatedItem" + prevRowIdx).hide();
    }
});
$('#tGLPostingListbody').on('click', '.edit', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    accCode = $("#agtAccCode" + rowIdx).val();
    accDesc = $("#ddlAGTAccDescription" + rowIdx).val();
    debit = $("#agtDebitAccount" + rowIdx).val();
    credit = $("#agtCreditAccount" + rowIdx).val();
    memo = $("#agtMemo" + rowIdx).val();

    $("#agtAccCode" + rowIdx).prop('disabled', false);
    $("#ddlAGTAccDescription" + rowIdx).prop('disabled', false);
    $("#agtDebitAccount" + rowIdx).prop('disabled', false);
    $("#agtCreditAccount" + rowIdx).prop('disabled', false);
    $("#agtMemo" + rowIdx).prop('disabled', false);
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).show();
});
$('#tGLPostingListbody').on('click', '.remove', function () {
    $(this).closest('tr').remove();
});
$('#tGLPostingListbody').on('click', '.sav', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    var agtDebitAccount = $("#agtDebitAccount" + rowIdx).val();
    var agtCreditAccount = $("#agtCreditAccount" + rowIdx).val();
    if (Number(agtDebitAccount) > 0 && Number(agtCreditAccount) > 0) {
        alertify.error('Only Debit or Credit allowed at a time.');
        response = false;
    }
    else if (Number(agtDebitAccount) <= 0 && Number(agtCreditAccount) <= 0) {
        alertify.error('Please Provide a debit or credit amount.');
        response = false;
    }
    else {
        $("#agtAccCode" + rowIdx).prop('disabled', true);
        $("#ddlAGTAccDescription" + rowIdx).prop('disabled', true);
        $("#agtDebitAccount" + rowIdx).prop('disabled', true);
        $("#agtCreditAccount" + rowIdx).prop('disabled', true);
        $("#agtMemo" + rowIdx).prop('disabled', true);
        $("#updateItem" + rowIdx).show();
        $("#saveAsUpdatedItem" + rowIdx).hide();
    }
});
$('#tGLPostingListbody').on('click', '.can', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    $("#agtAccCode" + rowIdx).val(accCode);
    $("#ddlAGTAccDescription" + rowIdx).val(accDesc);
    $("#ddlAGTAccDescription" + rowIdx).change();
    $("#agtDebitAccount" + rowIdx).val(debit);
    $("#agtCreditAccount" + rowIdx).val(credit);
    $("#agtMemo" + rowIdx).val(memo);

    $("#agtAccCode" + rowIdx).prop('disabled', true);
    $("#ddlAGTAccDescription" + rowIdx).prop('disabled', true);
    $("#agtDebitAccount" + rowIdx).prop('disabled', true);
    $("#agtCreditAccount" + rowIdx).prop('disabled', true);
    $("#agtMemo" + rowIdx).prop('disabled', true);
    $("#updateItem" + rowIdx).show();
    $("#saveAsUpdatedItem" + rowIdx).hide();
});
function addFirstRow(id) {
    rowIdx = Number(id);
    $('#tGLPostingListbody').append(`
    <tr id="R${rowIdx}">
        <td hidden>
            <input type="text" class="form-control" id="agtId${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control codeChangesFromRow" id="agtAccCode${rowIdx}" />
        </td>
        <td>
            <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlAGTAccDescription${rowIdx}"></select>
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="agtDebitAccount${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="agtCreditAccount${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control" id="agtMemo${rowIdx}" />
        </td>
        <td class="text-center">
            <div id="addItem${rowIdx}" class="row">
                <a style="cursor:pointer" class="btn btn-primary add"><i class="bi bi-plus-circle"></i>add</a>
            </div>
            <div id="updateItem${rowIdx}" class="row">
                <button style='margin-left:2px' class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                <button style='margin-left:2px' class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
            </div>
            <div id="saveAsUpdatedItem${rowIdx}" class="row">
                <button style='margin-left:2px' class="btn btn-sm  sav" type="button"><i class="fa-solid fa-check text-success" aria-hidden="true" title="save"></i></button>
                <button style='margin-left:2px' class="btn btn-sm  can" type="button"><i class="fa-solid fa-xmark text-success" aria-hidden="true" title="cancel"></i></button>
            </div>
        </td>
    </tr>`); 
    getAllAccountFromDB(rowIdx);
   // $('.autoSuggestionSelect').css('width', '100%');
    //$(".autoSuggestionSelect").select2({});
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).hide();
}