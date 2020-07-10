using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B50
    /// </summary>
    public partial class B50 : Win.Tems.Input1
    {
        /// <summary>
        /// B50
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            MyUtility.Tool.SetupCombox(this.comboCategory, 1, 1, "FABRIC,ACCESSORY,CHEMICAL,MACHINERY,OTHETS");
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.comboCategory.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtDescriptionofGoods.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 檢查必輸欄位
            if (MyUtility.Check.Empty(this.txtDescriptionofGoods.Text))
            {
                this.txtDescriptionofGoods.Focus();
                MyUtility.Msg.WarningBox("Description of Goods can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtHSCode.Text))
            {
                this.txtHSCode.Focus();
                MyUtility.Msg.WarningBox("HS Code can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.comboCategory.Text))
            {
                this.comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }

            // 填入NL Code
            if (this.IsDetailInserting)
            {
                string nlcode = MyUtility.GetValue.Lookup("select CONVERT(int,isnull(max(NLCode),0)) from KHGoodsHSCode WITH (NOLOCK) ");
                this.CurrentMaintain["NLCode"] = (MyUtility.Convert.GetInt(nlcode) + 1).ToString("00000");
            }

            return base.ClickSaveBefore();
        }
    }
}
