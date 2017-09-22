using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_Monitor_Board
{
    public class MonitorUser : Sci.IUserInfo
    {
        public string id { get; set; }
        public string AuthorityList { get { return ""; } }
        public string BrandList { get { return ""; } }
        public string Department { get { return ""; } }
        public string Director { get { return ""; } }
        public string Factory { get { return ""; } }
        public string FactoryList { get { return ""; } }
        public bool IsAdmin { get { return true; } }
        public bool IsMIS { get { return true; } }
        public bool IsTS { get { return false; } }
        public string Keyword { get { return ""; } }
        public string MailAddress { get { return ""; } }
        public string MemberList { get { return ""; } }
        public long PositionID { get { return 0; } }
        public string ProxyList { get { return ""; } }
        public string SpecialAuthorityList { get { return ""; } }
        public string UserID { get { return "MonitorUser"; } }
        public string UserName { get { return "MonitorUser"; } }
        public string UserPassword { get { return ""; } }

        public bool IsContainsAuthority(string id) { return false; }
        public bool IsContainsFactory(string id) { return false; }
        public bool IsContainsMember(string id) { return false; }
        public bool IsContainsProxy(string id) { return false; }
        public bool IsSameUser(string id) { return false; }
    }
}
