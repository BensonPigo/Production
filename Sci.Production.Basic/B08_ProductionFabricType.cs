using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B08_ProductionFabricType : Sci.Win.Subs.Input1A
    {
        public B08_ProductionFabricType(bool canedit, DataRow data)
            : base(canedit, data)
        {
            InitializeComponent();
        }

        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.UI.TextBox prodText = (Sci.Win.UI.TextBox)sender;
            Sci.Win.Tools.SelectItem item;
            string selectCommand = "select Name from Reason where Junk = 0 and ReasonTypeID = 'Style_Apparel_Type' order by ID";

            item = new Sci.Win.Tools.SelectItem(selectCommand, "20", prodText.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            prodText.Text = item.GetSelectedString();
        }

        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.UI.TextBox fabricText = (Sci.Win.UI.TextBox)sender;
            Sci.Win.Tools.SelectItem item;
            string selectCommand = "select Name from Reason where Junk = 0 and ReasonTypeID = 'Fabric_Kind' order by ID";

            item = new Sci.Win.Tools.SelectItem(selectCommand, "20", fabricText.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            fabricText.Text = item.GetSelectedString();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox prodTextValue = (Sci.Win.UI.TextBox)sender;
            if (!MyUtility.Check.Empty(prodTextValue.Text) && prodTextValue.Text != prodTextValue.OldValue)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@name";
                sp1.Value = prodTextValue.Text;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);

                string selectCommand = "select ID from Reason where ReasonTypeID = 'Style_Apparel_Type' and Name = @name";
                DataTable reasonID;
                DualResult result = DBProxy.Current.Select(null, selectCommand, cmds, out reasonID);
                if (result)
                {
                    if (reasonID.Rows.Count <= 0)
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Prod. Type : {0} > not found!!!", prodTextValue.Text));
                        prodTextValue.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                    prodTextValue.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox fabricTextValue = (Sci.Win.UI.TextBox)sender;
            if (!MyUtility.Check.Empty(fabricTextValue.Text) && fabricTextValue.Text != fabricTextValue.OldValue)
            {
                //sql參數
                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@name";
                sp1.Value = fabricTextValue.Text;

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);

                string selectCommand = "select ID from Reason where ReasonTypeID = 'Fabric_Kind' and Name = @name";
                DataTable reasonID;
                DualResult result = DBProxy.Current.Select(null, selectCommand, cmds, out reasonID);
                if (result)
                {
                    if (reasonID.Rows.Count <= 0)
                    {
                        MyUtility.Msg.WarningBox(string.Format("< Fabric Type : {0} > not found!!!", fabricTextValue.Text));
                        fabricTextValue.Text = "";
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                    fabricTextValue.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
