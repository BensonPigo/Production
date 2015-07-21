using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;

namespace Sci.Production.Warehouse
{
    public partial class P03_Refno : Sci.Win.Subs.Base
    {
        DataRow dr;
        protected Sci.Win.UI.ContextMenuStrip myCMS = new Win.UI.ContextMenuStrip();
        public P03_Refno(DataRow data)
        {
            InitializeComponent();
            dr = data;
            //Helper.Controls.ContextMenu.Generator(myCMS).Menu("item1", onclick: (s, e) => DoMyCMS());
            grid1.ContextMenuStrip = myCMS;
            
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1
                = string.Format(@"Select a.factoryid,  b.id, b.seq1+b.seq2 seq
                                            , b.colorid,  b.sizespec
                                            , c.suppid, a.sewinline
                                            , a.sewline, b.FinalETD
                                            , b.inqty - b.outqty + b.adjustqty Balance
                                            , b.stockunit 
                                            from orders a, po_supp_detail b, po_supp c
                                            where b.scirefno = '{0}'
                                            and a.id = b.id
                                            and a.id = c.id
                                            and b.seq1 = c.seq1
                                            and a.WhseClose is null
                                            order by ColorID, SizeSpec ,SewinLine
                                            ", dr["scirefno"].ToString());
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            else
            {
                listControlBindingSource1.DataSource = selectDataTable1;
            }
            //設定Grid1的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid1);
            MyUtility.Tool.AddMenuToPopupGridFilter(this, this.grid1, null,"factoryid,colorid,sizespec");
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("factoryid", header: "Factory", width: Widths.AnsiChars(8))
                 .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq", header: "Seq", width: Widths.AnsiChars(5))
                 .Text("colorid", header: "Color", width: Widths.AnsiChars(6))
                 .Text("sizespec", header: "Size", width: Widths.AnsiChars(15))
                 .Text("suppid", header: "Supp", width: Widths.AnsiChars(6))
                 .Date("sewinline", header: "Sewing Inline Date", width: Widths.AnsiChars(10))
                  .Text("sewline", header: "Sewing Line#", width: Widths.AnsiChars(10))
                   .Date("FinalETD", header: "FinalETD", width: Widths.AnsiChars(10))
                 .Numeric("balance", header: "Balance Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                  .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8))
                 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            dlg.Title = "Save as Excel File";
            dlg.FileName = "P03_Refno_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            // Get the selected file name and CopyToXls
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            {
                // Open document
                DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                DualResult result = MyUtility.Excel.CopyToXls(dt, dlg.FileName);
                if (result) { MyUtility.Excel.XlsAutoFit(dlg.FileName,"P03_Refno.xlt",4); }   //XlsAutoFit(dlg.FileName, "MMDR030.xlt", 12);
                else { MyUtility.Msg.WarningBox(result.ToMessages().ToString(), "Warning"); }
            }
            else
            {
                return;
            }
        }
    }
}
