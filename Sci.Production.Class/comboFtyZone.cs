using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Class
{
    public partial class comboFtyZone : Sci.Win.UI.ComboBox
    {
        private bool _IssupportJunk = false;
        private bool _FilteMDivision = false;
        private string _SelectTable = "Factory";

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
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", strMDivisionID.Empty() ? Sci.Env.User.Keyword : strMDivisionID));
            #endregion
            #region SQL Filte
            List<string> listFilte = new List<string>();
            if (this._IssupportJunk)
            {
                listFilte.Add("Junk = 0 ");
            }
            if (this._FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision ");
            }
            listFilte.Add("isnull(FtyZone,'') <> '' ");
            #endregion
            #region SQL CMD
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
            (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : "",
            _SelectTable);

            #endregion
            result = DBProxy.Current.Select(null, sqlcmd, listSqlPar, out dtFactoryData);
            if (result && dtFactoryData != null)
            {
                this.DataSource = dtFactoryData;
                this.ValueMember = "FtyZone";
                this.DisplayMember = "FtyZone";
            }
        }
    }
}
