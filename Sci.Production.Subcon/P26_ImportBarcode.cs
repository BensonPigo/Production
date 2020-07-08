using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Sci.Production.Subcon
{
    public partial class P26_ImportBarcode : Win.Subs.Base
    {
        public P26_ImportBarcode()
        {
            this.InitializeComponent();

            DataTable SubprocessDT;
            string querySql = @"select Id from SubProcess where IsRFIDProcess=1";
            DBProxy.Current.Select(null, querySql, out SubprocessDT);
            MyUtility.Tool.SetupCombox(this.comboSubprocess, 1, SubprocessDT);
            this.comboSubprocess.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("BundleNo", header: "Bundle#", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("BundleGroup", header: "Group#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Qty", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Orderid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SubprocessId", header: "Atrwork", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Patterncode", header: "Cutpart ID", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("PatternDesc", header: "Cutpart Name", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ErrorMsg", header: "Error Msg.", width: Widths.AnsiChars(34), iseditingreadonly: true);
        }

        // 從C:\temp\BUNDLEIN.TXT讀取資料
        DataTable leftDT;

        private void btnImportfromscanner_Click(object sender, EventArgs e)
        {
            if (this.leftDT != null)
            {
                this.leftDT.Clear();
            }
            #region 執行從機器載入
            string pcPath = "C:\\";
            string pcPara = "+B115200 +P8 " + pcPath + "temp\\(file)";
            P23_bhtprtd Bht = new P23_bhtprtd();
            string msg = Bht.csharpExecProtocol(this.Handle, pcPara, 2, 2);
            this.ShowInfo(msg);
            if (msg.Contains("Error"))
            {
                return;
            }
            #endregion

            #region 建立要讀檔的Table結構
            string selectCommand = @"Select BundleNo from Bundle_detail a WITH (NOLOCK) where 1=0";
            DataTable tmpDataTable;
            DualResult selectResult;
            if (!(selectResult = DBProxy.Current.Select(null, selectCommand, out tmpDataTable)))
            {
                MyUtility.Msg.WarningBox("Connection faile.!");
                return;
            }
            #endregion

            #region 單純把檔案部讀進來(不論長度 已問過Arger)
            string tmpFile = "C:\\temp\\BUNDLEIN.TXT";
            try
            { // Open the text file using a stream reader.
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
                            dr["BundleNo"] = sl[0].Trim();
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
                Console.WriteLine("The file could not be read! or C:\\temp\\BUNDLEIN.TXT is not exist!");
                Console.WriteLine(ex.Message);
            }
            #endregion

            #region 刪除檔案
            FileInfo fi = new FileInfo(tmpFile);
            try
            {
                fi.Delete();
            }
            catch (IOException ex)
            {
                MyUtility.Msg.ErrorBox(ex.Message);
            }
            #endregion

            #region 讀完後開始要判斷整批的BundleNO
            string stmp = string.Format(
                @"
select distinct 
	t.BundleNo
	,bd.BundleGroup
	,bd.Qty
	,b.Orderid
	,a.SubprocessId
	,bd.Patterncode
	,bd.PatternDesc
	,ErrorMsg = CASE 
		WHEN bda.SubprocessId is null THEN 'Can''t find in bundle card data'
		WHEN BTD.ReceiveDate IS NULL and BTD.id is not null and BTD.id like 'TC%' THEN CONCAT('This bundle already transfer in slip#', BTD.Id,' which not received.')
		ELSE ''
		END
into #tmp2
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
WHERE (bda.SubprocessId  = '{0}' or bda.SubprocessId is null) 
and (bd.Patterncode != 'ALLPARTS' or bd.Patterncode is null)

select t.BundleNo,t.BundleGroup,t.Qty,t.Orderid,t.SubprocessId,t.Patterncode,t.PatternDesc,ErrorMsg = max(t.ErrorMsg)
from #tmp2 t
group by t.BundleNo,t.BundleGroup,t.Qty,t.Orderid,t.SubprocessId,t.Patterncode,t.PatternDesc

drop table #tmp,#tmp2",
                this.comboSubprocess.Text);
            this.txtNumsofBundle.Text = "0";

            if (tmpDataTable.Rows.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(tmpDataTable, string.Empty, stmp, out this.leftDT);
                if (this.leftDT.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("No bundles of {0} be import from scanner.", this.comboSubprocess.Text));
                    return;
                }

                this.listControlBindingSource1.DataSource = this.leftDT;
                this.txtNumsofBundle.Text = this.leftDT.Rows.Count.ToString(); // grid的總數
            }
            #endregion
        }

        // CREATE
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (this.grid1.Rows.Count == 0)
            {
                return;
            }

            // 取沒有ErrorMsg
            DataRow[] dr = ((DataTable)this.listControlBindingSource1.DataSource).Select("ErrorMsg = ''");
            if (dr.Length == 0)
            {
                MyUtility.Msg.WarningBox("No Datas create!");
                return;
            }

            // 準備新增sqlcmd 準備ID
            string getID = MyUtility.GetValue.GetID("TC", "BundleTrack", DateTime.Today, 3, "ID", null);
            if (MyUtility.Check.Empty(getID))
            {
                MyUtility.Msg.WarningBox("GetID fail, please try again!");
                return;
            }

            string date = DateTime.Now.ToString("yyyy/MM/dd");
            string datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string sprocess = this.comboSubprocess.Text;

            // 主Table BundleTrack一筆
            string insertBundleTrack = string.Format(
                @"insert into BundleTrack (id,IssueDate,StartProcess,EndProcess,StartSite,EndSite,AddName,AddDate) 
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                getID, date, sprocess, sprocess, Sci.Env.User.Factory, Sci.Env.User.Factory, Sci.Env.User.UserID, datetime);

            // Detail BundleTrack_detail用ProcessWithDatatable方法整個table新增
            string insertBundleTrackDteail = string.Format(
                @"insert into BundleTrack_detail select '{0}',t.BundleNo,t.Orderid,'{1}','{2}',0 from #tmp t",
                getID, datetime, Sci.Env.User.UserID);

            // insert BundleTrack
            DualResult Result;
            Result = DBProxy.Current.Execute(null, insertBundleTrack);
            if (!Result)
            {
                this.ShowErr(Result);
                return;
            }

            // insert BundleTrack_detail使用ProcessWithDatatable一次新增
            DataTable dtmp = this.leftDT.Clone();
            foreach (DataRow row in dr)
            {
                dtmp.ImportRow(row);
            }

            DataTable n;
            Result = MyUtility.Tool.ProcessWithDatatable(this.leftDT, "BundleNo,Orderid", insertBundleTrackDteail, out n);
            if (!Result)
            {
                this.ShowErr(Result);
                return;
            }

            if (this.leftDT != null)
            {
                this.leftDT.Clear();
            }

            MyUtility.Msg.InfoBox("Create success " + getID);
            this.Close();
        }

        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1 = null;

            this.Close();
        }
    }
}
