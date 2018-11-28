using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    public partial class P30_IrregularPriceReason : Sci.Win.Subs.Base
    {
        DataTable _IrregularPriceReasonDT;
        DataTable OriginDT_FromDB;
        DataTable ModifyDT_FromP30;
        //int _Type = 0;
        bool _useDBdata;
        string _LocalPO_ID = string.Empty;
        string _FactoryID = string.Empty;

        //「未曾有過」價格異常紀錄，現在價格異常 Type = 1
        //「曾經」有過價格異常紀錄，現在還是異常 Type = 2
        //「曾經」有過價格異常紀錄，現在價格正常 Type = 3
        public P30_IrregularPriceReason(DataTable IrregularPriceReasonDT, string LocalPO_ID, string FactoryID, bool useDBdata)
        {
            InitializeComponent();

            this.EditMode = false;
            _IrregularPriceReasonDT = IrregularPriceReasonDT;
            _LocalPO_ID = LocalPO_ID;
            _FactoryID = FactoryID;
            _useDBdata = useDBdata;
        }

        protected override void OnFormLoaded()
        {

            if (_useDBdata || MyUtility.Check.Empty(_IrregularPriceReasonDT))
            {
                IrregularPriceReasonDT_Initial(true);
            }
            else
            {
                ModifyDT_FromP30 = _IrregularPriceReasonDT.Copy();
                listControlBindingSource1.DataSource = _IrregularPriceReasonDT;
                btnEdit.Enabled = true;
            }
            this.gridgridIrregularPrice.DataSource = listControlBindingSource1;

            DataGridViewGeneratorTextColumnSettings col_SubconReasonID = new DataGridViewGeneratorTextColumnSettings();

            col_SubconReasonID.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridgridIrregularPrice.GetDataRow<DataRow>(e.RowIndex);
                    string sqlCmd = $@"SELECT ID
                                        ,(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = SubconReason.Responsible) as ResponsibleName
                                        ,Reason 
                                        FROM SubconReason WHERE Type = 'IP' AND Junk = 0";
                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,20,20", string.Empty, "ID,Responsible,Reason");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        dr["SubconReasonID"] = item.GetSelectedString();

                        DataTable dt;
                        DBProxy.Current.Select(null, $"SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason FROM SubconReason WHERE ID='{item.GetSelectedString()}' AND Type='IP' AND Junk=0", out dt);
                        dr["ResponsibleID"] = dt.Rows[0]["ResponsibleID"];
                        dr["ResponsibleName"] = dt.Rows[0]["ResponsibleName"];
                        dr["Reason"] = dt.Rows[0]["Reason"];
                    }
                }
            };

            col_SubconReasonID.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;//非編輯模式 
                if (e.RowIndex == -1) return; //沒東西 return

                DataRow dr = gridgridIrregularPrice.GetDataRow(e.RowIndex);
                //允許空白
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["SubconReasonID"] = "";
                    dr["ResponsibleID"] = "";
                    dr["ResponsibleName"] = "";
                    dr["Reason"] = "";
                    return;
                }
                string ori_SubconReasonID = dr["SubconReasonID"].ToString();
                DataTable dt;
                DBProxy.Current.Select(null, $"SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason FROM SubconReason WHERE ID='{e.FormattedValue}' AND Type='IP' AND Junk=0", out dt);

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("No Data!!");
                    dr["SubconReasonID"] = "";
                    dr["ResponsibleID"] = "";
                    dr["ResponsibleName"] = "";
                    dr["Reason"] = "";
                }
                else
                {
                    dr["SubconReasonID"] = dt.Rows[0]["ID"];
                    dr["ResponsibleID"] = dt.Rows[0]["ResponsibleID"];
                    dr["ResponsibleName"] = dt.Rows[0]["ResponsibleName"];
                    dr["Reason"] = dt.Rows[0]["Reason"];
                }

            };

            #region Grid欄位設定

            Helper.Controls.Grid.Generator(this.gridgridIrregularPrice)
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Category", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("POID", header: "POID", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("PoPrice", header: "PO" + Environment.NewLine + "Price", decimal_places: 4, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("StdPrice", header: "Standard" + Environment.NewLine + "Price", decimal_places: 4, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("SubconReasonID", header: "Reason" + Environment.NewLine + "ID", width: Widths.AnsiChars(7), settings: col_SubconReasonID)
                .Text("ResponsibleID", header: "ResponsibleID", iseditingreadonly: true, width: null)
                .Text("ResponsibleName", header: "Responsible", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Reason", header: "Reason", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("AddDate", header: "Create" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("AddName", header: "Create" + Environment.NewLine + "Name", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditDate", header: "Edit" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditName", header: "Edit" + Environment.NewLine + "Name", iseditingreadonly: true, width: Widths.AnsiChars(10));
            #endregion

            this.gridgridIrregularPrice.Columns["SubconReasonID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridgridIrregularPrice.Columns["ResponsibleID"].Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (this.EditMode)
            {
                gridgridIrregularPrice.IsEditingReadOnly = true;

                StringBuilder sql = new StringBuilder();
                DataTable ModifyTable = (DataTable)listControlBindingSource1.DataSource;
                //ModifyTable 去掉 OriginDT_FromDB，剩下的不是新增就是修改
                var Insert_Or_Update = ModifyTable.AsEnumerable().Except(OriginDT_FromDB.AsEnumerable(), DataRowComparer.Default).Where(o => o.Field<string>("SubconReasonID").Trim() != "");

                //抓出ReasonID為空的出來刪除
                var Delete = ModifyTable.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() == "");

                #region SQL組合
                foreach (var item in Delete)
                {
                    string POID = item.Field<string>("POID");
                    string Category = item.Field<string>("Category");
                    sql.Append($"DELETE FROM [LocalPO_IrregularPrice] WHERE POID='{POID}' AND Category='{Category}'" + Environment.NewLine);
                    sql.Append(" " + Environment.NewLine);
                }

                foreach (var item in Insert_Or_Update)
                {
                    string POID = item.Field<string>("POID");
                    string Category = item.Field<string>("Category");
                    string SubconReasonID = item.Field<string>("SubconReasonID");
                    decimal POPrice = item.Field<decimal>("POPrice");
                    decimal StandardPrice = item.Field<decimal>("StdPrice");
                    DataTable dt;
                    DualResult result = DBProxy.Current.Select(null, $"SELECT * FROM LocalPO_IrregularPrice WHERE POID='{POID}' AND Category='{Category}'", out dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != SubconReasonID && !string.IsNullOrEmpty(SubconReasonID))
                            {
                                sql.Append($"UPDATE [LocalPO_IrregularPrice] SET [SubconReasonID]='{SubconReasonID}',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'" + Environment.NewLine);
                                sql.Append($"                                  WHERE [POID]='{POID}' AND [Category]='{Category}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sql.Append("INSERT INTO [LocalPO_IrregularPrice]([POID],[Category],[POPrice],[StandardPrice],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sql.Append($"                              VALUES ('{POID}','{Category}',{POPrice},{StandardPrice},'{SubconReasonID}',GETDATE(),'{Sci.Env.User.UserID}')" + Environment.NewLine);
                        }
                    }
                    sql.Append(" " + Environment.NewLine);
                }

                #endregion
                if (!MyUtility.Check.Empty(sql.ToString()))
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        DualResult upResult;
                        try
                        {

                            upResult = DBProxy.Current.Execute(null, sql.ToString());
                            if (!upResult)
                            {
                                ShowErr(sql.ToString(), upResult);
                                return;
                            }


                            scope.Complete();
                            scope.Dispose();
                            Check_Irregular_Price();
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            ShowErr("Commit transaction error.", ex);
                        }
                    }
                }
            }
            else
            {
                gridgridIrregularPrice.IsEditingReadOnly = false;
                IrregularPriceReasonDT_Initial();
            }

            this.EditMode = !this.EditMode;
            btnEdit.Text = this.EditMode ? "Save" : "Edit";
            btnClose.Text = this.EditMode ? "Undo" : "Close";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnClose.Text == "Close")
                Close();
            else
            {
                this.EditMode = !this.EditMode;
                btnEdit.Text = "Edit";
                btnClose.Text = "Close";
                gridgridIrregularPrice.IsEditingReadOnly = false;
                IrregularPriceReasonDT_Initial();
            }
        }

        private void IrregularPriceReasonDT_Initial(bool isSaveAct = false)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;


            sql.Append(" " + Environment.NewLine);
            sql.Append(" SELECT DISTINCT [Factory]=a.FactoryId" + Environment.NewLine);
            sql.Append(" ,[POID]=al.POID" + Environment.NewLine);
            sql.Append(" ,[Category]=al.Category" + Environment.NewLine);
            sql.Append(" ,o.StyleID" + Environment.NewLine);
            sql.Append(" ,o.BrandID" + Environment.NewLine);
            sql.Append(" ,[PoPrice]=al.POPrice" + Environment.NewLine);
            sql.Append(" ,[StdPrice]=al.StandardPrice" + Environment.NewLine);
            sql.Append(" ,al.SubconReasonID" + Environment.NewLine);
            sql.Append(" ,[ResponsibleID]=sr.Responsible" + Environment.NewLine);
            sql.Append(" ,[ResponsibleName]=(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = sr.Responsible)" + Environment.NewLine);
            sql.Append(" ,sr.Reason" + Environment.NewLine);
            sql.Append(" ,al.AddDate" + Environment.NewLine);
            sql.Append(" ,al.AddName" + Environment.NewLine);
            sql.Append(" ,al.EditDate" + Environment.NewLine);
            sql.Append(" ,al.EditName" + Environment.NewLine);
            sql.Append(" FROM LocalPO a" + Environment.NewLine);
            sql.Append(" INNER JOIN LocalPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
            sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
            sql.Append(" INNER JOIN LocalPO_IrregularPrice al ON al.POID = o.POID AND al.Category = a.Category" + Environment.NewLine);
            sql.Append(" LEFT JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
            sql.Append(" WHERE a.ID = @LocalPO_ID" + Environment.NewLine);

            parameters.Add(new SqlParameter("@LocalPO_ID", _LocalPO_ID));

            result = DBProxy.Current.Select(null, sql.ToString(), parameters, out OriginDT_FromDB);
            if (!result)
            {
                ShowErr(sql.ToString(), result);
                return;
            }

            if (isSaveAct)
            {
                ModifyDT_FromP30 = OriginDT_FromDB.Copy();
                P30._IrregularPriceReasonDT = OriginDT_FromDB.Copy();
            }

            //原始資料：OriginDT_FromDB
            //即將異動，寫回DB的資料：ModifyDT_FromP30
            listControlBindingSource1.DataSource = ModifyDT_FromP30.Copy();



        }

        private void Check_Irregular_Price()
        {
            //重置
            _IrregularPriceReasonDT = null;
            
            //採購價 > 標準價 =異常
            #region 變數宣告

            string poid = string.Empty;
            string category = string.Empty;
            string artworkType = string.Empty;
            string BrandID = string.Empty;
            string StyleID = string.Empty;
            string LocalPO_ID = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;

            DataTable IrregularPriceReason_InDB = new DataTable();//A DB現有的 "價格異常紀錄" IPR資料
            DataTable Price_Dt;// 紀錄所有價格的Table
            DataTable IrregularPriceReason_Real = new DataTable();//C " 所有價格異常紀錄" IPR資料，無論DB有無
            DataTable IrregularPriceReason_New;//B 最新的價格異常紀錄，DB沒有
            //DataTable IrregularPriceReason_Repeat;//D C和A重複的資料

            #endregion

            #region 欄位定義
            IrregularPriceReason_Real.Columns.Add(new DataColumn("Category", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("POID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("StyleID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("BrandID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("PoPrice", System.Type.GetType("System.Decimal")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("StdPrice", System.Type.GetType("System.Decimal")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("SubconReasonID", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("AddDate", System.Type.GetType("System.DateTime")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("AddName", System.Type.GetType("System.String")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("EditDate", System.Type.GetType("System.DateTime")));
            IrregularPriceReason_Real.Columns.Add(new DataColumn("EditName", System.Type.GetType("System.String")));
            #endregion

            try
            {
                LocalPO_ID = _LocalPO_ID;
                parameters.Add(new SqlParameter("@LocalPO_ID", LocalPO_ID));

                #region 查詢所有價格異常紀錄

                sql.Append(@"

--根據表頭LocalPO的ID，整理出Category、POID、OrderID
SELECT DISTINCT [ArtworkTypeID]=a.Category  ,[POID]=ad.POID  ,[OrderId]=ad.OrderID 
INTO #tmp_AllOrders 
FROM LocalPO a 
INNER JOIN LocalPO_Detail ad ON a.ID=ad.ID
WHERE a.ID = @LocalPO_ID

--從所有採購單中，找出同Category、POID，有被採購的OrderID（不限採購單）
SELECT DISTINCT ad.OrderID 
INTO #BePurchased
FROM LocalPO a 
INNER JOIN LocalPO_Detail ad ON a.ID=ad.ID 
INNER JOIn Orders ods ON ad.OrderID=ods.id 
WHERE  ods.POID IN  (SELECT POID FROM  #tmp_AllOrders)

--列出採購價的清單（尚未總和）
SELECT  ap.ID
		,ap.Category
		,Orders.POID
		,[OID]=apd.OrderId
		--,ap.currencyid  --維護時檢查用，所以先註解留著
		--,apd.Price
		--,apd.Qty
		,apd.Qty * apd.Price * dbo.getRate('FX',ap.CurrencyID,'USD',ap.issuedate) PO_amt
		--,dbo.getRate('FX',ap.CurrencyID,'USD',ap.issuedate) rate
INTO #total_PO
from LocalPO ap WITH (NOLOCK) 
INNER JOIN LocalPO_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
INNER JOIN  Orders WITH (NOLOCK) on orders.id = apd.orderid
WHERE  EXiSTS  ( 
				SELECT ArtworkTypeID,POID 
				FROM #tmp_AllOrders 
				WHERE ArtworkTypeID= ap.Category  AND POID=Orders.POID) --相同Category、POID
	   AND apd.OrderId  IN  ( SELECT OrderID FROM #BePurchased ) --且有被採購的OrderID


--開始整合
SELECT o.BrandID ,o.StyleID  ,t.ArtworkTypeID  ,t.POID 
,[stdPrice]=round(Standard.order_amt/iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[Po_price]=round(Po.PO_amt / iif(Standard.order_qty=0,1,Standard.order_qty),3) 
FROM #tmp_AllOrders t
INNER JOIN Orders o WITH (NOLOCK) on o.id = t.OrderId
INNER JOIN Brand bra on bra.id=o.BrandID
OUTER APPLY(--標準價
	        select orders.POID
	        ,sum(orders.qty) order_qty        --實際外發數量
	        ,sum(orders.qty*Price) order_amt  --外發成本
	        from orders WITH (NOLOCK) 
	        inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	        where POID= t.POID                   --相同母單
			AND ArtworkTypeID= t.ArtworkTypeID   --相同加工
			AND Order_TmsCost.ID  IN ( SELECT OrderID FROM #BePurchased ) --***限定 有被採購的訂單***
	        group by orders.poid,ArtworkTypeID
) Standard
OUTER APPLY (--採購價，根據Category、POID，作分組加總
	SELECT isnull(sum(Q.PO_amt),0.00) PO_amt
	FROM (
		SELECT PO_amt FROM #total_PO
		WHERE Category = t.artworktypeid 
		AND POID = t.POID 
	) Q
) Po	

GROUP BY  o.BrandID ,o.StyleID ,t.ArtworkTypeID ,t.POID ,Standard.order_amt ,Standard.order_qty ,Po.PO_amt ,Standard.order_qty

DROP TABLE #tmp_AllOrders ,#BePurchased ,#total_PO
" + Environment.NewLine);

                #endregion

                this.ShowWaitMessage("Data Loading...");
                result = DBProxy.Current.Select(null, sql.ToString(), parameters, out Price_Dt);
                sql.Clear();
                this.HideWaitMessage();

                if (!result)
                {
                    ShowErr(sql.ToString(), result);
                    return;
                }

                if (Price_Dt.Rows.Count > 0)
                {

                    #region 建立新的 價格異常紀錄 Dtatable物件
                    foreach (DataRow row in Price_Dt.Rows)
                    {

                        decimal StdPrice = 0;
                        decimal purchasePrice = 0;
                        //用來準備填入 C 最新的 " 價格異常紀錄" IPR資料
                        StdPrice = Convert.ToDecimal(MyUtility.Check.Empty(row["StdPrice"]) ? 0 : row["StdPrice"]);
                        purchasePrice = Convert.ToDecimal(MyUtility.Check.Empty(row["Po_price"]) ? 0 : row["Po_price"]);
                        poid = Convert.ToString(MyUtility.Check.Empty(row["Poid"]) ? "" : row["Poid"]);
                        category = Convert.ToString(MyUtility.Check.Empty(row["ArtworkTypeID"]) ? "" : row["ArtworkTypeID"]);
                        artworkType = Convert.ToString(MyUtility.Check.Empty(row["ArtworkTypeID"]) ? "" : row["ArtworkTypeID"]);
                        BrandID = Convert.ToString(MyUtility.Check.Empty(row["BrandID"]) ? "" : row["BrandID"]);
                        StyleID = Convert.ToString(MyUtility.Check.Empty(row["StyleID"]) ? "" : row["StyleID"]);

                        //只要有異常就顯示紅色
                        if (purchasePrice > StdPrice & StdPrice > 0)
                        {
                            IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, purchasePrice, StdPrice, IrregularPriceReason_Real);
                        }

                    }
                    #endregion

                    #region 準備 A: DB現有的 "價格異常紀錄" IPR資料

                    sql.Clear();
                    sql.Append(" SELECT DISTINCT al.POID ,al.Category ,o.BrandID ,o.StyleID ,al.POPrice, al.StandardPrice ,al.SubconReasonID ,al.AddDate ,al.AddName ,al.EditDate ,al.EditName " + Environment.NewLine);

                    sql.Append(" FROM LocalPO a" + Environment.NewLine);
                    sql.Append(" INNER JOIN LocalPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                    sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
                    sql.Append(" INNER JOIN LocalPO_IrregularPrice al ON al.POID = o.POID AND al.Category = a.Category" + Environment.NewLine);
                    sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
                    sql.Append(" WHERE a.ID = @LocalPO_ID AND sr.Junk=0" + Environment.NewLine);

                    result = DBProxy.Current.Select(null, sql.ToString(), parameters, out IrregularPriceReason_InDB);

                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Get LocalPO Irregular Price error!!");
                    }

                    IrregularPriceReason_InDB = MyUtility.Check.Empty(IrregularPriceReason_InDB) ? new DataTable() : IrregularPriceReason_InDB;
                    #endregion

                    #region 準備 B: 所有價格異常紀錄 - DB現有紀錄 = 新增的紀錄

                    var Btable = from c in IrregularPriceReason_Real.AsEnumerable()
                                 where !IrregularPriceReason_InDB.AsEnumerable().Any(o => o["POID"].ToString() == c["POID"].ToString() && o["Category"].ToString() == c["Category"].ToString())
                                 select c;

                    IrregularPriceReason_New = Btable.AsEnumerable().Count() > 0 ? Btable.AsEnumerable().CopyToDataTable() : new DataTable();


                    #endregion

                    #region 準備 D：找出實際紀錄 與 DB紀錄 重複的部分

                    //var Rep = from c in IrregularPriceReason_InDB.AsEnumerable()
                    //          where !IrregularPriceReason_Real.AsEnumerable().Any(o => o["POID"].ToString() == c["POID"].ToString() && o["ArtworkTypeID"].ToString() == c["ArtworkTypeID"].ToString())
                    //          select c;

                    //IrregularPriceReason_Repeat = Rep.AsEnumerable().Count() > 0 ? Rep.AsEnumerable().CopyToDataTable() : new DataTable();
                    #endregion

                    DataTable SubconReason;
                    DBProxy.Current.Select(null, "SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason  FROM SubconReason WHERE Type='IP' AND Junk=0", out SubconReason);

                    #region 資料串接

                    var summary = (from a in IrregularPriceReason_InDB.AsEnumerable()
                                   select new
                                   {
                                       POID = a.Field<string>("POID"),
                                       Category = a.Field<string>("Category"),
                                       BrandID = a.Field<string>("BrandID"),
                                       StyleID = a.Field<string>("StyleID"),
                                       PoPrice = a.Field<decimal>("POPrice"),
                                       StdPrice = a.Field<decimal>("StandardPrice"),
                                       SubconReasonID = a.Field<string>("SubconReasonID"),
                                       AddDate = a.Field<DateTime?>("AddDate"),
                                       AddName = a.Field<string>("AddName"),
                                       EditDate = a.Field<DateTime?>("EditDate"),
                                       EditName = a.Field<string>("EditName")

                                   }).Union
                             (from b in IrregularPriceReason_New.AsEnumerable()
                              join c in IrregularPriceReason_Real.AsEnumerable() on new { POID = b.Field<string>("POID"), ArtWorkType = b.Field<string>("Category") } equals new { POID = c.Field<string>("POID"), ArtWorkType = c.Field<string>("Category") }
                              select new
                              {
                                  POID = c.Field<string>("POID"),
                                  Category = c.Field<string>("Category"),
                                  BrandID = c.Field<string>("BrandID"),
                                  StyleID = c.Field<string>("StyleID"),
                                  PoPrice = c.Field<decimal>("PoPrice"),
                                  StdPrice = c.Field<decimal>("StdPrice"),
                                  SubconReasonID = c.Field<string>("SubconReasonID"),
                                  AddDate = c.Field<DateTime?>("AddDate"),
                                  AddName = c.Field<string>("AddName"),
                                  EditDate = c.Field<DateTime?>("EditDate"),
                                  EditName = c.Field<string>("EditName")
                              });


                    var total_IPR = from a in summary
                                    join s in SubconReason.AsEnumerable() on a.SubconReasonID equals s.Field<string>("ID") into sr
                                    from s in sr.DefaultIfEmpty()
                                    select new
                                    {
                                        Factory = _FactoryID,
                                        a.POID,
                                        a.Category,
                                        a.StyleID,
                                        a.BrandID,
                                        a.PoPrice,
                                        a.StdPrice,
                                        a.SubconReasonID,
                                        ResponsibleID = MyUtility.Check.Empty(s) ? "" : s.Field<string>("ResponsibleID"),
                                        ResponsibleName = MyUtility.Check.Empty(s) ? "" : s.Field<string>("ResponsibleName"),
                                        Reason = MyUtility.Check.Empty(s) ? "" : s.Field<string>("Reason"),
                                        a.AddDate,
                                        a.AddName,
                                        a.EditDate,
                                        a.EditName
                                    };

                    #endregion

                    #region 還原成 DataTable

                    DataTable IPR_Grid = new DataTable();
                    PropertyInfo[] props = null;

                    foreach (var item in total_IPR)
                    {
                        if (props == null)
                        {
                            Type t = item.GetType();
                            props = t.GetProperties();
                            foreach (PropertyInfo pi in props)
                            {
                                Type colType = pi.PropertyType;
                                //針對Nullable<>特別處理
                                if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    colType = colType.GetGenericArguments()[0];
                                }
                                //建立欄位
                                IPR_Grid.Columns.Add(pi.Name, colType);
                            }
                        }
                        DataRow row = IPR_Grid.NewRow();
                        foreach (PropertyInfo pi in props)
                        {
                            row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;
                        }
                        IPR_Grid.Rows.Add(row);
                    }

                    #endregion

                    /*
                          | DB | New |
                    ----------------------------
                         1| 有 | 無  | 帶DB的 Datatable
                    ----------------------------
                         2| 無 | 有  | 帶DB + New 的 Datatable
                    ----------------------------
                         3| 有 | 有  | 帶DB + New 的 Datatable
                    ----------------------------
                         4| 有 | 無  | 帶DB + New 的 Datatable
                    ----------------------------
                         5| 無 | 無  | 沒事

                     */
                    //「曾經」有過價格異常紀錄，現在價格正常，沒有新的異常紀錄，只需要帶DB資料
                    if (IPR_Grid.Rows.Count != 0)
                    {
                        listControlBindingSource1.DataSource = IPR_Grid;
                        ModifyDT_FromP30 = IPR_Grid.Copy();
                    }
                    else
                    {
                        IrregularPriceReasonDT_Initial(true);
                    }
                }

            }
            catch (Exception ex)
            {
                ShowErr(ex.Message, ex);
            }
        }



        private DataTable CreateIrregularPriceReasonDataTabel(string POID, string Category, string BrandID, string StyleID, decimal purchasePrice, decimal StdPrice, DataTable IrregularPriceReason_New)
        {
            DataRow ndr = IrregularPriceReason_New.NewRow();

            ndr["poid"] = POID;
            ndr["Category"] = Category;
            ndr["BrandID"] = BrandID;
            ndr["StyleID"] = StyleID;
            ndr["StdPrice"] = StdPrice;
            ndr["PoPrice"] = purchasePrice;
            ndr["SubconReasonID"] = "";
            ndr["AddDate"] = DBNull.Value;
            ndr["AddName"] = "";
            ndr["EditDate"] = DBNull.Value;
            ndr["EditName"] = "";

            IrregularPriceReason_New.Rows.Add(ndr);

            return IrregularPriceReason_New;
        }

    }
}
