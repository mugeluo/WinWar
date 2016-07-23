define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");
    var NewsDetail = require("scripts/home/newsdetail");

    var Paras = {
        keywords:'',
        parentTypeID: 6,
        typeID: 0,
        lastNewsCode: 0,
        pageIndex: 1,
        pageSize: 15
    };
    var NavsCache = [];//新闻导航缓存
    var NewsCache = [];//新闻列表缓存
    var ReadNewsCache = [];//已读新闻列表缓存
    var NoNewsData = false;//没有新闻数据
    var ObjectJS = {};
    ObjectJS.init = function (parentTypeID, userID) {
        ReadNewsCache = window.localStorage.getItem("ReadNewsCache");
        if (ReadNewsCache != null && ReadNewsCache != '') {
            ReadNewsCache = ReadNewsCache.split('|');
        }else {
            ReadNewsCache = [];
        }

        ObjectJS.userID = userID;
        Paras.parentTypeID = parentTypeID;

        ObjectJS.bindEvent();

        if (Paras.parentTypeID == 6) {
            ObjectJS.getNewsTypeByParentID();
        }

        NewsDetail.bindEvent();
    };

    ObjectJS.bindEvent = function () {
        //滚动加载新闻列表
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

        //弹出个人遮罩层
        $(".passport-icon").click(function () {
            if (ObjectJS.userID != 0) {
                $("#passport-box").fadeIn();
                $(".passport-box").animate({width:"250px"},200);
            }
            else {
                location.href = "/user/login?returnUrl=" + location.href;
            }
        });

        //个人遮罩层点击
        $(".overlay-passport-content").click(function (e) {
            if (!$(e.target).parents().hasClass("passport-box") && !$(e.target).hasClass("passport-box")) {
                $(".passport-box").animate({ width: "0px" }, 200, function () {
                    $(".overlay-passport-content").hide();
                });
                
            }
        });

        //弹出关键字遮罩层
        $(".search").click(function () {
            $('.overlay-search-keywords').show();
            $("#keywords").focus();

        });

        //关键字遮罩层点击
        $(".overlay-search-keywords").click(function (e) {
            if (!$(e.target).parents().hasClass("overlay-search") && !$(e.target).hasClass("overlay-search")) {
                $(".overlay-search-keywords").hide();
            }
        });

        //关键字查询
        $("#btn-search").click(function () {
            $('.overlay-search-keywords').hide();

            Paras.keywords = $("#keywords").val();
            if (Paras.keywords != '') {
                $("#keywords").val('');
                $(".overlay-keywords").show();
                $("#keywords-show").val(Paras.keywords);

                $(".content .no-more").hide();
                NoNewsData = false;
                Paras.pageIndex = 1;
                Paras.lastNewsCode = 0;
                ObjectJS.getNews();
            }
        });

        //取消关键字查询
        $("#btn-cancel").click(function () {
            $('.overlay-keywords').hide();
            $("#keywords-show").val('');

            $(".content .no-more").hide();
            NoNewsData = false;
            Paras.keywords = '';
            Paras.pageIndex = 1;
            Paras.lastNewsCode = 0;
            ObjectJS.getNews();
        });

        //一级菜单切换
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

        
        //一级菜单初始化
        if (Paras.parentTypeID != 6) {
            $(".menu-list .item[data-id='" + Paras.parentTypeID + "']").click();
        }
    };

    //获取二级菜单列表
    ObjectJS.getNewsTypeByParentID = function () {
        var data = NavsCache[Paras.parentTypeID];
        if (data == null) {
            Global.post("/Home/GetNewsTypeByParentID", { id: Paras.parentTypeID }, function (data) {
                NavsCache[Paras.parentTypeID] = data;

                ObjectJS.bindNewsTypeByParentID(data);
            });
        }else {
            ObjectJS.bindNewsTypeByParentID(data);
        }
    };

    //绑定二级菜单
    ObjectJS.bindNewsTypeByParentID = function (data) {
        $(".nav .swiper-wrapper").html('');

        for (var i = 0, len = data.items.length; i < len; i++) {
            var item = data.items[i];
            var html = '';
            if (i == 0) {
                html += '<div class="swiper-slide select" data-id="' + item.News_Type_2 + '"><div class="name">' + item.News_Type_Name2 + '</div><div class="circle"></div></div>';
                Paras.typeID = item.News_Type_2;
            }else {
                html += '<div class="swiper-slide" data-id="' + item.News_Type_2 + '"><div class="name">' + item.News_Type_Name2 + '</div><div class="circle"></div></div>';
            }
            $(".nav .swiper-wrapper").css({ "-webkit-transform": "translate3d(0px, 0px, 0px)", "transform": "translate3d(0px, 0px, 0px)" }).append(html);
        }

        $(".content .no-more").hide();
        NoNewsData = false;
        Paras.pageIndex = 1;
        Paras.lastNewsCode = 0;

        ObjectJS.bindNewsNavClick();
        ObjectJS.bindNewsNavSlide();
        ObjectJS.getNews();
    }
    //二级菜单点击
    ObjectJS.bindNewsNavClick = function () {
        $(".nav .swiper-wrapper .swiper-slide").unbind().click(function () {
            var _this = $(this);
            if (!_this.hasClass("select")) {
                _this.addClass("select").siblings().removeClass("select");

                NoNewsData = false;
                $(".content .no-more").hide();
                Paras.typeID = _this.data("id");
                Paras.pageIndex =1;
                Paras.lastNewsCode = 0;
                ObjectJS.getNews();
            }
        });
    }
    //二级菜单左右滑动
    ObjectJS.bindNewsNavSlide = function () {
        var swiper = new Swiper('.nav .swiper-container', {
            slidesPerView: 3,
            spaceBetween: 30
        });

        var $items = $(".nav .swiper-wrapper .swiper-slide");
        var width = $items.eq(0).width();
        $items.css({
            "margin-right":"0","width":(width+20)+"px"
        });
        $items.eq(0).css("width", (width + 50) + "px");
        $items.eq(1).css("width", (width - 40) + "px");
        $items.eq(2).css("width", (width + 50) + "px");
    }

    //获取新闻列表
    ObjectJS.getNews = function () {
        $(".content .data-loading").show();

        var data = null;
        if (Paras.pageIndex == 1) {
            data = NewsCache[Paras.parentTypeID +"_"+ Paras.typeID+"_" + Paras.keywords];
            $(".news-list .content ul").html('');
        }

        if (data == null)
        {
            Global.post("/Home/GetNews", Paras, function (data) {
                
                if (Paras.pageIndex == 1) {
                    NewsCache[Paras.parentTypeID+"_" + Paras.typeID +"_"+ Paras.keywords] = data;
                }

                if (Paras.pageSize > data.items.length) {
                    NoNewsData = true;
                }
                ObjectJS.bindNews(data);
            });
        }else{
            ObjectJS.bindNews(data);
        }
    }

    //绑定新闻列表
    ObjectJS.bindNews = function (data) {
        if (Paras.pageIndex == 1) {
            $(".news-list .content ul").html('');
        }

        var items = data.items;
        Paras.lastNewsCode = data.lastNewsCode;
        $(".content .data-loading").hide();
   
        if (items.length > 0) {
            DoT.exec("template/home/news-list.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                $(".news-list .content ul").append(innerhtml);
                innerhtml.fadeIn(400);

                if (NoNewsData) {
                    $(".content .no-more").show();
                }

                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    if (ReadNewsCache.indexOf(""+item.News_Uni_Code) > -1) {
                        $("#news_" + item.News_Uni_Code + " .news-title").addClass("news-read");
                    }
                }

                $(".news-list .content ul").find(".news").unbind().click(function () {
                    var id = $(this).data("id");
                    var scrollTop=$(document).scrollTop();
                    $("#news-list-box").hide();

                    NewsDetail.getNewsDetail(id, scrollTop);
                    $("#news-detail-box").fadeIn();
                    $("#news_" + id + " .news-title").addClass("news-read");
                });

                //if (Paras.pageIndex == 1) {
                //    var swiper = new Swiper('.news-list .swiper-container', {
                //        direction: 'vertical',
                //        onTouchMove: function () {
                //            var y = swiper.getWrapperTranslate("y");
                //            if (y > 20) {
                //                $(".data-load-new").show();
                //            }
                //        },
                //        onTouchEnd: function () {
                //            $(".data-load-new").hide();
                //        }
                //    });
                //}
                //$(".news-list .swiper-container .swiper-slide").css("height", "auto");
            });
        } else {
            if (Paras.pageIndex == 1) {
                $(".content ul").html('<li class="no-data">暂无新闻</li>');
                NoNewsData = true;
            }else {
                if (NoNewsData) {
                    $(".content .no-more").show();
                }
            }
        }
    }

    module.exports = ObjectJS;
});