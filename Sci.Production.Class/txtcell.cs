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
using Sci;
using Ict;
using Sci.Win;


namespace Sci.Production.Class
{
  
    public partial class txtCell : Sci.Win.UI.TextBox
    {
        private string mdivision = "";
        private string where = "";   //" Where junk = 0";
        [Category("Custom Properties")]
        public string MDivisionID
        {
            set { mdivision = value; }
            get { return mdivision; }
        }
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;
            if (!string.IsNullOrWhiteSpace(mdivision))
            {
                where = string.Format(" Where junk = 0 and mdivisionid = '{0}'", mdivision);
            }
            sql = "select distinct id from Production.dbo.CutCell WITH (NOLOCK) " + where;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "2", this.Text, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string tmp = null;
                if (!string.IsNullOrWhiteSpace(mdivision))
                    tmp = MyUtility.GetValue.Lookup("id", mdivision + str, "Production.dbo.Cutcell", "mdivisionid+id");
                else
                    tmp = MyUtility.GetValue.Lookup("id", str, "Production.dbo.Cutcell", "id");

                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Cut Cell> : {0} not found!!!", str));
                    return;
                }
                else
                {
                    string cJunk = null;
                    if (!string.IsNullOrWhiteSpace(mdivision))
                        cJunk = MyUtility.GetValue.Lookup("Junk", mdivision + str, "Production.dbo.CutCell", "mdivisionid+id");
                    else
                        cJunk = MyUtility.GetValue.Lookup("Junk", str, "Production.dbo.CutCell", "id");

                    if (cJunk == "True")
                    {
                        this.Text = "";
                        MyUtility.Msg.WarningBox(string.Format("Cut Cell already junk, you can't choose!!"));
                    }

                }
            }
        }
        public txtCell()
        {
            this.Width = 30;
        }

    }
}