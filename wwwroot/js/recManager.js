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
        url: "/RecManager/GetInitData",
        success: function (response) {
            if (response != null) {
                var accounts = response.recMasterDD;
                allAccounts = response.recMasterDD;
                allitem = response.chartMasterDD;
                var subGroups = response.recTypeDD;
                $('#ddliteCode').empty();
                $('#ddliteCode').append(new Option("--Select Item Code--", -1))

                for (var i = 0; i < subGroups.length; i++) {
                    var optgroup = "<optgroup label='" + subGroups[i].name + "'>";
                    var data = accounts.filter(x => x.accountGroupId == subGroups[i].id);
                    for (var j = 0; j < data.length; j++) {
                        optgroup += "<option value='" + data[j].id + "'>" + data[j].name + "</option>"
                    }
                    optgroup += "</optgroup>";
                    $("#ddliteCode").append(optgroup);
                }
            }
        }
    });
}


$("#rmItemName").change(function () {
    showErrorMessageBelowCtrl('rmItemName', 'Item Code is required', false);
    if (this.value.length > 0) {
        var account = allAccounts.filter(x => x.description == this.value)[0];
        $('#ddliteCode').val(account?.id);
        showErrorMessageBelowCtrl('rmItemName', 'Item Code is required', false);
    } else {
        showErrorMessageBelowCtrl('rmItemName', 'Item Code is required', true);
    }
});
$("#ddliteCode").change(function () {
    showErrorMessageBelowCtrl('ddliteCode', 'Item Name is required', false);
    if (this.value > 0) {
        var account = allAccounts.filter(x => x.id == this.value)[0];
        $('#rmItemName').val(account.description);
        showErrorMessageBelowCtrl('ddliteCode', 'Item Name is required', false);
    }
    else {
        $('#rmItemName').val('');
    }
});



function getAllStockTable() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/RecManager/GetAllStockTable",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tStockListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tStockListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="sgsId${rowTableIdx}">` + items[i].sgsId + `</td>
                        <td id="itemName${rowTableIdx}">` + items[i].itemName + `</td>
                        <td id="itemCode${rowTableIdx}">` + items[i].itemCode + `</td>
                        <td class="text-center">
                            <button id="` + items[i].sgsId + `" class="btn btn-sm  select" type="button"> <i class="fa-solid fa-plus text-success" aria-hidden="true" title="Select"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}

$('#tStockListbody').on('click', '.select', function () {
    var grmId = parseInt(this.id);
    if (grmId > 0) {
        var accounts = allitem.filter(x => x.id == grmId)[0];
        $('#sigItemCode').val(accounts.code);
        $('#sigItemName').val(accounts.description);
        $('#sigUnit').val(accounts.unit);
        $('#sgsUPrice').val(accounts.price);
    }
    else {
        showErrorMessageBelowCtrl('sigItemName', 'Item Name is required', true);
    }
});



/*Runtime Calculate Total Price */
$("#sgsUPrice, #rmdQty").keyup(function () {
    var total = 0;
    var x = $("#sgsUPrice").val();
    var y = $("#rmdQty").val();

    var total = x * y;

    $("#totalCost").val(total.toFixed(2));
});


function addItem() {
    var result = validationCheckForItem();
    if (result == false) { return; }
    let objectItem = {
        sigItemCode: $("#sigItemCode").val(),
        sigItemName: $("#sigItemName").val(),
        rmdQty: $("#rmdQty").val(),
        sigUnit: $("#sigUnit").val(),
        sgsUPrice: $("#sgsUPrice").val(),
        totalCost: $("#totalCost").val()
    };

    $('#titemListbody').append(
        `<tr id="Item${++rowTableIdx}">
            <td hidden id="rmdId${rowTableIdx}">` + 0 + `</td>
            <td id="sigItemCode${rowTableIdx}">` + objectItem.sigItemCode + `</td>
            <td id="sigItemName${rowTableIdx}">` + objectItem.sigItemName + `</td>
            <td id="rmdQty${rowTableIdx}">` + objectItem.rmdQty + `</td>
            <td id="sigUnit${rowTableIdx}">` + objectItem.sigUnit + `</td>
            <td id="sgsUPrice${rowTableIdx}">` + objectItem.sgsUPrice + `</td>
            <td id="totalCost${rowTableIdx}">` + objectItem.totalCost + `</td>
            <td class="text-center">
                <button class="btn btn-sm btn-danger remove" type="button"><i class="fa fa-trash" aria-hidden="true"></i></button>
            </td>
        </tr>`
    );
    resetFrom();
}

function validationCheckForItem() {

    var response = true;

    var sigItemCode = $("#sigItemCode").val();
    var sigItemName = $("#sigItemName").val();
    var rmdQty = $("#rmdQty").val();
    var sigUnit = $('#sigUnit').val();
    var sgsUPrice = $('#sgsUPrice').val();

    showErrorMessageBelowCtrl('sigItemCode', 'Item Code is required', false);
    showErrorMessageBelowCtrl('sigItemName', 'Item Name is required', false);
    showErrorMessageBelowCtrl('rmdQty', 'Quantity is required', false);
    showErrorMessageBelowCtrl('sigUnit', 'Unit is required', false);
    showErrorMessageBelowCtrl('sgsUPrice', 'Unit Cost is required', false);

    if (sgsUPrice == undefined || sgsUPrice.length <= 0) {
        showErrorMessageBelowCtrl('sgsUPrice', 'Unit Cost is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#sgsUPrice").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('sgsUPrice', 'Unit Cost is required', false);
    }

    if (sigUnit == undefined || sigUnit.length <= 0) {
        showErrorMessageBelowCtrl('sigUnit', 'Unit is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#sigUnit").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('sigUnit', 'Unit is required', false);
    }

    if (sigItemCode == undefined || sigItemCode.length <= 0) {
        showErrorMessageBelowCtrl('sigItemCode', 'Item Code  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#sigItemCode").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('sigItemCode', 'Item Code  is required', false);
    }
    if (sigItemName == undefined || sigItemName.length <= 0) {
        showErrorMessageBelowCtrl('sigItemName', 'Item Name  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#sigItemName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('sigItemName', 'Item Name  is required', false);
    }
    if (rmdQty == undefined || rmdQty.length <= 0) {
        showErrorMessageBelowCtrl('rmdQty', 'Quantity is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#rmdQty").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('rmdQty', 'Quantity is required', false);
    }

    return response;
}

function resetFrom() {
    $('#sigItemCode').val("");
    $('#sigItemName').val("");
    $('#rmdQty').val("");
    $('#sigUnit').val("");
    $('#sgsUPrice').val("");
    $('#totalCost').val("");
}
$('#titemListbody').on('click', '.remove', function () {
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

function SaveRequest() {
    var result = validationCheck();
    if (result == false) { return; }
    var data = new FormData();
    data.append('RMMId', 0);
    data.append('RMItemCode', $('#ddliteCode').val());
    data.append('RMItemName', $('#rmItemName').val());
    debugger
    var tablelength = $('#titemListbody tr').length;
    for (var i = 1; i <= tablelength; i++) {
        itemList.push({
            rmdId: $('#rmdId' + i).val(),
            sigitemCode: $('#sigItemCode' + i).val(),
            sigItemName: $('#sigItemName' + i).val(),
            rmdQty: $("#rmdQty" + i).val(),
            sigUnit: $('#sigUnit' + i).val(),
            sgsUPrice: $('#sgsUPrice' + i).val(),
            totalCost: $('#totalCost' + i).val()
        });

    } 
    data.append('Items', JSON.stringify(itemList));

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/RecManager/Save",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                alertify.notify('Saved Successfully!', 'success', 1, function () { window.location = "/RecManager/masterDetails"; });
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

    var rmItemName = $("#rmItemName").val();
    var ddliteCode = parseInt($("#ddliteCode option:selected").val());

    showErrorMessageBelowCtrl('rmItemName', 'Item Name is required', false);
    showErrorMessageBelowCtrl('ddliteCode', 'Item Code is required', false);

    if (rmItemName == undefined || rmItemName == '') {
        showErrorMessageBelowCtrl('rmItemName', 'Item Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#rmItemName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('rmItemName', 'Item Name is required', false);
    }

    if (isNaN(ddliteCode) || ddliteCode <= 0) {
        showErrorMessageBelowCtrl('ddliteCode', 'Item Code is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddliteCode").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddliteCode', 'Item Code is required', false);
    }

    return response;
}
$("#addNewButton").click(function () {
    SaveRequest();
});

function getAll() {
    rowJournalTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/RecManager/GetAll",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tJournalListbody').empty();
            for (var i = 0; i < items.length; i++) {

                $('#tJournalListbody').append(
                    `<tr id="Item${++rowJournalTableIdx}">
                        <td hidden id="rmmId{rowJournalTableIdx}">` + items[i].rmmId + `</td>
                        <td id="rmItemCode${rowJournalTableIdx}">` + items[i].rmItemCode + `</td>
                        <td id="rmItemName${rowJournalTableIdx}">` + items[i].rmItemName + `</td>
                        <td id="cUser${rowJournalTableIdx}">` + items[i].cUser + `</td>
                        <td class="text-center">
                            <button id="` + items[i].rmmId + `" class="btn btn-sm btn-primary view" type="button">Details</button>
                            <a class="btn btn-sm btn-secondary" href='/RecManager/Edit?id=` + items[i].rmmId + `'>Edit</a>
                            <button id="` + items[i].rmmId + `" class="btn btn-sm btn-danger delete" type="button">Delete</button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}

$('#tJournalListbody').on('click', '.delete', function () {

    var rmmId = parseInt(this.id);
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
                        url: "/RecManager/Delete?id=" + rmmId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAll();
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



$('#tJournalListbody').on('click', '.view', function () {
    window.location = '/RecManager/ItemDetails?id=' + $(this)[0].id;
});
$('#tJournalListbody').on('click', '.edit', function () {
    window.location = '/RecManager/Edit?id=' + $(this)[0].id;
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
                        <td id="itemDescription${rowTableIdx}">` + objectItem.itemDescriptionText + `</td>
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
            agtAccDescription: $('#itemName' + i).text(),
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





