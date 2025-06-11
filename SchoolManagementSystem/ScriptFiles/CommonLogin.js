var UserToken_Global_Layout = "";

$(document).ready(function () {

    $(".commonNavLinkClass").removeClass("active");
    $(".commonSubNavLinkClass").removeClass("active");
    $("#li_ManageReports_Sidebar").removeClass("menu-open");

    // First check for SuperAdmin
    $.get("/SuperAdmin/GetSuperAdminCookieDetail", null, function (dataSuperAdminToken) {
        if (dataSuperAdminToken != "" && dataSuperAdminToken != null) {

            UserToken_Global_Layout = dataSuperAdminToken;

            //--Get SuperAdmin-Profile Info
            GetSuperAdminProfileDetail_Layout();
        }
        else {
            // Then check for Admin
            $.get("/Admin/GetAdminCookieDetail", null, function (dataAdminToken) {
                if (dataAdminToken != "" && dataAdminToken != null) {

                    UserToken_Global_Layout = dataAdminToken;

                    //--Set Active Link in Sidebar
                    SetSideBarLink();
                    //--Get Admin-Profile Info
                    GetAdminProfileDetail_Layout();
                }
                else {
                    // Then check for Staff
                    $.get("/Staff/GetStaffCookieDetail", null, function (dataStaffToken) {
                        if (dataStaffToken != "" && dataStaffToken != null) {

                            UserToken_Global_Layout = dataStaffToken;

                            //--Get Staff-Profile Info
                            GetStaffProfileDetail_Layout();
                        }
                        else {
                           
                        }
                    });
                }
            });
        }
    });
});

function GetSuperAdminProfileDetail_Layout() {

    $("#img_SuperAdmin_SuperAdminLayout").attr("src", "");
    $("#a_SuperAdmin_SuperAdminLayout").html("");

    $.ajax({
        type: "GET",
        url: "/GetSuperAdminProfile",
        headers: {
            "Authorization": "Bearer " + UserToken_Global_Layout,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataSuperAdminLayout) {
            var str = dataSuperAdminLayout.data.superadmin;
            if (dataSuperAdminLayout.data.superadmin != null) {


                $("#img_SuperAdmin_SuperAdminLayout").attr("src", "/Content/SuperAdminImages/" + dataSuperAdminLayout.data.superadmin.ProfileImage);
                $("#a_SuperAdmin_SuperAdminLayout").html(dataSuperAdminLayout.data.superadmin.FirstName + " " + dataSuperAdminLayout.data.superadmin.LastName);

            }

            //--Set Active Link in Sidebar
            SetSideBarLink();
        },
        error: function (result) {

            //--Set Active Link in Sidebar
            SetSideBarLink();

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
function GetAdminProfileDetail_Layout() {

    $("#img_Admin_AdminLayout").attr("src", "");
    $("#a_Admin_AdminLayout").html("");

    $.ajax({
        type: "GET",
        url: "/GetAdminProfile",
        headers: {
            "Authorization": "Bearer " + UserToken_Global_Layout,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataAdminLayout) {
            var str = dataAdminLayout.data.Admin;
            if (dataAdminLayout.data.Admin != null) {



                $("#img_Admin_AdminLayout").attr("src", "/Content/AdminImages/" + dataAdminLayout.data.Admin.ProfileImage);
                $("#a_Admin_AdminLayout").html(dataAdminLayout.data.Admin.FirstName + " " + dataAdminLayout.data.Admin.LastName);

            }

            //--Set Active Link in Sidebar
            SetSideBarLink();
        },
        error: function (result) {

            //--Set Active Link in Sidebar
            SetSideBarLink();

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
function GetStaffProfileDetail_Layout() {

    $("#img_Staff_StaffLayout").attr("src", "");
    $("#a_Staff_StaffLayout").html("");
    $("#li_BranchName_StaffLayout").html("");

    $.ajax({
        type: "GET",
        url: "/GetStaffProfile",
        headers: {
            "Authorization": "Bearer " + UserToken_Global_Layout,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (dataStaffLayout) {

            if (dataStaffLayout.data.staff != null) {

                $("#li_BranchName_StaffLayout").html(dataStaffLayout.data.staff.BranchName);

                $("#img_Staff_StaffLayout").attr("src", "/Content/StaffImages/" + dataStaffLayout.data.staff.ProfileImage);
                $("#a_Staff_StaffLayout").html(dataStaffLayout.data.staff.FirstName + " " + dataStaffLayout.data.staff.LastName);

            }

            //--Set Active Link in Sidebar
            SetSideBarLink();
        },
        error: function (result) {

            //--Set Active Link in Sidebar
            SetSideBarLink();

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
function SetSideBarLink() {
    //--Get Selected Sidebar Page Link Value
    //$.get("/Admin/GetSidebarCookieDetail", null, function (dataSidebar) {

    //    //--Manage Dashboard
    //    if (dataSidebar == "manageDashboard") {
    //        $("#a_ManageDashboard_Sidebar").addClass("active");
    //    }
    //    //--Manage Class
    //    else if (dataSidebar == "manageClass") {
    //        $("#a_ManageClass_Sidebar").addClass("active");
    //    }
    //    //--Manage Session
    //    else if (dataSidebar == "manageSession") {
    //        $("#a_ManageSession_Sidebar").addClass("active");
    //    }
    //    //--Manage Session Fee
    //    else if (dataSidebar == "manageSessionFee") {
    //        $("#a_ManageSessionFee_Sidebar").addClass("active");
    //    }
    //    //--Manage Section
    //    else if (dataSidebar == "manageSection") {
    //        $("#a_ManageSection_Sidebar").addClass("active");
    //    }
    //    //--Manage Staff
    //    else if (dataSidebar == "manageStaff") {
    //        $("#a_ManageStaff_Sidebar").addClass("active");
    //    }
    //    //--Manage Student
    //    else if (dataSidebar == "manageStudent") {
    //        $("#a_ManageStudent_Sidebar").addClass("active");
    //    }
    //    //--Manage Pay-Fee
    //    else if (dataSidebar == "managePayFee") {
    //        $("#a_ManagePayFee_Sidebar").addClass("active");
    //    }
    //    //--Manage Pending-Fee
    //    else if (dataSidebar == "pendingFee") {
    //        $("#a_ManageReports_Sidebar").addClass("active");
    //        $("#a_ManagePendingFee_Sidebar").addClass("active");

    //        $("#li_ManageReports_Sidebar").addClass("menu-open");
    //    }
    //    //--Manage Expired-Course-Fee
    //    else if (dataSidebar == "expiredCourseFee") {
    //        $("#a_ManageReports_Sidebar").addClass("active");
    //        $("#a_ManageExpiredCourseFee_Sidebar").addClass("active");

    //        $("#li_ManageReports_Sidebar").addClass("menu-open");
    //    }
    //    //--Manage Completed-Course-Fee
    //    else if (dataSidebar == "completedCourseFee") {
    //        $("#a_ManageReports_Sidebar").addClass("active");
    //        $("#a_ManageCompletedCourseFee_Sidebar").addClass("active");

    //        $("#li_ManageReports_Sidebar").addClass("menu-open");
    //    }
    //    //--Manage Revenue
    //    else if (dataSidebar == "manageRevenue") {
    //        $("#a_ManageReports_Sidebar").addClass("active");
    //        $("#a_ManageRevenue_Sidebar").addClass("active");

    //        $("#li_ManageReports_Sidebar").addClass("menu-open");
    //    }
    //    // Change Password
    //    else if (dataSidebar == "changePassword") {
    //        $("#a_ChangePassword_Sidebar").addClass("active");
    //    }
    //    // Manage Profile
    //    else if (dataSidebar == "manageProfile") {
    //        //$("#a_ManageDiscount_Sidebar").addClass("active");
    //    }
    //});
    $.get("/Admin/GetSidebarCookieDetail", null, function (dataSidebar) {

        //--Manage Dashboard
        if (dataSidebar == "manageDashboard") {
            $("#a_ManageDashboard_SASidebar").addClass("active");
        }
        //--Manage Dashboard
        if (dataSidebar == "manageDashboardAdmin") {
            $("#a_AdminManageDashboard_Sidebar").addClass("active");
        }
        //--Manage School
        else if (dataSidebar == "manageSchool") {
            $("#a_ManageSuperAdmin_MSSidebar").addClass("active");
        }
        // Change Password
        else if (dataSidebar == "changePassword") {
            $("#a_ChangePassword_Sidebar").addClass("active");
        }
        //--Manage Staff
        else if (dataSidebar == "manageStaffAdmin") {
            $("#a_AdminManageStaff_Sidebar").addClass("active");
        }
        //--Manage Class
        else if (dataSidebar == "manageClassAdmin") {
            $("#a_AdminManageClass_Sidebar").addClass("active");
        }
        //--Manage Session
        else if (dataSidebar == "manageSessionAdmin") {
            $("#a_AdminManageSession_Sidebar").addClass("active");
        }
        //--Manage Session Fee
        else if (dataSidebar == "manageSessionFeeAdmin") {
            $("#a_AdminManageSessionFee_Sidebar").addClass("active");
        }
        //--Manage Section
        else if (dataSidebar == "manageSectionAdmin") {
            $("#a_AdminManageSection_Sidebar").addClass("active");
        }
    
        //--Manage Student
        else if (dataSidebar == "manageStudentAdmin") {
            $("#a_AdminManageStudent_Sidebar").addClass("active");
        }
        //--Manage Pay-Fee
        else if (dataSidebar == "manageDeviceConfigurationAdmin") {
            $("#a_ManageDeviceConfiguration_Sidebar").addClass("active");
        }
        //--Manage Pending-Fee
        else if (dataSidebar == "pendingFee") {
            $("#a_ManageReports_Sidebar").addClass("active");
            $("#a_ManagePendingFee_Sidebar").addClass("active");

            $("#li_ManageReports_Sidebar").addClass("menu-open");
        }
        //--Manage Expired-Course-Fee
        else if (dataSidebar == "expiredCourseFee") {
            $("#a_ManageReports_Sidebar").addClass("active");
            $("#a_ManageExpiredCourseFee_Sidebar").addClass("active");

            $("#li_ManageReports_Sidebar").addClass("menu-open");
        }
        //--Manage Completed-Course-Fee
        else if (dataSidebar == "completedCourseFee") {
            $("#a_ManageReports_Sidebar").addClass("active");
            $("#a_ManageCompletedCourseFee_Sidebar").addClass("active");

            $("#li_ManageReports_Sidebar").addClass("menu-open");
        }
        //--Manage Revenue
        else if (dataSidebar == "manageRevenue") {
            $("#a_ManageReports_Sidebar").addClass("active");
            $("#a_ManageRevenue_Sidebar").addClass("active");

            $("#li_ManageReports_Sidebar").addClass("menu-open");
        }
   
        // Manage Profile
        else if (dataSidebar == "manageProfile") {
            //$("#a_ManageDiscount_Sidebar").addClass("active");
        }
    });
}
function LogoutUser() {
    StartLoading();
    $.get("/Login/LogoutUser", null, function () {
        window.location = "/Login/Index";
    });
}