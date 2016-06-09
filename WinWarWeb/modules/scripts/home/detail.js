define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        id: 0,
        isAdd: 1,
        isCollect: 0,
        isPraise:0,
        content:''
    };

    var ObjectJS = {};

    ObjectJS.init = function (id, newsMain, isCollect, isPraise) {
        Paras.id = id;
        Paras.isCollect = isCollect;
        Paras.isPraise = isPraise;
        newsMain=newsMain.replace(/&quot;/g, '"');
        $("#newsMain").html(newsMain);

        ObjectJS.bindEvent();

        ObjectJS.getNewsComments();
    };

    ObjectJS.bindEvent = function () {

        $(".overlay").click(function (e) {
            
        });

        $("#li-addComment").click(function () {
            $('.overlay').show();
        });

        $(".overlay-cancel").click(function () {
            $('.overlay').hide();
        });

        $("#btn-addComment").click(function () {
            Paras.content = $("#comment-msg").val();
            if (Paras.content == '') {
                return false;
            }

            ObjectJS.addNewsComment();
        });

        $("#addNewsCollectCount").click(function () {
            Paras.isAdd = Paras.isCollect == 1 ? 0 : 1;
            ObjectJS.addNewsCollectCount();
        });

        $("#addNewsPraiseCount").click(function () {
            Paras.isAdd = Paras.isPraise == 1 ? 0 : 1;
            ObjectJS.addNewsPraiseCount();
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

    ObjectJS.getNewsComments = function () {
        
        Global.post("/Home/GetNewsComments", Paras, function (data) {
            var items = data.items;

            DoT.exec("template/home/reply-list.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);

                $(".reply-list ul").append(innerhtml);
                innerhtml.fadeIn(400);

            });
        });
    }

    ObjectJS.addNewsComment = function () {
        Global.post("/Home/AddNewsComment", Paras, function (data) {
            if (data.result > 0) {
                alert("评论成功");

                $("#comment-msg").val('');
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
                }
                else {
                    alert("取消点赞");
                    Paras.isPraise = 0;

                    $("#addNewsPraiseCount").find("img").attr("src", "/modules/images/like.png");
                    $("#addNewsPraiseCount").find(".praise-title").html("赞");
                }
            }
        });
    }
    module.exports = ObjectJS;
});