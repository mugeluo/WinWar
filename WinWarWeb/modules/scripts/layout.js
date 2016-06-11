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
   }

   module.exports = ObjectJS;
});