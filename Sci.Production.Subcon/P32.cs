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

namespace Sci.Production.Subcon
{
    public partial class P32 : Sci.Win.Tems.QueryForm
    {
        protected DataTable dtGrid1, dtGrid2; DataSet dataSet;
        string SP1, SP2,M; string orderid; string sqlWhere = "";
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
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15),iseditingreadonly:true)
                 .Text("StyleID", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Date("MinSciDelivery", header: "Earliest Sci Dlv", iseditingreadonly: true)
                 .Date("MinSewinLine", header: "Earliest SewInline", iseditingreadonly: true)
                 .Text("Carton", header: "Carton", width: Widths.AnsiChars(5), iseditingreadonly: true)
                 .Text("SPThread", header: "SP Thread", width: Widths.AnsiChars(5), iseditingreadonly: true)
                 .Text("EmbThread", header: "Emb Thread", width: Widths.AnsiChars(5), iseditingreadonly: true)
                 ;
            #endregion

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
                 .Text("RequestID", header: "RequestID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Numeric("InQty", header: "InQty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 0, iseditingreadonly: true)
                 .Numeric("APQty", header: "APQty", width: Widths.AnsiChars(8), integer_places: 8, decimal_places: 0, iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(40), iseditingreadonly: true)
                 ;
            #endregion  
        
            this.grid2.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.grid2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);

        }

        //Query
        private void button1_Click(object sender, EventArgs e)
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
          
            bool sewingdate1_Empty = !this.dateRange_Sewing.HasValue, scidate_Empty = !this.dateRange_SCI.HasValue, txtsp_Empty1 = this.txt_SPStart.Text.Empty(), txtsp_Empty = this.txt_SPEnd.Text.Empty();
            if (sewingdate1_Empty && scidate_Empty && txtsp_Empty1 && txtsp_Empty)
            {
                MyUtility.Msg.ErrorBox("Please select at least one field entry");
                txt_SPStart.Focus();
                return;
            }

            SP1 = txt_SPStart.Text.ToString();
            SP2 = txt_SPEnd.Text.ToString();
            sewingdate1 = dateRange_Sewing.Value1;
            sewingdate2 = dateRange_Sewing.Value2;
            scidate1 = dateRange_SCI.Value1;
            scidate2 = dateRange_SCI.Value2;
            M = Sci.Env.User.Keyword;
            #region --組WHERE--
            if (!this.txt_SPStart.Text.Empty())
            {
                sqlWheres.Add(" and LD.POID >= '"+ SP1 + "'");
            }
            if (!this.txt_SPEnd.Text.Empty())
            {
                sqlWheres.Add(" and LD.POID <= '" + SP2 + "'");
            }
            if (!this.dateRange_SCI.Value1.Empty() )
            {
                sqlWheres.Add(" and o.SciDelivery >= '" + Convert.ToDateTime(scidate1).ToShortDateString() + "'");

            } 
            if (!this.dateRange_SCI.Value2.Empty())
            {
                sqlWheres.Add(" and o.SciDelivery <= '" + Convert.ToDateTime(scidate2).ToShortDateString() + "'");

            }
            if (!this.dateRange_Sewing.Value1.Empty())
            {
                sqlWheres.Add(" and o.SewInLine >= '" + Convert.ToDateTime(sewingdate1).ToShortDateString() + "'");
            }
            if (!this.dateRange_Sewing.Value2.Empty())
            {
                sqlWheres.Add(" and o.SewInLine <= '" + Convert.ToDateTime(sewingdate2).ToShortDateString() + "'");
            }
             sqlWhere = string.Join(" ", sqlWheres);
            #endregion
          
            #region -- sql command --
             this.ShowWaitMessage("Data Loading....");
            sqlcmd.Append(string.Format(@"
                    select  distinct 
									LD.OrderId,
				                    O.StyleID,
			                        GetSCI.MinSciDelivery,
				                    GetSCI.MinSewinLine,
									(select distinct L.Category+',' 
					from LocalPO_Detail LD WITH (NOLOCK)
                    left join LocalPO L WITH (NOLOCK) on L.Id=LD.Id
                    left join orders O WITH (NOLOCK) on LD.OrderId=O.ID and LD.POID = o.POID
                    cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
                    where 1=1 "+ sqlWhere + @" and  L.MdivisionID= '{0}'
                    FOR XML PATH(''))as c
				into #tmp  	
				from LocalPO_Detail LD WITH (NOLOCK)
                    left join LocalPO L WITH (NOLOCK) on L.Id=LD.Id
                    left join orders O WITH (NOLOCK) on LD.OrderId=O.ID and LD.POID = o.POID
                    cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
                    where 1=1 " + sqlWhere + @" and  L.MdivisionID= '{0}' order by LD.OrderId
                    
                    select  #tmp.OrderId,
							#tmp.StyleID,
							#tmp.MinSciDelivery,
							#tmp.MinSewinLine,
							 case
							  when #tmp.c='CARTON,' then 'Y'
							  when #tmp.c='CARTON,SP_THREAD,' then 'Y'
							  when #tmp.c='CARTON,EMB_THREAD,' then 'Y'
							  when #tmp.c='CARTON,EMB_THREAD,SP_THREAD,' then 'Y'
							  ELSE 'N'
							END
							AS Carton,
								 case
							  when #tmp.c='SP_THREAD,' then 'Y'
							  when #tmp.c='CARTON,SP_THREAD,' then 'Y'
							  when #tmp.c='EMB_THREAD,SP_THREAD,' then 'Y'
							  when #tmp.c='CARTON,EMB_THREAD,SP_THREAD,' then 'Y'
							  ELSE 'N'
							END
							AS SPThread,
									 case
							  when #tmp.c='EMB_THREAD,' then 'Y'
							  when #tmp.c='CARTON,EMB_THREAD,' then 'Y'
							  when #tmp.c='EMB_THREAD,SP_THREAD,' then 'Y'
							  when #tmp.c='CARTON,EMB_THREAD,SP_THREAD,' then 'Y'
							  ELSE 'N'
							END
							AS EmbThread
					from #tmp
				
				drop table  #tmp", M));

            sqlcmd.Append(Environment.NewLine); // 換行
            //Grid2
            sqlcmd.Append(string.Format(@"
                    select distinct LD.OrderId,
	                       L.Category,
	                       L.LocalSuppID,
	                       S.Abb,
	                       LD.Refno,
	                       LD.ThreadColorID,
	                       I.Description,
	                       LD.Qty,
	                       LD.UnitId,
	                       LD.Price,
	                       Amount=LD.Qty*LD.Price,
	                       LD.Delivery,
	                       LD.RequestID,
	                       LD.InQty,
	                       LD.APQty,
	                       LD.Remark
                    from LocalPO_Detail LD WITH (NOLOCK)
                    left join LocalPO L WITH (NOLOCK) on LD.Id=L.Id
                    left join LocalSupp S WITH (NOLOCK) on L.LocalSuppID=S.ID
                    left join localitem I WITH (NOLOCK) on I.refno = LD.refno 
                    left join orders O WITH (NOLOCK) on LD.OrderId=O.ID and LD.POID = o.POID
                    cross apply dbo.GetSCI(O.ID , O.Category) as GetSCI
                    where 1=1 
                    " + sqlWhere + @"and  L.MdivisionID= '{0}' order by LD.OrderId, L.Category, L.LocalSuppID", M));
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
            this.grid1.AutoResizeColumns();//調整寬度
            this.grid2.AutoResizeColumns();
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listControlBindingSource1_PositionChanged(object sender, EventArgs e)
        {
            var parent = this.grid1.GetDataRow(this.listControlBindingSource1.Position);
            if (parent == null) return;
            orderid = parent["orderid"].ToString();
            listControlBindingSource2.Filter = " orderid = '"+ orderid+"'";
        }
        private void btn_Excel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_P32.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(dtGrid1, "", "Subcon_P32.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            objApp.Cells.EntireColumn.AutoFit(); //自動欄寬
           // objApp.Columns().ColumnWidth = 14;
            objApp.Visible = false;
           // objApp.Cells.ColumnWidth = 14;
            //objAppEntireColumn.AutoFit(); //自動欄寬
            
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[2];   // 取得工作表
            MyUtility.Excel.CopyToXls(dtGrid2, "", "Subcon_P32.xltx", 1, true, null, null, true, objSheets, false);// 將datatable copy to excel
            objSheets.Cells.EntireColumn.AutoFit(); //自動欄寬
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return;

        }

    }
}
