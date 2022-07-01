﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P38 : Win.Tems.QueryForm
    {
        private const byte UnLock = 0;
        private const byte Lock = 1;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        public P38(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.comboFIR.Items.AddRange(new string[] { "ALL", "Pass", "Fail", "Blank", "N/A" });
            this.comboFIR.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboStatus.SelectedIndex = 0;
            this.comboStockType.SelectedIndex = 0;
            this.comboDropDownList1.SelectedIndex = 0;
            Ict.Win.UI.DataGridViewTextBoxColumn columnStatus = new Ict.Win.UI.DataGridViewTextBoxColumn();
            DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings remark = new DataGridViewGeneratorTextColumnSettings();

            ns.CellMouseDoubleClick += (s, e) =>
                {
                    DataRow thisRow = this.gridMaterialLock.GetDataRow(e.RowIndex);
                    string dyelot = thisRow["dyelot"].ToString();
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    string tempstatus = thisRow["selected"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["dyelot"].ToString() == dyelot)
                        {
                            if (tempstatus == "0")
                            {
                                dr["selected"] = true;
                            }
                            else
                            {
                                dr["selected"] = false;
                            }
                        }
                    }
                };
            remark.CellValidating += (s, e) =>
             {
                 if (e.RowIndex != -1)
                 {
                     DataRow dr = this.gridMaterialLock.GetDataRow(e.RowIndex);
                     if (!MyUtility.Check.Empty(dr["Remark"]))
                     {
                         if (dr["Remark"].ToString().Length > 500)
                         {
                             MyUtility.Msg.WarningBox("<Remark> length cannot exceed 500");
                             e.Cancel = true;
                             return;
                         }
                     }
                 }
             };

            #region -- 設定Grid1的顯示欄位 --
            this.gridMaterialLock.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridMaterialLock.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridMaterialLock)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                 .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                  .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                  .Text("FabricType", header: "Material Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Text("roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: ns)
                  .Text("status", header: "Status", width: Widths.AnsiChars(10), iseditingreadonly: true).Get(out columnStatus)
                  .Date("WhseArrival", header: "Material ATA ", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .DateTime("lockdate", header: "Lock/Unlock" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("lockname", header: "Lock/Unlock" + Environment.NewLine + "Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .EditText("Remark", header: "Remark", width: Widths.AnsiChars(12), iseditingreadonly: false, settings: remark)
                  .Text("FIR", header: "FIR", width: Widths.AnsiChars(5), iseditingreadonly: true)
                  .Text("Scale", header: "Shade Band\r\nScale", width: Widths.AnsiChars(5), iseditingreadonly: true)
                  .Text("Tone", header: "Shade Band\r\nTone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Text("PointRate", header: "Point rate\n\rper 100yds", width: Widths.AnsiChars(2), iseditingreadonly: true)
                  .Text("WashLab Report", header: "WashLab Report", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("outqty", header: "Out Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("balanceqty", header: "Balance Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Text("stocktype", header: "Stocktype", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("location", header: "location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("description", header: "description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("styleid", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("colorid", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Date("earliest_BuyerDelivery", header: "Earliest" + Environment.NewLine + "BuyerDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Date("earliest_SciDelivery", header: "Earliest" + Environment.NewLine + "SciDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("brandid", header: "Brand", width: Widths.AnsiChars(12), iseditingreadonly: true)
                  .Text("factoryid", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  ;
            columnStatus.DefaultCellStyle.ForeColor = Color.Blue;
            this.gridMaterialLock.Columns["dyelot"].HeaderCell.Style.BackColor = Color.Orange;
            this.gridMaterialLock.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridMaterialLock.Columns["Remark"].DefaultCellStyle.ForeColor = Color.Red;

            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp1 = this.txtSP.Text.TrimEnd() + '%';
            string strMtlLocation = string.Empty;

            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.txtwkno.Text) && MyUtility.Check.Empty(this.txtReceivingid.Text)
                && this.dateATA.Value1.Empty() && this.dateATA.Value2.Empty())
            {
                MyUtility.Msg.WarningBox("SP# and WK NO and Receiving ID and ATA can't be empty!!");
                return;
            }

            if (!MyUtility.Check.Empty(this.txtMtlLocation.Text))
            {
                strMtlLocation = @"outer apply(
	select * from dbo.FtyInventory_Detail f2 WITH (NOLOCK)  
	inner join dbo.MtlLocation mtl WITH (NOLOCK)  on mtl.ID=f2.MtlLocationID and mtl.StockType=fi.StockType
	where fi.Ukey=Ukey 
)fi2";
            }

            strSQLCmd.Append($@"

--#tmp_FtyInventory
select 0 as [selected]
        , fi.POID
        , fi.seq1
        , fi.seq2
        , FabricType = case when pd.FabricType = 'F' then 'Fabric' when  pd.FabricType = 'A' then 'Accessory' end
        , fi.Roll
        , fi.Dyelot
        , iif(fi.Lock=0,'Unlocked','Locked') [status]
        , WhseArrival = case when pd.FabricType = 'F' then r.WhseArrival when  pd.FabricType = 'A' then pd.RevisedETA end 
        , fi.InQty
        , fi.OutQty
        , fi.AdjustQty
        , fi.ReturnQty
        , fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty as balanceqty
        , stocktype =  case fi.stocktype 
                        when 'B' then'Bulk' 
                        when 'I' then 'Inventory' 
                        else fi.StockType 
                       end 
        , fi.LockDate
        , LockName = (select id+'-'+name from dbo.pass1 WITH (NOLOCK) where id=fi.LockName) 
        , fi.ukey
        , [location] = dbo.Getlocation(fi.ukey)
        , [Description] = dbo.getMtlDesc(fi.poid,fi.seq1,fi.seq2,2,0)
        , pd.ColorID
        , o.styleid
        , o.BrandID
        , o.FactoryID
        , x.*
        , fi.Remark
		, pd.FabricType sFabricType
into #tmp_FtyInventory
from dbo.FtyInventory fi WITH (NOLOCK) 
left join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = fi.POID and pd.seq1 = fi.seq1 and pd.seq2  = fi.Seq2
left join View_WH_Orders o WITH (NOLOCK) on o.id = fi.POID
left join dbo.factory f WITH (NOLOCK) on o.FtyGroup=f.id
left join Receiving_Detail rd WITH (NOLOCK) on rd.PoId = fi.POID and rd.Seq1 = fi.seq1 and rd.seq2 = fi.seq2 and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
left join Receiving r WITH (NOLOCK) on r.id = rd.id
cross apply
(
	select  earliest_BuyerDelivery = min(o1.BuyerDelivery)  
            , earliest_SciDelivery = min(o1.SciDelivery)  
	from View_WH_Orders o1 WITH (NOLOCK) 
    where o1.POID = fi.POID and o1.Junk = 0
) x
{strMtlLocation}
where   f.MDivisionID = '{Env.User.Keyword}' 
        and fi.POID like @poid1 
");

            System.Data.SqlClient.SqlParameter sp1_1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@poid1",
            };

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            sp1_1.Value = sp1;
            cmds.Add(sp1_1);

            switch (this.comboStatus.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    strSQLCmd.Append(@" 
        and lock=1");
                    break;
                case 2:
                    strSQLCmd.Append(@" 
        and lock=0");
                    break;
            }

            switch (this.comboStockType.SelectedIndex)
            {
                case 0:
                    strSQLCmd.Append(@" 
        and (fi.stocktype='B' or fi.stocktype ='I')");
                    break;
                case 1:
                    strSQLCmd.Append(@" 
        and fi.stocktype='B'");
                    break;
                case 2:
                    strSQLCmd.Append(@" 
        and fi.stocktype='I'");
                    break;
            }

            if (!this.txtSeq.CheckSeq1Empty())
            {
                strSQLCmd.Append(string.Format(
                    @"
        and fi.seq1 = '{0}'", this.txtSeq.Seq1));
            }

            if (!this.txtSeq.CheckSeq2Empty())
            {
                strSQLCmd.Append(string.Format(
                    @" 
        and fi.seq2 = '{0}'", this.txtSeq.Seq2));
            }

            if (!MyUtility.Check.Empty(this.txtwkno.Text))
            {
                strSQLCmd.Append(string.Format(
                    @" 
and exists (select 1 from Export_Detail e where e.poid = fi.poid and e.seq1 = fi.seq1 and e.seq2 = fi.seq2 and e.id = '{0}')",
                    this.txtwkno.Text));
            }

            if (!MyUtility.Check.Empty(this.txtReceivingid.Text))
            {
                strSQLCmd.Append(string.Format(
                    @" 
and exists (select 1 from Receiving_Detail r where r.poid = fi.poid and r.seq1 = fi.seq1 and r.seq2 = fi.seq2 and r.Roll = fi.Roll and r.Dyelot = fi.Dyelot and r.id = '{0}' )",
                    this.txtReceivingid.Text));
            }

            if (!MyUtility.Check.Empty(this.dateATA.TextBox1.Value))
            {
                strSQLCmd.Append(string.Format(
                    @" 
and r.WhseArrival between '{0}' and '{1}'", this.dateATA.TextBox1.Text, this.dateATA.TextBox2.Text));
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList1.SelectedValue))
            {
                strSQLCmd.Append(string.Format(
                    @" 
and pd.FabricType in ({0})", this.comboDropDownList1.SelectedValue));
            }

            if (!MyUtility.Check.Empty(this.txtMtlLocation.Text))
            {
                strSQLCmd.Append($@"
and fi2.MtlLocationID ='{this.txtMtlLocation.Text}'");
            }

            strSQLCmd.Append($@"
--#tmpFirDetail
select * into #tmpFirDetail
from (
select	f.ID,
		rd.PoId,
		rd.Seq1,
		rd.Seq2,
		rd.Roll,
		rd.Dyelot,
		f.PhysicalEncode,
		f.Physical,
		f.WeightEncode,
		f.Weight,
		f.ShadebondEncode,
		f.ShadeBond,
		f.ContinuityEncode,
		f.Continuity,
		f.OdorEncode,
		f.Odor,
		f.Moisture,
		f.Result,
		f.nonContinuity,
		f.NonMoisture,
		f.nonOdor,
		f.Nonphysical,
		f.nonShadebond,
		f.nonWeight
from FIR f with (nolock)
inner join Receiving_Detail rd with (nolock) on f.ReceivingID = rd.Id and f.POID = rd.PoId and f.SEQ1 = rd.Seq1 and f.SEQ2 = rd.Seq2
where exists (select 1 from #tmp_FtyInventory tf where rd.POID = tf.POID and rd.SEQ1 = tf.Seq1 and rd.Seq2 = tf.Seq2 and rd.Roll = tf.Roll and rd.Dyelot = tf.Dyelot)
union all
select	f.ID,
		rd.PoId,
		rd.Seq1,
		rd.Seq2,
		rd.Roll,
		rd.Dyelot,
		f.PhysicalEncode,
		f.Physical,
		f.WeightEncode,
		f.Weight,
		f.ShadebondEncode,
		f.ShadeBond,
		f.ContinuityEncode,
		f.Continuity,
		f.OdorEncode,
		f.Odor,
		f.Moisture,
		f.Result,
		f.nonContinuity,
		f.NonMoisture,
		f.nonOdor,
		f.Nonphysical,
		f.nonShadebond,
		f.nonWeight
from FIR f with (nolock)
inner join TransferIn_Detail rd with (nolock) on f.ReceivingID = rd.Id and f.POID = rd.PoId and f.SEQ1 = rd.Seq1 and f.SEQ2 = rd.Seq2
where exists (select 1 from #tmp_FtyInventory tf where rd.POID = tf.POID and rd.SEQ1 = tf.Seq1 and rd.Seq2 = tf.Seq2 and rd.Roll = tf.Roll and rd.Dyelot = tf.Dyelot)
) a

update f set	f.Physical = Physical.Result,
				f.Weight = Weight.Result,
				f.ShadeBond = ShadeBond.Result,
				f.Continuity = Continuity.Result,
				f.Odor = Odor.Result,
				f.Moisture = Moisture.Result
from #tmpFirDetail f 
outer apply (select [Result] = case	when f.Nonphysical = 1 then 'N/A'
									when f.Physical <> '' then f.Physical
									when f.PhysicalEncode = 0 and exists(select 1 from FIR_Physical fp where f.id=fp.ID AND f.Dyelot = fp.Dyelot and f.Roll=fp.Roll) then 'Blank'
									else '' end) Physical 
outer apply (select [Result] = case	when f.nonWeight = 1 then 'N/A'
									when f.Weight <> '' then f.Weight
									when f.WeightEncode = 0 and exists(select 1 from FIR_Weight fw where f.id=fw.ID AND f.Dyelot =fw.Dyelot and f.Roll=fw.Roll) then 'Blank'
									else '' end) Weight 
outer apply (select [Result] = case	when f.nonShadebond = 1 then 'N/A'
									when f.ShadeBond <> '' then f.ShadeBond
									when f.ShadebondEncode = 0 and exists(select 1 from FIR_Shadebone fs where f.id=fs.ID AND f.Dyelot =fs.Dyelot and f.Roll=fs.Roll) then 'Blank'
									else '' end) ShadeBond 
outer apply (select [Result] = case	when f.nonContinuity = 1 then 'N/A'
									when f.Continuity <> '' then f.Continuity
									when f.ContinuityEncode = 0 and exists(select 1 from FIR_Continuity fc where f.id=fc.ID and f.Dyelot =fc.Dyelot and f.Roll=fc.Roll) then 'Blank'
									else '' end) Continuity
outer apply (select [Result] = case	when f.nonOdor = 1 then 'N/A'
									when f.Odor <> '' then f.Odor
									when f.OdorEncode = 0 and exists(select 1 from FIR_Odor fo where f.id=fo.ID and f.Dyelot=fo.Dyelot and f.Roll=fo.Roll) then 'Blank'
									else '' end) Odor
outer apply (select [Result] = case	when f.NonMoisture = 1 then 'N/A'
									when f.Moisture <> '' then f.Moisture
									when exists(select 1 from FIR_Odor fo where f.id=fo.ID and f.Dyelot=fo.Dyelot and f.Roll=fo.Roll) then 'Blank'
									else '' end) Moisture


update f set f.Result = case when Physical.Result = 'N/A' and Weight.Result = 'N/A' and ShadeBond.Result = 'N/A' and Continuity.Result = 'N/A' and Odor.Result = 'N/A' and Moisture.Result = 'N/A' then 'N/A'
						when f.Result <> '' then f.Result
						when Physical.Result = 'Fail' or Weight.Result = 'Fail' or ShadeBond.Result = 'Fail' or Continuity.Result = 'Fail' or Odor.Result = 'Fail' or Moisture.Result = 'Fail' then 'Fail'
						when Physical.Result in ('Blank', '') or Weight.Result in ('Blank', '') or ShadeBond.Result in ('Blank', '') or Continuity.Result in ('Blank', '') or Odor.Result in ('Blank', '') or Moisture.Result in ('Blank', '') then 'Blank'
						else 'Pass' end
from #tmpFirDetail f
outer apply (select [Result] = case when f.Physical <> '' then f.Physical
									else FIRST_VALUE(f.Physical) OVER (partition by f.PoId, f.Seq1, f.Seq2, f.Roll 
																		ORDER BY case	when f.Physical = 'Fail' then 0 
																						when f.Physical = 'Blank' then 1 
																						when f.Physical = 'Pass' then 2 
																						else 3 end
								) end
			 ) Physical
outer apply (select [Result] = case when f.Weight <> '' then f.Weight
									else FIRST_VALUE(f.Weight) OVER (partition by f.PoId, f.Seq1, f.Seq2, f.Roll 
																		ORDER BY case	when f.Weight = 'Fail' then 0 
																						when f.Weight = 'Blank' then 1 
																						when f.Weight = 'Pass' then 2 
																						else 3 end
								) end
			 ) Weight
outer apply (select [Result] = case when f.ShadeBond <> '' then f.ShadeBond
									else FIRST_VALUE(f.ShadeBond) OVER (partition by f.PoId, f.Seq1, f.Seq2, f.Roll 
																		ORDER BY case	when f.ShadeBond = 'Fail' then 0 
																						when f.ShadeBond = 'Blank' then 1 
																						when f.ShadeBond = 'Pass' then 2 
																						else 3 end
								) end
			 ) ShadeBond
outer apply (select [Result] = case when f.Continuity <> '' then f.Continuity
									else FIRST_VALUE(f.Continuity) OVER (partition by f.PoId, f.Seq1, f.Seq2, f.Roll 
																		ORDER BY case	when f.Continuity = 'Fail' then 0 
																						when f.Continuity = 'Blank' then 1 
																						when f.Continuity = 'Pass' then 2 
																						else 3 end
								) end
			 ) Continuity
outer apply (select [Result] = case when f.Odor <> '' then f.Odor
									else FIRST_VALUE(f.Odor) OVER (partition by f.PoId, f.Seq1, f.Seq2, f.Roll 
																		ORDER BY case	when f.Odor = 'Fail' then 0 
																						when f.Odor = 'Blank' then 1 
																						when f.Odor = 'Pass' then 2 
																						else 3 end
								) end
			 ) Odor
outer apply (select [Result] = case when f.Moisture <> '' then f.Moisture
									else FIRST_VALUE(f.Moisture) OVER (partition by f.PoId, f.Seq1, f.Seq2, f.Roll 
																		ORDER BY case	when f.Moisture = 'Fail' then 0 
																						when f.Moisture = 'Blank' then 1 
																						when f.Moisture = 'Pass' then 2 
																						else 3 end
								) end
			 ) Moisture

--#tmp_FIR_Result1
select  distinct
		PoId,
		Seq1,
		Seq2,
		Roll,
		Dyelot,
		[Result] = FIRST_VALUE(Result) OVER (partition by PoId, Seq1, Seq2, Roll, Dyelot order by case	when Result = 'Fail' then 0 
																										when Result = 'Blank' then 1 
																										when Result = 'Pass' then 2 
																										else 3 end)
into #tmp_FIR_Result1
from #tmpFirDetail

--#tmp_FIR_3
select f.POID,f.SEQ1,f.SEQ2,fi.Dyelot,fi.Roll,Scale = max(fs.Scale),Tone = max(fs.Tone)
into #tmp_FIR_3
from #tmp_FtyInventory fi
inner join fir f on f.POID= fi.POID	and f.SEQ1 =fi.Seq1 and f.SEQ2 = fi.Seq2 
inner join FIR_Shadebone fs on f.id=fs.ID AND F.ShadebondEncode=1 and fi.Dyelot =fs.Dyelot and fi.Roll=fs.Roll
group by  f.POID,f.SEQ1,f.SEQ2,fi.Dyelot,fi.Roll 

--#tmp_WashLab
select distinct f.POID,f.SEQ1,f.SEQ2
	,FLResult = case when fl >0 then 'Fail' when fl is null then null else 'Pass' end
	,ovenResult = case when oven>1 then 'Fail' when oven is null then null else 'Pass' end
	,cfResult = case when cf>1 then 'Fail' when cf is null then null else 'Pass' end
into #tmp_WashLab
from
(
	select f.POID,f.SEQ1,f.SEQ2
		, round(convert(float, sum(iif(fl = 'Fail',1,0))) / convert(float,(count(*))),2) fl
		, round(convert(float, sum(iif(oven = 'Fail',1,0))) / convert(float,(count(*))),2) oven
		, round(convert(float, sum(iif(cf = 'Fail',1,0))) / convert(float,(count(*))),2) cf
	from (
		SELECT distinct f.POID,f.SEQ1,f.SEQ2,FL.Result as fl ,O.Result as oven ,C.Result as cf
		FROM fir f
		left join FIR_Laboratory FL on f.POID= fl.POID and f.seq1= fl.SEQ1 and f.seq2= fl.SEQ2
		LEFT JOIN Oven O ON O.POID = FL.POID AND O.Status= 'Confirmed'
		LEFT JOIN ColorFastness C ON C.POID = FL.POID AND C.Status= 'Confirmed' 
		inner join #tmp_FtyInventory fi on f.POID= fi.POID	and f.SEQ1 =fi.Seq1 and f.SEQ2 = fi.Seq2 
	) f
	group by f.POID,f.SEQ1,f.SEQ2
)f

--#tmp_Air
select distinct a.POID,a.SEQ1,a.SEQ2,a.Result
into #tmp_Air
from AIR a
inner join #tmp_FtyInventory fi on a.POID= fi.POID	and a.SEQ1 =fi.Seq1 and a.SEQ2 = fi.Seq2  

--#tmp_Air_Lab
select distinct a.POID,a.SEQ1,a.SEQ2,a.Result
into #tmp_Air_Lab
from AIR_Laboratory a
inner join #tmp_FtyInventory fi on a.POID= fi.POID	and a.SEQ1 =fi.Seq1 and a.SEQ2 = fi.Seq2  

--#tmp_PointRate
SELECT POID,seq1,seq2,roll,Dyelot ,[PointRate]=ISNULL(Cast(FIR.PointRate as varchar),'Blank')
INTO #tmp_PointRate
FROM #tmp_FtyInventory t
OUTER APPLY(
	select fp.PointRate
	from FIR f
	left join FIR_Physical fp on fp.ID = f.ID
	WHERE 	f.POID = t.POID
		and f.SEQ1 = t.Seq1
		and f.SEQ2 = t.Seq2
		and fp.Roll = t.Roll
		and fp.Dyelot = t.Dyelot
)FIR


select distinct fi.*
	 ,[FIR] = case when fi.sFabricType='A' then iif(Air.Result ='','Blank',Air.Result)
				   else FIR_Result1.Result end
		,[PointRate]=IIF(fi.FabricType='Accessory','',PointRate.PointRate)
		,[WashLab Report] = case when fi.sFabricType='A' then iif(Air_Lab.Result='','Blank',Air_Lab.Result)
							when WashLab.FLResult='Fail' or WashLab.ovenResult='Fail' or WashLab.cfResult='Fail'
								then 'Fail'
							when WashLab.FLResult is null and WashLab.ovenResult is null and WashLab.cfResult is null 
								then 'Blank'
							else 'Pass' end 
        ,f3.Scale, f3.Tone
from #tmp_FtyInventory fi
left join #tmp_FIR_Result1 FIR_Result1 on FIR_Result1.POID=fi.POID	and FIR_Result1.SEQ1 = fi.Seq1 and FIR_Result1.SEQ2 = fi.Seq2 and FIR_Result1.Dyelot=fi.Dyelot AND FIR_Result1.Roll=fi.Roll 
left join #tmp_FIR_3 f3 on f3.POID=fi.POID	and f3.SEQ1 = fi.Seq1 and f3.SEQ2 = fi.Seq2 and f3.Dyelot=fi.Dyelot AND f3.Roll=fi.Roll
left join #tmp_WashLab WashLab on WashLab.POID= fi.POID	and WashLab.SEQ1 =fi.Seq1 and WashLab.SEQ2 = fi.Seq2 
left join #tmp_Air Air on Air.POID= fi.POID	and Air.SEQ1 =fi.Seq1 and Air.SEQ2 = fi.Seq2  
left join #tmp_Air_Lab Air_Lab on Air_Lab.POID= fi.POID	and Air_Lab.SEQ1 =fi.Seq1 and Air_Lab.SEQ2 = fi.Seq2  
left join #tmp_PointRate PointRate on PointRate.POID=fi.POID	and PointRate.SEQ1 = fi.Seq1 and PointRate.SEQ2 = fi.Seq2 and PointRate.Roll=fi.Roll and PointRate.Dyelot=fi.Dyelot   

drop table #tmp_FtyInventory,#tmp_FIR_Result1,#tmp_WashLab,#tmp_Air,#tmp_Air_Lab,#tmp_PointRate,#tmpFirDetail
");

            DualResult result;
            this.ShowWaitMessage("Data Loading...");
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out DataTable dtData))
            {
                if (dtData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = dtData;
                dtData.DefaultView.Sort = "poid,seq1,seq2,dyelot,roll,stocktype";
                this.listControlBindingSource1.Filter = this.comboFIR.Text == "ALL" ? string.Empty : $"FIR = '{this.comboFIR.Text}'";
            }
            else
            {
                this.ShowErr(result);
            }

            this.HideWaitMessage();
        }

        private void BtnLock_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            if (this.CheckStatus(Lock))
            {
                this.LockUnlock(Lock);
            }
            else
            {
                MyUtility.Msg.InfoBox("It cannot be Lock.");
            }
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            if (this.CheckStatus(UnLock))
            {
                this.LockUnlock(UnLock);
            }
            else
            {
                MyUtility.Msg.InfoBox("It cannot be UnLock.");
            }
        }

        private bool CheckStatus(byte flag)
        {
            bool check = true;
            string strCheckStatus = string.Empty;
            #region 確認 Lock Or UnLock
            switch (flag)
            {
                case UnLock:
                    strCheckStatus = "UnLocked";
                    break;
                case Lock:
                    strCheckStatus = "Locked";
                    break;
            }
            #endregion
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            check = !dt.AsEnumerable().Any(row => row["status"].EqualString(strCheckStatus) && row["Selected"].EqualString("1"));
            return check;
        }

        private void LockUnlock(byte flag)
        {
            if (!this.gridMaterialLock.ValidateControl())
            {
                MyUtility.Msg.WarningBox("grid1.ValidateControl failed");
            }

            this.gridMaterialLock.EndEdit();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            dt.AcceptChanges();

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] find;
            DialogResult dResult;

            string keyword;
            if (flag == 1)
            {
                keyword = "Lock";
            }
            else
            {
                keyword = "Unlock";
            }

            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dResult = MyUtility.Msg.QuestionBox(string.Format("Are you sure to {0} it?", keyword), "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (dResult == DialogResult.No)
            {
                return;
            }

            StringBuilder sqlcmd = new StringBuilder();
            foreach (DataRow item in find)
            {
                sqlcmd.Append(string.Format(@"update dbo.ftyinventory set lock={1},lockname='{2}',lockdate=GETDATE(),Remark='{3}' where ukey ={0};", MyUtility.Convert.GetLong(item["ukey"]), flag, Env.User.UserID, item["Remark"]));
            }

            DualResult result = DBProxy.Current.Execute(null, sqlcmd.ToString());
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                this.SendExcel();
                MyUtility.Msg.InfoBox($"{keyword} successful!!");
            }

            this.BtnQuery_Click(null, null);
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P38");

            if (this.SaveExcel(dt, strExcelName))
            {
                strExcelName.OpenFile();
            }
        }

        private void SendExcel()
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Where(row => row["selected"].EqualDecimal(1)).CopyToDataTable();

            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P38");

            if (this.SaveExcel(dt, strExcelName))
            {
                string strSendAccount = "select * from MailTo where Description='Material locked/Unlocked'";

                DualResult result = DBProxy.Current.Select(string.Empty, strSendAccount, out DataTable dtSendAccount);
                if (result)
                {
                    if (dtSendAccount != null && dtSendAccount.Rows.Count > 0)
                    {
                        string mailto = dtSendAccount.Rows[0]["ToAddress"].ToString();
                        string mailCC = dtSendAccount.Rows[0]["CCAddress"].ToString();
                        string subject = dtSendAccount.Rows[0]["Subject"].ToString();
                        string content = dtSendAccount.Rows[0]["Content"].ToString();
                        var email = new MailTo(Env.Cfg.MailFrom, mailto, mailCC, subject, strExcelName, content, false, true);
                        email.ShowDialog(this);
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                }
            }
        }

        private bool SaveExcel(DataTable printData, string strExcelName)
        {
            string sql = @"
select t.POID
	   , t.Seq1+' '+t.Seq2
	   , t.Roll
	   , t.Dyelot
	   , t.stocktype
	   , t.status
	   , t.InQty
	   , t.OutQty
	   , t.balanceqty
	   , t.location
	   , t.Description
	   , t.StyleID
	   , t.ColorID
	   , t.earliest_BuyerDelivery
	   , t.earliest_SciDelivery
	   , t.BrandID
	   , t.FactoryID
       , t.Remark
	   , LockDate = f.LockDate
	   , LockName = f.LockName
from #tmp t
inner join ftyinventory f with (NoLock) on t.ukey = f.ukey";

            DualResult result = MyUtility.Tool.ProcessWithDatatable(printData, string.Empty, sql, out DataTable k, "#Tmp");

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }
            else if (k.Rows.Count == 0)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P38.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(k, string.Empty, "Warehouse_P38.xltx", 2, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            #endregion

            return true;
        }

        private void ComboFIR_TextChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboFIR.Text))
            {
                return;
            }

            this.listControlBindingSource1.Filter = this.comboFIR.Text == "ALL" ? string.Empty : $"FIR = '{this.comboFIR.Text}'";
        }
    }
}
