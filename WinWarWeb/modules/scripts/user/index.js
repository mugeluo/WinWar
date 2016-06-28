define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var Paras = {
        lastFavoriteID: 0,
        pageSize:5
    };

    var ObjectJS = {};
    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        ObjectJS.getNewsCollect();
    };

    ObjectJS.bindEvent = function () {

        //加载更多新闻讨论
        $(".load-more").click(function () {
            ObjectJS.getNewsCollect();
        });
    };


    ObjectJS.getNewsCollect = function () {
        $(".data-loading").show();
        Global.post("/User/GetNewsFavorites", Paras, function (data) {
            $(".data-loading").hide();

            Paras.lastFavoriteID = data.lastFavoriteID;
            var items = data.items;
            var len=items.length;
            if (len > 0) {
                $(".content ul li.no-data").remove();
                DoT.exec("template/home/news-list.html", function (template) {
                    var innerhtml = template(items);
                    innerhtml = $(innerhtml);

                    $(".content ul").append(innerhtml);
                    innerhtml.fadeIn(400);

                    $(".content ul").find(".news").click(function () {
                        var id = $(this).data("id");
                        location.href = "/home/detail/" + id;
                        //$("#iframe-news-detail").attr("src", "/home/detail/" + id);
                        //$(".news-detail").fadeIn();
                        //$("#news_" + id + " .news-title").addClass("news-read");
                    });
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

    module.exports = ObjectJS;
});