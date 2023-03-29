var rowTableIdx = 0;
$(".onlyNumber").change(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        return false;
    }
});

function getAllDesignation() {
    rowTableIdx = 0;
    $.ajax({
        type: "GET",
        url: "/HRDesignation/GetAllDesigClass",
        data: "{}",
        success: function (data) {
            
            let items = data;
            $('#tDesigListbody').empty();
            for (var i = 0; i < items.length; i++) {
                $('#tDesigListbody').append(
                    `<tr id="Item${++rowTableIdx}">
                        <td hidden id="hrDeId${rowTableIdx}">` + items[i].hrDeId + `</td>
                        <td id="hrDeName${rowTableIdx}">` + items[i].hrDeName + `</td>
                        <td id="hrDeDes${rowTableIdx}">` + items[i].hrDeDes + `</td>
                        <td class="text-center">
                            <button id="` + items[i].hrDeId + `" class="btn btn-sm  edit" type="button"> <i class="fa-solid fa-pencil text-success" aria-hidden="true" title="Edit"></i> </button>
                            <button id="` + items[i].hrDeId + `" class="btn btn-sm  remove" type="button"> <i class="fa-solid fa-xmark text-danger" aria-hidden="true" title="Delete"></i> </button>
                        </td>
                    </tr>`
                );
            }
        }
    });
}
$('#tDesigListbody').on('click', '.remove', function () {

    var hrDeId = parseInt(this.id);
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
                        url: "/HRDesignation/delete?id=" + hrDeId,
                        data: "{}",
                        success: function (response) {
                            if (response.success) {
                                getAllDesignation();
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
$('#tDesigListbody').on('click', '.edit', function () {

    var hrDeId = parseInt(this.id);

    $.ajax({
        type: "GET",
        url: "/HRDesignation/GetById?id=" + hrDeId,
        data: "{}",
        success: function (response) {
            if (response) {
                $("#hrDeId").val(response.hrDeId);
                $("#hrDeName").val(response.hrDeName);
                $("#hrDeDes").val(response.hrDeDes);
                $("#addNewItem").hide();
                $("#updateExistingItem").show();
            }
        }
    });
});
$("#addNewButton").click(function () {
    $("#hrDeId").val(0);
    SaveOrUpdateRequest();
});

$("#hrDeName").blur(function () {
    var hrDeName = $("#hrDeName").val();
    showErrorMessageBelowCtrl('hrDeName', 'Designation Name is required', false);
    if (hrDeName == undefined || hrDeName == '') {
        showErrorMessageBelowCtrl('hrDeName', 'Designation Name is required', true);
    } else {
        showErrorMessageBelowCtrl('hrDeName', 'Designation Name is required', false);
    }
});

function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('hrDeId', $('#hrDeId').val());
    data.append('hrDeName', $('#hrDeName').val());
    data.append('hrDeDes', $('#hrDeDes').val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/HRDesignation/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAllDesignation();
                $("#hrDeName").val(null);
                $("#hrDeDes").val(null);
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


    var hrDeName = $("#hrDeName").val();

    showErrorMessageBelowCtrl('hrDeName', 'Designation Name is required', false);


    if (hrDeName == undefined || hrDeName == '') {
        showErrorMessageBelowCtrl('hrDeName', 'Designation Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#hrDeName").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('hrDeName', 'Designation Name is required', false);
    }

    return response;
}
$("#cancelButton").click(function () {
    $("#hrDeId").val(0);
    $("#hrDeName").val(null);
    $("#hrDeDes").val(null);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();

});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});