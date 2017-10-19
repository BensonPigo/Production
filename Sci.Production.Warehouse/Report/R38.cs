using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R38 : Sci.Win.Tems.PrintForm
    {
        string strSp1, strSp2, strM, strFty, strStockType;
        DataTable dataTable;
        public R38(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
            Dictionary < string, string> stocktype_source =  new Dictionary<string, string> {
                {"ALL","*" },
                {"Bulk","B" },
                {"Inventory","I" },
                {"Scrap","O" }  };
            comboStockType.DataSource = new BindingSource(stocktype_source, null);
            comboStockType.DisplayMember = "Key";
            comboStockType.ValueMember = "Value";

            comboStockType.SelectedIndex = 0;

            txtMdivision.Text = Env.User.Keyword;
        }

        protected override bool ValidateInput()
        {
            
            strSp1 = txtSPNoStart.Text.Trim();
            strSp2 = txtSPNoEnd.Text.Trim();
            strM = txtMdivision.Text.Trim();
            strFty = txtfactory.Text.Trim();
            strStockType = comboStockType.SelectedValue.ToString().Trim();
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string strSql = @"
select	fi.POID
        ,fi.Seq1
        ,fi.Seq2
        ,fi.Roll
        ,fi.Dyelot
        ,'Locked' Lock
        ,fi.LockDate
        ,fi.LockName
        ,fi.InQty
        ,fi.OutQty
        ,fi.AdjustQty
        ,Balance = fi.InQty - fi.OutQty + fi.AdjustQty
        ,fi.StockType
        ,fid.MtlLocationID
        ,Description = dbo.getMtlDesc(fi.POID, fi.Seq1,fi.Seq2,2,0) 
        ,o.StyleID
        ,pd. ColorID
        ,BuyerDelivery = min(o.BuyerDelivery) 
        ,SciDelivery = min(o.SciDelivery)
        ,o.BrandID
        ,o. FactoryID
        from FtyInventory  fi WITH (NOLOCK)
        left join FtyInventory_Detail fid  WITH (NOLOCK) on fi.Ukey = fid.Ukey
        left join Orders o WITH (NOLOCK)  on fi.POID = o.ID
        left join PO_Supp_Detail pd   WITH (NOLOCK) on fi.POID = pd.ID and fi.Seq1 = pd.Seq1 and fi.Seq2 = pd.Seq2
";

            List<string> listWhere = new List<string>();
            List<SqlParameter> listPar = new List<SqlParameter>();

            /*--- SP# ---*/
      
            if (!strSp1.Empty())
            {
                //只有 sp1 輸入資料
                listPar.Add(new SqlParameter("@SP1", strSp1));
                listWhere.Add(" (fi.POID like @SP1+'%' or fi.POID >= @SP1)  ");
            }
            if (!strSp2.Empty())
            {
                //只有 sp2 輸入資料
                listPar.Add(new SqlParameter("@SP2", strSp2));
                listWhere.Add(" (fi.POID like @SP2+'%' or fi.POID <= @SP2) ");
            }

            /*--- M ---*/
            if (!strM.Empty())
            {
                listPar.Add(new SqlParameter("@M", strM));
                listWhere.Add(" o.MDivisionID = @M ");
            }

            /*--- factory ---*/
            if (!strFty.Empty())
            {
                listPar.Add(new SqlParameter("@Fty", strFty));
                listWhere.Add(" o.FactoryID = @Fty ");
            }

            /*--- stock type ---*/
            if (!strStockType.Equals("*"))
            {
                listPar.Add(new SqlParameter("@stocktype", strStockType));
                listWhere.Add(" fi.StockType  = @stocktype ");
            }

          
            if (listWhere.Count > 0)
                strSql += "where" + listWhere.JoinToString("and") +
                          @" and fi.Lock = '1' 
                            group by 
		                    fi.POID
                            ,fi.Seq1
                            ,fi.Seq2
                            ,fi.Roll
                            ,fi.Dyelot
                            ,fi.LockDate
                            ,fi.LockName
                            ,fi.InQty
                            ,fi.OutQty
                            ,fi.AdjustQty
                            ,fi.InQty - fi.OutQty + fi.AdjustQty 
                            ,fi.StockType
                            ,fid.MtlLocationID
                            ,dbo.getMtlDesc(fi.POID, fi.Seq1,fi.Seq2,2,0) 
                            ,o.StyleID
                            ,pd. ColorID
		                    ,o.BrandID
                            ,o. FactoryID";
            #endregion 
            #region SQL Data Loading...
            DualResult result = DBProxy.Current.Select(null, strSql, listPar, out dataTable);
            #endregion

            if (result) return Result.True;
            else return new DualResult(false, "Query data fail\r\n" + result.ToString());
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                this.SetCount(dataTable.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R38.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dataTable, null, "Warehouse_R38.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R38");
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
