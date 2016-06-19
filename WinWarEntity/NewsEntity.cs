using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    public class NewsEntity
    {
        public long News_Uni_Code { get; set; }

        public DateTime Pub_Time { get; set; }

        public string News_Author { get; set; }

        public int Nega_Posi_Par { get; set; }

        public string Title_Main { get; set; }

        public string Title_Sub { get; set; }

        public string Title_App { get; set; }

        public int News_Type { get; set; }

        public int Source_Uni_Code { get; set; }

        public string News_Sum { get; set; }

        public string Is_Headline_Par { get; set; }

        public int Impt_Par { get; set; }

        public string Is_Main_News { get; set; }

        public string Is_Issue { get; set; }

        public string Real_Source_Name { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Pic_URL { get; set; }

        /// <summary>
        /// 浏览数
        /// </summary>
        public long View_Count { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public long Comment_Count { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public long Praise_Count { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        public long Collect_Count { get; set; }

        public string Txt_Content { get; set; }

        public string Html_Txt { get; set; }

        /// <summary>
        /// 是否点赞
        /// </summary>
        public int Is_Praise { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public int Is_Collect { get; set; }

        public string NEWS_TYPE_NAME2 { get; set; }

        public NewsTypeEntity NewsType { get; set; }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
