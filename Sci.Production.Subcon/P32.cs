using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Ict;
using System.Linq;
using Sci.Utility.Excel;

namespace Sci.Production.Subcon
{
    public partial class P32 : Win.Tems.QueryForm
    {
        protected DataTable dtGrid1;
        protected DataTable dtGrid2;
        protected DataTable dtGrid_NoCarton; private DataSet dataSet;
        private string SP1;
        private string SP2;
        private string M; private string POID; private string sqlWhere = string.Empty;
        private DateTime? sewingdate1;
        private DateTime? sewingdate2;
        private DateTime? scidate1;
        private DateTime? scidate2;
        private List<string> sqlWheres = new List<string>();
        private StringBuilder sqlcmd = new StringBuilder();

        public P32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;

            #region -- Grid1 Setting --
            this.grid1.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderId", header: "Mother SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Earliest BuyerDlv", iseditingreadonly: true)
                .Date("SciDelivery", header: "Earliest SciDlv", iseditingreadonly: true)
                .Date("SewinLine", header: "Earliest SewInline", iseditingreadonly: true)
                .Text("Carton", header: "Carton", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SPThread", header: "SP Thread", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("EmbThread", header: "Emb Thread", width: Widths.AnsiChars(5), iseditingreadonly: true);
            #endregion

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.EditingMouseDoubleClick += (s, e) =>
            {
                DataRow thisRow = this.grid2.GetDataRow(this.listControlBindingSource2.Position);
                var frm = new P30_InComingList(thisRow["Ukey"].ToString());
                DialogResult result = frm.ShowDialog(this);
            };

            DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.EditingMouseDoubleClick += (s, e) =>
            {
                DataRow thisRow = this.grid2.GetDataRow(this.listControlBindingSource2.Position);
                var frm = new P30_AccountPayble(thisRow["Ukey"].ToString());
                DialogResult result = frm.ShowDialog(this);
            };

            #region -- Grid2 Setting --
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.grid2)
                 .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("LocalSuppID", header: "LocalSupp", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Abb", header: "Abb", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Refno", header: "Refno", width: Widths.AnsiChars(21), iseditingreadonly: true)
                 .Text("ThreadColorID", header: "ThreadColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 0, iseditingreadonly: true)
                 .Text("UnitId", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Numeric("Price", header: "Price", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Date("Delivery", header: "Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("ID", header: "ID#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("RequestID", header: "RequestID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Numeric("InQty", header: "InQty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 0, iseditingreadonly: true, settings: ns)
                 .Numeric("APQty", header: "APQty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 0, iseditingreadonly: true, settings: ns2)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(40), iseditingreadonly: true)
                 ;
            #endregion

            this.grid2.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.grid2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (this.sqlcmd != null)
            {
                this.sqlcmd.Clear();
                this.sqlWhere = string.Empty;
                this.sqlWheres.Clear();
                if (this.dataSet != null)
                {
                    this.dataSet.Clear();
                    this.dtGrid1.Clear();
                    this.dtGrid2.Clear();
                }
            }

            bool sewingdate1_Empty = !this.dateSewingInline.HasValue, scidate_Empty = !this.dateSCIDelivery.HasValue, txtsp_Empty1 = this.txtSPStart.Text.Empty(), txtsp_Empty = this.txtSPEnd.Text.Empty();
            if (sewingdate1_Empty && scidate_Empty && txtsp_Empty1 && txtsp_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");
                this.txtSPStart.Focus();
                return;
            }

            this.SP1 = this.txtSPStart.Text.ToString();
            this.SP2 = this.txtSPEnd.Text.ToString();
            this.sewingdate1 = this.dateSewingInline.Value1;
            this.sewingdate2 = this.dateSewingInline.Value2;
            this.scidate1 = this.dateSCIDelivery.Value1;
            this.scidate2 = this.dateSCIDelivery.Value2;
            this.M = Env.User.Keyword;
            #region --組WHERE--
            if (!this.txtSPStart.Text.Empty())
            {
                this.sqlWheres.Add(" and O.ID >= '" + this.SP1 + "'");
            }

            if (!this.txtSPEnd.Text.Empty())
            {
                this.sqlWheres.Add(" and O.ID <= '" + this.SP2 + "'");
            }

            if (!this.dateSCIDelivery.Value1.Empty())
            {
                this.sqlWheres.Add(" and o.SciDelivery >= '" + Convert.ToDateTime(this.scidate1).ToShortDateString() + "'");
            }

            if (!this.dateSCIDelivery.Value2.Empty())
            {
                this.sqlWheres.Add(" and o.SciDelivery <= '" + Convert.ToDateTime(this.scidate2).ToShortDateString() + "'");
            }

            if (!this.dateSewingInline.Value1.Empty())
            {
                this.sqlWheres.Add(" and o.SewInLine >= '" + Convert.ToDateTime(this.sewingdate1).ToShortDateString() + "'");
            }

            if (!this.dateSewingInline.Value2.Empty())
            {
                this.sqlWheres.Add(" and o.SewInLine <= '" + Convert.ToDateTime(this.sewingdate2).ToShortDateString() + "'");
            }

            this.sqlWhere = string.Join(" ", this.sqlWheres);
            #endregion

            #region -- sql command --
            this.ShowWaitMessage("Data Loading....");
            this.sqlcmd.Append(string.Format(
                @"
select  DISTINCT o.Poid
        , o.FactoryID
	    , OrderId = O.id
	    , O.StyleID
        , BuyerDelivery = GetSCI.MinBuyerDelivery
        , SciDelivery = GetSCI.MinSciDelivery
        , SewinLine = GetSCI.MinSewinLine
	    , c = (select distinct L.Category+',' 
	           from LocalPO L WITH (NOLOCK)
	           where L.Id=L1.ID and  L.MdivisionID= '{0}'
               FOR XML PATH(''))
into #tmp1  	
from Orders  O WITH (NOLOCK)
left join LocalPO_Detail LD WITH (NOLOCK) on LD.OrderId = O.ID 
                                                and LD.POID = o.POID
left join LocalPO L1 WITH (NOLOCK) on L1.Id=LD.Id 
cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
where o.id = o.poid
and  o.MdivisionID= '{0}'
      " + this.sqlWhere + @"  
order by O.ID
                    
SELECT DISTINCT #tmp1.Poid
       , #tmp1.FactoryID
       , #tmp1.OrderId
	   , #tmp1.StyleID
	   , #tmp1.BuyerDelivery
	   , #tmp1.SciDelivery
       , #tmp1.SewinLine
       , Category = (SELECT TMP2.c+''
		             FROM #tmp1 AS TMP2
		             WHERE TMP2.OrderId = #tmp1.OrderId 
		             FOR XML PATH(''))
INTO #tmp
FROM #tmp1
                    
select  #tmp.Poid
        , #tmp.FactoryID
        , #tmp.OrderId
		, #tmp.StyleID
		, #tmp.BuyerDelivery
		, #tmp.SciDelivery
		, #tmp.SewinLine
		, Carton = case
			            when #tmp.Category like '%CARTON%' then 'Y'			          
			            ELSE 'N'
		          END
		, SPThread = case
			            when #tmp.Category like '%SP_THREAD%' then 'Y'			            
			            ELSE 'N'
		            END
        , EmbThread = case
			            when #tmp.Category like '%EMB_THREAD%' then 'Y'			           
			            ELSE 'N'
		              END
from #tmp order by #tmp.OrderId
DROP TABLE  #tmp1", this.M));

            this.sqlcmd.Append(Environment.NewLine); // 換行

            // Grid2
            this.sqlcmd.Append(string.Format(
                @"
select distinct o.Poid
       , LD.OrderId
	   , L.Category
	   , L.LocalSuppID
	   , S.Abb
	   , LD.Refno
	   , LD.ThreadColorID
	   , I.Description
	   , LD.Qty
	   , LD.UnitId
	   , LD.Price
	   , Amount = LD.Qty * LD.Price
	   , LD.Delivery
       , LD.ID
	   , LD.RequestID
	   , LD.InQty
	   , LD.APQty
	   , LD.Remark
       , LD.Ukey 
into #tmp2                           
from LocalPO_Detail LD WITH (NOLOCK)
left join LocalPO L WITH (NOLOCK) on LD.Id=L.Id
left join LocalSupp S WITH (NOLOCK) on L.LocalSuppID = S.ID
left join localitem I WITH (NOLOCK) on I.refno = LD.refno 
left join orders O WITH (NOLOCK) on LD.OrderId=O.ID 
                                    and LD.POID = o.POID
cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
where 1=1 
      " + this.sqlWhere + @"
      and  L.MdivisionID= '{0}' 

select * from #tmp2
order by OrderId, Category, LocalSuppID

-- 匯出 Carton = N 資料


select [Factory] = o.FactoryID
,[SP] = o.ID
,[Style] = o.StyleID
,[BuyerDlv] = o.BuyerDelivery
,[SciDlv] = o.SciDelivery
,[SewinLine] = o.SewInLine
,[Carton] = 'N'
,[SP_Thread] = isnull((	
		select 'Y' 
		where exists(
		select 1
		from #tmp2 
		where Category='SP_Thread' and OrderId=o.ID)
	),'N')
,[EMB_Thread] = isnull((
		select 'Y' 
		where exists( select 1
		from #tmp2 
		where Category='EMB_Thread' and OrderId=o.ID)
	),'N')
from orders o
inner join #tmp tmp on tmp.poid = o.POID
where not exists(
	select 1 
	from #tmp2 
	where Category='Carton'
	and OrderId=o.ID )
order by o.ID

drop table  #tmp,#tmp2
", this.M));
            #endregion

            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                if (!SQL.Selects(string.Empty, this.sqlcmd.ToString(), out this.dataSet))
                {
                    this.ShowErr(this.sqlcmd.ToString());
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBProxy.Current.DefaultTimeout = 0;
            }

            this.HideWaitMessage();
            this.dtGrid1 = this.dataSet.Tables[0];
            this.dtGrid2 = this.dataSet.Tables[1];
            this.dtGrid_NoCarton = this.dataSet.Tables[2];
            if (this.dtGrid1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            this.dtGrid1.TableName = "dtGrid1";
            this.dtGrid2.TableName = "dtGrid2";
            this.listControlBindingSource1.DataSource = this.dtGrid1;
            this.listControlBindingSource2.DataSource = this.dtGrid2;
            this.ListControlBindingSource1_PositionChanged(sender, e);
            this.grid1.AutoResizeColumns(); // 調整寬度
            this.grid2.AutoResizeColumns();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListControlBindingSource1_PositionChanged(object sender, EventArgs e)
        {
            var parent = this.grid1.GetDataRow(this.listControlBindingSource1.Position);
            if (parent == null)
            {
                return;
            }

            this.POID = parent["Poid"].ToString();
            this.listControlBindingSource2.Filter = " Poid = '" + this.POID + "'";
            this.grid2.AutoResizeColumns();
        }

        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            if (this.dataSet == null)
            {
                return;
            }

            if (this.dtGrid1.Rows.Count == 0)
            {
                return;
            }

            SaveXltReportCls x1 = new SaveXltReportCls("Subcon_P32.xltx");
            if (this.chkCarton.Checked)
            {
                if (this.dtGrid_NoCarton.Rows.Count == 0)
                {
                    return;
                }

                SaveXltReportCls.XltRptTable dt = new SaveXltReportCls.XltRptTable(this.dtGrid_NoCarton);
                x1.DicDatas.Add("##dt1", dt);
                dt.ShowHeader = false;
                Microsoft.Office.Interop.Excel.Worksheet ws = x1.ExcelApp.ActiveWorkbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Worksheet ws2 = x1.ExcelApp.ActiveWorkbook.Worksheets[2];
                ws.Cells[1, 2] = "SP#";
                ws.Cells[1, 4] = "BuyerDlv";
                ws.Cells[1, 5] = "SciDlv";
                ws.Cells[1, 6] = "SewInline";
                ws.Name = "Carton = N";
                ws2.Delete();
            }
            else
            {
                if (this.dtGrid2.Rows.Count == 0)
                {
                    return;
                }

                DataTable dtMaster = this.dtGrid1.AsEnumerable().Where(row => true).CopyToDataTable();
                DataTable dtChild = this.dtGrid2.AsEnumerable().Where(row => true).CopyToDataTable();
                dtMaster.Columns.Remove("Poid");
                dtChild.Columns.Remove("Poid");
                SaveXltReportCls.XltRptTable dt1 = new SaveXltReportCls.XltRptTable(dtMaster);

                // DataView dataView = dtGrid2.DefaultView;
                if (dtChild.Columns.Contains("Ukey"))
                {
                    dtChild.Columns.Remove("Ukey");
                }

                SaveXltReportCls.XltRptTable dt2 = new SaveXltReportCls.XltRptTable(dtChild);
                x1.DicDatas.Add("##dt1", dt1);
                x1.DicDatas.Add("##dt2", dt2);
                dt1.ShowHeader = false;
                dt2.ShowHeader = false;
            }

            x1.Save(Class.MicrosoftFile.GetName("Subcon_P32"));
            return;
        }
    }
}
