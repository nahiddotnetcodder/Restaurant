var menuItemList = [];
function getInitData() {
    $.ajax({
        type: "GET",
        url: "/Role/GetInitData",
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
function getAssignedMenus() {
    $.ajax({
        type: "GET",
        url: "/Role/GetAssignedMenus?roleId=" + $('#ddlRoles').find(":selected").val(),
        success: function (response) {
            if (response != null) {
                menuItemList = [];
                var menus = response.menus;
                var menuItems = response.menuItems;
                menuItemList = response.menuItems;
                $('#menu').empty();
                for (var i = 0; i < menus.length; i++) {
                    $('#menu').append(`<label>` + menus[i].name + `</label><hr style = 'border: 1px solid red'>`);
                    $('#menu').append(`<ol>`);
                    for (var j = 0; j < menuItems.length; j++) {
                        if (menus[i].id == menuItems[j].menuId) {
                            $('#menu').append(
                                `<li>`
                                + menuItems[j].name +
                                ` <input style = 'margin-left:150px'  type="checkbox" id='switch` + menuItems[j].id + `'>` +
                                `</li>`);
                            $('#switch' + menuItems[j].id).prop("checked", menuItems[j].isSelected);
                        }
                    }
                    $('#menu').append(`<ol>`);
                }
            }
        }
    });
}
function getRoles() {
    $.ajax({
        type: "GET",
        url: "/Role/GetRoles",
        success: function (response) {
            if (response != null) {
                var roles = response
                $('#ddlRoles').empty();
                $('#ddlRoles').append(new Option("Add New Role", -1))
                for (var i = 0; i < roles.length; i++) {
                    var option = new Option(roles[i].name, roles[i].id);
                    $(option).html(roles[i].name);
                    $("#ddlRoles").append(option);
                }
            }
        }
    });
}
$("#ddlRoles").change(function () {
    if (this.value == -1) {
        resetValue();
        getAssignedMenus();
    } else {
        $.ajax({
            type: "GET",
            url: "/Role/GetRoleById?id=" + this.value,
            success: function (response) {
                if (response != null) {
                    setValue(response);
                    getAssignedMenus();
                }
            }
        });
    }
});
function setValue(response) {
    $("#id").val(response.id);
    $("#name").val(response.name);
    $("#description").val(response.description);
    $("#ddlStatus").val(response.currentStatusId);
    $("#addNewItem").hide();
    $("#updateExistingItem").show();
}
function resetValue() {
    $("#id").val('');
    $("#name").val('');
    $("#description").val('');
    $("#ddlStatus").val(1);
    $("#ddlRoles").val(-1);
    $("#addNewItem").show();
    $("#updateExistingItem").hide();
    getAssignedMenus();
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

    var selectedMenuIds = [];
    for (var j = 0; j < menuItemList.length; j++) {
        if ($('#switch' + menuItemList[j].id).is(":checked")) {
            menuItemList[j].isSelected = true;
            selectedMenuIds.push(menuItemList[j]);
        }
        else {
            menuItemList[j].isSelected = false;
            selectedMenuIds.push(menuItemList[j]);
        }
    }
    var dataJson = JSON.stringify(selectedMenuIds);

    var data = new FormData();

    data.append('Id', $('#id').val());
    data.append('Name', $('#name').val());
    data.append('Description', $('#description').val());
    data.append('CurrentStatusId', $('#ddlStatus').find(":selected").val());
    data.append('SelectedMenuItems', dataJson);

    $.ajax({
        processData: false,
        contentType: false,
        type: "POST",
        url: "/Role/SaveOrUpdate",
        data: data,
        enctype: 'multipart/form-data',
        beforeSend: function () {
        },
        success: function (response) {
            if (response.success) {
                getRoles();
                resetValue();
                alertify.notify('Saved Successfully!', 'success', 1);
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
    var description = $("#description").val();

    showErrorMessageBelowCtrl('name', 'Name is required', false);
    showErrorMessageBelowCtrl('description', 'Description is required', false);

    if (name == undefined || name == '') {
        showErrorMessageBelowCtrl('name', 'Name is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#name").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('name', 'Name is required', false);
    }

    if (description == undefined || description == '') {
        showErrorMessageBelowCtrl('description', 'Description is required', true); response = false;
        $('html, body').animate({
            scrollTop: $("#description").offset().top
        }, 800);
    }
    else {
        showErrorMessageBelowCtrl('description', 'Description is required', false);
    }
    return response;
}