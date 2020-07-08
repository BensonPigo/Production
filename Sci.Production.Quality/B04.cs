using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B04 : Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // 設定combox 固定的值
            Dictionary<string, string> combobox1_RowSource = new Dictionary<string, string>();
            combobox1_RowSource.Add("F", "Fabric");
            combobox1_RowSource.Add("A", "Accessories");
            this.comboMaterialType.DataSource = new BindingSource(combobox1_RowSource, null);
            this.comboMaterialType.ValueMember = "Key";
            this.comboMaterialType.DisplayMember = "Value";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.txtLevel.Focus();
                MyUtility.Msg.WarningBox("< Level > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Range1"]) && this.CurrentMaintain["ID"].ToString() != "A")
            {
                this.txtRateRangeStart.Focus();
                MyUtility.Msg.WarningBox("< Lower Rate Range > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Range2"]))
            {
                this.txtRateRangeEnd.Focus();
                MyUtility.Msg.WarningBox("< Higher Rate Range > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Type"]))
            {
                this.comboMaterialType.Focus();
                MyUtility.Msg.WarningBox("< Material Type > can not be empty!");
                return false;
            }

            #endregion
            return base.ClickSaveBefore();
        }
    }
}
