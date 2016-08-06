define(function (require, exports, module) {
    var NewsDetail = require("scripts/home/newsdetail");

    var ObjectJS = {};

    ObjectJS.init = function (id, newsMain, isCollect, isPraise, userID) {
        NewsDetail.initData(id, isCollect, isPraise, userID, 0);
        newsMain = newsMain.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&quot;/g, '"');
        newsMain = newsMain.replace(/&amp;/g, ' &');
        $("#newsMain").html(newsMain);
        
        //var $newsimg = $(".header-newsimg img");
        //if ($newsimg.length == 1) {
        //    if ($newsimg.width() > $newsimg.height()) {
        //        $newsimg.css("height", $(window).width());
        //    }
        //    else {
        //        $newsimg.css("width", $(window).width());
        //    }
        //}

        NewsDetail.bindEvent(2);

        NewsDetail.getNewsComments();

        NewsDetail.setNewsMainHtml();
    };

    module.exports = ObjectJS;
});