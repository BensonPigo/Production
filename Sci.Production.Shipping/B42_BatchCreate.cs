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
using System.Transactions;

namespace Sci.Production.Shipping
{
    public partial class B42_BatchCreate : Sci.Win.Subs.Base
    {
        DataTable AllDetailData, MidDetailData;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        Ict.Win.UI.DataGridViewTextBoxColumn col_CustomSP;
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
            MyUtility.Tool.SetupCombox(comboCategory, 1, 1, "Bulk,Sample");
            comboCategory.SelectedIndex = -1;
            DataTable gridData;
            DBProxy.Current.Select(null, "select 0 as Selected,'' as CurrentCustomSP,'' as Article,'' as SizeCode,'' as Consumption,* from VNConsumption WITH (NOLOCK) where 1 = 0", out gridData);

            #region Current Custom的DbClick
            currentcustom.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
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
                        DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,StartDate,EndDate from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate order by StartDate", "15,10,10", MyUtility.Convert.GetString(dr["VNContractID"]), headercaptions: "Contract No.,Start Date, End Date");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        e.EditingControl.Text = item.GetSelectedString();
                    }
                }
            };

            vncontract.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["VNContractID"].ToString())
                {
                    if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", e.FormattedValue.ToString())))
                    {
                        dr["VNContractID"] = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Contract no. not found!!");
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
                        DataRow dr = this.gridBatchCreate.GetDataRow<DataRow>(e.RowIndex);
                        Sci.Production.Shipping.B42_BatchCreate_Consumption callNextForm = new Sci.Production.Shipping.B42_BatchCreate_Consumption(MidDetailData, AllDetailData, MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), MyUtility.Convert.GetString(dr["VNContractID"]));
                        DialogResult result = callNextForm.ShowDialog(this);
                        callNextForm.Dispose();
                    }
                }
            };
            #endregion

            this.gridBatchCreate.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridBatchCreate)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("CustomSP", header: "Custom SP#", width: Widths.AnsiChars(8)).Get(out col_CustomSP)
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

            col_CustomSP.MaxLength = 8;
            listControlBindingSource1.DataSource = gridData;
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateBuyerDelivery.Value1))
            {
                dateBuyerDelivery.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Buyer Delivery can't empty!");
                return;
            }
            DataTable GroupData, gridData;
            DataTable[] GandM;
            StringBuilder sqlCmd = new StringBuilder();
            string contractID = MyUtility.GetValue.Lookup(@"select ID from VNContract WITH (NOLOCK) where StartDate = (select MAX(StartDate) from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate and Status = 'Confirmed')");
            #region 組撈所有明細SQL
            sqlCmd.Append(string.Format(@"
Declare @vncontractid varchar(15)
set @vncontractid = '{0}';

select  o.StyleUkey
        , oqd.SizeCode
        , oqd.Article
        , o.Category
        , o.StyleID
        , o.SeasonID
        , o.BrandID as OrderBrandID
        , sum(oqd.Qty) as GMTQty
        , isnull(s.CPU,0) as StyleCPU
        , isnull(s.CTNQty,0) as CTNQty
into #tmpAllStyle
from Order_QtyShip oq WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on oq.ID = o.ID
inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID 
                                                     and oq.Seq = oqd.Seq
left join Style s WITH (NOLOCK) on o.StyleUkey = s.Ukey
where   1=1", contractID));

            if (!MyUtility.Check.Empty(dateBuyerDelivery.Value1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(dateBuyerDelivery.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateBuyerDelivery.Value2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(dateBuyerDelivery.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(txtstyle.Text))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", txtstyle.Text));
            }
            if (!MyUtility.Check.Empty(comboCategory.Text))
            {
                sqlCmd.Append(string.Format(" and o.Category = '{0}'", comboCategory.Text == "Bulk" ? "B" : "S"));
            }
            if (!MyUtility.Check.Empty(txtbrand.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", txtbrand.Text));
            }
            sqlCmd.Append(@"
group by o.StyleUkey, oqd.SizeCode, oqd.Article, o.Category, o.StyleID
         , o.SeasonID, o.BrandID, isnull(s.CPU,0), isnull(s.CTNQty,0)
		 
--------------------------------------------------------------------------------------------------------------------------------------------------
select  ts.*
        , sm.MarkerName
        , sm.FabricPanelCode
        , dbo.MarkerLengthToYDS(sm.MarkerLength) as markerYDS
        , sm.Width
        , sms.Qty
        , sc.FabricCode
        , sfqt.QTFabricCode
into #tmpMarkerData
from #tmpAllStyle ts
inner join Style_MarkerList sm WITH (NOLOCK) on sm.StyleUkey = ts.StyleUkey
inner join Style_MarkerList_SizeQty sms WITH (NOLOCK) on sm.Ukey = sms.Style_MarkerListUkey 
                                                         and sms.SizeCode = ts.SizeCode
inner join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sm.StyleUkey 
                                                and sc.FabricPanelCode = sm.FabricPanelCode
left join Style_MarkerList_Article sma WITH (NOLOCK) on sm.Ukey = sma.Style_MarkerListUkey 
left join Style_FabricCode_QT sfqt WITH (NOLOCK) on sm.FabricPanelCode = sfqt.FabricPanelCode 
                                                    and sm.StyleUkey = sfqt.StyleUkey
where   sm.MixedSizeMarker = 1 
        and (sma.Article is null or sma.Article = ts.Article) 
        and sc.Article = ts.Article
        and CHARINDEX('+',sm.MarkerLength) > 0

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.StyleID
        , t.SeasonID
        , t.OrderBrandID
        , t.Category
        , t.SizeCode
        , t.Article
        , t.GMTQty
        , t.markerYDS
        , t.Width
        , t.Qty
        , IIF(t.QTFabricCode is null, sb.SCIRefno, sb1.SCIRefno) as SCIRefNo
        , IIF(t.QTFabricCode is null, sb.SuppIDBulk, sb1.SuppIDBulk) as SuppIDBulk
        , t.StyleCPU
        , t.StyleUKey
into #tmpFabricCode
from #tmpMarkerData t
left join Style_BOF sb WITH (NOLOCK) on sb.StyleUkey = t.StyleUkey 
                                        and sb.FabricCode = t.FabricCode
left join Style_BOF sb1 WITH (NOLOCK) on sb1.StyleUkey = t.StyleUkey 
                                         and sb1.FabricCode = t.QTFabricCode

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.StyleID
        , t.SeasonID
        , t.OrderBrandID
        , t.Category
        , t.SizeCode
        , t.Article
        , t.GMTQty
        , t.StyleCPU
        , t.StyleUKey
        , t.markerYDS
        , 'YDS' as UsageUnit
        , t.Qty
        , f.SCIRefno
        , f.Refno
        , f.BrandID
        , f.NLCode
        , f.HSCode
        , f.CustomsUnit
        , f.Width,f.Type
        , f.PcsWidth
        , f.PcsLength
        , f.PcsKg
        , f.Description
        , isnull((select RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit),1) as RateValue
        , (select RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') as M2RateValue
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit),0) as UnitRate
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M'),0) as M2UnitRate
into #tmpBOFRateData
from #tmpFabricCode t
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = t.SCIRefno
where   (t.SuppIDBulk <> 'FTY' or t.SuppIDBulk <> 'FTY-C')
        and f.NoDeclare = 0

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , StyleCPU
        , StyleUKey
        , Description
        , IIF(Type = 'F','Fabric',IIF(Type = 'A','Accessory','')) as Type
        , ([dbo].getVNUnitTransfer(Type,UsageUnit,CustomsUnit,markerYDS,Width,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),iif(Qty=0.000,0.000,IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)))/Qty) as NewQty
into #tmpBOFNewQty
from #tmpBOFRateData

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , sum(isnull(NewQty,0)) as Qty
        , 0 as LocalItem
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , '' as SuppID
into #tmpBOFData
from #tmpBOFNewQty
group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article
         , GMTQty, SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit
         , StyleCPU, StyleUKey, Description, Type

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.*
        , sb.Ukey
        , sb.Refno
        , sb.SCIRefno
        , sb.SuppIDBulk
        , sb.SizeItem
        , sb.PatternPanel
        , sb.BomTypeArticle
        , sb.BomTypeColor
        , sb.ConsPC
        , sc.ColorID
        , f.UsageUnit
        , HSCode = isnull (f.HSCode, '')
        , NLCode = isnull (f.NLCode, '')
        , CustomsUnit = isnull (f.CustomsUnit, '')
        , f.PcsWidth
        , f.PcsLength
        , f.PcsKg
        , f.BomTypeCalculate
        , f.Type
        , f.BrandID
        , f.Description
into #tmpBOA
from #tmpAllStyle t
inner join Style_BOA sb WITH (NOLOCK) on t.StyleUkey = sb.StyleUkey
left join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sb.StyleUkey 
                                               and sc.PatternPanel = sb.PatternPanel 
                                               and sc.Article = t.Article
left join Fabric f WITH (NOLOCK) on sb.SCIRefno = f.SCIRefno
where   sb.IsCustCD <> 2
        and (sb.SuppIDBulk <> 'FTY' and sb.SuppIDBulk <> 'FTY-C')

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.*
        , IIF(t.BomTypeCalculate = 1, isnull(dbo.GetDigitalValue(s.SizeSpec),0)
				                    , ConsPC) as SizeSpec
        , isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.UsageUnit and TO_U = t.CustomsUnit),1) as RateValue
        , (select RateValue from dbo.View_Unitrate where FROM_U = t.UsageUnit and TO_U = 'M') as M2RateValue
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = t.CustomsUnit),'') as UnitRate
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = 'M'),'') as M2UnitRate
into #tmpBOAPrepareData
from #tmpBOA t
outer apply(
	select SizeSpec
	from Style_SizeSpec 
	where   StyleUkey = t.StyleUkey 
            and SizeItem = t.SizeItem 
            and SizeCode = t.SizeCode 
)S
where   (t.BomTypeArticle = 0 and t.BomTypeColor = 0) 
        or ((t.BomTypeArticle = 1 or t.BomTypeColor = 1) and t.ColorID is not null)

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , StyleCPU
        , StyleUKey
        , Description
        , IIF(Type = 'F','Fabric',IIF(Type = 'A','Accessory','')) as Type
        , [dbo].getVNUnitTransfer(Type,UsageUnit,CustomsUnit,SizeSpec,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
into #tmpBOANewQty
from #tmpBOAPrepareData

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , sum(ISNULL(NewQty,0)) as Qty
        , 0 as LocalItem
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , '' as SuppID
into #tmpBOAData
from #tmpBOANewQty
group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article
         , GMTQty, SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit
         , StyleCPU, StyleUKey, Description, Type

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.*
        , ld.Refno
        , ld.Qty
        , ld.UnitId
        , li.MeterToCone
        , li.NLCode
        , li.HSCode
        , CustomsUnit = isnull (li.CustomsUnit, '')
        , li.PcsWidth
        , li.PcsLength
        , li.PcsKg
        , o.Qty as OrderQty
        , isnull(vd.Waste,0) as Waste
        , li.Description
        , li.Category as Type
        , li.LocalSuppid as SuppID
into #tmpLocalPO
from #tmpAllStyle t
inner join LocalPO_Detail ld WITH (NOLOCK) on ld.OrderId = (select TOP 1 ID 
                                                            from Orders WITH (NOLOCK) 
                                                            where   StyleUkey = t.StyleUkey 
                                                                    and Category = t.Category 
                                                            order by BuyerDelivery, ID)
left join LocalItem li WITH (NOLOCK) on li.RefNo = ld.Refno
left join Orders o WITH (NOLOCK) on ld.OrderId = o.ID
left join View_VNNLCodeWaste vd WITH (NOLOCK) on  vd.NLCode = li.NLCode
where li.NoDeclare = 0

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , StyleCPU
        , StyleUKey
        , Refno
        , IIF(UnitId = 'CONE',Qty*MeterToCone,Qty) as Qty,OrderQty, IIF(UnitId = 'CONE','M',UnitId) as UnitId
        , Waste
        , NLCode
        , HSCode
        , CustomsUnit
        , PcsWidth
        , PcsLength
        , PcsKg
        , Description
        , Type
        , SuppID
into #tmpConeToM
from #tmpLocalPO

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , StyleCPU
        , StyleUKey
        , Refno
        , iif(OrderQty=0.000,0.000,Qty/OrderQty) as Qty,UnitId
        , NLCode
        , HSCode
        , CustomsUnit
        , PcsWidth
        , PcsLength
        , PcsKg
        , Waste
        , Description
        , Type
        , SuppID
        , isnull((select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = CustomsUnit),1) as RateValue
        , (select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = 'M') as M2RateValue
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = CustomsUnit),'') as UnitRate
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = 'M'),'') as M2UnitRate
into #tmpPrepareRate
from #tmpConeToM

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , Refno as SCIRefno
        , Refno
        , '' as BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , Waste
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , SuppID
        , [dbo].getVNUnitTransfer('',UnitId,CustomsUnit,Qty,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
into #tmpLocalNewQty
from #tmpPrepareRate

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , sum(isnull(NewQty,0)) as Qty
        , 1 as LocalItem
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , SuppID
into #tmpLocalData
from #tmpLocalNewQty
group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article, GMTQty
         , SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit ,StyleCPU 
         , StyleUKey, Description, Type, SuppID

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.StyleID
        , t.SeasonID
        , t.OrderBrandID
        , t.Category
        , t.SizeCode
        , Article = isnull (t.Article, '')
        , t.GMTQty
        , vfd.* 
        , sa.TissuePaper as ArticleTissuePaper
        , t.CTNQty
        , t.StyleCPU
        , t.StyleUKey
into #tmpFixDeclare
from VNFixedDeclareItem vfd WITH (NOLOCK) 
left join #tmpAllStyle t on 1 = 1
left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = t.StyleUkey 
                                            and sa.Article = t.Article

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , '' as SCIRefno
        , '' as Refno
        , '' as BrandID
        , '' as SuppID
        , NLCode
        , HSCode
        , UnitID as CustomsUnit
        , IIF(Type = 1, Qty, IIF(CTNQty = 0,0,ROUND(Qty/CTNQty,3))) as Qty
        , 0 as LocalItem
        , StyleCPU
        , StyleUKey
        , '' as Description
        , '' as Type
into #tmpFinalFixDeclare
from #tmpFixDeclare
where   TissuePaper = 0 
        or (TissuePaper = 1 and ArticleTissuePaper = 1)

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , Qty
        , LocalItem
        , StyleCPU
        , StyleUKey
into #tlast
from (
    select  StyleID
            , SeasonID
            , OrderBrandID
            , Category
            , SizeCode
            , Article
            , GMTQty
            , SCIRefno
            , Refno
            , BrandID
            , NLCode
            , HSCode
            , CustomsUnit
            , Qty
            , LocalItem
            , StyleCPU
            , StyleUKey
            , Description
            , Type
            , SuppID 
    from #tmpFinalFixDeclare
    
    union
    select  StyleID
            , SeasonID
            , OrderBrandID
            , Category
            , SizeCode
            , Article
            , GMTQty
            , SCIRefno
            , Refno
            , BrandID
            , NLCode
            , HSCode
            , CustomsUnit
            , sum(Qty) as Qty
            , LocalItem
            , StyleCPU
            , StyleUKey
            , Description
            , Type
            , SuppID
    from (
        select * 
        from #tmpBOFData

        union all
        select * 
        from #tmpBOAData

        union all
        select * 
        from #tmpLocalData
    ) a
    group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article
             , GMTQty, SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit
             , LocalItem, StyleCPU, StyleUKey,Description,Type,SuppID
)x

--------------------------------------------------------------------------------------------------------------------------------------------------
select 	t.*
		,0 Waste
from #tlast t 
--left join VNContract_Detail v with(nolock) on id = @vncontractid 
--											  and v.NLCode = t.NLCode

--------------------------------------------------------------------------------------------------------------------------------------------------
drop table #tmpAllStyle
drop table #tmpMarkerData
drop table #tmpFabricCode
drop table #tmpBOFRateData
drop table #tmpBOFNewQty
drop table #tmpBOFData
drop table #tmpBOA
drop table #tmpBOAPrepareData
drop table #tmpBOANewQty
drop table #tmpBOAData
drop table #tmpLocalPO
drop table #tmpConeToM
drop table #tmpPrepareRate
drop table #tmpLocalNewQty
drop table #tmpLocalData
drop table #tmpFixDeclare
drop table #tmpFinalFixDeclare
drop table #tlast");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out AllDetailData);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query detail data fail.\r\n{0}", result.ToString()));
                return;
            }
            #region 整理出依Style,Season Group的資料
            try
            {
                MyUtility.Tool.ProcessWithDatatable(AllDetailData, "",
                    @"
alter table #tmp alter column StyleID		varchar(100)
alter table #tmp alter column SeasonID		varchar(100)
alter table #tmp alter column OrderBrandID	varchar(100)
alter table #tmp alter column Category		varchar(100)
alter table #tmp alter column SizeCode		varchar(100)
alter table #tmp alter column Article		varchar(100)
alter table #tmp alter column GMTQty		varchar(100)
alter table #tmp alter column SCIRefno		varchar(100)
alter table #tmp alter column Refno			varchar(100)
alter table #tmp alter column BrandID		varchar(100)
alter table #tmp alter column NLCode		varchar(100)
alter table #tmp alter column HSCode		varchar(100)
alter table #tmp alter column CustomsUnit	varchar(100)
alter table #tmp alter column LocalItem		varchar(100)
alter table #tmp alter column StyleCPU		varchar(100)
alter table #tmp alter column StyleUKey		varchar(100)

select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , Article
        , SizeCode
        , NLCode
        , SUM(Qty) as Qty
        , CustomsUnit 
into #tmpSumConsData
from #tmp
group by StyleID, SeasonID, OrderBrandID, Category, Article, SizeCode, NLCode, CustomsUnit;

select  distinct StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , Article
        , SizeCode
        , GMTQty
        , StyleCPU
        , StyleUkey 
into #tmpGroupStyle
from #tmp

DECLARE cursor_tmpBasic CURSOR FOR
SELECT  Distinct StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , StyleCPU
        , StyleUkey 
FROM #tmpGroupStyle

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

select @contract = ID 
from VNContract WITH (NOLOCK) 
where StartDate = (select MAX(StartDate) 
                   from VNContract WITH (NOLOCK) 
                   where GETDATE() between StartDate and EndDate 
                         and Status = 'Confirmed')

OPEN cursor_tmpBasic
FETCH NEXT FROM cursor_tmpBasic INTO @style, @season, @brand, @category,@size,@cpu,@styleukey
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @firstrecord = 1
	SET @recordno = 0


	DECLARE cursor_tmpArticle CURSOR FOR
	select Article
           , GMTQty 
    from #tmpGroupStyle 
    where StyleID = @style 
          and SeasonID = @season 
          and OrderBrandID = @brand 
          and Category = @category 
          and SizeCode = @size;

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
				select Article 
                from @tempCombColor 
                where StyleID = @style 
                      and SeasonID = @season 
                      and BrandID = @brand 
                      and Category = @category 
                      and SizeCode = @size

				OPEN cursor_tmpCombColor
				FETCH NEXT FROM cursor_tmpCombColor INTO @comboarticle
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @allmatch = 0
					SET @comboarticle = SUBSTRING(@comboarticle,1,PATINDEX('%,%',@comboarticle))

					DECLARE cursor_tmpSumConsData CURSOR FOR
					select NLCode
                           , Qty
                           , CustomsUnit 
                    from #tmpSumConsData 
                    where StyleID = @style 
                          and SeasonID = @season 
                          and OrderBrandID = @brand 
                          and Category = @category 
                          and SizeCode = @size 
                          and Article = @comboarticle

					OPEN cursor_tmpSumConsData
					FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
					WHILE @@FETCH_STATUS = 0
					BEGIN
						
						select @allmatch = IIF(@usedqty = isnull(Qty,0),1,0) 
                        from #tmpSumConsData 
                        where StyleID = @style 
                              and SeasonID = @season 
                              and OrderBrandID = @brand 
                              and Category = @category 
                              and SizeCode = @size 
                              and Article = @comboarticle 
                              and NLCode = @nlcode 
                              and CustomsUnit = @customsunit
						
						IF @allmatch = 0
							Break;

						FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
					END
					CLOSE cursor_tmpSumConsData
					DEALLOCATE cursor_tmpSumConsData

					IF @allmatch = 0
						BEGIN
							DECLARE cursor_tmpSumConsData CURSOR FOR
							select NLCode
                                   , Qty
                                   , CustomsUnit 
                            from #tmpSumConsData 
                            where StyleID = @style 
                                  and SeasonID = @season 
                                  and OrderBrandID = @brand 
                                  and Category = @category 
                                  and SizeCode = @size 
                                  and Article = @article

							OPEN cursor_tmpSumConsData
							FETCH NEXT FROM cursor_tmpSumConsData INTO @nlcode,@usedqty,@customsunit
							WHILE @@FETCH_STATUS = 0
							BEGIN
						
								select @allmatch = IIF(@usedqty = isnull(Qty,0),1,0) 
                                from #tmpSumConsData 
                                where StyleID = @style 
                                      and SeasonID = @season 
                                      and OrderBrandID = @brand 
                                      and Category = @category 
                                      and SizeCode = @size 
                                      and Article = @article 
                                      and NLCode = @nlcode 
                                      and CustomsUnit = @customsunit
						
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
				insert into @tempCombColor (
                    StyleID             , SeasonID  , BrandID       , Category      , Article
                    , SizeCode          ,GMTQty     , StyleCPU      , VNContractID  , CustomSP  
                    , StyleUkey
                ) values (
                    @style              , @season   , @brand        , @category , @article + ','
                    , @size             , @gmtqty   , @cpu          , @contract , isnull((select top 1 v.CustomSP 
                                                                                          from VNConsumption v, VNConsumption_Article va, VNConsumption_SizeCode vs 
                                                                                          where v.VNContractID = @contract 
                                                                                                and v.StyleID = @style 
                                                                                                and v.SeasonID = @season 
                                                                                                and v.BrandID = @brand 
                                                                                                and v.ID = va.ID 
                                                                                                and v.ID = vs.ID 
                                                                                                and va.Article = @article 
                                                                                                and vs.SizeCode = @size),'')
                    , @styleukey)
			END
		ELSE
			BEGIN
				update @tempCombColor 
                set GMTQty = GMTQty + @gmtqty
                    , Article = Article + @article + ',' 
				where StyleID = @style 
                      and SeasonID = @season 
                      and BrandID = @brand 
                      and Category = @category 
                      and SizeCode = @size
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

select * 
from @tempCombColor 
order by StyleID, SeasonID, Category, Article, SizeCode

select distinct id = ''
       , NLCode
       ,t.HSCode
       , UnitID = t.CustomsUnit
       , Qty = sum(t.Qty) over(partition by t.StyleUkey,t.SizeCode,t.Article,t.NLCode,t.Category)
       , UserCreate = 0
       , x.StyleUkey
       , x.SizeCode
       , t.Article
       , t.Waste
       , t.Category
       , Deleted = 0
from #tmp t, @tempCombColor x
where t.StyleUKey = x.StyleUkey 
      and t.Article = SUBSTRING(x.Article,0, CHARINDEX(',',x.Article)) 
      and t.SizeCode = x.SizeCode 
      and t.Category = x.Category", out GandM);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query group data fail!!\r\n" + ex.ToString());
                return;
            }
            #endregion
            GroupData = GandM[0];
            MidDetailData = GandM[1];
            //撈出每個Consumption一定都要有的NLCode
            DataTable NecessaryItem;
            result = DBProxy.Current.Select(null, string.Format("select NLCode from VNContract_Detail WITH (NOLOCK) where NecessaryItem = 1 and ID = '{0}'", contractID), out NecessaryItem);
            if (!result)
            {
                MyUtility.Msg.WarningBox(string.Format("Query NecessaryItem data fail.\r\n{0}", result.ToString()));
                return;
            }

            gridData = (DataTable)listControlBindingSource1.DataSource;
            gridData.Clear();
            string colorway, lackNLCode;
            foreach (DataRow dr in GroupData.Rows)
            {
                colorway = MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(','));
                //找出是否有空白的NL Code
                DataRow[] findData = AllDetailData.Select(string.Format("StyleUKey = '{0}' and Article = '{1}' and SizeCode = '{2}' and NLCode = '' and Category = '{3}'",
                    MyUtility.Convert.GetString(dr["StyleUKey"]), colorway, MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Category"])));

                //找出是否有缺料
                lackNLCode = "";
                foreach (DataRow lack in NecessaryItem.Rows)
                {
                    DataRow[] findLackData = AllDetailData.Select(string.Format("StyleUKey = '{0}' and Article = '{1}' and SizeCode = '{2}' and NLCode = '{3}' and Category = '{4}'",
                    MyUtility.Convert.GetString(dr["StyleUKey"]), colorway, MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(lack["NLCode"]), MyUtility.Convert.GetString(dr["Category"])));
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
                newrow["Consumption"] = (MyUtility.Check.Empty(lackNLCode) ? "" : ("Lacking Customs Code:" + lackNLCode.Substring(0, lackNLCode.Length - 1) + ". ")) + (findData.Length > 0 ? "Appear empty Customs Code." : "");

                gridData.Rows.Add(newrow);
            }

            #region 組出中間層資料
            ///////////////////
            //result = DBProxy.Current.Select(null, "select *,0 as StyleUkey,'' as SizeCode, '' as Article,0.0 as Waste, 0 as Deleted from VNConsumption_Detail WITH (NOLOCK) where 1 = 0", out MidDetailData);
            //if (!result)
            //{
            //    MyUtility.Msg.WarningBox("Prepare detail structure fail!!");
            //    return;
            //}
            //foreach (DataRow dr in gridData.Rows)
            //{
            //    string article = MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(','));
            //    DataRow[] selectedData = AllDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}'", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), article));
            //    for (int i = 0; i < selectedData.Length; i++)
            //    {
            //        DataRow[] finddata = MidDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}'", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), article, MyUtility.Convert.GetString(selectedData[i]["NLCode"])));

            //        if (finddata.Length > 0)
            //        {
            //            finddata[0]["Qty"] = MyUtility.Convert.GetDecimal(finddata[0]["Qty"]) + MyUtility.Convert.GetDecimal(selectedData[i]["Qty"]);
            //        }
            //        else
            //        {
            //            DataRow newrow = MidDetailData.NewRow();
            //            newrow["NLCode"] = selectedData[i]["NLCode"];
            //            newrow["HSCode"] = selectedData[i]["HSCode"];
            //            newrow["UnitID"] = selectedData[i]["CustomsUnit"];
            //            newrow["Qty"] = selectedData[i]["Qty"];
            //            newrow["Waste"] = selectedData[i]["Waste"];
            //            newrow["UserCreate"] = 0;
            //            newrow["StyleUKey"] = dr["StyleUKey"];
            //            newrow["SizeCode"] = dr["SizeCode"];
            //            newrow["Article"] = article;
            //            newrow["Deleted"] = 0;
            //            MidDetailData.Rows.Add(newrow);
            //        }
            //    }
            //}
            #endregion

            listControlBindingSource1.DataSource = gridData;
        }

        //Auto Custom SP#
        private void btnAutoCustomSPNo_Click(object sender, EventArgs e)
        {
            gridBatchCreate.ValidateControl();
            DataTable CustomSP;
            DualResult result = DBProxy.Current.Select(null, "select VNContractID,MAX(CustomSP) as CustomSP from VNConsumption WITH (NOLOCK) group by VNContractID", out CustomSP);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail, please try again.\r\n" + result.ToString());
                return;
            }

            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && !MyUtility.Check.Empty(dr["VNContractID"]))
                {
                    DataRow[] findCustom = CustomSP.Select(string.Format("VNContractID = '{0}'", MyUtility.Convert.GetString(dr["VNContractID"])));
                    if (findCustom.Length > 0)
                    {
                        string lastCustomsp = "SP" + (MyUtility.Convert.GetInt(MyUtility.Convert.GetString(findCustom[0]["CustomSP"]).Substring(2)) + 1).ToString("000000");
                        if (lastCustomsp.Length > 8)
                        {
                            MyUtility.Msg.InfoBox(string.Format("<CustomSP : {0}>  length can't be more than 8 Characters", lastCustomsp));
                            return; 
                        }
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
        private void txtVNContractID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,StartDate,EndDate from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate order by StartDate", "15,10,10", txtVNContractID.Text, headercaptions: "Contract No.,Start Date, End Date");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtVNContractID.Text = item.GetSelectedString();
        }

        //update Contract
        private void txtVNContractID_Validating(object sender, CancelEventArgs e)
        {
            if (txtVNContractID.OldValue != txtVNContractID.Text)
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", txtVNContractID.Text)))
                {
                    txtVNContractID.Text = "";
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
            }
        }

        //update Contract
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            gridBatchCreate.ValidateControl();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1")
                {
                    dr["VNContractID"] = txtVNContractID.Text;
                }
            }
        }

        //update Date
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            gridBatchCreate.ValidateControl();
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["Selected"]) == "1")
                {
                    if (MyUtility.Check.Empty(dateCdate.Value))
                    {
                        dr["CDate"] = DBNull.Value;
                    }
                    else
                    {
                        dr["CDate"] = dateCdate.Value;
                    }
                }
            }
        }

        //Create
        private void btnCreate_Click(object sender, EventArgs e)
        {
            gridBatchCreate.ValidateControl();
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
                    @"
with tmpInvalidContract as (
    select t.VNContractID
           , t.CustomSP
           , Contract = isnull(v.ID,'')
           , CustomSPNo = isnull(c.CustomSP,'')
    from #tmp t
    left join VNContract v WITH (NOLOCK) on t.VNContractID = v.ID 
                                            and t.CDate between	v.StartDate and v.EndDate
    left join VNConsumption c WITH (NOLOCK) on t.CustomSP = c.CustomSP 
                                               and t.VNContractID = c.VNContractID
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
                TransactionScope transcation = new TransactionScope();
                using (transcation)
                {
                    if (MyUtility.Convert.GetString(dr["Selected"]) == "1" && MyUtility.Check.Empty(dr["ID"]))
                    {
                        insertCmds.Clear();
                        newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SP", "VNConsumption", Convert.ToDateTime(dr["CDate"]), 2, "ID", null);
                        maxVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(MAX(Version),0) as MaxVersion from VNConsumption WITH (NOLOCK) where StyleUKey = {0}", MyUtility.Convert.GetString(dr["StyleUKey"])));
                        vnMultiple = MyUtility.GetValue.Lookup("select VNMultiple from System WITH (NOLOCK) ");
                        insertCmds.Add(string.Format(@"
Insert into VNConsumption (
	ID 			, CustomSP 	, VNContractID 	, CDate 		, StyleID
	, StyleUKey , SeasonID 	, BrandID 		, Category 		, SizeCode
	, Qty 		, Version 	, CPU 			, VNMultiple 	, Status
	, AddName 	, AddDate
) Values (
	'{0}' 		, '{1}' 	, '{2}' 		, '{3}' 		, '{4}'
	, {5} 		, '{6}' 	, '{7}' 		, '{8}' 		, '{9}'
	, {10} 		, '{11}' 	, {12} 			, {13} 			, 'Confirmed'
	, '{14}' 	,GETDATE()
);", newID
        , MyUtility.Convert.GetString(dr["CustomSP"])
        , MyUtility.Convert.GetString(dr["VNContractID"])
        , Convert.ToDateTime(dr["CDate"]).ToString("d")
        , MyUtility.Convert.GetString(dr["StyleID"])
        , MyUtility.Convert.GetString(dr["StyleUKey"])
        , MyUtility.Convert.GetString(dr["SeasonID"])
        , MyUtility.Convert.GetString(dr["BrandID"])
        , MyUtility.Convert.GetString(dr["Category"])
        , MyUtility.Convert.GetString(dr["SizeCode"])
        , MyUtility.Convert.GetString(dr["Qty"])
        , MyUtility.Convert.GetString(MyUtility.Convert.GetInt(maxVersion) + 1).PadLeft(3, '0')
        , MyUtility.Convert.GetString(dr["CPU"])
        , vnMultiple
        , Sci.Env.User.UserID));

                        insertCmds.Add(string.Format(@"
Insert into VNConsumption_SizeCode (
	ID 		, SizeCode
) Values (
	'{0}' 	,'{1}'
);" , newID
    , MyUtility.Convert.GetString(dr["SizeCode"])));

                        string[] article = MyUtility.Convert.GetString(dr["Article"]).Split(',');
                        foreach (string str in article)
                        {
                            if (!MyUtility.Check.Empty(str))
                            {
                                insertCmds.Add(string.Format(@"
Insert into VNConsumption_Article (
	ID 		, Article
) Values (
	'{0}' 	, '{1}'
);", newID
        , str.ToString()));
                            }
                        }

                        DataRow[] selectedData = MidDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and Category = '{3}' and Deleted = 0", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), dr["Category"]));

                        for (int i = 0; i < selectedData.Length; i++)
                        {
                            insertCmds.Add(string.Format(@"
Insert into VNConsumption_Detail (
	ID 				, NLCode 	, HSCode 	, UnitID 	, Qty
	, UserCreate	, SystemQty
) Values (
	'{0}' 			, '{1}' 	, '{2}' 	, '{3}' 	, {4}
	, {5}			, {4}
);" , newID
    , MyUtility.Convert.GetString(selectedData[i]["NLCode"])
    , MyUtility.Convert.GetString(selectedData[i]["HSCode"])
    , MyUtility.Convert.GetString(selectedData[i]["UnitID"])
    , MyUtility.Convert.GetString(selectedData[i]["Qty"])
    , MyUtility.Convert.GetString(selectedData[i]["UserCreate"]).ToUpper() == "TRUE" ? "1" : "0"));

                            DataRow[] selectedDetailData = AllDetailData.Select(string.Format("StyleUKey = {0} and SizeCode = '{1}' and Article = '{2}' and NLCode = '{3}'", MyUtility.Convert.GetString(dr["StyleUKey"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Article"]).Substring(0, MyUtility.Convert.GetString(dr["Article"]).IndexOf(',')), MyUtility.Convert.GetString(selectedData[i]["NLCode"])));
                            for (int j = 0; j < selectedDetailData.Length; j++)
                            {
                                if (!MyUtility.Check.Empty(selectedDetailData[j]["RefNo"]))
                                {
                                    insertCmds.Add(string.Format(@"
Insert into VNConsumption_Detail_Detail (
	ID 			, NLCode 	, SCIRefno 	, RefNo 	, Qty
	, LocalItem
) Values (
	'{0}' 		, '{1}' 	, '{2}' 	, '{3}' 	, {4}
	,{5}
);" , newID
    , MyUtility.Convert.GetString(selectedData[i]["NLCode"])
    , MyUtility.Convert.GetString(selectedDetailData[j]["SCIRefno"].ToString().Replace("'", "''"))
    , MyUtility.Convert.GetString(selectedDetailData[j]["RefNo"].ToString().Replace("'", "''"))
    , MyUtility.Convert.GetString(selectedDetailData[j]["Qty"])
    , MyUtility.Convert.GetString(selectedDetailData[j]["LocalItem"])));
                                }
                            }
                        }

                        DualResult result = DBProxy.Current.Executes(null, insertCmds);
                        if (!result)
                        {
                            transcation.Dispose();
                            MyUtility.Msg.WarningBox("Insert data fail, pls try again!\r\n" + result.ToString());
                            return;
                        }
                        else
                        {
                            dr["ID"] = newID;
                        }
                        transcation.Complete();
                        transcation.Dispose();
                    }
                }
            }
            #endregion

            MyUtility.Msg.InfoBox("Complete!!");
        }

        //Empty NL Code (to Excel)
        private void btnEmptyNLCodetoExcel_Click(object sender, EventArgs e)
        {
            DataTable toExcelData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(AllDetailData, "NLCode,RefNo,SuppID,BrandID,Description,Type",
                    @"select distinct RefNo,SuppID,BrandID,Description,Type from #tmp where NLCode = ''", out toExcelData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query empty Customs Code data fail!!\r\n" + ex.ToString());
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
