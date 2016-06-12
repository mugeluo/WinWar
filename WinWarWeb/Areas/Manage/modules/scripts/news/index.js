define(function (require, exports, module) {
    var Global = require("global");
    var DoT = require("dot");

    var ObjectJS={};
    ObjectJS.init = function () {
        ObjectJS.bindEvent();
    };

    ObjectJS.bindEvent = function () { 
        //部门搜索
        var departs = [{ DepartID: '1', Name: '2' }];
        require.async("dropdown", function () {
            $("#ddlDepart").dropdown({
                prevText: "部门-",
                defaultText: "全部",
                defaultValue: "",
                data: departs,
                dataValue: "DepartID",
                dataText: "Name",
                width: "180",
                onChange: function (data) {
                    //ObjectJS.Params.PageIndex = 1;
                    //ObjectJS.Params.DepartID = data.value;
                    //ObjectJS.getList();
                }
            });
        });

        //搜索框
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                //ObjectJS.Params.PageIndex = 1;
                //ObjectJS.Params.keyWords = keyWords;
                //ObjectJS.getList();
            });
        });

    };

    module.exports = ObjectJS;
});