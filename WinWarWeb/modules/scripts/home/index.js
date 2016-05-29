define(function (require, exports, module) {
    var ObjectJS = {};

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

        ObjectJS.bindNav();
    };

    ObjectJS.bindNav = function () {
        n = $('.nav-list li').size();
        var wh = 100 * n + "%";
        $('.nav-list').width(wh);
        var lt = (100 / n / 3);
        var lt_li = lt + "%";
        $('.nav-list li').width(lt_li);
        var y = 0;
        var w = n / 3;
        $(".nav-list").swipe({
            swipeLeft: function () {
                if (y == -lt * w) {
                    //alert('已经到最后页');
                } else {
                    y = y - lt;
                    var t = y + "%";
                    $(this).css({ '-webkit-transform': "translate(" + t + ")", '-webkit-transition': '500ms linear' });
                }
            },
            swipeRight: function () {
                if (y != 0) {
                    y = y + lt;
                    var t = y + "%";
                    $(this).css({ '-webkit-transform': "translate(" + t + ")", '-webkit-transition': '500ms linear' });
                }

            }
        });
    }
    module.exports = ObjectJS;
});