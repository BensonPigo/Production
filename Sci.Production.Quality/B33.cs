﻿using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <summary>
    /// B33
    /// </summary>
    public partial class B33 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B33
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, ",,D,Day,N,Night");
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtfactory.ReadOnly = true;
            this.comboShift.ReadOnly = true;
            this.dateStartDate.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Shift"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SubprocessID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["StartDate"]))
            {
                MyUtility.Msg.WarningBox("<Start Date>,<Factory>, <Subprocess> and <Shift> cannot empty.");

                return false;
            }

            return base.ClickSaveBefore();
        }

        private void TxtRest1Start_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRest1Start.Text == ":")
            {
                this.CurrentMaintain["Rest1Start"] = DBNull.Value;
            }
        }

        private void TxtRest1End_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRest1End.Text == ":")
            {
                this.CurrentMaintain["Rest1End"] = DBNull.Value;
            }
        }

        private void TxtRest2Start_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRest2Start.Text == ":")
            {
                this.CurrentMaintain["Rest2Start"] = DBNull.Value;
            }
        }

        private void TxtRest2End_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRest2End.Text == ":")
            {
                this.CurrentMaintain["Rest2End"] = DBNull.Value;
            }
        }
    }
}
