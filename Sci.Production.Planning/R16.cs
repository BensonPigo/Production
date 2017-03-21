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
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Planning
{
    public partial class R16 : Sci.Win.Tems.PrintForm
    {
        //int selectindex = 0;
        //decimal months;
        string factory, mdivision;
        DateTime? sewingDate1, sewingDate2, sciDelivery1, sciDelivery2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();
        StringBuilder datelist = new StringBuilder();

        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtfactory1.Text = Sci.Env.User.Factory;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            //if (MyUtility.Check.Empty(sciDeliveryRange.Value1))
            //{
            //    MyUtility.Msg.WarningBox(" < Sci Delivery > can't be empty!!");
            //    return false;
            //}

            //if (MyUtility.Check.Empty(sewingDateRange.Value1))
            //{
            //    MyUtility.Msg.WarningBox(" < Sewing Date > can't be empty!!");
            //    return false;
            //}
            if (MyUtility.Check.Empty(sciDeliveryRange.Value1) && MyUtility.Check.Empty(sewingDateRange.Value1))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > & < Sewing Date > can't be empty!!");
                return false;
            }

            #region -- 必輸的條件 --
            sciDelivery1 = sciDeliveryRange.Value1;
            sciDelivery2 = sciDeliveryRange.Value2;
            sewingDate1 = sewingDateRange.Value1;
            sewingDate2 = sewingDateRange.Value2;
            #endregion
            mdivision = txtMdivision1.Text;
            factory = txtfactory1.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
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
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='02'),'','Y') as [Sketch Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03') as [AD/BOM/KIT Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03') as [AD/BOM/KIT Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='03'),'','Y') as [AD/BOM/KIT Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04') as [Sample Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04') as [Sample Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='04'),'','Y') as [Sample Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05') as [Mockup (Printing) Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05') as [Mockup (Printing) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='05'),'','Y') as [Mockup (Printing) Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='06') as [Mockup (Embroidery) Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='06') as [Mockup (Embroidery) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK)  where StyleUkey =o.StyleUkey and doc='06'),'','Y') as [Mockup (Embroidery) Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08') as [Mockup (Heat transfer) Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08') as [Mockup (Heat transfer) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='08'),'','Y') as [Mockup (Heat transfer) Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07') as [Mockup (Emboss/Deboss) Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07') as [Mockup (Emboss/Deboss) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='07'),'','Y') as [Mockup (Emboss/Deboss) Skip]

,(select max(sp.ProvideDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01') as [Trim card Reqd.]
,(select max(sp.FtyLastDate) from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01') as [Trim card Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp WITH (NOLOCK) where StyleUkey =o.StyleUkey and doc='01'),'','Y') as [Trim card Skip]


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
,'' -- 全面使用clog了，所以都是空白。
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
where o.qty > 0 and o.junk = 0 and o.LocalOrder = 0
"));

            #region --- 條件組合  ---
            condition.Clear();
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.scidelivery >= '{0}'", Convert.ToDateTime(sciDelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(sciDelivery2))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sewingDate1) && !MyUtility.Check.Empty(sewingDate2))
            {
                sqlCmd.Append(string.Format(@" AND (O.sewoffline !< '{0:d}' or o.sewInline !> '{1:d}')",
                sewingDate1, sewingDate2));
            }
            else
            {
                if (!MyUtility.Check.Empty(sewingDate1))
                {
                    sqlCmd.Append(string.Format(@" and O.sewoffline !< '{0}'", Convert.ToDateTime(sewingDate1).ToString("d")));
                }
                if (!MyUtility.Check.Empty(sewingDate2))
                {
                    sqlCmd.Append(string.Format(@" and o.sewInline !> '{0}'", Convert.ToDateTime(sewingDate2).ToString("d")));
                }
            }
           
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and O.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and O.FtyGroup = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            #endregion

            sqlCmd.Append(" order by o.factoryid,o.id");

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            DBProxy.Current.DefaultTimeout = 0;
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

            if (printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R16.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Planning_R16.xltx", 2, true, null, objApp);      // 將datatable copy to excel
            objApp.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet objSheet = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            ////objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            Excel.Range range = null;
            for (int i = 0; i < printData.Rows.Count; i++)  //列印動態欄位的表頭
            {
                if (!MyUtility.Check.Empty(printData.Rows[i]["PP Sample Material Arrival Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["PP Sample Material Arrival Act."])) 
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 9]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["PP Sample Material Arrival Reqd."].ToString()),DateTime.Parse(printData.Rows[i]["PP Sample Material Arrival Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 9]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Sample approval Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Sample approval Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 12]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Sample approval Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Sample approval Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 12]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["CMPQ confirm Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["CMPQ confirm Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 15]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["CMPQ confirm Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["CMPQ confirm Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 15]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["M/Notice Approve Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["M/Notice Approve Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 17]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["M/Notice Approve Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["M/Notice Approve Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 17]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Each cons. Approve Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Each cons. Approve Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 19]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Each cons. Approve Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Each cons. Approve Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 19]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Sketch Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Sketch Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 21]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Sketch Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Sketch Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 21]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["AD/BOM/KIT Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["AD/BOM/KIT Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 24]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["AD/BOM/KIT Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["AD/BOM/KIT Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 24]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Sample Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Sample Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 27]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Sample Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Sample Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 27]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Printing) Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Mockup (Printing) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 30]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Mockup (Printing) Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Mockup (Printing) Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 30]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Embroidery) Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Mockup (Embroidery) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 33]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Mockup (Embroidery) Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Mockup (Embroidery) Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 33]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Heat transfer) Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Mockup (Heat transfer) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 36]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Mockup (Heat transfer) Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Mockup (Heat transfer) Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 36]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Mockup (Emboss/Deboss) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 39]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Mockup (Emboss/Deboss) Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 39]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Trim card Reqd."]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Trim card Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 42]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Trim card Reqd."].ToString()), DateTime.Parse(printData.Rows[i]["Trim card Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 42]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Bulk marker request Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Bulk marker request Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 45]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Bulk marker request Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Bulk marker request Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 45]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Bulk marker release Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Bulk marker release Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 47]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Bulk marker release Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Bulk marker release Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 47]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Mtl LETA Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Mtl LETA Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 49]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Mtl LETA Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Mtl LETA Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 49]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Fabric receiving Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Fabric receiving Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 51]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Fabric receiving Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Fabric receiving Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 51]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Accessory receiving Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Accessory receiving Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 54]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Accessory receiving Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Accessory receiving Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 54]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Packing material receiving Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Packing material receiving Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 57]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Packing material receiving Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Packing material receiving Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 57]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Material Inspection Reqd"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Material Inspection Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 60]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Material Inspection Reqd"].ToString()), DateTime.Parse(printData.Rows[i]["Material Inspection Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 60]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Cutting inline Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Cutting Inline Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 62]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Cutting inline Reqd Complete"].ToString()), DateTime.Parse(printData.Rows[i]["Cutting Inline Act."].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 62]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["PPMeeting Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["PPMeeting Act. Complete"]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 67]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["PPMeeting Reqd Complete"].ToString()), DateTime.Parse(printData.Rows[i]["PPMeeting Act. Complete"].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 67]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Wahsing Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Washing Act. Complete"]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 70]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Wahsing Reqd Complete"].ToString()), DateTime.Parse(printData.Rows[i]["Washing Act. Complete"].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 70]);
                        range.Interior.ColorIndex = 3;
                    }
                }

                if (!MyUtility.Check.Empty(printData.Rows[i]["Carton Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Carton Act. Complete"]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 73]);
                        range.Interior.ColorIndex = 3;
                    }
                    else if (DateTime.Compare(DateTime.Parse(printData.Rows[i]["Carton Reqd Complete"].ToString()), DateTime.Parse(printData.Rows[i]["Carton Act. Complete"].ToString())) < 0)
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 73]);
                        range.Interior.ColorIndex = 3;
                    }
                }

            }
            objApp.Visible = true;


            //if (objSheet != null) Marshal.FinalReleaseComObject(objSheet);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
