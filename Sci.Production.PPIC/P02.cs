﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;
using Sci.Utility.Excel;

namespace Sci.Production.PPIC
{
    public partial class P02 : Sci.Win.Tems.QueryForm
    {
        DataGridViewGeneratorNumericColumnSettings oriqty = new DataGridViewGeneratorNumericColumnSettings();
        DataGridViewGeneratorNumericColumnSettings newqty = new DataGridViewGeneratorNumericColumnSettings();
        DataTable gridData;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable dtFactory;
            DualResult cbResult;
            if (cbResult = DBProxy.Current.Select(null, string.Format("select ID from Factory WITH (NOLOCK) where MDivisionID = '{0}'", Sci.Env.User.Keyword), out dtFactory))
            {

                MyUtility.Tool.SetupCombox(comboFactory, 1, dtFactory);

            }
            dtFactory.Rows.Add(new string[] { "" });
            comboFactory.SelectedValue= Sci.Env.User.Keyword;

            DataRow drOC;
            if (MyUtility.Check.Seek(string.Format(@"select top 1 UpdateDate 
from OrderComparisonList WITH (NOLOCK) 
where MDivisionID = '{0}' 
and UpdateDate = (select max(UpdateDate) from OrderComparisonList WITH (NOLOCK) where MDivisionID = '{0}')", Sci.Env.User.Keyword), out drOC))
            {
                dateUpdatedDate.Value = Convert.ToDateTime(drOC["UpdateDate"]);
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            

            //Grid設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.RowHeadersVisible = true;
            this.grid1.DataSource = listControlBindingSource1;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);

            //當欄位值為0時，顯示空白
            oriqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            newqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14))
                .Text("OriginalStyleID", header: "Style", width: Widths.AnsiChars(14))
                .Numeric("OriginalQty", header: "Order\r\nQty", settings: oriqty, width: Widths.AnsiChars(5))
                .Text("OriginalBuyerDelivery", header: "Buyer\r\nDel", width: Widths.AnsiChars(5))
                .Text("OriginalSCIDelivery", header: "SCI\r\nDel", width: Widths.AnsiChars(5))
                .Text("OriginalLETA", header: "SCHD L/ETA\r\n(Master SP)")
                .Text("KPILETA", header: "KPI\r\nL/ETA", width: Widths.AnsiChars(5))
                .Text("", header: "", width: Widths.AnsiChars(0))
                .Text("TransferToFactory", header: "Transfer to", width: Widths.AnsiChars(8))
                .Numeric("NewQty", header: "Order\r\nQty", settings: newqty, width: Widths.AnsiChars(5))
                .Text("NewBuyerDelivery", header: "Buyer\r\nDel", width: Widths.AnsiChars(5))
                .Text("NewSCIDelivery", header: "SCI\r\nDel", width: Widths.AnsiChars(5))
                .Text("NewLETA", header: "SCHD L/ETA\r\n(Master SP)")
                .Text("NewOrder", header: "New", width: Widths.AnsiChars(1))
                .Text("DeleteOrder", header: "Dele", width: Widths.AnsiChars(1))
                .Text("JunkOrder", header: "Junk", width: Widths.AnsiChars(1))
                .Text("CMPQDate", header: "CMPQ", width: Widths.AnsiChars(1))
                .Text("EachConsApv", header: "Each Cons", width: Widths.AnsiChars(1))
                .Text("NewMnorder", header: "M/Notice", width: Widths.AnsiChars(1))
                .Text("NewSMnorder", header: "S/M.Notice", width: Widths.AnsiChars(1))
                .Text("MnorderApv2", header: "VAS/SHAS", width: Widths.AnsiChars(1));
            //因為資料會有變色，所以按Grid Header不可以做排序
            for (int i = 0; i < this.grid1.ColumnCount; i++)
            {
                this.grid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            grid1.RowsAdded += (s, e) =>
            {
                DataTable dtData = (DataTable)listControlBindingSource1.DataSource;
                for (int i = 0; i < e.RowCount; i++)
                {
                    if ((dtData.Rows[i]["OriginalQty"].ToString() != dtData.Rows[i]["NewQty"].ToString() && dtData.Rows[i]["NewQty"].ToString() == "0") || dtData.Rows[i]["JunkOrder"].ToString() == "V" || dtData.Rows[i]["DeleteOrder"].ToString() == "V")
                    {
                        grid1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        grid1.Rows[i].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                    }
                }
            };

            QueryDate((string)comboFactory.SelectedValue,dateUpdatedDate.Value);
        }

        //Query Data
        private void QueryDate(string factoryID, DateTime? updateDate)
        {
            string sqlCmd = string.Format(@"select FactoryID,OrderId,OriginalStyleID,iif(convert(varchar,OriginalQty) = 0,'',convert(varchar,OriginalQty)) as OriginalQty,
RIGHT(CONVERT(VARCHAR(20),OriginalBuyerDelivery,111),5) as OriginalBuyerDelivery,
RIGHT(CONVERT(VARCHAR(20),OriginalSCIDelivery,111),5) as OriginalSCIDelivery,
RIGHT(CONVERT(VARCHAR(20),OriginalLETA,111),5) as OriginalLETA,
RIGHT(CONVERT(VARCHAR(20),KPILETA,111),5) as KPILETA,
TransferToFactory,iif(convert(varchar,NewQty) = 0,'',convert(varchar,NewQty)) as NewQty,
RIGHT(CONVERT(VARCHAR(20),NewBuyerDelivery,111),5) as NewBuyerDelivery,
RIGHT(CONVERT(VARCHAR(20),NewSCIDelivery,111),5) as NewSCIDelivery,
RIGHT(CONVERT(VARCHAR(20),NewLETA,111),5) as NewLETA,
IIF(NewOrder = 1, 'V','') as NewOrder,
iif(DeleteOrder=1,'V','') as DeleteOrder,iif(JunkOrder=1,'V','') as JunkOrder,
iif(NewCMPQDate is null,'','V') as CMPQDate,
iif(NewEachConsApv is null,iif(OriginalEachConsApv is null,'','★'),'V') as EachConsApv,
iif(NewMnorderApv is null,'','V') as NewMnorder,iif(NewSMnorderApv is null,'','V') as NewSMnorderApv,
iif(MnorderApv2 is null,'','V') as MnorderApv2, TransferDate
from OrderComparisonList WITH (NOLOCK) 
where {0} and UpdateDate {1}
order by FactoryID,OrderId", MyUtility.Check.Empty(factoryID) ? string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword) : string.Format("FactoryID = '{0}'", factoryID), MyUtility.Check.Empty(updateDate) ? "is null" : "='" + Convert.ToDateTime(updateDate).ToString("d") + "'");
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n"+result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            if (gridData.Rows.Count == 0)
            {
                dateLastDate.Value = null;
            }
            else
            {
                dateLastDate.Value = Convert.ToDateTime(gridData.Rows[0]["TransferDate"]);
            }

            this.grid1.AutoResizeColumns();
        }

        //Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Updated Date
        //private void dateBox2_Validated(object sender, EventArgs e)
        //{
        //    if (dateBox2.OldValue != dateBox2.Value)
        //    {
        //        QueryDate((string)comboBox1.SelectedValue, (DateTime?)dateBox2.Value);
        //    }

        //}

        //Factory
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboFactory.SelectedIndex < 0)
            {
                QueryDate("", (DateTime?)dateUpdatedDate.Value);
            }
            else
            {
                QueryDate((string)comboFactory.SelectedValue, (DateTime?)dateUpdatedDate.Value);//
            }
        }

        //Excel
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable ExcelTable;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "FactoryID,OrderId,OriginalStyleID,OriginalQty,OriginalBuyerDelivery,OriginalSCIDelivery,OriginalLETA,KPILETA,TransferToFactory,NewQty,NewBuyerDelivery,NewSCIDelivery,NewLETA,NewOrder,DeleteOrder,JunkOrder,CMPQDate,EachConsApv,NewMnorder,NewSMnorderApv,MnorderApv2", "select * from #tmp", out ExcelTable);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("To Excel error.\r\n" + ex.ToString());
                return;
            }

            if (ExcelTable.Rows.Count==0)
            {
                MyUtility.Msg.InfoBox("No data.");
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_P02.xltx");
            MyUtility.Excel.CopyToXls(ExcelTable, "", "PPIC_P02.xltx", 3, true, "", objApp);
            objApp.Cells[2, 3] = "Last Date " + dateLastDate.Value.Value.ToShortDateString();
            objApp.Cells[2, 9] = "Update Date " + dateUpdatedDate.Value.Value.ToShortDateString();
            int Number = 3;
            for (int j = 0; j < ExcelTable.Rows.Count; j++)
            {
                Number++;
            }
            objApp.get_Range("A" + 4, "A" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("A" + 4, "A" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            objApp.get_Range("A" + 4, "A" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("A" + 4, "A" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("B" + 4, "B" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("B" + 4, "B" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("H" + 4, "H" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("H" + 4, "H" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("M" + 4, "M" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("M" + 4, "M" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("N" + 4, "N" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("N" + 4, "N" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("O" + 4, "O" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("O" + 4, "O" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("P" + 4, "P" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("P" + 4, "P" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("Q" + 4, "Q" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("Q" + 4, "Q" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("R" + 4, "R" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("R" + 4, "R" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("S" + 4, "S" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("S" + 4, "S" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("T" + 4, "T" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("T" + 4, "T" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            objApp.get_Range("U" + 4, "U" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            objApp.get_Range("U" + 4, "U" + Number).Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
        }
              
        private void dateBox2_ValueChanged(object sender, EventArgs e)
        {
            QueryDate((string)comboFactory.SelectedValue, (DateTime?)dateUpdatedDate.Value);
        }
    }
}
