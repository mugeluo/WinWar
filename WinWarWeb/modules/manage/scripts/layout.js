define(function (require, exports, module) {
   var ObjectJS = {};
   ObjectJS.init = function () {
       ObjectJS.bindEvent();
   }

   ObjectJS.bindEvent = function () {
       //窗体滚动 置顶头部
       $(window).scroll(function () {
           if ($(window).scrollTop() > 70) {
               $(".back-top").fadeIn(500);
           }
           else {
               $(".back-top").fadeOut(1000);
           }
       });

       //返回顶部
       $(".back-top").click(function () {
           $('body,html').animate({ scrollTop: 0 }, 300);
           return false;
       });

       //登录信息展开
       $("#currentUser").click(function () {
           $(".dropdown-userinfo").fadeIn("1000");
       });

       $(document).click(function (e) {

           if (!$(e.target).parents().hasClass("currentuser") && !$(e.target).hasClass("currentuser")) {
               $(".dropdown-userinfo").fadeOut("1000");
           }

       });

       $(".controller-box").click(function () {
           var _this = $(this).parent();
           var _self = ObjectJS;
           if (!_this.hasClass("select")) {
               _self.setRotateR(_this.find(".open"), 0, 90);
               _this.addClass("select");
               _this.find(".action-box").slideDown(200);
           } else {
               _self.setRotateL(_this.find(".open"), 90, 0);
               _this.removeClass("select");
               _this.find(".action-box").slideUp(200);
           }
       });

       //一级菜单图标事件处理
       $("#modulesMenu a").mouseenter(function () {
           var _this = $(this).find("img");
           _this.attr("src", _this.data("hover"));
       });

       $("#modulesMenu a").mouseleave(function () {
           if (!$(this).hasClass("select")) {
               var _this = $(this).find("img");
               _this.attr("src", _this.data("ico"));
           }
       });

       $("#modulesMenu .select img").attr("src", $("#modulesMenu .select img").data("hover"));
   }

    //旋转按钮（顺时针）
   ObjectJS.setRotateR = function (obj, i, v) {
       var _self = this;
       if (i < v) {
           i += 3;
           setTimeout(function () {
               obj.css("transform", "rotate(" + i + "deg)");
               _self.setRotateR(obj, i, v);
           }, 5)
       }
   }

    //旋转按钮(逆时针)
   ObjectJS.setRotateL = function (obj, i, v) {
       var _self = this;
       if (i > v) {
           i -= 3;
           setTimeout(function () {
               obj.css("transform", "rotate(" + i + "deg)");
               _self.setRotateL(obj, i, v);
           }, 5)
       }
   }
   module.exports = ObjectJS;
});