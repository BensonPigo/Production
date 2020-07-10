using System.Collections.Generic;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B13
    /// </summary>
    public partial class B13 : Win.Tems.Input1
    {
        /// <summary>
        /// B13
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessories");
            comboBox1_RowSource.Add(string.Empty, string.Empty);
            this.comboMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboMaterialType.ValueMember = "Key";
            this.comboMaterialType.DisplayMember = "Value";
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 在編輯模式下，下列這些欄位都不可以被修改
            if (this.EditMode)
            {
                this.comboMaterialType.ReadOnly = true;
                this.checkJunk.ReadOnly = true;
                this.checkExtend.ReadOnly = true;
                this.checkZipper.ReadOnly = true;
                this.checkIsICRItem.ReadOnly = true;
                this.checkIsTrimCardOther.ReadOnly = true;
            }
        }
    }
}
