using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Endline_B12 : Sci.Win.Tems.Input1
    {
        /// <inheritdoc/>
        public Endline_B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            bool canJunk = Prgs.GetAuthority(Sci.Env.User.UserID, "Endline_B12 Idle Reason", "CanJunk");

            this.chkJunk.Enabled = canJunk;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Type"] = "ID";
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["id"].ToString()))
            {
                DualResult result;
                DataTable dt;
                if (result = DBProxy.Current.Select("ProductionTPE", "select max(id) max_id from DQSReason WITH (NOLOCK) ", out dt))
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
