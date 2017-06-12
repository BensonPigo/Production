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
    public partial class P23_ImportBarcode : Sci.Win.Subs.Base
    {
        DataRow Master;
        DataTable Detail;
        public P23_ImportBarcode()
        {
            InitializeComponent();

            DataTable SubprocessDT;
            string querySql = @"select Id from SubProcess where IsRFIDProcess=1";
            DBProxy.Current.Select(null, querySql, out SubprocessDT);
            MyUtility.Tool.SetupCombox(comboSubprocess, 1, SubprocessDT);
            comboSubprocess.SelectedIndex = 0;

            DataTable ToDT;
            string querySql2 = @"
                                select Abb,ID from LocalSupp where UseSBTS=1 and Junk=0
                                union all
                                select Abb,ID from Factory where Junk=0
                                order by Abb
                                ";
            DBProxy.Current.Select(null, querySql2, out ToDT);
            MyUtility.Tool.SetupCombox(comboTo, 1, ToDT);
            comboTo.SelectedIndex = 0;
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

            
        }
        //從外接機器讀取資料
        private void btnImportfromscanner_Click(object sender, EventArgs e)
        {
            //grid1的總數
            txtNumsofBundle.Text = grid1.Rows.Count.ToString();
        }
        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
