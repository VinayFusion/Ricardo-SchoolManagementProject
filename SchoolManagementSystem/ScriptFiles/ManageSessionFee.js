var logged_In_UserType_Global = 0;
var logged_In_UserTypeName_Global = "";
var UserToken_Global = "";
var FeeTypesdata = '';
var arrTableRow = [];
var rowNumber = 1;
var SessionFee_Id_Global = 0;
var Session_Id_Global = 0;
var currentSession = "";

$(document).ready(function () {
    StartLoading();
    var CurrentYear = new Date().getFullYear();
    var NextYear = CurrentYear + 1;
    currentSession = CurrentYear + " - " + NextYear; //`${CurrentYear} - ${NextYear}`;

    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {
            UserToken_Global = dataAdminToken;
            logged_In_UserType_Global = 1;
            logged_In_UserTypeName_Global = "Admin";
            InsertOneRow();
            GetDdlSessionDataForFilter(currentSession);
           
        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {
                    UserToken_Global = dataStaffToken;
                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Staff";
                    InsertOneRow();
                    GetDdlSessionDataForFilter(currentSession);
                }
                else {

                }
            });
        }
    });
    //---------------------- Delete row on delete button click---
    $(document).on("click", ".DeleteRow", function () {
        var idd = $(this).attr('id');
        var rowCount = $('#tbl_tablevalues tr').length;
        if (rowCount > 2 && idd != 'DeleteRow_1') {
            console.log(arrTableRow);
            var splitIdd = idd.split('_');
            console.log(splitIdd);
            arrTableRow = $.grep(arrTableRow, function (value) {
                return value != splitIdd[1];
            });
            console.log(arrTableRow);
            $(this).parents("tr").remove();
        }
        else {
            $('#ddlFeeTypes_1').val('0').trigger('change');
            $('#Fees_1').val('');
            $('#FeeTypeRemark_1').val('');

        }
    });

    //---------------------Add New Row in Table----
    $("#AddNewRow").click(function () {
        FeeTypesdata = FeeTypesdata != undefined && FeeTypesdata != '' ? FeeTypesdata : $('#ddlFeeTypes_1').html();
        AddRowInTable(FeeTypesdata);
    });
});


//---------------------------------------------------------Start working on Fee Type Table in Add New PayFee-------------------------------------------------------
//---------------input validation Number----
var validNumber = new RegExp(/^\d*\.?\d*$/);
var lastValid = document.getElementsByClassName("_Amount").value;
function validateNumber(elem) {
    var _id = $(elem).attr('id');
    ;
    if (validNumber.test(elem.value)) {
        lastValid = elem.value;
    } else {
        if (lastValid != undefined) {
            elem.value = lastValid;
        }
        else {
            elem.value = '';
        }
    }
}

//-------------Insert One row---
function InsertOneRow() {
    arrTableRow.push(rowNumber);
    var actions = '<a class="DeleteRow" id ="DeleteRow_' + rowNumber + '"  title="Delete" data-toggle="tooltip"><i class="fa fa-trash"></i></a>';
    var row = '<tr>' +
        '<td><select class="select2 select2-danger form-control" data-dropdown-css-class="select2-danger" style="width: 100%;" id="ddlFeeTypes_' + rowNumber + '"></select><span id="ddlFeeTypes_error_' + rowNumber + '" class="errorsClass"></span></td>' +
        '<td><input type="text" class="form-control _Amount"  name = "_Amount" placeholder="Enter Fee Amount" id="Fees_' + rowNumber + '"oninput="validateNumber(this);"/><span id="Fees_error_' + rowNumber + '" class="errorsClass"></td>' +
        '<td><input type="text" class="form-control" id="FeeTypeRemark_' + rowNumber + '" placeholder="Enter Remark"/></td>' +
        '<td style="text-align:center;">' + actions + '</td>' +
        '</tr>';
    $("#tbl_tablevalues").append(row);
    var value = $('#ddlFeeTypes_1').html();
    if (value == null || value == "" || value == undefined) {
        GetMultipleDataForddl("FeeTypes", "ddlFeeTypes_1");
    }
}

//---------------- Append table with add row form on add new button click --
function AddRowInTable(resFeeTypes) {
    var rowCount = $('#tbl_tablevalues tr').length;
    var datatypeCount = FeeTypesdata.split("</option>").map(Number);
    if (rowCount < (datatypeCount.length) - 1) {
        var feetype;
        if (resFeeTypes != undefined && resFeeTypes != '') {
            feetype = resFeeTypes;
        }
        else {
            feetype = FeeTypesdata;
        }
        rowNumber++;
        arrTableRow.push(rowNumber);
        var actions = '<a class="DeleteRow" id ="DeleteRow_' + rowNumber + '"  title="Delete" data-toggle="tooltip"><i class="fa fa-trash"></i></a>';
        var row = '<tr>' +
            '<td><select class="select2 select2-danger form-control" data-dropdown-css-class="select2-danger" style="width: 100%;" id="ddlFeeTypes_' + rowNumber + '">' + feetype + '</select><span id="ddlFeeTypes_error_' + rowNumber + '" class="errorsClass"></span></td>' +
            '<td><input type="text" class="form-control _Amount" name = "_Amount" placeholder="Enter Fee Amount" id="Fees_' + rowNumber + '"oninput="validateNumber(this);"/><span id="Fees_error_' + rowNumber + '" class="errorsClass"></td>' +
            '<td><input type="text" class="form-control" id="FeeTypeRemark_' + rowNumber + '" placeholder="Enter Remark" /></td>' +
            '<td style="text-align:center;">' + actions + '</td>' +
            '</tr>';
        $("#tbl_tablevalues").append(row);

        FeeTypesdata = feetype;
    }
};
//---------------------------------------------------------End working on Fee Type Table in Add New PayFee-------------------------------------------------------


function DefaultValues() {
    $('#ddlSelectClass_ManageSessionFee').val('0').trigger('change');

    $("#ddlSession_ManageSessionFee").html('');
    // $("#ddlSession_ManageSessionFee").append('<option value="0">Select Session of Class</option>');

    $("#txtSessionToFee_ManageSessionFee").val('');
    $("#txtpendingFee_ManageSessionFee").val('0');
    $("#FeeTypeRemark_1").val('');

    //--table Default Value 
    $('#ddlFeeTypes1').val('0').trigger('change');
    $('#Fees_1').val('');
    $('#FeeTypeRemark1').val('');
    $("#DeleteRow_1").css("pointer-events", "auto");
    $("#AddNewRow").removeClass('d-none');
    $('#Div_tbl_tablevalues').removeClass('col-md-12');
    $('#Div_tbl_tablevalues').addClass('col-md-11');

    $(".errorsClass").html('');
}

function GotoOnTableFormHideButton() {
    $("#btnSessionFee").show();  //--Show Add-New-SessionFee Button
    $("#dv_SessionFeeForm").hide(); //--Hide the Insert Update-SessionFee-Box only
    $("#dv_SessionFeeListBox").show(); // -- Show table Form 

    //----------------------table value---------------------------------
    rowNumber = 1;
    $("#tbl_tablevalues > tbody").empty();
    var rowCount = $('#tbl_tablevalues tr').length;
    if (rowCount == 1) {
        InsertOneRow();
    }
    arrTableRow = [1];
    //-----------------------------------------------------------------
    DefaultValues();
    SessionFee_Id_Global = 0;
    Session_Id_Global = 0;
}

function GotoOnInsertFormHideButton(InsertFormTitle, Action) {
    $("#Title_SessionFeeForm_ManageSessionFee").html(InsertFormTitle); //--Set the Form-Title in InsertUpdate
    $("#btnSessionFee").hide();  //--Hide Add-New-SessionFee Button
    $("#dv_SessionFeeForm").show(); //--Show the Insert Update-SessionFee-Box only
    $("#dv_SessionFeeListBox").hide(); // -- hide table Form 
    if (Action == "Update") {
        $("#btnSubmitManageSessionFee").hide();
        $("#btnUpdateManageSessionFee").show();
    }
    else {
        $("#btnSubmitManageSessionFee").show();
        $("#btnUpdateManageSessionFee").hide();
    }
}

function SessionFee_ShowForm() {
    GotoOnInsertFormHideButton('Session Course Fee', "Insert");
    DefaultValues();
}

function CancelForm() {
    GotoOnTableFormHideButton();
}

function SessionBindByClass() {
    var _ClassId = parseInt($("#ddlSelectClass_ManageSessionFee").val());
    var _is_valid = true;
    $(".errorsClass").html('');
    if (_ClassId == undefined || _ClassId == null || _ClassId == '' || _ClassId == 0) {
        _is_valid = false;
        $('#SelectClass_error_ManageSessionFee').html('Please select class!');
        $("#ddlSession_ManageSessionFee").html('');
    }
    if (_is_valid == true && _ClassId !== 0) {

        $.ajax({
            url: '/SessionBindByClass4SessionFee?classId=' + _ClassId,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            type: 'Get',
            success: function (datasessionFee) {

                var res_Sessionfee = ' <option value = "0" > Select Session of Selected Class</option> ';
                for (var i = 0; i < datasessionFee.data.SessionFee.length; i++) {

                    res_Sessionfee += '<option value="' + datasessionFee.data.SessionFee[i].SessionId + '">' + datasessionFee.data.SessionFee[i].SessionName + '</option>';
                }

                $("#ddlSession_ManageSessionFee").html('');
                $("#ddlSession_ManageSessionFee").append(res_Sessionfee);
                if (Session_Id_Global !=0) {
                $("#ddlSession_ManageSessionFee").val(Session_Id_Global).trigger('change');
                }
            },
            error: function (result) {

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

function InsertUpdateSessionFee(_mode) {
    $(".errorsClass").html('');
    var _is_valid = true;
    var ClassId = $('#ddlSelectClass_ManageSessionFee').val();
    var SessionId = $('#ddlSession_ManageSessionFee').val();
    var FeeTypes = "";
    var FeeAmount = "";
    var Remark = "";
    for (var i = 0; i < arrTableRow.length; i++) {
        var feetype = $('#ddlFeeTypes_' + arrTableRow[i]).val();
        var feeamount = $('#Fees_' + arrTableRow[i]).val();
        //if (feetype == "" && feetype == 0 && feetype == null && feetype == undefined) {
        //    $('#').val();
        //}
        if (feetype == undefined || feetype == null || feetype == "" || feetype == "0") {
             _is_valid = false;
            $("#ddlFeeTypes_error_" + arrTableRow[i]).html('Please select the Fee Type!');
        }
        if (feeamount == undefined || feeamount == null || feeamount == "") {
            _is_valid = false;
            $("#Fees_error_" + arrTableRow[i]).html('Please enter Amount!');
        }
        FeeTypes += feetype + "~";
        FeeAmount += feeamount + "~";
        Remark += $('#FeeTypeRemark_' + arrTableRow[i]).val() + "~";
    }

    if (ClassId == undefined || ClassId == null || ClassId == '' || ClassId == 0) {
        _is_valid = false;
        $("#SelectClass_error_ManageSessionFee").html('Please select the Class!');
    }
    if (SessionId == undefined || SessionId == null || SessionId == '' || SessionId == 0) {
        _is_valid = false;
        $("#Session_error_ManageSessionFee").html('Please select the Session!');
    }

    if (FeeTypes == undefined || FeeTypes == null || FeeTypes == '') {
        _is_valid = false;
        $.iaoAlert({
            msg: 'Please enter requried(*) field in Table row!',
            type: "error",
            mode: "dark",
        });
    }
    if (FeeAmount == undefined || FeeAmount == null || FeeAmount == " " || FeeAmount == "0") {
        _is_valid = false;
        $.iaoAlert({
            msg: 'Please enter requried(*) field in Table row!',
            type: "error",
            mode: "dark",
        });
    }
    if (_is_valid == true) {

        StartLoading();

        var data = new FormData();
        ;
        data.append("Id", SessionFee_Id_Global);
        data.append("ClassId", ClassId);
        data.append("SessionId", SessionId);
        data.append("FeeTypeId", FeeTypes);
        data.append("FeeAmount", FeeAmount);
        data.append("Remark", Remark);
        data.append("mode", _mode);

        $.ajax({
            url: '/InsertUpdateSessionFee',
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
                ;
                //--Parse into Json of response-json-string
                dataResponse = JSON.parse(dataResponse);
                GotoOnTableFormHideButton();

                Filter_GetSessionFeeList('','');

                //--If successfully added/updated
                if (dataResponse.status == 1 || dataResponse.status == 2) {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                }
                else {
                    CustomSwalPoup("Warning!", dataResponse.message, "warning");
                }
                Session_Id_Global = 0;

            },
            error: function (result) {
                Filter_GetSessionFeeList('', '');
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

function EditSessionFeeInfo(sid) {
    StartLoading();
    console.log($("#ddlSession_ManageSessionFee").html());
    SessionFee_Id_Global = sid;
    $.ajax({
        type: "GET",
        url: "/GetDetailOfSessionFee?sessionfeeId=" + SessionFee_Id_Global,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataSessionFee) {
            ;
            if (dataSessionFee.data.SessionFee != null) {
                debugger;
                $("#ddlSelectClass_ManageSessionFee").val(parseInt(dataSessionFee.data.SessionFee[0].ClassId)).trigger('change');
                Session_Id_Global = parseInt(dataSessionFee.data.SessionFee[0].SessionId);
                $("#ddlFeeTypes_1").val(parseInt(dataSessionFee.data.SessionFee[0].FeeTypeId)).trigger('change');
                $("#Fees_1").val(dataSessionFee.data.SessionFee[0].FeeAmount);
                $("#FeeTypeRemark_1").val(dataSessionFee.data.SessionFee[0].Remark);
                $("#DeleteRow_1").css("pointer-events", "none");
                $("#AddNewRow").addClass('d-none');
                $('#Div_tbl_tablevalues').removeClass('col-md-11');
                $('#Div_tbl_tablevalues').addClass('col-md-12');
                GotoOnInsertFormHideButton('Update SessionFee Information', "Update");

            }
            else if (dataSessionFee.data.SessionFee == null) {
                $.iaoAlert({
                    msg: 'No Session fee Found make Sure you pass Session Id not null!',
                    type: "error",
                    mode: "dark",
                });
            }
            $('html, body').animate({ scrollTop: 0 }, 1200);
            StopLoading();
            swal.close();
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

function Filter_GetSessionFeeList() {
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
            url: "/GetFilterAllSessionFee?classid=" + classId + "&sessionName=" + sessionName,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (dataSessionFee) {

                var sno = 0;
                var _edit = '';
                var data = [];

                var _table = $('#tblSessionFee_ManageSessionFee').DataTable();
                _table.destroy();

                for (var i = 0; i < dataSessionFee.data.SessionFee.length; i++) {
                    sno++;
                    _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Session Fee-Information" onclick="EditSessionFeeInfo(' + dataSessionFee.data.SessionFee[i].Id + ');" />';
                    if (logged_In_UserType_Global == 1) {
                        data.push([
                            sno,
                            dataSessionFee.data.SessionFee[i].SessionName,
                            dataSessionFee.data.SessionFee[i].ClassName,
                            dataSessionFee.data.SessionFee[i].FeeTypeName,
                            dataSessionFee.data.SessionFee[i].FeeAmount,
                            dataSessionFee.data.SessionFee[i].Remark,
                            _edit,
                        ]);
                    }
                    else {
                        data.push([
                            sno,
                            dataSessionFee.data.SessionFee[i].SessionName,
                            dataSessionFee.data.SessionFee[i].ClassName,
                            dataSessionFee.data.SessionFee[i].FeeTypeName,
                            dataSessionFee.data.SessionFee[i].FeeAmount,
                            dataSessionFee.data.SessionFee[i].Remark,
                            _edit,
                        ]);
                    }

                }

                $('#tblSessionFee_ManageSessionFee').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true
                });
                var value = $('#ddlFeeTypes_1').html();
                if (value == null || value == "" || value == undefined) {
                    GetMultipleDataForddl("FeeTypes", "ddlFeeTypes_1");
                }
                GetDdlClassAllData("ddlSelectClass_ManageSessionFee");
                StopLoading();
            },
            error: function (result) {
                var value = $('#ddlFeeTypes_1').html();
                if (value == null || value == "" || value == undefined) {
                    GetMultipleDataForddl("FeeTypes", "ddlFeeTypes_1");
                }
                GetDdlClassAllData("ddlSelectClass_ManageSessionFee");
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
