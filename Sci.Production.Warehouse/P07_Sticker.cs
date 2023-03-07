﻿using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07_Sticker : Win.Tems.QueryForm
    {
        private BindingList<P07_Sticker_Data> Data = new BindingList<P07_Sticker_Data>();

        /// <inheritdoc/>
        public P07_Sticker(DataRow row)
        {
            this.InitializeComponent();
            this.gridSticker.DataSource = this.Data;
            this.GridSetup();
            string rcvDate = string.Empty;
            if (!MyUtility.Check.Empty(row["WhseArrival"]))
            {
                rcvDate = ((DateTime)MyUtility.Convert.GetDate(row["WhseArrival"])).ToShortDateString();
            }

            DualResult result;
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();

            string id = row["ID"].ToString();

            #endregion

            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string cmdd = @"
select  r.POID,r.Seq1+' '+r.seq2 as SEQ,r.Roll,r.Dyelot
    ,r.stockqty,r.StockUnit,psd.refno 
    ,RTRIM(dbo.Getmtldesc(r.poid, r.seq1, r.seq2,2,0)) [Description]
    ,ColorID = isnull(psdsC.SpecValue, '')
    ,dbo.getTPEPass1( p.posmr )[MRName] 
    , p.posmr, o.Seasonid,o.BrandId,o.styleid ,rec.WhseArrival
    ,Packing = (select ID+',' from (select DISTINCT d.ID from dbo.PackingList_Detail d WITH (NOLOCK) where d.OrderID = r.POID) t for xml path(''))
from dbo.Receiving_Detail r WITH (NOLOCK) 
left join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.id=r.POID and psd.SEQ1=r.Seq1 and psd.SEQ2=r.seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join dbo.po p WITH (NOLOCK)  on  p.id = r.poid
left join dbo.View_WH_Orders o WITH (NOLOCK) on o.id = r.PoId
left join dbo.Receiving rec WITH (NOLOCK) on rec.id = r.id
where r.id= @ID
";
            result = DBProxy.Current.Select(string.Empty, cmdd, pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 傳 list 資料
            dtDetail.AsEnumerable()
                .Select(row1 => new P07_Sticker_Data()
             {
                    POID = row1["POID"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    Roll = row1["Roll"].ToString(),
                    Dyelot = row1["Dyelot"].ToString(),
                    Desc = row1["Description"].ToString(),
                    Color = row1["colorId"].ToString(),
                    Unit = row1["StockUnit"].ToString(),
                    QTY = row1["stockqty"].ToString(),
                    MRName = row1["MRName"].ToString(),
                    Season = row1["Seasonid"].ToString(),
                    RcvDate = rcvDate,
                    Brand = row1["BrandId"].ToString(),
                    RefNo = row1["refno"].ToString(),
                    Style = row1["styleid"].ToString(),
                    Packing = row1["Packing"].ToString(),
             })
                .ToList()
                .ForEach(d => this.Data.Add(d));

            #endregion

        }

        private void GridSetup()
        {
            this.gridSticker.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridSticker)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(14), iseditable: true, trueValue: true, falseValue: false)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(16), iseditingreadonly: true)
                .Text("SEQ", header: "SEQ", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(9), iseditingreadonly: true)
                ;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            List<P07_Sticker_Data> tmpData = this.Data.Where(data => data.Selected).ToList();

            // 指定是哪個 RDLC
            DualResult result;
            Type reportResourceNamespace = typeof(P07_Sticker_Data);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P07_Sticker.rdlc";
            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return;
            }

            ReportDefinition report = new ReportDefinition();
            report.ReportDataSource = tmpData;
            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            return;
        }
    }
}
