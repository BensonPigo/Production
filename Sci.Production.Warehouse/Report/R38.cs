using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// R38
    /// </summary>
    public partial class R38 : Win.Tems.PrintForm
    {
        private string strSp1;
        private string strSp2;
        private string strM;
        private string strFty;
        private string strStockType;
        private string strLockStatus;
        private DataTable dataTable;

        /// <summary>
        /// R38
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R38(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            Dictionary<string, string> stocktype_source = new Dictionary<string, string>
            {
                { "ALL", "*" },
                { "Bulk", "B" },
                { "Inventory", "I" },
                { "Scrap", "O" },
            };
            this.comboStockType.DataSource = new BindingSource(stocktype_source, null);
            this.comboStockType.DisplayMember = "Key";
            this.comboStockType.ValueMember = "Value";

            this.comboStockType.SelectedIndex = 0;

            // Status下拉選單
            Dictionary<string, string> status_source = new Dictionary<string, string>
            {
                { "ALL", "*" },
                { "Lock", "1" },
                { "UnLock", "0" },
            };

            this.comboStatus.DataSource = new BindingSource(status_source, null);
            this.comboStatus.DisplayMember = "Key";
            this.comboStatus.ValueMember = "Value";
            this.comboStatus.SelectedIndex = 0;

            this.txtMdivision.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strSp1 = this.txtSPNoStart.Text.Trim();
            this.strSp2 = this.txtSPNoEnd.Text.Trim();
            this.strM = this.txtMdivision.Text.Trim();
            this.strFty = this.txtfactory.Text.Trim();
            this.strStockType = this.comboStockType.SelectedValue.ToString().Trim();
            this.strLockStatus = this.comboStatus.SelectedValue.ToString().Trim();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNoStart.Text.Trim()) || MyUtility.Check.Empty(this.txtSPNoEnd.Text.Trim()))
            {
                return new DualResult(false, "<SP#> can't be empty!!");
            }

            #region Set SQL Command & SQLParameter
            string strSql = @"
select	fi.POID
        ,fi.Seq1
        ,fi.Seq2
        ,fi.Roll
        ,fi.Dyelot
        ,'Locked' Lock
        ,fi.LockDate
        ,LockName  = (select id+'-'+name from dbo.pass1 WITH (NOLOCK) where id=fi.LockName) 
        ,fi.Remark
        ,fi.InQty
        ,fi.OutQty
        ,fi.AdjustQty
        ,fi.ReturnQty
        ,Balance = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty
        ,case   when fi.StockType = 'B' then 'Bulk'
                when fi.StockType = 'I' then 'Inventory'
                when fi.StockType = 'O' then 'Scrap'
                else fi.StockType end as StockType
        ,MtlLocationID = dbo.Getlocation(fi.ukey)
        ,Description = dbo.getMtlDesc(fi.POID, fi.Seq1,fi.Seq2,2,0) 
        ,o.StyleID
        ,pd. ColorID
        ,BuyerDelivery = x.earliest_BuyerDelivery
        ,SciDelivery = x.earliest_SciDelivery
        ,o.BrandID
        ,o. FactoryID
        from FtyInventory  fi WITH (NOLOCK)
        left join FtyInventory_Detail fid  WITH (NOLOCK) on fi.Ukey = fid.Ukey
        left join Orders o WITH (NOLOCK)  on fi.POID = o.ID
        left join PO_Supp_Detail pd   WITH (NOLOCK) on fi.POID = pd.ID and fi.Seq1 = pd.Seq1 and fi.Seq2 = pd.Seq2
        cross apply
        (
        	select  earliest_BuyerDelivery = min(o1.BuyerDelivery)  
                    , earliest_SciDelivery = min(o1.SciDelivery)  
        	from dbo.orders o1 WITH (NOLOCK) 
            where o1.POID = fi.POID and o1.Junk = 0
        ) x
";

            List<string> listWhere = new List<string>();
            List<SqlParameter> listPar = new List<SqlParameter>();

            /*--- SP# ---*/

            if (!this.strSp1.Empty())
            {
                // 只有 sp1 輸入資料
                listPar.Add(new SqlParameter("@SP1", this.strSp1));
                listWhere.Add(" (fi.POID like @SP1+'%' or fi.POID >= @SP1)  ");
            }

            if (!this.strSp2.Empty())
            {
                // 只有 sp2 輸入資料
                listPar.Add(new SqlParameter("@SP2", this.strSp2));
                listWhere.Add(" (fi.POID like @SP2+'%' or fi.POID <= @SP2) ");
            }

            /*--- M ---*/
            if (!this.strM.Empty())
            {
                listPar.Add(new SqlParameter("@M", this.strM));
                listWhere.Add(" o.MDivisionID = @M ");
            }

            /*--- factory ---*/
            if (!this.strFty.Empty())
            {
                listPar.Add(new SqlParameter("@Fty", this.strFty));
                listWhere.Add(" o.FactoryID = @Fty ");
            }

            /*--- stock type ---*/
            if (!this.strStockType.Equals("*"))
            {
                listPar.Add(new SqlParameter("@stocktype", this.strStockType));
                listWhere.Add(" fi.StockType  = @stocktype ");
            }

            /*--- Lock status ---*/
            if (!this.strLockStatus.Equals("*"))
            {
                listPar.Add(new SqlParameter("@LockStatus", this.strLockStatus));
                listWhere.Add(" fi.Lock  = @LockStatus ");
            }

            if (listWhere.Count > 0)
            {
                strSql += "where" + listWhere.JoinToString("and");
            }
            #endregion
            #region SQL Data Loading...
            DualResult result = DBProxy.Current.Select(null, strSql, listPar, out this.dataTable);
            #endregion

            if (result)
            {
                return Ict.Result.True;
            }
            else
            {
                return new DualResult(false, "Query data fail\r\n" + result.ToString());
            }
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dataTable != null && this.dataTable.Rows.Count > 0)
            {
                this.SetCount(this.dataTable.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R38.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dataTable, null, "Warehouse_R38.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R38");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
            }
            else
            {
                this.SetCount(0);
                MyUtility.Msg.InfoBox("Data not found!!");
            }

            return true;
        }
    }
}
