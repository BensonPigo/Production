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
        private string strJunk;
        private bool FOC;
        private bool GMTCompleteShortage;

        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboCategory.SelectedIndex = 4;
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
            this.strJunk = this.chkcancelOrder.Checked ? string.Empty : "0";
            this.FOC = this.chkFOC.Checked;
            this.GMTCompleteShortage = this.chkGMTCompleteShortage.Checked;

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd = $@"
select 
	oq.BuyerDelivery
	,o.BrandID
	,oq.Id
	,oq.Seq
	,oq.ShipmodeID
	,[CancelOrder]=IIF(o.Junk=1,'Y','')
	,fs.ShipperID
	,o.MDivisionID
	,o.FactoryID
	,[PackingID] = p.ID
	,oq.SDPDate,oq.Qty
	,pkQty.CTNQty
	,pkQty.gw
	,l.CBM
	,foc.FOC
	,sew.QAQty
	,atCLog.ct
	,o.OrderTypeID
	,o.CustCDID
	,[Destination] = (select id+'-'+Alias from Country where Id=o.Dest)
	,o.GMTComplete
	,ShortageQty=iif(o.GMTComplete='S', isnull(o.Qty,0)-isnull(o.FOCQty,0)-isnull(PulloutQty.OrderQty,0)+isnull(inv.DiffQty,0),null)
	,[Category]=Category.Value--**
    ,[OutstandingReason]=OutstandingRemark.Value
	,[EstPODD]=o.EstPODD
	,[ReasonRemark]=o.OutstandingRemark
	,[PackingListRemark]=p.Remark
from Orders o
inner join Order_QtyShip oq on o.ID=oq.Id
left join Brand b on b.id=o.BrandID
inner join Factory f with(nolock) on f.id = o.FactoryID
outer apply (
	select distinct p.ID,p.Remark,p.INVNo
	from PackingList p
	inner join PackingList_Detail pd on p.ID=pd.ID	
	where pd.OrderID=oq.Id and pd.OrderShipmodeSeq=oq.Seq
) p 
outer apply(
	Select ShipperID 
	from FtyShipper_Detail  
	where oq.BuyerDelivery between BeginDate and EndDate 
	and FactoryId = o.FactoryID 
	and BrandID = o.BrandID
	and SeasonID = o.SeasonID
)fs1
outer apply(
	Select ShipperID =isnull(fs1.ShipperID,ShipperID)
	from FtyShipper_Detail  
	where oq.BuyerDelivery between BeginDate and EndDate 
	and FactoryId = o.FactoryID 
	and BrandID = o.BrandID
	and SeasonID = ''
)fs
outer apply(
	select sum(CTNQty) CTNQty , sum(GW) gw
	from PackingList_Detail
	where OrderID=oq.Id and OrderShipmodeSeq = oq.Seq
)pkQty
outer apply(    
    select ct=sum(b.CTNQty) 
    from PackingList a, PackingList_Detail b 
    where a.ID = b.ID and (a.Type = 'B' or a.Type = 'L') and b.DisposeFromClog = 0 and ReceiveDate is not null
    and TransferCFADate is null AND CFAReturnClogDate is null
    and b.OrderID=oq.Id and b.OrderShipmodeSeq = oq.Seq
)atCLog
outer apply(
	select sum(l.CBM) CBM
	from PackingList_Detail pd
	inner join LocalItem l on l.RefNo=pd.RefNo
	where pd.OrderID=oq.Id and pd.OrderShipmodeSeq=oq.Seq
)L
outer apply(
	select top 1 FOC='Y'
	from Order_QtyShip_Detail oqd WITH (NOLOCK) 
	left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = oqd.Id and ou1.Article = '----' and ou1.SizeCode = '----' 
	left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = oqd.Id and ou2.Article = oqd.Article and ou2.SizeCode = oqd.SizeCode 
	where oqd.Id = o.id
	and isnull(ou2.POPrice,isnull(ou1.POPrice,-1)) = 0
)FOC
outer apply(
	select QAQty = ISNULL(dbo.getMinCompleteSewQty(o.ID,null,null),0)
)sew
outer apply(
	select OrderQty=sum(pd.ShipQty)
	from Pullout_Detail pd WITH (NOLOCK)
	inner join Pullout p WITH (NOLOCK)on p.id = pd.id
	where pd.OrderID = o.ID and p.Status != 'New' and pd.PackingListType != 'F'
)PulloutQty
outer apply(
	select DiffQty=sum (iaq.DiffQty)
	from InvAdjust ia
	inner join  InvAdjust_Qty iaq WITH (NOLOCK) on ia.ID = iaq.ID
	where exists(select 1 from GMTBooking where id = ia.GarmentInvoiceID)
	and ia.OrderID = o.ID and ia.OrderShipmodeSeq = oq.Seq
)inv
outer apply(
	select[Value]=d.Name
	from DropDownList d
	where d.Type='Category' AND d.ID = o.Category
)Category
outer apply(
	select[Value]=d.Name
	from Reason d
	where d.ReasonTypeID = 'Delivery_OutStand' AND d.ID = o.OutstandingReason
)OutstandingRemark
where o.localOrder = 0
and (p.INVNo ='' or p.INVNo is null)
and not exists(
	select 1 
	from Order_QtyShip_Detail oq2
	inner join PackingList_Detail pd on pd.OrderID=oq2.Id and pd.OrderShipmodeSeq=oq2.Seq
	inner join PackingList p on p.ID=pd.ID and p.Type='F'
	where oq2.Id=oq.Id and oq2.Seq=oq.Seq
)
and f.IsProduceFty = 1
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

            if (!MyUtility.Check.Empty(this.strJunk))
            {
                sqlcmd += $@" and o.Junk  = {this.strJunk}" + Environment.NewLine;
            }

            if (!this.GMTCompleteShortage)
            {
                sqlcmd += $@" and isnull(o.GMTComplete,'') <> 'S' ";
                sqlcmd += $@" and isnull(o.GMTComplete,'') <> 'C' ";
            }

            if (!this.FOC)
            {
                sqlcmd += $@" and isnull(FOC.FOC,'') <> 'Y' ";
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
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
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
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(
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
