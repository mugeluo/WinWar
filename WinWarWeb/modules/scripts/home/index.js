define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        keywords:'',
        pageIndex:1,
        parentTypeID: 16,
        typeID: 0,
        lastNewsCode:0
    };
    var NavsCache = [];//新闻导航缓存
    var NewsCache = [];//新闻列表缓存
    var ObjectJS = {};

    ObjectJS.init = function (parentTypeID) {
        Paras.parentTypeID = parentTypeID;

        ObjectJS.bindEvent();

        if (Paras.parentTypeID == 16) {
            ObjectJS.getNewsTypeByParentID();
        }
    };

    ObjectJS.bindEvent = function () {
        //滚动加载 新闻列表
        $(window).bind("scroll", function () {
            var bottom = $(document).height() - document.documentElement.scrollTop - document.body.scrollTop - $(window).height();
            if (bottom <= 50) {
                setTimeout(function () {
                    Paras.pageIndex++;
                    ObjectJS.getNews();
                }, 500);

            }
        });

        //菜单切换
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

        if (Paras.parentTypeID != 16) {
            $(".menu-list .item[data-id='" + Paras.parentTypeID + "']").click();
        }
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

    ObjectJS.getNewsTypeByParentID = function () {
        var data = NavsCache[Paras.parentTypeID];
        if (data == null) {
            Global.post("/Home/GetNewsTypeByParentID", { id: Paras.parentTypeID }, function (data) {
                NavsCache[Paras.parentTypeID] = data;

                ObjectJS.bindNewsTypeByParentID(data);
            });
        }
        else {
            ObjectJS.bindNewsTypeByParentID(data);
        }
    };

    ObjectJS.bindNewsTypeByParentID = function (data) {
        $(".nav-list").html('');

        for (var i = 0, len = data.items.length; i < len; i++) {
            var item = data.items[i];
            var html = '';
            if (i == 0) {
                html += '<li><div class="nav-item active" data-id="' + item.News_Type_2 + '">' + item.News_Type_Name2 + '</div>';
                html += '<span class="select inline-block"></span>';
                Paras.typeID = item.News_Type_2;
            }
            else {
                html += '<li><div class="nav-item" data-id="' + item.News_Type_2 + '">' + item.News_Type_Name2 + '</div>';
                html += '<span class="inline-block"></span>';
            }
            html += '</li>';
            $(".nav-list").append(html);
        }

        Paras.pageIndex == 1;
        Paras.lastNewsCode = 0;
        ObjectJS.bindNavClick();
        ObjectJS.getNews();
    }

    ObjectJS.bindNavClick = function () {
        $(".nav li .nav-item").click(function () {
            var _this = $(this);
            if (!_this.hasClass("active")) {
                _this.parent().siblings().find(".nav-item").removeClass("active").next().removeClass("select");
                _this.addClass("active").next().addClass("select");

                Paras.typeID = _this.data("id");
                Paras.pageIndex == 1;
                Paras.lastNewsCode = 0;
                ObjectJS.getNews();
            }
        });
    }


    ObjectJS.getNews = function () {
        $(".data-loading").show();

        var data=NewsCache[Paras.parentTypeID+Paras.typeID];
        if(data==null){
            Global.post("/Home/GetNews", Paras, function (data) {
                NewsCache[Paras.parentTypeID + Paras.typeID] = data;
                ObjectJS.bindNews(data);
            });
        }
        else
        {
            ObjectJS.bindNews(data);
        }
    }

    ObjectJS.bindNews = function (data) {
        $(".data-loading").hide();
        if (Paras.pageIndex == 1) {
            $(".content ul").html('');
        }
        var items = data.items;
        Paras.lastNewsCode = data.lastNewsCode;

        DoT.exec("template/home/news-list.html", function (template) {
            var innerhtml = template(items);
            innerhtml = $(innerhtml);

            $(".content ul").append(innerhtml);
            innerhtml.fadeIn(400);

        });
    }

    module.exports = ObjectJS;
});