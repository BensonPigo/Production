﻿using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// SubCon_B02
    /// </summary>
    public partial class SubCon_B02 : Win.Tems.Input1
    {
        /// <summary>
        /// SubCon_B02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public SubCon_B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = "IP";
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Reason"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Reason > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                DualResult result;
                DataTable dt;
                if (result = DBProxy.Current.Select("ProductionTPE", "select max(id) max_id from SubconReason WITH (NOLOCK) where type='IP'", out dt))
                {
                    string id = dt.Rows[0]["max_id"].ToString();
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        this.CurrentMaintain["ID"] = "00001";
                    }
                    else
                    {
                        int newID = int.Parse(id) + 1;
                        this.CurrentMaintain["ID"] = Convert.ToString(newID).ToString().PadLeft(5, '0');
                    }
                }
                else
                {
                    this.ShowErr(result);
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
