var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

$("#hrlptDay").keypress(function (e) {

    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllLeavePolicyClass() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRLeavePolicy/GetAllLeavePolicyClass",
        data: "{}",
        success: function (data) {
            let items = data;
            $('#tLeavePolicybody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tLeavePolicybody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrLPId${rowTableIdx}">` + items[i].hrlpId + `</td>
                        <td id="hrlpName${rowTableIdx}">` + items[i].hrlpName + `</td>
                        <td id="hrlptDay${rowTableIdx}">` + items[i].hrlptDay + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrlpId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrlpId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tLeavePolicybody').on('click', '.remove', function () {

    var hrlpid = parseInt(this.id);
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
                        url: "/HRLeavePolicy/delete?id=" + hrlpid,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllLeavePolicyClass();
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
$('#tLeavePolicybody').on('click', '.edit', function () {

    var hrlpid = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HRLeavePolicy/GetById?id=" + hrlpid,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrlpId").val(response.hrlpId);
                $("#hrlpName").val(response.hrlpName);
                $("#hrlptDay").val(response.hrlptDay);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrlpId").val(0);
    SaveOrUpdateRequest();
});

$("#hrlptDay").blur(function () {
    var hrlpTDay = $("#hrlptDay").val();
    showErrorMessageBelowCtrl('hrlptDay', 'Leave Policy Day is required', false);
    if (hrlptDay == undefined || hrlptDay == '') {
        showErrorMessageBelowCtrl('hrlptDay', 'Leave Policy Day is required', true);
    } else {
        showErrorMessageBelowCtrl('hrlptDay', 'Leave Policy Day is required', false);
    }
});

$("#hrlpName").blur(function () {
    var hrlpName = $("#hrlpName").val();
    showErrorMessageBelowCtrl('hrlpName', 'Leave Policy Name is required', false);
    if (hrlpName == undefined || hrlpName == '') {
        showErrorMessageBelowCtrl('hrlpName', 'Leave Policy Name is required', true);
    } else {
        showErrorMessageBelowCtrl('hrlpName', 'Leave Policy Name is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('hrlpId', $('#hrlpId').val());
    data.append('hrlpName', $('#hrlpName').val());
    data.append('hrlptDay', $('#hrlptDay').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRLeavePolicy/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllLeavePolicyClass();
                $("#hrlpName").val(null);
                $("#hrlptDay").val(null);
                $("#addNewItem").show();
                $("#updateExistingItem").hide();
                alertify.notify('Update Successfully!', 'success');
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


    var hrlpName = $("#hrlpName").val();
    var hrlptDay = $("#hrlptDay").val();

    showErrorMessageBelowCtrl('hrlpName', 'Leave Policy Name is required', false);
    showErrorMessageBelowCtrl('hrlptDay', 'Total Day is required', false);

    if (hrlptDay == undefined || hrlptDay == '') {
        showErrorMessageBelowCtrl('hrlptDay', 'Total Day is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrlptDay").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrlptDay', 'Total Day is required', false);
    }

    if (hrlpName == undefined || hrlpName == '') {
        showErrorMessageBelowCtrl('hrlpName', 'Leave Policy Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrlpName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrlpName', 'Leave Policy is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#hrlpId").val(0);
    $("#hrlpName").val(null);
    $("#hrlptDay").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();

});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});