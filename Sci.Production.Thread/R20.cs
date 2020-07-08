using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Thread
{
    /// <summary>
    /// R20
    /// </summary>
    public partial class R20 : Win.Tems.PrintForm
    {
        private string sp1;
        private string sp2;
        private string EstBook1;
        private string EstBook2;
        private string EstArr1;
        private string EstArr2;
        private string fac;
        private string M;
        private string tableThreadRequisitionStatus;
        private List<SqlParameter> lis;
        private DataTable dt; private string cmd;

        /// <summary>
        /// R20
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory = null;
            string sqlcmd = @"select DISTINCT FTYGroup FROM DBO.Factory WITH (NOLOCK) ";
            DBProxy.Current.Select(string.Empty, sqlcmd, out factory);
            factory.Rows.Add(new string[] { string.Empty });
            factory.DefaultView.Sort = "FTYGroup";
            this.comboFactory.DataSource = factory;
            this.comboFactory.ValueMember = "FTYGroup";
            this.comboFactory.DisplayMember = "FTYGroup";
            this.comboFactory.SelectedIndex = 0;
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.comboBoxStatus.SelectedIndex = 0;

            this.comboMDivision.setDefalutIndex(true);
            this.print.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            bool sp_Empty1 = this.txtSPNoStart.Text.Empty(), sp_Empty2 = this.txtSPNoEnd.Text.Empty(), dateRange1_Empty = !this.dateEstBooking.HasValue, dateRange2_Empty = !this.dateEstArrived.HasValue;
            if (sp_Empty1 && sp_Empty2 && dateRange1_Empty && dateRange2_Empty)
            {
                MyUtility.Msg.ErrorBox("You must enter the SP No,Est.booking,Est.Arrived");

                this.txtSPNoStart.Focus();

                return false;
            }

            this.sp1 = this.txtSPNoStart.Text.ToString();
            this.sp2 = this.txtSPNoEnd.Text.ToString();
            this.EstBook1 = this.dateEstBooking.Value1.Empty() ? string.Empty : ((DateTime)this.dateEstBooking.Value1).ToString("yyyy/MM/dd");
            this.EstBook2 = this.dateEstBooking.Value2.Empty() ? string.Empty : ((DateTime)this.dateEstBooking.Value2).ToString("yyyy/MM/dd");
            this.EstArr1 = this.dateEstArrived.Value1.Empty() ? string.Empty : ((DateTime)this.dateEstArrived.Value1).ToString("yyyy/MM/dd");
            this.EstArr2 = this.dateEstArrived.Value2.Empty() ? string.Empty : ((DateTime)this.dateEstArrived.Value2).ToString("yyyy/MM/dd");
            this.fac = this.comboFactory.SelectedValue.ToString();
            this.M = this.comboMDivision.Text.ToString();
            this.tableThreadRequisitionStatus = this.comboBoxStatus.Text;
            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            string order = "order by ThreadTypeID,td.ThreadColorID,t.StyleID,t.OrderID";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!MyUtility.Check.Empty(this.txtSPNoStart.Text.ToString()) || !MyUtility.Check.Empty(this.txtSPNoEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(this.txtSPNoStart.Text.ToString()))
                {
                    sqlWheres.Add("t.OrderID >= @spNo1 ");
                    this.lis.Add(new SqlParameter("@spNo1", this.sp1));
                }

                if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text.ToString()))
                {
                    sqlWheres.Add("t.OrderID <= @spNo2 ");
                    this.lis.Add(new SqlParameter("@spNo2", this.sp2));
                }
            }

            if (!MyUtility.Check.Empty(this.EstBook1) || !MyUtility.Check.Empty(this.EstBook2))
            {
                if (!MyUtility.Check.Empty(this.EstBook1))
                {
                    sqlWheres.Add("@EstBook1 <= t.EstBookDate");
                    this.lis.Add(new SqlParameter("@EstBook1", this.EstBook1));
                }

                if (!MyUtility.Check.Empty(this.EstBook2))
                {
                    sqlWheres.Add("t.EstBookDate <= @EstBook2");
                    this.lis.Add(new SqlParameter("@EstBook2", this.EstBook2));
                }
            }

            if (!MyUtility.Check.Empty(this.EstArr1) || !MyUtility.Check.Empty(this.EstArr2))
            {
                if (!MyUtility.Check.Empty(this.EstArr1))
                {
                    sqlWheres.Add("@EstArr1 <= t.EstArriveDate");
                    this.lis.Add(new SqlParameter("@EstArr1", this.EstArr1));
                }

                if (!MyUtility.Check.Empty(this.EstArr2))
                {
                    sqlWheres.Add("t.EstArriveDate <= @EstArr2");
                    this.lis.Add(new SqlParameter("@EstArr2", this.EstArr2));
                }
            }

            if (!this.comboFactory.Text.Empty())
            {
                sqlWheres.Add("t.FactoryID = @fac");
                this.lis.Add(new SqlParameter("@fac", this.fac));
            }

            if (!this.M.Empty())
            {
                sqlWheres.Add("t.MDivisionID = @M");
                this.lis.Add(new SqlParameter("@M", this.M));
            }

            if (this.tableThreadRequisitionStatus.EqualString("All") == false)
            {
                sqlWheres.Add("t.Status = @Status");
                this.lis.Add(new SqlParameter("@Status", this.tableThreadRequisitionStatus));
            }

            if (sqlWheres.Count > 0)
            {
                sqlWhere = "where " + sqlWheres.JoinToString($"{Environment.NewLine}and ");
            }
            #endregion

            this.cmd = $@"
select t.FactoryID
	   , t.BrandID
	   , t.StyleID
	   , t.SeasonID
	   , t.EstBookDate
	   , t.EstArriveDate
	   , t.OrderID
       , t.Status
	   , o.SciDelivery
	   , o.SewInLine
       , [ThreadTypeID] = (select li.ThreadTypeID 
       					   from dbo.LocalItem li WITH (NOLOCK) 
       					   where li.RefNo = td.Refno) 
       , ROUND(td.ConsumptionQty / tdc.OrderQty, 3)
       , td.Refno
       , td.ThreadColorID
	   , tc.ThreadColorGroupID
       , [color_desc] = (select c.Description 
       					 from dbo.ThreadColor c WITH (NOLOCK) 
       					 where c.id = td.ThreadColorID) 
       , tdc.OrderQty
       , td.TotalQty
       , td.AllowanceQty
       , EstAllowance = CEILING (td.TotalQty * isnull(est.Allowance,0))
	   , Balance= td.AllowanceQty - CEILING (td.TotalQty * est.Allowance)
       , td.UseStockQty
       , td.PurchaseQty
       , isnull(ts.NewCone,0) NewCone
	   , isnull(ts.UsedCone,0) UsedCone
from dbo.ThreadRequisition t WITH (NOLOCK) 
inner join dbo.ThreadRequisition_Detail td WITH (NOLOCK) on td.orderid = t.OrderID
left join dbo.orders o WITH (NOLOCK) on o.id = t.OrderID
outer apply(
	select Allowance
	from ThreadAllowanceScale  tas WITH (NOLOCK)
	where tas.LowerBound <= td.TotalQty
	and td.TotalQty <= tas.UpperBound
)est
Outer apply
(
  select cast(sum(tdc.OrderQty)as float) as OrderQty
  from(
  select Article,OrderQty
  from [Production].[dbo].[ThreadRequisition_Detail_Cons] WITH (NOLOCK)
  where ThreadRequisition_DetailUkey=td.Ukey
  group by Article,OrderQty
  ) as tdc
) as tdc
outer apply
(
	select sum(NewCone) NewCone,sum(UsedCone)UsedCone 
	from ThreadStock WITH (NOLOCK)
	where Refno=td.Refno and ThreadColorID=td.ThreadColorID
)ts
outer apply(
	select ThreadColorGroupID from ThreadColor tc WITH (NOLOCK)
	where tc.id=td.ThreadColorID
)tc
{sqlWhere}
{order}";

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.dt);
            if (!res)
            {
                return res;
            }

            return res;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R20.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Thread_R20.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Thread_R20");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
