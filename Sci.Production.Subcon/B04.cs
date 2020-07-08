using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /*
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Reason"]))
            {
                MyUtility.Msg.WarningBox("< Reason > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                DualResult result;
                DataTable dt;
                if (result = DBProxy.Current.Select(null, "select max(id) max_id from SubconReason WITH (NOLOCK) where type='SQ' ", out dt))
                {
                    string id = dt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        this.CurrentMaintain["ID"] = "00001";
                    }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        this.CurrentMaintain["ID"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }
                }
                else
                {
                    this.ShowErr(result);
                }
            }

            this.CurrentMaintain["Type"] = "SQ";
            this.CurrentMaintain["Status"] = "New";

            return base.ClickSaveBefore();
        }

        /// /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            DBProxy.Current.Execute(null, $"UPDATE SubconReason SET Status = 'Junked',Junk=1 ,EditDate=GETDATE() ,EditName='{Sci.Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            DBProxy.Current.Execute(null, $"UPDATE SubconReason SET Status = 'New' ,Junk=0 ,EditDate=GETDATE() ,EditName='{Sci.Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }
        */
    }
}
