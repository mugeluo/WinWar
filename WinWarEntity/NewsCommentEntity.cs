using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    public class NewsCommentEntity
    {
        public int ID { get; set; }

        public int User_ID { get; set; }

        public string User_Name { get; set; }

        public int News_Uni_Code { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        public int Praise_Count { get; set; }

        public int Reply_Count { get; set; }

        public int Reply_ID { get; set; }

        public int Reply_User_ID { get; set; }

        public string Reply_User_Name { get; set; }

        public DateTime Create_Date { get; set; }

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
