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

    ObjectJS.init = function (id, newsMain, isCollect, isPraise) {
        Paras.id = id;
        Comment.News_Uni_Code = id;
        Paras.isCollect = isCollect;
        Paras.isPraise = isPraise;
        //newsMain = newsMain.replace(/&quot;/g, '"');
        //$("#newsMain").append(newsMain);

        var $newsimg = $(".header-newsimg img");
        if ($newsimg.length == 1) {
            if ($newsimg.width() > $newsimg.height()) {
                $newsimg.css("height", $(window).width());
            }
            else {
                $newsimg.css("width", $(window).width());
            }
        }
        ObjectJS.bindEvent();

        ObjectJS.getNewsComments();
    };

    ObjectJS.bindEvent = function () {
        $(".overlay").click(function (e) {
            if (!$(e.target).parents().hasClass("detail-reply-msg") && !$(e.target).hasClass("detail-reply-msg")) {
                $(".overlay").hide();
            }
        });

        $("#li-addComment").click(function () {
            $('.overlay').show();
        });

        //加载更多新闻讨论
        $(".load-more").click(function () {
            ObjectJS.getNewsComments();
        });

        //添加评论
        $("#btn-addComment").click(function () {
            Comment.Content = $("#comment-msg").val();
            if (Comment.Content == '') {
                return false;
            }

            ObjectJS.addNewsComment();
        });

        //收藏
        $("#addNewsCollectCount").click(function () {
            Paras.isAdd = Paras.isCollect == 1 ? 0 : 1;
            ObjectJS.addNewsCollectCount();
        });

        //喜欢
        $("#addNewsPraiseCount").click(function () {
            Paras.isAdd = Paras.isPraise == 1 ? 0 : 1;
            ObjectJS.addNewsPraiseCount();
        });
    };


    ObjectJS.getNewsComments = function () {
        $(".data-loading").show();
        Global.post("/Home/GetNewsComments", Paras, function (data) {
            $(".data-loading").hide();

            Paras.lastCommentID = data.lastCommentID;
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
                $(".load-more").show();
            }
            else {
                $(".load-more").hide();
            }

        });
    }

    ObjectJS.addNewsComment = function () {
        Global.post("/Home/AddNewsComment",{comment:JSON.stringify(Comment)}, function (data) {
            if (data.result > 0) {
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
        });
    }

    ObjectJS.addNewsCollectCount = function () {
        Global.post("/Home/AddNewsCollectCount", Paras, function (data) {
            if (data.result > 0) {
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
        });
    }

    ObjectJS.addNewsPraiseCount = function () {
        Global.post("/Home/AddNewsPraiseCount", Paras, function (data) {
            if (data.result > 0) {
                if (Paras.isAdd) {
                    alert("点赞成功");
                    Paras.isPraise = 1;

                    $("#addNewsPraiseCount").find("img").attr("src", "/modules/images/like_color.png");
                    $("#addNewsPraiseCount").find(".praise-title").html("取消赞");
                    $(".Praise_Count").html(parseInt($(".Praise_Count").html()) +1);
                }
                else {
                    alert("取消点赞");
                    Paras.isPraise = 0;

                    $("#addNewsPraiseCount").find("img").attr("src", "/modules/images/like.png");
                    $("#addNewsPraiseCount").find(".praise-title").html("赞");
                    $(".Praise_Count").html(parseInt($(".Praise_Count").html()) -1);
                }
            }
        });
    }

    module.exports = ObjectJS;
});