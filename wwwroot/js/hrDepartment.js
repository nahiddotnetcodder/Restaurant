var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllDeptClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRDepartment/GetAllDeptClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tDeptListbody').empty();
            $('#hrdDes').string.empty();
            for (var i = 0; i < items.length; i++) {
                $('#tDeptListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrdId${rowTableIdx}">` + items[i].hrdId + `</td>
                        <td id="hrdName${rowTableIdx}">` + items[i].hrdName + `</td>
                        <td id="hrdDes${rowTableIdx}">` + items[i].hrdDes + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrdId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrdId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tDeptListbody').on('click', '.remove', function () {

    var hrdId = parseInt(this.id);
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
                        url: "/HRDepartment/delete?id=" + hrdId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllDeptClass();
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
$('#tDeptListbody').on('click', '.edit', function () {

    var hrdId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HRDepartment/GetById?id=" + hrdId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrdId").val(response.hrdId);
                $("#hrdName").val(response.hrdName);
                $("#hrdDes").val(response.hrdDes);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrdId").val(0);
    SaveOrUpdateRequest();
});

$("#hrdName").blur(function () {
    var hrdName = $("#hrdName").val();
    showErrorMessageBelowCtrl('hrdName', 'Department Name is required', false);
    if (hrdName == undefined || hrdName == '') {
        showErrorMessageBelowCtrl('hrdName', 'Department Name is required', true);
    } else {
        showErrorMessageBelowCtrl('hrdName', 'Department Name is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('hrdId', $('#hrdId').val());
    data.append('hrdName', $('#hrdName').val());
    data.append('hrdDes', $('#hrdDes').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRDepartment/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllDeptClass();
                $("#hrdName").val(null);
                $("#hrdDes").val();
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


    var hrdName = $("#hrdName").val();

    showErrorMessageBelowCtrl('hrdName', 'Department Name is required', false);


    if (hrdName == undefined || hrdName == '') {
        showErrorMessageBelowCtrl('hrdName', 'Department Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrdName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrdName', 'Department Name is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#hrdId").val(0);
    $("#hrdName").val(null);
    $("#hrdDes").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();

});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});