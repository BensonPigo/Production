using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P12 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P12"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .CheckBox("Junk", header: "Cancel Order", iseditable: false)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeableQty", header: "Chargeable Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCQty", header: "FOC Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("ChargeablePulloutQty", header: "Chargeable Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FOCPulloutQty", header: "FOC Pullout Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("SewingOutputQty", header: "Sewing Output Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("FinishedFOCStockinQty", header: "Finished FOC Stock-in Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;
        }

        private void Find()
        {
            #region 必輸入條件
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.txtSP1.Text) &&
                MyUtility.Check.Empty(this.txtSP2.Text))
            {
                MyUtility.Msg.WarningBox("Please input <SP#> or <Buyer Delivery> !");
                return;
            }
            #endregion
            this.listControlBindingSource1.DataSource = null;
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                where += $@" and o.id >= '{this.txtSP1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                where += $@" and o.id <= '{this.txtSP2.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtbrand1.Text))
            {
                where += $@" and o.brandid = '{this.txtbrand1.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                where += $@" and o.BuyerDelivery between '{((DateTime)this.dateBuyerDelivery.Value1).ToString("d")}' and '{((DateTime)this.dateBuyerDelivery.Value2).ToString("d")}' ";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                where += $@" and o.FtyGroup ='{this.txtfactory.Text}'";
            }

            where += this.chkIncludeCancelOrder.Checked ? string.Empty : " and o.Junk = 0 ";
            #endregion
            string sqlcmd = $@"
select
	selected = cast(0 as bit),
	o.FactoryID,
	o.BrandID,
	o.BuyerDelivery,
	OrderID=o.ID,
    o.Junk,
	o.CustPONo,
	o.StyleID,
	o.SeasonID,
	o.Qty,
	ChargeableQty = o.Qty - o.FOCQty,
	o.FOCQty,
	ChargeablePulloutQty = isnull(ChargeablePullout.Qty,0),
    --isnull(ShipQty_ByType.TotalNotFocShipQty,0),
	FOCPulloutQty = isnull(ShipQty_ByType.TotalFocShipQty,0),
    SewingOutputQty = SewingOutPut.Qty ,
	FinishedFOCStockinQty = ISNULL(FocStockQty.Value ,0)    -- Function 取得 FOC 庫存
	,o.OrderTypeID
    ,[FinFOCQty] = isnull(ox.FOCQty, 0)
from orders o with(nolock)
inner join Factory f with(nolock) on f.id = o.FactoryID and f.IsProduceFty = 1
outer apply(
	select FOCQty=sum(ox.FOCQty) from Order_Finish ox where ox.id = o.ID
)ox
outer apply(
	select 	oq.ID,
	[Qty] = sum(isnull(C_Pullout.ShipQty,0) + isnull(TPE_Adjust.DiffQty,0))
	from Order_Qty oq
	left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = oq.Id and ou1.Article = oq.Article and ou1.SizeCode = oq.SizeCode 
	left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = oq.Id and ou2.Article = '----' and ou2.SizeCode = '----' 
	outer apply(
		SELECT [ShipQty]=SUM(pd.ShipQty)
		FROM PackingList p 
		INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
		INNER JOIN Pullout pu ON p.PulloutID=pu.ID
		WHERE pu.Status <> 'New' AND pd.OrderID = oq.ID
		and pd.Article = oq.Article and pd.SizeCode = oq.SizeCode
	)C_Pullout --  總出貨數
	outer apply(
		SELECt [DiffQty]= SUM(iq.DiffQty)
		FROm InvAdjust i
		INNER JOIN InvAdjust_Qty iq ON i.ID = iq.ID
		WHERE i.OrderID = oq.ID
		and iq.Article = oq.Article and iq.SizeCode = oq.SizeCode
	)TPE_Adjust -- 台北財務調整的數量
	where oq.ID=o.ID
	and ISNULL(ou1.POPrice, ISNULL(ou2.PoPrice, -1)) !=0
	group by oq.ID
)ChargeablePullout
outer apply(
	select sum(TotalNotFocShipQty) as TotalNotFocShipQty , sum(TotalFocShipQty) as TotalFocShipQty 
	from
	(	
		select 
		[TotalNotFocShipQty] = iif(pl.Type <> 'F',sum(pod.ShipQty),0),
		[TotalFocShipQty]=iif( pl.Type='F',sum(pod.ShipQty),0)
		from Pullout_Detail pod with(nolock)
		inner join PackingList pl with(nolock) on pl.ID = pod.PackingListID
		where pod.OrderID = o.ID
		group by pl.Type
	) a
)ShipQty_ByType
outer apply(
	select Qty = isnull(sum([dbo].getMinCompleteSewQty(oq.ID,oq.Article,oq.SizeCode)),0)
	from Order_Qty oq 
	where oq.ID=o.ID
)SewingOutPut
OUTER APPLY(
	--判斷FOC 是否建立 PackingList
	SELECT [Result]=IIF(COUNT(p.ID) > 0 ,'true' , 'false') 
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.ID=pd.ID 
	WHERE p.Type = 'F' AND pd.OrderID=o.ID
)PackingList_Chk_HasFoc

OUTER APPLY(
	--
	--如果 FOC 沒建立 PackingList，不需判斷出貨
	--如果 FOC 已建立了 PackingList，判斷是否『所有』的 FOC PackingList 都完成出貨（出貨的Pullout Stauts != New）。
	SELECT  [Result]=IIF( PackingList_Chk_HasFoc.Result='false','true'
			,IIF( COUNT(p.ID) > 0,'true','false' )) -- 有New的話表示不是全部都完成出貨
	FROM PackingList p
	INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
	WHERE   p.Type = 'F' AND pd.OrderID=o.ID --AND pu.Status = 'New'
			AND EXISTS 
			(	--判斷有無建立 Pullout
				SELECT 1 FROM Pullout pu WHERE  p.PulloutID=pu.ID
			)
			AND NOT EXISTS 
			(	--若有建立 Pullout，判斷是不是全部出貨
				SELECT 1 FROM Pullout pu WHERE  p.PulloutID=pu.ID AND pu.Status = 'New'
			)

)PackingList_Chk_IsAllPullout

OUTER APPLY(
	SELECT Value=dbo.GetFocStockByOrder(o.ID)
)FocStockQty

where   o.MDivisionID = '{Env.User.Keyword}'
        AND o.FOCQty > 0  --訂單有 FOC 數量
        AND FocStockQty.Value > 0  -- FOC 還有未出貨的數量
	    AND ISNULL(ChargeablePullout.Qty,0) = (o.Qty - o.FOCQty)  --Chargeable 必須『全數』出貨
        AND isnull(ox.FOCQty,0) < isnull(FocStockQty.Value,0)  --[系統紀錄 P12 完成入庫的庫存]小於[當下應有的庫存]
        AND (	
		        PackingList_Chk_HasFoc.Result='false' 
		        OR 
		        (PackingList_Chk_HasFoc.Result='true' AND PackingList_Chk_IsAllPullout.Result = 'true')
	    ) -- 排除 FOC 已建立 FOC PL 但是還沒出貨
        {where}
order by o.ID
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var query = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(dr => MyUtility.Convert.GetBool(dr["selected"]));
            if (query.ToList().Count == 0)
            {
                return;
            }

            DataTable dt = query.CopyToDataTable();
            DataTable odt;
            DualResult result;

            #region SewingOutput總產出不可少於訂單總數量
            string msgError = string.Empty;
            DataRow[] drlist = dt.Select("SewingOutputQty < Qty");
            if (drlist.Length > 0)
            {
                msgError = "Sewing output q'ty less than order q'ty, those SP# cannot do FOC stock-in. ";
            }

            foreach (DataRow dr in drlist)
            {
                msgError += Environment.NewLine + dr["OrderID"];
            }

            if (!MyUtility.Check.Empty(msgError))
            {
                MyUtility.Msg.WarningBox(msgError);
                return;
            }
            #endregion

            string sqlchk = $@"
select t.OrderID
from #tmp t
inner join Order_Finish ox with(nolock) on ox.id = t.OrderID
";
            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlchk, out odt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                string insertOrderFinished = $@"
update o
	set o.FOCQty = t.FinishedFOCStockinQty, o.AddDate = getdate(), o.AddName = '{Env.User.UserID}'
from #tmp t
inner join Order_Finish o on t.OrderID = o.ID
where t.FinishedFOCStockinQty > t.FinFOCQty

insert Order_Finish(ID,FOCQty,AddName,AddDate)
select t.OrderID, t.FinishedFOCStockinQty, [AddName] = '{Env.User.UserID}', [AddDate] = getdate()
from #tmp t
where not exists(select 1 from Order_Finish where ID = t.OrderID)
";
                result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, insertOrderFinished, out odt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            this.Find();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }
    }
}
