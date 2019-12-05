using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using static Sci.Production.Packing.P25;

namespace Sci.Production.Packing
{
    public partial class P25_AssignPackingList : Sci.Win.Tems.Base
    {
        private List<MappingModel> _MappingModels;
        private DataTable _P25Dt;
        private DataTable GridDt = new DataTable();
        public bool canConvert = false;

        public P25_AssignPackingList(List<MappingModel> MappingModels, DataTable P25Dt)
        {
            this.InitializeComponent();
            this._MappingModels = MappingModels;
            this._P25Dt = P25Dt;
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "ZPLFileName", DataType = typeof(string) });
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "PackingListID", DataType = typeof(string) });
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            foreach (var mappingModel in this._MappingModels)
            {
                DataRow dr = this.GridDt.NewRow();
                dr["ZPLFileName"] = mappingModel.FileName;
                dr["PackingListID"] = mappingModel.PackingListID;
                this.GridDt.Rows.Add(dr);
            }

            this.listControlBindingSource1.DataSource = this.GridDt;

            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
.Text("ZPLFileName", header: "ZPL File Name ", width: Widths.AnsiChars(35))
.Text("PackingListID", header: "PackingList#", width: Widths.AnsiChars(15), iseditingreadonly: false)
;
        }

        private void btnProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                DualResult result;
                bool packingListError = false;
                List<string> notMapping_FileName = new List<string>();

                #region 檢查表格內的每一個檔案，裡面的每一張ZPL有沒有Mapping
                foreach (DataRow dr in dt.Rows)
                {
                    string fileName = dr["ZPLFileName"].ToString();
                    string packingListID = dr["PackingListID"].ToString();
                    bool isAnyNotMapping = false;

                    MappingModel current = this._MappingModels.Where(o => o.FileName == fileName).FirstOrDefault();

                    foreach (var ZPL in current.ZPL_Content)
                    {
                        // 檢查Usee是否有輸入不對的PackingList ID
                        if (!this.checkPackingList_Exists(ZPL.CustPONo, ZPL.StyleID, packingListID, ZPL.Article, ZPL.CTNStartNo, ZPL.ShipQty, ZPL.SizeCode))
                        {
                            isAnyNotMapping = true;
                        }
                    }

                    if (isAnyNotMapping)
                    {
                        notMapping_FileName.Add(fileName);
                    }

                }
                #endregion

                // 失敗狀況
                if (notMapping_FileName.Count > 0)
                {

                    // 上一層表格填入結果
                    foreach (var fileName in notMapping_FileName.Distinct())
                    {
                        this._P25Dt.AsEnumerable().Where(o => o["ZPLFileName"].ToString() == fileName).FirstOrDefault()["Result"] = "Fail";
                    }

                    MyUtility.Msg.InfoBox("PackingList# does not Mapping.");
                    this.canConvert = false;
                    return;
                }
                // 成功狀況
                else
                {
                    #region 把每一個檔案，的每一張ZPL都寫入PackingList_Detail
                    foreach (DataRow dr in dt.Rows)
                    {
                        string fileName = dr["ZPLFileName"].ToString();
                        string packingListID = dr["PackingListID"].ToString();
                        string cmd = string.Empty;
                        string updateCmd = string.Empty;
                        int i = 0;

                        MappingModel current = this._MappingModels.Where(o => o.FileName == fileName).FirstOrDefault();

                        foreach (var ZPL in current.ZPL_Content)
                        {
                            cmd += $@"

SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

SELECT pd.*
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' AND pd.CustCTN = ''
	AND p.ID='{packingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmpOrders{i})
    AND CTNStartNo='{ZPL.CTNStartNo}' 
    AND Article = '{ZPL.Article}'
    AND pd.ShipQty={ZPL.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{ZPL.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{ZPL.SizeCode}'
        )

UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey

DROP TABLE #tmpOrders{i},#tmp{i}
";
                            i++;
                        }

                        updateCmd += Environment.NewLine + "---------" + Environment.NewLine + cmd;

                        // 成功狀況
                        using (TransactionScope transactionscope = new TransactionScope())
                        {
                            if (!(result = DBProxy.Current.Execute(null, updateCmd.ToString())))
                            {
                                transactionscope.Dispose();
                                this.canConvert = false;
                                this.ShowErr(result);
                            }
                            else
                            {
                                transactionscope.Complete();
                                transactionscope.Dispose();

                                this._P25Dt.AsEnumerable().Where(o => o["ZPLFileName"].ToString() == fileName).FirstOrDefault()["Result"] = "Pass";

                                this.canConvert = true;
                            }
                        }
                    }
                    #endregion
                    MyUtility.Msg.InfoBox("Mapping successful!");
                    this.Close();

                }

            }
            catch (Exception exp)
            {
                this.ShowErr(exp);
            }
        }

        /// <summary>
        /// 檢查User輸入的PL#是否正確
        /// </summary>
        private bool checkPackingList_Exists(string CustPONo, string StyleID, string PackingListID, string Article, string CTNStartNo, string ShipQty, string SizeCode)
        {
            string cmd = string.Empty;
            cmd = $@"


SELECT ID ,StyleID ,POID
INTO #tmpOrders0
FROM Orders 
WHERE CustPONo='{CustPONo}' AND StyleID='{StyleID}'

SELECT pd.*
INTO #tmp0
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' AND pd.CustCTN = ''
	AND p.ID='{PackingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmpOrders0)
    AND CTNStartNo='{CTNStartNo}' 
    AND Article = '{Article}'
    AND pd.ShipQty={ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders0) AND SizeSpec IN ('{SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{SizeCode}'
        )
		
SELECT pd.*
FROM PackingList_Detail pd
INNER JOIN #tmp0 t ON t.Ukey=pd.Ukey

DROP TABLE #tmpOrders0,#tmp0
";

            return MyUtility.Check.Seek(cmd);

        }
    }
}
