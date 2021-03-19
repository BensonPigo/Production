using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R03 : Win.Tems.PrintForm
    {
        private string season;
        private string mdivision;
        private string orderby;
        private string spno1;
        private string spno2;
        private string fabrictype;
        private string refno1;
        private string refno2;
        private string style;
        private string country;
        private string supp;
        private string factory;
        private string wkNo1;
        private string wkNo2;
        private string brand;
        private string IncludeJunk;
        private string ExcludeMaterial;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DateTime? suppDelivery1;
        private DateTime? suppDelivery2;
        private DateTime? eta1;
        private DateTime? eta2;
        private DateTime? ata1;
        private DateTime? ata2;
        private DataTable printData;

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Supplier,SP#");
            this.comboOrderBy.SelectedIndex = 0;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateSuppDelivery.Value1) && MyUtility.Check.Empty(this.dateSuppDelivery.Value2) &&
                MyUtility.Check.Empty(this.dateETA.Value1) && MyUtility.Check.Empty(this.dateFinalETA.Value2) &&
                MyUtility.Check.Empty(this.dateFinalETA.Value1) && MyUtility.Check.Empty(this.dateETA.Value2) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) && MyUtility.Check.Empty(this.txtSPNoEnd.Text)) &&
                (MyUtility.Check.Empty(this.txtRefnoStart.Text) && MyUtility.Check.Empty(this.txtRefnoEnd.Text)) &&
                (MyUtility.Check.Empty(this.txtWKNo1.Text) && MyUtility.Check.Empty(this.txtWKNo2.Text)))
            {
                MyUtility.Msg.WarningBox("< Supp Delivery > & < SCI Delivery > & < ETA > & < FinalETA >& < SP# > & < Refno > & < Wk# > can't be empty!!");
                return false;
            }
            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.suppDelivery1 = this.dateSuppDelivery.Value1;
            this.suppDelivery2 = this.dateSuppDelivery.Value2;
            this.eta1 = this.dateETA.Value1;
            this.eta2 = this.dateETA.Value2;
            this.ata1 = this.dateFinalETA.Value1;
            this.ata2 = this.dateFinalETA.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            this.refno1 = this.txtRefnoStart.Text;
            this.refno2 = this.txtRefnoEnd.Text;
            this.wkNo1 = this.txtWKNo1.Text;
            this.wkNo2 = this.txtWKNo2.Text;
            #endregion

            this.country = this.txtcountry.TextBox1.Text;
            this.supp = this.txtsupplier.TextBox1.Text;
            this.style = this.txtstyle.Text;
            this.season = this.txtseason.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.fabrictype = this.comboFabricType.SelectedValue.ToString();
            this.orderby = this.comboOrderBy.Text;
            this.brand = this.txtbrand.Text;

            if (this.chkIncludeJunk.Checked)
            {
                this.IncludeJunk = Environment.NewLine;
            }
            else
            {
                this.IncludeJunk = " AND PSD.Junk=0 ";
            }

            if (this.chkExcludeMaterial.Checked)
            {
                this.ExcludeMaterial = " AND o.Category <> 'M' ";
            }
            else
            {
                this.ExcludeMaterial = Environment.NewLine;
            }

            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
        ,fabrictype = (case PSD.fabrictype 
                        when 'F' then 'Fabric'
                        when 'A' then 'Accessory'
                        when 'O' then 'Other'
						else PSD.FabricType 
						end) + '-' + Fabric.MtlTypeID
        --,dbo.getMtlDesc(PSD.id,PSD.seq1,PSD.seq2,2,0)
		,ds5.string
        ,[Color] = iif(Fabric.MtlTypeID in ('EMB Thread', 'SP Thread', 'Thread') 
                , IIF(isnull(PSD.SuppColor,'') = '',dbo.GetColorMultipleID(O.BrandID, PSD.ColorID),PSD.SuppColor)
                , dbo.GetColorMultipleID(O.BrandID, PSD.ColorID))
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
        ,MDPD.ReturnQty
        ,MDPD.InQty - MDPD.OutQty + MDPD.AdjustQty - MDPD.ReturnQty balance
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
            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and '{0}' <= O.SciDelivery ", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and O.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(" and O.styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(" and O.seasonid = @season");
                sp_season.Value = this.season;
                cmds.Add(sp_season);
            }

            if (!MyUtility.Check.Empty(this.spno1) && !MyUtility.Check.Empty(this.spno2))
            {
                // 若 sp 兩個都輸入則尋找 sp1 - sp2 區間的資料
                sqlCmd.Append(" and PSD.id >= @spno1 and PSD.id <= @spno2");
                sp_spno1.Value = this.spno1.PadRight(10, '0');
                sp_spno2.Value = this.spno2.PadRight(10, 'Z');
                cmds.Add(sp_spno1);
                cmds.Add(sp_spno2);
            }
            else if (!MyUtility.Check.Empty(this.spno1))
            {
                // 只有 sp1 輸入資料
                sqlCmd.Append(" and PSD.id like @spno1 ");
                sp_spno1.Value = this.spno1 + "%";
                cmds.Add(sp_spno1);
            }
            else if (!MyUtility.Check.Empty(this.spno2))
            {
                // 只有 sp2 輸入資料
                sqlCmd.Append(" and PSD.id like @spno2 ");
                sp_spno2.Value = this.spno2 + "%";
                cmds.Add(sp_spno2);
            }

            if (!MyUtility.Check.Empty(this.suppDelivery1) || !MyUtility.Check.Empty(this.suppDelivery2))
            {
                if (!MyUtility.Check.Empty(this.suppDelivery1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= Coalesce(PSD.finaletd, PSD.CFMETD, PSD.SystemETD)", Convert.ToDateTime(this.suppDelivery1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.suppDelivery2))
                {
                    sqlCmd.Append(string.Format(@" and Coalesce(PSD.finaletd, PSD.CFMETD, PSD.SystemETD) <= '{0}'", Convert.ToDateTime(this.suppDelivery2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.eta1) || !MyUtility.Check.Empty(this.eta2))
            {
                if (!MyUtility.Check.Empty(this.eta1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= PSD.ETA", Convert.ToDateTime(this.eta1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.eta2))
                {
                    sqlCmd.Append(string.Format(@" and PSD.ETA <= '{0}'", Convert.ToDateTime(this.eta2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.ata1) || !MyUtility.Check.Empty(this.ata2))
            {
                if (!MyUtility.Check.Empty(this.ata1))
                {
                    sqlCmd.Append(string.Format(@" and '{0}' <= PSD.FinalETA", Convert.ToDateTime(this.ata1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.ata2))
                {
                    sqlCmd.Append(string.Format(@" and PSD.FinalETA <= '{0}'", Convert.ToDateTime(this.ata2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.country))
            {
                sqlCmd.Append(string.Format(" and S.countryID = '{0}'", this.country));
            }

            if (!MyUtility.Check.Empty(this.supp))
            {
                sqlCmd.Append(string.Format(" and PS.suppid = '{0}'", this.supp));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and F.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and O.FactoryID = @FactoryID");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(" and O.BrandID = @BrandID");
                sp_brand.Value = this.brand;
                cmds.Add(sp_brand);
            }

            if (!MyUtility.Check.Empty(this.fabrictype))
            {
                sqlCmd.Append(string.Format(@" and PSD.FabricType = '{0}'", this.fabrictype));
            }

            if (!MyUtility.Check.Empty(this.refno1) && !MyUtility.Check.Empty(this.refno2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and PSD.refno >= @refno1 and PSD.refno <= @refno2");
                sp_refno1.Value = this.refno1;
                sp_refno2.Value = this.refno2;
                cmds.Add(sp_refno1);
                cmds.Add(sp_refno2);
            }
            else if (!MyUtility.Check.Empty(this.refno1))
            {
                // 只輸入 Refno1
                sqlCmd.Append(" and PSD.refno like @refno1");
                sp_refno1.Value = this.refno1 + "%";
                cmds.Add(sp_refno1);
            }
            else if (!MyUtility.Check.Empty(this.refno2))
            {
                // 只輸入 Refno2
                sqlCmd.Append(" and PSD.refno like @refno2");
                sp_refno2.Value = this.refno2 + "%";
                cmds.Add(sp_refno2);
            }

            // Wkno 塞選條件
            if (!MyUtility.Check.Empty(this.wkNo1) && !MyUtility.Check.Empty(this.wkNo2))
            {
                // Refno 兩個都輸入則尋找 Refno1 - Refno2 區間的資料
                sqlCmd.Append(" and wk.wkno between @wkno1 and @wkno2 ");
                sp_wkno1.Value = this.wkNo1;
                sp_wkno2.Value = this.wkNo2;
                cmds.Add(sp_wkno1);
                cmds.Add(sp_wkno2);
            }
            else if (!MyUtility.Check.Empty(this.wkNo1))
            {
                // 只輸入 Refno1
                sqlCmd.Append(" and wk.wkno like @wkno1");
                sp_wkno1.Value = this.wkNo1 + "%";
                cmds.Add(sp_wkno1);
            }
            else if (!MyUtility.Check.Empty(this.wkNo2))
            {
                // 只輸入 Refno2
                sqlCmd.Append(" and wk.wkno like @wkno2");
                sp_wkno2.Value = this.wkNo2 + "%";
                cmds.Add(sp_wkno2);
            }

            int dwr = this.chkDWR.Checked ? 1 : 0;

            sqlCmd.Append($@" and fabric.DWR = {dwr}");

            if (this.chkWhseClose.Checked)
            {
                sqlCmd.Append(" and o.WhseClose is null");
            }

            sqlCmd.Append(this.IncludeJunk + Environment.NewLine);
            sqlCmd.Append(this.ExcludeMaterial + Environment.NewLine);
            sqlCmd.Append("and F.IsProduceFty = 1" + Environment.NewLine);

            if (this.orderby.ToUpper().TrimEnd() == "SUPPLIER")
            {
                sqlCmd.Append(" ORDER BY PS.SUPPID, PSD.ID, PSD.SEQ1, PSD.SEQ2 ");
            }
            else
            {
                sqlCmd.Append(" ORDER BY PSD.ID, PSD.SEQ1, PSD.SEQ2 ");
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R03.xltx"); // 預先開啟excel app
            this.ShowWaitMessage("Excel Processing...");
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_R03.xltx", objApp);

            // com.TransferArray_Limit = 200000;
            com.ColumnsAutoFit = true;
            com.WriteTable(this.printData, 2);

            if (this.chkSeparateByWK.Checked)
            {
                objApp.Sheets[1].Cells[1, 39].Value = "WK No.";
                objApp.Sheets[1].Cells[1, 40].Value = "WK ETA";
                objApp.Sheets[1].Cells[1, 41].Value = "WK Arrive W/H Date";
                objApp.Sheets[1].Cells[1, 42].Value = "WK ShipQty";
                objApp.Sheets[1].Cells[1, 43].Value = "WK F.O.C";
            }

            // Excel.Worksheet worksheet = objApp.Sheets[1];

            // for (int i = 1; i <= printData.Rows.Count; i++)
            // {
            //    string str = worksheet.Cells[i + 1, 12].Value;
            //    if (!MyUtility.Check.Empty(str))
            //        worksheet.Cells[i + 1, 12] = str.Trim();
            // }
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R03");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            // Marshal.ReleaseComObject(worksheet);
            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
