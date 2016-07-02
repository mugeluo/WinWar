define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");
    require("pager");

    var Params = {
        keywords: '',
        pageIndex: 1,
        pageSize: 15,
        parentTypeID: 16,
        publishStatus: 0,
        typeID: 0,
        bigTypeID: -1
    };
    var ObjectJS = {};
    var CacheTypes = [];
    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        ObjectJS.getList();
    };

    ObjectJS.bindEvent = function () {
        var _self = this;
        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });

        //发布状态
        $(".search-status li").click(function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                _this.siblings().removeClass("hover");
                _this.addClass("hover");
                Params.pageIndex = 1;
                Params.publishStatus = _this.data("id");
                _self.getList();
            }
        });

        //一级分类
        $(".search-bigtype .item").click(function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                _this.siblings().removeClass("hover");
                _this.addClass("hover");
                Params.pageIndex = 1;
                Params.bigTypeID = _this.data("id");
                Params.typeID = -1;
                _self.getList();
                _self.getChildTypes(_this.data("id"));
            }
        });

        //二级分类
        $(".search-type").delegate(".item", "click", function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                _this.siblings().removeClass("hover");
                _this.addClass("hover");
                Params.pageIndex = 1;
                Params.typeID = _this.data("id");
                _self.getList();
            }
        });

        //搜索框
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Params.pageIndex = 1;
                Params.keyWords = keyWords;
                ObjectJS.getList();
            });
        });

        $("#publishNews").click(function () {
            location.href = "/Manage/News/detail/"+$(this).data("id");
        });

        $("#cancelPublishNews").click(function () {
            location.href = "/Manage/News/detail/"+$(this).data("id");
        });

        $("#editNews").click(function () {
            location.href = "/Manage/News/Edit/" + $(this).data("id");
        });

    };

    ObjectJS.getChildTypes = function (typeid) {
        $(".search-type .item[data-id!='-1']").remove();
        $(".search-type .item").addClass("hover");
        if (typeid > 0) {
            if (CacheTypes[typeid]) {
                for (var i = 0; i < CacheTypes[typeid].length; i++) {
                    $(".search-type").append("<li class='item' data-id='" + CacheTypes[typeid][i].News_Type_2 + "'>" + CacheTypes[typeid][i].News_Type_Name2 + "</li>")
                }
            } else {
                Global.post("/Home/GetNewsTypeByParentID", { id: typeid }, function (data) {
                    CacheTypes[typeid] = data.items;
                    for (var i = 0; i < CacheTypes[typeid].length; i++) {
                        $(".search-type").append("<li class='item' data-id='" + CacheTypes[typeid][i].News_Type_2 + "'>" + CacheTypes[typeid][i].News_Type_Name2 + "</li>")
                    }
                });
            }
        }
    }

    ObjectJS.getList = function () {
        $(".table-list .tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='15'><div class='data-loading'><div></td></tr>");

        Global.post("/Manage/News/GetNews", Params, function (data) {
            $(".tr-header").nextAll().remove();

            var items = data.items;
            var len=items.length;
            if(len>0){
                DoT.exec("manage/template/news/news-list.html", function (template) {
                    var innerhtml = template(items);
                    innerhtml = $(innerhtml);
                    $(".table-list .tr-header").after(innerhtml);
                    innerhtml.fadeIn(400);

                    //操作
                    innerhtml.find(".dropdown").click(function () {
                        var _this = $(this);
                        $(".dropdown-ul li").show();
                        if (_this.data('publish') == 0) {
                            $("#cancelPublishNews").hide();
                        }
                        else {
                            $("#publishNews").hide();
                        }
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));

                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 80 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    });

                });
            }
            else
            {
                $(".tr-header").after("<tr><td colspan='15'><div class='nodata-txt' >暂无数据!<div></td></tr>");
            }

            $("#pager").paginate({
                total_count: data.totalCount,
                count: data.pageCount,
                start: Params.pageIndex,
                display: 5,
                images: false,
                mouse: 'slide',
                onChange: function (page) {
                    $(".tr-header").nextAll().remove();
                    Params.pageIndex = page;
                    ObjectJS.getList();
                }
            });
        });

    }

    ObjectJS.publishNews = function (id, isPublish) {
        Global.post("/manage/news/PublishNews", { id: id, isPublish: isPublish }, function (data) {
            if (data.result == 1) {
                ObjectJS.getList();
            }
            else {
                alert("保存失败");
            }
        });
    }
    module.exports = ObjectJS;
});