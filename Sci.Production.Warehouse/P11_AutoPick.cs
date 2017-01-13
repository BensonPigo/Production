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
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P11_AutoPick : Sci.Win.Subs.Base
    {
        StringBuilder sbSizecode;
        string poid, issueid, cutplanid,orderid;
        public DataTable BOA, BOA_Orderlist, BOA_PO, BOA_PO_Size,dtIssueBreakDown;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public Dictionary<DataRow, DataTable> dictionaryDatas = new Dictionary<DataRow, DataTable>();
        public P11_AutoPick(string _issueid, string _poid, string _cutplanid, string _orderid, DataTable _dtIssueBreakDown, StringBuilder _sbSizecode)
        {            
            InitializeComponent();
            poid = _poid;
            issueid = _issueid;
            cutplanid = _cutplanid;
            orderid = _orderid;
            dtIssueBreakDown = _dtIssueBreakDown;
            sbSizecode=_sbSizecode;
            this.Text += string.Format(" ({0})", poid);
            //gridBOA.RowPostPaint += (s, e) =>
            //{
            //    DataTable dtSource = (DataTable)listControlBindingSource1.DataSource;

            //    DataRow dr = gridBOA.GetDataRow(e.RowIndex);
            //    bool exists = dtSource
            //        .AsEnumerable()
            //        .Any(dataRow =>
            //        {
            //            return string.Compare(dataRow.Field<string>("scirefno"), dr.Field<string>("scirefno"), true) == 0 &&
            //                   string.Compare(dataRow.Field<string>("colorid"), dr.Field<string>("colorid"), true) == 0 &&
            //                   string.Compare(dataRow.Field<string>("sizespec"), dr.Field<string>("sizespec"), true) == 0;
            //        });

            //    if (exists)
            //    {
            //        gridBOA.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            //    }

            //};
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (dtIssueBreakDown == null )
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            Decimal sum = 0;
            foreach (DataRow dr in dtIssueBreakDown.Rows)
            {
                foreach (DataColumn dc in dtIssueBreakDown.Columns)
                {
                    if (Object.ReferenceEquals(sum.GetType(), dr[dc].GetType()))
                        sum += (Decimal)dr[dc];                 
                }
            }

            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            //POPrg.BOAExpend(poid, "0", 1, out BOA, out BOA_Orderlist);
            SqlConnection sqlConnection = null;
            SqlCommand sqlCmd = null;
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = null;
            DataTable[] result = null;

            BOA = null;
            BOA_Orderlist = null;
            BOA_PO = null;
            BOA_PO_Size = null;

            string sqlcmd;
            sqlcmd = string.Format(@";
;WITH UNPIVOT_1
AS
(
    SELECT * FROM #tmp
    UNPIVOT
    (
    QTY
    FOR SIZECODE IN ({0})
    )
    AS PVT
)select *
into #tmp2
from UNPIVOT_1 ;
delete from #tmp2 where qty = 0;

	declare @count as int;
	create Table #Tmp_Order_Qty
		(  ID Varchar(13), FactoryID Varchar(8), CustCDID Varchar(16), ZipperInsert Varchar(5)
		 , CustPONo VarChar(30), BuyMonth VarChar(16), CountryID VarChar(2), StyleID Varchar(15)
		 , Article VarChar(8), SizeSeq VarChar(2), SizeCode VarChar(8), Qty Numeric(6,0)
		);

    Insert Into #Tmp_Order_Qty
		Select Orders.ID, Orders.FactoryID, Orders.CustCDID, CustCD.ZipperInsert
			 , Orders.CustPONo, Orders.BuyMonth, Factory.CountryID, Orders.StyleID
			 , Order_Article.Article, Order_SizeCode.Seq, Order_SizeCode.SizeCode
			 , IsNull(#tmp2.Qty, 0) Qty
		  From dbo.Orders
		  Left Join dbo.Order_SizeCode
			On Order_SizeCode.ID = Orders.POID
		  Left Join dbo.Order_Article
			On Order_Article.ID = Orders.ID
		  Left Join #tmp2
			On	   #tmp2.OrderID = Orders.ID
			   And #tmp2.SizeCode = Order_SizeCode.SizeCode
			   And #tmp2.Article = Order_Article.Article
		  Left Join dbo.CustCD
			On	   CustCD.BrandID = Orders.BrandID
			   And CustCD.ID = Orders.CustCDID
		  Left Join dbo.Factory
			On Factory.ID = Orders.FactoryID
		 Where Orders.POID = '{3}'
		   And Orders.Junk = 0
		 --  AND Issue_Breakdown.ID = '{1}'
		 --Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
			--	, CountryID, StyleID, Article, Seq, SizeCode;

	select @count = count(1) from #Tmp_Order_Qty;
	if @count = 0
	begin
		Insert Into #Tmp_Order_Qty
		Select Orders.ID, Orders.FactoryID, Orders.CustCDID, CustCD.ZipperInsert
			 , Orders.CustPONo, Orders.BuyMonth, Factory.CountryID, Orders.StyleID
			 , Order_Article.Article, Order_SizeCode.Seq, Order_SizeCode.SizeCode
			 , IsNull(Order_Qty.Qty, 0) Qty
		  From dbo.Orders
		  Left Join dbo.Order_SizeCode
			On Order_SizeCode.ID = Orders.POID
		  Left Join dbo.Order_Article
			On Order_Article.ID = Orders.ID
		  Left Join dbo.Order_Qty
			On	   Order_Qty.ID = Orders.ID
			   And Order_Qty.SizeCode = Order_SizeCode.SizeCode
			   And Order_Qty.Article = Order_Article.Article
		  Left Join dbo.CustCD
			On	   CustCD.BrandID = Orders.BrandID
			   And CustCD.ID = Orders.CustCDID
		  Left Join dbo.Factory
			On Factory.ID = Orders.FactoryID
		 Where Orders.POID = '{3}'
		   And Orders.Junk = 0
		   AND Order_Qty.ID = '{2}'
		 --Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
			--	, CountryID, StyleID, Article, Seq, SizeCode;
		select @count = count(1) from #Tmp_Order_Qty;
		if @count = 0
		begin
			Insert Into #Tmp_Order_Qty
			Select Orders.ID, Orders.FactoryID, Orders.CustCDID, CustCD.ZipperInsert
				 , Orders.CustPONo, Orders.BuyMonth, Factory.CountryID, Orders.StyleID
				 , Order_Article.Article, Order_SizeCode.Seq, Order_SizeCode.SizeCode
				 , IsNull(Order_Qty.Qty, 0) Qty
			  From dbo.Orders
			  Left Join dbo.Order_SizeCode
				On Order_SizeCode.ID = Orders.POID
			  Left Join dbo.Order_Article
				On Order_Article.ID = Orders.ID
			  Left Join dbo.Order_Qty
				On	   Order_Qty.ID = Orders.ID
				   And Order_Qty.SizeCode = Order_SizeCode.SizeCode
				   And Order_Qty.Article = Order_Article.Article
			  Left Join dbo.CustCD
				On	   CustCD.BrandID = Orders.BrandID
				   And CustCD.ID = Orders.CustCDID
			  Left Join dbo.Factory
				On Factory.ID = Orders.FactoryID
			 Where Orders.POID = '{3}'
			   And Orders.Junk = 0
			 --Order By ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
				--	, CountryID, StyleID, Article, Seq, SizeCode;
		end
	end

	Create Table #Tmp_BoaExpend
		(  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
		 , RefNo VarChar(20), SCIRefNo VarChar(26), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
		 , SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
		 , OrderQty Numeric(6,0), Price Numeric(8,4), UsageQty Numeric(9,2), UsageUnit VarChar(8), SysUsageQty  Numeric(9,2)
		 , BomFactory VarChar(8), BomCountry VarChar(2), BomStyle VarChar(15), BomCustCD VarChar(20)
		 , BomArticle VarChar(8), BomZipperInsert VarChar(5), BomBuymonth VarChar(10), BomCustPONo VarChar(30), Keyword VarChar(Max)
		 , Primary Key (ExpendUkey)
		);
	Create NonClustered Index Idx_ID on #Tmp_BoaExpend (ID, Order_BOAUkey, ColorID) -- table index

	Exec dbo.BoaExpend '{3}', {4}, {5}, '{6}',0,1;
	Drop Table #Tmp_Order_Qty;
    --Select * From #Tmp_BoaExpend group by id,Order_BOAUkey,RefNo,SCIRefNo,Article,ColorID,SuppColor,SizeCode,SizeSpec,SizeUnit,Remark, OrderQty, Price, UsageQty, UsageUnit, SysUsageQty , BomZipperInsert , BomCustPONo;

	select p.id as [poid], p.seq1, p.seq2, p.SCIRefno,dbo.getMtlDesc(p.id, p.seq1, p.seq2,2,0) [description] 
	,p.ColorID, p.SizeSpec, p.Spec, p.Special, p.Remark into #tmpPO_supp_detail
		from dbo.PO_Supp_Detail as p 
	inner join dbo.Fabric f on f.SCIRefno = p.SCIRefno
	inner join dbo.MtlType m on m.id = f.MtlTypeID
	where p.id='{3}' and p.FabricType = 'A' and m.IssueType='{7}'

	;with cte2 
	as
	(
		select m.*,m.InQty-m.OutQty+m.AdjustQty as [balanceqty]
		from #tmpPO_supp_detail inner join dbo.FtyInventory m on m.POID = #tmpPO_supp_detail.poid and m.seq1 = #tmpPO_supp_detail.seq1 and m.seq2 = #tmpPO_supp_detail.SEQ2
		and m.MDivisionID = '{8}' and m.StockType = 'B' and Roll=''
		where lock = 0
	)
	select 0 as [Selected],''as id,a.Refno,b.*,isnull(sum(a.OrderQty),0.00) qty,concat(Ltrim(Rtrim(b.seq1)), ' ', b.seq2) as seq,cte2.MDivisionID,isnull(cte2.balanceqty,0) as balanceqty,cte2.Ukey as ftyinventoryukey,cte2.StockType,cte2.Roll,cte2.Dyelot
	from #tmpPO_supp_detail b
	left join cte2 on cte2.poid = b.poid and cte2.seq1 = b.seq1 and cte2.SEQ2 = b.SEQ2
	left join #Tmp_BoaExpend a on b.SCIRefno = a.scirefno and b.poid = a.ID
	 and (b.SizeSpec = a.SizeSpec) and (b.ColorID = a.ColorID)
	 group by b.poid,b.seq1,b.seq2,a.Refno,b.[description],b.ColorID,b.SizeSpec,b.SCIRefno,b.Spec,b.Special,b.Remark,cte2.MDivisionID,cte2.balanceqty,cte2.Ukey,cte2.StockType,cte2.Roll,cte2.Dyelot
	 order by b.scirefno,b.ColorID,b.SizeSpec,b.Special,b.poid,b.seq1,b.seq2;

	 with cte
	 as(
	 select b.poid,b.seq1,b.seq2,a.SizeCode,isnull(sum(a.OrderQty),0.00) qty 
				from (#tmpPO_supp_detail b left join #Tmp_BoaExpend a 
				on b.SCIRefno = a.scirefno and b.poid = a.ID and (b.SizeSpec = a.SizeSpec) and (b.ColorID = a.ColorID)) 
					group by b.poid,b.seq1,b.seq2,a.SizeCode
	 )
	 
	 select z.*,isnull(cte.qty,0) as qty,isnull(cte.qty,0) as ori_qty from
	 (select x.poid,x.seq1,x.seq2,order_sizecode.SizeCode,Order_SizeCode.Seq 
		from dbo.order_sizecode 
			,(select distinct poid,seq1,seq2 from cte) as x
		where Order_SizeCode.id = '{3}') z 
	left join cte on cte.SizeCode = z.SizeCode and cte.poid = z.poid and cte.seq1 = z.seq1 and cte.seq2 = z.seq2
	order by z.seq1,z.seq2,z.Seq", sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1), issueid, orderid, poid, 0, 1, Env.User.UserID, "Sewing", Env.User.Keyword);//.Replace("[", "[_")

            // 呼叫procedure，取得BOA展開結果
            try
            {
                //SqlConnection conn;
                DBProxy.Current.OpenConnection(null, out sqlConnection);
                //DataTable source = null, a = null; ;
                string aaa = sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1).Replace("[", "").Replace("]", "");//.Replace("[", "").Replace("]", "")
                var RESULT = MyUtility.Tool.ProcessWithDatatable(dtIssueBreakDown, "OrderID,Article," + aaa, sqlcmd, out result, "#tmp", conn: sqlConnection);
                //ProcessWithDatatable2(dtIssueBreakDown, "OrderID,Article," + aaa
                //    , sqlcmd, out result, "#tmp");
                if (!RESULT) ShowErr(RESULT);
                if (!RESULT) return;
                //var x = 0;
                //sqlCmd = new SqlCommand(sqlcmd, sqlConnection);
                //sqlCmd.CommandType = CommandType.Text;

                ////sqlCmd.CommandType = CommandType.StoredProcedure;
                ////sqlCmd.Parameters.Add(new SqlParameter("@IssueID", issueid));
                ////sqlCmd.Parameters.Add(new SqlParameter("@OrderID", issueid));
                ////sqlCmd.Parameters.Add(new SqlParameter("@POID", poid));
                ////sqlCmd.Parameters.Add(new SqlParameter("@Order_BOAUkey", "0"));
                ////sqlCmd.Parameters.Add(new SqlParameter("@TestType", "1"));
                ////sqlCmd.Parameters.Add(new SqlParameter("@UserID", Env.User.UserID));
                ////sqlCmd.Parameters.Add(new SqlParameter("@IssueType", "Sewing"));
                ////sqlCmd.Parameters.Add(new SqlParameter("@MDivisionId", Env.User.Keyword));
                //sqlCmd.CommandTimeout = 300;
                //sqlDataAdapter = new SqlDataAdapter(sqlCmd);

               // sqlDataAdapter.Fill(dataSet);

                if (result.Length > 0)
                {
                    dataSet.Tables.AddRange(result);
                    BOA = result[0];
                    BOA_Orderlist = result[1];
                    BOA_PO = result[2];
                    BOA_PO.DefaultView.Sort = "poid,seq1,seq2";
                    BOA_PO_Size = result[3];
                    BOA_PO.ColumnsStringAdd("Output");

                    DataRelation relation = new DataRelation("rel1"
                    , new DataColumn[] { BOA_PO.Columns["Poid"], BOA_PO.Columns["seq1"], BOA_PO.Columns["seq2"] }
                    , new DataColumn[] { BOA_PO_Size.Columns["Poid"], BOA_PO_Size.Columns["seq1"], BOA_PO_Size.Columns["seq2"] }
                    );
                    dataSet.Relations.Add(relation);
                    

                    foreach (DataRow dr in BOA_PO.Rows)
                    {

                        DataTable tmp = new DataTable();
                        tmp.ColumnsStringAdd("Poid");
                        tmp.ColumnsStringAdd("seq1");
                        tmp.ColumnsStringAdd("seq2");
                        tmp.ColumnsStringAdd("seq");
                        tmp.ColumnsStringAdd("sizecode");
                        tmp.ColumnsDecimalAdd("qty");
                        tmp.ColumnsDecimalAdd("ori_qty");

                        var drs = dr.GetChildRows(relation);
                        if (drs.Count() > 0)
                        {
                            var Output="";
                            foreach (DataRow dr2 in drs)
                            {
                                if (Convert.ToInt32(dr2["qty"]) != 0)
                                    Output += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00")+", ";
                                tmp.ImportRow(dr2);
                            }
                            dr["Output"] = Output;
                        }

                        tmp.AcceptChanges();

                        if (tmp.Rows.Count > 0)
                        {
                            dictionaryDatas.Add(dr, tmp);
                        }
                        else
                        {
                            dictionaryDatas.Add(dr, new DataTable());
                        }
                    }
                    var tmp2 = dictionaryDatas.Count;
                }

            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.GetBaseException().ToString(), "Error");

            }
            finally
            {
                //sqlCmd.Dispose();
                //sqlDataAdapter.Dispose();
                dataSet.Dispose();
                sqlConnection.Close();
            }

            this.listControlBindingSource1.DataSource = BOA_PO;
            this.grid1.DataSource = listControlBindingSource1;

            this.grid1.AutoResizeColumns();
            //this.gridBOA.DataSource = BOA;
            //Helper.Controls.Grid.Generator(this.gridBOA)

            //   .Text("ID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) //1
            //   .Text("RefNo", header: "RefNo", iseditingreadonly: true, width: Widths.AnsiChars(13)) //2
            //   .Text("SCIRefNo", header: "SCIRefNo", iseditingreadonly: true, width: Widths.AnsiChars(17)) //2
            //    .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //     .Text("ColorID", header: "ColorID", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //      .Text("SizeCode", header: "SizeCode", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //        .Text("SizeSpec", header: "SizeSpec", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //           .Text("OrderQty", header: "OrderQty", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //           .Text("UsageQty", header: "UsageQty", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //           .Text("UsageUnit", header: "UsageUnit", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2
            //           .Text("SysUsageQty", header: "SysUsageQty", iseditingreadonly: true, width: Widths.AnsiChars(6)) //2

              // .Numeric("", header: "SizeSpec", iseditingreadonly: true, decimal_places: 2, integer_places: 10) //3

               ;
            // this.gridBOA.AutoGenerateColumns = true;
            // this.gridBOA.AutoResizeColumns();


            #region --Pick Qty 開窗--
            Ict.Win.DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                var frm = new Sci.Production.Warehouse.P11_AutoPick_Detail();
                DataTable tmpDt = dictionaryDatas[grid1.GetDataRow(e.RowIndex)];
                //DataTable _clone = tmpDt.Copy();
                frm.SetGrid(tmpDt);
                DialogResult DResult = frm.ShowDialog(this);

                if (DResult == DialogResult.OK)
                    sum_subDetail(dr, tmpDt);
                if (tmpDt != null)
                {
                    var Output = "";
                    foreach (DataRow dr2 in tmpDt.ToList())
                    {
                        if (Convert.ToInt32(dr2["qty"]) != 0)
                            Output += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00") + ", ";
                    }
                    dr["Output"] = Output;
                }
                if (Convert.ToDecimal(dr["qty"]) > 0) dr["Selected"] = 1;
                else dr["Selected"] = 0;
                
            };
            #endregion
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {              
                var dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (null == dr) return;
                dr["qty"] = e.FormattedValue;
                if (Convert.ToDecimal(dr["qty"]) > 0 && Convert.ToDecimal(dr["Balanceqty"]) >= Convert.ToDecimal(dr["qty"])) dr["Selected"] = 1;
                else dr["Selected"] = 0;
            };

            #region --設定Grid1的顯示欄位--
            MyUtility.Tool.SetGridFrozen(grid1);

            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4), iseditingreadonly: true)
                 .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("scirefno", header: "SCI Refno", width: Widths.AnsiChars(23), iseditingreadonly: true)
                 .Text("colorid", header: "Color ID", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Text("sizespec", header: "SizeSpec", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("qty", header: "Pick Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, settings: ns2)
                 .Text("Output", header: "Output", width: Widths.AnsiChars(10), settings: ns)
                 .Text("Balanceqty", header: "Bulk Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("suppcolor", header: "Supp Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("sizecode", header: "SizeCode", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("sizeunit", header: "Size Unit", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("usageqty", header: "Usage Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("usageunit", header: "Usage Unit", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;
            grid1.Columns[8].Frozen = true;  //Qty
            grid1.Columns[8].DefaultCellStyle.BackColor = Color.Pink;   //Qty
            grid1.Columns[9].DefaultCellStyle.BackColor = Color.Pink;   //Qty
            #endregion

        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataRow[] dr2 = BOA_PO.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = BOA_PO.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Pick Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = BOA_PO.Select("qty > balanceqty and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Pick Qty of selected row can't over Bulk Qty!", "Warning");
                return;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp")
        {
            result = null;
            StringBuilder sb = new StringBuilder();
            if (temptablename.TrimStart().StartsWith("#"))
            {
                sb.Append(string.Format("create table {0} (", temptablename));
            }
            else
            {
                sb.Append(string.Format("create table #{0} (", temptablename));
            }
            string[] cols = tmp_columns.Split(',');
            for (int i = 0; i < cols.Length; i++)
            {
                if (MyUtility.Check.Empty(cols[i])) continue;
                switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
                {
                    case TypeCode.Boolean:
                        sb.Append(string.Format("[{0}] bit", cols[i]));
                        break;

                    case TypeCode.Char:
                        sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
                        break;

                    case TypeCode.DateTime:
                        sb.Append(string.Format("[{0}] datetime", cols[i]));
                        break;

                    case TypeCode.Decimal:
                        sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
                        break;

                    case TypeCode.Int32:
                        sb.Append(string.Format("[{0}] int", cols[i]));
                        break;

                    case TypeCode.String:
                        sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
                        break;

                    case TypeCode.Int64:
                        sb.Append(string.Format("[{0}] bigint", cols[i]));
                        break;
                    default:
                        break;
                }
                if (i < cols.Length - 1) { sb.Append(","); }
            }
            sb.Append(")");

            System.Data.SqlClient.SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }
                using (System.Data.SqlClient.SqlBulkCopy bulkcopy = new System.Data.SqlClient.SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }
                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }
                result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
                if (!result2) { MyUtility.Msg.ShowException(null, result2); return; }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
