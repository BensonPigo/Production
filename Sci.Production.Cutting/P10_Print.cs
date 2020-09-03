using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using System.IO;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P10_Print : Win.Tems.PrintForm
    {
        private DualResult result;
        private DataRow CurrentDataRow;
        private string pathName;

        /// <summary>
        /// Initializes a new instance of the <see cref="P10_Print"/> class.
        /// </summary>
        /// <param name="row">DataRow</param>
        public P10_Print(DataRow row)
        {
            this.InitializeComponent();
            this.CurrentDataRow = row;
            this.toexcel.Enabled = false;

            this.comboBoxSetting.DataSource = Enum.GetValues(typeof(Prg.BundleRFCard.BundleType));
        }

        private string Bundle_Card;
        private string Bundle_Check_list;
        private string Extend_All_Parts;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Bundle_Card = this.radioBundleCard.Checked.ToString();
            this.Bundle_Check_list = this.radioBundleChecklist.Checked.ToString();
            this.Extend_All_Parts = this.checkExtendAllParts.Checked.ToString();
            return base.ValidateInput();
        }

        private DataTable dt;

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.radioBundleCard.Checked == true)
            {
                #region report
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();

                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                    new SqlParameter("@CutRef", this.CurrentDataRow["cutref"].ToString()),
                    new SqlParameter("@POID", this.CurrentDataRow["POID"].ToString()),
                };
                if (this.checkExtendAllParts.Checked)
                {
                    pars.Add(new SqlParameter("@extend", "1"));
                }
                else
                {
                    pars.Add(new SqlParameter("@extend", "0"));
                }

                string scmd = string.Empty;
                if (this.checkExtendAllParts.Checked)
                {
                    // 有勾[Extend All Parts]
                    #region SQL
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
        --,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
        ,'(' + qq.Cutpart + ')' + a.PatternDesc [Desc]
        --,Artwork.Artwork [Artwork]
        ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
        ,a.Qty [Quantity]
        ,a.BundleNo [Barcode]
        ,SeasonID = concat(c.SeasonID,' ', c.dest)
        ,brand=c.brandid
        ,b.item
        ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
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
        ,b.item
        ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
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
                    #endregion
                }
                else
                {
                    // 沒勾[Extend All Parts]
                    #region SQL
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
        ,b.item
        ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
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
            ,b.item
            ,b.IsEXCESS
        ,NoBundleCardAfterSubprocess=(select top 1 N'(X)' from Bundle_Detail_Art bda with(nolock) where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
	left join dbo.orders c WITH (NOLOCK) on c.id=b.Orderid
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
                    #endregion
                }

                this.result = DBProxy.Current.Select(string.Empty, scmd, pars, out this.dt);

                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }
            else if (this.radioBundleChecklist.Checked)
            {
                #region excel
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();
                List<SqlParameter> lis = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                if (this.checkExtendAllParts.Checked)
                {
                    lis.Add(new SqlParameter("@extend", "1"));
                }
                else
                {
                    lis.Add(new SqlParameter("@extend", "0"));
                }

                string sqlcmd = string.Empty;
                if (this.checkExtendAllParts.Checked)
                {
                    // 有勾[Extend All Parts]
                    #region SQL
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
                    #endregion
                }
                else
                {
                    // 沒勾[Extend All Parts]
                    #region SQL
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
                    #endregion
                }

                this.result = DBProxy.Current.Select(string.Empty, sqlcmd, lis, out this.dt);
                if (!this.result)
                {
                    return this.result;
                }
                #endregion
            }
            else if (this.radioBundleCardRF.Checked)
            {
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                string scmd = Prg.BundleRFCard.BundelRFSQLCmd(this.checkExtendAllParts.Checked, string.Empty);
                this.result = DBProxy.Current.Select(string.Empty, scmd, pars, out this.dt);
                if (!this.result)
                {
                    return this.result;
                }
            }

            return this.result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dt.Rows.Count);
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
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
            MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Cutting_P10.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            objSheets.get_Range("D1:D1").ColumnWidth = 11;
            objSheets.get_Range("E1:E1").Columns.AutoFit();
            objSheets.get_Range("G1:H1").ColumnWidth = 9;
            objSheets.get_Range("I1:L1").ColumnWidth = 15;

            objSheets.Range[string.Format("A6:L{0}", this.dt.Rows.Count + 5)].Borders.Weight = 2; // 設定全框線

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
                #region Bundle Card
                if (this.dt == null || this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.dt.Rows.Count);

                DataTable dt1, dt2, dt3;

                // int count =dt.Rows.Count;
                int count = 1;
                dt1 = this.dt.Clone();
                dt2 = this.dt.Clone();
                dt3 = this.dt.Clone();
                foreach (DataRow dr in this.dt.Rows)
                {
                    // 第一列資料
                    if (count % 3 == 1)
                    {
                        dt1.ImportRow(dr);
                    }

                    // 第二列資料
                    if (count % 3 == 2)
                    {
                        dt2.ImportRow(dr);
                    }

                    // 第三列資料
                    if (count % 3 == 0)
                    {
                        dt3.ImportRow(dr);
                    }

                    count++;
                }

                // 傳 list 資料
                List<P10_PrintData> data = dt1.AsEnumerable()
                    .Select(row1 => new P10_PrintData()
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
                        NoBundleCardAfterSubprocess1 = row1["NoBundleCardAfterSubprocess"].ToString(),
                        Replacement1 = string.Empty,
                    }).ToList();
                data.AddRange(
                 dt2.AsEnumerable().Select(row1 => new P10_PrintData()
                 {
                     Group_right2 = row1["Group_right"].ToString(),
                     Group_left2 = row1["Group_left"].ToString(),
                     Line2 = row1["Line"].ToString(),
                     Cell2 = row1["Cell"].ToString(),
                     SP2 = row1["SP"].ToString(),
                     Style2 = row1["Style"].ToString(),
                     MarkerNo2 = row1["MarkerNo"].ToString(),
                     Body_Cut2 = row1["Body_Cut"].ToString(),
                     Parts2 = row1["Parts"].ToString(),
                     Color2 = row1["Color"].ToString(),
                     Size2 = row1["Size"].ToString(),
                     SizeSpec2 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? string.Empty : "(" + row1["SizeSpec"].ToString() + ")",
                     Desc2 = row1["Desc"].ToString(),
                     Artwork2 = row1["Artwork"].ToString(),
                     Quantity2 = row1["Quantity"].ToString(),
                     Barcode2 = row1["Barcode"].ToString(),
                     Season2 = row1["Seasonid"].ToString(),
                     Brand2 = row1["brand"].ToString(),
                     Item2 = row1["item"].ToString(),
                     EXCESS2 = MyUtility.Convert.GetBool(row1["isEXCESS"]) ? "EXCESS" : string.Empty,
                     NoBundleCardAfterSubprocess2 = row1["NoBundleCardAfterSubprocess"].ToString(),
                     Replacement2 = string.Empty,
                 }).ToList());

                data.AddRange(
                dt3.AsEnumerable().Select(row1 => new P10_PrintData()
                {
                    Group_right3 = row1["Group_right"].ToString(),
                    Group_left3 = row1["Group_left"].ToString(),
                    Line3 = row1["Line"].ToString(),
                    Cell3 = row1["Cell"].ToString(),
                    SP3 = row1["SP"].ToString(),
                    Style3 = row1["Style"].ToString(),
                    MarkerNo3 = row1["MarkerNo"].ToString(),
                    Body_Cut3 = row1["Body_Cut"].ToString(),
                    Parts3 = row1["Parts"].ToString(),
                    Color3 = row1["Color"].ToString(),
                    Size3 = row1["Size"].ToString(),
                    SizeSpec3 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? string.Empty : "(" + row1["SizeSpec"].ToString() + ")",
                    Desc3 = row1["Desc"].ToString(),
                    Artwork3 = row1["Artwork"].ToString(),
                    Quantity3 = row1["Quantity"].ToString(),
                    Barcode3 = row1["Barcode"].ToString(),
                    Season3 = row1["Seasonid"].ToString(),
                    Brand3 = row1["brand"].ToString(),
                    Item3 = row1["item"].ToString(),
                    EXCESS3 = MyUtility.Convert.GetBool(row1["isEXCESS"]) ? "EXCESS" : string.Empty,
                    NoBundleCardAfterSubprocess3 = row1["NoBundleCardAfterSubprocess"].ToString(),
                    Replacement3 = string.Empty,
                }).ToList());

                report.ReportDataSource = data;

                Type reportResourceNamespace = typeof(P10_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P10_Print.rdlc";

                IReportResource reportresource;
                if (!(this.result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
                {
                    this.ShowException(this.result);
                    return this.result;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                    DirectPrint = true,
                };
                frm.ShowDialog();
                #endregion
            }
            else if (this.radioBundleChecklist.Checked)
            {
                #region Bundle Check List
                if (this.dt == null || this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                // 顯示筆數於PrintForm上Count欄位
                this.SetCount(this.dt.Rows.Count);

                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); // 預先開啟excel app
                this.pathName = Env.Cfg.ReportTempDir + "Cutting_BundleChecklist" + DateTime.Now.ToFileTime() + ".xls";
                string tmpName = Env.Cfg.ReportTempDir + "tmp.xls";
                if (MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Cutting_P10.xltx", 5, false, null, objApp, false))
                {
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;
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

                    objSheets.Range[string.Format("A6:L{0}", this.dt.Rows.Count + 5)].Borders.Weight = 2; // 設定全框線

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

                    objApp = null;
                }

                File.Delete(this.pathName);

                // 刪除存檔
                #endregion
            }
            else if (this.radioBundleCardRF.Checked)
            {
                // 是否有資料
                if (this.dt == null || this.dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                try
                {
                    DualResult result = Prg.BundleRFCard.BundleRFCardPrint(this.dt);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox(result.ToString());
                        return false;
                    }

                    MyUtility.Msg.InfoBox("Printed success, Please check result in Bin Box.");
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox(ex.ToString());
                    return false;
                }
            }

            return true;
        }

        // public bool PrintExcel(string filePath)
        // {
        //    // 1. 判斷檔案是否存在
        //    //if (!System.IO.File.Exists(filePath)) return false;
        //    PrintDocument printDoc = new PrintDocument();
        //    PrintDialog pd = new PrintDialog();
        //    printDoc.DocumentName = filePath;
        //    pd.Document = printDoc;
        //    if (pd.ShowDialog() == DialogResult.OK)
        //        printDoc.Print();
        //    //System.IO.File.Delete(filePath);
        //    return true;
        // }
        private void RadioPanel1_Paint(object sender, PaintEventArgs e)
        {
            this.toexcel.Enabled = true;
            this.print.Enabled = true;
            if (this.radioBundleCard.Checked == true)
            {
                this.toexcel.Enabled = false;
            }
            else if (this.radioBundleCardRF.Checked)
            {
                this.toexcel.Enabled = false;
            }
        }

        /// <summary>
        /// RF 測試用
        /// 現行流程 Open -> C31 -> P21 -> P42 -> P35 -> P41 -> (F30 -> WDB) -> C34 -> Close
        /// </summary>
        private void BtnSetting_Click(object sender, EventArgs e)
        {
            if (this.dt == null || this.dt.Rows.Count == 0)
            {
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                string scmd = Prg.BundleRFCard.BundelRFSQLCmd(this.checkExtendAllParts.Checked, string.Empty);
                this.result = DBProxy.Current.Select(string.Empty, scmd, pars, out this.dt);
                if (!this.result)
                {
                    MyUtility.Msg.ErrorBox(this.result.Description.ToString());
                }
            }

            this.result = Prg.BundleRFCard.BundelTest(this.dt, (Prg.BundleRFCard.BundleType)Enum.Parse(typeof(Prg.BundleRFCard.BundleType), this.comboBoxSetting.SelectedValue.ToString()));
            if (!this.result)
            {
                MyUtility.Msg.ErrorBox(this.result.Description.ToString());
            }
        }
    }
}
