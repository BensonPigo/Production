using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07_ModifyRollDyelot : Win.Subs.Base
    {
        private DataTable source;
        private DataTable dtGridDyelot;
        private string docno = string.Empty;
        private string gridAlias;
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_roll;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_dyelot;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_fullroll;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_fulldyelot;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ActQty;

        /// <inheritdoc/>
        public P07_ModifyRollDyelot(object data, string data2, string gridAlias)
        {
            this.InitializeComponent();
            this.source = (DataTable)data;
            this.docno = data2;
            this.Text += " - " + this.docno;
            this.EditMode = true;
            this.gridAlias = gridAlias;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
            this.listControlBindingSource1.DataSource = this.source;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype = new Ict.Win.UI.DataGridViewComboBoxColumn();
            DataGridViewGeneratorNumericColumnSettings actqty = new DataGridViewGeneratorNumericColumnSettings();
            actqty.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridModifyRoll.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["ActualQty"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue != null)
                {
                    dr["Actualqty"] = e.FormattedValue;
                    if (!MyUtility.Check.Empty(dr["pounit"]) && !MyUtility.Check.Empty(dr["stockunit"]))
                    {
                        string rate = MyUtility.GetValue.Lookup(string.Format(
                            @"select RateValue from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", dr["pounit"], dr["stockunit"]));
                        dr["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                    }
                }
            };

            // 設定Grid1的顯示欄位
            this.gridModifyRoll.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridModifyRoll)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(7), iseditingreadonly: false).Get(out this.col_roll) // 3
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(7), iseditingreadonly: false).Get(out this.col_dyelot) // 4
            ;

            if (this.gridAlias.ToUpper().EqualString("RECEIVING_DETAIL"))
            {
                this.Helper.Controls.Grid.Generator(this.gridModifyRoll)
                .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(8), iseditingreadonly: false, decimal_places: 2, integer_places: 10, maximum: 999999999.99M, minimum: 0, settings: actqty).Get(out this.col_ActQty) // 5
                .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true) // 6
                .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 8
                .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype) // 9
                .Text("Location", header: "Location", iseditingreadonly: true) // 10
                .Text("remark", header: "Remark", iseditingreadonly: true) // 11
                .Text("FullRoll", header: "Full Roll", width: Widths.AnsiChars(9), iseditingreadonly: false).Get(out this.col_fullroll) // 3
                .Text("FullDyelot", header: "Full Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_fulldyelot) // 4
                ;
            }
            else if (this.gridAlias.ToUpper().EqualString("TRANSFERIN_DETAIL"))
            {
                this.Helper.Controls.Grid.Generator(this.gridModifyRoll)
                .Numeric("Qty", header: "Actual Qty", width: Widths.AnsiChars(11), iseditingreadonly: false, decimal_places: 2, integer_places: 10, maximum: 999999999.99M, minimum: 0).Get(out this.col_ActQty)
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)
                .Text("Location", header: "Location", iseditingreadonly: true)
                .Text("remark", header: "Remark", iseditingreadonly: true)
                .Text("FullRoll", header: "Full Roll", width: Widths.AnsiChars(9), iseditingreadonly: false).Get(out this.col_fullroll) // 3
                .Text("FullDyelot", header: "Full Dyelot", width: Widths.AnsiChars(10), iseditingreadonly: false).Get(out this.col_fulldyelot) // 4
                ;
            }

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220029
            if (Automation.UtilityAutomation.IsAutomationEnable == true)
            {
                this.Helper.Controls.Grid.Generator(this.gridModifyRoll)
                .DateTime("CompleteTime", header: "Complete Time", width: Widths.AnsiChars(18), iseditingreadonly: true);
            }

            cbb_stocktype.DataSource = new BindingSource(this.di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            this.Helper.Controls.Grid.Generator(this.gridDyelot)
            .Date("IssueDate", header: "IssueDate", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("ID", header: "ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("InQty", header: "InQty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("OutQty", header: "OutQty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("remark", header: "Remark", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("Location", header: "Location", iseditingreadonly: true)
            .Numeric("balance", header: "Balance", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            ;

            this.LoadDate();
            this.SetCloumn();
            this.Changeeditable();
        }

        private void Changeeditable()
        {
            #region roll
            this.col_roll.EditingControlShowing += (s, e) =>
            {
                this.RollDyelot_EditSetting(e);
            };
            this.col_roll.CellFormatting += (s, e) =>
            {
                this.RollDyelot_ChangeColor(e);
            };
            #endregion
            #region dyelot
            this.col_dyelot.EditingControlShowing += (s, e) =>
            {
                this.RollDyelot_EditSetting(e);
            };
            this.col_dyelot.CellFormatting += (s, e) =>
            {
                this.RollDyelot_ChangeColor(e);
            };
            #endregion
            #region fullRoll
            this.col_fullroll.EditingControlShowing += (s, e) =>
            {
                this.RollDyelot_EditSetting(e);
            };
            this.col_fullroll.CellFormatting += (s, e) =>
            {
                this.RollDyelot_ChangeColor(e);
            };
            #endregion
            #region fullDyelot
            this.col_fulldyelot.EditingControlShowing += (s, e) =>
            {
                this.RollDyelot_EditSetting(e);
            };
            this.col_fulldyelot.CellFormatting += (s, e) =>
            {
                this.RollDyelot_ChangeColor(e);
            };
            #endregion

            #region ActQty
            this.col_ActQty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridModifyRoll.GetDataRow(e.RowIndex);

                if (this.dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                }
            };
            this.col_ActQty.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridModifyRoll.GetDataRow(e.RowIndex);

                if (this.dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            #endregion
        }

        private void RollDyelot_ChangeColor(DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataRow dr = this.gridModifyRoll.GetDataRow(e.RowIndex);

            if (this.dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0 ||
            (MyUtility.Check.Empty(dr["CompleteTime"]) == false && Automation.UtilityAutomation.IsAutomationEnable == true))
            {
                e.CellStyle.BackColor = Color.White;
            }
            else
            {
                e.CellStyle.BackColor = Color.Pink;
            }
        }

        private void RollDyelot_EditSetting(Ict.Win.UI.DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataRow dr = this.gridModifyRoll.GetDataRow(e.RowIndex);

            if (this.dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0 ||
            (MyUtility.Check.Empty(dr["CompleteTime"]) == false && Automation.UtilityAutomation.IsAutomationEnable == true))
            {
                ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
            }
            else
            {
                ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void LoadDate()
        {
            this.GetgridDyelotData();
            this.btnCommit.Enabled = false;
        }

        private void SetCloumn()
        {
            foreach (DataGridViewRow item in this.gridModifyRoll.Rows)
            {
                bool existsDetail = this.dtGridDyelot.AsEnumerable()
                 .Where(s => s["poid"].Equals(item.Cells["poid"].Value) &&
                             s["seq"].Equals(item.Cells["seq"].Value) &&
                             s["roll"].Equals(item.Cells["roll"].Value) &&
                             s["dyelot"].Equals(item.Cells["dyelot"].Value)).Any();
                if (!existsDetail)
                {
                    this.btnCommit.Enabled = true;
                }
            }
        }

        private void GetgridDyelotData()
        {
            #region sql command
            string selectCommand1 =
                string.Format(
                    @"
SELECT DISTINCT POID,Seq1,Seq2,Roll,Dyelot,id into #tmp FROM {1} WHERE id = '{0}'

select IssueDate,inqty,outqty,adjust,id,Remark,location,tmp.name,POID,Seq1,Seq2, Roll,Dyelot,[Seq] = Seq1 + ' ' + Seq2
from (
        select  a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot,a.IssueDate
        ,Case type when 'A' then 'P35. Adjust Bulk Qty' 
                        when 'B' then 'P34. Adjust Stock Qty' end as Name
        ,0 as InQty,0 as OutQty, sum(QtyAfter - QtyBefore) Adjust, a.Remark ,'' Location
        from Adjust a WITH (NOLOCK) , Adjust_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.Seq1 = t.Seq1 and b.Seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
        where a.id = b.id
		group by a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot, a.remark,a.IssueDate,type

        union all

        select a.id,[poid] = b.FromPoId,[Seq1] = b.FromSeq1,[Seq2] = b.FromSeq2,[Roll] = b.FromRoll,[Dyelot] = b.FromDyelot, a.IssueDate
        ,case type when 'A' then 'P31. Material Borrow From' 
                        when 'B' then 'P32. Material Give Back From' end as name
        ,0 as inqty, sum(qty) released,0 as adjust, a.remark ,'' location
        from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.FromPoId = t.PoId and b.FromSeq1 = t.Seq1 and b.FromSeq2 = t.Seq2 and b.FromRoll = t.Roll and b.FromDyelot = t.Dyelot
        where Status='Confirmed'  and a.id = b.id 
		group by a.id, b.FromPoId, b.FromSeq1,b.FromSeq2,b.FromRoll,b.FromDyelot, a.remark,a.IssueDate,a.type

        union all

		select a.id,[poid] = b.ToPoid,[Seq1] = b.ToSeq1,[Seq2] = b.ToSeq2,[Roll] = b.ToRoll,[Roll] = b.ToDyelot, a.IssueDate
		,case type when 'A' then 'P31. Material Borrow To' 
					when 'B' then 'P32. Material Give Back To' end as name
		, sum(qty) arrived,0 as ouqty,0 as adjust, remark ,'' location
		from BorrowBack a WITH (NOLOCK) , BorrowBack_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.ToPoid = t.PoId and b.ToSeq1 = t.Seq1 and b.ToSeq2 = t.Seq2 and b.toroll = t.Roll and b.ToDyelot = t.Dyelot 
		where Status='Confirmed' and a.id = b.id 
		group by a.id, b.ToPoid, b.ToSeq1,b.ToSeq2,b.ToRoll,b.ToDyelot, a.remark,a.IssueDate,a.type

        union all

		select a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot,a.IssueDate
		,case type when 'A' then 'P10. Issue Fabric to Cutting Section' 
					when 'B' then 'P11. Issue Sewing Material by Transfer Guide' 
					when 'C' then 'P12. Issue Packing Material by Transfer Guide' 
					when 'D' then 'P13. Issue Material by Item' end name
		,0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
		from Issue a WITH (NOLOCK) , Issue_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where Status='Confirmed' and a.id = b.id
		group by a.id, b.poid, b.seq1,b.Seq2,b.Roll,b.Dyelot, a.remark,a.IssueDate,a.type  
		                                                                        
		union all

		select a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate
		,case FabricType when 'A' then 'P15. Issue Accessory Lacking & Replacement' 
									when 'F' then 'P16. Issue Fabric Lacking & Replacement' end as name
		, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, a.remark ,'' location
		from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where Status in ('Confirmed','Closed') and a.id = b.id
		group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.remark  ,a.IssueDate,a.FabricType       
		                                                        
		union all

		select a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate
		,'P17. R/Mtl Return' name
		, 0 as inqty, sum(b.Qty) released,0 as adjust, remark,'' location
		from IssueReturn a WITH (NOLOCK) , IssueReturn_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where status='Confirmed'  and a.id = b.id 
		group by a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, remark,a.IssueDate        
		                                                                         
		union all

		select a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,[IssueDate] = a.eta
		,'P07. Material Receiving' as name
		, sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
		from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where Status='Confirmed' and a.id = b.id and type='A' and a.id!='{0}'
		group by a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,a.eta,a.Type

		union all

		select a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,[IssueDate] = a.WhseArrival
				,'P08. Warehouse Shopfloor Receiving' as name
				, sum(b.StockQty) arrived,0 as ouqty,0 as adjust,'' remark ,'' location
		from Receiving a WITH (NOLOCK) , Receiving_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where Status='Confirmed'  and a.id = b.id and type='B'
		group by a.Id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot,a.WhseArrival,a.Type  
		                                                                            
		union all

		select a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate   
		,'P37. Return Receiving Material' name
		, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
		from ReturnReceipt a WITH (NOLOCK) , ReturnReceipt_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where Status='Confirmed' and a.id = b.id 
		group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, remark,a.IssueDate     
		                                                                          
		union all

		select a.id, b.frompoid, b.FromSeq1,b.FromSeq2, b.FromRoll,b.FromDyelot,a.IssueDate
		,case a.type when 'A' then 'P22. Transfer Bulk to Inventory'
							when 'B' then 'P23. Transfer Inventory to Bulk'
							when 'C' then 'P36. Transfer Scrap to Inventory' 
							when 'D' then 'P25. Transfer Bulk to Scrap' 
							when 'E' then 'P24. Transfer Inventory to Scrap' 
			end as name
		, 0 as inqty, sum(Qty) released,0 as adjust , '' remark ,'' location
		from SubTransfer a WITH (NOLOCK) , SubTransfer_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.Frompoid = t.PoId and b.Fromseq1 = t.Seq1 and b.FromSeq2 = t.Seq2 and b.fromroll = t.Roll and b.FromDyelot = t.Dyelot
		where Status='Confirmed' and a.id = b.id
		group by a.id, b.frompoid, b.FromSeq1,b.FromSeq2, b.FromRoll,b.FromDyelot,a.IssueDate,a.Type     
		                                                                          
		union all

		select a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate  
			,'P18. Transfer In' name
			, sum(Qty) arrived,0 as ouqty,0 as adjust, a.remark
			,(Select cast(tmp.Location as nvarchar)+',' 
								from (select b1.Location 
											from TransferIn a1 WITH (NOLOCK) 
											inner join TransferIn_Detail b1 WITH (NOLOCK) on a1.id = b1.id 
											where a1.status = 'Confirmed' and (b1.Location is not null or b1.Location !='')
												and b1.Poid = b.Poid
												and b1.Seq1 = b.Seq1
												and b1.Seq2 = b.Seq2 group by b1.Location) tmp 
								for XML PATH('')) as Location
		from TransferIn a WITH (NOLOCK) , TransferIn_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.roll = t.Roll and b.Dyelot = t.Dyelot
		where Status='Confirmed' and a.id = b.id and a.id!='{0}'
		group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, a.remark,a.IssueDate         
		                                                                        
		union all

		select a.id, b.poid, b.Seq1,b.Seq2, b.Roll,b.Dyelot, a.IssueDate
			,'P19. Transfer Out' name
			, 0 as inqty, sum(Qty) released,0 as adjust, remark,'' location
		from TransferOut a WITH (NOLOCK) , TransferOut_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.roll = t.Roll and b.Dyelot = t.Dyelot
		where Status='Confirmed' and a.id = b.id 
		group by a.id, b.poid, b.Seq1,b.Seq2, b.Roll,b.Dyelot, remark,a.IssueDate
			
		union all

		select [id] = f.POID,f.POID,f.SEQ1,f.SEQ2,fp.Roll,fp.Dyelot
		,[IssueDate] = fp.InspDate,[Name] = 'P01.Fabric Inspection',[InQty]=0,[OutQty]=0,[Adjust]=0
		,[Remark] = 'P01. Physical Inspection',[Location]=''
		from FIR f
		inner join #tmp t on f.ReceivingID=t.Id and f.POID=t.PoId and f.SEQ1=t.Seq1 and f.SEQ2=t.Seq2
		inner join FIR_Physical fp on fp.ID=f.ID and fp.Roll=t.roll AND FP.Dyelot=T.Dyelot

		union all

		select [id] = f.POID,f.POID,f.SEQ1,f.SEQ2,fp.Roll,fp.Dyelot
		,[IssueDate] = fp.InspDate,[Name] = 'P01.Fabric Inspection',[InQty]=0,[OutQty]=0,[Adjust]=0
		,[Remark] = 'P01.Weight Test',[Location]=''
		from FIR f
		inner join #tmp t on f.ReceivingID=t.Id and f.POID=t.PoId and f.SEQ1=t.Seq1 and f.SEQ2=t.Seq2
		inner join FIR_Weight fp on fp.ID=f.ID and fp.Roll=t.roll AND FP.Dyelot=T.Dyelot

		union all

		select [id] = f.POID,f.POID,f.SEQ1,f.SEQ2,fp.Roll,fp.Dyelot
		,[IssueDate] = fp.InspDate,[Name] = 'P01.Fabric Inspection',[InQty]=0,[OutQty]=0,[Adjust]=0
		,[Remark] = 'P01. ShadeBand Test',[Location]=''
		from FIR f
		inner join #tmp t on f.ReceivingID=t.Id and f.POID=t.PoId and f.SEQ1=t.Seq1 and f.SEQ2=t.Seq2
		inner join FIR_Shadebone fp on fp.ID=f.ID and fp.Roll=t.roll AND FP.Dyelot=T.Dyelot AND fp.Result !=''

		union all

		select [id] = f.POID,f.POID,f.SEQ1,f.SEQ2,fp.Roll,fp.Dyelot
		,[IssueDate] = fp.InspDate,[Name] = 'P01.Fabric Inspection',[InQty]=0,[OutQty]=0,[Adjust]=0
		,[Remark] = 'P01. Continuity Test',[Location]=''
		from FIR f
		inner join #tmp t on f.ReceivingID=t.Id and f.POID=t.PoId and f.SEQ1=t.Seq1 and f.SEQ2=t.Seq2
		inner join FIR_Continuity fp on fp.ID=f.ID and fp.Roll=t.roll AND FP.Dyelot=T.Dyelot

		union all

		select [id] = f.POID,f.POID,f.SEQ1,f.SEQ2,fp.Roll,fp.Dyelot
		,[IssueDate] = fp.InspDate,[Name] = 'P01.Fabric Inspection',[InQty]=0,[OutQty]=0,[Adjust]=0
		,[Remark] = 'P01. Odor Test',[Location]=''
		from FIR f
		inner join #tmp t on f.ReceivingID=t.Id and f.POID=t.PoId and f.SEQ1=t.Seq1 and f.SEQ2=t.Seq2
		inner join FIR_Odor fp on fp.ID=f.ID and fp.Roll=t.roll AND FP.Dyelot=T.Dyelot

) tmp
            order by IssueDate,name,id
            ",
                    this.docno,
                    this.gridAlias);
            #endregion

            DualResult result;
            this.ShowWaitMessage("Data Loading...");
            if (!(result = DBProxy.Current.Select(null, selectCommand1, out this.dtGridDyelot)))
            {
                this.ShowErr(selectCommand1, result);
            }

            this.HideWaitMessage();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void Grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dtGridDyelot == null)
            {
                this.GetgridDyelotData();
            }

            if (this.dtGridDyelot.Rows.Count == 0)
            {
                return;
            }

            if (e.RowIndex == -1)
            {
                return;
            }

            DataRow dr = this.gridModifyRoll.GetDataRow(e.RowIndex);

            // 上下grid連動, Balance需要依照IssueDate , ID 排序後重新計算
            DataTable dt = new DataTable();
            string cmdd =
                $@"
            select  IssueDate,inqty = iif(adjust>0,inqty+adjust,inqty),outqty = iif(adjust < 0,abs(adjust) + abs(outqty), abs(outqty))
            ,adjust,id,Remark,location,name,POID,Seq1,Seq2, Roll,Dyelot,[Seq] 
            ,[balance] = sum(inqty - abs(outqty) + adjust) over (order by convert(date,IssueDate),convert(varchar(15),id))
            from #tmp 
            where Poid='{dr["poid"]}' and seq1='{dr["seq1"]}' and seq2='{dr["seq2"]}'
            and roll='{dr["roll"]}' and dyelot='{dr["dyelot"]}'
            group by IssueDate,inqty,outqty,adjust,id,Remark,location,name,POID,Seq1,Seq2, Roll,Dyelot,Seq
            order by IssueDate,id,name
            ";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(
                this.dtGridDyelot,
                string.Empty, cmdd, out dt);
            if (dt.Rows.Count == 0)
            {
                this.gridDyelot.DataSource = null;
            }
            else
            {
                this.gridDyelot.DataSource = dt;
            }

            this.gridDyelot.AutoResizeColumns();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnCommit_Click(object sender, EventArgs e)
        {
            DualResult result;
            var modifyDrList = this.source.AsEnumerable().Where(s => s.RowState == DataRowState.Modified).ToList();
            if (modifyDrList.Count() == 0)
            {
                MyUtility.Msg.InfoBox("No data has been changed!");
                return;
            }

            if (modifyDrList.Where(s => MyUtility.Check.Empty(s["Roll"]) || MyUtility.Check.Empty(s["Dyelot"])).Any())
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# can't be empty!!");
                return;
            }

            // 修改到Roll或Dyelot 檢查
            string msg = string.Empty;
            foreach (var drModify in modifyDrList)
            {
                string original_Roll = drModify["roll", DataRowVersion.Original].ToString();
                string current_Roll = drModify["roll", DataRowVersion.Current].ToString();
                string original_Dyelot = drModify["dyelot", DataRowVersion.Original].ToString();
                string current_Dyelot = drModify["dyelot", DataRowVersion.Current].ToString();
                if (original_Roll != current_Roll || original_Dyelot != current_Dyelot)
                {
                    // 判斷 在 FtyInventory 是否存在
                    bool chkFtyInventory = PublicPrg.Prgs.ChkFtyInventory(MyUtility.Convert.GetString(drModify["POID"]), MyUtility.Convert.GetString(drModify["Seq1"]), MyUtility.Convert.GetString(drModify["Seq2"]), current_Roll, current_Dyelot, MyUtility.Convert.GetString(drModify["StockType"]));
                    if (!chkFtyInventory)
                    {
                        msg += $@"SP#: {drModify["poid"]} Seq#: {drModify["seq1"]}-{drModify["seq2"]} Roll#: {drModify["roll"]} Dyelot: {drModify["Dyelot"]}." + Environment.NewLine;
                    }
                }
            }

            if (!MyUtility.Check.Empty(msg))
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# already existed!!\r\n" + msg);
                return;
            }

            // CompleteTime is not null 則不可commit
            #region 檢查資料有任一筆WMS已完成
            string sqlcmdWMS = string.Empty;
            string errmsg = string.Empty;

            switch (this.gridAlias)
            {
                case "Receiving_Detail":
                    sqlcmdWMS = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.Receiving_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;

                case "TransferIn_Detail":
                    sqlcmdWMS = $@"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Dyelot
from dbo.TransferIn_Detail d  WITH (NOLOCK) 
where d.CompleteTime is not null
and exists(
	select 1 
    from #tmp s
	where s.ukey = d.ukey
)";
                    break;
            }

            if (!(result = MyUtility.Tool.ProcessWithDatatable(modifyDrList.CopyToDataTable(), string.Empty, sqlcmdWMS, out DataTable dtWMS)))
            {
                MyUtility.Msg.WarningBox(result.Messages.ToString());
                return;
            }
            else
            {
                if (dtWMS.Rows.Count > 0)
                {
                    foreach (DataRow tmp in dtWMS.Rows)
                    {
                        errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]}." + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("WMS system have finished it already, you cannot revise it." + Environment.NewLine + errmsg, "Warning");
                    return;
                }
            }
            #endregion

            #region 有任何交易紀錄都不能更改

            // 撈DB檢查是否有交易紀錄
            this.GetgridDyelotData();
            if (this.dtGridDyelot.Rows.Count > 0)
            {
                // 顯示有交易紀錄的訊息
                Class.MsgGridPrg form = new Class.MsgGridPrg(this.dtGridDyelot, "There are transaction records, cannot commit.");
                form.ShowDialog();

                // 還原回原本的資料
                foreach (var drModify in modifyDrList)
                {
                    bool existsDetail = this.dtGridDyelot.AsEnumerable()
                     .Where(s => s["poid"].Equals(drModify["poid", DataRowVersion.Original]) &&
                                 s["seq"].Equals(drModify["seq", DataRowVersion.Original]) &&
                                 s["roll"].Equals(drModify["roll", DataRowVersion.Original]) &&
                                 s["dyelot"].Equals(drModify["dyelot", DataRowVersion.Original])).Any();
                    if (existsDetail)
                    {
                        // 還原回原本的資料
                        drModify["Roll"] = drModify["Roll", DataRowVersion.Original];
                        drModify["dyelot"] = drModify["dyelot", DataRowVersion.Original];
                        drModify["ActualQty"] = drModify["ActualQty", DataRowVersion.Original];
                        string rate = MyUtility.GetValue.Lookup(string.Format(
                           @"select RateValue from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", drModify["pounit", DataRowVersion.Original], drModify["stockunit"]));
                        drModify["stockqty"] = MyUtility.Math.Round(decimal.Parse(drModify["ActualQty", DataRowVersion.Original].ToString()) * decimal.Parse(rate), 2);
                        drModify["FullDyelot"] = drModify["FullDyelot", DataRowVersion.Original];
                        drModify["FullRoll"] = drModify["FullRoll", DataRowVersion.Original];
                    }
                }
            }

            #endregion

            var allDatas = this.source.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            if (allDatas.GroupBy(o => new
            {
                POID = o["POID"].ToString(),
                Seq = o["Seq"].ToString(),
                Roll = o["Roll"].ToString(),
                Dyelot = o["Dyelot"].ToString(),
            })
                    .Select(g => new { g.Key.POID, g.Key.Seq, g.Key.Roll, g.Key.Dyelot, ct = g.Count() })
                    .Any(r => r.ct > 1))
            {
                var checkList = allDatas.GroupBy(o => new { POID = o["POID"].ToString(), Seq = o["Seq"].ToString(), Roll = o["Roll"].ToString(), Dyelot = o["Dyelot"].ToString() }).Select(g => new { g.Key.POID, g.Key.Seq, g.Key.Roll, g.Key.Dyelot, ct = g.Count() }).Where(o => o.ct > 1).ToList();

                List<string> duplicate_List = new List<string>();

                foreach (var item in checkList)
                {
                    duplicate_List.Add($"{item.POID}-{item.Seq}-{item.Roll}-{item.Dyelot}");
                }

                MyUtility.Msg.WarningBox(@"Roll# & Dyelot# already existed!!"
+ Environment.NewLine
+ "Duplicate list SP# - Seq - Roll# - Dyelot as below."
+ Environment.NewLine
+ duplicate_List.JoinToString(Environment.NewLine));
                return;
            }

            string sqlcmd;
            string sqlupd1 = string.Empty;
            string sqlupd2 = string.Empty;

            List<string> duplicateList = new List<string>();

            // 只修改ActualQty時，判斷Roll# & Dyelot#是否重複,須排除自己
            foreach (var drModify in modifyDrList)
            {
                // 只修改ActualQty時，判斷Roll# & Dyelot#是否重複,須排除自己
                sqlcmd = string.Format(
                    @"
select 1 from dbo.{6} WITH (NOLOCK) 
where id='{0}' and poid='{1}' and seq1='{2}' and seq2='{3}' and roll='{4}' and dyelot='{5}' and ( roll!='{4}' and dyelot!='{5}')
", this.docno, drModify["poid"], drModify["seq1"], drModify["seq2"], drModify["roll"], drModify["dyelot"], this.gridAlias);

                if (MyUtility.Check.Seek(sqlcmd, null))
                {
                    duplicateList.Add($"{drModify["poid"]}-{drModify["seq1"].ToString() + " " + drModify["seq2"].ToString()}-{drModify["roll"]}-{drModify["dyelot"]}");
                }
            }

            if (duplicateList.Count() > 0)
            {
                MyUtility.Msg.WarningBox(@"Roll# & Dyelot# already existed!!"
+ Environment.NewLine
+ "Duplicate list SP# - Seq - Roll# - Dyelot as below."
+ Environment.NewLine
+ duplicateList.JoinToString(Environment.NewLine));
                return;
            }

            DataTable dtFIR_Shadebone = new DataTable();
            dtFIR_Shadebone.Columns.Add("POID");
            dtFIR_Shadebone.Columns.Add("Seq1");
            dtFIR_Shadebone.Columns.Add("Seq2");
            dtFIR_Shadebone.Columns.Add("Roll");
            dtFIR_Shadebone.Columns.Add("Dyelot");
            dtFIR_Shadebone.Columns.Add("Result");

            // 修改到Roll或Dyelot時。因為主索引鍵被修改，需要檢查
            foreach (var drModify in modifyDrList)
            {
                string original_Roll = drModify["roll", DataRowVersion.Original].ToString();
                string current_Roll = drModify["roll", DataRowVersion.Current].ToString();

                string original_Dyelot = drModify["dyelot", DataRowVersion.Original].ToString();
                string current_Dyelot = drModify["dyelot", DataRowVersion.Current].ToString();

                if (original_Roll != current_Roll || original_Dyelot != current_Dyelot)
                {
                    string sqlCheckShadebandResult = $@"
select
    FIR.POID,
    FIR.Seq1,
    FIR.Seq2,
    fs.Roll,
    fs.Dyelot,
    fs.Result
from {this.gridAlias} sd with(nolock)
inner join PO_Supp_Detail psd with(nolock) on psd.ID = sd.PoId and psd.SEQ1 = sd.Seq1 and psd.SEQ2 = sd.Seq2
inner join FIR with (nolock) on FIR.ReceivingID = sd.ID and FIR.POID = sd.PoId and FIR.SEQ1 = sd.Seq1 and FIR.SEQ2 = sd.Seq2
inner join FIR_Shadebone fs with (nolock) on fs.id = FIR.ID
where sd.id = '{this.docno}'
and psd.FabricType = 'F'
and fs.Result <>''
and fs.roll='{original_Roll}'
and fs.dyelot='{original_Dyelot}'
";
                    if (MyUtility.Check.Seek(sqlCheckShadebandResult, out DataRow rowFIR_Shadebone))
                    {
                        DataRow dataRow = dtFIR_Shadebone.NewRow();
                        dataRow["POID"] = rowFIR_Shadebone["POID"];
                        dataRow["Seq1"] = rowFIR_Shadebone["Seq1"];
                        dataRow["Seq2"] = rowFIR_Shadebone["Seq2"];
                        dataRow["Roll"] = rowFIR_Shadebone["Roll"];
                        dataRow["Dyelot"] = rowFIR_Shadebone["Dyelot"];
                        dataRow["Result"] = rowFIR_Shadebone["Result"];
                        dtFIR_Shadebone.Rows.Add(dataRow);
                    }

                    // 只修改ActualQty時，判斷Roll# & Dyelot#是否重複,須排除自己
                    sqlcmd = string.Format(
                        @"
SELECT   [FirID]= f.ID
	    ,[Roll]= r.Roll
	    ,[Dyelot]= r.Dyelot
	    ,[StockQty]= r.{7}
	    ,r.Ukey
FROM {6} r
INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
INNER JOIN FIR f ON f.ReceivingID=r.ID AND f.POID=r.PoId AND f.SEQ1=r.Seq1 AND f.SEQ2=r.Seq2
WHERE r.ID='{0}' AND p.FabricType='F' AND p.FabricType='F' AND  roll='{1}' AND dyelot='{2}'
and r.seq1='{3}' and r.seq2='{4}' and r.poid='{5}' 
",
                        this.docno,
                        drModify["roll"],
                        drModify["dyelot"],
                        drModify["Seq1"],
                        drModify["Seq2"],
                        drModify["poid"],
                        this.gridAlias,
                        this.gridAlias.ToUpper().EqualString("RECEIVING_DETAIL") ? "StockQty" : "Qty");

                    if (MyUtility.Check.Seek(sqlcmd, null))
                    {
                        duplicateList.Add($"{drModify["poid"]} - {drModify["seq1"].ToString() + " " + drModify["seq2"].ToString()} - {drModify["roll"]} - {drModify["dyelot"]}");
                    }
                }
            }

            if (dtFIR_Shadebone.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid_LockScreen(dtFIR_Shadebone, msg: "Those fabric roll already completed shade band inspection, please check with QA team and revise inspection result to empty before modiy roll dyelot.", caption: "Warring");
                return;
            }

            if (duplicateList.Count() > 0)
            {
                MyUtility.Msg.WarningBox(@"Roll# & Dyelot# already existed!!"
+ Environment.NewLine
+ "Duplicate list SP# - Seq - Roll# - Dyelot as below."
+ Environment.NewLine
+ duplicateList.Distinct().JoinToString(Environment.NewLine));
                return;
            }

            foreach (var drModify in modifyDrList)
            {
                string original_Roll = drModify["roll", DataRowVersion.Original].ToString();
                string current_Roll = drModify["roll", DataRowVersion.Current].ToString();

                string original_Dyelot = drModify["dyelot", DataRowVersion.Original].ToString();
                string current_Dyelot = drModify["dyelot", DataRowVersion.Current].ToString();

                string original_Qty = "0";
                string current_Qty = "0";

                string firID = MyUtility.GetValue.Lookup($@"
SELECT   [FirID]=f.ID
FROM {this.gridAlias} r
INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
INNER JOIN FIR f ON f.ReceivingID=r.ID AND f.POID=r.PoId AND f.SEQ1=r.Seq1 AND f.SEQ2=r.Seq2
WHERE r.ID='{this.docno}' AND p.FabricType='F' AND ROLL='{drModify["roll", DataRowVersion.Original]}' AND  r.Dyelot='{drModify["dyelot", DataRowVersion.Original]}'
");

                if (this.gridAlias.ToUpper().EqualString("RECEIVING_DETAIL"))
                {
                    original_Qty = MyUtility.Convert.GetInt(MyUtility.Convert.GetDouble(drModify["ActualQty", DataRowVersion.Original])).ToString();
                    current_Qty = MyUtility.Convert.GetInt(MyUtility.Convert.GetDouble(drModify["ActualQty", DataRowVersion.Current])).ToString();

                    sqlupd1 += $@"
update dbo.receiving_detail 
set roll = '{drModify["roll"]}' 
,dyelot = '{drModify["dyelot"]}' 
,FullRoll = '{drModify["FullRoll"]}' 
,FullDyelot = '{drModify["FullDyelot"]}' 
,ActualQty = '{drModify["ActualQty"]}'
,StockQty  = '{drModify["stockqty"]}'
,ShipQty = '{drModify["ActualQty"]}'
where ukey = '{drModify["ukey"]}';";

                    sqlupd2 += $@"
update dbo.ftyinventory 
set roll = '{drModify["roll"]}'
,dyelot  = '{drModify["dyelot"]}'
,InQty   = '{drModify["stockqty"]}'
where poid ='{drModify["poid"]}' 
and seq1='{drModify["seq1"]}' and seq2='{drModify["seq2"]}' 
and roll='{original_Roll}' and dyelot='{original_Dyelot}' 
and stocktype = '{drModify["stocktype"]}';

UPDATE FIR_Shadebone SET 
Roll = '{drModify["roll"]}'
,Dyelot  = '{drModify["dyelot"]}'
,TicketYds   = '{drModify["ActualQty"]}'
,EditName = '{Env.User.UserID}'
,EditDate = GETDATE()
WHERE  roll='{original_Roll}' AND dyelot='{original_Dyelot}' AND ID='{firID}'
";
                }
                else if (this.gridAlias.ToUpper().EqualString("TRANSFERIN_DETAIL"))
                {
                    original_Qty = MyUtility.Convert.GetInt(MyUtility.Convert.GetDouble(drModify["Qty", DataRowVersion.Original])).ToString();
                    current_Qty = MyUtility.Convert.GetInt(MyUtility.Convert.GetDouble(drModify["Qty", DataRowVersion.Current])).ToString();

                    sqlupd1 += $@"
update dbo.TransferIn_detail 
set roll = '{drModify["roll"]}' 
,dyelot = '{drModify["dyelot"]}' 
,Qty = '{drModify["Qty"]}'
where ukey = '{drModify["ukey"]}';";

                    sqlupd2 += $@"
update dbo.ftyinventory 
set roll = '{drModify["roll"]}'
,dyelot  = '{drModify["dyelot"]}'
,InQty   = '{drModify["Qty"]}'
where poid ='{drModify["poid"]}' 
and seq1='{drModify["seq1"]}' and seq2='{drModify["seq2"]}' 
and roll='{original_Roll}' and dyelot='{original_Dyelot}' 
and stocktype = '{drModify["stocktype"]}';

UPDATE FIR_Shadebone SET 
Roll = '{drModify["roll"]}'
,Dyelot  = '{drModify["dyelot"]}'
,TicketYds   = '{drModify["Qty"]}'
,EditName = '{Env.User.UserID}'
,EditDate = GETDATE()
WHERE  roll='{original_Roll}' AND dyelot='{original_Dyelot}' AND ID='{firID}'
";
                }

                sqlupd2 += $@"
update dbo.MDivisionPoDetail
set InQty   = InQty + ({current_Qty} - {original_Qty})
where poid ='{drModify["poid"]}' 
and seq1='{drModify["seq1"]}' and seq2='{drModify["seq2"]}' 

if 'I' = '{drModify["stocktype"]}'
begin
    update dbo.MDivisionPoDetail
    set LInvQty= LInvQty + ({current_Qty} - {original_Qty})
    where poid ='{drModify["poid"]}' 
    and seq1='{drModify["seq1"]}' and seq2='{drModify["seq2"]}' 
end
";
            }

            DualResult result1, result2;

            // 檢查資料有任一筆WMS已完成
            string strFunction = (this.gridAlias.ToUpper() == "RECEIVING_DETAIL") ? "P07" : "P18";
            DataTable detailTable = new DataTable();
            if (modifyDrList.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).Any())
            {
                detailTable = modifyDrList.AsEnumerable().Where(x => !MyUtility.Check.Empty(x["SentToWMS"])).CopyToDataTable();
            }

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock(detailTable, detailTable, strFunction, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordListUnLock = new List<AutoRecord>();
            List<AutoRecord> autoRecordListRevise = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    if (!(result1 = DBProxy.Current.Execute(null, sqlupd1)))
                    {
                        throw result.GetException();
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Gensong_AutoWHFabric.Sent(false, detailTable, strFunction, EnumStatus.UnLock, EnumStatus.Unconfirm, typeCreateRecord: 1, autoRecord: autoRecordListUnLock);
                    Gensong_AutoWHFabric.Sent(false, detailTable, strFunction, EnumStatus.Revise, EnumStatus.Confirm, typeCreateRecord: 1, autoRecord: autoRecordListRevise);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                List<AutoRecord> autoRecordList = new List<AutoRecord>();
                Gensong_AutoWHFabric.Sent(false, detailTable, strFunction, EnumStatus.UnLock, EnumStatus.Unconfirm, typeCreateRecord: 1, autoRecord: autoRecordList);
                Gensong_AutoWHFabric.Sent(false, detailTable, strFunction, EnumStatus.UnLock, EnumStatus.Unconfirm, typeCreateRecord: 2, autoRecord: autoRecordList);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Gensong_AutoWHFabric.Sent(false, detailTable, strFunction, EnumStatus.UnLock, EnumStatus.Unconfirm, typeCreateRecord: 2, autoRecord: autoRecordListUnLock);
            Gensong_AutoWHFabric.Sent(false, detailTable, strFunction, EnumStatus.Revise, EnumStatus.Confirm, typeCreateRecord: 2, autoRecord: autoRecordListRevise);

            #region 更新FIR,AIR資料

            List<SqlParameter> fir_Air_Proce = new List<SqlParameter>();
            fir_Air_Proce.Add(new SqlParameter("@ID", this.docno));
            fir_Air_Proce.Add(new SqlParameter("@LoginID", Sci.Env.User.UserID));
            if (this.gridAlias.ToUpper().EqualString("RECEIVING_DETAIL"))
            {
                if (!(result = DBProxy.Current.Select(string.Empty, " exec dbo.insert_Air_Fir @ID,@LoginID", fir_Air_Proce, out DataTable[] airfirids)))
                {
                    this.ShowErr(result);
                    return;
                }

                if (airfirids[0].Rows.Count > 0 || airfirids[1].Rows.Count > 0)
                {
                    // 寫入PMSFile
                    string cmd = @"SET XACT_ABORT ON
";
                    var firinsertlist = airfirids[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["id"]));
                    if (firinsertlist.Any())
                    {
                        string firInsertIDs = firinsertlist.Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().JoinToString(",");
                        cmd += $@"
INSERT INTO SciPMSFile_FIR_Laboratory (ID)
select ID from FIR_Laboratory t WITH(NOLOCK) where id in ({firInsertIDs})
and not exists (select 1 from SciPMSFile_FIR_Laboratory s (NOLOCK) where s.ID = t.ID )
";
                    }

                    var firDeletelist = airfirids[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["deID"]));
                    if (firDeletelist.Any())
                    {
                        string firDeleteIDs = firDeletelist.Select(s => MyUtility.Convert.GetString(s["deID"])).Distinct().JoinToString(",");
                        cmd += $@"
Delete SciPMSFile_FIR_Laboratory where id in ({firDeleteIDs})
and ID NOT IN(select ID from FIR_Laboratory)
";
                    }

                    var airinsertlist = airfirids[1].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["id"]));
                    if (airinsertlist.Any())
                    {
                        string airInsertIDs = airinsertlist.Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().JoinToString(",");
                        cmd += $@"
INSERT INTO SciPMSFile_AIR_Laboratory (ID,POID,SEQ1,SEQ2)
select  ID,POID,SEQ1,SEQ2 from AIR_Laboratory t WITH(NOLOCK) where id in ({airInsertIDs})
and not exists (select 1 from SciPMSFile_AIR_Laboratory s WITH(NOLOCK) where s.ID = t.ID AND s.POID = t.POID AND s.SEQ1 = t.SEQ1 AND s.SEQ2 = t.SEQ2 )
";
                    }

                    var airDeletelist = airfirids[1].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["deID"]));
                    if (airDeletelist.Any())
                    {
                        string airDeleteIDs = airDeletelist.Select(s => MyUtility.Convert.GetString(s["deID"])).Distinct().JoinToString(",");
                        cmd += $@"
Delete a 
from SciPMSFile_AIR_Laboratory a
WHERE a.id in ({airDeleteIDs})
and NOT EXISTS(
    select 1 from AIR_Laboratory b
    where a.ID = b.ID AND a.POID=b.POID AND a.Seq1=b.Seq1 AND a.Seq2=b.Seq2    
)
";
                    }

                    result = DBProxy.Current.Execute(null, cmd);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            }
            else if (this.gridAlias.ToUpper().EqualString("TRANSFERIN_DETAIL"))
            {
                if (!(result = DBProxy.Current.Select(string.Empty, " exec dbo.insert_Air_Fir_TnsfIn @ID,@LoginID", fir_Air_Proce, out DataTable[] airfirids)))
                {
                    this.ShowErr(result);
                    return;
                }

                if (airfirids[0].Rows.Count > 0 || airfirids[1].Rows.Count > 0)
                {
                    // 寫入PMSFile
                    string cmd = @"SET XACT_ABORT ON
";
                    var firinsertlist = airfirids[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["id"]));
                    if (firinsertlist.Any())
                    {
                        string firInsertIDs = firinsertlist.Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().JoinToString(",");
                        cmd += $@"
INSERT INTO SciPMSFile_FIR_Laboratory (ID)
select ID from FIR_Laboratory t WITH(NOLOCK) where id in ({firInsertIDs})
and not exists (select 1 from SciPMSFile_FIR_Laboratory s (NOLOCK) where s.ID = t.ID )
";
                    }

                    var firDeletelist = airfirids[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["deID"]));
                    if (firDeletelist.Any())
                    {
                        string firDeleteIDs = firDeletelist.Select(s => MyUtility.Convert.GetString(s["deID"])).Distinct().JoinToString(",");
                        cmd += $@"
Delete SciPMSFile_FIR_Laboratory where id in ({firDeleteIDs})
and ID NOT IN(select ID from FIR_Laboratory)";
                    }

                    var airinsertlist = airfirids[1].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["id"]));
                    if (airinsertlist.Any())
                    {
                        string airInsertIDs = airinsertlist.Select(s => MyUtility.Convert.GetString(s["id"])).Distinct().JoinToString(",");
                        cmd += $@"
INSERT INTO SciPMSFile_AIR_Laboratory (ID,POID,SEQ1,SEQ2)
select  ID,POID,SEQ1,SEQ2 from AIR_Laboratory t WITH(NOLOCK) where id in ({airInsertIDs})
and not exists (select 1 from SciPMSFile_AIR_Laboratory s WITH(NOLOCK) where s.ID = t.ID AND s.POID = t.POID AND s.SEQ1 = t.SEQ1 AND s.SEQ2 = t.SEQ2 )
";
                    }

                    var airDeletelist = airfirids[1].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["deID"]));
                    if (airDeletelist.Any())
                    {
                        string airDeleteIDs = airDeletelist.Select(s => MyUtility.Convert.GetString(s["deID"])).Distinct().JoinToString(",");
                        cmd += $@"
Delete a 
from SciPMSFile_AIR_Laboratory a
where id in ({airDeleteIDs})
and NOT EXISTS(select 1 from AIR_Laboratory b    where a.ID = b.ID AND a.POID=b.POID AND a.Seq1=b.Seq1 AND a.Seq2=b.Seq2)
";
                    }

                    result = DBProxy.Current.Execute(null, cmd);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }
                }
            }
            #endregion

            DataTable dt;
            string selectCommand1 = string.Format(
                @"
SELECT DISTINCT a.POID, a.Seq1, a.Seq2, a.id 
FROM {0} a 
left join dbo.PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{1}' 
and p1.FabricType = 'F'",
                this.gridAlias,
                this.docno);

            if (!(result = DBProxy.Current.Select(null, selectCommand1, out dt)))
            {
                this.ShowErr(selectCommand1, result);
            }
            else
            {
                string sqlupdate = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    sqlupdate += string.Format(
                        @"
update FIR
set ArriveQty = (
    select sum({1})
    from {0} r
    where FIR.ReceivingID = r.Id 
    and FIR.POID = r.PoId
    and FIR.SEQ1 = r.Seq1 
    and FIR.SEQ2 = r.Seq2
)
from FIR
where ReceivingID = '{2}'
and POID = '{3}'
and SEQ1 = '{4}' and SEQ2 = '{5}'
",
                        this.gridAlias,
                        this.gridAlias.ToUpper().EqualString("RECEIVING_DETAIL") ? "StockQty" : "Qty",
                        dr["id"],
                        dr["Poid"],
                        dr["Seq1"],
                        dr["Seq2"]);
                }

                if (!(result = DBProxy.Current.Execute(string.Empty, sqlupdate)))
                {
                    this.ShowErr(sqlupdate, result);
                }

                this.btnCommit.Enabled = false;
                this.Close();
            }

            MyUtility.Msg.InfoBox("Commit successful");
        }
    }
}
