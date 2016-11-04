using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Subcon
{
    public partial class P37 : Sci.Win.Tems.Input6
    {
        public P37(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "Type = 'F'";

            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
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
                        this.DefaultWhere = "";
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

            MyUtility.Tool.SetupCombox(queryfors, 1, 1, "未結清,未確認,已確認未轉傳票,ALL");
            queryfors.SelectedIndex = 0;

        }
        // Refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            lblStatus.Text = CurrentMaintain["status"].ToString();
            lblSubconDebitNote.Visible = (!MyUtility.Check.Empty(CurrentMaintain["isSubcon"]));
            numBalance.Value = decimal.Parse(CurrentMaintain["amount"].ToString()) - decimal.Parse(CurrentMaintain["received"].ToString());

        }
        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
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
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["issuedate"] = System.DateTime.Today;
            CurrentMaintain["handle"] = Sci.Env.User.UserID;
            CurrentMaintain["Amount"] = 0;
            CurrentMaintain["Tax"] = 0;
            CurrentMaintain["TaxRate"] = 0;
            CurrentMaintain["Status"] = "New";
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select *
,(reasonid +' '+ isnull((select name from dbo.reason where reasontypeid='DebitNote_Reason' and id=reasonid),'')) reason_desc
from debit_detail Where debit_detail.id = '{0}' order by orderid ", masterID);

            return base.OnDetailSelectCommandPrepare(e);

        }

        private void btnDebitSchedule_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.P37_DebitSchedule(false, dr["ID"].ToString(), null, null);
            frm.ShowDialog(this);
            this.RenewData();

        }

        private void masterpanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
