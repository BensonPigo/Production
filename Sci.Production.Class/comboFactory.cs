using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// Combo Factory
    /// </summary>
    public partial class ComboFactory : Win.UI.ComboBox
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
        /// Initializes a new instance of the <see cref="ComboFactory"/> class.
        /// </summary>
        public ComboFactory()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboFactory"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboFactory(IContainer container)
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
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", strMDivisionID.Empty() ? Env.User.Keyword : strMDivisionID));
            #endregion
            #region SQL Filte
            List<string> listFilte = new List<string>();
            if (this.IssupportJunk)
            {
                listFilte.Add("Junk = 0");
            }

            if (this.FilteMDivision)
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
