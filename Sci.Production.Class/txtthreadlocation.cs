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
using Ict.Win;

namespace Sci.Production.Class
{
    public partial class txtthreadlocation : Sci.Win.UI.TextBox
    {
        
        public txtthreadlocation()
        {
            this.Size = new System.Drawing.Size(90, 23);
            this.IsSupportSytsemContextMenu = false;
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {            
            base.OnPopUp(e);
            if (e.IsHandled) return;

            string keyword = Sci.Env.User.Keyword;
            string sql = string.Format("select ID,Description from ThreadLocation WITH (NOLOCK) where mDivisionid ='{0}' order by ID", keyword);
            DataTable tbthLocation;
            DBProxy.Current.Select("Production", sql, out tbthLocation);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(tbthLocation, "ID,Description", "10,40", this.Text, "ID,Description");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
            e.IsHandled = true;
            
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string keyword = Sci.Env.User.Keyword;
            string str = this.Text;
            if (!MyUtility.Check.Empty(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(str, "ThreadLocation", "id") == false)
                {
                    this.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Thread Location : {0} > not found!!!", str));
                    return;
                }
            }
        }
    }
}
