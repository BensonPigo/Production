using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? sciDate1,sciDate2,prodiveDate1,prodiveDate2,rcvDate1,rcvDate2;
        string brand, style, season, mDivision, mr, smr, pohandle, posmr;
        int printType;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "ALL,MR Not Send,MR Send Not Receive,Factory Receive");
            comboBox2.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("SCI Delivery can't empty!!");
                return false;
            }

            mDivision = comboBox1.Text;
            sciDate1 = dateRange1.Value1;
            sciDate2 = dateRange1.Value2;
            prodiveDate1 = dateRange2.Value1;
            prodiveDate2 = dateRange2.Value2;
            rcvDate1 = dateRange3.Value1;
            rcvDate2 = dateRange3.Value2;
            brand = txtbrand1.Text;
            style = txtstyle1.Text;
            season = txtseason1.Text;
            mr = txttpeuser_canedit1.TextBox1.Text;
            smr = txttpeuser_canedit2.TextBox1.Text;
            pohandle = txttpeuser_canedit3.TextBox1.Text;
            posmr = txttpeuser_canedit4.TextBox1.Text;
            printType = comboBox2.SelectedIndex;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select s.BrandID,s.ID,s.SeasonID,sp.MDivisionID,sp.FactoryID,sp.DOC+'-'+isnull(r.Name,'') as Doc,
sp.SendDate,sp.ReceiveDate,sp.SendToQA,sp.QAReceived,sp.ProvideDate,sp.OrderId,sp.SCIDelivery,
sp.BuyerDelivery,IIF(sp.IsPF = 1,'Y','N') as PullForward,
sp.SendName+'-'+isnull((select Name from TPEPass1 where ID = sp.SendName),'') as Handle,
sp.MRHandle+'-'+isnull((select Name from TPEPass1 where ID = sp.MRHandle),'') as MRHandle,
sp.SMR+'-'+isnull((select Name from TPEPass1 where ID = sp.SMR),'') as SMR,
sp.PoHandle+'-'+isnull((select Name from TPEPass1 where ID = sp.PoHandle),'') as POHandle,
sp.POSMR+'-'+isnull((select Name from TPEPass1 where ID = sp.POSMR),'') as POSMR,
sp.FtyHandle+'-'+isnull((select Name from Pass1 where ID = sp.FtyHandle),'') as FtyHandle
from Style_ProductionKits sp
inner join Style s on s.Ukey = sp.StyleUkey
left join Reason r on r.ReasonTypeID = 'ProductionKits' and r.ID = sp.DOC
where sp.SCIDelivery between '{0}' and '{1}'", Convert.ToDateTime(sciDate1).ToString("d"), Convert.ToDateTime(sciDate2).ToString("d")));

            if (!MyUtility.Check.Empty(prodiveDate1))
            {
                sqlCmd.Append(string.Format(" and sp.ProvideDate between '{0}' and '{1}'", Convert.ToDateTime(prodiveDate1).ToString("d"), Convert.ToDateTime(prodiveDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(rcvDate1))
            {
                sqlCmd.Append(string.Format(" and sp.ReceiveDate between '{0}' and '{1}'", Convert.ToDateTime(rcvDate1).ToString("d"), Convert.ToDateTime(rcvDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and s.BrandID = '{0}'", brand));
            }

            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(string.Format(" and s.ID = '{0}'", style));
            }

            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(string.Format(" and s.SeasonID = '{0}'", season));
            }

            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and sp.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(mr))
            {
                sqlCmd.Append(string.Format(" and sp.MRHandle = '{0}'", mr));
            }

            if (!MyUtility.Check.Empty(smr))
            {
                sqlCmd.Append(string.Format(" and sp.SMR = '{0}'", smr));
            }

            if (!MyUtility.Check.Empty(pohandle))
            {
                sqlCmd.Append(string.Format(" and sp.PoHandle = '{0}'", pohandle));
            }

            if (!MyUtility.Check.Empty(posmr))
            {
                sqlCmd.Append(string.Format(" and sp.POSMR = '{0}'", posmr));
            }

            if (printType == 1)
            {
                sqlCmd.Append(" and sp.SendDate is null");
            }
            else if (printType == 2)
            {
                sqlCmd.Append(" and sp.SendDate is not null");
            }
            else if (printType == 3)
            {
                sqlCmd.Append(" and sp.ReceiveDate is not null");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                bool result = MyUtility.Excel.CopyToXls(printData, dlg.FileName, xltfile: "PPIC_R02_ProductionKits.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); return false; }
                return true;
            }
            else
            {
                return false;
            }

            //因為速度較慢，故不使用
            //MyUtility.Msg.WaitWindows("Starting EXCEL...");
            //string strXltName = Sci.Env.Cfg.XltPathDir + "PPIC_R02_ProductionKits.xltx";
            //Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            //if (excel == null) return false;
            //Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            ////填內容值
            //int intRowsStart = 2;
            //object[,] objArray = new object[1, 21];
            //foreach (DataRow dr in printData.Rows)
            //{
            //    objArray[0, 0] = dr["BrandID"];
            //    objArray[0, 1] = dr["ID"];
            //    objArray[0, 2] = dr["SeasonID"];
            //    objArray[0, 3] = dr["MDivisionID"];
            //    objArray[0, 4] = dr["FactoryID"];
            //    objArray[0, 5] = dr["Doc"];
            //    objArray[0, 6] = dr["SendDate"];
            //    objArray[0, 7] = dr["ReceiveDate"];
            //    objArray[0, 8] = dr["SendToQA"];
            //    objArray[0, 9] = dr["QAReceived"];
            //    objArray[0, 10] = dr["ProvideDate"];
            //    objArray[0, 11] = dr["OrderId"];
            //    objArray[0, 12] = dr["SCIDelivery"];
            //    objArray[0, 13] = dr["BuyerDelivery"];
            //    objArray[0, 14] = dr["PullForward"];
            //    objArray[0, 15] = dr["Handle"];
            //    objArray[0, 16] = dr["MRHandle"];
            //    objArray[0, 17] = dr["SMR"];
            //    objArray[0, 18] = dr["POHandle"];
            //    objArray[0, 19] = dr["POSMR"];
            //    objArray[0, 20] = dr["FtyHandle"];
            //    worksheet.Range[String.Format("A{0}:U{0}", intRowsStart)].Value2 = objArray;
            //    intRowsStart++;
            //}

            //excel.Cells.EntireColumn.AutoFit();
            //excel.Cells.EntireRow.AutoFit();
            //MyUtility.Msg.WaitClear();
            //excel.Visible = true;
            //return true;
        }
    }
}
