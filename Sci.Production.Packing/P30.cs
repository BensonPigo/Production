using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P30
    /// </summary>
    public partial class P30 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P30
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.displayNike.Text = "Nike";
            this.timerRefresh.Interval = 10000;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridDownloadStickerQueue)
                .Text("PackingID", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("CTNQty", header: "Ttl Ctns", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .EditText("ErrorMsg", header: "Error msg", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", iseditingreadonly: true)
                .DateTime("UpdateDate", header: "Update Date", iseditingreadonly: true);

            this.Query();
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Start();
        }

        private void Query()
        {
            string sqlGetData = @"
select  dq.PackingID,
        p.CTNQty,
        p.ShipQty,
        dq.ErrorMsg,
        dq.AddDate,
        dq.UpdateDate,
        dq.Processing
from DownloadStickerQueue dq with (nolock)
inner join PackingList p with (nolock) on p.ID = dq.PackingID
order by    dq.UpdateDate
";

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select("Production", sqlGetData, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridDownloadStickerQueue.DataSource = dtResult;

            foreach (DataGridViewRow gridRow in this.gridDownloadStickerQueue.Rows)
            {
                DataRow curRow = this.gridDownloadStickerQueue.GetDataRow(gridRow.Index);

                // 在這裡根據Row的內容設置背景色
                if (MyUtility.Convert.GetBool(curRow["Processing"]))
                {
                    gridRow.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    gridRow.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            this.Query();
        }
    }
}
