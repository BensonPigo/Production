﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Subcon
{
    public partial class P41 : Sci.Win.Tems.QueryForm
    {
        public P41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;

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

            //排除非生產工廠
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.FilteMDivision = true;
            this.txtFactory.Text = Sci.Env.User.Factory;


        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 欄位設定
            this.grid.DataSource = listControlBindingSource1;

            Helper.Controls.Grid.Generator(this.grid)
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
            .Text("SubProcessID", header: "Artwork", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("BundleGroup", header: "Group#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Qty", header: "Allocated Qty", width: Widths.AnsiChars(6))
            .Text("ReceiveQtySorting", header: "Receive Qty Sorting", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ReceiveQtyLoading", header: "Receive Qty Sorting", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("XXXRFIDIn", header: "RFID In", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("XXXRFIDOut", header: "RFID Out", width: Widths.AnsiChars(13), iseditingreadonly: true)            
            //.Text("", header: "Fab. Replacement", width: Widths.AnsiChars(13), iseditingreadonly: true)
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
            DualResult result ;
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            #region WHERE條件

            if (this.txtEstCutDate.Value != null)
            {
                sqlWhere.Append($"AND w.EstCutDate='{((DateTime)this.txtEstCutDate.Value).ToShortDateString()}'" +Environment.NewLine);
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
            if (this.comboComplete.Text=="Y")
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
            

            sqlWhere.Append($"ORDER BY b.Colorid,bd.SizeCode,b.PatternPanel,bd.BundleNo");

            //「Extend All Parts」勾選則true
            string IsExtendAllParts = this.chkExtendAllParts.Checked ? "true" : "false";

            string SubProcess = this.comboSubPorcess.Text;
            #endregion

            sqlCmd.Append($@"

SELECT DISTINCT
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
,[CutpartName]= CASE WHEN '{IsExtendAllParts}'='true'  AND  bd.Patterncode ='ALLPARTS' THEN  bdap.PatternDesc
				ELSE bd.PatternDesc  
				END --basic from 「Extend All Parts」 is checked or not

,[SubProcessID]= SubProcess.SubProcessID
,bd.BundleGroup
,bd.Qty
,[ReceiveQtySorting]= IIF(ReceiveQtySorting.OutGoing != '' OR ReceiveQtySorting.OutGoing IS NOT NULL , 'Complete' ,'Not Complete')
,[ReceiveQtyLoading]= IIF(ReceiveQtyLoading.InComing != '' OR ReceiveQtyLoading.InComing IS NOT NULL , 'Complete' ,'Not Complete')
,[XXXRFIDIn]=bio.OutGoing
,[XXXRFIDOut]=bio.InComing

FROM Bundle b
INNER JOIN Bundle_Detail bd ON bd.ID=b.Id
INNER JOIN Bundle_Detail_AllPart bdap ON bdap.ID=b.ID
INNER JOIN Orders O ON o.ID=b.Orderid
LEFT JOIN Workorder w ON W.Refno=b.CutRef AND w.ID=b.POID
LEFT JOIN BundleInOut ReceiveQtySorting ON ReceiveQtySorting.BundleNo=bd.BundleNo AND ReceiveQtySorting.RFIDProcessLocationID ='' AND ReceiveQtySorting.SubProcessId='Sorting'
LEFT JOIN BundleInOut ReceiveQtyLoading ON ReceiveQtyLoading.BundleNo=bd.BundleNo AND ReceiveQtyLoading.RFIDProcessLocationID ='' AND ReceiveQtyLoading.SubProcessId='Loading'
LEFT JOIN BundleInOut bio ON bio.BundleNo=bd.BundleNo AND bio.RFIDProcessLocationID ='' AND bio.SubProcessId='{SubProcess}'
OUTER APPLY(
	SELECT [SubProcessID]=LEFT(SubProcessID,LEN(SubProcessID)-1)  
	FROM
	(
		SELECT [SubProcessID]=
		(
			SELECT  SubProcessID + ' + '
			FROM Bundle_Detail_Art bda
			WHERE bda.ID=bd.Id AND bda.Bundleno=bd.BundleNo
			AND EXISTS( SELECT 1 FROM SubProcess s WHERE s.Id=bda.SubprocessId AND s.IsRFIDDefault=0)
			AND bda.SubProcessID='{SubProcess}'  --篩選條件
			FOR XML PATH('')
		)
	)M
)SubProcess
WHERE o.MDivisionID='{Sci.Env.User.Keyword}' AND ( SubProcess.SubProcessID LIKE '%{SubProcess}%' OR bd.Patterncode='ALLPARTS')

");

            result=DBProxy.Current.Select(null, sqlCmd.Append(sqlWhere).ToString(), out dt);

            if (!result)
            {
                MyUtility.Msg.WarningBox("DB Error");
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

            if (!validate())
            {
                MyUtility.Msg.WarningBox("Est.Cut Date and SP# cannot all empty.");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");
            Query();

            this.HideWaitMessage();
        }
    }
}
