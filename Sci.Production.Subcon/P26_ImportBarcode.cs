using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P26_ImportBarcode : Sci.Win.Subs.Base
    {
        DataRow Master;
        DataTable Detail;
        public P26_ImportBarcode()
        {
            InitializeComponent();

            DataTable SubprocessDT;
            string querySql = @"select Id from SubProcess where IsRFIDProcess=1";
            DBProxy.Current.Select(null, querySql, out SubprocessDT);
            MyUtility.Tool.SetupCombox(comboSubprocess, 1, SubprocessDT);
            comboSubprocess.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            //對應DataTable的欄位名稱先打上, 之後再變更
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Slip", header: "Slip ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Group", header: "Group#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Orderid", header: "SP#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Atrwork", header: "Atrwork", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Patterncode", header: "PTN Code", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("PatternDesc", header: "PTN Desc.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Error", header: "Error Msg.", width: Widths.AnsiChars(10), iseditingreadonly: true);

            Helper.Controls.Grid.Generator(this.grid2)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ErrorMsg", header: "Error Msg.", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }
        //從外接機器讀取資料
        private void btnImportfromscanner_Click(object sender, EventArgs e)
        {
            //右邊grid的總數
            txtNumsofBundle.Text = grid2.Rows.Count.ToString();
        }
        //右邊grid 刪除有ErrorMsg (先做, 等詳細規格)
        private void btnDeleteError_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in ((DataTable)grid2.DataSource).Select("ErrorMsg != ''"))
                dr.Delete();
        }
        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
