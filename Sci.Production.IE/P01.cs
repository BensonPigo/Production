using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Prg;
using Sci.Production.PublicForm;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using static Ict.Win.DataGridViewGenerator;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01
    /// </summary>
    public partial class P01 : Win.Tems.Input6
    {
        private readonly int seqIncreaseNumber = 10; // 新增一筆時 SEQ 從最大往上增加
        private DataGridViewGeneratorTextColumnSettings operation = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings machine = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings mold = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings frequency = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings smvsec = new DataGridViewGeneratorNumericColumnSettings();

        private string styleID;
        private string seasonID;
        private string brandID;
        private string comboType;
        private string strTimeStudtID = string.Empty;
        private List<P01_OperationClass> p01_OperationList = new List<P01_OperationClass>();

        /// <summary>
        /// 將目前DetailGrid裡面，使用者所選擇的Row，將它們綁定的資料列取出
        /// </summary>
        public IEnumerable<DataRow> SelectedDetailGridDataRow
        {
            get
            {
                if (this.detailgrid == null ||
                    this.detailgrid.DataSource == null ||
                    this.detailgrid.SelectedRows.Count == 0)
                {
                    return Enumerable.Empty<DataRow>();
                }
                else
                {
                    return this.detailgrid
                        .GetTable().AsEnumerable()
                        .Where(o => o.RowState != DataRowState.Deleted)
                        .Where(o => o["Selected"].ToString() == "1")
                        .ToList();
                }
            }
        }

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.Columns.DisableSortable();
            this.detailgrid.CellMouseMove += this.Detailgrid_CellMouseMove;
            this.detailgrid.CellMouseLeave += this.Detailgrid_CellMouseLeave;
            this.detailgrid.CellFormatting += this.Detailgrid_CellFormatting;
            this.detailgrid.Columns.DisableSortable();
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.detailgrid.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            new GridRowDrag(this.detailgrid, this.DetailGridAfterRowDragDo, enableDragCell: false);
        }

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="styleID">StyleID</param>
        /// <param name="brandID">BrandID</param>
        /// <param name="seasonID">SeasonID</param>
        /// <param name="comboType">ComboType</param>
        /// <param name="isReadOnly">isReadOnly</param>
        public P01(string styleID, string brandID, string seasonID, string comboType, bool isReadOnly = false)
        {
            this.InitializeComponent();
            this.detailgrid.Columns.DisableSortable();
            this.detailgrid.CellMouseMove += this.Detailgrid_CellMouseMove;
            this.detailgrid.CellMouseLeave += this.Detailgrid_CellMouseLeave;
            this.detailgrid.CellFormatting += this.Detailgrid_CellFormatting;

            StringBuilder df = new StringBuilder();
            df.Append(string.Format("StyleID = '{0}' ", styleID));
            if (!MyUtility.Check.Empty(brandID))
            {
                df.Append(string.Format(" and BrandID ='{0}' ", brandID));
            }

            if (!MyUtility.Check.Empty(seasonID))
            {
                df.Append(string.Format(" and SeasonID ='{0}' ", seasonID));
            }

            if (!MyUtility.Check.Empty(comboType))
            {
                df.Append(string.Format(" and ComboType ='{0}' ", comboType));
            }

            if (isReadOnly)
            {
                this.IsSupportEdit = false;
                this.IsSupportNew = false;
                this.IsSupportConfirm = false;
                this.IsSupportCopy = false;
                this.IsSupportDelete = false;
                this.IsDeleteOnBrowse = false;
            }
            else
            {
                this.IsSupportEdit = true;
                this.IsSupportNew = true;
                this.IsSupportConfirm = true;
                this.IsSupportCopy = true;
                this.IsSupportDelete = true;
                this.IsDeleteOnBrowse = true;
            }

            this.DefaultFilter = df.ToString();
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            new GridRowDrag(this.detailgrid, this.DetailGridAfterRowDragDo, enableDragCell: false);
        }

        private void Detailgrid_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.detailgrid.Cursor = Cursors.Default;
        }

        private void Detailgrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                this.detailgrid.Cursor = Cursors.Hand;
            }
        }

        private void Detailgrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == "IsNonSewingLine")
            {
                DataRow curRow = this.detailgrid.GetDataRow(e.RowIndex);
                this.detailgrid.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = !MyUtility.Convert.GetBool(curRow["IsNonSewingLineEditable"]);

                this.detailgrid.Rows[e.RowIndex].Cells["SewingSeq"].ReadOnly = MyUtility.Convert.GetBool(curRow["IsNonSewingLine"]);
            }

            // 確保只處理列索引為 0 的格式化事件（避免多次處理同一行）
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                // 重設所有行的背景色為白色
                foreach (DataGridViewRow row in this.detailgrid.Rows)
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }

                // 遍歷所有行檢查條件
                foreach (DataGridViewRow row in this.detailgrid.Rows)
                {
                    // 取得欄位 DesignateSeq 和 SewingSeq 的值
                    var designateSeqValue = row.Cells["DesignateSeq"].Value;
                    var sewingSeqValue = row.Cells["SewingSeq"].Value;

                    // 1. 若 DesignateSeq 欄位有值，該行設為黃色
                    if (!MyUtility.Check.Empty(designateSeqValue))
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);

                        // 2. 找出所有 DesignateSeq == SewingSeq 的行，並設為黃色
                        foreach (DataGridViewRow otherRow in this.detailgrid.Rows)
                        {
                            var otherSewingSeqValue = otherRow.Cells["SewingSeq"].Value; // SewingSeq
                            if (designateSeqValue.ToString() == MyUtility.Convert.GetString(otherSewingSeqValue))
                            {
                                otherRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 128);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">PrepareDetailSelectCommandEventArgs</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.strTimeStudtID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand =
            $@"
            DECLARE @EndSeq varchar(4)
            SET @EndSeq = (SELECT TOP 1 Seq FROM TimeStudy_Detail WHERE id = '{this.strTimeStudtID}' ORDER BY Seq DESC);

            With tmp1 as
            (
                SELECT 
                td.Seq
                ,[NextSeq] = CASE 
                        WHEN LEAD(td.Seq, 1, 0) OVER (ORDER BY td.Seq) = 0 
                        THEN @EndSeq
                        ELSE LEAD(td.Seq, 1, 0) OVER (ORDER BY td.Seq)
                    END
                ,td.OperationID
                from TimeStudy_Detail td WITH(NOLOCK)
                where td.id = '{this.strTimeStudtID}' and td.OperationID LIKE '-%' and td.smv = 0 
            )
            select distinct 0 as Selected, isnull(o.SeamLength,0) SeamLength
            ,ID.CodeFrom
            ,ID.IETMSUkey
            ,td.[ID]
            ,td.DesignateSeq
            ,td.[SEQ] -- Ori. Seq ( IsAdd = 0 ) 是從 Trade 轉進來的不可改變
			,[OriSewingSeq] = isnull(td.SewingSeq,'')
            ,td.[SewingSeq]
            ,[Location] = iif(td.IsAdd = 0,iif(td.[Location] = '' , isnull(t1.OperationID,''),isnull(td.[Location],'')),isnull(td.[Location],''))
	        ,td.[OperationID]
            ,td.[Annotation]
            ,td.[PcsPerHour]
            ,td.[Sewer]
            ,td.[MachineTypeID]
            ,td.[Frequency]
            ,td.[IETMSSMV]
            ,td.[Mold]
            ,td.[SMV]
            ,td.[OldKey]
            ,td.[Ukey] 
            ,o.DescEN as OperationDescEN
            ,td.MtlFactorID
            ,td.Template
            ,(isnull(td.Frequency,0) * isnull(o.SeamLength,0)) as ttlSeamLength
	        ,td.MasterPlusGroup
            ,[IsShow] = cast(iif( td.OperationID like '--%' , 1, isnull(show.val, 1)) as bit)
            ,[IsSubprocess] = isnull(td.IsSubprocess, 0)
            ,[MachineType_IsSubprocess] = isnull(md.IsSubprocess, 0)
            ,td.PPA
            ,PPAText = ISNULL(d.Name,'')
            ,td.IsNonSewingLine
            ,td.SewingMachineAttachmentID
            ,td.Thread_ComboID
            ,td.StdSMV
            ,[IsNonSewingLineEditable] = cast(case    when isnull(md.IsSubprocess, 0) = 1 then 1
                                                    when isnull(md.IsNonSewingLine, 0) = 1 then 1
                                                    else 0 end as bit)
            ,[Sort] = iif(td.Sort = 0 , ROW_NUMBER() OVER (ORDER BY td.seq ASC), td.Sort) -- Ori. Seq ( IsAdd = 0 ) 是從 Trade 轉進來的不可改變
            ,[IsAdd] = td.IsAdd
            from TimeStudy_Detail td WITH (NOLOCK) 
            INNER JOIN TimeStudy t WITH(NOLOCK) ON td.id = t.id
            LEFT JOIN IETMS i ON t.IETMSID = i.ID AND t.IETMSVersion = i.[Version]
            LEFT JOIN IETMS_Detail ID ON I.Ukey = ID.IETMSUkey AND ID.SEQ = TD.Seq
            left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
            left join MachineType_Detail md WITH (NOLOCK) on md.ID = td.MachineTypeID and md.FactoryID = '{Env.User.Factory}'
            left join Mold m WITH (NOLOCK) on m.ID=td.Mold
            left join DropDownList d (NOLOCK) on d.ID=td.PPA AND d.Type = 'PMS_IEPPA'
            outer apply (
	                select [val] = IIF(isnull(mt.IsNotShownInP01, 1) = 0, 1, 0)
                from Operation o2
	            Inner Join MachineType_Detail mt on o2.MachineTypeID = mt.ID and mt.FactoryID = '{Env.User.Factory}'
	            where o.ID = o2.ID
            )show
            OUTER APPLY
            (
	            SELECT TOP 1
	            OperationID FROM tmp1 t1 
	            WHERE t1.NextSeq = @EndSeq  OR t1.NextSeq > td.Seq  
	            ORDER BY t1.Seq asc
            )t1
            where td.ID = '{this.strTimeStudtID}'
            order by SewingSeq,sort
            ";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            // 新增一筆時 SEQ 從最大往上增加
            if (e.Details != null)
            {
                DataColumn idColumn = new DataColumn("IdetityKey", typeof(int));
                idColumn.AutoIncrement = true;       // 啟用自動編號
                idColumn.AutoIncrementSeed = 1;      // 起始值
                idColumn.AutoIncrementStep = 1;      // 增量
                idColumn.AllowDBNull = false;
                e.Details.Columns.Add(idColumn);

                e.Details.Columns.Add("DesignateSeqIdetityKey", typeof(int));

                foreach (DataRow row in e.Details.Rows)
                {
                    row["IdetityKey"] = row.Table.Rows.IndexOf(row) + 1;
                }

                e.Details.Columns["IdetityKey"].Unique = true;

                foreach (DataRow row in e.Details.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["DesignateSeq"])))
                {
                    this.MappingDesignateSeqSewingSeq(row);
                }
            }

            return base.OnRenewDataDetailPost(e);
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, "last 2 years modify,All");
            this.queryfors.SelectedIndex = 0;
            if (MyUtility.Check.Empty(this.DefaultWhere) && MyUtility.Check.Empty(this.DefaultFilter))
            {
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
            }
            else
            {
                this.queryfors.SelectedIndex = 1;
            }

            base.OnFormLoaded();

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
                        break;
                    case 1:
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }

                /* 底層bug
                 * 1. 放大鏡與DefaultFilter 問題
                    => 底層寫法 if(DefaultFilter) else(放大鏡) 所以 兩者只能用一個。
                 * 2. 放大鏡、DefaultOrder與DefaultWhere 衝突問題。
                    => 假如有設定DefaultOrder會導致，操作放大鏡後QueryExpress會恆寫入DefaultOrder所設定的值
                       ，導致在做ReloadDatas()都不會執行DefaultWhere結果。
                */
                if (this.QBCommand != null && this.QBCommand.Conditions.Count() == 0)
                {
                    this.QueryExpress = string.Empty;
                }

                this.ReloadDatas();

                GC.Collect();
            };

            // MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "T,B,I,O");
            MyUtility.Tool.SetupCombox(this.comboStatus, 1, 1, "Estimate,Initial,Prelim,Final");

            #region modify comboBox1 DataSource as Style_Location
            string sqlCmd = "select distinct Location from Style_Location WITH (NOLOCK) ";
            DualResult result;
            DataTable dtLocation;
            result = DBProxy.Current.Select(null, sqlCmd, out dtLocation);

            this.comboStyle.DataSource = dtLocation;
            this.comboStyle.DisplayMember = "Location";
            this.comboStyle.ValueMember = "Location";
            #endregion
        }

        /// <summary>
        /// OnDetailEntered()
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.detailgrid.AllowDrop = this.EditMode;
            this.GenCD(null, null);  // 撈CD Code
            bool isConfirmed = this.CurrentMaintain["Status"].ToString().ToLower().EqualString("confirmed");
            bool canEdit = PublicPrg.Prgs.GetAuthority(Env.User.UserID, "P01. Factory GSD", "CanEdit");
            this.btnNewVersion.Enabled = !this.EditMode && this.CurrentMaintain != null && canEdit && isConfirmed;
            this.btnNewStatus.Enabled = !this.EditMode && this.CurrentMaintain != null && canEdit && isConfirmed;
            this.btnHistory.Enabled = !this.EditMode && this.CurrentMaintain != null;
            this.btnStdGSDList.Enabled = !this.EditMode && this.CurrentMaintain != null;
            this.btnArtSum.Enabled = this.CurrentMaintain != null;
            this.btnSketch.Enabled = this.CurrentMaintain != null;

            string styleVersion = MyUtility.GetValue.Lookup($@"
select IETMSVersion from Style 
where id= '{this.CurrentMaintain["StyleID"]}'
and SeasonID= '{this.CurrentMaintain["SeasonID"]}'
and BrandID = '{this.CurrentMaintain["BrandID"]}'
and IETMSID = '{this.CurrentMaintain["IETMSID"]}'
");
            if (styleVersion != this.CurrentMaintain["IETMSVersion"].ToString() && this.EditMode == false)
            {
                this.labVersionWarning.Visible = true;
            }
            else
            {
                this.labVersionWarning.Visible = false;
            }

            if (this.EditMode)
            {
                this.txtInsertPosition.Text = string.Empty;
                this.ui_pnlBatchUpdate.Visible = true;
            }
            else
            {
                this.ui_pnlBatchUpdate.Visible = false;
            }

            var sumDetailItems = this.DetailDatas
                .Where(s => !MyUtility.Convert.GetBool(s["IsSubprocess"]) &&
                            !MyUtility.Convert.GetBool(s["IsNonSewingLine"]) &&
                            MyUtility.Convert.GetString(s["PPA"]) != "C");

            this.numericStdSMV.Value = 0;
            this.numericFtySMV.Value = 0;

            if (sumDetailItems.Any())
            {
                this.numericStdSMV.Value = sumDetailItems.Sum(s => MyUtility.Convert.GetDecimal(s["StdSMV"]));
                this.numericFtySMV.Value = sumDetailItems.Sum(s => MyUtility.Convert.GetDecimal(s["SMV"]));
            }

            this.numTotalSMV.Value = Convert.ToInt32(MyUtility.Convert.GetDecimal(((DataTable)this.detailgridbs.DataSource).Compute("SUM(SMV)", string.Empty)));
            this.HideRows();

            string sqlCheckThreadComboCanEdit = $@"
select 1
from    style with (nolock)
where   ID = '{this.CurrentMaintain["StyleID"]}' and
        BrandID = '{this.CurrentMaintain["BrandID"]}' and
        SeasonID = '{this.CurrentMaintain["SeasonID"]}' and
        IETMSID_Thread = '{this.CurrentMaintain["IETMSID"]}' and
        IETMSVersion_Thread = '{this.CurrentMaintain["IETMSVersion"]}'
";
            if (MyUtility.Check.Seek(sqlCheckThreadComboCanEdit))
            {
                ((TextColumn)this.detailgrid.Columns["Thread_ComboID"]).IsEditingReadOnly = false;
            }
            else
            {
                ((TextColumn)this.detailgrid.Columns["Thread_ComboID"]).IsEditingReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();

            // 檢查 detailgrid 是否為 null，或者程式是否仍在初始化
            if (this.detailgrid == null || !this.IsHandleCreated || this.Disposing || this.IsDisposed)
            {
                return; // 如果程式還在載入或 detailgrid 無法使用，直接返回
            }

            if (this.EditMode)
            {
                this.detailgrid.Columns.DisableSortable();
            }
            else
            {
                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                }
            }
        }

        private void HideRows()
        {
            for (int i = 0; i < this.detailgrid.Rows.Count; i++)
            {
                DataRow row = this.detailgrid.GetDataRow(i);
                if (!MyUtility.Convert.GetBool(row["IsShow"]))
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)this.BindingContext[this.detailgrid.DataSource];
                    currencyManager1.SuspendBinding();
                    this.detailgrid.Rows[i].Visible = false;
                    currencyManager1.ResumeBinding();
                }
            }
        }

        private void GetMachineType_Detail(string machineTypeID)
        {
            string sqlGetMachineType_Detail = $@"
select  IsSubprocess,
        IsNonSewingLine,
        [IsNonSewingLineEditable] = cast(case   when isnull(IsSubprocess, 0) = 1 then 1
                                                when isnull(IsNonSewingLine, 0) = 1 then 1
                                                else 0 end as bit)
from MachineType_Detail where FactoryID = '{Env.User.Factory}' and ID = '{machineTypeID}'";

            MyUtility.Check.Seek(sqlGetMachineType_Detail, out DataRow dataRow, "Production");

            if (dataRow != null)
            {
                this.CurrentDetailData["IsSubprocess"] = dataRow["IsSubprocess"];
                this.CurrentDetailData["IsNonSewingLine"] = dataRow["IsNonSewingLine"];
                this.CurrentDetailData["IsNonSewingLineEditable"] = dataRow["IsNonSewingLineEditable"];
            }
            else
            {
                this.CurrentDetailData["IsSubprocess"] = 0;
                this.CurrentDetailData["IsNonSewingLine"] = 0;
                this.CurrentDetailData["IsNonSewingLineEditable"] = 0;
            }
        }

        /// <summary>
        /// OnDetailGridSetup()
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings template = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings pardID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings ppa = new DataGridViewGeneratorTextColumnSettings();
            TxtMachineGroup.CelltxtMachineGroup txtSubReason = (TxtMachineGroup.CelltxtMachineGroup)TxtMachineGroup.CelltxtMachineGroup.GetGridCell();

            #region Seq & Operation Code & Frequency & SMV & ST/MC Type & Attachment按右鍵與Validating
            #region Operation Code
            this.operation.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                            if (dr["CodeFrom"].ToString().StartsWith("AT"))
                            {
                                DataTable dataSource = (DataTable)this.detailgridbs.DataSource;
                                var frm = new P01_Operation_AT(new string[] { "AT" }, ref dataSource, ref this.p01_OperationList, dr["IETMSUkey"].ToString(), dr["CodeFrom"].ToString(), strTimeStudyID: this.strTimeStudtID);
                                frm.ShowDialog();
                                this.detailgridbs.DataSource = dataSource;
                            }
                            else
                            {
                                P01_SelectOperationCode callNextForm = new P01_SelectOperationCode();
                                DialogResult result = callNextForm.ShowDialog(this);
                                if (result == DialogResult.Cancel)
                                {
                                    if (callNextForm.P01SelectOperationCode != null)
                                    {
                                        dr["OperationID"] = callNextForm.P01SelectOperationCode["ID"].ToString();
                                        dr["OperationDescEN"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                                        dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                                        dr["Mold"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Attachment')"); // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                                        dr["Template"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Template')");
                                        dr["MtlFactorID"] = callNextForm.P01SelectOperationCode["MtlFactorID"].ToString();
                                        dr["SeamLength"] = callNextForm.P01SelectOperationCode["SeamLength"].ToString();
                                        dr["SMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]) * 60;
                                        dr["StdSMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]) * 60;
                                        dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]);
                                        dr["Frequency"] = 1;
                                        dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                                        dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                                        dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                                        dr["MachineType_IsSubprocess"] = callNextForm.P01SelectOperationCode["MachineType_IsSubprocess"];
                                        dr["IsSubprocess"] = callNextForm.P01SelectOperationCode["IsSubprocess"];
                                        dr["IsNonSewingLine"] = callNextForm.P01SelectOperationCode["IsNonSewingLine"];
                                        dr["IsShow"] = 1;
                                        dr["IsAdd"] = 1;
                                        this.GetMachineType_Detail(dr["MachineTypeID"].ToString());
                                        dr.EndEdit();
                                    }
                                }

                                if (result == DialogResult.OK)
                                {
                                    dr["OperationID"] = callNextForm.P01SelectOperationCode["ID"].ToString();
                                    dr["OperationDescEN"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                                    dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                                    dr["Mold"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Attachment')"); // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                                    dr["Template"] = MyUtility.GetValue.Lookup($"select dbo.GetParseOperationMold('{callNextForm.P01SelectOperationCode["MoldID"]}', 'Template')");
                                    dr["MtlFactorID"] = callNextForm.P01SelectOperationCode["MtlFactorID"].ToString();
                                    dr["SeamLength"] = callNextForm.P01SelectOperationCode["SeamLength"].ToString();
                                    dr["SMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]) * 60;
                                    dr["StdSMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]) * 60;
                                    dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]);
                                    dr["Frequency"] = 1;
                                    dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                                    dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                                    dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                                    dr["MachineType_IsSubprocess"] = callNextForm.P01SelectOperationCode["MachineType_IsSubprocess"];
                                    dr["IsSubprocess"] = callNextForm.P01SelectOperationCode["IsSubprocess"];
                                    dr["IsNonSewingLine"] = callNextForm.P01SelectOperationCode["IsNonSewingLine"];
                                    dr["IsShow"] = 1;
                                    dr["IsAdd"] = 1;
                                    this.GetMachineType_Detail(dr["MachineTypeID"].ToString());
                                    dr.EndEdit();
                                }
                            }

                            this.AfterColumnEdit(dr); // OperationID 右鍵選擇 判斷是否自動帶入並重編 SewingSeq
                        }
                    }
                }
            };

            this.operation.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = MyUtility.Convert.GetString(dr["OperationID"]);
                    string newValue = MyUtility.Convert.GetString(e.FormattedValue);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && oldValue != newValue)
                    {
                        if (e.FormattedValue.ToString().Length > 1 && e.FormattedValue.ToString().Substring(0, 2) == "--")
                        {
                            dr["OperationID"] = newValue;
                            dr["OperationDescEN"] = string.Empty;
                            dr["MachineTypeID"] = string.Empty;
                            dr["Mold"] = string.Empty;
                            dr["Template"] = string.Empty;  // 將[Template]清空
                            dr["MtlFactorID"] = string.Empty;
                            dr["Frequency"] = 0;
                            dr["SeamLength"] = 0;
                            dr["SMV"] = 0;
                            dr["StdSMV"] = 0;
                            dr["IETMSSMV"] = 0;
                            dr["Annotation"] = string.Empty;
                            dr["MasterPlusGroup"] = string.Empty;
                            dr["IsShow"] = 1;
                        }
                        else
                        {
                            // sql參數
                            SqlParameter sp1 = new SqlParameter
                            {
                                ParameterName = "@id",
                                Value = newValue,
                            };

                            IList<SqlParameter> cmds = new List<SqlParameter>
                            {
                                sp1,
                            };

                            string sqlCmd = $@"
select  o.ID,
        o.DescEN,
        o.SMV,
        o.MachineTypeID,
        o.SeamLength,
        [Mold] = dbo.GetParseOperationMold(o.MoldID, 'Attachment'),
        [Template] = dbo.GetParseOperationMold(o.MoldID, 'Template'),
        o.MtlFactorID,
        o.Annotation,
        o.MasterPlusGroup,
        [MachineType_IsSubprocess] = isnull(md.IsSubprocess,0),
        [IsSubprocess] = isnull(md.IsSubprocess,0),
        [IsNonSewingLine] = isnull(md.IsNonSewingLine, 0)
from Operation o WITH (NOLOCK)
left join MachineType_Detail md WITH (NOLOCK) on md.ID = o.MachineTypeID and md.FactoryID = '{Sci.Env.User.Factory}'
where CalibratedCode = 1
and o.ID = @id";
                            DataTable opData;
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out opData);
                            if (result)
                            {
                                if (opData.Rows.Count <= 0)
                                {
                                    MyUtility.Msg.WarningBox(string.Format("< OperationCode: {0} > not found!!!", e.FormattedValue.ToString()));
                                    this.ChangeToEmptyData(dr);
                                }
                                else
                                {
                                    dr["OperationID"] = e.FormattedValue.ToString();
                                    dr["OperationDescEN"] = opData.Rows[0]["DescEN"].ToString();
                                    dr["MachineTypeID"] = opData.Rows[0]["MachineTypeID"].ToString();
                                    dr["Mold"] = opData.Rows[0]["Mold"].ToString(); // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                                    dr["Template"] = opData.Rows[0]["Template"].ToString();
                                    dr["MtlFactorID"] = opData.Rows[0]["MtlFactorID"].ToString();
                                    dr["Frequency"] = 1;
                                    dr["SeamLength"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SeamLength"]);
                                    dr["SMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]) * 60;
                                    dr["StdSMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]) * 60;
                                    dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]);
                                    dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                                    dr["Annotation"] = opData.Rows[0]["Annotation"].ToString();
                                    dr["MachineType_IsSubprocess"] = opData.Rows[0]["MachineType_IsSubprocess"];
                                    dr["IsSubprocess"] = opData.Rows[0]["IsSubprocess"];
                                    dr["IsNonSewingLine"] = opData.Rows[0]["IsNonSewingLine"];
                                    dr["MasterPlusGroup"] = opData.Rows[0]["MasterPlusGroup"].ToString();
                                    dr["IsShow"] = 1;
                                    this.GetMachineType_Detail(opData.Rows[0]["MachineTypeID"].ToString());
                                }
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                                this.ChangeToEmptyData(dr);
                            }
                        }

                        dr.EndEdit();

                        this.AfterColumnEdit(dr); // OperationID 手輸入, 判斷是否自動帶入並重編 SewingSeq
                    }

                    // 若為空則清空相關資料
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.ChangeToEmptyData(dr);
                    }
                }
            };
            #endregion
            #region Frequency
            this.frequency.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) != MyUtility.Convert.GetDecimal(dr["Frequency"]))
                    {
                        if (!MyUtility.Check.Empty(dr["OperationID"]) && dr["OperationID"].ToString().Substring(0, 2) != "--")
                        {
                            dr["Frequency"] = e.FormattedValue.ToString();
                            string smv = MyUtility.GetValue.Lookup(string.Format("select SMV from Operation WITH (NOLOCK) where ID = '{0}'", dr["OperationID"].ToString()));
                            if (smv == string.Empty)
                            {
                                dr["SMV"] = 0;
                                dr["IETMSSMV"] = 0;
                                dr["MtlFactorID"] = string.Empty;
                            }
                            else
                            {
                                string mtlFactorID = MyUtility.GetValue.Lookup(string.Format("select MtlFactorID from Operation WITH (NOLOCK) where ID = '{0}'", dr["OperationID"].ToString()));
                                dr["MtlFactorID"] = mtlFactorID;
                                dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(smv) * MyUtility.Convert.GetDecimal(dr["Frequency"]);
                                dr["SMV"] = MyUtility.Convert.GetDecimal(smv) * MyUtility.Convert.GetDecimal(dr["Frequency"]) * 60;
                            }
                        }

                        dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region SMV
            this.smvsec.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    // if (MyUtility.Convert.GetDecimal(e.FormattedValue) == MyUtility.Convert.GetDecimal(dr["SMV"]))
                    // {
                    dr["SMV"] = e.FormattedValue.ToString();
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) == 0)
                    {
                        dr["PcsPerHour"] = 0;
                    }
                    else
                    {
                        dr["PcsPerHour"] = MyUtility.Convert.GetDouble(dr["SMV"]) == 0 ? 0 : MyUtility.Math.Round(3600.0 / MyUtility.Convert.GetDouble(dr["SMV"]), 1);
                    }

                    dr.EndEdit();

                    // }
                }
            };
            #endregion
            #region ST/MC Type
            this.machine.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                if (e.RowIndex != -1)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    string sqlCmd = "select ID,Description from MachineType WITH (NOLOCK) where Junk = 0";
                    SelectItem item = new SelectItem(sqlCmd, "8,35", dr["MachineTypeID"].ToString());
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.GetMachineType_Detail(item.GetSelectedString());

                    e.EditingControl.Text = item.GetSelectedString();
                }
            };

            this.machine.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["MachineTypeID"] = string.Empty;
                    this.CurrentDetailData["IsSubprocess"] = 0;
                    this.CurrentDetailData["IsNonSewingLine"] = 0;
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                if (e.FormattedValue.ToString() == dr["MachineTypeID"].ToString())
                {
                    return;
                }

                if (!MyUtility.Check.Seek(string.Format("select ID,Description from MachineType WITH (NOLOCK) where Junk = 0 and ID = '{0}'", e.FormattedValue.ToString())))
                {
                    dr["MachineTypeID"] = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< ST/MC Type: {0} > not found!!!", e.FormattedValue.ToString()));
                    return;
                }

                this.GetMachineType_Detail(e.FormattedValue.ToString());

                dr["MachineTypeID"] = e.FormattedValue.ToString();
            };

            #endregion
            #region Attachment
            this.mold.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select ID,DescEN 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsAttachment = 1
";
                    SelectItem2 item = new SelectItem2(sqlcmd, "ID,DescEN", "13,60,10", this.CurrentDetailData["Mold"].ToString(), null, null, null)
                    {
                        Width = 666,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (MyUtility.Convert.GetString(this.CurrentDetailData["Mold"]) != item.GetSelectedString())
                    {
                        this.CurrentDetailData["SewingMachineAttachmentID"] = string.Empty;
                    }

                    this.CurrentDetailData["Mold"] = item.GetSelectedString();
                }
            };

            this.mold.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(this.CurrentDetailData["OperationID"]) } };
                    string sqlcmd = @"
select [Mold] = STUFF((
		        select concat(',' ,s.Data)
		        from SplitString(o.MoldID, ';') s
		        inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		        where m.IsAttachment = 1
                and m.Junk = 0
		        for xml path ('')) 
	        ,1,1,'')
	,[Template] = STUFF((
		            select concat(',' ,s.Data)
		            from SplitString(o.MoldID, ';') s
		            inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		            where m.IsTemplate = 1
                    and m.Junk = 0
		            for xml path ('')) 
	            ,1,1,'')
from Operation o WITH (NOLOCK) 
where o.ID = @OperationID";
                    DataTable dtOperation;
                    DualResult result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtOperation);
                    List<string> operationList = new List<string>();
                    if (!result)
                    {
                        this.CurrentDetailData["Mold"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    if (dtOperation.Rows.Count > 0)
                    {
                        operationList = dtOperation.Rows[0]["Mold"].ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            this.CurrentDetailData["Mold"] = dtOperation.Rows[0]["Mold"].ToString();
                            return;
                        }
                    }

                    sqlcmd = @"
select ID 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsAttachment = 1
union all
select ID
from SewingMachineAttachment WITH (NOLOCK) 
where Junk = 0";
                    DataTable dtMold;
                    result = DBProxy.Current.Select(null, sqlcmd, out dtMold);
                    if (!result)
                    {
                        this.CurrentDetailData["Mold"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                    List<string> getMold = e.FormattedValue.ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                    // 不存在 Mold
                    var existsMold = getMold.Except(dtMold.AsEnumerable().Select(x => x.Field<string>("ID")).ToList());
                    getMold.AddRange(operationList);
                    if (existsMold.Any())
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["Mold"] = string.Join(",", getMold.Where(x => existsMold.Where(y => !y.EqualString(x)).Any()).Distinct().ToList());
                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsMold.ToList()) + "  need include in Mold setting !!", "Data need include in setting");
                        return;
                    }

                    if (MyUtility.Convert.GetString(this.CurrentDetailData["Mold"]) != string.Join(",", getMold.Distinct().ToList()))
                    {
                        this.CurrentDetailData["SewingMachineAttachmentID"] = string.Empty;
                    }

                    this.CurrentDetailData["Mold"] = string.Join(",", getMold.Distinct().ToList());
                }
            };
            #endregion
            #region template
            template.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select PartID = smt.ID , m.DescEN ,MoldID = m.ID
from Mold m WITH (NOLOCK)
right join SewingMachineTemplate smt on m.ID = smt.MoldID
where m.Junk = 0 and m.IsTemplate = 1 and smt.Junk = 0
UNION
SELECT PartID=ID , DescEN ,MoldID = ID
from Mold
where Junk=0 and IsTemplate=1
";

                    SelectItem2 item = new SelectItem2(sqlcmd, "MoldID,DescEN,PartID", "13,60,20", this.CurrentDetailData["Template"].ToString(), null, null, null)
                    {
                        Width = 1000,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> selectedRows = item.GetSelecteds();

                    if (selectedRows.Any())
                    {
                        var t = selectedRows.ToList();
                        this.CurrentDetailData["Template"] = string.Join(",", t.Select(o => o["PartID"]).ToList());
                    }
                    else
                    {
                        this.CurrentDetailData["Template"] = string.Empty;
                    }
                }
            };

            template.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(this.CurrentDetailData["OperationID"]) } };
                    string sqlcmd = @"
select [Mold] = STUFF((
		        select concat(',' ,s.Data)
		        from SplitString(o.MoldID, ';') s
		        inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		        where m.IsAttachment = 1
                and m.Junk = 0
		        for xml path ('')) 
	        ,1,1,'')
	,[Template] = STUFF((
		            select concat(',' ,s.Data)
		            from SplitString(o.MoldID, ';') s
		            inner join Mold m WITH (NOLOCK) on s.Data = m.ID
		            where m.IsTemplate = 1
                    and m.Junk = 0
		            for xml path ('')) 
	            ,1,1,'')
from Operation o WITH (NOLOCK) 
where o.ID = @OperationID";
                    DataTable dtOperation;
                    DualResult result = DBProxy.Current.Select(null, sqlcmd, cmds, out dtOperation);
                    List<string> operationList = new List<string>();
                    if (!result)
                    {
                        this.CurrentDetailData["Mold"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    if (dtOperation.Rows.Count > 0)
                    {
                        operationList = dtOperation.Rows[0]["Mold"].ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            this.CurrentDetailData["Template"] = dtOperation.Rows[0]["Template"].ToString();
                            return;
                        }
                    }

                    this.CurrentDetailData["Template"] = e.FormattedValue;
                    sqlcmd = @"
select PartID = smt.ID , m.DescEN ,MoldID = m.ID
from Mold m WITH (NOLOCK)
right join SewingMachineTemplate smt on m.ID = smt.MoldID
where m.Junk = 0 and m.IsTemplate = 1 and smt.Junk = 0
UNION
SELECT PartID=ID , DescEN ,MoldID = ID
from Mold
where Junk=0 and IsTemplate=1
";
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["Template"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errTemplate = new List<string>();
                    List<string> trueTemplate = new List<string>();
                    foreach (string item in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["PartID"].EqualString(item)) && !item.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errTemplate.Add(item);
                        }
                        else if (!item.EqualString(string.Empty))
                        {
                            trueTemplate.Add(item);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Template : " + string.Join(",", errTemplate.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueTemplate.Sort();
                    this.CurrentDetailData["Template"] = string.Join(",", trueTemplate.ToArray());
                }
            };
            #endregion
            #region Part ID
            pardID.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex != -1)
                    {
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                        if (MyUtility.Check.Empty(dr["Mold"]))
                        {
                            return;
                        }

                        P01_PartID callNextForm = new P01_PartID(MyUtility.Convert.GetString(dr["Mold"]), MyUtility.Convert.GetString(dr["SewingMachineAttachmentID"]));
                        DialogResult result = callNextForm.ShowDialog(this);

                        if (result == DialogResult.Cancel)
                        {
                            if (callNextForm.P01SelectPartID != null)
                            {
                                dr["SewingMachineAttachmentID"] = callNextForm.P01SelectPartID["ID"].ToString();
                                dr.EndEdit();
                            }
                        }

                        if (result == DialogResult.OK)
                        {
                            if (callNextForm.P01SelectPartID != null)
                            {
                                dr["SewingMachineAttachmentID"] = callNextForm.P01SelectPartID["ID"].ToString();
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };

            pardID.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        return;
                    }

                    string newSewingMachineAttachmentID = MyUtility.Convert.GetString(e.FormattedValue);
                    string moldID = this.CurrentDetailData["Mold"].ToString();

                    string sqlcmd = $@"
select a.ID
    ,a.Description
    ,a.MachineMasterGroupID
    ,AttachmentTypeID
    ,MeasurementID
    ,FoldTypeID
from SewingMachineAttachment a
left join AttachmentType b on a.AttachmentTypeID = b.Type 
left join AttachmentMeasurement c on a.MeasurementID = c.Measurement
left join AttachmentFoldType d on a.FoldTypeID = d.FoldType 
where a.MoldID IN ('{string.Join("','", moldID.Split(','))}') ";

                    List<SqlParameter> paras = new List<SqlParameter>();

                    // SewingMachineAttachment.ID可以多選
                    if (newSewingMachineAttachmentID.Split(',').Length > 1)
                    {
                        List<string> paraList = new List<string>();
                        int idx = 0;
                        foreach (var item in newSewingMachineAttachmentID.Split(','))
                        {
                            paraList.Add($@"@item{idx}");
                            paras.Add(new SqlParameter($@"@item{idx}", item));
                            idx++;
                        }

                        sqlcmd += $@" AND a.ID IN ({string.Join(",", paraList)})";
                    }
                    else
                    {
                        sqlcmd += " AND a.ID = @ID";
                        paras.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(e.FormattedValue)));
                    }

                    DataTable dt;

                    DualResult r = DBProxy.Current.Select(null, sqlcmd, paras, out dt);

                    if (!r)
                    {
                        e.Cancel = true;
                        this.ShowErr(r);
                        return;
                    }

                    if (dt.Rows == null || dt.Rows.Count == 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Data not found");
                    }
                }
            };
            #endregion
            #region PPA
            ppa.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select PPAID=ID,PPA=Name 
from DropDownList 
where Type = 'PMS_IEPPA'
";

                    SelectItem item = new SelectItem(sqlcmd, "PPA ID,PPA", "10,10", this.CurrentDetailData["PPA"].ToString(), null, null, null)
                    {
                        Width = 666,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    DataRow dr = item.GetSelecteds()[0];
                    this.CurrentDetailData["PPA"] = dr["PPAID"];
                    this.CurrentDetailData["PPAText"] = dr["PPA"];
                }
            };

            ppa.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["PPA"] = string.Empty;
                        this.CurrentDetailData["PPAText"] = string.Empty;
                        return;
                    }

                    string newSewingMachineAttachmentID = MyUtility.Convert.GetString(e.FormattedValue);

                    string sqlcmd = $@"
select PPAID=ID,PPA=Name 
from DropDownList 
where Type = 'PMS_IEPPA'
and Name = @PPA
";

                    DataTable dt;
                    List<SqlParameter> paras = new List<SqlParameter>()
                                        {
                                            new SqlParameter("@PPA", MyUtility.Convert.GetString(e.FormattedValue)),
                                        };
                    DualResult r = DBProxy.Current.Select(null, sqlcmd, paras, out dt);

                    if (!r)
                    {
                        e.Cancel = true;
                        this.ShowErr(r);
                        return;
                    }

                    if (dt.Rows == null || dt.Rows.Count == 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Data not found");
                    }
                    else
                    {
                        DataRow dr = dt.Rows[0];
                        this.CurrentDetailData["PPA"] = dr["PPAID"];
                        this.CurrentDetailData["PPAText"] = dr["PPA"];
                    }
                }
            };
            #endregion
            #endregion

            template.MaxLength = 100;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(1), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Seq", header: "Ori.\r\nSeq", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("DesignateSeq", header: "Dsg.\r\nseq", width: Widths.AnsiChars(3), settings: this.Col_Seq())
                .Text("SewingSeq", header: "Sew.\r\nseq", width: Widths.AnsiChars(3), settings: this.Col_Seq())
                .Text("Location", header: "Location", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("OperationID", header: "Operation code", width: Widths.AnsiChars(13), settings: this.operation)
                .EditText("OperationDescEN", header: "Operation Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(30))
                .Numeric("Frequency", header: "Frequency", width: Widths.AnsiChars(3), integer_places: 2, decimal_places: 2, maximum: 99.99M, minimum: 0, settings: this.frequency)
                .Numeric("StdSMV", header: "Std. SMV\r\n(sec)", width: Widths.AnsiChars(3), integer_places: 4, decimal_places: 4, maximum: 9999.9999M, minimum: 0, iseditingreadonly: true)
                .Numeric("SMV", header: "Fty. SMV\r\n(sec)", width: Widths.AnsiChars(3), integer_places: 4, decimal_places: 4, maximum: 9999.9999M, minimum: 0, settings: this.smvsec)
                .Text("MachineTypeID", header: "ST/MC\r\nType", width: Widths.AnsiChars(3), settings: this.machine)
                .Text("MasterPlusGroup", header: "Machine\r\nGroup", width: Widths.AnsiChars(5), settings: txtSubReason)
                .Text("Mold", header: "Attachment", width: Widths.AnsiChars(8), settings: this.mold)
                .Text("SewingMachineAttachmentID", header: "Part ID", width: Widths.AnsiChars(7), settings: pardID)
                .Text("Template", header: "Template", width: Widths.AnsiChars(8), settings: template)
                .CheckBox("IsSubprocess", header: "Subprocess", width: Widths.AnsiChars(7), iseditable: false, trueValue: 1, falseValue: 0)
                .CheckBox("IsNonSewingLine", header: "Non-Sewing line", width: Widths.AnsiChars(7), iseditable: true, trueValue: 1, falseValue: 0, settings: this.Col_IsNonSewingLine())
                .Text("PPAText", header: "PPA", width: Widths.AnsiChars(10), settings: ppa)
                .CellThreadComboID("Thread_ComboID", "Thread Combination", this, width: Widths.AnsiChars(10))
                .Numeric("Sewer", header: "Sewer", integer_places: 2, decimal_places: 1, iseditingreadonly: true)
                .Numeric("PcsPerHour", header: "Pcs/hr", integer_places: 5, decimal_places: 1, iseditingreadonly: true)
                .Numeric("IETMSSMV", header: "Std. SMV", integer_places: 3, decimal_places: 4, iseditingreadonly: true)
                ;

            // 設定detailGrid Rows 是否可以編輯
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
            this.detailgrid.ColumnHeaderMouseClick += this.Detailgrid_ColumnHeaderMouseClick;
            this.detailgrid.Sorted += (s, e) =>
            {
                this.HideRows();
            };
        }

        /// <summary>
        /// 欄位:DesignateSeq 與 SewingSeq
        /// </summary>
        private DataGridViewGeneratorTextColumnSettings Col_Seq()
        {
            DataGridViewGeneratorTextColumnSettings setting = new DataGridViewGeneratorTextColumnSettings();
            setting.MaxLength = 4;
            setting.EditingKeyPress += (s, e) =>
            {
                // 限制只能輸入數字
                string input = e.EditingControl.Text;
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true; // 阻止非數字字符的輸入
                }
            };

            setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string columnName = this.detailgrid.Columns[e.ColumnIndex].DataPropertyName;
                string oldValue = MyUtility.Convert.GetString(dr[columnName]);
                string newValue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldValue == newValue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (columnName == "SewingSeq")
                    {
                        if (oldValue == string.Empty) // 原本就是空的, 點了不要重排
                        {
                            return;
                        }

                        dr[columnName] = string.Empty;
                        dr.EndEdit();
                        this.UpdateRowIndexBasedOnSewingSeqAndSeq(oldValue); // 從有值 → 清空 SewingSeq
                    }

                    return;
                }

                if (columnName == "SewingSeq")
                {
                    int sewingSeq = MyUtility.Convert.GetInt(e.FormattedValue);
                    this.InsertSewingSeqAndAdjust(sewingSeq); // 手動輸入 Sew Seq 有值

                    // 左邊補 0 至 4 碼
                    dr[columnName] = sewingSeq.ToString().PadLeft(4, '0');
                    this.UpdateRowIndexBasedOnSewingSeqAndSeq(oldValue); // 手動輸入Sew Seq 處理完重複後
                }
                else
                {
                    // 左邊補 0 至 4 碼
                    dr[columnName] = e.FormattedValue.ToString().PadLeft(4, '0');
                    this.MappingDesignateSeqSewingSeq(dr);
                }

                dr.EndEdit();
            };

            return setting;
        }

        private void MappingDesignateSeqSewingSeq(DataRow drDesignateSeq)
        {
            var matchingRow = this.DetailDatas.FirstOrDefault(r => r["SewingSeq"].EqualString(drDesignateSeq["DesignateSeq"]));
            drDesignateSeq["DesignateSeqIdetityKey"] = matchingRow != null ? matchingRow["IdetityKey"] : DBNull.Value;
        }


        private DataGridViewGeneratorCheckBoxColumnSettings Col_IsNonSewingLine()
        {
            DataGridViewGeneratorCheckBoxColumnSettings setting = new DataGridViewGeneratorCheckBoxColumnSettings();
            setting.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;
            setting.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                dr["Isnonsewingline"] = e.FormattedValue;
                dr.EndEdit();

                this.AfterColumnEdit(dr); // IsNonSewingLine 勾選/取消勾選, 判斷是否自動帶入並重編 SewingSeq
            };

            return setting;
        }

        private void Detailgrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.detailgridbs == null)
            {
                return;
            }

            if (this.detailgrid.CurrentCell == null)
            {
                return;
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Convert.GetBool(dr["MachineType_IsSubprocess"]) == false)
                    {
                        dr["IsSubprocess"] = 0;
                    }
                    else
                    {
                        if (MyUtility.Convert.GetBool(dr["IsSubprocess"]) == false)
                        {
                            dr["IsSubprocess"] = 0;
                        }
                    }
                }
            }
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || !this.EditMode)
            {
                return;
            }

            if (this.detailgridbs == null)
            {
                return;
            }
        }

        private void ChangeToEmptyData(DataRow dr)
        {
            dr["OperationID"] = string.Empty;
            dr["OperationDescEN"] = string.Empty;
            dr["MachineTypeID"] = string.Empty;
            dr["Mold"] = string.Empty;
            dr["Template"] = string.Empty;
            dr["MtlFactorID"] = string.Empty;
            dr["Frequency"] = 0;
            dr["SeamLength"] = 0;
            dr["SMV"] = 0;
            dr["StdSMV"] = 0;
            dr["IETMSSMV"] = 0;
        }

        /// <summary>
        /// ClickNewAfter()
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Phase"] = "Estimate";
            this.CurrentMaintain["Version"] = "01";
            this.CurrentMaintain["Status"] = "New";
        }

        /// <summary>
        /// ClickCopyBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickCopyBefore()
        {
            if (!this.CurrentMaintain["Status"].ToString().ToLower().EqualString("confirmed"))
            {
                MyUtility.Msg.WarningBox("This flowchart has not been confirmed and cannot be copied.");
                return false;
            }

            P01_Copy callNextForm = new P01_Copy(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                this.styleID = callNextForm.P01CopyStyleData.Rows[0]["ID"].ToString();
                this.seasonID = callNextForm.P01CopyStyleData.Rows[0]["SeasonID"].ToString();
                this.brandID = callNextForm.P01CopyStyleData.Rows[0]["BrandID"].ToString();
                this.comboType = callNextForm.P01CopyStyleData.Rows[0]["Location"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ClickCopyAfter()
        /// </summary>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["StyleID"] = this.styleID;
            this.CurrentMaintain["SeasonID"] = this.seasonID;
            this.CurrentMaintain["BrandID"] = this.brandID;
            this.CurrentMaintain["ComboType"] = this.comboType;
            this.CurrentMaintain["Version"] = "01";
            this.CurrentMaintain["ID"] = 0;

            // 如果SewingSeq都空白就自動補
            if (!this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["SewingSeq"])))
            {
                this.FillSewingSeqAfterCopy();
            }
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            base.ClickEditBefore();
            if (this.CurrentMaintain["Status"].ToString().ToLower().EqualString("confirmed"))
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't modify this record!!");
                this.HideRows();
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            // 如果SewingSeq有值,Sew Sew按鈕就read only
            if (this.DetailDatas.Any(r => !MyUtility.Check.Empty(r["SewingSeq"])))
            {
                this.btnSewSeq.Enabled = false;
            }
        }

        /// <summary>
        /// ClickDeleteBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString().ToLower().EqualString("confirmed"))
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't modify this record!!");
                return false;
            }

            if (MyUtility.Check.Seek(string.Format(@"select ID from SewingOutput_Detail WITH (NOLOCK) where OrderId in (select ID from Orders WITH (NOLOCK) where StyleID = '{0}' and BrandID = '{1}' and SeasonID = '{2}')", this.CurrentMaintain["StyleID"].ToString(), this.CurrentMaintain["BrandID"].ToString(), this.CurrentMaintain["SeasonID"].ToString())))
            {
                MyUtility.Msg.WarningBox("Sewing output > 0, can't be deleted!!");
                return false;
            }
            #region 用來增加CurrentDataRow ID的欄位
            /*
             * 如果不給browse增加ID欄位mapping 表身table
             * 刪除時,就無法正確對應並刪除表身的資料
             * 所以必須給currentDataRow增加ID
             */
            DataRow dr = this.CurrentDataRow;
            if (!dr.Table.Columns.Contains("ID"))
            {
                dr.Table.ColumnsIntAdd("ID");
            }

            dr["ID"] = this.CurrentMaintain["ID"].ToString();
            dr.AcceptChanges();
            #endregion
            return true;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            #region 應該上產線卻沒有 SewingSeq. SewingSeq 空白, OperationID 是--開頭, IsNonSewingLine = False 不能存檔
            var checkSewing = this.DetailDatas.AsEnumerable().Where(r => MyUtility.Check.Empty(r["SewingSeq"]) && this.NeedSewingSeq(r)).ToList();
            if (checkSewing.Any())
            {
                string oriSeq = checkSewing.Select(r => MyUtility.Convert.GetString(r["Seq"])).JoinToString(",");
                MyUtility.Msg.WarningBox($"Ori. Seq<{oriSeq}> must fill in Sew. Seq!");
                return false;
            }
            #endregion

            #region Seq 重複資料檢查
            if (this.DetailDatas.Count != 0)
            {
                var dataTable = this.DetailDatas.CopyToDataTable();
                var duplicateSeqs = dataTable.AsEnumerable()
                 .GroupBy(row => row.Field<string>("Seq"))
                 .Where(grp => grp.Count() > 1)
                 .Select(grp => grp.Key).ToList();

                if (duplicateSeqs.Any())
                {
                    string seqError = string.Empty;
                    foreach (var duplicateSeq in duplicateSeqs)
                    {
                        seqError += Environment.NewLine + $"'{duplicateSeq}'";
                    }

                    string strMsg = $"[Seq] cannot be duplicated! {seqError}";
                    MyUtility.Msg.WarningBox(strMsg);
                    return false;
                }
            }
            #endregion Seq 重複資料檢查

            #region SewingSeq 重複資料檢查
            var sewingSeqs = this.DetailDatas.Where(row => !MyUtility.Check.Empty(row["SewingSeq"])).Select(row => row["SewingSeq"].ToString()).ToList();
            var duplicateSewSeqs = sewingSeqs.GroupBy(seq => seq).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
            if (duplicateSewSeqs.Any())
            {
                MyUtility.Msg.WarningBox("Sew. Seq cannot be duplicated! Duplicates: " + string.Join(", ", duplicateSewSeqs));
                return false;
            }
            #endregion SewingSeq 重複資料檢查

            #region ST/MC Type檢查
            var listSTMCTypeCheckResult = this.DetailDatas
                .Where(s => !MyUtility.Check.Empty(s["OperationDescEN"]) &&
                            (MyUtility.Check.Empty(s["MachineTypeID"]) ||
                             (!s["MachineTypeID"].ToString().StartsWith("MM") && MyUtility.Check.Empty(s["MasterPlusGroup"]))));

            if (listSTMCTypeCheckResult.Any())
            {
                string errMsgSTMCTypeCheck = $@"
[ST/MC Type] or [Machine Group] can not be empty since it is operation.
{listSTMCTypeCheckResult.Select(s => s["Seq"].ToString()).JoinToString(Environment.NewLine)}";
                MyUtility.Msg.WarningBox(errMsgSTMCTypeCheck);
                return false;
            }
            #endregion
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]))
            {
                MyUtility.Msg.WarningBox("Style can't empty");
                this.txtStyle.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SeasonID"]))
            {
                MyUtility.Msg.WarningBox("Season can't empty");
                this.txtseason.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't empty");
                this.txtBrand.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ComboType"]))
            {
                MyUtility.Msg.WarningBox("Combo type can't empty");
                this.comboStyle.Focus();
                return false;
            }
            #endregion
            #region 檢查輸入的資料是否存在系統

            // sql參數
            SqlParameter sp1 = new SqlParameter();
            SqlParameter sp2 = new SqlParameter();
            SqlParameter sp3 = new SqlParameter();
            SqlParameter sp4 = new SqlParameter();
            sp1.ParameterName = "@styleid";
            sp1.Value = this.CurrentMaintain["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.CurrentMaintain["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = this.CurrentMaintain["BrandID"].ToString();
            sp4.ParameterName = "@combotype";
            sp4.Value = this.CurrentMaintain["ComboType"].ToString();

            IList<SqlParameter> cmds = new List<SqlParameter>
            {
                sp1,
                sp2,
                sp3,
                sp4,
            };
            string sqlCmd = "select sl.Location from Style s WITH (NOLOCK) , Style_Location sl WITH (NOLOCK) where s.ID = @styleid and s.SeasonID = @seasonid and s.BrandID = @brandid and s.Ukey = sl.StyleUkey and sl.Location = @combotype";
            DataTable locationData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out locationData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL connection fail!!\r\n" + result.ToString());
                return false;
            }
            else
            {
                if (locationData.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("This style not correct, can't save. [Style_Location] table don't have relation data.");
                    return false;
                }
            }

            #endregion
            #region 檢查表身不可為空
            if (((DataTable)this.detailgridbs.DataSource).DefaultView.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty");
                return false;
            }
            #endregion
            #region 檢查是否為套裝(Style.styleunit <> 'PCS')
            if (MyUtility.Check.Seek(string.Format(
                    @" 
select 1
from TimeStudy t
inner join style s on t.styleid=s.ID 
                  and t.BrandID=s.BrandID 
                  and t.SeasonID=s.SeasonID
where t.id <> {0} and t.StyleID='{1}' 
and t.BrandID='{2}' and t.SeasonID='{3}'
and s.StyleUnit='PCS'
",
                    MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? 0 : this.CurrentMaintain["ID"],
                    this.CurrentMaintain["StyleID"],
                    this.CurrentMaintain["BrandID"],
                    this.CurrentMaintain["SeasonID"])))
            {
                MyUtility.Msg.WarningBox(string.Format(@"The Stytle：{0}、Season：{1}、Brand：{2} styleunit is 'PCS', You can only create one data in P01. Factory GSD.", this.CurrentMaintain["StyleID"], this.CurrentMaintain["SeasonID"], this.CurrentMaintain["BrandID"]));
                return false;
            }

            #endregion

            #region 回寫表頭的Total SMV
            decimal ttlSMV = MyUtility.Convert.GetDecimal(((DataTable)this.detailgridbs.DataSource).Compute("sum(SMV)", string.Empty));
            this.numTotalSMV.Text = Convert.ToInt32(ttlSMV).ToString();
            #endregion

            #region 寫表頭的Total Sewing Time與表身的Sewer，只把ArtworkTypeID = 'SEWING'的秒數抓進來加總

            // 不分LocalStyle,且都只帶出ArtworkTypeID = sewing. by ISP20240873
            var machineSMV_List = ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(o => o.RowState != DataRowState.Deleted &&
                            !MyUtility.Convert.GetBool(o["IsSubprocess"]) &&
                            MyUtility.Convert.GetString(o["PPA"]) != "C")
                .Select(o => new { MachineTypeID = o["MachineTypeID"].ToString(), SMV = MyUtility.Convert.GetDecimal(o["SMV"]) })
                .ToList();
            DataTable tmp;
            DBProxy.Current.Select(null, " SELECT ID FROM Machinetype WHERE ArtworkTypeID = 'SEWING' ", out tmp);
            List<string> sewingMachine_List = tmp.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

            decimal ttlSewingTime = machineSMV_List.Where(o => sewingMachine_List.Contains(o.MachineTypeID)).Sum(o => o.SMV);

            this.CurrentMaintain["TotalSewingTime"] = MyUtility.Convert.GetInt(ttlSewingTime);

            string totalSewing = this.CurrentMaintain["TotalSewingTime"].ToString();
            this.numTotalSewingTimePc.Text = totalSewing;
            #endregion

            #region Total GSD 檢查

            // 先取得 Style.Ukey
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = this.CurrentMaintain["StyleID"].ToString() },
                new SqlParameter() { ParameterName = "@seasonid", Value = this.CurrentMaintain["SeasonID"].ToString() },
                new SqlParameter() { ParameterName = "@brandid", Value = this.CurrentMaintain["BrandID"].ToString() },
            };
            sqlCmd = "select Ukey from Style WITH (NOLOCK) where ID = @id and SeasonID = @seasonid and BrandID = @brandid";
            DataTable styleDT;
            result = DBProxy.Current.Select(null, sqlCmd, parameters, out styleDT);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return false;
            }

            long styleUkey = 0;
            if (styleDT.Rows.Count > 0)
            {
                styleUkey = MyUtility.Convert.GetLong(styleDT.Rows[0]["UKey"]);

                DataTable dtGSD_Summary;
                sqlCmd = $@" 
select tms = cast(CEILING(sum(i.ProSMV) * 60) as decimal(20,2))
from IETMS_Summary i
where i.IETMSUkey = (select distinct i.Ukey from Style s WITH (NOLOCK) inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version where s.ukey = '{styleUkey}')
group by i.Location,i.ArtworkTypeID";
                result = DBProxy.Current.Select(null, sqlCmd, out dtGSD_Summary);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Check <Total Sewing Time/pc> fail!\r\n" + result.ToString());
                }

                // 若舊資料IETMS_Summary沒有資料
                if (dtGSD_Summary.Rows.Count == 0)
                {
                    sqlCmd = $@" 
select cast(round(sum(isnull(id.smv,0)*id.Frequency*(isnull(id.MtlFactorRate,0)/100+1)*60),0) as decimal(20,2)) as tms 
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
inner join Operation o WITH (NOLOCK) on id.OperationID = o.ID
inner join MachineType m WITH (NOLOCK) on o.MachineTypeID = m.ID
where s.Ukey = {styleUkey}
group by id.Location,M.ArtworkTypeID";
                    result = DBProxy.Current.Select(null, sqlCmd, out dtGSD_Summary);
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Check <Total Sewing Time/pc> fail!\r\n" + result.ToString());
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Style not found!!");
            }
            #endregion

            decimal allSewer = MyUtility.Check.Empty(this.CurrentMaintain["NumberSewer"]) ? 0.0m : MyUtility.Convert.GetDecimal(this.CurrentMaintain["NumberSewer"]);
            foreach (DataRow dr in this.DetailDatas)
            {
                if (!MyUtility.Convert.GetBool(dr["IsSubprocess"]) &&
                   !MyUtility.Convert.GetBool(dr["IsNonSewingLine"]) &&
                   MyUtility.Convert.GetString(dr["PPA"]) != "C")
                {
                    dr["Sewer"] = ttlSewingTime == 0 ? 0 : MyUtility.Math.Round(allSewer * (MyUtility.Convert.GetDecimal(dr["SMV"]) / ttlSewingTime), 1);
                }
                else
                {
                    dr["Sewer"] = 0;
                }

                if (!MyUtility.Convert.GetBool(dr["IsSubprocess"].ToString()))
                {
                    dr["IsSubprocess"] = 0;
                }
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            dt.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(row.Field<string>("OperationID")) == string.Empty).ToList().ForEach(row => row.Delete());

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // 若ThreadColorComb已有資料要自動發信通知Style.ThreadEditname(去串pass1.EMail若為空或null則不需要發信)，通知使用者資料有變更。
            string sqlcmd = $@"
select distinct p.EMail
from TimeStudy ts with(nolock)
inner join style s with(nolock) on s.id = ts.StyleID and s.SeasonID = ts.SeasonID and s.BrandID = ts.BrandID
inner join ThreadColorComb tcc with(nolock) on tcc.StyleUkey = s.Ukey
inner join pass1 p on p.id = s.ThreadEditname
where p.EMail is not null and p.EMail <>'' and ts.id = '{this.CurrentMaintain["ID"]}'";
            DataRow dr;
            if (MyUtility.Check.Seek(sqlcmd, out dr))
            {
                string toAddress = MyUtility.Convert.GetString(dr[0]);
                string subject = $"IE P01 Factory GSD Style：{this.CurrentMaintain["StyleID"]} ,Brand：{this.CurrentMaintain["BrandID"]} ,Season：{this.CurrentMaintain["SeasonID"]} have changed ";
                string description = $@"Please regenerate Thread P01.Thread Color Combination data.";
                var email = new MailTo(Env.Cfg.MailFrom, toAddress, string.Empty, subject, string.Empty, description, true, true);
                email.ShowDialog();
            }

            this.p01_OperationList.Clear();
            this.HideRows();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            string sqlcmd = string.Empty;
            foreach (var item in this.p01_OperationList)
            {
                sqlcmd += $@"
                UPDATE IETMS_AT SET
                PieceOfSeamerEdited = {item.PieceOfSeamerEdited}
                , RPMEdited  = '{item.RPMEdited}' , LaserSpeedEdited = '{item.LaserSpeedEdited}' 
                WHERE IETMSUkey =  '{item.IETMSUkey}' AND CodeFrom  = '{item.CodeFrom}'

                UPDATE TimeStudy_Detail SET SMV = '{item.MM2AT_SMV}'
                from TimeStudy_Detail td WITH (NOLOCK) 
                INNER JOIN TimeStudy t WITH(NOLOCK) ON td.id = t.id
                INNER JOIN IETMS i ON t.IETMSID = i.ID AND t.IETMSVersion = i.[Version]
                INNER JOIN IETMS_Detail ID ON I.Ukey = ID.IETMSUkey AND ID.SEQ = TD.Seq
                WHERE  
                Id.IETMSUkey = {item.IETMSUkey} and 
                ID.CodeFrom = '{item.CodeFrom}' and 
                td.ID = '{item.ID}' and 
                td.[MachineTypeID] like 'MM2AT%'

                UPDATE TimeStudy_Detail SET SMV = '{item.AT_SMV}'
                from TimeStudy_Detail td WITH (NOLOCK) 
                INNER JOIN TimeStudy t WITH(NOLOCK) ON td.id = t.id
                INNER JOIN IETMS i ON t.IETMSID = i.ID AND t.IETMSVersion = i.[Version]
                INNER JOIN IETMS_Detail ID ON I.Ukey = ID.IETMSUkey AND ID.SEQ = TD.Seq
                WHERE  
                Id.IETMSUkey = {item.IETMSUkey} and 
                ID.CodeFrom = '{item.CodeFrom}' and 
                td.ID = '{item.ID}' and 
                td.[MachineTypeID] like 'AT%'
                ";
            }

            DBProxy.Current.Execute(null, sqlcmd);
            this.p01_OperationList.Clear();

            this.RenewData();
            this.HideRows();
        }

        /// <summary>
        /// OnSaveDetail
        /// </summary>
        /// <param name="details">details</param>
        /// <param name="detailtableschema">detailtableschema</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            return base.OnSaveDetail(details, detailtableschema);
        }

        /// <summary>
        /// ClickPrint()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("No data!!");
                return false;
            }

            P01_Print callNextForm = new P01_Print(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            string sqlcmd = $@"
update t 
    set t.Status = 'Confirmed', 
        t.EditName = '{Env.User.UserID}', 
        t.EditDate = Getdate()
from TimeStudy t 
where StyleID = '{this.CurrentMaintain["StyleID"]}' 
and SeasonID = '{this.CurrentMaintain["SeasonID"]}' 
and ComboType = '{this.CurrentMaintain["ComboType"]}' 
and BrandID = '{this.CurrentMaintain["BrandID"]}'";

            DBProxy.Current.Execute("Production", sqlcmd);
            base.ClickConfirm();
        }

        // Style PopUp
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select ID,SeasonID,Description,BrandID,UKey from Style WITH (NOLOCK) where Junk = 0 order by ID";
            DataTable styleData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out styleData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            SelectItem item = new SelectItem(styleData, "ID,SeasonID,Description,BrandID", "14,6,40,10", this.Text, headercaptions: "Style,Season,Description,Brand")
            {
                Width = 780,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            DataRow selectedData = item.GetSelecteds().FirstOrDefault();
            this.CurrentMaintain["StyleID"] = item.GetSelectedString();
            this.CurrentMaintain["SeasonID"] = MyUtility.Convert.GetString(selectedData["SeasonID"]);
            this.CurrentMaintain["BrandID"] = MyUtility.Convert.GetString(selectedData["BrandID"]);

            sqlCmd = string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetInt(selectedData["UKey"]).ToString());
            DataTable locationData;
            result = DBProxy.Current.Select(null, sqlCmd, out locationData);
            if (result
                && locationData.Rows.Count == 1)
            {
                this.CurrentMaintain["ComboType"] = MyUtility.Convert.GetString(locationData.Rows[0]["Location"]);
            }

            this.GenCD(null, null);  // 撈CD Code
        }

        // Style
        private void TxtStyle_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string styleID = MyUtility.Convert.GetString(this.txtStyle.Text);
            string ukey = string.Empty;
            string sqlCmd = string.Empty;
            DataTable dt = new DataTable();
            DualResult result;
            if (!this.EditMode
                || MyUtility.Check.Empty(styleID)
                || styleID.EqualString(MyUtility.Convert.GetString(this.CurrentMaintain["StyleID"])))
            {
                return;
            }

            IList<SqlParameter> cmds = new List<SqlParameter>()
            {
                 new SqlParameter()
                {
                    ParameterName = "@sytleID",
                    Value = styleID,
                },
            };

            sqlCmd = "select ID, SeasonID, BrandID, Description, UKey from Style WITH (NOLOCK) where ID = @sytleID and Junk = 0 order by ID";
            result = DBProxy.Current.Select(null, sqlCmd, cmds, out dt);
            if (!result)
            {
                this.CurrentMaintain["StyleID"] = string.Empty;
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            if (dt.Rows.Count == 0)
            {
                this.CurrentMaintain["StyleID"] = string.Empty;
                this.CurrentMaintain["SeasonID"] = string.Empty;
                this.CurrentMaintain["BrandID"] = string.Empty;
                MyUtility.Msg.WarningBox("Style not found!!");
                return;
            }

            if (dt.Rows.Count > 1)
            {
                SelectItem item = new SelectItem(dt, "ID,SeasonID,Description,BrandID", "14,6,40,10", this.Text, headercaptions: "Style,Season,Description,Brand")
                {
                    Width = 700,
                };

                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    this.CurrentMaintain["StyleID"] = string.Empty;
                    return;
                }

                IList<DataRow> selectedData = item.GetSelecteds();
                this.CurrentMaintain["StyleID"] = item.GetSelectedString();
                this.CurrentMaintain["SeasonID"] = selectedData[0]["SeasonID"].ToString();
                this.CurrentMaintain["BrandID"] = selectedData[0]["BrandID"].ToString();
                ukey = selectedData[0]["UKey"].ToString();
            }
            else
            {
                this.CurrentMaintain["StyleID"] = dt.Rows[0]["ID"].ToString();
                this.CurrentMaintain["SeasonID"] = dt.Rows[0]["SeasonID"].ToString();
                this.CurrentMaintain["BrandID"] = dt.Rows[0]["BrandID"].ToString();
                ukey = dt.Rows[0]["UKey"].ToString();
            }

            cmds = new List<SqlParameter>()
            {
                 new SqlParameter()
                {
                    ParameterName = "@UKey",
                    Value = ukey,
                },
            };
            sqlCmd = "select Location from Style_Location WITH (NOLOCK) where StyleUkey = @UKey";
            result = DBProxy.Current.Select(null, sqlCmd, cmds, out dt);
            if (!result)
            {
                this.CurrentMaintain["StyleID"] = string.Empty;
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            if (dt.Rows.Count == 1)
            {
                this.CurrentMaintain["ComboType"] = dt.Rows[0]["Location"].ToString();
            }

            this.GenCD(null, null); // 撈CD Code
        }

        // Art. Sum
        private void BtnArtSum_Click(object sender, EventArgs e)
        {
            P01_ArtworkSummary callNextForm = new P01_ArtworkSummary("TimeStudy_Detail", Convert.ToInt64(this.CurrentMaintain["ID"]));
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // Sketch
        private void BtnSketch_Click(object sender, EventArgs e)
        {
            P01_Sketch callNextForm = new P01_Sketch(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // New Version
        private void BtnNewVersion_Click(object sender, EventArgs e)
        {
            // 將現有資料寫入TimeStudyHistory,TimeStudyHistory_History，並將現有資料的Version+1
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to create new version?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                string executeCmd = string.Format(
                    @"
if exists(
	select 1 from TimeStudyHistory h
	inner join TimeStudy t on t.StyleID	   = h.StyleID
						  and t.SeasonID   = h.SeasonID
						  and t.ComboType  = h.ComboType
						  and t.BrandID	   = h.BrandID
						  and t.Version	   = h.Version
						  and t.Phase	   = h.Phase
	where t.ID = '{0}'
)
begin
	update TimeStudy 
	set Version =
		(
			select iif(isnull(max(ver.Version),0)+1 < 10,'0'+cast(isnull(max(ver.Version),0)+1 as varchar),cast(max(ver.Version)+1as varchar))
			from(
				select Version
				from TimeStudy where ID = {0}
				union
				select h.Version from TimeStudyHistory h
				inner join TimeStudy t on t.StyleID	   = h.StyleID
										and t.SeasonID   = h.SeasonID
										and t.ComboType  = h.ComboType
										and t.BrandID	   = h.BrandID
										and t.Phase	   = h.Phase
				where t.ID = {0}
			)ver
		)
end

insert into TimeStudyHistory (StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion)
select StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion from TimeStudy where ID = {0}

declare @id bigint
select @id = @@IDENTITY

insert into TimeStudyHistory_Detail(ID,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID,StdSMV,Thread_ComboID)
select @id,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID,StdSMV,Thread_ComboID from TimeStudy_Detail where ID = {0}

update TimeStudy 
set Version = (select iif(isnull(max(Version),0)+1 < 10,'0'+cast(isnull(max(Version),0)+1 as varchar),cast(max(Version)+1as varchar)) from TimeStudy where ID = {0}) ,
    AddName = '{1}',
	AddDate = GETDATE(),
	EditName = '',
	EditDate = null,
    Status = 'New'
where ID = {0}",
                    this.CurrentMaintain["ID"].ToString(),
                    Env.User.UserID);
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, executeCmd);
                        if (result)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Click new version  failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.ErrorBox("Connection transaction error.\r\n" + ex.ToString());
                        return;
                    }
                }

                this.RenewData();
                this.ClickEdit();

                this.btnSewSeq.PerformClick();
            }
        }

        // New Status
        private void BtnNewStatus_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain["Phase"].ToString() == "Final")
            {
                MyUtility.Msg.WarningBox("Can't change status!!");
                return;
            }

            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to create new status?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                string executeCmd = string.Format(
                    @"
if exists(
	select 1 from TimeStudyHistory h
	inner join TimeStudy t on t.StyleID	   = h.StyleID
						  and t.SeasonID   = h.SeasonID
						  and t.ComboType  = h.ComboType
						  and t.BrandID	   = h.BrandID
						  and t.Version	   = h.Version
						  and t.Phase	   = h.Phase
	where t.ID = '{0}'
)
begin
	update TimeStudy 
	set Version =
		(
			select iif(isnull(max(ver.Version),0)+1 < 10,'0'+cast(isnull(max(ver.Version),0)+1 as varchar),cast(max(ver.Version)+1as varchar))
			from(
				select Version
				from TimeStudy where ID = {0}
				union
				select h.Version from TimeStudyHistory h
				inner join TimeStudy t on t.StyleID	   = h.StyleID
										and t.SeasonID   = h.SeasonID
										and t.ComboType  = h.ComboType
										and t.BrandID	   = h.BrandID
										and t.Phase	   = h.Phase
				where t.ID = {0}
			)ver
		)
end

insert into TimeStudyHistory (StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion)
select StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion from TimeStudy where ID = {0}

declare @id bigint
select @id = @@IDENTITY

insert into TimeStudyHistory_Detail(ID,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID,StdSMV,Thread_ComboID)
select @id,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID,StdSMV,Thread_ComboID from TimeStudy_Detail where ID = {0}

declare @phase varchar(10)
select @phase = isnull(Phase,'') from TimeStudy where ID = {0}

update TimeStudy 
set Phase = iif(@phase = 'Estimate','Initial',iif(@phase = 'Initial','Prelim',iif(@phase = 'Prelim','Final','Estimate'))),
	Version = '01',
	EditName = '{1}',
	EditDate = GETDATE(),
    Status = 'New'
where ID = {0}",
                    this.CurrentMaintain["ID"].ToString(),
                    Env.User.UserID);
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, executeCmd);
                        if (result)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Click new version  failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.ErrorBox("Connection transaction error.\r\n" + ex.ToString());
                        return;
                    }
                }

                this.RenewData();
                this.ClickEdit();

                this.btnSewSeq.PerformClick();
            }
        }

        // Copy from style std. GSD
        private void BtnCopyFromStyleStdGSD_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtStyle.Text) || MyUtility.Check.Empty(this.comboStyle.Text) || MyUtility.Check.Empty(this.txtBrand.Text) || MyUtility.Check.Empty(this.txtseason.Text))
            {
                MyUtility.Msg.WarningBox("< Style > or < Combo Type > or < Season > or < Brand > can't empty!!");
                return;
            }

            #region  表身有資料時，就必須先問是否要將表身資料全部刪除
            if (((DataTable)this.detailgridbs.DataSource).DefaultView.Count > 0)
            {
                DialogResult confirmResult;
                confirmResult = MyUtility.Msg.QuestionBox("Detail data have operation code now! Are you sure you want to erase it?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
                if (confirmResult != DialogResult.Yes)
                {
                    return;
                }
            }
            #endregion

            #region 若STYLE為套裝(Style.StyleUnit='SETS')，需考慮location條件。
            bool isComboType = true;
            string sql = string.Format("select StyleUnit from Style  WITH (NOLOCK) where BrandID = '{0}' and ID = '{1}' and SeasonID = '{2}'", this.txtBrand.Text, this.txtStyle.Text, this.txtseason.Text);
            if (MyUtility.GetValue.Lookup(sql) == "SETS")
            {
                isComboType = true;
            }
            else
            {
                isComboType = false;
            }

            // isComboType = !MyUtility.Check.Empty(MyUtility.GetValue.Lookup(sql));
            #endregion

            #region 設定sql參數
            SqlParameter sp1 = new SqlParameter();
            SqlParameter sp2 = new SqlParameter();
            SqlParameter sp3 = new SqlParameter();
            SqlParameter sp4 = new SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.txtStyle.Text;
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.txtseason.Text;
            sp3.ParameterName = "@brandid";
            sp3.Value = this.txtBrand.Text;

            if (isComboType)
            {
                sp4.ParameterName = "@location";
                sp4.Value = this.comboStyle.Text;
            }

            IList<SqlParameter> cmds = new List<SqlParameter>
            {
                sp1,
                sp2,
                sp3,
            };
            if (isComboType)
            {
                cmds.Add(sp4);
            }
            #endregion

            DataTable ietmsData;
            string sqlCmd = string.Format(
                @"
select id.SEQ,
	id.OperationID,
	Location = IIF(id.OperationID LIKE '-%', id.OperationID, ''),
	o.DescEN as OperationDescEN,
	id.Annotation,
	iif(round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3) = 0,0,round(3600/round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3),1)) as PcsPerHour,
	id.Frequency as Sewer,
	o.MachineTypeID,
	id.Frequency,
	id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency as IETMSSMV,
	[Mold] = dbo.GetParseOperationMold(id.Mold, 'Attachment'),
	[Template] = dbo.GetParseOperationMold(id.Mold, 'Template'),
	id.MtlFactorID,
	round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3) as SMV, 
    round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3) as StdSMV, 
	o.SeamLength,
	s.IETMSID,
	s.IETMSVersion,
	(isnull(o.SeamLength,0) * isnull(id.Frequency,0))  as ttlSeamLength ,
	o.MasterPlusGroup,
    [IsShow] = cast(iif( id.OperationID like '--%' , 1, isnull(show.val, 1)) as bit)
    ,IsSubprocess = isnull(md.IsSubprocess,0)
    ,[MachineType_IsSubprocess] = isnull(md.IsSubprocess, 0)
    ,PPA = IIF(id.isPPA = 1 ,'C' , '')
    ,PPAText = IIF(id.isPPA = 1 ,'Centralized' , '')
    ,IsNonSewingLine =  ISNULL(md.IsNonSewingLine ,0)
    ,[Thread_ComboID] = Thread_ComboID.val
    ,[IsNonSewingLineEditable] = cast(case  when isnull(md.IsSubprocess, 0) = 1 then 1
                                            when isnull(md.IsNonSewingLine, 0) = 1 then 1
                                            else 0 end as bit)
from Style s WITH (NOLOCK) 
inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
left join Operation o WITH (NOLOCK) on id.OperationID = o.ID
left join MachineType_Detail md WITH (NOLOCK) on md.ID = o.MachineTypeID and md.FactoryID = '{0}'
outer apply (
    select　top 1 [val] = Thread_ComboID
    from Style_ThreadColorCombo st with (nolock)
    where	s.IETMSID_Thread = '{1}' and s.IETMSVersion_Thread = '{2}' and
            st.StyleUkey = s.Ukey and
    		st.MachineTypeID = o.MachineTypeID and
    		exists(select 1 from Style_ThreadColorCombo_Operation sto with (nolock) 
                            where   sto.Style_ThreadColorComboUkey = st.Ukey and 
                                    sto.OperationID = id.OperationID)
) Thread_ComboID
outer apply (
	 select [val] = IIF(isnull(mt.IsNotShownInP01 , 1) = 0, 1, 0)
    from Operation o2
	Inner Join MachineType_Detail mt on o2.MachineTypeID = mt.ID and mt.FactoryID = '{0}'
	where o.ID = o2.ID
)show
--left join MtlFactor m WITH (NOLOCK) on o.MtlFactorID = m.ID and m.Type = 'F'
where s.ID = @id 
and s.SeasonID = @seasonid 
and s.BrandID = @brandid ", Env.User.Factory,
                this.CurrentMaintain["IETMSID"],
                this.CurrentMaintain["IETMSVersion"]);

            // if (isComboType) sqlCmd += " and id.Location = @location ";
            if (isComboType)
            {
                sqlCmd += " and id.Location = @location ";
            }

            sqlCmd += " order by id.SEQ";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ietmsData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query ietms fail!\r\n" + result.ToString());
                return;
            }

            // 刪除原有資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            // 將IETMS_Detail資料寫入表身
            foreach (DataRow dr in ietmsData.Rows)
            {
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }

            if (ietmsData.Rows.Count > 0)
            {
                this.CurrentMaintain["IETMSID"] = ietmsData.Rows[0]["IETMSID"].ToString();
                this.CurrentMaintain["IETMSVersion"] = ietmsData.Rows[0]["IETMSVersion"].ToString();
                this.HideRows();
            }

            // 補上空的Location
            this.FillAllLocation();

            // 編碼 SewingSeq
            this.FillSewingSeqAfterCopy();
        }

        // History
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            P01_History callNextForm = new P01_History(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // Std. GSD List
        private void BtnStdGSDList_Click(object sender, EventArgs e)
        {
            // sql參數
            SqlParameter sp1 = new SqlParameter();
            SqlParameter sp2 = new SqlParameter();
            SqlParameter sp3 = new SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.CurrentMaintain["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.CurrentMaintain["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = this.CurrentMaintain["BrandID"].ToString();

            IList<SqlParameter> cmds = new List<SqlParameter>
            {
                sp1,
                sp2,
                sp3,
            };

            string sqlCmd = "select Ukey from Style WITH (NOLOCK) where ID = @id and SeasonID = @seasonid and BrandID = @brandid";
            DataTable styleUkey;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out styleUkey);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            if (styleUkey.Rows.Count > 0)
            {
                StdGSDList callNextForm = new StdGSDList(MyUtility.Convert.GetLong(styleUkey.Rows[0]["UKey"]));
                DialogResult dresult = callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.WarningBox("Style not found!!");
            }
        }

        /// <summary>
        /// GetStdGSD
        /// </summary>
        /// <param name="operationID">operationID</param>
        /// <returns>DataTable</returns>
        public DataTable GetStdGSD(string operationID)
        {
            DataTable dt;

            string cmd = $@"select OperationID=ID ,MoldID  from Operation where Junk=0 and ID = '{operationID}'";

            DBProxy.Current.Select(null, cmd, out dt);

            return dt;
        }

        /// <summary>
        /// DetailGrid Insert、Append (index = -1)
        /// </summary>
        /// <param name="index">index</param>
        protected override void OnDetailGridInsert(int index = 0)
        {
            int maxSeq = this.DetailDatas.AsEnumerable()
                .Select(r => MyUtility.Convert.GetInt(r["Seq"]))
                .DefaultIfEmpty(0)
                .Max();

            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            string location;
            var lastLocation = dt.AsEnumerable()
                .Where(row => (dt.Rows.IndexOf(row) <= index || index == -1) &&
                               row.RowState != DataRowState.Deleted);
            if (lastLocation.Any())
            {
                location = MyUtility.Convert.GetString(lastLocation.Last()["Location"]); // 最後一筆 Row
            }
            else
            {
                location = string.Empty; // 或其他預設值
            }

            base.OnDetailGridInsert(index);

            DataRow nowRow;
            if (index == -1)
            {
                nowRow = this.CurrentDetailData;
            }
            else
            {
                nowRow = dt.Rows[index];
            }

            nowRow["Seq"] = (maxSeq + this.seqIncreaseNumber).ToString().PadLeft(4, '0'); // 固定最大值往上增加
            nowRow["IsAdd"] = 1; // PMS 新增資訊
            nowRow["CodeFrom"] = "Operation";
            nowRow["IsShow"] = 1;
            nowRow["IETMSUkey"] = this.DetailDatas.Count == 0 ? 0 : MyUtility.Convert.GetLong(this.DetailDatas[0]["IETMSUkey"]); // 原規則都是取第一筆, 保持不變
            nowRow["Location"] = location;
            nowRow.EndEdit();
        }

        // Copy
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            // 將要Copy的資料記錄起來
            List<DataRow> listDr = new List<DataRow>();
            DataRow lastRow = null;
            int index = -1, lastIndex = 0;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                index++;
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["Selected"].ToString() == "1")
                    {
                        dr["Selected"] = 0;
                        lastIndex = index;
                        lastRow = dr;
                        listDr.Add(dr);
                    }
                }
            }

            this.detailgrid.ValidateControl();
            if (listDr.Count <= 0)
            {
                return;
            }

            // 將要Copy的資料塞進DataTable中
            int currentCellindex = this.detailgrid.CurrentCell.RowIndex;
            foreach (DataRow dr in listDr)
            {
                DataRow newRow = ((DataTable)this.detailgridbs.DataSource).NewRow();
                newRow.ItemArray = dr.ItemArray;
                currentCellindex++;
                ((DataTable)this.detailgridbs.DataSource).Rows.InsertAt(newRow, currentCellindex);
            }

            int seq = MyUtility.Convert.GetInt(lastRow["Seq"]);
            for (int i = lastIndex + 1; i < this.DetailDatas.Count; i++)
            {
                seq += 10;
                this.DetailDatas[i]["Seq"] = MyUtility.Convert.GetString(seq).PadLeft(4, '0');
            }
        }

        // 撈CD Code
        private void GenCD(object sender, EventArgs e)
        {
            IList<SqlParameter> cmds = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@styleid", Value = this.CurrentMaintain["StyleID"].ToString() },
                new SqlParameter() { ParameterName = "@seasonid", Value = this.CurrentMaintain["SeasonID"].ToString() },
                new SqlParameter() { ParameterName = "@brandid", Value = this.CurrentMaintain["BrandID"].ToString() },
            };

            this.displayCD.Value = string.Empty;
            DataTable dt;
            string sqlCmd = @"
select s.CdCodeID 
    , s.CDCodeNew
    , ProductType = r2.Name
	, FabricType = r1.Name
	, s.Lining
	, s.Gender
	, Construction = d1.Name
from Style s WITH (NOLOCK)
left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
where s.ID = @styleid 
and s.SeasonID = @seasonid 
and s.BrandID = @brandid";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out dt);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query New CDCode data fail!\r\n" + result.ToString());
                return;
            }

            this.displayCD.Value = dt.Rows.Count > 0 ? dt.Rows[0]["CdCodeID"].ToString() : string.Empty;
            this.displayCDCodeNew.Value = dt.Rows.Count > 0 ? dt.Rows[0]["CDCodeNew"].ToString() : string.Empty;
            this.displayProductType.Value = dt.Rows.Count > 0 ? dt.Rows[0]["ProductType"].ToString() : string.Empty;
            this.displayFabricType.Value = dt.Rows.Count > 0 ? dt.Rows[0]["FabricType"].ToString() : string.Empty;
            this.displayLining.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Lining"].ToString() : string.Empty;
            this.displayGender.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Gender"].ToString() : string.Empty;
            this.displayConstruction.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Construction"].ToString() : string.Empty;
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            List<DataRow> toDelete = new List<DataRow>();

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["Selected"].ToString() == "1")
                    {
                        toDelete.Add(dr);
                    }
                }
            }

            foreach (DataRow dr in toDelete)
            {
                dr.Delete();
            }

            return;
        }

        private void BtnCIPF_Click(object sender, EventArgs e)
        {
            string ietmsUKEY = MyUtility.GetValue.Lookup($"select ukey from IETMS where id = '{this.CurrentMaintain["Ietmsid"]}' and Version = '{this.CurrentMaintain["ietmsversion"]}'");

            var dlg = new CIPF(MyUtility.Convert.GetLong(ietmsUKEY));
            dlg.Show();
        }

        private void BtnBatchDelete_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var dt = (DataTable)this.detailgridbs.DataSource;

            if (this.detailgrid.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("no data for delete");
                return;
            }

            if (this.SelectedDetailGridDataRow
                .Where(o => o["MachineType_IsSubprocess"].ToString() == "True").Any())
            {
                MyUtility.Msg.WarningBox("Subprocess checked! This operation cannot delete!");
                return;
            }

            this.SelectedDetailGridDataRow
                .Where(o => o["MachineType_IsSubprocess"].ToString() == "False")
                .ToList()
                .ForEach(row =>
                {
                    row.Delete();
                });
        }

        private void BtnBatchCopy_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            var dt = (DataTable)this.detailgridbs.DataSource;
            if (this.SelectedDetailGridDataRow.Count() == 0)
            {
                MyUtility.Msg.WarningBox("no data for copy");
                return;
            }

            // Deleted資料列會影響Index抓取，故擋掉
            var deletedDatas = dt.AsEnumerable().Where(o => o.RowState == DataRowState.Deleted).ToList();

            if (deletedDatas.Any())
            {
                MyUtility.Msg.WarningBox("There is data be deleted, please save first.");
                return;
            }

            int insertPosition;

            // 取得要放置的Index
            if (MyUtility.Check.Empty(this.txtInsertPosition.Text))
            {
                insertPosition = dt.Rows.Count;
            }
            else
            {
                var seqTarget = this.txtInsertPosition.Text; // .Value.ToString("0000");
                var targetRowToInsertReplace = dt.Select("SEQ = '" + seqTarget + "'").FirstOrDefault();

                if (targetRowToInsertReplace == null)
                {
                    if (MyUtility.Msg.QuestionBox("there is no " + seqTarget + " exists, can't locate insert position, append to the last. \r\n\r\ncontinue?") != DialogResult.Yes)
                    {
                        return;
                    }

                    insertPosition = dt.Rows.Count;
                }
                else
                {
                    insertPosition = dt.Rows.IndexOf(targetRowToInsertReplace);
                }
            }

            // 複製並插入
            var copyRow = this.SelectedDetailGridDataRow.OrderBy(o => o["Seq"]);
            bool isAllNum = true;

            // 勾選了幾筆
            int insertStartIndex = insertPosition;
            int insertEndIndex = this.SelectedDetailGridDataRow.Count() + insertPosition - 1;

            int t = 0;
            if (copyRow.Any())
            {
                #region 直接塞最後面的情況

                // 直接塞最後面的情況
                if (insertPosition == dt.Rows.Count)
                {
                    // 若最後一筆Seq包含文字，則不自動編碼
                    if (int.TryParse(dt.Rows[dt.Rows.Count - 1]["Seq"].ToString(), out t))
                    {
                        int i = 1;
                        int maxSeq = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Seq"]);
                        copyRow.ToList().ForEach(row =>
                        {
                            var newRow = dt.NewRow();
                            newRow.ItemArray = row.ItemArray;
                            newRow["Selected"] = "0";
                            newRow["Seq"] = (maxSeq + (i * 10)).ToString().PadLeft(4, '0');
                            dt.Rows.InsertAt(newRow, insertPosition++);
                            i++;
                        });
                    }
                    else
                    {
                        copyRow.ToList().ForEach(row =>
                        {
                            var newRow = dt.NewRow();
                            newRow.ItemArray = row.ItemArray;
                            newRow["Selected"] = "0";
                            dt.Rows.InsertAt(newRow, insertPosition++);
                        });
                    }

                    return;
                }

                #endregion

                #region 重新編碼範圍內是否有不是數字的情況
                int x;

                // 判斷勾選的是否都是數字
                copyRow.ToList().ForEach(row =>
                {
                    if (!int.TryParse(row["Seq"].ToString(), out x))
                    {
                        isAllNum = false;
                    }
                });

                // 判斷後續要異動Seq的是否都是數字
                foreach (DataRow dr in dt.Rows)
                {
                    if (dt.Rows.IndexOf(dr) > insertEndIndex)
                    {
                        if (dt.Rows[dt.Rows.IndexOf(dr) - 1].RowState == DataRowState.Deleted)
                        {
                            continue;
                        }

                        if (!int.TryParse(dt.Rows[dt.Rows.IndexOf(dr) - 1]["Seq"].ToString(), out x))
                        {
                            isAllNum = false;
                        }
                    }
                }

                // 判斷被塞的位置是否為數字
                if (!int.TryParse(dt.Rows[insertPosition]["Seq"].ToString(), out x))
                {
                    isAllNum = false;
                }
                #endregion

                if (isAllNum)
                {
                    // 編碼範圍內都是數字，則自動編碼
                    int i = 0;
                    copyRow.ToList().ForEach(row =>
                    {
                        var newRow = dt.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        newRow["Selected"] = "0";
                        newRow["Seq"] = (Convert.ToInt32(dt.Rows[insertPosition]["Seq"]) + (i * 10)).ToString().PadLeft(4, '0');  // 取代原本的Seq
                        dt.Rows.InsertAt(newRow, insertPosition++);
                        i++;
                    });

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dt.Rows.IndexOf(dr) > insertEndIndex)
                        {
                            // 前一個Seq
                            int preSeq = Convert.ToInt32(dt.Rows[dt.Rows.IndexOf(dr) - 1]["Seq"]);
                            dr["Seq"] = MyUtility.Convert.GetString(preSeq + this.seqIncreaseNumber).PadLeft(4, '0');
                        }
                    }
                }
                else
                {
                    // 編碼範圍內有文字，則塞入就好
                    copyRow.ToList().ForEach(row =>
                    {
                        var newRow = dt.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        newRow["Selected"] = "0";
                        dt.Rows.InsertAt(newRow, insertPosition++);
                    });
                }
            }
        }

        /// <summary>
        /// gridIcon 刪除按鈕
        /// </summary>
        protected override void OnDetailGridRemoveClick()
        {
            if (this.CurrentMaintain == null || this.DetailDatas.Count == 0)
            {
                return;
            }

            this.detailgrid.ValidateControl();

            DataRow drSelect = this.detailgrid.GetDataRow(this.detailgridbs.Position);
            if (MyUtility.Check.Empty(drSelect["MachineType_IsSubprocess"]) == false)
            {
                MyUtility.Msg.WarningBox("Subprocess checked! This operation cannot delete!");
                return;
            }

            base.OnDetailGridRemoveClick();
            this.ReFillSeq(); // 移除一筆表身後重編 Ori.Seq
            this.ReFillSewingSeq(); // 移除一筆表身後重編 Sewing Seq
        }

        private void DetailGridAfterRowDragDo(DataRow dr)
        {
            int sortIndex = 1;

            foreach (DataRow row in this.DetailDatas)
            {
                row["Sort"] = sortIndex;
                sortIndex++;
            }

            // 把空白的拖移到 0001 之下 要給值
            bool flag = false;
            foreach (DataRow row in this.DetailDatas)
            {
                // 第一筆有值 0001
                if (!MyUtility.Check.Empty(row["SewingSeq"]) && !flag)
                {
                    flag = true;
                }

                // 先給 1 之後再使用 ReFillSewingSeq 重編碼
                if (flag && MyUtility.Check.Empty(row["SewingSeq"]) && this.NeedSewingSeq(row))
                {
                    row["SewingSeq"] = 1;
                }
            }

            this.ReFillSewingSeq(); // 拖拉後重排 SewSeq 是主功能, 畫面由上往下重排有填值的 SewSeq
        }

        /// <summary>
        /// 若新輸入的 SewingSeq 與現有資料有重複, 將原有的全部往後+1
        /// </summary>
        /// <param name="newSewingSeq">newSewingSeq</param>
        private void InsertSewingSeqAndAdjust(int newSewingSeq)
        {
            // 尋找是否已經有相同 SewingSeq
            if (this.DetailDatas.Any(row => MyUtility.Convert.GetInt(row["SewingSeq"]) == newSewingSeq))
            {
                // 若存在相同 SewingSeq，將所有後續的 SewingSeq 往後加 1
                foreach (var row in this.DetailDatas.AsEnumerable()
                                     .Where(r => MyUtility.Convert.GetInt(r["SewingSeq"]) >= newSewingSeq)
                                     .OrderBy(r => MyUtility.Convert.GetInt(r["SewingSeq"])))
                {
                    int currentSeq = MyUtility.Convert.GetInt(row["SewingSeq"]);
                    row["SewingSeq"] = (currentSeq + 1).ToString("D4"); // 確保格式為四碼數字
                }
            }
        }

        /// <summary>
        /// 依據 SewingSeq, Seq 改變資料列的Index
        /// </summary>
        /// <param name="oldSewingSeq">oldSewingSeq</param>
        protected void UpdateRowIndexBasedOnSewingSeqAndSeq(string oldSewingSeq)
        {
            int targetIndex = -1;
            try
            {
                int targetSewingSeq = MyUtility.Convert.GetInt(this.CurrentDetailData["SewingSeq"]); // 指定的 SewingSeq
                int targetSeq = MyUtility.Convert.GetInt(this.CurrentDetailData["Seq"]); // 指定的 Seq

                // 取得資料來源
                DataTable dt = (DataTable)this.detailgridbs.DataSource;

                // 確保資料表存在並且有資料
                if (dt.Rows.Count <= 1)
                {
                    return;
                }

                int sourceIndex = dt.Rows.IndexOf(this.CurrentDetailData); // 目標位置為當前綁定位置

                var sortedRows = dt.AsEnumerable()
                    .Where(r => r.RowState != DataRowState.Deleted)
                    .OrderBy(r => MyUtility.Convert.GetInt(r["SewingSeq"]))
                    .ThenBy(r => MyUtility.Convert.GetInt(r["Seq"]));

                // 確保原始位置在排序後的目標位置 (targetIndex)
                DataRow beforeRow = null;

                // 若是最後一筆, 直接放最後
                if (sortedRows.Last() == this.CurrentDetailData)
                {
                    targetIndex = dt.Rows.Count - 1;
                }
                else
                {
                    foreach (DataRow row in sortedRows)
                    {
                        if (beforeRow == null)
                        {
                            beforeRow = row;
                        }

                        if (MyUtility.Convert.GetInt(beforeRow["Seq"]) == targetSeq)
                        {
                            targetIndex = dt.Rows.IndexOf(row);
                            if (beforeRow == row)
                            {
                                targetIndex = 0;
                            }

                            break;
                        }

                        beforeRow = row;
                    }

                    if (targetIndex > sourceIndex && MyUtility.Check.Empty(oldSewingSeq))
                    {
                        targetIndex--;
                    }
                }

                // 確保索引有效
                if (sourceIndex == -1 || targetIndex < 0 || targetIndex >= dt.Rows.Count)
                {
                    return;
                }

                // 移動 DataRow
                this.MoveDataRow(dt, sourceIndex, targetIndex);
                this.ReFillSewingSeq(); // 調整 index 順序後, 再調整 SewSeq 連續性
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }

            // 清除所有行的選擇狀態
            foreach (DataGridViewRow row in this.detailgrid.Rows)
            {
                row.Selected = false;
            }
        }

        /// <summary>
        /// 當刪除Row 或 清空SewingSeq,有無點擊欄位排序, 重新排序 SewingSeq
        /// </summary>
        private void ReFillSewingSeq()
        {
            int sewingSeq = 1;
            foreach (DataRow row in this.DetailDatas.Where(r => !MyUtility.Check.Empty(r["SewingSeq"])))
            {
                row["SewingSeq"] = sewingSeq.ToString().PadLeft(4, '0');
                sewingSeq++;
            }

            // 對應DesignateSeq 要根據DesignateSeqIdetityKey透過IdetityKey重新抓取新的DesignateSeq
            foreach (DataRow row in this.DetailDatas.Where(r => !MyUtility.Check.Empty(r["DesignateSeq"])))
            {
                row["DesignateSeq"] = this.GetDesignateSeq(row["DesignateSeqIdetityKey"].ToString());
            }
        }

        /// <summary>
        /// 欄位:OperationID, IsNonSewingLine 驗證後
        /// 當 OperationID 不包含 --, 沒勾選 IsNonSewingLine
        /// 因為 SewingSeq 可以手動刪除. 不影響其它筆空的 SewingSeq
        /// 先填或清空(這筆) SewingSeq 讓它有值, 最後再把有值的 SewingSeq 重編
        /// </summary>
        private void AfterColumnEdit(DataRow dr)
        {
            int maxSewing = this.DetailDatas
                .Select(r => MyUtility.Convert.GetInt(r["SewingSeq"]))
                .DefaultIfEmpty(0)
                .Max();

            if (!MyUtility.Check.Empty(dr["OperationID"]) && !MyUtility.Convert.GetString(dr["OperationID"]).Contains("--") && !MyUtility.Convert.GetBool(dr["IsNonSewingLine"]))
            {
                dr["SewingSeq"] = maxSewing + 1;  // 點了其它欄位排序, 新增/插入 Sewing Seq 都是最大往上, 刪除中間時在 ReFillSewingSeq 以現有排序重排
            }
            else
            {
                dr["SewingSeq"] = string.Empty;
            }

            this.UpdateRowIndexBasedOnSewingSeqAndSeq(dr["SewingSeq"].ToString()); // 欄位編輯後自動帶入 SewSeq 並重新排序. 欄位: OperationID , IsNonSewingLine
        }

        /// <summary>
        /// copy 後重新編碼 SewingSeq
        /// </summary>
        private void FillSewingSeqAfterCopy()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                if (this.NeedSewingSeq(dr))
                {
                    dr["SewingSeq"] = "1"; // 先給任一值
                }
            }

            this.ReFillSewingSeq(); // Copy From Style Std GSD, 符合規則自動填入, 並將有值 SewingSeq 全部重編
        }

        private bool NeedSewingSeq(DataRow dr)
        {
            return !MyUtility.Check.Empty(dr["OperationID"]) && !MyUtility.Convert.GetString(dr["OperationID"]).Contains("--") && !MyUtility.Convert.GetBool(dr["IsNonSewingLine"]);
        }

        /// <summary>
        /// 刪除中間的資料要重編 Seq, 要連續 +10 不要有跳號. 不是 trade 轉來的資訊才編碼
        /// </summary>
        private void ReFillSeq()
        {
            // trade 轉來的資訊最大 Seq
            int maxSeqFormTrade = this.DetailDatas.AsEnumerable()
                .Where(r => !MyUtility.Convert.GetBool(r["IsAdd"]))
                .Select(r => MyUtility.Convert.GetInt(r["Seq"]))
                .DefaultIfEmpty(0) // 當沒有符合條件的筆數時，預設值為 0
                .Max();

            // PMS 新增的資訊
            var isAddDatas = this.DetailDatas.AsEnumerable().Where(r => MyUtility.Convert.GetBool(r["IsAdd"]));

            // 依據原本 Seq 順序進行重編, 才不會有跳號狀況
            int seq = maxSeqFormTrade;
            foreach (DataRow dr in isAddDatas.OrderBy(r => MyUtility.Convert.GetInt(r["Seq"])))
            {
                seq += this.seqIncreaseNumber;
                dr["Seq"] = seq.ToString().PadLeft(4, '0');
            }
        }

        /// <summary>
        /// 填入所有空的 Location 欄位。如果第一筆資料沒有 Location，會往下尋找第一筆有值的 Location 並向上填充。
        /// </summary>
        private void FillAllLocation()
        {
            // 填入與前一筆相同的 Location
            string preLocation = string.Empty;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Location"]))
                {
                    dr["Location"] = preLocation; // 與前一筆相同
                }

                preLocation = MyUtility.Convert.GetString(dr["Location"]); // 記錄前一筆
            }

            // 如果第一筆沒有 Location，往下尋找第一筆有值的 Location
            if (MyUtility.Check.Empty(this.DetailDatas.First()["Location"]))
            {
                string firstNonEmptyLocation = this.FindFirstNonEmptyLocation();
                if (!string.IsNullOrEmpty(firstNonEmptyLocation))
                {
                    // 將第一筆開始無值的部分往上填充
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        if (!MyUtility.Check.Empty(dr["Location"]))
                        {
                            break; // 已填充到有值為止
                        }

                        dr["Location"] = firstNonEmptyLocation;
                    }
                }
            }
        }

        /// <summary>
        /// 尋找 DetailDatas 中第一筆非空的 Location 值。
        /// </summary>
        /// <returns>第一筆非空的 Location 值，若無則返回空字串。</returns>
        private string FindFirstNonEmptyLocation()
        {
            foreach (DataRow dr in this.DetailDatas)
            {
                string location = MyUtility.Convert.GetString(dr["Location"]);
                if (!MyUtility.Check.Empty(location))
                {
                    return location;
                }
            }

            return string.Empty;
        }

        private void BtnATSummary_Click(object sender, EventArgs e)
        {
            string ietmsUKEY = MyUtility.GetValue.Lookup($"select ukey from IETMS where id = '{this.CurrentMaintain["Ietmsid"]}' and Version = '{this.CurrentMaintain["ietmsversion"]}'");
            DataTable dataSource = (DataTable)this.detailgridbs.DataSource;
            var windows = new P01_AT_Summary(ietmsUKEY, ref dataSource, ref this.p01_OperationList, this.EditMode, this.strTimeStudtID);
            windows.ShowDialog();
            this.detailgridbs.DataSource = dataSource;
        }

        private DataRow MoveDataRow(DataTable dataTable, int sourceIndex, int targetIndex)
        {
            if (dataTable.Rows.Count < 2)
            {
                return null;
            }

            DataRow dataRow = dataTable.Rows[sourceIndex];
            DataRow newRow = dataTable.NewRow();
            DataRowState oriRowState = dataRow.RowState;

            newRow.ItemArray = oriRowState == DataRowState.Modified ? this.GetRowItemArrayByVersion(dataRow, DataRowVersion.Original) : dataRow.ItemArray;
            object[] currentRowData = dataRow.ItemArray;

            // 從 DataTable 中移除資料列
            dataTable.Rows.Remove(dataRow);

            // 插入資料列至目標位置
            dataTable.Rows.InsertAt(newRow, targetIndex);
            if (oriRowState == DataRowState.Unchanged)
            {
                newRow.AcceptChanges();
            }
            else if (oriRowState == DataRowState.Modified)
            {
                newRow.AcceptChanges();
                newRow.ItemArray = currentRowData;
            }

            return newRow;
        }

        private object[] GetRowItemArrayByVersion(DataRow srcRow, DataRowVersion dataRowVersion)
        {
            object[] resultItemArray = new object[srcRow.Table.Columns.Count];
            int colIndex = 0;
            foreach (DataColumn col in srcRow.Table.Columns)
            {
                resultItemArray[colIndex] = srcRow[col.ColumnName, dataRowVersion];
                colIndex++;
            }

            return resultItemArray;
        }

        private void BtnSewSeq_Click(object sender, EventArgs e)
        {
            // Detail geid Sew Seq有值就不做
            if (this.DetailDatas.Any(r => !MyUtility.Check.Empty(r["SewingSeq"])))
            {
                return;
            }

            this.FillSewingSeqAfterCopy();
        }

        private string GetDesignateSeq(string designateSeqIdetityKey)
        {
            var matchingRow = this.DetailDatas.FirstOrDefault(r => r["IdetityKey"].ToString() == designateSeqIdetityKey);
            return matchingRow != null ? matchingRow["SewingSeq"].ToString() : string.Empty;
        }
    }
}
