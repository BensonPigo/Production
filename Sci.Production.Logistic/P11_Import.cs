using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// P11_Import
    /// </summary>
    public partial class P11_Import : Sci.Win.Subs.Base
    {
        private DataTable dtMasterDetail;
        private DataTable dtImport;
        private string id;

        /// <summary>
        /// P11_Import
        /// </summary>
        /// <param name="masterID">masterID</param>
        /// <param name="detail">detail</param>
        public P11_Import(string masterID, DataTable detail)
        {
            this.InitializeComponent();
            this.dtMasterDetail = detail;
            this.id = masterID;
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CTNStartNO", header: "CTN#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Article", header: "ColorWay", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Size", header: "Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .CellClogLocation("ClogLocationID", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true);

            #endregion 欄位設定
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            // 需至少輸入一個條件
            if (!this.dateRangeBuyer.HasValue &&
                MyUtility.Check.Empty(this.textSP_From.Text) && MyUtility.Check.Empty(this.textSP_To.Text) &&
                MyUtility.Check.Empty(this.textPackID.Text) &&
                MyUtility.Check.Empty(this.textCTN.Text) &&
                MyUtility.Check.Empty(this.txtcloglocation.Text) &&
                !this.checkCancelOrder.Checked)
            {
                MyUtility.Msg.WarningBox("search condition can not all is empty, you must enter at least one !!");
                return;
            }

            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            string sqlParDefine = string.Empty;
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.textPackID.Text))
            {
                sqlParDefine += "Declare @ID varchar(13) = @inputID" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputID", this.textPackID.Text));
                sqlWhere += " and pd.ID = @ID";
            }

            if (!MyUtility.Check.Empty(this.textSP_From.Text))
            {
                sqlParDefine += "Declare @SP_From varchar(13) = @inputSP_From" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputSP_From", this.textSP_From.Text));
                sqlWhere += " and pd.OrderID >= @SP_From";
            }

            if (!MyUtility.Check.Empty(this.textSP_To.Text))
            {
                sqlParDefine += "Declare @SP_To varchar(13) = @inputSP_To" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputSP_To", this.textSP_To.Text));
                sqlWhere += " and pd.OrderID <= @SP_To";
            }

            if (!MyUtility.Check.Empty(this.txtcloglocation.Text))
            {
                sqlParDefine += "Declare @ClogLocationID varchar(10) = @inputClogLocationID" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputClogLocationID", this.txtcloglocation.Text));
                sqlWhere += " and pd.ClogLocationID = @ClogLocationID";
            }

            if (!MyUtility.Check.Empty(this.textCTN.Text))
            {
                sqlParDefine += "Declare @CTNStartNo varchar(6) = @inputCTNStartNo" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputCTNStartNo", this.textCTN.Text));
                sqlWhere += " and pd.CTNStartNo = @CTNStartNo";
            }

            if (this.checkCancelOrder.Checked)
            {
                sqlWhere += " and o.Junk >= 1";
            }

            if (this.dateRangeBuyer.HasValue1)
            {
                sqlParDefine += "Declare @BuyerDeliveryFrom date = @inputBuyerDeliveryFrom" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputBuyerDeliveryFrom", this.dateRangeBuyer.DateBox1.Value));
                sqlWhere += " and o.BuyerDelivery >= @BuyerDeliveryFrom";
            }

            if (this.dateRangeBuyer.HasValue2)
            {
                sqlParDefine += "Declare @BuyerDeliveryTo date = @inputBuyerDeliveryTo" + Environment.NewLine;
                listSqlPar.Add(new SqlParameter("@inputBuyerDeliveryTo", this.dateRangeBuyer.DateBox2.Value));
                sqlWhere += " and o.BuyerDelivery <= @BuyerDeliveryTo";
            }

            string queryCMD = $@"
{sqlParDefine}

select
[selected] = 0,
[ID] = '',
[PackingListID] = pd.ID,
pd.CTNStartNO,
pd.OrderID,
o.CustPoNo,
o.StyleID,
pd.Article,
pd.Color,
[Size] =  (SELECT Stuff((select  concat( '/',SizeCode)
				from (select distinct pda.SizeCode,osca.Seq
						from PackingList_Detail pda with (nolock)
						inner join Orders o2 with (nolock) on pda.OrderID = o2.ID
						inner join Order_SizeCode osca with (nolock) on o2.POID = osca.ID and pda.SizeCode = osca.SizeCode
						where pda.ID = pd.ID and pda.CTNStartNO = pd.CTNStartNO ) a  order by Seq
			FOR XML PATH('')),1,1,'')) ,
[QtyPerCTN] = (SELECT Stuff((select  concat( '/',QtyPerCTN)
				from (select [QtyPerCTN] = sum(pda.QtyPerCTN),osca.Seq
						from PackingList_Detail pda with (nolock)
						inner join Orders o2 with (nolock) on pda.OrderID = o2.ID
						inner join Order_SizeCode osca with (nolock) on o2.POID = osca.ID and pda.SizeCode = osca.SizeCode
						where pda.ID = pd.ID and pda.CTNStartNO = pd.CTNStartNO group by  pda.SizeCode,osca.Seq) a  order by Seq
					FOR XML PATH('')),1,1,'')) ,
pd.ClogLocationID
from PackingList_Detail pd with (nolock)
inner join PackingList p with (nolock) on pd.ID = p.ID and p.MDivisionID  = '{Env.User.Keyword}' and len(p.PulloutID) = 0
inner join Orders o with (nolock) on pd.OrderID = o.ID
where pd.CTNQty = 1 {sqlWhere} and pd.QtyPerCTN > 0 and pd.DisposeFromClog = 0 and pd.ReceiveDate is not null and pd.TransferCFADate is null
and not exists 
(
	select 1 
	from ClogGarmentDispose a
	inner join ClogGarmentDispose_Detail b on a.ID = b.ID
	where b.PackingListId = pd.ID and b.CtnStartNO = pd.CTNStartNo
)

option (recompile)

";
            DualResult result = DBProxy.Current.Select(null, queryCMD, listSqlPar, out this.dtImport);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtImport.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No Data Found!");
            }

            this.gridImport.DataSource = this.dtImport;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var listCheckedData = this.dtImport.AsEnumerable().Where(s => (int)s["selected"] == 1);
            bool isCheckedData = listCheckedData.Any();

            if (!isCheckedData)
            {
                MyUtility.Msg.WarningBox("Please select import data");
                return;
            }

            string sql = @"
                select top 1 b.ID
                from ClogGarmentDispose a
                inner join ClogGarmentDispose_Detail b on a.ID = b.ID
                where exists (select PackingListID from #tmp t where t.PackingListID = b.PackingListId and t.CtnStartNO = b.CtnStartNO and t.selected = '1')
            ";
            DataTable dT;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtImport, string.Empty, sql, out dT);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dT.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox("This packing already exist " + dT.Rows[0]["ID"].ToString() + "(GD ID)");
                return;
            }

            var listOriData = this.dtMasterDetail.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            bool isExistsMasterDetail = false;
            foreach (DataRow drSelect in listCheckedData)
            {
                // 檢查import資料是否已存在
                isExistsMasterDetail = listOriData
                                        .Where(s => s["PackingListID"].Equals(drSelect["PackingListID"]) &&
                                                    s["CTNStartNO"].Equals(drSelect["CTNStartNO"])).Any();
                if (isExistsMasterDetail)
                {
                    continue;
                }

                drSelect["id"] = this.id;
                drSelect.AcceptChanges();
                drSelect.SetAdded();
                this.dtMasterDetail.ImportRow(drSelect);
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
