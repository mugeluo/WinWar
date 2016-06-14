define(function (require, exports, module) {
    var Global = require("global");
    var Upload = require("upload");

    var News = {
        Title_Main: '',
        Title_Sub: '',
        Title_App: '',
        News_Sum: '',
        News_Author: '',
        Real_Source_Name: '',
        Nega_Post_Par: 3,
        Impt_Par: 2,
        News_Type: 0,
        Html_Txt:''
    };
    var ObjectJS = {};
    ObjectJS.init = function () {
        ObjectJS.bindEvent();

        //ObjectJS.getList();
    };

    ObjectJS.bindEvent = function () {
        //部门搜索
        var ParentTypes = [{ ID: '16', Name: '首页咨询' }, { ID: '6', Name: '产业链' }, { ID: '17', Name: '极客秀' }];
        require.async("dropdown", function () {
            $("#ParentType").dropdown({
                prevText: "一级分类-",
                defaultText: "首页咨询",
                defaultValue: "16",
                data: ParentTypes,
                dataValue: "ID",
                dataText: "Name",
                width: "140",
                onChange: function (data) {
                    Global.post("/Home/GetNewsTypeByParentID", { id: data.value }, function (data) {
                        require.async("dropdown", function () {
                            var NewsTypes = data.items;
                            var NewsType = NewsTypes[0];
                            $("#NewsType").dropdown({
                                prevText: "二级分类-",
                                defaultText: NewsType.News_Type_Name2,
                                defaultValue: NewsType.News_Type_2,
                                data: NewsTypes,
                                dataValue: "News_Type_2",
                                dataText: "News_Type_Name2",
                                width: "140",
                                onChange: function (data) {
                                    //ObjectJS.Params.PageIndex = 1;
                                    //ObjectJS.Params.DepartID = data.value;
                                    //ObjectJS.getList();
                                }
                            });
                        });

                    });
                    
                }
            });
        });

        ProductIco = Upload.createUpload({
            element: "#Pic_URL",
            buttonText: "选择图片",
            className: "",
            data: { folder: '', action: 'add', oldPath: "" },
            success: function (data, status) {
                //if (data.Items.length > 0) {
                //    _self.ProductImage = data.Items[0];
                //    $("#productImg").attr("src", data.Items[0]);
                //}
                //else {
                //    alert("只能上传jpg/png/gif类型的图片，且大小不能超过5M！");
                //}
            }
        });

        $("#btn-saveNews").click(function () {
            alert(111);
            //ObjectJS.saveNews();
        });

    };

    ObjectJS.saveNews = function () {
        Global.post("/manage/news/saveNews", { news: JSON.stringify(News) }, function (data) {
            if (data.result == 1) {

            }
            else {

            }
        });
    }

    module.exports = ObjectJS;
});