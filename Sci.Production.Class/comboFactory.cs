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
    public partial class comboFactory : Sci.Win.UI.ComboBox
    {
        private bool _IssupportJunk = false;
        private bool _FilteMDivision = false;

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

        public comboFactory()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);            
        }

        public comboFactory(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);            
        }

        public void setDataSource()
        {
            DataTable dtFactoryData;
            DualResult result;
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
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
            string sqlcmd = string.Format(@"
select *
from (
    Select Factory = ''

    union all
    Select DISTINCT Factory = FtyGroup 
    from Factory WITH (NOLOCK) 
    {0}
) a
order by Factory", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : "");
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
