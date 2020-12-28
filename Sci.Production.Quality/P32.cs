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
        private readonly string _Type = string.Empty;
        private readonly P32Header _sourceHeader = new P32Header();
        private string _oldStage = string.Empty;
        private bool _canConfirm = false;
        private bool IsSapmle = false;
        private string topOrderID = string.Empty;
        private string topSeq = string.Empty;
        private string topCarton = string.Empty;
        private DataTable CFAInspectionRecord_OrderSEQ;

        /// <inheritdoc/>
        public P32(ToolStripMenuItem menuitem, string type)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Text = type == "1" ? "P32. CFA Inspection Record " : "P321. CFA Inspection Record (History)";
            this._Type = type;
            this._sourceHeader = null;

            string isfinished = type == "1" ? "0" : "1";
            string defaultFilter = $"EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE Ftygroup='{Sci.Env.User.Factory}' AND Finished = {isfinished} AND ID IN (SELECT OrderID FROM CFAInspectionRecord_OrderSEQ WHERE ID IN (CFAInspectionRecord.ID)))";
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

            string isfinished = type == "1" ? "0" : "1";
            string defaultFilter = $"EXISTS (SELECT 1 FROM Orders WITH (NOLOCK) WHERE Ftygroup='{Sci.Env.User.Factory}' AND Finished = {isfinished} AND ID IN (SELECT OrderID FROM CFAInspectionRecord_OrderSEQ WHERE ID IN (CFAInspectionRecord.ID)))";
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

            DataTable dt_GridSpSeq;
            string cmd = $@"SELECT * FROM CFAInspectionRecord_OrderSEQ WHERE ID='{this.CurrentMaintain["ID"]}' ORDER BY Ukey";
            DBProxy.Current.Select(null, cmd, out dt_GridSpSeq);

            this.CFAInspectionRecord_OrderSEQ = dt_GridSpSeq;
            this.topOrderID = dt_GridSpSeq != null && dt_GridSpSeq.Rows.Count > 0 ? MyUtility.Convert.GetString(dt_GridSpSeq.AsEnumerable().FirstOrDefault()["OrderID"]) : string.Empty;
            this.topSeq = dt_GridSpSeq != null && dt_GridSpSeq.Rows.Count > 0 ? MyUtility.Convert.GetString(dt_GridSpSeq.AsEnumerable().FirstOrDefault()["Seq"]) : string.Empty;

            this.AutoInsertBySP(this.topOrderID, this.topSeq);

            this.CauculateSQR();

            if (!MyUtility.Check.Empty(this.CurrentMaintain))
            {
                this.Reset_comboStage(this.topOrderID, MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]));
            }
            else
            {
                this.Reset_comboStage(this.topOrderID);
            }

            this.ComboStage_Change(this.CurrentMaintain["Stage"].ToString());

            this.CalInsepectionCtn(this.IsDetailInserting, MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]));

            this.txtSpSeq.TextBoxSPBinding = this.topOrderID;

            this.txtSpSeq.TextBoxSeqBinding = this.topSeq;

            this.btnSettingSpSeq.Enabled = this.EditMode && MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]);

            bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{this.topOrderID}' "));
            this.IsSapmle = isSample;

            #region txtInspectedCarton
            this.txtInspectedCarton.Text = string.Empty;
            this.topCarton = string.Empty;

            // 不是CombinePO才需要帶出這個值
            if (!MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]))
            {
                string carton = MyUtility.GetValue.Lookup($@"SELECT TOP 1 Carton FROM CFAInspectionRecord_OrderSEQ WHERE ID = '{this.CurrentMaintain["ID"]}' ");
                this.txtInspectedCarton.Text = carton;
                this.topCarton = carton;
            }

            #endregion

            #region -- Grid欄位設定 --
            this.gridSpSeq.DataSource = null;
            this.gridSpSeq.DataSource = dt_GridSpSeq;
            #endregion 欄位設定
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailGridSetup()
        {
            #region Defect Code 事件

            Ict.Win.DataGridViewGeneratorTextColumnSettings defectCodeSet = new DataGridViewGeneratorTextColumnSettings();
            defectCodeSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable dt;
                    string sqlcmd = "SELECT ID , Description ,GarmentDefectTypeID FROM GarmentDefectCode WITH(NOLOCK)";

                    IList<DataRow> selectedDatas;
                    DualResult dresult;
                    if (!(dresult = DBProxy.Current.Select(null, sqlcmd, out dt)))
                    {
                        this.ShowErr(sqlcmd, dresult);
                        return;
                    }

                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        dt,
                        "ID,Description,GarmentDefectTypeID",
                        "5,70,3", this.CurrentDetailData["GarmentDefectCodeID"].ToString(),
                        "Defect Code,Description,Type");

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    selectedDatas = selepoitem.GetSelecteds();

                    this.CurrentDetailData["GarmentDefectTypeID"] = selectedDatas[0]["GarmentDefectTypeID"];
                    this.CurrentDetailData["GarmentDefectCodeID"] = selectedDatas[0]["ID"];
                    this.CurrentDetailData["Description"] = selectedDatas[0]["Description"];

                    this.CurrentDetailData.EndEdit();
                }
            };

            defectCodeSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["GarmentDefectCodeID"].ToString()) != 0)
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

                        DualResult dresult;
                        if (!(dresult = DBProxy.Current.Select(null, sqlcmd, paras, out dt)))
                        {
                            this.ShowErr(sqlcmd, dresult);
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

            Ict.Win.DataGridViewGeneratorTextColumnSettings areaCodeSet = new DataGridViewGeneratorTextColumnSettings();
            areaCodeSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable dt;
                    string sqlcmd = "SELECT ID , Description FROM CfaArea WITH(NOLOCK)";

                    IList<DataRow> selectedDatas;
                    DualResult dresult;
                    if (!(dresult = DBProxy.Current.Select(null, sqlcmd, out dt)))
                    {
                        this.ShowErr(sqlcmd, dresult);
                        return;
                    }

                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(
                        dt,
                        "ID,Description",
                        "5,70,3",
                        this.CurrentDetailData["CFAAreaID"].ToString(),
                        "Area Code,Area Desc");

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    selectedDatas = selepoitem.GetSelecteds();

                    this.CurrentDetailData["CFAAreaID"] = selectedDatas[0]["ID"];
                    this.CurrentDetailData["CfaAreaDesc"] = selectedDatas[0]["Description"];

                    this.CurrentDetailData.EndEdit();
                }
            };
            areaCodeSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["CFAAreaID"].ToString()) != 0)
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

                        DualResult dresult;
                        if (!(dresult = DBProxy.Current.Select(null, sqlcmd, paras, out dt)))
                        {
                            this.ShowErr(sqlcmd, dresult);
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

            Ict.Win.DataGridViewGeneratorNumericColumnSettings defectSet = new DataGridViewGeneratorNumericColumnSettings();

            defectSet.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                // 總DefectQty
                this.CurrentDetailData["Qty"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();

                this.CauculateSQR();
            };

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("GarmentDefectTypeID", header: "Defect Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("GarmentDefectCodeid", header: "Defect Code", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: defectCodeSet)
            .Text("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("CFAAreaID", header: "Area Code", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: areaCodeSet)
            .Text("CfaAreaDesc", header: "Area Desc", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(15), iseditingreadonly: false)
            .Numeric("Qty", header: "No.of" + Environment.NewLine + "Defects", width: Widths.AnsiChars(15), decimal_places: 0, integer_places: 10, iseditingreadonly: false, settings: defectSet)
            .Text("Action", header: "Action", width: Widths.AnsiChars(15), iseditingreadonly: false)
            ;
            #endregion 欄位設定

            this.Helper.Controls.Grid.Generator(this.gridSpSeq)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("Carton", header: "Inspected Carton", width: Widths.AnsiChars(25), iseditingreadonly: true)
            ;

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
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

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
            try
            {
                base.ClickNewAfter();
                this.CurrentMaintain["Status"] = "New";
                this.CurrentMaintain["AuditDate"] = DateTime.Now;
                this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
                this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
                this.CurrentMaintain["CFA"] = Sci.Env.User.UserID;
                this.CurrentMaintain["Stage"] = string.Empty;
                this.CurrentMaintain["SewingLineID"] = string.Empty;
                this.CurrentMaintain["Result"] = string.Empty;
                this.CurrentMaintain["Team"] = string.Empty;
                this.CurrentMaintain["IsCombinePO"] = false;
                //this.disInsCtn.Value = 0;

                this.ComboStage_Change(this.CurrentMaintain["Stage"].ToString());

                this.txtSpSeq.TextBoxSP.ReadOnly = false;
                this.txtSpSeq.TextBoxSeq.ReadOnly = false;

                this.txtSpSeq.TextBoxSP.IsSupportEditMode = true;
                this.txtSpSeq.TextBoxSeq.IsSupportEditMode = true;

                if (this._sourceHeader != null)
                {
                    this.topOrderID = this._sourceHeader.OrderID;
                    this.topSeq = this._sourceHeader.Seq;
                    this.disPO.Value = this._sourceHeader.PO;
                    this.disStyle.Value = this._sourceHeader.Style;
                    this.disBrand.Value = this._sourceHeader.Brand;
                    this.disSeason.Value = this._sourceHeader.Season;
                    this.CurrentMaintain["MDivisionid"] = this._sourceHeader.M;
                    this.disOrderQty.Value = this._sourceHeader.OrderQty;
                    this.disDest.Value = this._sourceHeader.Dest;
                    this.disArticle.Value = this._sourceHeader.Article;
                    this.dateBuyerDev.Value = MyUtility.Convert.GetDate(this._sourceHeader.BuyerDev);

                    bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{this.topOrderID}' "));
                    this.IsSapmle = isSample;

                    DataRow nRow = this.CFAInspectionRecord_OrderSEQ.NewRow();
                    nRow["OrderID"] = this.topOrderID;
                    nRow["Seq"] = this.topSeq;

                    this.txtSpSeq.TextBoxSPBinding = this.topOrderID;

                    this.txtSpSeq.TextBoxSeqBinding = this.topSeq;
                    this.CFAInspectionRecord_OrderSEQ.Rows.Add(nRow);
                }

                this.Reset_comboStage(this.topOrderID);
                this.btnSettingSpSeq.Enabled = false;
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
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
        protected override bool ClickCopy()
        {
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["AuditDate"] = DateTime.Now;
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["CFA"] = Sci.Env.User.UserID;
            this.CurrentMaintain["FirstInspection"] = false;
            return base.ClickCopy();
        }

        /// <inheritdoc/>
        protected override bool ClickCopyBefore()
        {
            return base.ClickCopyBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            string updateCmd = string.Empty;

            List<string> tmp = new List<string>();
            List<string> tmpOrder_QtyShip = new List<string>();
            foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
            {
                tmp.Add($@" (b.OrderID = '{dr["OrderID"]}' AND b.Seq = '{dr["Seq"]}') ");
                tmpOrder_QtyShip.Add($@" (ID = '{dr["OrderID"]}' AND Seq = '{dr["Seq"]}') ");
            }

            if (!tmp.Any())
            {
                tmp.Add("1=0");
                tmpOrder_QtyShip.Add("1=0");
            }

            if (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString() == "3rd party")
            {
                string chkHasPass = $@"
SELECT 1 FROM Order_QtyShip WHERE ({tmpOrder_QtyShip.JoinToString(" OR ")})
AND  (";
                if (this.CurrentMaintain["Stage"].ToString() == "Final")
                {
                    chkHasPass += $@"		CFAFinalInspectResult='Pass' OR CFAFinalInspectResult='Fail but release'  ";
                }

                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    chkHasPass += $@"		CFA3rdInspectResult='Pass' OR CFA3rdInspectResult='Fail but release'  ";
                }

                chkHasPass += $@"       )";

                if (MyUtility.Check.Seek(chkHasPass))
                {
                    MyUtility.Msg.WarningBox("SP# has Pass record, can't confirm.");
                    return;
                }

                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='Confirmed',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' 
;";

                updateCmd += $@"     
UPDATE Order_QtyShip 
SET CFAUpdateDate=GETDATE()";
                if (this.CurrentMaintain["Stage"].ToString() == "Final")
                {
                    updateCmd += $@"
, CFAFinalInspectResult = '{this.CurrentMaintain["Result"].ToString()}'
, CFAFinalInspectDate = '{MyUtility.Convert.GetDate(this.CurrentMaintain["AuditDate"]).Value.ToString("yyyy/MM/dd")}'
, CFAFinalInspectHandle = '{Sci.Env.User.UserID}'
";
                }

                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    updateCmd += $@"
, CFA3rdInspectResult = '{this.CurrentMaintain["Result"].ToString()}'
, CFA3rdInspectDate = '{MyUtility.Convert.GetDate(this.CurrentMaintain["AuditDate"]).Value.ToString("yyyy/MM/dd")}'
, CFAIs3rdInspectHandle = '{Sci.Env.User.UserID}'
";
                }

                updateCmd += $@"
WHERE {tmpOrder_QtyShip.JoinToString(" OR ")}
";
            }
            else
            {
                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='Confirmed',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' 

UPDATE Order_QtyShip 
SET CFAUpdateDate=GETDATE()
WHERE {tmpOrder_QtyShip.JoinToString(" OR ")}
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
            string updateCmd = string.Empty;

            List<string> tmp = new List<string>();
            List<string> tmpOrder_QtyShip = new List<string>();
            foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
            {
                tmp.Add($@" (b.OrderID = '{dr["OrderID"]}' AND b.Seq = '{dr["Seq"]}') ");
                tmpOrder_QtyShip.Add($@" (ID = '{dr["OrderID"]}' AND Seq = '{dr["Seq"]}') ");
            }

            if (!tmp.Any())
            {
                tmp.Add("1=0");
                tmpOrder_QtyShip.Add("1=0");
            }

            if (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString() == "3rd party")
            {
                updateCmd += $@"
SELECT TOP 1 a.EditName
INTO #LastConfirm
FROM CFAInspectionRecord a
INNER JOIN CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
WHERE a.Result='Fail'
AND (  {tmp.JoinToString(" OR ")}  )
AND a.Stage='{this.CurrentMaintain["Stage"].ToString()}'
AND a.Status = 'Confirmed'
AND a.ID != '{this.CurrentMaintain["ID"].ToString()}'
ORDER BY a.EditDate DESC

UPDATE CFAInspectionRecord SET Status='New',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' ";

                updateCmd += $@"
IF NOT EXISTS(
    SELECT a.*
    FROM CFAInspectionRecord a
    INNER JOIN CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
    WHERE (Result = 'Pass' OR Result='Fail but release')	
    AND (  {tmp.JoinToString(" OR ")}  )
    AND a.Stage='{this.CurrentMaintain["Stage"].ToString()}'
    AND a.ID != '{this.CurrentMaintain["ID"].ToString()}'
)
BEGIN
";
                updateCmd += $@"
    SELECT TOP 1 a.* 
	INTO #LastFail
    FROM CFAInspectionRecord a
    INNER JOIN CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
    WHERE Result='Fail'
    AND (  {tmp.JoinToString(" OR ")}  )
    AND a.Stage='{this.CurrentMaintain["Stage"].ToString()}'
	AND a.Status = 'Confirmed'
    AND a.ID != '{this.CurrentMaintain["ID"].ToString()}'
	ORDER BY a.EditDate DESC

    UPDATE Order_QtyShip 
    SET CFAUpdateDate = GETDATE()";

                if (this.CurrentMaintain["Stage"].ToString() == "Final")
                {
                    updateCmd += $@"
        , CFAFinalInspectResult = (SELECT Result FROM #LastFail)
        , CFAFinalInspectDate =  IIF((SELECT EditDate FROM #LastFail)='',NULL,(SELECT EditDate FROM #LastFail))
        , CFAFinalInspectHandle  = ISNULL((SELECT EditName FROM #LastConfirm) ,'')
";
                }

                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    updateCmd += $@"
        , CFA3rdInspectResult = (SELECT Result FROM #LastFail)
        , CFA3rdInspectDate =  IIF((SELECT EditDate FROM #LastFail)='',NULL,(SELECT EditDate FROM #LastFail))
        , CFAIs3rdInspectHandle   = ISNULL( (SELECT EditName FROM #LastConfirm),'')
";
                }

                updateCmd += $@"    WHERE  {tmpOrder_QtyShip.JoinToString(" OR ")} ";
                updateCmd += $@"
END";
            }
            else
            {
                updateCmd += $@"
UPDATE CFAInspectionRecord SET Status='New',EditName='{Sci.Env.User.UserID}' ,EditDate=GETDATE() WHERE ID='{this.CurrentMaintain["ID"]}' 

UPDATE Order_QtyShip 
SET CFAUpdateDate=GETDATE()
WHERE  {tmpOrder_QtyShip.JoinToString(" OR ")} 
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
            try
            {
                // 檢查：AuditDate、Stage、Result、Team
                if (MyUtility.Check.Empty(this.CurrentMaintain["CFA"]) ||
                    this.CFAInspectionRecord_OrderSEQ.Rows.Count == 0 ||
                    MyUtility.Check.Empty(this.CurrentMaintain["AuditDate"]) ||
                    MyUtility.Check.Empty(this.CurrentMaintain["Stage"]) ||
                    MyUtility.Check.Empty(this.CurrentMaintain["Result"]) ||
                    MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
                {
                    MyUtility.Msg.WarningBox("CFA、SP & SEQ、Audit Date、Inspection stage、Inspection result、Team can't be all empty!!");
                    return false;
                }

                // 檢查：Line、Shift
                if (this.CurrentMaintain["Stage"].ToString() == "Staggered" && (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]) || MyUtility.Check.Empty(this.CurrentMaintain["Shift"])))
                {
                    MyUtility.Msg.WarningBox("Line、Shift can't be empty!!");
                    return false;
                }

                // 檢查：Defect Code、Area Code、No. of Defects
                bool anyEmpty = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).Where(o => MyUtility.Check.Empty(o["GarmentDefectCodeID"]) || MyUtility.Check.Empty(o["CFAAreaID"]) || MyUtility.Check.Empty(o["Qty"])).Any();

                if (anyEmpty)
                {
                    MyUtility.Msg.WarningBox("Defect Code、Area Code、No. of Defects can't be empty!!");
                    return false;
                }

                // 檢查：是否與登入者工廠不同
                bool sameFactory = MyUtility.Check.Seek($@"
SELECT 1 FROM Orders WHERE ID='{this.topOrderID}' AND FtyGroup = '{Sci.Env.User.Factory}'
");

                if (!sameFactory)
                {
                    MyUtility.Msg.WarningBox("Factory is different!!");
                    return false;
                }

                #region 檢查紙箱

                List<string> cartons = this.topCarton.Split(',').ToList();

                // Final、3rd party 且 IsCombinePO的話，檢查Sample單，不能輸入 Inspected Carton
                if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]) && (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString() == "3rd party"))
                {
                    if (this.IsSapmle && !MyUtility.Check.Empty(this.topCarton))
                    {
                        MyUtility.Msg.WarningBox("Can not input Inspected Carton!!");
                        return false;
                    }
                }

                // 檢查：[首次檢驗] 與 [非首次檢驗]
                if (this.CurrentMaintain["Stage"].ToString() == "Staggered")
                {
                    foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                    {
                        string cFAInspectionRecordID = this.IsDetailInserting ? string.Empty : this.CurrentMaintain["ID"].ToString();

                        string orderid = MyUtility.Convert.GetString(dr["OrderID"]);
                        string seq = MyUtility.Convert.GetString(dr["Seq"]);
                        List<string> carontList = MyUtility.Convert.GetString(dr["Carton"]).Split(',').ToList();

                        string sqlcmd = $@"
SELECT FirstStaggeredCFAInspectionRecordID ,CTNStartNo
FROM PackingList_Detail pd WITH(NOLOCK)
WHERE OrderID = '{orderid}'
AND OrderShipmodeSeq = '{seq}'
AND CTNStartNo IN ('{carontList.JoinToString("','")}')
";
                        DataTable t;
                        DualResult r = DBProxy.Current.Select(null, sqlcmd, out t);

                        // FirstStaggeredCFAInspectionRecordID不為空，且不是當前的ID，因此不是第一次檢驗
                        List<string> notFirst = t.AsEnumerable()
                            .Where(o => !MyUtility.Check.Empty(o["FirstStaggeredCFAInspectionRecordID"]) && MyUtility.Convert.GetString(o["FirstStaggeredCFAInspectionRecordID"]) != cFAInspectionRecordID)
                            .Select(o => MyUtility.Convert.GetString(o["CTNStartNo"])).Distinct().ToList();

                        // FirstStaggeredCFAInspectionRecordID為空，或紀錄是當前的ID，是第一次檢驗
                        List<string> isFirst = t.AsEnumerable()
                            .Where(o => MyUtility.Check.Empty(o["FirstStaggeredCFAInspectionRecordID"]) || MyUtility.Convert.GetString(o["FirstStaggeredCFAInspectionRecordID"]) == cFAInspectionRecordID)
                            .Select(o => MyUtility.Convert.GetString(o["CTNStartNo"])).Distinct().ToList();

                        // [首次檢驗] 與 [非首次檢驗]的紙箱都有
                        if (notFirst.Count > 0 && isFirst.Count > 0)
                        {
                            MyUtility.Msg.WarningBox($@"Cannot combine carton of [1st time inspect] and [not 1st time inspect] in the same record.
[1st time] : {isFirst.JoinToString(",")}
[not 1st time] : {notFirst.JoinToString(",")}");

                            return false;
                        }

                        // 全部紙箱皆為 [首次檢驗]
                        if (carontList.Count == isFirst.Count)
                        {
                            this.CurrentMaintain["FirstInspection"] = true;
                        }

                        // 全部紙箱皆為 [非首次檢驗]
                        if (carontList.Count == notFirst.Count)
                        {
                            this.CurrentMaintain["FirstInspection"] = false;
                        }
                    }
                }

                if (cartons.Where(o => !MyUtility.Check.Empty(o)).Count() == 0 &&
                    (this.CurrentMaintain["Stage"].ToString() == "Staggered" ||
                    this.CurrentMaintain["Stage"].ToString() == "Final" ||
                    this.CurrentMaintain["Stage"].ToString().ToLower() == "3rd party") &&
                    !this.IsSapmle &&
                    !MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]))
                {
                    MyUtility.Msg.WarningBox("Inspected Carton can't be empty!!");
                    return false;
                }

                string cmd = $@"
SELECT 1
FROM PackingList_Detail
WHERE OrderID = '{this.topOrderID}'
AND OrderShipmodeSeq ='{this.topSeq}'
AND CTNStartNo IN ('{cartons.JoinToString("','")}')
AND StaggeredCFAInspectionRecordID <> '{this.CurrentMaintain["ID"]}'
AND StaggeredCFAInspectionRecordID <> ''
";
                if (this.CurrentMaintain["Stage"].ToString() == "Staggered")
                {
                    bool duplicate = MyUtility.Check.Seek(cmd);

                    if (duplicate)
                    {
                        MyUtility.Msg.WarningBox("Carton already exists CFA Inspection Record!!");
                        return false;
                    }
                }
                #endregion

                // 檢查：Defects數量不大於檢驗數量
                if (MyUtility.Convert.GetInt(this.CurrentMaintain["DefectQty"]) > MyUtility.Convert.GetInt(this.CurrentMaintain["InspectQty"]))
                {
                    MyUtility.Msg.WarningBox("Defects Qty can't more than Inspect Qty!!");
                    return false;
                }

                // 檢查：檢驗數量不得大於訂單數量
                if (MyUtility.Convert.GetInt(this.CurrentMaintain["InspectQty"]) > MyUtility.Convert.GetInt(this.disOrderQty.Value))
                {
                    MyUtility.Msg.WarningBox("Inspect Qty can't more than Order Qty!!");
                    return false;
                }

                // 檢查：過去是否有Pass或Release的紀錄
                if ((this.CurrentMaintain["Stage"].ToString().ToLower() == "final" || this.CurrentMaintain["Stage"].ToString().ToLower() == "3rd party") && (this.CurrentMaintain["Result"].ToString().ToLower() == "pass" || this.CurrentMaintain["Result"].ToString().ToLower() == "fail but release"))
                {
                    List<string> tmpOrder_QtyShip = new List<string>();
                    foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                    {
                        tmpOrder_QtyShip.Add($@" (b.OrderID = '{dr["OrderID"]}' AND b.Seq = '{dr["Seq"]}') ");
                    }

                    cmd = $@"
SELECT 1
FROM CFAInspectionRecord a WITH(NOLOCK) 
INNER JOIN CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
WHERE a.ID <> '{this.CurrentMaintain["ID"]}'
AND ( {tmpOrder_QtyShip.JoinToString(" OR ")} )
AND a.Stage='{this.CurrentMaintain["Stage"].ToString()}'
AND (a.Result='Pass' OR a.Result='Fail but release')
";

                    bool hasSameSpSeq = MyUtility.Check.Seek(cmd);
                    if (hasSameSpSeq)
                    {
                        MyUtility.Msg.InfoBox("This SP# and Seq has passed or released.");
                        return false;
                    }
                }

                // 檢查：3rd party
                if (this.CurrentMaintain["Stage"].ToString() == "3rd party")
                {
                    cmd = $@"
SELECT 1
FROM Order_QtyShip WITH(NOLOCK)
WHERE ID = '{this.topOrderID}'
AND Seq = '{this.topSeq}'
AND CFAIs3rdInspect = 1
";
                    bool cFAIs3rdInspect = MyUtility.Check.Seek(cmd);
                    if (!cFAIs3rdInspect)
                    {
                        MyUtility.Msg.InfoBox($"SP#:{this.topOrderID} 、Sep:{this.topSeq}   is not 3rd Inspect.");
                        return false;
                    }
                }

                #region Clog received %(By piece) 計算
                List<string> tmp = new List<string>();
                foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                {
                    tmp.Add($@" (pd.OrderID = '{dr["OrderID"]}' AND pd.OrderShipmodeSeq = '{dr["Seq"]}') ");
                }

                if (!tmp.Any())
                {
                    tmp.Add("1=0");
                }

                this.CurrentMaintain["ClogReceivedPercentage"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($@"
SELECT  CAST(ROUND( SUM(IIF( CFAReceiveDate IS NOT NULL OR ReceiveDate IS NOT NULL
								,ShipQty
								,0)
						) * 1.0 
						/  SUM(ShipQty) * 100 
,0) AS INT) 
FROM PackingList_Detail pd WITH(NOLOCK)
WHERE {tmp.JoinToString(Environment.NewLine + "OR ")}
"));
                #endregion

                // 產生CFAInspectionRecord.ID
                if (this.IsDetailInserting)
                {
                    string tempId = MyUtility.GetValue.GetID(Sci.Env.User.Factory + "CI", "CFAInspectionRecord", DateTime.Now);
                    if (MyUtility.Check.Empty(tempId))
                    {
                        MyUtility.Msg.WarningBox("Get document ID fail!!");
                        return false;
                    }

                    this.CurrentMaintain["ID"] = tempId;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            // 只有Inspection stage為Staggered且Inspection result = Pass ，才需要將檢驗的箱號回寫PackingList_Detail.StaggeredCFAInspectionRecordID
            List<string> cartons = this.topCarton.Split(',').ToList();
            string cmd = string.Empty;
            string cmd_CFAInspectionRecord_OrderSEQ = string.Empty;

            // 設定CFAInspectionRecord_OrderSEQ.ID
            foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
            {
                dr["ID"] = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);
            }

            #region 更新PackingList_Detail

            // 清空
            cmd = $@"
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID = ''
WHERE StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}'
;
UPDATE PackingList_Detail
SET FirstStaggeredCFAInspectionRecordID  = ''
WHERE FirstStaggeredCFAInspectionRecordID  = '{this.CurrentMaintain["ID"]}'
;
";

            // 寫入StaggeredCFAInspectionRecordID
            if (this.CurrentMaintain["Stage"].ToString() == "Staggered" && this.CurrentMaintain["Result"].ToString() == "Pass")
            {
                cmd += $@"
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}'
WHERE OrderID = '{this.topOrderID}'
    AND OrderShipmodeSeq ='{this.topSeq}' 
    AND CTNStartNo IN ('{cartons.JoinToString("','")}')
    AND StaggeredCFAInspectionRecordID = ''
";
            }
            else
            {
                List<string> tmp = new List<string>();
                foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                {
                    tmp.Add($@"( OrderID = '{dr["OrderID"]}' AND OrderShipmodeSeq='{dr["Seq"]}' )");
                }

                if (!tmp.Any())
                {
                    tmp.Add("1=0");
                }

                cmd += $@"
UPDATE PackingList_Detail
SET StaggeredCFAInspectionRecordID=''
WHERE ( {tmp.JoinToString(" OR ")} )
AND StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}'
;
";
            }

            // FirstStaggeredCFAInspectionRecordID
            if (this.CurrentMaintain["Stage"].ToString() == "Staggered" && MyUtility.Convert.GetBool(this.CurrentMaintain["FirstInspection"]))
            {
                cmd += $@"
UPDATE PackingList_Detail
SET FirstStaggeredCFAInspectionRecordID  = '{this.CurrentMaintain["ID"]}'
WHERE OrderID = '{this.topOrderID}'
    AND OrderShipmodeSeq ='{this.topSeq}' 
    AND CTNStartNo IN ('{cartons.JoinToString("','")}')
    AND FirstStaggeredCFAInspectionRecordID  = ''
";
            }
            #endregion

            #region 更新CFAInspectionRecord_OrderSEQ
            int count = 1;
            string tempTable = string.Empty;

            // 組合temp table
            foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
            {
                string tmp = $"SELECT [ID]='{MyUtility.Convert.GetString(dr["ID"])}', [OrderID]='{MyUtility.Convert.GetString(dr["OrderID"])}', [Seq]='{MyUtility.Convert.GetString(dr["Seq"])}' , [Carton]='{MyUtility.Convert.GetString(dr["Carton"])}'";

                tempTable += tmp + Environment.NewLine;

                if (count == 1)
                {
                    tempTable += "INTO #source" + Environment.NewLine;
                }

                if (this.CFAInspectionRecord_OrderSEQ.Rows.Count > count)
                {
                    tempTable += "UNION" + Environment.NewLine;
                }

                count++;
            }

            cmd_CFAInspectionRecord_OrderSEQ = $@"
{tempTable}

DELETE t
FROM CFAInspectionRecord_OrderSEQ t
LEFT JOIN #source s ON t.ID = s.ID  AND t.OrderID = s.OrderID AND t.Seq = s.Seq AND t.Carton = s.Carton  
WHERE t.ID='{this.CurrentMaintain["ID"]}' AND ( s.ID IS NULL OR s.OrderID IS NULL OR s.Seq IS NULL OR s.Carton IS NULL )

INSERT CFAInspectionRecord_OrderSEQ   (ID, OrderID, Seq, Carton)
SELECT ID, OrderID, Seq, Carton
FROM #source s
WHERE NOT EXISTS(
    SELECT 1 FROM CFAInspectionRecord_OrderSEQ t 
    WHERE t.ID = s.ID AND t.OrderID = s.OrderID AND t.Seq = s.Seq AND t.Carton = s.Carton 
)

";
            #endregion

            cmd += cmd_CFAInspectionRecord_OrderSEQ;

            if (!MyUtility.Check.Empty(cmd))
            {
                DualResult result = DBProxy.Current.Execute(null, cmd);

                if (!result)
                {
                    this.ShowErr(result);
                    return result;
                }
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("The record status is confimed, you can not delete.");
                return false;
            }

            #region 檢查是否還有其他Staggered記錄（排除當下這個ID）
            if (this.CurrentMaintain["Stage"].ToString() == "Staggered" && MyUtility.Convert.GetBool(this.CurrentMaintain["FirstInspection"]))
            {
                List<string> hasOtherInspect = new List<string>();
                foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                {
                    string orderid = MyUtility.Convert.GetString(dr["OrderID"]);
                    string seq = MyUtility.Convert.GetString(dr["Seq"]);
                    string cartons = MyUtility.Convert.GetString(dr["Carton"]);

                    List<string> cartonList = cartons.Split(',').ToList();

                    foreach (var carton in cartonList)
                    {
                        string cmd = $@"
SELECT DISTINCT a.ID
FROM CFAInspectionRecord a
INNER JOIN CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
WHERE a.Stage='Staggered'
AND a.ID != '{this.CurrentMaintain["ID"]}'
AND b.OrderID='{orderid}' 
AND b.SEQ='{seq}'
AND (
		b.Carton = '{carton}'
	OR b.Carton LIKE  '{carton}' +',%' 
	OR b.Carton LIKE '%,'+  '{carton}' +',%' 
	OR b.Carton LIKE '%,'+  '{carton}'
)
";
                        bool hasOther = MyUtility.Check.Seek(cmd);
                        if (hasOther)
                        {
                            hasOtherInspect.Add($"SP#：{orderid}、Seq：{seq}、Carton：{carton}");
                        }
                    }
                }

                if (hasOtherInspect.Count > 0)
                {
                    string msg = "Cannot delete 1st time inspection record, due to 2nd time inspection record exists."+ Environment.NewLine + hasOtherInspect.JoinToString(Environment.NewLine);
                    MyUtility.Msg.WarningBox(msg);
                    return false;
                }
            }
            #endregion

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            string updateCmd = $@"
UPDATE PackingList_Detail SET StaggeredCFAInspectionRecordID = '' WHERE StaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' 
UPDATE PackingList_Detail SET FirstStaggeredCFAInspectionRecordID = '' WHERE FirstStaggeredCFAInspectionRecordID = '{this.CurrentMaintain["ID"]}' 
;
DELETE FROM CFAInspectionRecord_OrderSEQ WHERE ID = '{this.CurrentMaintain["ID"]}' 
";
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

                    this.toolbar.cmdNew.Enabled = !this.EditMode && canNew;
                    this.toolbar.cmdEdit.Enabled = !this.EditMode && canEdit;
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

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["ID"] = string.Empty;
            base.ClickCopyAfter();
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

            this.CalInsepectionCtn(this.IsDetailInserting, MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]));
        }

        private void TxtSpSeq_Leave(object sender, EventArgs e)
        {
            string newOrderID = this.txtSpSeq.TextBoxSPBinding;
            string newSeq = this.txtSpSeq.TextBoxSeqBinding;
            if (this.CurrentMaintain != null)
            {
                string oldOrderID = this.topOrderID;
                string oldSeq = this.topSeq;

                if ((newOrderID != oldOrderID) || (newSeq != oldSeq))
                {
                    this.topOrderID = newOrderID;
                    this.topSeq = newSeq;
                    this.CFAInspectionRecord_OrderSEQ.Clear();
                    DataRow nRow = this.CFAInspectionRecord_OrderSEQ.NewRow();
                    nRow["OrderID"] = this.topOrderID;
                    nRow["Seq"] = this.topSeq;
                    this.CFAInspectionRecord_OrderSEQ.Rows.Add(nRow);

                    this.AutoInsertBySP(newOrderID, newSeq);

                    this.CurrentMaintain["Stage"] = string.Empty;
                    this.CurrentMaintain["Result"] = string.Empty;
                    this.txtInspectedCarton.Text = string.Empty;
                    this.topCarton = string.Empty;
                }

                // 開始計算檢驗次數
                this.CalInsepectionCtn(this.IsDetailInserting, false);
            }

            bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{this.topOrderID}' "));
            this.IsSapmle = isSample;
            if (isSample && this.comboStage.Items.Contains("Staggered"))
            {
                this.comboStage.Items.RemoveAt(2);
            }
            else if (!isSample)
            {
                this.comboStage.Items.Clear();
                this.comboStage.Items.AddRange(new object[]
                {
            string.Empty,
            "Inline",
            "Staggered",
            "Final",
            "3rd party",
                });
            }
        }

        private void NumInspectQty_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CurrentMaintain["InspectQty"] = this.numInspectQty.Value;
            this.CauculateSQR();
        }

        private void ComboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain != null)
            {
                // this.CurrentMaintain["Team"] = this.comboTeam.SelectedItem.ToString();
            }
        }

        private void ComboResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain != null)
            {
                // this.CurrentMaintain["Result"] = this.comboResult.SelectedItem.ToString();
            }
        }

        private void TxtSewingLine_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
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

        private void TxtSewingLine_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.EditMode && this.txtSewingLine.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                List<string> lines = this.txtSewingLine.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Distinct().ToList();
                List<string> errorLines = new List<string>();
                foreach (var line in lines)
                {
                    DataTable dt;
                    string sqlCmd = $"SELECT 1 FROM SewingLine WITH(NOLOCK) WHERE FactoryID = '{Sci.Env.User.Factory}' AND ID = @ID";
                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@ID", line));

                    DualResult r = DBProxy.Current.Select(null, sqlCmd, paras, out dt);
                    if (!r)
                    {
                        this.ShowErr(r);
                    }
                    else
                    {
                        if (dt.Rows.Count == 0)
                        {
                            errorLines.Add(line);
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
                    this.CurrentMaintain["SewingLineID"] = lines.JoinToString(",");
                }
            }

            if (!this.txtSewingLine.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                this.CurrentMaintain["SewingLineID"] = string.Empty;
            }
        }

        private void TxtInspectedCarton_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.IsSapmle)
                {
                    return;
                }

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@OrderID", this.topOrderID));
                paras.Add(new SqlParameter("@Seq", this.topSeq));

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
                if (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString().ToLower() == "3rd party")
                {
                    sqlCmd = $@"

----記錄哪些箱號有混尺碼
SELECT ID,OrderID,OrderShipmodeSeq,CTNStartNo
		,[ArticleCount]=COUNT(DISTINCT Article)
		,[SizeCodeCount]=COUNT(DISTINCT SizeCode)
INTO #MixCTNStartNo
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
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
		FOR XML PATH(''))
		,1,1,'')
	)MixSizeCode
	OUTER APPLY(
		SELECT  [Val]=SUM(pd.ShipQty)
		FROM PackingList_Detail pd
		WHERE pd.ID = t.ID 
			AND pd.OrderID = t.OrderID 
			AND pd.CTNStartNo = t.CTNStartNo
	)ShipQty
) a
ORDER BY Cast([CTN#] as int)


DROP TABLE #MixCTNStartNo 

";
                }

                DataTable dt;
                DBProxy.Current.Select(null, sqlCmd, paras, out dt);
                Sci.Win.Tools.SelectItem2 item = new Sci.Win.Tools.SelectItem2(dt, "CTN#,Article,Size,Qty", "CTN#,Article,Size,Qty", "3,15,20,5", this.topCarton, null, null, null);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.txtInspectedCarton.Text = item.GetSelectedString();
                    this.topCarton = item.GetSelectedString();
                }
            }
        }

        private void TxtInspectedCarton_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.EditMode && this.txtInspectedCarton.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                if (this.IsSapmle)
                {
                    this.topCarton = string.Empty;
                    this.txtInspectedCarton.Text = string.Empty;
                    return;
                }

                List<string> cartons = this.txtInspectedCarton.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Distinct().ToList();
                List<string> errorCartons = new List<string>();

                foreach (var carton in cartons)
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
                    if (this.CurrentMaintain["Stage"].ToString() == "Final" || this.CurrentMaintain["Stage"].ToString().ToLower() == "3rd party")
                    {
                        sqlCmd = $@"
SELECT * 
FROM PackingList_Detail pd
WHERE OrderID = @OrderID
AND OrderShipmodeSeq = @Seq
AND CTNStartNo = @CTNStartNo

";
                    }

                    List<SqlParameter> paras = new List<SqlParameter>();
                    paras.Add(new SqlParameter("@OrderID", this.topOrderID));
                    paras.Add(new SqlParameter("@Seq", this.topSeq));
                    paras.Add(new SqlParameter("@CTNStartNo", carton));
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
                            errorCartons.Add(carton);
                        }
                    }
                }

                if (errorCartons.Count > 0)
                {
                    MyUtility.Msg.WarningBox($"CTN# NOT Found : " + errorCartons.JoinToString(","));

                    this.topCarton = string.Empty;
                    this.txtInspectedCarton.Text = string.Empty;
                }
                else
                {
                    this.topCarton = cartons.JoinToString(",");
                    this.txtInspectedCarton.Text = cartons.JoinToString(",");
                }
            }

            if (!this.txtInspectedCarton.Text.Split(',').Where(o => !MyUtility.Check.Empty(o)).Any())
            {
                this.topCarton = string.Empty;
                this.txtInspectedCarton.Text = string.Empty;
            }

            // 更新右邊視窗裡面的Carton欄位
            var currentOrderSEQ = this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["OrderID"]) == this.topOrderID
            && MyUtility.Convert.GetString(o["Seq"]) == this.topSeq);

            if (currentOrderSEQ.Any())
            {
                currentOrderSEQ.FirstOrDefault()["Carton"] = this.topCarton;
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

            if (this._oldStage != stage)
            {
                this.topCarton = string.Empty;
                this.txtInspectedCarton.Text = string.Empty;
                this.CurrentMaintain["FirstInspection"] = false;

                foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                {
                    dr["Carton"] = string.Empty;
                }
            }

            // 只有選擇Staggered時Inspected Carton、Shift才可以欄位才可以編輯，選到其他Stage時請一併清除這些欄位資料。
            if (stage == "Staggered")
            {
                this.txtInspectedCarton.IsSupportEditMode = true;
                this.txtInspectedCarton.ReadOnly = false;

                this.txtshift.IsSupportEditMode = true;
                this.txtshift.ReadOnly = false;
            }
            else if (stage == "Inline" || MyUtility.Check.Empty(stage))
            {
                this.txtInspectedCarton.Text = string.Empty;
                this.txtInspectedCarton.IsSupportEditMode = false;
                this.txtInspectedCarton.ReadOnly = true;

                this.CurrentMaintain["Shift"] = string.Empty;
                this.txtshift.Text = string.Empty;
                this.txtshift.IsSupportEditMode = false;
                this.txtshift.ReadOnly = true;
            }
            else
            {
                if (!MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]))
                {
                    this.txtInspectedCarton.IsSupportEditMode = true;
                    this.txtInspectedCarton.ReadOnly = false;
                }

                this.txtshift.IsSupportEditMode = true;
                this.txtshift.ReadOnly = false;
            }

            // Stage若是3rd party則要檢查Order_QtyShip.CFAIs3rdInspect是否為1，若不是則不能存檔，並跳出警告視窗
            if (stage == "3rd party")
            {
                List<string> errorSP = new List<string>();
                try
                {
                    foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                    {
                        bool cFAIs3rdInspect1 = MyUtility.Check.Seek($@"
SELECT 1
FROM Order_QtyShip WITH(NOLOCK)
WHERE ID = '{dr["OrderID"]}'
AND Seq = '{dr["Seq"]}'
AND CFAIs3rdInspect = 1
");
                        if (!cFAIs3rdInspect1)
                        {
                            errorSP.Add($"SP#:{dr["OrderID"]} 、Sep:{dr["Seq"]}");
                        }
                    }

                    if (errorSP.Any())
                    {
                        MyUtility.Msg.InfoBox("Below data is not 3rd Inspect : " + Environment.NewLine + errorSP.JoinToString(Environment.NewLine));

                        this.comboStage.SelectedIndex = 0;
                        this.CurrentMaintain["Stage"] = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    this.comboStage.SelectedIndex = 0;
                    this.CurrentMaintain["Stage"] = string.Empty;
                    this.ShowErr(ex);
                }
            }

            // Final的時候Inspection result才能有Fail but release
            if (stage == "Final")
            {
                // Final + IsCombinePO = 1，不允許輸入Carton
                if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]))
                {
                    foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
                    {
                        dr["Carton"] = string.Empty;
                    }
                }

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

            decimal inspectQty = MyUtility.Convert.GetDecimal(this.CurrentMaintain["InspectQty"]);

            // 計算SQR
            if (inspectQty == 0)
            {
                this.numSQR.Value = null;
                return;
            }

            decimal sQR = totalDefectQty / inspectQty * 100;
            this.numSQR.Value = sQR;
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

            List<string> tmp = new List<string>();
            foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
            {
                tmp.Add($@" (oq.ID= '{dr["OrderID"]}' AND oq.Seq = '{dr["Seq"]}') ");
            }

            if (!tmp.Any())
            {
                tmp.Add("1=0");
            }

            this.disOrderQty.Value = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($@"
SELECT SUM(Qty) 
FROM Order_QtyShip oq 
WHERE {tmp.JoinToString(Environment.NewLine + "OR ")}
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
    FROM Order_QtyShip_Detail oq
    WHERE {tmp.JoinToString(Environment.NewLine + "OR ")}
    FOR XML PATH('')
    )
,1,1,'')
");
        }

        private void Reset_comboStage(string orderID, bool isCombinePO = false)
        {
            if (!this.EditMode)
            {
                return;
            }

            bool isSample = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($@"SELECT  IIF(Category='S','True','False') FROM Orders WHERE ID = '{orderID}' "));

            this.comboStage.Items.Clear();
            this.comboStage.Items.AddRange(new object[]
            {
                string.Empty,
                "Inline",
                "Staggered",
                "Final",
                "3rd party",
            });
            if (isSample)
            {
                this.comboStage.Items.RemoveAt(2);
            }
            else if (isCombinePO)
            {
                // 刪除，後面的Index了會往前推，所以同一個Index就好
                this.comboStage.Items.RemoveAt(1);
                this.comboStage.Items.RemoveAt(1);
            }

            this.comboStage.SelectedItem = this.CurrentMaintain["Stage"].ToString();
        }

        private void Reset_comboResult(bool isFinal = false)
        {
            this.comboResult.Items.Clear();

            // Final的時候Inspection result才能有Fail but release
            if (!isFinal)
            {
                this.comboResult.Items.AddRange(new object[]
                {
                    string.Empty,
                    "Pass",
                    "Fail",
                });
            }
            else
            {
                this.comboResult.Items.AddRange(new object[]
                {
                    string.Empty,
                    "Pass",
                    "Fail",
                    "Fail but release",
                });
            }

            this.comboResult.SelectedItem = this.CurrentMaintain["Result"].ToString();
        }

        private void CalInsepectionCtn(bool isClickNew, bool isCombinePO)
        {
            string cmd = string.Empty;
            /*
            // 必須條件
            if (MyUtility.Check.Empty(this.topOrderID) || MyUtility.Check.Empty(this.topSeq) || MyUtility.Check.Empty(this.CurrentMaintain["Stage"]) || MyUtility.Check.Empty(this.CurrentMaintain["AuditDate"]))
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

            if (isCombinePO)
            {
                this.disInsCtn.Value = null;
                return;
            }
            */
            List<string> tmp = new List<string>();
            foreach (DataRow dr in this.CFAInspectionRecord_OrderSEQ.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted))
            {
                tmp.Add($@" (b.OrderID= '{dr["OrderID"]}' AND b.Seq = '{dr["Seq"]}') ");
            }

            if (!tmp.Any())
            {
                tmp.Add("1=0");
            }

//            cmd = $@"
//SELECT COUNT(1) + 1
//FROM CFAInspectionRecord a
//INNER JOIN CFAInspectionRecord_OrderSEQ b ON a.ID = b.ID
//WHERE ( {tmp.JoinToString(" OR ")} )
//AND Status = 'Confirmed'
//AND Stage='{this.CurrentMaintain["Stage"]}'
//AND AuditDate <= '{MyUtility.Convert.GetDate(this.CurrentMaintain["AuditDate"]).Value.ToString("yyyy/MM/dd")}'
//AND a.ID  != '{this.CurrentMaintain["ID"]}'
//";

//            this.disInsCtn.Value = MyUtility.GetValue.Lookup(cmd);
        }

        private void ChkIsCombinePO_CheckedChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.CurrentMaintain["IsCombinePO"] = this.chkIsCombinePO.Checked;
                this.txtSpSeq.TextBoxSPBinding = string.Empty;
                this.txtSpSeq.TextBoxSeqBinding = string.Empty;
                this.topOrderID = string.Empty;
                this.topSeq = string.Empty;

                this.AutoInsertBySP(this.topOrderID, this.topSeq);

                if (MyUtility.Convert.GetBool(this.CurrentMaintain["IsCombinePO"]))
                {
                    // 清空
                    this.topCarton = string.Empty;
                    this.txtInspectedCarton.Text = string.Empty;

                    this.txtSpSeq.TextBoxSP.ReadOnly = true;
                    this.txtSpSeq.TextBoxSeq.ReadOnly = true;
                    this.txtInspectedCarton.ReadOnly = true;

                    this.txtSpSeq.TextBoxSP.IsSupportEditMode = false;
                    this.txtSpSeq.TextBoxSeq.IsSupportEditMode = false;
                    this.txtInspectedCarton.IsSupportEditMode = false;

                    this.btnSettingSpSeq.Enabled = true && this.EditMode;
                    this.Reset_comboStage(this.topOrderID, true);

                    this.CFAInspectionRecord_OrderSEQ.Clear();
                }
                else
                {
                    this.txtSpSeq.TextBoxSP.ReadOnly = false;
                    this.txtSpSeq.TextBoxSeq.ReadOnly = false;
                    this.txtInspectedCarton.ReadOnly = false;

                    this.txtSpSeq.TextBoxSP.IsSupportEditMode = true;
                    this.txtSpSeq.TextBoxSeq.IsSupportEditMode = true;
                    this.txtInspectedCarton.IsSupportEditMode = true;

                    this.btnSettingSpSeq.Enabled = false && this.EditMode;

                    this.CFAInspectionRecord_OrderSEQ.Clear();
                    this.Reset_comboStage(this.topOrderID);
                }
            }
        }

        private void BtnSettingSpSeq_Click(object sender, EventArgs e)
        {
            bool canEdit = Prgs.GetAuthority(Sci.Env.User.UserID, "P32. CFA Inspection Record ", "CanEdit");
            P32_CombinePO form = new P32_CombinePO(canEdit, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), this.CFAInspectionRecord_OrderSEQ, this.CurrentMaintain);
            form.ShowDialog();

            if (this.CFAInspectionRecord_OrderSEQ.Rows.Count > 0)
            {
                this.topOrderID = MyUtility.Convert.GetString(this.CFAInspectionRecord_OrderSEQ.AsEnumerable().FirstOrDefault()["OrderID"]);
                this.topSeq = MyUtility.Convert.GetString(this.CFAInspectionRecord_OrderSEQ.AsEnumerable().FirstOrDefault()["Seq"]);

                this.txtSpSeq.TextBoxSPBinding = this.topOrderID;
                this.txtSpSeq.TextBoxSeqBinding = this.topSeq;

                this.AutoInsertBySP(this.topOrderID, this.topSeq);

                this.txtInspectedCarton.Text = string.Empty;
                this.topCarton = string.Empty;
            }
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class P32Header
    {
        /// <inheritdoc/>
        public string OrderID { get; set; }

        /// <inheritdoc/>
        public string Seq { get; set; }

        /// <inheritdoc/>
        public string PO { get; set; }

        /// <inheritdoc/>
        public string Style { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <inheritdoc/>
        public string Season { get; set; }

        /// <inheritdoc/>
        public string M { get; set; }

        /// <inheritdoc/>
        public string Factory { get; set; }

        /// <inheritdoc/>
        public string BuyerDev { get; set; }

        /// <inheritdoc/>
        public string OrderQty { get; set; }

        /// <inheritdoc/>
        public string Dest { get; set; }

        /// <inheritdoc/>
        public string Article { get; set; }
    }
}
