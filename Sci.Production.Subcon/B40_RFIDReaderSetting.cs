using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class B40_RFIDReaderSetting : Sci.Win.Subs.Input4
    {
        private string ID;
        private DataRow Master;

        public B40_RFIDReaderSetting(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow master) 
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.ID = keyvalue1;
            this.Master = master;
        }

        protected override bool OnGridSetup()
        {
            #region Grid事件
            Ict.Win.DataGridViewGeneratorTextColumnSettings CutCellID = new DataGridViewGeneratorTextColumnSettings();
            CutCellID.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"select ID,Description from CutCell with(nolock)where Junk = 0 and MDivisionID = '{this.Master["MDivisionID"]}' ";
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, "8,40", null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["CutCellID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            CutCellID.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["CutCellID"] = string.Empty;
                    return;
                }

                string sqlcmd = $@"select 1 from CutCell with(nolock)where Junk = 0 and MDivisionID = '{this.Master["MDivisionID"]}' and ID = '{e.FormattedValue}' ";
                if (MyUtility.Check.Seek(sqlcmd))
                {
                    dr["CutCellID"] = e.FormattedValue;
                }
                else
                {
                    MyUtility.Msg.WarningBox("data not found!");
                    dr["CutCellID"] = string.Empty;
                }
                dr.EndEdit();
            };
            #endregion

            this.grid.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid)
             .Text("PanelNo", header: "Panel No", width: Widths.AnsiChars(20), iseditingreadonly: true)
             .Text("CutCellID", header: "Cut Cell", width: Widths.AnsiChars(8), settings: CutCellID, iseditingreadonly: false)
             .DateTime("addDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("addName", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true);             
            this.grid.Columns["PanelNo"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["CutCellID"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        protected override void OnRequired()
        {
            base.OnRequired();
            string sqlcmd = $@"
select rp.RFIDReaderID,rp.PanelNo,rp.CutCellID,rp.AddDate,rp.EditDate,AddName=dbo.GetPass1(rp.AddName),EditName=dbo.GetPass1(rp.EditName)
from RFIDReader_Panel rp with(nolock)
where rp.RFIDReaderID ='{this.ID}'
";
            DataTable datas;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out datas);
            if (!result)
            {
                this.ShowErr(result);
            }
            this.gridbs.DataSource = datas;
        }

        protected override bool OnSaveBefore()
        {
            return base.OnSaveBefore();
        }
    }
}
