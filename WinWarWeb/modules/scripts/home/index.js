define(function (require, exports, module) {
    var ObjectJS={};

    ObjectJS.init = function () {
        ObjectJS.bindEvent();
    };

    ObjectJS.bindEvent = function () {
        $(".nav li .nav-item").click(function () {
            if (!$(this).hasClass("active")) {
                $(this).parent().siblings().find(".nav-item").removeClass("active");
                $(this).addClass("active");
            }
        });

        $(".menu li").click(function () {
            if (!$(this).hasClass("active")) {
                $(this).siblings().removeClass("active").find(".iconfont").css("color", "#666");
                $(this).addClass("active").find(".iconfont").css("color", "#4A98E7");
            }
        });
    };

    module.exports= ObjectJS;
});