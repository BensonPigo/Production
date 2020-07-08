using System;
using System.Data;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P03_Print
    /// </summary>
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        private string reportType;
        private string eta1;
        private string eta2;
        private string factory;
        private string shipmode;
        private string handle;
        private string ext;
        private string email;
        private DataRow masterData;
        private DataTable detailData;
        private DataTable printData;

        /// <summary>
        /// P03_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="detailData">detailData</param>
        public P03_Print(DataRow masterData, DataTable detailData)
        {
            this.InitializeComponent();
            this.radioDetailReport.Checked = true;
            this.dateETA.Enabled = false;
            this.txtfactory.Enabled = false;
            this.masterData = masterData;
            this.detailData = detailData;

            DataTable dt;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from ShipMode WITH (NOLOCK) where UseFunction like '%WK%' ", out dt);
            MyUtility.Tool.SetupCombox(this.comboBox1, 1, dt);
            this.comboBox1.SelectedIndex = 0;
        }

        // 控制ETA & Factory可否輸入
        private void RadioListReport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioListReport.Checked)
            {
                this.dateETA.Enabled = true;
                this.txtfactory.Enabled = true;
                this.comboBox1.Enabled = true;
            }
            else
            {
                this.dateETA.Enabled = false;
                this.txtfactory.Enabled = false;
                this.comboBox1.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.reportType = this.radioDetailReport.Checked ? "1" : "2";
            this.eta1 = MyUtility.Check.Empty(this.dateETA.Value1) ? string.Empty : Convert.ToDateTime(this.dateETA.Value1).ToString("d");
            this.eta2 = MyUtility.Check.Empty(this.dateETA.Value2) ? string.Empty : Convert.ToDateTime(this.dateETA.Value2).ToString("d");
            this.factory = this.txtfactory.Text;
            this.shipmode = this.comboBox1.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "1")
            {
                string sqlCmd = string.Format("select * from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.masterData["Handle"]));
                DataTable tPEPass1;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out tPEPass1);
                if (!result || tPEPass1.Rows.Count <= 0)
                {
                    this.handle = string.Empty;
                    this.ext = string.Empty;
                    this.email = string.Empty;
                }
                else
                {
                    this.handle = MyUtility.Convert.GetString(tPEPass1.Rows[0]["Name"]);
                    this.ext = MyUtility.Convert.GetString(tPEPass1.Rows[0]["ExtNo"]);
                    this.email = MyUtility.Convert.GetString(tPEPass1.Rows[0]["EMail"]);
                }
            }
            else
            {
                string sqlCmd = string.Format(
                    @"select e.ID,e.Eta,e.Blno,e.InvNo,e.PackingArrival,e.PortArrival,e.WhseArrival,e.DocArrival,e.Sono,e.Vessel,isnull(t.Name,'') as Name,isnull(t.ExtNo,'') as ExtNo,isnull(t.EMail,'') as EMail ,
case when e.Payer= 'S' then 'By Sci Taipei Office(Sender)' when e.Payer= 'M' then 'By Mill(Sender)' when e.Payer= 'F' then 'By Factory(Receiver)' else '' end as Payer
,[Loading] = e.ExportPort+'-'+e.ExportCountry,e.shipmodeID
from Export e WITH (NOLOCK) 
left join TPEPass1 t WITH (NOLOCK) on e.Handle = t.ID
where 1=1 and e.Junk = 0 {0}{1}{2}{3}
order by e.ID",
                    MyUtility.Check.Empty(this.eta1) ? string.Empty : " and e.Eta >= '" + this.eta1 + "'",
                    MyUtility.Check.Empty(this.eta2) ? string.Empty : " and e.Eta <= '" + this.eta2 + "'",
                    MyUtility.Check.Empty(this.factory) ? string.Empty : " and e.FactoryID = '" + this.factory + "'",
                    MyUtility.Check.Empty(this.shipmode) ? string.Empty : " and e.shipmodeID = '" + this.shipmode + "'");

                DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.reportType == "1")
            {
                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P03_Detail.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 2] = MyUtility.Convert.GetString(this.masterData["ID"]);
                worksheet.Cells[2, 6] = MyUtility.Convert.GetString(this.masterData["INVNo"]);
                worksheet.Cells[2, 9] = MyUtility.Check.Empty(this.masterData["PackingArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["PackingArrival"]).ToString("d");
                worksheet.Cells[3, 2] = MyUtility.Check.Empty(this.masterData["Eta"]) ? string.Empty : Convert.ToDateTime(this.masterData["Eta"]).ToString("d");
                worksheet.Cells[3, 6] = MyUtility.Convert.GetString(this.masterData["Payer"]) == "S" ? "By Sci Taipei Office(Sender)" : MyUtility.Convert.GetString(this.masterData["Payer"]) == "M" ? "By Mill(Sender)" : MyUtility.Convert.GetString(this.masterData["Payer"]) == "F" ? "By Factory(Receiver)" : string.Empty;
                worksheet.Cells[3, 9] = MyUtility.Check.Empty(this.masterData["PortArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["PortArrival"]).ToString("d");
                worksheet.Cells[4, 2] = MyUtility.Convert.GetString(this.masterData["Consignee"]);
                worksheet.Cells[4, 6] = MyUtility.Convert.GetString(this.masterData["Blno"]);
                worksheet.Cells[4, 9] = MyUtility.Check.Empty(this.masterData["WhseArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["WhseArrival"]).ToString("d");
                worksheet.Cells[5, 2] = MyUtility.Convert.GetString(this.masterData["Packages"]);
                worksheet.Cells[5, 6] = MyUtility.Convert.GetString(this.masterData["Vessel"]);
                worksheet.Cells[5, 9] = MyUtility.Check.Empty(this.masterData["DocArrival"]) ? string.Empty : Convert.ToDateTime(this.masterData["DocArrival"]).ToString("d");
                worksheet.Cells[6, 2] = MyUtility.Convert.GetString(this.masterData["CYCFS"]);
                worksheet.Cells[6, 6] = MyUtility.Convert.GetString(this.masterData["NetKg"]) + " / " + MyUtility.Convert.GetString(this.masterData["WeightKg"]);
                worksheet.Cells[7, 2] = MyUtility.Convert.GetString(this.masterData["Sono"]);
                worksheet.Cells[7, 6] = MyUtility.Convert.GetString(this.masterData["Cbm"]);
                worksheet.Cells[8, 2] = MyUtility.Convert.GetString(this.masterData["ExportPort"] + "-" + this.masterData["ExportCountry"]);
                worksheet.Cells[8, 6] = MyUtility.Convert.GetString(this.masterData["Remark"]);
                worksheet.Cells[9, 2] = this.handle;
                worksheet.Cells[9, 6] = this.ext;
                worksheet.Cells[9, 8] = this.email;

                int rownum = 11;
                object[,] objArray = new object[1, 18];
                foreach (DataRow dr in this.detailData.Rows)
                {
                    objArray[0, 0] = dr["FactoryID"];
                    objArray[0, 1] = dr["ProjectID"];
                    objArray[0, 2] = dr["POID"];
                    objArray[0, 3] = dr["SCIDlv"];
                    objArray[0, 4] = dr["Category"];
                    objArray[0, 5] = dr["InspDate"];
                    objArray[0, 6] = dr["Seq"];
                    objArray[0, 7] = dr["Preshrink"];
                    objArray[0, 8] = dr["Supp"];
                    objArray[0, 9] = dr["Description"];
                    objArray[0, 10] = dr["UnitId"];
                    objArray[0, 11] = dr["ColorID"];
                    objArray[0, 12] = dr["SizeSpec"];
                    objArray[0, 13] = dr["Qty"];
                    objArray[0, 14] = dr["Foc"];
                    objArray[0, 15] = dr["BalanceQty"];
                    objArray[0, 16] = dr["NetKg"];
                    objArray[0, 17] = dr["WeightKg"];
                    worksheet.Range[string.Format("A{0}:R{0}", rownum)].Value2 = objArray;

                    rownum++;
                }

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P03_Detail");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.printData.Rows.Count);

                if (this.printData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P03_List.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                worksheet.Cells[2, 2] = (MyUtility.Check.Empty(this.eta1) ? string.Empty : this.eta1) + " ~ " + (MyUtility.Check.Empty(this.eta2) ? string.Empty : this.eta2);
                worksheet.Cells[2, 7] = MyUtility.Check.Empty(this.factory) ? "All" : this.factory;

                int rownum = 4;
                object[,] objArray = new object[1, 14];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["shipmodeID"];
                    objArray[0, 2] = dr["Loading"];
                    objArray[0, 3] = dr["Eta"];
                    objArray[0, 4] = dr["Blno"];
                    objArray[0, 5] = dr["Vessel"];
                    objArray[0, 6] = dr["InvNo"];
                    objArray[0, 7] = dr["Payer"];
                    objArray[0, 8] = dr["PackingArrival"];
                    objArray[0, 9] = dr["PortArrival"];
                    objArray[0, 10] = dr["WhseArrival"];
                    objArray[0, 11] = dr["DocArrival"];
                    objArray[0, 12] = dr["Name"];
                    objArray[0, 13] = dr["ExtNo"];
                    worksheet.Range[string.Format("A{0}:N{0}", rownum)].Value2 = objArray;

                    rownum++;
                }

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.EntireRow.AutoFit();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_P03_List");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }
    }
}
