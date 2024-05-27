﻿using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_Detail_Detail : Win.Subs.Base
    {
        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Win.Subs.Base P10_Detail;
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        protected DataTable dtFtyinventory;
        private int Type;

        /// <inheritdoc/>
        public P10_Detail_Detail(DataRow master, DataTable detail, int type = 0)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.Type = type;
            if (type == 1)
            {
                this.Text = "P62_Detail_Detail";
            }
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(qty)", "selected = 1");
            if (!MyUtility.Check.Empty(localPrice) && !MyUtility.Check.Empty(this.dr_master["requestqty"].ToString()))
            {
                this.numRequestVariance.Value = Convert.ToDecimal(this.dr_master["requestqty"].ToString()) - Convert.ToDecimal(localPrice.ToString());
            }

            this.displayTotalQty.Value = localPrice.ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.displaySCIRefno.Text = this.dr_master["SCIRefno"].ToString();
            this.displayRefno.Text = this.dr_master["refno"].ToString();
            this.displaySPNo.Text = this.dr_master["poid"].ToString();
            this.displayColorID.Text = this.dr_master["colorid"].ToString();
            this.displaySizeSpec.Text = this.dr_master["sizespec"].ToString();
            this.displayDesc.Text = this.dr_master["description"].ToString();

            StringBuilder strSQLCmd = new StringBuilder();
            #region -- sqlcmd query --
            strSQLCmd.Append($@"
with cte as 
(
    select Dyelot
    , sum(inqty - OutQty + AdjustQty - ReturnQty) as GroupQty
    from dbo.FtyInventory a WITH (NOLOCK) 
    inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.id = a.POID and psd.seq1 = a.Seq1 and psd.seq2 = a.Seq2
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
    where poid = '{this.dr_master["poid"]}' 
    and Stocktype = 'B' 
    and inqty - OutQty + AdjustQty - ReturnQty > 0
    and psd.Refno = '{this.dr_master["Refno"]}' 
    and isnull(psdsC.SpecValue, '') = '{this.dr_master["colorid"]}' 
    {(this.Type == 0 ? " and a.Seq1 BETWEEN '00' AND '99'" : string.Empty)}--and a.Seq1 BETWEEN '00' AND '99'
    Group by Dyelot
) 
select 0 as selected 
       , id = '' 
       , PoId = psd.id
       , psd.Seq1
       , psd.Seq2
       , seq = concat(Ltrim(Rtrim(psd.seq1)), ' ', psd.Seq2)
       , psd.FabricType
       , psd.stockunit
       , [Description] = dbo.getmtldesc(psd.id,psd.seq1,psd.seq2,2,0)
       , Roll = Rtrim(Ltrim(c.Roll))
       , Dyelot = Rtrim(Ltrim(c.Dyelot))
       , Qty = 0.00
       , StockType = 'B'
       , ftyinventoryukey = c.ukey 
       , location = dbo.Getlocation(c.ukey) 
       , balanceqty = c.inqty-c.outqty + c.adjustqty - c.ReturnQty
       , c.inqty
       , c.outqty
       , c.adjustqty 
       , c.ReturnQty
       , [Tone] = c.Tone
       , [GMTWash] = isnull(GMTWash.val, '')
       , [Grade] = isnull(phy.Grade, '')
	   , [ActualWidth] = phy.ActualWidth
       , relax.Relaxtime
       , [RelaxEndTime] = format(fur.RelaxationEndTime,'yyyy/MM/dd hh:mm:ss')
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
inner join dbo.ftyinventory c WITH (NOLOCK) on c.poid = psd.id and c.seq1 = psd.seq1 and c.seq2 = psd.seq2 and c.stocktype = 'B'
inner join cte d on d.Dyelot=c.Dyelot
left join Fabric_UnrollandRelax fur on fur.Barcode = IIF(c.barcodeseq <> '',Concat (c.barcode, '-', c.barcodeseq),c.barcode )
outer apply(
    select top 1 [val] =  case  when sr.Status = 'Confirmed' then 'Done'
			                    when tt.Status = 'Confirmed' then 'Ongoing'
			                    else '' end
    from TransferToSubcon_Detail ttd with (nolock)
    inner join TransferToSubcon tt with (nolock) on tt.ID = ttd.ID
    left join  SubconReturn_Detail srd with (nolock) on srd.TransferToSubcon_DetailUkey = ttd.Ukey
    left join  SubconReturn sr with (nolock) on sr.ID = srd.ID and sr.Status = 'Confirmed'
    where   ttd.POID = c.POID and
			ttd.Seq1 = c.Seq1 and 
            ttd.Seq2 = c.Seq2 and
			ttd.Dyelot = c.Dyelot and 
            ttd.Roll = c.Roll and
			ttd.StockType = c.StockType and            
            tt.Subcon = 'GMT Wash'
) GMTWash
outer apply
(
	select 
	Grade
	, ActualWidth 
	from FIR f
	inner join FIR_Physical fp on fp.id = f.ID
	where f.poid = psd.id and f.SEQ1 =psd.seq1 and f.SEQ2 =psd.SEQ2 and fp.Roll =c.Roll and fp.Dyelot =c.Dyelot
)as phy
outer apply
(
    select fr.Relaxtime 
    from [ExtendServer].ManufacturingExecution.dbo.RefnoRelaxtime rr
    inner join [ExtendServer].ManufacturingExecution.dbo.FabricRelaxation fr on fr.id = rr.FabricRelaxationID
    where Refno = psd.Refno
)relax
Where psd.id = '{this.dr_master["poid"]}' and c.lock = 0 and c.inqty - c.outqty + c.adjustqty - c.ReturnQty > 0 
and psd.Refno='{this.dr_master["Refno"]}' and isnull(psdsC.SpecValue, '')='{this.dr_master["colorid"]}' {(this.Type == 0 ? " and ltrim(psd.seq1) between '01' and '99'" : string.Empty)}
order by d.GroupQty DESC,c.Dyelot,balanceqty DESC
");
            #endregion

            this.P10_Detail.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtFtyinventory))
            {
                if (this.dtFtyinventory.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtFtyinventory;
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            this.P10_Detail.HideWaitMessage();

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.gridRollNo.GetDataRow(this.gridRollNo.GetSelectedRowIndex())["qty"] = e.FormattedValue;
                        this.gridRollNo.GetDataRow(this.gridRollNo.GetSelectedRowIndex())["selected"] = true;
                        this.Sum_checkedqty();
                    }
                };

            this.gridRollNo.CellValueChanged += (s, e) =>
            {
                if (this.gridRollNo.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridRollNo.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balanceqty"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            this.gridRollNo.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridRollNo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridRollNo)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .Text("location", header: "Bulk Location", iseditingreadonly: true) // 2
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 3
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 4
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 5
                .Numeric("balanceqty", header: "Balance Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10) // 6
                .Numeric("qty", header: "Issue Qty", iseditable: true, decimal_places: 2, integer_places: 10, settings: ns) // 7
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) // 8
                .Text("Tone", header: "Shade Band" + Environment.NewLine + "Tone/Grp", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("GMTWash", header: "GMT Wash", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Grade", header: "Grade", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ActualWidth", header: "Act Width", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Relaxtime", header: "Relaxation", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 5, iseditingreadonly: true)
                .Text("RelaxEndTime", header: "Relax End Time", width: Widths.AnsiChars(20), iseditingreadonly: true)
                ;

            this.gridRollNo.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridRollNo.ValidateControl();
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
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty > balanceqty and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then balance qty!", "Warning");
                return;
            }

            dr2 = dtGridBS1.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.Select(string.Format(
                    "poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll = '{3}' and dyelot = '{4}'",
                    tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

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
