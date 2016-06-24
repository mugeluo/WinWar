using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarEntity
{
    [Serializable]
    public class Passport
    {
		private int _autoid;
		private string _userid;
		private string _loginname;
		private string _loginpwd;
		private string _name="";
		private string _email="";
		private string _mobilephone="";
		private string _officephone="";
		private string _citycode="";
		private string _address="";
		private DateTime _birthday;
		private int? _age=0;
		private int? _sex=0;
		private int? _education=0;
		private string _jobs="";
		private string _avatar="";
		private string _parentid;
		private int? _allocation=0;
		private int? _status=0;
		private string _description="";
		private DateTime _effecttime;
		private DateTime _turnovertime;
		private DateTime _createtime= DateTime.Now;
		private string _createuserid;


        public List<Menu> Menus { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int AutoID
		{
			set{ _autoid=value;}
			get{return _autoid;}
		}
		/// <summary>
		/// 
		/// </summary>
        [Property("Lower")]
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoginName
		{
			set{ _loginname=value;}
			get{return _loginname;}
		}

        /// <summary>
        /// 
        /// </summary>
        public string BindMobilePhone
        {
            set;
            get;
        }

		/// <summary>
		/// 
		/// </summary>
		public string LoginPWD
		{
			set{ _loginpwd=value;}
			get{return _loginpwd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MobilePhone
		{
			set{ _mobilephone=value;}
			get{return _mobilephone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OfficePhone
		{
			set{ _officephone=value;}
			get{return _officephone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CityCode
		{
			set{ _citycode=value;}
			get{return _citycode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Birthday
		{
			set{ _birthday=value;}
			get{return _birthday;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Age
		{
			set{ _age=value;}
			get{return _age;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Education
		{
			set{ _education=value;}
			get{return _education;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Jobs
		{
			set{ _jobs=value;}
			get{return _jobs;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Avatar
		{
			set{ _avatar=value;}
			get{return _avatar;}
		}
        /// <summary>
        /// 
        /// </summary>
        [Property("Lower")] 
		public string ParentID
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Allocation
		{
			set{ _allocation=value;}
			get{return _allocation;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EffectTime
		{
			set{ _effecttime=value;}
			get{return _effecttime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime TurnoverTime
		{
			set{ _turnovertime=value;}
			get{return _turnovertime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
        /// <summary>
        /// 
        /// </summary>
        [Property("Lower")] 
		public string CreateUserID
		{
			set{ _createuserid=value;}
			get{return _createuserid;}
		}


        [Property("Lower")]
        public string TeamID { get; set; }


        [Property("Lower")] 
        public string DepartID { get; set; }

        public string BindWeiXinID { get; set; }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }

}
