
function DateChangeFun(value) {

    if (value == 'StartDateChangeFun') {
        var _startDate_val = $("#_txtFromDate").val();
        $("#_txtToDate").val('');
        $('#_txtToDate').datepicker('option', 'minDate', _startDate_val);
    }
    else if (value == 'EndDateChangeFun') {
        var _from_date = $("#_txtFromDate").val();
        var _to_date = $("#_txtToDate").val();

        if (_from_date == undefined || _from_date == null || _from_date == '') {
            $.iaoAlert({
                msg: 'Please select From-Date!',
                type: "error",
                mode: "dark",
            });

            return false;
        }
        else if (_to_date == undefined || _to_date == null || _to_date == '') {
            $.iaoAlert({
                msg: 'Please select To-Date!',
                type: "error",
                mode: "dark",
            });

            return false;
        }
        else {
            //$('#btnShowAllRecordsFilter').show();
            $('#btnShowAllRecordsFilter').removeClass('d-none');
            FilterByBranch();
        }
    }
}

function showAllRecords() {
    $("#_txtToDate").val('');
    $("#_txtFromDate").val('');
    $('#btnShowAllRecordsFilter').addClass('d-none');
    FilterByBranch();
}
