using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Quality
{
    /// <summary>
    /// B13
    /// </summary>
    public partial class B13 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B13
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.displayEditName.Text = MyUtility.GetValue.Lookup(string.Format("select iif(c.QAEditName = '' , '', Concat(c.QAEditName, '-', (select Name from pass1 where ID = c.QAEditName))) from CustCD c WITH (NOLOCK) where c.ID = '{0}' and c.BrandID = '{1}'", this.CurrentMaintain["ID"].ToString(), this.CurrentMaintain["BrandID"].ToString()));
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            base.ClickEditBefore();
            this.txtCountry.TextBox1.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
            this.txtCountry.TextBox1.IsSupportEditMode = false;
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            string sqlcmd = string.Format(
                "update c set c.Need3rdInspect = '{2}', c.QAEditName = '{3}', c.QAEditDate = getdate() from CustCD c where c.ID = '{0}' and c.BrandID = '{1}'",
                this.CurrentMaintain["ID"].ToString(),
                this.CurrentMaintain["BrandID"].ToString(),
                this.CurrentMaintain["Need3rdInspect"].ToString(),
                Sci.Env.User.UserID);
            DualResult result = DBProxy.Current.Execute("Production", sqlcmd);
            return result;
        }
    }
}
