var UserToken_Global = "";
var Student_ID_Global = 0;
var logged_In_UserType_Global = 1;
var sessionId_Global = 0;
var sectionId_Global = 0;
var classId_Global = 0;
var IsExistPoup = 0;
var currentSession = "";
var CurrentYear = new Date().getFullYear();
var NextYear = CurrentYear + 1;
currentSession = CurrentYear + " - " + NextYear; //`${CurrentYear} - ${NextYear}`

var transportAmount_Global = 0;

$(document).ready(function () {
    StartLoading();

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
                else {

                }
            });
        }
    });
});

function GetAllDdlData() {
    GetMultipleDataForddl("Gender,Blood", "ddlGender_ManageStudent,ddlBlood_ManageStudent");
    GetDdlClassAllData("ddlSelectClass_ManageStudent");
    StopLoading();
}

function SessionBindByClass4Student() {
    FeeType_Dic = {};
    $('#txtTransport_ManageStudent').val('0');
    var _ClassId = parseInt($("#ddlSelectClass_ManageStudent").val());
    var _is_valid = true;
    $(".errorsClass").html('');
    if (_ClassId == undefined || _ClassId == null || _ClassId == '' || _ClassId == 0) {
        _is_valid = false;
        $('#SelectClass_error_ManageStudent').html('Please select class!');
    }
    if (_is_valid == true && _ClassId !== 0) {

        $.ajax({
            url: '/SessionBindByClass4Student?classId=' + _ClassId,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            type: 'Get',
            success: function (dataSession) {

                var res_Session;
                if (dataSession.data.Session.length != 0) {
                    res_Session = '<option value="0">Select Session</option>';
                }
                for (var i = 0; i < dataSession.data.Session.length; i++) {

                    res_Session += '<option value="' + dataSession.data.Session[i].SessionId + '">' + dataSession.data.Session[i].SessionName + '</option>';
                }
                $("#ddlSection_ManageStudent").html('');
                $("#ddlSession_ManageStudent").html('');
                $("#ddlSession_ManageStudent").append(res_Session);

                if (_ClassId == classId_Global) {
                    $("#ddlSession_ManageStudent").val(sessionId_Global).trigger('change');
                }
                else {
                    $('#ddlSection_ManageStudent').val('0').trigger('change');
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

function SectionBindBySession4Student() {
    FeeType_Dic = {};
    $('#txtTransport_ManageStudent').val('0');
    var _SessionId = parseInt($("#ddlSession_ManageStudent").val());
    var _is_valid = true;
    $(".errorsClass").html('');
    if (_SessionId == undefined || _SessionId == null || _SessionId == '' || _SessionId == 0) {
        _is_valid = false;
        $("#Session_error_ManageStudent").html('Please select Session!');
    }
    if (_is_valid == true && _SessionId !== 0) {

        $.ajax({
            url: '/SectionBindBySession4Student?sessionId=' + _SessionId,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            type: 'Get',
            success: function (dataResponse) {
                
                var allData = dataResponse.data.Section;
                var res_section = '<option value="0">Select Section of Class</option>';
                for (var i = 0; i < allData.length; i++) {
                    var sectionIdArray = (allData[i].SectionId).split(',');
                    var sectionNameArray = (allData[i].SectionName).split(',');
                    for (var a = 0; a < sectionIdArray.length; a++) {
                        res_section += '<option value="' + sectionIdArray[a] + '">' + sectionNameArray[a] + '</option>';
                    }
                    IsTransportStudent(transportAmount_Global);
                }

                $("#ddlSection_ManageStudent").html('');
                $("#ddlSection_ManageStudent").append(res_section);
                if (_SessionId == sessionId_Global) {
                    $('#ddlSection_ManageStudent').val(sectionId_Global).trigger('change');
                }
                else {
                    $('#ddlSection_ManageStudent').val('0').trigger('change');
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

function DefaultValues() {
    //--Set Default Fields
    $("#txtFirstName_ManageStudent").val('');
    $("#txtLastName_ManageStudent").val('');
    $("#txtFatherName_ManageStudent").val('');
    $("#txtMotherName_ManageStudent").val('');
    $("#txtEmail_ManageStudent").val('');
    $("#txtMobile_ManageStudent").val('');
    $("#txtUserName_ManageStudent").val('');
    $("#txtPassword_ManageStudent").val('');
    $("#txtPincode_ManageStudent").val('');
    $("#txtAddress_ManageStudent").val('');
    $("#txtDOB_ManageStudent").val('');
    $('#chkIsActive_ManageStudent').prop('checked', true); // Checks it
    $('#chkIsExist_ManageStudent').prop('checked', false);
    $('#chkTransport_ManageStudent').prop('checked', false);
    $('#txtTransport_ManageStudent').css('display', 'none');

    $('#ddlGender_ManageStudent').val('0').trigger('change')
    $('#ddlBlood_ManageStudent').val('0').trigger('change')

    $('#ddlSelectClass_ManageStudent').val('0').trigger('change')
    $("#ddlSession_ManageStudent").html('');
    $("#ddlSection_ManageStudent").html('');
    sessionId_Global = 0;
    sectionId_Global = 0;
    classId_Global = 0;
    IsAddFormRequest = 0;
    $(".errorsClass").html('');

}
function GoTOInsertUpdateForm(InsertFormTitle, Action) {
    $("#Title_StudentForm_ManageStudent").html(InsertFormTitle); //--Set the Form-Title in InsertUpdate
    $("#btnAddNewStudent").hide();  //--Hide Add-New-Student Button
    $("#dv_AddUpdateStudentForm").show(); //--Show the Insert Update-Student-Box only
    $("#dv_StudentListBox").hide(); // -- hide table Form 
    if (Action == "Update") {
        $("#btnSubmitManageStudent").hide();
        $("#btnUpdateManageStudent").show();
    }
    else {
        $("#btnSubmitManageStudent").show();
        $("#btnUpdateManageStudent").hide();
    }
}
function GoToTableForm() {
    $("#dv_AddUpdateStudentForm").hide();
    $("#dv_StudentListBox").show();
    $("#btnAddNewStudent").show();
    Student_ID_Global = 0;
    transportAmount_Global = 0;
    DefaultValues();
}
function CancelForm() {
    GoToTableForm();
}
function AddNewStudent_ShowForm() {
    GoTOInsertUpdateForm('Add New Student', "Insert");
    document.getElementById("chkIsExist_ManageStudent").disabled = false;
    document.getElementById("btnSubmitManageStudent").onclick = function () { AddUpdateStudent(1); };
}

function IsExistingChange() {
    if ($('#chkIsExist_ManageStudent').is(':checked')) {

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
                $('#chkIsExist_ManageStudent').prop('checked', false);
            }
            if (inputValue != false && inputValue != "" && inputValue != null) {
                EditStudentInfo(parseInt(inputValue), '');
            }
        });
    }
    else {
        DefaultValues();
    }
}

function AddUpdateStudent(_mode) {
    var _SessionId_MS = $("#ddlSession_ManageStudent").val();
    var _SectionId_MS = $("#ddlSection_ManageStudent").val();
    var _firstName_MS = $("#txtFirstName_ManageStudent").val();
    var _lastName_MS = $("#txtLastName_ManageStudent").val();
    var _DOB_MS = $("#txtDOB_ManageStudent").val();
    var _gender_MS = $("#ddlGender_ManageStudent").val();
    var _blood_MS = $("#ddlBlood_ManageStudent").val();
    var _fatherName_MS = $("#txtFatherName_ManageStudent").val();
    var _motherName_MS = $("#txtMotherName_ManageStudent").val();
    var _email_MS = $("#txtEmail_ManageStudent").val();
    var _userName_MS = $("#txtUserName_ManageStudent").val();
    var _password_MS = $("#txtPassword_ManageStudent").val();
    var _mobile_MS = $("#txtMobile_ManageStudent").val();
    var _pincode_MS = $("#txtPincode_ManageStudent").val();
    var _address_MS = $("#txtAddress_ManageStudent").val();
    var _hasTakenTransportService_MS = 0;
    var _transportAmount_MS = 0;
    if ($('#chkTransport_ManageStudent').is(':checked')) {
        _hasTakenTransportService_MS = 1;
        _transportAmount_MS = $('#txtTransport_ManageStudent').val();
        if (_transportAmount_MS == '' || _transportAmount_MS.replace(/\s/g, "") == "" || _transportAmount_MS == '0' || _transportAmount_MS == 0) {
            _is_valid = false;
            $("#Transport_error_ManageStudent").html('Transport Amount not be Zero please enter amount');
        }
    }

    var email_test = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var _is_valid = true;
    $(".errorsClass").html('');

    var _is_active_student = 0;
    if ($('#chkIsActive_ManageStudent').is(':checked')) {
        // checked
        _is_active_student = 1;
    }

    if (_SessionId_MS == undefined || _SessionId_MS == null || _SessionId_MS == '' || _SessionId_MS == 0) {
        _is_valid = false;
        $("#Session_error_ManageStudent").html('Please select the Session!');
    }
    if (_SectionId_MS == undefined || _SectionId_MS == null || _SectionId_MS == '' || _SectionId_MS == 0) {
        _is_valid = false;
        $("#Section_error_ManageStudent").html('Please select the Section!');
    }

    if (_firstName_MS == '' || _firstName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#firstName_error_ManageStudent").html('Please enter first name!');
    }

    if (_lastName_MS == '' || _lastName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#lastName_error_ManageStudent").html('Please enter last name!');
    }

    if (_fatherName_MS == '' || _fatherName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#FatherName_error_ManageStudent").html('Please enter father name!');
    }
    if (_motherName_MS == '' || _motherName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#MotherName_error_ManageStudent").html('Please enter mother name!');
    }
    if (_DOB_MS == undefined || _DOB_MS == null || _DOB_MS == '' || _DOB_MS == 0) {
        _is_valid = false;
        $("#DOB_error_ManageStudent").html('Please select the Date of birth!');
    }
    if (_gender_MS == undefined || _gender_MS == null || _gender_MS == '' || _gender_MS == 0) {
        _is_valid = false;
        $("#Gender_error_ManageStudent").html('Please select the Gender!');
    }
    if (_blood_MS == undefined || _blood_MS == null || _blood_MS == '' || _blood_MS == 0) {
        _is_valid = false;
        $("#Blood_error_ManageStudent").html('Please select the Blood group!');
    }

    //if (_email_MS == '' || _email_MS.replace(/\s/g, "") == "") {
    //    _is_valid = false;
    //    $("#email_error_ManageStudent").html('Please enter the email address!');
    //}
    if (_email_MS != '' && !email_test.test(_email_MS)) {
        _is_valid = false;
        $("#email_error_ManageStudent").html('Please enter the valid email address!');
    }

    if (_userName_MS == '' || _userName_MS.replace(/\s/g, "") == "") {
        // _is_valid = false;
        // $("#UserName_error_ManageStudent").html('Please enter user name password!');

        _password_MS = '-';
    }
    if (_password_MS == '' || _password_MS.replace(/\s/g, "") == "") {
        // _is_valid = false;
        // $("#password_error_ManageStudent").html('Please enter password!');

        _password_MS = '-';
    }

    if (_mobile_MS == '' || _mobile_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#mobile_error_ManageStudent").html('Please enter mobile-number!');
    }
    else if (_mobile_MS.toString().length < 10) {
        _is_valid = false;
        $("#mobile_error_ManageStudent").html('Mobile-number should be of 10 digits!');
    }

    if (_pincode_MS == '' || _pincode_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#pincode_error_ManageStudent").html('Please enter pincode!');
    }
    if (_address_MS == '' || _address_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#address_error_ManageStudent").html('Please enter address!');
    }

    if (_is_valid == true) {

        StartLoading();

        var data = new FormData();

        data.append("Id", Student_ID_Global);
        data.append("sessionId", _SessionId_MS);
        data.append("sectionId", _SectionId_MS);
        data.append("firstName", _firstName_MS);
        data.append("lastName", _lastName_MS);
        data.append("DOB", _DOB_MS);
        data.append("gender", _gender_MS);
        data.append("blood", _blood_MS);
        data.append("fatherName", _fatherName_MS);
        data.append("motherName", _motherName_MS);
        data.append("email", _email_MS.toLowerCase());
        data.append("mobile", '+91' + _mobile_MS);
        data.append("countryMobilePhoneCodeOnly", '+91');
        data.append("mobileNumberOnly", _mobile_MS);
        data.append("userName", _userName_MS);
        data.append("password", _password_MS);
        data.append("pincode", _pincode_MS);
        data.append("address", _address_MS);
        data.append("hasTakenTransportService", _hasTakenTransportService_MS);
        data.append("transportAmount", _transportAmount_MS);
        data.append("isActive", _is_active_student);
        data.append("mode", _mode);

        $.ajax({
            url: '/InsertUpdateStudent',
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
                    GoToTableForm();
                    //--refresh Students list
                    GetFilterStudentList()
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
}

function EditStudentInfo(sid, chkIsExist) {
    if (chkIsExist == 'disabled_chkIsExist') {
        document.getElementById("chkIsExist_ManageStudent").disabled = true;
        Student_ID_Global = sid;
    }
    else {
        document.getElementById("chkIsExist_ManageStudent").disabled = false;
    }
    StartLoading();

    $.ajax({
        type: "GET",
        url: "/GetStudentById?studentID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (Response) {
            var AllData = Response.data.student;
            if (AllData != null) {
                classId_Global = parseInt(AllData.ClassId);
                $("#ddlSelectClass_ManageStudent").val(parseInt(AllData.ClassId)).trigger('change');
                sessionId_Global = parseInt(AllData.SessionId);
                sectionId_Global = parseInt(AllData.SectionId);
                transportAmount_Global = AllData.TransportAmount;
                //$("#ddlSession_ManageStudent").val(parseInt(AllData.SessionId)).trigger('change');
                // $("#ddlSection_ManageStudent").val(parseInt(AllData.SectionId)).trigger('change');
                $("#txtFirstName_ManageStudent").val(AllData.FirstName);
                $("#txtLastName_ManageStudent").val(AllData.LastName);
                $("#txtDOB_ManageStudent").val(AllData.DateOfBirth);
                $("#ddlGender_ManageStudent").val(parseInt(AllData.Gender)).trigger('change');
                $("#ddlBlood_ManageStudent").val(parseInt(AllData.BloodGroup)).trigger('change');
                $("#txtFatherName_ManageStudent").val(AllData.FatherName);
                $("#txtMotherName_ManageStudent").val(AllData.MotherName);
                $("#txtEmail_ManageStudent").val(AllData.Email);
                $("#txtPassword_ManageStudent").val(AllData.Password);
                $("#txtUserName_ManageStudent").val(AllData.Username);
                $("#txtMobile_ManageStudent").val(AllData.PhoneNumber_Only);
                $("#txtPincode_ManageStudent").val(AllData.Pincode);
                $("#txtAddress_ManageStudent").val(AllData.Address);
                //--Check-Transport Status
                if (AllData.HasTakenTransportService == 1) {

                    $('#chkTransport_ManageStudent').prop('checked', true);
                }
                else {
                    $('#chkTransport_ManageStudent').prop('checked', false);
                }
                //--Check-Status
                if (AllData.LoginStatus == 1) {

                    $('#chkIsActive_ManageStudent').prop('checked', true);
                }
                else {
                    $('#chkIsActive_ManageStudent').prop('checked', false);
                }
                if ($('#chkIsExist_ManageStudent').is(':checked')) {
                    GoTOInsertUpdateForm('Add New Student', "Insert");
                    document.getElementById("btnSubmitManageStudent").onclick = function () { AddUpdateStudent(3); };
                }
                else {
                    GoTOInsertUpdateForm('Update Student Information', "Update");
}
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

function ConfirmDeleteStudent(sid) {
    swal({
        title: "Delete Student",
        text: "Are you sure to delete this Student?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        DeleteStudent(sid);
    });
}

function DeleteStudent(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/DeleteStudentById?studentID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if Student successfully deleted
            if (dataResponse.status == 1) {
                //--Get Student List
                GetFilterStudentList()
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


function GetFilterStudentList() {
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

    if (true) {
        $.ajax({
            type: "GET",
            url: "/GetFilterStudentList?classid=" + classId + "&sessionName=" + sessionName,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (dataStudent) {

                var sno = 0;
                var _info = '';
                var _edit = '';
                var _delete = '';
                // var _status = '';
                var _student_emailAddress = '';

                var data = [];

                var _table = $('#tblStudent_ManageStudent').DataTable();
                _table.destroy();

                for (var i = 0; i < dataStudent.data.student.length; i++) {
                    sno++;

                    if (dataStudent.data.student[i].Email) {
                        _student_emailAddress = '<br /><span>(' + dataStudent.data.student[i].Email + ')</span>';
                    }
                    else {
                        _student_emailAddress = '';
                    }

                    _info_4Admin = '<a href="/Admin/StudentDetail?sid=' + dataStudent.data.student[i].Id + '" target="_blank" style="color: red;cursor: pointer;"><i class="fas fa-external-link-alt" style="margin-right: 4px;font-size: 12px;"></i>(' + dataStudent.data.student[i].FirstName + ' ' + dataStudent.data.student[i].LastName + ')</a>' + _student_emailAddress + '<br /><span>(' + dataStudent.data.student[i].PhoneNumber + ')</span>';
                    _info_4Staff = '<a href="/Staff/StudentDetail?sid=' + dataStudent.data.student[i].Id + '" target="_blank" style="color: red;cursor: pointer;"><i class="fas fa-external-link-alt" style="margin-right: 4px;font-size: 12px;"></i>(' + dataStudent.data.student[i].FirstName + ' ' + dataStudent.data.student[i].LastName + ')</a>' + _student_emailAddress + '<br /><span>(' + dataStudent.data.student[i].PhoneNumber + ')</span>';

                    _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Student-Information" onclick="EditStudentInfo(' + dataStudent.data.student[i].Id + ',\'disabled_chkIsExist\');" />';
                    _delete = '<img src="/Content/Images/delete_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Delete Student" onclick="ConfirmDeleteStudent(' + dataStudent.data.student[i].Id + ');" />';
                    var _Address = dataStudent.data.student[i].Address + "<br/>" + dataStudent.data.student[i].Pincode;
                    if (logged_In_UserType_Global == 1) {
                        data.push([
                            sno,
                            _info_4Admin,
                            dataStudent.data.student[i].SessionName,
                            dataStudent.data.student[i].SectionName,
                            _Address,
                            _edit,
                            _delete
                        ]);
                    }
                    else {
                        data.push([
                            sno,
                            _info_4Staff,
                            dataStudent.data.student[i].SessionName,
                            dataStudent.data.student[i].SectionName,
                            _Address,
                            _edit,
                            _delete
                        ]);
                    }

                }

                $('#tblStudent_ManageStudent').DataTable({
                    data: data,
                    deferRender: true,
                    //scrollY: 200,
                    scrollCollapse: true,
                    scroller: true
                });

                GetAllDdlData();
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

function GetIdFromDic(dic, val) {
    var id = "";
    Object.entries(dic).forEach(([key, value]) => { if (value.hasOwnProperty(val)) { id = key; } });
    return id;
}

function IsTransportStudent(val) {
    //$('#txtTransport_ManageStudent').val('');
    if ($('#chkTransport_ManageStudent').is(':checked')) {
        $('#txtTransport_ManageStudent').css('display', 'block');
        if (val > 0 && val != null && val != undefined) {
            $("#txtTransport_ManageStudent").val(val);
        }
        else if (FeeType_Dic.hasOwnProperty(GetIdFromDic(FeeType_Dic, 'Transport')) && $('#ddlSession_ManageStudent').val()!='0') {
            $('#txtTransport_ManageStudent').val(FeeType_Dic[GetIdFromDic(FeeType_Dic, 'Transport')]['Transport']);
        }
        else {
            $('#txtTransport_ManageStudent').val(0);
        }
    }
    else {
        $('#txtTransport_ManageStudent').css('display', 'none');
        $('#txtTransport_ManageStudent').val(0);
        $("#Transport_error_ManageStudent").html('');
    }
}



