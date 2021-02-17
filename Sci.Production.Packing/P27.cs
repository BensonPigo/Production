using Ict;
using Ict.Win;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P27 : Sci.Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P27(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["PackingListID"]);
            string packingListID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["PackingListID"]);
            this.DetailSelectCommand = $@"
WITH PackingListDetail as(
	SELECT DISTINCT pd.ID
		,pd.OrderShipmodeSeq
		,pd.OrderID
		,o.CustPONO
		,pd.CTNStartNo
		,pd.CustCtn
		,pd.RefNo
		,pd.SCICtnNo
	FROM PackingList_Detail pd
	INNER JOIN ShippingMarkStamp a  ON pd.ID = a.PackingListID
	INNER JOIN ShippingMarkStamp_Detail b ON a.PackingListID = b.PackingListID  AND b.SCICtnNo = pd.SCICtnNo
	INNER JOIN Orders o ON o.ID = pd.OrderID
	WHERE pd.ID='{packingListID}'
)
SELECT pd.OrderID
    ,o.CustPONO
    ,pd.CTNStartNo
    ,pd.CustCtn
    ,pd.RefNo
    ,[ColorWay]=ColorWay.Val
    ,[Color]=Color.Val
    ,[SizeCode]=SizeCode.Val
    ,[ShippingMarkCombination]=comb.ID
    ,[ShippingMarkType]=st.ID
    , b.PackingListID
    , b.SCICtnNo
    , b.FileName
    , [ShippingMark]=IIF(b.Image IS NULL , 0 , 1 )
    , b.ShippingMarkCombinationUkey
    , b.ShippingMarkTypeUkey
    , b.Side
    , b.Seq
    , b.FromRight
    , b.FromBottom
    , b.Width
    , b.Length
    , [HTMLFile]=IIF(b.FilePath <> '' AND b.FileName <> '' ,1 ,0 )
FROm ShippingMarkStamp a
INNER JOIN ShippingMarkStamp_Detail b ON a.PackingListID = b.PackingListID
INNER JOIN PackingListDetail pd ON pd.ID = a.PackingListID AND b.SCICtnNo = pd.SCICtnNo
INNER JOIN Orders o ON o.ID = pd.OrderID
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = b.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = b.ShippingMarkTypeUkey
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Article
		FROM PackingList_Detail pd2
		WHERE pd2.ID = pd.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)ColorWay
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.Color
		FROM PackingList_Detail pd2
		WHERE pd2.ID = pd.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)Color
OUTER APPLY(
	SELECT [Val]=STUFF((
		SELECT DISTINCT ',' + pd2.SizeCode
		FROM PackingList_Detail pd2
		WHERE pd2.ID = pd.ID AND pd2.OrderID = pd.OrderID AND pd2.OrderShipmodeSeq = pd.OrderShipmodeSeq AND pd2.CTNStartNo = pd.CTNStartNo
		FOR XML PATH('')
	),1,1,'')
)SizeCode
WHERE a.PackingListID = '{masterID}'
ORDER BY pd.Seq ASC,pd.CTNQty DESC
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.detailgrid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CustPONO", header: "P.O. No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SCICtnNo", header: "SCI Ctn No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CustCTN", header: "Cust #", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ColorWay", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ShippingMarkCombination", header: "Stamp Combination", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ShippingMarkType", header: "Stamp Type", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Side", header: "Side", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("FromRight", header: "From Right (mm)", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("FromBottom", header: "From Bottom (mm)", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Width", header: "Width", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Length", header: "Length", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .CheckBox("HTMLFile", header: "HTMLFile", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            ;
            #region 關閉排序功能
            for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            {
                this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
            return base.OnGridSetup();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 檢查是否擁有Packing P27的Canedit 權限
            bool canCanEdit = Prgs.GetAuthority(Env.User.UserID, "P27. Shipping Mark Stamp (for GenSong)", "CanEdit");

            this.btnGenerate.Enabled = !this.EditMode && canCanEdit;
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {

            #region 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["PackingListID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
            #region 資料交換 - Sunrise
            if (Sunrise_FinishingProcesses.IsSunrise_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["PackingListID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion

            return base.ClickDelete();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            P27_Generate form = new P27_Generate();
            form.ShowDialog();
            this.Reload();
        }

        /// <inheritdoc/>
        public void Reload()
        {
            if (this.CurrentDataRow != null)
            {
                string idIndex = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentDataRow))
                {
                    if (!MyUtility.Check.Empty(this.CurrentDataRow["PackingListID"]))
                    {
                        idIndex = MyUtility.Convert.GetString(this.CurrentDataRow["PackingListID"]);
                    }
                }

                this.ReloadDatas();
                this.RenewData();
                if (!MyUtility.Check.Empty(idIndex))
                {
                    this.gridbs.Position = this.gridbs.Find("PackingListID", idIndex);
                }
            }
        }
    }
}
