function getExRate(id) {

    var lstbox = document.getElementById('dropdownExDay');
    var txtExrate = document.getElementById('txtExchangedays');

    var items = lstbox.options;
    for (var i = items.length - 1; i >= 0; i--) {
        if (items[i].value == id.value) {
            txtExrate.value = items[i].text;
            return;
        }
    }
    return;
}



document.addEventListener('change', function () {
    let date1 = new Date($("#HRLDLeaveSDate").val());
    let date2 = new Date($("#HRLDLeaveEDate").val());


    if (date1 > date2) {
        alert('Start Date is gratter than End Date')
    }

    if (date1.getTime() && date2.getTime()) {
        let diff = date2.getTime() - date1.getTime();
        let dayDifferance = diff / (1000 * 3600 * 24);



        $("#HRLDTDay").val(dayDifferance + 1);
    }
});