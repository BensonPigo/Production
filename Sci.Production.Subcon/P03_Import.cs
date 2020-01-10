﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win;
using Sci.Win.Tems;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using Ict.Win;

namespace Sci.Production.Subcon
{
    public partial class P03_Import : QueryForm
    {
        // 判斷進入此畫面的是 Subcon_P03, Subcon_P04
        public static readonly bool Subcon_P03 = true, Subcon_P04 = false;
        private bool Subcon;
        // 傳入參數，SQL 要取得的資料
        private const int MasterData = 1, DetailData = 2;
        // 判斷 Sel 欄位 Click 得進入點
        private bool masterHeaderClick;
        // 若 masterHeaderClick 進入點 = Header 則 ErrorMsg 暫存
        private StringBuilder errorMsg = new StringBuilder("");
        // Import 畫面上的參數
        private string strArtworkType, strFarmOutDateStart, strFarmOutDateEnd, strSPnum;
        /*
         * SubconDetailDT       : Subcon_P03 || Subcon_P04 Detail Data
         * masterDT, detailDT   : Import Data
         */
        private DataTable SubconDetailDT, masterDT, detailDT;
        private BindingSource bsm, bsd;

        public P03_Import(bool subcon, string strArtworkType, DataTable SubconDetailDT)
        {
            InitializeComponent();
            this.Subcon = subcon;
            this.strArtworkType = strArtworkType;
            this.SubconDetailDT = SubconDetailDT;
            #region set Title
            this.Text = (Subcon == Subcon_P03 ? "Import Farm Out. " : "Import Farm In.") + (strArtworkType.Empty() ? "" : "(" + strArtworkType + ")");
            #endregion
            #region Set Label Text
            this.labelFramDate.Text = Subcon == Subcon_P03 ? "Farm Out Date" : "Farm In Date";
            #endregion 
            #region Set Grid
            Ict.Win.UI.DataGridViewCheckBoxColumn masterChk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
            this.gridMaster.IsEditingReadOnly = false;
            this.gridDetail.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridMaster)
                .CheckBox("sel", header: "Sel", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out masterChk)
                .Text("BundleNo", header: "BundleNo", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .DateTime("InComing", header: "InComing", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .DateTime("OutGoing", header: "OutGoing", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("SP#", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Style", header: "Style", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("CutPartID", header: "CutPart ID", width: Widths.AnsiChars(10), iseditingreadonly: true);

            Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("sel", header: "Sel", width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false)
                .Text("BundleNo", header: "BundleNo", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("PO#", header: "PO#", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Artwork", header: "Artwork", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("CutpartID", header: "Cutpart ID", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Cutpart Name", header: "Cutpart Name", width: Widths.AnsiChars(40), iseditingreadonly: true)
                .Numeric("PoQty", header: (Subcon == Subcon_P03) ? "PoQty" : "FarmOut", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Farm", header: (Subcon == Subcon_P03) ? "FarmOut" : "FarmIn", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), iseditingreadonly: true);

            for (int i = 0; i < this.gridMaster.Columns.Count; i++)
                this.gridMaster.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            for (int i = 0; i < this.gridDetail.Columns.Count; i++)
                this.gridDetail.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            #region 判斷 Sel 欄位 Click 得進入點
            masterChk.CellClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    errorMsg = new StringBuilder("");
                    masterHeaderClick = true;
                    this.ShowWaitMessage("Data Processing...");
                }
                else
                    masterHeaderClick = false;
            };
            #endregion 
            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            DualResult result;
            bool bolIntoOut = false;
            string subProcessID = string.Empty;
            #region set Data
            /*
             * Farm Out Datet查詢條件，結束日期須加23:59:59
             */
            strFarmOutDateStart = dateRangeFarmOutDate.Value1.Empty() ? "" : ((DateTime)dateRangeFarmOutDate.Value1).ToString("yyyy/MM/dd");
            strFarmOutDateEnd = dateRangeFarmOutDate.Value2.Empty() ? "" : ((DateTime)dateRangeFarmOutDate.Value2).ToString("yyyy/MM/dd") + " 23:59:59";
            strSPnum = textSPnum.Text;
            #endregion
            #region check Data
            /*
             * check FarmOutDate and SP#
             */
            if (strFarmOutDateStart.Empty() && strFarmOutDateEnd.Empty() && strSPnum.Empty())
            {
                MyUtility.Msg.WarningBox("FarmOutDate and SP# can't all be empty.");
                return;
            }
            /*
             * check ArtworkType
             */
            if (!MyUtility.Check.Seek(string.Format(@"
select  ID
from SubProcess
where ArtworkTypeID = '{0}'", strArtworkType), null))
            {
                MyUtility.Msg.WarningBox(string.Format("Can't find ArtworkType = {0} data, please check [Cutting][B01]SubProcess Data !!", strArtworkType));
                return;
            }
            #endregion

            sql = string.Format("select ID, [IntoOut] = cast(case InOutRule when 3 then 1 else 0 end as bit) from SubProcess where ArtworkTypeID = '{0}'", strArtworkType);
            result = DBProxy.Current.Select(null, sql, out dt);
            foreach(DataRow dataRow in dt.Rows)
            {
                subProcessID = MyUtility.Convert.GetString(dataRow["ID"]);
                bolIntoOut = MyUtility.Convert.GetBool(dataRow["IntoOut"]);
            }

            #region SQL parameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>();
            listSQLParameter.Add(new SqlParameter("@M", Sci.Env.User.Keyword));
            listSQLParameter.Add(new SqlParameter("@ArtworkType", strArtworkType));
            listSQLParameter.Add(new SqlParameter("@SubProcessID", subProcessID));
            listSQLParameter.Add(new SqlParameter("@FarmStart", strFarmOutDateStart));
            listSQLParameter.Add(new SqlParameter("@FarmEnd", strFarmOutDateEnd));
            listSQLParameter.Add(new SqlParameter("@SPnum", strSPnum));
            #endregion
            #region SQL Where Filte
            #region FarmDateFilte 若為[Subcon][P03] => OutGoing, [Subcon][P04] => InComing
            string strFarmDateFilte = "";
            if (!strFarmOutDateStart.Empty() && !strFarmOutDateEnd.Empty())
            {
                strFarmDateFilte = Subcon == Subcon_P03 ? "and BIO.OutGoing between @FarmStart and @FarmEnd" : "and BIO.InComing between @FarmStart and @FarmEnd";
            }
            else if (!strFarmOutDateStart.Empty())
            {
                strFarmDateFilte = Subcon == Subcon_P03 ? "and @FarmStart <= BIO.OutGoing " : "and @FarmStart <= BIO.InComing ";
            }
            else if (!strFarmOutDateEnd.Empty())
            {
                strFarmDateFilte = Subcon == Subcon_P03 ? "and BIO.OutGoing <= @FarmEnd" : "and BIO.InComing <= @FarmEnd";
            }
            #endregion
            Dictionary<string, string> dictionaryFilte = new Dictionary<string, string>();
            dictionaryFilte.Add("FarmDate", strFarmDateFilte);
            dictionaryFilte.Add("SPnum", strSPnum.Empty() ? "" : "and B.OrderID = @SPnum");
            #endregion
            #region SQL Command
            #region Master
            string sqlMasterData = (Subcon == Subcon_P03) ? getDataSubconP03(MasterData, dictionaryFilte, bolIntoOut) : getDataSubconP04(MasterData, dictionaryFilte, bolIntoOut);
            #endregion
            #region Detail
            string sqlDetailData = (Subcon == Subcon_P03) ? getDataSubconP03(DetailData, dictionaryFilte, bolIntoOut) : getDataSubconP04(DetailData, dictionaryFilte, bolIntoOut);
            #endregion
            #endregion
            this.ShowWaitMessage("SQL Processing...");
            #region SQL Process 
            #region Master
            result = DBProxy.Current.Select(null, sqlMasterData, listSQLParameter, out masterDT);
            if (!result)
            {
                this.HideWaitMessage();
                MyUtility.Msg.WarningBox(result.Description);
                return;
            }
            masterDT.TableName = "master";
            #endregion
            #region Detail
            result = DBProxy.Current.Select(null, sqlDetailData, listSQLParameter, out detailDT);
            if (!result)
            {
                this.HideWaitMessage();
                MyUtility.Msg.WarningBox(result.Description);
                return;
            }
            detailDT.TableName = "detail";
            #endregion
            #endregion
            this.HideWaitMessage();
            if (masterDT == null || masterDT.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return;
            }
            #region Set DataRelation
            DataSet data = new DataSet();
            data.Tables.Add(masterDT);
            data.Tables.Add(detailDT);
            DataRelation relation = new DataRelation("Info"
               , new DataColumn[] {masterDT.Columns["bind"] }
               , new DataColumn[] { detailDT.Columns["bind"] }
               );
            data.Relations.Add(relation);   
            bsm = new BindingSource(data, "master");
            bsd = new BindingSource(bsm, "Info");
            gridMaster.DataSource = bsm;
            gridDetail.DataSource = bsd;
            #endregion 
            #region 若資料為一對一，則Master及detail自動打勾
            foreach (DataRow dr in masterDT.Rows)
            {
                int count = dr.GetChildRows("Info").Length;
                if (count == 1)
                {
                    dr["sel"] = true;
                    DataRow[] childRows = dr.GetChildRows("Info");
                    childRows[0]["sel"] = true;
                }
            }
            #endregion    
        }

        private string getDataSubconP03(int getData, Dictionary<string, string> dictionaryFilte, bool IntoOut)
        {
            StringBuilder strReturn = new StringBuilder("");
            //string strIntoOut = IntoOut ? "and APD.Farmin > APD.Farmout" : "and APD.PoQty > APD.Farmout";
            switch (getData)
            {
                case MasterData:
                    #region Master
                    strReturn.Append(string.Format(@"
select	distinct sel = 0 
		, BIO.BundleNo
		, BIO.InComing
		, BIO.OutGoing
		, [SP#] = B.Orderid
		, [Style] = O.StyleID 
		, [CutPartID] = BD.Patterncode
        , bind = Concat (BIO.BundleNo, '-', BD.Patterncode)
into #tmp_BundleInOut
from BundleInOut BIO
left join Bundle_Detail BD		on	BD.BundleNo = BIO.BundleNo
left join Bundle B				on	BD.Id = B.ID
left join ArtworkPO_Detail APD	on  APD.OrderID = B.Orderid 
									and APD.PatternCode = BD.Patterncode
                                    and APD.PatternDesc = BD.PatternDesc
									--若為[Subcon][P04]呼叫，則改為Farmin
									and APD.PoQty > APD.Farmout
left join ArtworkPO AP			on	AP.ID = APD.ID
left join Orders O				on	O.ID = B.Orderid and o.MDivisionID  = b.MDivisionID 
where	BIO.SubProcessId = @SubProcessID
        {0}
        --SP#查詢條件    
        {1}
        and isnull(BIO.RFIDProcessLocationID,'') = ''
		and BIO.OutGoing is not null 
		and AP.MDivisionID = @M
		--@ArtworkType
		and AP.ArtworkTypeID = @ArtworkType
		and AP.Status = 'Approved'
		--and AP.Closed = 0 
        

select t.*
from #tmp_BundleInOut t
--若BundleNo已確認且存在，則不用撈出來
where not exists (select FOD.BundleNo     
				from FarmOut_Detail FOD  --若為[Subcon][P04]呼叫，則改為Farmin
				left join FarmOut FO			on FO.Id = FOD.ID
				left join ArtworkPO_Detail APD on FOD.ArtworkPo_DetailUkey = APD.ukey
				where FOD.BundleNo = t.BundleNo
					and FO.Status = 'Confirmed' 
					--登入M  @ArtworkType
					and MDivisionID = @M
					and APD.ArtworkTypeID = @ArtworkType) 
--若為[Subcon][P04]呼叫，則改為InComing
order by t.OutGoing  

drop table #tmp_BundleInOut
", dictionaryFilte["FarmDate"]
 , dictionaryFilte["SPnum"]));
                    #endregion
                    break;
                case DetailData:
                    #region Detail
                    strReturn.Append(string.Format(@"
select	sel = 0 
		, BIO.BundleNo
		, [PO#] = AP.ID
		, [Supplier] = AP.LocalSuppID
		, [Artwork] = APD.ArtworkId
		, [CutpartID] = APD.PatternCode
		, [Cutpart Name] = APD.PatternDesc
		, APD.PoQty
        --若為[Subcon][P04]呼叫，則改為Farmin 
		, Farm = APD.Farmout
		, BD.Qty
        , APD.Ukey
        , bind = Concat (BIO.BundleNo, '-', BD.Patterncode)
        , BIO.OutGoing
into #tmp_BundleInOut
from BundleInOut BIO
left join Bundle_Detail BD		on	BD.BundleNo = BIO.BundleNo
left join Bundle B				on	BD.Id = B.ID
left join ArtworkPO_Detail APD	on	APD.OrderID = B.Orderid 
									and APD.PatternCode = BD.Patterncode
                                    and APD.PatternDesc = BD.PatternDesc
                                    --若為[Subcon][P04]呼叫，則改為Farmin 
									and APD.PoQty > APD.Farmout
left join ArtworkPO AP			on AP.ID = APD.ID
where	BIO.SubProcessId = @SubProcessID
        --Farm Out Datet查詢條件
        {0}
		--SP#查詢條件
		{1}
        and isnull(BIO.RFIDProcessLocationID,'') = ''
        --若為[Subcon][P04] => InComing
		and BIO.OutGoing is not null
		--登入M
		and AP.MDivisionID = @M
		--畫面上ArtworkType
		and AP.ArtworkTypeID = @ArtworkType
		and AP.Status = 'Approved'
		--and AP.Closed = 0

select t.*
from #tmp_BundleInOut t
		--若BundleNo已確認且存在，則不用撈出來
where not Exists (select FOD.BundleNo     
							from FarmOut_Detail FOD
                            --若為[Subcon][P04]呼叫，則改為 FarmIn
							left join FarmOut FO			on FO.Id=FOD.ID
							left join ArtworkPO_Detail APD on FOD.ArtworkPo_DetailUkey = APD.ukey
							where	FOD.BundleNo = t.BundleNo
                                    and FO.Status = 'Confirmed' 
									and MDivisionID = @M
									and APD.ArtworkTypeID = @ArtworkType)
--若為[Subcon][P04]呼叫，則改為 InComing  
order by t.OutGoing

drop table #tmp_BundleInOut
", dictionaryFilte["FarmDate"]
 , dictionaryFilte["SPnum"]));
                    #endregion
                    break;
            }
            return strReturn.ToString();
        }

        private string getDataSubconP04(int getData, Dictionary<string, string> dictionaryFilte, bool IntoOut)
        {
            StringBuilder strReturn = new StringBuilder("");
            //string strIntoOut = IntoOut ? "and APD.PoQty > APD.Farmin" : "and APD.Farmout > APD.Farmin";
            switch (getData)
            {
                case MasterData:
                    #region Master
                    strReturn.Append(string.Format(@"
select	distinct sel = 0
		, BIO.BundleNo
		, BIO.InComing
		, BIO.OutGoing
		, [SP#] = B.Orderid
		, [Style] = O.StyleID 
		, [CutPartID] = BD.Patterncode
        , bind = Concat (BIO.BundleNo, '-', BD.Patterncode)
into #tmp_BundleInOut
from BundleInOut BIO
left join Bundle_Detail BD		on	BD.BundleNo = BIO.BundleNo
left join Bundle B				on	BD.Id = B.ID
left join ArtworkPO_Detail APD	on  APD.OrderID = B.Orderid 
									and APD.PatternCode = BD.Patterncode
                                    and APD.PatternDesc = BD.PatternDesc
									--若為[Subcon][P03]呼叫，則改為Farmout 
									and APD.Farmout > APD.Farmin
left join ArtworkPO AP			on	AP.ID = APD.ID
left join Orders O				on	O.ID = B.Orderid  and o.MDivisionID  = b.MDivisionID 
where	BIO.SubProcessId = @SubProcessID
		{0}
		--SP#查詢條件
		{1}
		--若為[Subcon][P03] => OutGoing		
        and isnull(BIO.RFIDProcessLocationID,'') = ''
        and BIO.InComing is not null  
		--登入M
		and AP.MDivisionID = @M
		--@ArtworkType
		and AP.ArtworkTypeID = @ArtworkType
		and AP.Status = 'Approved'
		--and AP.Closed = 0


select t.*
from #tmp_BundleInOut t
		--若BundleNo已確認且存在，則不用撈出來
where not Exists (select FOD.BundleNo     
			      from FarmOut_Detail FOD  
			      --若為[Subcon][P03]呼叫，則改為FarmOut
			      left join FarmIn FI			on FI.Id = FOD.ID
			      left join ArtworkPO_Detail APD on FOD.ArtworkPo_DetailUkey = APD.ukey
			      where FOD.BundleNo = t.BundleNo
                        and FI.Status = 'Confirmed' 
			      		--登入M  @ArtworkType
			      		and MDivisionID = @M
			      		and APD.ArtworkTypeID = @ArtworkType)  
--若為[Subcon][P03]呼叫，則改為 OutGoing  
order by t.InComing

drop table #tmp_BundleInOut
", dictionaryFilte["FarmDate"]
 , dictionaryFilte["SPnum"]));
                    #endregion
                    break;
                case DetailData:
                    #region Detail
                    strReturn.Append(string.Format(@"
select	sel = 0 
		, BIO.BundleNo
		, [PO#] = AP.ID
		, [Supplier] = AP.LocalSuppID
		, [Artwork] = APD.ArtworkId
		, [CutpartID] = APD.PatternCode
		, [Cutpart Name] = APD.PatternDesc
		, PoQty = APD.Farmout
		--若為[Subcon][P03]呼叫，則改為Farmout 
		, Farm = APD.Farmin
		, BD.Qty
        , APD.Ukey
        , bind = Concat (BIO.BundleNo, '-', BD.Patterncode)
        , BIO.InComing
into #tmp_BundleInOut
from BundleInOut BIO
left join Bundle_Detail BD		on	BD.BundleNo = BIO.BundleNo
left join Bundle B				on	BD.Id = B.ID
left join ArtworkPO_Detail APD	on	APD.OrderID = B.Orderid 
									and APD.PatternCode = BD.Patterncode
                                    and APD.PatternDesc = BD.PatternDesc
									--若為[Subcon][P03]呼叫，則改為Farmout 
									and APD.Farmout > APD.Farmin
left join ArtworkPO AP			on AP.ID = APD.ID
where	BIO.SubProcessId = @SubProcessID
		--Farm Out Datet查詢條件
		{0}
		--SP#查詢條件
		{1}
        and isnull(BIO.RFIDProcessLocationID,'') = ''
		--若為[Subcon][P03] => OutGoing
		and BIO.InComing is not null  
		--登入M
		and AP.MDivisionID = @M
		--畫面上ArtworkType
		and AP.ArtworkTypeID = @ArtworkType
		and AP.Status = 'Approved'	
		--and AP.Closed = 0

select t.*
from #tmp_BundleInOut t
--若BundleNo已確認且存在，則不用撈出來
where not Exists (select BundleNo     
				  from FarmOut_Detail FOD
				  --若為[Subcon][P03]呼叫，則改為FarmOut
				  left join FarmIn FI			on FI.Id=FOD.ID
				  left join ArtworkPO_Detail APD on FOD.ArtworkPo_DetailUkey = APD.ukey
				  where	FOD.BundleNo = t.BundleNo
                    and FI.Status = 'Confirmed' 
				  	and MDivisionID = @M
				  	and APD.ArtworkTypeID = @ArtworkType)
--若為[Subcon][P03]呼叫，則改為 OutGoing  
order by t.InComing

drop table #tmp_BundleInOut
"
, dictionaryFilte["FarmDate"]
, dictionaryFilte["SPnum"]));
                    #endregion
                    break;
            }
            return strReturn.ToString();
        }

        private void gridMaster_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            #region master 勾選 Detail 自動全選
            DataRow masterDr = gridMaster.GetDataRow(e.RowIndex);
            bool value = Convert.ToBoolean(masterDr["sel"]);
            this.masterGridSelChange(masterDr, value);
            #endregion
            #region 如果 Sel 的改變是因為 Header Click
            /*
             * 需判斷 這次的變更是否 false => true
             * Yes，代表此次變更是將所有的資料勾選
             * 顯示 ErrorMsg 的時間點 : 當所有【Sel】欄位都為 true
             */
            if (masterHeaderClick)
            {
                bool masterSelectValue = Convert.ToBoolean(masterDr["Sel"]);
                bool checkChang = masterDT.AsEnumerable().Any(row => Convert.ToBoolean(row["Sel"]) != masterSelectValue);
                if (checkChang == false)
                {
                    if (masterSelectValue == true && !errorMsg.EqualString(""))
                    {
                        errorMsg.Append("already checked, can't duplicate check!!");
                        MyUtility.Msg.WarningBox(errorMsg.ToString());
                        errorMsg = null;
                    }
                    masterHeaderClick = false;
                    this.HideWaitMessage();
                }
            }
            #endregion 
        }

        private void gridDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            #region detail 勾選 master 自動勾選
            if (e.RowIndex != -1)
            {
                DataRow detailDr = this.gridDetail.GetDataRow(e.RowIndex);
                detailCheckData(detailDr);
            }
            #endregion
        }

        /// <summary> master 勾選 & 取消勾選，連帶更動 detail 的 Sel
        ///</summary>
        /// <param name="masterDr"></param>
        /// <param name="value">準備要更改【True or False】</param>
        private void masterGridSelChange(DataRow masterDr, bool value)
        {
            DataRow[] child = masterDr.GetChildRows("Info");
            foreach (DataRow childDr in child)
            {
                childDr["Sel"] = value;
                if (value == true) detailCheckData(childDr);
                childDr.EndEdit();
            }
        }

        /// <summary> detail check 同一個 master 是否有 【Artwork, Supplier】相同的 Detail 被勾選
        /// </summary>
        /// <param name="detailDr">正在改變的 Row</param>
        private void detailCheckData(DataRow detailDr)
        {
            if (Convert.ToBoolean(detailDr["Sel"]))
            {
                /*
                    * 執行到這邊，CheckBox 值已經改變
                    * 計算數量時要把自己算進去
                    * 因此 count > 1 而不是 0
                    */
                int count = detailDr.GetParentRow("Info").GetChildRows("Info").AsEnumerable().Where(row => Convert.ToBoolean(row["Sel"]) == true
                                                                                                            && row["Artwork"].EqualString(detailDr["Artwork"])
                                                                                                            && row["Supplier"].EqualString(detailDr["Supplier"])).ToList().Count();
                if (count > 1)
                {
                    // 如果 masterHeaderClick = true 則 Msg 需暫存
                    if (masterHeaderClick)
                    {
                        errorMsg.Append(string.Format("Bundle No = {0}, Artwork = {1}, Supplier = {2}" + Environment.NewLine, detailDr["BundleNo"], detailDr["Artwork"], detailDr["Supplier"]));
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox(string.Format("Bundle No = {0}, Artwork = {1}, Supplier = {2} already checked, can't duplicate check!!", detailDr["BundleNo"], detailDr["Artwork"], detailDr["Supplier"]));
                    }
                    detailDr["Sel"] = false;
                    detailDr.EndEdit();
                }                
            }
            #region  確認同一個 parent 的 child 勾選狀態
            var parentRow = detailDr.GetParentRow("Info");
            bool value = parentRow.GetChildRows("Info").AsEnumerable().Any(row => Convert.ToBoolean(row["Sel"]) == true);
            if (Convert.ToBoolean(parentRow["Sel"]) != value)
            {
                parentRow["Sel"] = value;
                parentRow.EndEdit();
            }
            #endregion
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            #region 確認 master 是否有資料尚未勾選
            bool checkMaster = masterDT.AsEnumerable().Any(row => !Convert.ToBoolean(row["Sel"]));
            if (checkMaster)
            {
                DialogResult dialogResult = MyUtility.Msg.QuestionBox("Some data is not checked. Do you want to continue?");
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            importData();
            #endregion 
        }

        private void importData()
        {
            #region 強制將所有勾選的資料匯出
            DataRow[] arraySelectDr = masterDT.AsEnumerable().Where(row => Convert.ToBoolean(row["Sel"])).ToArray();
            foreach (DataRow selectDr in arraySelectDr)
            {
                foreach (DataRow childDr in selectDr.GetChildRows("Info").AsEnumerable().Where(row => Convert.ToBoolean(row["Sel"])).ToArray())
                {
                    DataRow insertDr = SubconDetailDT.NewRow();
                    insertDr["BundleNo"] = selectDr["BundleNo"];
                    insertDr["OrderID"] = selectDr["SP#"];
                    insertDr["StyleID"] = selectDr["Style"];
                    insertDr["ArtworkPoID"] = childDr["Po#"];
                    insertDr["ArtworkID"] = childDr["Artwork"];
                    insertDr["PatternCode"] = childDr["CutPartID"];
                    insertDr["PatternDesc"] = childDr["CutPart Name"];
                    insertDr["ArtworkPoQty"] = childDr["PoQty"];
                    insertDr["onHand"] = childDr["Farm"];
                    insertDr["Variance"] = Convert.ToDecimal(childDr["PoQty"]) - Convert.ToDecimal(childDr["Farm"]);
                    insertDr["Qty"] = childDr["Qty"];
                    insertDr["BalQty"] = Convert.ToDecimal(childDr["PoQty"]) - Convert.ToDecimal(childDr["Farm"]) - Convert.ToDecimal(childDr["Qty"]);
                    insertDr["artworkPo_detailukey"] = childDr["ukey"];
                    insertDr["importData"] = 1;
                    SubconDetailDT.Rows.Add(insertDr);
                }
            }
            #endregion 
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
    }
}
