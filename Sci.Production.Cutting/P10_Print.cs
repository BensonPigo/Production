using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P10_Print : Win.Tems.PrintForm
    {
        private readonly DataRow CurrentDataRow;
        private DualResult result;

        /// <inheritdoc/>
        public P10_Print(DataRow row)
        {
            this.InitializeComponent();
            this.CurrentDataRow = row;
            this.toexcel.Enabled = false;
            MyUtility.Tool.SetupCombox(this.comboLayout, 2, 1, "0,Layout1,1,Layout2");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        private DataTable dtt;
        private DataTable dt;

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.radioBundleCard.Checked == true)
            {
                #region report
                string id = this.CurrentDataRow["ID"].ToString();

                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID_p", id),
                    new SqlParameter("@CutRef_p", this.CurrentDataRow["cutref"].ToString()),
                    new SqlParameter("@POID_p", this.CurrentDataRow["POID"].ToString()),
                    new SqlParameter("@extend_p", this.checkExtendAllParts.Checked ? "1" : "0"),
                };

                string scmd;
                if (this.checkExtendAllParts.Checked)
                {
                    scmd = string.Format(@"
declare @extend varchar(1) = @extend_p
,@POID varchar(13) = @POID_p
,@Cutref varchar(8) = @CutRef_p
,@ID varchar(13) = @ID_p

select distinct *
from (
    select a.BundleGroup [Group_right]
	    ,c.FactoryID  [Group_left]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,b.POID
	    ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
        ,c.StyleID [Style]
        ,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @POID),'') as [MarkerNo]
        , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno), iif(a.Tone='','','-'+a.Tone))
	    ,a.Parts [Parts]
        ,b.Article + '\' + b.Colorid [Color]
        ,b.Article
        ,a.SizeCode [Size]
        ,'(' + qq.Cutpart + ')' + a.PatternDesc [Desc]
        ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
        ,a.Qty [Quantity]
        ,a.BundleNo [Barcode]
        ,SeasonID = concat(c.SeasonID,' ', c.dest)
        ,brand=c.brandid
        ,brand.ShipCode
        ,b.item
        ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'X' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
    inner join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
    left join brand WITH (NOLOCK) on brand.id = c.brandid
    outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
    outer apply
    (
	    select Artwork = 
	    (
		    select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
		    from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
		    where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
		    for xml path('')
	    )
    )as Artwork
    where a.ID= @ID and a.Patterncode != 'ALLPARTS'

    union all

    select a.BundleGroup [Group_right]
	    ,c.FactoryID  [Group_left]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,b.POID
	    ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
        ,c.StyleID [Style]
        ,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @POID),'') as [MarkerNo]
        , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno), iif(a.Tone='','','-'+a.Tone))
	    ,d.Parts [Parts]
        ,b.Article + '\' + b.Colorid [Color]
        ,b.Article
        ,a.SizeCode [Size]
         ,'(' + qq.Cutpart + ')' + d.PatternDesc [Desc]
        --,Artwork.Artwork [Artwork]
        ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
        ,a.Qty [Quantity]
        ,a.BundleNo [Barcode]
        ,SeasonID = concat(c.SeasonID,' ', c.dest)
        ,brand=c.brandid
        ,brand.ShipCode
        ,b.item
        ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'X' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
    inner join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
    left join brand WITH (NOLOCK) on brand.id = c.brandid
    left join dbo.Bundle_Detail_Allpart d WITH (NOLOCK) on d.id=a.Id
    outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',d.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
    outer apply
    (
	    select Artwork = 
	    (
		    select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
		    from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
		    where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
		    for xml path('')
	    )
    )as Artwork
    where a.ID= @ID and a.Patterncode = 'ALLPARTS'
)x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m
		inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu
order by x.[Barcode]");
                }
                else
                {
                    scmd = string.Format(@"
declare @extend varchar(1) = @extend_p
,@POID varchar(13) = @POID_p
,@Cutref varchar(8) = @CutRef_p
,@ID varchar(13) = @ID_p

select distinct *
from (
	select a.BundleGroup [Group_right]
			,c.FactoryID  [Group_left]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
            ,b.POID
	        ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
			,c.StyleID [Style]
			,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @poid),'') as [MarkerNo]
            , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno), iif(a.Tone='','','-'+a.Tone))
			,a.Parts [Parts]
			,b.Article + '\' + b.Colorid [Color]
            ,b.Article
			,a.SizeCode [Size]
			,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
			,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
			,a.Qty [Quantity]
			,a.BundleNo [Barcode]
			,a.Patterncode
            ,SeasonID = concat(c.SeasonID, ' ', c.dest)
            ,brand=c.brandid
        ,brand.ShipCode
        ,b.item
        ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'X' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	inner join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
    left join brand WITH (NOLOCK) on brand.id = c.brandid
	outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
	outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
															from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
															where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
															for xml path('')))as Artwork
	where a.ID= @ID and a.Patterncode != 'ALLPARTS'

	union all

	select a.BundleGroup [Group_right]
			,c.FactoryID  [Group_left]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
            ,b.POID
	        ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
			,c.StyleID [Style]
			,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @poid),'') as [MarkerNo]
            , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno), iif(a.Tone='','','-'+a.Tone))
			,a.Parts [Parts]
			,b.Article + '\' + b.Colorid [Color]
            ,b.Article
			,a.SizeCode [Size]
			,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
	        ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
			,a.Qty [Quantity]
			,a.BundleNo [Barcode]
			,a.Patterncode
            ,SeasonID = concat(c.SeasonID, ' ', c.dest)
            ,brand=c.brandid
        ,brand.ShipCode
            ,b.item
            ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'X' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	inner join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
    left join brand WITH (NOLOCK) on brand.id = c.brandid
	outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
	outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
															from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
															where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
															for xml path('')))as Artwork
	where a.ID= @ID and a.Patterncode = 'ALLPARTS'
)x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m
		inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu

order by x.[Barcode]");
                }

                this.result = DBProxy.Current.Select(string.Empty, scmd, pars, out this.dt);

                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }
            else
            {
                #region excel
                string id = this.CurrentDataRow["ID"].ToString();
                List<SqlParameter> lis = new List<SqlParameter>
                {
                    new SqlParameter("@ID_p", id),
                    new SqlParameter("@extend_p", this.checkExtendAllParts.Checked ? "1" : "0"),
                };

                string sqlcmd;
                if (this.checkExtendAllParts.Checked)
                {
                    sqlcmd = string.Format(@"
declare @extend varchar(1) = @extend_p
,@ID varchar(13) = @ID_p

select distinct [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty]
from (
	select b.id [Bundle_ID]
	    ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
		,b.POID [POID]
		,c.StyleID [Style]
		,b.Sewinglineid [Line]
		,b.SewingCell [Cell]
		,b.Cutno [Cut]
		,b.Item [Item]
		,b.Article+' / '+b.Colorid [Article_Color]
		,a.BundleGroup [Group]
		,a.BundleNo [Bundle]
		,a.SizeCode [Size]
		,qq.Cutpart [Cutpart]
		,a.PatternDesc [Description]
		,Artwork.Artwork [SubProcess]
		,a.Parts [Parts]
		,a.Qty [Qty]
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	left join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
	outer apply( select a.PatternCode [Cutpart] )[qq]
	outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
														from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
														where e1.Bundleno=a.BundleNo
														for xml path('')))as Artwork
	where a.ID= @ID and a.Patterncode != 'ALLPARTS'

	union all

	select b.id [Bundle_ID]
	    ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
		,b.POID [POID]
		,c.StyleID [Style]
		,b.Sewinglineid [Line]
		,b.SewingCell [Cell]
		,b.Cutno [Cut]
		,b.Item [Item]
		,b.Article+' / '+b.Colorid [Article_Color]
		,a.BundleGroup [Group]
		,a.BundleNo [Bundle]
		,a.SizeCode [Size]
		,qq.Cutpart [Cutpart]
		,d.PatternDesc [Description]
		,Artwork.Artwork [SubProcess]
		,d.Parts [Parts]
		,a.Qty [Qty]
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	left join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
	left join dbo.Bundle_Detail_Allpart d WITH (NOLOCK) on d.id=a.Id
	outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',d.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
	outer apply(select Artwork = '' )as Artwork
	where a.ID= @ID and a.Patterncode = 'ALLPARTS'
)x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m
		inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu

order by x.[Bundle]");
                }
                else
                {
                    sqlcmd = string.Format(@"
declare @extend varchar(1) = @extend_p
,@ID varchar(13) = @ID_p

select distinct [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty]
from (
	select b.id [Bundle_ID]
	        ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
			,b.POID [POID]
			,c.StyleID [Style]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
			,b.Cutno [Cut]
			,b.Item [Item]
			,b.Article+' / '+b.Colorid [Article_Color]
			,a.BundleGroup [Group]
			,a.BundleNo [Bundle]
			,a.SizeCode [Size]
			,qq.Cutpart [Cutpart]
			,a.PatternDesc [Description]
			,Artwork.Artwork [SubProcess]
			,a.Parts [Parts]
			,a.Qty [Qty]
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	left join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
	outer apply ( select a.PatternCode [Cutpart] ) [qq]
	outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
															from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
															where e1.Bundleno=a.BundleNo
															for xml path('')))as Artwork
	where a.ID= @ID and a.Patterncode != 'ALLPARTS'

	union all

	select b.id [Bundle_ID]
	    ,SP=dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
			,b.POID [POID]
			,c.StyleID [Style]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
			,b.Cutno [Cut]
			,b.Item [Item]
			,b.Article+' / '+b.Colorid [Article_Color]
			,a.BundleGroup [Group]
			,a.BundleNo [Bundle]
			,a.SizeCode [Size]
			,qq.Cutpart [Cutpart]
			,a.PatternDesc [Description]
			,Artwork.Artwork [SubProcess]
			,a.Parts [Parts]
			,a.Qty [Qty]
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	left join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
	outer apply ( select a.PatternCode [Cutpart] ) [qq]
	outer apply ( select Artwork = '')as Artwork
	where a.ID= @ID and a.Patterncode = 'ALLPARTS'
)x
outer apply
(
	select iif(msso.SizeSpec is not null, msso.SizeSpec, mss.SizeSpec) as SizeSpec
	from MNOrder m
		inner join Production.dbo.MNOrder_SizeItem msi on msi.ID = m.OrderComboID
		left join Production.dbo.MNOrder_SizeCode msc on msi.Id = msc.Id
		left join Production.dbo.MNOrder_SizeSpec mss on msi.Id = mss.Id and msi.SizeItem = mss.SizeItem and mss.SizeCode = msc.SizeCode
		left join Production.dbo.MNOrder_SizeSpec_OrderCombo msso on msi.Id = msso.Id and msso.OrderComboID = m.id and msi.SizeItem = msso.SizeItem and msso.SizeCode = msc.SizeCode
	where(mss.SizeCode is not null or msso.SizeCode  is not null) AND msi.SizeItem = 'S01' and m.ID = x.[SP]
	and iif(mss.SizeCode is not null, mss.SizeCode, msso.SizeCode) = x.[Size]
)cu
order by x.[Bundle]");
                }

                this.result = DBProxy.Current.Select(string.Empty, sqlcmd, lis, out this.dtt);
                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }

            return this.result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ExcelProcess();
            return true;
        }

        /// <inheritdoc/>
        protected override bool OnToPrint(ReportDefinition report)
        {
            if (this.radioBundleCard.Checked)
            {
                if (this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                this.SetCount(this.dt.Rows.Count);

                this.ShowWaitMessage("Process Excel!");
                List<P10_PrintData> data = this.dt.AsEnumerable().Select(row1 => new P10_PrintData()
                {
                    Group_right = row1["Group_right"].ToString(),
                    Group_left = row1["Group_left"].ToString(),
                    Line = row1["Line"].ToString(),
                    Cell = row1["Cell"].ToString(),
                    POID = row1["POID"].ToString(),
                    SP = row1["SP"].ToString(),
                    Style = row1["Style"].ToString(),
                    MarkerNo = row1["MarkerNo"].ToString(),
                    Body_Cut = row1["Body_Cut"].ToString(),
                    Parts = row1["Parts"].ToString(),
                    Color = row1["Color"].ToString(),
                    Article = row1["Article"].ToString(),
                    Size = row1["Size"].ToString(),
                    SizeSpec = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? string.Empty : "(" + row1["SizeSpec"].ToString() + ")",
                    Desc = row1["Desc"].ToString(),
                    Artwork = row1["Artwork"].ToString(),
                    Quantity = row1["Quantity"].ToString(),
                    Barcode = row1["Barcode"].ToString(),
                    Season = row1["Seasonid"].ToString(),
                    Brand = row1["brand"].ToString(),
                    Item = row1["item"].ToString(),
                    EXCESS1 = MyUtility.Convert.GetBool(row1["isEXCESS"]) ? "EXCESS" : string.Empty,
                    NoBundleCardAfterSubprocess1 = row1["NoBundleCardAfterSubprocess"].ToString().Empty() ? string.Empty : "X",
                    Replacement1 = string.Empty,
                    ShipCode = MyUtility.Convert.GetString(row1["ShipCode"]),
                    FabricPanelCode = MyUtility.Convert.GetString(row1["FabricPanelCode"]),
                }).ToList();
                string fileName = "Cutting_P10_Layout1";
                Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
                Excel.Workbook workbook = excelApp.ActiveWorkbook;
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                int layout = this.comboLayout.SelectedValue.ToString() == "0" ? 1 : 2;
                RunPagePrint(data, excelApp, layout);
                this.HideWaitMessage();
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    string printer = pd.PrinterSettings.PrinterName;
                    workbook.PrintOutEx(ActivePrinter: printer);
                }

                string excelName = Class.MicrosoftFile.GetName(fileName);
                excelApp.ActiveWorkbook.SaveAs(excelName);
                workbook.Close();
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
                File.Delete(excelName);
            }
            else
            {
                this.ExcelProcess(true);
            }

            return true;
        }

        private void ExcelProcess(bool print = false)
        {
            if (this.dtt == null || this.dtt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dtt.Rows.Count);
            string fileName = "Cutting_P10";
            this.ShowWaitMessage("Process Excel!");
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx"); // 預先開啟excel app
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            MyUtility.Excel.CopyToXls(this.dtt, string.Empty, $"{fileName}.xltx", 6, false, null, excelApp);

            worksheet.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", this.CurrentDataRow["ID"].ToString().Substring(0, 3)));
            worksheet.Cells[3, 1] = "To Line: " + this.CurrentDataRow["sewinglineid"].ToString();
            worksheet.Cells[3, 3] = "Cell: " + this.CurrentDataRow["SewingCell"].ToString();
            worksheet.Cells[3, 4] = "Comb: " + this.CurrentDataRow["PatternPanel"].ToString();
            string sqlcmd = $@"select top 1 MarkerNo from WorkOrder where  CutRef='{this.CurrentDataRow["cutref"]}' and id = '{this.CurrentDataRow["poid"]}'";
            worksheet.Cells[3, 5] = "Marker No: " + (MyUtility.Check.Empty(this.CurrentDataRow["cutref"]) ? string.Empty : MyUtility.GetValue.Lookup(sqlcmd));
            worksheet.Cells[3, 7] = "Item: " + this.CurrentDataRow["item"].ToString();
            worksheet.Cells[3, 9] = "Article/Color: " + this.CurrentDataRow["article"].ToString() + "/ " + this.CurrentDataRow["colorid"].ToString();
            worksheet.Cells[3, 11] = "ID: " + this.CurrentDataRow["ID"].ToString();
            worksheet.Cells[4, 1] = "Style#: " + MyUtility.GetValue.Lookup($"Select Styleid from Orders WITH (NOLOCK) Where id='{this.CurrentDataRow["Orderid"]}'");
            worksheet.Cells[4, 5] = "Cutting#: " + this.CurrentDataRow["cutno"].ToString();
            worksheet.Cells[4, 9] = "MasterSP#: " + this.CurrentDataRow["POID"].ToString();
            worksheet.Cells[4, 11] = "DATE: " + DateTime.Today.ToShortDateString();
            sqlcmd = $@"select dbo.GetSinglelineSP((select distinct OrderID from Bundle_Detail_Order where id = '{this.CurrentDataRow["ID"]}' order by OrderID for XML RAW))";
            worksheet.Cells[5, 1] = "SP#: " + MyUtility.GetValue.Lookup(sqlcmd);

            worksheet.get_Range("D1:D1").ColumnWidth = 11;
            worksheet.get_Range("E1:E1").Columns.AutoFit();
            worksheet.get_Range("G1:H1").ColumnWidth = 9;
            worksheet.get_Range("I1:L1").ColumnWidth = 15;
            worksheet.Range[string.Format("A6:L{0}", this.dtt.Rows.Count + 6)].Borders.Weight = 2; // 設定全框線

            this.HideWaitMessage();
            string excelName = Class.MicrosoftFile.GetName(fileName);
            excelApp.ActiveWorkbook.SaveAs(excelName);

            if (print)
            {
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    string printer = pd.PrinterSettings.PrinterName;
                    workbook.PrintOutEx(ActivePrinter: printer);
                }

                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
                File.Delete(excelName);
            }
            else
            {
                excelApp.Visible = true;
                Marshal.ReleaseComObject(excelApp);
            }
        }

        /// <inheritdoc/>
        internal static void RunPagePrint(List<P10_PrintData> data, Excel.Application excelApp, int layout)
        {
            // 範本預設 A4 紙, 分割 9 格貼紙格式, 因印表機邊界, 9 格格式有點不同
            int page = ((data.Count - 1) / 9) + 1;
            for (int pi = 1; pi < page; pi++)
            {
                Excel.Worksheet worksheet2 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[1];
                Excel.Worksheet worksheetn = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[pi + 1];
                worksheet2.Copy(worksheetn);
                Marshal.ReleaseComObject(worksheet2);
                Marshal.ReleaseComObject(worksheetn);
            }

            // 先批次取得 BundleNo, No 資料, by POID, FabricPanelCode, Article, Size
            DataTable allNoDatas = null;
            data.Select(s => new { s.POID, s.FabricPanelCode, s.Article, s.Size }).Distinct().ToList().ForEach(r =>
            {
                if (allNoDatas == null)
                {
                    allNoDatas = GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size);
                }
                else
                {
                    allNoDatas.Merge(GetNoDatas(r.POID, r.FabricPanelCode, r.Article, r.Size));
                }
            });

            for (int pi = 1; pi <= page; pi++)
            {
                var writedata = data.Skip((pi - 1) * 9).Take(9).ToList();
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[pi];
                ProcessPrint(writedata, worksheet, layout, allNoDatas);
            }
        }

        /// <inheritdoc/>
        internal static void ProcessPrint(List<P10_PrintData> data, Excel.Worksheet worksheet, int layout, DataTable allNoDatas)
        {
            int i = 0;
            int col_ref = 0;
            int row_ref = 0;

            data.ForEach(r =>
            {
                string no = GetNo(r.Barcode, allNoDatas);
                string contian;
                if (layout == 1)
                {
                    contian = $@"Tone/Grp: {r.Group_right}  Line#: {r.Line}   {r.Group_left}  Cut/L:
SP#:{r.SP}
Style#: {r.Style}
Sea: {r.Season}     Brand: {r.ShipCode}
Marker#: {r.MarkerNo}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}
Desc: {r.Desc}
Sub Process: {r.Artwork}
Qty: {r.Quantity}     No: {no}";
                }
                else
                {
                    contian = $@"Tone/Grp: {r.Group_right}  Line#: {r.Line}   {r.Group_left}  Cut/L:
SP#:{r.SP}
Style#: {r.Style}
Sea: {r.Season}     Brand: {r.ShipCode}
Marker#: {r.MarkerNo}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}
Desc: {r.Desc}
Sub Process: {r.Artwork}
Qty: {r.Quantity}     Item: {r.Item}";
                }

                row_ref = i / 3;
                row_ref = (row_ref * 5) - (row_ref / 3);
                col_ref = (i % 3) * 4;
                int cutindex = contian.IndexOf("Cut/L");
                worksheet.Cells[1 + row_ref, 1 + col_ref] = contian;
                worksheet.Cells[1 + row_ref, 1 + col_ref].Characters(1, 8).Font.Bold = true; // 部分粗體
                worksheet.Cells[1 + row_ref, 1 + col_ref].Characters(cutindex, 6).Font.Bold = true; // 部分粗體
                worksheet.Cells[2 + row_ref, 1 + col_ref] = r.EXCESS1;
                worksheet.Cells[2 + row_ref, 2 + col_ref] = r.NoBundleCardAfterSubprocess1;
                worksheet.Cells[3 + row_ref, 1 + col_ref] = "*" + r.Barcode + "*";

                // 邊框 」貼紙裁線
                if (i % 3 != 2 && (i / 3) % 3 != 2)
                {
                    string colN = MyUtility.Excel.ConvertNumericToExcelColumn(3 + col_ref);
                    Excel.Range excelRange = worksheet.get_Range($"{colN}{4 + row_ref}");
                    excelRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                    excelRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = 1;
                }

                i++;
            });
        }

        /// <inheritdoc/>
        public static string GetNo(string bundleNo, DataTable dt = null, string poid = null, string fabricPanelCode = null, string article = null, string size = null)
        {
            if (dt == null)
            {
                dt = GetNoDatas(poid, fabricPanelCode, article, size);
            }

            DataRow[] drs = dt.Select($"BundleNo = '{bundleNo}'");
            if (drs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                return drs[0]["No"].ToString();
            }
        }

        /// <inheritdoc/>
        public static DataTable GetNoDatas(string poid, string fabricPanelCode, string article, string size)
        {
            string sqlcmd = $@"
SELECT bd.id, bd.BundleGroup, bd.BundleNo,bd.Patterncode, bd.Qty, IsPair
into #beforetmp
FROM BUNDLE_DETAIL bd with(nolock)
INNER JOIN BUNDLE B with(nolock) ON B.ID = bd.ID
WHERE  B.POID ='{poid}' And B.FabricPanelCode='{fabricPanelCode}' And B.Article = '{article}' AND bd.SizeCode='{size}'
ORDER BY BundleGroup,bd.BundleNo

--maxQty 為每組綁包的總數,在相同 ID, BundleGroup 加總數
--分子 Bundle_Detail_qty 在P15寫入,每組綁包都會寫入一筆, 但是沒有直接關係分別是哪一組綁包的
--直接除有幾個 BundleGroup, 是因P15寫入規則, 每組綁包資訊必須一樣才會合併在同一張單
select *,
	maxQty=(Select sum(Qty) from Bundle_Detail_qty bdq WITH (NOLOCK) Where bdq.id = bt.id)/(select count(distinct BundleGroup) from #beforetmp where id = bt.id)
into #tmp
from #beforetmp bt

--同Patterncode下有數量不同
--IsPair 兩個為一組
select t.*,	
	IsPairRn = IIF(IsPair = 0, 0, row_number() over(partition by ID,BundleGroup,Patterncode Order by BundleNo) % 2 + 1)	
into #tmpx0
from #tmp t

select t.*,
	tmpLastNo = IIF(Qty < maxQty, sum(qty) over(partition by ID,BundleGroup,Patterncode,IsPairRn Order by BundleNo), Qty)
into #tmpx1
from #tmpx0 t
order by bundleno

select distinct Id,BundleGroup,maxQty into #tmp2 from #tmp
select *, lastNo = SUM(maxQty) over(Order by Id,BundleGroup) into #tmp3 from #tmp2
select *, before = LAG(lastNo,1,0) over(Order by Id,BundleGroup) into #tmp4 from #tmp3

select
	x1.*,
	minPatterncodeNo = min(tmpLastNo)  over(partition by x1.ID,x1.BundleGroup,x1.Patterncode,x1.IsPairRn Order by x1.BundleNo),
	tmpbefore = t4.before + 1,
	lastno = t4.before + x1.tmpLastNo
into #tmp5
from #tmp4 t4
inner join #tmpx1 x1 on x1.Id = t4.Id and x1.BundleGroup = t4.BundleGroup

select t5.*,
	startNo = case when Qty = maxQty or tmpLastNo = minPatterncodeNo then tmpbefore
					else LAG(lastNo,1,0) over(partition by ID,BundleGroup,Patterncode,IsPairRn Order by BundleNo) + 1
					end
into #tmp6
from #tmp5 t5

select BundleNo,No = CONCAT(startNo,'~',lastno)
from #tmp6

drop table #tmpx1,#tmp,#tmp2,#tmp3,#tmp4,#tmp5,#tmp6
";
            DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            return dt;
        }

        private void RadioBundleCard_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBundleCard.Checked)
            {
                this.comboLayout.Enabled = true;
                this.print.Enabled = true;
                this.toexcel.Enabled = false;
            }
            else if (this.radioBundleChecklist.Checked)
            {
                this.comboLayout.Enabled = false;
                this.toexcel.Enabled = true;
                this.print.Enabled = true;
            }
        }

        private void P10_Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
