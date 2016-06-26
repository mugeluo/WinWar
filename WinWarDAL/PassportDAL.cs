using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarDAL
{
    public class PassportDAL:BaseDAL
    {
        public static PassportDAL BaseDAL = new PassportDAL();

        public bool BindWeiXinID(string name, string avatar, string weiXinID)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Avatar",avatar),
                                        new SqlParameter("@BindWeiXinID",weiXinID)
                                   };

            return ExecuteNonQuery(" if(not exists(Select * from passport where BindWeiXinID=@BindWeiXinID and status<>9)) insert into passport(Name,Avatar,BindWeiXinID) values(@Name,@Avatar,@BindWeiXinID) ", paras, CommandType.Text) > 0;
        }

        public DataTable GetPassportByWeiXinID(string weiXinID)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@WeiXinID",weiXinID),
                                   };

            return GetDataTable("Select * from passport where BindWeiXinID=@WeiXinID and status<>9 ", paras, CommandType.Text);
        }

        public DataTable GetPassportByID(long userID)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserID",userID),
                                   };

            return GetDataTable("Select * from passport where UserID=@UserID and status<>9 ", paras, CommandType.Text);
        }


    }
}
