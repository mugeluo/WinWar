using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarEntity;
using WinWarDAL;

namespace WinWarBusiness
{
    public class NewsBusiness
    {
        public static NewsBusiness BaseBusiness = new NewsBusiness();
        #region Cache

        private static List<NewsTypeEntity> _types { get; set; }

        private static List<NewsTypeEntity> CacheNewsType 
        {
            get 
            {
                if (_types == null)
                {
                    _types = new List<NewsTypeEntity>();
                    DataTable dt = NewsDAL.BaseDAL.GetNewsTypes();
                    foreach (DataRow dr in dt.Rows)
                    {
                        NewsTypeEntity model = new NewsTypeEntity();
                        model.FillData(dr);
                        _types.Add(model);
                    }
                }
                return _types;
            }
            set
            {
                _types = value;
            }
        }

        #endregion


        #region 查询

        public NewsTypeEntity GetNewsTypeByCode(int code)
        {
            if (CacheNewsType.Where(m => m.Cls_Code == code).Count() > 0)
            {
                return CacheNewsType.Where(m => m.Cls_Code == code).FirstOrDefault();
            }

            NewsTypeEntity model = new NewsTypeEntity();
            DataTable dt = NewsDAL.BaseDAL.GetNewsTypeByID(code);
            if ( dt.Rows.Count>0)
            {
                model.FillData(dt.Rows[0]);
                CacheNewsType.Add(model);
            }
            return model;
        }

        public List<NewsTypeEntity> GetNewsTypeByParentID(int id)
        {
            if (id > 0)
            {
                return CacheNewsType.FindAll(m => m.News_Type_1 == id && m.News_Type_2 > 0);
            }
            else
            {
                return CacheNewsType.FindAll(m => m.News_Type_2 <= 0);
            }
        }
        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <param name="typeid">新闻类型</param>
        /// <param name="pageSize">每页新闻数</param>
        /// <param name="newsCode">最大新闻Code,第一页传 0</param>
        /// <returns></returns>
        public List<NewsEntity> GetNews(string keyWords, int typeid, int pageSize, long userid, ref long newsCode)
        {
            List<NewsEntity> list = new List<NewsEntity>();

            DataTable dt = NewsDAL.BaseDAL.GetNews(keyWords, typeid, pageSize, userid, ref newsCode);
            foreach (DataRow dr in dt.Rows)
            {
                NewsEntity model = new NewsEntity();
                model.FillData(dr);

                list.Add(model);
            }

            return list;
        }

        public List<NewsEntity> GetNewsFavorites(long userid, int pageSize, ref long favoriteid)
        {
            List<NewsEntity> list = new List<NewsEntity>();

            DataTable dt = NewsDAL.BaseDAL.GetNewsFavorites(userid, pageSize, ref favoriteid);
            foreach (DataRow dr in dt.Rows)
            {
                NewsEntity model = new NewsEntity();
                model.FillData(dr);

                list.Add(model);
            }

            return list;
        }

        public NewsEntity GetNewsDetail(long newsCode, long userid)
        {
            NewsEntity item =null;

            DataTable dt = NewsDAL.BaseDAL.GetNewsDetail(newsCode, userid);
            if(dt.Rows.Count>0)
            {
                item = new NewsEntity();
                item.FillData(dt.Rows[0]);
                item.NewsType = GetNewsTypeByCode(item.News_Type);
            }

            return item;
        }
        /// <summary>
        /// 获取新闻评论
        /// </summary>
        /// <param name="newsCode"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<NewsCommentEntity> GetNewsComments(long newsCode, int pageSize, long userid, ref long id)
        {
            List<NewsCommentEntity> list = new List<NewsCommentEntity>();
            DataTable dt = NewsDAL.BaseDAL.GetNewsComments(newsCode, pageSize, userid, ref id);
            foreach (DataRow dr in dt.Rows)
            {
                NewsCommentEntity model = new NewsCommentEntity();
                model.FillData(dr);
                model.CreateUser = PassportBusiness.GetPassportByID(model.User_ID);
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region 添加

        /// <summary>
        /// 增加新闻浏览数
        /// </summary>
        /// <param name="newsCode"></param>
        /// <returns></returns>
        public bool AddNewsViewCount(int newsCode)
        {
            return NewsDAL.BaseDAL.AddNewsViewCount(newsCode);
        }

        /// <summary>
        /// 点赞新闻
        /// </summary>
        /// <param name="newsCode">新闻编码</param>
        /// <param name="isAdd">true 点赞；false 取消点赞</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public bool AddNewsPraiseCount(long newsCode, bool isAdd, long userid)
        {
            return NewsDAL.BaseDAL.AddNewsPraiseCount(newsCode, isAdd, userid);
        }

        /// <summary>
        /// 收藏新闻
        /// </summary>
        /// <param name="newsCode">新闻编码</param>
        /// <param name="isAdd">true 点赞；false 取消点赞</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public bool AddNewsCollectCount(long newsCode, bool isAdd, long userid)
        {
            return NewsDAL.BaseDAL.AddNewsCollectCount(newsCode, isAdd, userid);
        }

        /// <summary>
        /// 评论或回复
        /// </summary>
        /// <param name="content"></param>
        /// <param name="newsCode"></param>
        /// <param name="userid"></param>
        /// <param name="userName"></param>
        /// <param name="replyid"></param>
        /// <param name="replyUserID"></param>
        /// <param name="replyUserName"></param>
        /// <returns></returns>
        public bool AddNewsComment(string content, long newsCode, long userid, string userName, long replyid, long replyUserID, string replyUserName)
        {
            return NewsDAL.BaseDAL.AddNewsComment(content, newsCode, userid,userName,replyid,replyUserID,replyUserName);
        }

        #endregion

        #region manage
        public List<NewsEntity> GetNews(string keyWords, int bigTypeID, int typeid, int publishStatus, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            List<NewsEntity> list = new List<NewsEntity>();

            DataTable dt = NewsDAL.BaseDAL.GetNews(keyWords, bigTypeID, typeid, publishStatus, pageSize, pageIndex, ref totalCount, ref pageCount);
            foreach (DataRow dr in dt.Rows)
            {
                NewsEntity model = new NewsEntity();
                model.FillData(dr);
                if (model.News_Type > 0) 
                {
                    model.NewsType = GetNewsTypeByCode(model.News_Type);
                }
                list.Add(model);
            }

            return list;
        }

        public bool AddNews(NewsEntity news)
        {
            return NewsDAL.BaseDAL.AddNews(news.News_Uni_Code, news.Title_Main, news.Title_Sub, news.Title_App, news.News_Sum,
                news.News_Author, news.Real_Source_Name, news.Nega_Posi_Par, news.Impt_Par,news.Is_Issue,
                news.News_Type, news.Html_Txt);
        }

        public bool EditNews(NewsEntity news)
        {
            return NewsDAL.BaseDAL.EditNews(news.News_Uni_Code, news.Title_Main, news.Title_Sub, news.Title_App, news.News_Sum,
                news.News_Author, news.Real_Source_Name, news.Nega_Posi_Par, news.Impt_Par, news.Is_Issue,
                news.News_Type, news.Html_Txt);
        }

        public bool PublishNews(long newsCode, int isPublish)
        {
            return NewsDAL.BaseDAL.PublishNews(newsCode, isPublish);
        }
        #endregion

    }
}
