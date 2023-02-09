using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R14 : Win.Tems.PrintForm
    {
        private List<string> sqlWherelist;
        private List<SqlParameter> lisSqlParameter;
        private string strSQLWhere = string.Empty;
        private DataTable printTable;

        /// <inheritdoc/>
        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboShift.SetDataSource();
            this.comboMDivision.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboMDivision.Text);
            this.radioSummary.Checked = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboFactory.Text = Env.User.Factory;
            this.comboShift.Text = this.ChangShift(Env.User.Factory);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strSQLWhere = string.Empty;
            this.sqlWherelist = new List<string>();
            this.lisSqlParameter = new List<SqlParameter>();

            if (MyUtility.Check.Empty(this.dateLastInsDate.Value1) &&
                MyUtility.Check.Empty(this.dateLastInsDate.Value2) &&
                MyUtility.Check.Empty(this.txtInsCutRef.Text) &&
                MyUtility.Check.Empty(this.txtWorkOrderCuRef.Text) &&
                MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("[Last Ins. Date], [Work Order CutRef#], [Ins. CutRef#], [SP#] cannot be all empty.");
                return false;
            }

            if (!MyUtility.Check.Empty(this.dateLastInsDate.Value1) &&
                !MyUtility.Check.Empty(this.dateLastInsDate.Value2))
            {
                this.sqlWherelist.Add("s.LastInspectionDate between @date1 and @date2");
                this.lisSqlParameter.Add(new SqlParameter("@date1", this.dateLastInsDate.Value1));
                this.lisSqlParameter.Add(new SqlParameter("@date2", this.dateLastInsDate.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtInsCutRef.Text))
            {
                this.sqlWherelist.Add("si.CutRef = @InsCutRef");
                this.lisSqlParameter.Add(new SqlParameter("@InsCutRef", this.txtInsCutRef.Text));
            }

            if (!MyUtility.Check.Empty(this.txtWorkOrderCuRef.Text))
            {
                this.sqlWherelist.Add("so.CutRef = @WorkOrderCutRef");
                this.lisSqlParameter.Add(new SqlParameter("@WorkOrderCutRef", this.txtWorkOrderCuRef.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                this.sqlWherelist.Add(@"exists(select 1
                                       from SciProduction_WorkOrder_Distribute swd
			                           where swd.WorkOrderUkey=pms_wo.Ukey and swd.orderid = @SP)
                                       ");
                this.lisSqlParameter.Add(new SqlParameter("@SP", this.txtSP.Text));
            }

            if (!MyUtility.Check.Empty(this.comboShift.SelectedValue))
            {
                if (this.radioSummary.Checked)
                {
                    this.sqlWherelist.Add("siidata.Shift = @Shift");
                }
                else
                {
                    this.sqlWherelist.Add("sii.Shift = @Shift");
                }
                this.lisSqlParameter.Add(new SqlParameter("@Shift", this.comboShift.SelectedValue));
            }

            if (!MyUtility.Check.Empty(this.comboMDivision.SelectedValue))
            {
                this.sqlWherelist.Add("pms_wo.MDivisionId = @MDivisionId");
                this.lisSqlParameter.Add(new SqlParameter("@MDivisionId", this.comboMDivision.SelectedValue));
            }

            if (!MyUtility.Check.Empty(this.comboFactory.SelectedValue))
            {
                this.sqlWherelist.Add("pms_wo.FactoryID = @FactoryID");
                this.lisSqlParameter.Add(new SqlParameter("@FactoryID", this.comboFactory.SelectedValue));
            }

            this.strSQLWhere = string.Join(" and ", this.sqlWherelist);
            if (this.sqlWherelist.Count != 0)
            {
                this.strSQLWhere = " where " + this.strSQLWhere;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            if (this.radioSummary.Checked)
            {
                sqlcmd = $@"select 
                            [Factory] = pms_wo.FactoryID,
                            [OuiCutRef] = oui.[ouiCutRef],
                            [SubCutRef] = si.CutRef,
                            [Activate] = case when soc.[count] > sic.[count] then 'Combine'
			                                    when soc.[count] < sic.[count]  then 'Separate' 
				                                else '' end,
                            [InspectDate] = siidata.MaxAddDate,
                            [RFT] = RFT.val,
                            [Shift] = sh.val,
                            [SP] = SP.val,
                            [Style#] = sty.StyleID,
                            [Marker#] = m.MarkerNo,
                            [Fabric Description] = pms_f.[Description],
                            [Color] = color.val,
                            [Size] = size.val,
                            [Spreading Machine#] = s.SpreadingNoID,
                            [Fabric Roll] = FabricRoll.val,
                            [MarkerCheck] = isnull(ip_mc.Result,'N/A'),
                            [HandFeel] = isnull(ip_hf.Result,'N/A'),
                            [WrongFaceSide] = isnull(ip_ws.Result,'N/A'),
                            [ShortWidth] = isnull(ip_sw.Result,'N/A'),
                            [MachineTension] = isnull(ip_mt.Result,'N/A'),
                            [FabricColor] = isnull(ip_fc.Result,'N/A'),
                            [InsertPaper] = isnull(ip_ip.Result,'N/A'),
                            [FabricDefect] = isnull(ip_fd.Result,'N/A'),
                            [FabricHeight] = isnull(ip_fh.Result,'N/A'),
                            [FabricRelaxation] = isnull(ip_fr.Result,'N/A'),
                            [RejectPointQty] = siifc.[count],
                            [Inspector] =p.[Name],
                            [StartingTime] = siidata.MinAddDate,
                            [Remark] = si.Remark,
                            [InspectedTime] = DATEDIFF(second, siidata.MinAddDate,siidata.MaxEditDate)

                            from SpreadingInspection s 
                            left join SpreadingInspection_InsCutRef si with(nolock) on s.ID = si.ID
                            left join SpreadingInspection_OriCutRef so with(nolock) on s.id = so.id
                            left join SciProduction_WorkOrder pms_wo with(nolock)  on so.WorkOrderUkey=pms_wo.Ukey
                            left join SciProduction_Fabric pms_f with(nolock) on pms_wo.SCIRefno = pms_f.SCIRefno

                            outer apply(select top 1 * from SciProduction_WorkOrder where so.WorkOrderUkey = Ukey ) m
                            outer apply(select top 1 * from SciProduction_Orders where pms_wo.ID = ID ) sty
                            outer apply(select [count] = count(*) from SpreadingInspection_OriCutRef where  so.ID = ID ) soc
                            outer apply(select [count] = count(*) from SpreadingInspection_InsCutRef where  si.ID = ID  ) sic
                            outer apply(select [count] = Count(*) from SpreadingInspection_InsCutRef_Inspection where si.Ukey = SpreadingInspectionInsCutRefUkey and Result='Fail') siifc
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Marker Check')ip_mc
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Hand Feel')ip_hf
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Wrong Face Side')ip_ws
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Short Width')ip_sw
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Machine Tension')ip_mt
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Color')ip_fc
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Insert Paper')ip_ip
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Defect')ip_fd
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Height')ip_fh
                            outer apply(select Result from SpreadingInspection_InsCutRef_Inspection  where SpreadingInspectionInsCutRefUkey = si.Ukey and Item = 'Fabric Relaxation')ip_fr
                            outer apply
                            (
	                            select 
	                            [MaxAddDate] = max(AddDate),
	                            [MinAddDate] = min(AddDate),
	                            [MaxEditDate] = max(EditDate),
	                            [MinEditDate] = min(EditDate),
	                            [Shift]
	                            from SpreadingInspection_InsCutRef_Inspection 
	                            where SpreadingInspectionInsCutRefUkey = si.Ukey
	                            group by Shift
                            )siidata
                            outer apply
                            (
	                            select 
	                            AddName,
	                            EditName
	                            from SpreadingInspection_InsCutRef_Inspection 
	                            where isnull(EditDate,AddDate) = isnull( siidata.maxeditdate,siidata. MaxAddDate)
                            )siiMaxName
                            outer apply
                            (
	                            select 
	                            [Name]
	                            from Pass1
	                            where ID = isnull(siiMaxName.EditName,siiMaxName.AddName)
                            )p
                            outer apply
                            (
	                            select [ouiCutRef] = stuff((
	                            select concat('/',tmp.CutRef) from
	                            (
		                            select distinct CutRef
		                            from SpreadingInspection_OriCutRef
                                    where id = so.id
	                            ) tmp for xml path('')),1,1,'')
                            )oui
                            outer apply
                            (
	                            select val = ROUND(CAST(count(*) as float) / CAST( 10 as float),2)
	                            from SpreadingInspection_InsCutRef_Inspection
	                            where si.ID = ID and Result = 'Pass'
                            )RFT
                            outer apply
                            (
	                            SELECT val = stuff(
	                            (
	                            select concat('/',tmp.[Shift])
	                            FROM
	                            (
		                            SELECT  [Shift] 
		                            FROM SpreadingInspection_InsCutRef_Inspection  
		                            where SpreadingInspectionInsCutRefUkey = si.Ukey
	                            )
	                            tmp for xml path('')),1,1,'')
                            )sh
                            outer apply
                            (
	                            select val = dbo.SciProduction_GetSinglelineSP(
	                            (
                                    select distinct S.OrderID
                                    from SciProduction_WorkOrder_Distribute S with(nolock)
                                    where S.OrderID!='EXCESS'
                                    AND WorkOrderUkey in (so.WorkOrderUkey)
                                    order by OrderID
                                    for XML RAW)
	                            )
                            )SP
                            outer apply
                            (
	                            select val = stuff(
	                            (
		                            select concat('/',tmp.Colorid) 
		                            from
		                            (
			                            select distinct w.Colorid 
			                            from SciProduction_WorkOrder w
			                            where Ukey IN (so.WorkOrderUkey
		                            )
	                            )
	                            tmp for xml path('')),1,1,'')
                            )color
                            outer apply
                            (
	                            select val = stuff(
	                            (
		                            select  concat(char(10),tmp.DataList) 
		                            from
		                            (
			                            select [DataList] = concat(S.SizeCode ,'/', S.Qty)
			                            from SciProduction_WorkOrder_Distribute S
			                            where  WorkOrderUkey IN (so.WorkOrderUkey
		                            )
	                            ) 
	                            tmp for xml path('')),1,1,'')
                            )size
                            outer apply
                            (
	                            select val = stuff((select concat('/',tmp.Roll)
	                            from
	                            (
		                            select  Roll from SpreadingInspection_InsCutRef_Fabric where SpreadingInspectionInsCutRefUkey = si.Ukey
	                            ) 
	                            tmp for xml path('')),1,1,'')
                            )FabricRoll

                            {this.strSQLWhere}

                            Group by pms_wo.FactoryID,oui.[ouiCutRef],si.CutRef,soc.[count],sic.[count],RFT.val,sh.val,SP.val,sty.StyleID,m.MarkerNo,pms_f.[Description],
                                     color.val,size.val,s.[SpreadingNoID],FabricRoll.val,ip_mc.Result,ip_hf.Result,ip_ws.Result,ip_sw.Result,ip_mt.Result,siifc.count,
                                     si.Remark,siidata.MaxAddDate,siidata.MinAddDate,ip_fc.Result,ip_ip.Result,ip_fd.Result,ip_fh.Result,ip_fr.Result,siidata.MinAddDate,
                                     siidata.MaxEditDate,p.[Name]";
            }
            else if (this.radioDetail.Checked)
            {
                sqlcmd = $@"select 
                            [Factory] = pms_wo.FactoryID,
                            [OuiCutRef] = oui.[ouiCutRef],
                            [SubCutRef] = si.CutRef,
                            [Activate] = case when soc.[count] > sic.[count] then 'Combine'
			                                    when soc.[count] < sic.[count]  then 'Separate' 
				                                else '' end,
                            [InspectDate] = sii.AddDate,
                            [RFT] = RFT.Per,
                            [Shift] = sii.[Shift],
                            [SP] = SP.val,
                            [Style#] = sty.StyleID,
                            [Marker#] = m.MarkerNo,
                            [Fabric Description] = pms_f.[Description],
                            [Color] = color.val,
                            [Size] = size.val,
                            [Spreading Machine#] = s.SpreadingNoID,
                            [Fabric Roll] = FabricRoll.val,
                            [InspectedPointQty] = siic.[count],
                            [RejectPointQty] = siifc.[count],
                            [InspectPoint] = sii.Item,
                            [Result] = sii.Result,
                            [DefectCode] = case when sii.Item = 'Marker Check' then MC.val
					                            when sii.Item = 'Short Width' then 'Fabric Width: ' + CONVERT( varchar, pms_f.Width) + CHAR(10) + 'Actual Width:' + siid.ColumnValue
					                            when sii.Item = 'Machine Tension' then 'Speed:' + siid.ColumnValue
					                            when sii.Item = 'Fabric Defect' then fd.val
					                            else '' end,
                            [LastInspectionDate] = IsNull(sii.EditDate, sii.AddDate),
                            [Inspector] =p.[Name],
                            [StartingTime] = st.val,
                            [Remark] = si.Remark

                            from SpreadingInspection s 
                            left join SpreadingInspection_InsCutRef si with(nolock) on s.ID = si.ID
                            left join SpreadingInspection_InsCutRef_Inspection sii with(nolock) on si.Ukey = sii.SpreadingInspectionInsCutRefUkey 
                            left join SpreadingInspection_OriCutRef so with(nolock) on s.id = so.id
                            left join SpreadingInspection_InsCutRef_Inspection_Detail siid with(nolock) on siid.SpreadingInspectionInsCutRefInspectionUkey = sii.Ukey
                            left join Pass1 p with(nolock) on p.ID = iif(sii.EditName='',sii.AddName,sii.EditName)

                            left join SciProduction_WorkOrder pms_wo with(nolock)  on so.WorkOrderUkey =pms_wo.Ukey
                            left join SciProduction_Fabric pms_f with(nolock) on pms_wo.SCIRefno = pms_f.SCIRefno

                            outer apply( select top 1 * from SciProduction_WorkOrder where so.WorkOrderUkey = Ukey ) m
                            outer apply( select top 1 * from SciProduction_Orders where pms_wo.ID = ID ) sty
                            outer apply( select [count] = count(*) from SpreadingInspection_OriCutRef where  so.ID = ID ) soc
                            outer apply( select [count] = count(*) from SpreadingInspection_InsCutRef where  si.ID = ID  ) sic
                            outer apply( select [count] = count(*) from SpreadingInspection_InsCutRef_Inspection where SpreadingInspectionInsCutRefUkey = sii.SpreadingInspectionInsCutRefUkey) siic
                            outer apply( select [count] = Count(*) from SpreadingInspection_InsCutRef_Inspection where SpreadingInspectionInsCutRefUkey = sii.SpreadingInspectionInsCutRefUkey and Result='Fail') siifc
                            outer apply( select val = MIN(AddDate) from SpreadingInspection_InsCutRef_Inspection ms where SpreadingInspectionInsCutRefUkey = sii.SpreadingInspectionInsCutRefUkey) st

                            outer apply
                            (
	                            select [ouiCutRef] = stuff((
	                            select concat('/',tmp.CutRef) from
	                            (
		                            select distinct CutRef
		                            from SpreadingInspection_OriCutRef
                                    where id = so.id
	                            ) tmp for xml path('')),1,1,'')
                            )oui
                            outer apply
                            (
	                            select Per = ROUND(CAST(count(*) as float) / CAST( 10 as float),2)
	                            from SpreadingInspection_InsCutRef_Inspection
	                            where sii.SpreadingInspectionInsCutRefUkey = SpreadingInspectionInsCutRefUkey and Result = 'Pass'
                            )
                            RFT
                            outer apply
                            (
	                            select val = dbo.SciProduction_GetSinglelineSP(
	                            (
                                    select distinct S.OrderID
                                    from SciProduction_WorkOrder_Distribute S with(nolock)
                                    where S.OrderID!='EXCESS'
                                    AND WorkOrderUkey in (so.WorkOrderUkey)
                                    order by OrderID
                                    for XML RAW)
	                            )
                            )SP
                            outer apply
                            (
	                            select val = stuff(
	                            (
		                            select concat('/',tmp.Colorid) 
		                            from
		                            (
			                            select distinct w.Colorid 
			                            from SciProduction_WorkOrder w
			                            where Ukey IN (so.WorkOrderUkey
		                            )
	                            )
	                            tmp for xml path('')),1,1,'')
                            )
                            color
                            outer apply
                            (
	                            select val = stuff(
	                            (
		                            select  concat(char(10),tmp.DataList) 
		                            from
		                            (
			                            select [DataList] = concat(S.SizeCode ,'/', S.Qty)
			                            from SciProduction_WorkOrder_Distribute S
			                            where  WorkOrderUkey IN (so.WorkOrderUkey
		                            )
	                            ) 
	                            tmp for xml path('')),1,1,'')
                            )size
                            outer apply
                            (
	                            select val = stuff((select concat('/',tmp.Roll)
	                            from
	                            (
		                            select  Roll from SpreadingInspection_InsCutRef_Fabric where SpreadingInspectionInsCutRefUkey = si.Ukey
	                            ) 
	                            tmp for xml path('')),1,1,'')
                            )FabricRoll
                            outer apply
                            (
	                            select val = Stuff((
		                            select concat(char(10), s.[Description])
		                            from (
				                            select [Description]
				                            from SpreadingDefectCode
				                            where ID = siid.ColumnValue and
				                            [Type] ='MT'
			                            ) s
		                            for xml path ('')
	                            ) , 1, 1, '')
                            )MC
                            outer apply
                            (
	                            select val = Stuff((
		                            select val = (char(10) + pms_f_Type +'-'+ pms_f_EN)
		                            from (
				                            select 
			 	                            pms_f_Type = pms_fd.[Type],
				                            pms_f_EN = pms_fd.DescriptionEN
				                            from SciProduction_FabricDefect pms_fd with(nolock)
				                            where siid.ColumnValue=pms_fd.ID
			                            ) s
		                            for xml path ('')
	                            ) , 1, 1, '')
                            )FD
                            {this.strSQLWhere}";
            }

            DualResult result = DBProxy.Current.Select("ManufacturingExecution", sqlcmd, this.lisSqlParameter, out this.printTable);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string excelName = this.radioSummary.Checked ? "Quality_R14_Summary" : "Quality_R14_Detail";
            int excelRow = this.radioSummary.Checked ? 2 : 1;

            this.SetCount(this.printTable.Rows.Count);
            if (this.printTable == null || this.printTable.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printTable, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: excelRow, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);

            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;

            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion

            return true;
        }

        /// <summary>
        /// ChangShift
        /// </summary>
        /// <param name="strFactory">strFactory</param>
        /// <returns>string</returns>
        public string ChangShift(string strFactory)
        {
            string sqlCmd = $@"SELECT TOP 1 Shift FROM Shift
                            WHERE FactoryID= '{strFactory}'
                            and BeginTime <= CONVERT (time, GETDATE())
                            and EndTime >= CONVERT (time, GETDATE())
                            ORDER BY StartDate DESC
                            ";
            return MyUtility.GetValue.Lookup(sqlCmd, "ManufacturingExecution");
        }

        private void ComboFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboShift.Text = this.ChangShift(this.comboFactory.Text);
        }
    }
}
