var UserToken_Global = "";
var logged_In_UserType_Global = 1;
var Section_ID_Global = 0;
var Session_ID_Global = 0;
var Class_ID_Global = 0;
var arraySectionId;
var IsAddFormRequest = 0;// 0 its mean session Bind which doesn,t in section table
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
            GetMultipleDataForddl("Section", "ddlSection_ManageSection");
            GetDdlClassAllData("ddlSelectClass_ManageSection");
            GetDdlSessionDataForFilter(currentSession);
        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Staff";
                    GetMultipleDataForddl("Section", "ddlSection_ManageSection");
                    GetDdlClassAllData("ddlSelectClass_ManageSection");
                    GetDdlSessionDataForFilter(currentSession);
                }
                else {

                }
            });
        }
    });
});

function SessionBindByClass() {

    var _ClassId = parseInt($("#ddlSelectClass_ManageSection").val());


    var _is_valid = true;
    $(".errorsClass").html('');


    if (_ClassId == undefined || _ClassId == null || _ClassId == '' || _ClassId == 0) {
        _is_valid = false;
        $("#SelectClass_error_ManageSection").html('Please select class!');
    }
    if (_is_valid == true && _ClassId !== 0) {
        // ------url present in ManageSectionAPIController--------------
        $.ajax({
            url: '/SessionBindByClass?classId=' + _ClassId + '&IsAddFormRequest=' + IsAddFormRequest,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            type: 'Get',
            success: function (dataSection) {

                var res_Session = '';
                for (var i = 0; i < dataSection.data.Section.length; i++) {

                    res_Session += '<option value="' + dataSection.data.Section[i].SessionId + '">' + dataSection.data.Section[i].SessionName + '</option>';
                }

                $("#ddlSession_ManageSection").html('');
                $("#ddlSession_ManageSection").append(res_Session);
                if (IsAddFormRequest == 4) {
                    if (_ClassId == Class_ID_Global) {
                        $("#ddlSession_ManageSection").val(Session_ID_Global).trigger('change');
                        $('#ddlSection_ManageSection').val(arraySectionId).trigger('change');
                    }
                    else {
                        $('#ddlSection_ManageSection').val('0').trigger('change');
                    }
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

function SectionBindBySession() {
    var _SessionId = parseInt($("#ddlSession_ManageSection").val());
    if (_SessionId == undefined || _SessionId == null || _SessionId == '' || _SessionId == 0) {
        _is_valid = false;
        $("#Session_error_ManageSection").html('Please select Session!');
    }
    else {
        if (IsAddFormRequest ==4) {

        if (_SessionId == Session_ID_Global) {
           // $("#ddlSession_ManageSection").val(Session_ID_Global).trigger('change');
            $('#ddlSection_ManageSection').val(arraySectionId).trigger('change');
        }
        else {
            $('#ddlSection_ManageSection').val('0').trigger('change');
        }
        }
    }
}

function AddNewSection_ShowForm() {
    $("#dv_SectionListBox").hide();
    $("#btnAddNewSection").hide();
    $("#Title_SectionForm_ManageSection").html('Add New Section');
    $("#dv_AddUpdateSectionForm").show();
    $("#ddlSession_ManageSection").html('');
    $("#ddlSelectClass_ManageSection").attr('disabled', false);
    setClassFormDefaultValues();

}

function CancelForm() {
    setClassFormDefaultValues();
    $("#btnSubmitManageSection").show();
    $("#btnUpdateManageSection").hide();
    $("#dv_AddUpdateSectionForm").hide();
    $("#dv_SectionListBox").show();
    $("#btnAddNewSection").show();
}

function setClassFormDefaultValues() {
    //-----------Set Default Values------------
    $("#ddlSession_ManageSection").val('0').trigger('change');
    $("#ddlSelectClass_ManageSection").val('0').trigger('change');
    IsAddFormRequest = 0;
    $("#ddlSection_ManageSection").val('0').trigger('change');
    $(".errorsClass").html('');
}

function GetAllSectionList() {
    var _is_valid = true;
    var _sessionName = $('#ddlSession_Filter').find(':Selected').text().substring(0, 11);
    _sessionName = _sessionName == 'Select Sess' ? "" : _sessionName;
    if (_is_valid == true) {

        $.ajax({
            type: "GET",
            url: "/GetAllSectionList?sessionName=" + _sessionName,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (dataSection) {
                StopLoading();
                var sno = 0;
                var _edit = '';


                var data = [];

                var _table = $('#tblSection_ManageSection').DataTable();
                _table.destroy();

                for (var i = 0; i < dataSection.data.Section.length; i++) {
                    sno++;
                    _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Class-Information" onclick="EditClassInfo(' + dataSection.data.Section[i].SessionId + ');" />';

                    var sectionName = dataSection.data.Section[i].SectionName;

                    if (!(sectionName == null || sectionName == '' || sectionName == undefined)) {
                        var _SectionName = sectionName.replace(/,/g, "<br/>");

                    }
                    else {
                        _SectionName = '';
                    }

                    if (logged_In_UserType_Global == 1) {
                        data.push([
                            sno,
                            dataSection.data.Section[i].SessionName,
                            _SectionName,
                            _edit,
                        ]);
                    }
                    else {
                        data.push([
                            sno,
                            dataSection.data.Section[i].SessionName,
                            _SectionName
                        ]);
                    }

                }

                $('#tblSection_ManageSection').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true
                });
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

function AddUpdateClass(_mode) {
    var SessionId = $("#ddlSession_ManageSection").val();
    var SectionId = $("#ddlSection_ManageSection").val();

    var _is_valid = true;
    $(".errorsClass").html('');


    if (SectionId == undefined || SectionId == null || SectionId == '' || SectionId == 0) {
        _is_valid = false;
        $("#Section_error_ManageSection").html('Please select Section!');
    }
    if (SessionId == undefined || SessionId == null || SessionId == '' || SessionId == 0) {
        _is_valid = false;
        $("#Session_error_ManageSection").html('Please select Session!');
    }



    if (_is_valid == true) {

        StartLoading();

        var data = new FormData();

        data.append("Id", 0);
        data.append("SessionId", SessionId);
        data.append("SectionId", SectionId);
        data.append("mode", _mode);

        $.ajax({
            url: '/InsertUpdateSection',
            headers: {
                "Authorization": "Bearer " + UserToken_Global
            },
            data: data,
            processData: false,
            mimeType: 'multipart/form-data',
            contentType: false,
            type: 'POST',
            success: function (dataResponse) {

                //--Parse into Json of response-json-string
                dataResponse = JSON.parse(dataResponse);

                //--If successfully added/updated
                if (dataResponse.status == 1 || dataResponse.status == 2) {
                    CustomSwalPoup("Success!", dataResponse.message, "success");

                    $("#btnSubmitManageSection").show();
                    $("#btnUpdateManageSection").hide();

                    $("#dv_AddUpdateSectionForm").hide();
                    $("#dv_SectionListBox").show();
                    $("#btnAddNewSection").show();

                    IsAddFormRequest = 0;
                    //--refresh Students list
                    GetAllSectionList();
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
}

function EditClassInfo(sid) {
    // StartLoading();
    Session_ID_Global = sid;
    $("#ddlSelectClass_ManageSection").attr('disabled', 'disabled');
    $.ajax({
        type: "GET",
        url: "/GetSectionById?sID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataSection) {

            if (dataSection.data.Section != null) {


                IsAddFormRequest = 4;
                Class_ID_Global = dataSection.data.Section.ClassId;
                $("#ddlSelectClass_ManageSection").val(parseInt(dataSection.data.Section.ClassId)).trigger('change');

                arraySectionId = (dataSection.data.Section.SectionId).split(",").map(Number);
                $('#ddlSection_ManageSection').val(arraySectionId).trigger('change');


                //--Show Update Button only
                $("#btnSubmitManageSection").hide();
                $("#btnUpdateManageSection").show();

                //--Hide Add-New-Section Button
                $("#btnAddNewSection").hide();

                //--Set the Form-Name as (Update Section Information)
                $("#Title_SectionForm_ManageSection").html('Update Section Information');

                //--Show the Update-Section-Box only
                $("#dv_AddUpdateSectionForm").show();
                $("#dv_SectionListBox").hide();

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
