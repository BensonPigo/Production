using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        private DateTime? pulloutDate1;
        private DateTime? pulloutDate2;
        private string brand;
        private string mDivision;
        private string factory;
        private string category;
        private DataTable printData;

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Pullout Date can't empty!!");
            //    return false;
            // }
            this.mDivision = this.comboM.Text;
            this.pulloutDate1 = this.datePulloutDate.Value1;
            this.pulloutDate2 = this.datePulloutDate.Value2;
            this.brand = this.txtbrand.Text;
            this.factory = this.comboFactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            string sqlWhere = string.Empty;
            List<string> sqlList = new List<string>();

            #region 組Where 條件
            if (!MyUtility.Check.Empty(this.pulloutDate1))
            {
                sqlList.Add(string.Format(" p.PulloutDate >= '{0}' ", Convert.ToDateTime(this.pulloutDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.pulloutDate2))
            {
                sqlList.Add(string.Format(" p.PulloutDate <= '{0}' ", Convert.ToDateTime(this.pulloutDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlList.Add(string.Format(" o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlList.Add(string.Format(" o.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlList.Add(string.Format(" o.FtyGroup = '{0}'", this.factory));
            }

            sqlList.Add($" o.Category in ({this.category})");

            #endregion

            sqlWhere = string.Join(" and ", sqlList);
            if (!MyUtility.Check.Empty(sqlWhere))
            {
                sqlWhere = " and " + sqlWhere;
            }

            sqlCmd.Append(string.Format(
@"
select distinct p.PulloutDate,oq.BuyerDelivery,pd.OrderID,isnull(o.CustPONo,'') as CustPONo,
	isnull(o.StyleID,'') as StyleID,isnull(oq.Qty,0) as Qty,
	[ShipQty]= pd.ShipQty,
	IIF(ct.WorkType = '1','Y','') as byCombo,
	case pd.Status when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed'when 'S' then 'Shortage' else '' end as StatusExp,
	isnull(IIF(o.LocalOrder = 1, o.PoPrice,o.CMPPrice),0) as CMP,	
	o.LocalOrder,o.CPU,o.CPUFactor,
	isnull(o.PoPrice,0) as PoPrice,
	isnull(o.PoPrice,0)*pd.ShipQty as FOBAmt, 
	isnull(o.BrandID,'') as BrandID,
	isnull(o.MDivisionID,'') as MDivisionID,
	isnull(o.FactoryID,'') as FactoryID,
	isnull(oq.ShipmodeID,'') as ShipmodeID,
	isnull(c.Alias,'') as Alias,
	isnull(IIF(ct.WorkType = '1',(select sum(cw.Qty) from CuttingOutput_WIP cw WITH (NOLOCK) , Orders os WITH (NOLOCK) where cw.OrderID = os.ID and os.CuttingSP = o.CuttingSP),
	(select sum(Qty) from CuttingOutput_WIP WITH (NOLOCK) where OrderID = pd.OrderID)),0) as CutQty
	into #temp1
	from Pullout p WITH (NOLOCK) 
	inner join Pullout_Detail pd WITH (NOLOCK) on p.ID = pd.ID
	left join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
	left join Order_QtyShip oq WITH (NOLOCK) on pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
	left join Country c WITH (NOLOCK) on o.Dest = c.ID
	left join Cutting ct WITH (NOLOCK) on ct.ID = o.CuttingSP
	where p.Status <> 'New'
	and 1=1" + sqlWhere + @"  
	

	select PulloutDate,BuyerDelivery,OrderID,CustPONo,StyleID,Qty,
[ShipQty]= sum(ShipQty),
byCombo,StatusExp ,CMP
,PoPrice,FOBAmt,BrandID,MDivisionID,
FactoryID,ShipmodeID,Alias,CutQty,
LocalOrder,CPU,CPUFactor  
into #temp2
	from #temp1
	group by PulloutDate,BuyerDelivery,OrderID,CustPONo,StyleID,Qty,byCombo,StatusExp,CMP,PoPrice,FOBAmt,BrandID,MDivisionID,
FactoryID,ShipmodeID,Alias,CutQty,LocalOrder,CPU,CPUFactor  

select *, isnull(IIF(LocalOrder = 1, Round(PoPrice * ShipQty,3),Round(CPU * CPUFactor * ShipQty,3)),0) as CMPAmt
from #temp2
order by PulloutDate,OrderID

drop table #temp1,#temp2
"));

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
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
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_R03_ActualShipmentRecord.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 17];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["PulloutDate"];
                objArray[0, 1] = dr["BuyerDelivery"];
                objArray[0, 2] = dr["OrderID"];
                objArray[0, 3] = dr["CustPONo"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["Qty"];
                objArray[0, 6] = dr["CutQty"];
                objArray[0, 7] = dr["byCombo"];
                objArray[0, 8] = dr["ShipQty"];
                objArray[0, 9] = dr["StatusExp"];

                // objArray[0, 10] = dr["CMP"];
                // objArray[0, 10] = dr["CMPAmt"];
                objArray[0, 10] = dr["PoPrice"];
                objArray[0, 11] = dr["FOBAmt"];
                objArray[0, 12] = dr["BrandID"];
                objArray[0, 13] = dr["MDivisionID"];
                objArray[0, 14] = dr["FactoryID"];
                objArray[0, 15] = dr["ShipmodeID"];
                objArray[0, 16] = dr["Alias"];
                worksheet.Range[string.Format("A{0}:Q{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R03_ActualShipmentRecord");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
