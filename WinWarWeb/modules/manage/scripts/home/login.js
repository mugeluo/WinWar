

define(function (require, exports, module) {

    require("jquery");
    var Global = require("global")

    var ObjectJS = {};
    //登陆初始化
    ObjectJS.init = function (status) {
        ObjectJS.bindEvent();
    }
    //绑定事件
    ObjectJS.bindEvent = function () {

        $("#iptPwd").on("keypress", function (e) {
            if (e.keyCode == 13) {
                $("#btnLogin").click();
                $("#iptPwd").blur();
            }
        });

        //登录
        $("#btnLogin").click(function () {
            if (!$("#iptUserName").val()) {
                $(".registerErr").html("请输入账号").slideDown();
                return;
            }
            if (!$("#iptPwd").val()) {
                $(".registerErr").html("请输入密码").slideDown();
                return;
            }

            $(this).html("登录中...").attr("disabled", "disabled");

            Global.post("/Manage/Home/UserLogin", {
                userName: $("#iptUserName").val(),
                pwd: $("#iptPwd").val()
            },
            function (data)
            {

                $("#btnLogin").html("登录").removeAttr("disabled");

                if (data.result == 1)
                {
                    location.href = "Index";
                }
                else if (data.result == 0)
                {
                   $(".registerErr").html("账号或密码有误").slideDown();
                }
                else if (data.result == 9) {
                    $(".registerErr").html("您的账户已注销,请切换其他账户登录").show();
                }
            });
        });
    }

    module.exports = ObjectJS;
});