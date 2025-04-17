using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;
using System.Data.SqlClient;
using System;
using System.Data;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtMdivision
    /// </summary>
    public partial class TxtMdivision : Win.UI.TextBox
    {
        private bool _needInitialMdivision = false;
        private bool _defaultValue = false;

        /// <summary>
        /// Gets or sets a value indicating whether need initial Mdivision.
        /// </summary>
        public bool NeedInitialMdivision
        {
            get
            {
                return this._needInitialMdivision;
            }

            set
            {
                this._needInitialMdivision = value;
                if (this._needInitialMdivision)
                {
                    List<SqlParameter> listSqlPar = new List<SqlParameter>()
                    {
                        new SqlParameter("@Fty", Env.User.Factory),
                    };

                    this.Text = MyUtility.GetValue.Lookup("Select MdivisionID from Factory with (nolock) where ftygroup = @Fty", listSqlPar, "Production");
                }
            }
        }

        /// <summary>
        ///  sets a value Mdivision. by Keyword
        /// </summary>
        public bool DefaultValue
        {
            get
            {
                return this._defaultValue;
            }

            set
            {
                this._defaultValue = value;
                if (this._defaultValue)
                {
                    List<SqlParameter> listSqlPar = new List<SqlParameter>()
                    {
                        new SqlParameter("@key", Env.User.Keyword),
                    };
                    this.Text = MyUtility.GetValue.Lookup("Select ID from MDivision with (nolock) where id = @key", listSqlPar, "Production");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtMdivision"/> class.
        /// </summary>
        public TxtMdivision()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID from Production.dbo.MDivision WITH (NOLOCK) ", "8", this.Text, false, ",")
            {
                Size = new System.Drawing.Size(300, 250),
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", str));

            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (!MyUtility.Check.Seek("select ID from Production.dbo.MDivision WITH (NOLOCK) where id = @MDivision", listSqlPar, null))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< M : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
