using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    public partial class R14 : Sci.Win.Tems.PrintForm
    {
        private DataTable dtPrint;
        private string sqlcmd;

        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Sci.Env.User.Keyword;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboDropDownList.SelectedIndex = 4;
        }

        protected override bool ValidateInput()
        {
            string where = string.Empty;
            #region 檢查必輸條件 & 加入SQL where 參數
            if ((MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2))
                && (MyUtility.Check.Empty(this.txtSpNo1.Text) && MyUtility.Check.Empty(this.txtSpNo2.Text))
                && MyUtility.Check.Empty(this.txtBrand.Text)
                && (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2)))
            {
                MyUtility.Msg.WarningBox("Please key-in condition!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.dateIssueDate.Value1))
            {
                where += $" and a1.cdate >='{((DateTime)this.dateIssueDate.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                where += $" and a1.cdate <='{((DateTime)this.dateIssueDate.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $" and o.BrandID = '{this.txtBrand.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSpNo1.Text))
            {
                where += $" and a2.Orderid >='{this.txtSpNo1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSpNo2.Text))
            {
                where += $" and a2.Orderid <='{this.txtSpNo2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $" and oq.BuyerDelivery >='{((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                where += $" and oq.BuyerDelivery <='{((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList.Text))
            {
                where += $" and a1.status in ({this.comboDropDownList.SelectedValue})";
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                where += $" and a1.MdivisionID = '{this.txtMdivision.Text}'";
            }

            this.sqlcmd = $@"
select a1.id
,a1.cDate
,a1.MdivisionID
,o.FtyGroup
,o.BrandID
,o.SewLine
,a2.OrderID
,o.StyleID
,o.CustPONo
,c.Alias
,oq.Qty
,a2.OrderShipmodeSeq
,a2.ShipModeID
,oq.BuyerDelivery
,[TotalCrtns] = Packing.qty
,a3.RefNo
,[Junk] = iif(o.Junk = 1, 'Y','')
,a1.Handle
from AVO a1
left join AVO_Detail a2 on a1.ID=a2.ID
left join orders o on o.ID=a2.OrderID
left join Country c on c.ID=o.Dest
left join Order_QtyShip oq on oq.Id=o.ID and oq.ShipmodeID=a2.ShipModeID
	and oq.Seq=a2.OrderShipmodeSeq
outer apply(
	select SUM(PD.CTNQty) qty
	from PackingList_Detail PD 
	LEFT JOIN PackingList P ON PD.ID=P.ID
	where PD.orderid= a2.OrderID AND P.ShipModeID= a2.ShipModeID 
	AND PD.OrderShipmodeSeq = a2.OrderShipmodeSeq
)Packing
outer apply(
	select RefNo = STUFF((
		select concat(',',RefNo)
		from(
			select distinct Refno
			from AVO_Detail_RefNo
			where AVO_DetailUkey=a2.Ukey
		) s
	for xml path ('')
	) ,1,1,'')
)a3
where 1=1
 {where}
";
            #endregion
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(string.Empty, this.sqlcmd, out this.dtPrint);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtPrint.Rows.Count); // 顯示筆數

            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\PPIC_R14.xltx", objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            com.WriteTable(this.dtPrint, 3);
            worksheet.get_Range($"A3:R{MyUtility.Convert.GetString(2 + this.dtPrint.Rows.Count)}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 畫線
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            objApp.Rows.AutoFit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
