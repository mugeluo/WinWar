using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    public class NewsEntity
    {
        public int News_Uni_Code { get; set; }

        public DateTime Pub_Time { get; set; }

        public string News_Author { get; set; }

        public int Nega_Post_Par { get; set; }

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

        public string Pic_URL { get; set; }

        public int View_Count { get; set; }

        public int Comment_Count { get; set; }

        public int Praise_Count { get; set; }

        public int Collect_Count { get; set; }

        public string Txt_Content { get; set; }

        public string Html_Txt { get; set; }
    }
}
