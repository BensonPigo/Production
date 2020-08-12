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
        private string pathName;

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
                    new SqlParameter("@ID", id),
                    new SqlParameter("@CutRef", this.CurrentDataRow["cutref"].ToString()),
                    new SqlParameter("@POID", this.CurrentDataRow["POID"].ToString()),
                    new SqlParameter("@extend", this.checkExtendAllParts.Checked ? "1" : "0"),
                };

                string scmd;
                if (this.checkExtendAllParts.Checked)
                {
                    scmd = string.Format(@"
select distinct *
from (
    select a.BundleGroup [Group_right]
	    ,c.FactoryID  [Group_left]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,b.Orderid [SP]
        ,c.StyleID [Style]
        ,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @POID),'') as [MarkerNo]
        , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
	    ,a.Parts [Parts]
        ,b.Article + '\' + b.Colorid [Color]
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
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    inner join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
    inner join brand WITH (NOLOCK) on brand.id = c.brandid
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
        ,b.Orderid [SP]
        ,c.StyleID [Style]
        ,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @POID),'') as [MarkerNo]
        , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
	    ,d.Parts [Parts]
        ,b.Article + '\' + b.Colorid [Color]
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
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    inner join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
    inner join brand WITH (NOLOCK) on brand.id = c.brandid
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
select distinct *
from (
	select a.BundleGroup [Group_right]
			,c.FactoryID  [Group_left]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
			,b.Orderid [SP]
			,c.StyleID [Style]
			,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @poid),'') as [MarkerNo]
            , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
			,a.Parts [Parts]
			,b.Article + '\' + b.Colorid [Color]
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
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
	inner join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
    inner join brand WITH (NOLOCK) on brand.id = c.brandid
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
			,b.Orderid [SP]
			,c.StyleID [Style]
			,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @poid),'') as [MarkerNo]
            , [Body_Cut]=concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
			,a.Parts [Parts]
			,b.Article + '\' + b.Colorid [Color]
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
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
        ,b.FabricPanelCode
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
	inner join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
    inner join brand WITH (NOLOCK) on brand.id = c.brandid
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
                    new SqlParameter("@ID", id),
                    new SqlParameter("@extend", this.checkExtendAllParts.Checked ? "1" : "0"),
                };

                string sqlcmd;
                if (this.checkExtendAllParts.Checked)
                {
                    sqlcmd = string.Format(@"
select distinct [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty]
from (
	select b.id [Bundle_ID]
		,b.Orderid [SP]
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
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
	outer apply( select a.PatternCode [Cutpart] )[qq]
	outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
														from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
														where e1.Bundleno=a.BundleNo
														for xml path('')))as Artwork
	where a.ID= @ID and a.Patterncode != 'ALLPARTS'

	union all

	select b.id [Bundle_ID]
		,b.Orderid [SP]
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
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
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
select distinct [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty]
from (
	select b.id [Bundle_ID]
			,b.Orderid [SP]
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
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
	outer apply ( select a.PatternCode [Cutpart] ) [qq]
	outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
															from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
															where e1.Bundleno=a.BundleNo
															for xml path('')))as Artwork
	where a.ID= @ID and a.Patterncode != 'ALLPARTS'

	union all

	select b.id [Bundle_ID]
			,b.Orderid [SP]
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
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
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
            if (this.dtt == null || this.dtt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dtt.Rows.Count);
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); // 預先開啟excel app
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", this.CurrentDataRow["ID"].ToString().Substring(0, 3)));
            objSheets.Cells[3, 1] = "To Line: " + this.CurrentDataRow["sewinglineid"].ToString();
            objSheets.Cells[3, 3] = "Cell: " + this.CurrentDataRow["SewingCell"].ToString();
            objSheets.Cells[3, 4] = "Comb: " + this.CurrentDataRow["PatternPanel"].ToString();
            objSheets.Cells[3, 5] = "Marker No: " + (this.CurrentDataRow["cutref"].ToString() == string.Empty ? string.Empty
                : MyUtility.GetValue.Lookup(string.Format(@"select top 1 MarkerNo from WorkOrder where  CutRef='{0}' and id = '{1}'", this.CurrentDataRow["cutref"].ToString(), this.CurrentDataRow["poid"].ToString())));
            objSheets.Cells[3, 7] = "Item: " + this.CurrentDataRow["item"].ToString();
            objSheets.Cells[3, 9] = "Article/Color: " + this.CurrentDataRow["article"].ToString() + "/ " + this.CurrentDataRow["colorid"].ToString();
            objSheets.Cells[3, 11] = "ID: " + this.CurrentDataRow["ID"].ToString();
            objSheets.Cells[4, 1] = "SP#: " + this.CurrentDataRow["Orderid"].ToString();
            objSheets.Cells[4, 4] = "Style#: " + MyUtility.GetValue.Lookup(string.Format("Select Styleid from Orders WITH (NOLOCK) Where id='{0}'", this.CurrentDataRow["Orderid"].ToString()));
            objSheets.Cells[4, 7] = "Cutting#: " + this.CurrentDataRow["cutno"].ToString();
            objSheets.Cells[4, 9] = "MasterSP#: " + this.CurrentDataRow["POID"].ToString();
            objSheets.Cells[4, 11] = "DATE: " + DateTime.Today.ToShortDateString();
            MyUtility.Excel.CopyToXls(this.dtt, string.Empty, "Cutting_P10.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            objSheets.get_Range("D1:D1").ColumnWidth = 11;
            objSheets.get_Range("E1:E1").Columns.AutoFit();
            objSheets.get_Range("G1:H1").ColumnWidth = 9;
            objSheets.get_Range("I1:L1").ColumnWidth = 15;

            objSheets.Range[string.Format("A6:L{0}", this.dtt.Rows.Count + 5)].Borders.Weight = 2; // 設定全框線

            #region Save & Shwo Excel
            string strExcelName = Class.MicrosoftFile.GetName("Cutting_P10");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        /// <inheritdoc/>
        protected override bool OnToPrint(ReportDefinition report)
        {
            if (this.radioBundleCard.Checked)
            {
                if (this.dt == null || this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                this.SetCount(this.dt.Rows.Count);
                List<P10_PrintData> data = this.dt.AsEnumerable().Select(row1 => new P10_PrintData()
                {
                    Group_right = row1["Group_right"].ToString(),
                    Group_left = row1["Group_left"].ToString(),
                    Line = row1["Line"].ToString(),
                    Cell = row1["Cell"].ToString(),
                    SP = row1["SP"].ToString(),
                    Style = row1["Style"].ToString(),
                    MarkerNo = row1["MarkerNo"].ToString(),
                    Body_Cut = row1["Body_Cut"].ToString(),
                    Parts = row1["Parts"].ToString(),
                    Color = row1["Color"].ToString(),
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

                // 範本預設 A4 紙, 分割 9 格貼紙格式, 因印表機邊界, 9 格格式有點不同
                if (data.Count > 9)
                {
                    int p = (data.Count - 1) / 9;
                    for (int pi = 1; pi <= p; pi++)
                    {
                        Excel.Range r1 = worksheet.get_Range("A1", "A14").EntireRow;
                        Excel.Range r2 = worksheet.get_Range($"A{1 + (pi * 14)}");
                        r2.Insert(Excel.XlInsertShiftDirection.xlShiftDown, r1.Copy());
                    }
                }

                int i = 0;
                int col_ref = 0;
                int row_ref = 0;
                data.ForEach(r =>
                {
                    DataTable bdoDt = GetBundle_Detail_Order_Data(r.Barcode);
                    string sps = GetSpstring(bdoDt);
                    string no = GetNo(r.SP, r.FabricPanelCode, r.Size, r.Barcode);
                    string contian;
                    if (this.comboLayout.SelectedValue.ToString() == "0")
                    {
                        contian = $@"Tone/Grp: {r.Group_right}  Line#: {r.Line}   {r.Group_left}  Cut/L:
SP#:{sps}
Style#: {r.Style}
Sea: {r.Season}     Brand: {r.ShipCode}
Marker#: {r.MarkerNo}
Cut#: {r.Body_Cut}     No: {no}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}
Dese: {r.Desc}
Sub Process: {r.Artwork}
Qty: {r.Quantity}";
                    }
                    else
                    {
                        contian = $@"Tone/Grp: {r.Group_right}  Line#: {r.Line}   {r.Group_left}  Cut/L:
SP#:{sps}
Style#: {r.Style}
Sea: {r.Season}     Brand: {r.ShipCode}
Marker#: {r.MarkerNo}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}
Dese: {r.Desc}
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

                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    string printer = pd.PrinterSettings.PrinterName;
                    workbook.PrintOutEx(ActivePrinter: printer);
                }

                Marshal.ReleaseComObject(excelApp);

                #region Bundle Card RDLC 先不要刪 / 不要刪 / 不要刪

                // if (this.dt == null || this.dt.Rows.Count == 0)
                // {
                //    MyUtility.Msg.ErrorBox("Data not found");
                //    return false;
                // }

                //// 顯示筆數於PrintForm上Count欄位
                // this.SetCount(this.dt.Rows.Count);

                // DataTable dt1, dt2, dt3;

                //// int count =dt.Rows.Count;
                // int count = 1;
                // dt1 = this.dt.Clone();
                // dt2 = this.dt.Clone();
                // dt3 = this.dt.Clone();
                // foreach (DataRow dr in this.dt.Rows)
                // {
                //    // 第一列資料
                //    if (count % 3 == 1)
                //    {
                //        dt1.ImportRow(dr);
                //    }

                // // 第二列資料
                //    if (count % 3 == 2)
                //    {
                //        dt2.ImportRow(dr);
                //    }

                // // 第三列資料
                //    if (count % 3 == 0)
                //    {
                //        dt3.ImportRow(dr);
                //    }

                // count++;
                // }

                //// 傳 list 資料
                // List<P10_PrintData> data = dt1.AsEnumerable()
                //    .Select(row1 => new P10_PrintData()
                //    {
                //        Group_right = row1["Group_right"].ToString(),
                //        Group_left = row1["Group_left"].ToString(),
                //        Line = row1["Line"].ToString(),
                //        Cell = row1["Cell"].ToString(),
                //        SP = row1["SP"].ToString(),
                //        Style = row1["Style"].ToString(),
                //        MarkerNo = row1["MarkerNo"].ToString(),
                //        Body_Cut = row1["Body_Cut"].ToString(),
                //        Parts = row1["Parts"].ToString(),
                //        Color = row1["Color"].ToString(),
                //        Size = row1["Size"].ToString(),
                //        SizeSpec = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? string.Empty : "(" + row1["SizeSpec"].ToString() + ")",
                //        Desc = row1["Desc"].ToString(),
                //        Artwork = row1["Artwork"].ToString(),
                //        Quantity = row1["Quantity"].ToString(),
                //        Barcode = row1["Barcode"].ToString(),
                //        Season = row1["Seasonid"].ToString(),
                //        brand = row1["brand"].ToString(),
                //        item = row1["item"].ToString(),
                //        EXCESS1 = MyUtility.Convert.GetBool(row1["isEXCESS"]) ? "EXCESS" : string.Empty,
                //        NoBundleCardAfterSubprocess1 = row1["NoBundleCardAfterSubprocess"].ToString(),
                //        Replacement1 = string.Empty,
                //    }).ToList();
                // data.AddRange(
                // dt2.AsEnumerable().Select(row1 => new P10_PrintData()
                // {
                //     Group_right2 = row1["Group_right"].ToString(),
                //     Group_left2 = row1["Group_left"].ToString(),
                //     Line2 = row1["Line"].ToString(),
                //     Cell2 = row1["Cell"].ToString(),
                //     SP2 = row1["SP"].ToString(),
                //     Style2 = row1["Style"].ToString(),
                //     MarkerNo2 = row1["MarkerNo"].ToString(),
                //     Body_Cut2 = row1["Body_Cut"].ToString(),
                //     Parts2 = row1["Parts"].ToString(),
                //     Color2 = row1["Color"].ToString(),
                //     Size2 = row1["Size"].ToString(),
                //     SizeSpec2 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? string.Empty : "(" + row1["SizeSpec"].ToString() + ")",
                //     Desc2 = row1["Desc"].ToString(),
                //     Artwork2 = row1["Artwork"].ToString(),
                //     Quantity2 = row1["Quantity"].ToString(),
                //     Barcode2 = row1["Barcode"].ToString(),
                //     Season2 = row1["Seasonid"].ToString(),
                //     brand2 = row1["brand"].ToString(),
                //     item2 = row1["item"].ToString(),
                //     EXCESS2 = MyUtility.Convert.GetBool(row1["isEXCESS"]) ? "EXCESS" : string.Empty,
                //     NoBundleCardAfterSubprocess2 = row1["NoBundleCardAfterSubprocess"].ToString(),
                //     Replacement2 = string.Empty,
                // }).ToList());

                // data.AddRange(
                // dt3.AsEnumerable().Select(row1 => new P10_PrintData()
                // {
                //    Group_right3 = row1["Group_right"].ToString(),
                //    Group_left3 = row1["Group_left"].ToString(),
                //    Line3 = row1["Line"].ToString(),
                //    Cell3 = row1["Cell"].ToString(),
                //    SP3 = row1["SP"].ToString(),
                //    Style3 = row1["Style"].ToString(),
                //    MarkerNo3 = row1["MarkerNo"].ToString(),
                //    Body_Cut3 = row1["Body_Cut"].ToString(),
                //    Parts3 = row1["Parts"].ToString(),
                //    Color3 = row1["Color"].ToString(),
                //    Size3 = row1["Size"].ToString(),
                //    SizeSpec3 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? string.Empty : "(" + row1["SizeSpec"].ToString() + ")",
                //    Desc3 = row1["Desc"].ToString(),
                //    Artwork3 = row1["Artwork"].ToString(),
                //    Quantity3 = row1["Quantity"].ToString(),
                //    Barcode3 = row1["Barcode"].ToString(),
                //    Season3 = row1["Seasonid"].ToString(),
                //    brand3 = row1["brand"].ToString(),
                //    item3 = row1["item"].ToString(),
                //    EXCESS3 = MyUtility.Convert.GetBool(row1["isEXCESS"]) ? "EXCESS" : string.Empty,
                //    NoBundleCardAfterSubprocess3 = row1["NoBundleCardAfterSubprocess"].ToString(),
                //    Replacement3 = string.Empty,
                // }).ToList());

                // report.ReportDataSource = data;

                // Type reportResourceNamespace = typeof(P10_PrintData);
                // Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                // string reportResourceName = "P10_Print.rdlc";

                // if (!(this.result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                // {
                //    this.ShowException(this.result);
                //    return this.result;
                // }

                // report.ReportResource = reportresource;

                //// 開啟 report view
                // var frm = new Win.Subs.ReportView(report)
                // {
                //    MdiParent = this.MdiParent,
                //    DirectPrint = true,
                // };
                // frm.ShowDialog();
                #endregion
            }
            else
            {
                #region Bundle Check List
                if (this.dtt == null || this.dtt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.dtt.Rows.Count);

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); // 預先開啟excel app
                this.pathName = Env.Cfg.ReportTempDir + "Cutting_BundleChecklist" + DateTime.Now.ToFileTime() + ".xls";
                string tmpName = Env.Cfg.ReportTempDir + "tmp.xls";
                if (MyUtility.Excel.CopyToXls(this.dtt, string.Empty, "Cutting_P10.xltx", 5, false, null, objApp, false))
                {
                    Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Excel._Workbook objBook = objApp.ActiveWorkbook;
                    objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", this.CurrentDataRow["ID"].ToString().Substring(0, 3)));
                    objSheets.Cells[3, 1] = "To Line: " + this.CurrentDataRow["sewinglineid"].ToString();
                    objSheets.Cells[3, 3] = "Cell: " + this.CurrentDataRow["SewingCell"].ToString();
                    objSheets.Cells[3, 4] = "Comb: " + this.CurrentDataRow["PatternPanel"].ToString();
                    objSheets.Cells[3, 5] = "Marker No: " + (this.CurrentDataRow["cutref"].ToString() == string.Empty ? string.Empty
                        : MyUtility.GetValue.Lookup(string.Format(@"select top 1 MarkerNo from WorkOrder where  CutRef='{0}' and id = '{1}'", this.CurrentDataRow["cutref"].ToString(), this.CurrentDataRow["poid"].ToString())));
                    objSheets.Cells[3, 7] = "Item: " + this.CurrentDataRow["item"].ToString();
                    objSheets.Cells[3, 9] = "Article/Color: " + this.CurrentDataRow["article"].ToString() + "/ " + this.CurrentDataRow["colorid"].ToString();
                    objSheets.Cells[3, 11] = "ID: " + this.CurrentDataRow["ID"].ToString();
                    objSheets.Cells[4, 1] = "SP#: " + this.CurrentDataRow["Orderid"].ToString();
                    objSheets.Cells[4, 4] = "Style#: " + MyUtility.GetValue.Lookup(string.Format("Select Styleid from Orders WITH (NOLOCK) Where id='{0}'", this.CurrentDataRow["Orderid"].ToString()));
                    objSheets.Cells[4, 7] = "Cutting#: " + this.CurrentDataRow["cutno"].ToString();
                    objSheets.Cells[4, 9] = "MasterSP#: " + this.CurrentDataRow["POID"].ToString();
                    objSheets.Cells[4, 11] = "DATE: " + DateTime.Today.ToShortDateString();
                    objSheets.get_Range("D1:D1").ColumnWidth = 11;
                    objSheets.get_Range("E1:E1").Columns.AutoFit();
                    objSheets.get_Range("G1:H1").ColumnWidth = 9;
                    objSheets.get_Range("I1:L1").ColumnWidth = 15;

                    objSheets.Range[string.Format("A6:L{0}", this.dtt.Rows.Count + 5)].Borders.Weight = 2; // 設定全框線

                    // Random Excle名稱
                    Random random = new Random();
                    this.pathName = Env.Cfg.ReportTempDir + "Cutting_BundleChecklist - " + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss") + " - " + Convert.ToString(Convert.ToInt32(random.NextDouble() * 10000)) + ".xlsx";
                    this.pathName = Path.GetFullPath(this.pathName);
                    objBook.SaveAs(this.pathName);
                    PrintDialog pd = new PrintDialog();
                    if (pd.ShowDialog() == DialogResult.OK)
                    {
                        string printer = pd.PrinterSettings.PrinterName;
                        objBook.PrintOutEx(ActivePrinter: printer);
                    }

                    objBook.Close();
                    objApp.Workbooks.Close();
                    objApp.Quit();

                    if (objSheets != null)
                    {
                        Marshal.FinalReleaseComObject(objSheets);    // 釋放sheet
                    }

                    if (objApp != null)
                    {
                        Marshal.FinalReleaseComObject(objApp);          // 釋放objApp
                    }

                    if (objBook != null)
                    {
                        Marshal.FinalReleaseComObject(objBook);
                    }
                }

                File.Delete(this.pathName);

                // 刪除存檔
                #endregion
            }

            return true;
        }

        /// <inheritdoc/>
        public static string GetSpstring(DataTable dt)
        {
            string sps = string.Empty;
            if (dt.Rows.Count > 0)
            {
                sps = dt.Rows[0]["OrderID"].ToString();
                sps += GetSubSPstring(dt, true, 32);
            }

            return sps;
        }

        /// <inheritdoc/>
        public static string GetSubSPstring(DataTable dt, bool excludefirst, int subCount)
        {
            string sps = string.Empty;

            int i = 0;
            bool brk = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (excludefirst)
                {
                    excludefirst = false;
                    continue;
                }

                string sp = dr["OrderID"].ToString();
                sps += "/";
                if (MyUtility.Convert.GetString(dr["Category"]).EqualString("S"))
                {
                    sps += "S";
                }

                sps += sp.Length > 10 ? sp.Substring(10) : sp;
                if (i++ > subCount)
                {
                    brk = true;
                    break;
                }
            }

            if (brk)
            {
                string sp = dt.Rows[dt.Rows.Count - 1]["OrderID"].ToString();
                sps += "/.../" + (sp.Length > 10 ? sp.Substring(10) : sp);
            }

            return sps;
        }

        /// <inheritdoc/>
        public static DataTable GetBundle_Detail_Order_Data(string bundleno)
        {
            string sqlcmd = $@"select bdo.OrderID,o.Categoryfrom Bundle_Detail_Order bdo with(nolock)left join Orders o with(nolock) on o.ID = bdo.OrderIDwhere bundleno = '{bundleno}'order by OrderID";
            DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dt);
            return dt;
        }

        /// <inheritdoc/>
        public static string GetNo(string orderid, string fabricPanelCode, string size, string bundleNo)
        {
            string sqlcmd = $@"SELECT D.BundleNo, D.BundleGroup,D.Qty
into #tmp
FROM BUNDLE_DETAIL D
INNER JOIN BUNDLE B ON B.ID = D.ID  
WHERE B.ORDERID='{orderid}' AND B.FabricPanelCode='{fabricPanelCode}' AND D.SIZECODE='{size}'
ORDER BY BundleGroup

select distinct t.BundleGroup,t.Qty into #tmp2 from #tmp t ORDER BY BundleGroup
select t.BundleGroup, lastNo = SUM(t.Qty) over(order by t.BundleGroup) + 1,startNo = LAG(Qty,1,0)over(order by BundleGroup) + 1 into #tmp3 from #tmp2 t
select concat(startNo,'~',lastNo) from #tmp3 t3 inner join #tmp t on t.BundleGroup = t3.BundleGroup where t.BundleNo = '{bundleNo}'

drop table #tmp,#tmp2,#tmp3
";
            return MyUtility.GetValue.Lookup(sqlcmd);
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
    }
}
