using System;
using System.Data;
using Ict.Win;
using Sci.Data;
using Ict;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class P03_Refno : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable selectDataTable1;
        private Win.UI.ContextMenuStrip myCMS = new Win.UI.ContextMenuStrip();

        public P03_Refno(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.gridRefNo.ContextMenuStrip = this.myCMS;
            this.Text += string.Format(" ({0})", this.dr["refno"]);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string selectCommand1
                = string.Format(
                    @"
Select  a.FtyGroup
        ,b.id
        ,a.StyleID
        ,a.SeasonID
        ,a.BrandID
        , concat(Ltrim(Rtrim(b.seq1)), ' ', b.seq2) as seq --left(b.seq1+' ',3)+b.Seq2 as seq
        --, colorid = isnull(dbo.GetColorMultipleID(a.BrandID,b.colorid),'')
		, [ColorID]= IIF(f.MtlTypeID = 'EMB THREAD' OR f.MtlTypeID = 'SP THREAD' OR f.MtlTypeID = 'THREAD' ,b.SuppColor,dbo.GetColorMultipleID(a.BrandID,b.ColorID)) 
        , b.sizespec
        , c.suppid
        , a.sewinline
        , a.sewline
        , b.FinalETA
        , md.inqty - md.outqty + md.adjustqty Balance
        , b.stockunit 
        , md.BLocation
        , [LobQty] = isnull(md.LobQty,0)
from orders a WITH (NOLOCK) 
     , po_supp_detail b WITH (NOLOCK) 
left join fabric f WITH (NOLOCK) on f.SCIRefno = b.SCIRefno
left join dbo.MDivisionPoDetail md WITH (NOLOCK) on md.POID = b.id and md.seq1 = b.seq1 and md.seq2 = b.seq2
     , po_supp c WITH (NOLOCK) 
where   b.refno = '{0}'
        and a.id = b.id
        and a.id = c.id
        and b.seq1 = c.seq1
        and a.WhseClose is null
order by ColorID, SizeSpec ,SewinLine
", this.dr["refno"].ToString());
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out this.selectDataTable1);
            if (selectResult1 == false)
            {
                this.ShowErr(selectCommand1, selectResult1);
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.selectDataTable1;
            }

            ////分別加入comboboxitem
            // cmbFactory.Items.Clear();
            // comboSize.Items.Clear();

            // List<string> dts2 = selectDataTable1.AsEnumerable().Select(row => row["FtyGroup"].ToString()).Distinct().OrderBy(o => o[0]).ToList();
            // List<string> dts3 = selectDataTable1.AsEnumerable().Select(row => row["sizespec"].ToString()).Distinct().ToList();

            // dts2.Insert(0, "");
            // dts2.Insert(0, "All");
            // dts3.Insert(0, "All");

            // if (!dts2.Empty()) cmbFactory.DataSource = dts2;

            // if (!dts2.Empty()) comboSize.DataSource = dts3;
            string sizespeccmd = string.Format(
                @"
Select distinct b.sizespec ,a.WhseClose
into #tmp
from orders a WITH (NOLOCK) 
inner join po_supp_detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.MDivisionPoDetail md WITH (NOLOCK) on md.POID = b.id and md.seq1 = b.seq1 and md.seq2 = b.seq2
inner join  po_supp c WITH (NOLOCK) on a.id = c.id
where b.scirefno = '{0}'
group by b.sizespec ,a.WhseClose

select sizespec = 'All' 
union all 
select sizespec
from
(
    select sizespec = ''
    union 
    Select distinct a.sizespec
	from #tmp a
	where a.WhseClose is null
)xx

drop table #tmp
", this.dr["scirefno"].ToString());
            DataTable dtsizespec;
            DualResult selectResult4 = DBProxy.Current.Select(null, sizespeccmd, out dtsizespec);
            if (!selectResult4)
            {
                this.ShowErr(sizespeccmd, selectResult4);
                return;
            }

            MyUtility.Tool.SetupCombox(this.comboSize, 1, dtsizespec);

            string factorycmd = string.Format(
                @"

Select a.FtyGroup ,a.WhseClose
into #tmp
from orders a WITH (NOLOCK) 
inner join po_supp_detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.MDivisionPoDetail md WITH (NOLOCK) on md.POID = b.id and md.seq1 = b.seq1 and md.seq2 = b.seq2
inner join  po_supp c WITH (NOLOCK) on a.id = c.id
where b.scirefno = '{0}'  
group by a.FtyGroup ,a.WhseClose

select FtyGroup = 'All' 
union all
select FtyGroup
from
(
    select FtyGroup = ''
    union 
    Select distinct a.FtyGroup
	from #tmp a
    where a.WhseClose is null
)xx

drop table #tmp
", this.dr["scirefno"].ToString());
            DataTable dtfactory;
            DualResult selectResult3 = DBProxy.Current.Select(null, factorycmd, out dtfactory);
            if (!selectResult3)
            {
                this.ShowErr(factorycmd, selectResult3);
                return;
            }

            MyUtility.Tool.SetupCombox(this.cmbFactory, 1, dtfactory);

            string coloridcmd = string.Format(
                @"
Select a.BrandID,b.colorid,a.WhseClose
into #tmp
from orders a WITH (NOLOCK) 
inner join po_supp_detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.MDivisionPoDetail md WITH (NOLOCK) on md.POID = b.id and md.seq1 = b.seq1 and md.seq2 = b.seq2
inner join  po_supp c WITH (NOLOCK) on a.id = c.id
where b.scirefno = '{0}' 
group by a.BrandID,b.colorid,a.WhseClose

select colorid = 'All' 
union all
select colorid
from
(
	select colorid = ''
	union 
	select distinct colorid = isnull(dbo.GetColorMultipleID(a.BrandID,a.colorid),'')
	from
	(	
		select a.BrandID,a.colorid
		from  #tmp a
		where a.WhseClose is null
		group by a.BrandID,a.colorid
	)a
)xx

drop table #tmp
", this.dr["scirefno"].ToString());
            DataTable dtcolorid;
            DualResult selectResult2 = DBProxy.Current.Select(null, coloridcmd, out dtcolorid);
            if (!selectResult2)
            {
                this.ShowErr(coloridcmd, selectResult2);
                return;
            }

            MyUtility.Tool.SetupCombox(this.comboColor, 1, dtcolorid);

            // 設定Grid1的顯示欄位
            MyUtility.Tool.AddMenuToPopupGridFilter(this, this.gridRefNo, null, "factoryid,colorid,sizespec");
            this.gridRefNo.IsEditingReadOnly = true;
            this.gridRefNo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridRefNo)
                 .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(13))
                 .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                 .Text("StyleID", header: "Style", width: Widths.AnsiChars(13))
                 .Text("SeasonID", header: "Season", width: Widths.AnsiChars(13))
                 .Text("BrandID", header: "Brand", width: Widths.AnsiChars(13))
                 .Text("seq", header: "Seq", width: Widths.AnsiChars(5))
                 .Text("colorid", header: "Color", width: Widths.AnsiChars(6))
                 .Text("sizespec", header: "Size", width: Widths.AnsiChars(15))
                 .Text("suppid", header: "Supp", width: Widths.AnsiChars(6))
                 .Date("sewinline", header: "Sewing Inline Date", width: Widths.AnsiChars(10))
                 .Text("sewline", header: "Sewing Line#", width: Widths.AnsiChars(10))
                 .Date("FinalETA", header: "FinalETA", width: Widths.AnsiChars(10))
                 .Numeric("balance", header: "Balance Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2)
                 .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(8))
                 .Text("BLocation", header: "Bulk Location", width: Widths.AnsiChars(10))
                 .Text("LobQty", header: "Scarp Qty", width: Widths.AnsiChars(10))
                 ;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Excel Processing...");
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P03_Refno.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = MyUtility.GetValue.Lookup(string.Format(
                @"
select NameEN
from Factory
where id = '{0}'", Env.User.Keyword));
            objSheets.Cells[3, 2] = MyUtility.Convert.GetString(this.dr["refno"].ToString());

            MyUtility.Excel.CopyToXls(dt.DefaultView.ToTable(), string.Empty, "Warehouse_P03_Refno.xltx", 4, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            this.HideWaitMessage();
        }

        private void Combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbFactory.SelectedValue == null || this.comboSize.SelectedValue == null || this.comboColor.SelectedValue == null)
            {
                return;
            }

            string cmbFactoryvalue = this.cmbFactory.SelectedValue.ToString();
            string comboSizevalue = this.comboSize.SelectedValue.ToString();
            string comboColorvalue = this.comboColor.SelectedValue.ToString();
            string filter = string.Format(
                @"
(FtyGroup = '{0}' or 'All'='{0}')
and (sizespec = '{1}' or 'All'='{1}')
and (colorid = '{2}' or 'All'='{2}')", cmbFactoryvalue, comboSizevalue, comboColorvalue);
            ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
        }
    }
}
