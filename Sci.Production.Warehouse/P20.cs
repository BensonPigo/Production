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
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //data.Tables.Add("dtSummary");

            //設定Grid1的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid1);
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                 .Text("factoryid", header: "Factory", width: Widths.AnsiChars(6))
                 .Text("projectid", header: "Project ID", width: Widths.AnsiChars(8))
                 .Text("refno", header: "Refno", width: Widths.AnsiChars(18))
                 .Numeric("InputQty_unit", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("OutputQty_unit", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Qty_unit", header: "Balance", width: Widths.AnsiChars(10), integer_places: 6, decimal_places: 2)
                 .Text("Brandid", header: "Brand", width: Widths.AnsiChars(10))
                 .Text("FabricType", header: "Fabric Type", width: Widths.AnsiChars(10))
                 .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10))
                 .Date("eta",header:"ETA")
                 .Date("Deadline", header: "Deadline")
                 .Text("scirefno", header: "SCI Refno#", width: Widths.AnsiChars(20))
                 .Text("POunit", header: "PO Unit", width: Widths.AnsiChars(10))
                 .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(10))
                 ;

            //設定Grid2的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid2);
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = bindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
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
                    var dr = this.grid3.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Transaction(dr,true);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            //設定Grid3的顯示欄位
            MyUtility.Tool.SetGridFrozen(grid3);
            this.grid3.IsEditingReadOnly = true;
            this.grid3.DataSource = bindingSource3;
            Helper.Controls.Grid.Generator(this.grid3)
                 .Text("roll", header: "Roll#", width: Widths.AnsiChars(9))
                 .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(5))
                 .Numeric("inqty", header: "Arrived Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("outQty", header: "Released Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2,settings:ts6,iseditingreadonly:true)
                 ;
        }

        //Close
        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //Query
        private void button2_Click(object sender, EventArgs e)
        {
            //搜尋前先清空資料
            bindingSource1.DataSource = null;
            bindingSource2.DataSource = null;
            bindingSource3.DataSource = null;

            string spno, seq1, seq2;
            spno = textBox1.Text;
            seq1 = textBox2.Text;
            seq2 = textBox3.Text;
            if (MyUtility.Check.Empty(spno))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("It will take a lot of time for searching data if condition of SP# is empty, Do you continue?");
                if (dResult == DialogResult.No) return;
            }
            
            #region -- SQL Command --
            StringBuilder sqlcmd = new StringBuilder();
            sqlcmd.Append(@"select i.poid,i.seq1,i.seq2,i.BrandID,i.InputQty,i.OutputQty,i.Qty
,case b.FabricType when 'F' then 'Fabric' when 'A' then 'Accessory' else 'Other' end as fabrictype
,i.FactoryID,i.ETA
,i.MtlTypeID,i.ProjectID,i.Deadline,i.Refno,i.Ukey,I.SCIRefno,I.UnitID POUNIT,DBO.getStockUnit(b.SCIRefno,s.suppid) AS STOCKUNIT
,ISNULL((SELECT cast(V.Rate as numeric) FROM dbo.View_Unitrate V WHERE V.FROM_U=I.UnitID AND V.TO_U = DBO.getStockUnit(I.SCIRefno,I.suppid)),1.0) RATE
 from inventory i 
inner join factory f on i.FactoryID = f.ID 
left join dbo.PO_Supp_Detail b on i.PoID= b.id and i.Seq1 = b.SEQ1 and i.Seq2 = b.SEQ2
left join dbo.PO_Supp as s on s.ID = b.ID and s.Seq1 = b.SEQ1
where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(" and i.poid = @spno");
            if (!MyUtility.Check.Empty(seq1))
                sqlcmd.Append(" and i.seq1 = @seq1");
            if (!MyUtility.Check.Empty(seq2))
                sqlcmd.Append(" and i.seq2 = @seq2");
            sqlcmd.Append(Environment.NewLine);
            sqlcmd.Append(@";select poid,seq1,seq2,id,ConfirmDate,[type],qty,seq70
,sum(tmp.qty) over (partition by InventoryUkey 
                             order by InventoryUkey,ConfirmDate,ID
			rows between unbounded preceding and current row) as running_total
,FactoryID,TransferFactory
,ConfirmHandle+' : '+(select a.Name+'#'+a.ExtNo from dbo.TPEPASS1 a where a.id=ConfirmHandle) as ConfirmHandle
,Reasonid,Reason,InventoryUkey
from 
(
select i.poid,i.seq1,i.Seq2,t.id
,CHOOSE(T.TYPE,'1-INPUT','2-OUTOUT','3-TRANSFER-OUT','4-ADJUST','5-OBSOLETS','6-RETURN') AS TYPE
,T.ConfirmDate
,choose(T.TYPE,T.qty,0-t.qty,0-t.qty,t.qty,0-t.qty,t.qty) qty
,t.seq70poid+t.InventorySeq2+t.seq70seq2 as seq70
,t.FactoryID,TransferFactory
,T.ConfirmHandle
,t.ReasonID, (select InvtransReason.ReasonEN from InvtransReason where id=t.ReasonID) Reason
,T.InventoryUkey
,t.TransferUkey
 from inventory i inner join factory f on i.FactoryID = f.ID 
 inner join invtrans t on t.InventoryUkey = i.Ukey
 where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(" and i.poid = @spno");
            if (!MyUtility.Check.Empty(seq1))
                sqlcmd.Append(" and i.seq1 = @seq1");
            if (!MyUtility.Check.Empty(seq2))
                sqlcmd.Append(" and i.seq2 = @seq2 ");
            sqlcmd.Append(Environment.NewLine);
            sqlcmd.Append(@"union all
 select i.poid,i.seq1,i.Seq2,t.id
,'3-TRANSFER-IN' AS TYPE
,T.ConfirmDate
,t.qty
,t.seq70poid+t.InventorySeq2+t.seq70seq2 as seq70
,t.FactoryID,TransferFactory
,T.ConfirmHandle
,t.ReasonID, (select InvtransReason.ReasonEN from InvtransReason where id=t.ReasonID) name
,t.TransferUkey
,T.InventoryUkey
 from inventory i inner join factory f on i.FactoryID = f.ID 
 inner join invtrans t on T.TransferUkey = I.Ukey and t.type=3
 where f.Junk = 0 ");
            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(" and i.poid = @spno");
            if (!MyUtility.Check.Empty(seq1))
                sqlcmd.Append(" and i.seq1 = @seq1");
            if (!MyUtility.Check.Empty(seq2))
                sqlcmd.Append(" and i.seq2 = @seq2 ");

            sqlcmd.Append(@" ) tmp
 order by InventoryUkey,ConfirmDate,ID");
            sqlcmd.Append(Environment.NewLine);
            sqlcmd.Append(@";select f.poid as id,f.Poid,f.Seq1,f.seq2,f.Roll,f.Dyelot,f.InQty,f.OutQty,f.AdjustQty
 ,isnull((select t.MtlLocationID+',' as MtlLocationID from (select fd.MtlLocationID from FtyInventory_Detail fd where fd.Ukey = f.Ukey)t for xml path('')),'')  Location
 from FtyInventory f
 where stocktype='I'");
            if (!MyUtility.Check.Empty(spno))
                sqlcmd.Append(" and f.poid = @spno");
            if (!MyUtility.Check.Empty(seq1))
                sqlcmd.Append(" and f.seq1 = @seq1");
            if (!MyUtility.Check.Empty(seq2))
                sqlcmd.Append(" and f.seq2 = @seq2");

            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@spno";
            sp1.Value = spno;

            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            sp2.ParameterName = "@seq1";
            sp2.Value = seq1;

            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp3.ParameterName = "@seq2";
            sp3.Value = seq2;

            IList<System.Data.SqlClient.SqlParameter> paras = new List<System.Data.SqlClient.SqlParameter>();
            paras.Add(sp1);
            paras.Add(sp2);
            paras.Add(sp3);
            #endregion

            DataSet data;
            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                MyUtility.Msg.WaitWindows("Data Loading....");
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
                MyUtility.Msg.WaitClear();
            }
            
            dtTpeIventory = data.Tables[0];
            dtInvtrans = data.Tables[1];
            dtFtyInventory = data.Tables[2];

            if (dtTpeIventory.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return;
            }

            dtTpeIventory.Columns.Add("InputQty_unit", typeof(decimal), "InputQty * rate");
            dtTpeIventory.Columns.Add("OutputQty_unit", typeof(decimal), "OutputQty * rate");
            dtTpeIventory.Columns.Add("Qty_unit", typeof(decimal), "Qty * rate");

            dtTpeIventory.TableName = "dtTpeIventory";
            dtInvtrans.TableName = "dtInvtrans";
            dtFtyInventory.Columns.Add("balance", typeof(decimal),"inqty-outqty+adjustqty");
            
            DataRelation relation = new DataRelation("rel1"
                , new DataColumn[] { dtTpeIventory.Columns["Ukey"] }
                , new DataColumn[] { dtInvtrans.Columns["InventoryUkey"] }
                );
            data.Relations.Add(relation);

            dtInvtrans.Columns.Add("qty_unit", typeof(decimal), "qty * parent.rate");
            dtInvtrans.Columns.Add("running_total_unit", typeof(decimal), "running_total * parent.rate");

            bindingSource1.DataSource = data;
            bindingSource1.DataMember = "dtTpeIventory";
            bindingSource2.DataSource = bindingSource1;
            bindingSource2.DataMember = "rel1";

            bindingSource3.DataSource = dtFtyInventory;
            
            if (checkBox1.Checked)
            {
                bindingSource1.Filter = "qty > 0";
            }
            else
            {
                bindingSource1.Filter = "";
            }
            Grid3Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
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
            DataRow tmp = grid1.GetDataRow(bindingSource1.Position);
            if (MyUtility.Check.Empty(tmp)) return;
            dtFtyInventory.DefaultView.RowFilter = string.Format("poid = '{0}' and seq1='{1}' and seq2='{2}'", tmp["poid"], tmp["seq1"], tmp["seq2"]);
        }
    }
}
