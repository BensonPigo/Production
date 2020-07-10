using Ict.Win;
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
    public partial class P07_Sticker : Win.Tems.QueryForm
    {
        public DataRow CurrentDataRow { get; set; }

        BindingList<P07_Sticker_Data> Data = new BindingList<P07_Sticker_Data>();

        public P07_Sticker(DataRow row)
        {
            this.InitializeComponent();
            this.gridSticker.DataSource = this.Data;
            this.GridSetup();
            string RcvDate = string.Empty;
            if (!MyUtility.Check.Empty(row["WhseArrival"]))
            {
                RcvDate = ((DateTime)MyUtility.Convert.GetDate(row["WhseArrival"])).ToShortDateString();
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
            result = DBProxy.Current.Select(
                string.Empty,
                @"select  r.POID,r.Seq1+' '+r.seq2 as SEQ,r.Roll,r.Dyelot
		,r.stockqty,r.StockUnit,s.refno 
	    ,RTRIM(dbo.Getmtldesc(r.poid, r.seq1, r.seq2,2,0)) [Description],s.colorId 
        ,dbo.getTPEPass1( p.posmr )[MRName] 
		, p.posmr, o.Seasonid,o.BrandId,o.styleid ,rec.WhseArrival
        ,Packing = (select ID+',' from (select DISTINCT d.ID from dbo.PackingList_Detail d WITH (NOLOCK) where d.OrderID = r.POID) t for xml path(''))
         from dbo.Receiving_Detail r WITH (NOLOCK) 
         left join dbo.PO_Supp_Detail s WITH (NOLOCK) 
         on 
         s.id=r.POID and s.SEQ1=r.Seq1 and s.SEQ2=r.seq2
		 left join dbo.po p WITH (NOLOCK) 
		 on  p.id = r.poid
         left join dbo.Orders  o WITH (NOLOCK) 
            on o.id = r.PoId
         left join dbo.Receiving rec WITH (NOLOCK) 
            on rec.id = @ID
                where r.id= @ID", pars, out dtDetail);
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
                    RcvDate = RcvDate,
                    Brand = row1["BrandId"].ToString(),
                    RefNo = row1["refno"].ToString(),
                    Style = row1["styleid"].ToString(),
                    Packing = row1["Packing"].ToString(),
             })
                .ToList()
                .ForEach(d => this.Data.Add(d));

            #endregion

        }

        void GridSetup()
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<P07_Sticker_Data> tmpData = this.Data.Where(data => data.selected).ToList();

            // 指定是哪個 RDLC
            DualResult result;
            Type ReportResourceNamespace = typeof(P07_Sticker_Data);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P07_Sticker.rdlc";
            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
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
