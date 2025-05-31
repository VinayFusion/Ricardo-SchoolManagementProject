$(document).ready(function () {
    $("#txtEmail").focus();
    //--Stop Page-Loading
    StopLoading();
});

//function forgotPassword() {
$("#forgotPasswordForm").submit(function (e) {
    e.preventDefault();
    var email = $("#txtEmail").val();
    var TestEmail = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    $('.errorsClass').html('');
    $('.successClass').html('');
    var _is_valid = true;

    // validate
    if (email == "") {
        _is_valid = false;
        $("#email_error_Login").html('Please enter email!');
    }
    if (!TestEmail.test(email) && email != '') {
        _is_valid = false;
        $("#email_error_Login").html('Please enter the valid Email Address!');
    }

    if (_is_valid == true) {
        $('.errorsClass').html('');
        $('.successClass').html('');
        //console.log('email entered!');

        StartLoading();

        $.ajax({
            url: '/Login/ForgotPassword/?email=' + email,
            processData: false,
            contentType: 'application/json',
            type: 'POST',
            success: function (dataResponse) {
                StopLoading();
                //--Parse into Json of response-json-string
                //dataResponse = JSON.parse(dataResponse);

                //--If successfully added/updated
                if (dataResponse.status == 1) {
                    $("#resp_success_Login").html(dataResponse.message);
                    $('#resp_success_Login').append(`<br/><br/> <a href="${dataResponse.data.token}">${dataResponse.data.token}</a>`);
                }
                else if (dataResponse.status < 0) {
                    $("#resp_error_Login").html(dataResponse.message);
                }

            },
            error: function (result) {
                StopLoading();
                $("#resp_error_Login").html("There is some technical error, please try again!");
            }
        });
    }
});