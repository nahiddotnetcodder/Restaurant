var rowIdx = 0;
var rowTableIdx = 0;
var rowJournalTableIdx = 0;
var allAccounts = [];
var itemList = [];
var itemCode = '';
var ddlitemName = 0;
var grdUnit = 0;
var grdQty = 0;
var grduPrice = 0;
var grdtPrice = 0;

$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function dateValue() {
    $.ajax({
        type: "GET",
        url: "/StoreGIssue/DateValue",
        success: function (data) {
            if (data != null) {
                var data = new Date(data);
                var day = ("0" + data.getDate()).slice(-2);
                var month = ("0" + (data.getMonth() + 1)).slice(-2);
                var dayClose = data.getFullYear() + "-" + (month) + "-" + (day);
                $("#gimDate").val(dayClose);

            }
        }
    });
}

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/StoreGIssue/GetInitData",
        success: function (response) {
            if (response != null) {
                var deptDetails = response.deptDetails;
                var accounts = response.chartMasterDD;
                allAccounts = response.chartMasterDD;
                var subGroups = response.chartTypeDD;
                $('#ddlitemName').empty();
                $('#ddlitemName').append(new Option("--Select Item Name--", -1))

                $('#ddldeptName').append(new Option("--Select Department Name--", -1))
                for (var i = 0; i < deptDetails.length; i++) {
                    var option = new Option(deptDetails[i].name, deptDetails[i].id);
                    $(option).html(deptDetails[i].name);
                    $("#ddldeptName").append(option);
                }

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddlitemName").append(optgroup);
                }
            }
        }
    });
}

function addItem() {
    var result = validationCheckForItem();
    if (result == false) { return; }
    var descriptionId = $("#ddlitemName option:selected").val();
    var selectedAccount = allAccounts.filter(x => x.id == Number(descriptionId))[0];
    
    let objectItem = {
        itemCode: $("#itemCode").val(),
        itemDescriptionId: descriptionId,
        itemDescriptionText: selectedAccount.description,
        unit: $("#unit").val(),
        gidQty: $("#gidQty").val(),
        giduPrice: $("#giduPrice").val(),
        gidtPrice: $("#gidtPrice").val(),
    };

    $('#tGLPostingListbody').append(
        `<tr id="Item${++rowTableIdx}">
            <td hidden id="gidId${rowTableIdx}">` + 0 + `</td>
            <td id="itemCode${rowTableIdx}">` + objectItem.itemCode + `</td>
            <td hidden id="itemDescriptionId${rowTableIdx}">` + objectItem.itemDescriptionId + `</td>
            <td id="itemName${rowTableIdx}">` + objectItem.itemDescriptionText + `</td>
            <td id="unit${rowTableIdx}">` + objectItem.unit + `</td>
            <td id="gidQty${rowTableIdx}">` + objectItem.gidQty + `</td>
            <td id="giduPrice${rowTableIdx}">` + objectItem.giduPrice + `</td>
            <td id="gidtPrice${rowTableIdx}">` + objectItem.gidtPrice + `</td>
            <td class="text-center">
               <button class="btn btn-sm btn-danger remove" type="button"><i class="fa fa-trash" aria-hidden="true"></i></button>
            </td>
        </tr>`
    );
    resetFrom();
}



function validationCheckForItem() {

    var response = true;

    var itemCode = $("#itemCode").val();
    var ddlitemName = parseInt($("#ddlitemName option:selected").val());
    var gidQty = $("#gidQty").val();
    var giduPrice = $('#giduPrice').val();

    showErrorMessageBelowCtrl('itemCode', 'Item Code is required', false);
    showErrorMessageBelowCtrl('ddlitemName', 'Item Name is required', false);
    showErrorMessageBelowCtrl('gidQty', 'Quantity is required', false);
    showErrorMessageBelowCtrl('giduPrice', 'Unit Price is required', false);


    if (giduPrice == undefined || giduPrice.length <= 0) {
        showErrorMessageBelowCtrl('giduPrice', 'Unit is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#giduPrice").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('giduPrice', 'Unit is required', false);
    }

    if (itemCode == undefined || itemCode.length <= 0) {
        showErrorMessageBelowCtrl('itemCode', 'Item Code  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#itemCode").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('itemCode', 'Item Code  is required', false);
    }
    if (isNaN(ddlitemName) || ddlitemName <= 0) {
        showErrorMessageBelowCtrl('ddlitemName', 'Item Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlitemName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlitemName', 'Item Name is required', false);
    }
    if (gidQty == undefined || gidQty.length <= 0) {
        showErrorMessageBelowCtrl('gidQty', 'Quantity is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#gidQty").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('gidQty', 'Quantity is required', false);
    }

    return response;
}

function resetFrom() {
    $('#itemCode').val("");
    $("#ddlitemName").val(-1).change();
    $('#unit').val("");
    $('#gidQty').val("");
    $('#giduPrice').val("");
    $('#gidtPrice').val("");
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

$("#itemCode").change(function () {
    showErrorMessageBelowCtrl('itemCode', 'Item Code is required', false);
    if (this.value.length > 0) {
        var account = allAccounts.filter(x => x.code == this.value)[0];
        $('#ddlitemName').val(account?.id);
        $('#unit').val(account.unit);
        showErrorMessageBelowCtrl('itemCode', 'Item Code is required', false);
    } else {
        showErrorMessageBelowCtrl('itemCode', 'Item Code is required', true);
    }
});
$("#ddlitemName").change(function () {
    showErrorMessageBelowCtrl('ddlitemName', 'Item Name is required', false);
    if (this.value > 0) {
        var account = allAccounts.filter(x => x.id == this.value)[0];
        $('#itemCode').val(account.code);
        $('#giduPrice').val(account.price.toFixed(2));
        $('#unit').val(account.unit);
        showErrorMessageBelowCtrl('ddlitemName', 'Item Name is required', false);
    }
    else {
        $('#itemCode').val('');
    }
});




function SaveRequest() {
    var result = validationCheck();
    if (result == false) { return; }
    var data = new FormData();

    data.append('GIMId', 0);
    data.append('GIMDate', $('#gimDate').val());
    data.append('HRDId', $('#ddldeptName').val());
    data.append('GIMRemarks', $('#gimRemarks').val());

    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 0; i < tablelength; i++) {
        itemList.push({
            gidId: Number($('#gidId' + i).val()) ?? 0,
            itemCode: $('#itemCode' + i).val(),
            itemDescriptionId: $('#dditemName' + i).find(":selected").val(),
            ddlitemName: $('#dditemName' + i).find(":selected").text(),
            unit: $('#unit' + i).val(),
            gidQty: $('#gidQty' + i).val() ?? 0,
            giduPrice: $('#giduPrice' + i).val() ?? 0,
            gidtPrice: $('#gidtPrice' + i).val() ?? 0,
        });
    }
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreGIssue/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Saved Successfully!', 'success', 1, function () { window.location = "/StoreGIssue/masterDetails"; });
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
    var ddldeptName = parseInt($("#ddldeptName option:selected").val());

    showErrorMessageBelowCtrl('ddldeptName', 'Department Name is required', false);

    if (isNaN(ddldeptName) || ddldeptName <= 0) {
        showErrorMessageBelowCtrl('ddldeptName', 'Department is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddldeptName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddldeptName', 'Department is required', false);
    }

    return response;
}
$("#addNewButton").click(function () {
    SaveRequest();
});
function getAllReceive() {
    rowJournalTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/StoreGIssue/GetAll",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tJournalListbody').empty();
            for (var i = 0; i < items.length; i++) {

                $('#tJournalListbody').append(
                    `<tr id="Item${++rowJournalTableIdx}">
                        <td hidden id="gimId${rowJournalTableIdx}">` + items[i].gimId + `</td>
                        <td id="gimDateString${rowJournalTableIdx}">` + items[i].gimDateString + `</td>
                        <td id="hrDepartName${rowJournalTableIdx}">` + items[i].hrDepartName + `</td>
                        <td id="gimRemarks${rowJournalTableIdx}">` + items[i].gimRemarks + `</td>
                        <td class="text-center">
                            <button id="` + items[i].gimId + `" class="btn btn-sm btn-primary view" type="button">Details</button>
                        </td>
                    </tr>`
                    //< a class= "btn btn-sm btn-secondary edit" href = '/StoreGIssue/Edit?id=` + items[i].gimId + `' > Edit</a >
                    //<button id="` + items[i].gimId + `" class="btn btn-sm btn-danger delete" type="button">Delete</button>
                );
            }
        }
    });
}

$('#tJournalListbody').on('click', '.view', function () {
    window.location = '/StoreGIssue/ItemDetails?id=' + $(this)[0].id;
});
$('#tGLPostingListbody').on('click', '.edit', function () {
    window.location = '/StoreGIssue/Edit?id=' + $(this)[0].id;
});
$('.print').click(function () {
    $("#nonPrintArea").hide();
    $("#viewJournalModal").show();
    window.print();
});
function getForEdit() {
    var url = window.location.href;
    var id = url.substring(url.lastIndexOf('=') + 1);
    debugger
    $.ajax({
        type: "GET",
        url: "/StoreGIssue/GetById?id=" + id,
        data: "{}",
        success: function (data) {
            let items = data.items;
            $('#tGLPostingListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tGLPostingListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="gidId${rowTableIdx}">` + 0 + `</td>
                        <td id="itemCode${rowTableIdx}">` + objectItem.itemCode + `</td>
                        <td hidden id="itemDescriptionId${rowTableIdx}">` + objectItem.itemDescriptionId + `</td>
                        <td id="itemName${rowTableIdx}">` + objectItem.itemDescriptionText + `</td>
                        <td id="unit${rowTableIdx}">` + objectItem.unit + `</td>
                        <td id="gidQty${rowTableIdx}">` + objectItem.gidQty + `</td>
                        <td id="giduPrice${rowTableIdx}">` + objectItem.giduPrice + `</td>
                        <td id="gidtPrice${rowTableIdx}">` + objectItem.gidtPrice + `</td>
                        <td class="text-center">
                            <button id="` + items[i].gimId + `" class="btn btn-sm btn-danger remove" type="button"><i class="fa fa-trash" aria-hidden="true"></i></button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#updateButton').click(function () {

    data.append('GIMId', 0);
    data.append('GIMDate', $('#gimDate').val());
    data.append('HRDId', $('#ddldeptName').val());
    data.append('GIMRemarks', $('#gimRemarks').val());

    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 1; i <= tablelength; i++) {
        itemList.push({
            gidId: $('#gidId' + i).text(),
            itemCode: $('#itemCode' + i).text(),
            agtAccDescription: $('#itemName' + i).text(),
            unit: $('#unit' + i).text(),
            gidQty: $('#gidQty' + i).text(),
            giduPrice: $('#giduPrice' + i).text(),
            gidtPrice: $('#gidtPrice' + i).text()
        });
    }
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreGIssue/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Update Successfully!', 'success', 1, function () { window.location = "/StoreGIssue/masterDetails"; });
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

$('#tJournalListbody').on('click', '.delete', function () {

    var gimId = parseInt(this.id);
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
                        url: "/StoreGIssue/Delete?id=" + gimId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllReceive();
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



/*inline js*/
function getAllIssueFromDB(parameter) {
    $.ajax({
        type: "GET",
        url: "/StoreGIssue/GetAllIssue",
        data: "{}",
        success: function (response) {
            var accounts = response.chartMasterDD;
            var subGroups = response.chartTypeDD;

            var o = new Option("Select Item", "-1");
            $(o).html("Please Select Item");
            $("#ddlitemName" + parameter).append(o);

            for (var i = 0; i < subGroups.length; i++) {
                var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                for (var j = 0; j < data.length; j++) {
                    optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                }
                optgroup += "</optgroup>";
                $("#ddlitemName" + parameter).append(optgroup);
            }
        }
    });
}
$('#tGLPostingListbody').on('change',
    '.accountChangesFromRow',
    function () {
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(20);
        var value = $("#ddlitemName" + rowTrackId).val();
        if (value != null) {
            var account = allAccounts.filter(x => x.id == value)[0];

            if (account != undefined) {
                $('#itemCode' + rowTrackId).val(account.code);
            }
            else {
                $('#itemCode' + rowTrackId).val();
            }
        }
    });
$('#tGLPostingListbody').on('change',
    '.codeChangesFromRow',
    function () {
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(10);
        var value = $("#itemCode" + rowTrackId).val();
        if (value != null) {
            var account = allAccounts.filter(x => x.code == value)[0];
            if (account != undefined) {
                $('#ddlitemName' + rowTrackId).val(account.id);
                $('#ddlitemName' + rowTrackId).change();
            }
            else {
                $('#ddlitemName' + rowTrackId).val(-1);
                $('#ddlitemName' + rowTrackId).change();
            }
        }
    });
$('#tGLPostingListbody').on('click', '.add', function () {

    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    rowIdx += 1;
    $('#tGLPostingListbody').append(`
        <tr id="R${rowIdx}">
            <td hidden>
                <input type="text" class="form-control" id="gitId${rowIdx}" />
            </td>
            <td>
                <input type="text" class="form-control codeChangesFromRow" id="itemCode${rowIdx}" />
            </td>
            <td>
                <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}"></select>
            </td>
            <td>
                <input type="text" class="form-control accountChangesFromRow" placeholder="0.00" id="unit${rowIdx}" />
            </td>
            <td>
                <input type="text" class="form-control numbersOnly" placeholder="0.00" id="gidQty${rowIdx}" />
            </td>
            <td>
                <input type="text" class="form-control numbersOnly" placeholder="0.00" id="giduPrice${rowIdx}" />
            </td>
            <td>
                <input type="text" class="form-control numbersOnly" placeholder="0.00" id="gidtPrice${rowIdx}" />
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
    getAllIssueFromDB(rowIdx);
    // $('.autoSuggestionSelect').css('width', '100%');
    // $(".autoSuggestionSelect").select2({});
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).hide();
    var prevRowIdx = rowIdx - 1;
    $("#addItem" + prevRowIdx).hide();
    $("#itemCode" + prevRowIdx).prop('disabled', true);
    $("#ddlitemName" + prevRowIdx).prop('disabled', true);
    $("#unit" + prevRowIdx).prop('disabled', true);
    $("#gidQty" + prevRowIdx).prop('disabled', true);
    $("#giduPrice" + prevRowIdx).prop('disabled', true);
    $("#gidtPrice" + prevRowIdx).prop('disabled', true);
    $("#updateItem" + prevRowIdx).show();
    $("#saveAsUpdatedItem" + prevRowIdx).hide();
});
$('#tGLPostingListbody').on('click', '.edit', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    itemCode = $("#itemCode" + rowIdx).val();
    ddlitemName = $("#ddlitemName" + rowIdx).val();
    unit = $("#unit" + rowIdx).val();
    gidQty = $("#gitQty" + rowIdx).val();
    giduPrice = $("#giduPrice" + rowIdx).val();
    gidtPrice = $("#gidtPrice" + rowIdx).val();

    $("#itemCode" + rowIdx).prop('disabled', false);
    $("#ddlitemName" + rowIdx).prop('disabled', false);
    $("#unit" + rowIdx).prop('disabled', false);
    $("#gidQty" + rowIdx).prop('disabled', false);
    $("#giduPrice" + rowIdx).prop('disabled', false);
    $("#gidtPrice" + rowIdx).prop('disabled', false);
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).show();
});
$('#tGLPostingListbody').on('click', '.remove', function () {
    $(this).closest('tr').remove();
});
$('#tGLPostingListbody').on('click', '.sav', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));
        $("#itemCode" + rowIdx).prop('disabled', false);
        $("#ddlitemName" + rowIdx).prop('disabled', false);
        $("#unit" + rowIdx).prop('disabled', false);
        $("#gidQty" + rowIdx).prop('disabled', false);
        $("#giduPrice" + rowIdx).prop('disabled', false);
        $("#gidtPrice" + rowIdx).prop('disabled', false);
        $("#updateItem" + rowIdx).show();
        $("#saveAsUpdatedItem" + rowIdx).hide();
});
$('#tGLPostingListbody').on('click', '.can', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    $("#itemCode" + rowIdx).val(itemCode);
    $("#ddlitemName" + rowIdx).val(ddlitemName);
    $("#ddlitemName" + rowIdx).change();
    $("#unit" + rowIdx).val(unit);
    $("#gidQty" + rowIdx).val(gidQty);
    $("#giduPrice" + rowIdx).val(giduPrice);
    $("#gidtPrice" + rowIdx).val(gidtPrice);

    $("#itemCode" + rowIdx).prop('disabled', true);
    $("#ddlitemName" + rowIdx).prop('disabled', true);
    $("#unit" + rowIdx).prop('disabled', true);
    $("#gidQty" + rowIdx).prop('disabled', true);
    $("#giduPrice" + rowIdx).prop('disabled', true);
    $("#gidtPrice" + rowIdx).prop('disabled', true);
    $("#updateItem" + rowIdx).show();
    $("#saveAsUpdatedItem" + rowIdx).hide();
});
function addFirstRow(id) {
    debugger    
    rowIdx = Number(id);
    $('#tGLPostingListbody').append(`
    <tr id="R${rowIdx}">
        <td hidden>
            <input type="text" class="form-control" id="gidId${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control codeChangesFromRow" id="itemCode${rowIdx}" />
        </td>
        <td>
            <select class="form-control-sm accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}"></select>
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="unit${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="gidQty{rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="giduPrice{rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="gidTPrice{rowIdx}" />
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
    getAllIssueFromDB(rowIdx);
    // $('.autoSuggestionSelect').css('width', '100%');
    //$(".autoSuggestionSelect").select2({});
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).hide();
}


/*Runtime Calculate Total Price */
$("#giduPrice, #gidQty").keyup(function () {
    var total = 0;
    var x = $("#giduPrice").val();
    var y = $("#gidQty").val();

    var total = x * y;

    $("#gidtPrice").val(total.toFixed(2));
});