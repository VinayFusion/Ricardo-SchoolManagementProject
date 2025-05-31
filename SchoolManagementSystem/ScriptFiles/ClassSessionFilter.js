// use in payfee,section,session,sessionFee,setudent
function GetMultipleDataForddl(FieldType, ddlId, functionNameForCall = "") {
    // ------ field Type Means Like Class,Section,Gender,Session,Blood and pls note its is case senstive

    var val = encodeURIComponent(FieldType);
    $.ajax({
        type: "GET",
        url: "/GetMultipleDdlDataBinding?nameOfDataBind=" + val,
        headers: {
            "Authorization": "Bearer " + UserToken_Global,
            "Content-Type": "application/json"
        },
        contentType: 'application/json',
        success: function (ddlDataVal) {
            var DDlId = ddlId.split(',');
            var DataType = FieldType.split(',');
            for (var a = 0; a < DDlId.length; a++) {
                $("#" + DDlId[a] + "").html('');
                var name = DataType[a];
                var ddl_data;
                if (name != 'Section' && ddlDataVal.data.DdlValue.length != 0) {
                    if (name.indexOf("In") != -1) {
                        ddl_data = '<option value="0">Select ' + name.substring(0, name.indexOf("In")) + ' </option>';
                    }
                    else {
                        ddl_data = '<option value="0">Select ' + name + '</option>';
                    }
                }
                for (var i = 0; i < ddlDataVal.data.DdlValue.length; i++) {
                    if (ddlDataVal.data.DdlValue[i].FieldType == name) {
                        if (name.indexOf("SessionIn") != -1) {
                            ddl_data += '<option value="' + ddlDataVal.data.DdlValue[i].FieldValue + '">' + ddlDataVal.data.DdlValue[i].FieldValue + '</option>';
                        }
                        else {
                            ddl_data += '<option value="' + ddlDataVal.data.DdlValue[i].FieldId + '">' + ddlDataVal.data.DdlValue[i].FieldValue + '</option>';
                        }
                    }
                }
                $("#" + DDlId[a] + "").append(ddl_data);
                if ((functionNameForCall != null && functionNameForCall != "") && name.indexOf("SessionIn") != -1) {

                    functionNameForCall();

                }

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

function GetDdlSessionDataForFilter(currentSession, SessionddlName = "ddlSession_Filter") {
    var _is_valid = true;
    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetDdlSessionDataForFilter",
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (ddlDataVal) {
                var ddl_data = "";
                $("#" + SessionddlName +"").html('');
                ddl_data = '<option value="0">Select Session</option>';

                for (var i = 0; i < ddlDataVal.data.ddlDataList.length; i++) {
                    ddl_data += '<option value="' + ddlDataVal.data.ddlDataList[i].SessionName + '">' + ddlDataVal.data.ddlDataList[i].SessionName + '</option>';
                }
                $("#" + SessionddlName + "").append(ddl_data);
                if (currentSession != "" && currentSession != null) {
                    $("#" + SessionddlName + "").val(currentSession).trigger('change');

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
}

function GetDdlClassAllData(ddlIdName) {
    var _is_valid = true;
    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetDdlClassAllData",
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (ddlDataVal) {
                var ddl_data = "";
                $("#" + ddlIdName +"").html('');
                ddl_data = '<option value="0">Select Class </option>';

                for (var i = 0; i < ddlDataVal.data.ddlDataList.length; i++) {
                    ddl_data += '<option value="' + ddlDataVal.data.ddlDataList[i].ClassId + '">' + ddlDataVal.data.ddlDataList[i].ClassName + '</option>';
                }
                $("#" + ddlIdName + "").append(ddl_data);
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
}

function GetDdlClassDataForFilter(functionName = "", SessionddlName = "ddlSession_Filter", ClassddlName = "ddlSelectClass_Filter") {
    var _is_valid = false;
    var _sessionName = $("#" + SessionddlName + "").val();
    if (_sessionName != null && _sessionName != "" && _sessionName != "0") {
        _is_valid = true;
    }
    if (_is_valid == true) {
        $.ajax({
            type: "GET",
            url: "/GetDdlClassDataForFilter?sessionName=" + _sessionName,
            headers: {
                "Authorization": "Bearer " + UserToken_Global,
                "Content-Type": "application/json"
            },
            contentType: 'application/json',
            success: function (ddlDataVal) {
                var ddl_data = "";
                $("#"+ClassddlName+"").html('');
                ddl_data = '<option value="0">Select class of selected Session </option>';

                for (var i = 0; i < ddlDataVal.data.ddlDataList.length; i++) {

                    ddl_data += '<option value="' + ddlDataVal.data.ddlDataList[i].ClassId + '">' + ddlDataVal.data.ddlDataList[i].ClassName + '</option>';

                }
                $("#" + ClassddlName + "").append(ddl_data);
                if (functionName != "" && ClassddlName == "ddlSelectClass_Filter") {
                    functionName();
                }
            },
            error: function (result) {
                if (functionName != "") {
                    functionName();
                }

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
    else {
        if (functionName != "" && ClassddlName == "ddlSelectClass_Filter") {
        functionName();
        }
    }
}





