using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


namespace Sci.Production.Warehouse
{
    public partial class R22 : Sci.Win.Tems.PrintForm
    {
        string strSP1, strM, strFactory, strSP2, strRefno1, strRefno2, strColor1, strColor2, strSupp;

        private void chkQtyless0_CheckedChanged(object sender, EventArgs e)
        {
            if (chkQtyOver0.Checked)
            {
                chkQtyless0.Checked = false;
            }           
        }

        private void chkQtyOver0_CheckedChanged(object sender, EventArgs e)
        {
            if (chkQtyless0.Checked)
            {
                chkQtyOver0.Checked = false;
            }
        }

        DataTable dataTable;

        public R22(ToolStripMenuItem menuitem) 
            :base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            strSP1 = txtSPNoStart.Text.Trim();
            strSP2 = txtSPNoEnd.Text.Trim();
            strM = txtMdivision1.Text.Trim();
            strFactory = txtfactory.Text.Trim();
            strRefno1 = txtRefnoStart.Text.Trim();
            strRefno2 = txtRefnoEnd.Text.Trim();
            strColor1 = txtThreadColorStart.Text.Trim();
            strColor2 = txtThreadColorStart.Text.Trim();
            strSupp = txtSupplier.TextBox1.Text.Trim();
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region Set SQL Command & SQLParameter
            string strSql = @"
select	o.MDivisionID
		, [Factory] = O.FactoryID
		, [SP#] = Linv.OrderID
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
            //if (!strSP1.Empty() && !strSP2.Empty())
            //{
            //    //若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料                
            //    listPar.Add(new SqlParameter("@SP1", strSP1.PadRight(10, '0')));
            //    listPar.Add(new SqlParameter("@SP2", strSP2.PadRight(10, 'Z')));
            //    listWhere.Add(" Linv.OrderID between @SP1 and @SP2 ");
            //}
            if (!strSP1.Empty())
            {
                //只有 sp1 輸入資料
                listPar.Add(new SqlParameter("@SP1", strSP1));
                listWhere.Add(" (Linv.OrderID like @SP1+'%' or Linv.OrderID >= @SP1)  ");
            }
            if (!strSP2.Empty())
            {
                //只有 sp2 輸入資料
                listPar.Add(new SqlParameter("@SP2", strSP2 ));
                listWhere.Add(" (Linv.OrderID like @SP2+'%' or Linv.OrderID <= @SP2) ");
            }

            /*--- Refno ---*/
            if (!strRefno1.Empty() && !strColor2.Empty())
            {
                listPar.Add(new SqlParameter("@Refno1", strRefno1));
                listPar.Add(new SqlParameter("@Refno2", strRefno2));
                listWhere.Add(" Linv.Refno between @Refno1 and @Refno2 ");
            }
            else if (!strRefno1.Empty())
            {
                listPar.Add(new SqlParameter("@Refno1", strRefno1 + "%"));
                listWhere.Add(" Linv.Refno like @Refno1 ");
            }
            else if (!strRefno2.Empty())
            {
                listPar.Add(new SqlParameter("@Refno2", strRefno2 + "%"));
                listWhere.Add(" Linv.Refno like @Refno2 ");
            }

            /*--- Thread Color ---*/
            if (!strColor1.Empty() && !strColor2.Empty())
            {
                listPar.Add(new SqlParameter("@Color1", strColor1));
                listPar.Add(new SqlParameter("@Color2", strColor2));
                listWhere.Add(" Linv.ThreadColorID between @Color1 and @Color2 ");
            } else if (!strColor1.Empty())
            {
                listPar.Add(new SqlParameter("@Color1", strColor1 + "%"));
                listWhere.Add(" Linv.ThreadColorID like @Color1 ");
            } else if (!strColor2.Empty())
            {
                listPar.Add(new SqlParameter("@Colro2", strColor2 + "%"));
                listWhere.Add(" Linv.ThreadColorID like @Color2 ");
            }

            /*--- Supplier ---*/
            if (!strSupp.Empty())
            {
                listPar.Add(new SqlParameter("@Supp", strSupp));
                listWhere.Add(" LSupp.ID = @Supp ");
            }

            /*--- Factory ---*/
            if (!strFactory.Empty())
            {
                listPar.Add(new SqlParameter("@factory", strFactory));
                listWhere.Add(" O.FactoryID = @factory ");
            }

            /*--- Factory ---*/
            if (!strM.Empty())
            {
                listPar.Add(new SqlParameter("@strM", strM));
                listWhere.Add(" o.MDivisionID = @strM ");
            }

            if (chkQtyOver0.Checked)
            {
                listWhere.Add(" ((Linv.InQty - Linv.OutQty + Linv.AdjustQty) > 0 or LObQty > 0)");
            }

            if (chkQtyless0.Checked)
            {
                listWhere.Add(" ((Linv.InQty - Linv.OutQty + Linv.AdjustQty) < 0)");
            }

            if (listWhere.Count > 0)
                strSql += "where" + listWhere.JoinToString("and");
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

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R22.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dataTable, null, "Warehouse_R22.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
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
            } else
            {
                this.SetCount(0);
                MyUtility.Msg.InfoBox("Data not found!!");
            }
            return true;
        }
    }
}
