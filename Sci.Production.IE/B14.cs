﻿using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B10
    /// </summary>
    public partial class B14 : Win.Tems.Input1
    {
        /// <summary>
        /// B10
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
        }


        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Measurement"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Attachment Measurement > can not be empty!");
                return false;
            }

            DualResult result;
            DataTable dt;

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                if (result = DBProxy.Current.Select(null, "select max_id = max(id) from AttachmentMeasurement WITH (NOLOCK)", out dt))
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
                    return false;
                }
            }

            List<SqlParameter> parameters = new List<SqlParameter>()
                        {
                            new SqlParameter("@Measurement", this.CurrentMaintain["Measurement"].ToString()),
                            new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()),
                        };

            result = DBProxy.Current.Select(null, "select ID from AttachmentMeasurement WITH (NOLOCK) WHERE Measurement = @Measurement AND ID = @ID", parameters, out dt);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dt != null && dt.Rows.Count > 0 && this.IsDetailInserting)
            {
                MyUtility.Msg.WarningBox("This < Attachment Measurement > already exists.");
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
