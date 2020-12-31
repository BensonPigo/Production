using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using Sci.Production.Class;
using System.Data.SqlClient;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P03
    /// </summary>
    public partial class P03 : Win.Tems.Input6
    {
        private object totalGSD;
        private object totalCycleTime;
        private DataTable EmployeeData;
        private DataTable distdt;

        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboSewingLineTeam, 1, 1, "A,B");
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.gridicon.Append.Visible = false;

            this.splitContainer1.Panel1.Controls.Add(this.detailpanel);
            this.detailpanel.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// ClickLocate
        /// </summary>
        protected override void ClickLocate()
        {
            base.ClickLocate();
            this.OnDetailEntered();
        }

        /// <inheritdoc/>
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

                // 請參考IE P01註解
                if (this.QBCommand != null && this.QBCommand.Conditions.Count() == 0)
                {
                    this.QueryExpress = string.Empty;
                }

                this.ReloadDatas();

                GC.Collect();
            };

            this.grid.Columns["ID"].Visible = false;
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select *
    , [sortNO] = case when ld.IsHide = 1 then 1 
                      when ld.No = '' and ld.IsHide = 0 and ld.IsPPA = 0 then 2
                      when left(ld.No, 1) = 'P' then 3
                      else 4 end
from (
    select  ld.OriNO
	    , ld.No
	    , ld.IsPPA
        , [IsHide] = cast(iif(ld.IsHide is not null, ld.IsHide ,iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0)))) as bit)	    
	    , ld.MachineTypeID
	    , ld.MasterPlusGroup	
        , [Description]= IIF( o.DescEN = '' OR  o.DescEN IS NULL , ld.OperationID,o.DescEN)
	    , ld.Annotation
	    , ld.Template
	    , ld.Attachment
	    , ld.ThreadColor
	    , ld.Notice
	    , ld.EmployeeID
	    , e.Name as EmployeeName
        , e.Skill as EmployeeSkill
	    , iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100) as Efficiency
        , [IsGroupHeader] = cast(iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, 0) as bit)
	    , [IsSewingOperation] = cast(isnull(show.IsDesignatedArea, 0) as bit)
        , [IsShow] = cast(isnull(show.IsShowinIEP03, 1) as bit)
	    , ld.ID
	    , ld.GroupKey
	    , ld.Ukey
	    , ld.ActCycle
	    , ld.TotalGSD
	    , ld.TotalCycle
	    , ld.GSD
	    , ld.Cycle
        , ld.OperationID
        , ld.New
    from LineMapping_Detail ld WITH (NOLOCK) 
    left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
    left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
    outer apply (
	    select IsShowinIEP03 = atf.IsShowinIEP03
		    , IsSewingline = atf.IsSewingline
		    , IsDesignatedArea = m.IsDesignatedArea
	    from Operation o2 WITH (NOLOCK)
	    inner join MachineType m WITH (NOLOCK) on o2.MachineTypeID = m.ID
	    inner join ArtworkType at2 WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	    inner join ArtworkType_FTY atf WITH (NOLOCK) on at2.id= atf.ArtworkTypeID and atf.FactoryID = '{1}'
	    where o.ID = o2.ID
    )show
    where ld.ID = '{0}' 
)ld
order by case when ld.No = '' then 1
	    when left(ld.No, 1) = 'P' then 2
	    else 3
	    end, 
        ld.GroupKey",
                masterID,
                Env.User.Factory);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            if (this.CurrentMaintain.Empty())
            {
                this.numCPUPC.Value = null;
                this.displayDesc.Text = string.Empty;
                this.numTargetHrIdeal.Value = null;
                this.numDailydemandshiftIdeal.Value = null;
                this.numTaktTimeIdeal.Value = null;
                this.numTotalTimeDiff.Value = null;
                this.numEOLR.Value = null;
                this.numHighestTimeDiff.Value = null;
                this.numEffieiency.Value = null;
                this.numPPH.Value = null;
                this.numLBR.Value = null;
                this.numLLER.Value = null;
                this.listControlBindingSource1.DataSource = null;
                this.btnNotHitTargetReason.Enabled = false;
                return;
            }

            base.OnDetailEntered();
            string sqlCmd = string.Format("select Description,CPU from Style WITH (NOLOCK) where Ukey = {0}", this.CurrentMaintain["StyleUkey"].ToString());
            DataRow styleData;
            if (MyUtility.Check.Seek(sqlCmd, out styleData))
            {
                this.displayDesc.Value = styleData["Description"];
                this.numCPUPC.Value = Convert.ToDecimal(styleData["CPU"]);
            }
            else
            {
                this.displayDesc.Value = string.Empty;
                this.numCPUPC.Value = 0;
            }

            this.CalculateValue(0);
            this.SaveCalculateValue();
            this.btnNotHitTargetReason.Enabled = !MyUtility.Check.Empty(this.CurrentMaintain["IEReasonID"]);
            this.listControlBindingSource1.DataSource = this.distdt;

            this.Distable();
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Color backDefaultColor = this.detailgrid.DefaultCellStyle.BackColor;
            DataGridViewGeneratorTextColumnSettings no = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cycle = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings machine = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings operatorid = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings attachment = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings template = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings threadColor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings notice = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings ppa = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings hide = new DataGridViewGeneratorCheckBoxColumnSettings();

            TxtMachineGroup.CelltxtMachineGroup txtSubReason = (TxtMachineGroup.CelltxtMachineGroup)TxtMachineGroup.CelltxtMachineGroup.GetGridCell();

            #region No.的Valid
            no.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow[] selectrow = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("No = '{0}'", e.FormattedValue.ToString().Trim().PadLeft(4, '0')));
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (selectrow.Length == 0)
                    {
                        dr["ActCycle"] = DBNull.Value;
                    }
                    else
                    {
                        dr["ActCycle"] = selectrow[0]["ActCycle"];
                    }

                    if (MyUtility.Check.Empty(e.FormattedValue) || (e.FormattedValue.ToString() != dr["No"].ToString()))
                    {
                        string oldValue = MyUtility.Check.Empty(dr["No"]) ? string.Empty : dr["No"].ToString();
                        dr["No"] = MyUtility.Check.Empty(e.FormattedValue) ? string.Empty : e.FormattedValue.ToString().Trim().PadLeft(4, '0');
                        dr.EndEdit();

                        this.ReclculateGridGSDCycleTime(oldValue);
                        this.ReclculateGridGSDCycleTime(dr["No"].ToString());
                        this.ComputeTaktTime();
                    }

                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };

            no.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    return;
                }

                if (MyUtility.Convert.GetBool(dr["IsHide"]))
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
                }
                else
                {
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                }
            };
            #endregion
            #region Cycle的Valid
            cycle.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Cycle"] = 0;
                        dr["Efficiency"] = 0;
                    }
                    else
                    {
                        if (e.FormattedValue.ToString() != dr["Cycle"].ToString())
                        {
                            if (MyUtility.Convert.GetDecimal(e.FormattedValue) < 0)
                            {
                                dr["Cycle"] = 0;
                                dr["Efficiency"] = 0;
                                MyUtility.Msg.WarningBox("Cycle time can't less than 0!!");
                            }
                            else
                            {
                                dr["Cycle"] = MyUtility.Convert.GetDecimal(e.FormattedValue);
                                dr["Efficiency"] = Math.Round(MyUtility.Convert.GetDecimal(dr["GSD"]) / MyUtility.Convert.GetDecimal(dr["Cycle"]), 2) * 100;
                            }
                        }
                    }

                    dr.EndEdit();
                    this.ReclculateGridGSDCycleTime(MyUtility.Check.Empty(dr["No"]) ? string.Empty : dr["No"].ToString());
                    this.ComputeTaktTime();
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };
            #endregion
            #region ST/MC type的按右鍵與Validating
            machine.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = "select ID,Description from MachineType WITH (NOLOCK) where Junk = 0";
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8,43", dr["MachineTypeID"].ToString())
                            {
                                Width = 590,
                            };
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

            machine.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["MachineTypeID"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter
                        {
                            ParameterName = "@id",
                            Value = e.FormattedValue.ToString(),
                        };

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
                        {
                            sp1,
                        };
                        string sqlCmd = "select ID from MachineType WITH (NOLOCK) where Junk = 0 and ID = @id";
                        DataTable machineData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out machineData);
                        if (!result)
                        {
                            dr["MachineTypeID"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            return;
                        }
                        else
                        {
                            if (machineData.Rows.Count <= 0)
                            {
                                dr["MachineTypeID"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< ST/MC type: {0} > not found!!!", e.FormattedValue.ToString()));
                                return;
                            }
                        }
                    }
                }
            };
            #endregion
            #region Operator ID No.的按右鍵與Validating
            operatorid.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                            this.GetEmployee(null);

                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.EmployeeData, "ID,Name,Skill,SewingLineID,FactoryID", "10,18,16,2,5", dr["EmployeeID"].ToString(), headercaptions: "ID,Name,Skill,SewingLine,Factory")
                            {
                                Width = 700,
                            };
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> selectedData = item.GetSelecteds();
                            dr["EmployeeID"] = selectedData[0]["ID"];
                            dr["EmployeeName"] = selectedData[0]["Name"];
                            dr["EmployeeSkill"] = selectedData[0]["Skill"];
                            dr.EndEdit();
                        }
                    }
                }
            };
            operatorid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.ReviseEmployeeToEmpty(dr);
                        return;
                    }

                    if (e.FormattedValue.ToString() != dr["EmployeeID"].ToString())
                    {
                        this.GetEmployee(e.FormattedValue.ToString());
                        if (this.EmployeeData.Rows.Count <= 0)
                        {
                            this.ReviseEmployeeToEmpty(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< Employee ID: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            dr["EmployeeID"] = this.EmployeeData.Rows[0]["ID"];
                            dr["EmployeeName"] = this.EmployeeData.Rows[0]["Name"];
                            dr["EmployeeSkill"] = this.EmployeeData.Rows[0]["Skill"];
                            dr.EndEdit();
                        }
                    }
                }
            };
            #endregion
            #region Attachment
            attachment.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select ID,DescEN 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsAttachment = 1
union all
select ID, Description
from SewingMachineAttachment WITH (NOLOCK) 
where Junk = 0";

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "ID,DescEN", "13,60,10", this.CurrentDetailData["Attachment"].ToString(), null, null, null)
                    {
                        Width = 666,
                    };
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Attachment"] = item.GetSelectedString();
                }
            };

            attachment.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    List<SqlParameter> cmds = new List<SqlParameter>() { new SqlParameter { ParameterName = "@OperationID", Value = MyUtility.Convert.GetString(this.CurrentDetailData["OperationID"]) } };
                    string sqlcmd = @"
select [Attachment] = STUFF((
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
                        this.CurrentDetailData["Attachment"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    if (dtOperation.Rows.Count > 0)
                    {
                        operationList = dtOperation.Rows[0]["Attachment"].ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                        if (this.CurrentDetailData["Template"].Empty())
                        {
                            this.CurrentDetailData["Template"] = dtOperation.Rows[0]["Template"].ToString();
                        }

                        if (e.FormattedValue.Empty())
                        {
                            this.CurrentDetailData["Attachment"] = dtOperation.Rows[0]["Attachment"].ToString();

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
                        this.CurrentDetailData["Attachment"] = string.Empty;
                        MyUtility.Msg.WarningBox("SQL Connection failt!!\r\n" + result.ToString());
                    }

                    // 前端轉進來的資料是用[;]區隔，統一用[,]區隔
                    List<string> getMold = e.FormattedValue.ToString().Replace(";", ",").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                    // 不存在 operation
                    var existsOperation = operationList.Except(getMold);
                    if (existsOperation.Any() && operationList.Any())
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["Attachment"] = string.Join(",", getMold.Except(existsOperation).ToList());
                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsOperation.ToList()) + "  need include in Operation setting !!", "Data need include in setting");
                        return;
                    }

                    // 不存在 Mold
                    var existsMold = getMold.Except(dtMold.AsEnumerable().Select(x => x.Field<string>("ID")).ToList());
                    if (existsMold.Any())
                    {
                        e.Cancel = true;
                        this.CurrentDetailData["Attachment"] = string.Join(",", getMold.Where(x => existsMold.Where(y => !y.EqualString(x)).Any()).ToList());
                        MyUtility.Msg.WarningBox("Attachment : " + string.Join(",", existsMold.ToList()) + "  need include in Mold setting !!", "Data need include in setting");
                        return;
                    }

                    this.CurrentDetailData["Attachment"] = string.Join(",", getMold.ToList());
                }
            };
            #endregion
            #region Template
            template.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    string sqlcmd = @"
select ID,DescEN 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsTemplate = 1
union all
select ID, Description
from SewingMachineTemplate WITH (NOLOCK) 
where Junk = 0
";

                    Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, "ID,DescEN", "13,60,10", this.CurrentDetailData["Template"].ToString(), null, null, null)
                    {
                        Width = 666,
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
                    string sqlcmd = @"
select ID,DescEN 
from Mold WITH (NOLOCK) 
where Junk = 0
and IsTemplate = 1
union all
select ID, Description
from SewingMachineTemplate WITH (NOLOCK) 
where Junk = 0
";

                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
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

            hide.CellValidating += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!this.EditMode)
                {
                    dr["IsHide"] = dr["IsHide"];
                    return;
                }

                if (this.EditMode)
                {
                    /*
                     * if (MyUtility.Convert.GetBool(dr["IsGroupHeader"]))
                    {
                        MyUtility.Msg.ErrorBox("This operation is [Group Header], cannot modify.");
                        dr["IsHide"] = 1;
                        return;
                    }
                    */

                    /*
                    if (MyUtility.Convert.GetBool(dr["IsSewingOperation"]))
                    {
                        MyUtility.Msg.ErrorBox("This Artwrok is sewing operation, cannot modify.");
                        dr["IsHide"] = 1;
                        return;
                    }
                    */

                    if (MyUtility.Convert.GetBool(e.FormattedValue))
                    {
                        string noo = dr["No"].ToString(); // 紀錄要被刪除的No
                        dr["No"] = string.Empty;
                        dr["IsPPA"] = 0;
                        dr["IsHide"] = 1;
                        this.SumNoGSDCycleTime(dr["GroupKey"].ToString());
                        this.AssignNoGSDCycleTime(dr["GroupKey"].ToString());
                        if (noo != string.Empty)
                        {
                            this.ReclculateGridGSDCycleTime(noo); // 重算被刪除掉的No的TotalGSD & Total Cycle Time
                        }
                    }
                    else
                    {
                        dr["IsHide"] = 0;
                        this.SumNoGSDCycleTime(dr["GroupKey"].ToString());
                        this.AssignNoGSDCycleTime(dr["GroupKey"].ToString());
                    }

                    this.ComputeTaktTime();
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };

            ppa.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetBool(e.FormattedValue))
                    {
                        dr["IsPPA"] = 1;
                        dr["IsHide"] = 0;
                        dr.EndEdit();
                    }
                    else
                    {
                        dr["IsPPA"] = 0;
                        dr.EndEdit();
                    }

                    this.ComputeTaktTime();
                    this.Distable();
                    this.detailgrid.Focus();
                    this.detailgrid.CurrentCell = this.detailgrid[e.ColumnIndex, e.RowIndex];
                    this.detailgrid.BeginEdit(true);
                }
            };

            threadColor.MaxLength = 1;
            no.MaxLength = 4;
            notice.MaxLength = 600;

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("OriNo", header: "OriNo.", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Text("No", header: "No.", width: Widths.AnsiChars(4), settings: no)
            .CheckBox("IsPPA", header: "PPA", width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false, settings: ppa)
            .CheckBox("IsHide", header: "Hide", width: Widths.AnsiChars(1), iseditable: true, trueValue: true, falseValue: false, settings: hide)
            .Text("MachineTypeID", header: "ST/MC type", width: Widths.AnsiChars(10), settings: machine)
            .Text("MasterPlusGroup", header: "Machine Group", width: Widths.AnsiChars(10), settings: txtSubReason)
            .EditText("Description", header: "Operation", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .EditText("Annotation", header: "Annotation", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Numeric("GSD", header: "GSD Time", width: Widths.AnsiChars(5), decimal_places: 2, iseditingreadonly: true)
            .Numeric("Cycle", header: "Cycle Time", width: Widths.AnsiChars(5), integer_places: 4, decimal_places: 2, minimum: 0, settings: cycle)
            .Text("Attachment", header: "Attachment", width: Widths.AnsiChars(10), settings: attachment)
            .Text("Template", header: "Template", width: Widths.AnsiChars(10), settings: template)
            .Text("ThreadColor", header: "ThreadColor", width: Widths.AnsiChars(1), settings: threadColor)
            .Text("Notice", header: "Notice", width: Widths.AnsiChars(60), settings: notice)
            .Text("EmployeeID", header: "Operator ID No.", width: Widths.AnsiChars(10), settings: operatorid)
            .Text("EmployeeName", header: "Operator Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("EmployeeSkill", header: "Skill", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Efficiency", header: "Eff(%)", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true);

            this.detailgrid.Columns["OriNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["No"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["GSD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["Cycle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["MachineTypeID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["EmployeeID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["EmployeeName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.Columns["EmployeeSkill"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.detailgrid.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
                #region 變色規則，若該 Row 已經變色則跳過
                if (dr["New"].ToString().ToUpper() == "TRUE")
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(255, 186, 117))
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 186, 117);
                    }
                }
                else
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                    }
                }
                #endregion
            };

            // [No.] 特殊排序規則 [Hide]有打勾-> [No.][PPA][Hide]皆空白-> [No.]為P開頭-> [No.]為一般數字
            int rowIndex = 0;
            int columIndex = 0;
            this.detailgrid.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.detailgrid.Sorted += (s, e) =>
            {
                this.Distable();

                if (columIndex == 1)
                {
                    DataTable dt = (DataTable)this.detailgridbs.DataSource;
                    dt.DefaultView.Sort = "sortNO, No";
                    this.detailgridbs.DataSource = dt;

                    this.IsShowTable();
                }
            };

            Ict.Win.UI.DataGridViewNumericBoxColumn act;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("No", header: "No.", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("TotalCycle", header: "Act.\r\nCycle\r\nTime", width: Widths.AnsiChars(3), integer_places: 5, decimal_places: 2, iseditingreadonly: true/*, settings: ac*/).Get(out act)
            .Numeric("TotalGSD", header: "Ttl\r\nGSD\r\nTime", width: Widths.AnsiChars(3), decimal_places: 2, iseditingreadonly: true);
        }

        // 撈出Employee資料
        private void GetEmployee(string iD)
        {
            string sqlCmd;

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", this.CurrentMaintain["FactoryID"].ToString());
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "@id",
            };
            if (iD != null)
            {
                sp2.Value = iD;
            }
            else
            {
                sp2.Value = DBNull.Value;
            }

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
            {
                sp1,
                sp2,
            };

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                sqlCmd = "select ID,Name,Skill,SewingLineID,FactoryID from Employee WITH (NOLOCK) where ResignationDate is null" + (iD == null ? string.Empty : " and ID = @id");
            }
            else
            {
                sqlCmd = "select ID,Name,Skill,SewingLineID,FactoryID from Employee WITH (NOLOCK) where ResignationDate is null and FactoryID = @factoryid" + (iD == null ? string.Empty : " and ID = @id");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out this.EmployeeData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
            }
        }

        // 將Employee相關欄位值清空
        private void ReviseEmployeeToEmpty(DataRow dr)
        {
            dr["EmployeeID"] = string.Empty;
            dr["EmployeeName"] = string.Empty;
            dr["EmployeeSkill"] = string.Empty;
            dr.EndEdit();
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.txtStyleComboType.BackColor = Color.White;
        }

        /// <summary>
        /// ClickCopyAfter
        /// </summary>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["Version"] = DBNull.Value;
            this.CurrentMaintain["IEReasonID"] = string.Empty;
            this.CurrentMaintain["Status"] = "New";
            this.txtStyleComboType.BackColor = Color.White;
        }

        /// <summary>
        /// OnEditModeChanged
        /// </summary>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.grid1 != null)
            {
                this.grid1.IsEditingReadOnly = !this.EditMode;
            }
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't modify this record!!");
                return false;
            }

            this.txtStyleComboType.BackColor = Color.White;
            return true;
        }

        /// <summary>
        /// ClickDeleteBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
            if (!PublicPrg.Prgs.GetAuthority(this.CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so you can't delete this record!!");
                return false;
            }

            if (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, so can't delete this record!!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleID"]))
            {
                MyUtility.Msg.WarningBox("Style can't empty");
                this.txtStyleID.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ComboType"]))
            {
                MyUtility.Msg.WarningBox("Combo type can't empty");
                this.txtStyleComboType.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("Factory can't empty");
                this.txtFactory.Focus();
                return false;
            }

            if (MyUtility.Convert.GetDecimal(MyUtility.Convert.GetString(this.CurrentMaintain["Workhour"])) == 0)
            {
                MyUtility.Msg.WarningBox("<No .of Hours> cannot be 0!!");
                this.numNoOfHours.Focus();
                return false;
            }
            #endregion

            string chkfactory = $@"select 1 from factory where FTYGroup = '{this.txtFactory.Text}'";
            if (!MyUtility.Check.Seek(chkfactory))
            {
                MyUtility.Msg.WarningBox($"Factory:{this.txtFactory.Text} not found");
                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can not empty!");
                return false;
            }

            var queryIsGroupHeader = this.DetailDatas.Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == false && x.Field<bool?>("IsGroupHeader") == true);
            if (queryIsGroupHeader.Any())
            {
                string msg = "It must be selected if the operation is [Group Header]." + Environment.NewLine;
                foreach (DataRow row in queryIsGroupHeader)
                {
                    msg += "[OriNo]: " + row["OriNo"].ToString() + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            var queryHideAndPPA = this.DetailDatas.Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == true && x.Field<bool?>("IsPPA") == true);
            if (queryHideAndPPA.Any())
            {
                MyUtility.Msg.WarningBox("<PPA> and <Hide> cannot be selected at the same time.");
                return false;
            }

            var queryIsHide = this.DetailDatas.Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == true && !x.Field<string>("No").Empty());
            if (queryIsHide.Any())
            {
                string msg = "These operations cannot be [Hide] and have [No.] in the same time." + Environment.NewLine;
                foreach (DataRow row in queryIsHide)
                {
                    msg += "[Operation]: " + row["Description"].ToString() + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            var queryIsPPA = this.DetailDatas
                .Where(x => x.Field<bool?>("IsShow") == true && x.Field<bool?>("IsHide") == false
                               && ((x.Field<bool?>("IsPPA") == true && (x.Field<string>("No").Empty() || !x.Field<string>("No").Substring(0, 1).Equals("P")))
                                || (x.Field<bool?>("IsPPA") == false && !x.Field<string>("No").Empty() && x.Field<string>("No").Substring(0, 1).Equals("P"))));
            if (queryIsPPA.Any())
            {
                MyUtility.Msg.WarningBox("The [No.] first word must be P if the [PPA] is checked.");
                return false;
            }

            this.ComputeTaktTime();

            // Vision為空的話就要填值
            if (MyUtility.Check.Empty(this.CurrentMaintain["Version"]) || this.CurrentMaintain["Version"].ToString() == "0")
            {
                string newVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(max(Version),0)+1 as Newversion from LineMapping WITH (NOLOCK) where StyleUKey =  {0}", this.CurrentMaintain["StyleUkey"].ToString()));
                if (MyUtility.Check.Empty(newVersion))
                {
                    MyUtility.Msg.WarningBox("Get Version fail!!");
                    return false;
                }

                this.CurrentMaintain["Version"] = newVersion;
            }

            this.txtStyleComboType.BackColor = this.txtStyleID.BackColor;

            return true;
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
UPDATE LineMapping_Detail
SET MasterPlusGroup = '{item["MasterPlusGroup"]}'
    ,IsHide = '{item["IsHide"]}'
WHERE Ukey={item["Ukey"]}

";
                }

                DualResult reusult = DBProxy.Current.Execute(null, updCmd);
                if (!reusult)
                {
                    this.ShowErr(reusult);
                }
            }

            this.Distable();
        }

        /// <summary>
        /// OnDetailGridInsertClick
        /// </summary>
        protected override void OnDetailGridInsertClick()
        {
            DataRow newrow, tmp;

            // 先紀錄目前Grid所指道的那筆資料
            tmp = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            if (tmp.Empty())
            {
                return;
            }

            this.SumNoGSDCycleTime(this.CurrentDetailData["GroupKey"].ToString());
            base.OnDetailGridInsertClick();
            newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
            newrow.ItemArray = tmp.ItemArray; // 將剛剛紀錄的資料複製到新增的那筆record
            this.CurrentDetailData["New"] = true;
            this.CurrentDetailData["No"] = string.Empty;
            this.AssignNoGSDCycleTime(this.CurrentDetailData["GroupKey"].ToString());
            this.ComputeTaktTime();
        }

        /// <summary>
        /// OnDetailGridDelete
        /// </summary>
        protected override void OnDetailGridDelete()
        {
            if (this.detailgrid.Rows.Count != 0)
            {
                if (this.CurrentDetailData["New"].ToString().ToUpper() == "FALSE")
                {
                    if (MyUtility.Convert.GetDecimal(this.CurrentDetailData["GSD"]) != 0)
                    {
                        MyUtility.Msg.WarningBox("This record is set up by system, can't delete!!");
                        return;
                    }
                }

                string no = this.CurrentDetailData["No"].ToString(); // 紀錄要被刪除的No
                string groupkey = this.CurrentDetailData["GroupKey"].ToString();
                this.SumNoGSDCycleTime(groupkey);
                base.OnDetailGridDelete();
                this.AssignNoGSDCycleTime(groupkey);
                if (no != string.Empty)
                {
                    this.ReclculateGridGSDCycleTime(no); // 傳算被刪除掉的No的TotalGSD & Total Cycle Time
                }

                this.ComputeTaktTime();
            }
        }

        /// <summary>
        /// ClickPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            P03_Print callNextForm = new P03_Print(this.CurrentMaintain, MyUtility.Convert.GetDecimal(this.numCPUPC.Value));
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        /// <summary>
        /// ClickUndo
        /// </summary>
        protected override void ClickUndo()
        {
            base.ClickUndo();
            this.txtStyleComboType.BackColor = this.txtStyleID.BackColor;
            this.OnDetailEntered();
        }

        // 加總傳入的GroupKey的GSD & Cycle Time
        private void SumNoGSDCycleTime(string groupKey)
        {
            this.totalGSD = ((DataTable)this.detailgridbs.DataSource).Compute("sum(GSD)", string.Format("GroupKey = {0}", groupKey));
            this.totalCycleTime = ((DataTable)this.detailgridbs.DataSource).Compute("sum(Cycle)", string.Format("GroupKey = {0}", groupKey));
        }

        // 填輸入的GroupKey的GSD & Cycle Time
        private void AssignNoGSDCycleTime(string groupKey)
        {
            object countRec = ((DataTable)this.detailgridbs.DataSource).Compute("count(GroupKey)", string.Format("GroupKey = {0}", groupKey));
            decimal avgGSD = MyUtility.Check.Empty(Convert.ToDecimal(countRec)) ? MyUtility.Convert.GetDecimal(this.totalGSD) : Math.Round(MyUtility.Convert.GetDecimal(this.totalGSD) / MyUtility.Convert.GetDecimal(countRec), 2);
            decimal avgCycleTime = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(countRec)) ? MyUtility.Convert.GetDecimal(this.totalCycleTime) : Math.Round(MyUtility.Convert.GetDecimal(this.totalCycleTime) / MyUtility.Convert.GetDecimal(countRec), 2);
            DataRow[] findRow = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("GroupKey = {0}", groupKey));
            int i = 0;
            decimal sumGSD = 0, sumCycleTime = 0;

            // 平均分配Cycle Time與GSD，若有餘數就放置最後一筆
            foreach (DataRow dr in findRow)
            {
                i++;
                if (i >= MyUtility.Convert.GetInt(countRec))
                {
                    dr["GSD"] = MyUtility.Convert.GetDecimal(this.totalGSD) - sumGSD;
                    dr["Cycle"] = MyUtility.Convert.GetDecimal(this.totalCycleTime) - sumCycleTime;
                }
                else
                {
                    dr["GSD"] = avgGSD;
                    dr["Cycle"] = avgCycleTime;
                    sumGSD = sumGSD + MyUtility.Convert.GetDecimal(dr["GSD"]);
                    sumCycleTime = sumCycleTime + MyUtility.Convert.GetDecimal(dr["Cycle"]);
                }
            }

            foreach (DataRow dr in findRow)
            {
                this.ReclculateGridGSDCycleTime(dr["No"].ToString());
            }
        }

        // 計算DailyDemand,NetTime,TaktTime,LLER,Ideal Target / Hr. (100%), Ideal Daily demand/shift, Ideal Takt Time欄位值
        private void CalculateValue(int type)
        {
            if (type == 1)
            {
                this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StandardOutput"]), 0);
                this.CurrentMaintain["NetTime"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * 3600, 0);
                this.CurrentMaintain["TaktTime"] = MyUtility.Check.Empty(this.CurrentMaintain["DailyDemand"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 0);
            }

            this.numLLER.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TaktTime"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TaktTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]) * 100, 2);
            this.numTargetHrIdeal.Value = MyUtility.Check.Empty(this.CurrentMaintain["TotalGSD"]) ? 0 : MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["IdealOperators"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]), 0);
            this.numDailydemandshiftIdeal.Value = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(this.numTargetHrIdeal.Value), 0);
            this.numTaktTimeIdeal.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.numDailydemandshiftIdeal.Value)) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.numDailydemandshiftIdeal.Value), 0);
        }

        // Compute Takt Time
        private void ComputeTaktTime()
        {
            object sumGSD = ((DataTable)this.detailgridbs.DataSource).Compute("sum(GSD)", "(IsHide = 0 or  IsHide is null)");
            object sumCycle = ((DataTable)this.detailgridbs.DataSource).Compute("sum(Cycle)", "(IsHide = 0 or  IsHide is null)");
            object maxHighGSD = ((DataTable)this.detailgridbs.DataSource).Compute("max(GSD)", "(IsHide = 0 or  IsHide is null)");
            object maxHighCycle = ((DataTable)this.detailgridbs.DataSource).Compute("max(Cycle)", "(IsHide = 0 or  IsHide is null)");

            // object countopts = ((DataTable)detailgridbs.DataSource).Compute("count(No)", "");
            int countopts = 0;

            var temptable = this.DetailDatas.CopyToDataTable();
            temptable.DefaultView.Sort = "No";
            string no = string.Empty;
            foreach (DataRow dr in temptable.DefaultView.ToTable().Select("(IsHide = 0 or  IsHide is null)"))
            {
                if (!MyUtility.Check.Empty(dr["No"]) && no != dr["No"].ToString())
                {
                    countopts += 1;
                    no = dr["No"].ToString();
                }
            }

            this.CurrentMaintain["TotalGSD"] = sumGSD;
            this.CurrentMaintain["TotalCycle"] = sumCycle;
            this.CurrentMaintain["HighestGSD"] = maxHighGSD;
            this.CurrentMaintain["HighestCycle"] = maxHighCycle;
            this.CurrentMaintain["CurrentOperators"] = countopts;
            this.CurrentMaintain["StandardOutput"] = MyUtility.Check.Empty(this.CurrentMaintain["TotalCycle"]) ? 0 : MyUtility.Math.Round(3600 * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"]), 0);
            this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["StandardOutput"]), 0);
            this.CurrentMaintain["TaktTime"] = MyUtility.Check.Empty(this.CurrentMaintain["DailyDemand"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 0);
        }

        // 計算Total % time diff,Highest % time diff,Effieiency(%),Effieiency(%),PPH,LBR欄位值
        private void SaveCalculateValue()
        {
            this.numTotalTimeDiff.Value = MyUtility.Check.Empty(this.CurrentMaintain["TotalGSD"]) ? 0 : MyUtility.Math.Round(((MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]) - MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"])) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"])) * 100, 2);
            this.numHighestTimeDiff.Value = MyUtility.Check.Empty(this.CurrentMaintain["HighestGSD"]) ? 0 : MyUtility.Math.Round(((MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSD"]) - MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"])) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestGSD"])) * 100, 2);
            this.numEOLR.Value = MyUtility.Check.Empty(this.CurrentMaintain["HighestCycle"]) ? 0 : MyUtility.Math.Round(3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]), 2);
            this.numEffieiency.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round((MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalGSD"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) * 100, 2);
            this.numPPH.Value = MyUtility.Check.Empty(this.CurrentMaintain["CurrentOperators"]) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.numEOLR.Value) * MyUtility.Convert.GetDecimal(this.numCPUPC.Value) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]), 4);
            this.numLBR.Value = MyUtility.Check.Empty(MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"])) ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["TotalCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["HighestCycle"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["CurrentOperators"]) * 100, 2);
        }

        // 重新計算Grid的Cycle Time
        private void ReclculateGridGSDCycleTime(string no)
        {
            object gSD = ((DataTable)this.detailgridbs.DataSource).Compute("Sum(GSD)", string.Format("(IsHide = 0 or  IsHide is null)  and No = '{0}'", no));
            object cycle = ((DataTable)this.detailgridbs.DataSource).Compute("Sum(Cycle)", string.Format("(IsHide = 0 or  IsHide is null) and No = '{0}'", no));

            DataRow[] findRow = ((DataTable)this.detailgridbs.DataSource).Select(string.Format("(IsHide = 0 or  IsHide is null) and No = '{0}'", no));
            if (findRow.Length > 0)
            {
                foreach (DataRow dr in findRow)
                {
                    dr["TotalGSD"] = gSD;
                    dr["TotalCycle"] = cycle;
                    dr["ActCycle"] = cycle;
                    dr.EndEdit();
                }
            }
        }

        // No. of Hours
        private void NumNoOfHours_Validated(object sender, EventArgs e)
        {
            this.CalculateValue(1);
        }

        // Ideal No. of Oprts
        private void NumOprtsIdeal_Validated(object sender, EventArgs e)
        {
            this.CalculateValue(0);
        }

        // Style#
        private void TxtStyleID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select ID,SeasonID,BrandID,Description,CPU,Ukey from Style WITH (NOLOCK) where Junk = 0 order by ID,SeasonID";

            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "15,8,10,40,5,6", this.txtStyleID.Text, "Style#,Season,Brand,Description,CPU,Key", columndecimals: "0,0,0,0,3,0")
            {
                Width = 838,
            };
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            IList<DataRow> styleData = item.GetSelecteds();
            this.CurrentMaintain["StyleID"] = styleData[0]["ID"];
            this.CurrentMaintain["SeasonID"] = styleData[0]["SeasonID"];
            this.CurrentMaintain["BrandID"] = styleData[0]["BrandID"];
            this.CurrentMaintain["StyleUKey"] = styleData[0]["Ukey"];
            this.displayDesc.Value = styleData[0]["Description"];
            this.numCPUPC.Value = MyUtility.Convert.GetDecimal(styleData[0]["CPU"]);

            DataTable comboType;
            DualResult result = DBProxy.Current.Select(null, string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", this.CurrentMaintain["StyleUKey"].ToString()), out comboType);
            if (result)
            {
                if (comboType.Rows.Count > 1)
                {
                    item = new Win.Tools.SelectItem(comboType, "Location", "2", string.Empty, "Combo Type");
                    returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentMaintain["ComboType"] = item.GetSelectedString();
                }
                else
                {
                    if (comboType.Rows.Count != 0)
                    {
                        this.CurrentMaintain["ComboType"] = comboType.Rows[0]["Location"];
                    }
                    else
                    {
                        this.CurrentMaintain["ComboType"] = string.Empty;
                    }
                }
            }
        }

        // Combo Type
        private void TxtStyleComboType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select Location from Style_Location WITH (NOLOCK) where StyleUkey = {0}", this.CurrentMaintain["StyleUKey"].ToString()), "2", string.Empty, "Combo Type");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ComboType"] = item.GetSelectedString();
        }

        // 撈出ChgOverTarget資料
        private string FindTarget(string type)
        {
            return MyUtility.GetValue.Lookup(string.Format(
                @"select Target from ChgOverTarget WITH (NOLOCK) where Type = '{0}' and MDivisionID = '{1}' and EffectiveDate = (
select MAX(EffectiveDate) from ChgOverTarget WITH (NOLOCK) where Type = '{0}' and MDivisionID = '{1}' and EffectiveDate <= GETDATE())",
                type,
                Env.User.Keyword));
        }

        /// <summary>
        /// ClickConfirm
        /// </summary>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            if (!PublicPrg.Prgs.GetAuthority(this.CurrentMaintain["AddName"].ToString()))
            {
                MyUtility.Msg.WarningBox("This record is not created by yourself, so can't confirm!");
                return;
            }

            #region 檢查表身不可為空
            DataRow[] findrow = ((DataTable)this.detailgridbs.DataSource).Select("(IsHide = 0 or  IsHide is null) and (No = '' or No is null)");
            if (findrow.Length > 0)
            {
                MyUtility.Msg.WarningBox("< No. > can't empty!!");
                return;
            }
            #endregion

            this.ComputeTaktTime();

            string lBRTarget = this.FindTarget("LBR");
            string lLERTarget = this.FindTarget("LLER");
            bool checkLBR = !MyUtility.Check.Empty(lBRTarget) && Convert.ToDecimal(this.numLBR.Value) < Convert.ToDecimal(lBRTarget);
            bool checkLLER = !MyUtility.Check.Empty(lLERTarget) && Convert.ToDecimal(this.numLLER.Value) < Convert.ToDecimal(lLERTarget);
            string notHitReasonID = string.Empty;

            if (checkLBR || checkLLER)
            {
                StringBuilder msg = new StringBuilder();
                if (checkLBR)
                {
                    msg.Append("LBR is lower than target.\r\n");
                }

                if (checkLLER)
                {
                    msg.Append("LLER is lower than target.\r\n");
                }

                MyUtility.Msg.WarningBox(msg.ToString() + "Please select not hit target reason.");
                string sqlCmd = "select ID, Description from IEReason WITH (NOLOCK) where Type = 'LM' and Junk = 0";
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "5,30", string.Empty);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                notHitReasonID = item.GetSelectedString();
            }

            DualResult result;
            string updateCmd = string.Format("update LineMapping set Status = 'Confirmed', IEReasonID = '{0}',EditName = '{1}', EditDate = GETDATE() where ID = {2}", notHitReasonID, Env.User.UserID, this.CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Confirm fail!\r\n" + result.ToString());
                return;
            }
        }

        /// <summary>
        /// ClickUnconfirm
        /// </summary>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            DualResult result;
            string updateCmd = string.Format("update LineMapping set Status = 'New', IEReasonID = '',EditName = '{0}', EditDate = GETDATE() where ID = {1}", Env.User.UserID, this.CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Unconfirm fail!\r\n" + result.ToString());
                return;
            }
        }

        // Not hit target reason
        private void BtnNotHitTargetReason_Click(object sender, EventArgs e)
        {
            // 不使用MyUtility.Msg.InfoBox的原因為MyUtility.Msg.InfoBox都有MessageBoxIcon
            MessageBox.Show(MyUtility.GetValue.Lookup(string.Format("select Description from IEReason WITH (NOLOCK) where Type = 'LM' and ID = '{0}'", this.CurrentMaintain["IEReasonID"].ToString())).PadRight(60), caption: "Not hit target reason");
        }

        // Copy from other line mapping
        private void BtnCopyFromOtherLineMapping_Click(object sender, EventArgs e)
        {
            P03_CopyFromOtherStyle callNextForm = new P03_CopyFromOtherStyle();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DataTable copyLineMapDetail;
                string sqlCmd = string.Format(
                    @"
select ID = null
	   , OriNo = ld.OriNo
	   , No = ld.No
	   , ld.Annotation
	   , ld.GSD
	   , ld.TotalGSD
	   , ld.Cycle
	   , ld.TotalCycle
	   , ld.MachineTypeID
       , ld.Attachment
       , ld.Template 
	   , ld.OperationID
	   , ld.MoldID
	   , ld.GroupKey
	   , ld.New
	   , ld.EmployeeID
	   , [Description] = IIF(o.DescEN = '' OR o.DescEN IS NULL, ld.OperationID, o.DescEN)
	   , EmployeeName = e.Name
	   , EmployeeSkill = e.Skill
	   , Efficiency = iif(ld.Cycle = 0,0,ROUND(ld.GSD/ld.Cycle,2)*100)
       , ld.isppa
       , ld.Threadcolor
       , ld.ActCycle
       , ld.MasterPlusGroup
		,[IsHide] = iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0)))
		,[IsGroupHeader] = iif(SUBSTRING(ld.OperationID, 1, 2) = '--', 1, 0)
		,[IsSewingOperation] = cast(iif(show.IsDesignatedArea = 1, 1, 0) as bit)
        ,[IsShow] = isnull(show.IsShowinIEP03, 1)
from LineMapping_Detail ld WITH (NOLOCK) 
left join Employee e WITH (NOLOCK) on ld.EmployeeID = e.ID
left join Operation o WITH (NOLOCK) on ld.OperationID = o.ID
outer apply (
	select IsShowinIEP03 = atf.IsShowinIEP03
		, IsSewingline = atf.IsSewingline
		, IsDesignatedArea = m.IsDesignatedArea
	from Operation o2 WITH (NOLOCK)
	inner join MachineType m WITH (NOLOCK) on o2.MachineTypeID = m.ID
	inner join ArtworkType at2 WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	inner join ArtworkType_FTY atf WITH (NOLOCK) on at2.id= atf.ArtworkTypeID and atf.FactoryID = '{1}'
	where o.ID = o2.ID
)show
where ld.ID = {0} 
order by case when ld.No = '' then 1
			when left(ld.No, 1) = 'P' then 2
			else 3
			end, 
        ld.GroupKey",
                    callNextForm.P03CopyLineMapping["ID"].ToString(),
                    Env.User.Factory);
                DualResult selectResult = DBProxy.Current.Select(null, sqlCmd, out copyLineMapDetail);
                if (!selectResult)
                {
                    MyUtility.Msg.ErrorBox("Query copy linemapping detail fail!!\r\n" + selectResult.ToString());
                    return;
                }

                // 刪除現有表身資料
                foreach (DataRow dr in this.DetailDatas)
                {
                    dr.Delete();
                }

                // 將要複製的資料寫入表身Grid
                foreach (DataRow dr in copyLineMapDetail.Rows)
                {
                    if (callNextForm.P03CopyLineMapping["FactoryID"].ToString() != this.CurrentMaintain["FactoryID"].ToString())
                    {
                        dr["EmployeeID"] = string.Empty;
                        dr["EmployeeName"] = string.Empty;
                        dr["EmployeeSkill"] = string.Empty;
                        dr.EndEdit();
                    }

                    dr.AcceptChanges();
                    dr.SetAdded();
                    ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
                }

                // 填入表頭資料
                this.CurrentMaintain["IdealOperators"] = callNextForm.P03CopyLineMapping["IdealOperators"].ToString();
                this.CurrentMaintain["CurrentOperators"] = callNextForm.P03CopyLineMapping["CurrentOperators"].ToString();
                this.CurrentMaintain["StandardOutput"] = callNextForm.P03CopyLineMapping["StandardOutput"].ToString();
                this.CurrentMaintain["DailyDemand"] = callNextForm.P03CopyLineMapping["DailyDemand"].ToString();
                this.CurrentMaintain["Workhour"] = callNextForm.P03CopyLineMapping["Workhour"].ToString();
                this.CurrentMaintain["NetTime"] = callNextForm.P03CopyLineMapping["NetTime"].ToString();
                this.CurrentMaintain["TaktTime"] = callNextForm.P03CopyLineMapping["TaktTime"].ToString();
                this.CurrentMaintain["TotalGSD"] = callNextForm.P03CopyLineMapping["TotalGSD"].ToString();
                this.CurrentMaintain["TotalCycle"] = callNextForm.P03CopyLineMapping["TotalCycle"].ToString();
                this.CurrentMaintain["HighestGSD"] = callNextForm.P03CopyLineMapping["HighestGSD"].ToString();
                this.CurrentMaintain["HighestCycle"] = callNextForm.P03CopyLineMapping["HighestCycle"].ToString();
                this.CalculateValue(0);
                this.ComputeTaktTime();
            }

            this.Distable();
        }

        // Copy from GSD
        private void BtnCopyFromGSD_Click(object sender, EventArgs e)
        {
            // 刪除現有表身資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            DataRow timeStudy;
            DataTable timeStudy_Detail;
            string sqlCmd = string.Format(
                @"
select t.* 
from TimeStudy t WITH (NOLOCK) 
	 , Style s WITH (NOLOCK) 
where t.StyleID = s.ID 
	  and t.BrandID = s.BrandID 
	  and t.SeasonID = s.SeasonID 
	  and s.Ukey = {0}
	  and t.ComboType = '{1}'",
                this.CurrentMaintain["StyleUkey"].ToString(),
                this.CurrentMaintain["ComboType"].ToString());
            if (!MyUtility.Check.Seek(sqlCmd, out timeStudy))
            {
                MyUtility.Msg.WarningBox("Fty GSD data not found!!");
                return;
            }

            string ietmsUKEY = MyUtility.GetValue.Lookup($@" select i.Ukey from TimeStudy t WITH (NOLOCK) inner join IETMS i WITH (NOLOCK) on i.id = t.IETMSID and i.Version = t.IETMSVersion where t.id = '{timeStudy["ID"]}' ");
            sqlCmd = string.Format(
                @"
select distinct
	ID = null
	,No = ''
	,OriNo = '0'
	,Annotation = '**Cutting'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = ''
    ,Attachment = null
    ,Template = null
	,OperationID = 'PROCIPF00001'
	,MoldID = null
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Cutting'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,[IsHide] = cast(iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0)) as bit)
	,[IsGroupHeader] = 0
	,[IsSewingOperation] = cast(isnull(show.IsDesignatedArea, 0) as bit)
    ,[IsShow] = cast(isnull(show.IsShowinIEP03, 1) as bit)
from [IETMS_Summary] i
outer apply (
	select IsShowinIEP03 = atf.IsShowinIEP03
		, IsSewingline = atf.IsSewingline
		, IsDesignatedArea = m.IsDesignatedArea
	from ArtworkType_FTY atf WITH (NOLOCK)
	inner join ArtworkType at2 WITH (NOLOCK) on at2.id= atf.ArtworkTypeID
	inner join MachineType m WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	where i.ArtworkTypeID = atf.ArtworkTypeID
	and atf.FactoryID = '{2}'
)show
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Cutting'
union all
select ID = null
	   , No = ''
	   , OriNo = td.Seq
	   , td.Annotation
	   , GSD = td.SMV
	   , TotalGSD = td.SMV
	   , Cycle = td.SMV
	   , TotalCycle = td.SMV
	   , td.MachineTypeID
	   , [Attachment] = STUFF((
					select concat(',' ,s.Data)
					from SplitString(td.Mold, ',') s
					where not exists (select 1 from Mold m WITH (NOLOCK) where s.Data = m.ID and (m.Junk = 1 or m.IsTemplate = 1)) 
					for xml path ('')) 
				,1,1,'')
	    , [Template] = STUFF((
					select concat(',' ,s.Data)
					from SplitString(td.Template, ',') s
					where not exists (select 1 from Mold m WITH (NOLOCK) where s.Data = m.ID and (m.Junk = 1 or m.IsAttachment = 1)) 
					for xml path ('')) 
				,1,1,'')
	   , td.OperationID
	   , MoldID = td.Mold
	   , GroupKey = 0
	   , New = 0
	   , EmployeeID = ''
	   , Description = IIF(td.MachineTypeID IS NULL OR td.MachineTypeID = '' ,td.OperationID ,o.DescEN )
	   , EmployeeName = ''
	   , EmployeeSkill = ''
	   , Efficiency = 100
       , IsPPA = iif(CHARINDEX('--', td.OperationID) > 0, 0, iif(td.SMV > 0, 0, 1))
       ,o.MasterPlusGroup
	   ,[IsHide] = cast(iif(SUBSTRING(td.OperationID, 1, 2) = '--', 1, iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0))) as bit)
	   ,[IsGroupHeader] = cast(iif(SUBSTRING(td.OperationID, 1, 2) = '--', 1, 0) as bit)
	   ,[IsSewingOperation] = cast(isnull(show.IsDesignatedArea, 0) as bit)
       ,[IsShow] = cast(isnull(show.IsShowinIEP03, 1) as bit)
from TimeStudy_Detail td WITH (NOLOCK) 
left join Operation o WITH (NOLOCK) on td.OperationID = o.ID
outer apply (
	select IsShowinIEP03 = atf.IsShowinIEP03
		, IsSewingline = atf.IsSewingline
		, IsDesignatedArea = m.IsDesignatedArea
	from Operation o2 WITH (NOLOCK)
	inner join MachineType m WITH (NOLOCK) on o2.MachineTypeID = m.ID
	inner join ArtworkType at2 WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	inner join ArtworkType_FTY atf WITH (NOLOCK) on at2.id= atf.ArtworkTypeID and atf.FactoryID = '{2}'
	where o.ID = o2.ID
)show
where td.ID = '{1}'
union all
select distinct
	ID = null
	,No = ''
	,OriNo = '9970'
	,Annotation = '**Inspection'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = 'M'
    ,Attachment = null
    ,Template = null
	,OperationID = 'PROCIPF00002'
	,MoldID = null
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Inspection'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,[IsHide] = cast(iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0)) as bit)
	,[IsGroupHeader] = 0
	,[IsSewingOperation] = cast(isnull(show.IsDesignatedArea, 0) as bit)
    ,[IsShow] = cast(isnull(show.IsShowinIEP03, 1) as bit)
from [IETMS_Summary] i
outer apply (
	select IsShowinIEP03 = atf.IsShowinIEP03
		, IsSewingline = atf.IsSewingline
		, IsDesignatedArea = m.IsDesignatedArea
	from ArtworkType_FTY atf WITH (NOLOCK)
	inner join ArtworkType at2 WITH (NOLOCK) on at2.id= atf.ArtworkTypeID
	inner join MachineType m WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	where i.ArtworkTypeID = atf.ArtworkTypeID
	and atf.FactoryID = '{2}'
)show
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Inspection'
union all
select distinct
	ID = null
	,No = ''
	,OriNo = '9980'
	,Annotation = '**Pressing'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = 'MM2'
    ,Attachment = null
    ,Template = null
	,OperationID = 'PROCIPF00004'
	,MoldID = null
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Pressing'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,[IsHide] = cast(iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0)) as bit)
	,[IsGroupHeader] = 0
	,[IsSewingOperation] = cast(isnull(show.IsDesignatedArea, 0) as bit)
    ,[IsShow] = cast(isnull(show.IsShowinIEP03, 1) as bit)
from [IETMS_Summary] i
outer apply (
	select IsShowinIEP03 = atf.IsShowinIEP03
		, IsSewingline = atf.IsSewingline
		, IsDesignatedArea = m.IsDesignatedArea
	from ArtworkType_FTY atf WITH (NOLOCK)
	inner join ArtworkType at2 WITH (NOLOCK) on at2.id= atf.ArtworkTypeID
	inner join MachineType m WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	where i.ArtworkTypeID = atf.ArtworkTypeID
	and atf.FactoryID = '{2}'
)show
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Pressing'
union all
select distinct
	ID = null
	,No = ''
	,OriNo = '9990'
	,Annotation =  '**Packing'
	,GSD = round(ProTMS, 4)
	,TotalGSD = round(ProTMS, 4)
	,Cycle = round(ProTMS, 4)
	,TotalCycle = round(ProTMS, 4)
	,MachineTypeID = 'MM2'
    ,Attachment = null
    ,Template = null
	,OperationID = 'PROCIPF00003'
	,MoldID = null
	,GroupKey = 0
	,New = 0
	,EmployeeID = ''
	,Description = '**Packing'
	,EmployeeName = ''
	,EmployeeSkill = ''
	,Efficiency = 100
	,IsPPA = 0
    ,[MasterPlusGroup]=''
	,[IsHide] = cast(iif(show.IsDesignatedArea = 1, 1, iif(show.IsSewingline = 0, 1, 0)) as bit)
	,[IsGroupHeader] = 0
	,[IsSewingOperation] = cast(isnull(show.IsDesignatedArea, 0) as bit)
    ,[IsShow] = cast(isnull(show.IsShowinIEP03, 1) as bit)
from [IETMS_Summary] i
outer apply (
	select IsShowinIEP03 = atf.IsShowinIEP03
		, IsSewingline = atf.IsSewingline
		, IsDesignatedArea = m.IsDesignatedArea
	from ArtworkType_FTY atf WITH (NOLOCK)
	inner join ArtworkType at2 WITH (NOLOCK) on at2.id= atf.ArtworkTypeID
	inner join MachineType m WITH (NOLOCK) on m.ArtworkTypeID =at2.ID
	where i.ArtworkTypeID = atf.ArtworkTypeID
	and atf.FactoryID = '{2}'
)show
where i.location = '' and i.[IETMSUkey] = '{0}' and i.ArtworkTypeID = 'Packing'",
                ietmsUKEY,
                timeStudy["ID"],
                Env.User.Factory);

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out timeStudy_Detail);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query Fty GSD detail fail!!");
                return;
            }

            // 將要複製的資料寫入表身Grid
            int i = 0;
            foreach (DataRow dr in timeStudy_Detail.Rows)
            {
                dr["GroupKey"] = ++i;
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }

            object sumSMV = timeStudy_Detail.Compute("sum(GSD)", "(IsHide = 0 or  IsHide is null)");
            object maxSMV = timeStudy_Detail.Compute("max(GSD)", "(IsHide = 0 or  IsHide is null)");

            // 填入表頭資料
            this.CurrentMaintain["IdealOperators"] = timeStudy["NumberSewer"].ToString();
            this.CurrentMaintain["CurrentOperators"] = i;
            this.CurrentMaintain["StandardOutput"] = MyUtility.Convert.GetDecimal(sumSMV) == 0 ? 0 : MyUtility.Math.Round(3600 * i / MyUtility.Convert.GetDecimal(sumSMV));
            this.CurrentMaintain["DailyDemand"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["StandardOutput"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["Workhour"]), 0);
            this.CurrentMaintain["TaktTime"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]) == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["NetTime"]) / MyUtility.Convert.GetDecimal(this.CurrentMaintain["DailyDemand"]), 0);
            this.CurrentMaintain["TotalGSD"] = sumSMV;
            this.CurrentMaintain["TotalCycle"] = sumSMV;
            this.CurrentMaintain["HighestGSD"] = maxSMV;
            this.CurrentMaintain["HighestCycle"] = maxSMV;
            this.CalculateValue(0);
            this.ComputeTaktTime();
            this.Distable();
        }

        private void Distable()
        {
            if (this.listControlBindingSource1.DataSource != null)
            {
                this.listControlBindingSource1.DataSource = null;
            }

            if (this.grid1.DataSource != null)
            {
                this.grid1.DataSource = null;
            }

            this.IsShowTable();

            DataRow[] drs = ((DataTable)this.detailgridbs.DataSource).Select("No <> '' and IsShow = 1");
            if (drs.Length == 0)
            {
                return;
            }

            this.listControlBindingSource1.DataSource = drs
                .Select(x => new
                {
                    No = x.Field<string>("No"),
                    ActCycle = x.Field<decimal?>("ActCycle"),
                    TotalGSD = x.Field<decimal?>("TotalGSD"),
                    TotalCycle = x.Field<decimal?>("TotalCycle"),
                    SortA = x.Field<string>("No").Substring(0, 1),
                    SortB = x.Field<string>("No").Substring(1, x.Field<string>("No").Length - 1),
                })
                .Distinct()
                .OrderByDescending(x => x.SortA)
                .ThenBy(x => x.SortB)
                .ToList();
            this.grid1.DataSource = this.listControlBindingSource1;
        }

        private void IsShowTable()
        {
            for (int i = 0; i < this.detailgrid.Rows.Count; i++)
            {
                DataRow row = this.detailgrid.GetDataRow(i);
                if (!MyUtility.Convert.GetBool(row["IsShow"]))
                {
                    CurrencyManager currencyManager = (CurrencyManager)this.BindingContext[this.detailgrid.DataSource];
                    currencyManager.SuspendBinding();
                    this.detailgrid.Rows[i].Visible = false;
                    currencyManager.ResumeBinding();
                }
            }
        }
    }
}
