using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sci.Production.Class
{
    public partial class comboFtyZone : Sci.Win.UI.ComboBox
    {
        private bool _IssupportJunk = false;
        private bool _FilteMDivision = false;
        private string _SelectTable = "Factory";
        private bool _isIncludeSampleRoom = false;

        public bool IssupportJunk
        {
            get { return _IssupportJunk; }
            set { _IssupportJunk = value; }
        }
        public bool FilteMDivision
        {
            get { return _FilteMDivision; }
            set { _FilteMDivision = value; }
        }
        public string SelectTable
        {
            get { return _SelectTable; }
            set { _SelectTable = value; }
        }

        public bool IsIncludeSampleRoom
        {
            get { return _isIncludeSampleRoom; }
            set { _isIncludeSampleRoom = value; }
        }

        public comboFtyZone()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);            
        }

        public comboFtyZone(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);            
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        /// <param name="strMDivisionID">如果沒輸入，MDivision 預設 Sci.Env.User.Keywordd</param>
        public void setDataSource(string strMDivisionID = null)
        {
            DataTable dtFactoryData;
            DualResult result;
            List<SqlParameter> listSqlPar = new List<SqlParameter>();

            string sqlcmd = this.GetFtySQL(strMDivisionID, out listSqlPar);

            result = DBProxy.Current.Select(null, sqlcmd, listSqlPar, out dtFactoryData);
            if (result && dtFactoryData != null)
            {
                this.DataSource = dtFactoryData;
                this.ValueMember = "FtyZone";
                this.DisplayMember = "FtyZone";
            }
        }

        private string GetFtySQL(string strMDivisionID, out List<SqlParameter> listSqlPar)
        {
            listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", strMDivisionID.Empty() ? Sci.Env.User.Keyword : strMDivisionID));

            #region SQL Filte
            List<string> listFilte = new List<string>();
            if (this._IssupportJunk)
            {
                listFilte.Add("Junk = 0 ");
            }

            if (!this._isIncludeSampleRoom && this._SelectTable == "Factory")
            {
                listFilte.Add("IsSampleRoom = 0 ");
            }

            if (this._FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision ");
            }
            listFilte.Add("isnull(FtyZone,'') <> '' ");
            #endregion
            string sqlcmd = string.Format(@"
select *
from (
    Select FtyZone = ''

    union all
    Select DISTINCT FtyZone
    from {1} WITH (NOLOCK)
    {0}
) a
order by FtyZone",
            (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\r and ") : "",
            _SelectTable);

            return sqlcmd;
        }

        public DualResult setDataSourceAllFty()
        {
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' }).Where(s => !s.Contains("testing_PMS")).ToArray();
            List<string> connectionString = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                if (!MyUtility.Check.Empty(ss))
                {
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                    connectionString.Add(connections);
                }
            }
            DataTable dtFtyZone = null;
            foreach (string conString in connectionString)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    List<SqlParameter> listSqlPar = new List<SqlParameter>();
                    string sqlcmd = this.GetFtySQL(string.Empty, out listSqlPar);
                    DataTable dtResult;
                    DualResult result = DBProxy.Current.SelectByConn(conn, sqlcmd, null, out dtResult);
                    if (!result)
                    {
                        return result;
                    }

                    if (dtFtyZone == null)
                    {
                        dtFtyZone = dtResult.Clone();
                        dtFtyZone.Merge(dtResult);
                    }
                    else
                    {
                        dtFtyZone.Merge(dtResult);
                    }
                }
            }

            
            List<string> dicFtyZone = dtFtyZone.AsEnumerable().Select(s => s["FtyZone"].ToString()).Distinct().ToList();
            this.DataSource = dicFtyZone;

            return new DualResult(true);
        }
    }
}
