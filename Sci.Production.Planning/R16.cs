using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R16
    /// </summary>
    public partial class R16 : Sci.Win.Tems.PrintForm
    {
        private string factory;
        private string mdivision;
        private DateTime? sewingDate1;
        private DateTime? sewingDate2;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();
        private StringBuilder datelist = new StringBuilder();

        /// <summary>
        /// R16
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSciDelivery.Value1) && MyUtility.Check.Empty(this.dateSewingDate.Value1))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > & < Sewing Date > can't be empty!!");
                return false;
            }

            #region -- 必輸的條件 --
            this.sciDelivery1 = this.dateSciDelivery.Value1;
            this.sciDelivery2 = this.dateSciDelivery.Value2;
            this.sewingDate1 = this.dateSewingDate.Value1;
            this.sewingDate2 = this.dateSewingDate.Value2;
            #endregion
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            #endregion

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
select o.FactoryID [Factory]
	,o.id [SP#]
	,o.BrandID [Brand]
	,o.StyleID [Style]
	,o.SeasonID [Season]
	,o.SciDelivery [Sci Dlv.]
	,o.CdCodeID [CD Code]
	,[Qty] = o.Qty
	,[SMR Name] = ne1.ine
	,[Handle Name] = ne2.ine
	,[TW PR] = ne3.ine
	,(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-45,o.SciDelivery) and Holiday=0) as [PP Sample Material Arrival Reqd.]
	,iif(x1.MTLExport='OK',x1.MTLETA,null) as [PP Sample Material Arrival Act.]
	,iif(x1.SampleApv is null,'Y','') as [PP Sample Material Arrival Skip]

	,(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-2,o.Sewinline) and Holiday=0) [Sample approval Reqd.]
	,x1.SampleApv [Sample approval Act.]
	,iif(x1.SampleApv is null,'Y','') [Sample approval Skip]  
	,o.KPIMNOTICE as [M/Notice Approve Reqd.]
	,isnull(o.MnorderApv,o.SMnorderApv) as [M/Notice Approve Act.]
	,o.KPIEachConsApprove [Each cons. Approve Reqd.]
	,o.EachConsApv  [Each cons. Approve Act.]

	,[Sketch Reqd.] = sp02.ProvideDate
	,[Sketch Act.] = sp02.FtyLastDate
	,[Sketch Skip] = iif((sp02.FtyLastDate is not null) or (sp02.ProvideDate is not null),'','Y') 

	,[AD/BOM/KIT Reqd.] = sp03.ProvideDate
	,[AD/BOM/KIT Act.] = sp03.FtyLastDate
	,[AD/BOM/KIT Skip] = iif((sp03.FtyLastDate is not null) or (sp03.ProvideDate is not null),'','Y') 

	,[Sample Reqd.] = sp04.ProvideDate
	,[Sample Act.] = sp04.FtyLastDate
	,[Sample Skip] = iif((sp04.FtyLastDate is not null) or (sp04.ProvideDate is not null),'','Y') 

	,[Mockup (Printing) Reqd.] = sp05.ProvideDate
	,[Mockup (Printing) Act.] = sp05.FtyLastDate
	,[Mockup (Printing) Skip] = iif((sp05.FtyLastDate is not null) or (sp05.ProvideDate is not null),'','Y') 

	,[Mockup (Embroidery) Reqd.] = sp06.ProvideDate
	,[Mockup (Embroidery) Act.] = sp06.FtyLastDate
	,[Mockup (Embroidery) Skip] = iif((sp06.FtyLastDate is not null) or (sp06.ProvideDate is not null),'','Y') 

	,[Mockup (Heat transfer) Reqd.] = sp08.ProvideDate
	,[Mockup (Heat transfer) Act.] = sp08.FtyLastDate
	,[Mockup (Heat transfer) Skip] = iif((sp08.FtyLastDate is not null) or (sp08.ProvideDate is not null),'','Y') 

	,[Mockup (Emboss/Deboss) Reqd.] = sp07.ProvideDate
	,[Mockup (Emboss/Deboss) Act.] = sp07.FtyLastDate
	,[Mockup (Emboss/Deboss) Skip] = iif((sp07.FtyLastDate is not null) or (sp07.ProvideDate is not null),'','Y') 

	,[Trim card Reqd.] = sp01.ProvideDate
	,[Trim card Act.] = sp01.FtyLastDate
	,[Trim card Skip] = iif((sp01.FtyLastDate is not null) or (sp01.ProvideDate is not null),'','Y') 


	,o.KPIEachConsApprove [Bulk marker request Reqd]
	,(select min(sm.AddDate) from dbo.SMNotice sm WITH (NOLOCK) inner join dbo.Marker_Send ms WITH (NOLOCK) on ms.PatternSMID = sm.ID where sm.StyleUkey = o.StyleUkey) [Bulk marker request Act.]
	,(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-15,o.SewInLine) and Holiday=0) as [Bulk marker release Reqd]
	,(select min(ms.AddDate) from dbo.SMNotice sm WITH (NOLOCK) inner join dbo.Marker_Send ms WITH (NOLOCK) on ms.PatternSMID = sm.ID where sm.StyleUkey = o.StyleUkey) [Bulk marker release Act.]

	,o.KPILETA [Mtl LETA Reqd]
	,IIF(o.MTLExport = 'OK',o.MTLETA,null) [Mtl LETA Act.]

	,[Fabric receiving Reqd]=
		case when FabricReceiving.OrderID is not null then FabricReceiving.TargetDate			
		else 
			(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
			inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
			inner join MtlType m on m.id = f.MtlTypeID  
			where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )) 
		end
	, dbo.GetReveivingDate(o.POID,'Fabric') [Fabric receiving Act.]
    ,(select 'Y' where not exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )) [Fabric receiving Skip]

	,[Accessory receiving Reqd]=
		case when  AccessoryReceiving.OrderID is not null then AccessoryReceiving.TargetDate			
		else 
			(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
			inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
			inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
			where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )) 
		end
	, dbo.GetReveivingDate(o.POID,'Accessory') [Accessory receiving Act.]
    ,(select 'Y' where not exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )) [Accessory receiving Skip]

	,[Packing material receiving Reqd]=
		case when  PackingMaterialReceiving.OrderID is not null then PackingMaterialReceiving.TargetDate			
		else 
			(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
			inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
			inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
			where m.IssueType='Packing' and p.id = o.POID )) 
		end
	, dbo.GetReveivingDate(o.POID,'Packing') [Packing material receiving Act.]
    ,(select 'Y' where not exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType='Packing' and p.id = o.POID )) [Packing material receiving Skip]

	,[Material Inspection Reqd]=
		case when  MaterialInspectionResult.OrderID is not null  then MaterialInspectionResult.TargetDate			
		else 
			(select min(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,5,o.MTLETA) and Holiday=0)
		end 
	,(select max(inspectDate) from (
		select max(f.PhysicalDate) inspectDate from dbo.FIR f WITH (NOLOCK) 
		 where poid=o.POID and f.PhysicalEncode = 1
		union all
		select max(AddDate) from dbo.fir f WITH (NOLOCK) 
		where poid=o.POID and f.Nonphysical = 1
		union all
		select max(a.InspDate) from dbo.AIR a WITH (NOLOCK) where poid=o.POID and a.Status = 'Confirmed' ) t
	  ) [Material Inspection Act.]

	,[Cutting inline Reqd Complete]=
		case when  CuttingInlineDateEst.OrderID is not null then CuttingInlineDateEst.TargetDate			
		else 
			(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-5,o.SewInLine) and Holiday=0)
		end
	,o.CutInLine [Cutting Inline Act.]
	,o.CutOffLine [Cutting Offline Act.]
	,o.SewInLine [Sewing Inline Act.]
	,o.SewOffLine [Sewing Offline Act.]

	,[PPMeeting Reqd Complete]=
		case when FactoryPPMeeting.OrderID is not null then FactoryPPMeeting.TargetDate
		else 
			iif(s.NoNeedPPMeeting=1,
				(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-1,o.Sewinline) and Holiday=0),
				null)
		end
	,iif(s.NoNeedPPMeeting=1,s.PPMeeting,null) as [PPMeeting Act. Complete]
	,iif(s.NoNeedPPMeeting=1,'Y','') [PPMeeting Skip]

	,[Wahsing Reqd Complete]=
		case when  WashTestResultReceiving.OrderID is not null  then WashTestResultReceiving.TargetDate			
		else 
			x3.min_outputDate
		end
	,x3.max_garmentInspectDate as [Washing Act. Complete]
	,iif(x3.max_garmentInspectDate is null,'Y','') as [Wahsing Skip]

	,[Carton Reqd Complete]=
		case when CartonFinished.OrderID is not null then CartonFinished.TargetDate
		else 
			(select min(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,1,o.SewOffLine) and Holiday=0)
		end
	,(select max(pd.ReceiveDate) from  dbo.PackingList_Detail pd  WITH (NOLOCK) where pd.OrderID =o.ID ) as [Carton Act. Complete]
	,'' as [Carton Skip]-- 全面使用clog了，所以都是空白。
	,iif(o.PulloutComplete=1,o.ActPulloutDate,null) as [Garment Act. Complete]
	,[Fabric receiving Reqd CriticalActivity] = FabricReceiving.TargetDate
	,[Accessory receiving Reqd CriticalActivity] = AccessoryReceiving.TargetDate
	,[Packing material receiving Reqd CriticalActivity] = PackingMaterialReceiving.TargetDate
	,[Material Inspection Reqd CriticalActivity] = MaterialInspectionResult.TargetDate
	,[Cutting inline Reqd Complete CriticalActivity] = CuttingInlineDateEst.TargetDate
	,[PPMeeting Reqd Complete CriticalActivity] = FactoryPPMeeting.TargetDate
	,[Wahsing Reqd Complete CriticalActivity] = WashTestResultReceiving.TargetDate
	,[Carton Reqd Complete CriticalActivity] = CartonFinished.TargetDate
from dbo.orders o WITH (NOLOCK) 
inner join dbo.Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
outer apply (select top 1 o1.SciDelivery, o.MTLExport, o.MTLETA, s.SampleApv  
				from dbo.orders o1 WITH (NOLOCK) 
				inner join dbo.style s1 WITH (NOLOCK) on s1.ukey = o1.StyleUkey
				where o1.StyleUkey = o.StyleUkey and o.OrderTypeID in ('PP SAMPLE','MEGO','PP SMP')) x1
outer apply (select max(gd.inspdate) max_garmentInspectDate, min(s.OutputDate) min_outputDate
			from dbo.system WITH (NOLOCK) ,dbo.GarmentTest g WITH (NOLOCK) 
			INNER JOIN dbo.GarmentTest_Detail gd WITH (NOLOCK) on gd.ID = g.ID
			left join (dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			inner join dbo.SewingOutput s WITH (NOLOCK) on s.id = sdd.ID)  on sdd.OrderId = g.orderid and sdd.Article = g.Article
			where g.OrderID = o.ID) x3
outer apply (select ine=concat(ID,'-',REPLACE(Name,' ',''), iif(isnull(ExtNo,'')='','',' #'+ExtNo)) from TPEPass1 WITH (NOLOCK)WHERE ID=o.SMR )ne1
outer apply (select ine=concat(ID,'-',REPLACE(Name,' ',''), iif(isnull(ExtNo,'')='','',' #'+ExtNo)) from TPEPass1 WITH (NOLOCK)WHERE ID=o.MRHandle )ne2
outer apply (select ine=concat(ID,'-',REPLACE(Name,' ',''), iif(isnull(ExtNo,'')='','',' #'+ExtNo)) from TPEPass1 WITH (NOLOCK)WHERE ID=(SELECT POHandle FROM PO WHERE ID = o.POID) )ne3
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01') sp01
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='02') sp02
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03') sp03
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04') sp04
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05') sp05
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='06') sp06
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07') sp07
outer apply (select max(sp.ProvideDate) as ProvideDate,max(sp.FtyLastDate) as FtyLastDate from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08') sp08
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Fabric Receiving') FabricReceiving
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Accessory Receiving') AccessoryReceiving
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Packing Material Receiving') PackingMaterialReceiving
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Material Inspection Result') MaterialInspectionResult
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Cutting Inline Date (Est.)') CuttingInlineDateEst
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Factory PP Meeting') FactoryPPMeeting 
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Wash Test Result Receiving') WashTestResultReceiving 
outer apply (select * from CriticalActivity where OrderID = o.id and DropDownListID = 'Carton Finished') CartonFinished 
where o.qty > 0 and o.junk = 0 and o.LocalOrder = 0
"));

            #region --- 條件組合  ---
            this.condition.Clear();
            if (!MyUtility.Check.Empty(this.sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.scidelivery >= '{0}'", Convert.ToDateTime(this.sciDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sewingDate1) && !MyUtility.Check.Empty(this.sewingDate2))
            {
                sqlCmd.Append(string.Format(@" AND (O.sewoffline !< '{0:d}' or o.sewInline !> '{1:d}')", this.sewingDate1, this.sewingDate2));
            }
            else
            {
                if (!MyUtility.Check.Empty(this.sewingDate1))
                {
                    sqlCmd.Append(string.Format(@" and O.sewoffline !< '{0}'", Convert.ToDateTime(this.sewingDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.sewingDate2))
                {
                    sqlCmd.Append(string.Format(@" and o.sewInline !> '{0}'", Convert.ToDateTime(this.sewingDate2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and O.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and O.FtyGroup = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            #endregion

            sqlCmd.Append(" order by o.factoryid,o.id");

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R16.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R16.xltx", 2, false, null, objApp);      // 將datatable copy to excel
            objApp.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            Excel.Range range = null;

            // 列印動態欄位的表頭
            for (int i = 0; i < this.printData.Rows.Count; i++)
            {
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["PP Sample Material Arrival Reqd."])
                    && this.printData.Rows[i]["PP Sample Material Arrival Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["PP Sample Material Arrival Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 13];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["PP Sample Material Arrival Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["PP Sample Material Arrival Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 13];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Sample approval Reqd."])
                    && this.printData.Rows[i]["Sample approval Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Sample approval Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 16];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Sample approval Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Sample approval Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 16];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["M/Notice Approve Reqd."]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["M/Notice Approve Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 19];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["M/Notice Approve Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["M/Notice Approve Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 19];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Each cons. Approve Reqd."]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Each cons. Approve Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 19];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Each cons. Approve Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Each cons. Approve Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 19];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Sketch Reqd."])
                    && this.printData.Rows[i]["Sketch Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Sketch Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 23];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Sketch Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Sketch Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 23];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["AD/BOM/KIT Reqd."])
                    && this.printData.Rows[i]["AD/BOM/KIT Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["AD/BOM/KIT Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 26];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["AD/BOM/KIT Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["AD/BOM/KIT Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 26];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Sample Reqd."])
                    && this.printData.Rows[i]["Sample Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Sample Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 29];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Sample Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Sample Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 29];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Printing) Reqd."])
                    && this.printData.Rows[i]["Mockup (Printing) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Printing) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 32];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Printing) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Printing) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 32];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Embroidery) Reqd."])
                    && this.printData.Rows[i]["Mockup (Embroidery) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Embroidery) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 35];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Embroidery) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Embroidery) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 35];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Heat transfer) Reqd."])
                    && this.printData.Rows[i]["Mockup (Heat transfer) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Heat transfer) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 38];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Heat transfer) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Heat transfer) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 38];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."])
                    && this.printData.Rows[i]["Mockup (Emboss/Deboss) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Emboss/Deboss) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 41];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Emboss/Deboss) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 41];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Trim card Reqd."])
                    && this.printData.Rows[i]["Trim card Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Trim card Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 44];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Trim card Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Trim card Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 44];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker request Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker request Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 47];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Bulk marker request Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Bulk marker request Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 47];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker release Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker release Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 49];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Bulk marker release Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Bulk marker release Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 49];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mtl LETA Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mtl LETA Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 51];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mtl LETA Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Mtl LETA Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 51];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Fabric receiving Reqd"])
                    && this.printData.Rows[i]["Fabric receiving Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Fabric receiving Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 53];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Fabric receiving Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Fabric receiving Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 53];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Fabric receiving Reqd CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 52];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Accessory receiving Reqd"])
                    && this.printData.Rows[i]["Accessory receiving Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Accessory receiving Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 56];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Accessory receiving Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Accessory receiving Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 56];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Accessory receiving Reqd CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 55];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Packing material receiving Reqd"])
                    && this.printData.Rows[i]["Packing material receiving Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Packing material receiving Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 59];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Packing material receiving Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Packing material receiving Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 59];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Packing material receiving Reqd CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 58];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Material Inspection Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Material Inspection Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 62];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Material Inspection Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Material Inspection Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 62];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Material Inspection Reqd CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 61];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Cutting inline Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Cutting Inline Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 64];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Cutting inline Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["Cutting Inline Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 64];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Cutting inline Reqd Complete CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 63];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["PPMeeting Reqd Complete"])
                    && this.printData.Rows[i]["PPMeeting Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["PPMeeting Act. Complete"]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 69];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["PPMeeting Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["PPMeeting Act. Complete"].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 69];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["PPMeeting Reqd Complete CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 68];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Wahsing Reqd Complete"])
                    && this.printData.Rows[i]["Wahsing Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Washing Act. Complete"]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 72];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Wahsing Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["Washing Act. Complete"].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 72];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Wahsing Reqd Complete CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 71];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Carton Reqd Complete"])
                    && this.printData.Rows[i]["Carton Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Carton Act. Complete"]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 75];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Carton Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["Carton Act. Complete"].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 75];
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Carton Reqd Complete CriticalActivity"]))
                {
                    range = (Excel.Range)objSheet.Cells[i + 3, 74];
                    range.Interior.Color = Color.FromArgb(255, 255, 0); // 背景顏色
                }
            }

            // 刪除欄位
            objSheet.get_Range("BZ:CG").EntireColumn.Delete();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Planning_R16");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheet);
            Marshal.ReleaseComObject(range);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
