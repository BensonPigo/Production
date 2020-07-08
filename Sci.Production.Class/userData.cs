using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;

namespace Sci.Production.Class
{
    public partial class userData : Form
    {
        private string sql;
        private List<SqlParameter> sqlPar;
        private DataTable dt;

        public string errMsg { get; set; }

        public userData(string sql, List<SqlParameter> sqlPar)
        {
            this.errMsg = null;
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
                    this.errMsg = "Data not found!";
                }
            }
            else
            {
                this.errMsg = "Sql connection fail!\r\n" + result.ToString();
            }
        }
    }
}
