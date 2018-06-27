using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P20 : Sci.Win.Tems.QueryForm
    {
        DataSet data = new DataSet();
        DataTable dtTpeIventory , dtInvtrans , dtFtyInventory;
        public P20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.ActiveControl = txtSPNo;

            string sqlCmd = @"select '' ID union Select Distinct ID from Factory where junk = 0";
            DataTable Factory;
            DBProxy.Current.Select(null, sqlCmd, out Factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, Factory);
            comboFactory.Text = "";
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //data.Tables.Add("dtSummary");

            //設定Grid1的顯示欄位
            this.gridStockList.IsEditingReadOnly = true;
            this.gridStockList.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.gridStockList)
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
                 .Date("eta",header:"ETA")
                 .Date("Deadline", header: "Deadline")
                 .Text("scirefno", header: "SCI Refno#", width: Widths.AnsiChars(20))                               
                 ;

            //設定Grid2的顯示欄位
            this.gridTransactionID.IsEditingReadOnly = true;
            this.gridTransactionID.DataSource = bindingSource2;
            Helper.Controls.Grid.Generator(this.gridTransactionID)
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
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts6 = new DataGridViewGeneratorNumericColumnSettings();
            ts6.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.gridRoll.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Transaction(dr,true);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            //設定Grid3的顯示欄位
            this.gridRoll.IsEditingReadOnly = true;
            this.gridRoll.DataSource = bindingSource3;
            Helper.Controls.Grid.Generator(this.gridRoll)
                 .Text("roll", header: "Roll#", width: Widths.AnsiChars(9))
                 .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(5))
                 .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2,settings:ts6,iseditingreadonly:true)
                 ;
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            this.Close();
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //搜尋前先清空資料
            bindingSource1.DataSource = null;
            bindingSource2.DataSource = null;
            bindingSource3.DataSource = null;

            string spno;
            spno = txtSPNo.Text;
            if (MyUtility.Check.Empty(spno))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("It will take a lot of time for searching data if condition of SP# is empty, Do you continue?");
                if (dResult == DialogResult.No) return;
            }

            string Refno;
            Refno = txtRefNo.Text;
            string ColorID;
            ColorID = txtColorID.Text;
            string factory;
            factory = comboFactory.Text;

            #region -- SQL Command --
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"
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
        , b.ColorID
        , i.Ukey
        , I.SCIRefno
        , I.UnitID POUNIT
        , DBO.getStockUnit(b.SCIRefno,s.suppid) AS STOCKUNIT
        , dbo.GetUnitRate(I.UnitID, DBO.getStockUnit(I.SCIRefno,I.suppid)) RATE
from inventory i WITH (NOLOCK) 
inner join factory f WITH (NOLOCK) on i.FactoryID = f.ID 
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
left join dbo.PO_Supp as s WITH (NOLOCK) on s.ID = b.ID and s.Seq1 = b.SEQ1
where f.Junk = 0 ");

            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(" and i.poid = @spno");
            if (!txtSeq.checkSeq1Empty())
                sqlcmd.Append(" and i.seq1 = @seq1");
            if (!txtSeq.checkSeq2Empty())
                sqlcmd.Append(" and i.seq2 = @seq2");
            if (!MyUtility.Check.Empty(Refno))
                sqlcmd.Append(" and i.Refno = @Refno");
            if (!MyUtility.Check.Empty(ColorID))
                sqlcmd.Append(" and b.ColorID = @ColorID");
            if (!MyUtility.Check.Empty(factory))
                sqlcmd.Append(" and i.FactoryID = @factory");

            sqlcmd.Append(Environment.NewLine);
            sqlcmd.Append(@";
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
from (
    select  i.poid
            , i.seq1
            , i.Seq2
            , t.id
            , TYPE = CHOOSE(T.TYPE,'1-INPUT','2-OUTOUT','3-TRANSFER-OUT','4-ADJUST','5-OBSOLETS','6-RETURN')
            , T.ConfirmDate
            , qty = choose(T.TYPE,T.qty,0-t.qty,0-t.qty,t.qty,0-t.qty,t.qty)
            , seq70 = t.seq70poid+t.InventorySeq2+t.seq70seq2
            , t.FactoryID
            , TransferFactory
            , T.ConfirmHandle
            , t.ReasonID
            , Reason = (select InvtransReason.ReasonEN from InvtransReason WITH (NOLOCK) where id=t.ReasonID) 
            , T.InventoryUkey
            , t.TransferUkey
            , i.ProjectID
    from inventory i WITH (NOLOCK) 
    inner join factory f WITH (NOLOCK) on i.FactoryID = f.ID 
    inner join invtrans t WITH (NOLOCK) on t.InventoryUkey = i.Ukey
    left join dbo.PO_Supp_Detail b WITH (NOLOCK) on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
    where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(@" 
        and i.poid = @spno");
            if (!txtSeq.checkSeq1Empty())
                sqlcmd.Append(@" 
        and i.seq1 = @seq1");
            if (!txtSeq.checkSeq2Empty())
                sqlcmd.Append(@" 
        and i.seq2 = @seq2 ");
            if (!MyUtility.Check.Empty(Refno))
                sqlcmd.Append(" and i.Refno = @Refno");
            if (!MyUtility.Check.Empty(ColorID))
                sqlcmd.Append(" and b.ColorID = @ColorID");
            if (!MyUtility.Check.Empty(factory))
                sqlcmd.Append(" and i.FactoryID = @factory");

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
            , t.seq70poid+t.InventorySeq2+t.seq70seq2 as seq70
            , t.FactoryID
            , TransferFactory
            , T.ConfirmHandle
            , t.ReasonID
            , (select InvtransReason.ReasonEN from InvtransReason WITH (NOLOCK) where id=t.ReasonID) name
            , t.TransferUkey
            , T.InventoryUkey
            , i.ProjectID
    from inventory i WITH (NOLOCK) 
    inner join factory f WITH (NOLOCK) on i.FactoryID = f.ID 
    inner join invtrans t WITH (NOLOCK) on T.TransferUkey = I.Ukey and t.type=3
    left join dbo.PO_Supp_Detail b WITH (NOLOCK) on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
    where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(@" 
        and i.poid = @spno");
            if (!txtSeq.checkSeq1Empty())
                sqlcmd.Append(@" 
        and i.seq1 = @seq1");
            if (!txtSeq.checkSeq2Empty())
                sqlcmd.Append(@" 
        and i.seq2 = @seq2 ");
            if (!MyUtility.Check.Empty(Refno))
                sqlcmd.Append(" and i.Refno = @Refno");
            if (!MyUtility.Check.Empty(ColorID))
                sqlcmd.Append(" and b.ColorID = @ColorID");
            if (!MyUtility.Check.Empty(factory))
                sqlcmd.Append(" and i.FactoryID = @factory");

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
                sqlcmd.Append(@" 
        and f.poid = @spno");
            if (!txtSeq.checkSeq1Empty())
                sqlcmd.Append(@" 
        and f.seq1 = @seq1");
            if (!txtSeq.checkSeq2Empty())
                sqlcmd.Append(@" 
        and f.seq2 = @seq2");


            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@spno";
            sp1.Value = spno;

            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            sp2.ParameterName = "@seq1";
            sp2.Value = txtSeq.seq1;

            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp3.ParameterName = "@seq2";
            sp3.Value = txtSeq.seq2;

            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp4.ParameterName = "@Refno";
            sp4.Value = Refno;

            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
            sp5.ParameterName = "@ColorID";
            sp5.Value = ColorID;
            
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
                if (!SQL.Selects("", sqlcmd.ToString(), out data, paras))
                {
                    ShowErr(sqlcmd.ToString());
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
            dtTpeIventory = data.Tables[0];
            dtInvtrans = data.Tables[1];
            dtFtyInventory = data.Tables[2];

            if (dtTpeIventory.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return;
            }

            if (dtTpeIventory.Select("qty > 0", "").Count() == 0 && checkQty.Checked)
            {
                MyUtility.Msg.WarningBox("qty > 0 Data not found!!");
            }

            dtTpeIventory.Columns.Add("InputQty_unit", typeof(decimal), "Convert(InputQty * rate * 100, 'System.Int64') / 100.0 + iif(Convert(InputQty * rate * 1000, 'System.Int64') % 10 >= 5, 0.01, 0)");
            dtTpeIventory.Columns.Add("OutputQty_unit", typeof(decimal), "Convert(OutputQty * rate * 100, 'System.Int64') / 100.0 + iif(Convert(OutputQty * rate * 1000, 'System.Int64') % 10 >= 5, 0.01, 0)");
            dtTpeIventory.Columns.Add("Qty_unit", typeof(decimal), "Convert(Qty * rate * 100, 'System.Int64') / 100.0 + iif(Convert(Qty * rate * 1000, 'System.Int64') % 10 >= 5, 0.01, 0)");
            //dtTpeIventory.Columns.Add("Balance", typeof(decimal), "InputQty - OutputQty");
            dtTpeIventory.DefaultView.Sort="POID,SEQ1,SEQ2";
            dtTpeIventory.TableName = "dtTpeIventory";

            dtInvtrans.TableName = "dtInvtrans";
            dtFtyInventory.Columns.Add("balance", typeof(decimal), "InQty-outqty+adjustqty");

            DataRelation relation = new DataRelation("rel1"
                , new DataColumn[] { dtTpeIventory.Columns["PoID"], dtTpeIventory.Columns["Seq1"], dtTpeIventory.Columns["Seq2"], dtTpeIventory.Columns["ProjectID"] }
                , new DataColumn[] { dtInvtrans.Columns["PoID"], dtInvtrans.Columns["Seq1"], dtInvtrans.Columns["Seq2"], dtInvtrans.Columns["ProjectID"] }
                );
            data.Relations.Add(relation);
            
            dtInvtrans.Columns.Add("qty_unit", typeof(decimal), "Convert(Qty * parent.rate * 100, 'System.Int64') / 100.0 + iif(Convert(Qty * parent.rate * 1000, 'System.Int64') % 10 >= 5, 0.01, 0)");
            dtInvtrans.Columns.Add("running_total_unit", typeof(decimal), "Convert(running_total * parent.rate * 100, 'System.Int64') / 100.0 + iif(Convert(running_total * parent.rate * 1000, 'System.Int64') % 10 >= 5, 0.01, 0)");

            bindingSource1.DataSource = data;
            bindingSource1.DataMember = "dtTpeIventory";
            bindingSource2.DataSource = bindingSource1;
            bindingSource2.DataMember = "rel1";

            bindingSource3.DataSource = dtFtyInventory;
            
            if (checkQty.Checked)
            {
                bindingSource1.Filter = "qty > 0";
            }
            else
            {
                bindingSource1.Filter = "";
            }
            Grid3Refresh();
        }

        private void checkQty_CheckedChanged(object sender, EventArgs e)
        {
            if (checkQty.Checked)
            {
                bindingSource1.Filter = "qty > 0";
            }
            else
            {
                bindingSource1.Filter = "";
            }
            Grid3Refresh();
        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            
            Grid3Refresh();
        }

        private void Grid3Refresh()
        {
            if (dtFtyInventory==null) return;
            dtFtyInventory.DefaultView.RowFilter = string.Format("1=0");
            if (-1 == bindingSource1.Position) return;
            DataRow tmp = gridStockList.GetDataRow(bindingSource1.Position);
            if (MyUtility.Check.Empty(tmp)) return;
            dtFtyInventory.DefaultView.RowFilter = string.Format("poid = '{0}' and seq1='{1}' and seq2='{2}'", tmp["poid"], tmp["seq1"], tmp["seq2"]);
        }
    }
}
