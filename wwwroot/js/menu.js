function getInitData() {
    $.ajax({
        type: "GET",
        url: "/Menu/GetInitData",
        success: function (response) {
            if (response != null) {
                var status = response
                $('#ddlStatus').empty();
                for (var i = 0; i < status.length; i++) {
                    var option = new Option(status[i].name, status[i].id);
                    $(option).html(status[i].name);
                    $("#ddlStatus").append(option);
                }
            }
        }
    });
}
function getAll() {
    $.ajax({
        type: "GET",
        url: "/Menu/GetAll",
        success: function (response) {
            if (response != null) {
                var menus = response
                $('#ddlMenus').empty();
                $('#ddlMenus').append(new Option("Add New Menu", -1))
                for (var i = 0; i < menus.length; i++) {
                    var option = new Option(menus[i].name, menus[i].id);
                    $(option).html(menus[i].name);
                    $("#ddlMenus").append(option);
                }
            }
        }
    });
}
$("#ddlMenus").change(function () {
    if (this.value == -1) {
        resetValue();
    } else {
        $.ajax({
            type: "GET",
            url: "/Menu/GetById?id=" + this.value,
            success: function (response) {
                if (response != null) {
                    setValue(response);
                }
            }
        });
    }
});
function setValue(response) {
    $("#id").val(response.id);
    $("#name").val(response.name);
    $("#ddlStatus").val(response.statusId);
    $("#addNewItem").hide();
    $("#updateExistingItem").show();
}
function resetValue() {
    $("#id").val('');
    $("#name").val('');
    $("#ddlStatus").val(1);
    $("#ddlMenus").val(-1);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
}
$("#addNewButton").click(function () {
    $("#id").val('');
    SaveOrUpdateRequest();
});
$("#cancelButton").click(function () {
    resetValue();
});
$("#updateButton").click(function () {
    SaveOrUpdateRequest();
});
function SaveOrUpdateRequest() {

    var result = validationCheck();
    if (result == false) { return; }

    var data = new FormData();

    data.append('Id', $('#id').val());
    data.append('Name', $('#name').val());
    data.append('StatusId', $('#ddlStatus').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/Menu/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getAll();
                resetValue();
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

    var name = $("#name").val();

    showErrorMessageBelowCtrl('name', 'Name is required', false);

    if (name == undefined || name == '') {
        showErrorMessageBelowCtrl('name', 'Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#name").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('name', 'Name is required', false);
    }
    return response;
}