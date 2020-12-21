using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Ict;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P20 : Win.Tems.QueryForm
    {
        private DataSet data = new DataSet();
        private DataTable dtTpeIventory;
        private DataTable dtInvtrans;
        private DataTable dtFtyInventory;

        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.ActiveControl = this.txtSPNo;

            string sqlCmd = @"select '' ID union Select Distinct ID from Factory where junk = 0";
            DataTable factory;
            DBProxy.Current.Select(null, sqlCmd, out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = string.Empty;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // data.Tables.Add("dtSummary");

            // 設定Grid1的顯示欄位
            this.gridStockList.IsEditingReadOnly = true;
            this.gridStockList.DataSource = this.bindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridStockList)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Text("factoryid", header: "Factory", width: Widths.AnsiChars(6))
                 .Text("Brandid", header: "Brand", width: Widths.AnsiChars(10))
                 .Text("projectid", header: "Project ID", width: Widths.AnsiChars(8))
                 .Text("refno", header: "Refno", width: Widths.AnsiChars(18))
                 .Text("ColorID", header: "ColorID", width: Widths.AnsiChars(6))
                 .Text("POunit", header: "PO Unit", width: Widths.AnsiChars(10))
                 .Numeric("InputQty_unit", header: "Input Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutputQty_unit", header: "Output Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Qty_unit", header: "Balance", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                 .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(10))
                 .Text("FabricType", header: "Fabric Type", width: Widths.AnsiChars(10))
                 .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10))
                 .Date("eta", header: "ETA")
                 .Date("Deadline", header: "Deadline")
                 .Text("scirefno", header: "SCI Refno#", width: Widths.AnsiChars(20));

            // 設定Grid2的顯示欄位
            this.gridTransactionID.IsEditingReadOnly = true;
            this.gridTransactionID.DataSource = this.bindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridTransactionID)
                 .Text("id", header: "Transaction ID", width: Widths.AnsiChars(13))
                 .Text("type", header: "Type", width: Widths.AnsiChars(15))
                 .Date("ConfirmDate", header: "CFM Date", width: Widths.AnsiChars(10))
                 .Numeric("qty_unit", header: "Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("running_total_unit", header: "Running Total", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("seq70", header: "Used For SP#", width: Widths.AnsiChars(20))
                 .Text("Factoryid", header: "From FTY", width: Widths.AnsiChars(5))
                 .Text("TransferFactory", header: "To FTY", width: Widths.AnsiChars(5))
                 .Text("ConfirmHandle", header: "Handle", width: Widths.AnsiChars(15))
                 .Text("Reason", header: "Reason", width: Widths.AnsiChars(40))
                 ;
            #region -- Balance Qty 開窗 --
            DataGridViewGeneratorNumericColumnSettings ts6 = new DataGridViewGeneratorNumericColumnSettings();
            ts6.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.gridRoll.GetDataRow<DataRow>(e.RowIndex);
                    if (dr == null)
                    {
                        return;
                    }

                    var frm = new P03_Transaction(dr, true);
                    frm.ShowDialog(this);
                }
            };
            #endregion

            // 設定Grid3的顯示欄位
            this.gridRoll.IsEditingReadOnly = true;
            this.gridRoll.DataSource = this.bindingSource3;
            this.Helper.Controls.Grid.Generator(this.gridRoll)
                 .Text("roll", header: "Roll#", width: Widths.AnsiChars(9))
                 .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                 .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, settings: ts6, iseditingreadonly: true)
                 ;
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            // this.Dispose();
            this.Close();
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            // 搜尋前先清空資料
            this.bindingSource1.DataSource = null;
            this.bindingSource2.DataSource = null;
            this.bindingSource3.DataSource = null;

            string spno;
            spno = this.txtSPNo.Text;
            if (MyUtility.Check.Empty(spno))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("It will take a lot of time for searching data if condition of SP# is empty, Do you continue?");
                if (dResult == DialogResult.No)
                {
                    return;
                }
            }

            string refno;
            refno = this.txtRefNo.Text;
            string colorID;
            colorID = this.txtColorID.Text;
            string factory;
            factory = this.comboFactory.Text;

            #region -- SQL Command --
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
select i.*
	,[InputQty_unit] = cast(InputQty * rate as decimal(18,2))
	,[OutputQty_unit] = cast(OutputQty * rate as decimal(18,2))
	,[Qty_unit] = cast(Qty * rate as decimal(18,2))
into #tmp_TpeIventory
from 
(
	select  i.poid
			, i.seq1
			, i.seq2
			, i.BrandID
			, i.InputQty
			, i.OutputQty
			, i.Qty
			, case b.FabricType when 'F' then 'Fabric' when 'A' then 'Accessory' else 'Other' end as fabrictype
			, i.FactoryID
			, i.ETA
			, i.MtlTypeID
			, i.ProjectID
			, i.Deadline
			, i.Refno
			, [ColorID]=IIF(Fabric.MtlTypeID LIKE '%Thread%' ,b.SuppColor , b.ColorID)
			, i.Ukey
			, I.SCIRefno
			, I.UnitID POUNIT
			, [dbo].[GetStockUnitBySPSeq](i.PoID, i.Seq1, i.Seq2) AS STOCKUNIT
			, dbo.GetUnitRate(I.UnitID, [dbo].[GetStockUnitBySPSeq](i.PoID, i.Seq1, i.Seq2)) RATE	
	from inventory i WITH (NOLOCK) 
	inner join factory f WITH (NOLOCK) on i.FactoryID = f.ID 
	left join dbo.PO_Supp_Detail b WITH (NOLOCK) on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
	left join dbo.PO_Supp as s WITH (NOLOCK) on s.ID = b.ID and s.Seq1 = b.SEQ1
	left join fabric WITH (NOLOCK) on fabric.SCIRefno = b.scirefno
	where f.Junk = 0 ");

            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(" and i.poid = @spno");
            }

            if (!this.txtSeq.CheckSeq1Empty())
            {
                sqlcmd.Append(" and i.seq1 = @seq1");
            }

            if (!this.txtSeq.CheckSeq2Empty())
            {
                sqlcmd.Append(" and i.seq2 = @seq2");
            }

            if (!MyUtility.Check.Empty(refno))
            {
                sqlcmd.Append(" and i.Refno = @Refno");
            }

            if (!MyUtility.Check.Empty(colorID))
            {
                sqlcmd.Append(" and IIF(Fabric.MtlTypeID LIKE '%Thread%' ,b.SuppColor , b.ColorID) = @ColorID");
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append(" and i.FactoryID = @factory");
            }

            sqlcmd.Append(@"
)i; 
select * from #tmp_TpeIventory; ");
            sqlcmd.Append(@"
select  poid
        , seq1
        , seq2
        , id
        , ConfirmDate
        , [type]
        , qty
        , seq70
        , running_total = sum(tmp.qty) over (partition by InventoryUkey 
                                            order by InventoryUkey,ConfirmDate,ID
			                                rows between unbounded preceding and current row)  
        , FactoryID
        , TransferFactory
        , ConfirmHandle = ConfirmHandle+' : '+(select a.Name+'#'+a.ExtNo from dbo.TPEPASS1 a WITH (NOLOCK) where a.id=ConfirmHandle) 
        , Reasonid
        , Reason
        , InventoryUkey
        , ProjectID
		, [qty_unit] = cast(qty * RATE as decimal(18,2))
		, [running_total_unit] = cast(sum(tmp.qty) over (partition by InventoryUkey 
											order by InventoryUkey,ConfirmDate,ID
											rows between unbounded preceding and current row)  * RATE as decimal(18,2))
from (
    select  i.poid
            , i.seq1
            , i.Seq2
            , t.id
            , TYPE = case   when T.TYPE = '1' then '1-INPUT'
                            when T.TYPE = '2' then '2-OUTOUT'
                            when T.TYPE = '3' then '3-TRANSFER-OUT'
                            when T.TYPE = '4' then '4-ADJUST'
                            when T.TYPE = '5' then '5-OBSOLESCENCE'
                            when T.TYPE = '6' then '6-RETURN'
                            when T.TYPE = 'R' then 'R-Recover Inventory'
                            else '' end
            , T.ConfirmDate
            , qty = case    when T.TYPE = '1' then T.qty
                            when T.TYPE = '2' then 0-t.qty
                            when T.TYPE = '3' then 0-t.qty
                            when T.TYPE = '4' then t.qty
                            when T.TYPE = '5' then 0-t.qty
                            when T.TYPE = '6' then t.qty
                            when T.TYPE = 'R' then t.qty
                            else null end
            , seq70 = t.seq70poid + ' ' + t.seq70seq1 +'-' + t.seq70seq2
            , t.FactoryID
            , TransferFactory
            , T.ConfirmHandle
            , t.ReasonID
            , Reason = (select InvtransReason.ReasonEN from InvtransReason WITH (NOLOCK) where id=t.ReasonID) 
            , T.InventoryUkey
            , t.TransferUkey
            , i.ProjectID
            , tp.RATE
    from inventory i WITH (NOLOCK) 
    inner join factory f WITH (NOLOCK) on i.FactoryID = f.ID 
    inner join invtrans t WITH (NOLOCK) on t.InventoryUkey = i.Ukey
    inner join #tmp_TpeIventory tp on T.InventoryUkey = tp.Ukey
    left join dbo.PO_Supp_Detail b WITH (NOLOCK) on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
	left join fabric WITH (NOLOCK) on fabric.SCIRefno = b.scirefno
    where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(@" 
        and i.poid = @spno");
            }

            if (!this.txtSeq.CheckSeq1Empty())
            {
                sqlcmd.Append(@" 
        and i.seq1 = @seq1");
            }

            if (!this.txtSeq.CheckSeq2Empty())
            {
                sqlcmd.Append(@" 
        and i.seq2 = @seq2 ");
            }

            if (!MyUtility.Check.Empty(refno))
            {
                sqlcmd.Append(" and i.Refno = @Refno");
            }

            if (!MyUtility.Check.Empty(colorID))
            {
                sqlcmd.Append(" and IIF(Fabric.MtlTypeID LIKE '%Thread%' ,b.SuppColor , b.ColorID) = @ColorID");
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append(" and i.FactoryID = @factory");
            }

            sqlcmd.Append(Environment.NewLine);
            sqlcmd.Append(@"
    
    union all
    select  i.poid
            , i.seq1
            , i.Seq2
            , t.id
            , '3-TRANSFER-IN' AS TYPE
            , T.ConfirmDate
            , t.qty
            , t.seq70poid + ' ' +t.seq70seq1 + '-' + t.seq70seq2 as seq70
            , t.FactoryID
            , TransferFactory
            , T.ConfirmHandle
            , t.ReasonID
            , (select InvtransReason.ReasonEN from InvtransReason WITH (NOLOCK) where id=t.ReasonID) name
            , t.TransferUkey
            , T.InventoryUkey
            , i.ProjectID
            , tp.RATE
    from inventory i WITH (NOLOCK) 
    inner join factory f WITH (NOLOCK) on i.FactoryID = f.ID 
    inner join invtrans t WITH (NOLOCK) on T.TransferUkey = I.Ukey and t.type='3'
    inner join #tmp_TpeIventory tp on T.InventoryUkey = tp.Ukey
    left join dbo.PO_Supp_Detail b WITH (NOLOCK) on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
	left join fabric WITH (NOLOCK) on fabric.SCIRefno = b.scirefno
    where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(@" 
        and i.poid = @spno");
            }

            if (!this.txtSeq.CheckSeq1Empty())
            {
                sqlcmd.Append(@" 
        and i.seq1 = @seq1");
            }

            if (!this.txtSeq.CheckSeq2Empty())
            {
                sqlcmd.Append(@" 
        and i.seq2 = @seq2 ");
            }

            if (!MyUtility.Check.Empty(refno))
            {
                sqlcmd.Append(" and i.Refno = @Refno");
            }

            if (!MyUtility.Check.Empty(colorID))
            {
                sqlcmd.Append(" and IIF(Fabric.MtlTypeID LIKE '%Thread%' ,b.SuppColor , b.ColorID) = @ColorID");
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd.Append(" and i.FactoryID = @factory");
            }

            sqlcmd.Append(@" 
) tmp
order by InventoryUkey,ConfirmDate,ID");
            sqlcmd.Append(Environment.NewLine);
            sqlcmd.Append(@"
;select f.poid as id
        , f.Poid
        , f.Seq1
        , f.seq2
        , f.Roll
        , f.Dyelot
        , f.InQty
        , f.OutQty
        , f.AdjustQty
        , dbo.Getlocation(f.ukey)  Location
from FtyInventory f WITH (NOLOCK) 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on f.PoID= b.id and f.Seq1 = b.SEQ1 and f.Seq2 = b.SEQ2
where   stocktype='I'");
            if (!MyUtility.Check.Empty(spno))
            {
                sqlcmd.Append(@" 
        and f.poid = @spno");
            }

            if (!this.txtSeq.CheckSeq1Empty())
            {
                sqlcmd.Append(@" 
        and f.seq1 = @seq1");
            }

            if (!this.txtSeq.CheckSeq2Empty())
            {
                sqlcmd.Append(@" 
        and f.seq2 = @seq2");
            }

            sqlcmd.Append(Environment.NewLine + "drop table #tmp_TpeIventory;");

            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@spno";
            sp1.Value = spno;

            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            sp2.ParameterName = "@seq1";
            sp2.Value = this.txtSeq.Seq1;

            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp3.ParameterName = "@seq2";
            sp3.Value = this.txtSeq.Seq2;

            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp4.ParameterName = "@Refno";
            sp4.Value = refno;

            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
            sp5.ParameterName = "@ColorID";
            sp5.Value = colorID;

            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
            sp6.ParameterName = "@factory";
            sp6.Value = factory;

            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            paras.Add(sp1);
            paras.Add(sp2);
            paras.Add(sp3);
            paras.Add(sp4);
            paras.Add(sp5);
            paras.Add(sp6);
            #endregion
            this.ShowWaitMessage("Data Loading....");
            DataSet data;
            DBProxy.Current.DefaultTimeout = 3000;
            try
            {
                if (!SQL.Selects(string.Empty, sqlcmd.ToString(), out data, paras))
                {
                    this.ShowErr(sqlcmd.ToString());
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBProxy.Current.DefaultTimeout = 0;
            }

            this.HideWaitMessage();
            this.dtTpeIventory = data.Tables[0];
            this.dtInvtrans = data.Tables[1];
            this.dtFtyInventory = data.Tables[2];

            if (this.dtTpeIventory.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return;
            }

            if (this.dtTpeIventory.Select("qty > 0", string.Empty).Count() == 0 && this.checkQty.Checked)
            {
                MyUtility.Msg.WarningBox("qty > 0 Data not found!!");
            }

            this.dtTpeIventory.DefaultView.Sort = "POID,SEQ1,SEQ2";
            this.dtTpeIventory.TableName = "dtTpeIventory";

            this.dtInvtrans.TableName = "dtInvtrans";
            this.dtFtyInventory.Columns.Add("balance", typeof(decimal), "InQty-outqty+adjustqty");

            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { this.dtTpeIventory.Columns["Ukey"] },
                new DataColumn[] { this.dtInvtrans.Columns["InventoryUkey"] });
            data.Relations.Add(relation);

            this.bindingSource1.DataSource = data;
            this.bindingSource1.DataMember = "dtTpeIventory";
            this.bindingSource2.DataSource = this.bindingSource1;
            this.bindingSource2.DataMember = "rel1";

            this.bindingSource3.DataSource = this.dtFtyInventory;

            if (this.checkQty.Checked)
            {
                this.bindingSource1.Filter = "qty > 0";
            }
            else
            {
                this.bindingSource1.Filter = string.Empty;
            }

            this.Grid3Refresh();
        }

        private void CheckQty_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkQty.Checked)
            {
                this.bindingSource1.Filter = "qty > 0";
            }
            else
            {
                this.bindingSource1.Filter = string.Empty;
            }

            this.Grid3Refresh();
        }

        private void BindingSource1_PositionChanged(object sender, EventArgs e)
        {
            this.Grid3Refresh();
        }

        private void Grid3Refresh()
        {
            if (this.dtFtyInventory == null)
            {
                return;
            }

            this.dtFtyInventory.DefaultView.RowFilter = string.Format("1=0");
            if (this.bindingSource1.Position == -1)
            {
                return;
            }

            DataRow tmp = this.gridStockList.GetDataRow(this.bindingSource1.Position);
            if (MyUtility.Check.Empty(tmp))
            {
                return;
            }

            this.dtFtyInventory.DefaultView.RowFilter = string.Format("poid = '{0}' and seq1='{1}' and seq2='{2}'", tmp["poid"], tmp["seq1"], tmp["seq2"]);
        }
    }
}
