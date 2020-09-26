using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;

namespace Sci.Production.Subcon
{
    public partial class R52 : Win.Tems.PrintForm
   {
      private DataTable printData;

      public R52(ToolStripMenuItem menuitem)
      {
         this.InitializeComponent();
      }

      private string SeasonID;

        /// <inheritdoc/>
        protected override bool ValidateInput()
      {
         this.SeasonID = this.txtseason.Text;
         return base.ValidateInput();
      }

        /// <inheritdoc/>
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
		,x.ColorID
		,SA.AddDate
		,SA.AddName
		,SA.EditDate
		,SA.EditName
		, JSON=[dbo].[udf-Str-JSON](0,1,(Select Seq=ROW_NUMBER() over (order by ss.Seq), ss.SizeGroup, ss.SizeCode From Style_SizeCode ss where StyleUkey = vSA.StyleUkey order by ss.Seq for XML RAW ))
from dbo.View_style_Artwork vSA 
inner join Style_Artwork SA on vSA.StyleArtworkUkey = SA.Ukey
inner join style S on vSA.StyleUkey = S.Ukey
outer apply(
	select distinct sc.ColorID
	from (
		select distinct pgl.FabricPanelCode
		from Pattern_GL p 
		inner join Pattern_GL_LectraCode pgl on p.PatternUKEY = pgl.PatternUKEY and p.SEQ = pgl.SEQ
		where p.id = sa.SMNoticeID
			and p.Version = sa.PatternVersion
			and iif(len(p.PatternCode) > 10, substring(p.PatternCode, 11, len(p.PatternCode)-10), p.PatternCode) = sa.PatternCode
			and iif(sa.Article='----', 'F_CODE', pgl.ArticleGroup)= pgl.ArticleGroup
	) pa
	inner join Style_ColorCombo sc on sa.StyleUkey = sc.StyleUkey and sc.FabricPanelCode = pa.FabricPanelCode and sc.Article = vSA.Article
)x
Where	vSA.ArtworkTypeID = 'Printing'
");
         if (!MyUtility.Check.Empty(this.SeasonID))
         {
            sqlcmd += string.Format("  And S.SeasonID= '{0}'", this.SeasonID);
         }

         #endregion
         DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
         DualResult result;
         if (result = DBProxy.Current.Select(null, sqlcmd, out this.printData))
         {
            return result;
         }

         DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
         return base.OnAsyncDataLoad(e);
      }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
      {
         #region Check Data
         if (MyUtility.Check.Empty(this.printData) || this.printData.Rows.Count == 0)
         {
            MyUtility.Msg.InfoBox("Data not found.");
            return false;
         }
         #endregion
         this.SetCount(this.printData.Rows.Count);
         this.ShowWaitMessage("Excel Processing");

         #region  To Excel
         Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R52.xltx");
         MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R52.xltx", 2, showExcel: false, excelApp: objApp);
         Excel.Worksheet worksheet = objApp.Sheets[1];
         worksheet.Cells[1, 2] = this.SeasonID;
         worksheet.Columns.AutoFit();
         #endregion
         #region Save & Show Excel
         string strExcelName = Class.MicrosoftFile.GetName("Subcon_R52");
         objApp.ActiveWorkbook.SaveAs(strExcelName);
         objApp.Quit();
         Marshal.ReleaseComObject(objApp);
         Marshal.ReleaseComObject(worksheet);

         strExcelName.OpenFile();
         #endregion

         this.HideWaitMessage();
         return true;
      }

      private void BtnSketchDownload_Click(object sender, EventArgs e)
        {
            int ttlCnt = 0;
            string destination_path = MyUtility.GetValue.Lookup("select StyleSketch from System WITH (NOLOCK) ", null);

            DataTable dt;
            string sqlcmd = $@"
select * from
(
	select Picture =Picture1,ID,SeasonID,BrandID,PicNo = '01' from Style where SeasonID='{this.txtseason.Text}' and Picture1 !=''
	union all
	select Picture =Picture2,ID,SeasonID,BrandID,PicNo = '02' from Style where SeasonID='{this.txtseason.Text}' and Picture2 !=''
) a order by  Picture
";
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dt)))
            {
                this.ShowErr(result);
                return;
            }

            if (Directory.Exists(destination_path))
            {
                DirectoryInfo dir = new DirectoryInfo(destination_path);

                // 開窗選擇存放位置
                FolderBrowserDialog path = new FolderBrowserDialog();
                if (path.ShowDialog() == DialogResult.OK)
                {
                    this.ShowLoadingText("Loading...");
                    ttlCnt = 0;

                    if (!MyUtility.Check.Empty(dt))
                    {
                        ttlCnt = dt.Rows.Count;
                        int cnt = 1;

                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                string newName = dr["ID"].ToString().Trim() + "@" + dr["SeasonID"].ToString().Trim() + "@" + dr["BrandID"].ToString().Trim() + "@" + dr["PicNo"].ToString().Trim() + ".jpg";

                                // 複製檔案並更名到指定路徑
                                File.Copy(dir + "\\" + dr["Picture"].ToString(), path.SelectedPath + "\\" + newName, true);
                                this.ShowLoadingText($"{cnt}/{ttlCnt}");
                                cnt++;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    this.HideLoadingText();
                }
                else
                {
                    return;
                }
            }

            if (ttlCnt != 0)
            {
                MyUtility.Msg.InfoBox("Finished!");
            }
        }
    }
}
