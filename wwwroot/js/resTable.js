var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllResTableClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/ResTable/GetAllResTableClass",
        data: "{}",
        success: function (data) {
            
            let items = data;
            $('#tResTablebody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tResTablebody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="rtId${rowTableIdx}">` + items[i].rtId + `</td>
                        <td id="rtNumber${rowTableIdx}">` + items[i].rtNumber + `</td>
                        <td id="rtDescription${rowTableIdx}">` + items[i].rtDescription + `</td>
                        <td class="text-center">
                            <button id="` + items[i].rtId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].rtId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tResTablebody').on('click', '.remove', function () {

    var rtId = parseInt(this.id);
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
                        url: "/ResTable/delete?id=" + rtId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllResTableClass();
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
$('#tResTablebody').on('click', '.edit', function () {

    var rtId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/ResTable/GetById?id=" + rtId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#rtId").val(response.rtId);
                $("#rtNumber").val(response.rtNumber);
                $("#rtDescription").val(response.rtDescription);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#rtId").val(0);
    SaveOrUpdateRequest();
});

$("#rtNumber").blur(function () {
    var rtNumber = $("#rtNumber").val();
    showErrorMessageBelowCtrl('rtNumber', 'Table Number is required', false);
    if (rtNumber == undefined || rtNumber == '') {
        showErrorMessageBelowCtrl('rtNumber', 'Table Number is required', true);
    } else {
        showErrorMessageBelowCtrl('rtNumber', 'Table Number is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('rtId', $('#rtId').val());
    data.append('rtNumber', $('#rtNumber').val());
    data.append('rtDescription', $('#rtDescription').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/ResTable/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllResTableClass();
                $("#rtNumber").val(null);
                $("#rtDescription").val(null);
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


    var rtNumber = $("#rtNumber").val();
    var rtDescription = $("#rtDescription").val();

    showErrorMessageBelowCtrl('rtNumber', 'Table Number is required', false);
    showErrorMessageBelowCtrl('rtDescription', 'Table Description is required', false);


    if (rtNumber == undefined || rtNumber == '') {
        showErrorMessageBelowCtrl('rtNumber', 'Table Number is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#rtNumber").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('rtNumber', 'Table Number is required', false);
    }

    if (rtDescription == undefined || rtDescription == '') {
        showErrorMessageBelowCtrl('rtDescription', 'Table Description is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#rtDescription").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('rtDescription', 'Table Description is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#rtId").val(0);
    $("#rtNumber").val(null);
    $("#rtDescription").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();

});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});