using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;
using System.Configuration;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtCentralizedFactory
    /// </summary>
    public partial class TxtCentralizedFactory : Win.UI.TextBox
    {
        private string ConnServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TxtCentralizedFactory"/> class.
        /// </summary>
        public TxtCentralizedFactory()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            DataTable factoryData = new DataTable();
            DataRow row;

            // DataColumn column = new DataColumn("Factory", Type.GetType("System.String"));
            factoryData.Columns.Add("Factory", typeof(string));

            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            foreach (string strSever in strSevers)
            {
                string[] factorys = strSever.Split(new char[] { ':', ',' });
                for (int i = 1; i < factorys.Length; i++)
                {
                    row = factoryData.NewRow();
                    row["Factory"] = factorys[i];
                    factoryData.Rows.Add(row);
                }
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(factoryData, "Factory", "5", this.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            this.ConnServer = string.Empty;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
                foreach (string strSever in strSevers)
                {
                    if (!MyUtility.Check.Empty(this.ConnServer))
                    {
                        break;
                    }

                    string[] factorys = strSever.Split(new char[] { ':', ',' });
                    for (int i = 1; i < factorys.Length; i++)
                    {
                        if (str == factorys[i])
                        {
                            this.ConnServer = factorys[0];
                            break;
                        }
                    }
                }

                if (MyUtility.Check.Empty(this.ConnServer))
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
