var rowTableIdx = 0;
var rowJournalTableIdx = 0;
var allAccounts = [];
var itemList = [];

$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});


function dateValue() {
    $.ajax({
        type: "GET",
        url: "/StoreGReceive/DateValue",
        success: function (data) {
            if (data != null) {
                var data = new Date(data);
                var day = ("0" + data.getDate()).slice(-2);
                var month = ("0" + (data.getMonth() + 1)).slice(-2);
                var dayClose = data.getFullYear() + "-" + (month) + "-" + (day);
                $("#grmDate").val(dayClose);

            }
        }
    });
}

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/StoreGReceive/GetInitData",
        success: function (response) {
            if (response != null) {
                var empDetails = response.empDetails;
                var accounts = response.chartMasterDD;
                allAccounts = response.chartMasterDD;
                var subGroups = response.chartTypeDD;
                $('#ddlitemName').empty();
                $('#ddlitemName').append(new Option("--Select Item Name--", -1))

                $('#ddlsupplierName').append(new Option("--Select Suppliers Name--", -1))
                for (var i = 0; i < empDetails.length; i++) {
                    var option = new Option(empDetails[i].name, empDetails[i].id);
                    $(option).html(empDetails[i].name);
                    $("#ddlsupplierName").append(option);
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
function getDescriptionDD() {
    $.ajax({
        type: "GET",
        url: "/StoreGReceive/GetInitData",
        success: function (response) {
            if (response != null) {
                var accounts = response.chartMasterDD;
                allAccounts = response.chartMasterDD;
                var subGroups = response.chartTypeDD;
                $('#ddlitemName').empty();
                $('#ddlitemName').append(new Option("New account", -1))

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
        grdUnit: $("#grdUnit").val(),
        grdQty: $("#grdQty").val(),
        grduPrice: $("#grduPrice").val(),
        grdtPrice: $("#grdtPrice").val(),
    };

    $('#tGLPostingListbody').append(
        `<tr id="Item${++rowTableIdx}">
            <td hidden id="grdId${rowTableIdx}">` + 0 + `</td>
            <td id="itemCode${rowTableIdx}">` + objectItem.itemCode + `</td>
            <td hidden id="itemDescriptionId${rowTableIdx}">` + objectItem.itemDescriptionId + `</td>
            <td id="itemName${rowTableIdx}">` + objectItem.itemDescriptionText + `</td>
            <td id="grdUnit${rowTableIdx}">` + objectItem.grdUnit + `</td>
            <td id="grdQty${rowTableIdx}">` + objectItem.grdQty + `</td>
            <td id="grduPrice${rowTableIdx}">` + objectItem.grduPrice + `</td>
            <td id="grdtPrice${rowTableIdx}">` + objectItem.grdtPrice + `</td>
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
    var grdQty = $("#grdQty").val();
    var grduPrice = $('#grduPrice').val();

    showErrorMessageBelowCtrl('itemCode', 'Item Code is required', false);
    showErrorMessageBelowCtrl('ddlitemName', 'Item Name is required', false);
    showErrorMessageBelowCtrl('grdQty', 'Quantity is required', false);
    showErrorMessageBelowCtrl('grduPrice', 'Unit Price is required', false);

    if (grduPrice == undefined || grduPrice.length <= 0) {
        showErrorMessageBelowCtrl('grduPrice', 'Unit is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#grduPrice").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('grduPrice', 'Unit is required', false);
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
    if (grdQty == undefined || grdQty.length <= 0) {
        showErrorMessageBelowCtrl('grdQty', 'Quantity is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#grdQty").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('grdQty', 'Quantity is required', false);
    }

    return response;
}
function resetFrom() {
    $('#itemCode').val("");
    $("#ddlitemName").val(-1).change();
    $('#grdUnit').val("");
    $('#grdQty').val("");
    $('#grduPrice').val("");
    $('#grdtPrice').val("");
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
        $('#grdUnit').val(account.unit);
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
        $('#grdUnit').val(account.unit);
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

    data.append('GRMId', 0);
    data.append('GRMDate', $('#grmDate').val());
    data.append('SSId', $('#ddlsupplierName').val());
    data.append('GRMRemarks', $('#grmRemarks').val());

    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 1; i <= tablelength; i++) {
        itemList.push({
            grdId: $('#grdId' + i).text(),
            itemCode: $('#itemCode' + i).text(),
            itemName: $('#itemName' + i).text(),
            unit: $('#grdUnit' + i).text(),
            grdQty: $('#grdQty' + i).text(),
            grduPrice: $('#grduPrice' + i).text(),
            grdtPrice: $('#grdtPrice' + i).text()
        });
    }
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreGReceive/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Saved Successfully!', 'success', 1, function () { window.location = "/StoreGReceive/masterDetails"; });
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

    var ddlsupplierName = parseInt($("#ddlsupplierName option:selected").val());
    showErrorMessageBelowCtrl('ddlsupplierName', 'Supplier Name is required', false);

    if (isNaN(ddlsupplierName) || ddlsupplierName <= 0) {
        showErrorMessageBelowCtrl('ddlsupplierName', 'Supplier Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlsupplierName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlsupplierName', 'Supplier Name is required', false);
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
        url: "/StoreGReceive/GetAll",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tJournalListbody').empty();
            
            for (var i = 0; i < items.length; i++) {

                $('#tJournalListbody').append(
                    `<tr id="Item${++rowJournalTableIdx}">
                        <td hidden id="grmId${rowJournalTableIdx}">` + items[i].grmId + `</td>
                        <td id="grmDateString${rowJournalTableIdx}">` + items[i].grmDateString + `</td>
                        <td id="storeSuppliersName${rowJournalTableIdx}">` + items[i].storeSuppliersName + `</td>
                        <td id="grmRemarks${rowJournalTableIdx}">` + items[i].grmRemarks + `</td>
                        <td class="text-center">
                            <button id="` + items[i].grmId + `" class="btn btn-sm btn-primary view" type="button">Details</button>
                            
                        </td>
                    </tr>`
                    //< a class= "btn btn-sm btn-secondary" href = '/StoreGReceive/Edit?id=` + items[i].grmId + `' > Edit</a >
                    //<button id="` + items[i].grmId + `" class="btn btn-sm btn-danger delete" type="button">Delete</button>
                );
            }
        }
    });
}

$('#tJournalListbody').on('click', '.view', function () {
    window.location = '/StoreGReceive/ItemDetails?id=' + $(this)[0].id;
});
$('#tJournalListbody').on('click', '.edit', function () {
    window.location = '/StoreGReceive/Edit?id=' + $(this)[0].id;
});
$('.print').click(function () {
    $("#nonPrintArea").hide();
    $("#viewJournalModal").show();
    window.print();
});
function getForEdit() {
    debugger
    var url = window.location.href;
    var id = url.substring(url.lastIndexOf('=') + 1);

    $.ajax({
        type: "GET",
        url: "/StoreGReceive/GetById?id=" + id,
        data: "{}",
        success: function (data) {
            let items = data.items;
            $('#tGLPostingListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tGLPostingListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="grdId${rowTableIdx}">` + 0 + `</td>
                        <td id="itemCode${rowTableIdx}">` + objectItem.itemCode + `</td>
                        <td hidden id="itemDescriptionId${rowTableIdx}">` + objectItem.itemDescriptionId + `</td>
                        <td id="itemName{rowTableIdx}">` + objectItem.itemDescriptionText + `</td>
                        <td id="grdUnit${rowTableIdx}">` + objectItem.grdUnit + `</td>
                        <td id="grdQty${rowTableIdx}">` + objectItem.grdQty + `</td>
                        <td id="grduPrice${rowTableIdx}">` + objectItem.grduPrice + `</td>
                        <td id="grdtPrice${rowTableIdx}">` + objectItem.grdtPrice + `</td>
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

    data.append('GRMId', 0);
    data.append('GRMDate', $('#grmDate').val());
    data.append('SSId', $('#ddlsupplierName').val());
    data.append('GRMRemarks', $('#grmRemarks').val());

    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 1; i <= tablelength; i++) {
        itemList.push({
            grdId: $('#grdId' + i).text(),
            itemCode: $('#itemCode' + i).text(),
            agtAccDescription: $('#ItemName' + i).text(),
            grdUnit: $('#grdUnit' + i).text(),
            grdQty: $('#grdQty' + i).text(),
            grduPrice: $('#grduPrice' + i).text(),
            grdtPrice: $('#grdtPrice' + i).text()
        });
    }
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreGReceive/Update",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Update Successfully!', 'success', 1, function () { window.location = "/StoreGReceive/masterDetails"; });
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

    var grmId = parseInt(this.id);
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
                        url: "/StoreGReceive/Delete?id=" + grmId,
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


/*Runtime Calculate Total Price */
$("#grduPrice, #grdQty").keyup(function () {
    var total = 0;
    var x = $("#grduPrice").val();
    var y = $("#grdQty").val();

    var total = x * y;

    $("#grdtPrice").val(total);
});


/*inline edit*/
function getAllItemFromDB(parameter) {
    $.ajax({
        type: "GET",
        url: "/StoreGReceive/GetAllItems",
        data: "{}",
        success: function (response) {

            var accounts = response.chartMasterDD;
            var subGroups = response.chartTypeDD;

            var o = new Option("Select Account", "-1");
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
    '.itemChangesFromRow',
    function () {
        debugger
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(10);
        var value = $("#itemCode" + rowTrackId).val();
        var account = allAccounts.filter(x => x.code == value)[0];
        if (account != undefined) {
            $('#ddlitemName' + rowTrackId).val(account.id);
            $('#ddlitemName' + rowTrackId).change();
        }
        else {
            $('#ddlitemName' + rowTrackId).val(-1);
            $('#ddlitemName' + rowTrackId).change();
        }
    });
$('#tGLPostingListbody').on('change',
    '.itemChangesFromRow',
    function () {
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(20);
        var value = $("#ddlitemName" + rowTrackId).val();
        var account = allAccounts.filter(x => x.id == value)[0];

        if (account != undefined) {
            $('#itemCode' + rowTrackId).val(account.code);
            $('#unit' + rowTrackId).val(account.code);
        }
        else {
            $('#itemCode' + rowTrackId).val();
            $('#unit' + rowTrackId).val();
        }
    });
$('#tGLPostingListbody').on('click', '.add', function () {

    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

        rowIdx += 1;
        $('#tGLPostingListbody').append(`
            <tr id="R${rowIdx}">
                <td>
                    <input type="text" class="form-control itemChangesFromRow" id="itemCode${rowIdx}" />
                </td>
                <td>
                    <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}" name="ddlitemName"></select>
                </td>
                <td>
                    <input type="text" class="form-control accountChangesFromRow" id="unit${rowIdx}" />
                </td>
                <td>
                    <input type="text" class="form-control numbersOnly" placeholder="0.00" id="grdQty${rowIdx}" />
                </td>
                <td>
                    <input type="text" class="form-control numbersOnly" placeholder="0.00" id="grduPrice${rowIdx}" />
                </td>
                 <td>
                    <input type="text" class="form-control numbersOnly" placeholder="0.00" id="grdtPrice${rowIdx}" />
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
            getAllItemFromDB(rowIdx);
        $('.autoSuggestionSelect').css('width', '100%');
        $(".autoSuggestionSelect").select2({});
        $("#updateItem" + rowIdx).hide();
        $("#saveAsUpdatedItem" + rowIdx).hide();
        var prevRowIdx = rowIdx - 1;
        $("#addItem" + prevRowIdx).hide();
        $("#itemCode" + prevRowIdx).prop('disabled', true);
        $("#ddlitemName" + prevRowIdx).prop('disabled', true);
        $("#updateItem" + prevRowIdx).show();
        $("#saveAsUpdatedItem" + prevRowIdx).hide();
});
$('#tGLPostingListbody').on('click', '.edit', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    itemCode = $("#itemCode" + rowIdx).val();
    ddlitemName = $("#ddlitemName" + rowIdx).val();
    unit = $("#unit" + rowIdx).val();
    grduPrice = $("#grduPrice" + rowIdx).val();
    grdtPrice = $("#grdtPrice" + rowIdx).val();

    $("#itemCode" + rowIdx).prop('disabled', false);
    $("#ddlitemName" + rowIdx).prop('disabled', false);
    $("#unit" + rowIdx).prop('disabled', false);
    $("#grduPrice" + rowIdx).prop('disabled', false);
    $("#grdtPrice" + rowIdx).prop('disabled', false);
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).show();
});
$('#tGLPostingListbody').on('click', '.remove', function () {
    $(this).closest('tr').remove();
});
$('#tGLPostingListbody').on('click', '.sav', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    $("#itemCode" + rowIdx).prop('disabled', true);
    $("#ddlitemName" + rowIdx).prop('disabled', true);
    $("#unit" + rowIdx).prop('disabled', true);
    $("#grduPrice" + rowIdx).prop('disabled', true);
    $("#grdtPrice" + rowIdx).prop('disabled', true);
    $("#updateItem" + rowIdx).show();
    $("#saveAsUpdatedItem" + rowIdx).hide();
    
});
$('#tGLPostingListbody').on('click', '.can', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    $("#itemCode" + rowIdx).val(accCode);
    $("#ddlitemName" + rowIdx).val(accDesc);
    $("#ddlitemName" + rowIdx).change();
    //$("#agtDebitAccount" + rowIdx).val(debit);
    //$("#agtCreditAccount" + rowIdx).val(credit);
    //$("#agtMemo" + rowIdx).val(memo);

    $("#itemCode" + rowIdx).prop('disabled', true);
    $("#ddlitemName" + rowIdx).prop('disabled', true);
    $("#unit" + rowIdx).prop('disabled', true);
    $("#grduPrice" + rowIdx).prop('disabled', true);
    $("#grdtPrice" + rowIdx).prop('disabled', true);
    $("#updateItem" + rowIdx).show();
    $("#saveAsUpdatedItem" + rowIdx).hide();
});
function addFirstRow() {
    rowIdx = 0;
    $('#tGLPostingListbody').append(`
    <tr id="R${rowIdx}">
        <td>
            <input type="text" class="form-control itemChangesFromRow" id="itemCode${rowIdx}" />
        </td>
        <td>
            <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}" name="ddlitemName"></select>
        </td>
        <td>
            <input type="text" class="form-control accountChangesFromRow" id="unit${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="grdQty${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="grduPrice${rowIdx}" />
        </td>
         <td>
            <input type="text" class="form-control numbersOnly" placeholder="0.00" id="grdtPrice${rowIdx}" />
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
    getAllItemFromDB(rowIdx);
    $('.autoSuggestionSelect').css('width', '100%');
    $(".autoSuggestionSelect").select2({});
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).hide();
}