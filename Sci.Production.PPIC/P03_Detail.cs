﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P03_Detail
    /// </summary>
    public partial class P03_Detail : Sci.Win.Subs.Input6A
    {
        /// <summary>
        /// P03_Detail
        /// </summary>
        public P03_Detail()
        {
            this.InitializeComponent();
            this.txtuserFtyModifier.TextBox1.ReadOnly = true;
            this.txtuserFtyModifier.TextBox1.IsSupportEditMode = false;
        }

        /// <inheritdoc/>
        protected override void OnAttached(DataRow data)
        {
            base.OnAttached(data);

            if (!MyUtility.Check.Empty(this.CurrentData["MRLastDate"]))
            {
                this.displayMRLastUpdate.Text = Convert.ToDateTime(this.CurrentData["MRLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.displayMRLastUpdate.Text = string.Empty;
            }

            if (!MyUtility.Check.Empty(this.CurrentData["FtyLastDate"]))
            {
                this.displayFtyLastDate.Text = Convert.ToDateTime(this.CurrentData["FtyLastDate"]).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                this.displayFtyLastDate.Text = string.Empty;
            }
        }
    }
}
