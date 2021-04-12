using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P24_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtScrap = new DataTable();

        /// <inheritdoc/>
        public P24_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();
            string deadlind1 = string.Empty, deadlind2 = string.Empty;
            if (!MyUtility.Check.Empty(this.dateDead.TextBox1.Value))
            {
                deadlind1 = this.dateDead.TextBox1.Text;
                deadlind2 = this.dateDead.TextBox2.Text;
            }

            if (string.IsNullOrWhiteSpace(sp) && MyUtility.Check.Empty(deadlind1) && MyUtility.Check.Empty(deadlind2))
            {
                MyUtility.Msg.WarningBox("< SP# >&&<deadlind> can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                #region -- Sql Command --
                bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
                string where = string.Empty;
                if (!mtlAutoLock)
                {
                    where = " AND c.lock = 0 ";
                }

                strSQLCmd.Append(string.Format(
                    @"
select 	selected = 0   
		, id = '' 
		, FromFtyinventoryUkey = c.ukey
		, fromPoId = a.id
		, fromseq1 = a.Seq1
		, fromseq2 = a.Seq2 
        , fromFactoryID = orders.FactoryID
		, fromseq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2)
		, Description = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0)
        , i.Deadline as Deadline
		, fromRoll = c.Roll
		, fromDyelot = c.Dyelot 
		, fromStocktype = c.StockType 
		, balance = c.inqty - c.outqty + c.adjustqty - c.ReturnQty
		, qty = 0.00 
		, FabricType  = Case a.FabricType WHEN 'F' THEN 'Fabric' WHEN 'A' THEN 'Accessory' ELSE 'Other'  END 
		, a.stockunit
		, a.InputQty
		, topoid = a.id 
		, toseq1 = a.SEQ1 
		, toseq2 = a.SEQ2 
		, toroll = c.Roll 
		, todyelot = c.Dyelot 
        , toFactoryID = orders.FactoryID
		, toStocktype = 'O' 
		, Fromlocation = dbo.Getlocation(c.ukey)
        , ToLocation = stuff((select distinct isnull(CONCAT(',' , s.Data), '')
			                 from (	
				                select s.Data
				                from [dbo].[SplitString](dbo.Getlocation(c.ukey), ',') s
				                inner join MtlLocation m on s.Data = m.ID
				                where StockType = 'O'
				                and junk = 0
			                ) s
			                for xml path(''))
		                , 1, 1, '')
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join Orders on c.Poid = orders.id
inner join Factory on orders.FactoryID = factory.id
outer apply(select  Deadline = max(i.Deadline) from Inventory  i where i.POID = a.id and i.seq1 = a.seq1 and i.seq2 = a.seq2)i
Where   1=1
{1}
        and c.InQty - c.OutQty + c.AdjustQty - c.ReturnQty > 0 
        and c.stocktype = 'I'
        and factory.MDivisionID = '{0}'", Env.User.Keyword, where));
                #endregion

                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                sp1.ParameterName = "@sp1";

                System.Data.SqlClient.SqlParameter seq1 = new System.Data.SqlClient.SqlParameter();
                seq1.ParameterName = "@seq1";

                System.Data.SqlClient.SqlParameter seq2 = new System.Data.SqlClient.SqlParameter();
                seq2.ParameterName = "@seq2";

                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

                if (!MyUtility.Check.Empty(sp))
                {
                    strSQLCmd.Append(@" 
        and a.id = @sp1 ");
                    sp1.Value = sp;
                    cmds.Add(sp1);
                }

                if (!MyUtility.Check.Empty(this.dateDead.TextBox1.Value))
                {
                    strSQLCmd.Append(@" 
        and i.Deadline between @deadlind1 and @deadlind2");
                    cmds.Add(new System.Data.SqlClient.SqlParameter("@deadlind1", deadlind1));
                    cmds.Add(new System.Data.SqlClient.SqlParameter("@deadlind2", deadlind2));
                }

                seq1.Value = this.txtSeq.Seq1;
                seq2.Value = this.txtSeq.Seq2;
                cmds.Add(seq1);
                cmds.Add(seq2);
                if (!this.txtSeq.CheckSeq1Empty())
                {
                    strSQLCmd.Append(@" 
        and a.seq1 = @seq1 ");
                }

                if (!this.txtSeq.CheckSeq2Empty())
                {
                    strSQLCmd.Append(@" 
        and a.seq2 = @seq2");
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
                        this.dtScrap.DefaultView.Sort = "fromseq1,fromseq2,fromlocation,fromdyelot";
                    }

                    this.listControlBindingSource1.DataSource = this.dtScrap;
                }
                else
                {
                    this.ShowErr(strSQLCmd.ToString(), result);
                }

                this.HideWaitMessage();
            }
        }

        // Form Load

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region -- Transfer Qty Valid --
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex())["selected"] = true;
                        this.Sum_checkedqty();
                    }
                };
            #endregion

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
                    this.Sum_checkedqty();
                }
            };
            #region -- Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = this.gridImport.GetDataRow(this.gridImport.GetSelectedRowIndex());
                    Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(currentrow["ToStocktype"].ToString(), currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    currentrow["ToLocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", dr["ToStocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["ToLocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }

                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            #endregion Location 右鍵開窗
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("frompoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 1
                .Text("fromseq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 2
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 5
                .Text("Deadline", header: "Dead Line", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 6
                .Numeric("balance", header: "Inventory" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 7
                .Numeric("Qty", header: "Scrap" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 8
                .Text("fromlocation", header: "From Location", width: Widths.AnsiChars(20), iseditingreadonly: true) // 9
                .Text("tolocation", header: "To Location", iseditingreadonly: false, settings: ts2).Get(out col_tolocation)
               ;

            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
        }

        // Close
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
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
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["fromftyinventoryukey"].EqualString(tmp["fromftyinventoryukey"])
                                       && row["topoid"].EqualString(tmp["topoid"].ToString()) && row["toseq1"].EqualString(tmp["toseq1"])
                                       && row["toseq2"].EqualString(tmp["toseq2"].ToString()) && row["toroll"].EqualString(tmp["toroll"])
                                       && row["todyelot"].EqualString(tmp["todyelot"]) && row["tostocktype"].EqualString(tmp["tostocktype"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["Tolocation"] = tmp["Tolocation"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }

        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr2 in ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1"))
            {
                if (dr2["selected"].ToString() == "1")
                {
                    dr2["tolocation"] = this.txtLocation.Text;
                }
            }
        }

        private void TxtLocation_MouseDown(object sender, MouseEventArgs e)
        {
            Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("O", string.Empty);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtLocation.Text = item.GetSelectedString();
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            this.displayTotal.Value = localPrice.ToString();
        }
    }
}
