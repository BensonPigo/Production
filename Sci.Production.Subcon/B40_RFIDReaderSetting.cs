using Ict.Win;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Subcon
{
    public partial class B40_RFIDReaderSetting : Win.Subs.Input4
    {
        private string ID;
        private DataRow Master;
        public DataTable DetailDT;

        public B40_RFIDReaderSetting(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow master, DataTable dt)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.ID = keyvalue1;
            this.Master = master;
            this.DetailDT = dt;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            #region Grid事件
            DataGridViewGeneratorTextColumnSettings cutCellID = new DataGridViewGeneratorTextColumnSettings();
            cutCellID.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string sqlcmd = $@"select ID,Description from CutCell with(nolock)where Junk = 0 and MDivisionID = '{this.Master["MDivisionID"]}' ";
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "8,40", null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["CutCellID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            cutCellID.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.EditMode == false)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
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
            this.Helper.Controls.Grid.Generator(this.grid)
             .Text("PanelNo", header: "Panel No", width: Widths.AnsiChars(20))
             .Text("CutCellID", header: "Cut Cell", width: Widths.AnsiChars(8), settings: cutCellID, iseditingreadonly: false)
             .DateTime("addDate", header: "Add Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("addName", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.grid.Columns["PanelNo"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["CutCellID"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        /// <inheritdoc/>
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

            if (this.DetailDT != null && this.DetailDT.Rows.Count > 0)
            {
                datas = this.DetailDT.Copy();
            }

            this.gridbs.DataSource = datas;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            if (dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && MyUtility.Check.Empty(w["PanelNo"])).Count() > 0)
            {
                MyUtility.Msg.WarningBox("Panel No cannot empty");
                return false;
            }

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnSave()
        {
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override void OnSaveAfter()
        {
            base.OnSaveAfter();
            this.DetailDT = ((DataTable)this.gridbs.DataSource).Copy();
        }

        /// <inheritdoc/>
        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.revise.Visible = false;
        }
    }
}
