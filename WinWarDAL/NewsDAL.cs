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

        public DataTable GetNews(string keyWords, int typeid, int pageSize, int userid, ref long newsCode)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@NewsCode",SqlDbType.BigInt),
                                       new SqlParameter("@KeyWords",keyWords),
                                       new SqlParameter("@PageSize",pageSize),
                                       new SqlParameter("@TypeID",typeid),
                                       new SqlParameter("@UserID",userid)
                                       
                                   };
            paras[0].Value = newsCode;
            paras[0].Direction = ParameterDirection.InputOutput;
            DataTable dt = GetDataTable("P_GetNews_Mains", paras, CommandType.StoredProcedure);
            newsCode = Convert.ToInt64(paras[0].Value);
            return dt;

        }

        public DataTable GetNewsDetail(long newsCode, long userid)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@NewsCode",newsCode),
                                       new SqlParameter("@UserID",userid)
                                   };

            DataTable dt = GetDataTable("P_GetNewsDetail", paras, CommandType.StoredProcedure);

            return dt;

        }

        public DataTable GetNewsComments(long newsCode, int pageSize, int userid, ref long id)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@ID",SqlDbType.BigInt),
                                       new SqlParameter("@NewsCode",newsCode),
                                       new SqlParameter("@PageSize",pageSize),
                                       new SqlParameter("@UserID",userid)
                                   };
            paras[0].Value = id;
            paras[0].Direction = ParameterDirection.InputOutput;
            DataTable dt = GetDataTable("P_GetNewsComments", paras, CommandType.StoredProcedure);
            id = Convert.ToInt64(paras[0].Value);
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

        public bool AddNewsPraiseCount(long newsCode, bool isAdd, int userid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NewsCode",newsCode),
                                     new SqlParameter("@IsAdd",isAdd ? 1 : 0),
                                     new SqlParameter("@UserID",userid)
                                   };
            return ExecuteNonQuery("P_AddNewsPraiseCount", paras, CommandType.StoredProcedure) > 0;
        }

        public bool AddNewsCollectCount(long newsCode, bool isAdd, int userid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NewsCode",newsCode),
                                     new SqlParameter("@IsAdd",isAdd ? 1 : 0),
                                     new SqlParameter("@UserID",userid)
                                   };
            return ExecuteNonQuery("P_AddNewsCollectCount", paras, CommandType.StoredProcedure) > 0;
        }

        public bool AddNewsComment(string content, long newsCode, int userid, string userName, int replyid, int replyUserID, string replyUserName)
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
