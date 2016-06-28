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

    ObjectJS.init = function (id, newsMain, isCollect, isPraise, userID) {
        //
        var ReadNewsCache = window.localStorage.getItem("ReadNewsCache");
        if (ReadNewsCache == null || ReadNewsCache=='') {
            ReadNewsCache = [];
            ReadNewsCache.push(id)
            window.localStorage.setItem("ReadNewsCache", ReadNewsCache.join('|') );
        }
        else {
            ReadNewsCache = ReadNewsCache.split('|');
            if (ReadNewsCache.indexOf(id)==-1) {
                ReadNewsCache.push(id);
                window.localStorage.setItem("ReadNewsCache", ReadNewsCache.join('|'));
            }
        }

        Paras.id = id;
        Paras.isCollect = isCollect;
        Paras.isPraise = isPraise;
        Comment.News_Uni_Code = id;
        ObjectJS.userID = userID;
        newsMain = newsMain.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
        
        $("#newsMain").html(newsMain);

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
        //
        $(window).scroll(function () {
            if ($(window).scrollTop() > 70) {
                $(".header-back").css({"background-color":"#fff","border-bottom":"1px solid #ddd"}).find(".icon").css("color","#666");
            }
            else {
                $(".header-back").css({ "background": "none", "border-bottom": "none" }).find(".icon").css("color", "#fff");
            }
        });
        
        //
        $(".overlay").click(function (e) {
            if (!$(e.target).parents().hasClass("detail-reply-msg") && !$(e.target).hasClass("detail-reply-msg")) {
                $(".overlay").hide();
            }
        });

        //
        $("#li-addComment").click(function () {
            if (ObjectJS.userID == 0) {
                confirm("您还未登录，立即去登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });

                return;
            }

            $('.overlay').show();
        });

        //加载更多新闻讨论
        $(".load-more").click(function () {
            ObjectJS.getNewsComments();
        });

        //添加评论
        $("#btn-addComment").click(function () {
            if (ObjectJS.userID == 0) {
                confirm("您还未登录，立即去登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });

                return;
            }

            Comment.Content = $("#comment-msg").val();
            if (Comment.Content == '') {
                $('.overlay').hide();
                return false;
            }
            else {
                if (Comment.Content.length > 500) {
                    alert("内容太多了");
                    return;
                }
            }
           
            ObjectJS.addNewsComment();
        });

        //收藏
        $("#addNewsCollectCount").click(function () {
            if (ObjectJS.userID == 0) {
                confirm("您还未登录，立即去登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });

                return;
            }

            Paras.isAdd = Paras.isCollect == 1 ? 0 : 1;
            ObjectJS.addNewsCollectCount();
        });

        //喜欢
        $("#addNewsPraiseCount").click(function () {
            if (ObjectJS.userID == 0) {
                confirm("您还未登录，立即去登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });

                return;
            }

            Paras.isAdd = Paras.isPraise == 1 ? 0 : 1;
            ObjectJS.addNewsPraiseCount();
        });

        //header-back
        $(".header-back").click(function () {
            if (window.parent) {
                if ($(window.parent.document).find(".news-detail").length>0) {
                    $(window.parent.document).find(".news-detail").fadeOut();
                } else {
                    if (history.length > 1) {
                        history.go(-1);
                    } else {
                        location.href = "/home/index";
                    }
                }
            } else {
                if (history.length > 1) {
                    history.go(-1);
                } else {
                    location.href = "/home/index";
                }
            }
            //location.href = "/home/index";
            //history.go(-1);
            //window.opener = null;
            //window.open('', '_self');
            //window.close();
           
            //if (navigator.userAgent.indexOf("MSIE") > 0) {
            //    if (navigator.userAgent.indexOf("MSIE 6.0") > 0) {
            //        window.opener = null;
            //        window.close();
            //    }
            //    else {
            //        window.open('', '_top');
            //        window.top.close();
            //    }
            //}
            //else if (navigator.userAgent.indexOf("Firefox") > 0) {
            //    window.location.href = 'about:blank ';
            //}
            //else {
            //    window.opener = null; window.open('', '_self'); window.close();
            //}

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
                confirm("您还未登录，立即去登录", function () {
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
                confirm("您还未登录，立即去登录", function () {
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
                confirm("您还未登录，立即去登录", function () {
                    location.href = "/user/login?returnUrl="+location.href;
                });
            }

        });
    }

    module.exports = ObjectJS;
});