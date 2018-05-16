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
    public partial class txtseason : Sci.Win.UI.TextBox
    {
        public txtseason()
        {
            this.Size = new System.Drawing.Size(80, 23);
        }

        private Control brandObject;
        [Category("Custom Properties")]
        public Control BrandObjectName
        {
            set { this.brandObject = value; }
            get { return this.brandObject; }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            string textValue = this.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.OldValue)
            {
                if (!MyUtility.Check.Seek(textValue, "Season", "Id","Production"))
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Season : {0} > not found!!!", textValue));
                    return;
                }
                else
                {
                    if (this.brandObject != null)
                    {
                        if (!string.IsNullOrWhiteSpace((string)this.brandObject.Text))
                        {
                            string selectCommand = string.Format("select ID from Season WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}'", (string)this.brandObject.Text, this.Text.ToString());
                            if (!MyUtility.Check.Seek(selectCommand, "Production"))
                            {
                                this.Text = "";
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Season: {0} > not found!!!", textValue));
                                return;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);

            Sci.Win.Tools.SelectItem item;
            string selectCommand = "select distinct ID from Production.dbo.Season WITH (NOLOCK) order by id desc";
            if (this.brandObject != null && !string.IsNullOrWhiteSpace((string)this.brandObject.Text))
            {
                selectCommand = string.Format("select distinct ID from Production.dbo.Season WITH (NOLOCK) where BrandID = '{0}' order by id desc", this.brandObject.Text);
            }
            item = new Sci.Win.Tools.SelectItem(selectCommand, "11", this.Text);
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
    }
}
