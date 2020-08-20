using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Sci.Data;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class ComboxMaterialTypeAndID : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboxMaterialTypeAndID"/> class.
        /// </summary>
        public ComboxMaterialTypeAndID()
        {
            this.InitializeComponent();
            this.InitComboMaterialType();

            // 設定初始值 Value和Display都是空值!
            MyUtility.Tool.SetupCombox(this.comboMtlTypeID, 1, 1, ",");
        }

        private void InitComboMaterialType()
        {
            Dictionary<string, string> comboBoxRowSource = new Dictionary<string, string>
            {
                { "All", "All" },
                { "F", "Fabric" },
                { "A", "Accessory" },
            };
            this.comboMaterialType.DataSource = new BindingSource(comboBoxRowSource, null);
            this.comboMaterialType.ValueMember = "Key";
            this.comboMaterialType.DisplayMember = "Value";
            this.comboMaterialType.SelectedIndex = 0;
        }

        private void ComboMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = this.comboMaterialType.SelectedValue.ToString();
            if (type.Empty() || type.Contains("All"))
            {
                return;
            }

            DataTable dt = this.GetComboMtlTypeIDDataSource(type);
            this.comboMtlTypeID.DataSource = dt;
            this.comboMtlTypeID.ValueMember = "ID";
            this.comboMtlTypeID.DisplayMember = "ID";
        }

        private DataTable GetComboMtlTypeIDDataSource(string type)
        {
            DataTable dt = new DataTable();
            if (type.Empty() || type.Contains("All"))
            {
                return dt;
            }

            DBProxy.Current.Select(null, $"select distinct ID from MtlType where Type = '{type}'", out dt);
            DataRow dataRow = dt.NewRow();
            dataRow["ID"] = string.Empty;
            dt.Rows.Add(dataRow);
            dt.DefaultView.Sort = "ID";
            return dt;
        }
    }
}
