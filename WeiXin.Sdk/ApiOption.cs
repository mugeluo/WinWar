using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WeiXin.Sdk
{
    public enum ApiOption
    {
        [Description("/sns/oauth2/access_token")]
        access_token,

        [Description("/sns/userinfo")]
        userinfo,

        [DescriptionAttribute("erp.manufacture.pullFentGoodsCodes")]
        pullFentGoodsCodes,

        [Description("erp.manufacture.pullFentDataList")]
        pullFentDataList,

        [Description("erp.manufacture.batchUpdateFent")]
        batchUpdateFent,

        [Description("erp.manufacture.pullBulkGoodsCodes")]
        pullBulkGoodsCodes,

        [Description("erp.manufacture.pullBulkDataList")]
        pullBulkDataList,

        [Description("erp.manufacture.batchUpdateBulk")]
        batchUpdateBulk,

        [Description("erp.manufacture.pushProductionPlan")]
        pushProductionPlan
    }
}
