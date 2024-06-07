using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P09 : Win.Tems.Input6
    {
        private DataTable PatternPanelTb;

        /// <inheritdoc/>
        public P09(ToolStripMenuItem menuitem)
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
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
        }

        /// <summary>
        /// 匯入SCI 自行定義的Excel
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnImportMarker_Click(object sender, EventArgs e)
        {
            //CuttingWorkOrder cuttingWorkOrder = new CuttingWorkOrder();
            //DualResult result = cuttingWorkOrder.ImportMarkerExcel();
            DualResult result = new DualResult(true);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (result.Description == "NotImport")
            {
                return;
            }

            this.OnRefreshClick();
        }

        /// <summary>
        /// 匯入立克系統產生的檔案
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnImportMarkerLectra_Click(object sender, EventArgs e)
        {

            #region Benson 自行測試時使用
            string cmdsql = string.Format(@"Select *,0 as newKey From WorkOrder_PatternPanel WITH (NOLOCK) Where id='{0}'", this.CurrentMaintain["ID"]);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out this.PatternPanelTb);
            if (!dr)
            {
                this.ShowErr(cmdsql, dr);
            }
            #endregion

            string id = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            string sqlcmd = $@"
select top 1 s.SizeGroup, s.PatternNo, oe.markerNo, s.ID, p.Version
from Order_EachCons oe 
inner join dbo.SMNotice s on oe.SMNoticeID = s.ID
inner join SMNotice_Detail sd with(nolock)on sd.id = s.id
inner join Pattern p with(nolock)on p.id = sd.id
where oe.ID = '{id}'
and sd.PhaseID = 'Bulk'
and p.Status='Completed'
order by p.EditDate desc
";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow drSMNotice))
            {
                string styleUkey = MyUtility.GetValue.Lookup($@"select o.StyleUkey from Orders o where o.id = '{id}'");
                var form = new P09_ImportML(styleUkey, id, drSMNotice, (DataTable)this.detailgridbs.DataSource);
                form.ShowDialog();
            }
            else
            {
                MyUtility.Msg.InfoBox("Not found SMNotice Datas"); // 正常不會發生這狀況
            }

            #region 產生第3層 PatternPanel 只有一筆
            this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["ImportML"])).ToList().ForEach(row =>
            {
                DataRow drNEW = this.PatternPanelTb.NewRow();
                drNEW["id"] = this.CurrentMaintain["ID"];
                drNEW["WorkOrderUkey"] = 0;  // 新增WorkOrderUkey塞0
                drNEW["PatternPanel"] = row["PatternPanel"];
                drNEW["FabricPanelCode"] = row["FabricPanelCode"];
                drNEW["newkey"] = row["newkey"];
                this.PatternPanelTb.Rows.Add(drNEW);
            });
            #endregion

            int icount = this.DetailDatas.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["ImportML"])).Count();
            if (icount > 0)
            {
                for (int i = 0; i < icount; i++)
                {
                    if (this.detailgrid.CurrentCell != null)
                    {
                        this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["Layer"]; // 移動到指定cell 觸發 Con 計算
                    }
                }
            }

            if (icount > 0)
            {
                this.detailgrid.CurrentCell = this.detailgrid.Rows[0].Cells["Layer"];
                this.detailgrid.SelectRowTo(0);
            }
        }
    }
}
