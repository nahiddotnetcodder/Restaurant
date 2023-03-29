var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllCategoryClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/StoreCategory/GetAllCategoryClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tCategoryListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tCategoryListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="sCId${rowTableIdx}">` + items[i].scId + `</td>
                        <td id="scName${rowTableIdx}">` + items[i].scName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].scId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].scId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tCategoryListbody').on('click', '.remove', function () {

    var scid = parseInt(this.id);
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
                        url: "/StoreCategory/delete?id=" + scid,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllCategoryClass();
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
$('#tCategoryListbody').on('click', '.edit', function () {

    var scId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/StoreCategory/GetById?id=" + scId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#scId").val(response.scId);
                $("#scName").val(response.scName);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#scId").val(0);
    SaveOrUpdateRequest();
});

$("#scName").blur(function () {
    var suName = $("#scName").val();
    showErrorMessageBelowCtrl('scName', 'Category Name is required', false);
    if (suName == undefined || suName == '') {
        showErrorMessageBelowCtrl('scName', 'Category Name is required', true);
    } else {
        showErrorMessageBelowCtrl('scName', 'Category Name is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('scId', $('#scId').val());
     data.append('scName', $('#scName').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreCategory/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {

            if (response.success) {
                getAllCategoryClass();
                $("#scName").val(null);
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

    
    var scName = $("#scName").val();

    showErrorMessageBelowCtrl('scName', 'Category Name is required', false);


    if (scName == undefined || scName == '') {
        showErrorMessageBelowCtrl('scName', 'Category Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#scName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('scName', 'Category Name is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#scId").val(0);
    $("#scName").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});