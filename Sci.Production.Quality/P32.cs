using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Widths = Ict.Win.Widths;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P32 : Sci.Win.Tems.Input6
    {
        private string _Type = string.Empty;
        private P32Header _sourceHeader = new P32Header();
        private string _oldStage = string.Empty;
        private bool _canConfirm = false;

        /// <inheritdoc/>
        public P32(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Text = type == "1" ? "P32. CFA Inspection Record " : "P321. CFA Inspection Record (History)";
            this._Type = type;
            this._sourceHeader = null;

            string Isfinished = type == "1" ? "0" : "1";
            string defaultFilter = $"EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE MDivisionID='{Sci.Env.User.Keyword}' AND Finished = {Isfinished} AND ID = CFAInspectionRecord.OrderID)";
            this.DefaultFilter = defaultFilter;

            if (type != "1")
            {
                this.IsSupportNew = false;
                this.IsSupportEdit = false;
                this.IsSupportDelete = false;
                this.IsSupportConfirm = false;
                this.IsSupportUnconfirm = false;
                this.IsSupportCopy = false;
                this.IsSupportClip = false;
            }

            this.comboTeam.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        public P32(ToolStripMenuItem menuitem, string type, P32Header sourceHeader = null)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Text = type == "1" ? "P32. CFA Inspection Record " : "P321. CFA Inspection Record (History)";
            this._Type = type;
            this._sourceHeader = sourceHeader;

            string Isfinished = type == "1" ? "0" : "1";
            string defaultFilter = $"EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE MDivisionID='{Sci.Env.User.Keyword}' AND Finished = {Isfinished} AND ID = CFAInspectionRecord.OrderID)";
            this.DefaultFilter = defaultFilter;

            if (type != "1")
            {
                this.IsSupportNew = false;
                this.IsSupportEdit = false;
                this.IsSupportDelete = false;
                this.IsSupportConfirm = false;
                this.IsSupportUnconfirm = false;
                this.IsSupportCopy = false;
                this.IsSupportClip = false;
            }

            this.comboTeam.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            bool canConfrim = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanConfirm");
            this._canConfirm = canConfrim;

            // GridNew屬性 : Click New時，預設表身要帶入幾筆Row
            this.GridNew = 0;

            if (this._sourceHeader != null)
            {
                this.DoNew();
            }
            
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.AutoInsertBySP(this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["Seq"].ToString());

            this.CauculateSQR();

            this.Reset_comboStage(this.CurrentMaintain["OrderID"].ToString());

            this.ComboStage_Change(this.CurrentMaintain["Stage"].ToString());

            this.CalInsepectionCtn(this.IsDetailInserting);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region Defect Code 事件

            Ict.Win.DataGridViewGeneratorTextColumnSettings DefectCodeSet = new DataGridViewGeneratorTextColumnSettings();
            DefectCodeSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable dt;
                    string sqlcmd = "SELECT ID , Description ,GarmentDefectTypeID FROM GarmentDefectCode WITH(NOLOCK)";


                    IList<DataRow> selectedDatas;
                    DualResult Dresult;
                    if (!(Dresult = DBProxy.Current.Select(null, sqlcmd, out dt)))
                    {
                        this.ShowErr(sqlcmd, Dresult);
                        return;
                    }


                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                            , "ID,Description,GarmentDefectTypeID"
                            , "5,70,3", this.CurrentDetailData["GarmentDefectCodeID"].ToString()
                            , "Defect Code,Description,Type");

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }

                    selectedDatas = selepoitem.GetSelecteds();

                    this.CurrentDetailData["GarmentDefectTypeID"] = selectedDatas[0]["GarmentDefectTypeID"];
                    this.CurrentDetailData["GarmentDefectCodeID"] = selectedDatas[0]["ID"];
                    this.CurrentDetailData["Description"] = selectedDatas[0]["Description"];

                    this.CurrentDetailData.EndEdit();
                }
            };

            DefectCodeSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;

                if (String.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["GarmentDefectCodeID"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["GarmentDefectTypeID"] = string.Empty;
                        this.CurrentDetailData["GarmentDefectCodeID"] = string.Empty;
                        this.CurrentDetailData["Description"] = string.Empty;
                    }
                    else
                    {
                        DataTable dt;
                        DataRow row;
                        List<SqlParameter> paras = new List<SqlParameter>();
                        paras.Add(new SqlParameter("@ID", e.FormattedValue));
                        string sqlcmd = $"SELECT ID , Description ,GarmentDefectTypeID FROM GarmentDefectCode WITH(NOLOCK) WHERE ID = @ID ";

                        DualResult Dresult;
                        if (!(Dresult = DBProxy.Current.Select(null, sqlcmd, paras, out dt)))
                        {
                            this.ShowErr(sqlcmd, Dresult);
                            return;
                        }


                        if (dt == null || dt.Rows.Count == 0)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Defect CodeID");
                            return;
                        }
                        else
                        {
                            row = dt.Rows[0];

                            this.CurrentDetailData["GarmentDefectTypeID"] = row["GarmentDefectTypeID"];
                            this.CurrentDetailData["GarmentDefectCodeID"] = e.FormattedValue;
                            this.CurrentDetailData["Description"] = row["Description"];
                        }
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };

            #endregion

            #region Area Code 事件

            Ict.Win.DataGridViewGeneratorTextColumnSettings AreaCodeSet = new DataGridViewGeneratorTextColumnSettings();
            AreaCodeSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable dt;
                    string sqlcmd = "SELECT ID , Description FROM CfaArea WITH(NOLOCK)";


                    IList<DataRow> selectedDatas;
                    DualResult Dresult;
                    if (!(Dresult = DBProxy.Current.Select(null, sqlcmd, out dt)))
                    {
                        this.ShowErr(sqlcmd, Dresult);
                        return;
                    }


                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(dt
                            , "ID,Description"
                            , "5,70,3", this.CurrentDetailData["CFAAreaID"].ToString()
                            , "Area Code,Area Desc");

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }

                    selectedDatas = selepoitem.GetSelecteds();

                    this.CurrentDetailData["CFAAreaID"] = selectedDatas[0]["ID"];
                    this.CurrentDetailData["CfaAreaDesc"] = selectedDatas[0]["Description"];

                    this.CurrentDetailData.EndEdit();
                }
            };
            AreaCodeSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;

                if (String.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["CFAAreaID"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["CFAAreaID"] = string.Empty;
                        this.CurrentDetailData["CfaAreaDesc"] = string.Empty;
                    }
                    else
                    {
                        DataTable dt;
                        DataRow row;
                        List<SqlParameter> paras = new List<SqlParameter>();
                        paras.Add(new SqlParameter("@ID", e.FormattedValue));
                        string sqlcmd = $"SELECT ID , Description FROM CfaArea WITH(NOLOCK) WHERE ID = @ID ";

                        DualResult Dresult;
                        if (!(Dresult = DBProxy.Current.Select(null, sqlcmd, paras, out dt)))
                        {
                            this.ShowErr(sqlcmd, Dresult);
                            return;
                        }


                        if (dt == null || dt.Rows.Count == 0)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Area CodeID");
                            return;
                        }
                        else
                        {
                            row = dt.Rows[0];

                            this.CurrentDetailData["CFAAreaID"] = e.FormattedValue;
                            this.CurrentDetailData["CfaAreaDesc"] = row["Description"];
                        }
                    }

                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion


            Ict.Win.DataGridViewGeneratorNumericColumnSettings DefectSet = new DataGridViewGeneratorNumericColumnSettings();

            DefectSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;

                // 總DefectQty
                this.CurrentDetailData["Qty"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();

                this.CauculateSQR();
            };


            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("GarmentDefectTypeID", header: "Defect Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("GarmentDefectCodeid", header: "Defect Code", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: DefectCodeSet)
            .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("CFAAreaID", header: "Area Code", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: AreaCodeSet)
            .Text("CfaAreaDesc", header: "Area Desc", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: false)
            .Numeric("Qty", header: "No.of" + Environment.NewLine + "Defects", width: Widths.AnsiChars(15), decimal_places: 0, integer_places: 10, iseditingreadonly: false, settings: DefectSet)
            .Text("Action", header: "Action", width: Widths.AnsiChars(15), iseditingreadonly: false)
            ;
            #endregion 欄位設定


            #region 可編輯欄位變色
            this.detailgrid.Columns["GarmentDefectCodeid"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["CFAAreaID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Action"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion 可編輯欄位變色
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
SELECT 
		 [GarmentDefectTypeID]=g.GarmentDefectTypeID
        ,b.ID
		,b.GarmentDefectCodeid
		,g.Description
		,b.CFAAreaID
		,[CfaAreaDesc]=Ca.Description
		,b.Remark
		,b.Qty
		,b.Action
FROM CFAInspectionRecord_Detail b
INNER JOIN CFAInspectionRecord a On a.ID = b.ID
LEFT JOIN GarmentDefectCode g ON g.ID = b.GarmentDefectCodeID
LEFT JOIN CfaArea ca ON ca.ID = b.CFAAreaID
WHERE a.ID ='{masterID}'

";

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            string TempId = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CI", "CFAInspectionRecord", DateTime.Now);

            this.CurrentMaintain["ID"] = TempId;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["AuditDate"] = DateTime.Now;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["CFA"] = Sci.Env.User.UserID;
            this.CurrentMaintain["Stage"] = "";
            this.CurrentMaintain["SewingLineID"] = "";
            this.CurrentMaintain["Result"] = "";
            this.CurrentMaintain["Team"] = "";
            this.disInsCtn.Value = 0;

            this.ComboStage_Change(this.CurrentMaintain["Stage"].ToString());

            if (this._sourceHeader != null)
            {
                this.CurrentMaintain["OrderID"] = this._sourceHeader.OrderID;
                this.CurrentMaintain["Seq"] = this._sourceHeader.Seq;
                this.disPO.Value = this._sourceHeader.PO;
                this.disStyle.Value = this._sourceHeader.Style;
                this.disBrand.Value = this._sourceHeader.Brand;
                this.disSeason.Value = this._sourceHeader.Season;
                this.CurrentMaintain["MDivisionid"] = this._sourceHeader.M;
                this.CurrentMaintain["FactoryID"] = this._sourceHeader.Factory;
                this.disOrderQty.Value = this._sourceHeader.OrderQty;
                this.disDest.Value = this._sourceHeader.Dest;
                this.disArticle.Value = this._sourceHeader.Article;
                this.dateBuyerDev.Value = MyUtility.Convert.GetDate(this._sourceHeader.BuyerDev);

            }

            this.Reset_comboStage(this.CurrentMaintain["OrderID"].ToString());
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not modify.");
                return false;
            }
            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            string updateCmd = "";

            if (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString() == "3rd party")
            {
                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='Confirmed',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' 
;
IF NOT EXISTS(
    SELECT 1 FROM Order_QtyShip WHERE ID = '{this.CurrentMaintain["OrderID"]}' AND Seq = '{this.CurrentMaintain["Seq"]}'
    AND  (";
                if (this.CurrentMaintain["Stage"].ToString() == "Final")
                {
                    updateCmd += $@"		CFAFinalInspectResult='Pass' OR CFAFinalInspectResult='Fail but release'  ";
                }
                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    updateCmd += $@"		CFA3rdInspectResult='Pass' OR CFA3rdInspectResult='Fail but release'  ";
                }

                updateCmd += $@"       )
)
BEGIN
    UPDATE Order_QtyShip 
    SET CFAUpdateDate=GETDATE()";
                if (this.CurrentMaintain["Stage"].ToString() == "Final")
                {
                    updateCmd += $@"
    , CFAFinalInspectResult = '{this.CurrentMaintain["Result"].ToString()}'
    , CFAFinalInspectDate = '{MyUtility.Convert.GetDate(this.CurrentMaintain["AuditDate"]).Value.ToString("yyyy/MM/dd")}'
";
                }
                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    updateCmd += $@"
    , CFA3rdInspectResult = '{this.CurrentMaintain["Result"].ToString()}'
    , CFA3rdInspectDate = '{MyUtility.Convert.GetDate(this.CurrentMaintain["AuditDate"]).Value.ToString("yyyy/MM/dd")}'
";

                }

                updateCmd += $@"
    WHERE ID='{this.CurrentMaintain["OrderID"]}'  AND Seq = '{this.CurrentMaintain["Seq"]}'
END
";
            }
            else
            {

                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='Confirmed',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' 

UPDATE Order_QtyShip 
SET CFAUpdateDate=GETDATE()
WHERE ID='{this.CurrentMaintain["OrderID"]}'  AND Seq = '{this.CurrentMaintain["Seq"]}'
";
                
            }


            DualResult r;
            r = DBProxy.Current.Execute(null, updateCmd);
            if (!r)
            {
                this.ShowErr(r);
            }

            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            string updateCmd = "";
            if (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString() == "3rd party")
            {

                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='New',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' ";

                updateCmd += $@"
IF NOT EXISTS(
    SELECT * 
    FROM CFAInspectionRecord
    WHERE (Result = 'Pass' OR Result='Fail but release')	
    AND OrderID='{this.CurrentMaintain["OrderID"].ToString()}'
	AND Seq='{this.CurrentMaintain["Seq"].ToString()}'
    AND Stage='{this.CurrentMaintain["Stage"].ToString()}'
    AND ID != '{this.CurrentMaintain["ID"].ToString()}'
)
BEGIN
";
                updateCmd += $@"
    SELECT TOP 1 * 
	INTO #LastFail
    FROM CFAInspectionRecord
    WHERE Result='Fail'
    AND OrderID='{this.CurrentMaintain["OrderID"].ToString()}'
	AND Seq='{this.CurrentMaintain["Seq"].ToString()}'
    AND Stage='{this.CurrentMaintain["Stage"].ToString()}'
	AND Status = 'Confirmed'
    AND ID != '{this.CurrentMaintain["ID"].ToString()}'
	ORDER BY EditDate DESC

    UPDATE Order_QtyShip 
    SET CFAUpdateDate = GETDATE()";

                if (this.CurrentMaintain["Stage"].ToString() == "Final")
                {
                    updateCmd += $@"
        , CFAFinalInspectResult = (SELECT Result FROM #LastFail)
        , CFAFinalInspectDate =  IIF((SELECT EditDate FROM #LastFail)='',NULL,(SELECT EditDate FROM #LastFail))
";
                }
                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    updateCmd += $@"
        , CFA3rdInspectResult = (SELECT Result FROM #LastFail)
        , CFA3rdInspectDate =  IIF((SELECT EditDate FROM #LastFail)='',NULL,(SELECT EditDate FROM #LastFail))
";

                }

                updateCmd += $@"    WHERE ID='{this.CurrentMaintain["OrderID"]}'  AND Seq = '{this.CurrentMaintain["Seq"]}'";
                updateCmd += $@"
END";

            }
            else
            {

                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='New',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' 

UPDATE Order_QtyShip 
SET CFAUpdateDate=GETDATE()
WHERE ID='{this.CurrentMaintain["OrderID"]}'  AND Seq = '{this.CurrentMaintain["Seq"]}'
";

            }
            DualResult r;
            r = DBProxy.Current.Execute(null, updateCmd);
            if (!r)
            {
                this.ShowErr(r);
            }


            base.ClickUnconfirm();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["CFA"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Seq"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["AuditDate"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Stage"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Result"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Team"])
                )
            {
                MyUtility.Msg.WarningBox("CFA、SP & SEQ、Audit Date、Inspection stage、Inspection result、Team can't be all empty!!");
                return false;
            }

            if (this.CurrentMaintain["Stage"].ToString() == "Staggered" && (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]) || MyUtility.Check.Empty(this.CurrentMaintain["Shift"])))
            {
                MyUtility.Msg.WarningBox("Line、Shift can't be empty!!");
                return false;
            }
            // Defect Code、Area Code、No. of Defects不得為空，並跳出警告視窗
            bool anyEmpty = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).Where(o => MyUtility.Check.Empty(o["GarmentDefectCodeID"]) || MyUtility.Check.Empty(o["CFAAreaID"]) || MyUtility.Check.Empty(o["Qty"])).Any();

            if (anyEmpty)
            {
                MyUtility.Msg.WarningBox("Defect Code、Area Code、No. of Defects can't be empty!!");
                return false;
            }

            List<string> Cartons = this.CurrentMaintain["Carton"].ToString().Split(',').ToList();


            if (Cartons.Where(o => !MyUtility.Check.Empty(o)).Count() == 0 && this.CurrentMaintain["Stage"].ToString() == "Staggered")
            {
                MyUtility.Msg.WarningBox("Inspected Carton can't be empty!!");
                return false;
            }

            string cmd = $@"
SELECT 1
FROM PackingList_Detail
WHERE OrderID = '{this.CurrentMaintain["OrderID"]}'
AND OrderShipmodeSeq ='{this.CurrentMaintain["Seq"]}'
AND CTNStartNo IN ('{Cartons.JoinToString("','")}')
AND StaggeredCFAInspectionRecordID <> '{this.CurrentMaintain["ID"]}'
AND StaggeredCFAInspectionRecordID <> ''
";

            bool duplicate = MyUtility.Check.Seek(cmd);

            if (duplicate)
            {
                MyUtility.Msg.WarningBox("Carton already exists CFA Inspection Record!!");
                return false;
            }

            // 
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["DefectQty"]) > MyUtility.Convert.GetInt(this.CurrentMaintain["InspectQty"]))
            {
                MyUtility.Msg.WarningBox("Defects Qty can't more than Inspect Qty!!");
                return false;

            }

            // 
            if (MyUtility.Convert.GetInt(this.CurrentMaintain["InspectQty"]) > MyUtility.Convert.GetInt(this.disOrderQty.Value))
            {
                MyUtility.Msg.WarningBox("Inspect Qty can't more than Order Qty!!");
                return false;

            }
            //
            if ((this.CurrentMaintain["Stage"].ToString().ToLower() == "final" || this.CurrentMaintain["Stage"].ToString().ToLower() == "3rd party") && (this.CurrentMaintain["Result"].ToString().ToLower() == "pass" || this.CurrentMaintain["Result"].ToString().ToLower() == "fail but release"))
            {
                bool hasSameSpSeq = MyUtility.Check.Seek($@"
SELECT 1 
FROM CFAInspectionRecord WITH(NOLOCK)
WHERE ID <> '{this.CurrentMaintain["ID"]}'
AND OrderID = '{this.CurrentMaintain["OrderID"]}'
AND Seq = '{this.CurrentMaintain["Seq"]}'
AND Stage='{this.CurrentMaintain["Stage"].ToString()}'
AND (Result='Pass' OR Result='Fail but release')
");
                if (hasSameSpSeq)
                {
                    MyUtility.Msg.InfoBox("This SP# and Seq has passed or released.");
                    return false;
                }
            }
            //
            if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
            {
                bool CFAIs3rdInspect = MyUtility.Check.Seek($@"
SELECT 1
FROM Order_QtyShip WITH(NOLOCK)
WHERE ID = '{this.CurrentMaintain["OrderID"]}'
AND Seq = '{this.CurrentMaintain["Seq"]}'
AND CFAIs3rdInspect = 1
");
                if (!CFAIs3rdInspect)
                {
                    MyUtility.Msg.InfoBox($"SP#:{this.CurrentMaintain["OrderID"]} 、Sep:{this.CurrentMaintain["Seq"]}   is not 3rd Inspect.");
                    return false;
                }
            }
            //Clog received %(By piece) 計算
            this.CurrentMaintain["ClogReceivedPercentage"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($@"
SELECT  CAST(ROUND( SUM(IIF( CFAReceiveDate IS NOT NULL OR ReceiveDate IS NOT NULL
								,ShipQty
								,0)
						) * 1.0 
						/  SUM(ShipQty) * 100 
,0) AS INT) 
FROM PackingList_Detail WITH(NOLOCK)
where OrderID = '{this.CurrentMaintain["OrderID"]}' AND OrderShipmodeSeq = '{this.CurrentMaintain["Seq"]}'
"));


            bool IsSameM = MyUtility.Check.Seek($"SELECT 1 FROM Orders WHERE ID='{this.CurrentMaintain["OrderID"]}' AND MDivisionID = '{Sci.Env.User.Keyword}'");
            if (!IsSameM)
            {
                MyUtility.Msg.InfoBox("MDivisionID is different!!");
                return false;
            }
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            // Inspection stage為Staggered且Inspection result Fail不需要將檢驗的箱號回寫PackingList_Detail.StaggeredCFAInspectionRecordID

            List<string> Cartons = this.CurrentMaintain["Carton"].ToString().Split(',').ToList();
            string cmd = string.Empty;

            if (this.CurrentMaintain["Stage"].ToString() == "Staggered" && this.CurrentMaintain["Result"].ToString() == "Fail")
            {
                cmd = $@"
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID=''
WHERE OrderID = '{this.CurrentMaintain["OrderID"]}'
AND OrderShipmodeSeq ='{this.CurrentMaintain["Seq"]}' 
AND StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}'
";
            }
            else
            {
                cmd = $@"
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID=''
WHERE OrderID = '{this.CurrentMaintain["OrderID"]}'
AND OrderShipmodeSeq ='{this.CurrentMaintain["Seq"]}' 
AND StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}'
;
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}'
WHERE OrderID = '{this.CurrentMaintain["OrderID"]}'
    AND OrderShipmodeSeq ='{this.CurrentMaintain["Seq"]}' 
    AND CTNStartNo IN ('{Cartons.JoinToString("','")}')
    AND StaggeredCFAInspectionRecordID = ''
";

            }

            DualResult result = DBProxy.Current.Execute(null, cmd);

            if (!result)
            {
                this.ShowErr(result);
                return result;
            }
                    
            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            string updateCmd = $@"UPDATE PackingList_Detail SET StaggeredCFAInspectionRecordID = '' WHERE StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' ";
            DBProxy.Current.Execute(null, updateCmd);
            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (this._Type == "1")
            {
                if (this.tabs.SelectedIndex != 0)
                {
                    bool canConfrim = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanConfirm");
                    bool canUnConfrim = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanUnConfirm");
                    bool canNew = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanNew");
                    bool canEdit = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanEdit");
                    bool canDelete = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanDelete");


                    this.toolbar.cmdNew.Enabled = !this.EditMode  && canNew;
                    this.toolbar.cmdEdit.Enabled = !this.EditMode &&  canEdit;
                    this.toolbar.cmdDelete.Enabled = !this.EditMode && canDelete;

                    this.toolbar.cmdConfirm.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "New" && canConfrim;
                    this.toolbar.cmdUnconfirm.Enabled = !this.EditMode && this.CurrentMaintain != null && MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed" && canUnConfrim;

                    this.toolbar.cmdConfirm.Visible = true;
                    this.toolbar.cmdUnconfirm.Visible = true;
                }
                else
                {
                    this.toolbar.cmdConfirm.Enabled = false;
                    this.toolbar.cmdSave.Enabled = false;
                }
            }
            else
            {

                this.toolbar.cmdConfirm.Enabled = false;
                this.toolbar.cmdUnconfirm.Enabled = false;


                this.toolbar.cmdConfirm.Visible = true;
                this.toolbar.cmdUnconfirm.Visible = true;
            }
        }

        private void ComboStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._oldStage = this.CurrentMaintain["Stage"].ToString();
            this.CurrentMaintain["Stage"] = this.comboStage.SelectedItem.ToString();
            this.ComboStage_Change(this.comboStage.SelectedItem.ToString());
            if (this._oldStage != this.CurrentMaintain["Stage"].ToString())
            {
                this.CurrentMaintain["Result"] = string.Empty;
                this._oldStage = this.CurrentMaintain["Stage"].ToString();
            }
            this.CalInsepectionCtn(this.IsDetailInserting);
        }

        private void TxtSpSeq_Leave(object sender, EventArgs e)
        {
            string newOrderID = this.txtSpSeq.TextBoxSPBinding;
            string newSeq = this.txtSpSeq.TextBoxSeqBinding;
            if (this.CurrentMaintain != null)
            {
                string oldOrderID = this.CurrentMaintain["OrderID"].ToString();
                string oldSeq = this.CurrentMaintain["Seq"].ToString();

                if ((newOrderID != oldOrderID) || (newSeq != oldSeq))
                {
                    this.CurrentMaintain["OrderID"] = newOrderID;
                    this.CurrentMaintain["Seq"] = newSeq;
                    this.AutoInsertBySP(newOrderID, newSeq);

                    this.CurrentMaintain["Stage"] = string.Empty;
                    this.CurrentMaintain["Result"] = string.Empty;
                    this.CurrentMaintain["Carton"] = string.Empty;
                }

                // 開始計算檢驗次數
                this.CalInsepectionCtn(this.IsDetailInserting);
            }
            bool IsSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{this.CurrentMaintain["OrderID"].ToString()}' "));

            if (IsSample && this.comboStage.Items.Contains("Staggered"))
            {
                this.comboStage.Items.RemoveAt(2);
            }
            else if(!IsSample)
            {
                this.comboStage.Items.Clear();
                this.comboStage.Items.AddRange(new object[] {
            "",
            "Inline",
            "Staggered",
            "Final",
            "3rd party"});
            }

        }

        private void numInspectQty_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CurrentMaintain["InspectQty"] = this.numInspectQty.Value;
            this.CauculateSQR();
        }

        private void comboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain != null)
            {
                //this.CurrentMaintain["Team"] = this.comboTeam.SelectedItem.ToString();
            }
        }

        private void comboResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain != null)
            {
                //this.CurrentMaintain["Result"] = this.comboResult.SelectedItem.ToString();
            }

        }

        private void txtSewingLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                string sqlCmd = $"SELECT DISTINCT ID FROM SewingLine WITH(NOLOCK) WHERE FactoryID = '{Sci.Env.User.Factory}'";
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(sqlCmd, "ID", "10", this.txtSewingLine.Text, null, null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.txtSewingLine.Text = item.GetSelectedString();
                    this.CurrentMaintain["SewingLineID"] = item.GetSelectedString();
                }
            }
        }

        private void txtSewingLine_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.EditMode && this.txtSewingLine.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                List<string> Lines = this.txtSewingLine.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Distinct().ToList();
                List<string> errorLines = new List<string>();
                foreach (var Line in Lines)
                {
                    DataTable dt;
                    string sqlCmd = $"SELECT 1 FROM SewingLine WITH(NOLOCK) WHERE FactoryID = '{Sci.Env.User.Factory}' AND ID = @ID";
                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@ID", Line));

                    DualResult r = DBProxy.Current.Select(null, sqlCmd, paras, out dt);
                    if (!r)
                    {
                        this.ShowErr(r);
                    }
                    else
                    {
                        if (dt.Rows.Count == 0)
                        {
                            errorLines.Add(Line);
                        }
                    }

                }
                if (errorLines.Count > 0)
                {
                    MyUtility.Msg.WarningBox($"Sewing Line NOT Found : " + errorLines.JoinToString(","));
                    this.CurrentMaintain["SewingLineID"] = string.Empty;
                }
                else
                {
                    this.CurrentMaintain["SewingLineID"] = Lines.JoinToString(",");
                }

            }


            if (!this.txtSewingLine.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                this.CurrentMaintain["SewingLineID"] = string.Empty;
            }
        }

        private void txtInspectedCarton_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@OrderID", this.CurrentMaintain["OrderID"].ToString()));
                paras.Add(new SqlParameter("@Seq", this.CurrentMaintain["Seq"].ToString()));

                string sqlCmd = $@"

----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
AND (pd.StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' OR pd.StaggeredCFAInspectionRecordID = '')
GROUP BY ID,OrderID,OrderShipmodeSeq,CTNStartNo
HAVING COUNT(DISTINCT Article) > 1 OR COUNT(DISTINCT SizeCode) > 1


SELECT * FROM (
    ----不是混尺碼的正常做
	SELECT [CTN#]=CTNStartNo
		,Article 
		,[Size]=SizeCode 
		,[Qty]=SUM(ShipQty) 
	FROM PackingList_Detail pd
	WHERE pd.OrderID= @OrderID
	AND OrderShipmodeSeq =  @Seq
	AND (pd.StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' OR pd.StaggeredCFAInspectionRecordID = '')
	AND NOT EXISTS(
		SELECT  *  
		FROM #MixCTNStartNo t 
		WHERE t.ID = pd.ID AND t.OrderID = pd.OrderID 
		AND t.OrderShipmodeSeq=pd.OrderShipmodeSeq AND t.CTNStartNo=pd.CTNStartNo
	)
	GROUP BY CTNStartNo,Article ,SizeCode
	UNION
    ----混尺碼分開處理
	SELECt [CTN#]=t.CTNStartNo
		,[Article]=MixArticle.Val 
		,[Size]=MixSizeCode.Val
		,[Qty]=ShipQty.Val
	FROM #MixCTNStartNo t
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+Article  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
			AND (pd.StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' OR pd.StaggeredCFAInspectionRecordID = '')
		FOR XML PATH(''))
		,1,1,'')
	)MixArticle
	OUTER APPLY(
		SELECT  [Val]=  STUFF((
			SELECT DISTINCT ','+SizeCode  
			FROM PackingList_Detail pd
			WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
			AND (pd.StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' OR pd.StaggeredCFAInspectionRecordID = '')
		FOR XML PATH(''))
		,1,1,'')
	)MixSizeCode
	OUTER APPLY(
		SELECT  [Val]=SUM(pd.ShipQty)
		FROM PackingList_Detail pd
		WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
		    AND (pd.StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' OR pd.StaggeredCFAInspectionRecordID = '')
	)ShipQty
) a
ORDER BY Cast([CTN#] as int)


DROP TABLE #MixCTNStartNo 

";
                DataTable dt;
                DBProxy.Current.Select(null, sqlCmd, paras, out dt);
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(dt, "CTN#,Article,Size,Qty", "CTN#,Article,Size,Qty", "3,15,20,5", this.CurrentMaintain["Carton"].ToString(), null, null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.txtInspectedCarton.Text = item.GetSelectedString();
                    this.CurrentMaintain["Carton"] = item.GetSelectedString();
                }
            }
        }

        private void TxtInspectedCarton_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.EditMode && this.txtInspectedCarton.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {

                List<string> Cartons = this.txtInspectedCarton.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Distinct().ToList();
                List<string> errorCartons = new List<string>();

                foreach (var Carton in Cartons)
                {

                    DataTable dt;
                    string sqlCmd = $@"
SELECT * 
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
AND CTNStartNo = @CTNStartNo
AND (StaggeredCFAInspectionRecordID = @ID OR StaggeredCFAInspectionRecordID = '')

";
                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@OrderID", this.CurrentMaintain["OrderID"].ToString()));
                    paras.Add(new SqlParameter("@Seq", this.CurrentMaintain["Seq"].ToString()));
                    paras.Add(new SqlParameter("@CTNStartNo", Carton));
                    paras.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()));

                    DualResult r = DBProxy.Current.Select(null, sqlCmd, paras, out dt);
                    if (!r)
                    {
                        this.ShowErr(r);
                    }
                    else
                    {
                        if (dt.Rows.Count == 0)
                        {
                            errorCartons.Add(Carton);
                        }
                    }
                }

                if (errorCartons.Count > 0)
                {
                    MyUtility.Msg.WarningBox($"CTN# NOT Found : " + errorCartons.JoinToString(","));
                    this.CurrentMaintain["Carton"] = string.Empty;
                }
                else
                {
                    this.CurrentMaintain["Carton"] = Cartons.JoinToString(",");
                }
            }

            if (!this.txtInspectedCarton.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                this.CurrentMaintain["Carton"] = string.Empty;
            }
        }

        private void ComboStage_Change(string stage)
        {
            if (!this.EditMode)
            {
                this.txtInspectedCarton.IsSupportEditMode = false;
                this.txtInspectedCarton.ReadOnly = true;

                this.txtshift.IsSupportEditMode = false;
                this.txtshift.ReadOnly = true;
                return;
            }

            // 只有選擇Staggered時Inspected Carton、Shift才可以欄位才可以編輯，選到其他Stage時請一併清除這些欄位資料。
            if (stage == "Staggered")
            {
                this.txtInspectedCarton.IsSupportEditMode = true;
                this.txtInspectedCarton.ReadOnly = false;

                this.txtshift.IsSupportEditMode = true;
                this.txtshift.ReadOnly = false;
            }
            else
            {
                this.CurrentMaintain["Carton"] = string.Empty;
                this.txtInspectedCarton.Text = string.Empty;
                this.txtInspectedCarton.IsSupportEditMode = false;
                this.txtInspectedCarton.ReadOnly = true;

                this.CurrentMaintain["Shift"] = string.Empty;
                this.txtshift.Text = string.Empty;
                this.txtshift.IsSupportEditMode = false;
                this.txtshift.ReadOnly = true;
            }

            // Stage若是3rd party則要檢查Order_QtyShip.CFAIs3rdInspect是否為1，若不是則不能存檔，並跳出警告視窗
            if (stage == "3rd party" )
            {
                bool CFAIs3rdInspect = MyUtility.Check.Seek($@"
SELECT 1
FROM Order_QtyShip WITH(NOLOCK)
WHERE ID = '{this.CurrentMaintain["OrderID"]}'
AND Seq = '{this.CurrentMaintain["Seq"]}'
AND CFAIs3rdInspect = 1
");
                if (!CFAIs3rdInspect)
                {
                    MyUtility.Msg.InfoBox($"SP#:{this.CurrentMaintain["OrderID"]} 、Sep:{this.CurrentMaintain["Seq"]}   is not 3rd Inspect.");

                    // 不能存檔還留著的話太蠢，直接清空
                    this.comboStage.SelectedIndex = 0;
                    this.CurrentMaintain["Stage"] = string.Empty;
                }

            }

            // Final的時候Inspection result才能有Fail but release
            if (stage == "Final")
            {
                this.Reset_comboResult(true);
            }
            else
            {
                this.Reset_comboResult(false);
            }
        }

        private void CauculateSQR()
        {
            // 總DefectQty
            decimal totalDefectQty = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).Select(o => new { Qty = MyUtility.Convert.GetDecimal(o["Qty"]) }).ToList().Sum(o => o.Qty);

            this.CurrentMaintain["DefectQty"] = totalDefectQty;

            decimal InspectQty = MyUtility.Convert.GetDecimal(this.CurrentMaintain["InspectQty"]);

            // 計算SQR
            if (InspectQty == 0)
            {
                this.numSQR.Value = null;
                return;
            }

            decimal SQR = (totalDefectQty / InspectQty * 100);
            this.numSQR.Value = SQR;

        }

        private void AutoInsertBySP(string orderID, string seq)
        {

            this.disSeason.Value = MyUtility.GetValue.Lookup($@"
SELECT  SeasonID
FROM Orders  WITH(NOLOCK)
WHERE ID = '{orderID}'
");

            this.disPO.Value = MyUtility.GetValue.Lookup($@"
SELECT  CustPoNo
FROM Orders  WITH(NOLOCK)
WHERE ID = '{orderID}'
");
            this.disStyle.Value = MyUtility.GetValue.Lookup($@"
SELECT  StyleID
FROM Orders  WITH(NOLOCK)
WHERE ID = '{orderID}'
");

            this.disBrand.Value = MyUtility.GetValue.Lookup($@"
SELECT  BrandID
FROM Orders  WITH(NOLOCK)
WHERE ID = '{orderID}'
");

            this.dateBuyerDev.Value = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($@"
SELECT BuyerDelivery
FROM Order_QtyShip  WITH(NOLOCK)
WHERE ID = '{orderID}' AND Seq ='{seq}'
"));

            this.disOrderQty.Value = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($@"
SELECT Qty
FROM Order_QtyShip  WITH(NOLOCK)
WHERE ID = '{orderID}' AND Seq ='{seq}'
"));


            this.disDest.Value = MyUtility.GetValue.Lookup($@"
SELECT c.Alias
FROM Orders o
INNER JOIN Country c ON o.Dest = c.ID
WHERE o.ID = '{orderID}'
");

            this.disArticle.Value = MyUtility.GetValue.Lookup($@"
SELECT STUFF(
    (SELECT DISTINCT ','+Article 
    FROM Order_QtyShip_Detail 
    WHERE ID = '{orderID}' AND Seq = '{seq}'
    FOR XML PATH('')
    )
,1,1,'')
");
        }

        private void Reset_comboStage(string orderID)
        {
            bool IsSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{orderID}' "));

            this.comboStage.Items.Clear();
            this.comboStage.Items.AddRange(new object[] {
            "",
            "Inline",
            "Staggered",
            "Final",
            "3rd party"});
            if (IsSample)
            {
                this.comboStage.Items.RemoveAt(2);
            }

            this.comboStage.SelectedItem = this.CurrentMaintain["Stage"].ToString();
        }

        private void Reset_comboResult(bool isFinal = false)
        {
            this.comboResult.Items.Clear();

            // Final的時候Inspection result才能有Fail but release
            if (!isFinal)
            {
                this.comboResult.Items.AddRange(new object[] {
                    "",
                    "Pass",
                    "Fail"});
            }
            else
            {
                this.comboResult.Items.AddRange(new object[] {
                    "",
                    "Pass",
                    "Fail",
                    "Fail but release"});
            }

            this.comboResult.SelectedItem = this.CurrentMaintain["Result"].ToString();
        }

        private void CalInsepectionCtn(bool isClickNew)
        {
            string cmd = string.Empty;

            // 必須條件
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]) || MyUtility.Check.Empty(this.CurrentMaintain["SEQ"]) || MyUtility.Check.Empty(this.CurrentMaintain["Stage"]) || MyUtility.Check.Empty(this.CurrentMaintain["AuditDate"]))
            {
                this.disInsCtn.Value = 0;
                return;
            }

            // Final和3rd才需要計算
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Stage"]) != "Final" && MyUtility.Convert.GetString(this.CurrentMaintain["Stage"]) != "3rd party")
            {
                this.disInsCtn.Value = 0;
                return;
            }
                cmd = $@"
SELECT COUNT(1) + 1
FROM CFAInspectionRecord
WHERE OrderID='{this.CurrentMaintain["OrderID"]}' AND SEQ='{this.CurrentMaintain["Seq"]}'
AND Status = 'Confirmed'
AND Stage='{this.CurrentMaintain["Stage"]}'
AND AuditDate <= '{MyUtility.Convert.GetDate(this.CurrentMaintain["AuditDate"]).Value.ToString("yyyy/MM/dd")}'
AND ID  != '{this.CurrentMaintain["ID"]}'
";

            this.disInsCtn.Value = MyUtility.GetValue.Lookup(cmd);
        }
    }

    public class P32Header
    {
        public string OrderID { get; set; }
        public string Seq { get; set; }
        public string PO { get; set; }
        public string Style { get; set; }
        public string Brand { get; set; }
        public string Season { get; set; }
        public string M { get; set; }
        public string Factory { get; set; }
        public string BuyerDev { get; set; }
        public string OrderQty { get; set; }
        public string Dest { get; set; }
        public string Article { get; set; }
    }
}
