var UserToken_Global = "";
var Student_ID_Global = 0;
var logged_In_UserType_Global = 1;

$(document).ready(function () {
    StartLoading();

    Student_ID_Global = GetParameterValues('sid');

    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {

            UserToken_Global = dataAdminToken;

            logged_In_UserType_Global = 1;

           
            GetAllDdlData();
        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;

                   
                    GetAllDdlData();
                }
                else {

                }
            });
        }
    });
});

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
    var _FullName_MS = $("#StudentName_StudentDetail").html().replace(/ /g, '');
    var _firstName_MS = $.trim(_FullName_MS);
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

    var data = new FormData();
    data.append("id", Student_ID_Global)
    data.append("TypeProfileImage", _TypeProfileImage)
    data.append("_FirstName", _firstName_MS)
    if (_TypeProfileImage == "NewImage") {
        data.append(_profileImageName, _profileImageVal);
    }

    $.ajax({
        url: '/StudentProfileImageSet',
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
                document.getElementById('imgProfile_StudentDetail').src = ImagePath;
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

function GetAllDdlData() {
    // get data Session 
    $.ajax({
        type: "GET",
        url: "/GetAllDdlData4StudentProfile?studentID=" + Student_ID_Global,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataSession) {
            
            var res_Session = '<option value="0">Session(Class)</option>';

            for (var i = 0; i < dataSession.data.Student.length; i++) {

                res_Session += '<option value="' + dataSession.data.Student[i].SessionId+'">' + dataSession.data.Student[i].SessionName+'</option>';
            }

            $("#ddlSession_ManageStudent").html('');
            $("#ddlSession_ManageStudent").append(res_Session);
            GetStudentInfo();
        },
        error: function (result) {

            GetStudentInfo();

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

function GetStudentInfo() {

    $.ajax({
        type: "GET",
        url: "/GetStudentById?studentID=" + Student_ID_Global,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataStudent) {

            if (dataStudent.data.student != null) {

                $("#StudentName_StudentDetail").html(dataStudent.data.student.FirstName + ' ' + dataStudent.data.student.LastName);
                $("#StudentEmail_StudentDetail").html(dataStudent.data.student.Email);
                $("#Address_StudentDetail").html(dataStudent.data.student.Address);
                $("#Mobile_StudentDetail").html(dataStudent.data.student.PhoneNumber);
                $("#DateOfBirth_StudentDetail").html(dataStudent.data.student.DateOfBirth);
                $("#JoiningTime_StudentDetail").html(dataStudent.data.student.JoiningTime);

                $("#imgProfile_StudentDetail").attr('src', '/Content/StudentImages/' + dataStudent.data.student.ProfileImage);

                if (dataStudent.data.student.FeeData != null) {

                    var res_FeeData = '';
                    var _remark_val = '';

                    for (var i = 0; i < dataStudent.data.student.FeeData.length; i++) {

                        if (dataStudent.data.student.FeeData[i].Remarks == undefined || dataStudent.data.student.FeeData[i].Remarks == null) {
                            _remark_val = '';
                        }
                        else {
                            _remark_val = dataStudent.data.student.FeeData[i].Remarks;
                        }

                        res_FeeData += '<tr>' +
                            '<td>' + (i + 1) + '</td>' +
                            '<td>' + dataStudent.data.student.FeeData[i].CourseName + '</td>' +
                            '<td>' + dataStudent.data.student.FeeData[i].PaidFees + '</td>' +
                            '<td>' + dataStudent.data.student.FeeData[i].Discount + '</td>' +
                            '<td>' + dataStudent.data.student.FeeData[i].PendingFees + '</td>' +
                            '<td>' + dataStudent.data.student.FeeData[i].StartDate_FormatDate + '</td>' +
                            '<td>' + dataStudent.data.student.FeeData[i].EndDate_FormatDate + '</td>' +
                            '<td>' + _remark_val + '</td>' +
                            '</tr>';
                    }

                    $("#tbody_StudentDetail").append(res_FeeData);
                }
            }

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

function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}

function FilterPayFeeByCourse() {

}

//function GetAllJoinedClasses() {

//    $.ajax({
//        type: "GET",
//        url: "/GetAllJoinedClasses",
//        headers: {
//            "Authorization": "Bearer " + UserToken_Global,
//            "Content-Type": "application/json"
//        }, 
//        contentType: 'application/json',
//        success: function (dataClasses) {

//            var res_Courses_list = '<option value="0">All</option>';

//            for (var i = 0; i < dataClasses.data.courses.length; i++) {

//                res_Courses_list += '<option value="' + dataClasses.data.courses[i].Id + '">' + dataClasses.data.courses[i].Title + '</option>';
//            }

//            $("#ddlCourse_StudentDetail").append(res_Courses_list);

//            //--Get Student-Detail
//            GetStudentInfo();
//        },
//        error: function (result) {
//            //--Get Student-Detail
//            GetStudentInfo();

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