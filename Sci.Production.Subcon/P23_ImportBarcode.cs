using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P23_ImportBarcode : Sci.Win.Subs.Base
    {
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
order by Abb";
            DBProxy.Current.Select(null, querySql2, out ToDT);
            MyUtility.Tool.SetupCombox(comboTo, 1, ToDT);
            comboTo.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Group", header: "Group#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Qty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Orderid", header: "SP#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Atrwork", header: "Atrwork", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Patterncode", header: "Cutpart ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Error", header: "Error Msg.", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }
        //從C:\temp\BUNDLEIN.TXT讀取資料
        private void btnImportfromscanner_Click(object sender, EventArgs e)
        {
            #region 建立要讀檔的Table結構
            string selectCommand = @"Select BundleNo from Bundle_detail a WITH (NOLOCK) where 1=0";
            DataTable leftDT, tmpDataTable;
            DualResult selectResult;
            if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out tmpDataTable)))
            {
                MyUtility.Msg.WarningBox("Connection faile.!");
                return;
            }
            #endregion

            #region 單純把檔案部讀進來(不論長度 已問過Arger)
            string tmpFile = "C:\\temp\\BUNDLEOT.TXT";
            try
            {   // Open the text file using a stream reader.
                using (StreamReader reader = new StreamReader(tmpFile, System.Text.Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split("\r\n".ToCharArray());
                        DataRow dr = tmpDataTable.NewRow();
                        try
                        {
                            dr["BundleNo"] = sl[0];
                        }
                        catch (Exception)
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }
                        tmpDataTable.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }
            #endregion
            
            #region 讀完後開始要判斷整批的BundleNO
            //只判斷BundleTrack_detail存在就顯示,其它狀況都空白,可Create
            //bda.SubprocessId IS NULL or bda.SubprocessId  = '{0}'此條件 參照舊系統
            string stmp = string.Format(@"
select distinct 
	t.BundleNo
	,bd.BundleGroup
	,Qty = sum(bd.Qty) over(partition by t.BundleNo)
	,b.Orderid
	,a.SubprocessId
	,bd.Patterncode
	,bd.PatternDesc
	,ErrorMsg = CASE 
		WHEN BTD.BundleNo IS NOT NULL THEN 'Data already create'
		ELSE ''
		END
from #tmp t
left join Bundle_Detail bd WITH (NOLOCK) on bd.BundleNo = t.BundleNo
left join BundleTrack_detail BTD WITH (NOLOCK) on BTD.BundleNo = t.BundleNo
left join Bundle_Detail_art bda WITH (NOLOCK) on  bda.bundleno =bd.bundleno and bda.id = bd.id 
left join Bundle b WITH (NOLOCK) on b.id = bd.id
outer apply(
	select SubprocessId = stuff((
		select concat('+',bda2.SubprocessId)
		from Bundle_Detail_art bda2 WITH (NOLOCK)
		where bda2.bundleno = bd.bundleno and bda2.id = bd.id 
		for xml path('')
	),1,1,'')
)a
WHERE bda.SubprocessId  = '{0}' and bd.Patterncode != 'ALLPARTS'"
                , comboSubprocess.Text);
            txtNumsofBundle.Text = "0";
            if (tmpDataTable.Rows.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(tmpDataTable, "", stmp, out leftDT);
                listControlBindingSource1.DataSource = leftDT;
                txtNumsofBundle.Text = leftDT.Rows.Count.ToString();//grid的總數
            }
            #endregion

            #region 刪除檔案
            System.IO.FileInfo fi = new System.IO.FileInfo(tmpFile);
            try
            {
                fi.Delete();
            }
            catch (System.IO.IOException ex)
            {
                MyUtility.Msg.ErrorBox(ex.Message);
            }
            #endregion
        }
        //Create
        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
