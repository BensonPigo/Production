﻿using System;
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
using Sci.Production.PublicPrg;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Warehouse
{
    public partial class P07_ModifyRollDyelot : Sci.Win.Subs.Base
    {
        DataTable source;
        DataTable dtGridDyelot;
        string docno = "";
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        Ict.Win.UI.DataGridViewTextBoxColumn col_roll;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dyelot;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_ActQty;

        public P07_ModifyRollDyelot(object data, string data2)
        {
            InitializeComponent();
            source = (DataTable)data;
            docno = data2;
            this.Text += " - " + docno;
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();            
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            listControlBindingSource1.DataSource = source;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.DataGridViewGeneratorNumericColumnSettings actqty = new DataGridViewGeneratorNumericColumnSettings();
            actqty.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["ActualQty"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue) return;
                if (this.EditMode && e.FormattedValue != null)
                {
                    dr["Actualqty"] = e.FormattedValue;
                    if (!MyUtility.Check.Empty(dr["pounit"]) && !MyUtility.Check.Empty(dr["stockunit"]))
                    {
                        string rate = MyUtility.GetValue.Lookup(string.Format(@"select RateValue from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", dr["pounit"], dr["stockunit"]));
                        dr["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                    }
                }
            };

            //設定Grid1的顯示欄位
            this.gridModifyRoll.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridModifyRoll)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: false).Get(out col_roll)    //3
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: false).Get(out col_dyelot)   //4
            .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(11), iseditingreadonly: false, decimal_places: 2, integer_places: 10, maximum: 999999999.99M, minimum: 0, settings: actqty).Get(out col_ActQty)    //5
            .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)    //6
            .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))    //8
            .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)   //9
            .Text("Location", header: "Location", iseditingreadonly: true)    //10
            .Text("remark", header: "Remark", iseditingreadonly: true)    //11
            ;     //

            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            
            Helper.Controls.Grid.Generator(this.gridDyelot)
            .Date("IssueDate", header: "IssueDate", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("ID", header: "ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("Name", header: "Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Numeric("InQty", header: "InQty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("OutQty", header: "OutQty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("remark", header: "Remark", iseditingreadonly: true,width:Widths.AnsiChars(15))
            .Text("Location", header: "Location", iseditingreadonly: true)
            .Numeric("balance", header: "Balance", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            ;     //

            this.LoadDate();
            this.setCloumn();
            this.changeeditable();
        }

        private void changeeditable()
        {
            #region roll
            col_roll.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
            };
            col_roll.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            #endregion
            #region dyelot
            col_dyelot.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
            };
            col_dyelot.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            #endregion

            #region ActQty
            col_ActQty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                }
            };
            col_ActQty.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

                if (dtGridDyelot.Select($"poid = '{dr["poid"]}' and seq = '{dr["seq"]}' and roll = '{dr["roll"]}' and dyelot = '{dr["dyelot"]}' ").Length > 0)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void LoadDate()
        {
            this.GetgridDyelotData();
            this.btnCommit.Enabled = false;
        }

        private void setCloumn()
        {
            foreach (DataGridViewRow item in this.gridModifyRoll.Rows)
            {
                bool existsDetail = dtGridDyelot.AsEnumerable()
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
                string.Format(@"
SELECT DISTINCT POID,Seq1,Seq2,Roll,Dyelot,id into #tmp FROM Receiving_Detail WHERE id = '{0}'

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
		, 0 as inqty,sum(b.Qty) outqty ,0 as adjust, remark ,'' location
		from IssueLack a WITH (NOLOCK) , IssueLack_Detail b WITH (NOLOCK) 
		inner join #tmp t on b.poid = t.PoId and b.seq1 = t.Seq1 and b.seq2 = t.Seq2 and b.Roll = t.Roll and b.Dyelot = t.Dyelot
		where Status in ('Confirmed','Closed') and a.id = b.id
		group by a.id, b.poid, b.seq1,b.Seq2, b.Roll,b.Dyelot, remark  ,a.IssueDate,a.FabricType       
		                                                        
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
		where Status='Confirmed' and a.id = b.id
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
            "
                , docno);
            #endregion

            DualResult result;
            this.ShowWaitMessage("Data Loading...");
            if (!(result = DBProxy.Current.Select(null, selectCommand1, out dtGridDyelot)))
            {
                ShowErr(selectCommand1, result);
            }

            this.HideWaitMessage();
        }

        private void grid1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dtGridDyelot == null)
            {
                this.GetgridDyelotData();
            }

            if (dtGridDyelot.Rows.Count == 0)
            {
                return;
            }

            if (e.RowIndex == -1) return;
            DataRow dr = gridModifyRoll.GetDataRow(e.RowIndex);

            // 上下grid連動, Balance需要依照IssueDate , ID 排序後重新計算
            DataTable dt = new DataTable();
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtGridDyelot
 , string.Empty
 , $@"
            select  IssueDate,inqty = iif(adjust>0,inqty+adjust,inqty),outqty = iif(adjust < 0,abs(adjust) + abs(outqty), abs(outqty))
            ,adjust,id,Remark,location,name,POID,Seq1,Seq2, Roll,Dyelot,[Seq] 
            ,[balance] = sum(inqty - abs(outqty) + adjust) over (order by convert(date,IssueDate),convert(varchar(15),id))
            from #tmp 
            where Poid='{dr["poid"]}' and seq1='{dr["seq1"]}' and seq2='{dr["seq2"]}'
            and roll='{dr["roll"]}' and dyelot='{dr["dyelot"]}'
            group by IssueDate,inqty,outqty,adjust,id,Remark,location,name,POID,Seq1,Seq2, Roll,Dyelot,Seq
            order by IssueDate,id,name
            ", out dt);
            if (dt.Rows.Count == 0)
            {
                gridDyelot.DataSource = null;
            }
            else
            {
                gridDyelot.DataSource = dt;
            }
            
            
            gridDyelot.AutoResizeColumns();

        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            var modifyDrList = source.AsEnumerable().Where(s => s.RowState == DataRowState.Modified);
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

            if (modifyDrList.GroupBy(s => new { Roll = s["Roll"].ToString(), Dyelot = s["Dyelot"].ToString() })
                .Select(g => new { g.Key.Roll, g.Key.Dyelot, ct = g.Count() }).Any(r => r.ct > 1))
            {
                MyUtility.Msg.WarningBox("Roll# & Dyelot# can not  duplicate!!");
                return;
            }            

            string sqlcmd;
            string sqlupd1 = string.Empty;
            string sqlupd2 = string.Empty;

            foreach (var drModify in modifyDrList)
            {
                // 只修改ActualQty時，判斷Roll# & Dyelot#是否重複,須排除自己
                sqlcmd = string.Format(@"
select 1 from dbo.Receiving_Detail WITH (NOLOCK) 
where id='{0}' and poid='{1}' and seq1='{2}' and seq2='{3}' and roll='{4}' and dyelot='{5}' and ( roll!='{4}' and dyelot!='{5}')
"                    , docno, drModify["poid"], drModify["seq1"], drModify["seq2"], drModify["roll"], drModify["dyelot"]);
                if (MyUtility.Check.Seek(sqlcmd, null))
                {
                    MyUtility.Msg.WarningBox("Roll# & Dyelot# already existed!!");
                    return;
                }
                string Original_Roll = drModify["roll", DataRowVersion.Original].ToString();
                string Current_Roll = drModify["roll", DataRowVersion.Current].ToString();

                string Original_Dyelot = drModify["dyelot", DataRowVersion.Original].ToString();
                string Current_Dyelot = drModify["dyelot", DataRowVersion.Current].ToString();

                //修改到Roll或Dyelot時。因為主索引鍵被修改，需要檢查
                if (Original_Roll != Current_Roll || Original_Dyelot!= Current_Dyelot)
                {

                    sqlcmd = string.Format(@"
DECLARE @FIR_Shadebone_ID bigint

SELECT   [FirID]=f.ID
		,[Roll]=r.Roll
		,[Dyelot]=r.Dyelot
		,[StockQty]=r.StockQty
		,r.Ukey
INTO #tmpReceiving_Detail
FROM Receiving_Detail r
INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
INNER JOIN FIR f ON f.ReceivingID=r.ID AND f.POID=r.PoId AND f.SEQ1=r.Seq1 AND f.SEQ2=r.Seq2
WHERE r.ID='{0}' AND p.FabricType='F'

SELECT @FIR_Shadebone_ID=FirID FROM #tmpReceiving_Detail

SELECT 1
FROM FIR_Shadebone
WHERE roll='{1}' AND dyelot='{2}' AND ID=@FIR_Shadebone_ID

DROP TABLE #tmpReceiving_Detail
", docno, drModify["roll"], drModify["dyelot"]);
                    if (MyUtility.Check.Seek(sqlcmd, null))
                    {
                        MyUtility.Msg.WarningBox("Roll# & Dyelot# already existed!!");
                        return;
                    }

                }


                sqlupd1 += $@"
update dbo.receiving_detail 
set roll = '{drModify["roll"]}' 
,dyelot = '{drModify["dyelot"]}' 
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
and roll='{Original_Roll}' and dyelot='{Original_Dyelot}' 
and stocktype = '{drModify["stocktype"]}';


DECLARE @FIR_Shadebone_ID bigint

SELECT   [FirID]=f.ID
		,[Roll]=r.Roll
		,[Dyelot]=r.Dyelot
		,[StockQty]=r.StockQty
		,r.Ukey
INTO #tmpReceiving_Detail
FROM Receiving_Detail r
INNER JOIN PO_Supp_Detail p ON r.PoId=p.ID AND r.Seq1=p.SEQ1 AND r.Seq2=p.SEQ2 
INNER JOIN FIR f ON f.ReceivingID=r.ID AND f.POID=r.PoId AND f.SEQ1=r.Seq1 AND f.SEQ2=r.Seq2
WHERE r.ID='{docno}' AND p.FabricType='F'

SELECT @FIR_Shadebone_ID=FirID FROM #tmpReceiving_Detail

UPDATE FIR_Shadebone SET 
Roll = '{drModify["roll"]}'
,Dyelot  = '{drModify["dyelot"]}'
,TicketYds   = '{drModify["ActualQty"]}'
,EditName = '{Sci.Env.User.UserID}'
,EditDate = GETDATE()
WHERE  roll='{Original_Roll}' AND dyelot='{Original_Dyelot}' AND ID=@FIR_Shadebone_ID
";
            }

            
            DualResult result1, result2;

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result1 = DBProxy.Current.Execute(null, sqlupd1)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd1, result1);
                        return;
                    }

                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2, result2);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Commit successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            DataTable dt;
            DualResult result;
            string selectCommand1 = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.shipqty
,a.Weight
,a.ActualWeight
,a.Roll
,a.Dyelot
,a.ActualQty
,a.PoUnit
,a.StockQty
,a.StockUnit
,a.StockType
,a.Location
,a.remark
,a.ukey
from dbo.Receiving_Detail a WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}' and p1.FabricType='F'", docno);

            if (!(result = DBProxy.Current.Select(null, selectCommand1, out dt)))
            {
                ShowErr(selectCommand1, result);
            }
            else
            {
                string sqlupdate = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    sqlupdate += $@"
update FIR
set ArriveQty = (select sum(StockQty) 
from Receiving_Detail r
where FIR.ReceivingID=r.Id and FIR.POID=r.PoId 
and FIR.SEQ1=r.Seq1 and FIR.SEQ2=r.Seq2)
from FIR 
where ReceivingID='{dr["id"]}'
and POID='{dr["Poid"]}'
and SEQ1='{dr["Seq1"]}' and SEQ2='{dr["Seq2"]}'
";
                }
                if (!(result = DBProxy.Current.Execute(string.Empty,sqlupdate)))
                {
                    ShowErr(sqlupdate, result);
                }

                source = dt;
                gridModifyRoll.DataSource = dt;
                this.LoadDate();

                this.Close();
            }
        }
    }
}
