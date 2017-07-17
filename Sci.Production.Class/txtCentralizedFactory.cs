using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using System.Configuration;


namespace Sci.Production.Class
{
    public partial class txtCentralizedFactory : Sci.Win.UI.TextBox
    {
        private string ConnServer;
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            DataTable FactoryData = new DataTable();
            DataRow row;
            //DataColumn column = new DataColumn("Factory", Type.GetType("System.String"));
            FactoryData.Columns.Add("Factory", typeof(string));

            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            foreach (string strSever in strSevers)
            {
                string[] Factorys = strSever.Split(new char[] { ':',',' });
                for (int i = 1; i < Factorys.Length; i++)
                {
                    row = FactoryData.NewRow();
                    row["Factory"] = Factorys[i];
                    FactoryData.Rows.Add(row);
                }
            }
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(FactoryData, "Factory", "5", this.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            ConnServer = "";
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
                foreach (string strSever in strSevers)
                {
                    if (!MyUtility.Check.Empty(ConnServer)) break;
                    string[] Factorys = strSever.Split(new char[] { ':', ',' });
                    for (int i = 1; i < Factorys.Length; i++)
                    {
                        if (str == Factorys[i])
                        {
                            ConnServer = Factorys[0];
                            break;
                        }
                    }
                }
                if (MyUtility.Check.Empty(ConnServer))
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                    return;
                }
            }
        }

        public txtCentralizedFactory()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
