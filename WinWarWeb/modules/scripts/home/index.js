define(function (require, exports, module) {
    var ObjectJS={};

    ObjectJS.init = function () {
        ObjectJS.bindEvent();
    };

    ObjectJS.bindEvent = function () {
        $(".menu li").click(function () {
            $(this).addClass("menu-active");
        });
    };

    exports.module = ObjectJS;
});