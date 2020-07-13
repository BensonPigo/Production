using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B01
    /// </summary>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboNewRepeatAll, 2, 1, "A,All,N,New,R,Repeat");
        }

        /// <summary>
        /// ClickNewAfter()
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["UseFor"] = "A";
            this.CurrentMaintain["BaseOn"] = 1;
        }

        /// <summary>
        /// ClickEditAfter()
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.txtBrand.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Code"]))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            // 移除BrandID必填，避免無BrandID的舊資料無法更新
            // if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            // {
            //    MyUtility.Msg.WarningBox("< Brand > can not be empty!");
            //    textBox4.Focus();
            //    return false;
            // }
            if (MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
            {
                MyUtility.Msg.WarningBox("< Activities > can not be empty!");
                this.txtActivities.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["UseFor"]))
            {
                MyUtility.Msg.WarningBox("< New/Repeat > can not be empty!");
                this.comboNewRepeatAll.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        // Brand
        private void TxtBrand_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Brand WITH (NOLOCK)	WHERE Junk=0  ORDER BY Id";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "10,30,30", this.txtBrand.Text, false, ",");
            item.Size = new System.Drawing.Size(750, 500);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtBrand.Text = item.GetSelectedString();
        }

        private void TxtBrand_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtBrand.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtBrand.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"SELECT Id FROM Brand WITH (NOLOCK) WHERE Junk=0 AND id = '{0}'", textValue)))
                {
                    this.txtBrand.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Brand: {0} > not found!!!", textValue));
                    return;
                }
            }
        }
    }
}
