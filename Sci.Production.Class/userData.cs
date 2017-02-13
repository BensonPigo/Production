using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Win;

namespace Sci.Production.Class
{
    public partial class userData : Form
    {
        private string sql;
        private List<SqlParameter> sqlPar;
        private DataTable dt;
        public string errMsg { set; get; }

        public userData(string sql, List<SqlParameter> sqlPar)
        {
            errMsg = null;
            this.sql = sql;
            this.sqlPar = sqlPar;
            InitializeComponent();

            DualResult result;
            if (result = DBProxy.Current.Select(null, sql, sqlPar, out dt))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    displayBox1.Text = (dr["id"] != null) ? dr["id"].ToString() : "";
                    displayBox2.Text = (dr["name"] != null) ? dr["name"].ToString() : "";
                    displayBox3.Text = (dr["ext"] != null) ? dr["ext"].ToString() : "";
                    displayBox4.Text = (dr["mail"] != null) ? dr["mail"].ToString() : "";
                }
                else
                {
                    errMsg = "Data not found!";
                }
            }
            else
            {
                errMsg = "Sql connection fail!\r\n" + result.ToString();
            }
        }
    }
}
