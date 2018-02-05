using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

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
	,o.KPICMPQ as [CMPQ confirm Reqd.]
	,o.CMPQDate as [CMPQ confirm Act.]
	,o.KPIMNOTICE as [M/Notice Approve Reqd.]
	,isnull(o.MnorderApv,o.SMnorderApv) as [M/Notice Approve Act.]
	,o.KPIEachConsApprove [Each cons. Approve Reqd.]
	,o.EachConsApv  [Each cons. Approve Act.]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='02') as [Sketch Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='02') as [Sketch Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='02' and ReasonID=''),'','Y') as [Sketch Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03') as [AD/BOM/KIT Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03') as [AD/BOM/KIT Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03' and ReasonID=''),'','Y') as [AD/BOM/KIT Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04') as [Sample Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04') as [Sample Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04' and ReasonID=''),'','Y') as [Sample Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05') as [Mockup (Printing) Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05') as [Mockup (Printing) Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05' and ReasonID=''),'','Y') as [Mockup (Printing) Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='06') as [Mockup (Embroidery) Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='06') as [Mockup (Embroidery) Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK)  where StyleUkey =o.StyleUkey and doc='06' and ReasonID=''),'','Y') as [Mockup (Embroidery) Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08') as [Mockup (Heat transfer) Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08') as [Mockup (Heat transfer) Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08' and ReasonID=''),'','Y') as [Mockup (Heat transfer) Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07') as [Mockup (Emboss/Deboss) Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07') as [Mockup (Emboss/Deboss) Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07' and ReasonID=''),'','Y') as [Mockup (Emboss/Deboss) Skip]

	,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01') as [Trim card Reqd.]
	,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01') as [Trim card Act.]
	,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01' and ReasonID=''),'','Y') as [Trim card Skip]


	,o.KPIEachConsApprove [Bulk marker request Reqd]
	,(select min(sm.AddDate) from dbo.SMNotice sm WITH (NOLOCK) inner join dbo.Marker_Send ms WITH (NOLOCK) on ms.PatternSMID = sm.ID where sm.StyleUkey = o.StyleUkey) [Bulk marker request Act.]
	,(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-15,o.SewInLine) and Holiday=0) as [Bulk marker release Reqd]
	,(select min(ms.AddDate) from dbo.SMNotice sm WITH (NOLOCK) inner join dbo.Marker_Send ms WITH (NOLOCK) on ms.PatternSMID = sm.ID where sm.StyleUkey = o.StyleUkey) [Bulk marker release Act.]

	,o.KPILETA [Mtl LETA Reqd]
	,IIF(o.MTLExport = 'OK',o.MTLETA,null) [Mtl LETA Act.]

	,(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID  
		where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )) [Fabric receiving Reqd]
	, dbo.GetReveivingDate(o.POID,'Fabric') [Fabric receiving Act.]
	,(select 'Y' where not exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )) [Fabric receiving Skip]

	,(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )) [Accessory receiving Reqd]
	, dbo.GetReveivingDate(o.POID,'Accessory') [Accessory receiving Act.]
	,(select 'Y' where not exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )) [Accessory receiving Skip]

	,(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType='Packing' and p.id = o.POID )) [Packing material receiving Reqd]
	, dbo.GetReveivingDate(o.POID,'Packing') [Packing material receiving Act.]
	,(select 'Y' where not exists (select * from dbo.PO_Supp_Detail p WITH (NOLOCK) 
		inner join dbo.fabric f WITH (NOLOCK) on f.SCIRefno=p.SCIRefno 
		inner join MtlType m WITH (NOLOCK) on m.id = f.MtlTypeID  
		where m.IssueType='Packing' and p.id = o.POID )) [Packing material receiving Skip]

	,(select min(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,5,o.MTLETA) and Holiday=0) as [Material Inspection Reqd]
	,(select max(inspectDate) from (
		select max(f.PhysicalDate) inspectDate from dbo.FIR f WITH (NOLOCK) 
		 where poid=o.POID and f.PhysicalEncode = 1
		union all
		select max(AddDate) from dbo.fir f WITH (NOLOCK) 
		where poid=o.POID and f.Nonphysical = 1
		union all
		select max(a.InspDate) from dbo.AIR a WITH (NOLOCK) where poid=o.POID and a.Status = 'Confirmed' ) t
	  ) [Material Inspection Act.]
	,(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-5,o.SewInLine) and Holiday=0) [Cutting inline Reqd Complete]
	,o.CutInLine [Cutting Inline Act.]
	,o.CutOffLine [Cutting Offline Act.]
	,o.SewInLine [Sewing Inline Act.]
	,o.SewOffLine [Sewing Offline Act.]
	,iif(s.NoNeedPPMeeting=1,(select max(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date<=dateadd(day,-1,o.Sewinline) and Holiday=0),null) as [PPMeeting Reqd Complete]
	,iif(s.NoNeedPPMeeting=1,s.PPMeeting,null) as [PPMeeting Act. Complete]
	,iif(s.NoNeedPPMeeting=1,'Y','') [PPMeeting Skip]
	,x3.min_outputDate as [Wahsing Reqd Complete]
	,x3.max_garmentInspectDate as [Washing Act. Complete]
	,iif(x3.max_garmentInspectDate is null,'Y','') as [Wahsing Skip]
	,(select min(date) from dbo.WorkHour a WITH (NOLOCK) where FactoryID = o.FactoryID and a.Hours > 0 and a.date>=dateadd(day,1,o.SewOffLine) and Holiday=0) as [Carton Reqd Complete]
	,(select max(pd.ReceiveDate) from  dbo.PackingList_Detail pd  WITH (NOLOCK) where pd.OrderID =o.ID ) as [Carton Act. Complete]
	,'' as [Carton Skip]-- 全面使用clog了，所以都是空白。
	,iif(o.PulloutComplete=1,o.ActPulloutDate,null) as [Garment Act. Complete]
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

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["CMPQ confirm Reqd."]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["CMPQ confirm Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 19];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["CMPQ confirm Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["CMPQ confirm Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 19];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["M/Notice Approve Reqd."]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["M/Notice Approve Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 21];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["M/Notice Approve Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["M/Notice Approve Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 21];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Each cons. Approve Reqd."]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Each cons. Approve Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 21];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Each cons. Approve Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Each cons. Approve Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 21];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Sketch Reqd."])
                    && this.printData.Rows[i]["Sketch Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Sketch Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 25];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Sketch Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Sketch Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 25];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["AD/BOM/KIT Reqd."])
                    && this.printData.Rows[i]["AD/BOM/KIT Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["AD/BOM/KIT Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 28];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["AD/BOM/KIT Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["AD/BOM/KIT Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 28];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Sample Reqd."])
                    && this.printData.Rows[i]["Sample Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Sample Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 31];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Sample Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Sample Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 31];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Printing) Reqd."])
                    && this.printData.Rows[i]["Mockup (Printing) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Printing) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 34];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Printing) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Printing) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 34];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Embroidery) Reqd."])
                    && this.printData.Rows[i]["Mockup (Embroidery) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Embroidery) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 37];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Embroidery) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Embroidery) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 37];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Heat transfer) Reqd."])
                    && this.printData.Rows[i]["Mockup (Heat transfer) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Heat transfer) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 40];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Heat transfer) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Heat transfer) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 40];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."])
                    && this.printData.Rows[i]["Mockup (Emboss/Deboss) Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mockup (Emboss/Deboss) Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 43];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Mockup (Emboss/Deboss) Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 43];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Trim card Reqd."])
                    && this.printData.Rows[i]["Trim card Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Trim card Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 46];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Trim card Reqd."].ToString()), DateTime.Parse(this.printData.Rows[i]["Trim card Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 46];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker request Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker request Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 49];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Bulk marker request Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Bulk marker request Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 49];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker release Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Bulk marker release Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 51];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Bulk marker release Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Bulk marker release Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 51];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Mtl LETA Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Mtl LETA Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 53];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Mtl LETA Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Mtl LETA Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 53];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Fabric receiving Reqd"])
                    && this.printData.Rows[i]["Fabric receiving Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Fabric receiving Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 55];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Fabric receiving Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Fabric receiving Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 55];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Accessory receiving Reqd"])
                    && this.printData.Rows[i]["Accessory receiving Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Accessory receiving Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 58];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Accessory receiving Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Accessory receiving Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 58];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Packing material receiving Reqd"])
                    && this.printData.Rows[i]["Packing material receiving Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Packing material receiving Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 61];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Packing material receiving Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Packing material receiving Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 61];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Material Inspection Reqd"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Material Inspection Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 64];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Material Inspection Reqd"].ToString()), DateTime.Parse(this.printData.Rows[i]["Material Inspection Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 64];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Cutting inline Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Cutting Inline Act."]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 66];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Cutting inline Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["Cutting Inline Act."].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 66];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["PPMeeting Reqd Complete"])
                    && this.printData.Rows[i]["PPMeeting Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["PPMeeting Act. Complete"]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 71];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["PPMeeting Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["PPMeeting Act. Complete"].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 71];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Wahsing Reqd Complete"])
                    && this.printData.Rows[i]["Wahsing Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Washing Act. Complete"]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 74];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Wahsing Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["Washing Act. Complete"].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 74];
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(this.printData.Rows[i]["Carton Reqd Complete"])
                    && this.printData.Rows[i]["Carton Skip"].ToString().EqualString("Y") == false)
                {
                    if (MyUtility.Check.Empty(this.printData.Rows[i]["Carton Act. Complete"]))
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 77];
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(this.printData.Rows[i]["Carton Reqd Complete"].ToString()), DateTime.Parse(this.printData.Rows[i]["Carton Act. Complete"].ToString())) < 0)
                    {
                        range = (Excel.Range)objSheet.Cells[i + 3, 77];
                        range.Interior.ColorIndex = 3;
                    }
                }
            }

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
