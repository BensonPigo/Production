using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Linq;
using Sci.Win.UI;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P05_Import : Win.Subs.Base
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="P05_Import"/> class.
        /// </summary>
        public P05_Import()
        {
            this.InitializeComponent();
            this.txtCutCell.MDivisionID = this.keyWord;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("POID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ID", header: "Cutting SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("CutCellID", header: "Cut Cell", width: Widths.AnsiChars(13), iseditingreadonly: true);

            this.gridImport.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateEstCutDate.Value) && MyUtility.Check.Empty(this.txtCuttingSP.Text) && MyUtility.Check.Empty(this.txtMarkName.Text))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date>, <Cutting SP#> and <Marker Name> can not all be empty.");
                return;
            }

            this.gridBS.DataSource = null;  // 開始查詢前先清空資料

            StringBuilder where = new StringBuilder();
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@MDivisionID", this.keyWord));

            if (!MyUtility.Check.Empty(this.dateEstCutDate.Value))
            {
                where.AppendLine("and wofo.EstCutDate = @EstCutDate");
                paras.Add(new SqlParameter("@EstCutDate", this.dateEstCutDate.Text));
            }

            if (!MyUtility.Check.Empty(this.txtCutCell.Text))
            {
                where.AppendLine("and wofo.CutCellID = @CutCellID");
                paras.Add(new SqlParameter("@CutCellID", this.txtCutCell.Text));
            }

            if (!MyUtility.Check.Empty(this.txtCuttingSP.Text))
            {
                where.AppendLine("and wofo.ID = @CuttingSP");
                paras.Add(new SqlParameter("@CuttingSP", this.txtCuttingSP.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMarkName.Text))
            {
                where.AppendLine("and wofo.MarkerName = @MarkerName");
                paras.Add(new SqlParameter("@MarkerName", this.txtMarkName.Text));
            }

            string sqlcmd = $@"
;with main as (
	select distinct wofo.CutRef, wofo.EstCutDate
	from WorkOrderForOutput wofo with (nolock)
	where not exists (select 1 from MarkerReq_Detail_CutRef mrd with (nolock) where mrd.CutRef = wofo.CutRef)
	and wofo.MDivisionID = @MDivisionID
    {where}
)
select Sel = cast(0 as bit), m.CutRef, Top1Data.POID, Top1Data.ID, Top1Data.FtyGroup, Top1Data.CutCellID, m.EstCutDate
from main m
cross apply (
	select top 1 o.POID, o.ID, o.FtyGroup, wofo.CutCellID
	from WorkOrderForOutput wofo with (nolock)
	left join Orders o with (nolock) on wofo.ID = o.ID
	where wofo.CutRef = m.CutRef
	order by wofo.CutCellID
) Top1Data
order by EstCutDate, FtyGroup, CutCellID, CutRef";

            DataTable dt;
            DualResult dResult = DBProxy.Current.Select(null, sqlcmd, paras, out dt);
            if (dResult)
            {
                this.gridBS.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
            }
            else
            {
                this.ShowErr(sqlcmd, dResult);
                return;
            }
        }

        /// <inheritdoc/>
        public List<string> ImportedIDs { get; set; } = new List<string>();

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable gridTable = (DataTable)this.gridBS.DataSource;
            var selectDT = gridTable.AsEnumerable().Where(x => MyUtility.Convert.GetBool(x["Sel"]) == true).TryCopyToDataTable(gridTable);
            if (selectDT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, cannot import!");
                return;
            }

            this.ImportedIDs.Clear();

            try
            {
                var sql = @"
/* 傳入參數
declare @MDivisionID varchar(8)
declare @UserID varchar(10)
*/

declare @TmpSelectDt table
( RowID int, EstCutDate date, FtyGroup varchar(8), CutCellID varchar(2), CutRef varchar(10), OrderID varchar(13), SizeRatio nvarchar(max), MarkerName varchar(20)
, Layer numeric(5, 0), FabricCombo varchar(2), MarkerNo varchar(10), WorkOrderForOutputUkey bigint, CuttingWidth varchar(8), PatternPanel varchar(120) )
	
declare @Group table (RowID int, EstCutDate date, FtyGroup varchar(8), CutCellID varchar(2), ID varchar(13))
declare @RowID int
declare @RowCount int

declare @Now datetime = getdate()
declare @EstCutDate date
declare @FtyGroup varchar(8)
declare @CutCellID varchar(2)
declare @NewID varchar(13)

-- 整理勾選項目，帶出表身資訊
insert into @TmpSelectDt
( RowID, EstCutDate, FtyGroup, CutCellID, CutRef, OrderID, SizeRatio, MarkerName
, Layer, FabricCombo, MarkerNo, WorkOrderForOutputUkey, CuttingWidth, PatternPanel)
select RowID = DENSE_RANK() over(order by tmp.EstCutDate, tmp.FtyGroup, tmp.CutCellID)
, tmp.EstCutDate
, tmp.FtyGroup
, tmp.CutCellID
, step1.CutRef
, step1.OrderID
, step2.SizeRatio
, step1.MarkerName
, step1.Layer
, step1.FabricCombo
, step1.MarkerNo
, step1.WorkOrderForOutputUkey
, step2.CuttingWidth
, step2.PatternPanel
from #tmp tmp
outer apply (
	select wofo.CutRef
	, wofo.OrderID
	, wofo.MarkerName
	, Layer = sum(wofo.Layer)
	, wofo.FabricCombo
	, wofo.MarkerNo
	, WorkOrderForOutputUkey = max(wofo.Ukey)
	, Order_EachconsUkey = max(wofo.Order_EachconsUkey)
	from WorkOrderForOutput wofo with (nolock)
	where wofo.CutRef = tmp.CutRef
	group by wofo.CutRef, wofo.OrderID, wofo.MarkerName, wofo.FabricCombo, wofo.MarkerNo
) step1
outer apply (
	select SizeRatio = Stuff((
		select '/' + Concat(sr.SizeCode, '*', sr.Qty)
		from WorkOrderForOutput_SizeRatio sr with (nolock)
		where sr.WorkOrderForOutputUkey = step1.WorkOrderForOutputUkey
		for xml path('')), 1, 1, '')
	, CuttingWidth = (Select ec.CuttingWidth from Order_EachCons ec with (nolock) where ec.Ukey = step1.Order_EachconsUkey)
	, PatternPanel = Stuff((
		select '+' + pp.PatternPanel
		from WorkOrderForOutput_PatternPanel pp with (nolock)
		where pp.WorkOrderForOutputUkey = step1.WorkOrderForOutputUkey
		for xml path('')), 1, 1, '')
) step2
order by EstCutDate, FtyGroup, CutCellID, CutRef

-- 表頭分組
insert into @Group (RowID, EstCutDate, FtyGroup, CutCellID)
select distinct RowID, EstCutDate, FtyGroup, CutCellID
from @TmpSelectDt

Select @RowID = Min(RowID), @RowCount = Max(RowID) From @Group;
While @RowID <= @RowCount
Begin
	Select @EstCutDate = EstCutDate
	, @CutCellID = CutCellID
	, @FtyGroup = FtyGroup
	From @Group
    Where RowID = @RowID

	-- 檢查排除已存在CutRef後是否還有資料
    IF exists (
        select 1 from @TmpSelectDt tmp where tmp.RowID = @RowID
        and not exists(select 1 from MarkerReq_Detail_CutRef mrd with (nolock) where mrd.CutRef = tmp.CutRef))	
    BEGIN
        exec dbo.usp_getID @keyword = @MDivisionID, @docno = 'MK', @issuedate = null, @tablename = 'MarkerReq' , @newid = @NewID OUTPUT

        insert into MarkerReq (ID, EstCutdate, MDivisionid, FactoryID, CutCellID, Status, Cutplanid, AddName, AddDate)
        values (@NewID, @EstCutDate, @MDivisionID, @FtyGroup, @CutCellID, 'New', '', @UserID, @Now)
        
        -- 3. 寫入MarkerReq_Detail (依MarkerName群組，Layer加總。不含CutRef)
        insert into MarkerReq_Detail
        (ID, OrderID, SizeRatio, MarkerName, Layer, FabricCombo, ReqQty, ReleaseQty
        , ReleaseDate, MarkerNo, WorkOrderForOutputUkey, CuttingWidth, PatternPanel)
        select 
          @NewID, 
          OrderID, 
          SizeRatio, 
          MarkerName, 
          sum(Layer) as Layer, 
          FabricCombo, 
          0, 0, 
          null, 
          MarkerNo, 
          max(WorkOrderForOutputUkey), 
          CuttingWidth, 
          PatternPanel
        from @TmpSelectDt tmp
        where RowID = @RowID
        group by OrderID, SizeRatio, MarkerName, FabricCombo, MarkerNo, CuttingWidth, PatternPanel
        
        insert into MarkerReq_Detail_CutRef (MarkerReqDetailUkey, ID, CutRef, MarkerName, Layer)
        select 
          d.Ukey,  -- 關聯Ukey
          d.ID,
          t.CutRef,
          d.MarkerName,
          t.Layer
        from MarkerReq_Detail d
        inner join (
            select
            tmp.MarkerName,
            tmp.CutRef,
            sum(tmp.Layer) as Layer
            from @TmpSelectDt tmp
            where RowID = @RowID
            group by tmp.MarkerName, tmp.CutRef
        ) t on d.ID = @NewID and d.MarkerName = t.MarkerName

        update @Group set ID = @NewID where RowID = @RowID
    END

	Set @RowID += 1;
End

select g.RowID, g.ID, mrd.CutRef
from @Group g
left join MarkerReq_Detail_CutRef mrd on g.ID = mrd.ID

Drop Table #tmp
";
                var plis = new List<SqlParameter>
                {
                    new SqlParameter("@MDivisionID", this.keyWord),
                    new SqlParameter("@UserID", this.loginID),
                };

                DataTable dtOutput;
                DualResult result = MyUtility.Tool.ProcessWithDatatable(selectDT, string.Empty, sql, out dtOutput, paramters: plis);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dtOutput != null && dtOutput.Rows.Count > 0)
                {
                    dtOutput.AsEnumerable()
                        .Where(r => !VFP.Empty(r["ID"])).Select(r => r["ID"].ToString())
                        .Distinct().ToList().ForEach(id => this.ImportedIDs.Add(id));
                }

                if (this.ImportedIDs.Count > 0)
                {
                    MyUtility.Msg.WarningBox("Successfully");
                }

                var selCutRefList = new List<string>(); // 勾選的CutRef
                selCutRefList = selectDT.AsEnumerable().Select(r => r["CutRef"].ToString()).ToList();

                var insertCutRefList = new List<string>(); // 成功建立的CutRef
                insertCutRefList = dtOutput.AsEnumerable().Where(r => !VFP.Empty(r["ID"])).Select(r => r["CutRef"].ToString()).ToList();

                // 顯示本次未建立的CutRef
                var failCutRefList = selCutRefList.Except(insertCutRefList).ToList();
                if (failCutRefList.Count() > 0)
                {
                    DataTable dtMsg = new DataTable();
                    dtMsg.Columns.Add("CutRef", typeof(string));
                    failCutRefList.ForEach(r => dtMsg.Rows.Add(r));
                    MsgGridForm msgGridForm = new MsgGridForm(dtMsg, "<CutRef> already existed in Marker Request.", "Warning", "CutRef");
                    msgGridForm.grid1.Columns[0].HeaderText = "CutRef#";
                    msgGridForm.grid1.Columns[0].Width = 120;
                    msgGridForm.ShowDialog();
                }

                if (this.ImportedIDs.Count == 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                this.ImportedIDs.Clear();
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
