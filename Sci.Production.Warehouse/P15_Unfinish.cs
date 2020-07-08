using System;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P15_Unfinish : Sci.Win.Subs.Base
    {
        public static string TypeFabric = "F";
        public static string TypeAccessory = "A";
        protected string FabricType;

        public P15_Unfinish(string FabricType, string title)
        {
            this.Text = title.ToString();
            this.InitializeComponent();
            this.FabricType = FabricType;

            // 請勿刪除 Hide & Timer ！！！
            // this.timer();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.selectData();

            // 設定Grid1的顯示欄位
            this.gridUnfinish.IsEditingReadOnly = true;
            this.gridUnfinish.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridUnfinish)
                 .Text("ctype", header: "Type", width: Widths.AnsiChars(25))
                 .Text("ID", header: "Request#", width: Widths.AnsiChars(13))
                 .Date("issuedate", header: "Date", width: Widths.AnsiChars(13));
        }

        private void selectData()
        {
            string selectCmd = string.Format(
                @"
declare @Fabric varchar(30) = case when '{1}' = 'A' then 'Accessory' when '{1}' = 'F' then 'Fabric' end
SELECT TOP 30 (case when L.Type = 'L' then @Fabric + '-Lacking' 
					when L.Type = 'R' then @Fabric + '-Replacement' end) as ctype
, L.issuedate, L.ID 
FROM LACK  L WITH (NOLOCK) 
WHERE L.Status = 'Confirmed' and (L.apvname != '' OR L.ApvName is NOT null) AND (L.IssueLackId = '' OR L.IssueLackId is null) AND MDivisionID = '{0}' and L.FabricType = '{1}'
ORDER BY issuedate desc,id asc;", Sci.Env.User.Keyword, this.FabricType);
            DataTable selectDataTable1;
            this.ShowWaitMessage("Data Loading...");
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCmd, out selectDataTable1);

            if (selectResult1 == false)
            {
                this.ShowErr(selectCmd, selectResult1);
            }

            this.HideWaitMessage();
            this.bindingSource1.DataSource = selectDataTable1;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
            this.Hide();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.selectData();
        }

        private void timer()
        {
            this.timer1.Interval = 5000;
            this.timer1.Tick += (o, e) =>
            {
                this.timer1.Stop();

                // this.TopMost = true;
                this.ShowDialog();

                // this.Activate();
                // this.TopMost = false;
                this.selectData();
            };
        }
    }
}
