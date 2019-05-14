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
        private static string _Refno, _MaterialType, _Color;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
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
            ButtonOpen = false;
        }

        //Form to Form W/H.P01
        public P03(string P01SPNo, ToolStripMenuItem menuitem)
            : base(menuitem)
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

        // Form to Form W/H.P05
        public static void P05Filter(string P01SPNo,string Refno,string MaterialType, string Color, Form MdiParent)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Warehouse.P03)
                {
                    form.Activate();
                    Sci.Production.Warehouse.P03 activateForm = (Sci.Production.Warehouse.P03)form;
                    activateForm.setTxtSPNo(P01SPNo);
                    activateForm.Query();
                    return;
                }
            }

            ToolStripMenuItem P03MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P03. Material Status"))
                            {
                                P03MenuItem = ((ToolStripMenuItem)subMenuItem);
                                break;
                            }
                        }
                    }
                }
            }
            P03 call = new P03(P01SPNo, P03MenuItem);

            call.MdiParent = MdiParent;
            call.Show();
            //改到P03詢查相關的資料都要去檢查PPIC.P01 & WH / P01的[Material Status]
            call.P03Data(P01SPNo);
            call.Activate();
            _Refno = Refno;
            _MaterialType = ((MaterialType == "F") ? "Fabric" : (MaterialType == "A") ? "Accessory" : (MaterialType == "O") ? "Orher" : "");
            _Color = Color;
            call.grid_Filter();
            call.ChangeDetailColor();
        }

        //PPIC_P01 Called        
        public static void Call(string PPIC_SPNo, Form MdiParent)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Sci.Production.Warehouse.P03)
                {
                    form.Activate();
                    Sci.Production.Warehouse.P03 activateForm = (Sci.Production.Warehouse.P03)form;
                    activateForm.setTxtSPNo(PPIC_SPNo);
                    activateForm.Query();
                    return;
                }
            }

            ToolStripMenuItem P03MenuItem = null;
            foreach (ToolStripMenuItem toolMenuItem in Sci.Env.App.MainMenuStrip.Items)
            {
                if (toolMenuItem.Text.EqualString("Warehouse"))
                {
                    foreach (var subMenuItem in toolMenuItem.DropDown.Items)
                    {
                        if (subMenuItem.GetType().Equals(typeof(System.Windows.Forms.ToolStripMenuItem)))
                        {
                            if (((ToolStripMenuItem)subMenuItem).Text.EqualString("P03. Material Status"))
                            {
                                P03MenuItem = ((ToolStripMenuItem)subMenuItem);
                                break;
                            }
                        }
                    }
                }
            }
            P03 call = new P03(PPIC_SPNo, P03MenuItem);

            call.MdiParent = MdiParent;            
            call.Show();
            //改到P03詢查相關的資料都要去檢查PPIC.P01 & WH / P01的[Material Status]
            call.P03Data(PPIC_SPNo);
            call.Activate();
            call.ChangeDetailColor();
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
            
            comboSortBy.SelectedIndex = 0;

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
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_Refno(dr);
                    frm.ShowDialog(this);
                }
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
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_Wkno(dr);
                    frm.ShowDialog(this);
                }
            };
            #endregion
            #region Taipei Stock Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_TaipeiInventory(dr);
                    frm.ShowDialog(this);
                }
            };
            #endregion
            #region Released Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts5 = new DataGridViewGeneratorNumericColumnSettings();
            ts5.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_RollTransaction(dr);
                    frm.ShowDialog(this);
                }
            };
            #endregion
            #region Balance Qty 開窗
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ts6 = new DataGridViewGeneratorNumericColumnSettings();
            ts6.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_Transaction(dr);
                    frm.ShowDialog(this);
                    if (MyUtility.Check.Empty(dr["ukey"]))
                    {
                        DataRow drukey ;
                        if (MyUtility.Check.Seek($@"select ukey from MDivisionPoDetail 
where Poid='{dr["id"]}' and seq1='{dr["Seq1"]}' and seq2='{dr["Seq2"]}'",out drukey))
                        {
                            dr["ukey"] = drukey["ukey"];
                            dr.EndEdit();
                        }
                    }                   
                }
                else if (dr["From_Program"].Equals("P04"))
                {
                    var form = new Sci.Production.Warehouse.P04_LocalTransaction(dr, "P03");
                    form.Show(this);
                }
            };
            #endregion
            #region Inventory Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts7 = new DataGridViewGeneratorTextColumnSettings();
            ts7.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_InventoryStatus(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Scrap Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts8 = new DataGridViewGeneratorTextColumnSettings();
            ts8.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_Scrap(dr);
                    frm.ShowDialog(this);
                }
                else if (dr["From_Program"].Equals("P04"))
                {
                    var frm = new Sci.Production.Warehouse.P04_ScrapQty(dr["ID"].ToString(), dr["refno"].ToString(), dr["ColorID"].ToString());
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Bulk Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts9 = new DataGridViewGeneratorTextColumnSettings();
            ts9.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr, "B");
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Stock Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts11 = new DataGridViewGeneratorTextColumnSettings();
            ts11.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr, "I");
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region FIR 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts10 = new DataGridViewGeneratorTextColumnSettings();
            ts10.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridMaterialStatus.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                if (dr["From_Program"].Equals("P03"))
                {
                    var frm = new Sci.Production.Warehouse.P03_InspectionList(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.gridMaterialStatus)
            .Text("id", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))  //1
            .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(2))  //2
            .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(2))  //3
            .Text("Suppid", header: "Supp", iseditingreadonly: true, width: Widths.AnsiChars(4), settings: ts1)  //3
            .Text("eta", header: "Sup. 1st " + Environment.NewLine + "Cfm ETA", width: Widths.AnsiChars(2), iseditingreadonly: true)    //4
            .Text("RevisedETA", header: "Sup. Delivery" + Environment.NewLine + "Rvsd ETA", width: Widths.AnsiChars(2), iseditingreadonly: true)    //5
            .Text("refno", header: "Ref#", iseditingreadonly: true, settings: ts2)  //6
            .EditText("description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(33))  //8
            .Text("fabrictype2", header: "Material\r\nType", iseditingreadonly: true, width: Widths.AnsiChars(6))  //7            
            .Text("ColorID", header: "Color", iseditingreadonly: true,width:Widths.AnsiChars(6))  //9
            .Text("SizeSpec", header: "Size", iseditingreadonly: true, width: Widths.AnsiChars(2))  //10
            .EditText("GarmentSize", header: "Garment\r\nSize", iseditingreadonly: true, width: Widths.AnsiChars(2))  //8
            .Text("CurrencyID", header: "Currency", iseditingreadonly: true, width: Widths.AnsiChars(2))  //11
            .Text("unitqty", header: "Qty", iseditingreadonly: true, width: Widths.AnsiChars(2),alignment : DataGridViewContentAlignment.MiddleRight)  //12
            .Text("Qty", header: "Order\r\nQty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight)  //13     
            .Text("NETQty", header: "Net\r\nQty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight)  //14
            .Text("useqty", header: "Use\r\nQty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight)  //15
            .Text("ShipQty", header: "Ship\r\nQty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight, settings: ts3)  //16
            .Text("ShipFOC", header: "F.O.C", iseditingreadonly: true, width: Widths.AnsiChars(3), alignment: DataGridViewContentAlignment.MiddleRight)  //17
            .Text("InputQty", header: "Taipei" + Environment.NewLine + "Stock Qty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight, settings: ts4)  //19
            .Text("POUnit", header: "PO Unit", iseditingreadonly: true, width: Widths.AnsiChars(4))  //20
            .Text("Complete", header: "Cmplt", iseditingreadonly: true,width:Widths.AnsiChars(3))  //21
            .Text("FinalETA", header: "Act.ETA", width: Widths.AnsiChars(8), iseditingreadonly: true)    //22
            .Text("OrderIdList", header: "Order List", iseditingreadonly: true, settings: OrderList)  //23
            .Numeric("InQty", header: "Arrived" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(6), iseditingreadonly: true)    //24
            .Text("StockUnit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(4))  //25
            .Numeric("OutQty", header: "Released" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(4), iseditingreadonly: true, settings: ts5)    //26
            .Numeric("AdjustQty", header: "Adjust" + Environment.NewLine + "Qty", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(4), iseditingreadonly: true)    //27
            .Numeric("balanceqty", header: "Balance", decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(4), iseditingreadonly: true, settings: ts6)    //28
            .Text("LInvQty", header: "Stock Qty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight, settings: ts7)  //29
            .Text("LObQty", header: "Scrap Qty", iseditingreadonly: true, width: Widths.AnsiChars(6), alignment: DataGridViewContentAlignment.MiddleRight, settings: ts8)  //30
            .Text("ALocation", header: "Bulk Location", iseditingreadonly: true, settings: ts9)  //31
            .Text("BLocation", header: "Stock Location", iseditingreadonly: true, settings: ts11)  //32
            .Text("FIR", header: "FIR", iseditingreadonly: true, settings: ts10)  //33
            .Text("WashLab", header: "WashLab Report", iseditingreadonly: true, settings: ts10)  //33
            .Text("Preshrink", header: "Preshrink", iseditingreadonly: true)  //34
            .Text("Remark", header: "Remark", iseditingreadonly: true)  //35
            ;
            #endregion

            this.gridMaterialStatus.ColumnFrozen(gridMaterialStatus.Columns["fabrictype2"].Index); 
            gridMaterialStatus.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridMaterialStatus.Columns["FinalETA"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridMaterialStatus.Columns["CurrencyID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridMaterialStatus.Columns["ShipFOC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridMaterialStatus.Columns["InputQty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridMaterialStatus.Columns["seq1"].Width = 40;
            this.gridMaterialStatus.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);

            displayUseStock.BackColor = Color.FromArgb(255, 255, 128);
            displayFtySupp.BackColor = Color.FromArgb(220, 140, 255);
            displayCalSize.BackColor = Color.FromArgb(255, 170, 100);
            displayJunk.BackColor = Color.FromArgb(190, 190, 190);
        }

        public void ChangeDetailColor()
        {
            for (int index = 0; index < gridMaterialStatus.Rows.Count; index++)
            {
                DataRow dr = gridMaterialStatus.GetDataRow(index);
                if (gridMaterialStatus.Rows.Count <= index || index < 0) return;


                int i = index;

                

                

                if (dr["junk"].ToString() == "1")
                {
                    gridMaterialStatus.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                }
                else
                {
                    if (dr["ThirdCountry"].ToString() == "1")
                    {
                        gridMaterialStatus.Rows[i].Cells["Suppid"].Style.BackColor = Color.FromArgb(220, 140, 255);
                    }

                    if (dr["BomTypeCalculate"].ToString() == "1")
                    {
                        gridMaterialStatus.Rows[i].Cells["description"].Style.BackColor = Color.FromArgb(255, 170, 100);
                    }
                    //
                    decimal ShipQty = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(dr["ShipQty"]) + MyUtility.Convert.GetDecimal(dr["ShipFOC"])) ? 0 : MyUtility.Convert.GetDecimal(dr["ShipQty"]) + MyUtility.Convert.GetDecimal(dr["ShipFOC"]);
                    decimal Qty = MyUtility.Check.Empty(dr["Qty"].ToString()) ? 0 : MyUtility.Convert.GetDecimal(dr["Qty"]);
                    if (!dr["ShipQty"].ToString().Empty() && !dr["Qty"].ToString().Empty())
                    if (ShipQty < Qty)
                    {
                        gridMaterialStatus.Rows[i].Cells["ShipQty"].Style.ForeColor = Color.Red;
                    }

                    if (dr["SuppCountry"].ToString().EqualString(userCountry))
                    {
                        gridMaterialStatus.Rows[i].Cells["Seq1"].Style.BackColor = Color.FromArgb(255, 255, 128);
                        gridMaterialStatus.Rows[i].Cells["Seq2"].Style.BackColor = Color.FromArgb(255, 255, 128);
                    }

                    if (!dr["OutQty"].ToString().Empty() && !dr["NETQty"].ToString().Empty() && !dr["NETQty"].ToString().Equals("-"))
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
            string junk_where1 = "", junk_where2 = "";
            string spno = txtSPNo.Text.TrimEnd() + "%";
            #region -- SQL Command --           
            string sqlcmd
                = @"
declare @id varchar(20) = @sp1		

select distinct StyleID,BrandID,POID,FtyGroup 
into #tmpOrder
from orders where id like @id

select OrderID,Refno,ThreadColorID,UnitID
into #tmpLocalPO_Detail
from LocalPO_Detail with(nolock)
where OrderID like @id
Group by OrderID,RefNo,ThreadColorID,UnitID

;WITH QA AS (
	Select  c.InvNo InvNo
            ,a.POID POID
            ,a.SEQ1 SEQ1
            ,a.SEQ2 SEQ2
            , CASE 
	            when a.Nonphysical = 1 and a.nonContinuity=1 and nonShadebond=1 and a.nonWeight=1 and a.nonOdor=1 then 'N/A'
	            when isnull(a.result,'')='' then 'Blank'
	            else a.result
	          END as [Result] 
    from dbo.FIR a WITH (NOLOCK) 
    left join dbo.Receiving c WITH (NOLOCK) on c.Id = a.ReceivingID
    where   a.POID LIKE @id
    UNION all
	Select   c.InvNo InvNo
            ,a.POID POID
            ,a.SEQ1 SEQ1
            ,a.SEQ2 SEQ2
            , CASE 
	            when isnull(a.result,'')='' then 'Blank'
	            else a.result
	          END as [Result] 
	from dbo.AIR a WITH (NOLOCK) 
    left join dbo.Receiving c WITH (NOLOCK) on c.Id = a.ReceivingID
    where   a.POID like @id 
            and a.Result !=''
) , washlabQA as (
    select 
        a.POID,
        a.seq1,
        a.seq2,
	    washlab=
	    case  when a.[Crocking]='' or a.Heat='' or a.[Wash]='' or Oven.Result='' or ColorFastness.Result='' then 'Blank'
              when a.[Crocking]='Fail'or a.Heat='Fail' or a.[Wash]='Fail' or Oven.Result='Fail' or ColorFastness.Result='Fail' then 'Fail'
		      else 'Pass'
	    end
    from dbo.FIR_Laboratory a WITH (NOLOCK) 
    inner join dbo.FIR b WITH (NOLOCK) on b.id = a.id
    inner join dbo.Receiving c WITH (NOLOCK) on c.Id = b.ReceivingID
    outer apply(select (case when  sum(iif(od.Result = 'Fail' ,1,0)) > 0 then 'Fail'
						     when  sum(iif(od.Result = 'Pass' ,1,0)) > 0 then 'Pass'
				             else '' end) as Result,
					    MIN( ov.InspDate) as InspDate 
				    from Oven ov
				    inner join dbo.Oven_Detail od on od.ID = ov.ID
				    where ov.POID=a.POID and od.SEQ1=a.Seq1 
				    and seq2=a.Seq2 and ov.Status='Confirmed') as Oven
    outer apply(select (case when  sum(iif(cd.Result = 'Fail' ,1,0)) > 0 then 'Fail'
						     when  sum(iif(cd.Result = 'Pass' ,1,0)) > 0 then 'Pass'
				             else '' end) as Result,
					    MIN( CF.InspDate) as InspDate 
				    from dbo.ColorFastness CF WITH (NOLOCK) 
				    inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
				    where CF.Status = 'Confirmed' and CF.POID=a.POID 
				    and cd.SEQ1=a.Seq1 and cd.seq2=a.Seq2) as ColorFastness
    where a.POID like @id 
)
select *
from(
    select  ROW_NUMBER() over (partition by id,seq1,seq2 order by id,seq1,seq2,len_D) as ROW_NUMBER_D
            , ukey
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
            , unitqty = iif (unitqty = 0, '', format(unitqty,'#,###,###,###.####'))
            , Qty = iif (Qty = 0, '',format(Qty,'#,###,###,###.##'))
            , NetQty = iif (NetQty = 0, '', format(NetQty,'#,###,###,###.##'))
            , useqty = iif (useqty = 0, '',format(useqty,'#,###,###,###.##'))
            , shipQty = iif (shipQty = 0, '',format(shipQty,'#,###,###,###.##'))
            , ShipFOC = iif (ShipFOC = 0, '', format(ShipFOC,'###.##'))
            , InputQty = iif (InputQty = 0, '',format(InputQty,'#,###,###,###.##'))
            , POUnit
            , Complete
            , [FinalETA] = format(FinalETA,'yyyy/MM/dd') 
            , InQty = iif (InQty = 0, '', Convert (varchar, InQty))
            , StockUnit = iif (InQty = 0, '', StockUnit)
            , OutQty = iif (OutQty = 0, '', Convert (varchar, OutQty))
            , AdjustQty = iif (AdjustQty = 0, '', Convert (varchar, AdjustQty))
            , balanceqty = iif (balanceqty = 0, '', Convert (varchar, balanceqty))
            , LInvQty = iif (LInvQty = 0, '', format(LInvQty,'#,###,###,###.##'))
            , LObQty = iif (LObQty = 0, '',format(LObQty,'#,###,###,###.##'))
            , ALocation
            , BLocation 
            , ThirdCountry
            , junk
            , BomTypeCalculate
            , description
            , currencyid
            , FIR
            , washlab
            , Preshrink
            , Remark
            , OrderIdList
             , From_Program
            ,GarmentSize
    from (
        select  *
                , -len(description) as len_D 
        from (
            select  distinct m.ukey
                    , a.id
                    , a.seq1
                    , a.seq2
					, Orders.StyleID
                    , b.SuppID
                    , [SuppCountry] = (select CountryID from supp sup WITH (NOLOCK) where sup.ID = b.SuppID)
                    , [eta] = substring(convert(varchar, a.SystemETD, 101),1,5)
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
	                                                and inv.Type in ('1', '4')
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
                    , stuff((select Concat('/',t.Result) from ( SELECT seq1,seq2,Result 
                                                                FROM QA 
                                                                where   poid = m.POID 
                                                                        and seq1 = m.seq1 
                                                                        and seq2 = m.seq2 
                                                                )t order by t.seq1,t.seq2  for xml path('')),1,1,'') FIR
					,stuff((SELECT Concat('/',washlab)
                               FROM washlabQA wqa
                               where   wqa.poid = a.id 
                                       and wqa.seq1 = a.seq1 
                                       and wqa.seq2 = a.seq2 
                               for xml path('')
                               ),1,1,'') washlab
                    , iif(Fabric.Preshrink=1,'V','') Preshrink
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
                    , [OrderIdList] = stuff((select concat('/',tmp.OrderID) 
		                                    from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2
		                                    ) tmp for xml path(''))
                                    ,1,1,'')
                    , [From_Program] = 'P03'
                    , OSS.GarmentSize
            from #tmpOrder as orders WITH (NOLOCK) 
            inner join PO_Supp_Detail a WITH (NOLOCK) on a.id = orders.poid
	        left join dbo.MDivisionPoDetail m WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2
            left join fabric WITH (NOLOCK) on fabric.SCIRefno = a.scirefno
	        left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
            left join supp s WITH (NOLOCK) on s.id = b.suppid
            LEFT JOIN dbo.Factory f on orders.FtyGroup=f.ID
            outer apply(SELECT top 1 oBOA.SizeItem FROM Order_BOA oBOA WITH (NOLOCK) WHERE a.ID = oBOA.ID AND a.SEQ1 = oBOA.Seq1 AND a.SCIRefno = oBOA.SCIRefno 
                        ) LIST
            outer apply (select [GarmentSize] = Stuff((select concat( ',',LOSS.SizeCode) 
            					from Order_SizeSpec LOSS WITH (NOLOCK)
            					LEFT JOIN Order_SizeCode LOSC WITH (NOLOCK) ON LOSC.Id=LOSS.ID AND LOSC.SizeCode = LOSS.SizeCode
            					where LOSS.Id = a.ID AND LOSS.SizeItem = LIST.SizeItem AND LOSS.SizeSpec = a.SizeSpec ORDER BY LOSC.Seq ASC FOR XML PATH('')),1,1,'') 
                        ) OSS
            
--很重要要看到,修正欄位要上下一起改
            union

            select  distinct m.ukey
                    , a.id
                    , a.seq1
                    , a.seq2
					, o.StyleID
                    , b.SuppID
                    , [SuppCountry] = (select CountryID from supp sup WITH (NOLOCK) where sup.ID = b.SuppID)
                    , substring(convert(varchar, a.SystemETD, 101),1,5) as eta
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
	                                                and inv.Type in ('1', '4')
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
                    , stuff((select Concat('/',t.Result) from (SELECT seq1,seq2, Result FROM QA where poid = m.POID and seq1 =m.seq1 and seq2 = m.seq2 )t order by seq1,seq2 for xml path('')),1,1,'') FIR
					,stuff((SELECT Concat('/',washlab)
                               FROM washlabQA wqa
                               where   wqa.poid = a.id 
                                       and wqa.seq1 = a.seq1 
                                       and wqa.seq2 = a.seq2 
                               for xml path('')
                               ),1,1,'') washlab
                    , iif(Fabric.Preshrink=1,'V','') Preshrink
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
                    , [OrderIdList] = stuff((select concat('/',tmp.OrderID)
		                                     from (
			                                    select orderID from po_supp_Detail_orderList e
			                                    where e.ID = a.ID and e.SEQ1 =a.SEQ1 and e.SEQ2 = a.SEQ2
		                                     ) tmp for xml path(''))
                                            ,1,1,'')
                    , [From_Program] = 'P03'
                    , OSS.GarmentSize
        from dbo.MDivisionPoDetail m WITH (NOLOCK) 
        inner join #tmpOrder as o on o.poid = m.poid
        left join PO_Supp_Detail a WITH (NOLOCK) on  m.POID = a.ID and m.seq1 = a.SEQ1 and m.Seq2 = a.Seq2 
        left join fabric WITH (NOLOCK) on fabric.SCIRefno = a.scirefno
        left join po_supp b WITH (NOLOCK) on a.id = b.id and a.SEQ1 = b.SEQ1
        left join supp s WITH (NOLOCK) on s.id = b.suppid
        LEFT JOIN dbo.Factory f on o.FtyGroup=f.ID
        outer apply(SELECT top 1 oBOA.SizeItem FROM Order_BOA oBOA WITH (NOLOCK) WHERE a.ID = oBOA.ID AND a.SEQ1 = oBOA.Seq1 AND a.SCIRefno = oBOA.SCIRefno 
                        ) LIST
            outer apply (select [GarmentSize] = Stuff((select concat( ',',LOSS.SizeCode) 
            					from Order_SizeSpec LOSS WITH (NOLOCK)
            					LEFT JOIN Order_SizeCode LOSC WITH (NOLOCK) ON LOSC.Id=LOSS.ID AND LOSC.SizeCode = LOSS.SizeCode
            					where LOSS.Id = a.ID AND LOSS.SizeItem = LIST.SizeItem AND LOSS.SizeSpec = a.SizeSpec ORDER BY LOSC.Seq ASC FOR XML PATH('')),1,1,'') 
                        ) OSS
        where   1=1 
                AND a.id IS NOT NULL  
               ) as xxx
    ) as xxx2
) as xxx3
where ROW_NUMBER_D =1 

--加入P04資料
union all
select ROW_NUMBER_D = 1
	   , [ukey] = null
       , [ID] = a.orderid
       , [seq1] = '-'
       , [seq2] = '-'
       , [StyleID] = '-'
       , [SuppID]  = c.ID 
       , [SuppCountry] = '-'
       , [eta] = '-'
       , [RevisedETA] = '-'
       , [Refno]		= a.Refno
       , [SCIRefno] = '-'
       , [FabricType] = a.UnitID
       , [fabrictype2] = '-'
       , [fabrictypeOrderby] = null
       , [ColorID] = a.ThreadColorID
       , [SizeSpec] = '-'
       , [unitqty] = '-'
       , [Qty] = '-'
       , [NetQty] = '-'
       , [useqty] = '-'
       , [shipQty] = '-'
       , [ShipFOC] = '-'
       , [InputQty] = '-'
       , [POUnit] = '-'
       , [Complete] = '-'
       , [FinalETA] = '-'
       , [InQty] = iif (l.InQty = 0, '', Convert (varchar, l.InQty))
       , [StockUnit] = l.UnitID 
       , [OutQty] = iif (l.OutQty = 0, '', Convert (varchar, l.OutQty))
       , [AdjustQty] = iif (l.AdjustQty = 0, '', Convert (varchar, l.AdjustQty))
       , [balanceqty] = iif (InQty - OutQty + AdjustQty = 0, '', Convert (varchar, InQty - OutQty + AdjustQty))
       , [LInvQty] =  '-' 
       , [LObQty] = iif (l.LobQty = 0, '',format(l.LobQty,'#,###,###,###.##'))
       , [ALocation] = l.ALocation
       , [BLocation] = '-' 
       , [ThirdCountry] = 0
       , [junk] = 0
       , [BomTypeCalculate] = 0
       , [description]  = b.Description
       , [currencyid] = '-'
       , [FIR] = '-'
       , [washlab] = '-'
       , [Preshrink]= ''
       , [Remark] = '-'
       , [OrderIdList] = l.OrderID
       , [From_Program] = 'P04'
       , [GarmentSize] = ''
from #tmpLocalPO_Detail a
left join LocalInventory l on a.OrderId = l.OrderID and a.Refno = l.Refno and a.ThreadColorID = l.ThreadColorID
left join LocalItem b on a.Refno=b.RefNo
left join LocalSupp c on b.LocalSuppid=c.ID

drop table #tmpOrder,#tmpLocalPO_Detail
            ";
            #endregion
            #region -- 準備sql參數資料 --
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@sp1";
            sp1.Value = spno;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            #endregion
            this.ShowWaitMessage("Data Loading....");

            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtData))
            {
                if (dtData.Rows.Count == 0 && !ButtonOpen)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = ReCombineOrderList(dtData);
                grid_Filter();
                grid1_sorting();
                ChangeDetailColor();
                ButtonOpen = false;
            }
            else
            {
                ShowErr(result);
            }
            _Refno = string.Empty;
            _Color = string.Empty;
            _MaterialType = string.Empty;
            this.HideWaitMessage();
        }

        /// <summary>
        /// 重新組成 OrderList
        /// 規則入下：
        /// </summary>
        /// <param name="dtData">需要重新組成的 DataTable</param>
        /// <returns>重新組成的 DataTable</returns>
        private DataTable ReCombineOrderList(DataTable dtData)
        {
            foreach (DataRow dr in dtData.Rows)
            {
                /*
                 * 以下情況不需要重組 OrderIDList
                 * 1. OrderIDList 是空的
                 * 2. 該筆是 Local 物料 P04
                 */
                if (dr["OrderIdList"].ToString().Equals(string.Empty)
                    || dr["From_Program"].ToString().Equals("P04"))
                {
                    continue;
                }

                List<string> strNewOrderList = new List<string>();
                string[] arrayOrderList;
                bool listHaveDiffOrderID = OrderByOrderList(dr["OrderIdList"].ToString().Split('/'), dr["id"].ToString(), out arrayOrderList);

                // 搜尋的 OrderID
                string strID = dr["id"].ToString();

                // 比較的 OrderID
                string strCompareID = dr["id"].ToString();
                foreach (string item in arrayOrderList)
                {
                    // 目前比較的 ID 不存在於 item
                    // 代表 item 讀取到新的 ID
                    if (item.Contains(strCompareID) == false
                        || strCompareID.Equals(string.Empty))
                    {
                        strCompareID = item.Substring(0, 10);
                        strNewOrderList.Add(item);
                    }
                    else if (item.Equals(strCompareID))
                    {
                        if (item.Equals(strID))
                        {
                            /* 
                             * Case 1   OrderList 存在不同的 OrderID
                             *          則每一個新的 OrderID 都必須完整顯示
                             * Case 2   OrderList 每一項都與搜尋的 OrderID 相同
                             *          則不需要顯示完整的 item 
                             *          每一項只需從第八碼開始取字串
                             */
                            if (listHaveDiffOrderID)
                            {
                                strNewOrderList.Add(item);
                            }
                            else
                            {
                                strNewOrderList.Add(item.Substring(8));
                            }
                        }
                        else
                        {
                            strNewOrderList.Add(item);
                        }
                    }
                    else
                    {
                        strNewOrderList.Add(item.Substring(8));
                    }
                }

                dr["OrderIdList"] = strNewOrderList.Aggregate((a, b) => a + "/" + b);
            }

            return dtData;
        }

        /// <summary>
        /// 重新排序 OrderList
        /// OrderID = ID 排在第一位
        /// 剩下的按照順序做排序
        /// </summary>
        /// <param name="arrayOrderList">arrayOrderList</param>
        /// <param name="strOrderListID">strOrderListID</param>
        /// <param name="returnOrderList">returnOrderList</param>
        /// <returns>是否存在不同的 OrderID</returns>
        private bool OrderByOrderList(string[] arrayOrderList, string strOrderListID, out string[] returnOrderList)
        {
            returnOrderList = new string[arrayOrderList.Count()];
            string[] arraySameAsID = arrayOrderList.AsEnumerable().Where(index => index.Contains(strOrderListID) == true).ToArray();
            string[] arrayDiffAsID = arrayOrderList.AsEnumerable().Where(index => index.Contains(strOrderListID) == false).ToArray();

            Array.Sort(arraySameAsID);
            Array.Sort(arrayDiffAsID);

            arraySameAsID.CopyTo(returnOrderList, 0);
            arrayDiffAsID.CopyTo(returnOrderList, arraySameAsID.Length);
            return arrayDiffAsID.Count() > 0 ? true : false;
        }

        private void grid_Filter()
        {
            string filter = "";
            if (gridMaterialStatus.RowCount > 0)
            {
                switch (chk_includeJunk.Checked)
                {                    
                    case true:
                        if (MyUtility.Check.Empty(gridMaterialStatus)) break;
                        if (!MyUtility.Check.Empty(_Refno) && !MyUtility.Check.Empty(_Color) && !MyUtility.Check.Empty(_MaterialType))
                        {
                            filter = $@" refno='{_Refno}' and ColorID='{_Color}' and fabrictype2='{_MaterialType}'";
                        }
                        else
                        {
                            filter = "";
                        }
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case false:
                        if (MyUtility.Check.Empty(gridMaterialStatus)) break;
                        if (!MyUtility.Check.Empty(_Refno) && !MyUtility.Check.Empty(_Color) && !MyUtility.Check.Empty(_MaterialType))
                        {
                            filter = $@" refno='{_Refno}' and ColorID='{_Color}' and fabrictype2='{_MaterialType}' and junk=0";
                        }
                        else
                        {
                            filter = " junk=0";
                        }                        
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;
                }               
            }           
        }

        private void grid1_sorting()
        {
            if (gridMaterialStatus.RowCount > 0)
            {
                switch (comboSortBy.SelectedIndex)
                {
                    case 0:
                        if (MyUtility.Check.Empty(gridMaterialStatus)) break;
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.Sort = " from_Program ,refno ,id,fabrictypeOrderby, colorid";
                        break;
                    case 1:
                        if (MyUtility.Check.Empty(gridMaterialStatus)) break;
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.Sort = "from_Program ,id,seq1 , seq2";
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

            P03_Print p = new P03_Print(dr, comboSortBy.SelectedIndex, chk_includeJunk.Checked);
            p.MdiParent = MdiParent;
            p.TopMost = true;            
            p.Show();

            return;
           
        }
        private void comboSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtSPNo.Text))return;
            grid_Filter();
            grid1_sorting();
            ChangeDetailColor();
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
        private void gridMaterialStatus_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            this.ShowErr(e.Exception);
            e.ThrowException = false;
            if (listControlBindingSource1.DataSource != null && listControlBindingSource1.DataSource is DataTable)
            {
                DataTable data = (DataTable)listControlBindingSource1.DataSource;
                data.Clear();
                return;
            }
        }

        private void chk_includeJunk_CheckedChanged(object sender, EventArgs e)
        {
            grid_Filter();
            grid1_sorting();
            ChangeDetailColor();
        }
    }
}