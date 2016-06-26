

define(function (require, exports, module) {

    require("jquery");
    require("pager");
    var Global = require("global"),
        doT = require("dot");

    var Admin = {};
   
    //模块产品详情初始化
    Admin.detailInit = function ()
    {
        Admin.detailEvent();
    }
    //绑定事件
    Admin.detailEvent = function () {
        $("#OldPwd").blur(function () {
            if ($("#OldPwd").val() == '') {
                $("#OldPwdError").html('原密码不能为空');
            } else {
                $("#OldPwdError").html('');
            }
        });

        $("#NewPwd").blur(function (){
            if ($("#NewPwd").val() == '') {
                $("#NewPwdError").html('新密码不能为空');
                return false;
            }else {
                $("#NewPwdError").html('');
            }
        });

        $("#NewConfirmPwd").blur(function () {
            if ($("#NewConfirmPwd").val() == '') {
                $("#NewConfirmPwdError").html('确认密码不能为空');
                return false;
            }
            else {
                if ($("#NewConfirmPwd").val() != $("#NewPwd").val()) {
                    $("#NewConfirmPwdError").html('确认密码有误');
                    return false;
                }else {
                    $("#NewConfirmPwdError").html('');
                }
            }
        });

        //保存
        $("#saveAdmin").click(function () {
            Admin.validateData()
        });
    };

    Admin.validateData = function ()
    {
        if ($("#OldPwd").val() == '') {
            $("#OldPwdError").html('原密码不能为空');
            return false;
        } else {
            Global.post("/Manage/Organization/ConfirmLoginPwd", { loginPwd: $("#OldPwd").val() }, function (data) {
                if (data.Result == 1) {
                    $("#OldPwdError").html('');

                    if ($("#NewPwd").val() == '') {
                        $("#NewPwdError").html('新密码不能为空');
                        return false;
                    }
                    else {
                        $("#NewPwdError").html('');
                    }

                    if ($("#NewConfirmPwd").val() == '') {
                        $("#NewConfirmPwdError").html('确认密码不能为空');
                        return false;
                    } else {
                        if ($("#NewConfirmPwd").val() != $("#NewPwd").val()) {
                            $("#NewConfirmPwdError").html('确认密码有误');
                            return false;
                        } else {
                            $("#NewConfirmPwdError").html('');
                        }
                    }

                    Admin.saveAdmin();
                }
                else {
                    $("#OldPwdError").html('原密码有误');
                    return false;
                }
            });
        }
    };

    Admin.saveAdmin = function () {
        Global.post("/Manage/Organization/UpdateUserPass", {
            loginPwd: $("#NewPwd").val()
        }, function (data) {
            if (data.Result == "1") {
                alert("保存成功");
                location.href = location.href;
            }
        });
    };

    module.exports = Admin;
});