﻿using Ict;
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
using Sci.Win.Tools;

namespace Sci.Production.Warehouse
{
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        string userCountry = "";
        string SpNo = "";
        bool ButtonOpen = false;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Warehouse.P03)
                {
                    form.Activate();
                    this.Shown += (s, e) => { this.Close(); };
                    break;
                }
            }

            InitializeComponent();
            this.EditMode = true;

            #region set userCountry
            string sql = "select CountryID from Factory WITH (NOLOCK) where ID = @ID";
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@ID", Sci.Env.User.Factory));            
            DataTable dt;
            DualResult result;

            if (!(result = DBProxy.Current.Select(null, sql, sqlPar, out dt)))
            {
                MyUtility.Msg.ErrorBox(result.Description);
            }
            else
            {
                userCountry = dt.Rows[0]["CountryID"].ToString();
            }
            #endregion 
            ButtonOpen = false;
        }

        //Form to Form W/H.P01
        public P03(string P01SPNo)
        {            
            InitializeComponent();
            this.EditMode = true;

            #region set userCountry
            string sql = "select CountryID from Factory WITH (NOLOCK) where ID = @ID";
            List<SqlParameter> sqlPar = new List<SqlParameter>();
            sqlPar.Add(new SqlParameter("@ID", Sci.Env.User.Factory));
            DataTable dt;
            DualResult result;

            if (!(result = DBProxy.Current.Select(null, sql, sqlPar, out dt)))
            {
                MyUtility.Msg.ErrorBox(result.Description);
            }
            else
            {
                userCountry = dt.Rows[0]["CountryID"].ToString();
            }
            #endregion 
            SpNo = P01SPNo;
            this.txtSPNo.Text = SpNo.Trim();
            ButtonOpen = true;
        }

        //隨著 P01上下筆SP#切換資料
        public void P03Data(string P01SPNo)  
        {
            this.EditMode = true;
            SpNo = P01SPNo;
            this.txtSPNo.Text = SpNo.Trim();
            ButtonOpen = true;
            Query();
            ChangeDetailColor();
        }

        protected override void OnFormLoaded()
        {

            base.OnFormLoaded();
            comboSortBy.SelectedIndex = 1;

            #region Supp 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Supplier(dr);
                frm.ShowDialog(this);
            };
            #endregion

            #region refno 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Refno(dr);
                frm.ShowDialog(this);
            };
            #endregion

            #region OrderList 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings OrderList = new DataGridViewGeneratorTextColumnSettings();
            OrderList.CellMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    DataRow dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                    var frm = new Sci.Win.Tools.EditMemo(MyUtility.Convert.GetString(dr["OrderIdList"]), "Order List", false, null);
                    frm.ShowDialog(this);
                }
            };
            #endregion

            #region Ship qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts3 = new DataGridViewGeneratorNumericColumnSettings();
            ts3.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Wkno(dr);
                frm.ShowDialog(this);
            };
            #endregion
            #region Taipei Stock Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts4 = new DataGridViewGeneratorNumericColumnSettings();
            ts4.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_TaipeiInventory(dr);
                frm.ShowDialog(this);
            };
            #endregion
            #region Released Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts5 = new DataGridViewGeneratorNumericColumnSettings();
            ts5.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_RollTransaction(dr);
                frm.ShowDialog(this);
            };
            #endregion
            #region Balance Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts6 = new DataGridViewGeneratorNumericColumnSettings();
            ts6.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Transaction(dr);
                DialogResult DR =  frm.ShowDialog(this);
                if (DR == DialogResult.OK) btnQuery_Click(null, null);
            };
            #endregion
            #region Inventory Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts7 = new DataGridViewGeneratorNumericColumnSettings();
            ts7.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_InventoryStatus(dr);
                frm.ShowDialog(this);

            };
            #endregion
            #region Scrap Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts8 = new DataGridViewGeneratorNumericColumnSettings();
            ts8.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_Scrap(dr);
                frm.ShowDialog(this);

            };
            #endregion
            #region Bulk Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts9 = new DataGridViewGeneratorTextColumnSettings();
            ts9.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr, "B");
                frm.ShowDialog(this);

            };
            #endregion
            #region Stock Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts11 = new DataGridViewGeneratorTextColumnSettings();
            ts11.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr, "I");
                frm.ShowDialog(this);

            };
            #endregion
            #region FIR 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts10 = new DataGridViewGeneratorTextColumnSettings();
            ts10.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P03_InspectionList(dr);
                frm.ShowDialog(this);

            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.gridMaterialStatus)
            .Text("id", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))  //1
            .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))  //2
            .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(4))  //3
            //.Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(10))  //2
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
            //.Numeric("ApQty", header: "AP Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //18
            .Numeric("InputQty", header: "Taipei Stock Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts4)    //19
            .Text("POUnit", header: "PO Unit", iseditingreadonly: true)  //20
            .Text("Complete", header: "Cmplt", iseditingreadonly: true)  //21
            .Date("FinalETA", header: "Act. ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //22
            .Text("OrderIdList", header: "Order List", iseditingreadonly: true, settings: OrderList)  //23
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

            gridMaterialStatus.Columns["fabrictype2"].Frozen = true;  //Fabric Type
            gridMaterialStatus.Columns["id"].Width = 40;
            gridMaterialStatus.Columns["seq1"].Width = 40;
        }

        private void ChangeDetailColor()
        {
            for (int index = 0; index < gridMaterialStatus.Rows.Count; index++)
            {
                DataRow dr = gridMaterialStatus.GetDataRow(index);
                if (gridMaterialStatus.Rows.Count <= index || index < 0) return;

                int i = index;
                if (dr["junk"].ToString() == "True")
                {
                    gridMaterialStatus.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
                else
                {
                    //if (dr["ThirdCountry"].ToString() == "True")
                    //{
                    //    gridMaterialStatus.Rows[i].Cells[4].Style.BackColor = Color.DeepPink;
                    //}

                    if (dr["BomTypeCalculate"].ToString() == "True")
                    {
                        gridMaterialStatus.Rows[i].Cells["RevisedETA"].Style.BackColor = Color.Orange;
                    }

                    if (!dr["ShipQty"].ToString().Empty() && !dr["Qty"].ToString().Empty())
                    if (Convert.ToDecimal(dr["ShipQty"].ToString()) < Convert.ToDecimal(dr["Qty"].ToString()))
                    {
                        gridMaterialStatus.Rows[i].Cells["NETQty"].Style.ForeColor = Color.Red;
                    }

                    if (dr["SuppCountry"].ToString().EqualString(userCountry))
                    {
                        gridMaterialStatus.Rows[i].Cells["id"].Style.BackColor = Color.Yellow;
                        gridMaterialStatus.Rows[i].Cells["seq1"].Style.BackColor = Color.Yellow;
                    }

                    if (!dr["OutQty"].ToString().Empty() && !dr["NETQty"].ToString().Empty())
                    if (Convert.ToDecimal(dr["OutQty"].ToString()) > Convert.ToDecimal(dr["NETQty"].ToString()))
                    {
                        gridMaterialStatus.Rows[i].Cells["OutQty"].Style.ForeColor = Color.Red;
                    }
                        
                }
            }
        }

        // Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtSPNo.Text))
            {
                MyUtility.Msg.WarningBox("SP# can't be empty. Please fill SP# first!");
                txtSPNo.Focus();
                return;
            }
            Query();
        }

        public void Query()
        {
            DataTable dtData;

            string spno = txtSPNo.Text.TrimEnd() + "%";
            #region -- SQL Command --
            string sqlcmd
                = string.Format(@"
;WITH QA AS (
	Select  POID
            , SEQ1
            , SEQ2
            , CASE 
	            when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 then 'N/A'
	            else a.result
	          END as [Result] 
    from dbo.FIR a WITH (NOLOCK) 
    where   a.POID LIKE @sp1 
	        and (a.ContinuityEncode = 1 or a.PhysicalEncode = 1 or a.ShadebondEncode =1 or a.WeightEncode = 1 
	        or (a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1))
	
    UNION ALL
	Select  POID
            , SEQ1
            , SEQ2
            , a.result as [Result] 
	from dbo.AIR a WITH (NOLOCK) 
    where   a.POID LIKE @sp1 
            and a.Result !=''
) 
select *
from(
    select  ROW_NUMBER() over (partition by mdivisionid,id,seq1,seq2 order by mdivisionid,id,seq1,seq2,len_D) as ROW_NUMBER_D
            , ukey
            , mdivisionid
            , id
            , seq1
            , seq2
			, StyleID
            , SuppID
            , SuppCountry
            , eta
            , RevisedETA
            , Refno
            , SCIRefno
            , FabricType 
            , fabrictype2
            , fabrictypeOrderby
            , ColorID
            , SizeSpec
            , unitqty = iif (unitqty = 0, '', Convert (varchar, unitqty))
            , Qty = iif (Qty = 0, '', Convert (varchar, Qty))
            , NetQty = iif (NetQty = 0, '', Convert (varchar, NetQty))
            , useqty = iif (useqty = 0, '', Convert (varchar, useqty))
            , shipQty = iif (shipQty = 0, '', Convert (varchar, shipQty))
            , ShipFOC = iif (ShipFOC = 0, '', Convert (varchar, ShipFOC))
            , InputQty = iif (InputQty = 0, '', Convert (varchar, InputQty))
            , POUnit
            , Complete
            , FinalETA
            , InQty = iif (InQty = 0, '', Convert (varchar, InQty))
            , StockUnit
            , OutQty = iif (OutQty = 0, '', Convert (varchar, OutQty))
            , AdjustQty = iif (AdjustQty = 0, '', Convert (varchar, AdjustQty))
            , balanceqty = iif (balanceqty = 0, '', Convert (varchar, balanceqty))
            , LInvQty = iif (LInvQty = 0, '', Convert (varchar, LInvQty))
            , LObQty = iif (LObQty = 0, '', Convert (varchar, LObQty))
            , ALocation
            , BLocation 
            , ThirdCountry
            , junk
            , BomTypeCalculate
            , description
            , currencyid
            , FIR
            , Remark
            , OrderIdList
    from (
        select  *
                , -len(description) as len_D 
        from (
            select  m.ukey
                    , f.mdivisionid
                    , a.id
                    , a.seq1
                    , a.seq2
					, Orders.StyleID
                    , b.SuppID
                    , [SuppCountry] = (select CountryID from supp sup WITH (NOLOCK) where sup.ID = b.SuppID)
                    , [eta] = substring(convert(varchar, a.eta, 101),1,5)
                    , [RevisedETA] = substring(convert(varchar,a.RevisedETA, 101),1,5)
                    , a.Refno
                    , a.SCIRefno
                    , a.FabricType 
                    , iif(a.FabricType='F','Fabric',iif(a.FabricType='A','Accessory',iif(a.FabricType='O','Orher',a.FabricType))) as fabrictype2
                    , iif(a.FabricType='F',1,iif(a.FabricType='A',2,3)) as fabrictypeOrderby
                    , ColorID = dbo.GetColorMultipleID(Orders.BrandID,a.ColorID) 
                    , a.SizeSpec
                    , ROUND(a.UsedQty, 4) unitqty
                    , Qty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(A.Qty, 0)), 2)
                    , NetQty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(A.NETQty, 0)), 2)
                    , [useqty] = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, (isnull(A.NETQty,0)+isnull(A.lossQty,0))), 2)
                    , shipQty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(a.ShipQty, 0)), 2)
                    , ShipFOC = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(a.ShipFOC, 0)), 2)
                    , InputQty = isnull((select Round(sum(invtQty), 2)
                                         from (
                                            SELECT  dbo.getUnitQty(inv.UnitID, a.StockUnit, isnull(Qty, 0.00))  as invtQty
	                                        FROM InvTrans inv WITH (NOLOCK)
	                                        WHERE   inv.InventoryPOID = a.id
	                                                and inv.InventorySeq1 = a.Seq1
	                                                and inv.InventorySeq2 = a.seq2
	                                                and inv.Type in (1, 4)
                                         )tmp), 0.00)
                    , a.POUnit
                    , iif(a.Complete='1','Y','N') as Complete
                    , a.FinalETA
                    , m.InQty
                    , a.StockUnit
                    , iif(m.OutQty is null,'0.00',m.OutQty) as OutQty
                    , iif(m.AdjustQty is null,'0.00',m.AdjustQty) AdjustQty
                    , iif(m.InQty is null,'0.00',m.InQty) - iif(m.OutQty is null,'0.00',m.OutQty) + iif(m.AdjustQty is null,'0.00',m.AdjustQty)  balanceqty
                    , m.LInvQty
                    , m.LObQty
                    , m.ALocation
                    , m.BLocation 
                    , s.ThirdCountry
                    , a.junk
                    , fabric.BomTypeCalculate
                    , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) AS description
                    , s.currencyid
                    , stuff((select Concat('/',t.Result) from ( SELECT Result 
                                                                FROM QA 
                                                                where   poid = m.POID 
                                                                        and seq1 = m.seq1 
                                                                        and seq2 = m.seq2 
                                                                )t for xml path('')),1,1,'') FIR
                    ,(Select cast(tmp.Remark as nvarchar)+',' 
                      from (
			                    select b1.remark 
			                    from receiving a1 WITH (NOLOCK) 
			                    inner join receiving_detail b1 WITH (NOLOCK) on a1.id = b1.id 
			                    where a1.status = 'Confirmed' and (b1.Remark is not null or b1.Remark !='')
			                    and b1.poid = a.id
			                    and b1.seq1 = a.seq1
			                    and b1.seq2 = a.seq2 group by b1.remark
	                       ) tmp 
                      for XML PATH('')
                     ) as  Remark
                    , [OrderIdList] = stuff((select concat('/', SubString(tmp.OrderID, 9, 5)) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2
		                                    ) tmp for xml path(''))
                                    ,1,1,'')
            from Orders WITH (NOLOCK) 
            inner join PO_Supp_Detail a WITH (NOLOCK) on a.id = orders.poid
	        left join dbo.MDivisionPoDetail m WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2
            left join fabric WITH (NOLOCK) on fabric.SCIRefno = a.scirefno
	        left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
            left join supp s WITH (NOLOCK) on s.id = b.suppid
            LEFT JOIN dbo.Factory f on orders.FtyGroup=f.ID
            where orders.id like @sp1 and a.junk <> 'true'

--很重要要看到,修正欄位要上下一起改
            union

            select  m.ukey
                    , f.mdivisionid
                    , a.id
                    , a.seq1
                    , a.seq2
					, o.StyleID
                    , b.SuppID
                    , [SuppCountry] = (select CountryID from supp sup WITH (NOLOCK) where sup.ID = b.SuppID)
                    , substring(convert(varchar, a.eta, 101),1,5) as eta
                    , substring(convert(varchar,a.RevisedETA, 101),1,5) as RevisedETA
                    , a.Refno
                    , a.SCIRefno
                    , a.FabricType , iif(a.FabricType='F','Fabric',iif(a.FabricType='A','Accessory',iif(a.FabricType='O','Orher',a.FabricType))) as fabrictype2
                    , iif(a.FabricType='F',1,iif(a.FabricType='A',2,3)) as fabrictypeOrderby
                    , ColorID = dbo.GetColorMultipleID(o.BrandID,a.ColorID) 
                    , a.SizeSpec
                    , ROUND(a.UsedQty, 4) unitqty
                    , Qty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(A.Qty, 0)), 2)
                    , NetQty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(A.NETQty, 0)), 2)
                    , useqty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, (isnull(A.NETQty,0)+isnull(A.lossQty,0))), 2)
                    , ShipQty = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(a.ShipQty, 0)), 2)
                    , ShipFOC = Round(dbo.getUnitQty(a.POUnit, a.StockUnit, isnull(a.ShipFOC, 0)), 2)
                    , InputQty = isnull((select Round(sum(invtQty), 2)
                                         from (
	                                        SELECT dbo.getUnitQty(inv.UnitID, a.StockUnit, isnull(Qty, 0.0)) as invtQty
	                                        FROM InvTrans inv WITH (NOLOCK)
	                                        WHERE   inv.InventoryPOID = m.poid
	                                                and inv.InventorySeq1 = m.Seq1
	                                                and inv.InventorySeq2 = m.seq2
	                                                and inv.Type in (1, 4)
                                        )tmp), 0.00)
                    , a.POUnit
                    , iif(a.Complete='1','Y','N') as Complete
                    , a.FinalETA
                    , m.InQty
                    , a.StockUnit
                    , iif(m.OutQty is null,'0.00',m.OutQty) as OutQty
                    , iif(m.AdjustQty is null,'0.00',m.AdjustQty) AdjustQty
                    , iif(m.InQty is null,'0.00',m.InQty) - iif(m.OutQty is null,'0.00',m.OutQty) + iif(m.AdjustQty is null,'0.00',m.AdjustQty)  balanceqty
                    , m.LInvQty
                    , m.LObQty
                    , m.ALocation
                    , m.BLocation 
                    , s.ThirdCountry
                    , a.junk
                    , fabric.BomTypeCalculate
                    , dbo.getmtldesc(a.id,a.seq1,a.seq2,2,0) AS description
                    , s.currencyid
                    , stuff((select Concat('/',t.Result) from (SELECT Result FROM QA where poid = m.POID and seq1 =m.seq1 and seq2 = m.seq2 )t for xml path('')),1,1,'') FIR
                    , (Select cast(tmp.Remark as nvarchar)+',' 
                       from (
			                    select b1.remark 
			                    from receiving a1 WITH (NOLOCK) 
			                    inner join receiving_detail b1 WITH (NOLOCK) on a1.id = b1.id 
			                    where a1.status = 'Confirmed' and (b1.Remark is not null or b1.Remark !='')
			                    and b1.poid = a.id
			                    and b1.seq1 = a.seq1
			                    and b1.seq2 = a.seq2 group by b1.remark
		                    ) tmp 
                       for XML PATH('')
                     ) as  Remark
                    , [OrderIdList] = stuff((select concat('/', SubString(tmp.OrderID, 9, 5))
		                                     from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2
		                                     ) tmp for xml path(''))
                                            ,1,1,'')
        from dbo.MDivisionPoDetail m WITH (NOLOCK) 
        inner join Orders o on o.poid = m.poid
        left join PO_Supp_Detail a WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2 --and m.poid like @sp1  
        left join fabric WITH (NOLOCK) on fabric.SCIRefno = a.scirefno
        left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
        left join supp s WITH (NOLOCK) on s.id = b.suppid
        LEFT JOIN dbo.Factory f on o.FtyGroup=f.ID
        where   1=1 
                AND a.id IS NOT NULL 
                and a.junk <> 'true'--0000576: WAREHOUSE_P03_Material Status，避免出現空資料加此條件
                and o.id like @sp1
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
            this.ShowWaitMessage("Data Loading....");

            //MyUtility.Msg.WaitWindows("Data Loading....");
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtData))
            {
                if (dtData.Rows.Count == 0 && !ButtonOpen)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtData;
                grid1_sorting();
                ChangeDetailColor();
                ButtonOpen = false;
            }
            else
            {
                ShowErr(result);
            }
            this.HideWaitMessage();
        }

        private void grid1_sorting()
        {
            if (gridMaterialStatus.RowCount > 0)
            {
                switch (comboSortBy.SelectedIndex)
                {
                    case 0:
                        if (MyUtility.Check.Empty(gridMaterialStatus)) break;
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.Sort = "id,fabrictypeOrderby, refno , colorid";
                        break;
                    case 1:
                        if (MyUtility.Check.Empty(gridMaterialStatus)) break;
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.Sort = "id,seq1 , seq2";
                        break;
                }
            }
        }

        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            
            if (null == this.gridMaterialStatus.CurrentRow) return;
            var dr = this.gridMaterialStatus.GetDataRow<DataRow>(this.gridMaterialStatus.CurrentRow.Index);
            if (null == dr) return;

            P03_Print p = new P03_Print(dr);
            p.ShowDialog();

            return;
           
        }
        private void comboSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
           // this.grid1_sorting();
            if (MyUtility.Check.Empty(txtSPNo.Text))
            {
               // MyUtility.Msg.WarningBox("SP# can't be empty. Please fill SP# first!");
               // txtSPNo.Focus();
                return;
            }
            Query();
           // Query();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (txtSPNo.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        Query();
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void gridMaterialStatus_Sorted(object sender, EventArgs e)
        {
            ChangeDetailColor();
        }

        private void btnNewSearch_Click(object sender, EventArgs e)
        {
            txtSPNo.ResetText();
            txtSPNo.Select();
        }

        public void setTxtSPNo(string spNo)
        {
            this.txtSPNo.Text = spNo;
        }
    }
}