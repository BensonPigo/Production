using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
   public partial class R52 : Sci.Win.Tems.PrintForm
   {
      DataTable printData;
      public R52(ToolStripMenuItem menuitem)
      {
         InitializeComponent();
      }
      string SeasonID;

      protected override bool ValidateInput()
      {
         SeasonID = txtseason.Text;
         return base.ValidateInput();
      }
      protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
      {
         #region SQL Command
         string sqlcmd = string.Format(@"
select	[Style]= S.ID 
		, [Season]= S.SeasonID  
		, [Brand]= S.BrandID
		, [CutpartID]= vSA.PatternCode 
		, [CutpartName]= vSA.PatternDesc
		, S.Description
		, vSA.Article
		, Price = vSA.Cost
		, [FirstinlineDate] = (
								select MIN(O.SewInLine)
								from orders o
								where vsa.StyleUkey = o.StyleUkey
									  and o.Category in ('B', 'S')
									  and o.SewInLine is not null
								)
        , [Refno] = 
            (select stuff((
	            select distinct concat(',', sb.Refno)
	            from Style_Artwork sa
	            inner join Pattern_GL p on p.id = sa.SMNoticeID
		            and p.Version = sa.PatternVersion
		            and iif(len(p.PatternCode) > 10, substring(p.PatternCode, 11, len(p.PatternCode)-10), p.PatternCode) = sa.PatternCode
	            inner join Pattern_GL_LectraCode pgl on p.PatternUKEY = pgl.PatternUKEY and p.SEQ = pgl.SEQ
	            inner join Style_BOF sb on sa.StyleUkey = sb.StyleUkey and sb.FabricCode = pgl.FabricCode
	            where sa.ArtworkTypeID = 'printing' and sa.StyleUkey =  S.Ukey
	            for xml path('')
            ),1,1,''))
		,TypeofGarment=(select Name  from Reason where s.ApparelType = Reason.ID and Reason.ReasonTypeID = 'Style_Apparel_Type')
		,SA.AddDate
		,SA.AddName
		,SA.EditDate
		,SA.EditName
		, JSON=[dbo].[udf-Str-JSON](0,1,(Select Seq=ROW_NUMBER() over (order by ss.Seq), ss.SizeGroup, ss.SizeCode From Style_SizeCode ss where StyleUkey = vSA.StyleUkey order by ss.Seq for XML RAW ))
from dbo.View_style_Artwork vSA 
inner join Style_Artwork SA on vSA.StyleArtworkUkey = SA.Ukey
inner join style S on vSA.StyleUkey = S.Ukey
Where	vSA.ArtworkTypeID = 'Printing'
");
         if (!MyUtility.Check.Empty(SeasonID))
         {
            sqlcmd += (string.Format("  And S.SeasonID= '{0}'", SeasonID));
         }

         #endregion
         DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
         DualResult result;
         if (result = DBProxy.Current.Select(null, sqlcmd, out printData))
         {
            return result;
         }
         DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
         return base.OnAsyncDataLoad(e);
      }

      protected override bool OnToExcel(ReportDefinition report)
      {
         #region Check Data
         if (MyUtility.Check.Empty(printData) || printData.Rows.Count == 0)
         {
            MyUtility.Msg.InfoBox("Data not found.");
            return false;
         }
         #endregion
         this.SetCount(printData.Rows.Count);
         this.ShowWaitMessage("Excel Processing");

         #region  To Excel
         Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R52.xltx");
         MyUtility.Excel.CopyToXls(printData, "", "Subcon_R52.xltx", 2, showExcel: false, excelApp: objApp);
         Excel.Worksheet worksheet = objApp.Sheets[1];
         worksheet.Cells[1, 2] = SeasonID;
         worksheet.Columns.AutoFit();
         #endregion
         #region Save & Show Excel
         string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R52");
         objApp.ActiveWorkbook.SaveAs(strExcelName);
         objApp.Quit();
         Marshal.ReleaseComObject(objApp);
         Marshal.ReleaseComObject(worksheet);

         strExcelName.OpenFile();
         #endregion

         this.HideWaitMessage();
         return true;
      }
   }
}
