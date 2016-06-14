define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");
    require("pager");

    var Params = {
        keywords: '',
        pageIndex: 1,
        pageSize: 15,
        parentTypeID: 16,
        typeID: 0
    };
    var ObjectJS={};
    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        ObjectJS.getList();
    };

    ObjectJS.bindEvent = function () { 
        //部门搜索
        var ParentTypes = [{ ID: '16', Name: '首页咨询' }, { ID: '6', Name: '产业链' }, { ID: '17', Name: '极客秀' }];
        require.async("dropdown", function () {
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

    };

    ObjectJS.getList = function () {
        $(".table-list .tr-header").nextAll().remove();

        Global.post("/Manage/News/GetNews", Params, function (data) {
            var items = data.items;
            DoT.exec("manage/template/news/news-list.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                $(".table-list .tr-header").after(innerhtml);
                innerhtml.fadeIn(400);
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
        });

    }
    module.exports = ObjectJS;
});