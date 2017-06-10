using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Thread
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        string sp1; string sp2; DateTime? EstBook1; DateTime? EstBook2; DateTime? EstArr1; DateTime? EstArr2; string fac; string M;
        List<SqlParameter> lis;
        DataTable dt; string cmd;
        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory = null;
            string sqlcmd = (@"select DISTINCT FTYGroup FROM DBO.Factory WITH (NOLOCK) ");
            DBProxy.Current.Select("", sqlcmd, out factory);
            factory.Rows.Add(new string[] { "" });
            factory.DefaultView.Sort = "FTYGroup";
            this.comboFactory.DataSource = factory;
            this.comboFactory.ValueMember = "FTYGroup";
            this.comboFactory.DisplayMember = "FTYGroup";
            this.comboFactory.SelectedIndex = 0;
            this.comboFactory.Text = Sci.Env.User.Factory;

            this.comboMDivision.setDefalutIndex(true);
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            bool sp_Empty1 = this.txtSPNoStart.Text.Empty(), sp_Empty2 = this.txtSPNoEnd.Text.Empty(), dateRange1_Empty = !this.dateEstBooking.HasValue, dateRange2_Empty = !this.dateEstArrived.HasValue;
            if (sp_Empty1 && sp_Empty2 && dateRange1_Empty && dateRange2_Empty)
            {
                MyUtility.Msg.ErrorBox("You must enter the SP No,Est.booking,Est.Arrived");

                txtSPNoStart.Focus();

                return false;
            }
            sp1 = txtSPNoStart.Text.ToString();
            sp2 = txtSPNoEnd.Text.ToString();
            EstBook1 = dateEstBooking.Value1;
            EstBook2 = dateEstBooking.Value2;
            EstArr1 = dateEstArrived.Value1;
            EstArr2 = dateEstArrived.Value2;
            fac = comboFactory.SelectedValue.ToString();
            M = comboMDivision.Text.ToString();
            lis = new List<SqlParameter>();
            string sqlWhere = ""; string order = "order by ThreadTypeID,td.ThreadColorID,t.StyleID,t.OrderID";
            List<string> sqlWheres = new List<string>();
            #region --組WHERE--
            if (!MyUtility.Check.Empty(this.txtSPNoStart.Text.ToString()) || !MyUtility.Check.Empty(this.txtSPNoEnd.Text.ToString()))
            {
                if (!MyUtility.Check.Empty(this.txtSPNoStart.Text.ToString()))
                {
                    sqlWheres.Add("t.OrderID >= @spNo1 ");
                    lis.Add(new SqlParameter("@spNo1", sp1));
                }
                if (!MyUtility.Check.Empty(this.txtSPNoEnd.Text.ToString()))
                {
                    sqlWheres.Add("t.OrderID <= @spNo2 ");
                    lis.Add(new SqlParameter("@spNo2", sp2));
                }                         
            }
            if (!MyUtility.Check.Empty(EstBook1) || !MyUtility.Check.Empty(EstBook2))
            {
                if (!MyUtility.Check.Empty(EstBook1))
                {
                    sqlWheres.Add("@EstBook1 <= t.EstBookDate");
                    lis.Add(new SqlParameter("@EstBook1", EstBook1));
                }
                if (!MyUtility.Check.Empty(EstBook2))
                {
                    sqlWheres.Add("t.EstBookDate <= @EstBook2");
                    lis.Add(new SqlParameter("@EstBook2", EstBook2));
                }
            } 
            if (!MyUtility.Check.Empty(EstArr1) || !MyUtility.Check.Empty(EstArr2))
            {
                if (!MyUtility.Check.Empty(EstArr1))
                {
                    sqlWheres.Add("@EstArr1 <= t.EstArriveDate");
                    lis.Add(new SqlParameter("@EstArr1", EstArr1));
                }
                if (!MyUtility.Check.Empty(EstArr2))
                {
                    sqlWheres.Add("t.EstArriveDate <= @EstArr2");
                    lis.Add(new SqlParameter("@EstArr2", EstArr2));
                }
            } 
            if (!this.comboFactory.Text.Empty())
            {
                sqlWheres.Add("t.FactoryID = @fac");
                lis.Add(new SqlParameter("@fac", fac));
                
            }
            if (!this.M.Empty())
            {
                sqlWheres.Add("t.MDivisionID = @M");
                lis.Add(new SqlParameter("@M", M));
            }

            if(sqlWheres.Count > 0)
                sqlWhere = " where t.Status = 'Approved' and " + sqlWheres.JoinToString(" and ");
            #endregion

            cmd = string.Format(@"
             select distinct
               t.FactoryID,t.BrandID,t.StyleID,t.SeasonID,t.EstBookDate,t.EstArriveDate,t.OrderID,o.SciDelivery,o.SewInLine
               ,(select li.ThreadTypeID from dbo.LocalItem li WITH (NOLOCK) where li.RefNo = td.Refno) [ThreadTypeID]
               ,ROUND(td.ConsumptionQty/tdc.OrderQty,3),td.Refno,td.ThreadColorID
               ,(select c.Description from dbo.ThreadColor c WITH (NOLOCK) where c.id = td.ThreadColorID) [color_desc]
               ,tdc.OrderQty,td.TotalQty,td.AllowanceQty,td.UseStockQty,td.PurchaseQty
             from dbo.ThreadRequisition t WITH (NOLOCK) 
             inner join dbo.ThreadRequisition_Detail td WITH (NOLOCK) on td.orderid = t.OrderID
             left join dbo.ThreadRequisition_Detail_Cons tdc WITH (NOLOCK) on td.Ukey=tdc.ThreadRequisition_DetailUkey
             left join dbo.orders o WITH (NOLOCK) on o.id = t.OrderID" + sqlWhere + ' ' + order);

            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            if (!res)
            {
                return res;
            }
            return res;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }


            // 顯示筆數於PrintForm上Count欄位
            SetCount(dt.Rows.Count);

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_R20.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, "", "Thread_R20.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

            this.ShowWaitMessage("Excel Processing...");
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();
            objApp.Visible = true;

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);    //釋放worksheet

            this.HideWaitMessage();
            return true;
        }
    }
    
}
