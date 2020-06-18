using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Transactions;
using Sci.Win.Tools;
using Sci.Production.PublicForm;
using System.Data.SqlClient;
using System.Linq;
using Sci.Production.Class;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01
    /// </summary>
    public partial class P01 : Sci.Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private Ict.Win.DataGridViewGeneratorTextColumnSettings operation = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings machine = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings mold = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings frequency = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings smvsec = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private string styleID;
        private string seasonID;
        private string brandID;
        private string comboType;

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
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.detailgrid.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        }

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="styleID">StyleID</param>
        /// <param name="brandID">BrandID</param>
        /// <param name="seasonID">SeasonID</param>
        /// <param name="comboType">ComboType</param>
        public P01(string styleID, string brandID, string seasonID, string comboType)
        {
            this.InitializeComponent();
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

            this.DefaultFilter = df.ToString();
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">PrepareDetailSelectCommandEventArgs</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select 0 as Selected, isnull(o.SeamLength,0) SeamLength
      ,td.[ID]
      ,td.[SEQ]
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
	  ,o.MasterPlusGroup
from TimeStudy_Detail td WITH (NOLOCK) 
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
left join Mold m WITH (NOLOCK) on m.ID=td.Mold
where td.ID = '{0}'
order by td.Seq", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 1, 1, "last 2 years modify,All");
            this.queryfors.SelectedIndex = 0;
            if (MyUtility.Check.Empty(this.DefaultWhere))
            {
                this.DefaultWhere = " AddDate >= DATEADD(YY,-2,GETDATE()) OR EditDate >= DATEADD(YY,-2,GETDATE())";
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
            result = DBProxy.Current.Select(null, sqlCmd, out DataTable dtLocation);

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
            this.GenCD(null, null);  // 撈CD Code
            bool canEdit = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P01. Factory GSD", "CanEdit");
            this.btnNewVersion.Enabled = !this.EditMode && this.CurrentMaintain != null && canEdit;
            this.btnNewStatus.Enabled = !this.EditMode && this.CurrentMaintain != null && canEdit;
            this.btnHistory.Enabled = !this.EditMode && this.CurrentMaintain != null;
            this.btnStdGSDList.Enabled = !this.EditMode && this.CurrentMaintain != null;
            this.btnArtSum.Enabled = this.CurrentMaintain != null;
            this.btnSketch.Enabled = this.CurrentMaintain != null;

            this.detailgrid.AutoResizeColumn(0);
            this.detailgrid.AutoResizeColumn(1);
            this.detailgrid.AutoResizeColumn(2);
            this.detailgrid.AutoResizeColumn(4);
            this.detailgrid.AutoResizeColumn(5);
            this.detailgrid.AutoResizeColumn(6);
            this.detailgrid.AutoResizeColumn(7);
            this.detailgrid.AutoResizeColumn(8);
            this.detailgrid.AutoResizeColumn(9);
            this.detailgrid.AutoResizeColumn(11);
            this.detailgrid.AutoResizeColumn(12);
            this.detailgrid.AutoResizeColumn(13);
            this.detailgrid.AutoResizeColumn(14);

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

            this.numTotalSMV.Value = Convert.ToInt32(MyUtility.Convert.GetDecimal(((DataTable)this.detailgridbs.DataSource).Compute("SUM(SMV)", string.Empty)));
        }

        /// <summary>
        /// OnDetailGridSetup()
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Ict.Win.DataGridViewGeneratorTextColumnSettings seq = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings template = new DataGridViewGeneratorTextColumnSettings();

            celltxtMachineGroup txtSubReason = (celltxtMachineGroup)celltxtMachineGroup.GetGridCell();

            #region Seq & Operation Code & Frequency & SMV & ST/MC Type & Attachment按右鍵與Validating
            #region Seq的Valid
            seq.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue) || (e.FormattedValue.ToString() != dr["Seq"].ToString()))
                    {
                        string oldValue = MyUtility.Check.Empty(dr["Seq"]) ? string.Empty : dr["Seq"].ToString();
                        dr["Seq"] = MyUtility.Check.Empty(e.FormattedValue) ? string.Empty : e.FormattedValue.ToString().Trim().PadLeft(4, '0');
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region Operation Code
            this.operation.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode
                    && e.Button == MouseButtons.Right
                    && e.RowIndex != -1)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    Sci.Production.IE.P01_SelectOperationCode callNextForm = new Sci.Production.IE.P01_SelectOperationCode();
                    DialogResult result = callNextForm.ShowDialog(this);
                    if (result == DialogResult.Cancel)
                    {
                        if (callNextForm.P01SelectOperationCode != null)
                        {
                            dr["OperationID"] = callNextForm.P01SelectOperationCode["ID"].ToString();
                            dr["OperationDescEN"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                            dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                            dr["Mold"] = callNextForm.P01SelectOperationCode["MoldID"].ToString().Replace(";", ","); // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                            dr["Template"] = string.Empty;  // 將[Template]清空
                            dr["MtlFactorID"] = callNextForm.P01SelectOperationCode["MtlFactorID"].ToString();
                            dr["SeamLength"] = callNextForm.P01SelectOperationCode["SeamLength"].ToString();
                            dr["SMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]) * 60;
                            dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]);
                            dr["Frequency"] = 1;
                            dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                            dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                            dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                            dr.EndEdit();
                        }
                    }

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        dr["OperationID"] = callNextForm.P01SelectOperationCode["ID"].ToString();
                        dr["OperationDescEN"] = callNextForm.P01SelectOperationCode["DescEN"].ToString();
                        dr["MachineTypeID"] = callNextForm.P01SelectOperationCode["MachineTypeID"].ToString();
                        dr["Mold"] = callNextForm.P01SelectOperationCode["MoldID"].ToString().Replace(";", ","); // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                        dr["Template"] = string.Empty;  // 將[Template]清空
                        dr["MtlFactorID"] = callNextForm.P01SelectOperationCode["MtlFactorID"].ToString();
                        dr["SeamLength"] = callNextForm.P01SelectOperationCode["SeamLength"].ToString();
                        dr["SMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]) * 60;
                        dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(callNextForm.P01SelectOperationCode["SMV"]);
                        dr["Frequency"] = 1;
                        dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                        dr["Annotation"] = callNextForm.P01SelectOperationCode["Annotation"].ToString();
                        dr["MasterPlusGroup"] = callNextForm.P01SelectOperationCode["MasterPlusGroup"].ToString();
                        dr.EndEdit();
                    }
                    else
                    {
                        return;
                    }
                }
            };

            this.operation.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["OperationID"].ToString())
                    {
                        if (e.FormattedValue.ToString().Substring(0, 2) == "--")
                        {
                            dr["OperationID"] = e.FormattedValue.ToString();
                            dr["OperationDescEN"] = string.Empty;
                            dr["MachineTypeID"] = string.Empty;
                            dr["Mold"] = string.Empty;
                            dr["Template"] = string.Empty;  // 將[Template]清空
                            dr["MtlFactorID"] = string.Empty;
                            dr["Frequency"] = 0;
                            dr["SeamLength"] = 0;
                            dr["SMV"] = 0;
                            dr["IETMSSMV"] = 0;
                            dr["Annotation"] = string.Empty;
                        }
                        else
                        {
                            // sql參數
                            SqlParameter sp1 = new SqlParameter
                            {
                                ParameterName = "@id",
                                Value = e.FormattedValue.ToString()
                            };

                            IList<SqlParameter> cmds = new List<SqlParameter>
                            {
                                sp1
                            };

                            string sqlCmd = "select DescEN,SMV,MachineTypeID,SeamLength,MoldID,MtlFactorID,Annotation,MasterPlusGroup from Operation WITH (NOLOCK) where CalibratedCode = 1 and ID = @id";
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable opData);
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
                                    dr["Mold"] = opData.Rows[0]["MoldID"].ToString().Replace(";", ","); // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                                    dr["MtlFactorID"] = opData.Rows[0]["MtlFactorID"].ToString();
                                    dr["Frequency"] = 1;
                                    dr["SeamLength"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SeamLength"]);
                                    dr["SMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]) * 60;
                                    dr["IETMSSMV"] = MyUtility.Convert.GetDecimal(opData.Rows[0]["SMV"]);
                                    dr["ttlSeamLength"] = MyUtility.Convert.GetDecimal(dr["Frequency"]) * MyUtility.Convert.GetDecimal(dr["SeamLength"]);
                                    dr["Annotation"] = opData.Rows[0]["Annotation"].ToString();
                                    dr["MasterPlusGroup"] = opData.Rows[0]["MasterPlusGroup"].ToString();
                                }
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                                this.ChangeToEmptyData(dr);
                            }
                        }

                        dr.EndEdit();
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
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = "select ID,Description from MachineType WITH (NOLOCK) where Junk = 0";
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,35", dr["MachineTypeID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            this.machine.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["MachineTypeID"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("select ID,Description from MachineType WITH (NOLOCK) where Junk = 0 and ID = '{0}'", e.FormattedValue.ToString())))
                        {
                            dr["MachineTypeID"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< ST/MC Type: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            dr["MachineTypeID"] = e.FormattedValue.ToString();
                        }
                    }
                }
            };

            #endregion
            #region Attachment
            this.mold.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode
                    && e.Button == MouseButtons.Right
                    && e.RowIndex != -1)
                {
                    string sqlCmd = "select ID,DescEN from Mold WITH (NOLOCK) where Junk = 0";
                    SelectItem2 item = new SelectItem2(sqlCmd, "ID,DescEN", "8,15", this.CurrentDetailData["Mold"].ToString(), null, null, null);

                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Mold"] = item.GetSelectedString();
                }
            };

            this.mold.CellValidating += (s, e) =>
            {
                if (this.EditMode
                    && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    List<string> operationList = new List<string>();
                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(this.CurrentDetailData["OperationID"]) } };
                    string sqlcmd = "select ID from Mold WITH (NOLOCK) where Junk = 0";
                    DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtMold);
                    if (!result)
                    {
                        this.CurrentDetailData["Mold"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    sqlcmd = "select o.MoldID from Operation o WITH (NOLOCK) where o.ID = @OperationID";
                    result = DBProxy.Current.Select(null, sqlcmd, cmds, out DataTable dtOperation);
                    if (!result)
                    {
                        this.CurrentDetailData["Mold"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }
                    else
                    {
                        var query = dtOperation.AsEnumerable().Select(x => x.Field<string>("MoldID"));
                        if (query.Any())
                        {
                            operationList = query.FirstOrDefault().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                        }
                    }

                    // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                    List<string> getMold = e.FormattedValue.ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                    // 不存在 operation
                    var existsOperation = operationList.Except(getMold);
                    if (existsOperation.Any() && operationList.Any())
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["Mold"] = string.Join(",", getMold.Except(existsOperation).ToList());
                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsOperation.ToList()) + "  not in Operation setting !!", "Data not in setting");
                        return;
                    }

                    // 不存在 Mold
                    var existsMold = getMold.Except(dtMold.AsEnumerable().Select(x => x.Field<string>("ID")).ToList());
                    if (existsMold.Any())
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["Mold"] = string.Join(",", getMold.Where(x => existsMold.Where(y => !y.EqualString(x)).Any()).ToList());
                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsMold.ToList()) + "  not in Mold setting !!", "Data not in setting");
                        return;
                    }

                    this.CurrentDetailData["Mold"] = string.Join(",", getMold.ToList());
                }
            };
            #endregion
            #region template
            template.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = "select ID,Description from SewingMachineTemplate WITH (NOLOCK) where Junk = 0";

                    SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "ID,Description", "13,60,10", this.CurrentDetailData["Template"].ToString(), null, null, null)
                    {
                        Width = 666
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Template"] = item.GetSelectedString();
                }
            };

            template.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["Template"] = e.FormattedValue;
                    string sqlcmd = "select ID,Description from SewingMachineTemplate WITH (NOLOCK) where Junk = 0";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                    string[] getLocation = this.CurrentDetailData["Template"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errTemplate = new List<string>();
                    List<string> trueTemplate = new List<string>();
                    foreach (string item in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(item)) && !item.EqualString(string.Empty))
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
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errTemplate.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueTemplate.Sort();
                    this.CurrentDetailData["Template"] = string.Join(",", trueTemplate.ToArray());
                }
            };
            #endregion
            #endregion

            template.MaxLength = 100;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), settings: seq)
                .Text("OperationID", header: "Operation code", width: Widths.AnsiChars(13), settings: this.operation)
                .EditText("OperationDescEN", header: "Operation Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(30))
                .Numeric("Frequency", header: "Frequency", integer_places: 2, decimal_places: 2, maximum: 99.99M, minimum: 0, settings: this.frequency)
                .Text("MtlFactorID", header: "Factor", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("SMV", header: "SMV (sec)", integer_places: 4, decimal_places: 4, maximum: 9999.9999M, minimum: 0, settings: this.smvsec)
                .Text("MachineTypeID", header: "ST/MC Type", width: Widths.AnsiChars(8), settings: this.machine)
                .Text("MasterPlusGroup", header: "Machine Group", width: Widths.AnsiChars(8), settings: txtSubReason)
                .Text("Mold", header: "Attachment", width: Widths.AnsiChars(8), settings: this.mold)
                .Text("Template", header: "Template", width: Widths.AnsiChars(8), settings: template)
                .Numeric("PcsPerHour", header: "Pcs/hr", integer_places: 5, decimal_places: 1, iseditingreadonly: true)
                .Numeric("Sewer", header: "Sewer", integer_places: 2, decimal_places: 1, iseditingreadonly: true)
                .Numeric("IETMSSMV", header: "Std. SMV", integer_places: 3, decimal_places: 4, iseditingreadonly: true)
                .Numeric("SeamLength", header: "Seam length", integer_places: 7, decimal_places: 2, iseditingreadonly: true)
                .Numeric("ttlSeamLength", header: "ttlSeamLength", integer_places: 10, decimal_places: 2, iseditingreadonly: true);
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
        }

        /// <summary>
        /// ClickCopyBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickCopyBefore()
        {
            Sci.Production.IE.P01_Copy callNextForm = new Sci.Production.IE.P01_Copy(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
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
        }

        /// <summary>
        /// ClickDeleteBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
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
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
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
                sp4
            };
            string sqlCmd = "select sl.Location from Style s WITH (NOLOCK) , Style_Location sl WITH (NOLOCK) where s.ID = @styleid and s.SeasonID = @seasonid and s.BrandID = @brandid and s.Ukey = sl.StyleUkey and sl.Location = @combotype";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable locationData);
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
            var machineSMV_List = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).Select(o => new { MachineTypeID = o["MachineTypeID"].ToString(), SMV = MyUtility.Convert.GetDecimal(o["SMV"]) }).ToList();

            DBProxy.Current.Select(null, " SELECT ID FROM Machinetype WHERE ArtworkTypeID = 'SEWING' ", out DataTable tmp);
            List<string> sewingMachine_List = tmp.AsEnumerable().Select(o => o["ID"].ToString()).ToList();

            decimal ttlSewingTime = machineSMV_List.Where(o => sewingMachine_List.Contains(o.MachineTypeID)).Sum(o => o.SMV);

            this.CurrentMaintain["TotalSewingTime"] = Convert.ToInt32(ttlSewingTime); // MyUtility.Convert.GetInt(ttlSewingTime);

            string totalSewing = this.CurrentMaintain["TotalSewingTime"].ToString();
            this.numTotalSewingTimePc.Text = totalSewing;
            #endregion

            #region Total GSD 檢查

            // 先取得 Style.Ukey
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = this.CurrentMaintain["StyleID"].ToString() },
                new SqlParameter() { ParameterName = "@seasonid", Value = this.CurrentMaintain["SeasonID"].ToString() },
                new SqlParameter() { ParameterName = "@brandid", Value = this.CurrentMaintain["BrandID"].ToString() }
            };
            sqlCmd = "select Ukey from Style WITH (NOLOCK) where ID = @id and SeasonID = @seasonid and BrandID = @brandid";
            result = DBProxy.Current.Select(null, sqlCmd, parameters, out DataTable styleDT);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return false;
            }

            long styleUkey = 0;
            if (styleDT.Rows.Count > 0)
            {
                styleUkey = MyUtility.Convert.GetLong(styleDT.Rows[0]["UKey"]);

                sqlCmd = $@" 
select tms = cast(CEILING(sum(i.ProSMV) * 60) as decimal(20,2))
from IETMS_Summary i
where i.IETMSUkey = (select distinct i.Ukey from Style s WITH (NOLOCK) inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version where s.ukey = '{styleUkey}')
group by i.Location,i.ArtworkTypeID";
                result = DBProxy.Current.Select(null, sqlCmd, out DataTable dtGSD_Summary);
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

                bool isLocalStyle = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"SELECT LocalStyle FROM Style WHERE ID='{this.CurrentMaintain["StyleID"]}' AND SeasonID='{this.CurrentMaintain["SeasonID"]}' AND BrandID='{this.CurrentMaintain["BrandID"]}'"));

                // Local Style，才進行以下判斷
                if (!isLocalStyle)
                {
                    // Total Sewing Time重新計算過再來比
                    decimal totalSewingTime = MyUtility.Convert.GetDecimal(((DataTable)this.detailgridbs.DataSource).Compute("SUM(SMV)", string.Empty));
                    decimal totalGSD = 0;

                    if (dtGSD_Summary.Rows.Count > 0)
                    {
                        totalGSD = Convert.ToDecimal(dtGSD_Summary.Compute("sum(tms)", string.Empty));

                        if (totalSewingTime > totalGSD)
                        {
                            MyUtility.Msg.WarningBox($"Total sewing time cannot more than total GSD ({totalGSD}) of Std.GSD.");
                            return false;
                        }
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Style not found!!");
            }
            #endregion

            decimal allSewer = MyUtility.Check.Empty(this.CurrentMaintain["NumberSewer"]) ? 0.0m : MyUtility.Convert.GetDecimal(this.CurrentMaintain["NumberSewer"]);
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["Sewer"] = ttlSewingTime == 0 ? 0 : MyUtility.Math.Round(allSewer * (MyUtility.Convert.GetDecimal(dr["SMV"]) / ttlSewingTime), 1);
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            if (this.detailgridbs.DataSource != null)
            {
                DataTable detail = (DataTable)this.detailgridbs.DataSource;

                string updCmd = string.Empty;
                foreach (DataRow item in detail.Rows)
                {
                    updCmd += $@"
UPDATE TimeStudy_Detail
SET MasterPlusGroup = '{item["MasterPlusGroup"]}'
WHERE Ukey={item["Ukey"]}

";
                }

                DualResult reusult = DBProxy.Current.Execute(null, updCmd);
                if (!reusult)
                {
                    this.ShowErr(reusult);
                }
            }

            // 若ThreadColorComb已有資料要自動發信通知Style.ThreadEditname(去串pass1.EMail若為空或null則不需要發信)，通知使用者資料有變更。
            string sqlcmd = $@"
select distinct p.EMail
from TimeStudy ts with(nolock)
inner join style s with(nolock) on s.id = ts.StyleID and s.SeasonID = ts.SeasonID and s.BrandID = ts.BrandID
inner join ThreadColorComb tcc with(nolock) on tcc.StyleUkey = s.Ukey
inner join pass1 p on p.id = s.ThreadEditname
where p.EMail is not null and p.EMail <>'' and ts.id = '{this.CurrentMaintain["ID"]}'";
            if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
            {
                string toAddress = MyUtility.Convert.GetString(dr[0]);
                string subject = $"IE P01 Factory GSD Style：{this.CurrentMaintain["StyleID"]} ,Brand：{this.CurrentMaintain["BrandID"]} ,Season：{this.CurrentMaintain["SeasonID"]} have changed ";
                string description = $@"Please regenerate Thread P01.Thread Color Combination data.";
                var email = new MailTo(Sci.Env.Cfg.MailFrom, toAddress, string.Empty, subject, string.Empty, description, true, true);
                email.ShowDialog();
            }
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

            Sci.Production.IE.P01_Print callNextForm = new Sci.Production.IE.P01_Print(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        // Style
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            // sql參數
            SqlParameter sp1 = new SqlParameter
            {
                ParameterName = "@brandid",
                Value = MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"])
            };

            IList<SqlParameter> cmds = new List<SqlParameter>
            {
                sp1
            };

            string sqlCmd = "select ID,SeasonID,Description,BrandID,UKey from Style WITH (NOLOCK) where Junk = 0 order by ID";
            if (!MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                sqlCmd = "select ID,SeasonID,Description,BrandID,UKey from Style WITH (NOLOCK) where Junk = 0 and BrandID = @brandid order by ID";
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable styleData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            SelectItem item = new SelectItem(styleData, "ID,SeasonID,Description,BrandID", "14,6,40,10", this.Text, headercaptions: "Style,Season,Description,Brand")
            {
                Width = 780
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> selectedData = item.GetSelecteds();
            this.CurrentMaintain["StyleID"] = item.GetSelectedString();
            this.CurrentMaintain["SeasonID"] = selectedData[0]["SeasonID"].ToString();
            this.CurrentMaintain["BrandID"] = selectedData[0]["BrandID"].ToString();

            sqlCmd = string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetInt(selectedData[0]["UKey"]).ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out DataTable locationData);
            if (result)
            {
                if (locationData.Rows.Count == 1)
                {
                    this.CurrentMaintain["ComboType"] = locationData.Rows[0]["Location"].ToString();
                }
            }

            this.GenCD(null, null);  // 撈CD Code
        }

        // Brand
        private void TxtBrand_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
            SelectItem item = new SelectItem(sqlWhere, "10,30,30", this.Text, false, ",")
            {
                Width = 750
            };
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["BrandID"] = item.GetSelectedString();
        }

        // Art. Sum
        private void BtnArtSum_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_ArtworkSummary callNextForm = new Sci.Production.IE.P01_ArtworkSummary("TimeStudy_Detail", Convert.ToInt64(this.CurrentMaintain["ID"]));
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // Sketch
        private void BtnSketch_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_Sketch callNextForm = new Sci.Production.IE.P01_Sketch(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // New Version
        private void BtnNewVersion_Click(object sender, EventArgs e)
        {
            // 將現有資料寫入TimeStudyHistory,TimeStudyHistory_History，並將現有資料的Version+1
            DialogResult confirmResult;
            confirmResult = MyUtility.Msg.QuestionBox("Are you sure you want to create new version?", caption: "Confirm", buttons: MessageBoxButtons.YesNo);
            if (confirmResult == System.Windows.Forms.DialogResult.Yes)
            {
                string executeCmd = string.Format(
                    @"insert into TimeStudyHistory (StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion)
select StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion from TimeStudy where ID = {0}

declare @id bigint
select @id = @@IDENTITY

insert into TimeStudyHistory_Detail(ID,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID)
select @id,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID from TimeStudy_Detail where ID = {0}

update TimeStudy 
set Version = (select iif(isnull(max(Version),0)+1 < 10,'0'+cast(isnull(max(Version),0)+1 as varchar),cast(max(Version)+1as varchar)) from TimeStudy where ID = {0}) ,
    AddName = '{1}',
	AddDate = GETDATE(),
	EditName = '',
	EditDate = null
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
            if (confirmResult == System.Windows.Forms.DialogResult.Yes)
            {
                string executeCmd = string.Format(
                    @"insert into TimeStudyHistory (StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion)
select StyleID,SeasonID,ComboType,BrandID,Version,Phase,TotalSewingTime,NumberSewer,AddName,AddDate,EditName,EditDate,IETMSID,IETMSVersion from TimeStudy where ID = {0}

declare @id bigint
select @id = @@IDENTITY

insert into TimeStudyHistory_Detail(ID,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID)
select @id,Seq,OperationID,Annotation,PcsPerHour,Sewer,MachineTypeID,Frequency,IETMSSMV,Mold,SMV,SeamLength,MtlFactorID from TimeStudy_Detail where ID = {0}

declare @phase varchar(10)
select @phase = isnull(Phase,'') from TimeStudy where ID = {0}

update TimeStudy 
set Phase = iif(@phase = 'Estimate','Initial',iif(@phase = 'Initial','Prelim',iif(@phase = 'Prelim','Final','Estimate'))),
	Version = '01',
	EditName = '{1}',
	EditDate = GETDATE()
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
                if (confirmResult != System.Windows.Forms.DialogResult.Yes)
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
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
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
                sp3
            };
            if (isComboType)
            {
                cmds.Add(sp4);
            }

            #endregion

            string sqlCmd = @"select id.SEQ,id.OperationID,o.DescEN as OperationDescEN,id.Annotation,
                            iif(round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3) = 0,0,round(3600/round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3),1)) as PcsPerHour,
                            id.Frequency as Sewer,o.MachineTypeID,id.Frequency,
                            id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency as IETMSSMV,id.Mold,id.MtlFactorID,
                            round(id.SMV*(isnull(id.MtlFactorRate,0)/100+1)*id.Frequency*60,3) as SMV, o.SeamLength,s.IETMSID,s.IETMSVersion,
                            (isnull(o.SeamLength,0) * isnull(id.Frequency,0))  as ttlSeamLength ,o.MasterPlusGroup
                            from Style s WITH (NOLOCK) 
                            inner join IETMS i WITH (NOLOCK) on s.IETMSID = i.ID and s.IETMSVersion = i.Version
                            inner join IETMS_Detail id WITH (NOLOCK) on i.Ukey = id.IETMSUkey
                            left join Operation o WITH (NOLOCK) on id.OperationID = o.ID
                            --left join MtlFactor m WITH (NOLOCK) on o.MtlFactorID = m.ID and m.Type = 'F'
                            where s.ID = @id and s.SeasonID = @seasonid and s.BrandID = @brandid ";

            // if (isComboType) sqlCmd += " and id.Location = @location ";
            if (isComboType)
            {
                sqlCmd += " and id.Location = @location ";
            }

            sqlCmd += " order by id.SEQ";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable ietmsData);
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
                this.detailgrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
        }

        // History
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_History callNextForm = new Sci.Production.IE.P01_History(this.CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        // Std. GSD List
        private void BtnStdGSDList_Click(object sender, EventArgs e)
        {
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
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
                sp3
            };

            string sqlCmd = "select Ukey from Style WITH (NOLOCK) where ID = @id and SeasonID = @seasonid and BrandID = @brandid";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable styleUkey);
            if (!result)
            {
                MyUtility.Msg.WarningBox("SQL Connection fail!!\r\n" + result.ToString());
                return;
            }

            if (styleUkey.Rows.Count > 0)
            {
                Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(styleUkey.Rows[0]["UKey"]));
                DialogResult dresult = callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.WarningBox("Style not found!!");
            }
        }

        /// <summary>
        /// DetailGrid Insert、Append
        /// </summary>
        /// <param name="index">index</param>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            if (index == -1)
            {
                int seq = 0;
                if (((DataTable)this.detailgridbs.DataSource).DefaultView.Count > 1)
                {
                    seq = MyUtility.Convert.GetInt(((DataTable)this.detailgridbs.DataSource).Compute("max(seq)", string.Empty));
                }

                this.CurrentDetailData["Seq"] = MyUtility.Convert.GetString(seq + 10).PadLeft(4, '0');
            }
            else
            {
                DataRow dr = this.DetailDatas[this.detailgridbs.Position + 1];
                this.CurrentDetailData["Seq"] = dr["Seq"];
                int seq = MyUtility.Convert.GetInt(dr["Seq"]);
                for (int i = this.detailgridbs.Position + 1; i < this.DetailDatas.Count; i++)
                {
                    seq += 10;
                    this.DetailDatas[i]["Seq"] = MyUtility.Convert.GetString(seq).PadLeft(4, '0');
                }
            }
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
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@styleid";
            sp1.Value = this.CurrentMaintain["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.CurrentMaintain["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = this.CurrentMaintain["BrandID"].ToString();

            IList<SqlParameter> cmds = new List<SqlParameter>
            {
                sp1,
                sp2,
                sp3
            };

            string sqlCmd = "select CdCodeID from Style WITH (NOLOCK) where ID = @styleid and SeasonID = @seasonid and BrandID = @brandid";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out DataTable cdCode);
            if (!result)
            {
                this.displayCD.Value = string.Empty;
                MyUtility.Msg.ErrorBox("Query CdCode data fail!\r\n" + result.ToString());
            }
            else
            {
                this.displayCD.Value = cdCode.Rows.Count > 0 ? cdCode.Rows[0]["CdCodeID"].ToString() : string.Empty;
            }
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

        private void BtnLocationBatchUpdate_Click(object sender, EventArgs e)
        {
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

            this.SelectedDetailGridDataRow
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
                    if (MyUtility.Msg.QuestionBox("there is no " + seqTarget + " exists, can't locate insert position, append to the last. \r\n\r\ncontinue?") != System.Windows.Forms.DialogResult.Yes)
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
                            dr["Seq"] = MyUtility.Convert.GetString(preSeq + 10).PadLeft(4, '0');
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
    }
}
