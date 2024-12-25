using Ict;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.QrCode.Internal;
using ZXing.QrCode;
using ZXing;
using static Sci.Production.Automation.Guozi_AGV;
using static Sci.Production.PublicPrg.Prgs;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P10_Print : Win.Tems.PrintForm
    {
        private readonly DataRow CurrentDataRow;
        private DualResult result;
        private int strPagetype;

        /// <inheritdoc/>
        public P10_Print(DataRow row)
        {
            this.InitializeComponent();
            this.CurrentDataRow = row;
            this.toexcel.Enabled = false;
            this.comboBoxSetting.DataSource = Enum.GetValues(typeof(Prg.BundleRFCard.BundleType));
            this.linkLabelRFCardEraseBeforePrinting1.SetText();
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
            this.result = new DualResult(true);
            if (this.radioBundleCard.Checked || this.radioBundleCardRF.Checked || this.radioBundlewithQR.Checked)
            {
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", SqlDbType.VarChar, 13) { Value = this.CurrentDataRow["ID"].ToString() },
                    new SqlParameter("@POID", SqlDbType.VarChar, 13) { Value = this.CurrentDataRow["POID"].ToString() },
                    new SqlParameter("@CutRef", SqlDbType.VarChar, 8) { Value = this.CurrentDataRow["cutref"].ToString() },
                };

                string columns = $@"
    outer apply( select a.PatternCode) pc
    outer apply( select a.PatternDesc) pd
    outer apply( select a.Parts) pp
";
                if (this.checkExtendAllParts.Checked)
                {
                    columns = @"
    left join dbo.Bundle_Detail_Allpart bdap WITH (NOLOCK) on bdap.id=a.Id and a.Patterncode = 'ALLPARTS'
    outer apply( select PatternCode = iif(a.PatternCode = 'ALLPARTS',bdap.PatternCode,a.PatternCode)) pc
    outer apply( select PatternDesc = iif(a.PatternCode = 'ALLPARTS',bdap.PatternDesc,a.PatternDesc)) pd
    outer apply( select Parts = iif(a.PatternCode = 'ALLPARTS',bdap.Parts,a.Parts)) pp
";
                }

                string scmd = $@"
select  *
from (
    select
        [MarkerNo] = iif(@CutRef <>'',(select top 1 MarkerNo from WorkOrder where  CutRef=@CutRef and id = @POID),'')
		,[Group_right] = a.BundleGroup
        ,a.Tone
        ,[Barcode] = a.BundleNo
        ,[Size] = a.SizeCode
        ,[Quantity] = a.Qty
	    ,SP = dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID for XML RAW))
        ,[Desc] = CONCAT('(' + pc.PatternCode + ')', pd.PatternDesc)
	    ,pp.Parts
        ,NoBundleCardAfterSubprocess = (
			select top 1 N'X'
			from Bundle_Detail_Art bda with(nolock)
			where bda.Bundleno = a.Bundleno and bda.NoBundleCardAfterSubprocess = 1)
		
        ,b.POID
        ,b.Article
        ,[Color] = CONCAT(b.Article, '\' + b.Colorid)
        ,[Line] = b.Sewinglineid
        ,[Cell] = b.SewingCell
        ,b.FabricPanelCode
        ,b.item
        ,b.IsEXCESS
        ,[Body_Cut] = concat(isnull(b.PatternPanel,''),'-',b.FabricPanelCode ,'-',convert(varchar,b.Cutno))
        ,[Artwork]= iif( len(Artwork.Artwork )>43,substring(Artwork.Artwork ,0,43),Artwork.Artwork )

	    ,[Group_left] = c.FactoryID
        ,[Style] = c.StyleID
        ,SeasonID = concat(c.SeasonID,' ' + c.dest)
        ,brand = c.brandid
        ,brand.ShipCode
        , a.RFPrintDate
        , [BundleID] = b.ID
        , a.BundleNo
        , b.SubCutNo
        , a.RFIDScan
        , CutCell = (select top 1 CutCellID from workorder w where w.cutref = b.cutref)
        , a.Dyelot
        , a.ID
        , a.PatternDesc
    from dbo.Bundle_Detail a WITH (NOLOCK) 
    inner join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
    inner join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
    left join brand WITH (NOLOCK) on brand.id = c.brandid
    {columns}
    outer apply(
	    select Artwork = STUFF((
		    select iif(e1.SubprocessId is null or e1.SubprocessId='','','+'+e1.SubprocessId + iif(e1.PostSewingSubProcess = 1, '(S)', ''))
		    from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
			Left join SubProcessSeq_Detail sd With(nolock) on sd.StyleUkey = c.StyleUkey and sd.SubProcessID = e1.SubprocessId
			Left join SubProcess s With(nolock) on s.Id = e1.SubprocessId
		    where e1.id=b.id and e1.PatternCode= pc.PatternCode and e1.Bundleno=a.BundleNo
            order by Isnull(sd.Seq, s.Seq), IsNull(sd.SubProcessID, s.Id)
		    for xml path('')
	    ),1,1,'')
    )as Artwork

    where a.ID= @ID 
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
order by x.[Barcode]
";
                this.result = DBProxy.Current.Select(string.Empty, scmd, pars, out this.dt);

                if (!this.result)
                {
                    return this.result;
                }
            }
            else if (this.radioBundleChecklist.Checked)
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

select distinct [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty],[Dyelot]
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
        ,a.Dyelot
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	left join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
	outer apply( select a.PatternCode [Cutpart] )[qq]
	outer apply(select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
														from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
														where e1.Bundleno=a.BundleNo
                                                        order by Ukey
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
        ,a.Dyelot
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

select distinct [Group],[Bundle],[Size],[Cutpart],[Description],[SubProcess],[Parts],[Qty],[Dyelot]
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
            ,a.Dyelot
	from dbo.Bundle_Detail a WITH (NOLOCK) 
	left join dbo.Bundle b WITH (NOLOCK) on a.id=b.id
    outer apply(select top 1 OrderID from Bundle_Detail_Order where BundleNo = a.BundleNo order by OrderID)bdo
	left join dbo.orders c WITH (NOLOCK) on c.id=bdo.Orderid and c.MDivisionID = b.MDivisionID 
	outer apply ( select a.PatternCode [Cutpart] ) [qq]
	outer apply ( select Artwork = (select iif(e1.SubprocessId is null or e1.SubprocessId='','',e1.SubprocessId+'+')
															from dbo.Bundle_Detail_Art e1 WITH (NOLOCK) 
															where e1.Bundleno=a.BundleNo
                                                            order by Ukey
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
            ,a.Dyelot
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
            this.WritePrintDate();
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
                    CutRef = this.CurrentDataRow["CutRef"].ToString(),
                    Tone = MyUtility.Convert.GetString(row1["Tone"]),
                    Line = row1["Line"].ToString(),
                    Cell = row1["Cell"].ToString(),
                    POID = row1["POID"].ToString(),
                    SP = row1["SP"].ToString(),
                    Style = row1["Style"].ToString(),
                    MarkerNo = row1["MarkerNo"].ToString(),
                    Body_Cut = row1["Body_Cut"].ToString() + (MyUtility.Check.Empty(row1["SubCutNo"]) ? string.Empty : $"-{row1["SubCutNo"]}"),
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
                    BundleID = row1["BundleID"].ToString(),
                    RFIDScan = MyUtility.Convert.GetBool(row1["RFIDScan"]),
                    CutCell = row1["CutCell"].ToString(),
                    Dyelot = row1["Dyelot"].ToString(),
                }).ToList();
                string fileName = "Cutting_P10_Layout1";
                Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
                Excel.Workbook workbook = excelApp.ActiveWorkbook;
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                this.strPagetype = 9;
                RunPagePrint(data, excelApp, this.strPagetype);
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
                this.WritePrintDate();
            }
            else if (this.radioBundleChecklist.Checked)
            {
                this.ExcelProcess(true);
                this.WritePrintDate();
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
                    this.print.Enabled = false;
                    bool rfCardErase = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select RFCardEraseBeforePrinting from [System]"));
                    DataTable dataTable = this.dt;
                    var query = this.dt.AsEnumerable()
                                  .GroupBy(x => x.Field<DateTime?>("RFPrintDate").HasValue ?
                                                  "Y" : string.Empty)
                                  .Select(x => new
                                  {
                                      RFPrintDate = x.Key,
                                      Count = x.Count(),
                                  })
                                  .ToList();
                    if (query.Any() && query.Count > 1)
                    {
                        DialogResult confirmResult = Prg.MessageBoxEX.Show(
                                     "The last printing has not yet completed, do you want to continue printing?",
                                     "Continue Printing?",
                                     MessageBoxButtons.YesNoCancel,
                                     new string[] { "Continue", "Restart Printing", "Cancel" });
                        if (confirmResult.EqualString("Yes"))
                        {
                            dataTable = this.dt.AsEnumerable().Where(x => !x.Field<DateTime?>("RFPrintDate").HasValue).CopyToDataTable();
                        }
                        else if (confirmResult.EqualString("No"))
                        {
                            dataTable = this.dt;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    DataTable allNoDatas = null;
                    dataTable.AsEnumerable().Select(dr => new P10_PrintData()
                    {
                        POID = dr["POID"].ToString(),
                        FabricPanelCode = dr["FabricPanelCode"].ToString(),
                        Article = dr["Article"].ToString(),
                        Size = dr["Size"].ToString(),
                    })
                    .Select(s => new { s.POID, s.FabricPanelCode, s.Article, s.Size }).Distinct().ToList().ForEach(r =>
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

                    List<P10_PrintData> data = dataTable.AsEnumerable().Select(dr => new P10_PrintData()
                    {
                        Group_right = dr["Group_right"].ToString(),
                        Group_left = dr["Group_left"].ToString(),
                        CutRef = this.CurrentDataRow["CutRef"].ToString(),
                        Tone = dr["Tone"].ToString(),
                        Line = dr["Line"].ToString(),
                        Cell = dr["Cell"].ToString(),
                        POID = dr["POID"].ToString(),
                        SP = dr["SP"].ToString(),
                        Style = dr["Style"].ToString(),
                        MarkerNo = dr["MarkerNo"].ToString(),
                        Body_Cut = dr["Body_Cut"].ToString() + (MyUtility.Check.Empty(dr["SubCutNo"]) ? string.Empty : $"-{dr["SubCutNo"]}"),
                        Parts = dr["Parts"].ToString(),
                        Color = dr["Color"].ToString(),
                        Article = dr["Article"].ToString(),
                        Size = dr["Size"].ToString(),
                        SizeSpec = MyUtility.Check.Empty(dr["SizeSpec"].ToString()) ? string.Empty : "(" + dr["SizeSpec"].ToString() + ")",
                        Desc = dr["Desc"].ToString(),
                        Artwork = dr["Artwork"].ToString(),
                        Quantity = dr["Quantity"].ToString(),
                        Barcode = dr["Barcode"].ToString(),
                        Season = dr["Seasonid"].ToString(),
                        Brand = dr["brand"].ToString(),
                        Item = dr["item"].ToString(),
                        EXCESS1 = MyUtility.Convert.GetBool(dr["isEXCESS"]) ? "EXCESS" : string.Empty,
                        NoBundleCardAfterSubprocess1 = dr["NoBundleCardAfterSubprocess"].ToString().Empty() ? string.Empty : "X",
                        Replacement1 = string.Empty,
                        ShipCode = dr["ShipCode"].ToString(),
                        FabricPanelCode = dr["FabricPanelCode"].ToString(),
                        No = GetNo(dr["Barcode"].ToString(), allNoDatas),
                        BundleID = dr["BundleID"].ToString(),
                        BundleNo = dr["BundleNo"].ToString(),
                        RFIDScan = MyUtility.Convert.GetBool(dr["RFIDScan"]),
                        CutCell = dr["CutCell"].ToString(),
                    }).ToList();

                    this.ShowWaitMessage("Process Print!");
                    DualResult result = Prg.BundleRFCard.BundleRFCardPrintAndRetry(this, data, 0, rfCardErase);
                    if (!result)
                    {
                        this.HideWaitMessage();
                        MyUtility.Msg.ErrorBox(result.ToString());
                        return false;
                    }

                    #region sent data to GZ WebAPI
                    DataView dataView = dataTable.DefaultView;
                    DataTable insert_BundleNo = dataView.ToTable(true, "BundleNo");

                    List<string> listBundleNo = insert_BundleNo.AsEnumerable().Select(s => s["BundleNo"].ToString()).ToList();

                    Task.Run(() => new Guozi_AGV().SentBundleToAGV(listBundleNo))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                    #endregion

                    this.HideWaitMessage();
                    MyUtility.Msg.InfoBox("Printed success, Please check result in Bin Box.");
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox(ex.ToString());
                    return false;
                }
                finally
                {
                    this.print.Enabled = true;
                }
            }
            else if (this.radioBundleErase.Checked)
            {
                this.ShowWaitMessage("Process Erase!");

                // 放在Stacker的所有卡片擦除
                this.print.Enabled = false;
                DualResult result = Prg.BundleRFCard.BundleRFErase();
                this.print.Enabled = true;
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    this.HideWaitMessage();
                    return false;
                }

                this.HideWaitMessage();
                MyUtility.Msg.InfoBox("Erase success, Please check result in Bin Box.");
            }
            else if (this.radioBundlewithQR.Checked)
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
                    CutRef = this.CurrentDataRow["CutRef"].ToString(),
                    Tone = MyUtility.Convert.GetString(row1["Tone"]),
                    Line = row1["Line"].ToString(),
                    Cell = row1["Cell"].ToString(),
                    POID = row1["POID"].ToString(),
                    SP = row1["SP"].ToString(),
                    Style = row1["Style"].ToString(),
                    MarkerNo = row1["MarkerNo"].ToString(),
                    Body_Cut = row1["Body_Cut"].ToString() + (MyUtility.Check.Empty(row1["SubCutNo"]) ? string.Empty : $"-{row1["SubCutNo"]}"),
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
                    BundleID = row1["BundleID"].ToString(),
                    RFIDScan = MyUtility.Convert.GetBool(row1["RFIDScan"]),
                    CutCell = row1["CutCell"].ToString(),
                    Dyelot = row1["Dyelot"].ToString(),
                    ID = row1["ID"].ToString(),
                    BundleNo = row1["BundleNo"].ToString(),
                    PatternDesc = row1["PatternDesc"].ToString(),
                }).ToList();
                string fileName = "Cutting_P10_Layout2";
                Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{fileName}.xltx");
                Excel.Workbook workbook = excelApp.ActiveWorkbook;
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                this.strPagetype = 2;
                RunPagePrint(data, excelApp, this.strPagetype);
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
                this.WritePrintDate();
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

        /// strPagetype = 2 分割2格 ; strPagetype = 9 分割9格 <inheritdoc/>
        internal static void RunPagePrint(List<P10_PrintData> data, Excel.Application excelApp, int strPagetype)
        {
            // 範本預設 A4 紙, 分割 9 格貼紙格式, 因印表機邊界, 9 格格式有點不同
            int page = ((data.Count - 1) / strPagetype) + 1;
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
                var writedata = data.Skip((pi - 1) * strPagetype).Take(strPagetype).ToList();
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[pi];
                ProcessPrint(writedata, worksheet, allNoDatas, strPagetype);
            }
        }

        /// <inheritdoc/>
        internal static void ProcessPrint(List<P10_PrintData> data, Excel.Worksheet worksheet, DataTable allNoDatas, int strPagetype)
        {
            int i = 0;
            int col_ref = 0;
            int row_ref = 0;

            if (strPagetype == 9)
            {
                data.ForEach(r =>
                {
                    // 有改格式的話要連 Sci.Production.Prg.BundelRFCard  GetSettingText() 一併修改。
                    string no = GetNo(r.Barcode, allNoDatas);
                    string contian;
                    contian = $@"Grp: {r.Group_right}  Tone: {r.Tone}  Line#: {r.Line}  {r.Group_left}
SP#:{r.SP}
Style#: {r.Style} Cell: {r.CutCell}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}     Dyelot: {r.Dyelot}
Sea: {r.Season}     Brand: {r.ShipCode}
MK#: {r.MarkerNo}     Cut/L:
Sub Process: {r.Artwork}
Desc: {r.Desc}
Qty: {r.Quantity}(#{no})  Item: {r.Item}";

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
            else if (strPagetype == 2)
            {
                /*
                 Level L (Low)      7%  of codewords can be restored.
                 Level M (Medium)   15% of codewords can be restored.
                 Level Q (Quartile) 25% of codewords can be restored.
                 Level H (High)     30% of codewords can be restored.
                */
                BarcodeWriter writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new QrCodeEncodingOptions
                    {
                        // Create Photo
                        Height = 120,
                        Width = 120,
                        Margin = 0,
                        CharacterSet = "UTF-8",
                        PureBarcode = true,

                        // 錯誤修正容量
                        // L水平    7%的字碼可被修正
                        // M水平    15%的字碼可被修正
                        // Q水平    25%的字碼可被修正
                        // H水平    30%的字碼可被修正
                        ErrorCorrection = ErrorCorrectionLevel.L,
                    },
                };

                data.ForEach(r =>
                {
                    string no = GetNo(r.Barcode, allNoDatas);
                    string contian;
                    contian = $@"Grp: {r.Group_right}  Tone: {r.Tone}  Line#: {r.Line}  {r.Group_left}
SP#:{r.SP}
Style#: {r.Style} Cell: {r.CutCell}
Cut#: {r.Body_Cut}
Color: {r.Color}
Size: {r.Size}     Part: {r.Parts}     Dyelot: {r.Dyelot}
Sea: {r.Season}     Brand: {r.ShipCode}
MK#: {r.MarkerNo}     Cut/L:
Sub Process: {r.Artwork}
Desc: {r.Desc}
Qty: {r.Quantity}(#{no})  Item: {r.Item}";

                    string strQRCodeInfor;
                    strQRCodeInfor = $@"ID: {r.ID} 
Bundle No: {r.BundleNo}
Cutpart Name: {r.PatternDesc}";

                    row_ref = i / 2;
                    row_ref = (row_ref * 8) - (row_ref / 2);
                    col_ref = (i % 2) * 6;
                    int cutindex = contian.IndexOf("Cut/L");
                    worksheet.Cells[1 + row_ref, 1 + col_ref] = "○";
                    worksheet.Cells[2 + row_ref, 3 + col_ref] = contian;
                    worksheet.Cells[2 + row_ref, 3 + col_ref].Characters(1, 8).Font.Bold = true; // 部分粗體
                    worksheet.Cells[2 + row_ref, 3 + col_ref].Characters(cutindex, 6).Font.Bold = true; // 部分粗體
                    worksheet.Cells[3 + row_ref, 3 + col_ref] = r.EXCESS1;
                    worksheet.Cells[3 + row_ref, 4 + col_ref] = r.NoBundleCardAfterSubprocess1;
                    worksheet.Cells[4 + row_ref, 3 + col_ref] = "*" + r.Barcode + "*";
                    worksheet.Cells[7 + row_ref, 2 + col_ref] = strQRCodeInfor;

                    Bitmap newQRCode = writer.Write(r.BundleNo.ToString().Trim());

                    // 將圖片存儲為暫時檔案
                    string tempFilePath = System.IO.Path.GetTempFileName();
                    newQRCode.Save(tempFilePath);
                    Excel.Range rng = worksheet.Cells[7 + row_ref, 4 + col_ref];

                    // 將圖片插入到工作表中的指定位置
                    Excel.Shape pictureShape = worksheet.Shapes.AddPicture(
                        tempFilePath,
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoCTrue,
                        (float)rng.Left,
                        (float)rng.Top,
                        (float)rng.Height,
                        (float)rng.Height);

                    // 刪除暫時檔案
                    System.IO.File.Delete(tempFilePath);

                    // 邊框 」貼紙裁線
                    if (i % 2 != 2 && (i / 2) % 2 != 2)
                    {
                        string colN = MyUtility.Excel.ConvertNumericToExcelColumn(5 + col_ref);
                        Excel.Range excelRange = worksheet.get_Range($"{colN}{8 + row_ref}");
                        excelRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = 1;
                        excelRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = 1;
                    }

                    i++;
                });
            }
        }

        private void P10_Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
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

        private void RadioBundleCard_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtionChangeStatus();
        }

        private void RadioBundleCardRF_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtionChangeStatus();
        }

        private void RadioBundleChecklist_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtionChangeStatus();
        }

        private void RadioBundleErase_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtionChangeStatus();
        }

        private void RadioBundlewithQR_CheckedChanged(object sender, EventArgs e)
        {
            this.RadioButtionChangeStatus();
        }

        private void RadioButtionChangeStatus()
        {
            this.toexcel.Enabled = true;
            this.print.Enabled = true;
            if (this.radioBundleCard.Checked || this.radioBundlewithQR.Checked)
            {
                this.toexcel.Enabled = false;
            }
            else if (this.radioBundleChecklist.Checked)
            {
            }
            else if (this.radioBundleCardRF.Checked || this.radioBundleErase.Checked)
            {
                this.toexcel.Enabled = false;
            }
        }

        private void WritePrintDate()
        {
            string dtn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string sqlcmd = $@"update Bundle set PrintDate = '{dtn}' where ID = '{this.CurrentDataRow["ID"]}';
                  update Bundle_Detail set PrintDate = '{dtn}' where ID = '{this.CurrentDataRow["ID"]}';";
            DBProxy.Current.Execute(null, sqlcmd);
        }
    }
}
