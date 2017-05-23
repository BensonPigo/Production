﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Ict;
using Sci.Win.Tools;

namespace Sci.Production.Class
{
    public partial class txtcdcode : Sci.Win.UI.TextBox
    {
        public txtcdcode() 
        {
            this.Size = new System.Drawing.Size(54, 23);
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "CDCode", "Id"))
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< CD Code : {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("Select ID, convert(nvarchar(5),Cpu) Cpu, Description from CDCode WITH (NOLOCK) where Junk = 0 order by ID", "7,6,45", this.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
