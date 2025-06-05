var UserToken_Global = "";
var logged_In_UserType_Global = 1;

$(document).ready(function () {
    StartLoading();
    $('#txtOldPassword_ChangePassword').focus();

    // SuperAdmin check
    $.get("/SuperAdmin/GetSuperAdminCookieDetail", null, function (dataSuperAdminToken) {
        if (dataSuperAdminToken != "" && dataSuperAdminToken != null) {
            UserToken_Global = dataSuperAdminToken;
            logged_In_UserType_Global = 1; // SuperAdmin
            StopLoading();
        } else {
            // Admin check
            $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
                if (dataAdminToken != "" && dataAdminToken != null) {
                    UserToken_Global = dataAdminToken;
                    logged_In_UserType_Global = 2; // Admin
                    StopLoading();
                } else {
                    // Staff check
                    $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                        if (dataStaffToken != "" && dataStaffToken != null) {
                            UserToken_Global = dataStaffToken;
                            logged_In_UserType_Global = 3; // Staff
                            StopLoading();
                        } else {
                            StopLoading(); // No token found
                        }
                    });
                }
            });
        }
    });
});


function ResetForm() {
    //--Set Default Values Fields

    $('#txtOldPassword_ChangePassword').val('');
    $('#txtNewPassword_ChangePassword').val('');
    $('#txtConfirmNewPassword_ChangePassword').val('');

    $(".errorsClass").html('');
}

function updateChangePassword() {

    var _oldPassword_CP = $('#txtOldPassword_ChangePassword').val();
    var _newPassword_CP = $('#txtNewPassword_ChangePassword').val();
    var _confirmNewPassword_CP = $('#txtConfirmNewPassword_ChangePassword').val();

    var _is_valid = true;
    $(".errorsClass").html('');


    if (_oldPassword_CP == '') {
        _is_valid = false;
        $("#oldPassword_error_ChangePassword").html('Please enter the old password!');
    }
    if (_newPassword_CP == '') {
        _is_valid = false;
        $("#newPassword_error_ChangePassword").html('Please enter the new password!');
    }
    if (_confirmNewPassword_CP == '') {
        _is_valid = false;
        $("#confirmNewPassword_error_ChangePassword").html('Please re-enter the new password!');
    }

    if (_newPassword_CP != "" && _confirmNewPassword_CP != "" && _newPassword_CP != _confirmNewPassword_CP) {
        _is_valid = false;
        $('.errorsClass').html('');
        $("#confirmNewPassword_error_ChangePassword").html('Password does not matched!');
    }

    if (_is_valid == true) {
        StartLoading();

        var data = new FormData();

        data.append("oldPassword", _oldPassword_CP);
        data.append("newPassword", _newPassword_CP);
        data.append("confirmNewPassword", _confirmNewPassword_CP);
        console.log("Token: " + UserToken_Global);
        $.ajax({
            url: '/UpdatePassword',
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
                StopLoading();
                //--If successfully added/updated
                if (dataResponse.status == 1 || dataResponse.status == 2) {
                    CustomSwalPoup("Success!", dataResponse.message, "success");

                    //-----------Set Default Form Values------------
                    ResetForm();
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


function changePasswordToText() {
    if ($('#chkAllPassword_ChangePassword').is(':checked')) {
        $("#txtOldPassword_ChangePassword").prop("type", "text");
        $('#txtNewPassword_ChangePassword').prop("type", "text");
        $('#txtConfirmNewPassword_ChangePassword').prop("type", "text");
    }
    else {
        $('#txtOldPassword_ChangePassword').prop("type", "password");
        $('#txtNewPassword_ChangePassword').prop("type", "password");
        $('#txtConfirmNewPassword_ChangePassword').prop("type", "password");
    }
}