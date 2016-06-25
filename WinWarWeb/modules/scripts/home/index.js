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
    var NoNewsData = false;
    var ObjectJS = {};

    ObjectJS.init = function (parentTypeID, userID) {
        ObjectJS.userID = userID;
        Paras.parentTypeID = parentTypeID;

        ObjectJS.bindEvent();

        if (Paras.parentTypeID == 16) {
            ObjectJS.getNewsTypeByParentID();
        }
    };

    ObjectJS.bindEvent = function () {
        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("passport-icon") && !$(e.target).parents().hasClass("passport-box") && !$(e.target).hasClass("passport-box")) {
                $(".passport-box").fadeOut();
            }
        });

        //滚动加载 新闻列表
        $(window).bind("scroll", function () {
            if (!NoNewsData) {
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
                var $imgs = $(".menu-list .item").removeClass("active").find("img");
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

                NoNewsData = false;
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

            NoNewsData = false;
            $(".load-more").hide();
            Paras.keywords = '';
            Paras.pageIndex = 1;
            Paras.lastNewsCode = 0;
            ObjectJS.getNews();
        });

        //数据初始化
        if (Paras.parentTypeID != 16) {
            $(".menu-list .item[data-id='" + Paras.parentTypeID + "']").click();
        }

        $(".passport-icon").click(function () {
            if (ObjectJS.userID != '') {
                $(".passport-box").fadeIn();
            }
            else {
                location.href = "/user/login";
            }
        });
    };

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
        $(".swiper-wrapper").html('');

        for (var i = 0, len = data.items.length; i < len; i++) {
            var item = data.items[i];
            var html = '';
            if (i == 0) {
                html += '<div class="swiper-slide select" data-id="' + item.News_Type_2 + '"><div class="name">' + item.News_Type_Name2 + '</div><div class="circle"></div></div>';
                Paras.typeID = item.News_Type_2;
            }
            else {
                html += '<div class="swiper-slide" data-id="' + item.News_Type_2 + '"><div class="name">' + item.News_Type_Name2 + '</div><div class="circle"></div></div>';
            }
            $(".swiper-wrapper").append(html);
        }

        NoNewsData = false;
        $(".load-more").hide();
        Paras.pageIndex == 1;
        Paras.lastNewsCode = 0;

        ObjectJS.bindNewsNavClick();
        ObjectJS.bindNewsNavSlide();
        ObjectJS.getNews();
    }

    ObjectJS.bindNewsNavClick = function () {
        $(".swiper-wrapper .swiper-slide").unbind().click(function () {
            var _this = $(this);
            if (!_this.hasClass("select")) {
                _this.addClass("select").siblings().removeClass("select");

                NoNewsData = false;
                $(".load-more").hide();
                Paras.typeID = _this.data("id");
                Paras.pageIndex =1;
                Paras.lastNewsCode = 0;
                ObjectJS.getNews();
            }
        });
    }

    ObjectJS.bindNewsNavSlide = function () {
        var swiper = new Swiper('.swiper-container', {
            slidesPerView: 3,
            spaceBetween: 30
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
                    NoNewsData = true;
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

                if (NoNewsData) {
                    $(".load-more").show();
                }
            });
        }
        else {
            if (Paras.pageIndex == 1) {
                $(".content ul").html('<li class="no-data">暂无新闻</li>');
            }
            else {
                if (NoNewsData) {
                    $(".load-more").show();
                }
            }
        }

    }

    module.exports = ObjectJS;
});