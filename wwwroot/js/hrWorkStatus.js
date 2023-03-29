var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllStatusClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRWStatus/GetAllStatusClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tStatusListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tStatusListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrWSId${rowTableIdx}">` + items[i].hrwsId + `</td>
                        <td id="hrwsName${rowTableIdx}">` + items[i].hrwsName + `</td>
                        <td id="hrwsDes${rowTableIdx}">` + items[i].hrwsDes + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrwsId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrwsId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tStatusListbody').on('click', '.remove', function () {

    var hrwsId = parseInt(this.id);
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
                        url: "/HRWStatus/delete?id=" + hrwsId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllStatusClass();
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
$('#tStatusListbody').on('click', '.edit', function () {

    var hrwsId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HRWStatus/GetById?id=" + hrwsId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrwsId").val(response.hrwsId);
                $("#hrwsName").val(response.hrwsName);
                $("#hrwsDes").val(response.hrwsDes);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrwsId").val(0);
    SaveOrUpdateRequest();
});

$("#hrwsName").blur(function () {
    var hrwsName = $("#hrwsName").val();
    showErrorMessageBelowCtrl('hrwsName', 'Work Status Name is required', false);
    if (suName == undefined || suName == '') {
        showErrorMessageBelowCtrl('hrwsName', 'Work Status Name is required', true);
    } else {
        showErrorMessageBelowCtrl('hrwsName', 'Work Status Name is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('hrwsId', $('#hrwsId').val());
    data.append('hrwsName', $('#hrwsName').val());
    data.append('hrwsDes', $('#hrwsDes').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRWStatus/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllStatusClass();
                $("#hrwsName").val(null);
                $("#hrwsDes").val(null);
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

    
    var hrwsName = $("#hrwsName").val();

    showErrorMessageBelowCtrl('hrwsName', 'Work Status Name is required', false);
   

    if (hrwsName == undefined || hrwsName == '') {
        showErrorMessageBelowCtrl('hrwsName', 'Work Status Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrwsName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrwsName', 'Work Status Name is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#hrWSId").val(0);
    $("#hrwsName").val(null);
    $("#hrwsDes").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();    
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});