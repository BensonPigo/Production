using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_Print : Win.Tems.PrintForm
    {
        private DataRow drPrint;
        private string strCutNo;
        private DualResult result;
        private DataTable dtExcel;
        private DataRow DataRow;

        /// <inheritdoc/>
        public P10_Print(DataRow dr, string cutNo, DataRow dataRow)
        {
            this.InitializeComponent();
            this.Text = "P10 " + dr["ID"].ToString();
            this.drPrint = dr;
            this.strCutNo = cutNo;
            this.DataRow = dataRow;

            this.ButtonEnable();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            if (this.radioFabricsRelaxationLogsheet.Checked)
            {
                string sqlcmd = $@"
select 
     [Received Date] = null
    ,[Time Started] = null
     ,Refno = isnull (psd.Refno, '')
    , isS.Colorid
     , SPNo = isd.POID
     , Roll = isd.Roll
     ,[Cutting Schedule]=''
     , Description =dbo.getmtldesc(isd.POID,isd.seq1,isd.seq2,2,0)
     , Arrived = StockList.Arrived
     ,[Actual]='',[Remarks]='',[Pack Date]='',[Pack Time]=null,[Signature]=''
from Issue_Detail isd
left join Orders o on o.ID=isd.POID
left join Po_Supp_Detail psd on isd.POID = psd.ID
 and isd.Seq1 = psd.SEQ1
 and isd.Seq2 = psd.SEQ2
left join Issue_Summary isS with(nolock) on isS.Ukey = isd.Issue_SummaryUkey
outer apply(
select SUM(FI.InQty) AS Arrived from FtyInventory fi where fi.POID=isd.POID and fi.Seq1= isd.Seq1 and fi.Seq2 = isd.Seq2 and fi.Roll = isd.Roll and fi.Dyelot = isd.Dyelot
and StockType in ('B','I')
) as StockList
where isd.ID = '{this.drPrint["ID"]}'
order by psd.Refno,isd.POID,isd.Roll
";

                this.result = DBProxy.Current.Select(string.Empty, sqlcmd, out this.dtExcel);
                if (!this.result)
                {
                    this.ShowErr(this.result);
                }
            }

            return this.result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.radioFabricsRelaxationLogsheet.Checked)
            {
                if (this.dtExcel.Rows.Count == 0 || this.dtExcel == null)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                    return false;
                }

                this.SetCount(this.dtExcel.Rows.Count);
                string excelName = "Warehouse_P10_FabricsRelaxationLogsheet.xltx";
                Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelName);
                Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
                worksheet.Cells[3, 3] = MyUtility.Convert.GetString(this.DataRow["cutplanID"]);
                worksheet.Cells[4, 3] = ((DateTime)MyUtility.Convert.GetDate(this.DataRow["issuedate"])).ToString("d");

                #region 插入需要row數量
                for (int i = 1; i < this.dtExcel.Rows.Count; i++)
                {
                    worksheet.Rows[8 + i, Type.Missing].Insert(Excel.XlDirection.xlDown);
                }
                #endregion
                MyUtility.Excel.CopyToXls(this.dtExcel, string.Empty, excelName, 8, false, null, excelApp, wSheet: excelApp.Sheets[1]);

                // 固定寬度,避免格式資料跑掉
                worksheet.Columns[1].ColumnWidth = 6;
                worksheet.Columns[2].ColumnWidth = 7;
                worksheet.Columns[3].ColumnWidth = 8;
                worksheet.Columns[4].ColumnWidth = 8;
                worksheet.Columns[5].ColumnWidth = 9;
                worksheet.Columns[6].ColumnWidth = 5;
                worksheet.Columns[7].ColumnWidth = 7;
                worksheet.Columns[8].ColumnWidth = 44;
                worksheet.Columns[9].ColumnWidth = 6;
                worksheet.Columns[10].ColumnWidth = 6;
                worksheet.Columns[11].ColumnWidth = 9;
                worksheet.Columns[12].ColumnWidth = 6;
                worksheet.Columns[12].ColumnWidth = 7;
                worksheet.Columns[13].ColumnWidth = 8;
                excelApp.Cells.EntireRow.AutoFit();

                worksheet.Rows[7].RowHeight = 25;

                #region Save Excel
                string excelFile = Class.MicrosoftFile.GetName("Warehouse_P10_FabricsRelaxationLogsheet");
                excelApp.ActiveWorkbook.SaveAs(excelFile);
                excelApp.Quit();
                excelFile.OpenFile();
                Marshal.ReleaseComObject(excelApp);
                #endregion
            }

            return true;
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (MyUtility.Check.Empty(this.drPrint))
            {
                return false;
            }

            if (this.radioFabricSticker.Checked || this.radioTransferSlip.Checked)
            {
                if (string.Compare(this.drPrint["Status"].ToString(), "Confirmed", true) != 0)
                {
                    MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                    return false;
                }
            }

            if (this.radioTransferSlip.Checked)
            {
                string sqlcmd = $@"update Issue set  PrintName = '{Env.User.UserID}' , PrintDate = GETDATE()
                                where id = '{this.drPrint["id"]}'";

                DualResult result = DBProxy.Current.Execute(null, sqlcmd);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                string id = this.drPrint["ID"].ToString();
                string remark = this.drPrint["Remark"].ToString();
                string cutplanID = this.drPrint["cutplanID"].ToString();
                string issuedate = ((DateTime)MyUtility.Convert.GetDate(this.drPrint["issuedate"])).ToShortDateString();
                string cutno = this.strCutNo;
                string factoryID = this.drPrint["FactoryID"].ToString();

                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@MDivision", Env.User.Keyword),
                };
                DataTable dt;
                string cmdd = @"
select NameEN
from MDivision
where id = @MDivision";
                result = DBProxy.Current.Select(string.Empty, cmdd, pars, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                string rptTitle = dt.Rows[0]["NameEn"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutplanID", cutplanID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutno", cutno));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Factory", "Factory: " + factoryID));
                pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                string cCellNo, cPrintDate;
                sqlcmd = @"
select  b.CutCellID, [PrintDate] = Format(a.PrintDate, 'yyyy/MM/dd HH:mm')
from dbo.Issue as a WITH (NOLOCK) 
inner join dbo.cutplan as b WITH (NOLOCK) 
on b.id = a.cutplanid
where b.id = a.cutplanid
and a.id = @ID";
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable aa);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (aa.Rows.Count == 0)
                {
                    cCellNo = string.Empty;
                    cPrintDate = string.Empty;
                }
                else
                {
                    cCellNo = aa.Rows[0]["CutCellID"].ToString();
                    cPrintDate = aa.Rows[0]["PrintDate"].ToString();
                }

                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cCellNo", cCellNo));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cPrintDate", cPrintDate));

                pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                string cLineNo;
                sqlcmd = @"
select o.sewline 
from dbo.Orders o WITH (NOLOCK) 
where id in (select distinct poid from issue_detail WITH (NOLOCK) where id = @ID)";
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable cc);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (cc.Rows.Count == 0)
                {
                    cLineNo = string.Empty;
                }
                else
                {
                    cLineNo = cc.Rows[0]["sewline"].ToString();
                }

                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cLineNo", cLineNo));
                #endregion

                #region  抓表身資料
                pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", id),
                };
                sqlcmd = @"
select  [Poid] = IIF (( t.poid = lag (t.poid,1,'') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll) 
			            AND (t.seq1 = lag (t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			            AND (t.seq2 = lag (t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
			          , ''
                      , t.poid) 
        , [Seq] = IIF (( t.poid = lag (t.poid,1,'') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll) 
			             AND (t.seq1 = lag (t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			             AND (t.seq2 = lag (t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
			            , ''
                        , t.seq1+ '-' +t.seq2)
        , [GroupPoid] = t.poid 
        , [GroupSeq] = t.seq1+ '-' +t.seq2 
        , [desc] = IIF (( t.poid = lag (t.poid,1,'') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll) 
			              AND (t.seq1 = lag (t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			              AND (t.seq2 = lag (t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
				        , ''
                        ,( SELECT   Concat(stock7X.value
                                            , char(10)
                                            , rtrim( fbr.DescDetail)
                                            , char(10)
                                            , char(10)
                                            , (Select concat(ID, '-', Name) from Color WITH (NOLOCK) where id = iss.ColorId and BrandId = fbr.BrandID)
                                        )
                            FROM fabric fbr WITH (NOLOCK) WHERE SCIRefno = p.SCIRefno))
        , MDesc = 'Relaxation Type：'+(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = p.Refno)
        , t.Roll
        , t.Dyelot
        , t.Qty
        , p.StockUnit
        , [location]=dbo.Getlocation(b.ukey)      
        , [Total]=sum(t.Qty) OVER (PARTITION BY t.POID ,t.Seq1,t.Seq2 )       
from dbo.Issue_Detail t WITH (NOLOCK) 
inner join Issue_Summary iss WITH (NOLOCK) on t.Issue_SummaryUkey = iss.Ukey
left join dbo.PO_Supp_Detail p  WITH (NOLOCK) on    p.id= t.poid 
                                                    and p.SEQ1 = t.Seq1 
                                                    and p.seq2 = t.Seq2
left join FtyInventory b WITH (NOLOCK) on   b.poid = t.poid 
                                            and b.seq1 = t.seq1 
                                            and b.seq2= t.seq2 
                                            and b.Roll = t.Roll 
                                            and b.Dyelot = t.Dyelot 
                                            and b.StockType = t.StockType
outer apply (
    select value = iif (left (t.seq1, 1) != '7', ''
                                               , '**PLS USE STOCK FROM SP#:' + iif (isnull (concat (p.StockPOID, p.StockSeq1, p.StockSeq2), '') = '', '',concat (p.StockPOID, p.StockSeq1, p.StockSeq2)) + '**')
) as stock7X
where t.id= @ID";
                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable bb);
                if (!result)
                {
                    this.ShowErr(sqlcmd, result);
                }

                if (bb == null || bb.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", string.Empty);
                    return false;
                }

                // 傳 list 資料
                List<P10_PrintData> data = bb.AsEnumerable()
                    .Select(row1 => new P10_PrintData()
                    {
                        GroupPoid = row1["GroupPoid"].ToString().Trim(),
                        GroupSeq = row1["GroupSeq"].ToString().Trim(),
                        Poid = row1["poid"].ToString().Trim(),
                        Seq = row1["SEQ"].ToString().Trim(),
                        Desc = row1["desc"].ToString().Trim(),
                        MDesc = row1["MDesc"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim(),
                        Unit = row1["StockUnit"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        Dyelot = row1["Dyelot"].ToString().Trim(),
                        Qty = row1["Qty"].ToString().Trim(),
                        Total = row1["Total"].ToString().Trim(),
                    }).OrderBy(s => s.GroupPoid).ThenBy(s => s.GroupSeq).ThenBy(s => s.Dyelot).ThenBy(s => s.Roll).ToList();

                report.ReportDataSource = data;
                #endregion

                // 指定是哪個 RDLC
                #region  指定是哪個 RDLC

                // DualResult result;
                Type reportResourceNamespace = typeof(P10_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P10_Print.rdlc";

                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    return false;
                }

                report.ReportResource = reportresource;
                #endregion

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();
            }

            if (this.radioFabricSticker.Checked)
            {
                new P13_FabricSticker(this.drPrint["ID"], "Issue_Detail").ShowDialog();
            }

            if (this.radioRelaxationSticker.Checked)
            {
                new P10_RelaxationSticker(this.drPrint["ID"].ToString()).ShowDialog();
            }

            return true;
        }

        private void RadioGroup1_ValueChanged(object sender, EventArgs e)
        {
            this.ButtonEnable();
        }

        private void ButtonEnable()
        {
            if (this.radioFabricsRelaxationLogsheet.Checked)
            {
                this.print.Enabled = false;
                this.toexcel.Enabled = true;
            }
            else
            {
                this.print.Enabled = true;
                this.toexcel.Enabled = false;
            }
        }
    }
}
