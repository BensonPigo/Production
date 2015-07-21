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
    public partial class txtartworktype_fty : Sci.Win.UI.TextBox
    {
        private string m_type = string.Empty;
        private string m_subprocess = string.Empty;

        [Category("Custom Properties")]
        [Description("Classify 值需用單引號包起來；逗點分格。例如：'I','A','S'或單一個值'P'")]
        public string cClassify
        {
            set { this.m_type = value; }
            get { return this.m_type; }
        }

       [Category("Custom Properties")]
       [Description("IsSubprocess 填Y或不填")]
        public string cSubprocess
        {
            set { this.m_subprocess = value; }
            get { return this.m_subprocess; }
        }

       protected override void OnPopUp(TextBoxPopUpEventArgs e)
       {
           base.OnPopUp(e);

           string sqlWhere = "Where 1=1";
           string sqlCmd = string.Empty;

           if (!string.IsNullOrWhiteSpace(cClassify))
           {
               sqlWhere = sqlWhere + " And Classify in (" + this.cClassify + ")";
           };

           if (!string.IsNullOrWhiteSpace(cSubprocess))
           {
               if (this.cSubprocess == "Y")
               { sqlWhere = sqlWhere + " And IsSubprocess =1 "; }
               else
               { sqlWhere = sqlWhere + " And IsSubprocess =0 "; };
           };
           sqlCmd = "select ID, Abbreviation from ArtworkType " + sqlWhere + " order by Seq";
           Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "22,4", this.Text, false, ",");

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
               if (MyUtility.Check.Seek(str, "artworktype", "id") == false)
               {
                   MyUtility.Msg.WarningBox(string.Format("< Artworktype : {0} > not found!!!", str));
                   this.Text = "";
                   e.Cancel = true;
                   return;
               }
           }
           
       }

        public txtartworktype_fty()
        {
            this.Size = new System.Drawing.Size(140, 23);
        }
    }
}
