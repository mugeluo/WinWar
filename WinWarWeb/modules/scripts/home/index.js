define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        pageIndex:1,
        parentTypeID: 16,
        lastCode:0
    };
    var NavsCache = [];
    var ObjectJS = {};

    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        ObjectJS.getNewsTypeByParentID();

        ObjectJS.getNews();
    };

    ObjectJS.bindEvent = function () {
        $(window).bind("scroll", function () {
            var bottom = $(document).height() - document.documentElement.scrollTop - document.body.scrollTop - $(window).height();
            if (bottom <= 50) {
                setTimeout(function () {
                    Paras.pageIndex++;
                    ObjectJS.getNews();
                }, 500);

            }
        });

        $(".menu-list .item").click(function () {
            var _this=$(this);
            if (!_this.hasClass("active")) {
                var $imgs = _this.siblings().removeClass("active").find("img");
                $imgs.each(function () {
                    $(this).attr("src", "/modules/images/" + $(this).data("icon") + ".png");
                });
                
                var $img = _this.addClass("active").find("img");
                $img.each(function () {
                    $(this).attr("src", "/modules/images/" + $(this).data("icon") + "_color.png");
                });

                Paras.parentTypeID = _this.data("id");
                ObjectJS.getNewsTypeByParentID();
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

    ObjectJS.bindNavClick = function () {
        $(".nav li .nav-item").click(function () {
            if (!$(this).hasClass("active")) {
                $(this).parent().siblings().find(".nav-item").removeClass("active").next().removeClass("select");
                $(this).addClass("active").next().addClass("select");


            }
        });
    }

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

    ObjectJS.getNewsTypeByParentID = function () {
        var data = NavsCache[Paras.typeParentID];
        if (data == null) {
            Global.post("/Home/GetNewsTypeByParentID", { id: Paras.parentTypeID }, function (data) {
                NavsCache[Paras.typeParentID] = data;
                $(".nav-list").html('');

                for (var i = 0, len = data.items.length; i < len; i++) {
                    var item = data.items[i];
                    var html = '';
                    if (i == 0) {
                        html += '<li><div class="nav-item active">' + item.News_Type_Name2 + '</div>';
                        html += '<span class="select inline-block"></span>';
                    }
                    else {
                        html += '<li><div class="nav-item">' + item.News_Type_Name2 + '</div>';
                        html += '<span class="inline-block"></span>';
                    }
                    html += '</li>';
                    $(".nav-list").append(html);
                }

                ObjectJS.bindNavClick();
            });
        }
        else {
            $(".nav-list").html('');

            for (var i = 0, len = data.items.length; i < len; i++) {
                var item = data.items[i];
                var html = '';
                if (i == 0) {
                    html += '<li><div class="nav-item active">' + item.News_Type_Name2 + '</div>';
                    html += '<span class="select inline-block"></span>';
                }
                else {
                    html += '<li><div class="nav-item">' + item.News_Type_Name2 + '</div>';
                    html += '<span class="inline-block"></span>';
                }
                html += '</li>';
                $(".nav-list").append(html);
            }

            ObjectJS.bindNavClick();
        }
    };

    ObjectJS.getNews = function () {
        $(".data-loading").show();
        Global.post("/Home/GetNews", Paras, function (data) {
            $(".data-loading").hide();

            var items = data.items;
            Paras.lastCode = data.lastCode;

            DoT.exec("template/home/news-list.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);

                $(".content ul").append(innerhtml);
                innerhtml.fadeIn(400);

            });
        });
    }

    

    module.exports = ObjectJS;
});