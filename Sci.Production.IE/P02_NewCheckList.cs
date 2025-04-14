using System;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Windows.Forms;

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

        /// <inheritdoc/>
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
            ,CO.FactoryID
            FROM ChgOver_Check CC WITH(NOLOCK)
            INNER JOIN ChgOver CO WITH(NOLOCK) ON CO.ID  = CC.ID
            LEFT JOIN ChgOverCheckListBase Cb WITH(NOLOCK) ON CB.[ID] = CC.[No]
            OUTER APPLY
            (
	            SELECT val = isnull(iif((CC.Deadline IS NULL), 0, DATEDIFF(day,GETDATE(),CC.DeadLine) - (COUNT(1) + dbo.getDateRangeSundayCount(GETDATE(),cc.Deadline))),0)
	            FROM Holiday WITH(NOLOCK)
	            WHERE HolidayDate BETWEEN GETDATE() AND CC.Deadline AND FactoryID = CO.FactoryID
            )DaysLefCnt
            OUTER APPLY
            (
	            SELECT val = isnull(iif((CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,GETDATE()) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE()))),0)
	            FROM Holiday WITH(NOLOCK)
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )OverDay_Check_0
            OUTER APPLY
            (
	            SELECT val = isnull(iif((CC.CompletionDate IS NULL) OR (CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,CC.CompletionDate))),0)
	            FROM Holiday WITH(NOLOCK)
	            WHERE HolidayDate BETWEEN CC.Deadline AND CC.CompletionDate AND FactoryID = CO.FactoryID
            )OverDay_Check_1
            WHERE CC.id = {this.KeyValue1} AND CC.[NO] <> 0
            order by  CB.No";
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

        /// <summary>
        /// OnGridSetup
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings rk = new DataGridViewGeneratorTextColumnSettings();

            // DataGridViewGeneratorCheckBoxColumnSettings
            DataGridViewGeneratorCheckBoxColumnSettings cbs = new DataGridViewGeneratorCheckBoxColumnSettings();
            cbs.HeaderAction = DataGridViewGeneratorCheckBoxHeaderAction.None;

            // 設置CellValidating事件來處理CheckBox變動
            cbs.CellValidating += (s, e) =>
            {
                DataRowView drv = this.grid.SelectedRows[0].DataBoundItem as DataRowView;
                int index = this.chgOverChkList.Rows.IndexOf(drv.Row);

                DataRow row = this.chgOverChkList.Rows[index];
                var oridr = this.copyDt.AsEnumerable().Where(x => x.Field<int>("No") == MyUtility.Convert.GetInt(row["No"])).FirstOrDefault();

                if (!MyUtility.Convert.GetBool(oridr["Checked"]))
                {
                    if ((bool)e.FormattedValue)
                    {
                        string sqlcmd = $@"
                        DECLARE @CompletionDate Date = '{((DateTime)row["Deadline"]).ToString("yyyy/MM/dd")}'
                        DECLARE @Holiday int;
                        SELECT @Holiday = isnull(DATEDIFF(day,@CompletionDate,GETDATE()) - (COUNT(1) + dbo.getDateRangeSundayCount(@CompletionDate,GETDATE())),0)
                        FROM Holiday WITH(NOLOCK)
                        WHERE HolidayDate BETWEEN @CompletionDate and GETDATE() AND FactoryID = '{row["FactoryID"]}'
                        SELECT VAL = IIF(@Holiday <= 0, 0 , @Holiday)
                        ";
                        var Holiday = MyUtility.GetValue.Lookup(sqlcmd);
                        row["Checked"] = true;
                        row["DaysLeft"] = '-';
                        row["OverDays"] = MyUtility.Convert.GetInt(Holiday);
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

                this.gridbs.EndEdit();
            };

            // 設置Grid欄位顯示
            this.Helper.Controls.Grid.Generator(this.grid)
                .Numeric("No", header: "No", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("CHECKLISTS", header: "CHECKLISTS", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dep", header: "Dep", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("LeadTime", header: "Lead Time", iseditingreadonly: true)
                .Text("DaysLeft", header: "Days Left", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("Deadline", header: "Deadline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CheckBox("Checked", header: "Check", width: Widths.AnsiChars(6), trueValue: 1, falseValue: 0, settings: cbs)
                .Date("CompletionDate", header: "Completion Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OverDays", header: "Over Days", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("Remark", header: "Late Reason", width: Widths.AnsiChars(15))
                .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(15), iseditingreadonly: true);

            this.grid.Columns["Checked"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            return true;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {

            string strErrorMes = string.Empty;
            this.gridbs.EndEdit();
            foreach (DataRow dr in this.chgOverChkList.Rows)
            {
                if (MyUtility.Convert.GetBool(dr["Checked"]) &&
                    MyUtility.Convert.GetInt(dr["OverDays"]) > 0 &&
                    MyUtility.Check.Empty(dr["Remark"]))
                {
                    strErrorMes += $@"Please fill in [Late Reason] since NO.<{dr["No"]}> already passed the Deadline." + Environment.NewLine;
                }

                string da = MyUtility.Convert.GetString(dr["Remark"]);
                if (da.Length >= 60)
                 {
                    MyUtility.Msg.WarningBox("Input exceeds the 60 character limit. Please shorten your input.");
                    return false;
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
