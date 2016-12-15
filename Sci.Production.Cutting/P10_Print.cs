using Ict;
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

namespace Sci.Production.Cutting
{
    public partial class P10_Print : Sci.Win.Tems.PrintForm
    {
        DualResult result;
        DataRow CurrentDataRow;
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
            Bundle_Card = radioButton1.Checked.ToString();
            Bundle_Check_list = radioButton2.Checked.ToString();
            Extend_All_Parts = checkBox1.Checked.ToString();
            return base.ValidateInput();
        }
        DataTable dtt; 
        DataTable dt;
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                #region report
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();
                
                List<SqlParameter> pars = new List<SqlParameter>(); 
                pars.Add(new SqlParameter("@ID", id));
                if (checkBox1.Checked)
                    pars.Add(new SqlParameter("@extend", "1"));
                else
                    pars.Add(new SqlParameter("@extend", "0"));

                string scmd = string.Empty;
                if (checkBox1.Checked)  //有勾[Extend All Parts]
                {
                    #region SQL
                    scmd = string.Format(@"select a.BundleGroup [Group_right]
		                                            ,b.MDivisionid [Group_left]
                                                    ,b.Sewinglineid [Line]
                                                    ,b.SewingCell [Cell]
                                                    ,b.Orderid [SP]
                                                    ,c.StyleID [Style]
                                                    ,b.Item [Item]
                                                    ,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
		                                            ,a.Parts [Parts]
                                                    ,b.Article + '\' + b.Colorid [Color]
                                                    ,a.SizeCode [Size]
		                                            ,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
                                                    ,Artwork.Artwork [Artwork]
                                                    ,a.Qty [Quantity]
                                                    ,a.BundleNo [Barcode]
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                            outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
							                                                                        where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
							                                                                        for xml path('')))as Artwork
                                            where a.ID= @ID and a.Patterncode != 'ALLPARTS'

                                            union all

                                            select a.BundleGroup [Group_right]
		                                            ,b.MDivisionid [Group_left]
                                                    ,b.Sewinglineid [Line]
                                                    ,b.SewingCell [Cell]
                                                    ,b.Orderid [SP]
                                                    ,c.StyleID [Style]
                                                    ,b.Item [Item]
                                                    ,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
		                                            ,d.Parts [Parts]
                                                    ,b.Article + '\' + b.Colorid [Color]
                                                    ,a.SizeCode [Size]
		                                            ,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
                                                    ,Artwork.Artwork [Artwork]
                                                    ,a.Qty [Quantity]
                                                    ,a.BundleNo [Barcode]
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            left join dbo.Bundle_Detail_Allpart d on d.id=a.Id
                                            outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',d.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                            outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
							                                                                        where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
							                                                                        for xml path('')))as Artwork
                                            where a.ID= @ID and a.Patterncode = 'ALLPARTS'
                                            order by a.BundleNo ");
                        #endregion
                }
                else  //沒勾[Extend All Parts]
                {
                    #region SQL
                    scmd = string.Format(@"select a.BundleGroup [Group_right]
		                                            ,b.MDivisionid [Group_left]
                                                    ,b.Sewinglineid [Line]
                                                    ,b.SewingCell [Cell]
                                                    ,b.Orderid [SP]
                                                    ,c.StyleID [Style]
                                                    ,b.Item [Item]
                                                    ,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
		                                            ,a.Parts [Parts]
                                                    ,b.Article + '\' + b.Colorid [Color]
                                                    ,a.SizeCode [Size]
		                                            ,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
                                                    ,Artwork.Artwork [Artwork]
                                                    ,a.Qty [Quantity]
                                                    ,a.BundleNo [Barcode]
		                                            ,a.Patterncode
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
                                            outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
							                                                                        where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
							                                                                        for xml path('')))as Artwork
                                            where a.ID= @ID and a.Patterncode != 'ALLPARTS'

                                            union all

                                            select a.BundleGroup [Group_right]
		                                            ,b.MDivisionid [Group_left]
                                                    ,b.Sewinglineid [Line]
                                                    ,b.SewingCell [Cell]
                                                    ,b.Orderid [SP]
                                                    ,c.StyleID [Style]
                                                    ,b.Item [Item]
                                                    ,isnull(b.PatternPanel,'')+'-'+convert(varchar,b.Cutno) [Body_Cut]
		                                            ,a.Parts [Parts]
                                                    ,b.Article + '\' + b.Colorid [Color]
                                                    ,a.SizeCode [Size]
		                                            ,'(' + a.Patterncode + ')' + a.PatternDesc [Desc]
                                                    ,Artwork.Artwork [Artwork]
                                                    ,a.Qty [Quantity]
                                                    ,a.BundleNo [Barcode]
		                                            ,a.Patterncode
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
                                            outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
							                                                                        where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
							                                                                        for xml path('')))as Artwork
                                            where a.ID= @ID and a.Patterncode = 'ALLPARTS'
                                            order by a.BundleNo ");
                    #endregion
                }

                result = DBProxy.Current.Select("", scmd, pars, out dt);
                ReportDefinition report = e.Report;
                if (!result)
                {
                 return result;
                }
               
                // 傳 list 資料            
                List<P10_PrintData> data = dt.AsEnumerable()
                    .Select(row1 => new P10_PrintData()
                    {
                        Group_right = row1["Group_right"].ToString(),
                        Group_left = row1["Group_left"].ToString(),
                        Line = row1["Line"].ToString(),
                        Cell = row1["Cell"].ToString(),
                        SP = row1["SP"].ToString(),
                        Style = row1["Style"].ToString(),
                        Item = row1["Item"].ToString(),
                        Body_Cut = row1["Body_Cut"].ToString(),
                        Parts = row1["Parts"].ToString(),
                        Color = row1["Color"].ToString(),
                        Size = row1["Size"].ToString(),
                        Desc = row1["Desc"].ToString(),
                        Artwork = row1["Artwork"].ToString(),
                        Quantity = row1["Quantity"].ToString(),
                        Barcode = row1["Barcode"].ToString()
                    }).ToList();

                e.Report.ReportDataSource = data;
                 
                
               #endregion
            }
            else
            {
                #region excel
                DataRow row = this.CurrentDataRow;
                string id = row["ID"].ToString();
                List<SqlParameter> lis = new List<SqlParameter>();
                lis.Add(new SqlParameter("@ID", id));
                if (checkBox1.Checked)
                    lis.Add(new SqlParameter("@extend", "1"));
                else
                    lis.Add(new SqlParameter("@extend", "0"));

                string sqlcmd = string.Empty;
                if (checkBox1.Checked)  //有勾[Extend All Parts]
                {
                    #region SQL
                    sqlcmd = string.Format(@"select b.id [Bundle_ID]
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
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                            outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
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
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            left join dbo.Bundle_Detail_Allpart d on d.id=a.Id
                                            outer apply( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',d.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] )[qq]
                                            outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
							                                                                        where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
							                                                                        for xml path('')))as Artwork
                                            where a.ID= @ID and a.Patterncode = 'ALLPARTS'
                                            order by a.BundleNo ");
                    #endregion
                }
                else  //沒勾[Extend All Parts]
                {
                    #region SQL
                    sqlcmd = string.Format(@"select b.id [Bundle_ID]
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
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
                                            outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
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
                                            from dbo.Bundle_Detail a
                                            left join dbo.Bundle b on a.id=b.id
                                            left join dbo.orders c on c.id=b.Orderid
                                            outer apply ( select iif(a.PatternCode = 'ALLPARTS',iif(@extend='1',a.PatternCode,a.PatternCode),a.PatternCode) [Cutpart] ) [qq]
                                            outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
							                                                                        from dbo.Bundle_Detail_Art e1
							                                                                        where e1.id=b.id and e1.PatternCode= qq.Cutpart and e1.Bundleno=a.BundleNo
							                                                                        for xml path('')))as Artwork
                                            where a.ID= @ID and a.Patterncode = 'ALLPARTS'
                                            order by a.BundleNo ");
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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Cutting_P10.xltx"); //預先開啟excel app                         
            MyUtility.Excel.CopyToXls(dtt, "", "Cutting_P10.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }

        private void radioPanel1_Paint(object sender, PaintEventArgs e)
        {

            if ( radioButton1.Checked==true)
            {
                print.Enabled = true;
                toexcel.Enabled = false;
            }
            else if (radioButton2.Checked == true)
            {
                toexcel.Enabled = true;
                print.Enabled = false;
            }

        }
      
    }
}
