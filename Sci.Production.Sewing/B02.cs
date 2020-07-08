using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class B02 : Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("FactoryID = '{0}'", Sci.Env.User.Factory);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        protected override bool ClickSaveBefore()
        {
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            string sql = string.Empty;
            #region 檢查不可為空值
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtID.Focus();
                MyUtility.Msg.WarningBox("ID can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Name"]))
            {
                this.txtID.Focus();
                MyUtility.Msg.WarningBox("Name can't empty!!");
                return false;
            }
            #endregion

            if (this.IsDetailInserting)
            {
                sql = string.Format("select * from LineLocation where FactoryID = '{0}' and ID = '{1}'", Sci.Env.User.Factory, this.txtID.Text.ToString());
                if (MyUtility.Check.Seek(sql))
                {
                    this.txtID.Focus();
                    MyUtility.Msg.WarningBox(string.Format("ID:{0} already existed, please use another ID", this.txtID.Text.ToString()));
                    return false;
                }
            }

            sql = string.Format("select * from LineLocation where FactoryID = '{0}' and Name = '{1}' and ID <> '{2}'", Sci.Env.User.Factory, this.txtName.Text.ToString(), this.txtID.Text.ToString());
            if (MyUtility.Check.Seek(sql))
            {
                this.txtName.Focus();
                MyUtility.Msg.WarningBox(string.Format("Name:{0} already existed, please use another Name", this.txtName.Text.ToString()));
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
