using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

            item = new Sci.Win.Tools.SelectItem(selectCommand, "100", prodText.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            prodText.Text = item.GetSelectedString();
        }

        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.UI.TextBox fabricText = (Sci.Win.UI.TextBox)sender;
            Sci.Win.Tools.SelectItem item;
            string selectCommand = "select Name from Reason where Junk = 0 and ReasonTypeID = 'Fabric_Kind' order by ID";

            item = new Sci.Win.Tools.SelectItem(selectCommand, "100", fabricText.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            fabricText.Text = item.GetSelectedString();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox prodTextValue = (Sci.Win.UI.TextBox)sender;
            if (!string.IsNullOrWhiteSpace(prodTextValue.Text) && prodTextValue.Text != prodTextValue.OldValue)
            {
                string selectCommand = string.Format("select ID from Reason where ReasonTypeID = 'Style_Apparel_Type' and Name = '{0}'", prodTextValue.Text);
                if (!MyUtility.Check.Seek(selectCommand,null))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Prod. Type : {0} > not found!!!", prodTextValue.Text));
                    prodTextValue.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            Sci.Win.UI.TextBox fabricTextValue = (Sci.Win.UI.TextBox)sender;
            if (!string.IsNullOrWhiteSpace(fabricTextValue.Text) && fabricTextValue.Text != fabricTextValue.OldValue)
            {
                string selectCommand = string.Format("select ID from Reason where ReasonTypeID = 'Fabric_Kind' and Name = '{0}'", fabricTextValue.Text);
                if (!MyUtility.Check.Seek(selectCommand,null))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Fabric Type : {0} > not found!!!", fabricTextValue.Text));
                    fabricTextValue.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
