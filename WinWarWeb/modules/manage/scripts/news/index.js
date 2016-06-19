define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");
    require("pager");

    var Params = {
        keywords: '',
        pageIndex: 1,
        pageSize: 15,
        parentTypeID: 16,
        publishStatus:-1,
        typeID: 0
    };
    var ObjectJS={};
    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        ObjectJS.getList();
    };

    ObjectJS.bindEvent = function () {
        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });
        //
        var PublishStatusArr = [{ ID: '1', Name: '已发布' }, { ID: '0', Name: '未发布' }];
        var ParentTypes = [{ ID: '16', Name: '首页咨询' }, { ID: '6', Name: '产业链' }, { ID: '17', Name: '极客秀' }];
        require.async("dropdown", function () {
            $("#PublishStatus").dropdown({
                prevText: "发布状态-",
                defaultText: "全部",
                defaultValue: "-1",
                data: PublishStatusArr,
                dataValue: "ID",
                dataText: "Name",
                width: "140",
                onChange: function (data) {
                    Params.pageIndex = 1;
                    Params.publishStatus = data.value;
                    ObjectJS.getList();
                }
            });

            $("#ParentType").dropdown({
                prevText: "一级分类-",
                defaultText: "全部",
                defaultValue: "-1",
                data: ParentTypes,
                dataValue: "ID",
                dataText: "Name",
                width: "140",
                onChange: function (data) {
                    if (data.value != -1) {
                        Global.post("/Home/GetNewsTypeByParentID", { id: data.value }, function (data) {
                            require.async("dropdown", function () {
                                var NewsTypes = data.items;
                                var NewsType = NewsTypes[0];
                                $("#NewsType").dropdown({
                                    prevText: "二级分类-",
                                    defaultText: "全部",
                                    defaultValue: "-1",
                                    data: NewsTypes,
                                    dataValue: "News_Type_2",
                                    dataText: "News_Type_Name2",
                                    width: "140",
                                    onChange: function (data) {
                                        Params.pageIndex = 1;
                                        Params.typeID = data.value;
                                        ObjectJS.getList();
                                    }
                                });
                            });

                        });
                    }
                }
            });

            //Global.post("/Home/GetNewsTypeByParentID", { id: 16 }, function (data) {
            //    require.async("dropdown", function () {
            //        var NewsTypes = data.items;
            //        var NewsType = NewsTypes[0];
            //        $("#NewsType").dropdown({
            //            prevText: "二级分类-",
            //            defaultText: "全部",
            //            defaultValue: "-1",
            //            data: NewsTypes,
            //            dataValue: "News_Type_2",
            //            dataText: "News_Type_Name2",
            //            width: "140",
            //            onChange: function (data) {
            //                Params.pageIndex = 1;
            //                Params.typeID = data.value;
            //                ObjectJS.getList();
            //            }
            //        });
            //    });

            //});

        });

        //搜索框
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Params.pageIndex = 1;
                params.keyWords = keyWords;
                ObjectJS.getList();
            });
        });

        $("#publishNews").click(function () {
            ObjectJS.publishNews($(this).data("id"),1);
        });

        $("#cancelPublishNews").click(function () {
            ObjectJS.publishNews($(this).data("id"),0);
        });

    };

    ObjectJS.getList = function () {
        $(".table-list .tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='9'><div class='data-loading'><div></td></tr>");

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
                        $(".dropdown-ul li").hide();
                        if (_this.data('publish') == 0) {
                            $("#publishNews").show();
                        }
                        else {
                            $("#cancelPublishNews").show();
                        }
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));

                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 80 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    });

                });

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
            }
            else
            {
                $(".tr-header").after("<tr><td colspan='9'><div class='nodata-txt' >暂无数据!<div></td></tr>");
            }
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