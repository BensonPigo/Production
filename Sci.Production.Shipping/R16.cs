using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    public partial class R16 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DualResult result;
        private DateTime? dateBuyerDelivery1;
        private DateTime? dateBuyerDelivery2;
        private string strBuyer;
        private string strBrand;
        private string strFactory;
        private string strShipper;
        private string strCustCD;
        private string strDest;
        private string strCategory;
        private string strShipMode;

        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            this.dateBuyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.dateBuyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.strBuyer = this.txtbuyer.Text;
            this.strBrand = this.txtbrand.Text;
            this.strFactory = this.txtfactory.Text;
            this.strShipper = this.txtShipper.Text;
            this.strCustCD = this.txtcustcd.Text;
            this.strDest = this.txtcountry.TextBox1.Text;
            this.strCategory = this.comboCategory.SelectedValue.ToString();
            this.strShipMode = this.comboshipmode.Text;

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd = $@"
select oq.BuyerDelivery
,o.BrandID
,oq.Id
,oq.Seq
,oq.ShipmodeID
,fs.ShipperID
,o.MDivisionID,o.FactoryID
,[PackingID] = p.ID
,oq.SDPDate,oq.Qty
,pkQty.CTNQty,pkQty.gw
,l.CBM
,o.CustCDID
,[Destination] = (select id+'-'+Alias from Country where Id=o.Dest)
,p.Remark
from Orders o
left join Order_QtyShip oq on o.ID=oq.Id
left join Brand b on b.id=o.BrandID
inner join (
	select distinct p.id,p.remark ,pd.OrderID,pd.OrderShipmodeSeq
	from PackingList p
	inner join PackingList_Detail pd on p.ID=pd.ID
	where (p.INVNo ='' or p.INVNo is null)) p on p.OrderID=oq.Id and p.OrderShipmodeSeq=oq.Seq
outer apply(
	Select ShipperID 
	from FtyShipper_Detail  
	where oq.BuyerDelivery between BeginDate and EndDate 
	and FactoryId = o.FactoryID 
	and BrandID = o.BrandID
)fs
outer apply(
	select sum(CTNQty) CTNQty , sum(GW) gw
	from PackingList_Detail
	where OrderID=oq.Id and OrderShipmodeSeq = oq.Seq
)pkQty
outer apply(
	select sum(l.CBM) CBM from PackingList_Detail pd
	inner join LocalItem l on l.RefNo=pd.RefNo
	where pd.OrderID=oq.Id and pd.OrderShipmodeSeq=oq.Seq
)L
where o.localOrder = 0
and not exists(
	select 1 
	from Order_QtyShip_Detail oq2
	inner join PackingList_Detail pd on pd.OrderID=oq2.Id and pd.OrderShipmodeSeq=oq2.Seq
	inner join PackingList p on p.ID=pd.ID and p.Type='F'
	where oq2.Id=oq.Id and oq2.Seq=oq.Seq
)
";
            #region Where

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery1) && !MyUtility.Check.Empty(this.dateBuyerDelivery2))
            {
                sqlcmd += $@" and oq.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateBuyerDelivery2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strBuyer))
            {
                sqlcmd += $@" and b.BuyerID = '{this.strBuyer}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strBrand))
            {
                sqlcmd += $@" and o.BrandID = '{this.strBrand}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strFactory))
            {
                sqlcmd += $@" and o.FtyGroup = '{this.strFactory}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strShipper))
            {
                sqlcmd += $@" and fs.ShipperID = '{this.strShipper}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strCustCD))
            {
                sqlcmd += $@" and o.CustCDID = '{this.strCustCD}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strDest))
            {
                sqlcmd += $@" and o.Dest = '{this.strDest}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strCategory))
            {
                sqlcmd += $@" and o.Category in ({this.strCategory})" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.strShipMode))
            {
                sqlcmd += $@" and oq.ShipModeID  = '{this.strShipMode}'" + Environment.NewLine;
            }
            #endregion

            if (!(this.result = DBProxy.Current.Select(null, sqlcmd, out this.printData)))
            {
                this.ShowErr(this.result);
            }

            return this.result;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Starting EXCEL...");
            string reportName = "Shipping_R16.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R16");
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }

        private void TxtShipper_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                @" 
select distinct ShipperID 
from FtyShipper_Detail", "15", this.txtShipper.Text, headercaptions: "Shipper");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.txtShipper.Text = item.GetSelectedString();
        }

        private void TxtShipper_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.txtShipper.OldValue != this.txtShipper.Text)
            {
                if (!MyUtility.Check.Seek($@"select 1 from FtyShipper_Detail where ShipperID='{this.txtShipper.Text}'"))
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    this.txtShipper.Text = string.Empty;
                    e.Cancel = true;
                }
            }
        }
    }
}
