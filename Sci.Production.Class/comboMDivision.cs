using Sci.Data;
using System.ComponentModel;
using System.Data;

namespace Sci.Production.Class
{
    public partial class comboMDivision : Sci.Win.UI.ComboBox
    {
        public comboMDivision()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        public comboMDivision(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        public void setDefalutIndex(bool defaultValue = false)
        {
            DataTable dataTable;
            DBProxy.Current.Select(null, "Select ID From MDivision", out dataTable);
            DataRow dataRow = dataTable.NewRow();
            dataRow["ID"] = string.Empty;
            dataTable.Rows.Add(dataRow);
            dataTable.DefaultView.Sort = "ID";
            this.DataSource = dataTable;
            this.ValueMember = "ID";
            this.DisplayMember = "ID";

            this.SelectedValue = defaultValue ? Sci.Env.User.Keyword : string.Empty;
        }
    }
}
