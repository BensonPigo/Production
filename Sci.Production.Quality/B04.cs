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

namespace Sci.Production.Quality
{
    public partial class B04 : Sci.Win.Tems.Input1
    {        
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            // 設定combox 固定的值
            Dictionary<String,String> combobox1_RowSource = new Dictionary<string,string>();
            combobox1_RowSource.Add("F", "Fabric");
            combobox1_RowSource.Add("A", "Accessories");
            comboBox1.DataSource = new BindingSource(combobox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
           // this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            #region 必輸檢查
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Level > can not be empty!");
                this.textBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Range1"]))
            {
                MyUtility.Msg.WarningBox("< Lower Rate Range > can not be empty!");
                this.textBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Range2"]))
            {
                MyUtility.Msg.WarningBox("< Higher Rate Range > can not be empty!");
                this.textBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Type"]))
            {
                MyUtility.Msg.WarningBox("< Material Type > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }

            #endregion
            return base.ClickSaveBefore();
        }
    }
}
