using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P20 : Sci.Win.Tems.Input1
    {
        private string reasonTypeID;
        private DataTable shipdata1;
        private DataTable shipdata2;
        private DataTable shipApplydata1;
        private DataTable shipApplydata2;

        /// <inheritdoc/>
        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridQtyOrder.AutoGenerateColumns = true;
            this.gridQtyBreakDownbyArticleSizeDetail.AutoGenerateColumns = true;
            this.GridQtyOrder_Apply.AutoGenerateColumns = true;
            this.gridQtyBrkApplybyArticleSizeDetail.AutoGenerateColumns = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Not yet confirm,1,All");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = $"(select f.MDivisionID from Factory f WITH (NOLOCK) where f.ID = OrderChangeApplication.FactoryID) = '{Sci.Env.User.Keyword}' and isnull(OrderChangeApplication.ConfirmedName,'') = '' and OrderChangeApplication.ConfirmedDate is null";
            this.ReloadDatas();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                    default:
                        this.DefaultWhere = $"(select f.MDivisionID from Factory f WITH (NOLOCK) where f.ID = OrderChangeApplication.FactoryID) = '{Sci.Env.User.Keyword}' and isnull(OrderChangeApplication.ConfirmedName,'') = '' and OrderChangeApplication.ConfirmedDate is null";
                        break;
                    case "1":
                        this.DefaultWhere = $"(select f.MDivisionID from Factory f WITH (NOLOCK) where f.ID = OrderChangeApplication.FactoryID) = '{Sci.Env.User.Keyword}'";
                        break;
                }
                this.ReloadDatas();
            };

            this.GridQtyOrder.IsEditingReadOnly = true;
            this.gridQtyBreakDownByShipmode.IsEditingReadOnly = true;
            this.gridQtyBreakDownbyArticleSizeDetail.IsEditingReadOnly = true;
            this.GridQtyOrder_Apply.IsEditingReadOnly = true;

            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDownByShipmode)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
            .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
            .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10))
            .Date("FtyKPI", header: "Fty KPI Date", width: Widths.AnsiChars(10))
            .Numeric("NowQty", header: "Total Q'ty", width: Widths.AnsiChars(6))
            .Text("ReasonID", header: "Reason", width: Widths.AnsiChars(10))
            .Text("Name", header: "Reason Desc.", width: Widths.AnsiChars(10))
            .Text("ReasonRemark", header: "Reason Remark", width: Widths.AnsiChars(10))
            .Text("AddName", header: "Create by", width: Widths.AnsiChars(10))
            .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10))
            ;
            this.Helper.Controls.Grid.Generator(this.gridQtyBrkApplyByShipmode)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
            .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
            .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10))
            .Date("FtyKPI", header: "Fty KPI Date", width: Widths.AnsiChars(10))
            .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(6))
            .Text("ReasonID", header: "Reason", width: Widths.AnsiChars(10))
            .Text("Name", header: "Reason Desc.", width: Widths.AnsiChars(10))
            .Text("ReasonRemark", header: "Reason Remark", width: Widths.AnsiChars(10))
            .Text("AddName", header: "Create by", width: Widths.AnsiChars(10))
            .Text("EditName", header: "Edit by", width: Widths.AnsiChars(10))
            ;

            #region Load Reason
            DataTable dt;
            string sqlcmd = $@"SELECT ID, ID + '-' + Name as Name FROM Reason WHERE ReasonTypeID = 'OMQtychange'";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                this.CboReason.DataSource = dt;
                this.CboReason.ValueMember = "ID";
                this.CboReason.DisplayMember = "Name";
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.ClearAll();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ConfirmedDate"]) && !this.EditMode)
            {
                this.BtnConfirm.Enabled = true;
                this.BtnReject.Enabled = true;
            }
            else
            {
                this.BtnConfirm.Enabled = false;
                this.BtnReject.Enabled = false;
            }

            switch (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]))
            {
                case "Approved":
                    this.labelStatus.Text = "SMR Approved";
                    break;
                case "Confirmed":
                    this.labelStatus.Text = "Fty Confirmed";
                    break;
                case "Reject":
                    this.labelStatus.Text = "Fty Reject";
                    break;
                case "Junked":
                    this.labelStatus.Text = "MR Junked";
                    break;
                default:
                    this.labelStatus.Text = string.Empty;
                    break;
            }

            #region 載入簽核流程資訊
            DataTable dtSign;
            string sqlHdata = $@"
select x.Status,x.StatusDate,h.StatusUser
from(
	select h.Status,StatusDate=max(h.StatusDate)
	from OrderChangeApplication_History h
	where id = '{this.CurrentMaintain["ID"]}'
    group by h.Status
)x
inner join OrderChangeApplication_History h on h.Status = x.Status and h.StatusDate = x.StatusDate and h.id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Select(null, sqlHdata, out dtSign);
            if (!result)
            {
                this.ShowErr(result);
                this.BtnConfirm.Enabled = false;
                this.BtnReject.Enabled = false;
                return;
            }
            #endregion

            #region 載入Orders欄位
            string orderid = this.CurrentDataRow["Orderid"].ToString();
            DataRow currOrder;
            if (!MyUtility.Check.Seek($@"select * from Orders where ID = '{orderid}'", out currOrder))
            {
                MyUtility.Msg.WarningBox("OrderID not found!");
                this.BtnConfirm.Enabled = false;
                this.BtnReject.Enabled = false;
                return;
            }

            DataRow row = currOrder;
            string category = row["Category"].ToString();
            string poid = row["POID"].ToString();
            this.dispStyle.Text = row["Styleid"].ToString();
            this.dispSeason.Text = row["SeasonID"].ToString();
            this.cmbCategory.SelectedValue = row["Category"].ToString();
            this.dispBrand.Text = row["BrandID"].ToString();
            this.dispProgram.Text = row["ProgramID"].ToString();
            this.dispOrderType.Text = row["OrderTypeID"].ToString();
            this.dispProject.Text = row["ProjectID"].ToString();
            this.dateCFM.Value = Convert.ToDateTime(row["CFMDate"]);
            this.dateDelivery.Value = Convert.ToDateTime(row["BuyerDelivery"]);
            this.dateSCIDelivery.Value = Convert.ToDateTime(row["SCIDelivery"]);
            this.dispDescription.Text = row["Dest"].ToString();
            this.DisFOCQty.Text = row["FOCQty"].ToString();
            this.DisOrderQty.Text = row["QTY"].ToString();
            this.displayCPU.Text = row["CPU"].ToString();
            this.txttpeuserMRHandle.DisplayBox1.Text = row["MRHandle"].ToString();
            this.txttpeuserSMR.DisplayBox1.Text = row["SMR"].ToString();

            string firstcutdate = $@"select FirstCutDate from [Production].dbo.Cutting where ID = '{row["CuttingSP"]}'";
            DataRow cutrow;
            if (MyUtility.Check.Seek(firstcutdate, out cutrow))
            {
                this.dateActCutFstDate.Value = MyUtility.Convert.GetDate(cutrow["FirstCutDate"]);
            }

            if (category == "S")
            {
                this.reasonTypeID = "Order_BuyerDelivery_Sample";
            }
            else
            {
                this.reasonTypeID = "Order_BuyerDelivery";
            }

            // 撈Sewing Data
            string locationTable = string.Empty;
            if (MyUtility.Check.Seek($"select 1 from Order_Location where OrderId = '{orderid}'"))
            {
                locationTable = "Order_Location  sl WITH (NOLOCK) on sl.OrderId = o.id";
            }
            else
            {
                locationTable = "Style_Location  sl WITH (NOLOCK) on o.StyleUkey = sl.StyleUkey";
            }

            string sqlCmd = string.Format(
                @"
with SewQty as (
	select	oq.Article
			, oq.SizeCode
			, oq.Qty
			, ComboType = sl.Location
			, QAQty = isnull(sum(sdd.QAQty),0)
	from Orders o WITH (NOLOCK) 
	inner join {2}
	inner join Order_Qty oq WITH (NOLOCK) on oq.ID = o.ID
	left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = o.ID 
															  and sdd.Article = oq.Article 
															  and sdd.SizeCode = oq.SizeCode 
															  and sdd.ComboType = sl.Location
	where o.ID = '{0}'
	group by oq.Article,oq.SizeCode,oq.Qty,sl.Location
), 
minSewQty as (
	select	Article
			, SizeCode
			, QAQty = MIN(QAQty)
	from SewQty
	group by Article,SizeCode
),
PivotData as (
	select *
	from SewQty
	PIVOT (SUM(QAQty)
	FOR ComboType IN ([T],[B],[I],[O])) a
)
select SewQty = sum(m.QAQty )
from PivotData p
left join minSewQty m on m.Article = p.Article and m.SizeCode = p.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.ID = '{1}' and oa.Article = p.Article
left join Order_SizeCode os WITH (NOLOCK) on os.ID = '{1}' and os.SizeCode = p.SizeCode
",
                orderid,
                poid,
                locationTable);
            string sSewoutPutQty = MyUtility.GetValue.Lookup(sqlCmd);
            this.DisSewingQty.Text = sSewoutPutQty;
            #endregion

            // 載入PO
            DataRow currPO;
            if (MyUtility.Check.Seek($"select * from PO where ID = '{orderid}'", out currPO))
            {
                this.txttpeuserPoHandle.DisplayBox1.Text = currPO["POHandle"].ToString();
                this.txttpeuserPoSmr.DisplayBox1.Text = currPO["POSMR"].ToString();
            }

            if (MyUtility.Check.Seek($"select [OrderNewQty] =cast(sum(iif(oc.ReasonID IN ('OCR05', 'OCR06'), 0, Qty)) as int) from OrderChangeApplication_Detail ocd inner join OrderChangeApplication oc on oc.id = ocd.id where ocd.id = '{ this.CurrentDataRow["ID"].ToString()}'", out currPO))
            {
                this.DisOrderNewQty.Text = currPO["OrderNewQty"].ToString ();
            }

            // 設定為黃底
            this.DisOrderNewQty.BackColor = Color.FromArgb(254, 254, 120);

            this.QtybrkOrderbs.DataSource = null;
            this.QtybrkShipbs1.DataSource = null;
            this.QtybrkShipbs2.DataSource = null;
            this.QtybrkApplybs.DataSource = null;

            #region tab 1
            sqlCmd = $@"
select stuff((
	select concat(',[',SizeCode,']')
	from Order_SizeCode os with(nolock)
	where os.id = '{poid}'
	order by seq
	for xml path('')
),1,1,'')
";
            string sizeCode = MyUtility.GetValue.Lookup(sqlCmd);

            sqlCmd = string.Format(
                @"
with tmpData as (
      select oad.Article
             , oad.SizeCode
             , oad.NowQty
             , RowNo=1
      from OrderChangeApplication_Detail oad WITH (NOLOCK) 
      where oad.ID = '{2}'
),
SubTotal as (
      select 'TTL' as Article
             , SizeCode
             , SUM(NowQty) as NowQty
             , '9999' as RowNo
      from tmpData
      group by SizeCode
),
UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),
pivotData as (
      select *
      from UnionData
      pivot( 
            sum(NowQty) for SizeCode in ({1})
      ) a
)
select Total=(select sum(NowQty) from UnionData where Article = p.Article)
	,Article,{1}
from pivotData p
order by RowNo,Article",
                this.CurrentMaintain["Orderid"],
                sizeCode,
                this.CurrentMaintain["ID"]);
            DataTable qtybrk;
            result = DBProxy.Current.Select(null, sqlCmd, out qtybrk);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.QtybrkOrderbs.DataSource = qtybrk;
            #endregion
            #region tab 2
            sqlCmd = $@"
select os.*,NowQty=isnull(x.NowQty,0),r.Name,o.AddName,o.EditName
from OrderChangeApplication_Seq os
inner join OrderChangeApplication o on o.id = os.ID
left join Reason r WITH (NOLOCK) on r.ReasonTypeID = '{this.reasonTypeID}'  and r.ID = os.ReasonID
outer apply(select NowQty=sum(NowQty) from OrderChangeApplication_Detail oq where  oq.Id  = os.id and oq.Seq = os.Seq)x
where os.id = '{this.CurrentMaintain["ID"]}'
and os.Seq in (select Seq from OrderChangeApplication_Detail where id = o.ID and NowQty != 0)
order by os.Seq,Ukey";

            result = DBProxy.Current.Select(null, sqlCmd, out this.shipdata1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlCmd = string.Format(
@"with tmpData as (
	select oqd.Seq,oqd.Article,oqd.SizeCode,oqd.NowQty,oa.Seq as ASeq
	from OrderChangeApplication_Detail oqd WITH (NOLOCK) 
	left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
    where oqd.ID = '{0}'
),SubTotal as (
    select Seq,'TTL' as Article,SizeCode,SUM(NowQty) as NowQty, '9999' as ASeq
    from tmpData
    group by Seq,SizeCode
),UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),pivotData as (
    select *
    from UnionData
    pivot( sum(NowQty)    for SizeCode in ({1})) a
)
select 
	Total=(select sum(NowQty) from UnionData where Seq = p.Seq and Article = p.Article)
	,Article,{1}
    ,Seq
from pivotData p
order by ASeq",
                this.CurrentMaintain["ID"],
                sizeCode);
            result = DBProxy.Current.Select(null, sqlCmd, out this.shipdata2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.shipdata2 != null)
            {
                this.gridQtyBreakDownByShipmode.SelectionChanged += (s, e) =>
                {
                    this.gridQtyBreakDownByShipmode.ValidateControl();
                    DataRow dr = this.gridQtyBreakDownByShipmode.GetDataRow<DataRow>(this.gridQtyBreakDownByShipmode.GetSelectedRowIndex());
                    if (dr != null)
                    {
                        string filter = string.Format("Seq = '{0}'", MyUtility.Convert.GetString(dr["Seq"]));
                        this.shipdata2.DefaultView.RowFilter = filter;
                    }
                };
            }

            this.QtybrkShipbs1.DataSource = this.shipdata1;
            this.QtybrkShipbs2.DataSource = this.shipdata2;
            this.gridQtyBreakDownbyArticleSizeDetail.Columns["Seq"].Visible = false;
            #endregion
            #region tab 3
            sqlCmd = string.Format(
                @"
with SortBy as (
      select oq.Article  
             , RowNo = ROW_NUMBER() over (order by oq.Article) 
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '{0}'
      group by oq.Article
),tmpData as (
    select x.*,sb.RowNo
    from(
        select oq.Article
            , oq.SizeCode
            , Qty=sum(oq.Qty)
        from OrderChangeApplication_Detail oq WITH (NOLOCK) 
        where oq.ID = '{2}'
        group by oq.Article, oq.SizeCode
    )x
	inner join SortBy sb on x.Article = sb.Article
),SubTotal as (
      select 'TTL' as Article
             , SizeCode
             , SUM(Qty) as Qty
             , '9999' as RowNo
      from tmpData
      group by SizeCode
),UnionData as (
      select * from tmpData
      union all
      select * from SubTotal
),pivotData as (
      select *
      from UnionData
      pivot(sum(Qty) for SizeCode in ({1})) a
)
select Total=(select sum(Qty) from UnionData where Article = p.Article)
	,Article,{1}
from pivotData p
order by RowNo",
                this.CurrentMaintain["Orderid"],
                sizeCode,
                this.CurrentMaintain["ID"]);
            DataTable qtybrkApply;
            result = DBProxy.Current.Select(null, sqlCmd, out qtybrkApply);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.GridQtyOrder_Apply.DataSource = qtybrkApply;
            #endregion
            #region tab 4
            sqlCmd = $@"
select os.*,Qty=isnull(x.Qty,0),r.Name,o.AddName,o.EditName
from OrderChangeApplication_Seq os
inner join OrderChangeApplication o on o.id = os.ID
left join Reason r WITH (NOLOCK) on r.ReasonTypeID = '{this.reasonTypeID}'  and r.ID = os.ReasonID
outer apply(select qty=sum(qty) from OrderChangeApplication_Detail oq where  oq.Id  = os.id and oq.Seq = os.Seq)x
where os.id = '{this.CurrentMaintain["ID"]}'
order by os.Seq,Ukey";

            result = DBProxy.Current.Select(null, sqlCmd, out this.shipApplydata1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlCmd = string.Format(
@"with tmpData as (
	select oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq as ASeq
	from OrderChangeApplication_Detail oqd WITH (NOLOCK) 
	left join Order_Article oa WITH (NOLOCK) on oa.ID = oqd.ID and oa.Article = oqd.Article
    where oqd.ID = '{0}'
),SubTotal as (
    select Seq,'TTL' as Article,SizeCode,SUM(Qty) as Qty, '9999' as ASeq
    from tmpData
    group by Seq,SizeCode
),UnionData as (
    select * from tmpData
    union all
    select * from SubTotal
),pivotData as (
    select *
    from UnionData
    pivot( sum(Qty)    for SizeCode in ({1})) a
)
select 
	Total=(select sum(Qty) from UnionData where Seq = p.Seq and Article = p.Article)
	,Article,{1}
    ,Seq
from pivotData p
order by ASeq",
                this.CurrentMaintain["ID"],
                sizeCode);
            result = DBProxy.Current.Select(null, sqlCmd, out this.shipApplydata2);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.shipApplydata2 != null)
            {
                this.gridQtyBrkApplyByShipmode.SelectionChanged += (s, e) =>
                {
                    this.gridQtyBrkApplyByShipmode.ValidateControl();
                    DataRow dr = this.gridQtyBrkApplyByShipmode.GetDataRow<DataRow>(this.gridQtyBrkApplyByShipmode.GetSelectedRowIndex());
                    if (dr != null)
                    {
                        string filter = string.Format("Seq = '{0}'", MyUtility.Convert.GetString(dr["Seq"]));
                        this.shipApplydata2.DefaultView.RowFilter = filter;
                    }
                };
            }

            this.QtybrkShipApplybs1.DataSource = this.shipApplydata1;
            this.QtybrkShipApplybs2.DataSource = this.shipApplydata2;
            this.gridQtyBrkApplybyArticleSizeDetail.Columns["Seq"].Visible = false;
            #endregion

            this.txtuserCFM.TextBox1.ReadOnly = true;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            DualResult result;

            /*
             * 取消API
            result = Prgs.PostOrderChange(this.CurrentMaintain["ID"].ToString(), "Confirmed", this.CurrentMaintain["FTYComments"].ToString());
            if (!result)
            {
                this.ShowErr(result);
                return;
            }.
            */

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ConfirmedDate"]))
            {
                MyUtility.Msg.ErrorBox("ConfirmedDate must be empty!");
                return;
            }

            string updateCmd = string.Format(
                @"
update OrderChangeApplication set ConfirmedName = '{0}',ConfirmedDate = GETDATE(), EditName = '{0}', EditDate = GETDATE(),FTYComments = '{2}' where ID = '{1}'
INSERT INTO [dbo].[OrderChangeApplication_History]([ID],[Status],[StatusUser],[StatusDate])
VALUES('{1}','Confirmed','{0}',getdate())
", Sci.Env.User.UserID,
MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
MyUtility.Convert.GetString(this.CurrentMaintain["FTYComments"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }

            /*
             * 取消 Send Mail - 因資料交換不再是即時
            this.SendMail("Confirmed");
            */

            this.RenewData();
            this.OnDetailEntered();
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = Prgs.PostOrderChange(this.CurrentMaintain["ID"].ToString(), "Reject", this.CurrentMaintain["FTYComments"].ToString());
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string updateCmd = string.Format(
                @"
update OrderChangeApplication set Status = 'Reject',RejectName = '{0}',RejectDate = GETDATE(), EditName = '{0}', EditDate = GETDATE(),FTYComments = '{2}' where ID = '{1}'
INSERT INTO [dbo].[OrderChangeApplication_History]([ID],[Status],[StatusUser],[StatusDate])
VALUES('{1}','Reject','{0}',getdate())
", Sci.Env.User.UserID,
MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
MyUtility.Convert.GetString(this.CurrentMaintain["FTYComments"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }

            this.SendMail("Reject");

            this.RenewData();
            this.OnDetailEntered();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.RenewData();
            this.OnDetailEntered();
        }

        private void SendMail(string status)
        {
            string sqlcmd = string.Empty;
            List<string> emailList = new List<string>();
            sqlcmd = $@"select p.EMail from orders o left join TPEPass1 p on p.id = o.MRHandle where o.id = '{this.CurrentMaintain["Orderid"]}'";
            emailList.Add(MyUtility.GetValue.Lookup(sqlcmd));
            sqlcmd = $@"select p.EMail from orders o left join TPEPass1 p on p.id = o.SMR where o.id = '{this.CurrentMaintain["Orderid"]}'";
            emailList.Add(MyUtility.GetValue.Lookup(sqlcmd));
            var x = emailList.Where(w => !MyUtility.Check.Empty(w)).ToList();
            string toAddress = string.Join(";", x);
            string ccAddress = string.Empty;
            string factory = MyUtility.GetValue.Lookup($@"select factoryID from orders with(nolock) where id = '{this.CurrentMaintain["Orderid"]}'");
            string subject = $@"{this.CurrentMaintain["ID"]} {this.CurrentMaintain["Orderid"]} {factory} {status}";
            string description = $@"{status} SP#{this.CurrentMaintain["Orderid"]}-{factory} request change qty, please check, apply id# - {this.CurrentMaintain["ID"]}";

            var email = new MailTo(Sci.Env.Cfg.MailFrom, toAddress, ccAddress, subject, null, description, true, true);
            email.ShowDialog(this);
        }

        private void ClearAll()
        {
            this.ClearHeader();
            this.ClearBody();
        }

        private void ClearHeader()
        {
            // Order
            this.dispStyle.Text = string.Empty;
            this.dispSeason.Text = string.Empty;
            this.cmbCategory.SelectedValue = string.Empty;
            this.dispBrand.Text = string.Empty;
            this.dispProgram.Text = string.Empty;
            this.dispOrderType.Text = string.Empty;
            this.dispProject.Text = string.Empty;
            this.dateCFM.Text = string.Empty;
            this.dateDelivery.Text = string.Empty;
            this.dateSCIDelivery.Text = string.Empty;
            this.dispDescription.Text = string.Empty;
            this.DisFOCQty.Text = string.Empty;
            this.DisOrderQty.Text = string.Empty;
            this.DisOrderNewQty.Text = string.Empty;
        }

        private void ClearBody()
        {
            this.QtybrkOrderbs.DataSource = null;
            this.QtybrkShipbs1.DataSource = null;
            this.QtybrkShipbs2.DataSource = null;
            this.QtybrkApplybs.DataSource = null;
            this.QtybrkShipApplybs1.DataSource = null;
            this.QtybrkShipApplybs2.DataSource = null;
        }

        private void BtnProductionOutput_Click(object sender, EventArgs e)
        {
            string orderid = this.CurrentDataRow["Orderid"].ToString();
            DataRow currOrder;
            if (!MyUtility.Check.Seek($@"select * from Orders where ID = '{orderid}'", out currOrder))
            {
                MyUtility.Msg.WarningBox("OrderID not found!");
                return;
            }
            else
            {
                Sci.Production.PPIC.P01_ProductionOutput callNextForm = new Sci.Production.PPIC.P01_ProductionOutput(currOrder);
                callNextForm.tabControl1.TabPages.Remove(callNextForm.tabControl1.TabPages[2]);
                callNextForm.tabControl1.TabPages.Remove(callNextForm.tabControl1.TabPages[1]);
                callNextForm.ShowDialog(this);
            }
        }
    }
}
