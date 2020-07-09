using Sci.Data;
using System.ComponentModel;
using System.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// combo MDivision
    /// </summary>
    public partial class ComboMDivision : Win.UI.ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComboMDivision"/> class.
        /// </summary>
        public ComboMDivision()
        {
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// comboMDivision
        /// </summary>
        /// <param name="container">container</param>
        public ComboMDivision(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
            this.Size = new System.Drawing.Size(80, 23);
        }

        /// <summary>
        /// Set ComboBox Data
        /// </summary>
        /// <param name="defaultValue">如果沒輸入，SelectedValue 預設 Sci.Env.User.Keywordd</param>
        public void SetDefalutIndex(bool defaultValue = false)
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

            this.SelectedValue = defaultValue ? Env.User.Keyword : string.Empty;
        }
    }
}
