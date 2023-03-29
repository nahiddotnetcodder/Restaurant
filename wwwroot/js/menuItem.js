function getInitData() {
    $.ajax({
        type: "GET",
        url: "/MenuItem/GetInitData",
        success: function (response) {
            if (response != null) {
                var menus = response.menusDD;
                var status = response.statusDD;
                $('#ddlMenus').empty();
                $('#ddlMenus').append(new Option("Add New Menu", -1))
                for (var i = 0; i < menus.length; i++) {
                    var option = new Option(menus[i].name, menus[i].id);
                    $(option).html(menus[i].name);
                    $("#ddlMenus").append(option);
                }
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
        url: "/MenuItem/GetAll",
        success: function (response) {
            if (response != null) {
                var menuItems = response
                $('#ddlMenuItems').empty();
                $('#ddlMenuItems').append(new Option("Add New Menu Items", -1))
                for (var i = 0; i < menuItems.length; i++) {
                    var option = new Option(menuItems[i].name, menuItems[i].id);
                    $(option).html(menuItems[i].name);
                    $("#ddlMenuItems").append(option);
                }
            }
        }
    });
}
$("#ddlMenuItems").change(function () {
    if (this.value == -1) {
        resetValue();
    } else {
        $.ajax({
            type: "GET",
            url: "/MenuItem/GetById?id=" + this.value,
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
    $("#ddlMenus").val(response.menuId);
    $("#addNewItem").hide();
    $("#updateExistingItem").show();
}
function resetValue() {
    $("#id").val('');
    $("#name").val('');
    $("#ddlStatus").val(1);
    $("#ddlMenus").val(-1);
    $("#ddlMenuItems").val(-1);
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
    data.append('MenuId', $('#ddlMenus').find(":selected").val());

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/MenuItem/SaveOrUpdate",
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
    var ddlMenus = parseInt($("#ddlMenus").children("option:selected").val());

    showErrorMessageBelowCtrl('name', 'Name is required', false);
    showErrorMessageBelowCtrl('ddlMenus', 'Menu is required', false);

    if (name == undefined || name == '') {
        showErrorMessageBelowCtrl('name', 'Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#name").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('name', 'Name is required', false);
    }
    if (isNaN(ddlMenus) || ddlMenus <= 0) {
        showErrorMessageBelowCtrl('ddlMenus', 'Menu is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#ddlMenus").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('ddlMenus', 'Menu is required', false);
    }
    return response;
}