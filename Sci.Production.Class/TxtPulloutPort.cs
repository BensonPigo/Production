using Sci.Data;
using Sci.Win.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtProt
    /// </summary>
    public partial class TxtPulloutPort : _UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtPulloutPort"/> class.
        /// </summary>
        public TxtPulloutPort()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.TextBox1.Text; }
            set { this.TextBox1.Text = value; }
        }

        /// <inheritdoc/>
        private void TextBox1_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string sql = @"
SELECT p.ID,P.Name,p.CountryID,[Country Name]=c.NameEN 
    ,[AirPort]=IIF(p.AirPort=1,'Y','') 
    ,[SeaPort]=IIF(p.SeaPort=1,'Y','') 
FROM PulloutPort p 
INNER JOIN Country c ON p.CountryID = c.ID 
WHERE p.Junk = 0 
ORDER BY p.ID";

            DataTable source;
            DBProxy.Current.Select("Production", sql, out source);

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(source, "ID,Name,CountryID,Country Name,AirPort,SeaPort", "20,25,10,20,5,5", this.TextBox1.Text)
            {
                Size = new System.Drawing.Size(950, 666),
            };

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            DataRow dr = item.GetSelecteds()[0];

            this.TextBox1.Text = MyUtility.Convert.GetString(dr["ID"]);

            this.Validate();
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        /// <inheritdoc/>
        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
            string nPulloutPort = this.TextBox1.Text;

            if (!string.IsNullOrWhiteSpace(nPulloutPort) && nPulloutPort != this.TextBox1.OldValue)
            {
                string cmd = $"SELECT Name FROM PulloutPort WHERE ID=@ID AND Junk=0";

                List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ID", nPulloutPort) };

                if (!MyUtility.Check.Seek(cmd, parameters, "Production"))
                {
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< PulloutPort: {0} > not found!!!", nPulloutPort));
                }
            }

            if (MyUtility.Check.Empty(nPulloutPort))
            {
                this.TextBox1.Text = string.Empty;
            }

            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }
    }
}
