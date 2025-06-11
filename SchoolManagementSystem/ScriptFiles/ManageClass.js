var UserToken_Global = "";
var logged_In_UserType_Global = 0;
var Class_ID_Global = 0;
var Session_ID_Global = 0;

$(document).ready(function () {
    StartLoading();
    $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
        if (dataAdminToken != "" && dataAdminToken != null) {

            UserToken_Global = dataAdminToken;

            logged_In_UserType_Global = 1;
            logged_In_UserTypeName_Global = "Admin";

            GetAllClassList();
        }
        else {
            $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                if (dataStaffToken != "" && dataStaffToken != null) {

                    UserToken_Global = dataStaffToken;

                    logged_In_UserType_Global = 2;
                    logged_In_UserTypeName_Global = "Staff";


                    $('#btnShowAllRecordsFilter').hide();
                    GetAllClassList();
                }
            });
        }
    });
});

function GetAllClassList() {
        _url = "/GetAllClass";

    $.ajax({
        type: "GET",
        url: _url,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataClass) {
            StopLoading();
            var sno = 0;
            var _edit = '';
            var _delete = '';
            

            var data = [];

            var _table = $('#tblCLass_ManageClass').DataTable();
            _table.destroy();

            for (var i = 0; i < dataClass.data.Class.length; i++) {
                sno++;

                _edit = '<img src="/Content/Images/edit_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Edit Class-Information" onclick="EditClassInfo(' + dataClass.data.Class[i].ClassId + ');" />';
                _delete = '<img src="/Content/Images/delete_icon.png" style="width:25px;height:25px;cursor:pointer;" title="Delete Class" onclick="ConfirmDeleteClass(' + dataClass.data.Class[i].ClassId + ');" />';
                if (logged_In_UserType_Global ==1) {
                    data.push([
                        sno,
                        dataClass.data.Class[i].ClassName,
                        _edit,
                        _delete
                    ]);
                }
                else {
                    data.push([
                        sno,
                        dataClass.data.Class[i].ClassName,
                    ]);
                }
                
            }

            $('#tblCLass_ManageClass').DataTable({
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

function CancelForm() {
    setClassFormDefaultValues();

    $("#btnSubmitManageClass").show();
    $("#btnUpdateManageClass").hide();
    $("#dv_AddUpdateClassForm").hide();
    $("#dv_ClassListBox").show();
    $("#btnAddNewClass").show();
}

function AddNewClass_ShowForm() {
    $("#dv_ClassListBox").hide();
    $("#btnAddNewClass").hide();
    $("#Title_ClassForm_ManageClass").html(window.localizedLabels.classTitle);
    $("#dv_AddUpdateClassForm").show();
    setClassFormDefaultValues();
}

function AddUpdateClass(_mode) {
   var SessionId = 0 ; //$("#ddlSession_ManageClass").val();
    var className = $("#txtSelectClass_ManageClass").val();
    
    var _is_valid = true;
    $(".errorsClass").html('');
    
    //if (SectionName.length == 0){
    //    _is_valid = false;
    //    $("#Section_error_ManageClass").html('Please select at least one section!');
    //}
    if (className == undefined || className == null || className == '' || className == 0) {
        _is_valid = false;
        $("#SelectClass_error_ManageClass").html('Please select class!');
    }



    if (_is_valid == true) {

        StartLoading();

        var data = new FormData();

        data.append("CId", Class_ID_Global);
        data.append("SessionId", SessionId);
        data.append("ClassName", className);
        data.append("mode", _mode);

        $.ajax({
            url: '/InsertUpdateClass',
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

                    $("#btnSubmitManageClass").show();
                    $("#btnUpdateManageClass").hide();

                    $("#dv_AddUpdateClassForm").hide();
                    $("#dv_ClassListBox").show();
                    $("#btnAddNewClass").show();
                    

                    //--refresh Class list
                    GetAllClassList();
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

function ConfirmDeleteClass(Cid) {
    swal({
        title: "Delete Class",
        text: "Are you sure to delete this Class?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes',
        cancelButtonText: "No"
    }, function (isConfirm) {
        if (!isConfirm) return;
        DeleteClass(Cid);
    });
}

function DeleteClass(Cid) {
    StartLoading();
    $.ajax({
        type: "GET",
        url: "/DeleteClassById?ClassID=" + Cid,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataResponse) {
            StopLoading();

            //--Check if course successfully deleted
            if (dataResponse.status == 1) {
                setTimeout(function () {
                    CustomSwalPoup("Success!", dataResponse.message, "success");
                    //--Get Sessions List
                    GetAllClassList();
                }, 100);
            }
            else if (dataResponse.status == 2) {
                setTimeout(function () {
                swal("Warning!", dataResponse.message, "warning");
                    //--Get Sessions List
                    //GetAllClassList();
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

function EditClassInfo(Cid) {
        StartLoading();
        Class_ID_Global = Cid;
        $.ajax({
            type: "GET",
            url: "/GetClassById?ClassID=" + Cid,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (dataClass) {

                if (dataClass.data.Class != null) {
                   // Session_ID_Global = dataClass.data.Class.SessionId;
                    
                   // $("#ddlSession_ManageClass").val(parseInt(dataClass.data.Class.SessionId)).trigger('change');
                    $("#txtSelectClass_ManageClass").val(dataClass.data.Class.ClassName);
                    //--Show Update Button only
                    $("#btnSubmitManageClass").hide();
                    $("#btnUpdateManageClass").show();

                    //--Hide Add-New-Class Button
                    $("#btnAddNewClass").hide();

                    //--Set the Form-Name as (Update Class Information)
                    $("#Title_ClassForm_ManageClass").html('Update Class Information');

                    //--Show the Update-Class-Box only
                    $("#dv_AddUpdateClassForm").show();
                    $("#dv_ClassListBox").hide();
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

function setClassFormDefaultValues() {
    //-----------Set Default Values------------
    $("#txtSelectClass_ManageClass").val('');
    $(".errorsClass").html('');
}