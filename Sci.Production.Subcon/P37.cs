using System;
using System.Data;
using System.Windows.Forms;

using Ict;
using Ict.Win;

namespace Sci.Production.Subcon
{
    public partial class P37 : Sci.Win.Tems.Input6
    {
        public P37(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "Type = 'F'";

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "settled !='Y'";
                        break;
                    case 1:
                        this.DefaultWhere = "Status = 'New'";
                        break;
                    case 2:
                        this.DefaultWhere = "Status ='Confirmed'";
                        break;
                    case 3:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        break;
                }

                this.ReloadDatas();
            };
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, "Outstanding,Not yet CFM,CFM w/o voucher,ALL");
            this.queryfors.SelectedIndex = 0;
        }

        // Refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.lblStatus.Text = this.CurrentMaintain["status"].ToString();
            this.lblSubconDebitNote.Visible = !MyUtility.Check.Empty(this.CurrentMaintain["isSubcon"]);
            this.numBalance.Value = decimal.Parse(this.CurrentMaintain["amount"].ToString()) - decimal.Parse(this.CurrentMaintain["received"].ToString());
            DataRow ftyVD;
            MyUtility.Check.Seek(
                string.Format(
                @"SELECT VoucherDate FROM SciFMS_Voucher WHERE ID=(SELECT VoucherFactory FROM Debit WHERE ID='{0}')",
                this.displayDebitNo.Text), out ftyVD);
            if (ftyVD != null)
           {
               this.dateFactoryVoucherDate.Text = Convert.ToDateTime(ftyVD["VoucherDate"]).ToString("yyyy/MM/dd");
           }
           else
            {
                this.dateFactoryVoucherDate.Text = string.Empty;
            }
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 6, iseditingreadonly: true)
                .Numeric("amount", header: "Amount", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                .Text("reason_desc", header: "Reason", iseditingreadonly: true, width: Widths.AnsiChars(25))

                ;
            #endregion

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["issuedate"] = System.DateTime.Today;
            this.CurrentMaintain["handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["Amount"] = 0;
            this.CurrentMaintain["Tax"] = 0;
            this.CurrentMaintain["TaxRate"] = 0;
            this.CurrentMaintain["Status"] = "New";
            this.dateFactoryVoucherDate.ReadOnly = true;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"select *
,(reasonid +' '+ isnull((select name from dbo.reason WITH (NOLOCK) where reasontypeid='DebitNote_Reason' and id=reasonid),'')) reason_desc
from debit_detail WITH (NOLOCK) Where debit_detail.id = '{0}' order by orderid ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void btnDebitSchedule_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new Sci.Production.Subcon.P37_DebitSchedule(false, dr["ID"].ToString(), null, null, this.CurrentMaintain, "P37");
            frm.ShowDialog(this);
            this.RenewData();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }
    }
}
