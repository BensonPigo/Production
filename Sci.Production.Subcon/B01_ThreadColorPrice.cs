using Ict.Win;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Subcon
{
    public partial class B01_ThreadColorPrice : Win.Subs.Input4
    {
        private string _refno;

        public B01_ThreadColorPrice(bool canedit, string Refno, string keyvalue2, string keyvalue3)
            : base(canedit, Refno, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.WorkAlias = "LocalItem_ThreadBuyerColorGroupPrice";
            this.KeyField1 = "Refno";
            this._refno = Refno;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        protected override bool OnGridSetup()
        {
            #region Grid事件
            DataGridViewGeneratorTextColumnSettings col_threadColor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings col_Buyer = new DataGridViewGeneratorTextColumnSettings();

            col_threadColor.EditingMouseDown += (s, e) =>
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
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        @"Select ID,description  from ThreadColorGroup WITH (NOLOCK) where JUNK=0 order by ID", "10,45", null);
                    item.Size = new Size(630, 535);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ThreadColorGroupID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_threadColor.CellValidating += (s, e) =>
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
                    dr["ThreadColorGroupID"] = string.Empty;
                    return;
                }

                if (MyUtility.Check.Seek($@"select 1 from ThreadColorGroup WITH (NOLOCK) where junk=0 and id='{e.FormattedValue}'"))
                {
                    dr["ThreadColorGroupID"] = e.FormattedValue;
                }
                else
                {
                    MyUtility.Msg.WarningBox("data not found!");
                    dr["ThreadColorGroupID"] = string.Empty;
                    e.Cancel = true;
                }

                dr.EndEdit();
            };

            col_Buyer.EditingMouseDown += (s, e) =>
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
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                        @"Select ID,NameEN  from Buyer WITH (NOLOCK) where JUNK=0 order by ID", "10,45", null);
                    item.Size = new Size(630, 535);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["BuyerID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_Buyer.CellValidating += (s, e) =>
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
                    dr["BuyerID"] = string.Empty;
                    return;
                }

                if (MyUtility.Check.Seek($@"select 1 from Buyer WITH (NOLOCK) where junk=0 and id='{e.FormattedValue}'"))
                {
                    dr["BuyerID"] = e.FormattedValue;
                }
                else
                {
                    MyUtility.Msg.WarningBox("data not found!");
                    dr["BuyerID"] = string.Empty;
                    e.Cancel = true;
                }

                dr.EndEdit();
            };
            #endregion

            this.grid.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid)
             .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("BuyerID", header: "Buyer", width: Widths.AnsiChars(10), settings: col_Buyer, iseditingreadonly: false)
             .Text("ThreadColorGroupID", header: "Thread Color Group ID", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: col_threadColor)
             .Numeric("Price", header: "Price", integer_places: 12, decimal_places: 4, width: Widths.AnsiChars(12))
             .DateTime("addDate", header: "Create Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("addName", header: "Create Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.grid.Columns["ThreadColorGroupID"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["BuyerID"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Price"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        protected override void OnInsert()
        {
            base.OnInsert();
        }

        protected override void OnInsertPrepare(DataRow data)
        {
            data["Refno"] = this._refno;
            base.OnInsertPrepare(data);
        }

        protected override bool OnSaveBefore()
        {
            foreach (DataRow dr in this.Datas)
            {
                if (MyUtility.Check.Empty(dr["ThreadColorGroupID"]))
                {
                    MyUtility.Msg.WarningBox("<Thread Color Group ID> cannot be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Price"]))
                {
                    MyUtility.Msg.WarningBox("<Price> cannot be empty or 0!");
                    return false;
                }
            }

            #region 判斷Color是否有重複
            DataTable dtfilter = (DataTable)this.gridbs.DataSource;
            if (dtfilter.DefaultView.ToTable(true, new string[] { "Refno", "ThreadColorGroupID", "BuyerID" }).Rows.Count !=
                dtfilter.DefaultView.ToTable(false, new string[] { "Refno", "ThreadColorGroupID", "BuyerID" }).Rows.Count)
            {
                MyUtility.Msg.WarningBox("<Refno><Thread Color><Buyer> has been repeating, cannot save!");
                return false;
            }
            #endregion
            return base.OnSaveBefore();
        }

        protected override DualResult OnSave()
        {
            return base.OnSave();
        }
    }
}
