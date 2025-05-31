var UserToken_Global = "";
var logged_In_UserType_Global = 1;
var logged_In_UserTypeName_Global = "Admin";
var ActiveBranchForStaff_Global = 0;
var _CurrentDate_Global = '';

$(document).ready(function () {
    StartLoading();
    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {

            UserToken_Global = dataAdminToken;

            logged_In_UserType_Global = 1;
            logged_In_UserTypeName_Global = "Admin";
            StopLoading();
            //--Get Active Branches List
            //GetBranchsList();
        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Staff";
                    StopLoading();
                    //--Get Active Branches List
                    //GetBranchsList();
                }
                else {

                }
            });
        }
    });
});

function GetBranchsList() {

    var _url = '';

    //--if Admin Logged-in
    if (logged_In_UserType_Global == 1) {
        _url = "/GetAllActiveBranches";
    }
    else {
        _url = "/GetActiveBranchOfStaff";
    }

    $.ajax({
        type: "GET",
        url: _url,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataBranches) {

            var res_Branches_Filter = '<option value="0">All</option>';

            for (var i = 0; i < dataBranches.data.branches.length; i++) {

                res_Branches_Filter += '<option value="' + dataBranches.data.branches[i].Id + '">' + dataBranches.data.branches[i].Name + '</option>';
            }

            //--if Admin Logged-in
            if (logged_In_UserType_Global == 1) {
                $("#ddlBranch_Filter_ManagePendingFee").append(res_Branches_Filter);
            }

            //--Get Pending-Fee List Data
            GetPendingFeeListData();
        },
        error: function (result) {
            //--Get Pending-Fee List Data
            GetPendingFeeListData();

            if (result["status"] == 401) {
                $.iaoAlert({
                    msg: 'Unauthorized! Invalid Token!',
                    type: "error",
                    mode: "dark",
                });
            }
            else {
                $.iaoAlert({
                    msg: 'There is some technical error, please try again!',
                    type: "error",
                    mode: "dark",
                });
            }
        }
    });
}

function GetPendingFeeListData() {

    //var _url = "";

    ////--if Admin Logged-In
    //if (logged_In_UserType_Global == 1) {
    //    _url = "/GetPendingFeeData";
    //}
    //else {
    //    _url = "/GetPendingFeeDataByStaff";
    //}

    //GetPendingFeeData_FromAPI(_url);

    $.get("/Admin/GetCurrentDate", null, function (dataCurrentDate) {
        $("#_txtFromDate").val('');
        $("#_txtToDate").val('');

        //--Disable all previous dates from the start-date
        $('#_txtToDate').datepicker('option', 'minDate', dataCurrentDate);

        //--Set Current Date into the Global variable
        _CurrentDate_Global = dataCurrentDate;

        FilterPendingFeeDataByBranch();
    });
}

function FilterByBranch() {
    StartLoading();
    FilterPendingFeeDataByBranch();
}

function FilterPendingFeeDataByBranch() {
    StartLoading();

    var _from_date = '';
    var _to_date = '';
    var _branch_id = $("#ddlBranch_Filter_ManagePendingFee").val();
    //--if staff-member logged-In
    if (logged_In_UserType_Global == 2) {

        _from_date = $("#_txtFromDate").val();
        _to_date = $("#_txtToDate").val();
        _branch_id = ActiveBranchForStaff_Global; // Added
    }
    else {
        _from_date = $("#_txtFromDate").val();
        _to_date = $("#_txtToDate").val();
        _branch_id = (_branch_id) ? _branch_id : 0;
    }

    var _Params = {
        fromDate: _from_date,
        toDate: _to_date,
        branchID: _branch_id
    }

    var _url_val = "";

    if (logged_In_UserType_Global == 1) {
        var _branch_id_Filter = $("#ddlBranch_Filter_ManagePendingFee").val();

        if (_branch_id_Filter != undefined && _branch_id_Filter != null && _branch_id_Filter != '' && _branch_id_Filter != '0' && _branch_id_Filter != 0) {
            _url_val = "/GetPendingFeeDataByBranch?branchID=" + _branch_id_Filter;
        }
        else {
            _url_val = "/GetPendingFeeData";
        }
    }
    else if (logged_In_UserType_Global == 2) {
        _url_val = "/GetPendingFeeDataByStaff";
    }
    GetPendingFeeData_FromAPI(_url_val, _Params);
}

function GetPendingFeeData_FromAPI(_url, _Params) {
    $.ajax({
        type: "POST",
        url: _url,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        data: JSON.stringify(_Params),
        contentType: 'application/json',
        success: function (dataFee) {

            var sno = 0;
            var _student_info = '';

            var data = [];

            var _table = $('#tblPendingFeeData_ManagePendingFee').DataTable();
            _table.destroy();

            if (dataFee.data.feeData != null) {

                for (var i = 0; i < dataFee.data.feeData.length; i++) {

                    sno++;

                    _student_info = '<a href="/' + logged_In_UserTypeName_Global + '/StudentDetail?sid=' + dataFee.data.feeData[i].StudentId + '" target="_blank" style="color: red;cursor: pointer;"><i class="fas fa-external-link-alt" style="margin-right: 4px;font-size: 12px;"></i>' + dataFee.data.feeData[i].StudentName + ' (' + dataFee.data.feeData[i].StuentID_Formatted + ')</a>';
                    var _startDate = `<span style="display:none;">${moment(dataFee.data.feeData[i].StartDate_DateTimeFormat.split("T")[0]).format('YYYYMMDD')}</span>${dataFee.data.feeData[i].StartDate_FormatDate}`;
                    var _endDate = `<span style="display:none;">${moment(dataFee.data.feeData[i].EndDate_DateTimeFormat.split("T")[0]).format('YYYYMMDD')}</span>${dataFee.data.feeData[i].EndDate_FormatDate}`;

                    data.push([
                        sno,
                        _student_info,
                        dataFee.data.feeData[i].CourseName,
                        dataFee.data.feeData[i].PendingFees,
                        _startDate, //dataFee.data.feeData[i].StartDate_FormatDate,
                        _endDate//dataFee.data.feeData[i].EndDate_FormatDate
                    ]);
                }

                var commonExportOptions = {
                    stripHtml: false, //used to allow html edits on export option like print. 
                    format: {
                        body: function (data, row, column, node) {
                            // Strip anchor tag from student name column to show only name not link
                            if (column === 1) {
                                // regX to remove all html tags from data and get only text.
                                var regX = /(<([^>]+)>)/ig;
                                return data.replace(regX, '');
                            }
                            else {
                                return data;
                            }

                        }
                    }
                }

                $('#tblPendingFeeData_ManagePendingFee').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true,
                    dom: "<'row my-3'<'col-sm-12'B>><'row'<'col-sm-6'l><'col-sm-6'f>><'row'<'col-sm-12'tr>><'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                    buttons: [
                        {
                            extend: 'print',
                            exportOptions: commonExportOptions,
                            customize: function (win) {
                                $(win.document.body).find('th').css('color', 'black');
                            }
                        }
                    ]
                    // scrollX: true
                });
            }

            StopLoading();
        },
        error: function (result) {
            StopLoading();

            if (result["status"] == 401) {
                $.iaoAlert({
                    msg: 'Unauthorized! Invalid Token!',
                    type: "error",
                    mode: "dark",
                });
            }
            else {
                $.iaoAlert({
                    msg: 'There is some technical error, please try again!',
                    type: "error",
                    mode: "dark",
                });
            }
        }
    });
}