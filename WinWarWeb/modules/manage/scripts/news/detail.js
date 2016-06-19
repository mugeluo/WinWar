define(function (require, exports, module) {
    var Global = require("global");
    var Upload = require("upload");
    var Verify = require("verify"), VerifyObject;

    var News = {
        News_Uni_Code:0,
        Title_Main: '',
        Title_Sub: '',
        Title_App: '',
        News_Sum: '',
        News_Author: '',
        Real_Source_Name: '',
        Nega_Post_Par: 3,
        Impt_Par: 2,
        News_Type: "",
        News_Type_Name2:'',
        News_Type_1: "",
        News_Type_Name1: "",
        Html_Txt:''
    };
    var ObjectJS = {};
    var Editor = null;
    ObjectJS.init = function (um) {
        ObjectJS.News_Type = 0;
        Editor = um;
        ObjectJS.bindEvent();

        VerifyObject = Verify.createVerify({
            element: ".verify",
            emptyAttr: "data-empty",
            verifyType: "data-type",
            regText: "data-text"
        });

        VerifyObject.isPass();
    };

    ObjectJS.initEdit = function (um, News_Uni_Code, News_Type_1, News_Type_Name1, News_Type, News_Type_Name2, Is_Issue, Nega_Posi_Par, Impt_Par, Html_Txt) {
        News.News_Type_1 = News_Type_1;
        News.News_Type_Name1 = News_Type_Name1;
        News.News_Type = News_Type;
        News.News_Type_Name2 = News_Type_Name2;
        ObjectJS.News_Type = News_Type;
        Html_Txt = Html_Txt.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
        
        Editor = um;
        ObjectJS.bindEvent();

        VerifyObject = Verify.createVerify({
            element: ".verify",
            emptyAttr: "data-empty",
            verifyType: "data-type",
            regText: "data-text"
        });

        $(".Is_Issue .radiobox .ico-radiobox[data-value='" + Is_Issue + "']").addClass("hover");
        $(".Impt_Par .radiobox .ico-radiobox[data-value='" + Impt_Par + "']").addClass("hover");
        $(".Nega_Posi_Par .radiobox .ico-radiobox[data-value='" + Nega_Posi_Par + "']").addClass("hover");
        Editor.ready(function () {
            Editor.setContent(Html_Txt);
        });
        
    };

    ObjectJS.bindEvent = function () {
        var _self = this;
        require.async("dropdown", function () {
            Global.post("/Home/GetNewsTypeByParentID", { id: -1 }, function (data) {
                $("#ParentType").dropdown({
                    prevText: "一级分类-",
                    defaultText: News.News_Type_Name1 || data.items[0].News_Type_Name1,
                    defaultValue: News.News_Type_1 || data.items[0].News_Type_1,
                    data: data.items,
                    dataValue: "News_Type_1",
                    dataText: "News_Type_Name1",
                    width: "140",
                    onChange: function (data) {
                        _self.getChildTypes(data.value, null, null);
                    }
                });

                if (News.News_Type && News.News_Type_Name2) {
                    _self.getChildTypes(News.News_Type_1, News.News_Type, News.News_Type_Name2);
                } else {
                    _self.getChildTypes(data.items[0].News_Type_1, null, null);
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

        $(".Is_Issue .radiobox").click(function () {
            var _this = $(this);
            var _radiobox=_this.find(".ico-radiobox");
            if (!_radiobox.hasClass("hover")) {
                _radiobox.addClass("hover");
                _this.siblings().find(".ico-radiobox").removeClass("hover");
            }

        });

        $(".Nega_Post_Par .radiobox").click(function () {
            var _this = $(this);
            var _radiobox = _this.find(".ico-radiobox");
            if (!_radiobox.hasClass("hover")) {
                _radiobox.addClass("hover");
                _this.siblings().find(".ico-radiobox").removeClass("hover");
            }

        });

        $(".Impt_Par .radiobox").click(function () {
            var _this = $(this);
            var _radiobox = _this.find(".ico-radiobox");
            if (!_radiobox.hasClass("hover")) {
                _radiobox.addClass("hover");
                _this.siblings().find(".ico-radiobox").removeClass("hover");
            }

        });

        $("#btn-saveNews").click(function () {
            if (!VerifyObject.isPass()) {
                return false;
            }
            if (ObjectJS.News_Type == 0) {
                alert("选择新闻二级分类");
                return;
            }

            News = {
                News_Uni_Code:News.News_Uni_Code,
                Title_Main: $("#Title_Main").val(),
                Title_Sub: $("#Title_Sub").val(),
                Title_App: $("#Title_App").val(),
                News_Sum: $("#News_Sum").val(),
                News_Author: $("#News_Author").val(),
                Real_Source_Name: $("#Real_Source_Name").val(),
                Is_Issue: 0,
                Nega_Posi_Par: $(".Nega_Posi_Par .radiobox .hover").data("value"),
                Impt_Par: $(".Impt_Par .radiobox .hover").data("value"),
                News_Type: ObjectJS.News_Type,
                Html_Txt: Editor.getContent()
            };

            ObjectJS.saveNews();
        });
    };

    //获取下级分类
    ObjectJS.getChildTypes = function (parentid, typeid, typename) {
        Global.post("/Home/GetNewsTypeByParentID", { id: parentid }, function (data) {
            require.async("dropdown", function () {
                $("#NewsType").dropdown({
                    prevText: "二级分类-",
                    defaultText: typename || data.items[0].News_Type_Name2,
                    defaultValue: typeid || data.items[0].News_Type_2,
                    data: data.items,
                    dataValue: "News_Type_2",
                    dataText: "News_Type_Name2",
                    width: "140",
                    onChange: function (data) {
                        ObjectJS.News_Type = data.value;
                    }
                });
            });

        });
    }

    ObjectJS.saveNews = function () {
        Global.post("/manage/news/saveNews", { news: JSON.stringify(News) }, function (data) {
            if (data.result == 1) {
                if (News.News_Uni_Code == 0) {
                    confirm("是否继续新建新闻?", function () {
                        location.href = location.href;
                    },
                    function () {
                        location.href = "/Manage/News/Index";
                    });
                }
                else {
                    alert("保存成功");
                    setTimeout(function () {
                        location.href = "/Manage/News/Index";
                    }, 500);
                }
            }
            else {
                alert("抱歉,保存失败");
            }
        });
    }

    module.exports = ObjectJS;
});