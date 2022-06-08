﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P01_ReTransferMtlToScrap : Win.Tems.QueryForm
    {
        private string poID;
        private DataTable dtScrapHistory;
        private DataTable dtBulk;

        /// <inheritdoc/>
        public P01_ReTransferMtlToScrap(string poID)
        {
            this.InitializeComponent();
            this.Text = $"Re-Transfer Mtl. to Scrap ({poID})";
            this.poID = poID;
            this.EditMode = true;
            this.gridBulk.SupportEditMode = Win.UI.AdvEditModesReadOnly.True;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridScrapHistory)
           .Text("fabrictype", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
           .Text("FromSeq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
           .Text("FromSeq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
           .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
           .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
           .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
           ;

            this.Helper.Controls.Grid.Generator(this.gridBulk)
            .CheckBox("select", trueValue: 1, falseValue: 0, iseditable: true)
            .Text("fabrictype", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)
            .Text("Lock", header: "Lock", width: Widths.AnsiChars(5), iseditingreadonly: true)
            ;

            this.Query();
        }

        private void Query()
        {
            DualResult result;
            string sqlScrapHistory = $@"
select 
	a.FromPOID
    ,a.FromSeq1
    ,a.FromSeq2
    ,FabricType = case p1.FabricType    
                    when 'F' then 'Fabric' 
                    when 'A' then 'Accessory' 
                    when 'O' then 'Other' 
                    else p1.FabricType 
                  end  
    ,p1.stockunit
    ,[description] = dbo.getmtldesc(a.FromPoId,a.FromSeq1,a.FromSeq2,2,0)
    ,a.FromRoll
    ,a.FromDyelot
    ,Qty = sum(a.Qty)
from dbo.SubTransfer_Detail a WITH (NOLOCK) 
inner join dbo.SubTransfer s with (nolock) on a.ID = s.ID and s.Type = 'D' and MDivisionID = '{Env.User.Keyword}'
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId and p1.seq1 = a.FromSeq1 and p1.SEQ2 = a.FromSeq2
left join FtyInventory f WITH (NOLOCK) on a.FromPOID=f.POID and a.FromSeq1=f.Seq1 and a.FromSeq2=f.Seq2 and a.FromRoll=f.Roll and a.FromDyelot=f.Dyelot and a.FromStockType=f.StockType
Where s.POID = '{this.poID}'
group by a.FromPOID ,a.FromSeq1 ,a.FromSeq2,p1.FabricType ,p1.stockunit,a.FromRoll,a.FromDyelot
";

            result = DBProxy.Current.Select(null, sqlScrapHistory, out this.dtScrapHistory);
            if (!result)
            {
                this.ShowErr(result);
            }

            string sqlBulk = $@"
select  [select] = 0
        ,f.Seq1
        ,f.Seq2
        ,FabricType = case p1.FabricType    
                        when 'F' then 'Fabric' 
                        when 'A' then 'Accessory' 
                        when 'O' then 'Other' 
                        else p1.FabricType 
                      end  
        ,p1.stockunit
        ,[description] = dbo.getmtldesc(f.PoId,f.Seq1,f.Seq2,2,0)
        ,f.Roll
        ,f.Dyelot
        ,[Qty] = f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty
        ,f.Ukey
        ,[Lock] = iif(f.Lock = 1, 'Y', '')
        ,f.WMSLock
        ,[MDivisionPoDetailUkey] = mdp.Ukey
from FtyInventory f 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = f.PoId and p1.seq1 = f.Seq1 and p1.SEQ2 = f.Seq2
left join MDivisionPoDetail mdp on  mdp.POID=p1.ID and 
                                    mdp.Seq1=p1.SEQ1 and 
                                    mdp.Seq2=p1.SEQ2
inner join orders o on o.id = f.POID
where (f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty) > 0 
and f.StockType='B' 
and o.WhseClose is not null 
and o.MDivisionID='{Env.User.Keyword}' 
and f.POID = '{this.poID}'
";

            result = DBProxy.Current.Select(null, sqlBulk, out this.dtBulk);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.gridScrapHistory.DataSource = this.dtScrapHistory;
            this.gridBulk.DataSource = this.dtBulk;

            this.ChangeGridStyle();
        }

        private void ChangeGridStyle()
        {
            foreach (DataGridViewRow gridRow in this.gridBulk.Rows)
            {
                if (gridRow.Cells["Lock"].Value.ToString() == "Y")
                {
                    gridRow.DefaultCellStyle.BackColor = Color.Silver;
                    gridRow.Cells["select"].ReadOnly = true;
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRetransferToScrap_Click(object sender, EventArgs e)
        {
            var selectedBulk = this.dtBulk.AsEnumerable().Where(s => (int)s["select"] == 1).ToList();

            if (!selectedBulk.Any())
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            DataRow[] chkWMSLock = this.dtBulk.Select(" select = 1 and WMSLock = 1");
            string errmsg = string.Empty;
            if (chkWMSLock.Length > 0)
            {
                foreach (DataRow tmp in chkWMSLock)
                {
                    errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox("Material Locked cause from WMS system not received below material yet." + Environment.NewLine + errmsg, "Warning");
                return;
            }

            DataTable dtSubTransfer_Detail = new DataTable();
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    string subTransferId = MyUtility.GetValue.GetID(Env.User.Keyword + "AC", "SubTransfer", DateTime.Now);
                    if (MyUtility.Check.Empty(subTransferId))
                    {
                        MyUtility.Msg.WarningBox("Get document ID fail!!");
                        return;
                    }

                    DualResult result = PublicPrg.Prgs.ReTransferMtlToScrapByPO(subTransferId, this.poID, selectedBulk);
                    if (!result)
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Select(null, $"select * from SubTransfer_Detail where id = '{subTransferId}'", out dtSubTransfer_Detail)))
                    {
                        throw result.GetException();
                    }

                    // 檢查 Barcode不可為空
                    if (dtSubTransfer_Detail.Rows.Count > 0)
                    {
                        Prgs.GetFtyInventoryData(dtSubTransfer_Detail, "P25", out DataTable dtOriFtyInventory);
                        if (!Prgs.CheckBarCode(dtOriFtyInventory, "P25"))
                        {
                            transactionscope.Dispose();
                            return;
                        }
                    }

                    // 上方 Auto Create P25 Confrim 後, 寫入新的 BarCode
                    if (!(result = Prgs.UpdateWH_Barcode(true, dtSubTransfer_Detail, "P25", out bool fromNewBarcode)))
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // SubTransfer_Detail
            if (dtSubTransfer_Detail.Rows.Count > 0)
            {
                Gensong_AutoWHFabric.Sent(false, dtSubTransfer_Detail, "P25", EnumStatus.New, EnumStatus.Confirm);
                Vstrong_AutoWHAccessory.Sent(false, dtSubTransfer_Detail, "P25", EnumStatus.New, EnumStatus.Confirm);
            }

            MyUtility.Msg.InfoBox("Complete!");
            this.Query();
        }

        private void GridBulk_Sorted(object sender, EventArgs e)
        {
            this.ChangeGridStyle();
        }
    }
}
