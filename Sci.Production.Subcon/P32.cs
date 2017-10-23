using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using System.Linq;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Sci.Utility.Excel;

namespace Sci.Production.Subcon
{
    public partial class P32 : Sci.Win.Tems.QueryForm
    {
        protected DataTable dtGrid1, dtGrid2; DataSet dataSet;
        string SP1, SP2,M; string POID; string sqlWhere = "";
        DateTime? sewingdate1, sewingdate2, scidate1, scidate2;
        List<string> sqlWheres = new List<string>();
        StringBuilder sqlcmd = new StringBuilder();
    
        public P32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent(); 
            this.EditMode = true;

            #region -- Grid1 Setting --
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderId", header: "Mother SP#", width: Widths.AnsiChars(15),iseditingreadonly:true)
                .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("SciDelivery", header: "Earliest SciDlv", iseditingreadonly: true)
                .Date("SewinLine", header: "Earliest SewInline", iseditingreadonly: true)
                .Text("Carton", header: "Carton", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SPThread", header: "SP Thread", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("EmbThread", header: "Emb Thread", width: Widths.AnsiChars(5), iseditingreadonly: true);
            #endregion

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.EditingMouseDoubleClick += (s, e) =>
            {
                DataRow thisRow = this.grid2.GetDataRow(this.listControlBindingSource2.Position);
                var frm = new Sci.Production.Subcon.P30_InComingList(thisRow["Ukey"].ToString());
                DialogResult result = frm.ShowDialog(this);
            };

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.EditingMouseDoubleClick += (s, e) =>
            {
                DataRow thisRow = this.grid2.GetDataRow(this.listControlBindingSource2.Position);
                var frm = new Sci.Production.Subcon.P30_AccountPayble(thisRow["Ukey"].ToString());
                DialogResult result = frm.ShowDialog(this);
            };

            #region -- Grid2 Setting --
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
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

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            
            if (sqlcmd != null)
            {
                sqlcmd.Clear();
                sqlWhere = "";
                sqlWheres.Clear();
                if (dataSet != null) 
                {
                    dataSet.Clear();
                    dtGrid1.Clear();
                    dtGrid2.Clear();
                }
            }
          
            bool sewingdate1_Empty = !this.dateSewingInline.HasValue, scidate_Empty = !this.dateSCIDelivery.HasValue, txtsp_Empty1 = this.txtSPStart.Text.Empty(), txtsp_Empty = this.txtSPEnd.Text.Empty();
            if (sewingdate1_Empty && scidate_Empty && txtsp_Empty1 && txtsp_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");
                txtSPStart.Focus();
                return;
            }

            SP1 = txtSPStart.Text.ToString();
            SP2 = txtSPEnd.Text.ToString();
            sewingdate1 = dateSewingInline.Value1;
            sewingdate2 = dateSewingInline.Value2;
            scidate1 = dateSCIDelivery.Value1;
            scidate2 = dateSCIDelivery.Value2;
            M = Sci.Env.User.Keyword;
            #region --組WHERE--
            if (!this.txtSPStart.Text.Empty())
            {
                sqlWheres.Add(" and O.ID >= '"+ SP1 + "'");
            }
            if (!this.txtSPEnd.Text.Empty())
            {
                sqlWheres.Add(" and O.ID <= '" + SP2 + "'");
            }
            if (!this.dateSCIDelivery.Value1.Empty() )
            {
                sqlWheres.Add(" and o.SciDelivery >= '" + Convert.ToDateTime(scidate1).ToShortDateString() + "'");

            } 
            if (!this.dateSCIDelivery.Value2.Empty())
            {
                sqlWheres.Add(" and o.SciDelivery <= '" + Convert.ToDateTime(scidate2).ToShortDateString() + "'");

            }
            if (!this.dateSewingInline.Value1.Empty())
            {
                sqlWheres.Add(" and o.SewInLine >= '" + Convert.ToDateTime(sewingdate1).ToShortDateString() + "'");
            }
            if (!this.dateSewingInline.Value2.Empty())
            {
                sqlWheres.Add(" and o.SewInLine <= '" + Convert.ToDateTime(sewingdate2).ToShortDateString() + "'");
            }
             sqlWhere = string.Join(" ", sqlWheres);
            #endregion
          
            #region -- sql command --
             this.ShowWaitMessage("Data Loading....");
            sqlcmd.Append(string.Format(@"
select  DISTINCT o.Poid
        , o.FactoryID
	    , OrderId = O.id
	    , O.StyleID
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
left join LocalPO L1 WITH (NOLOCK) on L1.Id=LD.Id and  L1.MdivisionID= '{0}'
cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
where o.id = o.poid
      " + sqlWhere + @"  
order by O.ID
                    
SELECT DISTINCT #tmp1.Poid
       , #tmp1.FactoryID
       , #tmp1.OrderId
	   , #tmp1.StyleID
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
DROP TABLE  #tmp1
drop table  #tmp", M));

            sqlcmd.Append(Environment.NewLine); // 換行
            //Grid2
            sqlcmd.Append(string.Format(@"
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
from LocalPO_Detail LD WITH (NOLOCK)
left join LocalPO L WITH (NOLOCK) on LD.Id=L.Id
left join LocalSupp S WITH (NOLOCK) on L.LocalSuppID = S.ID
left join localitem I WITH (NOLOCK) on I.refno = LD.refno 
left join orders O WITH (NOLOCK) on LD.OrderId=O.ID 
                                    and LD.POID = o.POID
cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
where 1=1 
      " + sqlWhere + @"
      and  L.MdivisionID= '{0}' 
order by LD.OrderId, L.Category, L.LocalSuppID", M));
            #endregion
           
            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                if (!SQL.Selects("", sqlcmd.ToString(), out dataSet))
                {
                    ShowErr(sqlcmd.ToString());
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
            dtGrid1 = dataSet.Tables[0];
            dtGrid2 = dataSet.Tables[1];
            if (dtGrid1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }
            dtGrid1.TableName = "dtGrid1";
            dtGrid2.TableName = "dtGrid2";
            listControlBindingSource1.DataSource = dtGrid1;
            listControlBindingSource2.DataSource = dtGrid2;
            listControlBindingSource1_PositionChanged(sender,e);
            this.grid1.AutoResizeColumns();//調整寬度
            this.grid2.AutoResizeColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listControlBindingSource1_PositionChanged(object sender, EventArgs e)
        {
            var parent = this.grid1.GetDataRow(this.listControlBindingSource1.Position);
            if (parent == null)
            {
                return;
            }
            POID = parent["Poid"].ToString();
            listControlBindingSource2.Filter = " Poid = '"+ POID+"'";
            this.grid2.AutoResizeColumns();
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (dataSet == null) return;
            if (dtGrid1.Rows.Count == 0) return;
            if (dtGrid2.Rows.Count == 0) return;
            DataTable dtMaster = dtGrid1.AsEnumerable().Where(row => true).CopyToDataTable();
            DataTable dtChild = dtGrid2.AsEnumerable().Where(row => true).CopyToDataTable();
            dtMaster.Columns.Remove("Poid");
            dtChild.Columns.Remove("Poid");
            Sci.Utility.Excel.SaveXltReportCls x1 = new Sci.Utility.Excel.SaveXltReportCls("Subcon_P32.xltx");
            Sci.Utility.Excel.SaveXltReportCls.XltRptTable dt1 = new SaveXltReportCls.XltRptTable(dtMaster);
            //DataView dataView = dtGrid2.DefaultView;

            if (dtChild.Columns.Contains("Ukey")) dtChild.Columns.Remove("Ukey");
            Sci.Utility.Excel.SaveXltReportCls.XltRptTable dt2 = new SaveXltReportCls.XltRptTable(dtChild);
            x1.DicDatas.Add("##dt1", dt1);
            x1.DicDatas.Add("##dt2", dt2);
            dt1.ShowHeader = false;
            dt2.ShowHeader = false;
            x1.Save(Sci.Production.Class.MicrosoftFile.GetName("Subcon_P32"));
            return ;

        }
    }
}
