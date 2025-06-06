var UserToken_Global = "";
var logged_In_UserType_Global = 1;
var logged_In_UserTypeName_Global = "SuperAdmin";
var Staff_ID_Global = 0;
var Admin_ID_Global = 0;
var SuperAdmin_ID_Global = 0;


//$(document).ready(function () {
//    StartLoading();
//    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
//        if (dataAdminToken != "" && dataAdminToken != null) {

//            UserToken_Global = dataAdminToken;
//            logged_In_UserType_Global = 1;
//            logged_In_UserTypeName_Global = "Admin";


//            GetProfileData();
//            StopLoading();
//        }
//        else {
//            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
//                if (dataStaffToken != "" && dataStaffToken != null) {

//                    UserToken_Global = dataStaffToken;
//                    logged_In_UserType_Global = 2;
//                    logged_In_UserTypeName_Global = "Staff";


//                    GetProfileData();
//                    StopLoading();
//                }
//                else {

//                }
//            });
//        }
//    });
//});
$(document).ready(function () {
    StartLoading();

    // Step 1: Check SuperAdmin
    $.get("/SuperAdmin/GetSuperAdminCookieDetail", null, function (dataSuperAdminToken) {
        if (dataSuperAdminToken != "" && dataSuperAdminToken != null) {

            UserToken_Global = dataSuperAdminToken;
            logged_In_UserType_Global = 1;
            logged_In_UserTypeName_Global = "SuperAdmin";

            GetProfileData();
            StopLoading();
        }
        else {
            // Step 2: Check Admin
            $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
                if (dataAdminToken != "" && dataAdminToken != null) {

                    UserToken_Global = dataAdminToken;
                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Admin";

                    GetProfileData();
                    StopLoading();
                }
                else {
                    // Step 3: Check Staff
                    $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                        if (dataStaffToken != "" && dataStaffToken != null) {

                            UserToken_Global = dataStaffToken;
                            logged_In_UserType_Global = 3;
                            logged_In_UserTypeName_Global = "Staff";

                            GetProfileData();
                            StopLoading();
                        }
                        else {
                            StopLoading(); // No user found
                        }
                    });
                }
            });
        }
    });
});

function ChangeTextEve(firstName, lastName, email) {
    var _firstName = $("#" + firstName).val()
    var _lastName = $("#" + lastName).val()
    var _email = $("#" + email).val()

    $("#ProfileName_ManageProfile").html(_firstName + " " + _lastName);
    $("#ProfileEmail_ManageProfile").html(_email);

}

function GetProfileData() {

    //if (logged_In_UserType_Global == 1) {
    //    document.getElementById('a_Admin_AdminLayout').style.color = "red";
    //}
    //else {
    //    document.getElementById('a_Staff_StaffLayout').style.color = "red";
    //}

    if (logged_In_UserType_Global === 1) {
        // SuperAdmin
        document.getElementById('a_SuperAdmin_SuperAdminLayout').style.color = "#FFFFFF";
    }
    else if (logged_In_UserType_Global === 2) {
        // Admin
        document.getElementById('a_Admin_AdminLayout').style.color = "#FFFFFF";
    }
    else if (logged_In_UserType_Global === 3) {
        // Staff
        document.getElementById('a_Staff_StaffLayout').style.color = "#FFFFFF";
    }
    else {
        console.warn("Unknown user type");
    }


    StartLoading();
    $.ajax({
        type: "GET",
        url: "/GetProfileDataById",
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        //success: function (ProfileData) {

        //    if (ProfileData.data.Admin != null) {


        //        $("#txtFirstName_ManageProfile").val(ProfileData.data.Admin.FirstName);
        //        $("#txtLastName_ManageProfile").val(ProfileData.data.Admin.LastName);
        //        $("#txtEmail_ManageProfile").val(ProfileData.data.Admin.Email);
        //        $("#txtUserName_ManageProfile").val((ProfileData.data.Admin.Username));
        //        $("#txtMobile_ManageProfile").val(ProfileData.data.Admin.PhoneNumber_Only);
        //        $("#txtPincode_ManageProfile").val(ProfileData.data.Admin.Pincode);
        //        $("#txtAddress_ManageProfile").val(ProfileData.data.Admin.Address);
        //        $("#ProfileName_ManageProfile").html(ProfileData.data.Admin.FirstName + " " + ProfileData.data.Admin.LastName);
        //        $("#a_Admin_AdminLayout").html(ProfileData.data.Admin.FirstName + " " + ProfileData.data.Admin.LastName);
        //        $("#ProfileEmail_ManageProfile").html(ProfileData.data.Admin.Email);
        //        $("#imgProfile_ManageProfile").attr('src', '/Content/AdminImages/' + ProfileData.data.Admin.ProfileImage);
        //        $("#img_Admin_AdminLayout").attr('src', '/Content/AdminImages/' + ProfileData.data.Admin.ProfileImage);
        //        if (ProfileData.data.Admin.LoginStatus == 1) {

        //            $('#chkIsActive_ManageProfile').prop('checked', true);
        //        }
        //        else {
        //            $('#chkIsActive_ManageProfile').prop('checked', false);
        //        }
        //    }

        //    else if (ProfileData.data.Staff != null) {


        //        $("#txtFirstName_ManageProfile").val(ProfileData.data.Staff.FirstName);
        //        $("#txtLastName_ManageProfile").val(ProfileData.data.Staff.LastName);
        //        $("#txtEmail_ManageProfile").val(ProfileData.data.Staff.Email);
        //        $("#txtUserName_ManageProfile").val((ProfileData.data.Staff.Username));
        //        $("#txtMobile_ManageProfile").val(ProfileData.data.Staff.PhoneNumber_Only);
        //        $("#txtPincode_ManageProfile").val(ProfileData.data.Staff.Pincode);
        //        $("#txtAddress_ManageProfile").val(ProfileData.data.Staff.Address);
        //        $("#ProfileName_ManageProfile").html(ProfileData.data.Staff.FirstName + " " + ProfileData.data.Staff.LastName);
        //        $("#ProfileEmail_ManageProfile").html(ProfileData.data.Staff.Email);
        //        $("#imgProfile_ManageProfile").attr('src', '/Content/StaffImages/' + ProfileData.data.Staff.ProfileImage);
        //        if (ProfileData.data.Staff.LoginStatus == 1) {

        //            $('#chkIsActive_ManageProfile').prop('checked', true);
        //        }
        //        else {
        //            $('#chkIsActive_ManageProfile').prop('checked', false);
        //        }
        //    }

        //    $('html, body').animate({ scrollTop: 0 }, 1200);

        //    StopLoading();

        //},
        success: function (ProfileData) {

            if (ProfileData.data.SuperAdmin != null) {

                $("#txtFirstName_ManageProfile").val(ProfileData.data.SuperAdmin.FirstName);
                $("#txtLastName_ManageProfile").val(ProfileData.data.SuperAdmin.LastName);
                $("#txtEmail_ManageProfile").val(ProfileData.data.SuperAdmin.Email);
                $("#txtUserName_ManageProfile").val(ProfileData.data.SuperAdmin.Username);
                $("#txtMobile_ManageProfile").val(ProfileData.data.SuperAdmin.PhoneNumber_Only);
                $("#txtPincode_ManageProfile").val(ProfileData.data.SuperAdmin.Pincode);
                $("#txtAddress_ManageProfile").val(ProfileData.data.SuperAdmin.Address);
                $("#ProfileName_ManageProfile").html(ProfileData.data.SuperAdmin.FirstName + " " + ProfileData.data.SuperAdmin.LastName);
                $("#a_SuperAdmin_SuperAdminLayout").html(ProfileData.data.SuperAdmin.FirstName + " " + ProfileData.data.SuperAdmin.LastName);
                $("#ProfileEmail_ManageProfile").html(ProfileData.data.SuperAdmin.Email);
                $("#imgProfile_ManageProfile").attr('src', '/Content/SuperAdminImages/' + ProfileData.data.SuperAdmin.ProfileImage);
                $("#img_SuperAdmin_SuperAdminLayout").attr('src', '/Content/SuperAdminImages/' + ProfileData.data.SuperAdmin.ProfileImage);

                $('#chkIsActive_ManageProfile').prop('checked', ProfileData.data.SuperAdmin.LoginStatus == 1);
            }

            else if (ProfileData.data.Admin != null) {

                $("#txtFirstName_ManageProfile").val(ProfileData.data.Admin.FirstName);
                $("#txtLastName_ManageProfile").val(ProfileData.data.Admin.LastName);
                $("#txtEmail_ManageProfile").val(ProfileData.data.Admin.Email);
                $("#txtUserName_ManageProfile").val(ProfileData.data.Admin.Username);
                $("#txtMobile_ManageProfile").val(ProfileData.data.Admin.PhoneNumber_Only);
                $("#txtPincode_ManageProfile").val(ProfileData.data.Admin.Pincode);
                $("#txtAddress_ManageProfile").val(ProfileData.data.Admin.Address);
                $("#ProfileName_ManageProfile").html(ProfileData.data.Admin.FirstName + " " + ProfileData.data.Admin.LastName);
                $("#a_Admin_AdminLayout").html(ProfileData.data.Admin.FirstName + " " + ProfileData.data.Admin.LastName);
                $("#ProfileEmail_ManageProfile").html(ProfileData.data.Admin.Email);
                $("#imgProfile_ManageProfile").attr('src', '/Content/AdminImages/' + ProfileData.data.Admin.ProfileImage);
                $("#img_Admin_AdminLayout").attr('src', '/Content/AdminImages/' + ProfileData.data.Admin.ProfileImage);

                $('#chkIsActive_ManageProfile').prop('checked', ProfileData.data.Admin.LoginStatus == 1);
            }

            else if (ProfileData.data.Staff != null) {

                $("#txtFirstName_ManageProfile").val(ProfileData.data.Staff.FirstName);
                $("#txtLastName_ManageProfile").val(ProfileData.data.Staff.LastName);
                $("#txtEmail_ManageProfile").val(ProfileData.data.Staff.Email);
                $("#txtUserName_ManageProfile").val(ProfileData.data.Staff.Username);
                $("#txtMobile_ManageProfile").val(ProfileData.data.Staff.PhoneNumber_Only);
                $("#txtPincode_ManageProfile").val(ProfileData.data.Staff.Pincode);
                $("#txtAddress_ManageProfile").val(ProfileData.data.Staff.Address);
                $("#ProfileName_ManageProfile").html(ProfileData.data.Staff.FirstName + " " + ProfileData.data.Staff.LastName);
                $("#a_Staff_StaffLayout").html(ProfileData.data.Staff.FirstName + " " + ProfileData.data.Staff.LastName);
                $("#ProfileEmail_ManageProfile").html(ProfileData.data.Staff.Email);
                $("#imgProfile_ManageProfile").attr('src', '/Content/StaffImages/' + ProfileData.data.Staff.ProfileImage);
                $("#img_Staff_StaffLayout").attr('src', '/Content/StaffImages/' + ProfileData.data.Staff.ProfileImage);

                $('#chkIsActive_ManageProfile').prop('checked', ProfileData.data.Staff.LoginStatus == 1);
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

function AddUpdateAdmin() {
    var _firstName_MS = $("#txtFirstName_ManageProfile").val();
    var _lastName_MS = $("#txtLastName_ManageProfile").val();
    var _mobile_MS = $("#txtMobile_ManageProfile").val();
    var _pincode_MS = $("#txtPincode_ManageProfile").val();
    var _address_MS = $("#txtAddress_ManageProfile").val();
    //var _captcha = CheckCaptcha('mainCaptcha', 'inputCaptcha', 'mainCaptcha_error');
    var email_test = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var _email_MS = '';

    var _is_active_staff = 0;
    if ($('#chkIsActive_ManageProfile').is(':checked')) {
        // checked
        _is_active_staff = 1;
    }
    var _is_valid = true;
    $(".errorsClass").html('');


    if (_firstName_MS == '' || _firstName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#firstName_error_ManageProfile").html('Please enter first name!');
    }

    if (_lastName_MS == '' || _lastName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#lastName_error_ManageProfile").html('Please enter last name!');
    }
    if (logged_In_UserType_Global == 1) {
        _email_MS = ($("#txtEmail_ManageProfile").val()).toLowerCase();
        if (_email_MS == '' || _email_MS.replace(/\s/g, "") == "") {
            _is_valid = false;
            $("#email_error_ManageProfile").html('Please enter the email address!');
        }
        else if (!email_test.test(_email_MS)) {
            _is_valid = false;
            $("#email_error_ManageProfile").html('Please enter the valid email address!');
        }
    }

    if (_mobile_MS == '' || _mobile_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#mobile_error_ManageProfile").html('Please enter mobile-number!');
    }
    else if (_mobile_MS.toString().length < 10) {
        _is_valid = false;
        $("#mobile_error_ManageProfile").html('Mobile-number should be of 10 digits!');
    }

    if (_pincode_MS == '' || _pincode_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#pincode_error_ManageProfile").html('Please enter pincode!');
    }
    if (_address_MS == '' || _address_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#address_error_ManageProfile").html('Please enter address!');
    }
    if (_is_valid == true) {
        StartLoading();

        var data = new FormData();

        data.append("firstName", _firstName_MS);
        data.append("lastName", _lastName_MS);
        data.append("email", _email_MS);
        data.append("mobile", '+91' + _mobile_MS);
        data.append("countryMobilePhoneCodeOnly", '+91');
        data.append("mobileNumberOnly", _mobile_MS);
        data.append("pincode", _pincode_MS);
        data.append("address", _address_MS);
        data.append("isActive", _is_active_staff);


        $.ajax({
            url: '/InsertUpdateProfileData',
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
                    if (dataResponse.status == 1) {
                        CustomSwalPoup("Success!", dataResponse.message, "success");
                        $(".errorsClass").html('');


                        Staff_ID_Global = 0;
                        Admin_ID_Global = 0;
                        SuperAdmin_ID_Global = 0;
                        //--refresh Staff And Admin list
                        GetProfileData();
                    }
                    else {
                        GetProfileData();
                        CustomSwalPoup("Message!", dataResponse.message, "warning");
                    }
                
            },
            error: function (result) {
                GetProfileData();

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

function ConfirmProfileImageReset() {
    swal({
        title: "Reset Profile Image",
        text: "Are you sure to reset your profile image?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        TypeProfileImage('ResetImage');
    });
}

function TypeProfileImage(_TypeProfileImage) {
    var _firstName_MS = $("#txtFirstName_ManageProfile").val();
    var _profileImageName = null;
    var _profileImageVal = null;
    if (_TypeProfileImage == "NewImage") {
        var _Image = $("#imgupload").get(0);
        if (!$("#imgupload").val().length)
            return;
        var _profileImage = _Image.files;
        var num = _profileImage.length;
        if (num > 0) {
            _profileImageName = _profileImage[0].name;
            _profileImageVal = _profileImage[0];
        }
        else {
            return;
        }
    }
    //Reset Image Uplaod
    $("#imgupload").val('');

    //StartLoading();

    var data = new FormData();
    data.append("TypeProfileImage", _TypeProfileImage)
    data.append("_FirstName", _firstName_MS)
    if (_TypeProfileImage == "NewImage") {
        data.append(_profileImageName, _profileImageVal);
    }

    $.ajax({
        url: '/ProfileImageSet',
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
            if (dataResponse.status == 1) {
                var ImagePath = dataResponse.data.imagePath;
                if (dataResponse.data.usertype == 1) {
                    document.getElementById('img_SuperAdmin_SuperAdminLayout').src = ImagePath;
                }
                else if (dataResponse.data.usertype == 2) {
                    document.getElementById('img_Admin_AdminLayout').src = ImagePath;
                }
                else if (dataResponse.data.usertype == 3) {
                    document.getElementById('img_Staff_StaffLayout').src = ImagePath;
                }

                document.getElementById('imgProfile_ManageProfile').src = ImagePath;
                CustomSwalPoup("Success!", dataResponse.message, "success");
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
