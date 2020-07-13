using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P03_BatchConfirm : Win.Subs.Base
    {
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P03_BatchConfirm()
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
               .Text("ID", header: "ID", iseditingreadonly: true, width: Widths.AnsiChars(14))
               .Text("FactoryID", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Text("OrderID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
               .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("CustCDID", header: "CustCD", iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Text("Dest", header: "Dest", iseditingreadonly: true, width: Widths.AnsiChars(5))
               .Text("ShipModeID", header: "Ship Mode", iseditingreadonly: true, width: Widths.AnsiChars(6))
               .Text("INVNo", header: "Invoive No.", iseditingreadonly: true, width: Widths.AnsiChars(10))
               .Text("Status", header: "Status", iseditingreadonly: true, width: Widths.AnsiChars(5))
               .Text("CartonFinishRate", header: "% of ttl ctn" + Environment.NewLine + " transferred", iseditingreadonly: true)
               .Date("buyerdelivery", header: "Buyer Delivery", iseditingreadonly: true)
               .Date("PulloutDate", header: "Pullout Date", iseditingreadonly: true)
               .EditText("ErrorMsg", header: "Error Message", iseditingreadonly: true, width: Widths.AnsiChars(20))
              ;

            // 設定detailGrid Rows 是否可以編輯
            this.grid.RowEnter += this.Detailgrid_RowEnter;
            this.grid.Columns["Selected"].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var data = ((DataRowView)this.grid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            // ErrorMsg 是空值,代表可以confirmed才能勾選
            if (MyUtility.Check.Empty(data["ErrorMsg"].ToString()))
            {
                this.col_chk.IsEditable = true;
            }
            else
            {
                this.col_chk.IsEditable = false;
            }
        }

        private void btnFindNow_Click(object sender, EventArgs e)
        {
            this.QueryData(false);
        }

        public void QueryData(bool AutoQuery)
        {
            DateTime? pulloutdate1;
            StringBuilder strSQLCmd = new StringBuilder();
            pulloutdate1 = this.dateBoxPulloutDate.Value;
            string buyerDelivery1 = this.dateBuyerDelivery.Value1.Empty() ? string.Empty : ((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            string buyerDelivery2 = this.dateBuyerDelivery.Value2.Empty() ? string.Empty : ((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd");
            string brand = this.txtbrand.Text;
            string factory = this.txtmfactory.Text;

            string sqlcmd = $@"
select distinct [Selected]=0
,p1.ID,p1.FactoryID,Pack2.OrderID,p1.BrandID,p1.CustCDID,p1.Dest,p1.ShipModeID,p1.INVNo,p1.Status
,IIF(isnull(o.TotalCTN,0) = 0,'0',CONVERT(varchar,Round(CONVERT(float,isnull(o.ClogCTN,0)) / o.TotalCTN * 100,2)) ) + '%' as CartonFinishRate,QtyShip.BuyerDelivery,p1.PulloutDate,[ErrorMsg]=''
from PackingList p1
inner join PackingList_Detail p2 on p1.ID=p2.ID
left join Orders o on o.ID=p2.OrderID
outer apply(
	select OrderID = stuff((
		select concat(',',s.OrderID)
		from(
			select distinct OrderId
			from PackingList_Detail
			where ID=p1.ID
			)s
		for xml path('')
	),1,1,'')
)Pack2
outer apply(
	select min(BuyerDelivery) BuyerDelivery
	from Order_QtyShip os where os.Id=o.ID 
	and os.Seq=p2.OrderShipmodeSeq
)QtyShip
where 1=1 and p1.Status='New' and p1.MDivisionID='{Env.User.Keyword}' and p1.Type='B'
";
            if (!MyUtility.Check.Empty(pulloutdate1))
            {
                sqlcmd += $" and p1.PulloutDate = '{this.dateBoxPulloutDate.Text}'";
            }

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlcmd += $" and QtyShip.BuyerDelivery >= '{buyerDelivery1}'";
            }

            if (!MyUtility.Check.Empty(buyerDelivery2))
            {
                sqlcmd += $" and QtyShip.BuyerDelivery <= '{buyerDelivery2}'";
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlcmd += $" and o.BrandID='{brand}'";
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd += $" and p1.FactoryID='{factory}'";
            }

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out this.dt_detail))
            {
                if (this.dt_detail.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            #region 塞入Error Msg
            foreach (DataRow dr in this.dt_detail.Rows)
            {
                sqlcmd = $@"exec dbo.usp_Packing_P03_Confirm '{dr["id"]}','{Env.User.Factory}','{Env.User.UserID}','0'";

                DataTable dtSP = new DataTable();
                if (result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtSP))
                {
                    dr["ErrorMsg"] = dtSP.Rows[0]["msg"];
                }
            }
            #endregion

            this.listControlBindingSource1.DataSource = this.dt_detail;
            this.HideWaitMessage();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBatchConfirmed_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1 and ErrorMsg='' ");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to batch Confirme?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            int cnt = 1;
            string sqlcmd = string.Empty;
            DualResult result;
            foreach (DataRow dr in dr2)
            {
                this.ShowWaitMessage($"{cnt}/{dr2.Length}");

                // 有三個參數, PackingList.Id,Factory,UserID,1 = update Status, 0 = 只有Query
                sqlcmd = $@"exec dbo.usp_Packing_P03_Confirm '{dr["id"]}','{Env.User.Factory}','{Env.User.UserID}','1'";

                if (!(result = DBProxy.Current.Execute(string.Empty, sqlcmd)))
                {
                    this.ShowErr(result);
                }

                cnt++;
            }

            MyUtility.Msg.InfoBox("Finish bath Confirmed!");
            this.HideWaitMessage();
            this.QueryData(true);
        }
    }
}
