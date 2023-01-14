using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// IE_B91
    /// </summary>
    public partial class IE_B91 : Win.Tems.Input1
    {
        /// <summary>
        /// IE_B91
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public IE_B91(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.CurrentMaintain["Description"].Empty())
            {
                MyUtility.Msg.WarningBox("Description cannot be empty!!");
                return false;
            }

            DualResult result;
            DataTable dt;

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                if (result = DBProxy.Current.Select("ProductionTPE", "select max_id = max(id) from IEReason  WITH (NOLOCK) WHERE  Type = 'LB'", out dt))
                {
                    string id = dt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        this.CurrentMaintain["ID"] = "00001";
                    }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        this.CurrentMaintain["ID"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }

                    this.CurrentMaintain["Type"] = "LB";
                }
                else
                {
                    this.ShowErr(result);
                    return false;
                }
            }

            List<SqlParameter> parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("@Description", this.CurrentMaintain["Description"].ToString()),
                        };

            result = DBProxy.Current.Select("ProductionTPE", "select ID from IEReason  WITH (NOLOCK) WHERE Type = 'LB' AND Description = @Description", parameters, out dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox($@"<Description> already exists as ID:<{dt.Rows[0]["ID"]}>!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
