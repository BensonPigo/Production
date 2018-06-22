using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using Microsoft.Office.Interop.Excel;

using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Production.Class.Commons;
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
    public partial class R03 : Sci.Win.Tems.PrintForm
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
            this.Text = PrivUtils.getVersion(this.Text);
            DualResult result;
            base.OnFormLoaded();
            this.comboDropDownListCategory.SelectedIndex = 0;

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
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DualResult result = Result.True;
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

Select Orders.ID, Orders.ProgramID, Orders.StyleID, Orders.SeasonID, Orders.BrandID , Orders.FactoryID
,Orders.POID , Orders.Category, Orders.CdCodeID 
,Orders.CPU
,CPURate = (SELECT * FROM GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'Order')) * Orders.CPU  
,Orders.BuyerDelivery, Orders.SCIDelivery
,SewingOutput.SewingLineID 
,SewingOutput.ManPower
,SewingOutput_Detail.ComboType
,SewingOutput_Detail.WorkHour
,SewingOutput_Detail.QAQty 
,QARate = SewingOutput_Detail.QAQty * isnull([dbo].[GetOrderLocation_Rate](Orders.id ,SewingOutput_Detail.ComboType)/100,1)
,Round(SewingOutput_Detail.WorkHour * SewingOutput.ManPower,2) as TotalManHour 
,CDDesc = CDCode.Description 
,StyleDesc = Style.Description
,STYLE.ModularParent, STYLE.CPUAdjusted
,OutputDate,Shift, Team
,SCategory = SewingOutput.Category, CPUFactor, Orders.MDivisionID,orderid
,Rate = isnull([dbo].[GetOrderLocation_Rate]( Orders.id ,SewingOutput_Detail.ComboType)/100,1) 
,ActManPower= IIF(SewingOutput_Detail.QAQty = 0, SewingOutput.Manpower, SewingOutput.Manpower * SewingOutput_Detail.QAQty)
into #stmp
FROM Orders WITH (NOLOCK), SewingOutput WITH (NOLOCK), SewingOutput_Detail WITH (NOLOCK) , Brand WITH (NOLOCK) , Factory WITH (NOLOCK), CDCode WITH (NOLOCK) , Style WITH (NOLOCK)
Where SewingOutput_Detail.OrderID = Orders.ID 
And SewingOutput.ID = SewingOutput_Detail.ID And SewingOutput.Shift <> 'O'  
And  Orders.BrandID = Brand.ID AND Orders.FactoryID  = Factory.ID AND Orders.CdCodeID = CDCode.ID AND Orders.StyleUkey  = Style.Ukey 
and Factory.IsProduceFty = '1'
--排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
and ((Orders.LocalOrder = 1 and Orders.SubconInSisterFty = 1) or (Orders.LocalOrder = 0 and Orders.SubconInSisterFty = 0))
";
                if (this.dateRange1.Value1.HasValue)
                {
                    strSQL += string.Format(" and SewingOutput.OutputDate >= '{0}'", ((DateTime)this.dateRange1.Value1).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange1.Value2.HasValue)
                {
                    strSQL += string.Format(" and SewingOutput.OutputDate <= '{0}'", ((DateTime)this.dateRange1.Value2).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange2.Value1.HasValue)
                {
                    strSQL += string.Format(" and Orders.BuyerDelivery  >= '{0}'", ((DateTime)this.dateRange2.Value1).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange2.Value2.HasValue)
                {
                    strSQL += string.Format(" and Orders.BuyerDelivery  <= '{0}'", ((DateTime)this.dateRange2.Value2).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange3.Value1.HasValue)
                {
                    strSQL += string.Format(" and Orders.SCIDelivery  >= '{0}'", ((DateTime)this.dateRange3.Value1).ToString("yyyy-MM-dd"));
                }

                if (this.dateRange3.Value2.HasValue)
                {
                    strSQL += string.Format(" and Orders.SCIDelivery  <= '{0}'", ((DateTime)this.dateRange3.Value2).ToString("yyyy-MM-dd"));
                }

                if (this.txtSeason1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Orders.SeasonID = '{0}' ", this.txtSeason1.Text);
                }

                if (this.txtBrand1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Orders.BrandID = '{0}' ", this.txtBrand1.Text);
                }

                if (this.txtstyle1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Orders.StyleID = '{0}' ", this.txtstyle1.Text);
                }

                if (this.gstrMRTeam != string.Empty)
                {
                    strSQL += string.Format(" AND Brand.MRTeam = '{0}' ", this.gstrMRTeam);
                }

                if (this.txtCentralizedFactory1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Orders.FactoryID = '{0}' ", this.txtCentralizedFactory1.Text);
                }

                if (this.gstrCategory != string.Empty)
                {
                    strSQL += string.Format(" AND Orders.Category in ({0})", this.gstrCategory);
                }

                if (this.txtCountry1.TextBox1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Factory.CountryID = '{0}' ", this.txtCountry1.TextBox1.Text);
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
, MDivisionID
, QAQty = sum(QAQty)
, ActManPower= Sum(Round(ActManPower,2))
, WorkHour = sum(Round(WorkHour,2))
into #stmp2		
from #stmp
group by OutputDate, Category, Shift, SewingLineID, Team, orderid, ComboType, SCategory, FactoryID
, ProgramID, CPU, CPUFactor, StyleID, Rate,MDivisionID

select 
a.ID, a.ProgramID, a.StyleID, a.SeasonID, a.BrandID , a.FactoryID, a.POID , a.Category, a.CdCodeID 
, CPU = sum(a.CPU)
, CPURate = sum(a.CPURate)
, a.BuyerDelivery, a.SCIDelivery, a.SewingLineID , a.ComboType
, ManPower = sum(a.ManPower)
, WorkHour = sum(Round(a.WorkHour,2)) 
, QARate = convert(numeric(12,2)
, sum(a.QARate))
, TotalManHour =
	(select sum(ROUND(IIF(QAQty > 0, ActManPower / QAQty, ActManPower) * WorkHour, 2))
	from #stmp2 f 
	where f.MDivisionID = a.MDivisionID and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID 
	and f.orderid = a.orderid
	and f.CPU = a.CPU and f.CPUFactor = a.CPUFactor and f.Rate = a.Rate
	and f.OutputDate=a.OutputDate and f.Category = a.Category and f.Shift = a.Shift and f.Team = a.Team and f.ComboType = a.ComboType and f.SCategory = a.SCategory)
, a.CDDesc
, a.StyleDesc
, a.ModularParent
, CPUAdjusted = sum(a.CPUAdjusted)
,QAQty = sum(a.QAQty) 
,TotalCPUOut =
	(select sum(Round(CPU * CPUFactor * Rate * QAQty,2))  
	from #stmp2 f 
	where f.MDivisionID = a.MDivisionID and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID 
	and f.orderid = a.orderid
	and f.CPU = a.CPU and f.CPUFactor = a.CPUFactor and f.Rate = a.Rate
	and f.OutputDate=a.OutputDate and f.Category = a.Category and f.Shift = a.Shift and f.Team = a.Team and f.ComboType = a.ComboType and f.SCategory = a.SCategory)
into #tmpz
from #stmp a
group by a.ID, a.ProgramID, a.StyleID, a.SeasonID, a.BrandID , a.FactoryID, a.POID , a.Category, a.CdCodeID ,a.BuyerDelivery, a.SCIDelivery, a.SewingLineID 
, a.CDDesc, a.StyleDesc,a.ComboType,a.ModularParent,OutputDate, Category, Shift, SewingLineID, Team, orderid, ComboType, SCategory, FactoryID, ProgramID, CPU, CPUFactor, StyleID, Rate,MDivisionID
";
                #region 1.	By Factory
                string strFactory = string.Format(@"{0} Select FactoryID AS A, QARate, TotalCPUOut, TotalManHour FROM #tmpz ", strSQL);
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

                sqlcmd = @"select A,B=sum(QARate),C=sum(TotalCPUOut),D=sum(TotalManHour)
,E=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,F=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
from #tmp Group BY A order by A ";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData1o, string.Empty, sqlcmd, out this.gdtData1);
                #endregion 1.	By Factory

                #region 2.	By Brand
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
                #endregion 2.	By Brand

                #region 3.	By Brand + Factory
                string strFBrand = string.Format(
                    @"
{0} 
Select BrandID AS A, FactoryID AS B, QARate, TotalCPUOut,TotalManHour
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

                sqlcmd = @"select A,B,C=sum(QARate),D=sum(TotalCPUOut),E=sum(TotalManHour)
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)  
from #tmp Group BY A,B order by A,B";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData3o, string.Empty, sqlcmd, out this.gdtData3);
                #endregion 3.	By Brand + Factory

                #region 4.	By Style
                string strStyle = string.Format(
                    @"
{0} 
Select StyleID AS A, BrandID AS B, CDCodeID AS C, CDDesc AS D, StyleDesc AS E, SeasonID AS F
, QARate, TotalCPUOut,TotalManHour, ModularParent AS L, CPUAdjusted AS M
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

                sqlcmd = @"select A,B,C,D,E,F
,G=sum(QARate)
,H=sum(TotalCPUOut),I=sum(TotalManHour)
,J=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)
,K=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
,L,M 

from #tmp Group BY A,B,C,D,E,F,L,M order by A,B,C,E";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData4o, string.Empty, sqlcmd, out this.gdtData4);
                #endregion 4.	By Style

                #region 5.	By CD
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
                #endregion 5.	By CD

                #region 6.	By Factory Line
                string strFactoryLine = string.Format(@"{0}  Select FactoryID AS A, SewingLineID AS B, QARate, TotalCPUOut,TotalManHour FROM #tmpz ", strSQL);
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

                sqlcmd = @"select A,B, Sum(QARate) AS C, Sum(TotalCPUOut) AS D, SUM(TotalManHour) AS E
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B order by A,B";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData6o, string.Empty, sqlcmd, out this.gdtData6);
                #endregion 6.	By Factory Line

                #region 7.	By Factory, Brand , CDCode
                string strFBCDCode = string.Format(
                    @"{0}  Select BrandID AS A, FactoryID AS B, CdCodeID AS C, CDDesc AS D, QARate, TotalCPUOut,TotalManHour
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

                sqlcmd = @"select A,B,C,D,E=sum(QARate),F=sum(TotalCPUOut),G=sum(TotalManHour)
,H=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,I=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D order by A,B,C";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData7o, string.Empty, sqlcmd, out this.gdtData7);
                #endregion 7.	By Factory, Brand , CDCode

                #region 8.	By PO Combo
                string strPOCombo = string.Format(
                    @"
{0} 
Select POID AS A, StyleID AS B, BrandID AS C, CdCodeID AS D, CDDesc AS E, StyleDesc AS F, SeasonID AS G, ProgramID AS H, QARate, TotalCPUOut, TotalManHour
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

                sqlcmd = @"select A,B,C,D,E,F,G,H
,I=sum(QARate),J=sum(TotalCPUOut),K=sum(TotalManHour)
,L=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,M=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D,E,F,G,H order by A,B,C,D,G";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData8o, string.Empty, sqlcmd, out this.gdtData8);
                #endregion 8.	By PO Combo

                #region 9.	By Program
                string strProgram = string.Format(@"{0}  Select ProgramID AS A, StyleID AS B, FactoryID AS C, BrandID AS D, CdCodeID AS E, CDDesc AS F, StyleDesc AS G, SeasonID AS H, QARate,TotalCPUOut, TotalManHour FROM #tmpz ", strSQL);
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

                sqlcmd = @"select A,B,C,D,E,F,G,H
,I=sum(QARate),J=sum(TotalCPUOut),K=sum(TotalManHour)
,L=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)
,M=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D,E,F,G,H order by A,B,C,D,E,H";

                MyUtility.Tool.ProcessWithDatatable(this.gdtData9o, string.Empty, sqlcmd, out this.gdtData9);
                #endregion 9.	By Program

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
            DualResult result = Result.True;
            string strPath = PrivUtils.getPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
            this.temfile = strPath + @"\Centralized-R03.Prod. Efficiency Analysis Report.xltx";

            try
            {
                if (!(result = PrivUtils.Excels.CreateExcel(this.temfile, out this.excel)))
                {
                    return result;
                }

                Microsoft.Office.Interop.Excel.Worksheet wsSheet;
                #region 1.	By Factory
                int intRowsCount = this.gdtData1.Rows.Count;
                int intRowsStart = 2; // 匯入起始位置
                int rownum = intRowsStart; // 每筆資料匯入之位置
                int intColumns = 6; // 匯入欄位數
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

                        wsSheet.Range[string.Format("A{0}:F{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 1.	By Factory

                #region 2.	By Brand
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
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 2.	By Brand

                #region 3.	By Brand-Factory
                if ((this.gdtData3 != null) && (this.gdtData3.Rows.Count > 0))
                {
                    intColumns = 7; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[3];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData3.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData3.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 3.	By Brand-Factory

                #region 4.	By Style
                if ((this.gdtData4 != null) && (this.gdtData4.Rows.Count > 0))
                {
                    intColumns = 13; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[4];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData4.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData4.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:M{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 4.	By Style

                #region 5.	By CD
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
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 5.	By CD

                #region 6.	By Factory Line
                if ((this.gdtData6 != null) && (this.gdtData6.Rows.Count > 0))
                {
                    intColumns = 7; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[6];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData6.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData6.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 6.	By Factory Line

                #region 7.	By Brand-Factory-CD
                if ((this.gdtData7 != null) && (this.gdtData7.Rows.Count > 0))
                {
                    intColumns = 9; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[7];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData7.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData7.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:I{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 7.	By Brand-Factory-CD

                #region 8.	By PO Combo
                if ((this.gdtData8 != null) && (this.gdtData8.Rows.Count > 0))
                {
                    intColumns = 13; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[8];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData8.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData8.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:M{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 8.	By PO Combo

                #region 9.	By Program
                if ((this.gdtData9 != null) && (this.gdtData9.Rows.Count > 0))
                {
                    intColumns = 13; // 匯入欄位數
                    wsSheet = this.excel.ActiveWorkbook.Worksheets[9];
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    for (int intIndex = 0; intIndex < this.gdtData9.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = this.gdtData9.Rows[intIndex][aryAlpha[intIndex_C]];
                        }

                        wsSheet.Range[string.Format("A{0}:M{0}", intIndex + rownum)].Value2 = objArray;
                    }

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 9.	By Program

                #region Save & Show Excel
                this.excel.Visible = true;
                Workbook workbook = this.excel.Workbooks[1];
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Centralized-R03.Prod. Efficiency Analysis Report");
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
