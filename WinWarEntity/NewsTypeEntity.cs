using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    public class NewsTypeEntity
    {
        public int ID { get; set; }

        public int News_Type_1 { get; set; }

        public string News_Type_Name1 { get; set; }

        public int News_Type_2 { get; set; }

        public string News_Type_Name2 { get; set; }

        public int News_Type_3 { get; set; }

        public string News_Type_Name3 { get; set; }
    }

    /// <summary>
    /// 新闻分类（加载）
    /// </summary>
    public class NewsTypeMenu
    {
        public int News_Type { get; set; }

        public string News_Type_Name { get; set; }

        public int P_News_Type { get; set; }

        public NewsTypeMenu ChildTypes { get; set; }
    }
}
