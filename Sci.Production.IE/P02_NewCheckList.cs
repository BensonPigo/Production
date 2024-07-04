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
        DataTable copyDt;
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

            if (changeoverType == "R")
            {
                this.Text = "Check List";
            }
            else
            {
                this.Text = "Check List";
            }
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
                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                var oridr = this.copyDt.AsEnumerable().Where(x => x.Field<string>("No") == MyUtility.Convert.GetString(dr["No"])).FirstOrDefault();

                if (MyUtility.Convert.GetBool(e.FormattedValue) != MyUtility.Convert.GetBool(oridr["Checked"]))
                {
                    if ((bool)e.FormattedValue)
                    {
                        dr["DaysLeft"] = '-';
                        dr["OverDays"] = dr["OverDay_Check_1"];
                    }
                    else
                    {
                        dr["DaysLeft"] = dr["DaysLeft1"];
                        dr["OverDays"] = dr["OverDay_Check_0"];
                    }
                }
            };

            this.Helper.Controls.Grid.Generator(this.grid)
                .Numeric("No", header: "No", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CHECKLISTS", header: "CHECKLISTS", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dep", header: "Dep", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("LeadTime", header: "Lead Time", iseditingreadonly: true)
                .Numeric("DaysLeft", header: "Days Left", iseditingreadonly: true)
                .Date("Deadline", header: "Deadline", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .CheckBox("Checked", header: "Check", width: Widths.AnsiChars(15), trueValue: 1, falseValue: 0)
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
            ,[Dep] = CKD.ResponseDep
            ,[LeadTime] = CKD.LeadTime
            ,[DaysLeft] = iif(CC.[Checked] = 1 ,'-' ,  iif(DaysLefCnt.val < 0 , 0 ,DaysLefCnt.val ))
            ,[Deadline] = CC.Deadline
            ,CC.[Checked]
            ,[CompletionDate] = CC.CompletionDate
            ,[OverDays] = iif(CC.[Checked] = 0 , OverDay_Check_0.VAL,OverDay_Check_1.VAL)
            ,CC.Remark
            ,[EditName] = CC.EditName
            ,[EditDate] = CC.EditDate
            ,[OverDay_Check_0] = OverDay_Check_0.val
            ,[OverDay_Check_1] = OverDay_Check_1.val
            ,[DaysLeft1] = iif(DaysLefCnt.val < 0 , 0 ,DaysLefCnt.val )
            FROM ChgOver_Check CC
            INNER JOIN ChgOver CO WITH(NOLOCK) ON CO.ID  = CC.ID
            LEFT JOIN ChgOverCheckList CK WITH(NOLOCK) ON CC.ChgOverCheckListID = CK.ID
            LEFT join ChgOverCheckList_Detail CKD WITH(NOLOCK) ON  CKD.ID = CK.ID
            INNER JOIN ChgOverCheckListBase Cb WITH(NOLOCK) ON CB.ID = CKD.ChgOverCheckListBaseID AND CB.[NO] = CC.[NO]
            OUTER APPLY
            (
	            SELECT val = DATEDIFF(day,GETDATE(),CC.DeadLine) -( COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE()))
	            FROM Holiday
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )DaysLefCnt
            OUTER APPLY
            (
	            SELECT val = DATEDIFF(day,CC.DeadLine,GETDATE()) -( COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE()))
	            FROM Holiday
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )OverDay_Check_0
            OUTER APPLY
            (
		            SELECT val = DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -( COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE()))
	            FROM Holiday
	            WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
            )OverDay_Check_1
            WHERE CC.id = {this.KeyValue1}
            order by cc.ChgOverCheckListID";
            DualResult returnResult;
            DataTable chgOverChkList = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out chgOverChkList);
            if (!returnResult)
            {
                return returnResult;
            }

            this.gridbs.DataSource = chgOverChkList;

            this.copyDt = chgOverChkList.Copy();
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();
            string strErrorMes = string.Empty;

            foreach (DataRow dr in this.Datas)
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
    }
}
