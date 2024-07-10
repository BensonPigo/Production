using System;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P02_NewCheckList
    /// </summary>
    public partial class P02_NewCheckList : Win.Subs.Input4
    {
        private DataTable copyDt;
        private DataTable chgOverChkList;

        /// <summary>
        /// P02_NewCheckList
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        /// <param name="changeoverType">changeoverType</param>
        public P02_NewCheckList(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, string changeoverType)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
            this.Text = "Check List";
        }

        /// <summary>
        /// OnGridSetup
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnGridSetup()
        {
            // DataGridViewGeneratorTextColumnSettings
            DataGridViewGeneratorCheckBoxColumnSettings cbs = new DataGridViewGeneratorCheckBoxColumnSettings();

            cbs.CellValidating += (s, e) =>
            {
                DataRow row = this.chgOverChkList.Rows[e.RowIndex];
                var oridr = this.copyDt.AsEnumerable().Where(x => x.Field<int>("No") == MyUtility.Convert.GetInt(row["No"])).FirstOrDefault();

                if (! MyUtility.Convert.GetBool(oridr["Checked"]))
                {
                    if ((bool)e.FormattedValue)
                    {
                        row["Checked"] = true;
                        row["DaysLeft"] = '-';
                        row["OverDays"] = row["OverDay_Check_1"];
                        row["CompletionDate"] = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        row["Checked"] = false;
                        row["DaysLeft"] = row["DaysLeft1"];
                        row["OverDays"] = row["OverDay_Check_0"];
                        row["CompletionDate"] = DBNull.Value;
                    }
                }
                else
                {
                    row["Checked"] = true;
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Numeric("No", header: "No", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CHECKLISTS", header: "CHECKLISTS", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dep", header: "Dep", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("LeadTime", header: "Lead Time", iseditingreadonly: true)
                .Text("DaysLeft", header: "Days Left", iseditingreadonly: true)
                .Date("Deadline", header: "Deadline", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .CheckBox("Checked", header: "Check", width: Widths.AnsiChars(15), trueValue: 1, falseValue: 0, settings: cbs)
                .Date("CompletionDate", header: "Completion Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OverDays", header: "Over Days", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Remark", header: "Late Reason", width: Widths.AnsiChars(30))
                .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.grid.Columns["Checked"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        /// <summary>
        /// OnRequery
        /// </summary>
        /// <returns>bool</returns>
        protected override DualResult OnRequery()
        {
            string selectCommand = $@"
            SELECT 
            [ID] = CC.ID
            ,[ChgOverCheckListID] = CC.ChgOverCheckListID
            ,[No] = CB.No
            ,[CHECKLISTS]  = CB.CheckList
            ,[Dep] = CC.ResponseDep
            ,[LeadTime] = CC.LeadTime
            ,[DaysLeft] = iif(CC.[Checked] = 1 ,'-' ,  CONVERT( VARCHAR(10),iif(DaysLefCnt.val < 0 , 0 ,DaysLefCnt.val )))
            ,[Deadline] = CC.Deadline
            ,CC.[Checked]
            ,[CompletionDate] = CC.CompletionDate
            ,[OverDays] = iif(CC.[Checked] = 0 , iif(OverDay_Check_0.VAL < 0,0,OverDay_Check_0.VAL) ,iif(OverDay_Check_1.VAL < 0,0,OverDay_Check_1.VAL))
            ,CC.Remark
            ,[EditName] = CC.EditName
            ,[EditDate] = CC.EditDate
            ,[OverDay_Check_0] = iif(OverDay_Check_0.VAL < 0,0, OverDay_Check_0.VAL)
            ,[OverDay_Check_1] = iif(OverDay_Check_1.VAL < 0,0, OverDay_Check_1.VAL)
            ,[DaysLeft1] = iif(DaysLefCnt.val < 0 , 0 ,isnull(DaysLefCnt.val,0))
            FROM ChgOver_Check CC WITH(NOLOCK)
            INNER JOIN ChgOver CO WITH(NOLOCK) ON CO.ID  = CC.ID
            LEFT JOIN ChgOverCheckListBase Cb WITH(NOLOCK) ON CB.[NO] = CC.[NO]
            OUTER APPLY
            (
	            SELECT val = isnull(DATEDIFF(day,GETDATE(),CC.DeadLine) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE())),0)
	            FROM Holiday WITH(NOLOCK)
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )DaysLefCnt
            OUTER APPLY
            (
	            SELECT val = isnull(DATEDIFF(day,CC.DeadLine,GETDATE()) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE())),0)
	            FROM Holiday WITH(NOLOCK)
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )OverDay_Check_0
            OUTER APPLY
            (
	            SELECT val = isnull(iif(CC.CompletionDate IS NULL, 0, DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,CC.CompletionDate))),0)
	            FROM Holiday WITH(NOLOCK)
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )OverDay_Check_1
            WHERE CC.id = {this.KeyValue1}
            order by cc.ChgOverCheckListID";
            DualResult returnResult;
            returnResult = DBProxy.Current.Select(null, selectCommand, out this.chgOverChkList);
            if (!returnResult)
            {
                return returnResult;
            }

            this.gridbs.DataSource = this.chgOverChkList;

            this.copyDt = this.chgOverChkList.Copy();
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();
            string strErrorMes = string.Empty;

            foreach (DataRow dr in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (MyUtility.Convert.GetBool(dr["Checked"]) &&
                    MyUtility.Convert.GetInt(dr["OverDays"]) > 0 &&
                    MyUtility.Check.Empty(dr["Remark"]))
                {
                    strErrorMes += $@"Please fill in [Late Reason] since NO.<{dr["No"]}> already passed the Deadline." + Environment.NewLine;
                }
            }

            if (!MyUtility.Check.Empty(strErrorMes))
            {
                MyUtility.Msg.WarningBox(strErrorMes);
                return false;
            }

            return base.OnSaveBefore();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }

    }
}
