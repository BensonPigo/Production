using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P25_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtScrap;

        public P25_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.txtSPNo.Text = this.dr_master["id"].ToString();
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSPNo.Text.TrimEnd();

            if (string.IsNullOrWhiteSpace(sp))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                this.txtSPNo.Focus();
                return;
            }
            else
            {
                // 建立可以符合回傳的Cursor
                #region -- SQL Command --
                bool mtlAutoLock = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select MtlAutoLock from system"));
                string where = string.Empty;
                if (!mtlAutoLock)
                {
                    where = " AND c.lock = 0 ";
                }

                strSQLCmd.Append(string.Format(
                    @"
select  selected = 0 
        , id = '' 
        , FromFtyinventoryUkey = c.ukey 
        , fromPoId = a.id 
        , fromseq1 = a.Seq1 
        , fromseq2 = a.Seq2 
        , fromseq = concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) 
        , [Description] = dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) 
        , fromRoll = c.Roll 
        , fromDyelot = c.Dyelot 
        , fromFactoryID = orders.FactoryID
        , fromStocktype = c.StockType 
        , balance = c.inqty-c.outqty + c.adjustqty 
        , qty = 0.00 
        , FabricType =  case a.FabricType 
                            when 'f' then 'Fabric'
					        when 'a' then 'Accessory'
					        else 'Other'				  
		                end 
        , a.stockunit
        , a.InputQty
        , topoid = a.id 
        , toseq1 = a.SEQ1 
        , toseq2 = a.SEQ2 
        , toroll = c.Roll 
        , todyelot = c.Dyelot 
        , toFactoryID  = orders.FactoryID
        , toStocktype = 'O' 
        , Fromlocation = dbo.Getlocation(c.ukey)
from dbo.PO_Supp_Detail a WITH (NOLOCK) 
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.id and c.seq1 = a.seq1 and c.seq2  = a.seq2 
inner join Orders on c.Poid = Orders.id
inner join Factory on Orders.FactoryID = Factory.id
Where  1=1
{1}
        and c.InQty-c.OutQty+c.AdjustQty > 0 
        and c.stocktype = 'B'
        and Factory.MDivisionID = '{0}'", Env.User.Keyword, where));
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

                seq1.Value = this.txtSeq.Seq1;
                seq2.Value = this.txtSeq.Seq2;
                cmds.Add(seq1);
                cmds.Add(seq2);
                if (!this.txtSeq.CheckSeq1Empty())
                {
                    strSQLCmd.Append(@" 
        and a.seq1 = @seq1");
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
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("frompoid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14)) // 1
                .Text("fromseq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 2
                .Text("fromroll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("fromdyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 5
                .Text("stockunit", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 6
                .Numeric("balance", header: "Bulk" + Environment.NewLine + "Qty", iseditable: false, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6)) // 7
                .Numeric("Qty", header: "Scrap" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(6)) // 8
                .Text("fromlocation", header: "From Location", width: Widths.AnsiChars(30), iseditingreadonly: true) // 9
               ;

            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
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
    }
}
