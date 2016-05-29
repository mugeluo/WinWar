
/*基础配置*/
seajs.config({
    base: "/modules/",
    alias: {
        "jquery": "/Scripts/jquery-1.11.1.js",
        "global": "scripts/global.js",
        //HTML模板引擎
        "dot": "plug/doT.js"
    },
    map: [
        //可配置版本号
        ['.css', '.css?v=20160525'],
        ['.js', '.js?v=20160525']
    ]
});


seajs.config({
    alias: {
        //数据验证
        "verify": "plug/verify.js"
    }
});

