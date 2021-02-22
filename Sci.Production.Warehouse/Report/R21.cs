using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R21 : Win.Tems.PrintForm
    {
        private string StartSPNo;
        private string EndSPNo;
        private string MDivision;
        private string Factory;
        private string StartRefno;
        private string EndRefno;
        private string Color;
        private string MT;
        private string MtlTypeID;
        private string ST;
        private string WorkNo;
        private string location1;
        private string location2;
        private bool bulk;
        private bool sample;
        private bool material;
        private bool smtl;
        private bool complete;
        private DateTime? BuyerDelivery1;
        private DateTime? BuyerDelivery2;
        private DateTime? ETA1;
        private DateTime? ETA2;
        private DateTime? arriveWH1;
        private DateTime? arriveWH2;
        private string sqlcolumn = @"select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
    ,[Category] = case when o.Category='B'then'Bulk'
						when o.Category='G'then'Garment'
						when o.Category='M'then'Material'
						when o.Category='S'then'Sample'
						when o.Category='T'then'Sample mtl.'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='' then'Bulk fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='D' then'Dev. sample fc'
						when isnull(o.Category,'')=''and isnull(o.ForecastSampleGroup,'')='S' then'Sa. sample fc'
					end
	,[OrderType] = o.OrderTypeID
	,[WeaveType] = d.WeaveTypeID
    ,[BuyerDelivery]=o.BuyerDelivery
    ,[MaterialComplete] = case when psd.Complete = 1 then 'Y' else '' end
    ,[ETA] = psd.FinalETA
    ,[ArriveWHDate] = stuff((
                    select distinct concat(char(10),isnull(Format(a.date,'yyyy/MM/dd'),'　'))
                    from (
	                    select date = Export.whsearrival
	                    from Export_Detail with (nolock) 
	                    inner join Export with (nolock) on Export.ID = Export_Detail.ID
	                    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2

	                    union all

	                    select date = ts.IssueDate
	                    from TransferIn ts with (nolock) 
	                    inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	                    and ts.Status='Confirmed'

                        union all

                        select date = r.WhseArrival
                        from Receiving r with (nolock) 
                        inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	                    where r.Status = 'Confirmed' 
	                    and r.Type = 'B' 
	                    and rd.POID = psd.ID 
	                    and rd.Seq1 = psd.Seq1 
	                    and rd.Seq2 = psd.Seq2
                    )a
                    for xml path('')
	            ),1,1,'')
    ,[WK] = stuff((
	            	select concat(char(10),ID)
	            	from Export_Detail with (nolock) 
	            	where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2 order by Export_Detail.ID
	            	for xml path('')
	            ),1,1,'')
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = psd.FabricType
    ,[stock sp]=psd.StockPOID
    ,[StockSeq1]=psd.StockSeq1
    ,[StockSeq2]=psd.StockSeq2
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
	,[Description] = d.Description
	,[Color] = CASE WHEN Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' THEN psd.SuppColor 
					ELSE psd.ColorID  
			   END
	,[Size] = psd.SizeSpec
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.Qty)
    ,[Order Qty] = o.Qty
	,[Ship Qty] = dbo.GetUnitQty(psd.PoUnit, psd.StockUnit, psd.ShipQty)
	,[Roll] = fi.Roll
	,[Dyelot] = fi.Dyelot
	,[Stock Type] = case when fi.StockType = 'B' then 'Bulk'
						 when fi.StockType = 'I' then 'Inventory'
						 when fi.StockType = 'O' then 'Scrap'
						 end
	,[In Qty] = round(fi.InQty,2)
	,[Out Qty] = round(fi.OutQty,2)
	,[Adjust Qty] = round(fi.AdjustQty,2)
    ,[Return Qty] = round(fi.ReturnQty,2)
	,[Balance Qty] = round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2) - round(fi.ReturnQty,2)
	,[Location] = f.MtlLocationID
    ,[MCHandle] = isnull(dbo.getPassEmail(o.MCHandle) ,'')
	,[POHandle] = isnull(dbo.getPassEmail(p.POHandle) ,'')
	,[POSMR] = isnull(dbo.getPassEmail(p.POSMR) ,'')     
    ";

        private string sqlcolumn_sum = @"select
	[M] = o.MDivisionID
	,[Factory] = o.FactoryID
	,[SP#] = psd.id
	,[OrderType] = o.OrderTypeID
	,[WeaveType] = d.WeaveTypeID
    ,[BuyerDelivery]=o.BuyerDelivery
    ,[ETA] = psd.FinalETA
    ,[ArriveWHDate] = stuff((
                    select distinct concat(char(10),isnull(Format(a.date,'yyyy/MM/dd'),'　'))
                    from (
	                    select date = Export.whsearrival
	                    from Export_Detail with (nolock) 
	                    inner join Export with (nolock) on Export.ID = Export_Detail.ID
	                    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2

	                    union all

	                    select date = ts.IssueDate
	                    from TransferIn ts with (nolock) 
	                    inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	                    where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	                    and ts.Status='Confirmed'

                        union all

                        select date = r.WhseArrival
                        from Receiving r with (nolock) 
                        inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	                    where r.Status = 'Confirmed' 
	                    and r.Type = 'B' 
	                    and rd.POID = psd.ID 
	                    and rd.Seq1 = psd.Seq1 
	                    and rd.Seq2 = psd.Seq2
                    )a
                    for xml path('')
	            ),1,1,'')
    ,[WK] = stuff((
	            	select concat(char(10),ID)
	            	from Export_Detail with (nolock) 
	            	where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2 order by Export_Detail.ID
	            	for xml path('')
	            ),1,1,'')
	,[Brand] = o.BrandID
	,[Style] = o.StyleID
	,[Season] = o.SeasonID
	,[Project] = o.ProjectID
	,[Program] = o.ProgramID
	,[Seq1] = psd.SEQ1
	,[Seq2] = psd.SEQ2
	,[Material Type] = psd.FabricType
    ,[stock sp]=psd.StockPOID
    ,[StockSeq1]=psd.StockSeq1
    ,[StockSeq2]=psd.StockSeq2
	,[Refno] = psd.Refno
	,[SCI Refno] = psd.SCIRefno
    ,[Description] = d.Description
	,[Color] = CASE WHEN Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' THEN psd.SuppColor 
					ELSE psd.ColorID  
			   END
	,[Size] = psd.SizeSpec
	,[Stock Unit] = psd.StockUnit
	,[Purchase Qty] = round(ISNULL(r.RateValue,1) * psd.Qty,2)
    ,[Order Qty] = o.Qty
	,[Ship Qty] = round(ISNULL(r.RateValue,1) * psd.ShipQty,2)
	,[In Qty] = round(mpd.InQty,2)
	,[Out Qty] = round(mpd.OutQty,2)
	,[Adjust Qty] = round(mpd.AdjustQty,2)
    ,[Return Qty] = round(mpd.ReturnQty,2)
	,[Balance Qty] = round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2) - round(mpd.ReturnQty,2)
	,[Bulk Qty] = (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.ReturnQty,2) - round(mpd.LInvQty,2)
	,[Inventory Qty] = round(mpd.LInvQty,2)
	,[Scrap Qty] = round(mpd.LObQty ,2)
	,[Bulk Location] = mpd.ALocation
	,[Inventory Location] = mpd.BLocation
    ,[MCHandle] = isnull(dbo.getPassEmail(o.MCHandle) ,'')
	,[POHandle] = isnull(dbo.getPassEmail(p.POHandle) ,'')
	,[POSMR] = isnull(dbo.getPassEmail(p.POSMR) ,'') 
    ";

        private string sql_yyyy = @"select distinct left(CONVERT(CHAR(8),o.SciDelivery, 112),4) as SciYYYY";

        private string sql_cnt = @"select count(*) as datacnt";

        private int _reportType;
        private bool boolCheckQty;
        private int data_cnt = 0;
        private StringBuilder sqlcmd = new StringBuilder();
        private StringBuilder sqlcmd_fin = new StringBuilder();
        private DataTable printData;
        private DataTable printData_cnt;
        private DataTable printData_yyyy;
        private List<SqlParameter> parameters = new List<SqlParameter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="R21"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            Dictionary<string, string> comboBox2_RowSource = new Dictionary<string, string>
            {
                { "All", "All" },
                { "B", "Bulk" },
                { "I", "Inventory" },
                { "O", "Scrap" },
            };
            this.cmbStockType.DataSource = new BindingSource(comboBox2_RowSource, null);
            this.cmbStockType.ValueMember = "Key";
            this.cmbStockType.DisplayMember = "Value";
            this.cmbStockType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.StartSPNo = this.textStartSP.Text;
            this.EndSPNo = this.textEndSP.Text;
            this.MDivision = this.txtMdivision1.Text;
            this.Factory = this.txtfactory1.Text;
            this.StartRefno = this.textStartRefno.Text;
            this.EndRefno = this.textEndRefno.Text;
            this.Color = this.textColor.Text;
            this.MT = this.comboxMaterialTypeAndID.comboMaterialType.SelectedValue.ToString();
            this.MtlTypeID = this.comboxMaterialTypeAndID.comboMtlTypeID.SelectedValue.ToString();
            this.ST = this.cmbStockType.SelectedValue.ToString();
            this._reportType = this.rdbtnDetail.Checked ? 0 : 1;
            this.boolCheckQty = this.checkQty.Checked;
            this.BuyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.BuyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.ETA1 = this.dateETA.Value1;
            this.ETA2 = this.dateETA.Value2;
            this.arriveWH1 = this.dateArriveDate.Value1;
            this.arriveWH2 = this.dateArriveDate.Value2;
            this.WorkNo = this.txtWorkNo.Text;
            this.location1 = this.txtMtlLocation1.Text;
            this.location2 = this.txtMtlLocation2.Text;

            this.bulk = this.checkBulk.Checked;
            this.sample = this.checkSample.Checked;
            this.material = this.checkMaterial.Checked;
            this.smtl = this.checkSMTL.Checked;
            this.complete = this.chkComplete.Checked;
            if (MyUtility.Check.Empty(this.StartSPNo) &&
                MyUtility.Check.Empty(this.EndSPNo) &&
                !this.dateETA.HasValue &&
                !this.dateArriveDate.HasValue &&
                !this.dateBuyerDelivery.HasValue &&
                MyUtility.Check.Empty(this.StartRefno) &&
                MyUtility.Check.Empty(this.EndRefno))
            {
                MyUtility.Msg.WarningBox("<SP#>,<ETA>,<Arrive W/H Date>,<Buyer Delivery>,<Refno> at least one entry is required");
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.sqlcmd.Clear();
            this.sqlcmd_fin.Clear();
            this.parameters.Clear();

            if (this._reportType == 0)
            {
                #region 主要sql Detail
                this.sqlcmd.Append($@" 
from Orders o with (nolock)
inner join PO p with (nolock) on o.id = p.id
inner join PO_Supp_Detail psd with (nolock) on p.id = psd.id
{(!string.IsNullOrEmpty(this.WorkNo) ? $"INNER JOIN Export_Detail ed ON ed.POID=psd.ID AND ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2 AND ed.ID='{this.WorkNo}'" : string.Empty)}
left join FtyInventory fi with (nolock) on fi.POID = psd.id and fi.Seq1 = psd.SEQ1 and fi.Seq2 = psd.SEQ2
left join Fabric WITH (NOLOCK) on psd.SCIRefno = fabric.SCIRefno
outer apply
(
	select MtlLocationID = stuff(
	(
		select concat(',',MtlLocationID)
		from FtyInventory_Detail fid with (nolock) 
		where fid.Ukey = fi.Ukey
		for xml path('')
	),1,1,'')
)f
outer apply
(
	select Description ,WeaveTypeID
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
where 1=1
");
                #endregion
            }
            else
            {
                #region 主要sql summary
                this.sqlcmd.Append($@"
from Orders o with (nolock)
inner join PO p with (nolock) on o.id = p.id
inner join PO_Supp_Detail psd with (nolock) on p.id = psd.id
{(!string.IsNullOrEmpty(this.WorkNo) ? $"INNER JOIN Export_Detail ed ON ed.POID=psd.ID AND ed.Seq1 = psd.SEQ1 and ed.Seq2 = psd.SEQ2 AND ed.ID='{this.WorkNo}'" : string.Empty)}
left join MDivisionPoDetail mpd with (nolock) on mpd.POID = psd.id and mpd.Seq1 = psd.SEQ1 and mpd.seq2 = psd.SEQ2
left join Fabric WITH (NOLOCK) on psd.SCIRefno = fabric.SCIRefno
outer apply
(
	select Description ,WeaveTypeID
	from Fabric f with (nolock)
	where f.SCIRefno = psd.SCIRefno
)d
outer apply
(
	select RateValue
	from Unit_Rate WITH (NOLOCK) 
	where UnitFrom = psd.PoUnit and UnitTo = psd.StockUnit
)r
where 1=1
");
                #endregion
            }

            #region where條件
            if (!MyUtility.Check.Empty(this.StartSPNo))
            {
                this.sqlcmd.Append(string.Format(" and psd.id >= '{0}'", this.StartSPNo));
            }

            if (!MyUtility.Check.Empty(this.EndSPNo))
            {
                this.sqlcmd.Append(string.Format(" and (psd.id <= '{0}' or psd.id like '{0}%')", this.EndSPNo));
            }

            if (!MyUtility.Check.Empty(this.MDivision))
            {
                this.sqlcmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.MDivision));
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                this.sqlcmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.Factory));
            }

            if (!MyUtility.Check.Empty(this.StartRefno))
            {
                this.sqlcmd.Append(string.Format(" and psd.Refno >= '{0}'", this.StartRefno));
            }

            if (!MyUtility.Check.Empty(this.EndRefno))
            {
                this.sqlcmd.Append(string.Format(" and (psd.Refno <= '{0}' or psd.Refno like '{0}%')", this.EndRefno));
            }

            if (!MyUtility.Check.Empty(this.Color))
            {
                this.sqlcmd.Append(string.Format(" and psd.ColorID = '{0}'", this.Color));
            }

            if (!MyUtility.Check.Empty(this.MT))
            {
                if (this.MT != "All")
                {
                    this.sqlcmd.Append(string.Format(" and psd.FabricType = '{0}'", this.MT));
                }
            }

            if (!MyUtility.Check.Empty(this.MtlTypeID))
            {
                this.sqlcmd.Append(string.Format(" and fabric.MtlTypeID = '{0}'", this.MtlTypeID));
            }

            if (!MyUtility.Check.Empty(this.ST))
            {
                if (this.ST != "All")
                {
                    if (this._reportType == 0)
                    {
                            {
                            this.sqlcmd.Append(string.Format(" and fi.StockType = '{0}'", this.ST));
                        }
                        }
                    else
                    {
                        if (this.ST == "B")
                        {
                            this.sqlcmd.Append(" and (round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2)) - round(mpd.ReturnQty,2) - round(mpd.LInvQty,2)>0");
                        }
                        else if (this.ST == "I")
                        {
                            this.sqlcmd.Append(" and round(mpd.LInvQty,2) > 0");
                        }
                        else if (this.ST == "O")
                        {
                            this.sqlcmd.Append(" and round(mpd.LObQty ,2) > 0");
                        }
                    }
                }
            }

            if (this.boolCheckQty)
            {
                if (this._reportType == 0)
                {
                    this.sqlcmd.Append(" and (round(fi.InQty,2) - round(fi.OutQty,2) + round(fi.AdjustQty,2)) - round(fi.ReturnQty,2) > 0");
                }
                else
                {
                    this.sqlcmd.Append(" and ((round(mpd.InQty,2) - round(mpd.OutQty,2) + round(mpd.AdjustQty,2) - round(mpd.ReturnQty,2) > 0) or mpd.LInvQty >0 or mpd.LObQty >0)  ");
                }
            }

            if (!MyUtility.Check.Empty(this.BuyerDelivery1))
            {
                this.sqlcmd.Append($" and o.BuyerDelivery >='{((DateTime)this.BuyerDelivery1).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(this.BuyerDelivery2))
            {
                this.sqlcmd.Append($" and o.BuyerDelivery <='{((DateTime)this.BuyerDelivery2).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(this.ETA1))
            {
                this.sqlcmd.Append($" and psd.FinalETA >='{((DateTime)this.ETA1).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(this.ETA2))
            {
                this.sqlcmd.Append($" and psd.FinalETA <='{((DateTime)this.ETA2).ToString("yyyy/MM/dd")}'");
            }

            if (this.bulk || this.sample || this.material || this.smtl)
            {
                this.sqlcmd.Append(" and (1=0");
                if (this.bulk)
                {
                    this.sqlcmd.Append(" or o.Category = 'B'");
                }

                if (this.sample)
                {
                    this.sqlcmd.Append(" or o.Category = 'S'");
                }

                if (this.material)
                {
                    this.sqlcmd.Append(" or o.Category = 'M'");
                }

                if (this.smtl)
                {
                    this.sqlcmd.Append(" or o.Category = 'T'");
                }

                this.sqlcmd.Append(")");
            }

            if (this.complete)
            {
                this.sqlcmd.Append(" and psd.Complete = '1'");
            }

            if (this.chkNoLocation.Checked)
            {
                this.sqlcmd.Append(@"
and not exists(
    select 1
    from FtyInventory_Detail fid with (nolock) 
    where fid.Ukey = fi.Ukey and isnull(fid.MtlLocationID, '') <> ''
)
");
            }

            if (!MyUtility.Check.Empty(this.arriveWH1) && !MyUtility.Check.Empty(this.arriveWH2))
            {
                this.sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival between '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.arriveWH2).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate between '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.arriveWH2).ToString("yyyy/MM/dd")}') 
or 
    exists ( 
	select 1 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	where r.WhseArrival between '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.arriveWH2).ToString("yyyy/MM/dd")}'
    and r.Status = 'Confirmed' 
    and r.Type = 'B' 
	and rd.POID = psd.ID 
	and rd.Seq1 = psd.Seq1 
	and rd.Seq2 = psd.Seq2 )
)
");
            }
            else if (!MyUtility.Check.Empty(this.arriveWH1))
            {
                this.sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival >= '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate >= '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}' ) 
or 
    exists ( 
	select 1 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	where r.WhseArrival >= '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}'
    and r.Status = 'Confirmed' 
    and r.Type = 'B' 
	and rd.POID = psd.ID 
	and rd.Seq1 = psd.Seq1 
	and rd.Seq2 = psd.Seq2 )
)
");
            }
            else if (!MyUtility.Check.Empty(this.arriveWH2))
            {
                this.sqlcmd.Append($@" 
 and (
	exists (
	select 1 from Export_Detail with (nolock) 
	inner join Export with (nolock) on Export.ID = Export_Detail.ID
	where   POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	and Export.whsearrival <= '{((DateTime)this.arriveWH2).ToString("yyyy/MM/dd")}' )
or
	exists (	
	select 1
	from TransferIn ts with (nolock) 
	inner join TransferIn_Detail tsd with (nolock) on tsd.ID = ts.ID
	where tsd.POID = psd.id and tsd.Seq1 = psd.SEQ1 and tsd.Seq2 = psd.SEQ2
	and ts.Status='Confirmed'
	and ts.IssueDate <= '{((DateTime)this.arriveWH2).ToString("yyyy/MM/dd")}' ) 
or 
    exists ( 
	select 1 
    from Receiving r with (nolock) 
    inner join Receiving_detail rd with (nolock) on r.Id = rd.Id
	where r.WhseArrival <= '{((DateTime)this.arriveWH1).ToString("yyyy/MM/dd")}'
    and r.Status = 'Confirmed' 
    and r.Type = 'B' 
	and rd.POID = psd.ID 
	and rd.Seq1 = psd.Seq1 
	and rd.Seq2 = psd.Seq2 )
)
");
            }

            if (!MyUtility.Check.Empty(this.location1) && !MyUtility.Check.Empty(this.location2))
            {
                this.parameters.Add(new SqlParameter("@location1", this.location1));
                this.parameters.Add(new SqlParameter("@location2", this.location2));
                this.sqlcmd.Append(
                    @" 
        and exists ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where fi.ukey = ukey
                        and mtllocationid >= @location1
                        and mtllocationid <= @location2 ) " + Environment.NewLine);
            }
            else if (!MyUtility.Check.Empty(this.location1))
            {
                this.parameters.Add(new SqlParameter("@location1", this.location1));
                this.sqlcmd.Append(
                    @" 
        and exists ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where fi.ukey = ukey
                        and mtllocationid = @location1) " + Environment.NewLine);
            }
            else if (!MyUtility.Check.Empty(this.location2))
            {
                this.parameters.Add(new SqlParameter("@location2", this.location2));
                this.sqlcmd.Append(
                    @" 
        and exists ( select ukey 
                        from dbo.ftyinventory_detail WITH (NOLOCK) 
                        where fi.ukey = ukey
                        and mtllocationid = @location2) " + Environment.NewLine);
            }
            #endregion

            #region Get Data
            DualResult result;

            // 先抓出資料筆數 大於100萬筆需另外處理(按年出多excel)
            if (result = DBProxy.Current.Select(null, this.sql_cnt + this.sqlcmd.ToString(), this.parameters, out this.printData_cnt))
            {
                this.data_cnt = (int)this.printData_cnt.Rows[0]["datacnt"];

                return result;
            }

            #endregion
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this.data_cnt == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(this.data_cnt);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            DualResult result;
            string reportname = string.Empty;
            if (this._reportType == 0)
            {
                this.sqlcmd_fin.Append(this.sqlcolumn + this.sqlcmd.ToString());
                reportname = "Warehouse_R21_Detail.xltx";
            }
            else
            {
                this.sqlcmd_fin.Append(this.sqlcolumn_sum + this.sqlcmd.ToString());
                reportname = "Warehouse_R21_Summary.xltx";
            }

            if (this.data_cnt > 1000000)
            {
                string sqlcmd_dtail;
                Excel.Application objApp;
                Excel.Worksheet tmpsheep = null;
                Utility.Report.ExcelCOM com;
                string strExcelName = Class.MicrosoftFile.GetName((this._reportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
                int sheet_cnt = 1;
                int split_cnt = 500000;
                result = DBProxy.Current.Select(null, this.sql_yyyy + this.sqlcmd, this.parameters, out this.printData_yyyy);
                if (!result)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.ErrorBox(result.Messages.ToString());
                    return result;
                }

                string[] exl_name = new string[this.printData_yyyy.Rows.Count];
                DataTable tmpTb;
                for (int i = 0; i < this.printData_yyyy.Rows.Count; i++)
                {
                    sqlcmd_dtail = this.sqlcmd_fin.ToString() + string.Format("  and o.SciDelivery >= '{0}' and  o.SciDelivery < = '{1}' ", this.printData_yyyy.Rows[i][0].ToString() + "0101", this.printData_yyyy.Rows[i][0].ToString() + "1231");
                    strExcelName = Class.MicrosoftFile.GetName((this._reportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary" + this.printData_yyyy.Rows[i][0].ToString());
                    exl_name[i] = strExcelName;
                    com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + reportname, null);
                    objApp = com.ExcelApp;
                    com.TransferArray_Limit = 10000;
                    com.ColumnsAutoFit = false;
                    sheet_cnt = 1;

                    if (DBProxy.Current.Select(null, sqlcmd_dtail, this.parameters, out this.printData))
                    {
                        // 如果筆數超過split_cnt再拆一次sheet
                        if (this.printData.Rows.Count > split_cnt)
                        {
                            int max_sheet_cnt = (int)Math.Floor((decimal)(this.printData.Rows.Count / split_cnt));
                            for (int j = 0; j <= max_sheet_cnt; j++)
                            {
                                if (j < max_sheet_cnt)
                                {
                                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]).Copy(objApp.Workbooks[1].Worksheets[sheet_cnt]);
                                }

                                tmpsheep = (Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt];
                                tmpsheep.Name = this.printData_yyyy.Rows[i][0].ToString() + "-" + (j + 1).ToString();
                                ((Excel.Worksheet)objApp.ActiveWorkbook.Sheets[sheet_cnt]).Select();
                                tmpTb = this.printData.AsEnumerable().Skip(j * split_cnt).Take(split_cnt).CopyToDataTable();

                                DualResult ok = com.WriteTable(tmpTb, 2);

                                sheet_cnt++;
                                if (tmpTb != null)
                                {
                                    tmpTb.Rows.Clear();
                                    tmpTb.Constraints.Clear();
                                    tmpTb.Columns.Clear();
                                    tmpTb.ExtendedProperties.Clear();
                                    tmpTb.ChildRelations.Clear();
                                    tmpTb.ParentRelations.Clear();
                                    tmpTb.Dispose();

                                    GC.Collect();
                                    GC.WaitForPendingFinalizers();
                                    GC.Collect();
                                }
                            }
                        }
                        else
                        {
                            // 複製sheet
                            tmpsheep = (Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt];
                            tmpsheep.Name = this.printData_yyyy.Rows[i][0].ToString();
                            ((Excel.Worksheet)objApp.ActiveWorkbook.Sheets[sheet_cnt]).Select();
                            com.WriteTable(this.printData, 2);
                            sheet_cnt++;
                        }

                        if (this.printData != null)
                        {
                            this.printData.Rows.Clear();
                            this.printData.Constraints.Clear();
                            this.printData.Columns.Clear();
                            this.printData.ExtendedProperties.Clear();
                            this.printData.ChildRelations.Clear();
                            this.printData.ParentRelations.Clear();
                            this.printData.Dispose();

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                        }
                    }

                    // 刪除多餘sheet
                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt + 1]).Delete();
                    ((Excel.Worksheet)objApp.Workbooks[1].Worksheets[sheet_cnt]).Delete();
                    objApp.ActiveWorkbook.SaveAs(strExcelName);
                    if (tmpsheep != null)
                    {
                        Marshal.ReleaseComObject(tmpsheep);
                    }

                    for (int f = 1; f <= objApp.Workbooks[1].Worksheets.Count; f++)
                    {
                        Excel.Worksheet sheet = objApp.Workbooks[1].Worksheets.Item[f];
                        if (sheet != null)
                        {
                            Marshal.ReleaseComObject(sheet);
                        }
                    }

                    objApp.Workbooks[1].Close();
                    objApp.Quit();
                    Marshal.ReleaseComObject(objApp);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                Marshal.ReleaseComObject(tmpsheep);
                foreach (string filrstr in exl_name)
                {
                    filrstr.OpenFile();
                }
            }
            else
            {
                result = DBProxy.Current.Select(null,  this.sqlcmd_fin.ToString(), this.parameters, out this.printData);
                if (!result)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.ErrorBox(result.Messages.ToString());
                    return result;
                }

                Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + reportname, null);
                Excel.Application objApp = com.ExcelApp;

                // MyUtility.Excel.CopyToXls(printData, "", reportname, 1, showExcel: false, excelApp: objApp);
                com.WriteTable(this.printData, 2);
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName((this._reportType == 0) ? "Warehouse_R21_Detail" : "Warehouse_R21_Summary");
                for (int f = 1; f <= objApp.Workbooks[1].Worksheets.Count; f++)
                {
                    Excel.Worksheet sheet = objApp.Workbooks[1].Worksheets.Item[f];
                    sheet.UsedRange.Rows.AutoFit();
                    if (sheet != null)
                    {
                        Marshal.ReleaseComObject(sheet);
                    }
                }

                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Workbooks[1].Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);

                strExcelName.OpenFile();
            }

            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void RadioGroupReportType_ValueChanged(object sender, EventArgs e)
        {
            if (this.radioGroupReportType.Value == "D")
            {
                this.chkNoLocation.ReadOnly = false;
                this.txtMtlLocation1.ReadOnly = false;
                this.txtMtlLocation2.ReadOnly = false;
            }
            else
            {
                this.txtMtlLocation1.Text = string.Empty;
                this.txtMtlLocation2.Text = string.Empty;
                this.txtMtlLocation1.ReadOnly = true;
                this.txtMtlLocation2.ReadOnly = true;
                this.chkNoLocation.ReadOnly = true;
                this.chkNoLocation.Checked = false;
            }
        }
    }
}
