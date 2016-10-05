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
using Sci;

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
            if (cbResult = DBProxy.Current.Select(null, string.Format("select ID from Factory where MDivisionID = '{0}'",Sci.Env.User.Keyword), out dtFactory))
            {
                MyUtility.Tool.SetupCombox(comboBox1, 1, dtFactory);
            }

            comboBox1.SelectedIndex = -1;
            //comboBox1.SelectedValue = "";
            DataRow drOC;
            if (MyUtility.Check.Seek(string.Format(@"select top 1 UpdateDate 
from OrderComparisonList
where MDivisionID = '{0}' 
and UpdateDate = (select max(UpdateDate) from OrderComparisonList where MDivisionID = '{0}')", Sci.Env.User.Keyword), out drOC))
            {
                dateBox2.Value = Convert.ToDateTime(drOC["UpdateDate"]);
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            

            //Grid設定
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);

            //當欄位值為0時，顯示空白
            oriqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            newqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14))
                .Text("OriginalStyleID", header: "Style", width: Widths.AnsiChars(10))
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
                    if ((dtData.Rows[i]["OriginalQty"].ToString() != dtData.Rows[i]["NewQty"].ToString() && dtData.Rows[i]["NewQty"].ToString() == "0") || dtData.Rows[i]["JunkOrder"].ToString() == "V")
                    {
                        grid1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        grid1.Rows[i].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
                    }
                }
            };

            QueryDate((string)comboBox1.SelectedValue,dateBox2.Value);
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
from OrderComparisonList 
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
                dateBox1.Value = null;
            }
            else
            {
                dateBox1.Value = Convert.ToDateTime(gridData.Rows[0]["TransferDate"]);
            }
        }

        //Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Updated Date
        private void dateBox2_Validated(object sender, EventArgs e)
        {
            if (dateBox2.OldValue != dateBox2.Value)
            {
                QueryDate((string)comboBox1.SelectedValue, (DateTime?)dateBox2.Value);
            }
        }

        //Factory
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                QueryDate("", (DateTime?)dateBox2.Value);
            }
            else
            {
                QueryDate((string)comboBox1.SelectedValue, (DateTime?)dateBox2.Value);
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

            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            //dlg.FileName = "ComparisonList_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            // Get the selected file name and CopyToXls
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                bool result = MyUtility.Excel.CopyToXls(ExcelTable, dlg.FileName, xltfile: "PPIC_P02.xltx", headerRow: 3);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }
    }
}
