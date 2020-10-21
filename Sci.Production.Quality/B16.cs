using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System.Linq;
using System.Collections.Generic;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class B16 : Sci.Win.Tems.Input6
    {
        /// <inheritdoc/>
        public B16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Description", header: "Description", width: Widths.AnsiChars(3))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(2))
                ;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["SubProcessID"]))
            {
                MyUtility.Msg.WarningBox("SubProcess can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["AssignColumn"]))
            {
                MyUtility.Msg.WarningBox("Assign Column can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["DisplayName"]))
            {
                MyUtility.Msg.WarningBox("DisplayName can't empty!");
                return false;
            }

            if (this.DetailDatas.AsEnumerable().Where(w => MyUtility.Check.Empty(w["Description"])).Any())
            {
                MyUtility.Msg.WarningBox("Description can't empty");
                return false;
            }

            if (this.DetailDatas.AsEnumerable()
                .GroupBy(g => new { Description = MyUtility.Convert.GetString(g["Description"]) })
                .Select(s => new { s.Key.Description, ct = s.Count() })
                .Where(w => w.ct > 1)
                .Any())
            {
                MyUtility.Msg.WarningBox("Description can't duplicate");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            var result = base.ClickSave();
            string msg = result.ToString().ToUpper();
            if (msg.Contains("PK") && msg.Contains("DUPLICAT"))
            {
                result = Ict.Result.F("SubProcess, Assign Column duplicated", result.GetException());
            }

            return result;
        }

        private void TxtSubProcessID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CheckSubProcessID();
        }

        private void TxtSubProcessID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CheckSubProcessID(true);
        }

        private void CheckSubProcessID(bool validating = false)
        {
            string sqlcmd = $@"select ID,ArtworkTypeId from SubProcess where IsSubprocessInspection = 1 and junk = 0";
            if (validating)
            {
                sqlcmd += $" and id = '{this.txtSubProcessID.Text}'";
            }

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (validating)
            {
                if (dt.Rows.Count == 0 && !this.txtSubProcessID.Text.Empty())
                {
                    MyUtility.Msg.WarningBox($"{this.txtSubProcessID.Text} not found!");
                    this.txtSubProcessID.Text = string.Empty;
                    return;
                }
            }
            else
            {
                SelectItem item = new SelectItem(dt, "ID,ArtworkTypeId", string.Empty, this.txtSubProcessID.Text, false, ",");
                if (item.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["SubProcessID"] = item.GetSelectedString();
            }
        }

        private void TxtAssignColumn_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            this.CheckAssignColumn();
        }

        private void TxtAssignColumn_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CheckAssignColumn(true);
        }

        private void CheckAssignColumn(bool validating = false)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CustomColumn", typeof(string));
            for (int i = 1; ; i++)
            {
                string sqlcmd = $@"SELECT 1 FROM sys.columns WHERE Name = N'CustomColumn{i}' AND Object_ID = Object_ID(N'SubProInsRecord')";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    DataRow dr = dt.NewRow();
                    dr["CustomColumn"] = $"CustomColumn{i}";
                    dt.Rows.Add(dr);
                }
                else
                {
                    break;
                }
            }

            if (validating)
            {
                if (!dt.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["CustomColumn"]).Contains(this.txtAssignColumn.Text)).Any())
                {
                    MyUtility.Msg.WarningBox($"{this.txtAssignColumn.Text} not exists");
                    this.txtAssignColumn.Text = string.Empty;
                    return;
                }
            }
            else
            {
                SelectItem item = new SelectItem(dt, "CustomColumn", string.Empty, this.txtAssignColumn.Text, false, ",");
                if (item.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["AssignColumn"] = item.GetSelectedString();
            }
        }
    }
}
