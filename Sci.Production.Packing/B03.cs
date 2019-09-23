using Ict;
using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Sci.Win.Tems.Input1
    {
        private Hashtable ht = new Hashtable();

        /// <summary>
        /// B03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            if (this.ht.Count == 0)
            {
                this.ht.Add("Picture1", path + "CTN.jpg");
                this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
            }
        }

        private void TxtCTNRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select RefNo  from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' ", null, this.txtCTNRefno.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCTNRefno.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Seq"] = 1;
        }

        private void TxtCTNRefno_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtCTNRefno.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtCTNRefno.OldValue)
            {
                if (!MyUtility.Check.Seek($"Select 1 from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' and RefNo = '{textValue}'"))
                {
                    this.txtCTNRefno.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< RefNo : {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CustCD"]))
            {
                MyUtility.Msg.WarningBox("CustCD can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CTNRefno"]))
            {
                MyUtility.Msg.WarningBox("CTNRefno can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Side"]))
            {
                MyUtility.Msg.WarningBox("Side can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Seq"]))
            {
                MyUtility.Msg.WarningBox("Seq can not empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
