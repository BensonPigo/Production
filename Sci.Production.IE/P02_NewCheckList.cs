using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    public partial class P02_NewCheckList : Sci.Win.Subs.Input4
    {
        public P02_NewCheckList(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
        }

        protected override bool OnGridSetup()
        {
            
            Helper.Controls.Grid.Generator(this.grid)
                .Numeric("DayBe4Inline", header: "Days before", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("BaseOnDesc", header: "Base On", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ChkListDesc", header: "Activities", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Date("ScheduleDate",header:"Schedule Date")
                .Date("ActualDate", header: "Actual Date")
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(30));

            grid.Columns[3].DefaultCellStyle.BackColor = Color.Pink;
            grid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            grid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }
        
        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format(@"select cc.*,iif(cc.BaseOn = 1,'Change Over','SCI Delivery') as BaseOnDesc,
cl.Description as ChkListDesc
     from ChgOver_Check cc
left join ChgOverCheckList cl on cc.ChgOverCheckListID = cl.ID
where cc.ID = {0} order by cc.ChgOverCheckListID", this.KeyValue1);
            Ict.DualResult returnResult;
            DataTable ChgOverChkList = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out ChgOverChkList);
            if (!returnResult)
            {
                return returnResult;
            }
            SetGrid(ChgOverChkList);
            return Result.True;
        }

        //Save -- Append/Revise/Delete按鈕要隱藏
        private void save_Click(object sender, EventArgs e)
        {
            append.Visible = false;
            revise.Visible = false;
            delete.Visible = false;
        }

        //To Excel
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable ExcelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)gridbs.DataSource, "DayBe4Inline,BaseOnDesc,ChkListDesc,ScheduleDate,ActualDate,Remark", "select * from #tmp", out ExcelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\IE_P02_ChkListNew.xltx");
            MyUtility.Excel.CopyToXls(ExcelTable, "", "IE_P02_ChkListNew.xltx", 2, true, "", objApp);

        }
    }
}
