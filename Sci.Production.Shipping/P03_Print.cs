using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        string reportType, eta1, eta2, factory, handle, ext, email;
        DataRow masterData;
        DataTable detailData, printData;
        public P03_Print(DataRow MasterData, DataTable DetailData)
        {
            InitializeComponent();
            radioButton1.Checked = true;
            dateRange1.Enabled = false;
            txtfactory1.Enabled = false;
            masterData = MasterData;
            detailData = DetailData;
        }

        //控制ETA & Factory可否輸入
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                dateRange1.Enabled = true;
                txtfactory1.Enabled = true;
            }
            else
            {
                dateRange1.Enabled = false;
                txtfactory1.Enabled = false;
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioButton1.Checked ? "1" : "2";
            eta1 = MyUtility.Check.Empty(dateRange1.Value1) ? "" : Convert.ToDateTime(dateRange1.Value1).ToString("d");
            eta2 = MyUtility.Check.Empty(dateRange1.Value2) ? "" : Convert.ToDateTime(dateRange1.Value2).ToString("d");
            factory = txtfactory1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1")
            {
                string sqlCmd = string.Format("select * from TPEPass1 where ID = '{0}'", MyUtility.Convert.GetString(masterData["Handle"]));
                DataTable TPEPass1;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out TPEPass1);
                if (!result || TPEPass1.Rows.Count <= 0)
                {
                    handle = "";
                    ext = "";
                    email = "";
                }
                else
                {
                    handle = MyUtility.Convert.GetString(TPEPass1.Rows[0]["Name"]);
                    ext = MyUtility.Convert.GetString(TPEPass1.Rows[0]["ExtNo"]);
                    email = MyUtility.Convert.GetString(TPEPass1.Rows[0]["EMail"]);
                }
            }
            else
            {
                string sqlCmd = string.Format(@"select e.ID,e.Eta,e.Blno,e.InvNo,e.PackingArrival,e.PortArrival,e.WhseArrival,e.DocArrival,e.Sono,e.Vessel,isnull(t.Name,'') as Name,isnull(t.ExtNo,'') as ExtNo,isnull(t.EMail,'') as EMail ,
case when e.Payer= 'S' then 'By Sci Taipei Office(Sender)' when e.Payer= 'M' then 'By Mill(Sender)' when e.Payer= 'F' then 'By Factory(Receiver)' else '' end as Payer
from Export e
left join TPEPass1 t on e.Handle = t.ID
where 1=1{0}{1}{2}
order by e.ID", (MyUtility.Check.Empty(eta1) ? "" : " and e.Eta >= '" + eta1 + "'"), (MyUtility.Check.Empty(eta2) ? "" : " and e.Eta <= '" + eta2 + "'"), (MyUtility.Check.Empty(factory) ? "" : " and e.FactoryID = '" + factory + "'"));
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (reportType == "1")
            {
                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P03_Detail.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                worksheet.Cells[2, 2] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[2, 6] = MyUtility.Convert.GetString(masterData["INVNo"]);
                worksheet.Cells[2, 9] = MyUtility.Check.Empty(masterData["PackingArrival"]) ? "" : Convert.ToDateTime(masterData["PackingArrival"]).ToString("d");
                worksheet.Cells[3, 2] = MyUtility.Check.Empty(masterData["Eta"]) ? "" : Convert.ToDateTime(masterData["Eta"]).ToString("d");
                worksheet.Cells[3, 6] = MyUtility.Convert.GetString(masterData["Payer"]) == "S" ? "By Sci Taipei Office(Sender)" : MyUtility.Convert.GetString(masterData["Payer"]) == "M" ? "By Mill(Sender)" : MyUtility.Convert.GetString(masterData["Payer"]) == "F" ? "By Factory(Receiver)" : "";
                worksheet.Cells[3, 9] = MyUtility.Check.Empty(masterData["PortArrival"]) ? "" : Convert.ToDateTime(masterData["PortArrival"]).ToString("d");
                worksheet.Cells[4, 2] = MyUtility.Convert.GetString(masterData["Consignee"]);
                worksheet.Cells[4, 6] = MyUtility.Convert.GetString(masterData["Blno"]);
                worksheet.Cells[4, 9] = MyUtility.Check.Empty(masterData["WhseArrival"]) ? "" : Convert.ToDateTime(masterData["WhseArrival"]).ToString("d");
                worksheet.Cells[5, 2] = MyUtility.Convert.GetString(masterData["Packages"]);
                worksheet.Cells[5, 6] = MyUtility.Convert.GetString(masterData["Vessel"]);
                worksheet.Cells[5, 9] = MyUtility.Check.Empty(masterData["DocArrival"]) ? "" : Convert.ToDateTime(masterData["DocArrival"]).ToString("d");
                worksheet.Cells[6, 2] = MyUtility.Convert.GetString(masterData["CYCFS"]);
                worksheet.Cells[6, 6] = MyUtility.Convert.GetString(masterData["NetKg"]) + " / " + MyUtility.Convert.GetString(masterData["WeightKg"]);
                worksheet.Cells[7, 2] = MyUtility.Convert.GetString(masterData["Sono"]);
                worksheet.Cells[7, 6] = MyUtility.Convert.GetString(masterData["Cbm"]);
                worksheet.Cells[8, 2] = MyUtility.Convert.GetString(masterData["Remark"]);
                worksheet.Cells[9, 2] = handle;
                worksheet.Cells[9, 6] = ext;
                worksheet.Cells[9, 8] = email;

                int rownum = 11;
                object[,] objArray = new object[1, 17];
                foreach (DataRow dr in detailData.Rows)
                {
                    objArray[0, 0] = dr["FactoryID"];
                    objArray[0, 1] = dr["ProjectID"];
                    objArray[0, 2] = dr["POID"];
                    objArray[0, 3] = dr["SCIDlv"];
                    objArray[0, 4] = dr["Category"];
                    objArray[0, 5] = dr["InspDate"];
                    objArray[0, 6] = dr["Seq"];
                    objArray[0, 7] = dr["Supp"];
                    objArray[0, 8] = dr["Description"];
                    objArray[0, 9] = dr["UnitId"];
                    objArray[0, 10] = dr["ColorID"];
                    objArray[0, 11] = dr["SizeSpec"];
                    objArray[0, 12] = dr["Qty"];
                    objArray[0, 13] = dr["Foc"];
                    objArray[0, 14] = dr["BalanceQty"];
                    objArray[0, 15] = dr["NetKg"];
                    objArray[0, 16] = dr["WeightKg"];
                    worksheet.Range[String.Format("A{0}:Q{0}", rownum)].Value2 = objArray;

                    rownum++;
                }
                excel.Visible = true;
            }
            else
            {
                // 顯示筆數於PrintForm上Count欄位
                SetCount(printData.Rows.Count);

                if (printData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P03_List.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                worksheet.Cells[2, 2] = (MyUtility.Check.Empty(eta1) ? "" : eta1) + " ~ " + (MyUtility.Check.Empty(eta2) ? "" : eta2);
                worksheet.Cells[2, 7] = MyUtility.Check.Empty(factory) ? "All" : factory;

                int rownum = 4;
                object[,] objArray = new object[1, 14];
                foreach (DataRow dr in printData.Rows)
                {
                    objArray[0, 0] = dr["ID"];
                    objArray[0, 1] = dr["Eta"];
                    objArray[0, 2] = dr["Blno"];
                    objArray[0, 3] = dr["InvNo"];
                    objArray[0, 4] = dr["Payer"];
                    objArray[0, 5] = dr["PackingArrival"];
                    objArray[0, 6] = dr["PortArrival"];
                    objArray[0, 7] = dr["WhseArrival"];
                    objArray[0, 8] = dr["DocArrival"];
                    objArray[0, 9] = dr["Sono"];
                    objArray[0, 10] = dr["Vessel"];
                    objArray[0, 11] = dr["Name"];
                    objArray[0, 12] = dr["ExtNo"];
                    objArray[0, 13] = dr["EMail"];
                    worksheet.Range[String.Format("A{0}:N{0}", rownum)].Value2 = objArray;

                    rownum++;
                }

                excel.Cells.EntireColumn.AutoFit();
                excel.Cells.EntireRow.AutoFit();

                excel.Visible = true;
            }
            
            return true;
        }
    }
}
