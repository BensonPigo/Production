using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// Combo Construction
    /// </summary>
    public partial class ComboConstruction : Win.UI.ComboBox
    {
        /// <summary>
        /// Style Gender
        /// </summary>
        public string StyleGender { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboConstruction"/> class.
        /// </summary>
        public ComboConstruction()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboConstruction"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboConstruction(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        public void SetDataSource()
        {
            DualResult result;
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>
            {
                new SqlParameter("@ID", this.StyleGender),
            };
            #endregion

            #region SQL Filte
            List<string> listFilte = new List<string>()
            {
                "d.type = 'StyleConstruction'",
            };

            if (!this.StyleGender.Empty())
            {
                listFilte.Add("ID = @ID");
            }
            #endregion

            #region SQL CMD
            string sqlcmd = string.Format(
                @"
select *
from (
    Select ID = '', Name = ''

    union all
    select distinct ID, Name 
    from DropDownList d WITH (NOLOCK)
    {0}
) a
order by ID, Name", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : string.Empty);
            #endregion
            result = DBProxy.Current.Select(null, sqlcmd, listSqlPar, out DataTable dt);
            if (result && dt != null)
            {
                this.DataSource = dt;
                this.ValueMember = "ID";
                this.DisplayMember = "Name";
            }
        }
    }
}
