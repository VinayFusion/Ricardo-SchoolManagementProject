var logged_In_UserType_Global = 1;
var logged_In_UserTypeName_Global = "Admin";
var currentSession = "";
var CurrentYear = new Date().getFullYear();
var NextYear = CurrentYear + 1;
currentSession = CurrentYear + " - " + NextYear; //`${CurrentYear} - ${NextYear}`
var UserToken_Global = "";
var CurrentDate_formatted_date = "";
var Student_Global_ID = 0;
var AnnualPending_Global = 0;

function GetIdFromDic(dic, val) {
    var id = 0;
    Object.entries(dic).forEach(([key, value]) => {
        if (typeof value == 'object') {
            if (value.hasOwnProperty(val)) { id = key; }
        }
        else {
            if (value == val) { id = key; }
        }
    });
    return id;
}
function FineCalculate(dic) {
    let total = 0;
    if (Object.keys(dic).length != 0) {
        let maxVal = 0;
        Object.entries(dic).forEach(([key, value]) => {
            if (value >= 0) {
                maxVal = value > maxVal ? value : maxVal;
            }
        });
        total = maxVal * 10
        $('#txtFine_ManagePayFee').val(total);
    }
    else if (Object.keys(dic).length == 0) {
        $('#txtFine_ManagePayFee').val(0);
    }
}
$(document).ready(function () {
    StartLoading();
    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {

            UserToken_Global = dataAdminToken;

            logged_In_UserType_Global = 1;
            logged_In_UserTypeName_Global = "Admin";

            GetDdlSessionDataForFilter(currentSession);
            GetMultipleDataForddl('PaymentTypes', 'ddlPaymentMethod_ManagePayFee');
            //GetFilterPayFeeData();
            // GetAllFeeTypes();
            StopLoading();


        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Staff";

                    payFeeTable = $("#tblPayFee_ManagePayFee").DataTable();
                    GetDdlSessionDataForFilter(currentSession);
                    GetMultipleDataForddl('PaymentTypes', 'ddlPaymentMethod_ManagePayFee');
                    //GetFilterPayFeeData();
                    // GetAllFeeTypes();
                    StopLoading();
                }
                else {

                }
            });
        }
    });
});

var arrSelectedFeeType = [];
var arrSelectedFeeTypeAmount = [];
const monthsShort = { 'Jan': '01', 'Feb': '02', 'Mar': '03', 'Apr': '04', 'May': '05', 'Jun': '06', 'Jul': '07', 'Aug': '08', 'Sep': '09', 'Oct': '10', 'Nov': '11', 'Dec': '12', };
var DicSelectedMonth = {};
var totalMonthchecked = 0;
var monthDay = {};
//----------------------------------start Working on Feetype click on ChkBox and Month click on ChkBox ---------------------------------
function TriggerLastInsertedTab(val) {
    if (arrTableRow.length != 0) {
        for (var i = 0; i < arrTableRow.length; i++) {
            var obj = Object.keys(FeeType_Dic[arrTableRow[arrTableRow.length - 1]]);
            if (obj[0] != 'Annual') {
                if ($('#chk' + obj[0] + '_ManagePayFee').is(':checked')) {
                    $('#' + obj[0].toLowerCase() + '-tab').click();
                    break;
                }
            }
        }
    }
}
function IsMonthly() {
    debugger;
    if ($('#chkMonthly_ManagePayFee').is(':checked')) {
        AddRowInTable(GetIdFromDic(FeeType_Dic, "Monthly"));
        DicSelectedMonth.Monthly = "";
        $('#monthly-tab').css('display', 'block');
        $('#monthly-tab').click();
    }
    else {
        $('._Monthly').each(function () {
            if ($(this).is(':checked')) {
                $(this).prop({ 'checked': false }).trigger('change');
            }
        });
        $('#monthly-tab').css('display', 'none');
        var element = document.getElementById("monthly-tab");
        element.classList.remove("text-danger");
        DeleteRowTable("FeeType_" + GetIdFromDic(FeeType_Dic, "Monthly"));
        delete DicSelectedMonth.Monthly;
        TriggerLastInsertedTab();
    }
    ClickOnFeeType();
}
function IsAnnual() {
    if ($('#chkAnnual_ManagePayFee').is(':checked')) {
        GetAnnualPendingAmount();

    }
    else {
        DeleteRowTable("FeeType_" + GetIdFromDic(FeeType_Dic, "Annual"));
    }
    ClickOnFeeType();
}
function IsTransport() {
    if ($('#chkTransport_ManagePayFee').is(':checked')) {
        AddRowInTable(GetIdFromDic(FeeType_Dic, "Transport"));

        DicSelectedMonth.Transport = "";
        $('#transport-tab').css('display', 'block');
        $('#transport-tab').click();
    }
    else {
        $('._Transport').each(function () {
            if ($(this).is(':checked')) {
                $(this).prop({ 'checked': false }).trigger('change');
            }
        });
        $('#transport-tab').css('display', 'none');
        var element = document.getElementById("transport-tab");
        element.classList.remove("text-danger");
        DeleteRowTable("FeeType_" + GetIdFromDic(FeeType_Dic, "Transport"));
        delete DicSelectedMonth.Transport;
        TriggerLastInsertedTab();
    }
    ClickOnFeeType();
}
function IsActivity() {
    if ($('#chkActivity_ManagePayFee').is(':checked')) {
        AddRowInTable(GetIdFromDic(FeeType_Dic, "Activity"));
        DicSelectedMonth.Activity = "";
        $('#activity-tab').css('display', 'block');
        $('#activity-tab').click();
    }
    else {
        $('._Activity').each(function () {
            if ($(this).is(':checked')) {
                $(this).prop({ 'checked': false }).trigger('change');
            }
        });
        $('#activity-tab').css('display', 'none');
        var element = document.getElementById("activity-tab");
        element.classList.remove("text-danger");
        DeleteRowTable("FeeType_" + GetIdFromDic(FeeType_Dic, "Activity"));
        delete DicSelectedMonth.Activity;
        TriggerLastInsertedTab();
    }
    ClickOnFeeType();
}
//----------------------------------End Working on Feetype click on ChkBox and Month click on ChkBox ---------------------------------



function ClickOnFeeType() {
    //-------Month Card Hide unhide
    $(".errorsClass").html('');
    if ($('#chkMonthly_ManagePayFee').is(':checked') || $('#chkTransport_ManagePayFee').is(':checked') || $('#chkActivity_ManagePayFee').is(':checked')) {
        $('#Monthcard_ManagePayFee').attr('hidden', false);
    }
    else {
        $('#Monthcard_ManagePayFee').attr('hidden', true);
    }
    //----clear discount and Fine unchecked all fee Type
    if ($('#chkMonthly_ManagePayFee').is(':checked') || $('#chkTransport_ManagePayFee').is(':checked') || $('#chkActivity_ManagePayFee').is(':checked') || $('#chkAnnual_ManagePayFee').is(':checked')) {
    } else {
        $('#txtFine_ManagePayFee').val(0);
        $('#txtDiscount_ManagePayFee').val(0);
    }
}
function clickOnMonthBox(data) {
    $(".errorsClass").html('');
    var months = [];
    var ids = $(data).attr('id');
    var arrId = ids.split('_');
    //------add month name in months array -----
    $('._' + arrId[0]).each(function () {
        var id = $(this).attr('id');
        var arr = id.split('_');
        if (!$('#' + id + '').is(':disabled')) {
            if ($('#' + id + '').is(':checked')) {
                months.push(arr[2]);
                //------------calculate fine ----------------------
                var monthvalue = monthsShort[arr[2]];
                let date_2 = new Date();
                let date_1 = new Date(monthvalue + '/11/' + date_2.getFullYear());
                const days = (date_1, date_2) => {
                    let difference = date_2.getTime() - date_1.getTime();
                    let TotalDays = Math.ceil(difference / (1000 * 3600 * 24));
                    return TotalDays;
                }
                monthDay[arr[2]] = days(date_1, date_2);
                //-----------------------------------------------
            }
            else {
                var index = months.indexOf(arr[2]);
                if (index > -1) {
                    months.splice(index, 1);
                }
                //----For Fine --------
                if (!$('#Activity_chk_' + arr[2]).is(':checked') && !$('#Monthly_chk_' + arr[2]).is(':checked') && !$('#Transport_chk_' + arr[2]).is(':checked')) {
                    delete monthDay[arr[2]];
                }
                //-------------------
            }
        }
        //----For Fine --------
        FineCalculate(monthDay);
        //-------------------
    });
    //------add month array in DicSelectedMonth-------
    DicSelectedMonth[arrId[0]] = months;
    if (DicSelectedMonth[arrId[0]].length != 0) {
        var element = document.getElementById(arrId[0].toLowerCase() + "-tab");
        element.classList.remove("text-danger");
    }
    else {
        var element = document.getElementById(arrId[0].toLowerCase() + "-tab");
        element.classList.add("text-danger");
    }
    //---------------------------
    //----Calculated amount and show in Fee Type table in Payable Amount coloumn-----
    var feeAmountMonth = 0;
    var feeAmountAnnual = 0;
    var feeAmountTransport = 0;
    var feeAmountActivity = 0;
    var id = 0;
    switch (arrId[0]) {
        case "Monthly": id = GetIdFromDic(FeeType_Dic, "Monthly"); feeAmountMonth = $('#BaseFeeAmount_' + id).text(); break;
        case "Annualy": id = GetIdFromDic(FeeType_Dic, "Annualy"); feeAmountAnnual = $('#BaseFeeAmount_' + id).text(); break;
        case "Transport": id = GetIdFromDic(FeeType_Dic, "Transport"); feeAmountTransport = $('#BaseFeeAmount_' + id).text(); break;
        case "Activity": id = GetIdFromDic(FeeType_Dic, "Activity"); feeAmountActivity = $('#BaseFeeAmount_' + id).text(); break;
    }
    var feeAmount = feeAmountMonth != 0 ? feeAmountMonth : feeAmountTransport != 0 ? feeAmountTransport : feeAmountActivity != 0 ? feeAmountActivity : 0;
    feeAmount = parseInt(feeAmount) * (months.length != 0 ? months.length : 1);

    if (id != 0 && id != GetIdFromDic(FeeType_Dic, "Annualy")) {
        $("#CalAmount_" + id + "").text(feeAmount);
    }
    else if (id != 0 && id == GetIdFromDic(FeeType_Dic, "Annualy")) {
        var val = feeAmountAnnual != 0 ? feeAmountAnnual : 0;
        $('#BaseFeeAmount_' + id).text(val);
        $("#CalAmount_" + id + "").text(val);
    }
    CalculateTotalAmount();
}

//----------------------------------End Working on Feetype click on ChkBox and Month click on ChkBox---------------------------------
//----------------------------------Start working on Fee Type Table in Add New PayFee---------------------------------
var FeeType_Dic = {}; //---AlL--- FeeTypeId :[FeeTypeName:FeeTypeAmount]
var PaidMonthFeeType_Dic = {}; //---If paid amount then -- FeeTypeId :[FeeTypeName:"Jan,Feb..etc"]
var arrTableRow = [];
function DeleteRowTable(id) {
    $(".errorsClass").html('');
    var idd = id;
    var splitIdd = idd.split('_');
    arrTableRow = $.grep(arrTableRow, function (value) {
        return value != splitIdd[1];
    });
    $("#" + id + "").parents("tr").remove();
    CalculateTotalAmount();
}
//----Monthly is 1 ,Yearly is 2, Transport is 3, Activity is 4---------
function AddRowInTable(rowNumber) {
    $(".errorsClass").html('');
    arrTableRow.push(rowNumber);
    var row = '<tr>' +
        '<td><label class="_FeeTypes" id="FeeType_' + rowNumber + '"></label></td>' +
        '<td><label class=" _BaseFeeAmount" id="BaseFeeAmount_' + rowNumber + '"></label></td>' +
        '<td><label class=" _CalAmount text-center" name = "_CalAmount" id="CalAmount_' + rowNumber + '"></td>' +
        '</tr>';
    $("#tbl_tablevalues").append(row);
    AppendFeeTypeData("FeeType_" + rowNumber);

};
function AppendFeeTypeData(DDlIdName) {
    var idd = DDlIdName.split('_');
    var value = $("#" + DDlIdName + "").html();
    if (value == null || value == "" || value == undefined) {
        var dic = FeeType_Dic;
        var typevalues = "";
        for (feeId in dic) {
            for (feeType in dic[feeId]) {
                var type = feeType.trim();
                if (type == 'Monthly' && $('#chkMonthly_ManagePayFee').is(':checked') && DDlIdName == "FeeType_" + GetIdFromDic(FeeType_Dic, "Monthly")) {
                    typevalues = type;
                    amountvalues = dic[feeId][feeType];
                }
                else if (type == 'Annual' && $('#chkAnnual_ManagePayFee').is(':checked') && DDlIdName == "FeeType_" + GetIdFromDic(FeeType_Dic, "Annual")) {
                    typevalues = type;
                    amountvalues = dic[feeId][feeType];
                }
                else if (type == 'Transport' && $('#chkTransport_ManagePayFee').is(':checked') && DDlIdName == "FeeType_" + GetIdFromDic(FeeType_Dic, "Transport")) {
                    typevalues = type;
                    amountvalues = dic[feeId][feeType];
                }
                else if (type == 'Activity' && $('#chkActivity_ManagePayFee').is(':checked') && DDlIdName == "FeeType_" + GetIdFromDic(FeeType_Dic, "Activity")) {
                    typevalues = type;
                    amountvalues = dic[feeId][feeType];
                }

            }
        }
        $("#" + DDlIdName + "").text(typevalues);
        $("#BaseFeeAmount_" + idd[1] + "").text(amountvalues);

        if (idd[1] == GetIdFromDic(FeeType_Dic, "Annual")) {
            var A1 = AnnualPending_Global > 0 ? AnnualPending_Global : amountvalues;
            $("#CalAmount_" + idd[1] + "").text(A1);
        }
        else {
            $("#CalAmount_" + idd[1] + "").text(amountvalues);
        }
        CalculateTotalAmount();
    }
}
$(document).on("keyup", "[name=_CalAmount]", function (e) {
    CalculateTotalAmount();
});
$(document).on("keyup", "[name=_FineAmount]", function (e) {
    CalculateTotalAmount();
});
$(document).on("keyup", "[name=_DiscountAmount]", function (e) {
    CalculateTotalAmount();
});
$(document).on("keyup", "[name=_FeeToPayAmount]", function (e) {
    CalculateAmountOnChange('PendingFee');
});
//-----calculate Grand Total ---------------------
function CalculateTotalAmount() {
    var totalCalAmount = 0;
    var Grandtotal = 0;
    $("._CalAmount").each(function () {
        if ($(this).text() != "") {
            totalCalAmount += parseInt($(this).text());
        }
    });
    //$("._FineAmount").each(function () {
    //    if ($(this).text() != "") {
    //        totalTotalFine += parseInt($(this).text());
    //    }
    //});
    //$('#txtFine_ManagePayFee').text(totalTotalFine);
    var totalTotalFine = $('#txtFine_ManagePayFee').val();
    var totalTotalDiscount = $('#txtDiscount_ManagePayFee').val();
    Grandtotal = parseInt(totalCalAmount) + parseInt(totalTotalFine != "" ? totalTotalFine : 0) - parseInt(totalTotalDiscount != "" ? totalTotalDiscount : 0);
    $('#txtGrandTotalAmount_ManagePayFee').text(Grandtotal).trigger('change');
};

//-----calculate Fee to Pay And Pending Fee ---------------------
function CalculateAmountOnChange(ChangeTextboxName) {
    $('#pendingFee_error_ManagePayFee').html('');
    var totalAmount = 0;
    var Grandtotal = $('#txtGrandTotalAmount_ManagePayFee').text();
    var totalPending = 0;
    var totalTotalFine = $('#txtFine_ManagePayFee').val();
    var totalTotalDiscount = $('#txtDiscount_ManagePayFee').val();
    //---------------fee to paid calculate --------
    $("._CalAmount").each(function () {

        if ($(this).text() != "") {
            totalAmount += parseInt($(this).text());
        }
        else {
            totalAmount = 0;
        }
    });
    if (ChangeTextboxName == "FeeToPay") {
        $('#txtTotalPaid_ManagePayFee').val(parseInt(totalAmount != "" ? totalAmount : 0) + parseInt(totalTotalFine != "" ? totalTotalFine : 0) - parseInt(totalTotalDiscount != "" ? totalTotalDiscount : 0)).trigger('change');
    }
    //---------------------------------------------------------------------------------------
    if (ChangeTextboxName == "PendingFee") {
        var PayToFee = $('#txtTotalPaid_ManagePayFee').val();
        totalPending = parseInt(Grandtotal != "" ? Grandtotal : 0) - parseInt(PayToFee != "" ? PayToFee : 0);
        if (totalPending < 0) {
            $('#pendingFee_error_ManagePayFee').html('Enter valid amount less then Fee To Pay')
        }
        if (totalPending > 0) {
            $('#PendingDate_ManageStudent').prop('disabled', false);
            $('#txtRemarks_ManagePayFee').prop('disabled', false);
        }
        else {
            $('#PendingDate_ManageStudent').prop('disabled', true);
            $('#txtRemarks_ManagePayFee').prop('disabled', true);
        }
        $('#txtpendingAmount_ManagePayFee').val(totalPending);
    }
}
//----------------------------------End working on Fee Type Table in Add New PayFee---------------------------------

function DefaultValues() {
    $('#txtGrandTotalAmount_ManagePayFee').text('0');

    $('#txtFine_ManagePayFee').val('0');

    $("#ddlStudent_ManagePayFee").html('');

    $('#txtReceipt_ManagePayFee').val('');

    $('#ddlPaymentMethod_ManagePayFee').val('0').trigger('change');

    $('#txtReferenceNo_ManagePayFee').val('');


    $("#txtTotalPaid_ManagePayFee").val('0');
    $("#txtpendingAmount_ManagePayFee").val('0');
    $("#txtRemarks_ManagePayFee").val('');

    $("#txtDiscount_ManagePayFee").val(0);
    $("#txtFine_ManagePayFee").val(0);

    //--table Default Value 
    //$('#txtEndDate_ManagePayFee').datepicker('option', 'minDate', new Date().toJSON().slice(0, 10).replace(/-/g, '/'));

    //--Disable Fields
    $('#txtReferenceNo_ManagePayFee').attr('disabled', true);
    $("#txtPaidOnDate_ManagePayFee").prop('disabled', true);
    //$("#txtTotalPaid_ManagePayFee").prop('disabled', true);
    $("#txtpendingAmount_ManagePayFee").prop('disabled', true);
    $("#txtRemarks_ManagePayFee").prop('disabled', true);
    $(".errorsClass").html('');
}

function GotoOnTableFormHideButton() {
    $("#btnPayFee").show();  //--Hide Add-New-PayFee Button
    $("#dv_PayFeeForm").hide(); //--Show the Insert Update-PayFee-Box only
    $("#dv_PayFeeListBox").show(); // -- hide table Form    
    arrTableRow = [];
    Session_Id_Global = 0;
    MonthlyDisabled = 0;
    yearlyDisabled = 0;
    Student_Global_ID = 0;
    AnnualPending_Global = 0;
    arrSelectedFeeType = [];
    arrSelectedFeeTypeAmount = [];
    totalMonthchecked = 0;
    monthDay = {};
    PaidMonthFeeType_Dic = {}; //---If paid amount then -- FeeTypeId :[FeeTypeName:"Jan,Feb..etc"] and disabled month
    arrTableRow = [];

    //----------------uncheck and disabled and delete row----
    $('#chkMonthly_ManagePayFee').prop({ 'disabled': true, 'checked': false }).trigger('change');
    $('#chkAnnual_ManagePayFee').prop({ 'disabled': true, 'checked': false }).trigger('change');
    $('#chkTransport_ManagePayFee').prop({ 'disabled': true, 'checked': false }).trigger('change');
    $('#chkActivity_ManagePayFee').prop({ 'disabled': true, 'checked': false }).trigger('change');
    //-----------------

    DefaultValues();
}

function GotoOnInsertFormHideButton(InsertFormTitle, Action) {
    //---------------Set Current-Date In the Paid-On Field-------------------
    const _date = new Date();
    var dd = _date.getDate();
    var mm = _date.getMonth() + 1;

    var yyyy = _date.getFullYear();
    if (dd < 10) {
        dd = "0" + dd;
    }
    if (mm < 10) {
        mm = "0" + mm;
    }
    CurrentDate_formatted_date = yyyy + "-" + mm + "-" + dd;
    $("#txtPaidOnDate_ManagePayFee").val(CurrentDate_formatted_date);
    //-----------------------------------------------------------------------
    $("#Title_PayFeeForm_ManagePayFee").html(InsertFormTitle); //--Set the Form-Title in InsertUpdate
    $("#btnPayFee").hide();  //--Hide Add-New-PayFee Button
    $("#dv_PayFeeForm").show(); //--Show the Insert Update-PayFee-Box only
    $("#dv_PayFeeListBox").hide(); // -- hide table Form 
    if (Action == "Update") {
        $("#btnSubmitManagePayFee").hide();
        // $("#btnUpdateManagePayFee").show();
    }
    else {
        $("#btnSubmitManagePayFee").show();
        //$("#btnUpdateManagePayFee").hide();
    }
    // $('#txtEndDate_ManagePayFee').datepicker('option', 'minDate', new Date().toJSON().slice(0, 10).replace(/-/g, '/'));
}

function PayFee_ShowForm() {
    if (logged_In_UserType_Global == 2) {
        $("#txtPaidOnDate_ManagePayFee").prop('disabled', true);
    }
    GotoOnInsertFormHideButton('Pay Course Fee', "Insert");
    // GetAllClassFeeTypes();
    GetPaidMonths();
}

function CancelForm() {
    GotoOnTableFormHideButton();
}

function ChangePaymentMethod() {
    var Type = $('#ddlPaymentMethod_ManagePayFee').find(':Selected').text();
    if (Type != 'Select PaymentTypes' && Type != 'Cash') {
        $('#txtReferenceNo_ManagePayFee').attr('disabled', false);
    }
    else {
        $('#txtReferenceNo_ManagePayFee').attr('disabled', true);
        $('#txtReferenceNo_ManagePayFee').val('');
    }
}

function FindStudentIDPopUp() {

    swal({
        title: "Finding Student Record",
        text: "Enter Student Id",
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        animation: "slide-from-top",
        inputPlaceholder: "Enter Student Id"
    }, function (inputValue) {
        if (inputValue === null) return false;

        if (inputValue === "") {
            $.iaoAlert({
                msg: 'Please enter Id in input field if you want search!',
                type: "error",
                mode: "dark",
            });
            return false;
        }
        if (inputValue === false) {

        }
        if (inputValue != false && inputValue != "" && inputValue != null) {
            GetStudentDetailByID(inputValue);
        }
    });
}

function GetStudentDetailByID(sid) {
    var _is_valid = true;
    var id = sid //$('#StudentID_ManagePayFee').val();
    if (id == 0 || id == "0" || id == "" || id == null || id == undefined) {
        _is_valid = false;
        CustomSwalPoup("Message!", 'Student not equal to zero Or null Please enter ID', "warning");
    }
    else {
        Student_Global_ID = id
    }
    StartLoading();
    if (_is_valid == true) {
        document.getElementById("FeeTypeRow_ManagePayFee").innerHTML = '';
        $.ajax({
            type: "GET",
            url: "/GetStudentDetailByID?studentID=" + Student_Global_ID,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (response) {
                var AllData = response.data.student;
                if (AllData != null) {

                    $("#ProfileName_ManagePayFee").html(AllData.FirstName + " " + AllData.LastName);
                    $("#ProfileEmail_ManagePayFee").html(AllData.Email);
                    $("#imgProfile_ManagePayFee").attr('src', '/Content/StudentImages/' + AllData.ProfileImage);
                    //------------------------------------------------------
                    $('#StudentFatherLabel').html(AllData.FatherName);
                    $('#StudentMobileLabel').html(AllData.PhoneNumber);
                    $('#SessionLabel').html(AllData.SectionName);
                    $('#ClassLabel').html(AllData.ClassName);
                    $('#SectionLabel').html(AllData.SectionName);
                    //------------------------------------------------------
                    //-----For Dynamic FeeType show --------------------------------------
                    var feeIdArray = (AllData.FeeId).split(',');
                    var feeNameArray = (AllData.FeeName).split(',');
                    var feeAmountArray = (AllData.FeeAmount).split(',');
                    var div = "";
                    //-----------check student take transport service or not if not then hide transport checkbox through remove name--
                    if (AllData.HasTakenTransportService == 0) {
                        var ind1 = feeNameArray.indexOf("Transport");
                        if (ind1 > -1) {
                            feeIdArray = feeIdArray.slice(0, ind1);
                            feeNameArray = feeNameArray.slice(0, ind1);
                            feeAmountArray = feeAmountArray.slice(0, ind1);
                        }
                    }
                    else {
                        var ind2 = feeNameArray.indexOf("Transport");
                        //----Update Transport amount if student select transport and found in SessionFees Table
                        if (ind2 > -1) {
                            feeAmountArray[ind2] = AllData.TransportAmount;
                        }
                        //----Add Transport if not found in SessionFees Table and student select transport
                        else {
                            feeIdArray.push(0);
                            feeNameArray.push("Transport");
                            feeAmountArray.push(AllData.TransportAmount);
                        }
                    }
                    //------------------------------------------------
                    for (var b = 0; b < feeNameArray.length; b++) {
                        div += '<div class="col-md-3"><div class="form-check ">' +
                            ' <input type="checkbox" class="form-check-input _feetypechk" id="chk' + feeNameArray[b] + '_ManagePayFee" onchange="Is' + feeNameArray[b] + '();">' +
                            '<label class="form-check-label" for="chk' + feeNameArray[b] + '_ManagePayFee"  style="cursor:pointer;"><b>' + feeNameArray[b] + '</b><span id= "lab' + feeNameArray[b] + '_ManagePayFee"></span></label></div></div>'
                        FeeType_Dic[feeIdArray[b]] = {};
                        FeeType_Dic[feeIdArray[b]][feeNameArray[b]] = feeAmountArray[b];
                    }
                    document.getElementById("FeeTypeRow_ManagePayFee").innerHTML = div;
                    //------if pending fee zero then disable annual check box--------
                    if (AllData.PendingAmount == 0) {
                        $('#chkAnnual_ManagePayFee').prop('disabled', true);
                        $('#labAnnual_ManagePayFee').text('(paid)');
                    }
                    else {
                        $('#chkAnnual_ManagePayFee').prop('disabled', false);
                        $('#labAnnual_ManagePayFee').text('');
                    }
                    //-------------------------------------------------
                    //----------------------------------------------------------------------
                    PayFee_ShowForm();
                }
                else if (AllData == null) {
                    $('#chkIsExist_ManageStudent').prop('checked', false);
                    $.iaoAlert({
                        msg: 'You entered ID Not found, please try again Put other Id!',
                        type: "error",
                        mode: "dark",
                    });
                }
                $('html, body').animate({ scrollTop: 0 }, 1200);
                DisabledMonth(AllData.MonthName, AllData.MonthNumber, AllData.TotalMonth);
                StopLoading();
                swal.close();
            },
            error: function (result) {
                swal.close();
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

function GetAnnualPendingAmount() {
    var _is_valid = true;
    var sid = Student_Global_ID //$('#StudentID_ManagePayFee').val();
    if (sid == 0 || sid == "0" || sid == "" || sid == null || sid == undefined) {
        _is_valid = false;
        CustomSwalPoup("Message!", 'Student not equal to zero Or null Please enter ID', "warning");
    }
    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetAnnualPendingAmount?studentID=" + sid,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (response) {
                var AllData = response.data.payfee;
                if (AllData != null && AllData.Is_Paid != 1 && AllData.PendingAmount > 0) {
                    AnnualPending_Global = AllData.PendingAmount;
                }
                else {
                    AnnualPending_Global = 0;
                }
                AddRowInTable(GetIdFromDic(FeeType_Dic, "Annual"));
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

}

function GetPaidMonths() {
    var _is_valid = true;
    var sid = Student_Global_ID //$('#StudentID_ManagePayFee').val();
    if (sid == 0 || sid == "0" || sid == "" || sid == null || sid == undefined) {
        _is_valid = false;
        CustomSwalPoup("Message!", 'Student not equal to zero Or null Please enter ID', "warning");
    }
    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetPaidMonths?studentID=" + sid,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (response) {
                var AllData = response.data.payfee;
                if (AllData != null) {
                    for (var i = 0; i < AllData.length; i++) {
                        if (AllData[i].FeeTypeId != 0 && AllData[i].FeeTypeName != "" && AllData[i].MonthName != "") {
                            PaidMonthFeeType_Dic[AllData[i].FeeTypeId] = {};
                            PaidMonthFeeType_Dic[AllData[i].FeeTypeId][AllData[i].FeeTypeName] = AllData[i].MonthName;
                            var arrMonth = AllData[i].MonthName.split(',');
                            for (var a = 0; a < arrMonth.length; a++) {
                                switch (AllData[i].FeeTypeName) {
                                    case "Monthly": $("#Monthly_chk_" + arrMonth[a]).prop('disabled', true); break;
                                    case "Transport": $('#Transport_chk_' + arrMonth[a]).prop('disabled', true); break;
                                    case "Activity": $('#Activity_chk_' + arrMonth[a]).prop('disabled', true); break;
                                }
                            }
                        }
                    }
                }
                else {
                    AnnualPending_Global = 0;
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

}

function DisabledMonth(MonthName, MonthNumber, TotalMonth) {
    var AllMonth = [];
    $("._Month").each(function () {
        var id = $(this).attr('id');
        // var monthName = id.split('_');
        $("#" + id + "").prop('disabled', false);
        AllMonth.push(id);
    });

    for (var i = MonthNumber - 1; i >= (MonthNumber - TotalMonth); i--) {
        //$("#" + arrAllMonth[i] + "").prop('checked', true);
        $("#" + AllMonth[i] + "").prop('disabled', true);
    }
}

function GetFilterPayFeeData() {
    var _is_valid = true;
    var sessionName = "";
    var classId = $('#ddlSelectClass_Filter').val();
    var sessionId = $('#ddlSession_Filter').val();
    classId = classId == "" || classId == null || classId == undefined ? 0 : classId;
    sessionId = sessionId == "" || sessionId == null || sessionId == undefined ? 0 : sessionId;
    if (sessionId == "0") {
        $("#ddlSelectClass_Filter").prop('disabled', true);
        $("#ddlSelectClass_Filter").html('');
    }
    if (typeof sessionId == "string") {
        if (sessionId == currentSession || sessionId.indexOf("-") != -1) {
            sessionId = 0;
            sessionName = $('#ddlSession_Filter').find(':Selected').val();
            if (classId == 0) {
                $("#ddlSelectClass_Filter").prop('disabled', false);
            }
        }
    }

    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetFilterPayFeeData?classid=" + classId + "&sessionName=" + sessionName,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (Response) {
                var AllData = Response.data.PayFeedata;
                var sno = 0;
                var _delete = '';
                var print_receipt = '';
                var email_receipt = '';
                var _delete = '';
                var data = [];

                var _table = $('#tblPayFee_ManagePayFee').DataTable();
                _table.destroy();
                for (var i = 0; i < AllData.length; i++) {
                    sno++;
                    var student = AllData[i].StudentName + '<br/> (' + AllData[i].FatherName + ')';
                    print_receipt = '<span class="float-right badge bg-danger" style="float:none!important;cursor:pointer;" title="Print Receipt" onclick="PrintPayFeeReceipt(' + AllData[i].Id + ');"><i class="fas fa-print" style="margin:2px;font-size:15px;"></i></span>';
                    email_receipt = '<span class="float-right badge bg-danger" style="float:none!important;cursor:pointer;margin-left:3px;" title="Email Receipt" onclick="EmailReceipt(' + AllData[i].Id + ');"><i class="fas fa-envelope" style="margin:2px;font-size:15px;"></i></span>';
                    _delete = '<span class="float-right badge bg-danger" style="float:none!important;cursor:pointer;margin-left:3px;" title="Delete Receipt" onclick="ConfirmDeletePayFee(' + AllData[i].Id + ');"><i class="fas fa-trash-alt" style="margin:2px;font-size:15px;"></i></span>';

                    if (logged_In_UserType_Global == 1) {
                        data.push([
                            sno,
                            student,
                            AllData[i].TotalReceiptAmount,
                            AllData[i].TotalPaid,
                            AllData[i].PaidOn,
                            AllData[i].PendingAmount,
                            '<div style="width:115px;">' + print_receipt + email_receipt + _delete + '</div>'
                        ]);
                    }
                    else {
                        data.push([
                            sno,
                            student,
                            AllData[i].TotalReceiptAmount,
                            AllData[i].TotalPaid,
                            AllData[i].PaidOn,
                            AllData[i].PendingAmount,
                            _delete
                        ]);
                    }

                }

                $('#tblPayFee_ManagePayFee').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true
                });

                //GetAllFeeTypes();
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

function InsertUpdateManagePayFee(_mode) {

    var _studentId = parseInt(Student_Global_ID);
    var _paidOn = $("#txtPaidOnDate_ManagePayFee").val();
    var _totalFeeTypeAmount = 0;
    var _totalMonths = 0;
    var _totalFine = parseFloat($("#txtFine_ManagePayFee").val());
    var _totalDiscount = parseFloat($("#txtDiscount_ManagePayFee").val());
    var _totalReceiptAmount = parseFloat($("#txtGrandTotalAmount_ManagePayFee").text());
    var _totalPaid = parseFloat($("#txtTotalPaid_ManagePayFee").val());
    var _pendingAmount = parseFloat($("#txtpendingAmount_ManagePayFee").val());
    var _pendingDate = $("#PendingDate_ManageStudent").val();
    var _remark = $("#txtRemarks_ManagePayFee").val();
    var _paymentMethodId = $("#ddlPaymentMethod_ManagePayFee").val();
    var _referenceNumber = $("#txtReferenceNo_ManagePayFee").val();
    var _feeTypeId = "";
    var _feeTypeName = "";
    var _feeTypeAmount = "";
    var _monthlyMonthName = "";
    var _transportMonthName = "";
    var _activityMonthName = "";
    var _monthlyFine = 0;
    var _transportFine = 0;
    var _activityFine = 0;
    var _is_valid = true;
    $(".errorsClass").html('');
    var errorMsg = '';
    var TotalotherFeeAmount = 0; // except Annual fee amount --- sum only other seleted fee Types
    var otherFeeIsChecked = false; // except Annual fee amount --- true if other seleted fee Types
    $("._FeeTypes").each(function () {
        var id = $(this).attr('id');
        var feetypes = $('#' + id + '').text();
        arrSelectedFeeType.push(feetypes);
        for (feeId in FeeType_Dic) {
            for (feeType in FeeType_Dic[feeId]) {
                var type = feeType.trim();
                if (type == feetypes && ($('#chkMonthly_ManagePayFee').is(':checked') || $('#chkAnnual_ManagePayFee').is(':checked') || $('#chkTransport_ManagePayFee').is(':checked') || $('#chkActivity_ManagePayFee').is(':checked'))) {
                    arrSelectedFeeTypeAmount.push(FeeType_Dic[feeId][feeType]);
                    _totalFeeTypeAmount += parseFloat(FeeType_Dic[feeId][feeType]);
                    _feeTypeId += feeId + ',';
                    _feeTypeName += type + ',';
                    _feeTypeAmount += FeeType_Dic[feeId][feeType] + ',';
                    if (type != "Annual") {
                        TotalotherFeeAmount += parseFloat($('#CalAmount_' + GetIdFromDic(FeeType_Dic, type) + '').text());
                        otherFeeIsChecked = true;
                    }
                }
            }
        }
    });

    //--------------check month selected of select FeeType or not ----------------
    var val = '';
    if ($('#chkMonthly_ManagePayFee').is(':checked')) {
        if (DicSelectedMonth["Monthly"].length == 0) {
            _is_valid = false;
            var element = document.getElementById("monthly-tab");
            element.classList.add("text-danger");
            val = "Monthly";
        }
        else {
            _totalMonths += DicSelectedMonth["Monthly"].length;
            Object.entries(DicSelectedMonth["Monthly"]).forEach(([key, value]) => {
                _monthlyMonthName += value + ',';
            });
        }
    }
    if ($('#chkTransport_ManagePayFee').is(':checked')) {
        if (DicSelectedMonth["Transport"].length == 0) {
            _is_valid = false;
            var element = document.getElementById("transport-tab");
            element.classList.add("text-danger");
            val += " Transport";
        }
        else {
            _totalMonths += DicSelectedMonth["Transport"].length;
            Object.entries(DicSelectedMonth["Transport"]).forEach(([key, value]) => {
                _transportMonthName += value + ',';
            });
        }
    }
    if ($('#chkActivity_ManagePayFee').is(':checked')) {
        if (DicSelectedMonth["Activity"].length == 0) {
            _is_valid = false;
            var element = document.getElementById("activity-tab");
            element.classList.add("text-danger");
            val += " Activity";
        }
        else {
            _totalMonths += DicSelectedMonth["Activity"].length;
            Object.entries(DicSelectedMonth["Activity"]).forEach(([key, value]) => {
                _activityMonthName += value + ',';
            });
        }
    }
    if (_is_valid == false && val != '') {
        $.iaoAlert({
            msg: 'Please Select at least one Month in ' + val,
            type: "error",
            mode: "dark",
        });
    }
    //---------------------------------------------------------------

    //--------show error if fee to pay less then total amount of other feeType amount
    debugger;
    $("#PendingDate_error_ManageStudent").html('');
    if ($('#chkAnnual_ManagePayFee').is(':checked') && otherFeeIsChecked == true) {
        var amount = TotalotherFeeAmount + parseFloat($('#txtFine_ManagePayFee').val()) - parseFloat($('#txtDiscount_ManagePayFee').val())
        if (parseFloat(_totalPaid) < amount) {
            //$('#txtTotalPaid_ManagePayFee').val(TotalotherFeeAmount).trigger('change');
            _is_valid = false;
            $("#TotalPaid_error_ManagePayFee").html('Total Paid Amount not less than ' + amount);
        }
    }
    else {
        //-----pending calucalte or must be zero if annual is unchecked
        var totalAmountPayable = $('#txtGrandTotalAmount_ManagePayFee').text();
        if (parseInt(_totalPaid) < parseInt(totalAmountPayable)) {
            //$('#txtTotalPaid_ManagePayFee').val(totalAmountPayable).trigger('change');
            _is_valid = false;
            $("#TotalPaid_error_ManagePayFee").html('Total Paid Amount not less than ' + totalAmountPayable);
        }
    }

    if (_studentId == undefined || _studentId == null || _studentId == '' || _studentId == 0) {
        _is_valid = false;
        errorMsg = 'student Id not null or empty';
    }
    if (_paidOn == undefined || _paidOn == null || _paidOn == '' || _paidOn == 0) {
        _is_valid = false;
        errorMsg = 'Paid on Date not null or empty';
    }
    if ($('#chkMonthly_ManagePayFee').is(':checked') || $('#chkTransport_ManagePayFee').is(':checked') || $('#chkActivity_ManagePayFee').is(':checked')) {
        if ((_totalMonths == undefined || _totalMonths == null || _totalMonths == '' || _totalMonths == 0) && val == '') {
            _is_valid = false;
            errorMsg = ' Please select at least one month';
        }
    }
    if (_totalReceiptAmount == undefined || _totalReceiptAmount == null || _totalReceiptAmount == '' || _totalReceiptAmount == 0) {
        _is_valid = false;
        errorMsg = 'Total Amount Payable not null or empty';
    }
    if (_totalPaid == undefined || _totalPaid == null || _totalPaid == '' || _totalPaid == 0) {
        _is_valid = false;
        $("#TotalPaid_error_ManagePayFee").html('Total Paid Amount not null or empty');

    }
    if (_pendingAmount == undefined || _pendingAmount < 0) {
        _is_valid = false;
        $("#PendingAmount_error_ManagePayFee").html('Pending Amount not null or Not less then 0');
    }
    else if ((_pendingDate == undefined || _pendingDate == null || _pendingDate == '') && _pendingAmount > 0) {
        _is_valid = false;
        $("#PendingDate_error_ManageStudent").html('Pending Fee Date not null or empty');
    }
    if (_paymentMethodId == undefined || _paymentMethodId == null || _paymentMethodId == '' || _paymentMethodId == 0) {
        _is_valid = false;
        $("#PaymentMethod_error_ManagePayFee").html('Please select the Payment method');
    }
    var type = $('#ddlPaymentMethod_ManagePayFee').find(':Selected').text();
    if (type != 'Select PaymentTypes' && type != 'Cash') {
        if (_referenceNumber == undefined || _referenceNumber == null || _referenceNumber == '' || _referenceNumber.replace(/\s/g, "") == "") {
            _is_valid = false;
            $("#ReferenceNo_error_ManagePayFee").html('Please enter Reference Number');
        }
    }

    if (_is_valid == true) {

        StartLoading();

        var data = new FormData();
        data.append("_studentId", _studentId);
        data.append("_paidOn", _paidOn);
        data.append("_totalFeeTypeAmount", _totalFeeTypeAmount);
        data.append("_totalMonths", _totalMonths);
        data.append("_totalFine", _totalFine);
        data.append("_totalDiscount", _totalDiscount);
        data.append("_totalReceiptAmount", _totalReceiptAmount);
        data.append("_totalPaid", _totalPaid);
        data.append("_pendingAmount", _pendingAmount);
        data.append("_pendingDate", _pendingDate);
        data.append("_remark", _remark);
        data.append("_paymentMethodId", _paymentMethodId);
        data.append("_referenceNumber", _referenceNumber);
        //----------ReceiptFeeType Table ----------------
        data.append("_feeTypeId", _feeTypeId);
        data.append("_feeTypeName", _feeTypeName);
        data.append("_feeTypeAmount", _feeTypeAmount);
        data.append("_monthlyMonthName", _monthlyMonthName);
        data.append("_transportMonthName", _transportMonthName);
        data.append("_activityMonthName", _activityMonthName);
        data.append("_monthlyFine", _monthlyFine);
        data.append("_transportFine", _transportFine);
        data.append("_activityFine", _activityFine);
        //---------------------------------------------
        data.append("_mode", _mode);

        $.ajax({
            url: '/InsertUpdateManageFee',
            headers: {
                "Authorization": "Bearer " + UserToken_Global
            },
            data: data,
            processData: false,
            mimeType: 'multipart/form-data',
            contentType: false,
            //contentType: 'application/json',
            type: 'POST',
            success: function (dataResponse) {
                //--Parse into Json of response-json-string
                dataResponse = JSON.parse(dataResponse);

                //--If successfully added/updated
                if (dataResponse.status == 1 || dataResponse.status == 2) {
                    CustomSwalPoup("Success!", dataResponse.message, "success");


                    GotoOnTableFormHideButton();
                    //--refresh PayFee list
                    GetFilterPayFeeData();
                    StopLoading();
                }
                else {
                    StopLoading();
                    CustomSwalPoup("Message!", dataResponse.message, "warning");
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
    else {
        if (errorMsg != '' && errorMsg != null) {
            $.iaoAlert({
                msg: errorMsg,
                type: "error",
                mode: "dark",
            });
        }
    }

}

function ConfirmDeletePayFee(pfid) {
    swal({
        title: "Delete Pay Fee",
        text: "Are you sure to delete this Pay Fee Record?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        DeletePayFee(pfid);
    });
}

function DeletePayFee(pfid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/DeletePayFeeById?PayFeeID=" + pfid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if PayFee successfully deleted
            if (dataResponse.status == 1) {
                //--Get PayFee List
                GetFilterPayFeeData();
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                }, 100);
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


function PrintPayFeeReceipt(_ID) {

    StartLoading();

    $.ajax({
        type: "GET",
        url: "/print/payfee/invoice?id=" + _ID,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (Response) {
            let AllData = Response.data.invoiceData;
            if (AllData) {

                let a = window.open("");
                a.document.write(AllData);

                setTimeout(function () {

                    StopLoading();

                    a.document.close();
                    a.print();
                }, 1000);
            }
            else {
                StopLoading();
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
var _ID_SendInoiveReceiptEmail_Global = 0;
function EmailReceipt(_ID) {
    StartLoading();

    $("#txtEmail_ManagePayFee_Modal").val('');
    $("#email_FeeModal_error_ManagePayFee").html('');

    //--Set Row-Id in the Global Variable
    _ID_SendInoiveReceiptEmail_Global = _ID

    $.ajax({
        type: "GET",
        url: "/GetPayFeeDetailById?id=" + _ID,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (Resopnse) {
            let AllData = Response.data.feeData;
            StopLoading();

            if (AllData) {
                $("#txtEmail_ManagePayFee_Modal").val(AllData.StudentEmail);
                //--Open Modal Popup
                $("#btn_SendReceipt_PayFee_Modal").click();
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

function ConfirmSendReceiptEmail() {

    var _email = $("#txtEmail_ManagePayFee_Modal").val();
    var email_test = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var _is_valid = true;
    $("#email_FeeModal_error_ManagePayFee").html('');

    if (_email == '' || _email.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#email_FeeModal_error_ManagePayFee").html('Please enter the email address');
    }
    else if (!email_test.test(_email)) {
        _is_valid = false;
        $("#email_FeeModal_error_ManagePayFee").html('Please enter the valid email address!');
    }

    if (_is_valid == true) {
        swal({
            title: "Are you sure?",
            text: "You are going to send this Invoice as Email to this Student?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: '#DD6B55',
            confirmButtonText: 'YES, SEND IT',
            cancelButtonText: "CANCEL"
        }, function (isConfirm) {
            if (!isConfirm) return;
            SendReceiptEmail();
        });
    }
}

function SendReceiptEmail() {
    var _email = $("#txtEmail_ManagePayFee_Modal").val();

    StartLoading();

    var data = new FormData();
    data.append("Id", _ID_SendInoiveReceiptEmail_Global);
    data.append("studentEmail", _email.toLowerCase());

    $.ajax({
        url: "/send/payfee/invoice",
        headers: {
            "Authorization": "Bearer " + UserToken_Global
        },
        data: data,
        processData: false,
        mimeType: 'multipart/form-data',
        contentType: false,
        //contentType: 'application/json',
        type: 'POST',
        success: function (dataResponse) {

            StopLoading();

            //--Parse into Json of response-json-string
            dataResponse = JSON.parse(dataResponse);

            //--If successfully send
            if (dataResponse.status == 1) {
                swal("Invoice Sent!", dataResponse.message, "success");

                //--Set Row-Id in the Global Variable
                _ID_SendInoiveReceiptEmail_Global = 0;

                //--Close Modal Popup
                $("#btnClose_SendReceipt_PayFee_Modal").click();
            }
            else {
                swal("Message!", dataResponse.message, "warning");
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

