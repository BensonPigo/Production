using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string po1;
        private string po2;
        private string sp1;
        private string sp2;
        private string brand;
        private string mDivision;
        private string location1;
        private string location2;
        private string Cancel;
        private DateTime? bdate1;
        private DateTime? bdate2;
        private DataTable printData;

        /// <summary>R02</summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Env.User.Keyword;
            this.comboCancel.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.po1 = this.txtPONoStart.Text;
            this.po2 = this.txtPONoEnd.Text;
            this.sp1 = this.txtSPNoStart.Text;
            this.sp2 = this.txtSPNoEnd.Text;
            this.brand = this.txtbrand.Text;
            this.mDivision = this.comboM.Text;
            this.Cancel = this.comboCancel.Text;
            this.location1 = this.txtcloglocationLocationStart.Text;
            this.location2 = this.txtcloglocationLocationEnd.Text;
            this.bdate1 = this.dateBuyerDelivery.Value1;
            this.bdate2 = this.dateBuyerDelivery.Value2;

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">ReportEventArgs</param>
        /// <returns>Result</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 900; // 15分鐘
            StringBuilder sqlWHERE = new StringBuilder();
            StringBuilder sqlcmd = new StringBuilder();

            if (!MyUtility.Check.Empty(this.po1))
            {
                sqlWHERE.Append(string.Format(" and o.CustPONo >= '{0}'", this.po1));
            }

            if (!MyUtility.Check.Empty(this.po2))
            {
                sqlWHERE.Append(string.Format(" and o.CustPONo <= '{0}'", this.po2));
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlWHERE.Append(string.Format(" and pd.OrderID >= '{0}'", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.bdate1))
            {
                sqlWHERE.Append(string.Format(" and o.BuyerDelivery between '{0}' and '{1}'", Convert.ToDateTime(this.bdate1).ToString("yyyy/MM/dd"), Convert.ToDateTime(this.bdate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlWHERE.Append(string.Format(" and pd.OrderID <= '{0}'", this.sp2));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlWHERE.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlWHERE.Append(string.Format(" and p.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.location1))
            {
                sqlWHERE.Append(string.Format(" and pd.ClogLocationId >= '{0}'", this.location1));
            }

            if (!MyUtility.Check.Empty(this.location2))
            {
                sqlWHERE.Append(string.Format(" and pd.ClogLocationId <= '{0}'", this.location2));
            }

            if (!MyUtility.Check.Empty(this.Cancel))
            {
                sqlWHERE.Append(string.Format(" and o.Junk = '{0}'", this.Cancel == "Y" ? 1 : 0));
            }

            sqlcmd.Append(@"
select a.MDivisionID,a.FactoryID,a.OrderID,a.StyleID,a.PackingID,a.CTNStartNo,a.ReceiveDate,a.CustPONo
,a.OriClogLocationId
,a.ClogLocationId
,a.BrandID
,a.Cancelled
,TTLQty
,[QtyPerSize]
,a.PulloutComplete
,ActPulloutDate
,reason
from(
    select 
    p.MDivisionID
    ,o.FactoryID
    ,pd.OrderID
    ,o.StyleID
    ,[PackingID] = p.id
    ,pd.CTNStartNo
    ,pd.ReceiveDate
    ,o.CustPONo
    ,[OriClogLocationId] = iif(pd.CFAReceiveDate is not null,'CFA',pd.OriClogLocationID)
    ,[ClogLocationId] = iif(pd.CFAReceiveDate is not null,'CFA',pd.ClogLocationId)
    ,p.BrandID
    ,Cancelled = iif(o.junk=1,'Y','N')
    ,[TTLQty] = TTL.Qty
    ,[QtyPerSize] = SizeCombo.combo
    ,pd.id,pd.Seq
    ,[PulloutComplete] = case when o.qty > isnull(s.ShipQty,0) then 'S'
						                   when o.qty <= isnull(s.ShipQty,0) then'Y'  end
    ,[ActPulloutDate] = o.ActPulloutDate
    ,rea.reason
    from PackingList p WITH (NOLOCK) 
    inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
    inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
    outer apply(
	    select ShipQty = sum(pad.ShipQty)
        from PackingList_Detail pad WITH (NOLOCK)
        inner join Order_Qty oq WITH (NOLOCK) on oq.ID = pad.OrderID and oq.Article = pad.Article and oq.SizeCode = pad.SizeCode
        inner join PackingList p WITH (NOLOCK) on p.ID = pad.ID
        where pad.OrderID = o.ID and p.PulloutID <> ''
    )s
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
	    select Qty = 
	    sum(pp.QtyPerCTN * iif(pp.CTNQty=0,1,pp.CTNQty)) 
	    from PackingList_Detail pp
	    where pp.ID= pd.ID and pp.CTNStartNo=pd.CTNStartNo
    ) TTL
    outer apply(
	    select distinct  reason=concat(c.ClogReasonID,'-'+cr.Description)
	    from ClogGarmentDispose_Detail cd
	    inner join ClogGarmentDispose c on c.ID=cd.ID
	    left join ClogReason cr on cr.id = c.ClogReasonID
	    where cd.PackingListID = p.ID
    )rea
    where pd.CTNQty > 0
    and pd.ReceiveDate is not null
    and o.PulloutComplete = 1
    and pd.DisposeFromClog= 0
    and p.PulloutID = ''
");
            sqlcmd.Append(sqlWHERE);
            sqlcmd.Append(@"
union all
select 
p.MDivisionID
,o.FactoryID
,pd.OrderID
,o.StyleID
,[PackingID] = p.id
,pd.CTNStartNo
,pd.ReceiveDate
,o.CustPONo
,[OriClogLocationId] = iif(pd.CFAReceiveDate is not null,pd.CFALocationID,pd.OriClogLocationID)
,[ClogLocationId] = iif(pd.CFAReceiveDate is not null,pd.CFALocationID,pd.ClogLocationId)
,p.BrandID
,Cancelled = iif(o.junk=1,'Y','N')
,[TTLQty] = TTL.Qty
,[QtyPerSize] = SizeCombo.combo
,pd.id,pd.Seq
,[PulloutComplete] = 'N'
,[ActPulloutDate] = o.ActPulloutDate
,rea.reason
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
outer apply(
	select combo = Stuff((
	    select concat('/',SizeCode+':'+ convert(varchar(10),QtyPerCTN))
	    from(
		    select distinct pd1.SizeCode,pd1.QtyPerCTN
		    from PackingList_Detail pd1
		    where pd1.ID=pd.ID and pd1.CTNStartNo=pd.CTNStartNo
	     )s
	    for xml path('')
	),1,1,'')
) SizeCombo
outer apply(
	select Qty = 
	sum(pp.QtyPerCTN * iif(pp.CTNQty=0,1,pp.CTNQty)) 
	from PackingList_Detail pp
	where pp.ID= pd.ID and pp.CTNStartNo=pd.CTNStartNo
) TTL
outer apply(
	select distinct  reason=concat(c.ClogReasonID,'-'+cr.Description)
	from ClogGarmentDispose_Detail cd
	inner join ClogGarmentDispose c on c.ID=cd.ID
	left join ClogReason cr on cr.id = c.ClogReasonID
	where cd.PackingListID = p.ID
)rea
where pd.CTNQty > 0
and pd.ReceiveDate is not null
and (p.PulloutID = '' or p.PulloutStatus = 'New')
and pd.DisposeFromClog= 0
and o.PulloutComplete = 0
");
            sqlcmd.Append(sqlWHERE);
            sqlcmd.Append(@" )a
order by PulloutComplete desc,ClogLocationId, MDivisionID, FactoryID, OrderID, ID, Seq");

            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            DBProxy.Current.DefaultTimeout = 300; // 改回原本設定
            return Ict.Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
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
            string strXltName = Env.Cfg.XltPathDir + "\\Logistic_R02_ClogAuditList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 2] = this.po1 + " ~ " + this.po2;
            worksheet.Cells[3, 2] = this.sp1 + " ~ " + this.sp2;
            worksheet.Cells[4, 2] = this.location1 + " ~ " + this.location2;
            worksheet.Cells[2, 10] = this.brand;
            worksheet.Cells[3, 10] = this.mDivision;

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, strXltName, 5, false, null, excel); // 將datatable copy to excel

            // 此欄位只有 Clog R02 擁有 Confirm 權限的使用者可以『看到』，其餘的則移除該欄位。
            if (!this.Perm.Confirm)
            {
                worksheet.get_Range("M:P").EntireColumn.Delete();
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Logistic_R02_ClogAuditList");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
