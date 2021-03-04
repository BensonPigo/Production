using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

using Sci.Data;
using Ict;
using Sci.Win;
using System.Xml.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;
using Sci.Production.Prg;
using System.Runtime.InteropServices;

// from trade  planning R14
namespace Sci.Production.Centralized
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Win.Tems.PrintForm
    {
        private string temfile;
        private Microsoft.Office.Interop.Excel.Application excel = null;

        private string gstrMRTeam = string.Empty;

        private string gstrCategory = string.Empty;

        private System.Data.DataTable gdtData1o;
        private System.Data.DataTable gdtData2o;
        private System.Data.DataTable gdtData3o;
        private System.Data.DataTable gdtData4o;
        private System.Data.DataTable gdtData5o;
        private System.Data.DataTable gdtData6o;
        private System.Data.DataTable gdtData7o;
        private System.Data.DataTable gdtData8o;
        private System.Data.DataTable gdtData9o;
        private System.Data.DataTable gdtData1;
        private System.Data.DataTable gdtData2;
        private System.Data.DataTable gdtData3;
        private System.Data.DataTable gdtData4;
        private System.Data.DataTable gdtData5;
        private System.Data.DataTable gdtData6;
        private System.Data.DataTable gdtData7;
        private System.Data.DataTable gdtData8;
        private System.Data.DataTable gdtData9;
        private System.Data.DataTable gdtData;

        private string FtyZone;

        /// <summary>
        /// R03
        /// </summary>
        public R03()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.print.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.Text = PrivUtilsPMS.GetVersion(this.Text);
            DualResult result;
            base.OnFormLoaded();
            this.comboDropDownListCategory.SelectedIndex = 0;
            this.comboFtyZone.SetDataSource();
            #region 取得 MR Team 資料
            System.Data.DataTable dt_ref = null;
            string sql = @"select * from Department WITH (NOLOCK) where Department.Type = 'MR'";
            result = DBProxy.Current.Select("Trade", sql, out dt_ref);
            if (dt_ref != null && dt_ref.Rows.Count > 0)
            {
                this.comboBox1.Add("ALL", string.Empty);
                for (int j = 0; j < dt_ref.Rows.Count; j++)
                {
                    DataRow dr2 = dt_ref.Rows[j];
                    this.comboBox1.Add(dr2["Name"].ToString(), dr2["ID"].ToString());
                }

                this.comboBox1.SelectedIndex = 0;
            }
            #endregion
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.gstrCategory = this.comboDropDownListCategory.SelectedValue.ToString();
            this.FtyZone = this.comboFtyZone.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            DualResult result = Ict.Result.True;
            if (this.excel == null)
            {
                return true;
            }

            this.gdtData1 = null;
            this.gdtData2 = null;
            this.gdtData3 = null;
            this.gdtData4 = null;
            this.gdtData5 = null;
            this.gdtData6 = null;
            this.gdtData7 = null;
            this.gdtData8 = null;
            this.gdtData9 = null;

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd;

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            DualResult result = new DualResult(true);
            try
            {
                string strSQL = @"

Select o.ID, o.ProgramID, o.StyleID, o.SeasonID
, [BrandID] = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end
, o.FactoryID
,o.POID , o.Category, o.CdCodeID 
,o.CPU
,CPURate = (SELECT * FROM GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'Order')) * o.CPU  
,o.BuyerDelivery, o.SCIDelivery
,so.SewingLineID 
,so.ManPower
,sod.ComboType
,sod.WorkHour
,sod.QAQty 
,QARate = sod.QAQty * isnull([dbo].[GetOrderLocation_Rate](o.id ,sod.ComboType)/100,1)
,Round(sod.WorkHour * so.ManPower,2) as TotalManHour 
,CDDesc = c.Description 
,StyleDesc = s.Description
,s.ModularParent, s.CPUAdjusted
,OutputDate,Shift, Team
,SCategory = so.Category, CPUFactor
, [FtyZone]=f.FtyZone
,orderid
,Rate = isnull([dbo].[GetOrderLocation_Rate]( o.id ,sod.ComboType)/100,1) 
,ActManPower= so.Manpower
,c.ProductionFamilyID
into #stmp
from Orders o WITH (NOLOCK) 
    inner join SewingOutput_Detail sod WITH (NOLOCK) on sod.OrderId = o.ID
    inner join SewingOutput so WITH (NOLOCK) on so.ID = sod.ID and so.Shift <> 'O'  
    inner join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
    inner join CDCode c WITH (NOLOCK) on c.ID = o.CdCodeID
	inner join Factory f WITH (NOLOCK) on o.FactoryID=f.id
    inner join Brand b WITH (NOLOCK) on o.BrandID=b.ID
outer apply( select BrandID from orders o1 where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Style where id = o.StyleID 
    and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
Where 1=1

and f.IsProduceFty = '1'
--排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
and ((o.LocalOrder = 1 and o.SubconInType in ('1','2')) or (o.LocalOrder = 0 and o.SubconInType in ('0','3')))
";
                if (this.dateRange1.Value1.HasValue)
                {
                    strSQL += string.Format(" and so.OutputDate >= '{0}'", ((DateTime)this.dateRange1.Value1).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange1.Value2.HasValue)
                {
                    strSQL += string.Format(" and so.OutputDate <= '{0}'", ((DateTime)this.dateRange1.Value2).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange2.Value1.HasValue)
                {
                    strSQL += string.Format(" and o.BuyerDelivery  >= '{0}'", ((DateTime)this.dateRange2.Value1).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange2.Value2.HasValue)
                {
                    strSQL += string.Format(" and o.BuyerDelivery  <= '{0}'", ((DateTime)this.dateRange2.Value2).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange3.Value1.HasValue)
                {
                    strSQL += string.Format(" and o.SCIDelivery  >= '{0}'", ((DateTime)this.dateRange3.Value1).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange3.Value2.HasValue)
                {
                    strSQL += string.Format(" and o.SCIDelivery  <= '{0}'", ((DateTime)this.dateRange3.Value2).ToString("yyyy-MM-dd"));
                }

                // if (this.txtSeason1.Text != string.Empty)
                // {
                //    strSQL += string.Format(" AND o.SeasonID = '{0}' ", this.txtSeason1.Text);
                // }
                if (this.txtmultiSeason.Text != string.Empty)
                {
                    strSQL += string.Format(" AND o.SeasonID IN ('{0}') ", this.txtmultiSeason.Text.Replace(",", "','"));
                }

                if (this.txtBrand1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND o.BrandID = '{0}' ", this.txtBrand1.Text);
                }

                if (this.txtstyle1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND o.StyleID = '{0}' ", this.txtstyle1.Text);
                }

                if (this.gstrMRTeam != string.Empty)
                {
                    strSQL += string.Format(" AND b.MRTeam = '{0}' ", this.gstrMRTeam);
                }

                if (this.txtCentralizedFactory1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND o.FactoryID = '{0}' ", this.txtCentralizedFactory1.Text);
                }

                if (this.gstrCategory != string.Empty)
                {
                    strSQL += string.Format(" AND o.Category in ({0})", this.gstrCategory);
                }

                if (this.txtCountry1.TextBox1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND f.CountryID = '{0}' ", this.txtCountry1.TextBox1.Text);
                }

                if (this.chkType.Checked)
                {
                    strSQL += " AND f.Type <>'S' ";
                }

                if (!MyUtility.Check.Empty(this.FtyZone))
                {
                    strSQL += string.Format(" AND f.FtyZone = '{0}' ", this.FtyZone);
                }

                if (this.numNewStyleBaseOn.Value != 0)
                {
                    strSQL += string.Format(@" and dateadd(month, {0}, o.SciDelivery ) < so.OutputDate", -this.numNewStyleBaseOn.Value);
                }

                strSQL += @"
select OutputDate
,Category
, Shift
, SewingLineID
, Team
, OrderId 
, ComboType
, SCategory
, FactoryID
, ProgramID
, CPU
, CPUFactor
, StyleID
, Rate
, FtyZone
, ProductionFamilyID
, QAQty = sum(QAQty)
, ActManPower= ActManPower
, WorkHour = sum(Round(WorkHour,3))
into #stmp2		
from #stmp
group by OutputDate, Category, Shift, SewingLineID, Team, orderid, ComboType, SCategory, FactoryID
, ProgramID, CPU, CPUFactor, StyleID, Rate,FtyZone, ProductionFamilyID, ActManPower

select 
a.ID, a.ProgramID, a.StyleID, a.SeasonID, a.BrandID , a.FtyZone,a.FactoryID, a.POID , a.Category, a.CdCodeID 
, CPU = sum(a.CPU)
, CPURate = sum(a.CPURate)
, a.BuyerDelivery, a.SCIDelivery, a.SewingLineID , a.ComboType
, ManPower = sum(a.ManPower)
, WorkHour = sum(Round(a.WorkHour,2)) 
, QARate = convert(numeric(12,2)
, sum(a.QARate))
, TotalManHour =
    (select sum(ROUND( ActManPower * WorkHour, 2)) 
	from #stmp2 f 
	where f.FtyZone = a.FtyZone and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID 
	and f.orderid = a.orderid
	and f.CPU = a.CPU and f.CPUFactor = a.CPUFactor and f.Rate = a.Rate
	and f.OutputDate=a.OutputDate and f.Category = a.Category and f.Shift = a.Shift and f.Team = a.Team and f.ComboType = a.ComboType and f.SCategory = a.SCategory)
, a.CDDesc
, a.StyleDesc
, a.ModularParent
, a.ProductionFamilyID
, CPUAdjusted = sum(a.CPUAdjusted)
,QAQty = sum(a.QAQty) 
,TotalCPUOut =
	(select sum(Round(CPU * CPUFactor * Rate * QAQty,2))  
	from #stmp2 f 
	where f.FtyZone = a.FtyZone and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID 
	and f.orderid = a.orderid
	and f.CPU = a.CPU and f.CPUFactor = a.CPUFactor and f.Rate = a.Rate
	and f.OutputDate=a.OutputDate and f.Category = a.Category and f.Shift = a.Shift and f.Team = a.Team and f.ComboType = a.ComboType and f.SCategory = a.SCategory)
,a.OutputDate
into #tmpz
from #stmp a
group by a.ID, a.ProgramID, a.StyleID, a.SeasonID, a.BrandID , a.FactoryID, a.POID , a.Category, a.CdCodeID ,a.BuyerDelivery, a.SCIDelivery, a.SewingLineID 
, a.CDDesc, a.StyleDesc,a.ComboType,a.ModularParent, a.ProductionFamilyID,a.OutputDate, Category, Shift, SewingLineID, Team, orderid, ComboType, SCategory, FactoryID, ProgramID, CPU, CPUFactor, StyleID, Rate,FtyZone
";
                #region 1.  By Factory
                string strFactory = string.Format(@"{0} Select A=FtyZone,B=FactoryID, QARate, TotalCPUOut, TotalManHour FROM #tmpz ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFactory, null, out this.gdtData);
                    if (this.gdtData1o == null)
                    {
                        this.gdtData1o = this.gdtData.Clone();
                    }

                    this.gdtData1o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"select A,B,C=sum(QARate),D=sum(TotalCPUOut),E=sum(TotalManHour)
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
from #tmp Group BY A,B order by A,B ";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData1o, string.Empty, sqlcmd, out this.gdtData1);
                #endregion 1.   By Factory

                #region 2.  By Brand
                string strBrand = string.Format(@"{0}  Select BrandID AS A, QARate,TotalCPUOut,TotalManHour FROM #tmpz ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strBrand, null, out this.gdtData);
                    if (this.gdtData2o == null)
                    {
                        this.gdtData2o = this.gdtData.Clone();
                    }

                    this.gdtData2o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"select A,B=sum(QARate),C=sum(TotalCPUOut),D=sum(TotalManHour),
E=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2),
F=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) from #tmp Group BY A order by A";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData2o, string.Empty, sqlcmd, out this.gdtData2);
                #endregion 2.   By Brand

                #region 3.  By Brand + Factory
                string strFBrand = string.Format(
                    @"
{0} 
Select A=BrandID, B=FtyZone,C=FactoryID, QARate, TotalCPUOut,TotalManHour
FROM #tmpz ",
                    strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFBrand, null, out this.gdtData);
                    if (this.gdtData3o == null)
                    {
                        this.gdtData3o = this.gdtData.Clone();
                    }

                    this.gdtData3o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"select A,B,C,D=sum(QARate),E=sum(TotalCPUOut),F=sum(TotalManHour)
,G=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,H=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)  
from #tmp Group BY A,B,C order by A,B,C";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData3o, string.Empty, sqlcmd, out this.gdtData3);
                #endregion 3.   By Brand + Factory

                #region 4.  By Style
                string strStyle = string.Format(
                    @"
{0} 
Select StyleID AS A, BrandID AS B, CDCodeID AS C, CDDesc AS D, StyleDesc AS E, SeasonID AS F
, QARate, TotalCPUOut,TotalManHour, ModularParent AS L, CPUAdjusted AS M, Category, OutputDate, SewingLineID, FactoryID
FROM #tmpz ",
                    strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strStyle, null, out this.gdtData);
                    if (this.gdtData4o == null)
                    {
                        this.gdtData4o = this.gdtData.Clone();
                    }

                    this.gdtData4o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"
alter table #tmp alter column A  varchar(15)
alter table #tmp alter column B  varchar(8)
alter table #tmp alter column E  varchar(100)

select A,B,E,F,OutputDate = max(OutputDate)
into #tmp_MaxOutputDate
from #tmp 
group by A,B,E,F

select A,B,C,D,E,F
,G=sum(QARate)
,H=sum(TotalCPUOut),I=sum(TotalManHour)
,J=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)
,K=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
,L,M
,N= Stuff((select   concat('/', a.FactoryID + ' ' + (case     when Max(a.OutputDate) is null then 'New Style'
                                                               when sum(iif(a.Category = 'S',1,0)) > 0 AND sum(iif(a.Category = 'B',1,0)) = 0 then 'New Style'
                                                               else concat((Stuff((
																	select distinct concat(' ', t.SewingLineID)
																	from #tmp t
																	where t.A = #tmp.A
                                                                    and t.B = #tmp.B
                                                                    and t.E = #tmp.E
                                                                    and t.F = #tmp.F
																	and exists (select 1 from #tmp_MaxOutputDate t2
																				where t2.A = t.A 
																				and t2.B = t.B 
																				and t2.E = t.E
																				and t2.F = t.F
																				and t2.OutputDate = t.OutputDate)
																	FOR XML PATH('')) ,1,1,'')),'(',format(Max(a.OutputDate), 'yyyy/MM/dd'),')')
                                                               end))
                from #tmp a where   a.A = #tmp.A and 
                                    a.B = #tmp.B and
                                    a.E = #tmp.E and
                                    a.F = #tmp.F
                group by a.FactoryID FOR XML PATH(''))
        ,1,1,'') 
from #tmp 
Group BY A,B,C,D,E,F,L,M 
order by A,B,C,E";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData4o, string.Empty, sqlcmd, out this.gdtData4);
                #endregion 4.   By Style

                #region 5.  By CD
                string strCdCodeID = string.Format(@"{0}  Select CdCodeID AS A, CDDesc AS B, QARate, TotalCPUOut,TotalManHour FROM #tmpz ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strCdCodeID, null, out this.gdtData);
                    if (this.gdtData5o == null)
                    {
                        this.gdtData5o = this.gdtData.Clone();
                    }

                    this.gdtData5o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"select A,B,C=sum(QARate),D=sum(TotalCPUOut),E=sum(TotalManHour)
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
from #tmp Group BY A,B order by A";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData5o, string.Empty, sqlcmd, out this.gdtData5);
                #endregion 5.   By CD

                #region 6.  By Factory Line
                string strFactoryLine = string.Format(@"{0}  Select A=FtyZone,B=FactoryID,C=SewingLineID, QARate, TotalCPUOut,TotalManHour FROM #tmpz ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFactoryLine, null, out this.gdtData);
                    if (this.gdtData6o == null)
                    {
                        this.gdtData6o = this.gdtData.Clone();
                    }

                    this.gdtData6o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"select A,B,C, Sum(QARate) AS D, Sum(TotalCPUOut) AS E, SUM(TotalManHour) AS F
,G=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,H=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C order by A,B,C";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData6o, string.Empty, sqlcmd, out this.gdtData6);
                #endregion 6.   By Factory Line

                #region 7.  By Factory, Brand , CDCode
                string strFBCDCode = string.Format(
                    @"{0}  Select A=BrandID, B=FtyZone,C=FactoryID, D=CdCodeID,E=CDDesc, QARate, TotalCPUOut,TotalManHour
FROM #tmpz  ",
                    strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFBCDCode, null, out this.gdtData);
                    if (this.gdtData7o == null)
                    {
                        this.gdtData7o = this.gdtData.Clone();
                    }

                    this.gdtData7o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"select A,B,C,D,E,F=sum(QARate),G=sum(TotalCPUOut),H=sum(TotalManHour)
,I=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,J=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D,E order by A,B,C,D,E";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData7o, string.Empty, sqlcmd, out this.gdtData7);
                #endregion 7.   By Factory, Brand , CDCode

                #region 8.  By PO Combo
                string strPOCombo = string.Format(
                    @"
{0} 
Select A=FtyZone,POID AS B, StyleID AS C, BrandID AS D, CdCodeID AS E, CDDesc AS F,G=ProductionFamilyID, StyleDesc AS H, SeasonID AS I, ProgramID AS J, QARate, TotalCPUOut, TotalManHour, Category, OutputDate, SewingLineID
FROM #tmpz  ",
                    strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strPOCombo, null, out this.gdtData);
                    if (this.gdtData8o == null)
                    {
                        this.gdtData8o = this.gdtData.Clone();
                    }

                    this.gdtData8o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"
alter table #tmp alter column B  varchar(13)

select B,C,D,H,I,OutputDate = max(OutputDate)
into #tmp_MaxOutputDate
from #tmp 
group by B,C,D,H,I

select A,B,C,D,E,F,G,H,I,J
,K=sum(QARate),L=sum(TotalCPUOut),M=sum(TotalManHour)
,N=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,O=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
,P= case    when Max(OutputDate) is null then 'New Style'
            when sum(iif(Category = 'S',1,0)) > 0 AND sum(iif(Category = 'B',1,0)) = 0 then 'New Style'
            else concat((Stuff((
							select distinct concat(' ', t.SewingLineID)
							from #tmp t
							where t.C = #tmp.C
							and t.D = #tmp.D
							and t.H = #tmp.H
                            and t.I = #tmp.I
                            and t.B = #tmp.B
							and exists (select 1 from #tmp_MaxOutputDate t2
										where t2.C = t.C
										and t2.D = t.D 
										and t2.H = t.H
										and t2.I = t.I
										and t2.B = t.B
										and t2.OutputDate = t.OutputDate)
							FOR XML PATH('')) ,1,1,'')),'(',format(Max(OutputDate), 'yyyy/MM/dd'),')')
            end
from #tmp 
Group BY A,B,C,D,E,F,G,H,I,J 
order by A,B,C,D,G";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData8o, string.Empty, sqlcmd, out this.gdtData8);
                #endregion 8.   By PO Combo

                #region 9.  By Program
                string strProgram = string.Format(@"{0}  Select ProgramID AS A, StyleID AS B, C=FtyZone,D=FactoryID, BrandID AS E, CdCodeID AS F, CDDesc AS G,H=ProductionFamilyID, StyleDesc AS I, SeasonID AS J, QARate,TotalCPUOut, TotalManHour, Category, OutputDate, SewingLineID FROM #tmpz ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strProgram, null, out this.gdtData);
                    if (this.gdtData9o == null)
                    {
                        this.gdtData9o = this.gdtData.Clone();
                    }

                    this.gdtData9o.Merge(this.gdtData);
                    if (!result)
                    {
                        return result;
                    }
                }

                sqlcmd = @"
select B,E,I,J,OutputDate = max(OutputDate)
into #tmp_MaxOutputDate
from #tmp 
group by B,E,I,J

select A,B,C,D,E,F,G,H,I,J
,K=sum(QARate),L=sum(TotalCPUOut),M=sum(TotalManHour)
,N=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)
,O=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
,P= case    when Max(OutputDate) is null then 'New Style'
            when sum(iif(Category = 'S',1,0)) > 0 AND sum(iif(Category = 'B',1,0)) = 0 then 'New Style'
            else concat((Stuff((
							select distinct concat(' ', t.SewingLineID)
							from #tmp t
							where t.B = #tmp.B
							and t.E = #tmp.E
							and t.I = #tmp.I
							and t.J = #tmp.J
							and exists (select 1 from #tmp_MaxOutputDate t2
										where t2.B = t.B
										and t2.E = t.E 
										and t2.I = t.I
										and t2.J = t.J
										and t2.OutputDate = t.OutputDate)
							FOR XML PATH('')) ,1,1,'')),'(',format(Max(OutputDate), 'yyyy/MM/dd'),')')
            end
from #tmp 
Group BY A,B,C,D,E,F,G,H,I,J 
order by A,B,C,D,E,F,H";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData9o, string.Empty, sqlcmd, out this.gdtData9);
                #endregion 9.   By Program

                if (((this.gdtData1 != null) && (this.gdtData1.Rows.Count > 0)) || ((this.gdtData2 != null) && (this.gdtData2.Rows.Count > 0)) || ((this.gdtData3 != null) && (this.gdtData3.Rows.Count > 0))
                     || ((this.gdtData4 != null) && (this.gdtData4.Rows.Count > 0)) || ((this.gdtData5 != null) && (this.gdtData5.Rows.Count > 0)) || ((this.gdtData6 != null) && (this.gdtData6.Rows.Count > 0))
                     || ((this.gdtData7 != null) && (this.gdtData7.Rows.Count > 0)) || ((this.gdtData8 != null) && (this.gdtData8.Rows.Count > 0)) || ((this.gdtData9 != null) && (this.gdtData9.Rows.Count > 0)))
                {
                    if (!(result = this.TransferToExcel()))
                    {
                        return result;
                    }
                }
                else
                {
                    return new DualResult(false, "Datas not found!");
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }

            return result;
        }

        private DualResult TransferToExcel()
        {
            string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            DualResult result = Ict.Result.True;
            string strPath = PrivUtilsPMS.GetPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
            this.temfile = strPath + @"\Centralized-R03.Prod. Efficiency Analysis Report.xltx";

            try
            {
                if (!(result = PrivUtilsPMS.Excels.CreateExcel(this.temfile, out this.excel)))
                {
                    return result;
                }

                Worksheet wsSheet;
                #region 1.  By Factory
                int intRowsCount = this.gdtData1.Rows.Count;
                int intRowsStart = 2; // 匯入起始位置
                int rownum = intRowsStart; // 每筆資料匯入之位置
                int intColumns = 7; // 匯入欄位數
                if ((this.gdtData1 != null) && (this.gdtData1.Rows.Count > 0))
                {
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[1];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData1.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData1.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 1.   By Factory

                #region 2.  By Brand
                if ((this.gdtData2 != null) && (this.gdtData2.Rows.Count > 0))
                {
                    intColumns = 6; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[2];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData2.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData2.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:F{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 2.   By Brand

                #region 3.  By Brand-Factory
                if ((this.gdtData3 != null) && (this.gdtData3.Rows.Count > 0))
                {
                    intColumns = 8; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[3];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData3.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData3.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:H{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 3.   By Brand-Factory

                #region 4.  By Style
                if ((this.gdtData4 != null) && (this.gdtData4.Rows.Count > 0))
                {
                    intColumns = 14; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[4];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData4.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData4.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:N{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 4.   By Style

                #region 5.  By CD
                if ((this.gdtData5 != null) && (this.gdtData5.Rows.Count > 0))
                {
                    intColumns = 7; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[5];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData5.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData5.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 5.   By CD

                #region 6.  By Factory Line
                if ((this.gdtData6 != null) && (this.gdtData6.Rows.Count > 0))
                {
                    intColumns = 8; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[6];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData6.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData6.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:H{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 6.   By Factory Line

                #region 7.  By Brand-Factory-CD
                if ((this.gdtData7 != null) && (this.gdtData7.Rows.Count > 0))
                {
                    intColumns = 10; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[7];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData7.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData7.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:J{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 7.   By Brand-Factory-CD

                #region 8.  By PO Combo
                if ((this.gdtData8 != null) && (this.gdtData8.Rows.Count > 0))
                {
                    intColumns = 16; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[8];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData8.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData8.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:P{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 8.   By PO Combo

                #region 9.  By Program
                if ((this.gdtData9 != null) && (this.gdtData9.Rows.Count > 0))
                {
                    intColumns = 16; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[9];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData9.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData9.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:P{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 9.   By Program

                #region Save & Show Excel
                this.excel.Visible = true;
                Workbook workbook = this.excel.Workbooks[1];
                string strExcelName = Class.MicrosoftFile.GetName("Centralized-R03.Prod. Efficiency Analysis Report");
                workbook.SaveAs(strExcelName);
                workbook.Close();
                this.excel.Quit();
                Marshal.ReleaseComObject(this.excel);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            catch (Exception ex)
            {
                if (this.excel != null)
                {
                    this.excel.DisplayAlerts = false;
                    this.excel.Quit();
                }

                this.Clear();
                return new DualResult(false, "Export excel error.", ex);
            }

            this.Clear();
            return result;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gstrMRTeam = this.comboBox1.SelectedIndex == -1 ? string.Empty : this.comboBox1.SelectedValue2.ToString();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Clear()
        {
            this.gdtData1o = null;
            this.gdtData2o = null;
            this.gdtData3o = null;
            this.gdtData4o = null;
            this.gdtData5o = null;
            this.gdtData6o = null;
            this.gdtData7o = null;
            this.gdtData8o = null;
            this.gdtData9o = null;
            return;
        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {
        }
    }
}
