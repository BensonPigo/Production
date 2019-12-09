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
    public partial class P20 : Sci.Win.Tems.Input1
    {
        private string reasonTypeID;
        private DataTable shipdata1;
        private DataTable shipdata2;
        private DataTable shipApplydata1;
        private DataTable shipApplydata2;

        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridQtyOrder.AutoGenerateColumns = true;
            this.gridQtyBreakDownbyArticleSizeDetail.AutoGenerateColumns = true;
            this.GridQtyOrder_Apply.AutoGenerateColumns = true;
            this.gridQtyBrkApplybyArticleSizeDetail.AutoGenerateColumns = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.GridQtyOrder.IsEditingReadOnly = true;
            this.gridQtyBreakDownByShipmode.IsEditingReadOnly = true;
            this.gridQtyBreakDownbyArticleSizeDetail.IsEditingReadOnly = true;
            this.GridQtyOrder_Apply.IsEditingReadOnly = true;

            this.Helper.Controls.Grid.Generator(this.gridQtyBreakDownByShipmode)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(2))
            .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
            .Date("BuyerDelivery", header: "Delivery", width: Widths.AnsiChars(10))
            .Date("FtyKPI", header: "Fty KPI Date", width: Widths.AnsiChars(10))
            .Numeric("Qty", header: "Total Q'ty", width: Widths.AnsiChars(6))
            .Text("ReasonID", header: "Reason", width: Widths.AnsiChars(10))
            .Text("Name", header: "Reason Desc.", width: Widths.AnsiChars(10))
            .Text("Remark", header: "Reason Remark", width: Widths.AnsiChars(10))
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
            string sqlcmd = $@"SELECT ID, Name FROM Reason WHERE ReasonTypeID = 'OMQtychange'";
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

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Approved")
            {
                this.BtnConfirm.Enabled = true;
                this.BtnReject.Enabled = true;
            }
            else
            {
                this.BtnConfirm.Enabled = false;
                this.BtnReject.Enabled = false;
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
                return;
            }

            var setTxtbox = new Func<string, Sci.Production.Class.txtuser, Sci.Win.UI.TextBox, bool>((signAction, tbName, tbDate) =>
            {
                DataRow rowSign = dtSign.AsEnumerable().Where(rr => rr["Status"].ToString() == signAction).FirstOrDefault();

                tbName.TextBox1Binding = string.Empty;
                tbDate.Text = string.Empty;

                if (rowSign != null)
                {
                    tbName.TextBox1Binding = rowSign["StatusUser"].ToString();
                    tbDate.Text = rowSign["StatusDate"].ToString();
                }

                return true;
            });

            var setTxtbox2 = new Func<string, Sci.Production.Class.txttpeuser, Sci.Win.UI.TextBox, bool>((signAction, tbName, tbDate) =>
            {
                DataRow rowSign = dtSign.AsEnumerable().Where(rr => rr["Status"].ToString() == signAction).FirstOrDefault();

                tbName.DisplayBox1Binding = string.Empty;
                tbDate.Text = string.Empty;

                if (rowSign != null)
                {
                    tbName.DisplayBox1Binding = rowSign["StatusUser"].ToString();
                    tbDate.Text = rowSign["StatusDate"].ToString();
                }

                return true;
            });

            setTxtbox("Confirmed", this.txtuserCFM, this.TxtCFMDate);
            setTxtbox("Reject", this.txtuserReject, this.TxtRejectDate);
            setTxtbox2("Sent", this.txtuserSent, this.TxtSentDate);
            setTxtbox2("Approved", this.txtuserApprove, this.TxtAppDate);
            setTxtbox2("Junk", this.txtuserJunk, this.TxtJunkDate);
            setTxtbox2("Closed", this.txtuserClose, this.TxtCloseDate);
            #endregion

            #region 載入Orders欄位
            DataRow currOrder;
            if (!MyUtility.Check.Seek($@"select * from Orders where ID = '{this.CurrentMaintain["Orderid"]}'", out currOrder))
            {
                MyUtility.Msg.WarningBox("OrderID not found!");
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
            this.numFOC.Text = row["FOCQty"].ToString();
            this.numOderQty.Text = row["QTY"].ToString();
            this.dispModel.Text = row["Model"].ToString();

            if (category == "S")
            {
                this.reasonTypeID = "Order_BuyerDelivery_Sample";
            }
            else
            {
                this.reasonTypeID = "Order_BuyerDelivery";
            }
            #endregion

            this.QtybrkOrderbs.DataSource = null;
            this.QtybrkShipbs1.DataSource = null;
            this.QtybrkShipbs2.DataSource = null;
            this.QtybrkApplybs.DataSource = null;

            #region tab 1
            string sqlCmd = $@"
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
with SortBy as (
      select oq.Article  
             , RowNo = ROW_NUMBER() over (order by oq.Article) 
      from Order_Qty oq WITH (NOLOCK) 
      where oq.ID = '{0}'
      group by oq.Article
),
tmpData as (
      select oq.Article
             , oq.SizeCode
             , oq.Qty
             , sb.RowNo
      from Order_Qty oq WITH (NOLOCK) 
      inner join SortBy sb on oq.Article = sb.Article
      where oq.ID = '{0}'
),
SubTotal as (
      select 'TTL' as Article
             , SizeCode
             , SUM(Qty) as Qty
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
            sum(Qty) for SizeCode in ({1})
      ) a
)
select Total=(select sum(Qty) from UnionData where Article = p.Article)
	,Article,{1}
from pivotData p
order by RowNo",
                this.CurrentMaintain["Orderid"],
                sizeCode);
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
select oq.*,r.Name,r.Remark
from Order_QtyShip oq WITH (NOLOCK) 
left join Reason r WITH (NOLOCK) on r.ReasonTypeID = '{this.reasonTypeID}'  and r.ID = oq.ReasonID
where oq.ID = '{this.CurrentMaintain["Orderid"]}'
order by Seq";

            result = DBProxy.Current.Select(null, sqlCmd, out this.shipdata1);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlCmd = string.Format(
@"with tmpData as (
    select oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty,oa.Seq as ASeq
    from Order_QtyShip_Detail oqd WITH (NOLOCK) 
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
                this.CurrentMaintain["Orderid"],
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
outer apply(select qty=sum(qty) from OrderChangeApplication_Detail oq where  oq.Id  = os.id and oq.Seq = os.NewSeq)x
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

        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = Prgs.PostOrderChange(this.CurrentMaintain["ID"].ToString(), "Confirmed");
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string updateCmd = string.Format(@"
update OrderChangeApplication set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'
INSERT INTO [dbo].[OrderChangeApplication_History]([ID],[Status],[StatusUser],[StatusDate])
VALUES('{1}','Confirmed','{0}',getdate())
", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }

            this.SendMail("Confirmed");

            this.RenewData();
            this.OnDetailEntered();
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = Prgs.PostOrderChange(this.CurrentMaintain["ID"].ToString(), "Reject");
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string updateCmd = string.Format(@"
update OrderChangeApplication set Status = 'Reject', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'
INSERT INTO [dbo].[OrderChangeApplication_History]([ID],[Status],[StatusUser],[StatusDate])
VALUES('{1}','Reject','{0}',getdate())
", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
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
            string ccAddress = "jeff.yeh@sportscity.com.tw";
            string factory = MyUtility.GetValue.Lookup($@"select factoryID from orders with(nolock) where id = '{this.CurrentMaintain["Orderid"]}'");
            string subject = $@"== this is my test mail (Testing) ==Order Qty Change SP#{this.CurrentMaintain["Orderid"]}-{factory}";
            string description = $@"== this is my test mail (Testing) ==
{status} SP#{this.CurrentMaintain["Orderid"]}-{factory} request change qty, please check, apply id# - {this.CurrentMaintain["ID"]}";

            var email = new MailTo(Sci.Env.Cfg.MailFrom, toAddress, ccAddress, subject, null, description, true, true);
            email.ShowDialog(this);
        }
    }
}
