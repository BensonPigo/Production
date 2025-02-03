using Ict;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class B17 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = $"JUNK = 0";
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                this.ReloadDatas();
            };
        }

        private void TxtDept_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("SELECT DISTINCT Dept FROM Employee WHERE Dept IS NOT NULL AND Dept <> ''", "35", this.txtDept.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["Dept"] = item.GetSelectedString();
            }
        }

        private void TxtPosition_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem("SELECT DISTINCT Position FROM Employee WHERE Position IS NOT NULL AND Position <> ''", "35", this.txtDept.Text);

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["Position"] = item.GetSelectedString();
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            DualResult result;
            DataTable dt;

            if (MyUtility.Check.Empty(this.txtfactory1.Text))
            {
                MyUtility.Msg.WarningBox("<Factory> can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtDept.Text))
            {
                MyUtility.Msg.WarningBox("<Dept> can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtPosition.Text))
            {
                MyUtility.Msg.WarningBox("<Position> can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                if (result = DBProxy.Current.Select(null, "select max_id = max(id) from EmployeeAllocationSetting WITH (NOLOCK)", out dt))
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
                }
                else
                {
                    this.ShowErr(result);
                    return false;
                }
            }

            string sqlcmd = $@"
            select 1 
            from EmployeeAllocationSetting WITH (NOLOCK) 
            Where 
            FactoryID = '{this.txtfactory1.Text}' and 
            Dept = '{this.txtDept.Text}' and 
            Position = '{this.txtPosition.Text}'";

            if (result = DBProxy.Current.Select(null, sqlcmd, out dt))
            {
                if (!this.EditMode)
                {
                    string id = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : string.Empty;
                    if (id == "1")
                    {
                        MyUtility.Msg.WarningBox("This [Factory + Dept + Position] already exists!");
                        return false;
                    }
                }
            }
            else
            {
                this.ShowErr(result);
                return false;
            }


            string strDuplicated = MyUtility.GetValue.Lookup($@"Select 1 FROM EmployeeAllocationSetting WITH (NOLOCK) WHERE FactoryID = '{this.txtfactory1.Text}' AND Dept = '{this.txtDept.Text}' AND Position = '{this.txtPosition.Text}'");

            if (strDuplicated == "1")
            {
                MyUtility.Msg.WarningBox($"Factory:<{this.txtfactory1.Text}>, Dept:<{this.txtDept.Text}>, Position:<{this.txtPosition.Text}> already exists!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
