using Ict;
using Ict.Win;
using Microsoft.Office.Core;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Class.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using MsExcel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_AccessoryCard : Win.Tems.QueryForm
    {
        private string poID;
        private string orderID;
        private string brandID;
        private string styleID;
        private string seasonID;
        private string factoryID;

        DataTable selArticle;

        public P01_AccessoryCard(string _orderID, string _brandID, string _styleID, string _seasonID, string _factoryID)
        {
            InitializeComponent();

            this.orderID = _orderID;
            this.brandID = _brandID;
            this.styleID = _styleID;
            this.seasonID = _seasonID;
            this.factoryID = _factoryID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            OnGridSetup();
            OnRequery();

            this.grid.IsEditingReadOnly = false;
        }

        private void OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("sel", header: "Sel", width: Widths.AnsiChars(4), trueValue: true, falseValue: false, iseditable: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(12), iseditingreadonly: true)
                ;
        }

        private void OnRequery()
        {
            this.poID = DBProxy.Current.LookupEx<string>($"Select POID from Orders where ID = '{this.orderID}'").ExtendedData;

            DualResult result;
            List<SqlParameter> paras = new List<SqlParameter>();
            string sqlCmd = @"Select Convert(Bit, 0) as Sel
                                 , oa.Article,sa.ArticleName
                              From dbo.Orders o
                              Left Join dbo.Order_Article oa
                                On oa.ID = o.ID
                              LEFT JOIN Orders o2 on o2.ID = o.POID
                              LEFT JOIN Style s ON s.BrandID = o2.BrandID and s.Id=o2.StyleID and s.SeasonID = o2.SeasonID
                              LEFT JOIN Style_Article sa on s.Ukey = sa.StyleUkey and sa.Article = oa.Article
                             Where o.PoID = @PoID
                             Group by oa.Article,sa.ArticleName
                             Order by oa.Article";
            paras.Add(new SqlParameter("@PoID", this.poID));

            result = DBProxy.Current.Select(null, sqlCmd, paras, out this.selArticle);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.grid.DataSource = this.selArticle;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            if (this.RdbSCI.Checked)
            {
                DataTable tmpRefNo;
                DualResult result = GetAccessoryCard(this.selArticle, this.poID, out tmpRefNo);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (tmpRefNo == null || tmpRefNo.Rows.Count == 0)
                {
                    this.ShowErr("No data!!");
                    return;
                }

                string dotPath = System.IO.Path.Combine(Env.Cfg.XltPathDir, "Purchase_P01_TrimCard.dot");
                Sci.Utility.Word.SaveDotReportCls sdr = new Utility.Word.SaveDotReportCls(dotPath);

                DataRow dr;
                bool res = MyUtility.Check.Seek(
                    @"
select Orders.BrandID,Orders.SeasonID,Orders.StyleID,FactoryID,Orders.ID,isnull(smr.IdAndNameAndExt,'') IdAndNameAndExt,d.artno from Orders 
left join PO ON PO.ID = Orders.ID
Left Join dbo.GetName as Smr On Smr.ID = PO.POHandle
OUTER APPLY(SELECT STUFF((SELECT '/'+Article FROM Order_Article WHERE ID = Orders.ID  order by Article FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)'),1,1,'') as artno) d
where Orders.ID = @ID
", new List<SqlParameter> { new SqlParameter("@ID", this.poID) }, out dr, null);

                if (!res)
                {
                    this.ShowErr("Get data fail.");
                }

                List<string> lisArticle = new List<string>();
                DataTable tmpRefnoColorGP = tmpRefNo.Copy();
                tmpRefnoColorGP.Columns.Remove("RowID");
                tmpRefnoColorGP.DefaultView.Sort = "SortField, RefNo";
                tmpRefnoColorGP = tmpRefnoColorGP.DefaultView.ToTable(true);

                foreach (DataRow row in tmpRefnoColorGP.Rows)
                {
                    string Article = row["Article"].ToString().Trim();
                    if (!lisArticle.Contains(Article)) lisArticle.Add(Article);
                }

                sdr.dicDatas.Add(sdr._v + "BrandID", dr["BrandID"]);
                sdr.dicDatas.Add(sdr._v + "SeasonID", dr["SeasonID"]);
                sdr.dicDatas.Add(sdr._v + "STYLENO", dr["StyleID"]);
                sdr.dicDatas.Add(sdr._v + "ARTICLENO", string.Join(",", lisArticle));
                sdr.dicDatas.Add(sdr._v + "FACTORYID", dr["FactoryID"]);
                sdr.dicDatas.Add(sdr._v + "SPNO", this.poID);
                sdr.dicDatas.Add(sdr._v + "USER", dr["IdAndNameAndExt"]);

                string str = string.Empty;

                str = this.exportAccessoryCar(tmpRefnoColorGP, str, sdr._BreakPage, 1, "F");
                str = this.exportAccessoryCar(tmpRefnoColorGP, str, sdr._BreakPage, 4, "A");

                sdr.dicDatas.Add(sdr._v + "DATAS", str);

                string FileName = DateTime.Now.ToString("MMddmm") + this.poID.Trim() + ".doc";
                string outPath = Path.Combine(Env.Cfg.ReportTempDir, FileName);
                sdr.Save(outPath);
            }
            else if (this.RdbNF.Checked && this.brandID == "N.FACE")
            {
                var colorwaydrs = this.selArticle.Select("Sel=1");
                if (colorwaydrs.Length == 0)
                {
                    this.ShowErr("No data!!");
                    return;
                }

                var xltDir = Env.Cfg.XltPathDir;

                var xltName = @"Purchase_P01_AccessoryCard.xltx";

                var xltFullPath = System.IO.Path.Combine(xltDir, xltName);
                DataRow dr;

                // 取得COVER頁資料
                bool res = MyUtility.Check.Seek(
                    @"SELECT o.ID,o.FactoryID,s.Picture1,s.Picture2,o.StyleID,s.StyleName,o.SeasonID,SeasonForDisplay,o.BrandID,o.ProgramID FROM Orders o
LEFT JOIN Style s on o.StyleID =s.Id and o.SeasonID = s.SeasonID and o.BrandID = s.BrandID
LEFT JOIN Season on Season.BrandID = o.BrandID and Season.ID = o.SeasonID
WHERE o.ID= @ID
",
                    new List<SqlParameter> { new SqlParameter("@ID", this.poID) },
                    out dr,
                    null);
                var picturePath = DBProxy.Current.LookupEx<string>("SELECT StyleSketch FROM dbo.System").ExtendedData;
                var fabricPath = DBProxy.Current.LookupEx<string>("SELECT FabricPath  FROM dbo.System").ExtendedData;
                if (!res)
                {
                    this.ShowErr("Get data fail.");
                    return;
                }

                DualResult result = Result.F("not init");
                var tempDir = Env.Cfg.ReportTempDir;
                var fileName = $@"{dr["SeasonID"]}-{dr["StyleID"]}";

                var fullSavePath = string.Empty;
                var app = new MsExcel.Application();
                try
                {
#if DEBUG
                    app.Visible = true;
#else
                app.Visible = false;
#endif
                    app.DisplayAlerts = false;
                    app.Workbooks.Add(xltFullPath);
                    MsExcel.Workbook book = app.Workbooks[1];
                    var title = "TNF " + dr["SeasonForDisplay"].ToString() + " " + dr["ProgramID"].ToString() + " " + dr["StyleID"].ToString() + "-SP#" + dr["ID"].ToString();
                    int articles = colorwaydrs.Length;

                    if (articles > 3)
                    {
                        articles = 3;
                    }

                    int refnocounts = 0;

                    #region Cover
                    var sheetCover = (MsExcel.Worksheet)book.Worksheets["COVER"];
                    sheetCover.Range["B9"].Value = dr["StyleID"].ToString();
                    sheetCover.Range["B10"].Value = dr["StyleName"].ToString();
                    sheetCover.Range["B11"].Value = dr["SeasonForDisplay"].ToString();
                    sheetCover.Range["B12"].Value = dr["FactoryID"].ToString();
                    sheetCover.Range["B13"].Value = dr["ID"].ToString();

                    if (string.IsNullOrWhiteSpace(dr["Picture1"].ToString()) == false && !picturePath.IsNullOrWhiteSpace())
                    {
                        var imgpath = System.IO.Path.Combine(picturePath, dr["Picture1"].ToString());
                        if (System.IO.File.Exists(imgpath))
                        {
                            var cell = sheetCover.Cells[8, 3];
                            sheetCover.Shapes.AddPicture(
                                imgpath,
                                Microsoft.Office.Core.MsoTriState.msoFalse,
                                Microsoft.Office.Core.MsoTriState.msoTrue,
                                cell.Left + 1,
                                cell.Top + 2,
                                205,
                                295);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(dr["Picture2"].ToString()) == false && !picturePath.IsNullOrWhiteSpace())
                    {
                        var imgpath = System.IO.Path.Combine(picturePath, dr["Picture2"].ToString());
                        if (System.IO.File.Exists(imgpath))
                        {
                            var cell = sheetCover.Cells[8, 4];
                            sheetCover.Shapes.AddPicture(
                                imgpath,
                                Microsoft.Office.Core.MsoTriState.msoFalse,
                                Microsoft.Office.Core.MsoTriState.msoTrue,
                                cell.Left + 2,
                                cell.Top + 2,
                                205,
                                295);
                        }
                    }
                    #endregion

                    #region FAB
                    List<SqlParameter> paras = new List<SqlParameter>();
                    DataTable dtFAB;
                    string sqlCmd = @"
                                SELECT DISTINCT bof.Seq1,bof.Refno,fs.SuppRefno,bof.FabricCode,ColorID = isnull(oc.ColorID,''),PatternPanel = isnull(oc.PatternPanel,''),isnull(DetailColor.Name,isnull(c.Name,'')) ColorName,Article = isnull(oc.Article,'') FROM Order_BOF bof
                                LEFT JOIN Order_ColorCombo oc on oc.Id = bof.Id and oc.FabricCode = bof.FabricCode
                                LEFT JOIN Fabric_Supp fs on fs.SCIRefno = bof.SCIRefno and fs.SuppID = bof.SuppID
                                LEFT JOIN Color C on OC.ColorID = C.ID and C.BrandId ='N.FACE'
                                OUTER APPLY (
                                    SELECT
	                                    NAME =
	                                    STUFF((SELECT DISTINCT
			                                    ';' + DetailColor.Name
		                                    FROM Color_multiple cm
			                                    LEFT JOIN Color DetailColor On  DetailColor.BrandID = cm.BrandID
			                                    Where cm.ColorUkey = c.Ukey And DetailColor.ID = cm.ColorID
		                                    FOR XML PATH (''))
	                                    , 1, 1, '')) DetailColor
                                WHERE bof.ID=@PoID and bof.SuppID <>'FTY'
                                Order by bof.Seq1,bof.Refno,PatternPanel,ColorID";
                    paras.Add(new SqlParameter("@PoID", this.poID));

                    result = DBProxy.Current.Select(null, sqlCmd, paras, out dtFAB);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    DataTable distinctRefno = dtFAB.DefaultView.ToTable(true, "Seq1", "Refno", "SuppRefno", "PatternPanel");
                    var sheetFAB = (MsExcel.Worksheet)book.Worksheets["FAB"];
                    sheetFAB.Range["C2"].Value = title;
                    var fabSheetcount = 0;

                    if (distinctRefno.Rows.Count > 5)
                    {
                        refnocounts = 5;
                    }
                    else
                    {
                        refnocounts = distinctRefno.Rows.Count;
                    }

                    if (refnocounts == 0)
                    {
                        sheetFAB.Delete();
                    }
                    else
                    {
                        // 先計算總共會產生幾個sheet
                        for (int i = 0; i < Math.Ceiling(MyUtility.Convert.GetDouble(colorwaydrs.Length) / 3); i++)
                        {
                            if (i > 0)
                            {
                                fabSheetcount++;
                            }

                            for (int j = 0; j < Math.Ceiling(MyUtility.Convert.GetDouble(distinctRefno.Rows.Count) / 5); j++)
                            {
                                if (j > 0)
                                {
                                    fabSheetcount++;
                                }
                            }
                        }

                        // 複製Sheet
                        for (int i = fabSheetcount; i > 0; i--)
                        {
                            int index = sheetFAB.Index;
                            sheetFAB.Copy(System.Type.Missing, sheetFAB);
                            MsExcel.Worksheet newWS = (MsExcel.Worksheet)book.Worksheets[index + 1];
                            newWS.Name = string.Format("FAB ({0})", i);
                        }

                        // 執行資料寫入
                        for (double i = 0; i <= fabSheetcount; i++)
                        {
                            int index = sheetFAB.Index;
                            MsExcel.Worksheet currentWS = (MsExcel.Worksheet)book.Worksheets[index + i];
                            var articlePages = Math.Ceiling(Convert.ToDouble(colorwaydrs.Length) / 3);
                            var refnoPages = Math.Ceiling(Convert.ToDouble(distinctRefno.Rows.Count) / 5);
                            var nowArtStr = MyUtility.Convert.GetInt((i % articlePages) * 3); // 紀錄colorWay寫入到哪
                            if (articlePages > 1 && refnoPages > 1)
                            {
                                nowArtStr = MyUtility.Convert.GetInt(Math.Floor(MyUtility.Convert.GetDouble(i / refnoPages))) * articles; // 紀錄colorWay寫入到哪
                            }

                            var nowRefStr = MyUtility.Convert.GetInt((i % refnoPages) * refnocounts); // 紀錄Refno寫入到哪

                            string refno = string.Empty;
                            for (int k = 0; k < refnocounts; k++)
                            {
                                var cellRefno = currentWS.Cells[4, 2 + k];
                                var cellSuppRefno = currentWS.Cells[5, 2 + k];
                                if (k < articles)
                                {
                                    var cell = currentWS.Cells[6 + k, 1];
                                    if (colorwaydrs.Length - nowArtStr - k > 0)
                                    {
                                        cell.Value = colorwaydrs[nowArtStr + k]["ArticleName"] + Environment.NewLine + "[" + colorwaydrs[nowArtStr + k]["Article"] + "]";
                                    }
                                }

                                if (distinctRefno.Rows.Count - nowRefStr - k > 0)
                                {
                                    cellRefno.Value = distinctRefno.Rows[nowRefStr + k]["Refno"];
                                    cellSuppRefno.Value = distinctRefno.Rows[nowRefStr + k]["SuppRefno"];
                                    if (refno == distinctRefno.Rows[nowRefStr + k]["Refno"].ToString())
                                    {
                                        MsExcel.Range top = currentWS.Cells[4, 2 + k - 1];
                                        MsExcel.Range bottom = currentWS.Cells[4, 2 + k];
                                        MsExcel.Range all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                        all.Merge(0);
                                        top = currentWS.Cells[5, 2 + k - 1];
                                        bottom = currentWS.Cells[5, 2 + k];
                                        all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                        all.Merge(0);
                                    }

                                    refno = distinctRefno.Rows[nowRefStr + k]["Refno"].ToString();
                                }

                                string color1 = string.Empty;
                                string color2 = string.Empty;
                                string color3 = string.Empty;
                                for (int z = 0; z < articles; z++)
                                {
                                    var cellColor = currentWS.Cells[6 + z, 2 + k];
                                    if (distinctRefno.Rows.Count - nowRefStr - k > 0 && colorwaydrs.Length - nowArtStr - z > 0)
                                    {
                                        var color = dtFAB.Select($"(Article='{colorwaydrs[nowArtStr + z]["Article"]}' or Article='') and Refno='{distinctRefno.Rows[nowRefStr + k]["Refno"]}' and PatternPanel='{distinctRefno.Rows[nowRefStr + k]["PatternPanel"]}'").AsEnumerable().Select(row => row["ColorName"].ToString()).Select(name => name).Distinct().JoinToString(Environment.NewLine);
                                        color = color.Replace(";", Environment.NewLine);
                                        cellColor.Value = color;

                                        switch (z)
                                        {
                                            case 0:
                                                color1 = color;
                                                break;
                                            case 1:
                                                color2 = color;
                                                break;
                                            case 2:
                                                color3 = color;
                                                break;
                                        }
                                    }

                                    if (colorwaydrs.Length - nowArtStr - z - 1 == 0 || z == articles - 1)
                                    {
                                        int beginrow = 6;
                                        int endrow = 6;
                                        if (color1 == color2 && color2 == color3 && !color2.IsNullOrWhiteSpace())
                                        {
                                            endrow = 8;
                                        }
                                        else if (color1 == color2 && !color2.IsNullOrWhiteSpace())
                                        {
                                            endrow = 7;
                                        }
                                        else if (color2 == color3 && !color2.IsNullOrWhiteSpace())
                                        {
                                            beginrow = 7;
                                            endrow = 8;
                                        }

                                        MsExcel.Range top = currentWS.Cells[beginrow, 2 + k];
                                        MsExcel.Range bottom = currentWS.Cells[endrow, 2 + k];
                                        MsExcel.Range all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                        all.Merge(0);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region ACC (ZIPPER)
                    paras = new List<SqlParameter>();
                    DataTable dtAccZipper;
                    sqlCmd = @"
                                SELECT DISTINCT boa.Seq1,boa.Refno,f.MtltypeId,boa.FabricPanelCode,ColorID=isnull(oc.ColorID,''),isnull(DetailColor.Name,isnull(c.Name,'')) ColorName,Article = isnull(oc.Article,'') FROM Order_BOA boa
                                LEFT JOIN Fabric f on boa.SCIRefno = f.SCIRefno
                                LEFT JOIN Fabric_Supp fs on fs.SCIRefno = boa.SCIRefno and fs.SuppID = boa.SuppID
                                LEFT JOIN Order_ColorCombo oc on oc.Id = boa.Id and oc.FabricPanelCode = boa.FabricPanelCode
                                LEFT JOIN Color C on OC.ColorID = C.ID and C.BrandId ='N.FACE'
                                OUTER APPLY (
                                  SELECT
                                      NAME =
                                      STUFF((SELECT DISTINCT
                                              ';' + DetailColor.Name
                                          FROM Color_multiple cm
                                              LEFT JOIN Color DetailColor On  DetailColor.BrandID = cm.BrandID
                                              Where cm.ColorUkey = c.Ukey And DetailColor.ID = cm.ColorID
                                          FOR XML PATH (''))
                                      , 1, 1, '')
                                ) DetailColor
                                WHERE boa.ID=@PoID and f.MtltypeId='ZIPPER' and boa.SuppID <>'FTY'
                                ORDER BY boa.Seq1,boa.Refno,boa.FabricPanelCode,ColorID";
                    paras.Add(new SqlParameter("@PoID", this.poID));

                    result = DBProxy.Current.Select(null, sqlCmd, paras, out dtAccZipper);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    distinctRefno = dtAccZipper.DefaultView.ToTable(true, "Seq1", "Refno", "MtltypeId", "FabricPanelCode");
                    var sheetAccZipper = (MsExcel.Worksheet)book.Worksheets["ACC (ZIPPER)"];
                    sheetAccZipper.Range["C2"].Value = title;
                    var zipperSheetcount = 0;

                    if (distinctRefno.Rows.Count == 0)
                    {
                        sheetAccZipper.Delete();
                    }
                    else
                    {
                        // 先計算總共會產生幾個sheet
                        for (int i = 0; i < Math.Ceiling(MyUtility.Convert.GetDouble(colorwaydrs.Length) / 3); i++)
                        {
                            if (i > 0)
                            {
                                zipperSheetcount++;
                            }

                            for (int j = 0; j < distinctRefno.Rows.Count; j++)
                            {
                                if (j > 0)
                                {
                                    zipperSheetcount++;
                                }
                            }
                        }

                        // 複製Sheet
                        for (int i = zipperSheetcount; i > 0; i--)
                        {
                            int index = sheetAccZipper.Index;
                            sheetAccZipper.Copy(System.Type.Missing, sheetAccZipper);
                            MsExcel.Worksheet newWS = (MsExcel.Worksheet)book.Worksheets[index + 1];
                            newWS.Name = string.Format("ACC (ZIPPER) ({0})", i);
                        }

                        // 執行資料寫入
                        for (int i = 0; i <= zipperSheetcount; i++)
                        {
                            int index = sheetAccZipper.Index;
                            MsExcel.Worksheet currentWS = (MsExcel.Worksheet)book.Worksheets[index + i];
                            var articlePages = Math.Ceiling(Convert.ToDouble(colorwaydrs.Length) / 3);
                            var refnoPages = Math.Ceiling(Convert.ToDouble(distinctRefno.Rows.Count) / 1);
                            var nowArtStr = MyUtility.Convert.GetInt((i % articlePages) * 3); // 紀錄colorWay寫入到哪
                            if (articlePages > 1 && refnoPages > 1)
                            {
                                nowArtStr = MyUtility.Convert.GetInt(Math.Floor(MyUtility.Convert.GetDouble(i / refnoPages))) * articles; // 紀錄colorWay寫入到哪
                            }

                            var nowRefStr = MyUtility.Convert.GetInt(i % refnoPages); // 紀錄Refno寫入到哪

                            var cellRefno = currentWS.Cells[4, 2];
                            cellRefno.Value = distinctRefno.Rows[nowRefStr]["Refno"];

                            string color1 = string.Empty;
                            string color2 = string.Empty;
                            string color3 = string.Empty;
                            for (int z = 0; z < articles; z++)
                            {
                                var cell = currentWS.Cells[6 + z, 1];
                                if (colorwaydrs.Length - nowArtStr - z > 0)
                                {
                                    cell.Value = colorwaydrs[nowArtStr + z]["ArticleName"] + Environment.NewLine + "[" + colorwaydrs[nowArtStr + z]["Article"] + "]";
                                }

                                var cellColor = currentWS.Cells[6 + z, 2];
                                if (distinctRefno.Rows.Count - nowRefStr > 0 && colorwaydrs.Length - nowArtStr - z > 0)
                                {
                                    var color = dtAccZipper.Select($"(Article='{colorwaydrs[nowArtStr + z]["Article"]}' or Article='') and Refno='{distinctRefno.Rows[nowRefStr]["Refno"]}' and FabricPanelCode='{distinctRefno.Rows[nowRefStr]["FabricPanelCode"]}'").AsEnumerable().Select(row => row["ColorName"].ToString()).Select(name => name).Distinct().JoinToString(Environment.NewLine);
                                    color = color.Replace(";", Environment.NewLine);
                                    cellColor.Value = color;
                                    switch (z)
                                    {
                                        case 0:
                                            color1 = color;
                                            break;
                                        case 1:
                                            color2 = color;
                                            break;
                                        case 2:
                                            color3 = color;
                                            break;
                                    }
                                }

                                if (colorwaydrs.Length - nowArtStr - z - 1 == 0 || z == articles - 1)
                                {
                                    int beginrow = 6;
                                    int endrow = 6;
                                    if (color1 == color2 && color2 == color3 && (!color2.IsNullOrWhiteSpace() || (distinctRefno.Rows.Count - nowRefStr > 0 && distinctRefno.Rows[nowRefStr]["FabricPanelCode"].ToString().IsNullOrWhiteSpace())))
                                    {
                                        endrow = 8;
                                    }
                                    else if (color1 == color2 && !color2.IsNullOrWhiteSpace())
                                    {
                                        endrow = 7;
                                    }
                                    else if (color2 == color3 && !color2.IsNullOrWhiteSpace())
                                    {
                                        beginrow = 7;
                                        endrow = 8;
                                    }

                                    MsExcel.Range top = currentWS.Cells[beginrow, 2];
                                    MsExcel.Range bottom = currentWS.Cells[endrow, 2];
                                    MsExcel.Range all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                    all.Merge(0);
                                }
                            }
                        }
                    }
                    #endregion

                    #region ACC
                    paras = new List<SqlParameter>();
                    DataTable dtAcc;
                    sqlCmd = @"
                        Select * From
                        (
                                SELECT DISTINCT  boa.Seq1,boa.Refno,f.MtltypeId,boa.FabricPanelCode,ColorID = isnull(oc.ColorID,''),isnull(DetailColor.Name,isnull(c.Name,'')) ColorName,Article = isnull(oc.Article,''),f.picture,[Keyword]=iif(f.MtltypeId='LABEL', Keyword.value,''),[Size]=size.value FROM Order_BOA boa
                                LEFT JOIN Fabric f on boa.SCIRefno = f.SCIRefno
                                LEFT JOIN Fabric_Supp fs on fs.SCIRefno = boa.SCIRefno and fs.SuppID = boa.SuppID
                                LEFT JOIN Order_ColorCombo oc on oc.Id = boa.Id and oc.FabricPanelCode = boa.FabricPanelCode
                                LEFT JOIN Color C on OC.ColorID = C.ID and C.BrandId ='N.FACE'
                                OUTER APPLY (Select dbo.GetKeyword(boa.ID,boa.ukey,boa.Keyword,'','','',1) as [value]) Keyword
                                OUTER APPLY (
                                  SELECT
                                      NAME =
                                      STUFF((SELECT DISTINCT
                                              ';' + DetailColor.Name
                                          FROM Color_multiple cm
                                              LEFT JOIN Color DetailColor On  DetailColor.BrandID = cm.BrandID
                                              Where cm.ColorUkey = c.Ukey And DetailColor.ID = cm.ColorID
                                          FOR XML PATH (''))
                                      , 1, 1, '')
                                ) DetailColor
                                OUTER APPLY (
                                  SELECT
                                      value =
                                      STUFF((SELECT DISTINCT
                                              ';' + os.SizeSpec
                                          FROM Order_SizeSpec os
                                              Where os.Id = boa.Id and os.SizeItem = boa.SizeItem and f.MtltypeId='POLYBAG'
                                          FOR XML PATH (''))
                                      , 1, 1, '')
                                ) Size
                                WHERE boa.ID=@PoID and f.MtltypeId<>'ZIPPER' and boa.SuppID <>'FTY'
                        )a
                        UNION
                        Select * FROM
                        (
								SELECT DISTINCT Seq1= 'Z99',f.Refno,f.MtltypeId,FabricPanelCode ='',ColorID = isnull(schd.ColorID,''),isnull(DetailColor.Name,isnull(c.Name,'')) ColorName,Article =isnull(schd.Article,''),f.picture,[Keyword]='',[Size]='' FROM PO 
								LEFT Join Style_ThreadColorCombo_History sch on po.StyleUkey = sch.StyleUkey and po.ThreadVersion = sch.Version
								left join Style_ThreadColorCombo_History_Detail schd on sch.Ukey = schd.Style_ThreadColorCombo_HistoryUkey
								 LEFT JOIN Fabric f on schd.SCIRefNo = f.SCIRefno								 					
                                LEFT JOIN Color C on schd.ColorID = C.ID and C.BrandId ='N.FACE'
								  OUTER APPLY ( SELECT
										  NAME =
										  STUFF((SELECT DISTINCT
												  ';' + DetailColor.Name
											  FROM Color_multiple cm
											  LEFT JOIN Color DetailColor
												  ON DetailColor.BrandID = cm.BrandID
											  WHERE cm.ColorUkey = c.Ukey
											  AND DetailColor.ID = cm.ColorID
											  FOR XML PATH (''))
										  , 1, 1, '')
                                ) DetailColor
								WHERE po.ID=@PoID and schd.SuppId <>'FTY'
                        )b
                        ORDER BY Seq1,Refno,FabricPanelCode,ColorID
";
                    paras.Add(new SqlParameter("@PoID", this.poID));

                    result = DBProxy.Current.Select(null, sqlCmd, paras, out dtAcc);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    distinctRefno = dtAcc.DefaultView.ToTable(true, "Seq1", "Refno", "MtltypeId", "FabricPanelCode", "Keyword");
                    var sheetAcc = (MsExcel.Worksheet)book.Worksheets["ACC"];
                    sheetAcc.Range["C2"].Value = title;
                    var accSheetcount = 0;

                    if (distinctRefno.Rows.Count > 6)
                    {
                        refnocounts = 6;
                    }
                    else
                    {
                        refnocounts = distinctRefno.Rows.Count;
                    }

                    if (refnocounts == 0)
                    {
                        sheetAcc.Delete();
                    }
                    else
                    {
                        // 先計算總共會產生幾個sheet
                        for (int i = 0; i < Math.Ceiling(MyUtility.Convert.GetDouble(colorwaydrs.Length) / 3); i++)
                        {
                            if (i > 0)
                            {
                                accSheetcount++;
                            }

                            for (int j = 0; j < Math.Ceiling(MyUtility.Convert.GetDouble(distinctRefno.Rows.Count) / 6); j++)
                            {
                                if (j > 0)
                                {
                                    accSheetcount++;
                                }
                            }
                        }

                        // 複製Sheet
                        for (int i = accSheetcount; i > 0; i--)
                        {
                            int index = sheetAcc.Index;
                            sheetAcc.Copy(System.Type.Missing, sheetAcc);
                            MsExcel.Worksheet newWS = (MsExcel.Worksheet)book.Worksheets[index + 1];
                            newWS.Name = string.Format("ACC ({0})", i);
                        }

                        // 執行資料寫入
                        for (double i = 0; i <= accSheetcount; i++)
                        {
                            int index = sheetAcc.Index;
                            MsExcel.Worksheet currentWS = (MsExcel.Worksheet)book.Worksheets[index + i];
                            var articlePages = Math.Ceiling(Convert.ToDouble(colorwaydrs.Length) / 3);
                            var refnoPages = Math.Ceiling(Convert.ToDouble(distinctRefno.Rows.Count) / 6);
                            var nowArtStr = MyUtility.Convert.GetInt((i % articlePages) * 3); // 紀錄colorWay寫入到哪
                            if (articlePages > 1 && refnoPages > 1)
                            {
                                nowArtStr = MyUtility.Convert.GetInt(Math.Floor(MyUtility.Convert.GetDouble(i / refnoPages))) * articles; // 紀錄colorWay寫入到哪
                            }

                            var nowRefStr = MyUtility.Convert.GetInt((i % refnoPages) * refnocounts); // 紀錄Refno寫入到哪

                            string refno = string.Empty;
                            string mtltypeId = string.Empty;
                            for (int k = 0; k < refnocounts; k++)
                            {
                                var cellRefno = currentWS.Cells[4, 2 + k];
                                var cellSuppRefno = currentWS.Cells[5, 2 + k];
                                if (k < articles)
                                {
                                    var cell = currentWS.Cells[6 + k, 1];
                                    if (colorwaydrs.Length - nowArtStr - k > 0)
                                    {
                                        cell.Value = colorwaydrs[nowArtStr + k]["ArticleName"] + Environment.NewLine + "[" + colorwaydrs[nowArtStr + k]["Article"] + "]";
                                    }
                                }

                                if (distinctRefno.Rows.Count - nowRefStr - k > 0)
                                {
                                    cellRefno.Value = distinctRefno.Rows[nowRefStr + k]["Refno"];
                                    cellSuppRefno.Value = distinctRefno.Rows[nowRefStr + k]["MtltypeId"];
                                    mtltypeId = distinctRefno.Rows[nowRefStr + k]["MtltypeId"].ToString();
                                    if (mtltypeId.ToUpper().IsOneOfThe("LABEL", "HANGTAG"))
                                    {
                                        currentWS.GetRange($"{ExcelColumnHelper.ToExcelColumnName(2 + k)}:{ExcelColumnHelper.ToExcelColumnName(2 + k)}").ColumnWidth = 21.5;
                                    }

                                    if (refno == distinctRefno.Rows[nowRefStr + k]["Refno"].ToString())
                                    {
                                        MsExcel.Range top = currentWS.Cells[4, 2 + k - 1];
                                        MsExcel.Range bottom = currentWS.Cells[4, 2 + k];
                                        MsExcel.Range all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                        all.Merge(0);
                                        top = currentWS.Cells[5, 2 + k - 1];
                                        bottom = currentWS.Cells[5, 2 + k];
                                        all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                        all.Merge(0);
                                    }

                                    refno = distinctRefno.Rows[nowRefStr + k]["Refno"].ToString();
                                }

                                string color1 = string.Empty;
                                string color2 = string.Empty;
                                string color3 = string.Empty;

                                for (int z = 0; z < articles; z++)
                                {
                                    var cellColor = currentWS.Cells[6 + z, 2 + k];
                                    if (distinctRefno.Rows.Count - nowRefStr - k > 0 && colorwaydrs.Length - nowArtStr - z > 0 && refno != "L3NO-SMS" && !mtltypeId.ToUpper().IsOneOfThe("POLYBAG", "LABEL"))
                                    {
                                        var color = dtAcc.Select($"(Article='{colorwaydrs[nowArtStr + z]["Article"]}' or Article='' ) and Refno='{refno}' and FabricPanelCode='{distinctRefno.Rows[nowRefStr + k]["FabricPanelCode"]}'").AsEnumerable().Select(row => row["ColorName"].ToString()).Select(name => name).Distinct().JoinToString(Environment.NewLine);
                                        color = color.Replace(";", Environment.NewLine);
                                        cellColor.Value = color;
                                        switch (z)
                                        {
                                            case 0:
                                                color1 = color;
                                                break;
                                            case 1:
                                                color2 = color;
                                                break;
                                            case 2:
                                                color3 = color;
                                                break;
                                        }
                                    }

                                    if (colorwaydrs.Length - nowArtStr - z - 1 == 0 || z == articles - 1)
                                    {
                                        int beginrow = 6;
                                        int endrow = 6;
                                        if (color1 == color2 && color2 == color3 && (!color2.IsNullOrWhiteSpace() || refno == "L3NO-SMS" || mtltypeId.ToUpper().IsOneOfThe("POLYBAG", "LABEL") || (distinctRefno.Rows.Count - nowRefStr - k > 0 && distinctRefno.Rows[nowRefStr + k]["FabricPanelCode"].ToString().IsNullOrWhiteSpace())))
                                        {
                                            endrow = 8;
                                        }
                                        else if (color1 == color2 && !color2.IsNullOrWhiteSpace())
                                        {
                                            endrow = 7;
                                        }
                                        else if (color2 == color3 && !color2.IsNullOrWhiteSpace())
                                        {
                                            beginrow = 7;
                                            endrow = 8;
                                        }

                                        if ((refno == "L3NO-SMS" || mtltypeId.ToUpper().IsOneOfThe("POLYBAG", "LABEL")) && distinctRefno.Rows.Count - nowRefStr - k > 0)
                                        {
                                            var descCell = currentWS.Cells[6, 2 + k];
                                            descCell.EntireColumn.NumberFormat = "@";

                                            MsExcel.Range top = currentWS.Cells[beginrow, 2 + k];
                                            MsExcel.Range bottom = currentWS.Cells[endrow, 2 + k];
                                            MsExcel.Range all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                            all.Merge(0);

                                            // 放圖片
                                            if (refno == "L3NO-SMS" || mtltypeId.ToUpper() == "POLYBAG")
                                            {
                                                var picturedr = dtAcc.Select($"Refno='{refno}' and MtltypeId='{mtltypeId}'")[0];
                                                if (!string.IsNullOrEmpty(picturedr["picture"].ToString()))
                                                {
                                                    var imgpath = System.IO.Path.Combine(fabricPath, this.brandID, picturedr["picture"].ToString());
                                                    if (System.IO.File.Exists(imgpath))
                                                    {
                                                        using (var img = Image.FromFile(imgpath))
                                                        {
                                                            int max = img.Width > img.Height ? img.Width : img.Height;
                                                            decimal ratio = 0m;
                                                            if (max < 100)
                                                            {
                                                                ratio = 1;
                                                            }
                                                            else
                                                            {
                                                                ratio = 100m / max;
                                                            }

                                                            var width = refno == "L3NO-SMS" ? 135 : 110;
                                                            var imgWidth = (float)(img.Width * ratio);
                                                            var imgHeight = (float)(img.Height * ratio);

                                                            // 置中距離計算
                                                            float leftDiff = (width - imgWidth) / 2;

                                                            currentWS.Shapes.AddPicture(imgpath, MsoTriState.msoFalse, MsoTriState.msoTrue, descCell.Left + leftDiff, descCell.Top + 15, imgWidth, imgHeight);
                                                        }
                                                    }
                                                }
                                            }

                                            if (distinctRefno.Rows.Count - nowRefStr - k > 0 && colorwaydrs.Length - nowArtStr - z > 0 && mtltypeId.ToUpper() == "LABEL")
                                            {
                                                var keyword = dtAcc.AsEnumerable().Where(r => (r["Article"].ToString() == colorwaydrs[nowArtStr + z]["Article"].ToString() || r["Article"].ToString() == string.Empty) && r["Refno"].ToString() == refno && r["FabricPanelCode"].ToString() == distinctRefno.Rows[nowRefStr + k]["FabricPanelCode"].ToString() && r["Keyword"].ToString() == distinctRefno.Rows[nowRefStr + k]["Keyword"].ToString()).Select(row => row["Keyword"].ToString()).FirstOrDefault();
                                                descCell.Value = keyword;
                                            }

                                            if (distinctRefno.Rows.Count - nowRefStr - k > 0 && colorwaydrs.Length - nowArtStr - z > 0 && mtltypeId.ToUpper() == "POLYBAG")
                                            {
                                                var size = dtAcc.Select($"(Article='{colorwaydrs[nowArtStr + z]["Article"]}' or Article='') and Refno='{refno}' and FabricPanelCode='{distinctRefno.Rows[nowRefStr + k]["FabricPanelCode"]}'").AsEnumerable().Select(row => row["Size"].ToString()).Select(name => name).Distinct().JoinToString(Environment.NewLine);
                                                size = size.Replace(";", Environment.NewLine);
                                                descCell.Value = size;
                                            }
                                        }
                                        else
                                        {
                                            MsExcel.Range top = currentWS.Cells[beginrow, 2 + k];
                                            MsExcel.Range bottom = currentWS.Cells[endrow, 2 + k];
                                            MsExcel.Range all = (MsExcel.Range)currentWS.get_Range(top, bottom);
                                            all.Merge(0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    fullSavePath = System.IO.Path.Combine(tempDir, fileName) + ".xlsx";
                    if (System.IO.File.Exists(fullSavePath))
                    {
                        try
                        {
                            System.IO.File.Delete(fullSavePath);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    while (System.IO.File.Exists(fullSavePath))
                    {
                        fullSavePath = System.IO.Path.Combine(tempDir, fileName) + $".xlsx";
                    }

                    book.SaveAs(Filename: fullSavePath);
                    book.Close();
                    result = Result.True;
                }
                finally
                {
                    app.Workbooks.Close();
                    app.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                    if (result == true)
                    {
                        if (File.Exists(fullSavePath))
                        {
                            System.Diagnostics.Process pro = new System.Diagnostics.Process();
                            pro.StartInfo.FileName = fullSavePath;
                            pro.Start();
                        }
                    }
                }
            }
        }

        string exportAccessoryCar(DataTable dt, string str, string brk, int newPageCnt, string FabricType)
        {
            List<string> tmprefno = new List<string>();
            List<string> tmprefnoColor = new List<string>();

            int cnt = 1;
            DataRow[] rows = dt.Select(string.Format("FabricType = '{0}'", FabricType));
            foreach (DataRow row in rows)
            {
                string refno = row["RefNo"].ToString().Trim();
                string des = row["DescDetail"].ToString().Trim();
                string ColorDesc = row["ColorDesc"].ToString().Trim();
                string colorDetail = row["ColorDetail"].ToString().Trim();
                string str1 = "＊ " + refno;
                string str2 = des + Environment.NewLine;
                string str3 = ColorDesc + "  " + colorDetail;

                if (tmprefno.Contains(refno + des))
                {
                    str2 = string.Empty;
                }

                if (tmprefnoColor.Contains(refno + ColorDesc + colorDetail))
                {
                    str3 = string.Empty;
                }

                if (str2 == string.Empty && str3 == string.Empty)
                {
                    continue;
                }

                str += string.Format("{0}  {1}{2}", str1, str2, str3);

                if (cnt != 0 && (FabricType == "F" || cnt != rows.Length) && cnt % newPageCnt == 0)
                {
                    str += brk;
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        str += Environment.NewLine;
                    }
                }

                cnt += 1;
                tmprefno.Add(refno + des);
                tmprefnoColor.Add(refno + ColorDesc);
            }

            return str;
        }

        public static DualResult GetAccessoryCard(DataTable selArticle, string poID, out DataTable tmpRefNo)
        {
            tmpRefNo = null;
            string sqlCmd = string.Empty;

            sqlCmd = $@"	
Declare @tmpRefNo Table
		(  RowID BigInt Identity(1,1) Not Null, Article VarChar(8), SciRefNo VarChar(30), RefNo VarChar(36)
		 , ColorDetail VarChar(100), FabricType VarChar(1), DescDetail NVarCHar(Max)
		 , ColorID VarChar(6), NeedExpend Bit, ColorDesc NVarCHar(Max), SortField VarChar(3) 
		);
	----------------------------------------------------------------------
	--抓取母單的資料
	--1. Fabric
	Insert Into @tmpRefNo
		(Article, SciRefNo, RefNo, ColorDetail, FabricType, DescDetail, ColorID, NeedExpend, ColorDesc, SortField)
		Select SelArticle.Article, Order_BOF.SCIRefNo, Order_BOF.RefNo
			 , Order_BOF.ColorDetail, Fabric.Type, Fabric.DescDetail
			 , Order_ColorCombo.ColorID, 1 as NeedExpend
			 , RTrim(Order_ColorCombo.ColorID) + ' ' + RTrim(Color.Name) as ColorDesc
			 , Order_BOF.FabricCode
		  From dbo.Order_BOF
		  Left Join dbo.Orders
			On Orders.ID = Order_BOF.ID
		 Cross Apply (Select Article From #tmp Where #tmp.Sel = 1) SelArticle
		  Left Join dbo.Order_FabricCode
			On     Order_FabricCode.ID = Order_BOF.ID
			   And Order_FabricCode.FabricCode = Order_BOF.FabricCode
		  Left Join dbo.Order_ColorCombo
			On	   Order_ColorCombo.ID = Order_BOF.ID
			   And Order_ColorCombo.FabricPanelCode = Order_FabricCode.FabricPanelCode
			   And Order_ColorCombo.Article = SelArticle.Article
		  Left Join dbo.Fabric
			On Fabric.SCIRefNo = Order_BOF.SCIRefNo
		  Left Join dbo.Color
			On	   Color.BrandID = Orders.BrandID
			   And Color.ID = Order_ColorCombo.ColorID
		 Where Order_BOF.ID = '{poID}'
		   And Order_BOF.SuppID != 'FTY'
		   And IsNull(Order_ColorCombo.ColorID,'') != ''
		 Order by Order_BOF.FabricCode, SelArticle.Article;
	
	--2. Accessory
	Insert Into @tmpRefNo
		(Article, SciRefNo, RefNo, ColorDetail, FabricType, DescDetail, ColorID, NeedExpend, ColorDesc, SortField)
		Select SelArticle.Article, Order_BOA.SCIRefNo, Order_BOA.RefNo
			 , Order_BOA.ColorDetail, Fabric.Type, Fabric.DescDetail
			 , isnull(Order_ColorCombo.ColorID,'') ColorID
			 , IIF(Order_BOA.BomTypeColor = 1 Or Order_BOA.BomTypeSize = 1, 1, 0) as NeedExpend
			 , isnull(RTrim(Order_ColorCombo.ColorID) + ' ' + RTrim(Color.Name),'') as ColorDesc
			 , Order_BOA.Seq1
		  From dbo.Order_BOA
		  Left Join dbo.Orders
			On Orders.ID = Order_BOA.ID
		 Cross Apply (Select Article From #tmp Where #tmp.Sel = 1) SelArticle
		  Left Join dbo.Order_ColorCombo
			On	   Order_ColorCombo.ID = Order_BOA.ID
			   And Order_ColorCombo.FabricPanelCode = Order_BOA.FabricPanelCode
			   And Order_ColorCombo.Article = SelArticle.Article
			   And Order_BOA.BomTypeColor = 1 --Edward : 有勾選依照Color展開，才需要join
		  Left Join dbo.Fabric
			On Fabric.SCIRefNo = Order_BOA.SCIRefNo
		  Left Join dbo.Color
			On	   Color.BrandID = Orders.BrandID
			   And Color.ID = Order_ColorCombo.ColorID
		 Where Order_BOA.ID = '{poID}'
		   And Order_BOA.SuppID != 'FTY'
		   --And IsNull(Order_ColorCombo.ColorID,'') != ''
		 group by SelArticle.Article, Order_BOA.SCIRefNo, Order_BOA.RefNo
			 , Order_BOA.ColorDetail, Fabric.Type, Fabric.DescDetail
			 , Order_ColorCombo.ColorID,Order_BOA.BomTypeColor,Order_BOA.BomTypeSize
			 , Order_ColorCombo.ColorID,Color.Name
			 , Order_BOA.Seq1
		 Order by Order_BOA.Seq1, SelArticle.Article;

	Select * From @tmpRefNo;";

            var result = MyUtility.Tool.ProcessWithDatatable(selArticle, string.Empty, sqlCmd, out tmpRefNo, "#tmp");

            return result;
        }
    }

    public static class ExcelColumnHelper
    {
        public static string ToExcelColumnName(this int sender)
        {
            if (sender <= 26)
            {
                return ((char)(64 + sender)).ToString() ?? string.Empty;
            }

            int num = (sender - 1) / 26;
            int num2 = ((sender - 1) % 26) + 1;
            return ((char)(num + 64)).ToString() + (char)(num2 + 64);
        }
    }
}