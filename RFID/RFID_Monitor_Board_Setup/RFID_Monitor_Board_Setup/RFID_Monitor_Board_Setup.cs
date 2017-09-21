using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RFID_Monitor_Board_Setup
{
    public partial class RFID_Monitor_Board_Setup : Sci.Win.Tems.Input7
    {
        public RFID_Monitor_Board_Setup()
        {
            InitializeComponent();
        }
        protected override bool ClickSaveBefore()
        {
            DateTime? m1, m2, n1, n2;
            m1 = MyUtility.Convert.GetDate(txtMorningShiftStart.Text);
            m2 = MyUtility.Convert.GetDate(txtMorningShiftEnd.Text);
            n1 = MyUtility.Convert.GetDate(txtNightShiftStart.Text);
            n2 = MyUtility.Convert.GetDate(txtNightShiftEnd.Text) < m1 ? MyUtility.Convert.GetDate(txtNightShiftEnd.Text).Value.AddDays(1) : MyUtility.Convert.GetDate(txtNightShiftEnd.Text);
            if ((m1 <= n1 && n1 <= m2) || (m1 <= n2 && n2 <= m2))
            {
                MyUtility.Msg.ErrorBox("<Morning Shift> and <Night Shift> can't be repeared!");
                return false;
            }
            if (n1 >= n2)
            {
                MyUtility.Msg.ErrorBox("<Night Shift Start> can't later than <Night Shift End>");
                return false;
            }
            if (MyUtility.Check.Empty(txtSubProcess.Text))
            {
                MyUtility.Msg.ErrorBox("SubProcess can't empty");
                return false;
            }
            return base.ClickSaveBefore();
        }
        //Factory右鍵與驗證
        string sqlcmd_factory = "select ID from Factory where Junk=0";
        private void txtFactory_PopUp(object sender, Sci.Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd_factory, "8", txtFactory.Text, false, ",");
            item.Width = 350;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtFactory.Text = item.GetSelectedString();
        }
        private void txtFactory_Validating(object sender, CancelEventArgs e)
        {
            txtFactory.Text = MyUtility.GetValue.Lookup(string.Format(sqlcmd_factory + " and ID = '{0}'", txtFactory.Text));
        }

        string sqlcmd_subprocess = "select ID from SubProcess where junk=0 and IsRFIDProcess=1";
        private void txtSubProcess_PopUp(object sender, Sci.Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd_subprocess, "20", txtFactory.Text, false, ",");
            item.Width = 450;
            item.Height = 600;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtSubProcess.Text = item.GetSelectedString();
        }
        private void txtSubProcess_Validating(object sender, CancelEventArgs e)
        {
            txtSubProcess.Text = MyUtility.GetValue.Lookup(string.Format(sqlcmd_subprocess + " and ID = '{0}'", txtSubProcess.Text));
        }
    }
}