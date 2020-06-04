using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P08_Import : Sci.Win.Subs.Base
    {
        DataTable gridTable;
        DataTable copyTable;
        public P08_Import()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false; //必設定
            Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("CuttingID", header: "CuttingID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Fab Combo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("MarkerName", header: "Mark Name", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Numeric("Each Cons", header: "Each Cons", width: Widths.AnsiChars(2), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .DateTime("Mtl ETA", header: "Mtl ETA", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .DateTime("1stSewingDate", header: "1st Sewing Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Type of Cutting", header: "Type of Cutting", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Cut Width", header: "Cut Width", width: Widths.AnsiChars(2), integer_places: 8, decimal_places: 4, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 5, decimal_places: 2, iseditingreadonly: true)
            .Numeric("ReleaseQty", header: "Release Qty", width: Widths.AnsiChars(8), integer_places: 5, decimal_places: 2)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5))
            .DateTime("SCI Dlv", header: "SCI Dlv.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("Buyer Dlv", header: "Buyer Dlv..", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Ref#", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Color Desc", header: "Color Desc", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
            ;

            this.gridImport.Columns["Selected"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["ReleaseQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["Dyelot"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateEstCutDate.Value))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> can not be empty.");
                return;
            }

            this.listControlBindingSource1.DataSource = null;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ID", this.txtCuttingID.Text));

            string sqlcmd = $@"
declare @CuttingID varchar(13) = @ID
select
	Selected = CAST(0 as bit)
	, o.FactoryID
	, CuttingID = @CuttingID
	, [Fab Combo] = e.FabricCombo
	, MarkerName = e.MarkerName
	, [Each Cons] = format(o.EachConsApv,'yyyy/MM/dd')
	, [Mtl ETA] = format(mtl.ETA,'yyyy/MM/dd')
	, [1stSewingDate] = format(CutCombo.[1stSewingDate],'yyyy/MM/dd')
	, [Type of Cutting] = e.Direction
	, [Cut Width] = e.CuttingWidth
	, Qty = c.YDS
	, ReleaseQty = c.YDS
	, Seq1 = mtl.Seq1
	, Seq2 = mtl.Seq2
	, [Dyelot] = ''
	, [SCI Dlv] = format(CutCombo.SciDelivery,'yyyy/MM/dd')
	, [Buyer Dlv] = format(Cutcombo.BuyerDelivery,'yyyy/MM/dd')
	, [Ref#] = Fabric.Refno
	, [Color Desc] = col.ColorDesc
	, Remark.remark
	, c.ColorID
	, Order_EachConsUkey = e.Ukey
	, POID = o.POID
    , MDivisionID = (select MDivisionID from Factory where Id = o.FactoryID)
from dbo.Order_EachCons e WITH (NOLOCK)
left join dbo.Order_EachCons_Color c WITH (NOLOCK) on c.Order_EachConsUkey = e.Ukey
left join dbo.Order_BOF bof WITH (NOLOCK) on bof.Id = e.Id and bof.FabricCode = e.FabricCode
left join dbo.Fabric WITH (NOLOCK) on Fabric.SCIRefno = bof.SCIRefno
left join dbo.Orders o WITH (NOLOCK) on o.ID = e.Id
outer apply(
	select 
		BuyerDelivery= min(t.BuyerDelivery)
		, SciDelivery= min(t.SciDelivery)
		, [1stSewingDate] = min(t.SewInLine)
	from dbo.Orders t WITH (NOLOCK) 
	where t.CuttingSP = @CuttingID And t.IsForecast = 0 AND t.Junk = 0 AND t.LocalOrder = 0 AND t.category not in('M','T')
) CutCombo
outer apply(
	select ETA = max(FinalETA), Seq1 = min(po3.SEQ1), Seq2 = min(po3.SEQ2)
	from (
		select s.FinalETA, s.SEQ1, s.SEQ2
		from dbo.PO_Supp_Detail s WITH (NOLOCK) 
		where s.id = o.poid AND s.SCIRefno = bof.SCIRefno AND s.ColorID = c.ColorID 
			AND s.SEQ1 = 'A1' AND ((s.Special NOT LIKE ('%DIE CUT%')) and s.Special is not null)		
		union all
		select b1.FinalETA , s.SEQ1, s.SEQ2
		from PO_Supp_Detail s WITH (NOLOCK) , PO_Supp_Detail b1 WITH (NOLOCK) 
		where s.id = o.poid AND s.SCIRefno = bof.SCIRefno AND s.ColorID = c.ColorID 
			AND s.SEQ1 = 'A1' AND ((s.Special NOT LIKE ('%DIE CUT%')) and s.Special is not null) 
			AND b1.ID = s.StockPOID   and b1.SEQ1 = s.Stockseq1 and b1.SEQ2 = s.StockSeq2
	) po3
) mtl
outer apply(
	select ColorDesc = color.Name	
	from PO_Supp_Detail b  WITH (NOLOCK) ,color WITH (NOLOCK) 
	where b.ID = o.poid and b.SEQ1 = mtl.Seq1 and b.SEQ2 = mtl.Seq2
		and color.id = b.ColorID and color.BrandId = o.brandid
) col
outer apply(
	select remark = STUFF((
		select concat(',',wd.OrderID,'\',wd.Article,'\',wd.SizeCode,'\',wd.Qty)
		from(
			select distinct wd.OrderID,wd.Article,wd.SizeCode,wd.Qty,wd.WorkOrderUkey
			from WorkOrder w with(nolock)
			inner join WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w.Ukey
			where w.Order_EachconsUkey = e.Ukey
		)wd
		order by wd.OrderID,wd.Article,wd.SizeCode,wd.Qty
		for xml path('')
	),1,1,'')
)remark
where e.Id = @CuttingID and e.CuttingPiece = 1 
order by MarkerName, ColorID 

";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out gridTable);
            if (!result)
            {
                ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = gridTable;
        }

        private void DateEstCutDate_Validating(object sender, CancelEventArgs e)
        {
            // 只能輸入今天和未來的日期
            if (MyUtility.Check.Empty(this.dateEstCutDate.Value) && ((DateTime)this.dateEstCutDate.Value).Date < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't earlier than today!");
                e.Cancel = true;
                return;
            }
        }

        public List<String> importedIDs = new List<string>();
        private void btnImport_Click(object sender, EventArgs e)
        {
            importedIDs.Clear();
            if (this.listControlBindingSource1.DataSource == null || gridTable.Rows.Count == 0)
            {
                return;
            }

            // 只能輸入今天和未來的日期
            if (MyUtility.Check.Empty(this.dateEstCutDate.Value) && ((DateTime)this.dateEstCutDate.Value).Date < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("Est. Cutting Date can't earlier than today!");
                dateEstCutDate.Value = null;
                return;
            }

            // Release Qty 要大於 0
            if (this.gridTable.AsEnumerable().Where(w => MyUtility.Convert.GetDecimal(w["IssueQty"]) <= 0).Any())
            {
                MyUtility.Msg.WarningBox("Dyelot or Release Qty can't empty!");
                return;
            }

            #region 相同的 CuttingID, FabCombo, MarkerName, ColorID, Dyelot 只能有出現一次
            if (this.gridTable.AsEnumerable()
                .GroupBy(g => new
                {
                    CuttingID = g["CuttingID"].ToString(),
                    FabCombo = g["FabCombo"].ToString(),
                    MarkerName = g["MarkerName"].ToString(),
                    ColorID = g["ColorID"].ToString(),
                    Dyelot = g["Dyelot "].ToString(),
                })
                .Select(s => new
                {
                    s.Key.CuttingID,
                    s.Key.FabCombo,
                    s.Key.MarkerName,
                    s.Key.ColorID,
                    s.Key.Dyelot,
                    ct = s.Count()
                }).Any(r => r.ct > 1))
            {
                MyUtility.Msg.WarningBox("CuttingID, Fabric Combo, MarkerName, ColorID, Dyelot can not duplicate!!");
                return;
            }
            #endregion

            #region 檢查 Release Qty, 依據 FabCombo, MarkerName, ColorID 做群組加總, Sum of Release Qty 不能大於該群組的 Qty
            var x = this.gridTable.AsEnumerable()
                .GroupBy(g => new
                {
                    CuttingID = g["CuttingID"].ToString(),
                    FabCombo = g["FabCombo"].ToString(),
                    MarkerName = g["MarkerName"].ToString(),
                    ColorID = g["ColorID"].ToString(),
                    Qty = MyUtility.Convert.GetDecimal(g["Qty"])
                })
                .Select(s => new
                {
                    s.Key.CuttingID,
                    s.Key.FabCombo,
                    s.Key.MarkerName,
                    s.Key.ColorID,
                    s.Key.Qty,
                    IssueQty = s.Sum(i => MyUtility.Convert.GetDecimal(i["IssueQty"]))
                });
            if (x.Where(w => w.IssueQty > w.Qty).Any())
            {
                MyUtility.Msg.WarningBox("According to <Fab Combo>, <Mark Name> and <Color> the total <Release Qty> cannot be greater than <Qty>");
                return;
            }
            #endregion


            string cutTapePlanID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CT", "CutTapePlan");
            importedIDs.Add(cutTapePlanID);
            string insertsql = $@"
INSERT INTO [dbo].[CutTapePlan]
           ([ID]
           ,[CuttingID]
           ,[MDivisionID]
           ,[FactoryID]
           ,[EstCutDate]
           ,[Status]
           ,[AddDate]
           ,[AddName])
     VALUES
           ('{cutTapePlanID}'
           ,'{this.gridTable.Rows[0]["CuttingID"]}'
           ,'{this.gridTable.Rows[0]["MDivisionID"]}'
           ,'{this.gridTable.Rows[0]["FactoryID"]}'
           ,'{((DateTime)this.dateEstCutDate.Value).ToString("yyyy/MM/dd")}'
           ,'New'
           ,getdate()
           ,'{Sci.Env.User.UserID}')
GO
INSERT INTO [dbo].[CutTapePlan_Detail]
           ([ID]
           ,[MarkerName]
           ,[ColorID]
           ,[Dyelot]
           ,[RefNo]
           ,[Qty]
           ,[ReleaseQty]
           ,[FabricCombo]
           ,[Direction]
           ,[CuttingWidth]
           ,[ConsPC]
           ,[Seq1]
           ,[Seq2]
           ,[Remark]
           ,[Order_EachConsUkey])
select
    [ID]='{cutTapePlanID}'
    ,[MarkerName]
    ,[ColorID]
    ,[Dyelot]
    ,[RefNo]
    ,[Qty]
    ,[ReleaseQty]
    ,[FabricCombo]
    ,[Direction]
    ,[CuttingWidth]
    ,[ConsPC]
    ,[Seq1]
    ,[Seq2]
    ,[Remark]
    ,[Order_EachConsUkey]
from #tmp
";


            #region transaction
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                if (!(upResult = DBProxy.Current.Execute(null, insertsql)))
                {
                    _transactionscope.Dispose(); // 彈窗前,要先釋放,不然會咬住
                    this.ShowErr(upResult);
                    return;
                }

                _transactionscope.Complete();
            }

            MyUtility.Msg.WarningBox("Successfully");
            #endregion
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void BtnSplit_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || gridTable.Rows.Count == 0) return;

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            this.gridTable = null;
        }
    }
}
