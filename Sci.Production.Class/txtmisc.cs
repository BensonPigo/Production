using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Sci;
using Ict;
using Sci.Win;
using Ict.Win;
using Sci.Win.Tools;


namespace Sci.Production.Class
{

    public partial class txtmisc : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select id from Misc where junk=0", "23", this.Text, false, ",");
            //
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = MyUtility.GetValue.Lookup("id", str, "Misc", "id");
                if (string.IsNullOrWhiteSpace(tmp))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Miscellaneous> : {0} not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
                string cjunk = MyUtility.GetValue.Lookup("Junk", str, "Misc", "id");
                if (cjunk == "True")
                {
                    MyUtility.Msg.WarningBox(string.Format("Miscellanesou already junk, you can't choose!!"));
                    this.Text = "";
                }
            }
        }
        public txtmisc()
        {
            this.Width = 156;
            this.IsSupportSytsemContextMenu = false;
        }
    }
    public class cellmisc : DataGridViewGeneratorTextColumnSettings
    {
        public static DataGridViewGeneratorTextColumnSettings GetGridCell()
        {
            cellmisc ts = new cellmisc();
            // 右鍵彈出功能
            ts.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    // Parent form 若是非編輯狀態就 return 
                    if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    SelectItem sele = new SelectItem("Select id From Misc where Junk=0", "23", row["miscid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }


            };
            // 正確性檢查
            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                // Parent form 若是非編輯狀態就 return 
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode) { return; }
                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                String oldValue = row["miscid"].ToString();
                String newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                string sql = string.Format("Select * from Misc where Junk = 0 and ID = '{0}'", newValue);
                if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                {
                    if (!MyUtility.Check.Seek(sql))
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Miscellaneous > : {0} not found!!!", newValue));
                        row["miscid"] = "";
                        row.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }

            };
            return ts;
        }

    }
}