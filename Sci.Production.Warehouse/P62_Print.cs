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

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P62_Print : Win.Tems.PrintForm
    {
        private DataRow drPrint;
        private DualResult result;
        private DataTable dtExcel;

        /// <summary>
        /// Initializes a new instance of the <see cref="P62_Print"/> class.
        /// </summary>
        /// <param name="dr">Master DataRow</param>
        public P62_Print(DataRow dr)
        {
            this.InitializeComponent();
            this.Text = "P62 " + dr["ID"].ToString();
            this.drPrint = dr;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return this.result;
        }

        /// <inheritdoc/>
        protected override bool ToPrint()
        {
            if (MyUtility.Check.Empty(this.drPrint))
            {
                return false;
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
                string factoryID = this.drPrint["FactoryID"].ToString();
                string confirmTime = this.drPrint["Status"].EqualString("CONFIRMED") ? MyUtility.Convert.GetDate(this.drPrint["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@MDivision", Env.User.Keyword),
                };
                result = DBProxy.Current.Select(string.Empty, @"select NameEN from MDivision where id = @MDivision", pars, out DataTable dt);
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
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("confirmTime", confirmTime));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Factory", "Factory: " + factoryID));
                #endregion

                #region  抓表身資料
                sqlcmd = @"
select[Poid] = IIF((t.poid = lag(t.poid, 1, '') over(order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)

                        AND(t.seq1 = lag(t.seq1, 1, '') over(order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))

                        AND(t.seq2 = lag(t.seq2, 1, '') over(order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)))
			          , ''
                      , t.poid) 
        , [Seq] = IIF((t.poid = lag(t.poid, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)

                         AND(t.seq1 = lag(t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			             AND(t.seq2 = lag(t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
			            , ''
                        , t.seq1+ '-' +t.seq2)
        , [GroupPoid] = t.poid 
        , [GroupSeq] = t.seq1+ '-' +t.seq2 
        , [desc] = IIF((t.poid = lag(t.poid, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)
                          AND(t.seq1 = lag(t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			              AND(t.seq2 = lag(t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
				        , ''
                        , (SELECT Concat(stock7X.value
                                         , char(10)
                                            , rtrim(fbr.DescDetail)
                                            , char (10)
                                            , char (10)
                                            , (Select concat(ID, '-', Name) from Color WITH(NOLOCK) where id = iss.ColorId and BrandId = fbr.BrandID)
                                        )
									FROM fabric fbr WITH(NOLOCK) WHERE SCIRefno = p.SCIRefno))
		, Mdesc = 'Relaxation Type：'+(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = p.Refno)
        , t.Roll
        , t.Dyelot
        , t.Qty
        , p.StockUnit
        , [location]=dbo.Getlocation(b.ukey)      
        , [Total]=sum(t.Qty) OVER(PARTITION BY t.POID , t.Seq1, t.Seq2 )
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
    select value = iif(left(t.seq1, 1) != '7', ''
                                               , '**PLS USE STOCK FROM SP#:' + iif(isnull(concat(p.StockPOID, p.StockSeq1, p.StockSeq2), '') = '', '', concat(p.StockPOID, p.StockSeq1, p.StockSeq2)) + '**')
) as stock7X
where t.id= @ID";

                pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", this.drPrint["ID"].ToString()),
                };

                this.result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out this.dtExcel);
                if (!this.result)
                {
                    this.ShowErr(this.result);
                }

                if (this.dtExcel == null || this.dtExcel.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", string.Empty);
                    return false;
                }

                // 傳 list 資料 (直接利用P10_PrintData的結構)
                List<P10_PrintData> data = this.dtExcel.AsEnumerable()
                    .Select(row1 => new P10_PrintData()
                    {
                        GroupPoid = row1["GroupPoid"].ToString().Trim(),
                        GroupSeq = row1["GroupSeq"].ToString().Trim(),
                        Poid = row1["poid"].ToString().Trim(),
                        Seq = row1["SEQ"].ToString().Trim(),
                        Desc = row1["desc"].ToString().Trim(),
                        MDesc = row1["Mdesc"].ToString().Trim(),
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
                string reportResourceName = "P62_Print.rdlc";

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

            return true;
        }
    }
}
