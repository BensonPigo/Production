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

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P15P16_ReuqeustList : Win.Subs.Base
    {
        private string strPPICReasonType = string.Empty;
        private DataRow dataRow;

        /// <inheritdoc/>
        public P15P16_ReuqeustList(string strP15orP16Form, DataRow data)
        {
            this.InitializeComponent();
            this.strPPICReasonType = strP15orP16Form == "P15" ? "AL" : "FL";
            this.dataRow = data;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 欄位設定

            // 設定Grid1的顯示欄位
            this.gridAccumulatedQty.IsEditingReadOnly = true;
            this.gridAccumulatedQty.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAccumulatedQty)
                 .Text("SP", header: "SP#", width: Widths.AnsiChars(15))
                 .Text("seq", header: "Seq#", width: Widths.AnsiChars(10))
                 .Text("Description", header: "Description", width: Widths.AnsiChars(40))
                 .Text("Unit", header: "Unit", width: Widths.AnsiChars(8))
                 .Text("RequestQty", header: "Request" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8))
                 .Text("Process", header: "Process", width: Widths.AnsiChars(10))
                 .Text("Reason", header: "Reason", width: Widths.AnsiChars(20))
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
                 ;
            #endregion 欄位設定

            // SQL
            string sqlcmd = $@"select
                            [SP] = l.POID
                            ,[seq] = Concat(ld.Seq1, ' ', ld.Seq2)
                            ,[Description] =strDescription.val
                            ,[Unit] = psd.StockUnit
                            ,[RequestQty] = ld.RequestQty
                            ,[Process] = ld.Process
                            ,[Reason] = Concat (ld.PPICReasonID, '-', iif(p.DeptID <> '', concat(p.DeptID, '-', p.Description), p.Description))
                            ,[Remark] = ld.Remark
                            from Lack l with(nolock) 
                            inner join Lack_Detail ld with(nolock) on l.ID = ld.ID
                            left join PO_Supp_Detail psd with(nolock) on psd.ID = l.POID
										                                and psd.SEQ1 = ld.Seq1
										                                and psd.SEQ2 = ld.Seq2
                            left join PPICReason p with (nolock) on p.Type = '{this.strPPICReasonType}' and p.ID = ld.PPICReasonID
                            outer apply
                            (
	                            select val = dbo.getMtlDesc(l.poid, ld.seq1, ld.seq2,2,0)
                            )strDescription
                            where l.ID = '{MyUtility.Convert.GetString(this.dataRow["requestid"])}'";

            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd,out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.ErrorBox(MyUtility.Convert.GetString(dualResult.Messages));
                return;
            }

            this.gridAccumulatedQty.DataSource = dt;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
