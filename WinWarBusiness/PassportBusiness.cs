using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarDAL;
using WinWarEntity;

namespace WinWarBusiness
{
    public class PassportBusiness
    {
        public static bool BindWeiXinID(string name, string avatar, string weiXinID)
        {
           return PassportDAL.BaseDAL.BindWeiXinID(name, avatar,weiXinID);
        }

        public static Passport GetPassportByWeiXinID(string weiXinID)
        {
            Passport item = new Passport();
            DataTable dt = PassportDAL.BaseDAL.GetPassportByWeiXinID(weiXinID);

            if (dt.Rows.Count > 0) {
                item.FillData(dt.Rows[0]);
            }

            return item;
        }
    }
}
