define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        code:16
    };
    var ObjectJS = {};


    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        ObjectJS.getNewsTypeByParentCode();

        ObjectJS.getNews();
    };

    ObjectJS.bindEvent = function () {
        $(".nav li .nav-item").click(function () {
            if (!$(this).hasClass("active")) {
                $(this).parent().siblings().find(".nav-item").removeClass("active").next().removeClass("inline-block");
                $(this).addClass("active").next().addClass("inline-block");
            }
        });

        $(".menu-list .item").click(function () {
            var _this=$(this);
            if (!_this.hasClass("active")) {
                _this.siblings().removeClass("active").find(".iconfont").css("color", "#666");
                _this.addClass("active").find(".iconfont").css("color", "#4A98E7");

                Paras.code = _this.data("id");
                ObjectJS.getNewsTypeByParentCode();
            }
        });

        $(".search").click(function () {
            $('.overlay').show();
//            $('.overlay').css("height", document.body.scrollTop
//+ document.body.clientHeight + "px").show();

        });

        $(".overlay-cancel").click(function () {
            $('.overlay').hide();
        });

        //ObjectJS.bindNav();
    };

    ObjectJS.bindNav = function () {
        var n = $('.nav-list li').size();
        var wh = 100 * n + "%";
        $('.nav-list').width(wh);
        var lt = (100 / n / 3);
        var lt_li = lt + "%";
        $('.nav-list li').width(lt_li);
        var y = 0;
        var w = n / 2;

        $(".nav-list").swipe({
            swipeLeft: function () {
                alert(111);
                if (y == -lt * w) {
                    alert('已经到最后页');
                } else {
                    y = y - lt;
                    var t = y + "%";
                    $(this).css({ '-webkit-transform': "translate(" + t + ")", '-webkit-transition': '500ms linear' });
                }
            },
            swipeRight: function () {
                if (y == 0) {
                    alert('已经到第一页')
                } else {
                    y = y + lt;
                    var t = y + "%";
                    $(this).css({ '-webkit-transform': "translate(" + t + ")", '-webkit-transition': '500ms linear' });
                }

            }
        });
    }

    ObjectJS.getNews = function () {
        Global.post("/Home/GetNews", {paras:JSON.stringify(Paras)}, function (data) {
            var items = data.items;
            DoT.exec("template/home/news-list.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);

                $(".content ul").append(innerhtml);
                innerhtml.fadeIn(400);

            });
        });
    }

    ObjectJS.getNewsTypeByParentCode = function () {
        Global.post("/Home/GetNewsTypeByParentCode", { code: Paras.code }, function (data) {
            $(".nav-list").html('');

            for (var i = 0,len = data.items.length; i < len; i++) {
                var item = data.items[i];
                var html = '';
                html += '<li><div class="nav-item active">' + item.News_Type_Name2 + '</div>';
                if (i ==0) {
                    html += '<span class="select inline-block"></span>';
                }
                else {
                    html += '<span class="inline-block"></span>';
                }
                html += '</li>';
                $(".nav-list").append(html);
            }
        });
    };

    module.exports = ObjectJS;
});