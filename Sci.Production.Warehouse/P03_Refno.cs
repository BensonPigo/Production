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
using System.Linq;
using Sci.Utility.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P03_Refno : Sci.Win.Subs.Base
    {
        string comboMvalue = "All", comboColorvalue = "All", comboSizevalue = "All";
        DataRow dr;
        DataTable selectDataTable1;
        protected Sci.Win.UI.ContextMenuStrip myCMS = new Win.UI.ContextMenuStrip();
        public P03_Refno(DataRow data)
        {
            InitializeComponent();
            dr = data;
            //Helper.Controls.ContextMenu.Generator(myCMS).Menu("item1", onclick: (s, e) => DoMyCMS());
            grid1.ContextMenuStrip = myCMS;
            this.Text += string.Format(" ({0})", dr["refno"]);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1
                = string.Format(@"Select md.mdivisionid ,  b.id
, concat(Ltrim(Rtrim(b.seq1)), ' ', b.seq2) as seq --left(b.seq1+' ',3)+b.Seq2 as seq
, b.colorid,  b.sizespec
, c.suppid, a.sewinline
, a.sewline, b.FinalETA
, md.inqty - md.outqty + md.adjustqty Balance
, b.stockunit 
from orders a
, po_supp_detail b inner join dbo.MDivisionPoDetail md on md.POID = b.id and md.seq1 = b.seq1 and md.seq2 = b.seq2
, po_supp c
where b.scirefno = '{0}'
and a.id = b.id
and a.id = c.id
and b.seq1 = c.seq1
and a.WhseClose is null
--and md.mdivisionid='{1}'
order by ColorID, SizeSpec ,SewinLine
", dr["scirefno"].ToString(), Sci.Env.User.Keyword);
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false) ShowErr(selectCommand1, selectResult1);
            else
            {
                listControlBindingSource1.DataSource = selectDataTable1;
            }

            //分別加入comboboxitem
            comboM.Items.Clear();
            comboColor.Items.Clear();
            comboSize.Items.Clear();
//            string s1
//                = string.Format(@"select '' as ID union all 
//Select distinct md.mdivisionid
//from orders a
//, po_supp_detail b left join dbo.MDivisionPoDetail md on md.POID = b.id and md.seq1 = b.seq2 and md.seq2 = b.seq2
//, po_supp c
//where b.scirefno = '{0}'
//and a.id = b.id
//and a.id = c.id
//and b.seq1 = c.seq1
//and a.WhseClose is null
//and md.mdivisionid='{1}'
//", dr["scirefno"].ToString(), Sci.Env.User.Keyword);
//            string s2
//                = string.Format(@"select '' as ID union all 
//Select distinct b.colorid
//from orders a
//, po_supp_detail b left join dbo.MDivisionPoDetail md on md.POID = b.id and md.seq1 = b.seq2 and md.seq2 = b.seq2
//, po_supp c
//where b.scirefno = '{0}'
//and a.id = b.id
//and a.id = c.id
//and b.seq1 = c.seq1
//and a.WhseClose is null
//and md.mdivisionid='{1}'
//", dr["scirefno"].ToString(), Sci.Env.User.Keyword);
//            string s3
//                = string.Format(@"select '' as ID union all 
//Select distinct b.sizespec
//from orders a
//, po_supp_detail b left join dbo.MDivisionPoDetail md on md.POID = b.id and md.seq1 = b.seq2 and md.seq2 = b.seq2
//, po_supp c
//where b.scirefno = '{0}'
//and a.id = b.id
//and a.id = c.id
//and b.seq1 = c.seq1
//and a.WhseClose is null
//and md.mdivisionid='{1}'
//", dr["scirefno"].ToString(), Sci.Env.User.Keyword);
            List<string> dts1 = selectDataTable1.AsEnumerable().Select(row => row["mdivisionid"].ToString()).Distinct().ToList();
            List<string> dts2 = selectDataTable1.AsEnumerable().Select(row => row["colorid"].ToString()).Distinct().ToList();
            List<string> dts3 = selectDataTable1.AsEnumerable().Select(row => row["sizespec"].ToString()).Distinct().ToList();
            dts1.Insert(0, "All");
            dts2.Insert(0, "All");
            dts3.Insert(0, "All");
            //DataTable dt1;
            //DataTable dt2;
            //DataTable dt3;
            //DBProxy.Current.Select(null, s1, out dt1);
            //DBProxy.Current.Select(null, s2, out dt2);
            //DBProxy.Current.Select(null, s3, out dt3);
            if (!dts1.Empty())
            {
                //MyUtility.Tool.SetupCombox(comboM, 1, dt1);
                comboM.DataSource = dts1;
            }
            if (!dts2.Empty())
            {
                comboColor.DataSource = dts2;
                //MyUtility.Tool.SetupCombox(comboColor, 1, dt2);
            }
            if (!dts2.Empty())
            {
                comboSize.DataSource = dts3;
                //MyUtility.Tool.SetupCombox(comboSize, 1, dt3);
            }
            //設定Grid1的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid1);
            MyUtility.Tool.AddMenuToPopupGridFilter(this, this.grid1, null, "factoryid,colorid,sizespec");
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("mdivisionid", header: "M", width: Widths.AnsiChars(8))
                 .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq", header: "Seq", width: Widths.AnsiChars(5))
                 .Text("colorid", header: "Color", width: Widths.AnsiChars(6))
                 .Text("sizespec", header: "Size", width: Widths.AnsiChars(15))
                 .Text("suppid", header: "Supp", width: Widths.AnsiChars(6))
                 .Date("sewinline", header: "Sewing Inline Date", width: Widths.AnsiChars(10))
                  .Text("sewline", header: "Sewing Line#", width: Widths.AnsiChars(10))
                   .Date("FinalETA", header: "FinalETA", width: Widths.AnsiChars(10))
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
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P03_Refno.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dt, "", "Warehouse_P03_Refno.xltx", 4, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[3, 2] = MyUtility.Convert.GetString(dr["refno"].ToString());
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);
            //string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.StartupPath);
            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.RestoreDirectory = true;
            //dlg.InitialDirectory = MyDocumentsPath;     //指定"我的文件"路徑
            //dlg.Title = "Save as Excel File";
            //dlg.FileName = "P03_Refno_ToExcel_" + DateTime.Now.ToString("yyyyMMdd") + @".xls";

            //dlg.Filter = "Excel Files (*.xls)|*.xls";            // Set filter for file extension and default file extension

            //// Display OpenFileDialog by calling ShowDialog method ->ShowDialog()
            //// Get the selected file name and CopyToXls
            //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != null)
            //{
            //    // Open document
            //    DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            //    //DualResult result = MyUtility.Excel.CopyToXls(dt, dlg.FileName);
            //    //if (result) { MyUtility.Excel.XlsAutoFit(dlg.FileName,"P03_Refno.xltx",4); }   //XlsAutoFit(dlg.FileName, "MMDR030.xltx", 12);
            //    //else { MyUtility.Msg.WarningBox(result.ToMessages().ToString(), "Warning"); }
            //}
            //else
            //{
            //    return;
            //}
        }

        private void comboM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboM.SelectedValue == null) return;
            if (comboM.SelectedValue.ToString() == comboMvalue) return;
            comboMvalue = comboM.SelectedValue.ToString();
            Filter();       
        }

        private void comboColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboColor.SelectedValue == null) return;
            if (comboColor.SelectedValue.ToString() == comboColorvalue || comboColor.SelectedValue.ToString() == null) return;
            comboColorvalue = comboColor.SelectedValue.ToString();
            Filter();
        }

        private void comboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboSize.SelectedValue == null) return;
            if (comboSize.SelectedValue.ToString() == comboSizevalue || comboSize.SelectedValue.ToString() == null) return;
            comboSizevalue = comboSize.SelectedValue.ToString();
            Filter();
        }

        protected void Filter()
        {
            DataTable ntb=null;
            //string s1 = comboM.Text, s2 = comboColor.Text, s3 = comboSize.Text;
            //第一趟進來combobox都還未加入item都是""字串
            if (comboMvalue != "" || comboColorvalue != "" || comboSizevalue != "")
            {
                IEnumerable<DataRow> query =
                    from o in selectDataTable1.AsEnumerable()
                    where
                    (comboMvalue != "All" ? o.Field<String>("mdivisionid") == comboMvalue : 1 == 1) &&
                    (comboColorvalue != "All" ? o.Field<String>("colorid") == comboColorvalue : 1 == 1) &&
                    (comboSizevalue != "All" ? o.Field<String>("sizespec") == comboSizevalue : 1 == 1)
                    select o;

                int FilterCount = query.ToList<DataRow>().Count;
                if (FilterCount > 0)  ntb = query.CopyToDataTable<DataRow>();
                listControlBindingSource1.DataSource = ntb;

            }
            else
            {
                listControlBindingSource1.DataSource = selectDataTable1;
            }

            DataTable temp = (DataTable)listControlBindingSource1.DataSource;
            comboM.DataSource = null;
            comboM.Items.Clear();
            comboColor.DataSource = null;
            comboColor.Items.Clear();
            comboSize.DataSource = null;
            comboSize.Items.Clear();
            List<string> dts1 = temp.AsEnumerable().Select(row => row["mdivisionid"].ToString()).Distinct().ToList();
            List<string> dts2 = temp.AsEnumerable().Select(row => row["colorid"].ToString()).Distinct().ToList();
            List<string> dts3 = temp.AsEnumerable().Select(row => row["sizespec"].ToString()).Distinct().ToList();
            if (!dts1.Empty())
            {
                dts1.Insert(0, "All");
                if (comboMvalue != "All")
                {
                    dts1.Remove(comboMvalue);
                    dts1.Insert(0, comboMvalue);
                }
                comboM.DataSource = dts1;
            }
            if (!dts2.Empty())
            {
                dts2.Insert(0, "All");
                if (comboColorvalue != "All")
                {
                    dts2.Remove(comboColorvalue);
                    dts2.Insert(0, comboColorvalue);
                }
                comboColor.DataSource = dts2;
            }
            if (!dts3.Empty())
            {
                dts3.Insert(0, "All");
                if (comboSizevalue != "All")
                {
                    dts3.Remove(comboSizevalue);
                    dts3.Insert(0, comboSizevalue);
                }
                comboSize.DataSource = dts3;
            }
        }
    }
}
