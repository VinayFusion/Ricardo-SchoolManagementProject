$(document).ready(function () {
    $("#txtEmail").focus();
    //--Stop Page-Loading
    StopLoading();
});

function CheckLogin() {
    var _email = $("#txtEmail").val();
    var _password = $("#txtPassword").val();
    var TestEmail = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    var _is_valid = true;

    $(".errorsClass").html('');

    if (_email == '') {
        _is_valid = false;
        $("#email_error_Login").html('Please enter email!');
    }
    if (!TestEmail.test(_email) && _email != '') {
        _is_valid = false;
        $("#email_error_Login").html('Please enter the valid Email Address!');
    }

    if (_password == '') {
        _is_valid = false;
        $("#password_error_Login").html('Please enter Password!');
    }

    if (_is_valid == false) {
        return false;
    }
    else {
        return true;
    }
}