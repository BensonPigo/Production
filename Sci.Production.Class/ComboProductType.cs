using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <summary>
    /// Combo Product Type
    /// </summary>
    public partial class ComboProductType : Win.UI.ComboBox
    {
        /// <summary>
        /// Is Junk
        /// </summary>
        public bool IsJunk { get; set; } = false;

        /// <summary>
        /// Style ApparelType
        /// </summary>
        public string StyleApparelType { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboProductType"/> class.
        /// </summary>
        public ComboProductType()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComboProductType"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public ComboProductType(IContainer container)
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
                new SqlParameter("@ID", this.StyleApparelType),
            };
            #endregion

            #region SQL Filte
            List<string> listFilte = new List<string>()
            {
                "r.ReasonTypeID = 'Style_Apparel_Type'",
            };

            if (!this.IsJunk)
            {
                listFilte.Add("Junk = 0");
            }

            if (!this.StyleApparelType.Empty())
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
    from Reason r WITH (NOLOCK)
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
