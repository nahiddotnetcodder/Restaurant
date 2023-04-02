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
    debugger
    var tablelength = $('#tGLPostingListbody tr').length;
    for (var i = 0; i < tablelength - 1; i++) {
        itemList.push({
            grdId: Number($('#grdId' + i).val()) ?? 0,
            itemCode: $('#itemCode' + i).val(),
            itemDescriptionId: $('#dditemName' + i).find(":selected").val(),
            ddlitemName: $('#dditemName' + i).find(":selected").text(),
            grdUnit: $('#grdUnit' + i).val(),
            grdQty: $('#grdQty' + i).val() ?? 0,
            grduPrice: $('#grduPrice' + i).val() ?? 0,
            grdtPrice: $('#grdtPrice' + i).val() ?? 0,
            
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
    var url = window.location.href;
    var id = url.substring(url.lastIndexOf('=') + 1);

    $.ajax({
        type: "GET",
        url: "/StoreGReceive/GetById?id=" + id,
        data: "{}",
        success: function (data) {
            allAccounts = data.allAccounts;
            let items = data.items;
            rowIdx = 0;
            $('#tGLPostingListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tGLPostingListbody').append(
                    `<tr id="R${rowIdx}">
                        <td hidden>
                            <input type="text" class="form-control" id="grdId${rowIdx}" />
                        </td>
                        <td>
                            <input type="text" class="form-control codeChangesFromRow" id="itemAccCode${rowIdx}" />
                        </td>
                        <td>
                            <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}"></select>
                        </td>
                        <td>
                            <input type="text" class="form-control codeChangesFromRow" id="grdUnit${rowIdx}" />
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
                                <button style='margin-left:2px' class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                                <button style='margin-left:2px' class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                            </div>
                            <div id="saveAsUpdatedItem${rowIdx}" class="row">
                                <button style='margin-left:2px' class="btn btn-sm  sav" type="button"><i class="fa-solid fa-check text-success" aria-hidden="true" title="save"></i></button>
                                <button style='margin-left:2px' class="btn btn-sm  can" type="button"><i class="fa-solid fa-xmark text-success" aria-hidden="true" title="cancel"></i></button>
                            </div>
                        </td>
                    </tr>`
                );
                getAllItemFromDB(rowIdx);
                $("#grdId$" + rowIdx).val(items[i].grdId$);
                $("#itemCode" + rowIdx).val(items[i].itemCode);
                $("#ddlitemName" + rowIdx).val(items[i].itemDescriptionId);
                $("#ddlitemName" + rowIdx).change();
                $("#grdUnit" + rowIdx).val(items[i].grdUnit);
                $("#grdQty" + rowIdx).val(items[i].grdQty);
                $("#grduPrice" + rowIdx).val(items[i].grduPrice);
                $("#grdtPrice" + rowIdx).val(items[i].grdtPrice);

                $("#itemCode" + rowIdx).prop('disabled', true);
                $("#ddlitemName" + rowIdx).prop('disabled', true);
                $("#grdUnit" + rowIdx).prop('disabled', true);
                $("#grdQty" + rowIdx).prop('disabled', true);
                $("#grduPrice" + rowIdx).prop('disabled', true);
                $("#grdtPrice" + rowIdx).prop('disabled', true);

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
        let getDomId = $(this).attr('id'); debugger
        var rowTrackId = getDomId.slice(11);
        var value = $("#ddlitemName" + rowTrackId).val();
        if (value != null) {
            var account = allAccounts.filter(x => x.id == value)[0];

            if (account != undefined) {
                $('#itemCode' + rowTrackId).val(account.code);
                $('#grdUnit' + rowTrackId).val(account.unit);
            }
            else {
                $('#itemCode' + rowTrackId).val();
                $('#grdUnit' + rowTrackId).val();
            }
        }
    });
$('#tGLPostingListbody').on('change',
    '.codeChangesFromRow',
    function () {
        let getDomId = $(this).attr('id');
        var rowTrackId = getDomId.slice(8);
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

    var grdQty = $("#grdQty" + rowIdx).val();
    var grduPrice = $("#grduPrice" + rowIdx).val();
    if (Number(grdQty) == '' || Number(grdQty) == undefined) {
        alertify.error('Quantity is Required!');
        response = false;
    }
    else if (Number(grduPrice) == '' && Number(grduPrice) == undefined) {
        alertify.error('Unit Price is Requied!');
        response = false;
    }

    else {
        rowIdx += 1;
        $('#tGLPostingListbody').append(`
            <tr id="R${rowIdx}">
                <td hidden>
                    <input type="text" class="form-control" id="grdId${rowIdx}" />
                </td>
                <td>
                    <input type="text" class="form-control codeChangesFromRow" id="itemCode${rowIdx}" />
                </td>
                <td>
                    <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}"></select>
                </td>
                <td>
                    <input type="text" class="form-control codeChangesFromRow" id="grdUnit${rowIdx}" />
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
                        <button style='margin-left:2px' class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                        <button style='margin-left:2px' class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                    </div>
                    <div id="saveAsUpdatedItem${rowIdx}" class="row">
                        <button style='margin-left:2px' class="btn btn-sm  sav" type="button"><i class="fa-solid fa-check text-success" aria-hidden="true" title="save"></i></button>
                        <button style='margin-left:2px' class="btn btn-sm  can" type="button"><i class="fa-solid fa-xmark text-success" aria-hidden="true" title="cancel"></i></button>
                    </div>
                </td>
            </tr>`);
        getAllItemFromDB(rowIdx);
        // $('.autoSuggestionSelect').css('width', '100%');
        // $(".autoSuggestionSelect").select2({});
        $("#updateItem" + rowIdx).hide();
        $("#saveAsUpdatedItem" + rowIdx).hide();
        var prevRowIdx = rowIdx - 1;
        $("#addItem" + prevRowIdx).hide();
        $("#itemCode" + prevRowIdx).prop('disabled', true);
        $("#ddlitemName" + prevRowIdx).prop('disabled', true);
        $("#grdUnit" + prevRowIdx).prop('disabled', true);
        $("#grdQty" + prevRowIdx).prop('disabled', true);
        $("#grduPrice" + prevRowIdx).prop('disabled', true);
        $("#grdtPrice" + prevRowIdx).prop('disabled', true);
        $("#updateItem" + prevRowIdx).show();
        $("#saveAsUpdatedItem" + prevRowIdx).hide();
    }
});
$('#tGLPostingListbody').on('click', '.edit', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    itemCode = $("itemCode" + rowIdx).val();
    ddlitemName = $("#ddlitemName" + rowIdx).val();
    grdUnit = $("#grdUnit" + rowIdx).val();
    grdQty = $("#grdQty" + rowIdx).val();
    grduPrice = $("#grduPrice" + rowIdx).val();
    grdtPrice = $("#grdtPrice" + rowIdx).val();

    $("#itemCode" + rowIdx).prop('disabled', false);
    $("#ddlitemName" + rowIdx).prop('disabled', false);
    $("#grdUnit" + rowIdx).prop('disabled', false);
    $("#grdQty" + rowIdx).prop('disabled', false);
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
    $("#dditemName" + rowIdx).prop('disabled', true);
    $("#grdUnit" + rowIdx).prop('disabled', true);
    $("#grdQty" + rowIdx).prop('disabled', true);
    $("#grduPrice" + rowIdx).prop('disabled', true);
    $("#grdtPrice" + rowIdx).prop('disabled', true);
    $("#updateItem" + rowIdx).show();
    $("#saveAsUpdatedItem" + rowIdx).hide();
});
$('#tGLPostingListbody').on('click', '.can', function () {
    let getDomId = $(this).closest('tr').attr('id');
    rowIdx = parseInt(getDomId.slice(1));

    $("#itemCode" + rowIdx).val(itemCode);
    $("#dditemName" + rowIdx).val(dditemName);
    $("#dditemName" + rowIdx).change();
    $("#grdUnit" + rowIdx).val(grdUnit);
    $("#grdQty" + rowIdx).val(grdQty);
    $("#grduPrice" + rowIdx).val(grduPrice);
    $("#grdtPrice" + rowIdx).val(grdtPrice);

    $("#itemCode" + rowIdx).prop('disabled', true);
    $("#dditemName" + rowIdx).prop('disabled', true);
    $("#grdUnit" + rowIdx).prop('disabled', true);
    $("#grdQty" + rowIdx).prop('disabled', true);
    $("#grduPrice" + rowIdx).prop('disabled', true);
    $("#grdtPrice" + rowIdx).prop('disabled', true);
    $("#updateItem" + rowIdx).show();
    $("#saveAsUpdatedItem" + rowIdx).hide();
});
function addFirstRow(id) {
    rowIdx = Number(id);
    $('#tGLPostingListbody').append(`
    <tr id="R${rowIdx}">
        <td hidden>
            <input type="text" class="form-control" id="grdId${rowIdx}" />
        </td>
        <td>
            <input type="text" class="form-control codeChangesFromRow" id="itemAccCode${rowIdx}" />
        </td>
        <td>
            <select class="form-control accountChangesFromRow autoSuggestionSelect" id="ddlitemName${rowIdx}"></select>
        </td>
        <td>
            <input type="text" class="form-control codeChangesFromRow" id="grdUnit${rowIdx}" />
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
                <button style='margin-left:2px' class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></button>
                <button style='margin-left:2px' class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
            </div>
            <div id="saveAsUpdatedItem${rowIdx}" class="row">
                <button style='margin-left:2px' class="btn btn-sm  sav" type="button"><i class="fa-solid fa-check text-success" aria-hidden="true" title="save"></i></button>
                <button style='margin-left:2px' class="btn btn-sm  can" type="button"><i class="fa-solid fa-xmark text-success" aria-hidden="true" title="cancel"></i></button>
            </div>
        </td>
    </tr>`); 
    getAllItemFromDB(rowIdx);
    // $('.autoSuggestionSelect').css('width', '100%');
    //$(".autoSuggestionSelect").select2({});
    $("#updateItem" + rowIdx).hide();
    $("#saveAsUpdatedItem" + rowIdx).hide();
}