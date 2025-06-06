$(document).ready(function () {
    $("#txtPassword").focus();
    //--Stop Page-Loading
    StopLoading();
});


function resetPassword() {
    var password = $("#Password").val();
    var confirm_pass = $("#ConfirmPassword").val();

    $('.errorsClass').html('');
    var isValid = true;
    // validate
    if (password == "") {
        isValid = false;
        $("#password_error_Login").html('Please enter new password!');
    }
    if (confirm_pass == "") {
        isValid = false;
        $("#confirm_password_error_Login").html('Please enter confrim password!');
    }

    if (password != "" && confirm_pass != "" && password != confirm_pass) {
        isValid = false;
        $('.errorsClass').html('');
        $("#confirm_password_error_Login").html('Password does not matched!');
    }
    if (isValid == true) {
        $('.errorsClass').html('');
        /*console.log('password matched!');*/
        return true;
    }
    else {
        return false;
    }
}