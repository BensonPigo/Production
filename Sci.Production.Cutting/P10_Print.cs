﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;


namespace Sci.Production.Cutting
{
    public partial class P10_Print : Sci.Win.Tems.PrintForm
    {
        DualResult result;
        DataRow CurrentDataRow;
        string pathName;
        public P10_Print(DataRow row)
        {
            InitializeComponent();
            this.CurrentDataRow = row;
            toexcel.Enabled = false;
        }

        string Bundle_Card;
        string Bundle_Check_list;
        string Extend_All_Parts;
        protected override bool ValidateInput()
        {
            Bundle_Card = radioBundleCard.Checked.ToString();
            Bundle_Check_list = radioBundleChecklist.Checked.ToString();
            Extend_All_Parts = checkExtendAllParts.Checked.ToString();
            return base.ValidateInput();
        }
        DataTable dtt;
        DataTable dt;
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (radioBundleCard.Checked == true)
            {
                #region report
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();

                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@ID", id));
                pars.Add(new SqlParameter("@CutRef", CurrentDataRow["cutref"].ToString()));
                if (checkExtendAllParts.Checked)
                    pars.Add(new SqlParameter("@extend", "1"));
                else
                    pars.Add(new SqlParameter("@extend", "0"));

                string scmd = string.Empty;
                if (checkExtendAllParts.Checked)  //有勾[Extend All Parts]
                {
                    #region SQL
                    scmd = string.Format(@"
select *
from (
    select a.BundleGroup [Group_right]
	    ,c.FactoryID  [Group_left]
        ,b.Sewinglineid [Line]
        ,b.SewingCell [Cell]
        ,b.Orderid [SP]
        ,c.StyleID [Style]
        ,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef),'') as [MarkerNo]
        ,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
	    ,a.Parts [Parts]
        ,b.Article + '\' + b.Colorid [Color]
        ,a.SizeCode [Size]
        --,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
        ,'(' + qq.Cutpart + ')' + a.PatternDesc [Desc]
        --,Artwork.Artwork [Artwork]
    ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
        ,a.Qty [Quantity]
        ,a.BundleNo [Barcode]
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
        ,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef),'') as [MarkerNo]
        ,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
	    ,d.Parts [Parts]
        ,b.Article + '\' + b.Colorid [Color]
        ,a.SizeCode [Size]
         ,'(' + qq.Cutpart + ')' + d.PatternDesc [Desc]
        --,Artwork.Artwork [Artwork]
        ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
        ,a.Qty [Quantity]
        ,a.BundleNo [Barcode]
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
                else  //沒勾[Extend All Parts]
                {
                    #region SQL
                    scmd = string.Format(@"
select *
from (
	select a.BundleGroup [Group_right]
			,c.FactoryID  [Group_left]
			,b.Sewinglineid [Line]
			,b.SewingCell [Cell]
			,b.Orderid [SP]
			,c.StyleID [Style]
			,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef),'') as [MarkerNo]
			,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
			,a.Parts [Parts]
			,b.Article + '\' + b.Colorid [Color]
			,a.SizeCode [Size]
			,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
			,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
			,a.Qty [Quantity]
			,a.BundleNo [Barcode]
			,a.Patterncode
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
			,iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef),'') as [MarkerNo]
			,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
			,a.Parts [Parts]
			,b.Article + '\' + b.Colorid [Color]
			,a.SizeCode [Size]
			,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
	,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )
			,a.Qty [Quantity]
			,a.BundleNo [Barcode]
			,a.Patterncode
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

                result = DBProxy.Current.Select("", scmd, pars, out dt);

                if (!result)
                {
                    return result;
                }
                #endregion
            }
            else
            {
                #region excel
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();
                List<SqlParameter> lis = new List<SqlParameter>();
                lis.Add(new SqlParameter("@ID", id));
                if (checkExtendAllParts.Checked)
                    lis.Add(new SqlParameter("@extend", "1"));
                else
                    lis.Add(new SqlParameter("@extend", "0"));

                string sqlcmd = string.Empty;
                if (checkExtendAllParts.Checked)  //有勾[Extend All Parts]
                {
                    #region SQL
                    sqlcmd = string.Format(@"
select [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty]
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
	outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
	outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
														from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
														where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
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
	outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
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

order by x.[Bundle]");
                    #endregion
                }
                else  //沒勾[Extend All Parts]
                {
                    #region SQL
                    sqlcmd = string.Format(@"
select [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty]
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
	outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
	outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
															from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
															where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
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
order by x.[Bundle]");
                    #endregion
                }

                result = DBProxy.Current.Select("", sqlcmd, lis, out dtt);
                if (!result)
                {
                    return result;
                }
                #endregion
            }            
            return result;
        }


        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (dtt == null || dtt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            // 顯示筆數於PrintForm上Count欄位
            SetCount(dtt.Rows.Count);
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); //預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", CurrentDataRow["ID"].ToString().Substring(0, 3)));
            objSheets.Cells[3, 1] = "To Line: " + CurrentDataRow["sewinglineid"].ToString();
            objSheets.Cells[3, 3] = "Cell: " + CurrentDataRow["SewingCell"].ToString();
            objSheets.Cells[3, 4] = "Comb: " + CurrentDataRow["PatternPanel"].ToString();
            objSheets.Cells[3, 5] = "Marker No: " + (CurrentDataRow["cutref"].ToString() == "" ? ""
                : MyUtility.GetValue.Lookup(string.Format(@"select top 1 MarkerNo from WorkOrder where  CutRef='{0}'", CurrentDataRow["cutref"].ToString())));
            objSheets.Cells[3, 7] = "Item: " + CurrentDataRow["item"].ToString();
            objSheets.Cells[3, 9] = "Article/Color: " + CurrentDataRow["article"].ToString() + "/ " + CurrentDataRow["colorid"].ToString();
            objSheets.Cells[3, 11] = "ID: " + CurrentDataRow["ID"].ToString();
            objSheets.Cells[4, 1] = "SP#: " + CurrentDataRow["Orderid"].ToString();
            objSheets.Cells[4, 4] = "Style#: " + MyUtility.GetValue.Lookup(string.Format("Select Styleid from Orders WITH (NOLOCK) Where id='{0}'", CurrentDataRow["Orderid"].ToString()));
            objSheets.Cells[4, 7] = "Cutting#: " + CurrentDataRow["cutno"].ToString();
            objSheets.Cells[4, 9] = "MasterSP#: " + CurrentDataRow["POID"].ToString();
            objSheets.Cells[4, 11] = "DATE: " + DateTime.Today.ToShortDateString();
            MyUtility.Excel.CopyToXls(dtt, "", "Cutting_P10.xltx", 5, false, null, objApp);      // 將datatable copy to excel
            objSheets.get_Range("D1:D1").ColumnWidth = 11;
            objSheets.get_Range("E1:E1").Columns.AutoFit();
            objSheets.get_Range("G1:H1").ColumnWidth = 9;
            objSheets.get_Range("I1:L1").ColumnWidth = 15;


            objSheets.Range[String.Format("A6:L{0}", dtt.Rows.Count+5)].Borders.Weight = 2;//設定全框線

            #region Save & Shwo Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Cutting_P10");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion 
            return true;
        }

        protected override bool OnToPrint(ReportDefinition report)
        {
            if (radioBundleCard.Checked)
            {
                #region Bundle Card
                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
                // 顯示筆數於PrintForm上Count欄位
                SetCount(dt.Rows.Count);


                DataTable dt1, dt2, dt3;
                //int count =dt.Rows.Count;
                int count = 1;
                dt1 = dt.Clone();
                dt2 = dt.Clone();
                dt3 = dt.Clone();
                foreach (DataRow dr in dt.Rows)
                {
                    //第一列資料
                    if (count % 3 == 1)
                    {
                        dt1.ImportRow(dr);
                    }
                    //第二列資料
                    if (count % 3 == 2)
                    {
                        dt2.ImportRow(dr);
                    }
                    //第三列資料
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
                        SizeSpec = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? "" : "(" + row1["SizeSpec"].ToString() + ")",
                        Desc = row1["Desc"].ToString(),
                        Artwork = row1["Artwork"].ToString(),
                        Quantity = row1["Quantity"].ToString(),
                        Barcode = row1["Barcode"].ToString()
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
                     SizeSpec2 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? "" : "(" + row1["SizeSpec"].ToString() + ")",
                     Desc2 = row1["Desc"].ToString(),
                     Artwork2 = row1["Artwork"].ToString(),
                     Quantity2 = row1["Quantity"].ToString(),
                     Barcode2 = row1["Barcode"].ToString()
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
                    SizeSpec3 = MyUtility.Check.Empty(row1["SizeSpec"].ToString()) ? "" : "(" + row1["SizeSpec"].ToString() + ")",
                    Desc3 = row1["Desc"].ToString(),
                    Artwork3 = row1["Artwork"].ToString(),
                    Quantity3 = row1["Quantity"].ToString(),
                    Barcode3 = row1["Barcode"].ToString()
                }).ToList());

                report.ReportDataSource = data;

                Type ReportResourceNamespace = typeof(P10_PrintData);
                Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
                string ReportResourceName = "P10_Print.rdlc";

                IReportResource reportresource;
                if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
                {
                    this.ShowException(result);
                    return result;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Sci.Win.Subs.ReportView(report);
                frm.MdiParent = MdiParent;
                frm.ShowDialog();
                #endregion
            }
            else
            {
                #region Bundle Check List
                if (dtt == null || dtt.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
                // 顯示筆數於PrintForm上Count欄位
                SetCount(dtt.Rows.Count);

                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); //預先開啟excel app
                pathName = Sci.Env.Cfg.ReportTempDir + "Cutting_BundleChecklist" + DateTime.Now.ToFileTime() + ".xls";
                string tmpName = Sci.Env.Cfg.ReportTempDir + "tmp.xls";
                if (MyUtility.Excel.CopyToXls(dtt, "", "Cutting_P10.xltx", 5, false, null, objApp, false))
                {
                    Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                    Microsoft.Office.Interop.Excel._Workbook objBook = objApp.ActiveWorkbook;
                    objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory where id = '{0}'", CurrentDataRow["ID"].ToString().Substring(0, 3)));
                    objSheets.Cells[3, 1] = "To Line: " + CurrentDataRow["sewinglineid"].ToString();
                    objSheets.Cells[3, 3] = "Cell: " + CurrentDataRow["SewingCell"].ToString();
                    objSheets.Cells[3, 4] = "Comb: " + CurrentDataRow["PatternPanel"].ToString();
                    objSheets.Cells[3, 5] = "Marker No: " + (CurrentDataRow["cutref"].ToString() == "" ? ""
                        : MyUtility.GetValue.Lookup(string.Format(@"select top 1 MarkerNo from WorkOrder where  CutRef='{0}'", CurrentDataRow["cutref"].ToString())));
                    objSheets.Cells[3, 7] = "Item: " + CurrentDataRow["item"].ToString();
                    objSheets.Cells[3, 9] = "Article/Color: " + CurrentDataRow["article"].ToString() + "/ " + CurrentDataRow["colorid"].ToString();
                    objSheets.Cells[3, 11] = "ID: " + CurrentDataRow["ID"].ToString();
                    objSheets.Cells[4, 1] = "SP#: " + CurrentDataRow["Orderid"].ToString();
                    objSheets.Cells[4, 4] = "Style#: " + MyUtility.GetValue.Lookup(string.Format("Select Styleid from Orders WITH (NOLOCK) Where id='{0}'", CurrentDataRow["Orderid"].ToString()));
                    objSheets.Cells[4, 7] = "Cutting#: " + CurrentDataRow["cutno"].ToString();
                    objSheets.Cells[4, 9] = "MasterSP#: " + CurrentDataRow["POID"].ToString();
                    objSheets.Cells[4, 11] = "DATE: " + DateTime.Today.ToShortDateString();
                    objSheets.get_Range("D1:D1").ColumnWidth = 11;
                    objSheets.get_Range("E1:E1").Columns.AutoFit();
                    objSheets.get_Range("G1:H1").ColumnWidth = 9;
                    objSheets.get_Range("I1:L1").ColumnWidth = 15;

                    objSheets.Range[String.Format("A6:L{0}", dtt.Rows.Count + 5)].Borders.Weight = 2;//設定全框線

                    //Random Excle名稱
                    Random random = new Random();
                    pathName = Sci.Env.Cfg.ReportTempDir + "Cutting_BundleChecklist - " + Convert.ToDateTime(DateTime.Now).ToString("yyyyMMddHHmmss") + " - " + Convert.ToString(Convert.ToInt32(random.NextDouble() * 10000)) + ".xlsx";
                    pathName = Path.GetFullPath(pathName);
                    objBook.SaveAs(pathName);
                    PrintDialog pd = new PrintDialog();
                    if (pd.ShowDialog() == DialogResult.OK)
                    {
                        string printer = pd.PrinterSettings.PrinterName;
                        objBook.PrintOutEx(ActivePrinter: printer);
                    }
                    objBook.Close();
                    objApp.Workbooks.Close();
                    objApp.Quit();

                    if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
                    if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
                    if (objBook != null) Marshal.FinalReleaseComObject(objBook);
                    objApp = null;
                }
                System.IO.File.Delete(pathName);
                //刪除存檔
                #endregion
            }
            return true;
        }

        //public bool PrintExcel(string filePath)
        //{
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
        //}

        private void radioPanel1_Paint(object sender, PaintEventArgs e)
        {

            if (radioBundleCard.Checked == true)
            {
                print.Enabled = true;
                toexcel.Enabled = false;
            }
            else if (radioBundleChecklist.Checked == true)
            {
                toexcel.Enabled = true;
                print.Enabled = true;
            }

        }

    }
}
