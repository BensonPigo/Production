using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Subcon
{
    public partial class P41 : Sci.Win.Tems.QueryForm
    {
        public P41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;

            #region combo box預設值

            MyUtility.Tool.SetupCombox(this.comboComplete, 1, 1, "All,Y,N");
            this.comboComplete.SelectedIndex = 0;

            DataTable dt;
            DBProxy.Current.Select(null, @"
SELECT [Text]=ID,[Value]=ID FROM SubProcess WITH(NOLOCK) WHERE Junk=0 AND IsRFIDProcess=1
", out dt);

            this.comboSubPorcess.DataSource = dt;
            this.comboSubPorcess.ValueMember = "Value";
            this.comboSubPorcess.DisplayMember = "Text";
            this.comboSubPorcess.SelectedIndex = 0;
            #endregion

            // 排除非生產工廠
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.FilteMDivision = true;
            this.txtFactory.Text = Sci.Env.User.Factory;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings BundleReplacement = new DataGridViewGeneratorTextColumnSettings();
            BundleReplacement.CellMouseDoubleClick += (s, e) =>
            {
                DataRow drSelected = this.grid.GetDataRow(e.RowIndex);
                if (drSelected == null)
                {
                    return;
                }

                string sqlcmd = $@"
select
	Date = bi.DefectUpdateDate,
	BundleNo=bi.BundleNo,
	SubProcess=bi.SubProcessID,
	[Reason Descripotion] = concat(bi.ReasonID, ' ', bdr.Reason),
	[Defect Qty (pcs)]=bi.DefectQty,
	[Replacement Qty (pcs)]=bi.ReplacementQty,
	Status=iif(bi.DefectQty=bi.ReplacementQty,'Complete','')
from dbo.SciMES_BundleInspection bi with(nolock)
left join dbo.SciMES_BundleDefectReason bdr with(nolock) on bdr.ID = bi.ReasonID and bdr.SubProcessID = bi.SubProcessID
where bi.BundleNo='{drSelected["BundleNo"]}'
";
                DataTable dt;
                DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: $"{drSelected["BundleNo"]} Bundle Replacement");
            };

            #region 欄位設定
            this.grid.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("BundleNo", header: "Bundle No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("POID", header: "Master SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Category", header: "Category", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ProgramID", header: "Program", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CutCellID", header: "Cut Cell", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Sewinglineid", header: "Inline Line#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("PatternPanel", header: "Comb", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Cutpart", header: "Cutpart", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CutpartName", header: "Cutpart Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("SubProcessID", header: "Artwork", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("BundleGroup", header: "Group#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Qty", header: "Allocated Qty", width: Widths.AnsiChars(6))
            .Text("ReceiveQtySorting", header: "Receive Qty Sorting", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ReceiveQtyLoading", header: "Receive Qty Loading", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("XXXRFIDIn", header: "RFID In", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("XXXRFIDOut", header: "RFID Out", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("BundleReplacement", header: "Bundle Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: BundleReplacement)
            .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            #endregion
        }

        private bool validate()
        {
            if (this.txtEstCutDate.Value == null && string.IsNullOrEmpty(this.txtSPNo.Text))
            {
                return false;
            }

            return true;
        }

        private void Query()
        {
            DataTable dt;
            DualResult result;
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            #region WHERE條件

            if (this.txtEstCutDate.Value != null)
            {
                sqlWhere.Append($"AND w.EstCutDate='{((DateTime)this.txtEstCutDate.Value).ToShortDateString()}'" + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(this.txtSPNo.Text))
            {
                sqlWhere.Append($"AND b.Orderid='{this.txtSPNo.Text}'" + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(this.txtFactory.Text))
            {
                sqlWhere.Append($"AND o.FtyGroup='{this.txtFactory.Text}'" + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(this.txtCutCell.Text))
            {
                sqlWhere.Append($"AND w.CutCellid='{this.txtCutCell.Text}'" + Environment.NewLine);
            }

            // Inline Line#
            if (!string.IsNullOrEmpty(this.txtSewingLineID.Text))
            {
                sqlWhere.Append($"AND b.Sewinglineid='{this.txtSewingLineID.Text}'" + Environment.NewLine);
            }

            /*
                All : 顯示全部
                Y : 篩選 Loading 已經有 InComing 的 Bundle
                N : 篩選 Loading 尚未 InComing 的 Bundle
             */
            if (this.comboComplete.Text == "Y")
            {
                sqlWhere.Append($"AND (ReceiveQtyLoading.InComing != '' OR ReceiveQtyLoading.InComing IS NOT NULL)" + Environment.NewLine);
            }

            if (this.comboComplete.Text == "N")
            {
                sqlWhere.Append($"AND (ReceiveQtyLoading.InComing = '' OR ReceiveQtyLoading.InComing IS NULL)" + Environment.NewLine);
            }

            if (this.chkAllPartOnly.Checked)
            {
                sqlWhere.Append($"AND bd.PatternCode ='ALLPARTS' " + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(this.comboSubPorcess.Text))
            {
                sqlWhere.Append($"AND ( DefaultSubProcess.SubProcessID LIKE '%{this.comboSubPorcess.Text}%' OR HasSubProcess.Value > 0 )" + Environment.NewLine);
            }

            sqlWhere.Append($"ORDER BY b.Colorid,bd.SizeCode,b.PatternPanel,bd.BundleNo");

            // 「Extend All Parts」勾選則true
            // string IsExtendAllParts = this.chkExtendAllParts.Checked ? "true" : "false";
            string SubProcess = this.comboSubPorcess.Text;
            #endregion

            sqlCmd.Append($@"
SELECT
bd.BundleNo
,b.Orderid
,b.POID
,o.FactoryID
,o.Category
,o.ProgramID
,o.StyleID
,o.SeasonID
,b.Colorid
,b.Article
,bd.SizeCode
,w.CutCellID
,b.Sewinglineid
,b.PatternPanel

,[Cutpart]= bd.Patterncode
,[CutpartName]= {(this.chkExtendAllParts.Checked ?
                    "CASE WHEN bd.Patterncode = 'ALLPARTS' THEN bdap.PatternDesc ELSE bd.PatternDesc END --basic from 「Extend All Parts」 is checked or not"
                    :
                    "bd.PatternDesc")}
,[SubProcessID]= SubProcess.SubProcessID
,[DefaultSubProcess]=DefaultSubProcess.SubProcessID
,bd.BundleGroup
,bd.Qty
,[ReceiveQtySorting]= IIF(ReceiveQtySorting.OutGoing != '' OR ReceiveQtySorting.OutGoing IS NOT NULL , 'Complete' ,'Not Complete')
,[ReceiveQtyLoading]= IIF(ReceiveQtyLoading.InComing != '' OR ReceiveQtyLoading.InComing IS NOT NULL , 'Complete' ,'Not Complete')
,[XXXRFIDIn]=bio.InComing
,[XXXRFIDOut]=bio.OutGoing
, ps.NoBundleCardAfterSubprocess_String
, nbs.PostSewingSubProcess_String
,b.CutRef
into #tmpMain
FROM Bundle b
INNER JOIN Bundle_Detail bd ON bd.ID=b.Id
{(this.chkExtendAllParts.Checked ? "LEFT JOIN Bundle_Detail_AllPart bdap ON bdap.ID=b.ID AND bd.Patterncode ='ALLPARTS'" : string.Empty)}
INNER JOIN Orders O ON o.ID=b.Orderid
LEFT JOIN Workorder w ON W.CutRef=b.CutRef AND w.ID=b.POID
LEFT JOIN BundleInOut ReceiveQtySorting ON ReceiveQtySorting.BundleNo=bd.BundleNo AND ReceiveQtySorting.RFIDProcessLocationID ='' AND ReceiveQtySorting.SubProcessId='Sorting'
LEFT JOIN BundleInOut ReceiveQtyLoading ON ReceiveQtyLoading.BundleNo=bd.BundleNo AND ReceiveQtyLoading.RFIDProcessLocationID ='' AND ReceiveQtyLoading.SubProcessId='Loading'
LEFT JOIN BundleInOut bio ON bio.BundleNo=bd.BundleNo AND bio.RFIDProcessLocationID ='' AND bio.SubProcessId='{SubProcess}'
OUTER APPLY(
	--用來判斷，該Bundle ID、Bundle No，是否包含User選定的SubProcess
	SELECT [Value]=IIF( COUNT(bda.SubProcessID) > 0 , 1 ,0 )
	FROM Bundle_Detail_Art bda
	WHERE  bda.BundleNo = bd.BundleNo 
	AND bda.ID = b.ID   
	AND bda.SubProcessID ='{SubProcess}'
)HasSubProcess

OUTER APPLY(
    --顯示該Bundle ID、Bundle No，所有的SubProcess
	SELECT [SubProcessID]=LEFT(SubProcessID,LEN(SubProcessID)-1)  
	FROM
	(
		SELECT [SubProcessID]=
		(
			SELECT ID+ ' + '
			FROM SubProcess s
			WHERE EXISTS
			(
				SELECT 1 FROM Bundle_Detail_Art bda
				WHERE  bda.BundleNo = bd.BundleNo 
				AND bda.ID = b.ID   
				AND bda.SubProcessID = s.ID
			)
			FOR XML PATH('')
		)
	)M
)SubProcess

outer apply
(
    select NoBundleCardAfterSubprocess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=bd.BundleNo and e1.NoBundleCardAfterSubprocess = 1
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
) as ps
outer apply
(
    select PostSewingSubProcess_String = 
    stuff((
        select concat('+',e1.Subprocessid)
        from dbo.Bundle_Detail_Art e1 WITH (NOLOCK)
        where e1.id=b.id and e1.Bundleno=bd.BundleNo and e1.PostSewingSubProcess = 1
		Order by e1.Subprocessid
        for xml path('')
    ),1,1,'')
) as nbs

OUTER APPLY(
	--每個Bundle都會有的SubProcess
	SELECT [SubProcessID]=LEFT(SubProcessID,LEN(SubProcessID)-1)  
	FROM
	(
		SELECT [SubProcessID]=
		(
			SELECT ID+ ' + '
			FROM SubProcess s
			WHERE 
			s.IsRFIDDefault = 1
			FOR XML PATH('')
		)
	)M
)DefaultSubProcess
WHERE o.MDivisionID='{Sci.Env.User.Keyword}' {sqlWhere}

select
bi.BundleNo,
bi.ReasonID,
bi.DefectQty,
bi.ReplacementQty
into #tmpBundleInspection
from dbo.SciMES_BundleInspection bi  with(nolock)
where exists(select 1 from #tmpMain where BundleNo = bi.BundleNo) and isnull(bi.DefectQty,0) <> isnull(bi.ReplacementQty,0)


select 
t.BundleNo
,t.Orderid
,t.POID
,t.FactoryID
,t.Category
,t.ProgramID
,t.StyleID
,t.SeasonID
,t.Colorid
,t.Article
,t.SizeCode
,t.CutCellID
,t.Sewinglineid
,t.PatternPanel
,t.Cutpart
,t.CutpartName
,t.SubProcessID
,t.DefaultSubProcess
,t.BundleGroup
,t.Qty
,t.ReceiveQtySorting
,t.ReceiveQtyLoading
,t.XXXRFIDIn
,t.XXXRFIDOut
,BundleReplacement = BR.RQ
, t.NoBundleCardAfterSubprocess_String
, t.PostSewingSubProcess_String
, CutRef
from #tmpMain t
outer apply(
    select RQ=stuff((
	    select concat(',',ReasonID,'(',sum(isnull(bi.DefectQty,0)-isnull(bi.ReplacementQty,0)),')')
	    from #tmpBundleInspection bi  with(nolock)
	    where bi.BundleNo = t.BundleNo
	    group by ReasonID
	    for xml path('')
    ),1,1,'')
)BR

drop table #tmpMain,#tmpBundleInspection

");

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out dt);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found !!");
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (!this.validate())
            {
                MyUtility.Msg.WarningBox("Est.Cut Date and SP# cannot all empty.");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");
            this.Query();

            this.grid.Columns["XXXRFIDIn"].HeaderText = this.comboSubPorcess.Text + " RFID In";
            this.grid.Columns["XXXRFIDOut"].HeaderText = this.comboSubPorcess.Text + " RFID Out";

            // Helper.Controls.Grid.Generator(this.grid)
            // .Text("BundleNo", header: "Bundle No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("Orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("POID", header: "Master SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("Category", header: "Category", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("ProgramID", header: "Program", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("SeasonID", header: "Season", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("Colorid", header: "Color", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("Article", header: "Article", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("SizeCode", header: "Size", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("CutCellID", header: "Cut Cell", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("Sewinglineid", header: "Inline Line#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("PatternPanel", header: "Comb", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("Cutpart", header: "Cutpart", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("CutpartName", header: "Cutpart Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
            // .Text("SubProcessID", header: "Artwork", width: Widths.AnsiChars(30), iseditingreadonly: true)
            // .Text("BundleGroup", header: "Group#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Numeric("Qty", header: "Allocated Qty", width: Widths.AnsiChars(6))
            // .Text("ReceiveQtySorting", header: "Receive Qty Sorting", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("ReceiveQtyLoading", header: "Receive Qty Sorting", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("XXXRFIDIn", header: this.comboSubPorcess.Text+" RFID In", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // .Text("XXXRFIDOut", header: this.comboSubPorcess.Text + " RFID Out", width: Widths.AnsiChars(13), iseditingreadonly: true)
            ////.Text("", header: "Fab. Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true)
            // ;
            this.HideWaitMessage();
        }
    }
}
