var UserToken_Global = "";
var _RevenueData_Printable_SendInEmail = "";
var logged_In_UserType_Global = 1;
var _CurrentDate_Global = '';
var logged_In_UserTypeName_Global = "Admin";
var TotalRevenue_Global = 0; //Added
var TotalExpenditure_Global = 0; //Added
var ActiveBranchForStaff_Global = 0; //Added

$(document).ready(function () {
    StartLoading();
    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {

            UserToken_Global = dataAdminToken;

            logged_In_UserType_Global = 1;
            logged_In_UserTypeName_Global = "Admin";

            StopLoading();
            //--Get All Active Branches
            //GetBranchsList();
        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Staff";

                    
                    $('#btnShowAllRecordsFilter').hide();
                    StopLoading();
                    //--Get All Active Branches
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
                $("#ddlBranch_Filter_ManageRevenue").append(res_Branches_Filter);
            }
            else if (logged_In_UserType_Global == 2) {
                //-- if staff logged in
                ActiveBranchForStaff_Global = dataBranches.data.branches[0].Id;
            }

            //--Get Revenue List Records
            GetRevenueListData();
        },
        error: function (result) {
            //--Get Revenue List Records
            GetRevenueListData();

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

function GetRevenueListData() {
    $.get("/Admin/GetCurrentDate", null, function (dataCurrentDate) {
        //$("#txtFromDate_ManageRevenue").val(dataCurrentDate);
        //$("#txtToDate_ManageRevenue").val(dataCurrentDate);

        ////--Disable all previous dates from the start-date
        //$('#txtToDate_ManageRevenue').datepicker('option', 'minDate', dataCurrentDate);

        //--if staff-member logged-In
        if (logged_In_UserType_Global == 2) {
            $("#_txtFromDate").prop("disabled", true);
            $("#_txtToDate").prop("disabled", true);
        }

        $("#_txtFromDate").val(dataCurrentDate);
        $("#_txtToDate").val(dataCurrentDate);

        //--Disable all previous dates from the start-date
        $('#_txtToDate').datepicker('option', 'minDate', dataCurrentDate);

        //--Set Current Date into the Global variable
        _CurrentDate_Global = dataCurrentDate;

        FilterRevenueListData();
    });
}

function FilterRevenueListData() {

   var _from_date_MR = '';
    var _to_date_MR = '';
    var _branch_id_MR = $("#ddlBranch_Filter_ManageRevenue").val();

    //--if staff-member logged-In
    if (logged_In_UserType_Global == 2) {

        _from_date_MR = _CurrentDate_Global;
        _to_date_MR = _CurrentDate_Global;
        //_branch_id_MR = 0;
        _branch_id_MR = ActiveBranchForStaff_Global; // Added
    }
    else {
        _from_date_MR = $("#_txtFromDate").val();
        _to_date_MR = $("#_txtToDate").val();
        _branch_id_MR = (_branch_id_MR) ? _branch_id_MR : 0;
    }

    var _Params = {
        fromDate: _from_date_MR,
        toDate: _to_date_MR,
        branchID: _branch_id_MR
    }

    //--Hide Print Buttons
    $("#dv_PrintableRevenueButtons_Section").hide();
    $("#tbody_RevenueData_Printable").html('');
    $("#span_TotalRevenue_ManageRevenue_Printable").html('Rs. 0');
    _RevenueData_Printable_SendInEmail = "";

    $.ajax({
        url: "/FilterRevenueData",
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        data: JSON.stringify(_Params),
        processData: false,
        contentType: 'application/json',
        type: 'POST',
        success: function (dataFee) {

            var sno = 0;
            var _student_info = '';
            var _total_revenue = 0;
           
            var data = [];
            var res_revenueData_print = '';

            var _table = $('#tblRevenueData_ManageRevenue').DataTable();
            _table.destroy();

            if (dataFee.data.feeData != null) {

                if (dataFee.data.feeData.length > 0) {
                    //--Show Print Buttons
                    $("#dv_PrintableRevenueButtons_Section").show();
                }

                for (var i = 0; i < dataFee.data.feeData.length; i++) {

                    sno++;

                    _total_revenue = _total_revenue + dataFee.data.feeData[i].PaidFees;

                    _student_info = '<a href="/' + logged_In_UserTypeName_Global + '/StudentDetail?sid=' + dataFee.data.feeData[i].StudentId + '" target="_blank" style="color: red;cursor: pointer;"><i class="fas fa-external-link-alt" style="margin-right: 4px;font-size: 12px;"></i>' + dataFee.data.feeData[i].StudentName + ' (' + dataFee.data.feeData[i].StuentID_Formatted + ')</a>';
                    var _paidOnDate = `<span style="display:none;">${moment(dataFee.data.feeData[i].PaidOn_FormatDate).format('YYYYMMDD')}</span>${dataFee.data.feeData[i].PaidOn_FormatDate}`;
                    
                    data.push([
                        sno,
                        dataFee.data.feeData[i].ReceiptNumber,
                        _student_info,
                        dataFee.data.feeData[i].PaidFees,
                        _paidOnDate//dataFee.data.feeData[i].PaidOn_FormatDate
                    ]);

                    //-------------------------For Print--------------------------------
                    res_revenueData_print += "<tr>" +
                        "<td style='text-align:center;border:1px solid #ccc;padding-top:4px;padding-bottom:4px;'>" + sno + "</td>" +
                        "<td style='text-align:center;border:1px solid #ccc;padding-top:4px;padding-bottom:4px;'>" + dataFee.data.feeData[i].ReceiptNumber + "</td>" +
                        "<td style='text-align:center;border:1px solid #ccc;padding-top:4px;padding-bottom:4px;'>" + dataFee.data.feeData[i].StudentName + " (" + dataFee.data.feeData[i].StuentID_Formatted + ")</td>" +
                        "<td style='text-align:center;border:1px solid #ccc;padding-top:4px;padding-bottom:4px;'>" + dataFee.data.feeData[i].PaidFees + "</td>" +
                        "<td style='text-align:center;border:1px solid #ccc;padding-top:4px;padding-bottom:4px;'>" + dataFee.data.feeData[i].PaidOn_FormatDate + "</td>" +
                        "</tr>";
                    //------------------------------------------------------------------
                }

                _total_revenue = parseFloat(_total_revenue).toFixed(2); //Added
                TotalRevenue_Global = _total_revenue;  //Added

                $('#tblRevenueData_ManageRevenue').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true
                    //dom: 'Bfrtip',
                    //buttons: [
                    //    {
                    //        extend: 'print',
                    //        exportOptions: {
                    //            columns: [0, 1, 2, 3, 4]
                    //        },
                    //        customize: function (win) {
                    //            $(win.document.body).find('th').css('color', 'black');
                    //        }
                    //    }
                    //]
                    // scrollX: true
                });

                $("#span_TotalRevenue_ManageRevenue").html('Rs. ' + _total_revenue);

                //--for print
                $("#tbody_RevenueData_Printable").append(res_revenueData_print);
                $("#span_TotalRevenue_ManageRevenue_Printable").html('Rs. ' + _total_revenue);
                //-------For send preintable revenue-data---------
                if (dataFee.data.feeData.length > 0) {
                    _RevenueData_Printable_SendInEmail = "<div style='font-size:22px;padding-top:10px;padding-bottom:15px;'>" +
                        "Revenue Data" +
                        "</div>" +
                        "<table style='width: 100%; border-collapse: collapse;'>" +
                        "<thead>" +
                        "<tr>" +
                        "<th style='text-align: center;border:1px solid #ccc;padding-top:4px;padding-bottom:4px;'>#</th>" +
                        "<th style='text-align: center; border: 1px solid #ccc; padding-top: 4px; padding-bottom: 4px;'>Receipt-Number</th>" +
                        "<th style='text-align: center; border: 1px solid #ccc; padding-top: 4px; padding-bottom: 4px;'>Student</th>" +
                        "<th style='text-align: center; border: 1px solid #ccc; padding-top: 4px; padding-bottom: 4px;'>Paid Fee</th>" +
                        "<th style='text-align: center; border: 1px solid #ccc; padding-top: 4px; padding-bottom: 4px;'>Paid On</th>" +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>" +
                        res_revenueData_print +
                        "</tbody>" +
                        "</table>";
                        //"<div style='margin-top:20px;'>" +
                        //"<span style='font-weight:bold;font-size:15px;'>Total Revenue</span>" +
                        //"<span style='font-weight: bold; font-size: 20px;margin-left:10px;color:green;'>Rs. " + _total_revenue +"</span>" +
                        //"</div>";
                }
                //------------------------------------------------
            }
            FilterExpenditureData(_Params); //Added
            //StopLoading();
        },
        error: function (result) {
            FilterExpenditureData(_Params);

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

//function StartDateChangeFun() {
//    var _startDate_val = $("#txtFromDate_ManageRevenue").val();

//    $("#txtToDate_ManageRevenue").val('');
//    $('#txtToDate_ManageRevenue').datepicker('option', 'minDate', _startDate_val);
//}

//function EndDateChangeFun() {
//    var _from_date_MR = $("#txtFromDate_ManageRevenue").val();
//    var _to_date_MR = $("#txtToDate_ManageRevenue").val();

//    if (_from_date_MR == undefined || _from_date_MR == null || _from_date_MR == '') {
//        $.iaoAlert({
//            msg: 'Please select From-Date!',
//            type: "error",
//            mode: "dark",
//        });

//        return false;
//    }
//    else if (_to_date_MR == undefined || _to_date_MR == null || _to_date_MR == '') {
//        $.iaoAlert({
//            msg: 'Please select To-Date!',
//            type: "error",
//            mode: "dark",
//        });

//        return false;
//    }
//    else {
//        StartLoading();
//        FilterRevenueListData();
//    }
//}


function FilterByBranch() {
    StartLoading();
    FilterRevenueListData();
}

function PrintRevenueData() {
    var a = window.open("");
    a.document.write($("#dv_PrintRevenueData_Section").html());

    setTimeout(function () {

        a.document.close();
        a.print();
    }, 1000);
}

function SendRevenueData() {
    $("#txtEmail_ManageRevenue_Modal").val('');
    $("#email_error_ManageRevenue").html('');
    $("#btn_SendRevenueData_ManageRevenue_Modal").click();
}

function ConfirmSendRevenueDataEmail() {

    var _email = $("#txtEmail_ManageRevenue_Modal").val();
    var email_test = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var _is_valid = true;
    $("#email_error_ManageRevenue").html('');

    if (_email == '' || _email.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#email_error_ManageRevenue").html('Please enter the email address!');
    }
    else if (!email_test.test(_email)) {
        _is_valid = false;
        $("#email_error_ManageRevenue").html('Please enter the valid email address!');
    }

    if (_is_valid == true) {
        swal({
            title: "Are you sure?",
            text: "You are going to send the Revenue-Data to this Email?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: '#DD6B55',
            confirmButtonText: 'YES, SEND IT',
            cancelButtonText: "CANCEL"
        }, function (isConfirm) {
            if (!isConfirm) return;
            SendRevenueDataEmail();
        });
    }
}

function SendRevenueDataEmail() {

    StartLoading();

    var _email_MR = $("#txtEmail_ManageRevenue_Modal").val();
    var _revenueData_MR = _RevenueData_Printable_SendInEmail;

    if (_revenueData_MR == "") {
        $.iaoAlert({
            msg: 'Sorry, data is not valid!',
            type: "error",
            mode: "dark",
        });
        return false;
    }
    else {
        var _Params = {
            email: _email_MR,
            revenueData: _revenueData_MR
        }

        $.ajax({
            url: "/send/revenue/data",
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            data: JSON.stringify(_Params),
            processData: false,
            contentType: 'application/json',
            type: 'POST',
            success: function (dataResponse) {

                StopLoading();

                if (dataResponse.status == 1) {

                    $("#txtEmail_ManageRevenue_Modal").val('');
                    $("#email_error_ManageRevenue").html('');
                    $("#btnClose_ManageRevenue_Modal").click();

                    CustomSwalPoup("Success!", dataResponse.message, "success");
                }
                else {
                    $.iaoAlert({
                        msg: 'There is some technical error, please try again!',
                        type: "error",
                        mode: "dark",
                    });
                }
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
}

function FilterRevenueListDataByBranch() {
    StartLoading();

    FilterRevenueListData();
}

function FilterExpenditureData(_Params) {

    $.ajax({
        url: "/FilterExpenditureData",
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        data: JSON.stringify(_Params),
        processData: false,
        contentType: 'application/json',
        type: 'POST',
        success: function (dataExpenditure) {
            var _total_expenditures = 0;

            if (dataExpenditure.data.total_amount != null) {

                //if (dataExpenditure.data.expenditures.length > 0) {
                //    //--Show Print Buttons
                //    $("#dv_PrintableRevenueButtons_Section").show();
                //}

                //for (var i = 0; i < dataExpenditure.data.expenditures.length; i++) {
                //    _total_expenditures += dataExpenditure.data.expenditures[i].Amount;
                //}
                //console.log(_total_expenditures);
                _total_expenditures = dataExpenditure.data.total_amount;

            }
            if (parseFloat(_total_expenditures) > 0) {
                //--Show Print Buttons
                $("#dv_PrintableRevenueButtons_Section").show();
            }
            TotalExpenditure_Global = parseFloat(_total_expenditures).toFixed(2);
            var totalRemaining = parseFloat(TotalRevenue_Global - TotalExpenditure_Global).toFixed(2);
            $("#span_TotalExpenditure_ManageRevenue").html('Rs. ' + TotalExpenditure_Global);
            $("#span_TotalExpenditure_ManageRevenue_Printable").html('Rs. ' + TotalExpenditure_Global);
            $("#span_TotalRemaining_ManageRevenue").html('Rs. ' + totalRemaining);
            $("#span_TotalRemaining_ManageRevenue_Printable").html('Rs. ' + totalRemaining);

            if (parseFloat(totalRemaining) >= 0) {
                $("#span_TotalRemaining_ManageRevenue").css('color', 'green');
                $("#span_TotalRemaining_ManageRevenue_Printable").css('color', 'green');
            }
            else {
                $("#span_TotalRemaining_ManageRevenue").css('color', 'red');
                $("#span_TotalRemaining_ManageRevenue_Printable").css('color', 'red');
            }

            //--------------For Send Revenue-Email-----------
            _RevenueData_Printable_SendInEmail = _RevenueData_Printable_SendInEmail + `<table id="revenue_Calculation_Table_Printable" style="margin-top:20px;">
                <tbody>
            <tr>
                <td><span style="font-weight:bold;font-size:15px;">Total Revenue</span></td>
                <td style="padding-left: 50px; text-align: right;"><span style="font-weight: bold; font-size: 20px;margin-left:10px;color:green;float:right;">Rs. ${TotalRevenue_Global}</span></td>
            </tr>
            <tr>
                <td><span style="font-weight:bold;font-size:15px;">Total Expenditure</span></td>
                <td style="padding-left: 50px; text-align: right;"><span style="font-weight: bold; font-size: 20px;margin-left:10px;color:red;float:right;">Rs. ${TotalExpenditure_Global}</span></td>
            </tr>
            <tr>
                <td><span style="font-weight:bold;font-size:15px;">Revenue - Expenditure</span></td>
                <td style="padding-left: 50px; text-align: right;"><span style="font-weight: bold; font-size: 20px;margin-left:10px;color:green;float:right;">Rs. ${totalRemaining}</span></td>
            </tr>
        </tbody>
    </table>`;
            //------------------------------------------------
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