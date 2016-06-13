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

        public NewsTypeEntity GetNewsTypeByCode(int id)
        {
            if (CacheNewsType.Where(m => m.ID == id).Count() > 0)
            {
                CacheNewsType.Where(m => m.ID == id).FirstOrDefault();
            }
            NewsTypeEntity model = new NewsTypeEntity();
            DataTable dt = NewsDAL.BaseDAL.GetNewsTypeByID(id);
            if ( dt.Rows.Count>0)
            {
                model.FillData(dt.Rows[0]);
                CacheNewsType.Add(model);
            }
            return model;
        }

        public List<NewsTypeEntity> GetNewsTypeByParentID(int id)
        {
            return CacheNewsType.FindAll(m => m.News_Type_1 == id && m.News_Type_2 != -1);
        }
        /// <summary>
        /// 获取新闻
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <param name="typeid">新闻类型</param>
        /// <param name="pageSize">每页新闻数</param>
        /// <param name="newsCode">最大新闻Code,第一页传 0</param>
        /// <returns></returns>
        public List<NewsEntity> GetNews(string keyWords, int typeid, int pageSize, int userid, ref long newsCode)
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
        public List<NewsCommentEntity> GetNewsComments(long newsCode, int pageSize, int userid, ref long id)
        {
            List<NewsCommentEntity> list = new List<NewsCommentEntity>();
            DataTable dt = NewsDAL.BaseDAL.GetNewsComments(newsCode, pageSize, userid, ref id);
            foreach (DataRow dr in dt.Rows)
            {
                NewsCommentEntity model = new NewsCommentEntity();
                model.FillData(dr);

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
        public bool AddNewsPraiseCount(long newsCode, bool isAdd, int userid)
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
        public bool AddNewsCollectCount(long newsCode, bool isAdd, int userid)
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
        public bool AddNewsComment(string content, long newsCode, int userid, string userName, long replyid, long replyUserID, string replyUserName)
        {
            return NewsDAL.BaseDAL.AddNewsComment(content, newsCode, userid,userName,replyid,replyUserID,replyUserName);
        }

        #endregion

        #region manage
        public List<NewsEntity> GetNews(string keyWords, int typeid,
            int pageSize, ref long newsCode)
        {
            List<NewsEntity> list = new List<NewsEntity>();

            DataTable dt = NewsDAL.BaseDAL.GetNews(keyWords, typeid, 
                pageSize, ref newsCode);
            foreach (DataRow dr in dt.Rows)
            {
                NewsEntity model = new NewsEntity();
                model.FillData(dr);

                list.Add(model);
            }

            return list;
        }
        #endregion

    }
}
