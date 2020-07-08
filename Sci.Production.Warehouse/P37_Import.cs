using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class P37_Import : Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtScrap;

        public P37_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        private void btnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string transid = this.txtTransaction.Text.TrimEnd();
            string wkno = this.txtWK.Text.TrimEnd();

            if (MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(wkno))
            {
                MyUtility.Msg.WarningBox("Please fill < SP# > or < Transaction > or < WK# > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                #region -- SQL Command --
                if (MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(wkno))
                {
                    strSQLCmd.Append(string.Format(@"
select  0 as selected 
        , '' id
        , '' ExportId
        , null as ETA
        , f.PoId
        , f.seq1
        , f.seq2
        , concat(Ltrim(Rtrim(f.seq1)), ' ', f.Seq2) as seq
        , f.Roll
        , f.Dyelot
        , p1.stockunit
        , f.StockType 
        , f.InQty - f.OutQty + f.AdjustQty balance
        , 0.00 as qty
        , dbo.Getlocation(f.ukey) as location
        , dbo.getMtlDesc(f.PoId,f.seq1,f.seq2,2,0) [description]
        , f.ukey ftyinventoryukey
from dbo.FtyInventory f WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = f.PoId and p1.seq1 = f.SEQ1 and p1.SEQ2 = f.seq2
where   f.InQty - f.OutQty + f.AdjustQty > 0 
        and f.lock=0 
        and stocktype !='O'"));
                }
                else
                {
                    strSQLCmd.Append(string.Format(@"
select  0 as selected 
        , '' id
        , a.ExportId
        , a.ETA
        , b.PoId
        , b.seq1
        , b.seq2
        , concat(Ltrim(Rtrim(b.seq1)), ' ', b.seq2) as seq
        , b.Roll
        , b.Dyelot
        , p1.stockunit
        , b.StockType 
        , f.InQty - f.OutQty + f.AdjustQty balance
        , 0.00 as qty
        , dbo.Getlocation(f.ukey) as location
        , dbo.getMtlDesc(b.PoId,b.seq1,b.seq2,2,0) [description]
        , f.ukey ftyinventoryukey
from dbo.Receiving a WITH (NOLOCK) 
inner join dbo.Receiving_Detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.FtyInventory f WITH (NOLOCK) on f.POID = b.PoId and f.seq1 = b.seq1 and f.seq2 = b.Seq2 and f.stocktype = b.stocktype
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = b.PoId and p1.seq1 = b.SEQ1 and p1.SEQ2 = b.seq2
where   f.InQty - f.OutQty + f.AdjustQty > 0 
        and f.lock = 0 
        and a.Status = 'Confirmed' 
        and b.StockType!='O'
"));
                }
                #endregion

                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";

                System.Data.SqlClient.SqlParameter seq1 = new System.Data.SqlClient.SqlParameter();
                seq1.ParameterName = "@seq1";

                System.Data.SqlClient.SqlParameter seq2 = new System.Data.SqlClient.SqlParameter();
                seq2.ParameterName = "@seq2";

                System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                sp3.ParameterName = "@transid";

                System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                sp4.ParameterName = "@wkno";

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(@" 
        and f.poid = @sp1 ");
                    sp1.Value = sp;
                    cmds.Add(sp1);
                }

                seq1.Value = this.txtSeq.seq1;
                seq2.Value = this.txtSeq.seq2;
                cmds.Add(seq1);
                cmds.Add(seq2);
                if (!this.txtSeq.checkSeq1Empty())
                {
                    strSQLCmd.Append(@"
        and f.seq1 = @seq1");
                }

                if (!this.txtSeq.checkSeq2Empty())
                {
                    strSQLCmd.Append(@" 
        and f.seq2 = @seq2");
                }

                if (!MyUtility.Check.Empty(transid))
                {
                    strSQLCmd.Append(@" 
        and a.id = @transid ");
                    sp3.Value = transid;
                    cmds.Add(sp3);
                }

                if (!MyUtility.Check.Empty(wkno))
                {
                    strSQLCmd.Append(@" 
        and a.exportid = @wkno ");
                    sp4.Value = wkno;
                    cmds.Add(sp4);
                }

                this.ShowWaitMessage("Data Loading....");
                DualResult result;
                if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out this.dtScrap))
                {
                    if (this.dtScrap.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found!!");
                    }
                    else
                    {
                        this.dtScrap.DefaultView.Sort = "poid,seq1,seq2,location,dyelot";
                    }

                    this.listControlBindingSource1.DataSource = this.dtScrap;

                    this.gridImport.Columns["exportid"].Visible = !(MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(wkno));
                    this.gridImport.Columns["eta"].Visible = !(MyUtility.Check.Empty(transid) && MyUtility.Check.Empty(wkno));
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.HideWaitMessage();
            }
        }

        // Form Load
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region -- Transfer Qty Valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["selected"] = true;
                    }
                };
            #endregion

            DataGridViewGeneratorTextColumnSettings ns2 = new DataGridViewGeneratorTextColumnSettings();
            ns2.CellFormatting = (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                switch (dr["StockType"].ToString())
                {
                    case "B":
                        e.Value = "Bulk";
                        break;
                    case "I":
                        e.Value = "Inventory";
                        break;
                    case "O":
                        e.Value = "Scrap";
                        break;
                }
            };

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;

            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (this.gridImport.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("exportid", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 1
                .Text("eta", header: "ETA", iseditingreadonly: true, width: Widths.AnsiChars(10)) // 2
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 3
                .Text("seq", header: "SEQ", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 4
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 6
                .Text("stocktype", header: "stocktype", iseditingreadonly: true, width: Widths.AnsiChars(6), settings: ns2) // 7
                .Numeric("balance", header: "Balance" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 8
                .Numeric("Qty", header: "Return" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 9
                .Text("location", header: "Location", width: Widths.AnsiChars(30), iseditingreadonly: true) // 10
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 11
               ;

            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Close
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            string remark = string.Empty;
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = dtGridBS1.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Scrap Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format(
                    @"FtyinventoryUkey = {0} ",
                    tmp["FtyInventoryUkey"]));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }

                if (!remark.Contains(tmp["exportid"].ToString()))
                {
                    remark += tmp["exportid"].ToString() + ",";
                }
            }

            this.dr_master["remark"] = remark;
            this.Close();
        }
    }
}
