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
        int selectindex = 0;
        decimal months;
        string factory, mdivision;
        DateTime? sewingDate1, sewingDate2, sciDelivery1, sciDelivery2;
        DataTable printData, dtDateList;
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
            if (MyUtility.Check.Empty(sciDeliveryRange.Value1))
            {
                MyUtility.Msg.WarningBox(" < Sci Delivery > can't be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(sewingDateRange.Value1))
            {
                MyUtility.Msg.WarningBox(" < Sewing Date > can't be empty!!");
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
,(select max(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date<dateadd(day,-45,o.SciDelivery)) as [PP Sample Material Arrival Reqd.]
,IIF(o.MTLExport = 'OK',o.MTLETA,null) as [PP Sample Material Arrival Act.]
,iif(x1.SampleApv is null,'Y','') as [PP Sample Material Arrival Skip]

,(select max(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date<dateadd(day,-2,o.Sewinline)) [Sample approval Reqd.]
,x1.SampleApv [Sample approval Act.]
,iif(x1.SampleApv is null,'Y','') [Sample approval Skip]  
,o.KPICMPQ as [CMPQ confirm Reqd.]
,o.CMPQDate as [CMPQ confirm Act.]
,o.KPIMNOTICE as [M/Notice Approve Reqd.]
,isnull(o.MnorderApv,o.SMnorderApv) as [M/Notice Approve Act.]
,o.KPIEachConsApprove [Each cons. Approve Reqd.]
,o.EachConsApv  [Each cons. Approve Act.]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='02') as [Sketch Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='02') as [Sketch Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='02'),'','Y') as [Sketch Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='03') as [AD/BOM/KIT Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='03') as [AD/BOM/KIT Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='03'),'','Y') as [AD/BOM/KIT Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='04') as [Sample Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='04') as [Sample Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='04'),'','Y') as [Sample Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='05') as [Mockup (Printing) Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='05') as [Mockup (Printing) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='05'),'','Y') as [Mockup (Printing) Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='06') as [Mockup (Embroidery) Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='06') as [Mockup (Embroidery) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='06'),'','Y') as [Mockup (Embroidery) Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='08') as [Mockup (Heat transfer) Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='08') as [Mockup (Heat transfer) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='08'),'','Y') as [Mockup (Heat transfer) Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='07') as [Mockup (Emboss/Deboss) Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='07') as [Mockup (Emboss/Deboss) Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='07'),'','Y') as [Mockup (Emboss/Deboss) Skip]

,(select sp.ProvideDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='01') as [Trim card Reqd.]
,(select sp.FtyLastDate from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='01') as [Trim card Act.]
,iif(exists(select * from dbo.Style_ProductionKits sp where ukey =o.StyleUkey and doc='01'),'','Y') as [Trim card Skip]


,o.KPIEachConsApprove [Bulk marker request Reqd]
,(select min(sm.AddDate) from dbo.SMNotice sm inner join dbo.Marker_Send ms on ms.PatternSMID = sm.ID where sm.StyleUkey = o.StyleUkey) [Bulk marker request Act.]
,(select max(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date<dateadd(day,-15,o.SewInLine)) as [Bulk marker release Reqd]
,(select min(ms.AddDate) from dbo.SMNotice sm inner join dbo.Marker_Send ms on ms.PatternSMID = sm.ID where sm.StyleUkey = o.StyleUkey) [Bulk marker release Act.]

,o.KPILETA [Mtl LETA Reqd]
,iif(x1.MTLExport='OK',x1.MTLETA,null) [Mtl LETA Act.]
,(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )) [Fabric receiving Reqd]
,(select max(iss.IssueDate) from dbo.PO_Supp_Detail p inner join dbo.fabric f on f.SCIRefno=p.SCIRefno inner join MtlType m on m.id = f.MtlTypeID 
	inner join Issue_Detail issd on issd.POID = p.id and issd.seq1 = p.seq1 and issd.seq2 = p.seq2 
	inner join Issue iss on iss.id = issd.id 
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType='F'
	and not exists(select * from dbo.PO_Supp_Detail p2 inner join dbo.fabric f2 on f2.SCIRefno=p2.SCIRefno 
	inner join MtlType m2 on m2.id = f2.MtlTypeID  
	where m2.IssueType!='Packing'  and p2.id = o.POID and p2.FabricType='F'  and p2.Complete =0  )) [Fabric receiving Act.]
,(select 'Y' where exists (select * from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType = 'F' )) [Fabric receiving Skip]
,(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )) [Accessory receiving Reqd]
,(select max(iss.IssueDate) from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID 
	inner join Issue_Detail issd on issd.POID = p.id and issd.seq1 = p.seq1 and issd.seq2 = p.seq2 
	inner join Issue iss on iss.id = issd.id 
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F'
	and not exists(select * from dbo.PO_Supp_Detail p3 inner join dbo.fabric f3 on f3.SCIRefno=p3.SCIRefno 
					inner join MtlType m3 on m3.id = f3.MtlTypeID  
					where m3.IssueType != 'Packing'  and p3.id = o.POID and p3.FabricType != 'F'  and p3.Complete =0  )) [Accessory receiving Act.]
,(select 'Y' where exists (select * from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType!='Packing' and p.id = o.POID and p.FabricType!='F' )) [Accessory receiving Skip]
,(select o.KPILETA where exists (select * from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType='Packing' and p.id = o.POID )) [Packing material receiving Reqd]
,(select max(iss.IssueDate) from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID 
	inner join Issue_Detail issd on issd.POID = p.id and issd.seq1 = p.seq1 and issd.seq2 = p.seq2 
	inner join Issue iss on iss.id = issd.id 
	where m.IssueType='Packing' and p.id = o.POID and not exists(select * from dbo.PO_Supp_Detail p1 inner join dbo.fabric f1 on f1.SCIRefno=p1.SCIRefno 
	inner join MtlType m1 on m1.id = f1.MtlTypeID  where m1.IssueType='Packing' and p1.id = o.POID and p1.Complete =0  )) [Packing material receiving Act.]
,(select 'Y' where exists (select * from dbo.PO_Supp_Detail p 
	inner join dbo.fabric f on f.SCIRefno=p.SCIRefno 
	inner join MtlType m on m.id = f.MtlTypeID  
	where m.IssueType='Packing' and p.id = o.POID )) [Packing material receiving Skip]
,(select min(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date>dateadd(day,5,o.MTLETA)) as [Material Inspection Reqd]
,(select max(inspectDate) from (
	select max(f.PhysicalDate) inspectDate from dbo.FIR f
	 where poid=o.POID and f.PhysicalEncode = 1
	union all
	select max(AddDate) from dbo.fir f 
	where poid=o.POID and f.Nonphysical = 1
	union all
	select max(a.InspDate) from dbo.AIR a where poid=o.POID and a.Status = 'Confirmed' ) t
  ) [Material Inspection Act.]
,(select max(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date<dateadd(day,-5,o.SewInLine)) [Cutting inline Reqd Complete]
,o.CutInLine [Cutting Inline Act.]
,o.CutOffLine [Cutting Offline Act.]
,o.SewInLine [Sewing Inline Act.]
,o.SewOffLine [Sewing Offline Act.]
,iif(s.NoNeedPPMeeting=1,(select max(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date<dateadd(day,-1,o.Sewinline)),null) as [PPMeeting Reqd Complete]
,iif(s.NoNeedPPMeeting=1,s.PPMeeting,null) as [PPMeeting Act. Complete]
,iif(s.NoNeedPPMeeting=1,'Y','') [PPMeeting Skip]
,x3.min_outputDate as [Wahsing Reqd Complete]
,x3.max_garmentInspectDate as [Washing Act. Complete]
,iif(x3.max_garmentInspectDate is null,'Y','') as [Wahsing Skip]
,(select min(date) from dbo.WorkHour a where FactoryID = o.FactoryID and a.Hours > 0 and a.date>dateadd(day,1,o.SewOffLine)) as [Carton Reqd Complete]
,(select max(pd.ReceiveDate) from  dbo.PackingList_Detail pd  where pd.OrderID =o.ID ) as [Carton Act. Complete]
,'' -- 全面使用clog了，所以都是空白。
,iif(o.PulloutComplete=1,o.ActPulloutDate,null) as [Garment Act. Complete]
from dbo.orders o
inner join dbo.Style s on s.Ukey = o.StyleUkey
outer apply (select top 1 o1.SciDelivery, o.MTLExport, o.MTLETA, s.SampleApv  
				from dbo.orders o1 
				inner join dbo.style s1 on s1.ukey = o1.StyleUkey
				where o1.StyleUkey = o.StyleUkey and o.OrderTypeID in ('PP SAMPLE           ','MEGO    ','PP SMP              ')) x1
outer apply (select max(gd.inspdate) max_garmentInspectDate, min(s.OutputDate) min_outputDate
			from dbo.system,dbo.GarmentTest g 
			INNER JOIN dbo.GarmentTest_Detail gd on gd.ID = g.ID
			left join (dbo.SewingOutput_Detail_Detail sdd 
			inner join dbo.SewingOutput s on s.id = sdd.ID)  on sdd.OrderId = g.orderid and sdd.Article = g.Article
			where g.OrderID = o.ID) x3
where o.qty > 0 and o.junk = 0 and o.LocalOrder = 0
"));

            #region --- 條件組合  ---
            condition.Clear();
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" AND o.scidelivery between '{0:d}' and '{1:d}'",
                sciDelivery1, sciDelivery2));
            }
            if (!MyUtility.Check.Empty(sewingDate1))
            {
                sqlCmd.Append(string.Format(@" AND (O.sewoffline !< '{0:d}' or o.sewInline !> '{1:d}')",
                sewingDate1, sewingDate2));
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
                    if (MyUtility.Check.Empty(printData.Rows[i]["PP Sample Material Arrival Act."]) 
                        || DateTime.Compare(DateTime.Parse(printData.Rows[i]["PP Sample Material Arrival Reqd."].ToString()),DateTime.Parse(printData.Rows[i]["PP Sample Material Arrival Act."].ToString())) < 0
                      )
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 9]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Sample approval Reqd."]))
                {
                    if (printData.Rows[i]["Sample approval Reqd."].ToString().CompareTo(printData.Rows[i]["Sample approval Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Sample approval Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 12]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["CMPQ confirm Reqd."]))
                {
                    if (printData.Rows[i]["CMPQ confirm Reqd."].ToString().CompareTo(printData.Rows[i]["CMPQ confirm Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["CMPQ confirm Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 15]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["M/Notice Approve Reqd."]))
                {
                    if (printData.Rows[i]["M/Notice Approve Reqd."].ToString().CompareTo(printData.Rows[i]["M/Notice Approve Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["M/Notice Approve Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 17]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Each cons. Approve Reqd."]))
                {
                    if (printData.Rows[i]["Each cons. Approve Reqd."].ToString().CompareTo(printData.Rows[i]["Each cons. Approve Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Each cons. Approve Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 19]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Sketch Reqd."]))
                {
                    if (printData.Rows[i]["Sketch Reqd."].ToString().CompareTo(printData.Rows[i]["Sketch Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Sketch Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 21]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["AD/BOM/KIT Reqd."]))
                {
                    if (printData.Rows[i]["AD/BOM/KIT Reqd."].ToString().CompareTo(printData.Rows[i]["AD/BOM/KIT Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["AD/BOM/KIT Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 24]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Sample Reqd."]))
                {
                    if (printData.Rows[i]["Sample Reqd."].ToString().CompareTo(printData.Rows[i]["Sample Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Sample Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 27]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Printing) Reqd."]))
                {
                    if (printData.Rows[i]["Mockup (Printing) Reqd."].ToString().CompareTo(printData.Rows[i]["Mockup (Printing) Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Mockup (Printing) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 30]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Embroidery) Reqd."]))
                {
                    if (printData.Rows[i]["Mockup (Embroidery) Reqd."].ToString().CompareTo(printData.Rows[i]["Mockup (Embroidery) Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Mockup (Embroidery) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 33]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Heat transfer) Reqd."]))
                {
                    if (printData.Rows[i]["Mockup (Heat transfer) Reqd."].ToString().CompareTo(printData.Rows[i]["Mockup (Heat transfer) Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Mockup (Heat transfer) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 36]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."]))
                {
                    if (printData.Rows[i]["Mockup (Emboss/Deboss) Reqd."].ToString().CompareTo(printData.Rows[i]["Mockup (Emboss/Deboss) Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Mockup (Emboss/Deboss) Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 39]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Trim card Reqd."]))
                {
                    if (printData.Rows[i]["Trim card Reqd."].ToString().CompareTo(printData.Rows[i]["Trim card Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Trim card Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 42]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Bulk marker request Reqd"]))
                {
                    if (printData.Rows[i]["Bulk marker request Reqd"].ToString().CompareTo(printData.Rows[i]["Bulk marker request Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Bulk marker request Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 45]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Bulk marker release Reqd"]))
                {
                    if (printData.Rows[i]["Bulk marker release Reqd"].ToString().CompareTo(printData.Rows[i]["Bulk marker release Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Bulk marker release Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 47]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Mtl LETA Reqd"]))
                {
                    if (printData.Rows[i]["Mtl LETA Reqd"].ToString().CompareTo(printData.Rows[i]["Mtl LETA Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Mtl LETA Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 49]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Fabric receiving Reqd"]))
                {
                    if (printData.Rows[i]["Fabric receiving Reqd"].ToString().CompareTo(printData.Rows[i]["Fabric receiving Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Fabric receiving Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 51]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Accessory receiving Reqd"]))
                {
                    if (printData.Rows[i]["Accessory receiving Reqd"].ToString().CompareTo(printData.Rows[i]["Accessory receiving Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Accessory receiving Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 54]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Packing material receiving Reqd"]))
                {
                    if (printData.Rows[i]["Packing material receiving Reqd"].ToString().CompareTo(printData.Rows[i]["Packing material receiving Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Packing material receiving Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 57]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Material Inspection Reqd"]))
                {
                    if (printData.Rows[i]["Material Inspection Reqd"].ToString().CompareTo(printData.Rows[i]["Material Inspection Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Material Inspection Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 60]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Cutting inline Reqd Complete"]))
                {
                    if (printData.Rows[i]["Cutting inline Reqd Complete"].ToString().CompareTo(printData.Rows[i]["Cutting Inline Act."].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Cutting Inline Act."]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 62]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["PPMeeting Reqd Complete"]))
                {
                    if (printData.Rows[i]["PPMeeting Reqd Complete"].ToString().CompareTo(printData.Rows[i]["PPMeeting Act. Complete"].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["PPMeeting Act. Complete"]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 67]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Wahsing Reqd Complete"]))
                {
                    if (printData.Rows[i]["Wahsing Reqd Complete"].ToString().CompareTo(printData.Rows[i]["Washing Act. Complete"].ToString()) < 0
                        || MyUtility.Check.Empty(printData.Rows[i]["Washing Act. Complete"]))
                    {
                        range = ((Excel.Range)objSheet.Cells[i + 3, 70]);
                        range.Interior.ColorIndex = 3;
                    }
                }
                if (!MyUtility.Check.Empty(printData.Rows[i]["Carton Reqd Complete"]))
                {
                    if (MyUtility.Check.Empty(printData.Rows[i]["Carton Act. Complete"]) ||
                        DateTime.Compare(DateTime.Parse( printData.Rows[i]["Carton Reqd Complete"].ToString()),DateTime.Parse(printData.Rows[i]["Carton Act. Complete"].ToString())) < 0
                        )
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
