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
select 
[Style]= S.ID 
,[Season]= S.SeasonID  
,[Brand]= S.BrandID
,[CutpartID]= SA.PatternCode 
,[CutpartName]= SA.PatternDesc
,S.Description
,OA.Article
,SA.Price
,[FirstinlineDate]= MIN(O.SewInLine)
,SA.AddDate
,SA.AddName
,SA.EditDate
,SA.EditName
from Style_Artwork SA 
inner join style S on( SA.StyleUkey = S.Ukey)
left join Orders O on (O.StyleUkey = SA.StyleUkey)
left join ArtworkPO_Detail APD on (APD.OrderID = O.ID)
left join ArtworkPO AP on (AP.ID = APD.ID)
inner join dbo.View_Order_Artworks OA on OA.ID=o.ID and OA.PatternCode=APD.PatternCode 
and OA.ArtworkID=APD.ArtworkId and OA.ArtworkTypeID=APD.ArtworkTypeID
Where AP.POType='O' and APD.ArtworkTypeID='PRINTING' 
And  AP.LocalSuppID = (select top 1 PrintingSuppID from System)
");
         if (!MyUtility.Check.Empty(SeasonID))
         {
            sqlcmd += (string.Format("  And S.SeasonID= '{0}'", SeasonID));
         }

         sqlcmd += @"group by  S.ID,S.SeasonID,S.BrandID,SA.PatternCode,SA.PatternDesc,S.Description,
                                      OA.Article,SA.Price,SA.AddDate,SA.AddName,SA.EditDate,SA.EditName";
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
         worksheet.Cells[1, 6] = MyUtility.GetValue.Lookup("SELECT TOP 1 PrintingSuppID FROM [Production].[dbo].SYSTEM");
         worksheet.Columns.AutoFit();
         #endregion
         #region Save & Show Excel
         string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R51");
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
