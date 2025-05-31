var UserToken_Global = "";
var Staff_ID_Global = 0;
var StaffTypeId_Global = 0;

$(document).ready(function () {
    StartLoading();
    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {

            UserToken_Global = dataAdminToken;
            StopLoading();
            //--Get Active StaffTypes List
            GetStaffTypeList();
        }
        else {
        }
    });
});

function AddNewStaff_ShowForm() {
    $("#dv_StaffListBox").hide();
    $("#btnAddNewStaff").hide();
    $("#dv_AddUpdateStaffForm").show();

    $("#Title_StaffForm_ManageStaff").html('Add New Staff');
}

function CancelForm() {
    //--Set Default Fields
    //$("#ddlBranch_ManageStaff").val(0);
    $("#txtFirstName_ManageStaff").val('');
    $("#txtLastName_ManageStaff").val('');
    $("#txtEmail_ManageStaff").val('');
    $("#txtPassword_ManageStaff").val('');
    $("#txtMobile_ManageStaff").val('');
    $("#txtPincode_ManageStaff").val('');
    $("#txtAddress_ManageStaff").val('');
    $('#chkIsActive_ManageStaff').prop('checked', true);


    $("#ddlGender_ManageStaff").val(0).trigger('change');
    $("#ddlStaffType_ManageStaff").val(0).trigger('change');
    $("#txtWrokExperience_ManageStaff").val('');
    $("#txtSalary_ManageStaff").val('');
    $("#txtJoiningDate_ManageStaff").val('');

    $(".errorsClass").html('');
   
    $("#btnSubmitManageStaff").show();
    $("#btnUpdateManageStaff").hide();

    $("#dv_AddUpdateStaffForm").hide();
    $("#dv_StaffListBox").show();
    $("#btnAddNewStaff").show();

    //--Set Global Variable
    Staff_ID_Global = 0;
    StaffTypeId_Global = 0;
}

function GetStaffTypeList() {
    $.ajax({
        type: "GET",
        url: "/GetAllActiveStaffTypes",
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataStaffTypes) {

            var res_StaffTypeVales = '<option value="0">Select</option>';
           
            for (var i = 0; i < dataStaffTypes.data.staffTypeValues.length; i++) {
                res_StaffTypeVales += '<option value="' + dataStaffTypes.data.staffTypeValues[i].Id + '">' + dataStaffTypes.data.staffTypeValues[i].Value + '</option>';
            }

            $("#ddlStaffType_ManageStaff").append(res_StaffTypeVales);
            //if (StaffTypeId_Global > 0) {
            //    $("#ddlStaffType_ManageStaff").val(StaffTypeId_Global).trigger('change');
            //}
            //StaffTypeId_Global = 0;

            //--Get Staff List
            GetStaffList();
        },
        error: function (result) {
            //--Get Staff List
            GetStaffList();

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



function GetStaffList() {

    $.ajax({
        type: "GET",
        url: "/GetAllStaff",
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataStaff) {

            var sno = 0;
            var _info = '';
            var _edit = '';
            var _delete = '';
            var _status = '';
           
            var data = [];

            var _table = $('#tblStaff_ManageStaff').DataTable();
            _table.destroy();

            for (var i = 0; i < dataStaff.data.staff.length; i++) {
                sno++;

                _info = '<span>' + dataStaff.data.staff[i].FirstName + ' ' + dataStaff.data.staff[i].LastName + '</span>' +
                    '<br /><span>(' + dataStaff.data.staff[i].Email + ')</span>' +
                    '<br /><span>(' + dataStaff.data.staff[i].PhoneNumber + ')</span>';

                _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Staff-Information" onclick="EditStaffInfo(' + dataStaff.data.staff[i].Id + ');" />';
                _delete = '<img src="/Content/Images/delete_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Delete Staff" onclick="ConfirmDeleteStaff(' + dataStaff.data.staff[i].Id + ');" />';

                //---Check Staff-Status
                if (dataStaff.data.staff[i].LoginStatus == 1) {
                    _status = '<a class="btn btn-success btn-sm" style="width:80px;" onclick="ConfirmChangeStatusStaff(' + dataStaff.data.staff[i].Id + ');">Active</a>';
                }
                else {
                    _status = '<a class="btn btn-danger btn-sm" style="width:80px;" onclick="ConfirmChangeStatusStaff(' + dataStaff.data.staff[i].Id + ');">In-Active</a>';
                }
                console.log(dataStaff.data.staff[i].StaffTypeName);
                data.push([
                    sno,
                    _info,
                    dataStaff.data.staff[i].StaffTypeName,
                    dataStaff.data.staff[i].Gender,
                    dataStaff.data.staff[i].Pincode,
                    _status,
                    _edit,
                    _delete
                ]);
            }

            $('#tblStaff_ManageStaff').DataTable({
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

function AddUpdateStaff(_mode) {
    //var _branchId_MS = $("#ddlBranch_ManageStaff").val();
    var _firstName_MS = $("#txtFirstName_ManageStaff").val();
    var _lastName_MS = $("#txtLastName_ManageStaff").val();
    var _email_MS = $("#txtEmail_ManageStaff").val();
    var _password_MS = $("#txtPassword_ManageStaff").val();
    var _mobile_MS = $("#txtMobile_ManageStaff").val();
    var _pincode_MS = $("#txtPincode_ManageStaff").val();
    var _address_MS = $("#txtAddress_ManageStaff").val();

    var _gender_MS = $("#ddlGender_ManageStaff").val(); // gender_error_ManageStaff
    var _staffType_MS = $("#ddlStaffType_ManageStaff").val(); //staffType_error_ManageStaff
    var _workExperience_MS = $("#txtWrokExperience_ManageStaff").val(); //workExperience_error_ManageStaff
    var _salary_MS = $("#txtSalary_ManageStaff").val(); //salary_error_ManageStaff
    var _joiningDate_MS = $("#txtJoiningDate_ManageStaff").val(); //joiningDate_error_ManageStaff

    var email_test = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var _is_valid = true;
    $(".errorsClass").html('');

    var _is_active_staff = 0;
    if ($('#chkIsActive_ManageStaff').is(':checked')) {
        // checked
        _is_active_staff = 1;
    }

    if (_gender_MS == undefined || _gender_MS == null || _gender_MS == '' || _gender_MS == 0) {
        _is_valid = false;
        $("#gender_error_ManageStaff").html('Please select the Gender!');
    }

    if (_staffType_MS == undefined || _staffType_MS == null || _staffType_MS == '' || _staffType_MS == 0) {
        _is_valid = false;
        $("#staffType_error_ManageStaff").html('Please select the Staff Type!');
    }

    if (_workExperience_MS == '' || _workExperience_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#workExperience_error_ManageStaff").html('Please enter first name!');
    }
    
    if (_salary_MS == '' || _salary_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#salary_error_ManageStaff").html('Please enter first name!');
    }

    if (_joiningDate_MS == '' || _joiningDate_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#joiningDate_error_ManageStaff").html('Please enter first name!');
    }

    if (_firstName_MS == '' || _firstName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#firstName_error_ManageStaff").html('Please enter first name!');
    }

    if (_lastName_MS == '' || _lastName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#lastName_error_ManageStaff").html('Please enter last name!');
    }

    if (_email_MS == '' || _email_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#email_error_ManageStaff").html('Please enter the email address!');
    }
    else if (!email_test.test(_email_MS)) {
        _is_valid = false;
        $("#email_error_ManageStaff").html('Please enter the valid email address!');
    }

    if (_password_MS == '' || _password_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#password_error_ManageStaff").html('Please enter password!');
    }

    if (_mobile_MS == '' || _mobile_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#mobile_error_ManageStaff").html('Please enter mobile-number!');
    }
    else if (_mobile_MS.toString().length < 10) {
        _is_valid = false;
        $("#mobile_error_ManageStaff").html('Mobile-number should be of 10 digits!');
    }

    if (_pincode_MS == '' || _pincode_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#pincode_error_ManageStaff").html('Please enter pincode!');
    }
    if (_address_MS == '' || _address_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#address_error_ManageStaff").html('Please enter address!');
    }

    if (_is_valid == true) {
        StartLoading();

        var data = new FormData();

        data.append("Id", Staff_ID_Global);
        //data.append("branchId", _branchId_MS);
        data.append("firstName", _firstName_MS);
        data.append("lastName", _lastName_MS);
        data.append("email", _email_MS.toLowerCase());
        data.append("mobile", '+91' + _mobile_MS);
        data.append("countryMobilePhoneCodeOnly", '+91');
        data.append("mobileNumberOnly", _mobile_MS);
        data.append("password", _password_MS);
        data.append("pincode", _pincode_MS);
        data.append("address", _address_MS);
        data.append("isActive", _is_active_staff);
        data.append("gender", _gender_MS);
        data.append("staffTypeId", _staffType_MS);
        data.append("workExperience", _workExperience_MS);
        data.append("salary", _salary_MS);
        data.append("joiningDate", _joiningDate_MS);
        data.append("mode", _mode);

        $.ajax({
            url: '/InsertUpdateStaff',
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

                    //-----------Set Default Values------------
                    //$("#ddlBranch_ManageStaff").val(0);
                    $("#txtFirstName_ManageStaff").val('');
                    $("#txtLastName_ManageStaff").val('');
                    $("#txtEmail_ManageStaff").val('');
                    $("#txtPassword_ManageStaff").val('');
                    $("#txtMobile_ManageStaff").val('');
                    $("#txtPincode_ManageStaff").val('');
                    $("#txtAddress_ManageStaff").val('');
                    $('#chkIsActive_ManageStaff').prop('checked', true);

                    $("#ddlGender_ManageStaff").val(0).trigger('change');
                    $("#ddlStaffType_ManageStaff").val(0).trigger('change');
                    $("#txtWrokExperience_ManageStaff").val('');
                    $("#txtSalary_ManageStaff").val('');
                    $("#txtJoiningDate_ManageStaff").val('');

                    $(".errorsClass").html('');

                    $("#btnSubmitManageStaff").show();
                    $("#btnUpdateManageStaff").hide();

                    $("#dv_AddUpdateStaffForm").hide();
                    $("#dv_StaffListBox").show();
                    $("#btnAddNewStaff").show();

                    //--Set Staff-ID in the Global Variable
                    Staff_ID_Global = 0;
                    StaffTypeId_Global = 0;
                    //-------------------------------------

                    //--refresh Staff list
                    GetStaffList();
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

function EditStaffInfo(sid) {
    StartLoading();
    Staff_ID_Global = sid;
    $.ajax({
        type: "GET",
        url: "/GetStaffById?staffID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataStaff) {

            if (dataStaff.data.staff != null) {

                //$("#ddlBranch_ManageStaff").val(dataStaff.data.staff.BranchId);
                $("#txtFirstName_ManageStaff").val(dataStaff.data.staff.FirstName);
                $("#txtLastName_ManageStaff").val(dataStaff.data.staff.LastName);
                $("#txtEmail_ManageStaff").val(dataStaff.data.staff.Email);
                $("#txtPassword_ManageStaff").val(dataStaff.data.staff.Password);
                $("#txtMobile_ManageStaff").val(dataStaff.data.staff.PhoneNumber_Only);
                $("#txtPincode_ManageStaff").val(dataStaff.data.staff.Pincode);
                $("#txtAddress_ManageStaff").val(dataStaff.data.staff.Address);

                $("#ddlGender_ManageStaff").val(dataStaff.data.staff.Gender).trigger('change');
                $("#ddlStaffType_ManageStaff").val(dataStaff.data.staff.StaffFieldTypeValueId).trigger('change');
                //StaffTypeId_Global = dataStaff.data.staff.StaffFieldTypeValueId;
                $("#txtWrokExperience_ManageStaff").val(dataStaff.data.staff.WorkExperienceInYears);
                $("#txtSalary_ManageStaff").val(dataStaff.data.staff.Salary);
                $("#txtJoiningDate_ManageStaff").val(dataStaff.data.staff.JoiningDate);

                //--Branch-Status
                if (dataStaff.data.staff.LoginStatus == 1) {

                    $('#chkIsActive_ManageStaff').prop('checked', true);
                }
                else {
                    $('#chkIsActive_ManageStaff').prop('checked', false);
                }

                //--Show Update Button only
                $("#btnSubmitManageStaff").hide();
                $("#btnUpdateManageStaff").show();

                //--Hide Add-New-Staff Button
                $("#btnAddNewStaff").hide();

                //--Set the Form-Title as (Update Staff Information)
                $("#Title_StaffForm_ManageStaff").html('Update Staff Information');

                //--Show the Update-Staff-Box only
                $("#dv_AddUpdateStaffForm").show();
                $("#dv_StaffListBox").hide();
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

function ConfirmDeleteStaff(sid) {
    swal({
        title: "Delete Staff",
        text: "Are you sure to delete this Staff-Member?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        DeleteStaff(sid);
    });
}

function DeleteStaff(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/DeleteStaffById?staffID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if staff successfully deleted
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get Staff List
                    GetStaffList();
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

function ConfirmChangeStatusStaff(sid) {
    swal({
        title: "Change Status",
        text: "Are you sure to change status of this Staff-Member?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        ChangeStatusStaff(sid);
    });
}

function ChangeStatusStaff(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/ChangeStaffStatusById?staffID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if staff-status successfully updated
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get Staff List
                    GetStaffList();
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

//function FilterStaffByBranch() {
//    StartLoading();

//    var _url_val = "/GetAllStaff";

//    $.ajax({
//        type: "GET",
//        url: _url_val,
//        headers: {
//            "Authorization": "Bearer " + UserToken_Global,
//            "Content-Type": "application/json"
//        },
//        contentType: 'application/json',
//        success: function (dataStaff) {

//            var sno = 0;
//            var _info = '';
//            var _edit = '';
//            var _delete = '';
//            var _status = '';

//            var data = [];

//            var _table = $('#tblStaff_ManageStaff').DataTable();
//            _table.destroy();

//            for (var i = 0; i < dataStaff.data.staff.length; i++) {
//                sno++;

//                _info = '<span>' + dataStaff.data.staff[i].FirstName + ' ' + dataStaff.data.staff[i].LastName + '</span>' +
//                    '<br /><span>(' + dataStaff.data.staff[i].Email + ')</span>' +
//                    '<br /><span>(' + dataStaff.data.staff[i].PhoneNumber + ')</span>';

//                _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Staff-Information" onclick="EditStaffInfo(' + dataStaff.data.staff[i].Id + ');" />';
//                _delete = '<img src="/Content/Images/delete_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Delete Staff" onclick="ConfirmDeleteStaff(' + dataStaff.data.staff[i].Id + ');" />';

//                //---Check Staff-Status
//                if (dataStaff.data.staff[i].LoginStatus == 1) {
//                    _status = '<a class="btn btn-success btn-sm" style="width:80px;" onclick="ConfirmChangeStatusStaff(' + dataStaff.data.staff[i].Id + ');">Active</a>';
//                }
//                else {
//                    _status = '<a class="btn btn-danger btn-sm" style="width:80px;" onclick="ConfirmChangeStatusStaff(' + dataStaff.data.staff[i].Id + ');">In-Active</a>';
//                }

//                data.push([
//                    sno,
//                    _info,
//                    dataStaff.data.staff[i].Gender,
//                    dataStaff.data.staff[i].Pincode,
//                    _status,
//                    _edit,
//                    _delete
//                ]);
//            }

//            $('#tblStaff_ManageStaff').DataTable({
//                data: data,
//                deferRender: true,
//                //scrollY: 200,
//                scrollCollapse: true,
//                scroller: true
//            });

//            StopLoading();
//        },
//        error: function (result) {
//            StopLoading();

//            if (result["status"] == 401) {
//                $.iaoAlert({
//                    msg: 'Unauthorized! Invalid Token!',
//                    type: "error",
//                    mode: "dark",
//                });
//            }
//            else {
//                $.iaoAlert({
//                    msg: 'There is some technical error, please try again!',
//                    type: "error",
//                    mode: "dark",
//                });
//            }
//        }
//    });
//}

