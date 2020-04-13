using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        string season, mdivision, orderby, spno1, spno2, fabrictype, refno1, refno2, style, country, supp, factory, wkNo1, wkNo2 ,brand;
        string IncludeJunk, ExcludeMaterial;
        private string zone;
        DateTime? sciDelivery1, sciDelivery2, suppDelivery1, suppDelivery2, eta1, eta2, ata1, ata2;
        DataTable printData;

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(comboOrderBy, 1, 1, "Supplier,SP#");
            comboOrderBy.SelectedIndex = 0;


            DataTable zone;
            string strSelectSql = @"select '' as Zone,'' as Fty union all
select distinct f.Zone,f.Zone+' - '+(select CONCAT(ID,'/') from Factory WITH (NOLOCK) where Zone = f.Zone for XML path('')) as Fty
from Factory f WITH (NOLOCK) where Zone <> ''";

            DBProxy.Current.Select(null, strSelectSql, out zone);
            MyUtility.Tool.SetupCombox(this.comboZone, 2, zone);
            this.comboZone.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateSCIDelivery.Value1) && MyUtility.Check.Empty(dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(dateSuppDelivery.Value1) && MyUtility.Check.Empty(dateSuppDelivery.Value2) &&
                MyUtility.Check.Empty(dateETA.Value1) && MyUtility.Check.Empty(dateFinalETA.Value2) &&
                MyUtility.Check.Empty(dateFinalETA.Value1) && MyUtility.Check.Empty(dateETA.Value2) &&
                (MyUtility.Check.Empty(txtSPNoStart.Text) && MyUtility.Check.Empty(txtSPNoEnd.Text)) &&
                (MyUtility.Check.Empty(txtRefnoStart.Text) && MyUtility.Check.Empty(txtRefnoEnd.Text)) &&
                (MyUtility.Check.Empty(txtWKNo1.Text) && MyUtility.Check.Empty(txtWKNo2.Text)))
            {
                MyUtility.Msg.WarningBox("< Supp Delivery > & < SCI Delivery > & < ETA > & < FinalETA >& < SP# > & < Refno > & < Wk# > can't be empty!!");
                return false;
            }
            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateSCIDelivery.Value1;
            sciDelivery2 = dateSCIDelivery.Value2;
            suppDelivery1 = dateSuppDelivery.Value1;
            suppDelivery2 = dateSuppDelivery.Value2;
            eta1 = dateETA.Value1;
            eta2 = dateETA.Value2;
            ata1 = dateFinalETA.Value1;
            ata2 = dateFinalETA.Value2;
            spno1 = txtSPNoStart.Text;
            spno2 = txtSPNoEnd.Text;
            refno1 = txtRefnoStart.Text;
            refno2 = txtRefnoEnd.Text;
            wkNo1 = txtWKNo1.Text;
            wkNo2 = txtWKNo2.Text;
            #endregion

            country = txtcountry.TextBox1.Text;
            supp = txtsupplier.TextBox1.Text;
            style = txtstyle.Text;
            season = txtseason.Text;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            fabrictype = comboFabricType.SelectedValue.ToString();
            orderby = comboOrderBy.Text;
            brand = txtbrand.Text;
            this.zone = MyUtility.Convert.GetString(this.comboZone.SelectedValue);

            if (this.chkIncludeJunk.Checked)
            {
                IncludeJunk = Environment.NewLine;
            }
            else
            {
                IncludeJunk = " AND PSD.Junk=0 ";
            }

            if (this.chkExcludeMaterial.Checked)
            {
                ExcludeMaterial = " AND o.Category <> 'M' ";
            }
            else
            {
                ExcludeMaterial = Environment.NewLine;
            }

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlSeparateByWK = string.Empty;

            #region Separate By WK
            string sqlColSeparateByWK = string.Empty;
            string sqlJoinSeparateByWK = string.Empty;
            if (this.chkSeparateByWK.Checked)
            {
                sqlColSeparateByWK = @"
        ,[WKNo] = exd.ID
		,[WKETA] = ex.Eta
        ,[WKArriveW/HDate] = ex.WhseArrival
		,[WKShipQty] = exd.Qty
		,[WKFoc] = exd.Foc
";
                sqlJoinSeparateByWK = @"
left join Export_Detail exd with (nolock) on exd.POID = psd.id and exd.Seq1 = psd.SEQ1 and exd.Seq2 = psd.SEQ2
left join Export ex with (nolock) on ex.ID = exd.ID
";
            }
            #endregion
            #region -- sql parameters declare --
            System.Data.SqlClient.SqlParameter sp_spno1 = new System.Data.SqlClient.SqlParameter();
            sp_spno1.ParameterName = "@spno1";

            System.Data.SqlClient.SqlParameter sp_spno2 = new System.Data.SqlClient.SqlParameter();
            sp_spno2.ParameterName = "@spno2";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            System.Data.SqlClient.SqlParameter sp_season = new System.Data.SqlClient.SqlParameter();
            sp_season.ParameterName = "@season";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@FactoryID";
            
            System.Data.SqlClient.SqlParameter sp_brand = new System.Data.SqlClient.SqlParameter();
            sp_brand.ParameterName = "@BrandID";

            System.Data.SqlClient.SqlParameter sp_refno1 = new System.Data.SqlClient.SqlParameter();
            sp_refno1.ParameterName = "@refno1";

            System.Data.SqlClient.SqlParameter sp_refno2 = new System.Data.SqlClient.SqlParameter();
            sp_refno2.ParameterName = "@refno2";

            System.Data.SqlClient.SqlParameter sp_wkno1 = new System.Data.SqlClient.SqlParameter();
            sp_wkno1.ParameterName = "@wkno1";

            System.Data.SqlClient.SqlParameter sp_wkno2 = new System.Data.SqlClient.SqlParameter();
            sp_wkno2.ParameterName = "@wkno2";

            System.Data.SqlClient.SqlParameter sp_zone = new System.Data.SqlClient.SqlParameter();
            sp_zone.ParameterName = "@zone";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format($@"
select  F.MDivisionID
        ,O.FactoryID
        ,[Wkno] = wk.wkno
        ,PS.id
        ,style = si.StyleID
		,o.BrandID
        ,PSD.FinalETD
		,[ActETA]=PSD.FinalETA
		,[Sup Delivery Rvsd ETA]=PSD.RevisedETA
		,[Category]=o.Category
        ,supp = concat(PS.suppid,'-',S.NameEN )
        ,S.CountryID
        ,PSD.Refno
        ,PSD.SEQ1
        ,PSD.SEQ2
        ,fabrictype = case PSD.fabrictype 
                        when 'F' then 'Fabric'
                        when 'A' then 'Accessory'
                        when 'O' then 'Other'
                      end 
        --,dbo.getMtlDesc(PSD.id,PSD.seq1,PSD.seq2,2,0)
		,ds5.string
        ,PSD.Qty
        ,PSD.NETQty
        ,PSD.NETQty+PSD.LossQty
        ,PSD.ShipQty
        ,PSD.ShipFOC
        ,PSD.ApQty
        ,PSD.InputQty
        ,[Scrap Qty]= isnull(MDPD.LObQty,0)
        ,PSD.POUnit
        ,iif(PSD.Complete=1,'Y','N')
        --,PSD.ETA
        ,PSD.FinalETA
        ,orderlist = a.orderlist
        ,MDPD.InQty
        ,PSD.StockUnit
        ,MDPD.OutQty
        ,MDPD.AdjustQty
        ,MDPD.InQty - MDPD.OutQty + MDPD.AdjustQty balance
        ,MDPD.ALocation
        ,MDPD.BLocation
        ,case PSD.FabricType 
            when 'F' then FT.F
            when 'A' then FT2.A
         end
        {sqlColSeparateByWK}
from dbo.PO_Supp_Detail PSD
join dbo.PO_Supp PS on PSD.id = PS.id and PSD.Seq1 = PS.Seq1
join dbo.Supp S on S.id = PS.SuppID
join dbo.Orders O on o.id = PSD.id
join dbo.Factory F on f.id = o.FactoryId
left join dbo.MDivisionPoDetail MDPD on MDPD.POID = PSD.ID and MDPD.Seq1 = PSD.Seq1 and MDPD.Seq2 = PSD.Seq2
left join dbo.Fabric on fabric.SciRefno = psd.SciRefno
{sqlJoinSeparateByWK}
outer apply(select StyleID from dbo.orders WITH (NOLOCK) where id = PS.id) si
outer apply
(
	select orderlist = 
	stuff((
		select concat(',',OrderID)
        from DBO.PO_Supp_Detail_OrderList WITH (NOLOCK) 
        where id=PSD.id and seq1=PSD.seq1 and seq2 = PSD.SEQ2
        for xml path('')
	 ),1,1,'')
)a
outer apply
(
	select F = 
	stuff((
		select concat('/',iif(t3.result='P','Pass',iif(t3.result='F','Fail',t3.Result)))         
        from dbo.AIR t3 WITH (NOLOCK) 
        where t3.POID = PSD.ID and t3.seq1 = PSD.seq1 and t3.seq2 = PSD.seq2
        for xml path('')
	),1,1,'')
)FT
outer apply
(
	select A = 
	stuff((
		select concat('/',x.result )
        from (select result = iif(t2.result='P','Pass',iif(t2.result='F','Fail',t2.Result)) 
                from dbo.FIR t2 WITH (NOLOCK) 
                where t2.POID = PSD.ID and t2.seq1 = PSD.seq1 and t2.seq2 = PSD.seq2) x 
        for xml path('')
	 ),1,1,'')
)FT2
outer apply
(
	SELECT p.SCIRefno
		, p.Refno
		, suppcolor = Concat(iif(ISNULL(p.SuppColor,'') = '', '', p.SuppColor)
							,iif(ISNULL(p.SuppColor,'') != '' and ISNULL(p.ColorID,'') != '',CHAR(10),'')
						    ,iif(ISNULL(p.ColorID,'') = '', '', p.ColorID + ' - ') 
							,c.Name)
		, StockSP = concat(iif(isnull(p.StockPOID,'')='','',p.StockPOID+' ')
						  ,iif(isnull(p.StockSeq1,'')='','',p.StockSeq1+' ')
						  ,p.StockSeq2)
		, po_desc= concat(iif(ISNULL(p.ColorDetail,'') = '', '', 'ColorDetail : ' + p.ColorDetail)
						 ,iif(ISNULL(p.sizespec,'') = '', '', p.sizespec + ' ')
						 ,p.SizeUnit
						 ,p.Special
						 ,p.Spec
						 ,p.Remark)
		, Spec = iif(stockPO3.Spec is null,p.Spec ,stockPO3.Spec)
		, fabric_detaildesc = f.DescDetail
		, zn.ZipperName
	from dbo.po_supp_detail p WITH (NOLOCK)
	left join fabric f WITH (NOLOCK) on p.SCIRefno = f.SCIRefno
	left join Color c WITH (NOLOCK) on f.BrandID = c.BrandId and p.ColorID = c.ID 
	outer apply ( 
		select Spec,BomZipperInsert 
		from PO_Supp_Detail tmpPO3
		where tmpPO3.ID = p.StockPOID and tmpPO3.Seq1 =  p.StockSeq1 and tmpPO3.Seq2 =  p.StockSeq2
	) stockPO3
	outer apply
	(
		Select ZipperName = DropDownList.Name
		From Production.dbo.DropDownList
		Where Type = 'Zipper' And ID = iif(stockPO3.BomZipperInsert is null,p.BomZipperInsert ,stockPO3.BomZipperInsert)
	)zn
	WHERE p.ID=PSD.id and seq1 = PSD.seq1 and seq2=PSD.seq2
)ds
outer apply
(
	select string = 
	concat(iif(isnull(ds.fabric_detaildesc,'')='','',ds.fabric_detaildesc+CHAR(10))
		  ,iif(isnull(ds.suppcolor,'')='','',ds.suppcolor+CHAR(10))
		  ,replace(ds.po_desc,char(10),'')
		  )
)ds2
outer apply
(
	select string = iif(left(PSD.seq1,1) = '7'
					,concat('**PLS USE STOCK FROM SP#:', ds.StockSP, '**', iif(isnull(ds2.string,'')='', '', CHAR(10) + ds2.string))
					,ds2.string)
)ds3
outer apply(select string=concat(iif(isnull(ds3.string,'')='','',ds3.string+CHAR(10)),IIF(IsNull(ds.ZipperName,'') = '','','Spec:'+ ds.ZipperName+Char(10)),RTrim(ds.Spec)))ds4
outer apply(select string=replace(replace(replace(replace(ds4.string,char(13),char(10)),char(10)+char(10),char(10)),char(10)+char(10),char(10)),char(10)+char(10),char(10)))ds5
outer apply(
select wkno = stuff((
	    select concat(char(10),ID)
	    from Export_Detail with (nolock) 
	    where POID = psd.id and Seq1 = psd.SEQ1 and Seq2 = psd.SEQ2
	    for xml path('')
	),1,1,'')
)Wk
where 1=1  
"));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and '{0}' <= O.SciDelivery ", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and O.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and O.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }
            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(" and O.seasonid = @season");
                sp_season.Value = season;
                cmds.Add(sp_season);
            }
            if (!MyUtility.Check.Empty(spno1) && !MyUtility.Check.Empty(spno2))
            {
                //若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and PSD.id >= @spno1 and PSD.id <= @spno2");
                sp_spno1.Value = spno1.PadRight(10, '0');
                sp_spno2.Value = spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(spno1))
            {
                //只有 sp1 輸入資料
                sqlCmd.Append(" and PSD.id like @spno1 ");
                sp_spno1.Value = spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(spno2))
            {
                //只有 sp2 輸入資料
                sqlCmd.Append(" and PSD.id like @spno2 ");
                sp_spno2.Value = spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(suppDelivery1) || !MyUtility.Check.Empty(suppDelivery2))
            {
                if (!MyUtility.Check.Empty(suppDelivery1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= Coalesce(PSD.finaletd, PSD.CFMETD, PSD.SystemETD)", Convert.ToDateTime(suppDelivery1).ToString("d")));
                if (!MyUtility.Check.Empty(suppDelivery2))
                    sqlCmd.Append(string.Format(@" and Coalesce(PSD.finaletd, PSD.CFMETD, PSD.SystemETD) <= '{0}'", Convert.ToDateTime(suppDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(eta1) || !MyUtility.Check.Empty(eta2))
            {
                if (!MyUtility.Check.Empty(eta1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= PSD.ETA", Convert.ToDateTime(eta1).ToString("d")));
                if (!MyUtility.Check.Empty(eta2))
                    sqlCmd.Append(string.Format(@" and PSD.ETA <= '{0}'", Convert.ToDateTime(eta2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(ata1) || !MyUtility.Check.Empty(ata2))
            {
                if (!MyUtility.Check.Empty(ata1))
                    sqlCmd.Append(string.Format(@" and '{0}' <= PSD.FinalETA", Convert.ToDateTime(ata1).ToString("d")));
                if (!MyUtility.Check.Empty(ata2))
                    sqlCmd.Append(string.Format(@" and PSD.FinalETA <= '{0}'", Convert.ToDateTime(ata2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(country))
            {
                sqlCmd.Append(string.Format(" and S.countryID = '{0}'", country));
            }

            if (!MyUtility.Check.Empty(supp))
            {
                sqlCmd.Append(string.Format(" and PS.suppid = '{0}'",supp));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and F.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and O.FactoryID = @FactoryID");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(zone))
            {
                sqlCmd.Append(" and f.Zone = @Zone");
                sp_zone.Value = zone;
                cmds.Add(sp_zone);
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(" and O.BrandID = @BrandID");
                sp_brand.Value = brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(fabrictype))
            {
                sqlCmd.Append(string.Format(@" and PSD.FabricType = '{0}'", fabrictype));
            }

            if (!MyUtility.Check.Empty(refno1) && !MyUtility.Check.Empty(refno2))
            {
                //Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and PSD.refno >= @refno1 and PSD.refno <= @refno2");
                sp_refno1.Value = refno1;
                sp_refno2.Value = refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(refno1))
            {
                //只輸入 Refno1
                sqlCmd.Append(" and PSD.refno like @refno1");
                sp_refno1.Value = refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(refno2))
            {
                //只輸入 Refno2
                sqlCmd.Append(" and PSD.refno like @refno2");
                sp_refno2.Value = refno2 + "%";
                cmds.Add(sp_refno2);
            }

            // Wkno 塞選條件
            if (!MyUtility.Check.Empty(wkNo1) && !MyUtility.Check.Empty(wkNo2))
            {
                //Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and wk.wkno between @wkno1 and @wkno2 ");
                sp_wkno1.Value = wkNo1;
                sp_wkno2.Value = wkNo2;
                cmds.Add(sp_wkno1);
                cmds.Add(sp_wkno2);
            }
            else if (!MyUtility.Check.Empty(wkNo1))
            {
                //只輸入 Refno1
                sqlCmd.Append(" and wk.wkno like @wkno1");
                sp_wkno1.Value = wkNo1 + "%";
                cmds.Add(sp_wkno1);
            }
            else if (!MyUtility.Check.Empty(wkNo2))
            {
                //只輸入 Refno2
                sqlCmd.Append(" and wk.wkno like @wkno2");
                sp_wkno2.Value = wkNo2 + "%";
                cmds.Add(sp_wkno2);
            }
            int dwr = (chkDWR.Checked) ? 1 : 0;

            sqlCmd.Append($@" and fabric.DWR = {dwr}");

            if (chkWhseClose.Checked)
            {
                sqlCmd.Append(" and o.WhseClose is null");
            }


            sqlCmd.Append(IncludeJunk + Environment.NewLine);
            sqlCmd.Append(ExcludeMaterial + Environment.NewLine);

            if (orderby.ToUpper().TrimEnd() == "SUPPLIER")
            {
                sqlCmd.Append(" ORDER BY PS.SUPPID, PSD.ID, PSD.SEQ1, PSD.SEQ2 ");
            }
            else
            {
                sqlCmd.Append(" ORDER BY PSD.ID, PSD.SEQ1, PSD.SEQ2 ");
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R03.xltx"); //預先開啟excel app
            this.ShowWaitMessage("Excel Processing...");
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R03.xltx", objApp);

            //com.TransferArray_Limit = 200000;
            com.ColumnsAutoFit = true;
            com.WriteTable(printData,2);

            if (this.chkSeparateByWK.Checked)
            {
                objApp.Sheets[1].Cells[1, 38].Value = "WK No.";
                objApp.Sheets[1].Cells[1, 39].Value = "WK ETA";
                objApp.Sheets[1].Cells[1, 40].Value = "WK Arrive W/H Date";
                objApp.Sheets[1].Cells[1, 41].Value = "WK ShipQty";
                objApp.Sheets[1].Cells[1, 42].Value = "WK F.O.C";
            }
            //Excel.Worksheet worksheet = objApp.Sheets[1];

            //for (int i = 1; i <= printData.Rows.Count; i++)
            //{
            //    string str = worksheet.Cells[i + 1, 12].Value;
            //    if (!MyUtility.Check.Empty(str))
            //        worksheet.Cells[i + 1, 12] = str.Trim();
            //}

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R03");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            //Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
