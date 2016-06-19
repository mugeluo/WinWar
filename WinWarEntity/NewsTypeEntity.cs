using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    public class NewsTypeEntity
    {
        public int Cls_Code { get; set; }

        public int News_Type_1 { get; set; }

        public string News_Type_Name1 { get; set; }

        public int News_Type_2 { get; set; }

        public string News_Type_Name2 { get; set; }

        public int News_Type_3 { get; set; }

        public string News_Type_Name3 { get; set; }

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
