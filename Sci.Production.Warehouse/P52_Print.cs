using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P52_Print : Sci.Win.Tems.PrintForm
    {
        DataRow drP52Title;
        List<P52_PrintData> printData;

        public P52_Print(DataRow drP52Title, DataTable dtP52Detail)
        {
            this.InitializeComponent();
            this.drP52Title = drP52Title;

            switch (drP52Title["Type"].ToString())
            {
                case "F":
                    this.printData = dtP52Detail.AsEnumerable().Select(
                        row => new P52_PrintData()
                        {
                            Poid = row["Poid"].ToString().Trim(),
                            Refno = row["Refno"].ToString().Trim(),
                            Color = row["Color"].ToString().Trim(),
                            Location = row["Location"].ToString().Trim(),
                            QtyBefore = Convert.ToDecimal(row["QtyBefore"].ToString()),
                            QtyAfter = Convert.ToDecimal(row["QtyAfter"].ToString()),
                            Variance = Convert.ToDecimal(row["Variance"].ToString()),
                            UnitID = row["UnitID"].ToString().Trim(),
                        }).ToList();
                    break;
                default:
                    this.printData = new List<P52_PrintData>();
                    for (int i = 0; i < 32; i++)
                    {
                        P52_PrintData drNew = new P52_PrintData();
                        this.printData.Add(drNew);
                    }

                    break;
            }

            this.ReportResourceNamespace = typeof(P52_PrintData);
            this.ReportResourceAssembly = this.ReportResourceNamespace.Assembly;
        }

        protected override bool ValidateInput()
        {
            this.ReportResourceName = this.radioButtonBookQty.Checked ? (this.drP52Title["Type"].EqualString("F") ? "P52_StocktakingWithoutBookQty.rdlc" : "P52_StocktakingWithoutBookQtyBack.rdlc")
                                                                        : (this.drP52Title["Type"].EqualString("F") ? "P52_StocktakingList.rdlc" : "P52_StocktakingListBack.rdlc");
            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string strTitle = string.Empty;
            if (this.radioButtonBookQty.Checked)
            {
                strTitle = this.drP52Title["Type"].EqualString("F") ? "Forward Stocktaking without Book Qty" : "Backward Stocktaking";
            }
            else
            {
                strTitle = this.drP52Title["Type"].EqualString("F") ? "Forward Stocktaking List" : "Backward Stocktaking List";
            }

            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("txtTitle", strTitle));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("txtID", this.drP52Title["ID"].ToString()));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("txtIssueDate", ((DateTime)this.drP52Title["IssueDate"]).ToString("yyyy/MM/dd")));
            e.Report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("txtStockType", this.drP52Title["StockType"].ToString()));
            e.Report.ReportDataSource = this.printData;
            return new DualResult(true);
        }
    }
}
