using System.Data;

namespace Sci.Production.Class.Commons
{
    /// <summary>
    /// User class
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// User Name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// User Ext_no
        /// </summary>
        public string Ext_No { get; set; } = string.Empty;

        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User Factory
        /// </summary>
        public string Factory { get; set; } = string.Empty;

        // public String email = "";
        // public String supervisor = "";
        // public String manager = "";
        // public String DepartmentID = "";
        // public String Country = "";
        // public String OrderGroup = "";
        // public String PWDWindows = "";
        // public String PWDSystem = "";
        // public String PWDEmail = "";
        // public DateTime? CreateDate = null;
        // public DateTime? Onboard = null;
        // public DateTime? Resign = null;
        // public bool Sample = false;
        // public bool Trade = false;
        // public bool Accounting = false;
        // public bool FromFactory = false;
        // public String Department = "";
        // public String Deputy = "";
        // public bool Kpi = false;
        // public bool TeamAvg = false;
        // public bool MultipleRank = false;
        // public String KpiRemark = "";
        // public String Duty = "";
        // public string CodePage = "";

        /// <summary>
        /// User
        /// </summary>
        /// <param name="row">DataRow</param>
        public User(DataRow row)
        {
            this.Id = row["ID"].ToString().TrimEnd();
            this.Name = row["name"].ToString().TrimEnd();
            this.Ext_No = row["Ext_no"].ToString().TrimEnd();
            this.Email = row["Email"].ToString().TrimEnd();
            this.Factory = row["Factory"].ToString().TrimEnd();

            // this.Country = row["Country"].ToString().TrimEnd();
            // this.OrderGroup = row["OrderGroup"].ToString().TrimEnd();
            // this.DepartmentID = row["DepartmentID"].ToString().TrimEnd();
            //  this.email = row["Email"].ToString().TrimEnd();
            // this.supervisor = row["Supervisor"].ToString().TrimEnd();
            // this.manager = row["Manager"].ToString().TrimEnd();
            // this.PWDWindows = row["OrderGroup"].ToString().TrimEnd();
            // this.PWDSystem = row["OrderGroup"].ToString().TrimEnd();
            // this.PWDEmail = row["OrderGroup"].ToString().TrimEnd();
            // this.CreateDate = SciConvert.GetDate(row["CreateDate"]);
            // this.Onboard = SciConvert.GetDate(row["Onboard"]);
            // this.Resign = SciConvert.GetDate(row["Resign"]);
            // this.Sample = Check.isTrue( row["Sample"]);
            // this.Trade = Check.isTrue( row["Trade"]);
            // this.Accounting = Check.isTrue( row["Accounting"]);
            // this.FromFactory = Check.isTrue( row["FromFactory"]);
            // this.Department = row["OrderGroup"].ToString().TrimEnd();
            // this.Deputy = row["OrderGroup"].ToString().TrimEnd();
            // this.Kpi = Check.isTrue( row["Kpi"]);
            // this.TeamAvg = Check.isTrue( row["TeamAvg"]);
            // this.MultipleRank = Check.isTrue( row["MultipleRank"]);
            // this.KpiRemark = row["OrderGroup"].ToString().TrimEnd();
            // this.Duty = row["Duty"].ToString().TrimEnd();
            // this.CodePage = row["CodePage"].ToString().TrimEnd();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }
    }
}
