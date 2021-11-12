using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class ComboSewingTeam : Win.UI.ComboBox
    {
        /// <inheritdoc/>
        [Category("IssupportJunk")]
        public bool IssupportJunk { get; set; } = false;

        /// <inheritdoc/>
        [Category("IssupportEmptyitem")]
        public bool IssupportEmptyitem { get; set; } = false;

        /// <inheritdoc/>
        public ComboSewingTeam()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(88, 23);
        }

        /// <inheritdoc/>
        public ComboSewingTeam(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(88, 23);
        }

        /// <inheritdoc/>
        public void SetDataSource()
        {
            List<string> listFilte = new List<string>();
            if (!this.IssupportJunk)
            {
                listFilte.Add("Junk = 0");
            }

            string where = string.Empty;
            if (listFilte.Count > 0)
            {
                where = "where " + listFilte.JoinToString("\n\rand ");
            }

            string sqlEmptyitem = $"select ID = '' union all ";
            string sqlcmd = $@"select ID from SewingTeam {where}";
            if (this.IssupportEmptyitem)
            {
                sqlcmd = sqlEmptyitem + sqlcmd;
            }

            DualResult result = DBProxy.Current.Select("Production", sqlcmd, out DataTable dt);
            if (result && dt != null)
            {
                this.DataSource = dt;
                this.ValueMember = "ID";
                this.DisplayMember = "ID";
            }
        }
    }
}
