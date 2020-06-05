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
    public partial class P01_IrregularPriceReason : Sci.Win.Subs.Base
    {
        DataTable OriginDT_FromDB;
        DataTable ModifyDT_FromP01;
        DataTable _detailDatas;
        DataRow _masterData;

        public int ReasonNullCount = 0;
        string _ArtWorkPO_ID = string.Empty;
        string _FactoryID = string.Empty;

        /// <summary>
        /// 用於存放使用者輸入"後"的異常價格，僅限新增模式使用
        /// </summary>
        public static List<tmp_IrregularPriceReason> tmp_IrregularPriceReason_List=new List<tmp_IrregularPriceReason>();


        /// <summary>
        /// 用於存放使用者所輸入"前"的異常價格，用於Undo還原Grid上的欄位，僅限新增模式使用
        /// </summary>
        public static List<tmp_IrregularPriceReason> tmp_IrregularPriceReason_List_Ori = new List<tmp_IrregularPriceReason>();


        public P01_IrregularPriceReason(string ArtWorkPO_ID, string FactoryID, DataRow masterData, DataTable detailDatas)
        {
            InitializeComponent();

            this.EditMode = false;
            _ArtWorkPO_ID = ArtWorkPO_ID;
            _FactoryID = FactoryID;
            _detailDatas = detailDatas;
            _masterData = masterData;
        }

        protected override void OnFormLoaded()
        {
            //重新計算價格
            if (MyUtility.Check.Empty(_masterData["ID"]))
            {
                this.Check_Irregular_Price_Without_PO();
            }
            else
            {
                this.Check_Irregular_Price();
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
                        DBProxy.Current.Select(null, $"SELECT ID,[ResponsibleID]=Responsible,(select Name from DropDownList d where d.type = 'Pms_PoIr_Responsible' and d.ID = SubconReason.Responsible) as ResponsibleName,Reason FROM SubconReason WHERE ID='{item.GetSelectedString()}' AND Type='IP'", out dt);
                        dr["ResponsibleID"] = dt.Rows[0]["ResponsibleID"];
                        dr["ResponsibleName"] = dt.Rows[0]["ResponsibleName"];
                        dr["Reason"] = dt.Rows[0]["Reason"];

                        // 如果清空，則將集合內的資料刪掉，否則add進資料列
                        if (MyUtility.Check.Empty(dr["SubconReasonID"]))
                        {
                            if (tmp_IrregularPriceReason_List.Count > 0 && tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).Any())
                            {
                                tmp_IrregularPriceReason_List.RemoveAt(tmp_IrregularPriceReason_List.IndexOf(tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).FirstOrDefault()));

                            }
                        }
                        else
                        {
                            if (MyUtility.Check.Empty(_masterData["ID"]))
                            {
                                if (tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).Any())
                                {
                                    tmp_IrregularPriceReason_List.RemoveAt(tmp_IrregularPriceReason_List.IndexOf(tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).FirstOrDefault()));
                                }

                                tmp_IrregularPriceReason tmp = new tmp_IrregularPriceReason()
                                {
                                    POID = dr["POID"].ToString(),
                                    ArtWorkType_ID = _masterData["ArtWorkTypeID"].ToString(),
                                    SubconReasonID = dr["SubconReasonID"].ToString(),
                                    ResponsibleID = dt.Rows[0]["ResponsibleID"].ToString(),
                                    ResponsibleName = dt.Rows[0]["ResponsibleName"].ToString(),
                                    Reason = dt.Rows[0]["Reason"].ToString()
                                };
                                tmp_IrregularPriceReason_List.Add(tmp);
                            }
                        }
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
                    // 如果清空，則將集合內的資料刪掉
                    if (tmp_IrregularPriceReason_List.Count > 0 && tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).Any())
                    {
                        tmp_IrregularPriceReason_List.RemoveAt(tmp_IrregularPriceReason_List.IndexOf(tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).FirstOrDefault()));
                    }

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

                    // add進資料列
                    if (MyUtility.Check.Empty(_masterData["ID"]))
                    {
                        if (tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).Any())
                        {
                            tmp_IrregularPriceReason_List.RemoveAt(tmp_IrregularPriceReason_List.IndexOf(tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString()).FirstOrDefault()));
                        }
                        tmp_IrregularPriceReason tmp = new tmp_IrregularPriceReason()
                        {
                            POID = dr["POID"].ToString(),
                            ArtWorkType_ID = _masterData["ArtWorkTypeID"].ToString(),
                            SubconReasonID = dt.Rows[0]["ID"].ToString(),
                            ResponsibleID = dt.Rows[0]["ResponsibleID"].ToString(),
                            ResponsibleName = dt.Rows[0]["ResponsibleName"].ToString(),
                            Reason = dt.Rows[0]["Reason"].ToString()
                        };
                        tmp_IrregularPriceReason_List.Add(tmp);
                    }
                }

            };

            #region Grid欄位設定

            Helper.Controls.Grid.Generator(this.gridgridIrregularPrice)
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Type", header: "Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("POID", header: "POID", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("PoPrice", header: "PO" + Environment.NewLine + "Price", decimal_places: 4, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Numeric("StdPrice", header: "Standard" + Environment.NewLine + "Price", decimal_places: 4, iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("SubconReasonID", header: "Reason" + Environment.NewLine + "ID", width: Widths.AnsiChars(7), settings: col_SubconReasonID)
                .Text("ResponsibleName", header: "Responsible", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Reason", header: "Reason", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .DateTime("AddDate", header: "Create" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("AddName", header: "Create" + Environment.NewLine + "Name", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .DateTime("EditDate", header: "Edit" + Environment.NewLine + "Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("EditName", header: "Edit" + Environment.NewLine + "Name", iseditingreadonly: true, width: Widths.AnsiChars(10));
            #endregion

            this.gridgridIrregularPrice.Columns["SubconReasonID"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.gridgridIrregularPrice.Columns.Count; i++)
            {
                this.gridgridIrregularPrice.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (this.EditMode)
            {
                DataTable ModifyTable = (DataTable)listControlBindingSource1.DataSource;
                
                gridgridIrregularPrice.IsEditingReadOnly = true;

                StringBuilder sql = new StringBuilder();
                //ModifyTable 去掉 OriginDT_FromDB，剩下的不是新增就是修改
                var Insert_Or_Update = ModifyTable.AsEnumerable().Except(OriginDT_FromDB.AsEnumerable(), DataRowComparer.Default).Where(o => o.Field<string>("SubconReasonID").Trim() != "");

                //抓出ReasonID為空的出來刪除
                var Delete = ModifyTable.AsEnumerable().Where(o => o.Field<string>("SubconReasonID").Trim() == "");

                #region SQL組合
                foreach (var item in Delete)
                {
                    string POID = item.Field<string>("POID");
                    string ArtworkType = item.Field<string>("Type");
                    sql.Append($"DELETE FROM [ArtworkPO_IrregularPrice] WHERE POID='{POID}' AND ArtworkTypeID='{ArtworkType}'" + Environment.NewLine);
                    sql.Append(" " + Environment.NewLine);
                }

                foreach (var item in Insert_Or_Update)
                {
                    string POID = item.Field<string>("POID");
                    string ArtworkType = item.Field<string>("Type");
                    string SubconReasonID = item.Field<string>("SubconReasonID");
                    decimal POPrice = item.Field<decimal>("POPrice");
                    decimal StandardPrice = item.Field<decimal>("StdPrice");

                    DataTable dt;

                    DualResult result = DBProxy.Current.Select(null, $"SELECT * FROM ArtworkPO_IrregularPrice WHERE POID='{POID}' AND ArtworkTypeID='{ArtworkType}'", out dt);
                    if (result)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["SubconReasonID"].ToString() != SubconReasonID && !string.IsNullOrEmpty(SubconReasonID))
                            {
                                sql.Append($"UPDATE [ArtworkPO_IrregularPrice] SET [SubconReasonID]='{SubconReasonID}',EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'" + Environment.NewLine);
                                sql.Append($"                                  WHERE [POID]='{POID}' AND [ArtworkTypeID]='{ArtworkType}'" + Environment.NewLine);
                            }
                        }
                        else
                        {
                            sql.Append("INSERT INTO [ArtworkPO_IrregularPrice]([POID],[ArtworkTypeID],[POPrice],[StandardPrice],[SubconReasonID],[AddDate],[AddName])" + Environment.NewLine);
                            sql.Append($"                              VALUES ('{POID}','{ArtworkType}',{POPrice},{StandardPrice},'{SubconReasonID}',GETDATE(),'{Sci.Env.User.UserID}')" + Environment.NewLine);
                        }
                    }
                    sql.Append(" " + Environment.NewLine);
                }
                #endregion

                //若沒有表頭(意即新增模式)，則不執行異動DB
                if (MyUtility.Check.Empty(_masterData["ID"]))
                {
                    // 新增時需給AddDate,AddName
                    foreach (DataRow dr in ModifyTable.Rows)
                    {
                        if (!MyUtility.Check.Empty(dr["SubconReasonID"]))
                        {
                            dr["Adddate"] = DateTime.Now;
                            dr["AddName"] = Env.User.UserID;
                        }
                    }

                    P01.tmp_ModifyTable = ModifyTable;
                    P01.tmp_OriginDT_FromDB = OriginDT_FromDB;
                }
                else
                {
                    P01.tmp_ModifyTable = null;
                    P01.tmp_OriginDT_FromDB = null;
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
                                //存檔完成後，再重新計算一次
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
            }
            else
            {
                gridgridIrregularPrice.IsEditingReadOnly = false;

                // 新增模式使用不同處理Function
                if (MyUtility.Check.Empty(_masterData["ID"]))
                {
                    //重新計算價格
                    Check_Irregular_Price_Without_PO();
                }
                else
                {
                    //重新計算價格
                    Check_Irregular_Price();
                }
                //初始化OriginDT_FromDB，用來比對User修改後的DataTable
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

                //點選Undo，則把之前備份下來的資料覆蓋回去，後面的IrregularPriceReasonDT_Initial() 會把資料塞回去Grid
                tmp_IrregularPriceReason_List = tmp_IrregularPriceReason_List_Ori.ToList();

                //回到檢視模式，並且重新取得資料
                IrregularPriceReasonDT_Initial();

                // 新增模式使用不同處理Function
                if (MyUtility.Check.Empty(_masterData["ID"]))
                {
                    //重新計算價格
                    Check_Irregular_Price_Without_PO();
                }
                else
                {
                    //重新計算價格
                    Check_Irregular_Price();
                }
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
            sql.Append(" ,[Type]=a.ArtworkTypeID" + Environment.NewLine);
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
            sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
            sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
            sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
            sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
            sql.Append(" LEFT JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
            sql.Append(" WHERE a.ID = @artWorkPO_ID" + Environment.NewLine);

            parameters.Add(new SqlParameter("@artWorkPO_ID", _ArtWorkPO_ID));

            result = DBProxy.Current.Select(null, sql.ToString(), parameters, out OriginDT_FromDB);
            if (!result)
            {
                ShowErr(sql.ToString(), result);
                return;
            }

            //執行存檔動作，DB資料會異動，因此帶資料庫的
            if (isSaveAct)
            {
                ModifyDT_FromP01 = OriginDT_FromDB.Copy();
            }

            // 如果存有使用者輸入的異常價格原因，則代入Grid
            if (tmp_IrregularPriceReason_List.Count > 0 && MyUtility.Check.Empty(_masterData["ID"]))
            {
                foreach (DataRow dr in ModifyDT_FromP01.Rows)
                {
                    if (tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString() && o.ArtWorkType_ID == dr["Type"].ToString()).Any())
                    {
                        tmp_IrregularPriceReason data = tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString() && o.ArtWorkType_ID == dr["Type"].ToString()).FirstOrDefault();
                        dr["SubconReasonID"] = data.SubconReasonID;
                        dr["ResponsibleID"] = data.ResponsibleID;
                        dr["ResponsibleName"] = data.ResponsibleName;
                        dr["Reason"] = data.Reason;
                    }
                }

            }

            //原始資料：OriginDT_FromDB
            //即將異動，寫回DB的資料：ModifyDT_FromP01
            listControlBindingSource1.DataSource = ModifyDT_FromP01.Copy();

            // 存入靜態物件，以便於在P01 Save的時候執行異常價格SQL
            P01.tmp_ModifyTable = ModifyDT_FromP01.Copy();
        }

        /// 重新計算價格並產生DataSource
        public bool Check_Irregular_Price(bool IsNeedUpdateDT = true)
        {
            //是否有價格異常，用以區別P01的按鈕要不要變色
            bool Has_Irregular_Price = false;

            //採購價 > 標準價 =異常
            #region 變數宣告

            string poid = string.Empty;
            string artworkType = string.Empty;
            string BrandID = string.Empty;
            string StyleID = string.Empty;
            string artWorkPO_ID = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;

            DataTable IrregularPriceReason_InDB;//A DB現有的 "價格異常紀錄" IPR資料
            DataTable Price_Dt;// 紀錄所有價格的Table
            DataTable IrregularPriceReason_Real = new DataTable();//C "當下實際存在的價格異常紀錄" IPR資料，無論DB有無
            DataTable IrregularPriceReason_New;//B 最新的價格異常紀錄，DB沒有
            //DataTable IrregularPriceReason_Repeat;//D C和A重複的資料
            #endregion

            #region 欄位定義
            IrregularPriceReason_Real.Columns.Add(new DataColumn("ArtworkTypeID", System.Type.GetType("System.String")));
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
                artWorkPO_ID = _ArtWorkPO_ID;
                parameters.Add(new SqlParameter("@artWorkPO_ID", artWorkPO_ID));

                #region 查詢所有價格異常紀錄

                sql.Append(@"
SELECT t.POID,[Amount]=ISNULL(Sum(t.Amount*dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate)),0)
INTO #AmountList
FROM #TmpSource t
LEFT join ArtworkPO ap with(nolock) on ap.id = t.id
GROUP BY POID

--根據表頭LocalPO的ID，整理出ArtworkTypeID、POID、OrderID
/*SELECT DISTINCT[ArtworkTypeID] = ad.ArtworkTypeID ,[OrderId] = ad.OrderID  ,[POID]=o.POID
INTO #tmp_AllOrders 
FROM ArtworkPO a 
INNER JOIN ArtworkPO_Detail ad ON a.ID=ad.ID
INNER JOIn ORDERS o ON ad.OrderID=o.id
WHERE a.ID =  @artWorkPO_ID*/

SELECT DISTINCT
    [ArtworkTypeID] = a.ArtworkTypeID 
    ,[OrderId] = a.OrderID  
    ,[POID]=o.POID
INTO #tmp_AllOrders 
FROM #TmpSource a 
INNER JOIn ORDERS o ON a.OrderID=o.id

/*
從所有採購單中，找出同ArtworkTypeID、POID，有被採購的OrderID（不限採購單）
註解原因 各項目可能會再其他子單進行採購)
SELECT DISTINCT ad.OrderID 
INTO #BePurchased
FROM ArtworkPO a 
INNER JOIN ArtworkPO_Detail ad ON a.ID=ad.ID 
INNER JOIn Orders ods ON ad.OrderID=ods.id 
WHERE  ods.POID IN  (SELECT POID FROM  #tmp_AllOrders)
*/

--列出採購價的清單（尚未總和）
SELECT  ap.ID
		,ap.ArtworkTypeID
		,Orders.POID
		,[OID]=apd.OrderId
		,ap.currencyid  --維護時檢查用，所以先註解留著
		--,apd.PoQty
		--,apd.PoQty * apd.Price * dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate) PO_amt

		-- 已關單代表不會再使用這一張採購單進行「外發」，但是已經外發（arm Out）的數量後續還是會建立請款，因此已關單的要計算的是已實際外發的數量
		,[Qty]=IIF(ap.Status!='Closed',apd.PoQty ,apd.Farmout)
		,[PO_amt]= IIF(ap.Status!='Closed'
						,apd.PoQty * apd.Price * dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate) 
						,apd.Farmout * apd.Price * dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate) )

		,dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate) rate
INTO #total_PO
FROM ArtworkPO ap WITH (NOLOCK) 
INNER JOIN ArtworkPO_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
INNER JOIN  Orders WITH (NOLOCK) on orders.id = apd.orderid
WHERE  EXiSTS  ( 
				SELECT ArtworkTypeID,POID 
				FROM #tmp_AllOrders 
				WHERE ArtworkTypeID= ap.ArtworkTypeID  AND POID=Orders.POID) --相同Category、POID
	   --AND apd.OrderId  IN  ( SELECT OrderID FROM #BePurchased ) 且有被採購的OrderID (註解原因 各項目可能會再其他子單進行採購)
       --AND ap.Status = 'Approved' 現在不需要過濾狀態
       AND ap.ID <> @artWorkPO_ID  --SQL撈資料要排除當下的LocalPO.ID

--繡花成本處理：列出同POID、Category=EMB_Thread（繡線）的總額清單
SELECT   LPD.POID
		,LP.currencyid
		, [Price]=LPd.Price  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) --採購單價

		--, LPD.Qty localap_qty  --採購數量
		--, [LocalPo_amt]=LPD.Price* LPD.Qty  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) --採購總額
		-- 已關單代表不會再使用這一張採購單進行「收料」，但是已經收料（In Qty ）的數量後續還是會建立請款，因此已關單的要計算的是已實際收料的數量
		, [Qty]=IIF(LP.Status!='Closed',LPD.Qty ,LPD.InQty)  --數量
		, [LocalPo_amt]=IIF(LP.Status!='Closed'
							, LPD.Price * LPD.Qty  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) --採購總額
							,LPD.Price * LPD.InQty  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate))

		, dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) rate
INTO #Embroidery_List
FROM LocalPO LP
inner join LocalPO_Detail LPD on LP.Id = LPD.Id
INNER JOIN Orders ON Orders.ID=LPD.OrderId
WHERE LP.Category = 'EMB_Thread'  
	  AND Orders.POID IN  (SELECT POID FROM  #tmp_AllOrders)

--開始整合
SELECT o.BrandID ,o.StyleID  ,t.ArtworkTypeID  ,t.POID 
,[stdPrice]=round(Standard.order_amt/iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[PoPrice]=round( (po.Po_Amt + (SELECT Amount FROM #AmountList WHERE POID=t.POID)) / iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[PoPriceWithEmbroidery] =IIF(Embroidery.LocalPo_amt IS NULL ,
							round( (po.Po_Amt + (SELECT Amount FROM #AmountList WHERE POID=t.POID)) / iif(Standard.order_qty=0,1,Standard.order_qty),3) , 
							round(( (po.Po_Amt + (SELECT Amount FROM #AmountList WHERE POID=t.POID)) +Embroidery.LocalPo_amt) / iif(Standard.order_qty=0,1,Standard.order_qty)
							,3) )
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
			--AND Order_TmsCost.ID  IN ( SELECT OrderID FROM #BePurchased ) ***限定 有被採購的訂單*** (註解原因 各項目可能會再其他子單進行採購)
	        group by orders.poid,ArtworkTypeID
) Standard
OUTER APPLY (--採購價，根據ArtworkTypeID、POID，作分組加總
	SELECT isnull(sum(Q.PO_amt),0.00) PO_amt
	FROM (
		SELECT PO_amt FROM #total_PO
		WHERE ArtworkTypeID = t.artworktypeid 
		AND POID = t.POID 
	) Q
) Po	
outer apply(--繡花成本，根據POID，作分組加總
	SELECT  x.POID,[LocalPo_amt]=SUM(x.LocalPo_amt) 
	FROM (	
		SELECT * FROM #Embroidery_List
		WHERE POID=t.POID
		) x   
	GROUP BY  x.POID
) Embroidery

GROUP BY  o.BrandID ,o.StyleID ,t.ArtworkTypeID ,t.POID ,Standard.order_amt ,Standard.order_qty ,po.Po_Amt ,Standard.order_qty ,Embroidery.LocalPo_amt

DROP TABLE #tmp_AllOrders --,#BePurchased 
,#total_PO ,#Embroidery_List


" + Environment.NewLine);

                #endregion

                this.ShowWaitMessage("Data Loading...");
                //result = DBProxy.Current.Select(null, sql.ToString(), parameters, out Price_Dt);
                result = MyUtility.Tool.ProcessWithDatatable(_detailDatas, "", sql.ToString(), out Price_Dt, "#TmpSource", null, parameters);
                sql.Clear();
                this.HideWaitMessage();

                if (!result)
                {
                    ShowErr(sql.ToString(), result);
                }

                if (Price_Dt.Rows.Count > 0)
                {
                    #region 準備 C：當下實際存在的價格異常紀錄
                    foreach (DataRow row in Price_Dt.Rows)
                    {
                        decimal StdPrice = 0;
                        decimal purchasePrice = 0;
                        decimal PoPriceWithEmbroidery = 0;
                        //用來準備填入 C 最新的 " 價格異常紀錄" IPR資料
                        poid = Convert.ToString(MyUtility.Check.Empty(row["Poid"]) ? "" : row["Poid"]);
                        BrandID = Convert.ToString(MyUtility.Check.Empty(row["BrandID"]) ? "" : row["BrandID"]);
                        StyleID = Convert.ToString(MyUtility.Check.Empty(row["StyleID"]) ? "" : row["StyleID"]);
                        artworkType = Convert.ToString(MyUtility.Check.Empty(row["ArtworkTypeID"]) ? "" : row["ArtworkTypeID"]);
                        StdPrice = Convert.ToDecimal(MyUtility.Check.Empty(row["StdPrice"]) ? 0 : row["StdPrice"]);
                        purchasePrice = Convert.ToDecimal(MyUtility.Check.Empty(row["PoPrice"]) ? 0 : row["PoPrice"]);
                        PoPriceWithEmbroidery = Convert.ToDecimal(MyUtility.Check.Empty(row["PoPriceWithEmbroidery"]) ? 0 : row["PoPriceWithEmbroidery"]);


                        //如果ArtworkType是繡花（ArtworkTypeID = Embroidery ），要加上繡花物料成本
                        if (artworkType.ToUpper() == "EMBROIDERY")
                        {
                            //只要有異常就顯示紅色
                            if (PoPriceWithEmbroidery > StdPrice & StdPrice > 0)
                            {
                                IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, PoPriceWithEmbroidery, StdPrice, IrregularPriceReason_Real);
                            }
                        }
                        else
                        {
                            //不用加上繡花物料成本
                            //只要有異常就顯示紅色
                            if (purchasePrice > StdPrice & StdPrice > 0)
                            {
                                IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, purchasePrice, StdPrice, IrregularPriceReason_Real);
                            }
                        }
                    }

                    #endregion

                    #region 準備 A：DB現有的 "價格異常紀錄"

                    sql.Clear();
                    //sql.Append(" SELECT DISTINCT al.POID ,al.ArtworkTypeID ,o.BrandID ,o.StyleID ,al.POPrice, al.StandardPrice ,al.SubconReasonID ,al.AddDate ,al.AddName ,al.EditDate ,al.EditName " + Environment.NewLine);

                    //sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
                    //sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                    //sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
                    //sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
                    //sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
                    //sql.Append(" WHERE a.ID = @artWorkPO_ID" + Environment.NewLine);

                    //result = DBProxy.Current.Select(null, sql.ToString(), parameters, out IrregularPriceReason_InDB);
                    //if (!result)
                    //{
                    //    ShowErr(sql.ToString(), result);
                    //}
                    //IrregularPriceReason_InDB = MyUtility.Check.Empty(IrregularPriceReason_InDB) ? new DataTable() : IrregularPriceReason_InDB;

                    sql.Append(" SELECT DISTINCT al.POID ,al.ArtworkTypeID ,o.BrandID ,o.StyleID ,al.POPrice, al.StandardPrice ,al.SubconReasonID ,al.AddDate ,al.AddName ,al.EditDate ,al.EditName " + Environment.NewLine);

                    sql.Append(" FROM #TmpSource t" + Environment.NewLine);
                    sql.Append(" INNER JOIN Orders o ON o.ID = t.OrderID" + Environment.NewLine);
                    sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = t.ArtworkTypeID" + Environment.NewLine);
                    sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);

                    result = MyUtility.Tool.ProcessWithDatatable(_detailDatas, "", sql.ToString(), out IrregularPriceReason_InDB, "#TmpSource", null, parameters);

                    if (!result)
                    {
                        ShowErr(sql.ToString(), result);
                        return false;
                    }
                    #endregion

                    #region 準備 B：新增的紀錄 = 當下實際存在的價格異常紀錄 - DB現有紀錄

                    var Btable = from c in IrregularPriceReason_Real.AsEnumerable()
                                 where !IrregularPriceReason_InDB.AsEnumerable().Any(o => o["POID"].ToString() == c["POID"].ToString() && o["ArtworkTypeID"].ToString() == c["ArtworkTypeID"].ToString())
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

                    //同一組POID 、 ArtworkTypeID，若DB有資料就帶DB的所有資料；DB沒有就帶上面查詢的所有資料
                    var summary = (from a in IrregularPriceReason_InDB.AsEnumerable()
                                   select new
                                   {
                                       POID = a.Field<string>("POID"),
                                       Type = a.Field<string>("ArtworkTypeID"),
                                       BrandID = a.Field<string>("BrandID"),
                                       StyleID = a.Field<string>("StyleID"),
                                       PoPrice = a.Field<decimal>("POPrice"),// DB有紀錄一律帶DB，無論價格是否一樣
                                       StdPrice = a.Field<decimal>("StandardPrice"),// DB有紀錄一律帶DB，無論價格是否一樣  c.Field<decimal>("StdPrice") 
                                       SubconReasonID = a.Field<string>("SubconReasonID"),
                                       AddDate = a.Field<DateTime?>("AddDate"),
                                       AddName = a.Field<string>("AddName"),
                                       EditDate = a.Field<DateTime?>("EditDate"),
                                       EditName = a.Field<string>("EditName")

                                   }).Union
                             (from b in IrregularPriceReason_New.AsEnumerable()
                              join c in IrregularPriceReason_Real.AsEnumerable() on new { POID = b.Field<string>("POID"), ArtWorkType = b.Field<string>("ArtworkTypeID") } equals new { POID = c.Field<string>("POID"), ArtWorkType = c.Field<string>("ArtworkTypeID") }
                              select new
                              {
                                  POID = b.Field<string>("POID"),
                                  Type = b.Field<string>("ArtworkTypeID"),
                                  BrandID = b.Field<string>("BrandID"),
                                  StyleID = b.Field<string>("StyleID"),
                                  PoPrice = b.Field<decimal>("PoPrice"),
                                  StdPrice = b.Field<decimal>("StdPrice"),
                                  SubconReasonID = b.Field<string>("SubconReasonID"),
                                  AddDate = b.Field<DateTime?>("AddDate"),
                                  AddName = b.Field<string>("AddName"),
                                  EditDate = b.Field<DateTime?>("EditDate"),
                                  EditName = b.Field<string>("EditName")
                              });

                    //串SubconReason 資料表進來，組合成P01_IrregularPrice 裡面Grid的樣子
                    var total_IPR = from a in summary
                                    join s in SubconReason.AsEnumerable() on a.SubconReasonID equals s.Field<string>("ID") into sr
                                    from s in sr.DefaultIfEmpty()
                                    select new
                                    {
                                        Factory = _FactoryID,
                                        a.POID,
                                        a.Type,
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
                    if (IPR_Grid.Rows.Count > 0)
                    {
                        //只有開啟Form的時候才需要把紀錄Copy到Datasource，否則會出事
                        if (IsNeedUpdateDT)
                        {
                            listControlBindingSource1.DataSource = IPR_Grid.Copy();
                        }
                        ModifyDT_FromP01 = IPR_Grid.Copy();
                        this.ReasonNullCount = IPR_Grid.Select("SubconReasonID=''").Length;
                    }
                    else
                    {
                        //沒有價格紀錄，所以全部帶DB就好
                        IrregularPriceReasonDT_Initial(true);
                    }

                    //當下有異常則true，讓P30判斷是否需要按鈕變色
                    if (IrregularPriceReason_Real.Rows.Count > 0)
                    {                        
                        Has_Irregular_Price = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ShowErr(ex.Message, ex);
            }

            return Has_Irregular_Price;
        }

        /// <summary>
        /// 重新計算價格並產生DataSource (僅限新增模式使用)
        /// </summary>
        /// <param name="IsNeedUpdateDT"></param>
        /// <returns></returns>
        public bool Check_Irregular_Price_Without_PO(bool IsNeedUpdateDT = true)
        {
            //是否有價格異常，用以區別P01的按鈕要不要變色
            bool Has_Irregular_Price = false;
            P01.tmp_ModifyTable = null;
            //採購價 > 標準價 =異常
            #region 變數宣告

            string poid = string.Empty;
            string artworkType = string.Empty;
            string BrandID = string.Empty;
            string StyleID = string.Empty;
            string artWorkPO_ID = string.Empty;
            string currencyID = _masterData["CurrencyID"].ToString();
            string issueDate =Convert.ToDateTime(_masterData["issueDate"]).ToAppDateFormatString();

            //List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            DualResult result;

            DataTable IrregularPriceReason_InDB;//A DB現有的 "價格異常紀錄" IPR資料
            DataTable Price_Dt;// 紀錄所有價格的Table
            DataTable IrregularPriceReason_Real = new DataTable();//C "當下實際存在的價格異常紀錄" IPR資料，無論DB有無
            DataTable IrregularPriceReason_New;//B 最新的價格異常紀錄，DB沒有

            #endregion

            #region 欄位定義
            IrregularPriceReason_Real.Columns.Add(new DataColumn("ArtworkTypeID", System.Type.GetType("System.String")));
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
                artWorkPO_ID = _ArtWorkPO_ID;
                //parameters.Add(new SqlParameter("@artWorkPO_ID", artWorkPO_ID));

                #region 查詢所有價格異常紀錄

                sql.Append($@"

--根據表身的資料，整理出ArtworkTypeID、POID、OrderID
SELECT DISTINCT
    [ArtworkTypeID] = a.ArtworkTypeID 
    ,[OrderId] = a.OrderID  
    ,[POID]=o.POID
INTO #tmp_AllOrders 
FROM #TmpSource a 
INNER JOIn ORDERS o ON a.OrderID=o.id


--列出採購價的清單（尚未總和）
SELECT * INTO #total_PO FROM(
    --第一部分：相同ArtworkTypeID + POID的其他採購單

    SELECT  ap.ArtworkTypeID
		    ,Orders.POID
		    ,[OID]=apd.OrderId
		    -- 已關單代表不會再使用這一張採購單進行「外發」，但是已經外發（arm Out）的數量後續還是會建立請款，因此已關單的要計算的是已實際外發的數量
		    ,[Qty]=IIF(ap.Status!='Closed',apd.PoQty ,apd.Farmout)
		    ,[PO_amt]= IIF(ap.Status!='Closed'
						    ,apd.PoQty * apd.Price * dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate) 
						    ,apd.Farmout * apd.Price * dbo.getRate('KP',ap.CurrencyID,'USD',ap.issuedate) )

    FROM ArtworkPO ap WITH (NOLOCK) 
    INNER JOIN ArtworkPO_Detail apd WITH (NOLOCK) on apd.id = ap.Id 
    INNER JOIN  Orders WITH (NOLOCK) on orders.id = apd.orderid
    WHERE EXiSTS  ( 
				    SELECT ArtworkTypeID,POID 
				    FROM #tmp_AllOrders 
				    WHERE ArtworkTypeID= ap.ArtworkTypeID  AND POID=Orders.POID) --相同Category、POID
    UNION ALL
    --第二部分：自己這張，因為還沒有存入DB所以要納入
    SELECT  ArtworkTypeID,o.POID,OrderId
		    ,[Qty]=PoQty
		    ,[PO_amt]= amount * dbo.getRate('KP','{this._masterData["CurrencyID"]}','USD',GETDATE()) 
    FROM #TmpSource
	INNER JOIN Orders o ON o.ID=#TmpSource.orderid

)allPO

--繡花成本處理：列出同POID、Category=EMB_Thread（繡線）的總額清單
SELECT   LPD.POID
		,LP.currencyid
		, [Price]=LPd.Price  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) --採購單價
		-- 已關單代表不會再使用這一張採購單進行「收料」，但是已經收料（In Qty ）的數量後續還是會建立請款，因此已關單的要計算的是已實際收料的數量
		, [Qty]=IIF(LP.Status!='Closed',LPD.Qty ,LPD.InQty)  --數量
		, [LocalPo_amt]=IIF(LP.Status!='Closed'
							, LPD.Price * LPD.Qty  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) --採購總額
							,LPD.Price * LPD.InQty  * dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate))

		, dbo.getRate('KP', LP.CurrencyID, 'USD', LP.issuedate) rate
INTO #Embroidery_List
FROM LocalPO LP
inner join LocalPO_Detail LPD on LP.Id = LPD.Id
INNER JOIN Orders ON Orders.ID=LPD.OrderId
WHERE LP.Category = 'EMB_Thread'  
	  AND Orders.POID IN  (SELECT POID FROM  #tmp_AllOrders)

--開始整合
SELECT o.BrandID ,o.StyleID  ,t.ArtworkTypeID  ,t.POID 
,[stdPrice]=round(Standard.order_amt/iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[PoPrice]=round( (po.Po_Amt ) / iif(Standard.order_qty=0,1,Standard.order_qty),3) 
,[PoPriceWithEmbroidery] =IIF(Embroidery.LocalPo_amt IS NULL ,
							round( (po.Po_Amt ) / iif(Standard.order_qty=0,1,Standard.order_qty),3) , 
							round(( (po.Po_Amt ) +Embroidery.LocalPo_amt) / iif(Standard.order_qty=0,1,Standard.order_qty)
							,3) )
FROM #tmp_AllOrders t
INNER JOIN Orders o WITH (NOLOCK) on o.id = t.OrderId
INNER JOIN Brand bra on bra.id=o.BrandID
OUTER APPLY(--標準價
	        select orders.POID
	        ,sum(oq.qty) order_qty        --實際外發數量
	        ,sum(oq.qty*Price) order_amt  --外發成本
	        from orders WITH (NOLOCK) 
            inner join Order_Qty oq with (nolock) on orders.ID = oq.ID
	        inner join Order_TmsCost WITH (NOLOCK) on Order_TmsCost.id = orders.ID 
	        where POID= t.POID                   --相同母單
			AND ArtworkTypeID= t.ArtworkTypeID   --相同加工
			--AND Order_TmsCost.ID  IN ( SELECT OrderID FROM #BePurchased ) ***限定 有被採購的訂單*** (註解原因 各項目可能會再其他子單進行採購)
	        group by orders.poid,ArtworkTypeID
) Standard
OUTER APPLY (--採購價，根據ArtworkTypeID、POID，作分組加總
	SELECT isnull(sum(Q.PO_amt),0.00) PO_amt
	FROM (
		SELECT PO_amt FROM #total_PO
		WHERE ArtworkTypeID = t.artworktypeid 
		AND POID = t.POID 
	) Q
) Po	
outer apply(--繡花成本，根據POID，作分組加總
	SELECT  x.POID,[LocalPo_amt]=SUM(x.LocalPo_amt) 
	FROM (	
		SELECT * FROM #Embroidery_List
		WHERE POID=t.POID
		) x   
	GROUP BY  x.POID
) Embroidery

GROUP BY  o.BrandID ,o.StyleID ,t.ArtworkTypeID ,t.POID ,Standard.order_amt ,Standard.order_qty ,po.Po_Amt ,Standard.order_qty ,Embroidery.LocalPo_amt

DROP TABLE #tmp_AllOrders ,#total_PO ,#Embroidery_List ,#TmpSource


" + Environment.NewLine);

                #endregion

                this.ShowWaitMessage("Data Loading...");
                //result = DBProxy.Current.Select(null, sql.ToString(), parameters, out Price_Dt);
                result = MyUtility.Tool.ProcessWithDatatable(_detailDatas, "", sql.ToString(), out Price_Dt, "#TmpSource", null);
                sql.Clear();
                this.HideWaitMessage();

                if (!result)
                {
                    ShowErr(sql.ToString(), result);
                }

                if (Price_Dt.Rows.Count > 0)
                {
                    #region 準備 C：當下實際存在的價格異常紀錄
                    foreach (DataRow row in Price_Dt.Rows)
                    {
                        decimal StdPrice = 0;
                        decimal purchasePrice = 0;
                        decimal PoPriceWithEmbroidery = 0;
                        //用來準備填入 C 最新的 " 價格異常紀錄" IPR資料
                        poid = Convert.ToString(MyUtility.Check.Empty(row["Poid"]) ? "" : row["Poid"]);
                        BrandID = Convert.ToString(MyUtility.Check.Empty(row["BrandID"]) ? "" : row["BrandID"]);
                        StyleID = Convert.ToString(MyUtility.Check.Empty(row["StyleID"]) ? "" : row["StyleID"]);
                        artworkType = Convert.ToString(MyUtility.Check.Empty(row["ArtworkTypeID"]) ? "" : row["ArtworkTypeID"]);
                        StdPrice = Convert.ToDecimal(MyUtility.Check.Empty(row["StdPrice"]) ? 0 : row["StdPrice"]);
                        purchasePrice = Convert.ToDecimal(MyUtility.Check.Empty(row["PoPrice"]) ? 0 : row["PoPrice"]);
                        PoPriceWithEmbroidery = Convert.ToDecimal(MyUtility.Check.Empty(row["PoPriceWithEmbroidery"]) ? 0 : row["PoPriceWithEmbroidery"]);


                        //如果ArtworkType是繡花（ArtworkTypeID = Embroidery ），要加上繡花物料成本
                        if (artworkType.ToUpper() == "EMBROIDERY")
                        {
                            //只要有異常就顯示紅色
                            if (PoPriceWithEmbroidery > StdPrice & StdPrice > 0)
                            {
                                IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, PoPriceWithEmbroidery, StdPrice, IrregularPriceReason_Real);
                            }
                        }
                        else
                        {
                            //不用加上繡花物料成本
                            //只要有異常就顯示紅色
                            if (purchasePrice > StdPrice & StdPrice > 0)
                            {
                                IrregularPriceReason_Real = CreateIrregularPriceReasonDataTabel(poid, artworkType, BrandID, StyleID, purchasePrice, StdPrice, IrregularPriceReason_Real);
                            }
                        }
                    }

                    #endregion

                    #region 準備 A：DB現有的 "價格異常紀錄"
                    // 注意！因為是新增模式下，DB絕對不會有這個PO的價格異常紀錄，所以這邊直接給他空資料
                    sql.Clear();
                    //sql.Append(" SELECT DISTINCT al.POID ,al.ArtworkTypeID ,o.BrandID ,o.StyleID ,al.POPrice, al.StandardPrice ,al.SubconReasonID ,al.AddDate ,al.AddName ,al.EditDate ,al.EditName " + Environment.NewLine);

                    //sql.Append(" FROM ArtWorkPO a" + Environment.NewLine);
                    //sql.Append(" INNER JOIN ArtworkPO_Detail ad ON a.ID = ad.ID" + Environment.NewLine);
                    //sql.Append(" INNER JOIN Orders o ON ad.OrderID = o.ID" + Environment.NewLine);
                    //sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = ad.ArtworkTypeID" + Environment.NewLine);
                    //sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);
                    //sql.Append(" WHERE 1=0" + Environment.NewLine);

                    //result = DBProxy.Current.Select(null, sql.ToString(),  out IrregularPriceReason_InDB);
                    //if (!result)
                    //{
                    //    ShowErr(sql.ToString(), result);
                    //}
                    //IrregularPriceReason_InDB = MyUtility.Check.Empty(IrregularPriceReason_InDB) ? new DataTable() : IrregularPriceReason_InDB;

                    sql.Append(" SELECT DISTINCT al.POID ,al.ArtworkTypeID ,o.BrandID ,o.StyleID ,al.POPrice, al.StandardPrice ,al.SubconReasonID ,al.AddDate ,al.AddName ,al.EditDate ,al.EditName " + Environment.NewLine);

                    sql.Append(" FROM #TmpSource t" + Environment.NewLine);
                    sql.Append(" INNER JOIN Orders o ON o.ID = t.OrderID" + Environment.NewLine);
                    sql.Append(" INNER JOIN ArtworkPO_IrregularPrice al ON al.POID = o.POID AND al.ArtworkTypeID = t.ArtworkTypeID" + Environment.NewLine);
                    sql.Append(" INNER JOIN SubconReason sr ON sr.Type = 'IP' AND sr.ID = al.SubconReasonID" + Environment.NewLine);

                    result = MyUtility.Tool.ProcessWithDatatable(_detailDatas, "", sql.ToString(), out IrregularPriceReason_InDB, "#TmpSource", null);

                    if (!result)
                    {
                        ShowErr(sql.ToString(), result);
                        return false;
                    }
                    #endregion

                    #region 準備 B：新增的紀錄 = 當下實際存在的價格異常紀錄 - DB現有紀錄

                    var Btable = from c in IrregularPriceReason_Real.AsEnumerable()
                                 where !IrregularPriceReason_InDB.AsEnumerable().Any(o => o["POID"].ToString() == c["POID"].ToString() && o["ArtworkTypeID"].ToString() == c["ArtworkTypeID"].ToString())
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

                    //同一組POID 、 ArtworkTypeID，若DB有資料就帶DB的所有資料；DB沒有就帶上面查詢的所有資料
                    var summary = (from a in IrregularPriceReason_InDB.AsEnumerable()
                                   select new
                                   {
                                       POID = a.Field<string>("POID"),
                                       Type = a.Field<string>("ArtworkTypeID"),
                                       BrandID = a.Field<string>("BrandID"),
                                       StyleID = a.Field<string>("StyleID"),
                                       PoPrice = a.Field<decimal>("POPrice"),// DB有紀錄一律帶DB，無論價格是否一樣
                                       StdPrice = a.Field<decimal>("StandardPrice"),// DB有紀錄一律帶DB，無論價格是否一樣  c.Field<decimal>("StdPrice") 
                                       SubconReasonID = a.Field<string>("SubconReasonID"),
                                       AddDate = a.Field<DateTime?>("AddDate"),
                                       AddName = a.Field<string>("AddName"),
                                       EditDate = a.Field<DateTime?>("EditDate"),
                                       EditName = a.Field<string>("EditName")

                                   }).Union
                             (from b in IrregularPriceReason_New.AsEnumerable()
                              join c in IrregularPriceReason_Real.AsEnumerable() on new { POID = b.Field<string>("POID"), ArtWorkType = b.Field<string>("ArtworkTypeID") } equals new { POID = c.Field<string>("POID"), ArtWorkType = c.Field<string>("ArtworkTypeID") }
                              select new
                              {
                                  POID = b.Field<string>("POID"),
                                  Type = b.Field<string>("ArtworkTypeID"),
                                  BrandID = b.Field<string>("BrandID"),
                                  StyleID = b.Field<string>("StyleID"),
                                  PoPrice = b.Field<decimal>("PoPrice"),
                                  StdPrice = b.Field<decimal>("StdPrice"),
                                  SubconReasonID = b.Field<string>("SubconReasonID"),
                                  AddDate = b.Field<DateTime?>("AddDate"),
                                  AddName = b.Field<string>("AddName"),
                                  EditDate = b.Field<DateTime?>("EditDate"),
                                  EditName = b.Field<string>("EditName")
                              });

                    //串SubconReason 資料表進來，組合成P01_IrregularPrice 裡面Grid的樣子
                    var total_IPR = from a in summary
                                    join s in SubconReason.AsEnumerable() on a.SubconReasonID equals s.Field<string>("ID") into sr
                                    from s in sr.DefaultIfEmpty()
                                    select new
                                    {
                                        Factory = _FactoryID,
                                        a.POID,
                                        a.Type,
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
                    if (IPR_Grid.Rows.Count > 0)
                    {

                        tmp_IrregularPriceReason_List_Ori.Clear();
                        if (tmp_IrregularPriceReason_List.Count > 0)
                        {
                            //原本的資料備份下來
                            tmp_IrregularPriceReason_List_Ori = tmp_IrregularPriceReason_List.ToList();
                            foreach (DataRow dr in IPR_Grid.Rows)
                            {
                                if (tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString() && o.ArtWorkType_ID == dr["Type"].ToString()).Any())
                                {
                                    tmp_IrregularPriceReason data = tmp_IrregularPriceReason_List.Where(o => o.POID == dr["POID"].ToString() && o.ArtWorkType_ID == dr["Type"].ToString()).FirstOrDefault();
                                    dr["SubconReasonID"] = data.SubconReasonID;
                                    dr["ResponsibleID"] = data.ResponsibleID;
                                    dr["ResponsibleName"] = data.ResponsibleName;
                                    dr["Reason"] = data.Reason;
                                }
                            }
                            IPR_Grid.AcceptChanges();
                        }

                        //只有開啟Form的時候才需要把紀錄Copy到Datasource，否則會出事
                        if (IsNeedUpdateDT)
                        {
                            listControlBindingSource1.DataSource = IPR_Grid.Copy();
                        }

                        ModifyDT_FromP01 = IPR_Grid.Copy();
                        P01.tmp_ModifyTable = IPR_Grid.Copy();
                        this.ReasonNullCount = IPR_Grid.Select("SubconReasonID=''").Length;
                    }
                    else
                    {
                        //沒有價格紀錄，所以全部帶DB就好
                        IrregularPriceReasonDT_Initial(true);
                    }

                    //當下有異常則true，讓P30判斷是否需要按鈕變色
                    if (IrregularPriceReason_Real.Rows.Count > 0)
                    {
                        Has_Irregular_Price = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ShowErr(ex.Message, ex);
            }

            return Has_Irregular_Price;
        }

        private DataTable CreateIrregularPriceReasonDataTabel(string POID, string ArtWorkType, string BrandID, string StyleID, decimal purchasePrice, decimal StdPrice, DataTable IrregularPriceReason_New)
        {
            DataRow ndr = IrregularPriceReason_New.NewRow();

            ndr["poid"] = POID;
            ndr["ArtworkTypeID"] = ArtWorkType;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }

    /// <summary>
    /// Grid上的異常價格資料列
    /// </summary>
    public class tmp_IrregularPriceReason
    {
        public string POID { get; set; }
        public string ArtWorkType_ID { get; set; }
        public string SubconReasonID { get; set; }
        public string ResponsibleID { get; set; }
        public string ResponsibleName { get; set; }
        public string Reason { get; set; }
    }
}
