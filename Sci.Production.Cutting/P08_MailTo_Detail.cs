using System.Data;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P08_MailTo_Detail
    /// </summary>
    public partial class P08_MailTo_Detail : Sci.Win.Subs.Input6A
    {
        /// <summary>
        /// P08_MailTo_Detail
        /// </summary>
        public P08_MailTo_Detail()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.OnAttached(this.CurrentData);
        }

        /// <summary>
        /// OnAttached
        /// </summary>
        /// <param name="data">data</param>
        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);

            if (this.EditMode)
            {
                this.txtCode.ReadOnly = !MyUtility.Check.Empty(data["id"]);
            }
        }

        /// <summary>
        /// DoSave
        /// </summary>
        /// <returns>bool</returns>
        protected override bool DoSave()
        {
            if (MyUtility.Check.Empty(this.txtCode.Text))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.editMailTo.Text))
            {
                MyUtility.Msg.WarningBox("< Mail to > can not be empty!");
                this.editMailTo.Focus();
                return false;
            }

            return base.DoSave();
        }
    }
}
