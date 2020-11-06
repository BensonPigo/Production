using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_ImportFromPOTransferOutNo
    /// </summary>
    public partial class P02_ImportFromPOTransferOutNo : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P02_ImportFromPOTransferOutNo
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P02_ImportFromPOTransferOutNo(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.displayCategory.Value = "Material";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorTextColumnSettings ctnno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiver = new DataGridViewGeneratorTextColumnSettings();

            // CTNNo要Trim掉空白字元
            ctnno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                    dr["CTNNo"] = MyUtility.Convert.GetString(e.FormattedValue).Trim();
                }
            };
            receiver.CharacterCasing = CharacterCasing.Normal;
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out Ict.Win.UI.DataGridViewCheckBoxColumn col_chk)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq1", header: "Seq1#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2#", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(10), settings: receiver)
                .Text("Leader", header: "Team Leader", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CTNNo", header: "CTN No.", width: Widths.AnsiChars(5), settings: ctnno)
                .Numeric("Price", header: "Price", decimal_places: 4, iseditingreadonly: true)
                .Numeric("POQty", header: "PO Qty", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m)
                .Numeric("AccuExpressQty", header: "Accu. Express Qty", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m)
                .Numeric("Qty", header: "Q'ty", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("NW", header: "N.W. (kg)", integer_places: 5, decimal_places: 2, maximum: 99999.99m, minimum: 0m);

            this.gridImport.Columns["Selected"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Receiver"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["CTNNo"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["NW"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtTransferOutNo.Text))
            {
                this.txtTransferOutNo.Focus();
                MyUtility.Msg.WarningBox("Transfer Out No. can't empty!!");
                return;
            }

            if (MyUtility.Convert.GetString(this.txtTransferOutNo.Text).IndexOf("'") != -1)
            {
                this.txtTransferOutNo.Text = this.txtTransferOutNo.Text.Replace("'", string.Empty);
            }

            string sqlCmd = $@"
select Selected = 0 
    , a.ID,a.SEQ1,SEQ2,Price,UnitID,BrandID,Leader,LeaderID,SuppID,Supplier,Receiver,CTNNo,NW,FOC
    , POQty = isnull(a.POQty,0) + isnull(a.FOC,0)
    , AccuExpressQty = a.ExpressQty
    , Qty = iif(isnull(a.POQty,0)+isnull(a.FOC,0)-isnull(a.ExpressQty,0)>0,isnull(a.POQty,0)+isnull(a.FOC,0)-isnull(a.ExpressQty,0),0)
    , expressID = '{this.masterData["ID"]}'
from(
    select psd.ID, psd.SEQ1, psd.SEQ2, psd.Price, psd.POUnit as UnitID, isnull(o.BrandID, '') as BrandID,
        isnull(t.Name, '') as Leader, o.SMR as LeaderID, ps.SuppID, ps.SuppID + '-' + s.AbbEN as Supplier, psd.Qty as POQty,
        (select isnull(sum(ed.Qty), 0) from Express_Detail ed WITH(NOLOCK) where ed.OrderID = psd.ID and ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2) as ExpressQty,
        '' as Receiver, '' as CTNNo, 0.0 as NW,
        psd.FOC

    from(
        select distinct A.PoId, A.Seq1, A.Seq2

        from dbo.TransferOut_Detail A WITH(NOLOCK)
        where a.Id = '{this.txtTransferOutNo.Text}'

    )a

    left join PO_Supp_Detail psd WITH(NOLOCK) on psd.ID = a.POID and psd.SEQ1 = a.Seq1 and psd.SEQ2 = a.Seq2

    left join PO_Supp ps WITH(NOLOCK) on psd.ID = ps.ID and psd.SEQ1 = ps.SEQ1

    left join Supp s WITH(NOLOCK) on ps.SuppID = s.ID

    left join Orders o WITH(NOLOCK) on psd.ID = o.ID

    left join TPEPass1 t WITH(NOLOCK) on o.SMR = t.ID
)a";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }

            if (selectData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            this.listControlBindingSource1.DataSource = selectData;
        }

        // Update
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, can't update!");
                return;
            }

            DataRow[] selectedRow = dt.Select("Selected = 1");
            if (selectedRow.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select datas first!!");
                return;
            }

            DataTable sourcedt = selectedRow.CopyToDataTable();
            if (!this.BeforeUpdate(sourcedt))
            {
                return;
            }

            IList<string> insertCmds = new List<string>();
            foreach (DataRow dr in selectedRow)
            {
                insertCmds.Add(string.Format(
                    @"
insert into Express_Detail(ID,OrderID,Seq1,Seq2,Qty,NW,CTNNo,Category,SuppID,Price,UnitID,Receiver,BrandID,Leader,Remark,InCharge,AddName,AddDate)
 values('{0}','{1}','{2}','{3}',{4},{5},'{6}','4','{7}',{8},'{9}','{10}','{11}','{12}','{13}','{14}','{14}',GETDATE());
",
                    MyUtility.Convert.GetString(this.masterData["ID"]),
                    MyUtility.Convert.GetString(dr["ID"]),
                    MyUtility.Convert.GetString(dr["Seq1"]),
                    MyUtility.Convert.GetString(dr["Seq2"]),
                    MyUtility.Convert.GetString(dr["Qty"]),
                    MyUtility.Convert.GetString(dr["NW"]),
                    MyUtility.Convert.GetString(dr["CTNNo"]),
                    MyUtility.Convert.GetString(dr["SuppID"]),
                    MyUtility.Convert.GetString(dr["Price"]),
                    MyUtility.Convert.GetString(dr["UnitID"]),
                    MyUtility.Convert.GetString(dr["Receiver"]),
                    MyUtility.Convert.GetString(dr["BrandID"]),
                    MyUtility.Convert.GetString(dr["LeaderID"]),
                    this.txtRemark.Text.Replace("'", "''"),
                    Env.User.UserID));
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Executes(null, insertCmds);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    result = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.masterData["ID"])));
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Update complete!!");
        }

        private bool BeforeUpdate(DataTable sourcedt)
        {
            if (MyUtility.Check.Empty(this.txtRemark.Text))
            {
                this.txtRemark.Focus();
                MyUtility.Msg.WarningBox("Remark can't empty!!");
                return false;
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.CheckP02Status(this.masterData["ID"].ToString()))
            {
                return false;
            }

            StringBuilder chkQty = new StringBuilder();
            foreach (DataRow dr in sourcedt.Rows)
            {
                if (MyUtility.Check.Empty(dr["Receiver"]))
                {
                    MyUtility.Msg.WarningBox("Receiver can't empty!!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["CTNNo"]))
                {
                    MyUtility.Msg.WarningBox("CTN No. can't empty!!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["NW"]))
                {
                    MyUtility.Msg.WarningBox("N.W. (kg) can't empty!!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    chkQty.Append($"SP#:{MyUtility.Convert.GetString(dr["ID"])}, Seq1#:{MyUtility.Convert.GetString(dr["Seq1"])}, Seq2#:{MyUtility.Convert.GetString(dr["Seq2"])}\r\n");
                }
            }

            // Qty不可為0
            if (chkQty.Length > 0)
            {
                MyUtility.Msg.WarningBox("Q'ty can't be 0!!\r\n" + chkQty.ToString());
                return false;
            }

            // ISP20201574 檢查 SP#, Seq1, Seq2, CTN No., Category = (DB 固定 4, 顯示 Material) 重複. A.勾選重複, B.DB與勾選重複
            string sqlcmd = $@"
select distinct ID,Seq1,Seq2,CTNNo
from(
    select t.ID,t.Seq1,t.Seq2,t.CTNNo from #tmp t
    group by ID,Seq1,Seq2,CTNNo
    having count(1) >1

    union
    select t.ID, t.Seq1, t.Seq2, t.CTNNo
    from #tmp t
    inner join Express_Detail ed on ed.OrderID = t.ID and ed.Seq1 = t.Seq1 and ed.Seq2 = t.Seq2 and ed.CTNNo = t.CTNNo
        and t.expressID = ed.id
    where ed.Category = '4'
)x
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(sourcedt, string.Empty, sqlcmd, out DataTable checkData);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (checkData.Rows.Count > 0)
            {
                StringBuilder msgStr = new StringBuilder();
                msgStr.Append("Data can't duplicate!!\r\n");
                foreach (DataRow dr in checkData.Rows)
                {
                    msgStr.Append($"SP#:{dr["ID"]}, Seq1#:{dr["Seq1"]}, Seq2#:{dr["Seq2"]}, CTN No.:{dr["CTNNo"]}, Category:Material\r\n");
                }

                MyUtility.Msg.WarningBox(msgStr.ToString());
                return false;
            }

            // 檢查Total Qty是否有超過PO Qty
            sqlcmd = @"
select a.ID,a.Seq1,a.Seq2
from (
    select e.ID,e.Seq1,e.Seq2,e.POQty,e.FOC,
    TtlQty = e.Qty +
        isnull((
            select sum(ed.Qty)
            from Express_Detail ed WITH (NOLOCK)
            where ed.OrderID = e.ID and ed.Seq1 = e.Seq1 and ed.Seq2 = e.Seq2
        ), 0)
    from #tmp e
) a
where a.TtlQty > a.POQty
";
            result = MyUtility.Tool.ProcessWithDatatable(sourcedt, string.Empty, sqlcmd, out checkData);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (checkData.Rows.Count > 0)
            {
                StringBuilder msgStr = new StringBuilder();
                foreach (DataRow dr in checkData.Rows)
                {
                    msgStr.Append(string.Format("SP#:{0}, Seq1#:{1}, Seq2#:{2}\r\n", MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["Seq1"]), MyUtility.Convert.GetString(dr["Seq2"])));
                }

                msgStr.Append("Total Qty > PO Qty, pls check!!");
                MyUtility.Msg.WarningBox(msgStr.ToString());
                return false;
            }

            return true;
        }
    }
}
