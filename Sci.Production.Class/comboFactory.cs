using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class comboFactory : Sci.Win.UI.ComboBox
    {
        private bool _IssupportJunk = false;
        private bool _FilteMDivision = false;

        public bool IssupportJunk
        {
            get { return this._IssupportJunk; }
            set { this._IssupportJunk = value; }
        }

        public bool FilteMDivision
        {
            get { return this._FilteMDivision; }
            set { this._FilteMDivision = value; }
        }

        public comboFactory()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        public comboFactory(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
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
                listFilte.Add("Junk = 0");
            }

            if (this._FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision");
            }
            #endregion
            #region SQL CMD
            string sqlcmd = string.Format(
                @"
select *
from (
    Select Factory = ''

    union all
    Select DISTINCT Factory = FtyGroup 
    from Factory WITH (NOLOCK) 
    {0}
) a
order by Factory", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : string.Empty);
            #endregion
            result = DBProxy.Current.Select(null, sqlcmd, listSqlPar, out dtFactoryData);
            if (result && dtFactoryData != null)
            {
                this.DataSource = dtFactoryData;
                this.ValueMember = "Factory";
                this.DisplayMember = "Factory";
            }
        }
    }
}
