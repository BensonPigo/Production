using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.IE
{
    /// <inheritdoc/>
    public partial class R22 : Win.Tems.PrintForm
    {
        private string dDate1;
        private string dDate2;
        private string iDate1;
        private string iDate2;
        private string strStyle;
        private string productType;
        private string strSP;
        private string strCategory;
        private string strCell;
        private string strRD;
        private string strFactory;
        private DataTable[] printData;

        /// <inheritdoc/>
        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.cbProductType.SetDataSource();
            this.cbFactoryID.SetDataSource();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dtAddEdit.HasValue1 && !this.dtAddEdit.HasValue2 && !this.dtInline.HasValue1 && !this.dtInline.HasValue2 && string.IsNullOrEmpty(this.txtSPNO.Text))
            {
                MyUtility.Msg.InfoBox("Please fill in at least one：<Deadline> or <Inline Date> or <SP>!");
                return false;
            }

            this.dDate1 = string.Empty;
            this.dDate2 = string.Empty;
            this.iDate1 = string.Empty;
            this.iDate2 = string.Empty;

            if (this.dtAddEdit.HasValue1 && this.dtAddEdit.HasValue2)
            {
                this.dDate1 = this.dtAddEdit.Value1.Value.ToString("yyyy-MM-dd");
                this.dDate2 = this.dtAddEdit.Value2.Value.ToString("yyyy-MM-dd");
            }

            if (this.dtInline.HasValue1 && this.dtInline.HasValue2)
            {
                this.iDate1 = this.dtInline.Value1.Value.ToString("yyyy-MM-dd");
                this.iDate2 = this.dtInline.Value2.Value.ToString("yyyy-MM-dd");
            }

            this.productType = this.cbProductType.Text;
            this.strStyle = this.txtStyle.Text;
            this.strSP = this.txtSPNO.Text;
            this.strCategory = this.txtCategory.Text;
            this.strCell = this.txtCell.Text;
            this.strRD = this.txtRD.Text;
            this.strFactory = this.cbFactoryID.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            string sqlWhere = string.Empty;
            List<SqlParameter> listParameter = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.dDate1))
            {
                listParameter.Add(new SqlParameter("@sdate1", this.dDate1));
                listParameter.Add(new SqlParameter("@sdate2", this.dDate2));
                sqlWhere += $@"and cc.Deadline >= @sdate1 and cc.Deadline < DateAdd(day , 1, @sdate2) ";
            }

            if (!MyUtility.Check.Empty(this.iDate1))
            {
                listParameter.Add(new SqlParameter("@date1", this.iDate1));
                listParameter.Add(new SqlParameter("@date2", this.iDate2));
                sqlWhere += $@"and co.Inline >= @date1 and co.Inline < DateAdd(day , 1, @date2) ";
            }

            if (!MyUtility.Check.Empty(this.strSP))
            {
                sqlWhere += string.Format($" and co.OrderID in ( {string.Join(",", this.strSP.Split(',').Select(s => "'" + s + "'").ToList())} ) ");
            }

            if (!MyUtility.Check.Empty(this.strStyle))
            {
                sqlWhere += string.Format($" and co.StyleID in ( {string.Join(",", this.strStyle.Split(',').Select(s => "'" + s + "'").ToList())} ) ");
            }

            if (!MyUtility.Check.Empty(this.productType))
            {
                sqlWhere += string.Format($" and r.Name = '{this.productType}'");
            }

            if (!MyUtility.Check.Empty(this.strCategory))
            {
                sqlWhere += string.Format($" and co.Category in ( {string.Join(",", this.strCategory.Split(',').Select(s => "'" + s + "'").ToList())} ) ");
            }

            if (!MyUtility.Check.Empty(this.strCell))
            {
                sqlWhere += string.Format($" and sl.SewingCell in ( {string.Join(",", this.strCell.Split(',').Select(s => "'" + s + "'").ToList())} ) ");
            }

            if (!MyUtility.Check.Empty(this.strRD))
            {
                sqlWhere += string.Format($" and ccldx.ResponseDep in ({string.Join(",", this.strRD.Split(',').Select(s => "'" + s + "'").ToList())} ) ");
            }

            if (!MyUtility.Check.Empty(this.strFactory))
            {
                sqlWhere += string.Format($" and co.FactoryID = '{this.strFactory}'");
            }

            if (this.chkOutstanding.Checked)
            {
                sqlWhere += $@" AND (iif(CC.[Checked] = 0 , iif(OverDay_Check_0.VAL < 0,0,OverDay_Check_0.VAL) ,iif(OverDay_Check_1.VAL < 0,0,OverDay_Check_1.VAL))) > 0";
            }

            sqlCmd = $@"
--Summary
SELECT  distinct
     [Factory] = co.FactoryID,
     [InlineDate] = CONVERT(varchar, co.Inline, 23),
     [Ready (all checked)] = iif ((SELECT COUNT(1) FROM ChgOver_Check WITH(NOLOCK) WHERE [Checked] = 0 AND ID = CO.ID) > 0 ,'','V'),
     [SewingLine] = co.SewingLineID,
     [OldSP] = oldco.OrderID,
     [OldStyle] = oldco.StyleID,
     [OldComboType] = oldco.ComboType,
     [NewSP] = co.OrderID,
     [NewStyle] = co.StyleID,
     [NewComboType] = co.ComboType,
     [Style Type] = iif(co.Type = 'N', 'New', 'Repeat'),
     [Category] = co.Category,
     [First Sewing Output Date] = GetOutputDate.OutputDate
FROM ChgOver co WITH (NOLOCK)
INNER JOIN ChgOver_Check CC WITH (NOLOCK) ON cc.ID = co.ID And cc.No <> 0
LEFT JOIN Style s WITH (NOLOCK) ON s.ID = co.StyleID and co.SeasonID = s.SeasonID
LEFT JOIN Reason r WITH (NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type' 
LEFT JOIN SewingLine sl WITH (NOLOCK) ON sl.ID = co.SewingLineID AND sl.FactoryID = co.FactoryID
LEFT JOIN ChgOverCheckList ccl WITH(NOLOCK) ON ccl.Category = co.Category AND ccl.StyleType = co.Type and ccl.FactoryID = co.FactoryID
OUTER APPLY
(
    SELECT LTRIM(RTRIM(m.n.value('.[1]', 'varchar(500)'))) AS ResponseDep
    FROM (
            SELECT CAST('<XMLRoot><RowData>' + REPLACE(ResponseDep, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
            FROM ChgOverCheckList_Detail ccld WITH(NOLOCK)
	        WHERE ccl.ID = ccld.ID  and ccld.ChgOverCheckListBaseID = CC.[No]
        ) t
    CROSS APPLY x.nodes('/XMLRoot/RowData') m(n)
) AS ccldx
OUTER APPLY 
(
    SELECT TOP 1 b.OrderID, b.StyleID, b.ComboType
    FROM ChgOver b
    WHERE b.Inline = (
                        SELECT MAX(c.Inline) 
                        FROM ChgOver c WITH(NOLOCK)
                        WHERE c.FactoryID = co.FactoryID 
                          AND c.SewingLineID = co.SewingLineID 
                          AND c.Inline < co.Inline
                       )
    AND b.FactoryID = co.FactoryID
    AND b.SewingLineID = co.SewingLineID
) AS oldco
OUTER APPLY
(
	SELECT val = isnull(DATEDIFF(day,CC.DeadLine,GETDATE()) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE())),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = co.FactoryID
)OverDay_Check_0
OUTER APPLY
(
	SELECT val = isnull(iif((CC.CompletionDate IS NULL) OR (CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,CC.CompletionDate))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND CC.CompletionDate AND FactoryID = CO.FactoryID
)OverDay_Check_1
OUTER APPLY
(
    select top(1) s.OutputDate
    from SewingOutput s WITH (NOLOCK) 
    inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
    where 
    sd.OrderId = co.OrderId
    and sd.ComboType = co.ComboType
    and s.SewingLineID = co.SewingLineID
    and s.FactoryID = co.FactoryID
)GetOutputDate
WHERE 1 = 1
{sqlWhere}
ORDER BY [InlineDate], [SewingLine], [OldSP], [NewSP] 
 
--Detail
SELECT Distinct
    [Factory] = co.FactoryID,
    [SP#] = co.OrderID,
    [Style] = co.StyleID,
    [Category] = co.Category,
    [Product Type] = r.Name,
    [Cell] = sl.SewingCell,
    [DaysLeft] = iif(cc.Checked = 1 ,'-' ,  CONVERT( VARCHAR(10),iif(DaysLefCnt.val < 0 , 0 ,DaysLefCnt.val ))),
    [Inline Date] = CONVERT(varchar, co.Inline, 23),
    [Over Days] = iif(cc.[Checked] = 0 , iif(OverDay_Check_0.VAL < 0,0,OverDay_Check_0.VAL) ,iif(OverDay_Check_1.VAL < 0,0,OverDay_Check_1.VAL)),
    [Check] = IIF(cc.Checked = 0, '', 'V'),
    [Completion Date] = CONVERT(varchar, cc.CompletionDate, 23),
    [Response Dep.] = CC.ResponseDep,
    [Check List No] = cc.No,
    [Check List Item] = colb.CheckList,
    [Late Reason] = cc.Remark
FROM ChgOver_Check CC WITH (NOLOCK)
INNER JOIN ChgOver co WITH (NOLOCK) ON CC.ID = co.ID
LEFT JOIN Style s WITH (NOLOCK) ON s.ID = co.StyleID and co.SeasonID = s.SeasonID
LEFT JOIN SewingLine sl WITH (NOLOCK) ON sl.ID = co.SewingLineID AND sl.FactoryID = co.FactoryID
LEFT JOIN Reason r WITH (NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
LEFT JOIN ChgOverCheckList ccl WITH(NOLOCK) ON ccl.Category = co.Category AND ccl.StyleType = co.Type and ccl.FactoryID = co.FactoryID
LEFT JOIN ChgOverCheckListBase colb WITH(NOLOCK) ON colb.NO = CC.[NO]
LEFT JOIN ChgOverCheckList_Detail ccld WITH(NOLOCK) ON ccld.ID = ccl.ID and ccld.ChgOverCheckListBaseID = Colb.ID
OUTER APPLY
(
    SELECT LTRIM(RTRIM(m.n.value('.[1]', 'varchar(500)'))) AS ResponseDep
    FROM (
        SELECT CAST('<XMLRoot><RowData>' + REPLACE(ResponseDep, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
        FROM ChgOverCheckList_Detail ccldx WITH(NOLOCK)
	    WHERE ccl.ID = ccldx.ID  and ccldx.ChgOverCheckListBaseID = CC.[No]
        and ccld.ID = ccldx.ID
        ) t
    CROSS APPLY x.nodes('/XMLRoot/RowData') m(n)
) AS ccldx
OUTER APPLY
(
	SELECT val = isnull(iif((CC.Deadline IS NULL), 0, DATEDIFF(day,GETDATE(),CC.DeadLine) - (COUNT(1) + dbo.getDateRangeSundayCount(GETDATE(),cc.Deadline))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN GETDATE() AND CC.Deadline AND FactoryID = CO.FactoryID
)DaysLefCnt
OUTER APPLY
(
	SELECT val = isnull(iif((CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,GETDATE()) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE()))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
)OverDay_Check_0
OUTER APPLY
(
	SELECT val = isnull(iif((CC.CompletionDate IS NULL) OR (CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,CC.CompletionDate))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND CC.CompletionDate AND FactoryID = CO.FactoryID
)OverDay_Check_1
WHERE cc.No <> 0
            {sqlWhere}
Order by  [Inline Date], [SP#], Style, Category, [Product Type], Cell, [Check List No]
            ";

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listParameter, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData[0].Rows.Count);
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = "IE_R22.xltx";
            Excel.Application excelapp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            if (this.printData[0].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[1]);
            }

            if (this.printData[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, filename, 1, false, null, excelapp, wSheet: excelapp.Sheets[2]);
            }

            excelapp.Columns.AutoFit();
            string excelfile = Class.MicrosoftFile.GetName("IE_R22");
            excelapp.ActiveWorkbook.SaveAs(excelfile);
            excelapp.Visible = true;
            Marshal.ReleaseComObject(excelapp);
            return true;
        }

        private void TxtSPNO_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "select OrderID from Production.dbo.ChgOver WITH (NOLOCK) order by ID";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, "OrderID", "30", this.txtSPNO.Text)
            {
                Size = new System.Drawing.Size(400, 530),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNO.Text = item.GetSelectedString();
            this.ValidateControl();
        }

        private void TxtCell_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "Select DISTINCT Cell = SewingCell from  SewingLine WITH (NOLOCK) where SewingCell <> '' order by SewingCell ASC";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, string.Empty, this.txtCell.Text)
            {
                Size = new System.Drawing.Size(300, 330),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCell.Text = item.GetSelectedString();
            this.ValidateControl();
        }

        private void TxtCategory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "select DISTINCT Category from ChgOver WITH (NOLOCK) where Category <> '' order by Category ASC";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, string.Empty, this.txtCategory.Text)
            {
                Size = new System.Drawing.Size(300, 330),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCategory.Text = item.GetSelectedString();
            this.ValidateControl();
        }

        private void TxtRD_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string selectCommand = "select DISTINCT Dept from Employee WITH (NOLOCK) where dept <> '' order by Dept ASC";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(selectCommand, "Response Dep.", "20", this.txtCategory.Text)
            {
                Size = new System.Drawing.Size(400, 430),
            };
            DialogResult returnResult = item.ShowDialog();

            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtRD.Text = item.GetSelectedString();
            this.ValidateControl();
        }
    }
}
