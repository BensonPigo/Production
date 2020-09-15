using Sci.Win.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtProt
    /// </summary>
    public partial class TxtPort : _UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtPort"/> class.
        /// </summary>
        public TxtPort()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public DisplayBox DisplayBox1 { get; private set; }

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
SELECT p.ID,[PortName]=p.Name,p.CountryID,[Country Name]=c.NameEN 
    ,[AirPort]=IIF(p.AirPort=1,'Y','') 
    ,[SeaPort]=IIF(p.SeaPort=1,'Y','') 
FROM [dbo].Port p 
INNER JOIN Country c ON p.CountryID = c.ID 
WHERE p.Junk = 0 
ORDER BY p.ID";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "20,10,10,20,5,5", this.TextBox1.Text, false, ",");
            item.Size = new System.Drawing.Size(888, 666);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            DataRow dr = item.GetSelecteds()[0];

            this.TextBox1.Text = MyUtility.Convert.GetString(dr["ID"]);
            this.DisplayBox1.Text = MyUtility.Convert.GetString(dr["PortName"]);
        }

        /// <inheritdoc/>
        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
            string nPortID = this.TextBox1.Text;

            if (!string.IsNullOrWhiteSpace(nPortID) && nPortID != this.TextBox1.OldValue)
            {
                string cmd = $"SELECT Name FROM Port WHERE ID=@ID AND Junk=0";
                List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("@ID", nPortID) };

                if (!MyUtility.Check.Seek(cmd, parameters))
                {
                    this.TextBox1.Text = string.Empty;
                    this.DisplayBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Port: {0} > not found!!!", nPortID));
                }
                else
                {
                    this.DisplayBox1.Text = MyUtility.GetValue.Lookup(cmd, parameters);
                }
            }

            // this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }
    }
}
