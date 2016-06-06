using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarDAL
{
    public class NewsDAL : BaseDAL
    {
        public static NewsDAL BaseDAL = new NewsDAL();

        #region 查询

        public DataTable GetNewsTypes()
        {
            return GetDataTable("Select * from NEWS_TYPE");
        }

        public DataTable GetNewsTypeByID(int id)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@ID",id),
                                   };
            return GetDataTable("Select * from NEWS_TYPE where ID=@ID", paras, CommandType.Text);
        }

        public DataTable GetNews(string keyWords, int typeid, int pageSize, int userid, ref int newsCode)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@NewsCode",SqlDbType.Int),
                                       new SqlParameter("@KeyWords",keyWords),
                                       new SqlParameter("@PageSize",pageSize),
                                       new SqlParameter("@TypeID",typeid),
                                       new SqlParameter("@UserID",userid)
                                       
                                   };
            paras[0].Value = newsCode;
            paras[0].Direction = ParameterDirection.InputOutput;
            DataTable dt = GetDataTable("P_GetNews_Mains", paras, CommandType.StoredProcedure);
            newsCode = Convert.ToInt32(paras[0].Value);
            return dt;

        }

        #endregion

        #region 查询

        public bool AddNewsViewCount(int newsCode)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NEWS_UNI_CODE",newsCode),
                                   };
            return ExecuteNonQuery("update NEWS_MAIN set View_Count=View_Count+1 where NEWS_UNI_CODE=@NEWS_UNI_CODE", paras, CommandType.Text) > 0;
        }

        public bool AddNewsPraiseCount(int newsCode, bool isAdd, int userid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NEWS_UNI_CODE",newsCode),
                                     new SqlParameter("@IsAdd",isAdd ? 1 : 0),
                                     new SqlParameter("@UserID",userid)
                                   };
            return ExecuteNonQuery("P_AddNewsPraiseCount", paras, CommandType.StoredProcedure) > 0;
        }

        public bool AddNewsCollectCount(int newsCode, bool isAdd, int userid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NEWS_UNI_CODE",newsCode),
                                     new SqlParameter("@IsAdd",isAdd ? 1 : 0),
                                     new SqlParameter("@UserID",userid)
                                   };
            return ExecuteNonQuery("P_AddNewsCollectCount", paras, CommandType.StoredProcedure) > 0;
        }

        public bool AddNewsComment(string content, int newsCode, int userid, string userName, int replyid, int replyUserID, string replyUserName)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NewsCode",newsCode),
                                     new SqlParameter("@Content",content),
                                     new SqlParameter("@UserID",userid),
                                     new SqlParameter("@UserName",userName),
                                     new SqlParameter("@ReplyID",replyid),
                                     new SqlParameter("@ReplyUserID",replyUserID),
                                     new SqlParameter("@ReplyUserName",replyUserName)
                                   };
            return ExecuteNonQuery("P_AddNewsComment", paras, CommandType.StoredProcedure) > 0;
        }

        #endregion
    }
}
