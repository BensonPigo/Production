using System;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P02_RepeatCheckList
    /// </summary>
    public partial class P02_RepeatCheckList : Win.Subs.Input4
    {
        /// <summary>
        /// P02_RepeatCheckList
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public P02_RepeatCheckList(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnGridSetup
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Numeric("DayBe4Inline", header: "Days before", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("BaseOnDesc", header: "Base On", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ChkListDesc", header: "Activities", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Date("ActualDate", header: "Actual Date")
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30));

            this.grid.Columns["ActualDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        /// <summary>
        /// OnRequery
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format(
                @"select cc.*,iif(cc.BaseOn = 1,'Change Over','SCI Delivery') as BaseOnDesc,
cl.Description as ChkListDesc
from ChgOver_Check cc WITH (NOLOCK) 
left join ChgOverCheckList cl WITH (NOLOCK) on cc.ChgOverCheckListID = cl.ID
where cc.ID = {0} order by cc.ChgOverCheckListID", this.KeyValue1);
            DualResult returnResult;
            DataTable chgOverChkList = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out chgOverChkList);
            if (!returnResult)
            {
                return returnResult;
            }

            this.SetGrid(chgOverChkList);
            this.grid.AutoResizeColumns();
            return Result.True;
        }

        // Save -- Append/Revise/Delete按鈕要隱藏
        private void Save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.gridbs.DataSource, "DayBe4Inline,BaseOnDesc,ChkListDesc,ActualDate,Remark", "select * from #tmp", out excelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\IE_P02_ChkListRepeat.xltx");
            MyUtility.Excel.CopyToXls(excelTable, string.Empty, "IE_P02_ChkListRepeat.xltx", 2, true, string.Empty, objApp);
        }
    }
}
