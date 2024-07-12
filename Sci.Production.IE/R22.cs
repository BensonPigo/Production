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
        private DataTable[] printData;

        /// <inheritdoc/>
        public R22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.cbProductType.SetDataSource();
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
                sqlWhere += $@"and coc.Deadline between @sdate1 and @sdate2 ";
            }

            if (!MyUtility.Check.Empty(this.iDate1))
            {
                listParameter.Add(new SqlParameter("@date1", this.iDate1));
                listParameter.Add(new SqlParameter("@date2", this.iDate2));
                sqlWhere += $@"and co.Inline between @date1 and @date2 ";
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
                sqlWhere += string.Format($" and ccld.ResponseDep in ( {string.Join(",", this.strRD.Split(',').Select(s => "'" + s + "'").ToList())} ) ");
            }

            if (this.chkOutstanding.Checked)
            {
                sqlWhere += $@"and coc.CompletionDate > coc.Deadline";
            }

            sqlCmd = $@"
--Summary
SELECT Distinct 
     [Inline] = CONVERT(varchar, co.Inline, 23),
     [Ready (all checked)] = CASE WHEN (SELECT COUNT(*) 
                                        FROM ChgOver_Check coc2
                                        WHERE coc2.ID = co.ID
                                          AND coc2.Checked = 1) = 
                                       (SELECT COUNT(*) 
                                        FROM ChgOver_Check coc3
                                        WHERE coc3.ID = co.ID) 
                                   THEN 'V'
                                   ELSE '' END,
     [SewingLine] = co.SewingLineID,
     [OldSP] = oldco.OrderID,
     [OldStyle] = oldco.StyleID,
     [OldComboType] = oldco.ComboType,
     [NewSP] = co.OrderID,
     [NewStyle] = co.StyleID,
     [NewComboType] = co.ComboType,
     [Style Type] = co.Type,
     [Category] = co.Category
FROM ChgOver co WITH (NOLOCK)
INNER JOIN ChgOver_Check coc WITH (NOLOCK) ON coc.ID = co.ID
LEFT JOIN Style s WITH (NOLOCK) ON s.ID = co.StyleID
LEFT JOIN Reason r WITH (NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type' 
LEFT JOIN SewingLine sl WITH (NOLOCK) ON sl.ID = co.SewingLineID AND sl.FactoryID = co.FactoryID
LEFT JOIN ChgOverCheckList ccl WITH(NOLOCK) ON ccl.Category = co.Category AND ccl.StyleType = co.Type
LEFT JOIN ChgOverCheckList_Detail ccld WITH(NOLOCK) ON ccl.ID = ccld.ID
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
WHERE 1 = 1
{sqlWhere}
 
--Detail
SELECT Distinct
    [SP#] = co.OrderID,
    [Style] = co.StyleID,
    [Category] = co.Category,
    [Product Type] = r.Name,
    [Cell] = sl.SewingCell,
    [Days Left] = DaysLeft.value,
    [Inline Date] = CONVERT(varchar, co.Inline, 23),
    [Over Days] = IIF(coc.Checked = 0, DaysLeft.Value, OverDays.Value),
    [Check] = IIF(coc.Checked = 0, '', 'V'),
    [Completion Date] = CONVERT(varchar, coc.CompletionDate, 23),
    [Response Dep.] = cod.ResponseDep,
    [Check List No] = coc.No,
    [Check List Item] = colb.CheckList,
    [Late Reason] = co.Remark
FROM ChgOver_Check coc WITH (NOLOCK)
INNER JOIN ChgOver co WITH (NOLOCK) ON coc.ID = co.ID
LEFT JOIN Style s WITH (NOLOCK) ON s.ID = co.StyleID
LEFT JOIN SewingLine sl WITH (NOLOCK) ON sl.ID = co.SewingLineID AND sl.FactoryID = co.FactoryID
LEFT JOIN Reason r WITH (NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
LEFT JOIN ChgOverCheckList ccl WITH(NOLOCK) ON ccl.Category = co.Category AND ccl.StyleType = co.Type
LEFT JOIN ChgOverCheckListBase colb WITH(NOLOCK) ON colb.ID = ccl.ID
LEFT JOIN ChgOverCheckList_Detail ccld WITH(NOLOCK) on ccld.ID = ccl.ID
OUTER APPLY
(
	select ResponseDep = Stuff((
			select concat(',',ResponseDep)
			from (
					select 	distinct
						d.ResponseDep
					from dbo.ChgOverCheckList_Detail d WITH(NOLOCK)
					where ID = ccl.ID
				) s
			for xml path ('')
		) , 1, 1, '')
) as cod
OUTER APPLY 
(
    SELECT COUNT(*) AS value
    FROM (
        SELECT DATEADD(DAY, number, GETDATE()) AS DateInPeriod
        FROM master..spt_values
        WHERE type = 'P'
          AND number <= DATEDIFF(DAY, GETDATE(), coc.Deadline)
    ) AS DateRange
    WHERE DATEPART(WEEKDAY, DateInPeriod) <> 1
      AND NOT EXISTS (
          SELECT 1
          FROM Holiday h WITH(NOLOCK)
          WHERE h.HolidayDate = DateInPeriod
            AND h.FactoryID = co.FactoryID
      )
) AS DaysLeft
OUTER APPLY 
(
    SELECT COUNT(*) AS value
    FROM (
        SELECT DATEADD(DAY, number, coc.Deadline) AS DateInPeriod
        FROM master..spt_values
        WHERE type = 'P'
          AND number <= DATEDIFF(DAY, coc.Deadline, coc.CompletionDate)
    ) AS DateRange
    WHERE DATEPART(WEEKDAY, DateInPeriod) <> 1
      AND NOT EXISTS (
          SELECT 1
          FROM Holiday h WITH(NOLOCK)
          WHERE h.HolidayDate = DateInPeriod
            AND h.FactoryID = co.FactoryID
      )
) AS OverDays
WHERE 1 = 1
            {sqlWhere}
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
