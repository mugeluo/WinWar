using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    public class NewsCommentEntity
    {
        public long ID { get; set; }

        public long User_ID { get; set; }

        public string User_Name { get; set; }

        public long News_Uni_Code { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        public long Praise_Count { get; set; }

        public long Reply_Count { get; set; }

        public long Reply_ID { get; set; }

        public long Reply_User_ID { get; set; }

        public string Reply_User_Name { get; set; }

        public DateTime Create_Date { get; set; }

        /// <summary>
        /// 大于0都等于点赞
        /// </summary>
        public long Is_Praise { get; set; }

        public Passport CreateUser { get; set; }

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
