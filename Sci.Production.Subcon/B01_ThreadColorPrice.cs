using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;

namespace Sci.Production.Subcon
{
    public partial class B01_ThreadColorPrice : Sci.Win.Subs.Input4
    {
        private string _refno;
        public B01_ThreadColorPrice(bool canedit, string Refno, string keyvalue2, string keyvalue3) : base(canedit, Refno, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.WorkAlias = "LocalItem_ThreadColorPrice";
            this.KeyField1 = "Refno";
            _refno = Refno;
        }

        protected override void OnFormLoaded()
        {          
            base.OnFormLoaded();
        }

        protected override bool OnGridSetup()
        {
            #region Grid事件
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_threadColor = new DataGridViewGeneratorTextColumnSettings();
            col_threadColor.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = grid.GetDataRow(e.RowIndex);                   
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
                        (@"Select ID,description  from threadcolor WITH (NOLOCK) where JUNK=0 order by ID", "10,45", null);
                    item.Size = new System.Drawing.Size(630, 535);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["ThreadColorID"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_threadColor.CellValidating += (s, e) => 
            {
                if (e.RowIndex == -1) return;
                if (this.EditMode == false) return;
                DataRow dr = grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["ThreadColorID"] = string.Empty;
                    return;
                }
                if (MyUtility.Check.Seek($@"select 1 from threadcolor WITH (NOLOCK) where junk=0 and id='{e.FormattedValue}'"))
                {                    
                    dr["ThreadColorID"] = e.FormattedValue;
                }
                else
                {
                    MyUtility.Msg.WarningBox("data not found!");
                    dr["ThreadColorID"] = string.Empty;
                    e.Cancel = true;
                }
                dr.EndEdit();
            };
            #endregion

            this.grid.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid)
             .Text("Refno", header: "Refno", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .Text("ThreadColorID", header: "Thread Color", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: col_threadColor)
             .Numeric("Price", header: "Price", integer_places: 12, decimal_places: 4, width: Widths.AnsiChars(12))
             .DateTime("addDate", header: "Create Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("addName", header: "Create Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
             .DateTime("editDate", header: "Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format: DataGridViewDateTimeFormat.yyyyMMddHHmmss)
             .Text("editName", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true);             
            this.grid.Columns["ThreadColorID"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Price"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        protected override void OnInsert()
        {            
            base.OnInsert();
        }

        protected override void OnInsertPrepare(DataRow data)
        {
            data["Refno"] = _refno;
            base.OnInsertPrepare(data);
        }

        protected override bool OnSaveBefore()
        {
            foreach (DataRow dr in Datas)
            {
                if (MyUtility.Check.Empty(dr["ThreadColorID"]))
                {
                    MyUtility.Msg.WarningBox("<ThreadColorID> cannot be empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Price"]))
                {
                    MyUtility.Msg.WarningBox("<Price> cannot be empty!");
                    return false;
                }
            }

            #region 判斷Color是否有重複
            DataTable dtfilter = (DataTable)gridbs.DataSource;
            if (dtfilter.DefaultView.ToTable(true, new string[] { "Refno", "ThreadColorID" }).Rows.Count !=
                dtfilter.DefaultView.ToTable(false, new string[] { "Refno", "ThreadColorID" }).Rows.Count)
            {
                MyUtility.Msg.WarningBox("<Refno><Thread Color> has been repeating, cannot save!");
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
