using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P07 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P07"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.SetDetailGrid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="P07"/> class.
        /// </summary>
        /// <param name="sp">SP</param>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P07(string sp, ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.SetDetailGrid();
        }

        /// <summary>
        /// 隨著 P02上下筆SP#切換資料
        /// </summary>
        /// <param name="p02SPNo">P02 SP No</param>
        public void P07Data(string p02SPNo)
        {
            this.EditMode = true;
            this.txtCuttingSPNo.Text = p02SPNo;
            this.Queryable();
        }

        /// <summary>
        /// Set Txt SP No
        /// </summary>
        /// <param name="spNo">sp No</param>
        public void SetTxtSPNo(string spNo)
        {
            this.txtCuttingSPNo.Text = spNo;
            this.txtfactoryByM.Text = string.Empty;
            this.txtSPNo.Text = string.Empty;
            this.txtSEQ.Text = string.Empty;
            this.dateEstCutDate.Text = string.Empty;
            this.dateSewingInline.Text = string.Empty;
            this.txtCutRefNo.Text = string.Empty;
            this.txtCutplanID.Text = string.Empty;
        }

        /// <summary>
        /// Set Detail Grid
        /// </summary>
        public void SetDetailGrid()
        {
            #region set grid
            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .Text("factoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ID", header: "Cutting SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Date("OrgEstCutDate", header: "Org.Est.\nCut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("NewEstCutDate", header: "New Est.\nCut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("OrgSpreadingNoID", header: "Org.\nSpreading No", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("NewSpreadingNoID", header: "New\nSpreading No", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("OrgCutCellID", header: "Org.\nCut Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("NewCutCellID", header: "New\nCut Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("CutReasonID", header: "Reason", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("ReasonDescription", header: "Reason Desc", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("PatternPanel", header: "Pattern Panel", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
            .Text("AddName", header: "Change by", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            #endregion
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Queryable();
        }

        /// <summary>
        /// Query table
        /// </summary>
        public void Queryable()
        {
            this.gridDetail.DataSource = null;
            string cutsp = this.txtCuttingSPNo.Text;
            string sp = this.txtSPNo.Text;
            string seq = this.txtSEQ.Text;
            string estcutdate = this.dateEstCutDate.Text;
            string sewinginline = this.dateSewingInline.Text;
            string cutref = this.txtCutRefNo.Text;
            string cutplanID = this.txtCutplanID.Text;
            if (MyUtility.Check.Empty(cutsp) && MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(this.dateEstCutDate.Value) && MyUtility.Check.Empty(cutplanID))
            {
                MyUtility.Msg.WarningBox("You must be entry conditions <Cutting SP#> or <SP#> or <Est. Cut Date> or <CutplanID>");
                return;
            }

            string sql = @"
Select * 
From
(
    Select a.FactoryID,a.id,a.OrderID,a.SEQ1,a.SEQ2,we.OrgEstCutDate,we.NewEstCutDate,a.CutRef,a.Cutno,
		we.OrgSpreadingNoID,we.NewSpreadingNoID,we.OrgCutCellid,we.NewCutCellid,we.CutReasonid,
		ReasonDescription=(select cr.Description from CutReason cr where id = we.CutReasonid),
		a.Markername,a.FabricCombo,a.Colorid,a.layer,
		addName=concat(format(we.AddDate,'yyyy/MM/dd HH:mm:ss'),' ',dbo.getPass1 (we.AddName)),
        (
            Select distinct Article+'/ ' 
	        From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
	        Where b.workorderukey = a.Ukey and b.article!=''
            For XML path('')
        ) as article,
        (
            Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
            From WorkOrder_SizeRatio c WITH (NOLOCK) 
            Where c.WorkOrderUkey =a.Ukey 
            For XML path('')
        ) as SizeCode,
        (
            Select PatternPanel+'+ ' 
            From WorkOrder_PatternPanel c WITH (NOLOCK) 
            Where c.WorkOrderUkey =a.Ukey 
            For XML path('')
        ) as PatternPanel,
        (
	        Select  isnull(CONVERT (DATE, Min(sew.Inline),101),'')  as inline 
	        From SewingSchedule sew WITH (NOLOCK) ,SewingSchedule_detail sew_b,WorkOrder_Distribute h WITH (NOLOCK) 
	        Where h.WorkOrderUkey = a.ukey and sew.id=sew_b.id and h.orderid = sew_b.OrderID 
			        and h.Article = sew_b.Article and h.SizeCode = h.SizeCode and h.orderid = sew.orderid
        )  as Sewinline,
        (
	        Select isnull(Min(cut.cdate),null)
	        From cuttingoutput cut WITH (NOLOCK) ,cuttingoutput_detail cut_b WITH (NOLOCK) 
	        Where cut_b.workorderukey = a.Ukey and cut.id = cut_b.id
        )  as actcutdate
    from Workorder a
	inner join WorkOrder_Estcutdate we with(nolock) on a.Ukey = we.WorkOrderUkey

";
            string where = string.Format(" Where a.cutplanid!='' and a.MDivisionId = '{0}'", Env.User.Keyword);
            if (!MyUtility.Check.Empty(cutsp))
            {
                where = where + string.Format(" and a.id='{0}'", cutsp);
            }

            if (!MyUtility.Check.Empty(sp))
            {
                where = where + string.Format(" and a.OrderID='{0}'", sp);
            }

            if (!MyUtility.Check.Empty(seq))
            {
                where = where + string.Format(" and a.Seq1+a.SEQ2='{0}'", seq);
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate.Value))
            {
                where = where + string.Format("and a.estcutdate='{0}'", estcutdate);
            }

            if (!MyUtility.Check.Empty(cutref))
            {
                where = where + string.Format(" and a.cutref='{0}'", cutref);
            }

            if (!MyUtility.Check.Empty(this.txtfactoryByM.Text))
            {
                where = where + string.Format(" and a.Factoryid='{0}'", this.txtfactoryByM.Text);
            }

            if (!MyUtility.Check.Empty(cutplanID))
            {
                where = where + string.Format(" and a.CutplanID='{0}'", cutplanID);
            }

            sql = sql + where + " ) as #tmp ";
            where = " Where 1=1 and actcutdate is null ";
            if (!MyUtility.Check.Empty(this.dateSewingInline.Value))
            {
                where = where + string.Format(" and Sewinline='{0}'", sewinginline);
            }

            sql = sql + where;
            DualResult dResult = DBProxy.Current.Select(null, sql, out DataTable detailTb);
            if (!dResult)
            {
                this.ShowErr(dResult);
                return;
            }

            if (detailTb.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found!!");
                return;
            }

            this.gridDetail.DataSource = this.gridbs;
            this.gridbs.DataSource = detailTb;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
