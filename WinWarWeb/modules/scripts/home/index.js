define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        keywords:'',
        pageIndex: 1,
        pageSize:15,
        parentTypeID: 16,
        typeID: 0,
        lastNewsCode:0
    };
    var NavsCache = [];//新闻导航缓存
    var NewsCache = [];//新闻列表缓存
    var NoNewsDate = false;
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
            if (!NoNewsDate) {
                var bottom = $(document).height() - document.documentElement.scrollTop - document.body.scrollTop - $(window).height();
                if (bottom <= 20) {
                    setTimeout(function () {
                        Paras.pageIndex++;
                        ObjectJS.getNews();
                    }, 500);

                }
            }

        });

        $(".overlay").click(function (e) {
            if (!$(e.target).parents().hasClass("overlay-search") && !$(e.target).hasClass("overlay-search")) {
                $(".overlay").hide();
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

        //弹出 输入关键字层
        $(".search").click(function () {
            $('.overlay').show();
            $("#keywords").focus();

        });

        //关键字查询
        $("#btn-search").click(function () {
            $('.overlay').hide();

            Paras.keywords = $("#keywords").val();
            if (Paras.keywords != '') {
                $("#keywords").val('');
                $(".overlay-keywords").show();
                $("#keywords-show").val(Paras.keywords);

                NoNewsDate = false;
                $(".load-more").hide();
                Paras.pageIndex = 1;
                Paras.lastNewsCode = 0;
                ObjectJS.getNews();
            }
        });
        //取消关键字查询
        $("#btn-cancel").click(function () {
            $('.overlay-keywords').hide();
            $("#keywords-show").val('');

            NoNewsDate = false;
            $(".load-more").hide();
            Paras.keywords = '';
            Paras.pageIndex = 1;
            Paras.lastNewsCode = 0;
            ObjectJS.getNews();
        });
        //ObjectJS.bindNav();

        //初始化
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
                html += '<li ><div class="nav-item active" data-id="' + item.News_Type_2 + '">' + item.News_Type_Name2 + '</div>';
                html += '<span class="select inline-block"></span>';
                Paras.typeID = item.News_Type_2;
            }
            else {
                html += '<li ><div class="nav-item" data-id="' + item.News_Type_2 + '">' + item.News_Type_Name2 + '</div>';
                html += '<span class="inline-block"></span>';
            }
            html += '</li>';
            $(".nav-list").append(html);
        }

        NoNewsDate = false;
        $(".load-more").hide();
        Paras.pageIndex == 1;
        Paras.lastNewsCode = 0;
        ObjectJS.bindNavClick();
        ObjectJS.getNews();
    }

    ObjectJS.bindNavClick = function () {
        $(".nav-list li .nav-item").click(function () {
            var _this = $(this);
            if (!_this.hasClass("active")) {
                _this.parent().siblings().find(".nav-item").removeClass("active").next().removeClass("select");
                _this.addClass("active").next().addClass("select");

                NoNewsDate = false;
                $(".load-more").hide();
                Paras.typeID = _this.data("id");
                Paras.pageIndex =1;
                Paras.lastNewsCode = 0;
                ObjectJS.getNews();
            }
        });
    }

    ObjectJS.getNews = function () {
        $(".data-loading").show();

        var data = null;
        if (Paras.pageIndex == 1) {
            data = NewsCache[Paras.parentTypeID + Paras.typeID+Paras.keywords];
        }

        if (data == null)
        {
            Global.post("/Home/GetNews", Paras, function (data) {
                if (Paras.pageIndex == 1) {
                    NewsCache[Paras.parentTypeID + Paras.typeID + Paras.keywords] = data;
                }

                if (Paras.pageSize > data.items.length) {
                    NoNewsDate = true;
                }
                ObjectJS.bindNews(data);
            });
        }
        else
        {
            ObjectJS.bindNews(data);
        }
    }

    ObjectJS.bindNews = function (data) {
        if (Paras.pageIndex == 1) {
            $(".content ul").html('');
        }

        var items = data.items;
        Paras.lastNewsCode = data.lastNewsCode;
        $(".data-loading").hide();
   
        if (items.length > 0) {
            DoT.exec("template/home/news-list.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                $(".content ul").append(innerhtml);
                innerhtml.fadeIn(400);

                if (NoNewsDate) {
                    $(".load-more").show();
                }
            });
        }
        else {
            if (Paras.pageIndex == 1) {
                $(".content ul").html('<li class="no-data">暂无新闻</li>');
            }
        }
    }

    module.exports = ObjectJS;
});