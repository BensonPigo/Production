using Ict.Win.UI;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Widths = Ict.Win.Widths;
using Ict;
using Sci.Data;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P04_FabricDeleteHistory : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P04_FabricDeleteHistory(string ID)
        {
            this.InitializeComponent();
            string sqlcmd = $@"
            SELECT
            [Sewinglineid] = cddh.Sewinglineid
            ,[CutRef] = cddh.CutRef
            ,[CutNo] = cddh.CutNo
            ,[FabricCombo] = wo.FabricCombo
            ,[FabricCode] = wo.FabricCode
            ,[FabricPanelCode] = wo.FabricPanelCode
            ,[OrderID] = cddh.OrderID
            ,[WeaveTypeID] = f.WeaveTypeID
            ,[SCIRefno] = wo.SCIRefno
            ,[Refno] = wo.Refno
            ,[Article] = Article.val
            ,[ColorID] = cddh.Colorid
            ,[SEQ1] = wo.SEQ1
            ,[SEQ2] = wo.SEQ2
            ,[Size] = Size.val
            ,[TotalCutQty] = TotalCutQty.val
            ,[Cons] = cddh.Cons
            ,[Remark] = cddh.Remark
            ,[AddDate] = cddh.Adddate
            from CutPlan_DetailDeletedHistory cddh
            LEFT JOIN WorkOrderForPlanning wo WITH(NOLOCK) ON wo.Ukey = cddh.WorkOrderForPlanningUkey
            LEFT JOIN Fabric f WITH(NOLOCK) ON f.SCIRefno = wo.SCIRefno
            OUTER APPLY
            (
	            select val = stuff(
	            (
		            Select CONCAT(',', c.sizecode+'/ '+ convert(varchar(8),c.qty))
		            From WorkOrderForPlanning_SizeRatio c WITH (NOLOCK) 
		            Where  c.WorkOrderForPlanningUkey = cddh.WorkOrderForPlanningUkey 
		             For XML path('')
	            ),1,1,'')
            ) AS Size
            OUTER APPLY
            (
	            select val = stuff(
	            (
		            Select  CONCAT(',', c.sizecode+'/ '+convert(varchar(8),c.qty*wo.layer))
		            From WorkOrderForPlanning_SizeRatio c WITH (NOLOCK) 
		            Where  c.WorkOrderForPlanningUkey = cddh.WorkOrderForPlanningUkey and c.WorkOrderForPlanningUkey = wo.Ukey
		            For XML path('')

	            ),1,1,'')
            ) AS TotalCutQty
            OUTER APPLY
            (
	            select val = stuff(
	            (
		            Select distinct CONCAT('/ ', wpd.Article)
                    From dbo.WorkOrderForPlanning_Distribute wpd WITH (NOLOCK) 
                    Where wpd.WorkOrderForPlanningUkey = wo.Ukey and wpd.Article!=''
                    For XML path('')
	            ),1,1,'')
            ) AS Article
            WHERE cddh.id = '{ID}'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                this.grid1.DataSource = dt;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Sewinglineid", header: "Line#", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("Sewinglineid", header: "CutRef#", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("FabricCombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("Fabriccode", header: "Fabric\r\nCode", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Size", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("TotalCutQty", header: "Total\r\nCutQty", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("Cons", header: "Cons", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Date("AddDate", header: "Deleted Date", width: Widths.AnsiChars(20), iseditingreadonly: true);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
