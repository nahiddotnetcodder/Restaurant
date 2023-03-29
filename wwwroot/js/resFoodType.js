var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllFoodTypeClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/ResFoodType/GetAllFoodTypeClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tResFoodTypebody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tResFoodTypebody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="rftId${rowTableIdx}">` + items[i].rftId + `</td>
                        <td id="rftName${rowTableIdx}">` + items[i].rftName + `</td>
                        <td id="rftDescription${rowTableIdx}">` + items[i].rftDescription + `</td>
                        <td class="text-center">
                            <button id="` + items[i].rftId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].rftId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tResFoodTypebody').on('click', '.remove', function () {

    var rftId = parseInt(this.id);
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
                        url: "/ResFoodType/delete?id=" + rftId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllFoodTypeClass();
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
$('#tResFoodTypebody').on('click', '.edit', function () {

    var rftId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/ResFoodType/GetById?id=" + rftId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#rftId").val(response.rftId);
                $("#rftName").val(response.rftName);
                $("#rftDescription").val(response.rftDescription);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#rftId").val(0);
    SaveOrUpdateRequest();
});

$("#rftName").blur(function () {
    var rftName = $("#rftName").val();
    showErrorMessageBelowCtrl('rftName', 'Food Type is required', false);
    if (rftName == undefined || rftName == '') {
        showErrorMessageBelowCtrl('rftName', 'Food Type is required', true);
    } else {
        showErrorMessageBelowCtrl('rftName', 'Food Type is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('rftId', $('#rftId').val());
    data.append('rftName', $('#rftName').val());
    data.append('rftDescription', $('#rftDescription').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/ResFoodType/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllFoodTypeClass();
                $("#rftName").val(null);
                $("#rftDescription").val(null);
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


    var rftName = $("#rftName").val();

    showErrorMessageBelowCtrl('rftName', 'Food Type Name is required', false);


    if (rftName == undefined || rftName == '') {
        showErrorMessageBelowCtrl('rftName', 'Food Type Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#rftName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('rftName', 'Food Type Name is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#rftId").val(0);
    $("#rftName").val(null);
    $("#rftDescription").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();

});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});