﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// B09
    /// </summary>
    public partial class B09 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B09
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ClickSaveBefore()
        {
            string sqlchk = $@"
select 1
from SubprocessLeadTime s
where id <> '{this.CurrentMaintain["ID"]}'
and (select stuff((select concat('+', SubprocessID) from SubprocessLeadTime_Detail sd where s.ID = sd.ID order by sd.SubprocessID for xml path('')),1,1,''))='{this.txtSubprocess.Text}' 
";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Data duplication, can't save !!");
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            string[] subprocessID = this.txtSubprocess.Text.Split('+');

            string sqlcmd = $@"delete SubprocessLeadTime_Detail where id = {this.CurrentMaintain["ID"]}" + "\r\n";
            if (!MyUtility.Check.Empty(subprocessID))
            {
                foreach (string item in subprocessID)
                {
                    sqlcmd += $"insert into SubprocessLeadTime_Detail values({this.CurrentMaintain["ID"]},'{item}')" + "\r\n";
                }
            }

            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        private decimal? LeadTime;
        private string Subprocess;
        private string ArtworkType;

        protected override bool ClickCopyBefore()
        {
            this.LeadTime = this.numLeadTime.Value;
            this.Subprocess = this.txtSubprocess.Text;
            this.ArtworkType = this.disArtworkType.Text;

            return base.ClickCopyBefore();
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["ID"] = DBNull.Value;
            this.numLeadTime.Value = this.LeadTime;
            this.txtSubprocess.Text = this.Subprocess;
            this.disArtworkType.Text = this.ArtworkType;
        }

        protected override DualResult ClickDeletePost()
        {
            string sqlcmd = $@"delete SubprocessLeadTime_Detail where id = {this.CurrentMaintain["ID"]}";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        private void TxtSubprocess_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd = $@"
Select s.id as [Subprocess],s.ArtworkTypeId AS [Artwork Type] from subprocess S 
inner join ArtworkType a on a.id=s.ArtworkTypeId
where S.junk=0 and s.IsSelection=1
order by s.id asc
";
            //SelectItem item = new SelectItem(sqlcmd, string.Empty, this.txtSubprocess.Text, true, "+");
            SelectItem2 item = new SelectItem2(sqlcmd, "Subprocess", "12", this.txtSubprocess.Text, null, null, null);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.txtSubprocess.Text = item.GetSelectedString().Replace(",", "+");

            this.disArtworkType.Text = item.GetSelecteds().AsEnumerable().Select(s => MyUtility.Convert.GetString(s["Artwork Type"])).JoinToString("+");
        }

        private void TxtSubprocess_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSubprocess.Text))
            {
                this.txtSubprocess.Text = string.Empty;
                this.disArtworkType.Text = string.Empty;
                return;
            }

            string[] subprocessID = this.txtSubprocess.Text.Split('+');

            if (subprocessID.AsEnumerable().GroupBy(g => new { subprocessID = g }).Select(s => new { s.Key.subprocessID, ct = s.Count() }).Any(a => a.ct > 1))
            {
                MyUtility.Msg.WarningBox("Subprocess duplication!!");
                e.Cancel = true;
                return;
            }

            string sqlcmd = $@"select ID from Subprocess where junk = 0 and IsSelection=1";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            var notinList = subprocessID.AsEnumerable().Where(w => !dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ID"])).Contains(w)).ToList();
            if (notinList.Count > 0)
            {
                MyUtility.Msg.WarningBox("Subprocess:" + string.Join(",", notinList) + " not found !");
                e.Cancel = true;
                return;
            }

            sqlcmd = $@"
Select s.ArtworkTypeId
from subprocess S 
inner join ArtworkType a on a.id=s.ArtworkTypeId
where S.junk=0 and s.IsSelection=1
and s.id in({"'" + string.Join("','", subprocessID) + "'"})
order by s.id asc
";
            result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            
            this.disArtworkType.Text = dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ArtworkTypeId"])).JoinToString("+");
        }

        private void NumLeadTime_Validating(object sender, CancelEventArgs e)
        {
            if (!(numLeadTime.Value >0 && numLeadTime.Value <128))
            {
                MyUtility.Msg.WarningBox("Lead Time need >0 and <128");
                e.Cancel = true;
                return;
            }
        }
    }
}
