var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllUnitClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/StoreUnit/GetAllUnitClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tUnitListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tUnitListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="sUId${rowTableIdx}">` + items[i].suId + `</td>
                        <td id="suName{rowTableIdx}">` + items[i].suName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].suId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].suId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tUnitListbody').on('click', '.remove', function () {

    var suid = parseInt(this.id);
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
                        url: "/StoreUnit/delete?id=" + suid,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllUnitClass();
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
$('#tUnitListbody').on('click', '.edit', function () {

    var suId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/StoreUnit/GetById?id=" + suId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#suId").val(response.suId);
                $("#suName").val(response.suName);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#suId").val(0);
    SaveOrUpdateRequest();
});

$("#suName").blur(function () {
    var suName = $("#suName").val();
    showErrorMessageBelowCtrl('suName', 'Unit is required', false);
    if (suName == undefined || suName == '') {
        showErrorMessageBelowCtrl('suName', 'Unit is required', true);
    } else {
        showErrorMessageBelowCtrl('suName', 'Unit is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('suId', $('#suId').val());
    data.append('suName', $('#suName').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreUnit/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllUnitClass();
                $("#suName").val(null);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
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

    
    var suName = $("#suName").val();

    showErrorMessageBelowCtrl('suName', 'Unit is required', false);


    if (suName == undefined || suName == '') {
        showErrorMessageBelowCtrl('suName', 'Unit is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#suName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('suName', 'Unit is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#suId").val(0);
    $("#suName").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});