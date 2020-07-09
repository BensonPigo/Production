using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// UserData
    /// </summary>
    public partial class UserData : Form
    {
        private string sql;
        private List<SqlParameter> sqlPar;
        private DataTable dt;

        /// <summary>
        /// Err Msg
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        /// <param name="sql">sqlCmd</param>
        /// <param name="sqlPar">SqlParameter</param>
        public UserData(string sql, List<SqlParameter> sqlPar)
        {
            this.ErrMsg = null;
            this.sql = sql;
            this.sqlPar = sqlPar;
            this.InitializeComponent();

            DualResult result;
            if (result = DBProxy.Current.Select(null, sql, sqlPar, out this.dt))
            {
                if (this.dt != null && this.dt.Rows.Count > 0)
                {
                    DataRow dr = this.dt.Rows[0];
                    this.displayBox1.Text = (dr["id"] != null) ? dr["id"].ToString() : string.Empty;
                    this.displayBox2.Text = (dr["name"] != null) ? dr["name"].ToString() : string.Empty;
                    this.displayBox3.Text = (dr["ext"] != null) ? dr["ext"].ToString() : string.Empty;
                    this.displayBox4.Text = (dr["mail"] != null) ? dr["mail"].ToString() : string.Empty;
                }
                else
                {
                    this.ErrMsg = "Data not found!";
                }
            }
            else
            {
                this.ErrMsg = "Sql connection fail!\r\n" + result.ToString();
            }
        }
    }
}
