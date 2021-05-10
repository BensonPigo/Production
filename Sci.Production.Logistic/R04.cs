using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class R04 : Win.Tems.PrintForm
    {
        private string po1;
        private string po2;
        private string sp1;
        private string sp2;
        private string brand;
        private string mDivision;
        private DateTime? gdate;
        private DataTable printData;
        private StringBuilder sqlcmd = new StringBuilder();

        /// <inheritdoc/>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex(true);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlcmd.Clear();
            if (MyUtility.Check.Empty(this.dateGenerate.Value))
            {
                MyUtility.Msg.WarningBox("<Generate Date> Cannot be empty!!");
                return false;
            }

            if ((!MyUtility.Check.Empty(this.txtPONo1.Text) && MyUtility.Check.Empty(this.txtPONo2.Text)) ||
                (MyUtility.Check.Empty(this.txtPONo1.Text) && !MyUtility.Check.Empty(this.txtPONo2.Text)) ||
                (!MyUtility.Check.Empty(this.txtSPNo1.Text) && MyUtility.Check.Empty(this.txtSPNo2.Text)) ||
                (MyUtility.Check.Empty(this.txtSPNo1.Text) && !MyUtility.Check.Empty(this.txtSPNo2.Text)) ||
                (MyUtility.Check.Empty(this.txtPONo1.Text) && MyUtility.Check.Empty(this.txtSPNo1.Text) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value1)))
            {
                MyUtility.Msg.WarningBox("Please fill <PO#>, <SP#> or <Buyer Delivery>");
                return false;
            }

            this.po1 = this.txtPONo1.Text;
            this.po2 = this.txtPONo2.Text;
            this.sp1 = this.txtSPNo1.Text;
            this.sp2 = this.txtSPNo2.Text;
            this.brand = this.txtbrand.Text;
            this.mDivision = this.comboM.Text;
            this.gdate = this.dateGenerate.Value;

            #region Where
            StringBuilder where = new StringBuilder();

            if (!MyUtility.Check.Empty(this.txtPONo1.Text))
            {
                where.Append(string.Format("\r\nand o.CustPONo >= '{0}'", this.txtPONo1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPONo2.Text))
            {
                where.Append(string.Format("\r\nand o.CustPONo <= '{0}'", this.txtPONo2.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                where.Append(string.Format("\r\nand pd.OrderID >= '{0}'", this.txtSPNo1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo2.Text))
            {
                where.Append(string.Format("\r\nand pd.OrderID <= '{0}'", this.txtSPNo2.Text));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where.Append(string.Format("\r\nand o.BuyerDelivery between '{0}' and '{1}'", Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd"), Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                where.Append(string.Format("\r\nand o.BrandID = '{0}'", this.txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(this.comboM.Text))
            {
                where.Append(string.Format("\r\nand p.MDivisionID = '{0}'", this.comboM.Text));
            }

            #endregion

            this.sqlcmd.Append($@"
declare @GenerateDate date = '{((DateTime)this.gdate).ToString("yyyy/MM/dd")}'
select
	p.MDivisionID,
	o.FactoryID,
	pd.OrderID,
	o.StyleID,
	PackingID = p.id,
	pd.CTNStartNo,
	ReceiveDate = (select MAX(ReceiveDate) from ClogReceive c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReceiveDate <= @GenerateDate),
	o.CustPONo,
	ClogLocationId=iif(pd.CFAReceiveDate is not null, 'CFA' ,pd.ClogLocationId),
	p.BrandID,
	Cancelled = iif(o.Junk=1, 'Y', 'N'),
	TTLQty.TTLQty,
	[QtyPerSize] = SizeCombo.combo,
	[PulloutComplete] = case when o.qty > isnull(s.ShipQty,0) then 'S'
							 when o.qty <= isnull(s.ShipQty,0) then'Y' end,
	o.ActPulloutDate,
	rea.reason
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
outer apply(
	select TTLQty = sum(pp.QtyPerCTN * iif(pp.CTNQty = 0, 1, pp.CTNQty)) 
	from PackingList_Detail pp
	where pp.ID= pd.ID and pp.CTNStartNo = pd.CTNStartNo
) TTLQty
outer apply(
	select combo = Stuff((
	    select concat('/',SizeCode+':'+ convert(varchar(10),QtyPerCTN))
	    from(
		    select distinct pp.SizeCode,pp.QtyPerCTN
		    from PackingList_Detail pp
		    where pp.ID=pd.ID and pp.CTNStartNo=pd.CTNStartNo
	    )s
	    for xml path('')
	),1,1,'')
) SizeCombo
outer apply(
	select ShipQty = sum(podd.ShipQty) 
	from Pullout_Detail_Detail podd WITH (NOLOCK) 
	inner join Order_Qty oq WITH (NOLOCK) on oq.id=podd.OrderID 
	and podd.Article= oq.Article and podd.SizeCode=oq.SizeCode
	where podd.OrderID = o.ID
)s
outer apply(
	select top 1 reason=concat(c.ClogReasonID,'-'+cr.Description)
	from ClogGarmentDispose_Detail cd
	inner join ClogGarmentDispose c on c.ID=cd.ID
	left join ClogReason cr on cr.id = c.ClogReasonID
	where cd.PackingListID = p.ID
)rea
where pd.CTNQty > 0
and pd.DisposeFromClog= 0
and o.PulloutComplete = 1
{where.ToString()}
and ((select MAX(AddDate) from ClogReceive c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReceiveDate <= @GenerateDate)
		> (select MAX(AddDate) from ClogReturn c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReturnDate <= @GenerateDate)
	or (exists (select 1 from ClogReceive c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReceiveDate <= @GenerateDate) 
		and not exists (select 1 from ClogReturn c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReturnDate <= @GenerateDate)))

union  all

select
	p.MDivisionID,
	o.FactoryID,
	pd.OrderID,
	o.StyleID,
	PackingID = p.id,
	pd.CTNStartNo,
	ReceiveDate = (select MAX(ReceiveDate) from ClogReceive c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReceiveDate <= @GenerateDate),
	o.CustPONo,
	ClogLocationId=iif(pd.CFAReceiveDate is not null,pd.CFALocationID,pd.ClogLocationId),
	p.BrandID,
	Cancelled = iif(o.Junk=1, 'Y', 'N'),
	TTLQty.TTLQty,
	[QtyPerSize] = SizeCombo.combo,
	[PulloutComplete] = 'N',
	o.ActPulloutDate,
	rea.reason
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Pullout po WITH (NOLOCK) on p.PulloutID = po.ID
outer apply(
	select TTLQty = sum(pp.QtyPerCTN * iif(pp.CTNQty = 0, 1, pp.CTNQty)) 
	from PackingList_Detail pp
	where pp.ID= pd.ID and pp.CTNStartNo = pd.CTNStartNo
) TTLQty
outer apply(
	select combo = Stuff((
	    select concat('/',SizeCode+':'+ convert(varchar(10),QtyPerCTN))
	    from(
		    select distinct pp.SizeCode,pp.QtyPerCTN
		    from PackingList_Detail pp
		    where pp.ID=pd.ID and pp.CTNStartNo=pd.CTNStartNo
	    )s
	    for xml path('')
	),1,1,'')
) SizeCombo
outer apply(
	select top 1 reason=concat(c.ClogReasonID,'-'+cr.Description)
	from ClogGarmentDispose_Detail cd
	inner join ClogGarmentDispose c on c.ID=cd.ID
	left join ClogReason cr on cr.id = c.ClogReasonID
	where cd.PackingListID = p.ID
)rea
where pd.CTNQty > 0
and pd.DisposeFromClog= 0
and o.PulloutComplete = 0
and (p.PulloutID = '' or po.Status = 'New')
{where.ToString()}
and ((select MAX(AddDate) from ClogReceive c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReceiveDate <= @GenerateDate)
		> (select MAX(AddDate) from ClogReturn c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReturnDate <= @GenerateDate)
	or (exists (select 1 from ClogReceive c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReceiveDate <= @GenerateDate) 
		and not exists (select 1 from ClogReturn c where c.PackingListID = p.id and c.CTNStartNo = pd.CTNStartNo and ReturnDate <= @GenerateDate)))
");

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd.ToString(), out this.printData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\Logistic_R04.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Logistic_R04.xltx", 4, false, null, excel);

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 8] = ((DateTime)this.gdate).ToString("yyyy/MM/dd");
            worksheet.Cells[2, 2] = this.po1 + " ~ " + this.po2;
            worksheet.Cells[3, 2] = this.sp1 + " ~ " + this.sp2;
            worksheet.Cells[2, 6] = this.brand;
            worksheet.Cells[3, 6] = this.mDivision;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Logistic_R04");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            strExcelName.OpenFile();
            #endregion

            this.HideWaitMessage();
            return true;
        }
    }
}
