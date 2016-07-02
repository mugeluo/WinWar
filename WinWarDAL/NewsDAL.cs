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
            return GetDataTable("Select * from NEWS_TYPE order by Cls_Code");
        }

        public DataTable GetNewsTypeByID(int code)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@Cls_Code",code),
                                   };
            return GetDataTable("Select * from NEWS_TYPE where Cls_Code=@Cls_Code", paras, CommandType.Text);
        }

        public DataTable GetNews(string keyWords, int typeid, int pageSize, long userid, ref long newsCode)
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
            if (paras[0].Value != DBNull.Value)
            {
                newsCode = Convert.ToInt64(paras[0].Value);
            }
            return dt;

        }

        public DataTable GetNewsFavorites(long userid, int pageSize, ref long favoriteid)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@FavoriteID",SqlDbType.BigInt),
                                       new SqlParameter("@PageSize",pageSize),
                                       new SqlParameter("@UserID",userid)
                                       
                                   };
            paras[0].Value = favoriteid;
            paras[0].Direction = ParameterDirection.InputOutput;
            DataTable dt = GetDataTable("P_GetNEWS_Favorites", paras, CommandType.StoredProcedure);
            if (paras[0].Value != DBNull.Value)
            {
                favoriteid = Convert.ToInt64(paras[0].Value);
            }

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

        public DataTable GetNewsComments(long newsCode, int pageSize, long userid, ref long id)
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
            if (paras[0].Value != DBNull.Value)
            {
                id = Convert.ToInt64(paras[0].Value);
            }
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

        public bool AddNewsPraiseCount(long newsCode, bool isAdd, long userid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NewsCode",newsCode),
                                     new SqlParameter("@IsAdd",isAdd ? 1 : 0),
                                     new SqlParameter("@UserID",userid)
                                   };
            return ExecuteNonQuery("P_AddNewsPraiseCount", paras, CommandType.StoredProcedure) > 0;
        }

        public bool AddNewsCollectCount(long newsCode, bool isAdd, long userid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NewsCode",newsCode),
                                     new SqlParameter("@IsAdd",isAdd ? 1 : 0),
                                     new SqlParameter("@UserID",userid)
                                   };
            return ExecuteNonQuery("P_AddNewsCollectCount", paras, CommandType.StoredProcedure) > 0;
        }

        public bool AddNewsComment(string content, long newsCode, long userid, string userName, long replyid, long replyUserID, string replyUserName)
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

        public DataTable GetNews(string keyWords, int bigTypeID, int typeid, int publishStatus, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@TotalCount",SqlDbType.Int),
                                       new SqlParameter("@PageCount",SqlDbType.Int),
                                       new SqlParameter("@KeyWords",keyWords),
                                       new SqlParameter("@PageSize",pageSize),
                                       new SqlParameter("@PageIndex",pageIndex),
                                       new SqlParameter("@PublishStatus",publishStatus),
                                       new SqlParameter("@TypeID",typeid),
                                       new SqlParameter("@BigTypeID",bigTypeID)
                                       
                                   };
            paras[0].Value = totalCount;
            paras[1].Value = pageCount;
            paras[0].Direction = ParameterDirection.InputOutput;
            paras[1].Direction = ParameterDirection.InputOutput;
            DataTable dt = GetDataTable("M_GetNews_Mains", paras, CommandType.StoredProcedure);

            totalCount = Convert.ToInt32(paras[0].Value);
            pageCount = Convert.ToInt32(paras[1].Value);
            return dt;

        }

        public bool AddNews(long id,string title, string titleSub,string titleApp,
            string newsSum, string author, string source, string picUrl,
            int posiPar,int important,string isIssue,
            int typeid,string content)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@ID",id),
                                     new SqlParameter("@Title",title),
                                     new SqlParameter("@TitleSub",titleSub),
                                     new SqlParameter("@TitleApp",titleApp),
                                     new SqlParameter("@NewsSum",newsSum),
                                     new SqlParameter("@Author",author),
                                     new SqlParameter("@Source",source),
                                     new SqlParameter("@PicUrl",picUrl),
                                     new SqlParameter("@PosiPar",posiPar),
                                     new SqlParameter("@Important",important),
                                     new SqlParameter("@IsIssue",isIssue),
                                     new SqlParameter("@TypeID",typeid),
                                     new SqlParameter("@Content",content)
                                   };
            return ExecuteNonQuery("M_AddNews", paras, CommandType.StoredProcedure) > 0;
        }

        public bool EditNews(long id, string title, string titleSub, string titleApp,
            string newsSum, string author, string source,string picUrl,
            int posiPar, int important, string isIssue,
            int typeid, string content)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@ID",id),
                                     new SqlParameter("@Title",title),
                                     new SqlParameter("@TitleSub",titleSub),
                                     new SqlParameter("@TitleApp",titleApp),
                                     new SqlParameter("@NewsSum",newsSum),
                                     new SqlParameter("@Author",author),
                                     new SqlParameter("@Source",source),
                                     new SqlParameter("@PicUrl",picUrl),
                                     new SqlParameter("@PosiPar",posiPar),
                                     new SqlParameter("@Important",important),
                                     new SqlParameter("@IsIssue",isIssue),
                                     new SqlParameter("@TypeID",typeid),
                                     new SqlParameter("@Content",content)
                                   };
            return ExecuteNonQuery("M_EditNews", paras, CommandType.StoredProcedure) > 0;
        }

        public bool PublishNews(long newsCode, int isPublish)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@NewsCode",newsCode),
                                     new SqlParameter("@IsPublish",isPublish)
                                   };
            return ExecuteNonQuery("M_PublishNews", paras, CommandType.StoredProcedure) > 0;
        }

    }
}
