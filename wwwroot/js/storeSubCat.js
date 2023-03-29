var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getInitData() {
    $.ajax({
        type: "GET",
        url: "/StoreSCategory/GetInitData",
        success: function (response) {
            if (response != null) {
                var data = response;
                $('#ddlcatName').append(new Option("--Select Category --", -1))
                for (var i = 0; i < data.length; i++) {
                    var option = new Option(data[i].name, data[i].id);
                    $(option).html(data[i].name);
                    $("#ddlcatName").append(option);
                }
            }
        }
    });
}
function getAllSubCategoryClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/StoreSCategory/GetAllSubCategoryClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tSubCatListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tSubCatListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="sscId{rowTableIdx}">` + items[i].sscId + `</td>
                        <td id="storeCatName{rowTableIdx}">` + items[i].storeCatName + `</td>
                        <td id="sscName{rowTableIdx}">` + items[i].sscName + `</td>
                        <td class="text-center">
                            <button id="` + items[i].sscId + `" class="btn btn-sm  edit" type="button"><i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i></i></button>
                            <button id="` + items[i].sscId + `" class="btn btn-sm  remove" type="button"><i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i></button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tSubCatListbody').on('click', '.remove', function () {

    var sscId = parseInt(this.id);
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
                        url: "/StoreSCategory/Delete?id=" + sscId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllSubCategoryClass();
                            } else {
                                alertify.error(response.message);
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
$('#tSubCatListbody').on('click', '.edit', function () {

    var sscId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/StoreSCategory/GetById?id=" + sscId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#sscId").val(response.sscId);
                $("#ddlcatName").val(response.scId);
                $("#sscname").val(response.sscName);
                $("#ddlcatName").attr('disabled', true);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});

$("#addNewButton").click(function () {
    $("#sscId").val(0);
    SaveOrUpdateRequest();
});
$("#ddlcatName").blur(function () {
    var ddlcatName = $("#ddlcatName").val();
    showErrorMessageBelowCtrl('ddlcatName', 'Category Name is required', false);
    if (actClassId == undefined || ddlempName == '') {
        showErrorMessageBelowCtrl('ddlcatName', 'Category Name is required', true);
    } else {
        showErrorMessageBelowCtrl('ddlcatName', 'Category Name is required', false);
    }
});
$("#sscname").blur(function () {
    var sscname = $("#sscname").val();
    showErrorMessageBelowCtrl('sscname', 'Sub Category field is required', false);
    if (sscname == undefined || sscname == '') {
        showErrorMessageBelowCtrl('sscname', 'Sub Category field is required', true);
    } else {
        showErrorMessageBelowCtrl('sscname', 'Sub Category field is required', false);
    }
});


function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('SSCId', $('#sscId').val());
    data.append('SCId', $('#ddlcatName').find(":selected").val());
    data.append('SSCName', $('#sscname').val());
   

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/StoreSCategory/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllSubCategoryClass();
                $("#ddlcatName").val(-1);
                $("#sscname").val(null);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                $("#ddlcatName").attr('disabled', false);
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
    var sscname = $("#sscname").val();
    var ddlcatName = parseInt($("#ddlcatName").children("option:selected").val());

    showErrorMessageBelowCtrl('sscname', 'Shift Type is required', false);
    showErrorMessageBelowCtrl('ddlcatName', 'Employee Name is required', false);

    if (sscname == undefined || sscname == '') {
        showErrorMessageBelowCtrl('sscname', 'Sub Category Field  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#sscname").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('sscname', 'Sub Category Field Id is required', false);
    }
    if (ddlcatName == undefined || ddlcatName == '') {
        showErrorMessageBelowCtrl('ddlcatName', 'Category Field  is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlcatName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlcatName', 'Category Field Id is required', false);
    }


    return response;
}
$("#cancelButton").click(function () {
    $("#sscId").val(0);
    $("#sscname").val(null);
    $("#ddlcatName").val(-1);
    $("#ddlempName").val(0);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    $("#ddlcatName").attr('disabled', false);
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});