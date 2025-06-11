var UserToken_Global = "";
var School_ID_Global = 0;
var SchoolTypeId_Global = 0;

$(document).ready(function () {

    StartLoading();

    $.get("/SuperAdmin/GetSuperAdminCookieDetail", null, function (dataSuperAdminToken) {
        if (dataSuperAdminToken != "" && dataSuperAdminToken != null) {

            UserToken_Global = dataSuperAdminToken;
            
            // Wait for GetSchoolList to finish
            GetSchoolList().done(function () {
                StopLoading();
            });
        }
        
        else {
           
        }
    });
});

// Function to show the form for adding a new school
function AddNewSchool_ShowForm() {

    $('#chkIsActive_ManageSchool').prop('checked', false);
    $("#dv_SchoolListBox").hide();
    $("#btnAddNewSchool").hide();
    $("#dv_AddUpdateSchoolForm").show();

    $("#Title_SchoolForm_ManageSchool").html('Add New School');
    handleShowSchoolLogoImagePreview('/Content/Images/fileuploadpreview.jpg');
}

// Function to cancel the form and reset fields
function CancelForm() {

    $('#chkIsActive_ManageSchool').prop('checked', false);
    $("#schoolLogoImage").val(''); // Reset file input
    $("#txtFirstName_ManageSchool").val('');
    $("#txtLastName_ManageSchool").val('');
    $("#txtSchoolName_ManageSchool").val('');
    $("#ddlSchoolType_ManageSchool").val(0).trigger('change');
    $("#txtEmail_ManageSchool").val('');
    $("#txtPassword_ManageSchool").val('');
    $("#txtMobile_ManageSchool").val('');
    $("#txtPincode_ManageSchool").val('');
    $("#txtAddress_ManageSchool").val('');
    $('#chkIsActive_ManageSchool').prop('checked', true);

    $(".errorsClass").html('');

    $("#btnSubmitManageSchool").show();
    $("#btnUpdateManageSchool").hide();

    $("#dv_AddUpdateSchoolForm").hide();
    $("#dv_SchoolListBox").show();
    $("#btnAddNewSchool").show();

    //--Set Global Variable
    School_ID_Global = 0;
    SchoolTypeId_Global = 0;
}

// Function to get the list of schools and populate the DataTable
function GetSchoolList() {

    $.ajax({
        type: "GET",
        url: "/GetAllSchool",
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (data) {
            ;
            var sno = 0;
            var tableData = [];

            // Destroy old DataTable instance if exists
            if ($.fn.DataTable.isDataTable('#tblSchool_ManageSchool')) {
                $('#tblSchool_ManageSchool').DataTable().destroy();
            }

            // Loop through schools array
            for (var i = 0; i < data.data.school.length; i++) {
                var school = data.data.school[i];
                sno++;

                // School info with name, email, and contact
                var _info = '<span>' + school.SchoolName + '</span>' +
                    '<br /><span>(' + (school.Email || '-') + ')</span>' +
                    '<br /><span>(' + (school.PhoneNumber || '-') + ')</span>';

                // Edit and delete icons with onclick events passing index or ID if you have
                // Assuming you have an ID field, else use index 'i'
                var schoolId = school.Id || i;

                var _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit School-Information" onclick="EditSchoolInfo(' + schoolId + ');" />';
                var _delete = '<img src="/Content/Images/delete_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Delete School" onclick="ConfirmDeleteSchool(' + schoolId + ');" />';

                // Status button
                var _status = '';
                if (school.LoginStatus == 1) {
                    _status = '<a class="btn btn-success btn-sm" style="width:80px;" onclick="ConfirmChangeStatusSchool(' + schoolId + ');">' + window.localizedLabels.active + '</a>';
                } else {
                    _status = '<a class="btn btn-danger btn-sm" style="width:80px;" onclick="ConfirmChangeStatusSchool(' + schoolId + ');">' + window.localizedLabels.inactive + '</a>';
                }

                var schoolTypeMap = {
                    1: "Preschool",
                    2: "Elementary & Middle School",
                    3: "High School",
                    4: "Technical / Vocational School"
                };
                
                tableData.push([
                    sno,
                    _info,
                    schoolTypeMap[school.SchoolType],
                    school.Address,
                    school.Pincode,
                    _status,
                    _edit,
                    _delete
                ]);
            }

            $('#tblSchool_ManageSchool').DataTable({
                data: tableData,
                deferRender: true,
                //scrollY: 200,
                scrollCollapse: true,
                scroller: true
            });


            StopLoading();
        },
        error: function (result) {
            StopLoading();

            if (result.status == 401) {
                $.iaoAlert({
                    msg: 'Unauthorized! Invalid Token!',
                    type: "error",
                    mode: "dark",
                });
            } else {
                $.iaoAlert({
                    msg: 'There is some technical error, please try again!',
                    type: "error",
                    mode: "dark",
                });
            }
        }
    });
}

// Function to add or update school information
function AddUpdateSchool(_mode) {
    var _firstName_MS = $("#txtFirstName_ManageSchool").val();
    var _lastName_MS = $("#txtLastName_ManageSchool").val();
    var _schoolName_MS = $("#txtSchoolName_ManageSchool").val();
    var _schoolType_MS = $("#ddlSchoolType_ManageSchool").val(); // schooltype_error_ManageSchool
    var _email_MS = $("#txtEmail_ManageSchool").val();
    var _password_MS = $("#txtPassword_ManageSchool").val();
    var _mobile_MS = $("#txtMobile_ManageSchool").val();
        
    var _pincode_MS = $("#txtPincode_ManageSchool").val();
    var _address_MS = $("#txtAddress_ManageSchool").val();
    

    var email_test = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    var _is_valid = true;
    $(".errorsClass").html('');

    var status = 0;
    if ($('#chkIsActive_ManageSchool').is(':checked')) {
        // checked
        status = 1;
    }
    if (_firstName_MS == '' || _firstName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#firstName_error_ManageSchool").html('Please enter first name!');
    }

    if (_lastName_MS == '' || _lastName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#lastName_error_ManageSchool").html('Please enter last name!');
    }
    if (_schoolName_MS == '' || _schoolName_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#schoolName_error_ManageSchool").html('Please enter school name!');
    }
    if (_schoolType_MS == undefined || _schoolType_MS == null || _schoolType_MS == '' || _schoolType_MS == 0) {
        _is_valid = false;
        $("#schoolType_error_ManageSchool").html('Please select the School Type!');
    }
    if (_email_MS == '' || _email_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#email_error_ManageSchool").html('Please enter the email address!');
    }
    else if (!email_test.test(_email_MS)) {
        _is_valid = false;
        $("#email_error_ManageSchool").html('Please enter the valid email address!');
    }

    if (_password_MS == '' || _password_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#password_error_ManageSchool").html('Please enter password!');
    }

    if (_mobile_MS == '' || _mobile_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#mobile_error_ManageSchool").html('Please enter mobile-number!');
    }
    else if (_mobile_MS.toString().length < 10) {
        _is_valid = false;
        $("#mobile_error_ManageSchool").html('Mobile-number should be of 10 digits!');
    }

    if (_pincode_MS == '' || _pincode_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#pincode_error_ManageSchool").html('Please enter pincode!');
    }
    if (_address_MS == '' || _address_MS.replace(/\s/g, "") == "") {
        _is_valid = false;
        $("#address_error_ManageSchool").html('Please enter address!');
    }
    // Validate school logo input
    var fileInput = $('#schoolLogoImage')[0];
    if (!fileInput.files || fileInput.files.length === 0) {
        _is_valid = false;
        $("#schoolLogo_error_ManageSchool").html('Please upload a school logo image!');
    } else {
        $("#schoolLogo_error_ManageSchool").html('');
    }

    if (_is_valid == true) {
        StartLoading();

        var data = new FormData();

        data.append("Id", School_ID_Global);
        data.append("firstName", _firstName_MS);
        data.append("lastName", _lastName_MS)
        data.append("schoolName", _schoolName_MS)
        data.append("schoolTypeId", _schoolType_MS);
        data.append("email", _email_MS.toLowerCase());
        data.append("mobile", '+91' + _mobile_MS);
        data.append("countryMobilePhoneCodeOnly", '+91');
        data.append("mobileNumberOnly", _mobile_MS);
        data.append("password", _password_MS);
        data.append("pincode", _pincode_MS);
        data.append("address", _address_MS);
        data.append("status", status);
        data.append("mode", _mode);

        // Get the schoollogo -image choosen
        var fileInput = $('#schoolLogoImage')[0];

        // Check if a file is selected
        if (fileInput.files && fileInput.files.length > 0) {
            data.append("schoolLogoImage", fileInput.files[0]);
        }

        $.ajax({
            url: '/InsertUpdateSchool',
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
                    $("#txtFirstName_ManageSchool").val('');
                    $("#schoolLogoImage").val(''); // Reset file input
                    $("#txtLastName_ManageSchool").val('');
                    $("#txtSchoolName_ManageSchool").val('');
                    $("#ddlSchoolType_ManageSchool").val(0).trigger('change');
                    $("#txtEmail_ManageSchool").val('');
                    $("#txtPassword_ManageSchool").val('');
                    $("#txtMobile_ManageSchool").val('');
                    $("#txtPincode_ManageSchool").val('');
                    $("#txtAddress_ManageSchool").val('');
                    $('#chkIsActive_ManageSchool').prop('checked', true);

                    $(".errorsClass").html('');

                    $("#btnSubmitManageSchool").show();
                    $("#btnUpdateManageSchool").hide();

                    $("#dv_AddUpdateSchoolForm").hide();
                    $("#dv_SchoolListBox").show();
                    $("#btnAddNewSchool").show();

                    //--Set School-ID in the Global Variable
                    School_ID_Global = 0;
                    SchoolTypeId_Global = 0;
                    //-------------------------------------

                    //--refresh School list
                    GetSchoolList();
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

// Function to edit school information by ID
function EditSchoolInfo(sid) {
    StartLoading();
    School_ID_Global = sid;
    $.ajax({
        type: "GET",
        url: "/GetSchoolById?schoolID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (data) {
       
            if (data.data.School != null) {
                var school = data.data.School;
                $("#txtFirstName_ManageSchool").val(school.FirstName);
                $("#txtLastName_ManageSchool").val(school.LastName);
 /*               $("#schoolLogoImage").val(school.SchoolLogoImage);*/
                $("#txtSchoolName_ManageSchool").val(school.SchoolName);
                $("#txtEmail_ManageSchool").val(school.Email);
                $("#txtPassword_ManageSchool").val(school.Password);
                $("#txtMobile_ManageSchool").val(school.PhoneNumber_Only);
                $("#txtPincode_ManageSchool").val(school.Pincode);
                $("#txtAddress_ManageSchool").val(school.Address);
                $("#ddlSchoolType_ManageSchool").val(school.SchoolType).trigger('change');

                // Set the school logo preview
                if (school.SchoolLogoImage && school.SchoolLogoImage.trim() != "") {
                    $('#schoolLogoImagePreview').attr('src', school.SchoolLogoImage)
                }


                // Status checkbox
                //$('#chkIsActive_ManageSchool').prop('checked', school.Status == 1);

                //--School-Status
                if (school.LoginStatus == 1) {

                    $('#chkIsActive_ManageSchool').prop('checked', true);
                }
                else {
                    $('#chkIsActive_ManageSchool').prop('checked', false);
                }

                //--Show Update Button only
                $("#btnSubmitManageSchool").hide();
                $("#btnUpdateManageSchool").show();

                //--Hide Add-New-School Button
                $("#btnAddNewSchool").hide();

                //--Set the Form-Title as (Update School Information)
                $("#Title_SchoolForm_ManageSchool").html('Update School Information');

                //--Show the Update-School-Box only
                $("#dv_AddUpdateSchoolForm").show();
                $("#dv_SchoolListBox").hide();
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

// Function to confirm deletion of a school
function ConfirmDeleteSchool(sid) {
    swal({
        title: "Delete School",
        text: "Are you sure to delete this School?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        DeleteSchool(sid);
    });
}

// Function to delete a school by ID
function DeleteSchool(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/DeleteSchoolById?schoolID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if school successfully deleted
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get School List
                    GetSchoolList();
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

// Function to confirm change of status for a school
function ConfirmChangeStatusSchool(sid) {
    swal({
        title: "Change Status",
        text: "Are you sure to change status of this School?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        ChangeStatusSchool(sid);
    });
}

// Function to change the status of a school by ID
function ChangeStatusSchool(sid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/ChangeSchoolStatusById?schoolID=" + sid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if school-status successfully updated
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get School List
                    GetSchoolList();
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

function ShowSchoolImagePreview(input) {

    $('#schoolLogo_error').html('');

    var fileExtension = ['jpeg', 'jpg', 'png'];
    if ($.inArray(input.value.split('.').pop().toLowerCase(), fileExtension) == -1) {
        //removeBarcodeImage();
        $('#schoolLogo_error').html('Sorry!, Only .jpeg, .jpg and .png  formats are allowed.')

    }
    else {
        //--if select the valid image file---

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {

                var image = new Image();
                //Set the Base64 string return from FileReader as source.
                image.src = e.target.result;
                image.onload = function () {

                    //var width = this.width;
                    //var height = this.height;

                    // Create a square canvas
                    const canvas = document.getElementById('canvas');
                    const ctx = canvas.getContext('2d');

                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                    // Determine the size of the square (smallest dimension)
                    const squareSize = Math.min(image.width, image.height);

                    // Set canvas dimensions to the square size
                    canvas.width = squareSize;
                    canvas.height = squareSize;

                    // Calculate the top-left coordinates to crop the image to the center
                    const xOffset = (image.width - squareSize) / 2;
                    const yOffset = (image.height - squareSize) / 2;

                    // Draw the cropped image onto the canvas
                    ctx.drawImage(image, xOffset, yOffset, squareSize, squareSize, 0, 0, squareSize, squareSize);

                    // Convert canvas to Blob
                    canvas.toBlob(function (blob) {
                        // Create a File object
                        const croppedFile = new File([blob], 'schoollogo_image.png', { type: 'image/png' });

                        var fileInput = document.getElementById('schoolLogoImage');

                        // Replace the file input with the cropped image
                        var dataTransfer = new DataTransfer(); // Create a DataTransfer object
                        dataTransfer.items.add(croppedFile); // Add the cropped file
                        fileInput.files = dataTransfer.files; // Assign the new file list to the input

                        // Call the function to handle the preview of the cropped image
                        handleShowSchoolLogoImagePreview(URL.createObjectURL(croppedFile));
                    }, 'image/png');


                    // Convert canvas back to an image URL and set it to the img element
                    //const croppedImageUrl = canvas.toDataURL('image/png');
                    //document.getElementById('croppedImage').src = croppedImageUrl;
                    //document.getElementById('croppedImage').style.display = 'block';

                    //handleShowStudentProfileImagePreview(croppedImageUrl);
                    //// Check if the image is square
                    //if (width === height) {
                    //    handleShowStudentProfileImagePreview(e.target.result);
                    //    return true;
                    //} else {
                    //    $('#profilePicture_error_ManageStudent').html('Sorry!, Only square images are allowed (equal width and height).')

                    //    input.value = '';
                    //    return false;
                    //}
                };
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
}

function handleShowSchoolLogoImagePreview(imageSrc) {
    $('#schoolLogoImagePreview').attr('src', imageSrc);
}