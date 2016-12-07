using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production;
using Sci.Production.PublicPrg;
using Sci.Utility.Excel;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Transactions;

namespace Sci.Production.Warehouse
{
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {

            base.OnFormLoaded();
            comboBox1.SelectedIndex = 1;
            ChangeDetailColor();
            MyUtility.Tool.SetGridFrozen(this.grid1);

            #region Supp 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Supplier(dr);
                frm.ShowDialog(this);
            };
            #endregion

            #region refno 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Refno(dr);
                frm.ShowDialog(this);
            };
            #endregion

            #region Ship qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts3 = new DataGridViewGeneratorNumericColumnSettings();
            ts3.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Wkno(dr);
                frm.ShowDialog(this);
            };
            #endregion
            #region Taipei Stock Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts4 = new DataGridViewGeneratorNumericColumnSettings();
            ts4.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_TaipeiInventory(dr);
                frm.ShowDialog(this);
            };
            #endregion
            #region Released Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts5 = new DataGridViewGeneratorNumericColumnSettings();
            ts5.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_RollTransaction(dr);
                frm.ShowDialog(this);
            };
            #endregion
            #region Balance Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts6 = new DataGridViewGeneratorNumericColumnSettings();
            ts6.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Transaction(dr);
                DialogResult DR =  frm.ShowDialog(this);
                if (DR == DialogResult.OK) button1_Click(null,null);
            };
            #endregion
            #region Inventory Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts7 = new DataGridViewGeneratorNumericColumnSettings();
            ts7.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_InventoryStatus(dr);
                frm.ShowDialog(this);

            };
            #endregion
            #region Scrap Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts8 = new DataGridViewGeneratorNumericColumnSettings();
            ts8.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Scrap(dr);
                frm.ShowDialog(this);

            };
            #endregion
            #region Bulk Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts9 = new DataGridViewGeneratorTextColumnSettings();
            ts9.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr, "B");
                frm.ShowDialog(this);

            };
            #endregion
            #region Stock Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts11 = new DataGridViewGeneratorTextColumnSettings();
            ts11.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr, "I");
                frm.ShowDialog(this);

            };
            #endregion
            #region FIR 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts10 = new DataGridViewGeneratorTextColumnSettings();
            ts10.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_InspectionList(dr);
                frm.ShowDialog(this);

            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.grid1)
            .Text("id", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))  //0
            .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))  //1
            .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(4))  //2
            .Text("Suppid", header: "Supp", iseditingreadonly: true, width: Widths.AnsiChars(4), settings: ts1)  //3
            .Text("eta", header: "Sup. 1st " + Environment.NewLine + "Cfm ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //4
            .Text("RevisedETA", header: "Sup. Delivery" + Environment.NewLine + "Rvsd ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //5
            .Text("refno", header: "Ref#", iseditingreadonly: true, settings: ts2)  //6
            .Text("fabrictype2", header: "Fabric Type", iseditingreadonly: true)  //7
            .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(15))  //8
            .Text("ColorID", header: "Color", iseditingreadonly: true)  //9
            .Text("SizeSpec", header: "Size", iseditingreadonly: true)  //10
            .Text("CurrencyID", header: "Currency", iseditingreadonly: true)  //11
            .Numeric("unitqty", header: "@Qty", decimal_places: 4, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //12
            .Numeric("Qty", header: "Order Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //13
            .Numeric("NETQty", header: "Net Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //14
            .Numeric("useqty", header: "Use Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //15
            .Numeric("ShipQty", header: "Ship Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts3)    //16
            .Numeric("ShipFOC", header: "F.O.C", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //17
            .Numeric("ApQty", header: "AP Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //18
            .Numeric("InputQty", header: "Taipei Stock Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts4)    //19
            .Text("POUnit", header: "PO Unit", iseditingreadonly: true)  //20
            .Text("Complete", header: "Cmplt", iseditingreadonly: true)  //21
            .Date("FinalETA", header: "Act. ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //22
            .Text("OrderIdList", header: "Order List", iseditingreadonly: true)  //23
            .Numeric("InQty", header: "Arrived Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //24
            .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true)  //25
            .Numeric("OutQty", header: "Released Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts5)    //26
            .Numeric("AdjustQty", header: "Adjust Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //27
            .Numeric("balanceqty", header: "Balance", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts6)    //28
            .Numeric("LInvQty", header: "Stock Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts7)    //29
            .Numeric("LObQty", header: "Scrap Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts8)    //30
            .Text("ALocation", header: "Bulk Location", iseditingreadonly: true, settings: ts9)  //31
            .Text("BLocation", header: "Stock Location", iseditingreadonly: true, settings: ts11)  //32
            .Text("FIR", header: "FIR", iseditingreadonly: true, settings: ts10)  //33
            .Text("Remark", header: "Remark", iseditingreadonly: true)  //34
            ;
            #endregion

            grid1.Columns[7].Frozen = true;  //Fabric Type
        }

        private void ChangeDetailColor()
        {
            grid1.RowPostPaint += (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                if (grid1.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                int i = e.RowIndex;
                if (dr["junk"].ToString() == "True")
                {
                    grid1.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
                else
                {
                    if (dr["ThirdCountry"].ToString() == "True")
                    {
                        grid1.Rows[i].Cells[2].Style.BackColor = Color.DeepPink;
                    }

                    if (dr["BomTypeCalculate"].ToString() == "True")
                    {
                        grid1.Rows[i].Cells[6].Style.BackColor = Color.Orange;
                    }
                }
            };
        }

        // Query
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtData;
            if (MyUtility.Check.Empty(tb_Spno.Text))
            {
                MyUtility.Msg.WarningBox("SP# can't be empty. Please fill SP# first!");
                tb_Spno.Focus();
                return;
            }
            string spno = tb_Spno.Text.TrimEnd() + "%";
            #region -- SQL Command --
            string sqlcmd
                = string.Format(@"
;WITH QA AS (
	Select POID,SEQ1,SEQ2,CASE 
	when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 then 'N/A'
	else a.result
	END as [Result] from dbo.FIR a where a.POID LIKE @sp1 
	and (a.ContinuityEncode = 1 or a.PhysicalEncode = 1 or a.ShadebondEncode =1 or a.WeightEncode = 1 
	or (a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1))
	UNION ALL
	Select POID,SEQ1,SEQ2,a.result as [Result] 
	from dbo.AIR a where a.POID LIKE @sp1 and a.Result !=''
) 

select *
from(
select ROW_NUMBER() over (partition by mdivisionid,id,seq1,seq2 order by mdivisionid,id,seq1,seq2,len_D) as ROW_NUMBER_D,*
from (
select *,-len(description) as len_D from (
select 
m.ukey,m.mdivisionid,a.id,a.seq1,a.seq2,b.SuppID
,[eta] = substring(convert(varchar, a.eta, 101),1,5)
,[RevisedETA] = substring(convert(varchar,a.RevisedETA, 101),1,5)
,a.Refno,a.SCIRefno
,a.FabricType , iif(a.FabricType='F','Fabric',iif(a.FabricType='A','Accessory',iif(a.FabricType='O','Orher',a.FabricType))) as fabrictype2
, iif(a.FabricType='F',1,iif(a.FabricType='A',2,3)) as fabrictypeOrderby
,a.ColorID,a.SizeSpec
,ROUND(a.UsedQty,4) unitqty
,A.Qty
,A.NETQty
,[useqty] = isnull(A.NETQty,0)+isnull(A.lossQty,0)
,a.ShipQty,a.ShipFOC,a.ApQty,a.InputQty,a.POUnit,iif(a.Complete='1','Y','N') as Complete
,a.FinalETA,m.InQty,a.StockUnit
,iif(m.OutQty is null,'0.00',m.OutQty) as OutQty
,iif(m.AdjustQty is null,'0.00',m.AdjustQty) AdjustQty
,iif(m.InQty is null,'0.00',m.InQty) - iif(m.OutQty is null,'0.00',m.OutQty) + iif(m.AdjustQty is null,'0.00',m.AdjustQty)  balanceqty
,m.LInvQty,m.LObQty,m.ALocation,m.BLocation 
,s.ThirdCountry,a.junk,fabric.BomTypeCalculate
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,iif(a.scirefno = lag(a.scirefno,1,'') over (order by a.id,a.seq1,a.seq2),1,0)) AS description
,s.currencyid
,stuff((select Concat('/',t.Result) from (SELECT Result FROM QA where poid = m.POID and seq1 =m.seq1 and seq2 = m.seq2 )t for xml path('')),1,1,'') FIR
,(Select cast(tmp.Remark as nvarchar)+',' 
  from (
			select b1.remark 
			from receiving a1 
			inner join receiving_detail b1 on a1.id = b1.id 
			where a1.status = 'Confirmed' and (b1.Remark is not null or b1.Remark !='')
			and b1.poid = a.id
			and b1.seq1 = a.seq1
			and b1.seq2 = a.seq2 group by b1.remark
	   ) tmp 
  for XML PATH('')
 ) as  Remark
,[OrderIdList]=e.OrderID

from Orders inner join PO_Supp_Detail a on a.id = orders.poid
	left join dbo.MDivisionPoDetail m on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2 AND m.MDivisionID='{0}'
    left join fabric on fabric.SCIRefno = a.scirefno
	left join po_supp b on a.id = b.id and a.SEQ1 = b.SEQ1
    left join supp s on s.id = b.suppid
    left join PO_Supp_Detail_OrderList e on e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2
where orders.poid like @sp1 and orders.mdivisionid= '{0}' 

--很重要要看到,修正欄位要上下一起改
union

select m.ukey,m.mdivisionid,a.id,a.seq1,a.seq2,b.SuppID,substring(convert(varchar, a.eta, 101),1,5) as eta
,substring(convert(varchar,a.RevisedETA, 101),1,5) as RevisedETA,a.Refno,a.SCIRefno
,a.FabricType , iif(a.FabricType='F','Fabric',iif(a.FabricType='A','Accessory',iif(a.FabricType='O','Orher',a.FabricType))) as fabrictype2
    , iif(a.FabricType='F',1,iif(a.FabricType='A',2,3)) as fabrictypeOrderby
,a.ColorID,a.SizeSpec
,ROUND(a.UsedQty,4) unitqty,A.Qty,A.NETQty,isnull(A.NETQty,0)+isnull(A.lossQty,0) useqty ,a.ShipQty,a.ShipFOC,a.ApQty,a.InputQty,a.POUnit,iif(a.Complete='1','Y','N') as Complete
,a.FinalETA,m.InQty,a.StockUnit
,iif(m.OutQty is null,'0.00',m.OutQty) as OutQty
,iif(m.AdjustQty is null,'0.00',m.AdjustQty) AdjustQty
,iif(m.InQty is null,'0.00',m.InQty) - iif(m.OutQty is null,'0.00',m.OutQty) + iif(m.AdjustQty is null,'0.00',m.AdjustQty)  balanceqty
,m.LInvQty,m.LObQty,m.ALocation,m.BLocation 
,s.ThirdCountry,a.junk,fabric.BomTypeCalculate
,dbo.getmtldesc(a.id,a.seq1,a.seq2,2,iif(a.scirefno = lag(a.scirefno,1,'') over (order by a.id,a.seq1,a.seq2),1,0)) AS description
,s.currencyid
,stuff((select Concat('/',t.Result) from (SELECT Result FROM QA where poid = m.POID and seq1 =m.seq1 and seq2 = m.seq2 )t for xml path('')),1,1,'') FIR
,(Select cast(tmp.Remark as nvarchar)+',' 
  from (
			select b1.remark 
			from receiving a1 
			inner join receiving_detail b1 on a1.id = b1.id 
			where a1.status = 'Confirmed' and (b1.Remark is not null or b1.Remark !='')
			and b1.poid = a.id
			and b1.seq1 = a.seq1
			and b1.seq2 = a.seq2 group by b1.remark
		) tmp 
  for XML PATH('')
) as  Remark
,[OrderIdList]=e.OrderID
from dbo.MDivisionPoDetail m
left join  PO_Supp_Detail a on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2 AND m.MDivisionID='{0}' and m.poid like @sp1  
left join fabric on fabric.SCIRefno = a.scirefno
left join po_supp b on a.id = b.id and a.SEQ1 = b.SEQ1
left join supp s on s.id = b.suppid
left join PO_Supp_Detail_OrderList e on e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2
where 1=1
    AND a.id IS NOT NULL --0000576: WAREHOUSE_P03_Material Status，避免出現空資料加此條件
) as xxx
) as xxx2
) as xxx3
where ROW_NUMBER_D =1 
      
            "
            , Sci.Env.User.Keyword);
            #endregion
            #region -- 準備sql參數資料 --
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@sp1";
            sp1.Value = spno;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            #endregion

            MyUtility.Msg.WaitWindows("Data Loading....");
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtData))
            {
                if (dtData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtData;
                grid1_sorting();
            }
            else
            {
                ShowErr(sqlcmd, result);
            }
            MyUtility.Msg.WaitClear();
        }

        private void grid1_sorting()
        {
            if (grid1.RowCount > 0)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        if (MyUtility.Check.Empty(grid1)) break;
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.Sort = "id,fabrictypeOrderby, refno , colorid";
                        break;
                    case 1:
                        if (MyUtility.Check.Empty(grid1)) break;
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.Sort = "id,seq1 , seq2";
                        break;
                }
            }
        }

        // Close
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //locate
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (MyUtility.Check.Empty(listControlBindingSource1.DataSource)) return;
            
        //    int index = listControlBindingSource1.Find("id", tb_locate.Text.TrimEnd());
        //    if (index == -1)
        //    { MyUtility.Msg.WarningBox("Data was not found!!"); }
        //    else
        //    { listControlBindingSource1.Position = index; }                
            
        //}

        //Excel
        private void button3_Click(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            //if (MyUtility.Check.Empty(dt) || dt.Rows.Count == 0) return;
            //MyUtility.Excel.CopyToXls(dt, "");
            
            if (null == this.grid1.CurrentRow) return;
            var dr = this.grid1.GetDataRow<DataRow>(this.grid1.CurrentRow.Index);
            if (null == dr) return;

            P03_Print p = new P03_Print(dr);
            p.ShowDialog();

            return;
           
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grid1_sorting();
        }

       
    }
}





