define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        id: 0,
        lastCommentID: 0,
        pageSize:5,
        isAdd: 1,
        isCollect: 0,
        isPraise:0
    };

    var Comment = {
        News_Uni_Code: 0,
        Content: '',
        Reply_ID:0,
        Reply_User_ID: 0,
        Reply_User_Name: ''
    };

    var ObjectJS = {};

    ObjectJS.bindEvent = function (option) {
        ObjectJS.scrollTop = 0;
        ObjectJS.option = 1;
        if (option) {
            ObjectJS.option = 2;
        }
        //返回 滚动悬浮
        $(window).scroll(function () {
            if ($(window).scrollTop() > 70) {
                $(".header-back").css({"background-color":"#fff","border-bottom":"1px solid #ddd"}).find(".icon").css("color","#666");
            }
            else {
                $(".header-back").css({ "background": "none", "border-bottom": "none" }).find(".icon").css("color", "#fff");
            }
        });

        //返回
        $(".header-back").click(function () {
            if (ObjectJS.option ==2) {
                if (history.length > 1) {
                    history.go(-1);
                } else {
                    location.href = "/home/index";
                }
            } else {
                $("#news-detail-box").hide();
                $("#news-list-box").fadeIn();
                $('body,html').animate({ scrollTop: ObjectJS.scrollTop }, 100);
            }
        });

        //弹出添加讨论遮罩层
        $("#li-addComment").click(function () {
            if (!ObjectJS.validateLogin()) {
                return;
            }
            $(".overlay-add-reply").css("height", $("#news-detail-box").height() + "px");
            $(document).scrollTop($("#news-detail-box").height() - $(window).height());
            $('.overlay-add-reply').show();
        });

        //添加讨论遮罩层点击
        $(".overlay-add-reply").click(function (e) {
            if (!$(e.target).parents().hasClass("detail-reply-msg") && !$(e.target).hasClass("detail-reply-msg")) {
                $(".overlay-add-reply").hide();
            }
        });

        //加载更多新闻讨论
        $(".detail-replys .load-more").click(function () {
            ObjectJS.getNewsComments();
        });

        //添加评论
        $("#btn-addComment").click(function () {
            if (!ObjectJS.validateLogin()) {
                return;
            }

            Comment.Content = $("#comment-msg").val();
            if (Comment.Content == '') {
                $('.overlay-add-reply').hide();
                return;
            }else {
                if (Comment.Content.length > 500) {
                    alert("内容太多了");
                    return;
                }
            }
           
            ObjectJS.addNewsComment();
        });

        //收藏
        $("#addNewsCollectCount").click(function () {
            if (!ObjectJS.validateLogin()) {
                return;
            }

            Paras.isAdd = Paras.isCollect == 1 ? 0 : 1;
            ObjectJS.addNewsCollectCount();
        });

        //喜欢
        $("#addNewsPraiseCount").click(function () {
            if (!ObjectJS.validateLogin()) {
                return;
            }

            Paras.isAdd = Paras.isPraise == 1 ? 0 : 1;
            ObjectJS.addNewsPraiseCount();
        });
    };

    ObjectJS.getNewsDetail = function (id, scrollTop) {
        Global.post("/Home/GetNewsDetail", { id: id }, function (data) {
            var item = data.item;
            var passport = data.passport;

            ObjectJS.initData(id, item.Is_Collect, item.Is_Praise, passport.UserID, scrollTop);
            ObjectJS.setNewsDetail(item);
            ObjectJS.getNewsComments();

        });
    }

    ObjectJS.initData = function (id, isCollect, isPraise, userID, scrollTop) {
        $('body,html').animate({ scrollTop: 0 }, 50);

        //
        var ReadNewsCache = window.localStorage.getItem("ReadNewsCache");
        if (ReadNewsCache == null || ReadNewsCache == '') {
            ReadNewsCache = [];
            ReadNewsCache.push(id)
            window.localStorage.setItem("ReadNewsCache", ReadNewsCache.join('|'));
        }
        else {
            ReadNewsCache = ReadNewsCache.split('|');
            if (ReadNewsCache.indexOf(id) == -1) {
                ReadNewsCache.push(id);
                window.localStorage.setItem("ReadNewsCache", ReadNewsCache.join('|'));
            }
        }

        Paras.lastCommentID = 0;
        Paras.id = id;
        Paras.isCollect = isCollect;
        Paras.isPraise = isPraise;
        Comment.News_Uni_Code = id;
        ObjectJS.userID = userID;
        ObjectJS.scrollTop = scrollTop;
        $(".reply-list ul").html('');
    };

    ObjectJS.setNewsDetail = function (Item) {
        $(".header-content .source").html(Item.Real_Source_Name == '' ? "来源未知" : Item.Real_Source_Name);
        $(".header-content .title").html(Item.Title_Main);
        $(".news-author .author-name").html(Item.News_Author == '' ? "--" : Item.News_Author);
        $(".news-author .author-time").html(Item.Pub_Time.toDate("yyyy-MM-dd hh:mm:ss"));
        $(".author-read .read").html(Item.View_Count);

        var newsMain = decodeURI(Item.Html_Txt);//.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
        $("#newsMain").html(newsMain);
        if (Item.Pic_URL!='') {
            $(".header-newsimg").html('<img src="' + Item.Pic_URL + '" />');

            var $newsimg = $(".header-newsimg img");
            if ($newsimg.length == 1) {
                if ($newsimg.width() > $newsimg.height()) {
                    $newsimg.css("height", $(window).width());
                }
                else {
                    $newsimg.css("width", $(window).width());
                }
            }
        }

        if (Item.Is_Collect==1) {
            $("#addNewsCollectCount").find("img").attr("src", "/modules/images/collect_color.png");
            $("#addNewsCollectCount").find("span").html("取消收藏");
        }

        if (Item.Is_Praise == 1) {
            $("#addNewsPraiseCount").find("img").attr("src", "/modules/images/like_color.png");
        }
        $(".Praise_Count").html(Item.Praise_Count);
        $("#Comment_Count").html(Item.Comment_Count);
    }

    ObjectJS.getNewsComments = function () {
        $(".detail-replys .data-loading").show();
        Global.post("/Home/GetNewsComments", Paras, function (data) {
            $(".detail-replys .data-loading").hide();

            var items = data.items;
            var len=items.length;
            if (len > 0) {
                $(".reply-list ul li.no-data").remove();
                DoT.exec("template/home/reply-list.html", function (template) {
                    var innerhtml = template(items);
                    innerhtml = $(innerhtml);

                    $(".reply-list ul").append(innerhtml);
                    innerhtml.fadeIn(400);

                });
            }

            if (len == Paras.pageSize) {
                $(".detail-replys .load-more").show();
            }
            else {
                if (len == 0 && Paras.lastCommentID == 0) {
                    $(".reply-list ul").append('<li class="no-data">暂无评论</li>');
                }
                $(".detail-replys .load-more").hide();

            }

            Paras.lastCommentID = data.lastCommentID;

        });
    }

    ObjectJS.addNewsComment = function () {
        Global.post("/Home/AddNewsComment",{comment:JSON.stringify(Comment)}, function (data) {
            if (data.result == 1) {
                alert("评论成功");

                $(".reply-list .no-data").remove();
                var items = data.items;
                DoT.exec("template/home/reply-list.html", function (template) {
                    var innerhtml = template(items);
                    innerhtml = $(innerhtml);

                    $(".reply-list ul").prepend(innerhtml);
                    innerhtml.fadeIn(400);

                });

                $("#comment-msg").val('');
                Comment.Content = '';
                $("#Comment_Count").html(parseInt( $("#Comment_Count").html())+1 );
                $('.overlay').hide();

            }
            else if (data.result == -1) {
                confirm("登录后才能操作，立即登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });
            }

        });
    }

    ObjectJS.addNewsCollectCount = function () {
        Global.post("/Home/AddNewsCollectCount", Paras, function (data) {
            if (data.result == 1) {
                if (Paras.isAdd == 1) {
                    alert("收藏成功");
                    Paras.isCollect = 1;

                    $("#addNewsCollectCount").find("img").attr("src", "/modules/images/collect_color.png");
                    $("#addNewsCollectCount").find("span").html("取消收藏");
                }
                else {
                    alert("取消收藏");
                    Paras.isCollect = 0;

                    $("#addNewsCollectCount").find("img").attr("src", "/modules/images/collect.png");
                    $("#addNewsCollectCount").find("span").html("收藏");
                }
            }
            else if (data.result == -1) {
                confirm("登录后才能操作，立即登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });
            }

        });
    }

    ObjectJS.addNewsPraiseCount = function () {
        Global.post("/Home/AddNewsPraiseCount", Paras, function (data) {
            if (data.result == 1) {
                if (Paras.isAdd==1) {
                    alert("点赞成功");
                    Paras.isPraise = 1;

                    $("#addNewsPraiseCount").find("img").attr("src", "/modules/images/like_color.png");
                    $(".Praise_Count").html(parseInt($(".Praise_Count").html()) +1);
                }
                else {
                    alert("取消点赞");
                    Paras.isPraise = 0;

                    $("#addNewsPraiseCount").find("img").attr("src", "/modules/images/like.png");
                    $(".Praise_Count").html(parseInt($(".Praise_Count").html()) -1);
                }
            }
            else if (data.result == -1) {
                confirm("登录后才能操作，立即登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });
            }

        });
    }

    ObjectJS.validateLogin = function () {
        if (ObjectJS.userID == 0) {
            confirm("登录后才能操作，立即登录", function () {
                location.href = "/user/login?returnUrl=" + location.href;
            });

            return false;
        }

        return true;
    }
    module.exports = ObjectJS;
});