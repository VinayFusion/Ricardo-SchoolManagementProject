var UserToken_Global = "";
var Session_ID_Global = 0;
var logged_In_UserType_Global = 1;
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
            
            GetDdlSessionDataForFilter(currentSession);

        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;

                    GetDdlSessionDataForFilter(currentSession);
                }
            });
        }
    });
});

function SessionNameChange() {
    var ClassName = $("#ddlSelectClass_ManageSession option:selected").text();
    var StartYear = $("#ddlStartYear_ManageSession option:selected").text();
    var EndYear = $("#ddlEndYear_ManageSession option:selected").text();
    if (ClassName != 'Select Class' && StartYear !="" && EndYear != "") {
        $("#txtSessionName_ManageSession").val(StartYear + ' - ' + EndYear + ' ( ' + ClassName + ' )');
    }
}

function StartYearChange() {
    $("#ddlEndYear_ManageSession").html('');
    $('#ddlEndYear_ManageSession').select2('enable');
    var _startYear = parseInt($("#ddlStartYear_ManageSession").val()) + 1;

    // add year in End drop box
    for (var i = _startYear; i <= 2099; i++) {

        $("#ddlEndYear_ManageSession").append('<option value="' + i + '">' + i + '</option>');
    }

    SessionNameChange();
}

function AddNewSession_ShowForm() {
    $("#dv_SessionListBox").hide();
    $("#btnAddNewSession").hide();
    $("#dv_AddUpdateSessionForm").show();
    $("#txtSessionName_ManageSession").val('');
    $("#Title_SessionForm_ManageSession").html('Add New Session');

    
}

function CancelForm() {
    setSessionFormDefaultValues();

    $("#btnSubmitManageSession").show();
    $("#btnUpdateManageSession").hide();

    $("#dv_AddUpdateSessionForm").hide();
    $("#dv_SessionListBox").show();
    $("#btnAddNewSession").show();
}

function setSessionFormDefaultValues() {
    //-----------Set Default Values------------
    $("#txtSessionName_ManageSession").val('');
    $("#ddlStartYear_ManageSession").val('2000').trigger('change');
    $("#ddlEndYear_ManageSession").html('');
    $("#ddlEndYear_ManageSession").attr('disabled', 'disabled');
    $("#ddlSelectClass_ManageSession").val('0').trigger('change');
    $('#chkIsActive_ManageSession').prop('checked', true);

    //--Set Session-ID in the Global Variable
    Session_ID_Global = 0;
    //-------------------------------------

    $(".errorsClass").html('');
}

function GetAllClassForddl() {
    // add year in start drop box
    $("#ddlStartYear_ManageSession").html('');
    for (var i = 2000; i <= 2099; i++) {

        $("#ddlStartYear_ManageSession").append('<option value="' + i + '">' + i + '</option>');
    }
    GetDdlClassAllData("ddlSelectClass_ManageSession")
}

function GetSessionsList() {
    var _is_valid = true;
    var _sessionName = $('#ddlSession_Filter').find(':Selected').text().substring(0, 11);
    _sessionName = _sessionName == 'Select Sess' ? "" : _sessionName;
    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetAllSessions?sessionName=" + _sessionName,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (dataSessions) {
                StopLoading();
                var sno = 0;
                var _status = '';
                var _edit = '';
                var _delete = '';

                var data = [];

                var _table = $('#tblSessions_ManageSession').DataTable();
                _table.destroy();

                for (var i = 0; i < dataSessions.data.sessions.length; i++) {
                    sno++;
                    //---Check Session-Status
                    if (dataSessions.data.sessions[i].Status == 1) {
                        _status = '<a class="btn btn-success btn-sm" style="width:80px;" onclick="ConfirmChangeStatusSession(' + dataSessions.data.sessions[i].Id + ');">Active</a>';
                    }
                    else {
                        _status = '<a class="btn btn-danger btn-sm" style="width:80px;" onclick="ConfirmChangeStatusSession(' + dataSessions.data.sessions[i].Id + ');">In-Active</a>';
                    }

                    _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Session-Information" onclick="EditSessionInfo(' + dataSessions.data.sessions[i].Id + ');" />';
                    _delete = '<img src="/Content/Images/delete_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Delete Session" onclick="ConfirmDeleteSession(' + dataSessions.data.sessions[i].Id + ');" />';
                    if (logged_In_UserType_Global == 1) {
                        data.push([
                            sno,
                            dataSessions.data.sessions[i].ClassName,
                            dataSessions.data.sessions[i].SessionName,
                            _status,
                            _edit,
                            _delete
                        ]);
                    }
                    else {
                        data.push([
                            sno,
                            dataSessions.data.sessions[i].ClassName,
                            dataSessions.data.sessions[i].SessionName,
                            _status
                        ]);
                    }

                }

                $('#tblSessions_ManageSession').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true
                });
                GetAllClassForddl();
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
    else {

    }
}

function AddUpdateSession(_mode) {
    var _sessionName_MS = $("#txtSessionName_ManageSession").val();
    var _ClassID_ms = $("#ddlSelectClass_ManageSession").val();
    var _startYear_ms = $("#ddlStartYear_ManageSession").val();
    var _endYear_ms = $("#ddlEndYear_ManageSession").val();
    var _is_active_session = 0;
    if ($('#chkIsActive_ManageSession').is(':checked')) {
        // checked
        _is_active_session = 1;
    }

    var _is_valid = true;
    $(".errorsClass").html('');

    // Validations--------------
    if (_sessionName_MS == '' || _sessionName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#sessionName_error_ManageSession").html('Please enter the session name!');
    }
    if (_startYear_ms == '' || _startYear_ms.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#startYear_error_ManageSession").html('Please select the session Start Year!');
    }
    if (_endYear_ms == '' || _endYear_ms.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#endYear_error_ManageSession").html('Please select the session End Year!');
    }
    if (_ClassID_ms == '' || _ClassID_ms == null || _ClassID_ms == undefined) {
        _is_valid = false;
        $("#SelectClass_error_ManageSession").html('Please select the Class!');
    }
    
    //----------------

    if (_is_valid == true) {
        StartLoading();

        var data = new FormData();
        data.append("Id", Session_ID_Global);
        data.append("ClassId", _ClassID_ms);
        data.append("sessionName", _sessionName_MS);
        data.append("startYear", _startYear_ms);
        data.append("endYear", _endYear_ms);
        data.append("status", _is_active_session);
        data.append("mode", _mode);

        $.ajax({
            url: '/InsertUpdateSession',
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
                StopLoading();
                //--If successfully added/updated
                if (dataResponse.status == 1 || dataResponse.status == 2) {
                    CustomSwalPoup("Success!", dataResponse.message, "success");

                    // Set Default Values for Session Form
                    setSessionFormDefaultValues();

                    $("#btnSubmitManageSession").show();
                    $("#btnUpdateManageSession").hide();

                    $("#dv_AddUpdateSessionForm").hide();
                    $("#dv_SessionListBox").show();
                    $("#btnAddNewSession").show();


                    //--refresh courses list
                    GetSessionsList();
                }
                else {
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
}


function EditSessionInfo(sid) {
    StartLoading();
    Session_ID_Global = sid;
    $.ajax({
        type: "GET",
        url: "/GetSessionById?sessionID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataSession) {

            if (dataSession.data.session != null) {
                Session_ID_Global = dataSession.data.session.Id;
                $("#txtSessionName_ManageSession").val(dataSession.data.session.SessionName);
                $("#ddlSelectClass_ManageSession").val(dataSession.data.session.ClassId).trigger('change');
                $("#ddlStartYear_ManageSession").val(dataSession.data.session.StartYear).trigger('change');
                $("#ddlEndYear_ManageSession").val(dataSession.data.session.EndYear).trigger('change');

                if (dataSession.data.session.Status == 1) {
                    $('#chkIsActive_ManageSession').prop('checked', true);
                }
                else {
                    $('#chkIsActive_ManageSession').prop('checked', false);
                }
                
                //--Show Update Button only
                $("#btnSubmitManageSession").hide();
                $("#btnUpdateManageSession").show();

                //--Hide Add-New-Session Button
                $("#btnAddNewSession").hide();

                //--Set the Form-Name as (Update Session Information)
                $("#Title_SessionForm_ManageSession").html('Update Session Information');

                //--Show the Update-Session-Box only
                $("#dv_AddUpdateSessionForm").show();
                $("#dv_SessionListBox").hide();
            }

            $('html, body').animate({ scrollTop: 0 }, 1200);

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


function ConfirmChangeStatusSession(sid) {
    swal({
        title: "Change Status",
        text: "Are you sure to change status of this Session?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        ChangeStatusSession(sid);
    });
}


function ChangeStatusSession(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/ChangeSessionStatusById?sessionID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if session-status successfully updated
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get Session List
                    GetSessionsList();
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


function ConfirmDeleteSession(sid) {
    swal({
        title: "Delete Session",
        text: "Are you sure to delete this Session?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        DeleteSession(sid);
    });
}


function DeleteSession(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/DeleteSessionById?sessionID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();
            //--Check if course successfully deleted
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get Sessions List
                    GetSessionsList();
                }, 100);
            }
            else if (dataResponse.status == 2) {
                setTimeout(function () {
                    CustomSwalPoup("Warning!", dataResponse.message, "warning");
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

