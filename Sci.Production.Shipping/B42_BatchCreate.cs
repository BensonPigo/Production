using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class B42_BatchCreate : Sci.Win.Subs.Base
    {
        DataTable AllDetailData, MidDetailData;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Ict.Win.DataGridViewGeneratorTextColumnSettings vncontract = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings currentcustom = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings consumption = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

        public B42_BatchCreate()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "Bulk,Sample");
            comboBox1.SelectedIndex = -1;
            DataTable gridData;
            DBProxy.Current.Select(null, "select 0 as Selected,'' as CurrentCustomSP,'' as Article,'' as SizeCode,'' as Consumption,* from VNConsumption where 1 = 0", out gridData);

            #region Current Custom的DbClick
            currentcustom.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        string selected = MyUtility.Convert.GetString(dr["Selected"]) == "1" ? "0" : "1";
                        foreach (DataRow all in ((DataTable)listControlBindingSource1.DataSource).Rows)
                        {
                            if (MyUtility.Convert.GetString(all["CurrentCustomSP"]) == MyUtility.Convert.GetString(dr["CurrentCustomSP"]))
                            {
                                all["Selected"] = selected;
                            }
                        }
                    }
                }
            };
            #endregion

            #region Contract no 按右鍵與validating
            vncontract.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,StartDate,EndDate from VNContract where GETDATE() between StartDate and EndDate order by StartDate", "15,10,10", MyUtility.Convert.GetString(dr["VNContractID"]), headercaptions: "Contract No.,Start Date, End Date");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            };

            vncontract.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["VNContractID"].ToString())
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract where ID = '{0}'", e.FormattedValue.ToString())))
                    {
                        MyUtility.Msg.WarningBox("Contract no. not found!!");
                        dr["VNContractID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            };
            #endregion

            #region Consumption的DbClick
            consumption.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Production.Shipping.B42_BatchCreate_Consumption callNextForm = new Sci.Production.Shipping.B42_BatchCreate_Consumption(MidDetailData, AllDetailData, MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), MyUtility.Convert.GetString(dr["VNContractID"]));
                        DialogResult result = callNextForm.ShowDialog(this);
                        callNextForm.Dispose();
                    }
                }
            };
            #endregion

            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8))
                .Text("CurrentCustomSP", header: "Current Custom", width: Widths.AnsiChars(8), settings: currentcustom, iseditingreadonly: true)
                .Text("VNContractID", header: "Contract no", width: Widths.AnsiChars(15), settings: vncontract)
                .Date("CDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Category", header: "Category", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6))
                .Text("Consumption", header: "Consumption", width: Widths.AnsiChars(40), settings: consumption, iseditingreadonly: true);

            listControlBindingSource1.DataSource = gridData;
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery can't empty!");
                dateRange1.TextBox1.Focus();
                return;
            }
            DataTable GroupData, gridData;
            StringBuilder sqlCmd = new StringBuilder();
            string  contractID = MyUtility.GetValue.Lookup(@"select ID from VNContract where StartDate = (select MAX(StartDate) from VNContract where GETDATE() between StartDate and EndDate and Status = 'Confirmed')");
            #region 組撈所有明細SQL
            sqlCmd.Append(string.Format(@"Declare @vncontractid varchar(15)
set @vncontractid = '{0}';

with tmpAllStyle
as (
select o.StyleUkey,oqd.SizeCode,oqd.Article,o.Category,o.StyleID,o.SeasonID,o.BrandID as OrderBrandID,sum(oqd.Qty) as GMTQty,isnull(s.CPU,0) as StyleCPU,isnull(s.CTNQty,0) as CTNQty
from Order_QtyShip oq
inner join Orders o on oq.ID = o.ID
inner join Order_QtyShip_Detail oqd on oq.ID = oqd.ID and oq.Seq = oqd.Seq
left join Style s on o.StyleUkey = s.Ukey
where oq.BuyerDelivery between '{1}' and '{2}'", 
                                             contractID,
                                             Convert.ToDateTime(dateRange1.Value1).ToString("d"),Convert.ToDateTime(dateRange1.Value2).ToString("d")));
            if (!MyUtility.Check.Empty(txtstyle1.Text))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'",txtstyle1.Text));
            }
            if (!MyUtility.Check.Empty(comboBox1.Text))
            {
                sqlCmd.Append(string.Format(" and o.Category = '{0}'", comboBox1.Text == "Bulk" ? "B" : "S"));
            }
            if (!MyUtility.Check.Empty(txtbrand1.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'",txtbrand1.Text));
            }
            sqlCmd.Append(@"
group by o.StyleUkey,oqd.SizeCode,oqd.Article,o.Category,o.StyleID,o.SeasonID,o.BrandID,isnull(s.CPU,0),isnull(s.CTNQty,0)
),
tmpMarkerData
as (
select ts.*,sm.MarkerName,sm.LectraCode,dbo.MarkerLengthToYDS(sm.MarkerLength) as markerYDS,
sm.Width,sms.Qty,sc.FabricCode,sfqt.QTFabricCode
from tmpAllStyle ts
inner join Style_MarkerList sm on sm.StyleUkey = ts.StyleUkey
inner join Style_MarkerList_SizeQty sms on sm.Ukey = sms.Style_MarkerListUkey and sms.SizeCode = ts.SizeCode
inner join Style_ColorCombo sc on sc.StyleUkey = sm.StyleUkey and sc.LectraCode = sm.LectraCode
left join Style_MarkerList_Article sma on sm.Ukey = sma.Style_MarkerListUkey 
left join Style_FabricCode_QT sfqt on sm.LectraCode = sfqt.LectraCode and sm.StyleUkey = sfqt.StyleUkey
where sm.MixedSizeMarker = 1 and (sma.Article is null or sma.Article = ts.Article) and sc.Article = ts.Article
and CHARINDEX('+',sm.MarkerLength) > 0
),
tmpFabricCode
as (
select t.StyleID,t.SeasonID,t.OrderBrandID,t.Category,t.SizeCode,t.Article,t.GMTQty,t.markerYDS,t.Width,t.Qty, IIF(t.QTFabricCode is null, sb.SCIRefno, sb1.SCIRefno) as SCIRefNo,
IIF(t.QTFabricCode is null, sb.SuppIDBulk, sb1.SuppIDBulk) as SuppIDBulk,t.StyleCPU,t.StyleUKey
from tmpMarkerData t
left join Style_BOF sb on sb.StyleUkey = t.StyleUkey and sb.FabricCode = t.FabricCode
left join Style_BOF sb1 on sb1.StyleUkey = t.StyleUkey and sb1.FabricCode = t.QTFabricCode
),
tmpBOFRateData
as (
select t.StyleID,t.SeasonID,t.OrderBrandID,t.Category,t.SizeCode,t.Article,t.GMTQty,t.StyleCPU,t.StyleUKey,
t.markerYDS,'YDS' as UsageUnit,t.Qty,f.SCIRefno,f.Refno,f.BrandID,f.NLCode,f.HSCode,f.CustomsUnit,f.Width,f.Type,
f.PcsWidth,f.PcsLength,f.PcsKg,f.Description,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = 'YDS' and UnitTo = 'M'),'') as M2UnitRate
from tmpFabricCode t
inner join Fabric f on f.SCIRefno = t.SCIRefno
where (t.SuppIDBulk <> 'FTY' or t.SuppIDBulk <> 'FTY-C')
and f.NoDeclare = 0
),
tmpBOFNewQty
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,StyleCPU,StyleUKey,Description,IIF(Type = 'F','Fabric',IIF(Type = 'A','Accessory','')) as Type,
([dbo].getVNUnitTransfer(Type,UsageUnit,CustomsUnit,markerYDS,Width,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)))/Qty as NewQty
from tmpBOFRateData
),
tmpBOFData
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(isnull(NewQty,0)) as Qty, 0 as LocalItem,StyleCPU,StyleUKey,Description,Type,'' as SuppID
from tmpBOFNewQty
group by StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,StyleCPU,StyleUKey,Description,Type
),
tmpBOA
as (
select t.*,sb.Ukey,sb.Refno,sb.SCIRefno,sb.SuppIDBulk,sb.SizeItem,sb.PatternPanel,sb.BomTypeArticle,sb.BomTypeColor,sb.ConsPC,
sc.ColorID,f.UsageUnit,f.HSCode,f.NLCode,f.CustomsUnit,f.PcsWidth,f.PcsLength,f.PcsKg,f.BomTypeCalculate,f.Type,f.BrandID,
f.Description
from tmpAllStyle t
inner join Style_BOA sb on  t.StyleUkey = sb.StyleUkey
left join Style_ColorCombo sc on sc.StyleUkey = sb.StyleUkey and sc.PatternPanel = sb.PatternPanel and sc.Article = t.Article
left join Fabric f on sb.SCIRefno = f.SCIRefno
where sb.IsCustCD <> 2
and (sb.SuppIDBulk <> 'FTY' and sb.SuppIDBulk <> 'FTY-C')
),
tmpBOAPrepareData
as (
select t.*,IIF(t.BomTypeCalculate = 1,(select isnull(dbo.GetDigitalValue(SizeSpec),0) from Style_SizeSpec where StyleUkey = t.StyleUkey and SizeItem = t.SizeItem and SizeCode = t.SizeCode),ConsPC) as SizeSpec,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.UsageUnit and TO_U = t.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = t.UsageUnit and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate where UnitFrom = t.UsageUnit and UnitTo = t.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = t.UsageUnit and UnitTo = 'M'),'') as M2UnitRate
from tmpBOA t
where (t.BomTypeArticle = 0 and t.BomTypeColor = 0) or ((t.BomTypeArticle = 1 or t.BomTypeColor = 1) and t.ColorID is not null)
),
tmpBOANewQty
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,StyleCPU,StyleUKey,Description,IIF(Type = 'F','Fabric',IIF(Type = 'A','Accessory','')) as Type,
[dbo].getVNUnitTransfer(Type,UsageUnit,CustomsUnit,SizeSpec,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
from tmpBOAPrepareData
),
tmpBOAData
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(ISNULL(NewQty,0)) as Qty, 0 as LocalItem,StyleCPU,StyleUKey,Description,Type, '' as SuppID
from tmpBOANewQty
group by StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,StyleCPU,StyleUKey,Description,Type
),
tmpLocalPO
as (
select t.*,ld.Refno,ld.Qty,ld.UnitId,li.MeterToCone,li.NLCode,li.HSCode,li.CustomsUnit,li.PcsWidth,li.PcsLength,li.PcsKg,o.Qty as OrderQty,isnull(vd.Waste,0) as Waste,
li.Description,li.Category as Type,li.LocalSuppid as SuppID
from tmpAllStyle t
inner join LocalPO_Detail ld on ld.OrderId = (select TOP 1 ID from Orders where StyleUkey = t.StyleUkey and Category = t.Category order by BuyerDelivery,ID)
left join LocalItem li on li.RefNo = ld.Refno
left join Orders o on ld.OrderId = o.ID
left join VNContract_Detail vd on vd.ID = @vncontractid and vd.NLCode = li.NLCode
where li.NoDeclare = 0
),
tmpConeToM
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,StyleCPU,StyleUKey,
Refno,IIF(UnitId = 'CONE',Qty*MeterToCone,Qty) as Qty,OrderQty, IIF(UnitId = 'CONE','M',UnitId) as UnitId,Waste,
NLCode,HSCode,CustomsUnit,PcsWidth,PcsLength,PcsKg,Description,Type,SuppID
from tmpLocalPO
),
tmpPrepareRate
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,StyleCPU,StyleUKey,
Refno,Qty/OrderQty as Qty,UnitId,NLCode,HSCode,CustomsUnit,PcsWidth,PcsLength,PcsKg,Waste,Description,Type,SuppID,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate where UnitFrom = UnitId and UnitTo = CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate where UnitFrom = UnitId and UnitTo = 'M'),'') as M2UnitRate
from tmpConeToM
),
tmpLocalNewQty
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,Refno as SCIRefno,Refno,'' as BrandID,NLCode,HSCode,CustomsUnit,Waste,StyleCPU,StyleUKey,Description,Type,SuppID,
[dbo].getVNUnitTransfer('',UnitId,CustomsUnit,Qty,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
from tmpPrepareRate
),
tmpLocalData
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,
CustomsUnit,sum(isnull(NewQty,0)-(isnull(NewQty,0)*isnull(Waste,0))) as Qty, 1 as LocalItem,StyleCPU,StyleUKey,Description,Type,SuppID
from tmpLocalNewQty
group by StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,StyleCPU,StyleUKey,Description,Type,SuppID
),
tmpFixDeclare
as (
select t.StyleID,t.SeasonID,t.OrderBrandID,t.Category,t.SizeCode,t.Article,t.GMTQty,vfd.* ,sa.TissuePaper as ArticleTissuePaper,t.CTNQty,t.StyleCPU,t.StyleUKey
from VNFixedDeclareItem vfd
left join tmpAllStyle t on 1 = 1
left join Style_Article sa on sa.StyleUkey = t.StyleUkey and sa.Article = t.Article
),
tmpFinalFixDeclare
as (
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,'' as SCIRefno,'' as Refno,'' as BrandID,'' as SuppID,
NLCode,HSCode,UnitID as CustomsUnit,IIF(Type = 1, Qty, IIF(CTNQty = 0,0,ROUND(Qty/CTNQty,3))) as Qty,0 as LocalItem,StyleCPU,StyleUKey,'' as Description,'' as Type
from tmpFixDeclare
where TissuePaper = 0 or (TissuePaper = 1 and ArticleTissuePaper = 1)
)

select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,Qty,LocalItem,StyleCPU,StyleUKey,Description,Type,SuppID from tmpFinalFixDeclare
union
select StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(Qty) as Qty,LocalItem,StyleCPU,StyleUKey,Description,Type,SuppID
from (
select * from tmpBOFData
union all
select * from tmpBOAData
union all
select * from tmpLocalData) a
group by StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,LocalItem,StyleCPU,StyleUKey,Description,Type,SuppID
");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out AllDetailData);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query detail data fail.\r\n{0}",result.ToString()));
                return;
            }
            #region 整理出依Style,Season Group的資料
            try
            {
                MyUtility.Tool.ProcessWithDatatable(AllDetailData, "StyleID,SeasonID,OrderBrandID,Category,SizeCode,Article,GMTQty,SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,Qty,LocalItem,StyleCPU,StyleUKey",
                    @"select StyleID,SeasonID,OrderBrandID,Category,Article,SizeCode,NLCode,SUM(Qty) as Qty,CustomsUnit into #tmpSumConsData
from #tmp
group by StyleID,SeasonID,OrderBrandID,Category,Article,SizeCode,NLCode,CustomsUnit;

select distinct StyleID,SeasonID,OrderBrandID,Category,Article,SizeCode,GMTQty,StyleCPU,StyleUkey into #tmpGroupStyle
from #tmp

DECLARE cursor_tmpBasic CURSOR FOR
SELECT Distinct StyleID,SeasonID,OrderBrandID,Category,SizeCode,StyleCPU,StyleUkey FROM #tmpGroupStyle

DECLARE @tempCombColor TABLE (
   StyleID VARCHAR(15),
   SeasonID VARCHAR(10),
   BrandID VARCHAR(8),
   Category VARCHAR(1),
   Article VARCHAR(max),
   SizeCode VARCHAR(8),
   GMTQty INT,
   StyleCPU NUMERIC(5,3),
   CustomSP VARCHAR(8),
   VNContractID VARCHAR(15),
   StyleUkey BIGINT
)

DECLARE @style varchar(15),
		@season varchar(10),
		@brand varchar(8),
		@category varchar(1),
		@size varchar(8),
		@cpu numeric(5,3),
		@article varchar(15),
		@gmtqty int,
		@firstrecord bit,
		@recordno int,
		@newdata bit,
		@consist bit,
		@allmatch bit,
		@comboarticle varchar(15),
		@nlcode varchar(5),
		@usedqty numeric(14,3),
		@customsunit varchar(8),
		@contract varchar(15),
		@styleukey bigint
select @contract = ID from VNContract where StartDate = (select MAX(StartDate) from VNContract where GETDATE() between StartDate and EndDate and Status = 'Confirmed')

OPEN cursor_tmpBasic
FETCH NEXT FROM cursor_tmpBasic INTO @style, @season, @brand, @category,@size,@cpu,@styleukey
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @firstrecord = 1
	SET @recordno = 0


	DECLARE cursor_tmpArticle CURSOR FOR
	select Article,GMTQty from #tmpGroupStyle where StyleID = @style and SeasonID = @season and OrderBrandID = @brand and Category = @category and SizeCode = @size;

	OPEN cursor_tmpArticle
	FETCH NEXT FROM cursor_tmpArticle INTO @article,@gmtqty
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @newdata = 0
		IF @firstrecord = 1
			BEGIN
				SET @newdata = 1
			END
		ELSE
			BEGIN
				--檢查NL Code與用量
				SET @consist = 0
				DECLARE cursor_tmpCombColor CURSOR FOR
				select Article from @tempCombColor where StyleID = @style and SeasonID = @season and BrandID = @brand and Category = @category and SizeCode = @size
				OPEN cursor_tmpCombColor
				FETCH NEXT FROM cursor_tmpCombColor INTO @comboarticle
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @allmatch = 0
					SET @comboarticle = SUBSTRING(@comboarticle,1,PATINDEX('%,%',@comboarticle))

					DECLARE cursor_tmpSumConsData CURSOR FOR
					select NLCode,Qty,CustomsUnit from #tmpSumConsData where StyleID = @style and SeasonID = @season and OrderBrandID = @brand and Category = @category and SizeCode = @size and Article = @comboarticle
					OPEN cursor_tmpSumConsData
					FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
					WHILE @@FETCH_STATUS = 0
					BEGIN
						
						select @allmatch = IIF(@usedqty = isnull(Qty,0),1,0) from #tmpSumConsData where StyleID = @style and SeasonID = @season and OrderBrandID = @brand and Category = @category and SizeCode = @size and Article = @comboarticle and NLCode = @nlcode and CustomsUnit = @customsunit
						
						IF @allmatch = 0
							Break;

						FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
					END
					CLOSE cursor_tmpSumConsData
					DEALLOCATE cursor_tmpSumConsData

					IF @allmatch = 0
						BEGIN
							DECLARE cursor_tmpSumConsData CURSOR FOR
							select NLCode,Qty,CustomsUnit from #tmpSumConsData where StyleID = @style and SeasonID = @season and OrderBrandID = @brand and Category = @category and SizeCode = @size and Article = @article
							OPEN cursor_tmpSumConsData
							FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
							WHILE @@FETCH_STATUS = 0
							BEGIN
						
								select @allmatch = IIF(@usedqty = isnull(Qty,0),1,0) from #tmpSumConsData where StyleID = @style and SeasonID = @season and OrderBrandID = @brand and Category = @category and SizeCode = @size and Article = @article and NLCode = @nlcode and CustomsUnit = @customsunit
						
								IF @allmatch = 0
									Break;

								FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
							END
							CLOSE cursor_tmpSumConsData
							DEALLOCATE cursor_tmpSumConsData

							IF @allmatch = 0
								SET @consist = 1
								break;
						END

					IF @consist = 0
						SET @newdata = 1

					FETCH NEXT FROM cursor_tmpCombColor INTO @comboarticle
				END
				CLOSE cursor_tmpCombColor
				DEALLOCATE cursor_tmpCombColor

			END
		IF @newdata = 1
			BEGIN
				insert into @tempCombColor (StyleID,SeasonID,BrandID,Category,Article,SizeCode,GMTQty,StyleCPU,VNContractID,CustomSP,StyleUkey)
				values (@style,@season,@brand,@category,@article+',',@size,@gmtqty,@cpu,@contract,isnull((select top 1 v.CustomSP from VNConsumption v, VNConsumption_Article va, VNConsumption_SizeCode vs where v.VNContractID = @contract and v.StyleID = @style and v.SeasonID = @season and v.BrandID = @brand and v.ID = va.ID and v.ID = vs.ID and va.Article = @article and vs.SizeCode = @size),''),@styleukey)
			END
		ELSE
			BEGIN
				update @tempCombColor set GMTQty = GMTQty + @gmtqty, Article = Article + @article+',' 
				where StyleID = @style and SeasonID = @season and BrandID = @brand and Category = @category and SizeCode = @size
			END
		SET @firstrecord = 0
		FETCH NEXT FROM cursor_tmpArticle INTO @article,@gmtqty
	END
	CLOSE cursor_tmpArticle
	DEALLOCATE cursor_tmpArticle


	FETCH NEXT FROM cursor_tmpBasic INTO @style, @season, @brand, @category,@size,@cpu,@styleukey
END
CLOSE cursor_tmpBasic
DEALLOCATE cursor_tmpBasic

select * from @tempCombColor order by StyleID,SeasonID,Category,Article,SizeCode", out GroupData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query group data fail!!\r\n" + ex.ToString());
                return;
            }
            #endregion

            //撈出每個Consumption一定都要有的NLCode
            DataTable NecessaryItem;
            result = DBProxy.Current.Select(null, string.Format("select NLCode from VNContract_Detail where NecessaryItem = 1 and ID = '{0}'", contractID), out NecessaryItem);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query NecessaryItem data fail.\r\n{0}", result.ToString()));
                return;
            }

            gridData = (DataTable)listControlBindingSource1.DataSource;
            gridData.Clear();
            string colorway,lackNLCode;
            foreach (DataRow dr in GroupData.Rows)
            {
                colorway = MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(','));
                //找出是否有空白的NL Code
                DataRow[] findData = AllDetailData.Select(string.Format("StyleUKey = '{0}' and Article = '{1}' and SizeCode = '{2}' and NLCode = ''",
                    MyUtility.Convert.GetString(dr["StyleUKey"]), colorway, MyUtility.Convert.GetString(dr["SizeCode"])));

                //找出是否有缺料
                lackNLCode = "";
                foreach (DataRow lack in NecessaryItem.Rows)
                {
                    DataRow[] findLackData = AllDetailData.Select(string.Format("StyleUKey = '{0}' and Article = '{1}' and SizeCode = '{2}' and NLCode = '{3}'",
                    MyUtility.Convert.GetString(dr["StyleUKey"]), colorway, MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(lack["NLCode"])));
                    if (findLackData.Length <= 0)
                    {
                        lackNLCode = lackNLCode + MyUtility.Convert.GetString(lack["NLCode"]) + ",";
                    }
                }


                DataRow newrow = gridData.NewRow();
                newrow["Selected"] = 0;
                newrow["CustomSP"] = "";
                newrow["CurrentCustomSP"] = dr["CustomSP"];
                newrow["VNContractID"] = dr["VNContractID"];
                newrow["CDate"] = DateTime.Today;
                newrow["StyleID"] = dr["StyleID"];
                newrow["SeasonID"] = dr["SeasonID"];
                newrow["BrandID"] = dr["BrandID"];
                newrow["Category"] = dr["Category"];
                newrow["Article"] = dr["Article"];
                newrow["SizeCode"] = dr["SizeCode"];
                newrow["Qty"] = dr["GMTQty"];
                newrow["StyleUKey"] = dr["StyleUKey"];
                newrow["CPU"] = dr["StyleCPU"];
                newrow["Consumption"] = (MyUtility.Check.Empty(lackNLCode) ? "" : ("Lacking NL Code:" + lackNLCode.Substring(0, lackNLCode.Length - 1) + ". ")) + (findData.Length > 0 ? "Appear empty NL Code." : "");

                gridData.Rows.Add(newrow);
            }

            #region 組出中間層資料
            result = DBProxy.Current.Select(null, "select *,0 as StyleUkey,'' as SizeCode, '' as Article,0.0 as Waste, 0 as Deleted from VNConsumption_Detail where 1 = 0", out MidDetailData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Prepare detail structure fail!!");
                return;
            }
            foreach (DataRow dr in gridData.Rows)
            {
                string article = MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(','));
                DataRow[] selectedData = AllDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}'", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), article));
                for (int i = 0; i < selectedData.Length; i++)
                {
                    DataRow[] finddata = MidDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}'", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), article, MyUtility.Convert.GetString(selectedData[i]["NLCode"])));

                    if (finddata.Length > 0)
                    {
                        finddata[0]["Qty"] = MyUtility.Convert.GetDecimal(finddata[0]["Qty"]) + MyUtility.Convert.GetDecimal(selectedData[i]["Qty"]);
                    }
                    else
                    {
                        DataRow newrow = MidDetailData.NewRow();
                        newrow["NLCode"] = selectedData[i]["NLCode"];
                        newrow["HSCode"] = selectedData[i]["HSCode"];
                        newrow["UnitID"] = selectedData[i]["CustomsUnit"];
                        newrow["Qty"] = selectedData[i]["Qty"];
                        newrow["Waste"] = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format("select Waste from VNContract_Detail where ID = '{0}' and NLCode = '{1}'", contractID, MyUtility.Convert.GetString(selectedData[i]["NLCode"]))));
                        newrow["UserCreate"] = 0;
                        newrow["StyleUKey"] = dr["StyleUKey"];
                        newrow["SizeCode"] = dr["SizeCode"];
                        newrow["Article"] = article;
                        newrow["Deleted"] = 0;
                        MidDetailData.Rows.Add(newrow);
                    }
                }
            }
            #endregion

            listControlBindingSource1.DataSource = gridData;
        }

        //Auto Custom SP#
        private void button2_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable CustomSP;
            DualResult result = DBProxy.Current.Select(null, "select VNContractID,MAX(CustomSP) as CustomSP from VNConsumption group by VNContractID", out CustomSP);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail, please try again.\r\n"+result.ToString());
                return;
            }

            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && !MyUtility.Check.Empty(dr["VNContractID"]))
                {
                    DataRow[] findCustom = CustomSP.Select(string.Format("VNContractID = '{0}'", MyUtility.Convert.GetString(dr["VNContractID"])));
                    if (findCustom.Length > 0)
                    {
                        string lastCustomsp = "SP"+(MyUtility.Convert.GetInt(MyUtility.Convert.GetString(findCustom[0]["CustomSP"]).Substring(2)) + 1).ToString("000000");
                        findCustom[0]["CustomSP"] = lastCustomsp;
                        dr["CustomSP"] = lastCustomsp;
                    }
                    else
                    {
                        DataRow newrow = CustomSP.NewRow();
                        newrow["VNContractID"] = MyUtility.Convert.GetString(dr["VNContractID"]);
                        newrow["CustomSP"] = "SP000001";
                        dr["CustomSP"] = "SP000001";
                    }
                }
            }
            MyUtility.Msg.InfoBox("Complete!!");
        }

        //update Contract
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,StartDate,EndDate from VNContract where GETDATE() between StartDate and EndDate order by StartDate", "15,10,10", textBox1.Text, headercaptions: "Contract No.,Start Date, End Date");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        //update Contract
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNContract where ID = '{0}'", textBox1.Text)))
                {
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    textBox1.Text = "";
                    return;
                }
            }
        }

        //update Contract
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1")
                {
                    dr["VNContractID"] = textBox1.Text;
                }
            }
        }

        //update Date
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1")
                {
                    if (MyUtility.Check.Empty(dateBox1.Value))
                    {
                        dr["CDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CDate"] = dateBox1.Value;
                    }
                }
            }
        }
        
        //Create
        private void button3_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            #region 檢查必輸欄位
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && (MyUtility.Check.Empty(dr["CustomSP"]) || MyUtility.Check.Empty(dr["VNContractID"]) || MyUtility.Check.Empty(dr["CDate"])))
                {
                    MyUtility.Msg.WarningBox("Custom SP# or Contract no. or Date can't empty!!");
                    return;
                }
            }
            #endregion

            #region 檢查合約是否合法，Custom SP#是否有重複
            DataTable ErrorData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource1.DataSource, "Selected,CustomSP,VNContractID,CDate",
                    @"with tmpInvalidContract
as (
select t.VNContractID, t.CustomSP, isnull(v.ID,'') as Contract, isnull(c.CustomSP,'') as CustomSPNo
from #tmp t
left join VNContract v on t.VNContractID = v.ID and t.CDate between	v.StartDate and v.EndDate
left join VNConsumption c on t.CustomSP = c.CustomSP and t.VNContractID = c.VNContractID
where t.Selected = 1
)

select *
from tmpInvalidContract
where Contract = '' or CustomSPNo <> ''", out ErrorData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query invalid date fail!!\r\n" + ex.ToString());
                return;
            }

            if (ErrorData.Rows.Count > 0)
            {
                if (MyUtility.Check.Empty(ErrorData.Rows[0]["Contract"]))
                {
                    MyUtility.Msg.WarningBox(string.Format("Custom SP# {0}'s contract can't use.", MyUtility.Convert.GetString(ErrorData.Rows[0]["CustomSP"])));
                    return;
                }
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("Custom SP# {0} already exist!!", MyUtility.Convert.GetString(ErrorData.Rows[0]["CustomSP"])));
                    return;
                }
            }
            #endregion

            #region 存檔
            IList<string> insertCmds = new List<string>();
            string newID, maxVersion, vnMultiple;
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && MyUtility.Check.Empty(dr["ID"]))
                {
                    insertCmds.Clear();
                    newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SP", "VNConsumption", Convert.ToDateTime(dr["CDate"]), 2, "ID", null);
                    maxVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(MAX(Version),0) as MaxVersion from VNConsumption where StyleUKey = {0}", MyUtility.Convert.GetString(dr["StyleUKey"])));
                    vnMultiple = MyUtility.GetValue.Lookup("select VNMultiple from System");
                    insertCmds.Add(string.Format(@"Insert into VNConsumption(ID,CustomSP,VNContractID,CDate,StyleID,StyleUKey,SeasonID,BrandID,Category,SizeCode,Qty,Version,CPU,VNMultiple,Status,AddName,AddDate)
Values ('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}',{10},'{11}',{12},{13},'Confirmed','{14}',GETDATE());", newID, MyUtility.Convert.GetString(dr["CustomSP"]), MyUtility.Convert.GetString(dr["VNContractID"])
                                                                                                                      , Convert.ToDateTime(dr["CDate"]).ToString("d"), MyUtility.Convert.GetString(dr["StyleID"]), MyUtility.Convert.GetString(dr["StyleUKey"])
                                                                                                                      , MyUtility.Convert.GetString(dr["SeasonID"]), MyUtility.Convert.GetString(dr["BrandID"]), MyUtility.Convert.GetString(dr["Category"])
                                                                                                                      , MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"]), MyUtility.Convert.GetString(MyUtility.Convert.GetInt(maxVersion) + 1).PadLeft(3, '0')
                                                                                                                      , MyUtility.Convert.GetString(dr["CPU"]), vnMultiple, Sci.Env.User.UserID));

                    insertCmds.Add(string.Format(@"Insert into VNConsumption_SizeCode (ID, SizeCode)
Values ('{0}','{1}');", newID, MyUtility.Convert.GetString(dr["SizeCode"])));

                    string[] article = MyUtility.Convert.GetString(dr["Article"]).Split(',');
                    foreach (string str in article)
                    {
                        if (!MyUtility.Check.Empty(str))
                        {
                            insertCmds.Add(string.Format(@"Insert into VNConsumption_Article (ID, Article)
Values ('{0}','{1}');", newID, str.ToString()));
                        }
                    }

                    DataRow[] selectedData = MidDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and Deleted = 0", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(','))));
                   
                    for (int i = 0; i < selectedData.Length; i++)
                    {
                        insertCmds.Add(string.Format(@"Insert into VNConsumption_Detail (ID,NLCode,HSCode,UnitID,Qty,UserCreate)
Values ('{0}','{1}','{2}','{3}',{4},{5});", newID, MyUtility.Convert.GetString(selectedData[i]["NLCode"]), MyUtility.Convert.GetString(selectedData[i]["HSCode"])
                          , MyUtility.Convert.GetString(selectedData[i]["UnitID"]), MyUtility.Convert.GetString(selectedData[i]["Qty"])
                          , MyUtility.Convert.GetString(selectedData[i]["UserCreate"]).ToUpper() == "TRUE" ? "1" : "0"));

                        DataRow[] selectedDetailData = AllDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}'", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), MyUtility.Convert.GetString(selectedData[i]["NLCode"])));
                        for (int j = 0; j < selectedDetailData.Length; j++)
                        {
                            if (!MyUtility.Check.Empty(selectedDetailData[j]["RefNo"]))
                            {
                                insertCmds.Add(string.Format(@"Insert into VNConsumption_Detail_Detail (ID, NLCode,SCIRefno,RefNo,Qty, LocalItem)
Values ('{0}','{1}','{2}','{3}',{4},{5});", newID, MyUtility.Convert.GetString(selectedData[i]["NLCode"]), MyUtility.Convert.GetString(selectedDetailData[j]["SCIRefno"])
                              , MyUtility.Convert.GetString(selectedDetailData[j]["RefNo"]), MyUtility.Convert.GetString(selectedDetailData[j]["Qty"]), MyUtility.Convert.GetString(selectedDetailData[j]["LocalItem"])));
                            }
                        }
                    }

                    DualResult result = DBProxy.Current.Executes(null, insertCmds);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Insert data fail, pls try again!\r\n" + result.ToString());
                        return;
                    }
                    else
                    {
                        dr["ID"] = newID;
                    }
                }
            }
            #endregion

            MyUtility.Msg.InfoBox("Complete!!");
        }

        //Empty NL Code (to Excel)
        private void button5_Click(object sender, EventArgs e)
        {
            DataTable toExcelData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(AllDetailData, "NLCode,RefNo,SuppID,BrandID,Description,Type",
                    @"select distinct RefNo,SuppID,BrandID,Description,Type from #tmp where NLCode = ''", out toExcelData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query empty NL Code data fail!!\r\n" + ex.ToString());
                return;
            }

            if (toExcelData.Rows.Count <= 0)
            {
                MyUtility.Msg.InfoBox("No data!!");
                return;
            }

            bool result = MyUtility.Excel.CopyToXls(toExcelData, "");
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
        }
    }
}
