using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sci.Production.Class
{
    /// <summary>
    /// Combo FtyZone
    /// </summary>
    public partial class ComboFtyZone : Win.UI.ComboBox
    {
        /// <summary>
        /// Is support Junk
        /// </summary>
        public bool IssupportJunk { get; set; } = false;

        /// <summary>
        /// Filte MDivision
        /// </summary>
        public bool FilteMDivision { get; set; } = false;

        /// <summary>
        /// Select Table
        /// </summary>
        public string SelectTable { get; set; } = "Factory";

        /// <summary>
        /// Is Include Sample Room
        /// </summary>
        public bool IsIncludeSampleRoom { get; set; } = false;

        /// <summary>
        /// Is Filter Factory.IsProduceFty = 1
        /// </summary>
        public bool IsProduceFty { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboFtyZone"/> class.
        /// </summary>
        public ComboFtyZone()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// ComboFtyZone
        /// </summary>
        /// <param name="container">container</param>
        public ComboFtyZone(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        /// <param name="strMDivisionID">如果沒輸入，MDivision 預設 Sci.Env.User.Keywordd</param>
        public void SetDataSource(string strMDivisionID = null)
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
            listSqlPar.Add(new SqlParameter("@MDivision", strMDivisionID.Empty() ? Env.User.Keyword : strMDivisionID));

            #region SQL Filte
            List<string> listFilte = new List<string>();
            if (this.IssupportJunk)
            {
                listFilte.Add("Junk = 0 ");
            }

            if (!this.IsIncludeSampleRoom && this.SelectTable == "Factory")
            {
                listFilte.Add("IsSampleRoom = 0 ");
            }

            if (this.IsProduceFty && this.SelectTable == "Factory")
            {
                listFilte.Add("IsProduceFty = 1");
            }

            if (this.FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision ");
            }

            listFilte.Add("isnull(FtyZone,'') <> '' ");
            #endregion
            string sqlcmd = string.Format(
                @"
select *
from (
    Select FtyZone = ''

    union all
    Select DISTINCT FtyZone
    from {1} WITH (NOLOCK)
    {0}
) a
order by FtyZone",
                (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\r and ") : string.Empty,
                this.SelectTable);

            return sqlcmd;
        }

        /// <summary>
        /// Set Data Source AllFty
        /// </summary>
        /// <returns>DualResult</returns>
        public DualResult SetDataSourceAllFty()
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
