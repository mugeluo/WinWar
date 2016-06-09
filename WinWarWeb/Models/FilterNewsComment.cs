using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinWarWeb.Models
{
    public class FilterNewsComment
    {
        public long ID { get; set; }

        public long News_Uni_Code { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

        public long Reply_Count { get; set; }

        public long Reply_ID { get; set; }

        public long Reply_User_ID { get; set; }

        public string Reply_User_Name { get; set; }

        public DateTime Create_Date { get; set; }
    }
}