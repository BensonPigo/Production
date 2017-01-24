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

namespace Sci.Production.Class
{
    public partial class txtcurrency : Sci.Win.UI.TextBox
    {
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql = "select ID,NameCH,NameEN from Currency where Junk = 0 order by ID";
            DataTable tbCurrency;
            DBProxy.Current.Select("Production", sql, out tbCurrency);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbCurrency, "ID,NameCH,NameEn", "5,14,30", this.Text, "ID,NameCH,NameEn");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(str,"currency","id","Production")==false)
                {
                    MyUtility.Msg.WarningBox(string.Format("< Currency : {0} > not found!!!", str));
                    this.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        public txtcurrency()
        {
            this.Size = new System.Drawing.Size(48, 23);
        }
    }
}
