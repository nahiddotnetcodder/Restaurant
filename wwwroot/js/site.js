function openErrorModal(strMessage) {
    var myDiv = document.getElementById("MyModalErrorAlertBody");
    myDiv.innerHTML = strMessage;
    $('#myModalError').modal('show');
}

function openSuccessModal(strMessage) {
    var myDiv = document.getElementById("MyModalSuccessAlertBody");
    myDiv.innerHTML = strMessage;
    $('#myModalSuccess').modal('show');
}

 /*Runtime Calculate Total Price */
$("#GRUPrice, #GRQty").keyup(function () {
    var total = 0;
    var x = $("#GRUPrice").val();
    var y = $("#GRQty").val();

    var total = x * y;

    $("#GRTPrice").val(total);
});

/*Item Generation js*/
function FillSubCat(lstCategoryCtrl, lstSubCatId) {
    var lstSub = $("#" + lstSubCatId);
    lstSub.empty();

    var selectedCategory = lstCategoryCtrl.options[lstCategoryCtrl.selectedIndex].value;

    if (selectedCategory != null && selectedCategory != '') {
        $.getJSON("/StoreIGen/GetCatBySub", { SCId: selectedCategory }, function (subCat) {
            if (subCat != null && !jQuery.isEmptyObject(subCat)) {
                $.each(subCat, function (index, sub) {
                    lstSub.append($('<option/>',
                        {
                            value: sub.value,
                            text: sub.text
                        }));
                });
            };
        });
    }
    return;
}


/*Emp Attendance time calculation*/
var start = document.getElementById("start").value;
var end = document.getElementById("end").value;

document.getElementById("start").onchange = function () { diff(start, end) };
document.getElementById("end").onchange = function () { diff(start, end) };


function diff(start, end) {
    start = document.getElementById("start").value; //to update time value in each input bar
    end = document.getElementById("end").value; //to update time value in each input bar

    const startTime = new Date(`1970-01-01T${document.getElementById('start').value}:00`);
    const endTime = new Date(`1970-01-01T${document.getElementById('end').value}:00`);
    const timeDiff = endTime.getTime() - startTime.getTime();
    const timeDiffMinutes = Math.floor(timeDiff / 1000 / 60);

    return timeDiffMinutes;

}
setInterval(function () {
    console.log(diff(start, end));
    document.getElementById("HREATMinute").value = diff(start, end);
}, 1000);

