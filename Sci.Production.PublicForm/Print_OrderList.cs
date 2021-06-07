using System;
using System.Collections.Generic;
using System.Data;
using Sci.Data;
using Ict;
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Data.SqlClient;
using Sci.Utility.Excel;
using Sci.Production.PublicPrg;

namespace Sci.Production.PublicForm
{
    /// <inheritdoc/>
    public partial class Print_OrderList : Win.Tems.QueryForm
    {
        private string _id;
        private int _finished;

        /// <summary>
        /// Initializes a new instance of the <see cref="Print_OrderList"/> class.
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="f">f</param>
        public Print_OrderList(string args, int f = 0)
        {
            this._id = args;
            this._finished = f;
            this.InitializeComponent();
            this.EditMode = true;
        }

        private bool ToExcel()
        {
            if (this.radioEachConsumption.Checked)
            {
                return Prgs.EachConsumption(this._id);
            }

            if (this.radioTTLConsumption.Checked)
            {
                return Prgs.TTLConsumption(this._id);
            }

            return true;
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.ToExcel();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
