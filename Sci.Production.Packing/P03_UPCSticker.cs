using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Sci.Production.Packing
{
    /// <summary>
    /// P03_UPCSticker
    /// </summary>
    public partial class P03_UPCSticker : Win.Forms.Base
    {
        private string ID;
        private DataTable dt;

        /// <summary>
        /// P03_UPCSticker
        /// </summary>
        /// <param name="id">id</param>
        public P03_UPCSticker(string id)
        {
            this.InitializeComponent();
            this.grid1.IsEditingReadOnly = false;
            this.ID = id;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sqlcmd = $@"
select DISTINCT
P.BrandID,
O.StyleID,
pd.Article,
pd.Color,
pd.SizeCode,
pd.Barcode,
OS.Seq
from PackingList_Detail PD 
LEFT JOIN ORDERS O ON PD.OrderID = O.ID
LEFT JOIN Order_SizeCode OS ON OS.Id = O.POID AND PD.SizeCode = OS.SizeCode
LEFT JOIN PackingList P ON PD.ID=P.ID
where pd.id='{this.ID}' --放入該表頭的Pack ID
ORDER BY O.StyleID,pd.Article,OS.Seq ASC
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource1.DataSource = this.dt;

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("StyleID", header: "Style#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("Article", header: "Colorway", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("Barcode", header: "Barcode", width: Widths.AnsiChars(16))
            ;
            this.grid1.Columns["Barcode"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            string updateSqlCmd = $@"
update pd set PD.Barcode = #tmp.Barcode
FROM Orders o
LEFT JOIN PackingList_Detail PD ON PD.OrderID = O.ID 
,#tmp
WHERE O.StyleID=#tmp.StyleID AND O.BrandID=#tmp.BrandID AND PD.Article=#tmp.Article and pd.SizeCode=#tmp.SizeCode

select  distinct PD.ID
from    Orders o
LEFT JOIN PackingList_Detail PD ON PD.OrderID = O.ID
LEFT JOIN PackingList P ON P.ID = PD.ID
left join Pullout pu on P.PulloutID = pu.ID
WHERE   exists(select 1 from #tmp where O.StyleID=#tmp.StyleID AND O.BrandID=#tmp.BrandID AND PD.Article=#tmp.Article and pd.SizeCode=#tmp.SizeCode) and
        isnull (pu.Status, '') not in ('Confirmed', 'Locked')

";
            DataTable udt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dt, string.Empty, updateSqlCmd, out udt);
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                if (udt.Rows.Count > 0)
                {
                    string listID = udt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["ID"])).JoinToString(",");
                    Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(listID, string.Empty))
                               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                    // 因為會傳圖片，拆成單筆 PackingListNo 轉出，避免一次傳出的容量過大超過api大小限制
                    foreach (DataRow dr in udt.Rows)
                    {
                        #region ISP20201607 資料交換 - Gensong
                        if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
                        {
                            // 不透過Call API的方式，自己組合，傳送API
                            Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(MyUtility.Convert.GetString(dr["ID"]), string.Empty))
                                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        #endregion
                    }
                }

                MyUtility.Msg.InfoBox("Update success!");
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
