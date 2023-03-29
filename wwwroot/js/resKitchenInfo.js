var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllKitchenClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/ResKitchenInfo/GetAllKitchenClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tResKitchenbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tResKitchenbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="rkId${rowTableIdx}">` + items[i].rkId + `</td>
                        <td id="rKitchenName${rowTableIdx}">` + items[i].rKitchenName + `</td>
                        <td id="rkDescription{rowTableIdx}">` + items[i].rkDescription + `</td>
                        <td class="text-center">
                            <button id="` + items[i].rkId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].rkId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tResKitchenbody').on('click', '.remove', function () {

    var rkId = parseInt(this.id);
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
                        url: "/ResKitchenInfo/delete?id=" + rkId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllKitchenClass();
                                alertify.notify('Deleted Successfully!', 'success');
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
$('#tResKitchenbody').on('click', '.edit', function () {

    var rkId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/ResKitchenInfo/GetById?id=" + rkId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#rkId").val(response.rkId);
                $("#rKitchenName").val(response.rKitchenName);
                $("#rkDescription").val(response.rkDescription);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#rkId").val(0);
    SaveOrUpdateRequest();
});

$("#rKitchenName").blur(function () {
    var rKitchenName = $("#rKitchenName").val();
    showErrorMessageBelowCtrl('rKitchenName', 'Kitchen Name is required', false);
    if (rKitchenName == undefined || rftName == '') {
        showErrorMessageBelowCtrl('rKitchenName', 'Kitchen Name is required', true);
    } else {
        showErrorMessageBelowCtrl('rKitchenName', 'Kitchen Name is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('rkId', $('#rkId').val());
    data.append('rKitchenName', $('#rKitchenName').val());
    data.append('rkDescription', $('#rkDescription').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/ResKitchenInfo/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllKitchenClass();
                $("#rKitchenName").val(null);
                $("#rkDescription").val(null);
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


    var rKitchenName = $("#rKitchenName").val();

    showErrorMessageBelowCtrl('rKitchenName', 'Kitchen Name is required', false);


    if (rKitchenName == undefined || rKitchenName == '') {
        showErrorMessageBelowCtrl('rKitchenName', 'Kitchen Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#rKitchenName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('rKitchenName', 'Kitchen Name is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#rkId").val(0);
    $("#rKitchenName").val(null);
    $("#rkDescription").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();

});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});