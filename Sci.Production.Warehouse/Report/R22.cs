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
    public partial class R22 : Win.Tems.PrintForm
    {
        string strSP1;
        string strM;
        string strFactory;
        string strSP2;
        string strRefno1;
        string strRefno2;
        string strColor1;
        string strColor2;
        string strSupp;

        private void chkQtyless0_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkQtyOver0.Checked)
            {
                this.chkQtyless0.Checked = false;
            }
        }

        private void chkQtyOver0_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkQtyless0.Checked)
            {
                this.chkQtyOver0.Checked = false;
            }
        }

        DataTable dataTable;

        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            this.strSP1 = this.txtSPNoStart.Text.Trim();
            this.strSP2 = this.txtSPNoEnd.Text.Trim();
            this.strM = this.txtMdivision1.Text.Trim();
            this.strFactory = this.txtfactory.Text.Trim();
            this.strRefno1 = this.txtRefnoStart.Text.Trim();
            this.strRefno2 = this.txtRefnoEnd.Text.Trim();
            this.strColor1 = this.txtThreadColorStart.Text.Trim();
            this.strColor2 = this.txtThreadColorStart.Text.Trim();
            this.strSupp = this.txtSupplier.TextBox1.Text.Trim();
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string strSql = @"
select	o.MDivisionID
		, [Factory] = O.FactoryID
		, [SP#] = Linv.OrderID
        , o.OrderTypeID
		, o.BrandID
		, o.StyleID
		, o.SeasonID
		, o.ProjectID
		, o.ProgramID
		, Linv.Refno
		, Litem.Description
		, Linv.ThreadColorID
		, [Unit] = Linv.UnitID
		, [Supplier] = LSupp.ID + '-' + LSupp.Abb 
        , [Category]=Litem.Category
		, [Arrived Qty] = Linv.InQty
		, [Released Qty] = Linv.OutQty
		, Linv.AdjustQty
 		, [Balance] = InQty - OutQty + AdjustQty
		, Linv.LObQty
		, Linv.ALocation
		, Linv.CLocation
from LocalInventory Linv 
left join LocalItem Litem on Linv.Refno = Litem.RefNo
left join LocalSupp LSupp on Litem.LocalSuppid = LSupp.ID
left join Orders O ON Linv.OrderID = O.ID
";

            List<string> listWhere = new List<string>();
            List<SqlParameter> listPar = new List<SqlParameter>();

            /*--- SP# ---*/

            // if (!strSP1.Empty() && !strSP2.Empty())
            // {
            //    //若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
            //    listPar.Add(new SqlParameter("@SP1", strSP1.PadRight(10, '0')));
            //    listPar.Add(new SqlParameter("@SP2", strSP2.PadRight(10, 'Z')));
            //    listWhere.Add(" Linv.OrderID between @SP1 and @SP2 ");
            // }
            if (!this.strSP1.Empty())
            {
                // 只有 sp1 輸入資料
                listPar.Add(new SqlParameter("@SP1", this.strSP1));
                listWhere.Add(" (Linv.OrderID like @SP1+'%' or Linv.OrderID >= @SP1)  ");
            }

            if (!this.strSP2.Empty())
            {
                // 只有 sp2 輸入資料
                listPar.Add(new SqlParameter("@SP2", this.strSP2));
                listWhere.Add(" (Linv.OrderID like @SP2+'%' or Linv.OrderID <= @SP2) ");
            }

            /*--- Refno ---*/
            if (!this.strRefno1.Empty() && !this.strColor2.Empty())
            {
                listPar.Add(new SqlParameter("@Refno1", this.strRefno1));
                listPar.Add(new SqlParameter("@Refno2", this.strRefno2));
                listWhere.Add(" Linv.Refno between @Refno1 and @Refno2 ");
            }
            else if (!this.strRefno1.Empty())
            {
                listPar.Add(new SqlParameter("@Refno1", this.strRefno1 + "%"));
                listWhere.Add(" Linv.Refno like @Refno1 ");
            }
            else if (!this.strRefno2.Empty())
            {
                listPar.Add(new SqlParameter("@Refno2", this.strRefno2 + "%"));
                listWhere.Add(" Linv.Refno like @Refno2 ");
            }

            /*--- Thread Color ---*/
            if (!this.strColor1.Empty() && !this.strColor2.Empty())
            {
                listPar.Add(new SqlParameter("@Color1", this.strColor1));
                listPar.Add(new SqlParameter("@Color2", this.strColor2));
                listWhere.Add(" Linv.ThreadColorID between @Color1 and @Color2 ");
            }
            else if (!this.strColor1.Empty())
            {
                listPar.Add(new SqlParameter("@Color1", this.strColor1 + "%"));
                listWhere.Add(" Linv.ThreadColorID like @Color1 ");
            }
            else if (!this.strColor2.Empty())
            {
                listPar.Add(new SqlParameter("@Colro2", this.strColor2 + "%"));
                listWhere.Add(" Linv.ThreadColorID like @Color2 ");
            }

            /*--- Supplier ---*/
            if (!this.strSupp.Empty())
            {
                listPar.Add(new SqlParameter("@Supp", this.strSupp));
                listWhere.Add(" LSupp.ID = @Supp ");
            }

            /*--- Factory ---*/
            if (!this.strFactory.Empty())
            {
                listPar.Add(new SqlParameter("@factory", this.strFactory));
                listWhere.Add(" O.FactoryID = @factory ");
            }

            /*--- Factory ---*/
            if (!this.strM.Empty())
            {
                listPar.Add(new SqlParameter("@strM", this.strM));
                listWhere.Add(" o.MDivisionID = @strM ");
            }

            if (this.chkQtyOver0.Checked)
            {
                listWhere.Add(" ((Linv.InQty - Linv.OutQty + Linv.AdjustQty) > 0 or LObQty > 0)");
            }

            if (this.chkQtyless0.Checked)
            {
                listWhere.Add(" ((Linv.InQty - Linv.OutQty + Linv.AdjustQty) < 0)");
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
                return Result.True;
            }
            else
            {
                return new DualResult(false, "Query data fail\r\n" + result.ToString());
            }
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dataTable != null && this.dataTable.Rows.Count > 0)
            {
                this.SetCount(this.dataTable.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R22.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dataTable, null, "Warehouse_R22.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R22");
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
