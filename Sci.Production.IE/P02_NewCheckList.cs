using System;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P02_NewCheckList
    /// </summary>
    public partial class P02_NewCheckList : Win.Subs.Input4
    {
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

            if (changeoverType == "R")
            {
                this.Text = "Check List - Repeat";
            }
            else
            {
                this.Text = "Check List - New";
            }
        }

        /// <summary>
        /// OnGridSetup
        /// </summary>
        /// <returns>bool</returns>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Numeric("No", header: "No", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CHECKLISTS", header: "CHECKLISTS", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Dep", header: "Dep", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Date("LeadTime", header: "Lead Time")
                .Date("DaysLeft", header: "Days Left")
                .Text("Deadline", header: "Deadline", width: Widths.AnsiChars(30))
                .Text("Check", header: "Check", width: Widths.AnsiChars(30))
                .Text("CompletionDate", header: "Completion Date", width: Widths.AnsiChars(30))
                .Text("OverDays", header: "Over Days", width: Widths.AnsiChars(30))
                .Text("LateReason", header: "Late Reason", width: Widths.AnsiChars(30))
                .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(30))
                .Text("EditDate", header: "Edit Date", width: Widths.AnsiChars(30));

            this.grid.Columns["ScheduleDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid.Columns["ActualDate"].DefaultCellStyle.BackColor = Color.Pink;
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
            [No] = CB.No
            ,[CHECKLISTS]  = CB.CheckList
            ,[Dep] = CKD.ResponseDep
            ,[LeadTime] = CKD.LeadTime
            ,[DaysLeft] = ''
            ,[Deadline] = CC.Deadline
            ,[Check] = CC.[Check]
            ,[CompletionDate] = CC.ActualDate
            ,[OverDays] = iif(CC.[Check] = 0 , OverDay_Check_0.VAL,OverDay_Check_1.VAL)
            ,[LateReason] = CC.Remark
            ,[EditName] = CC.EditName
            ,[EditDate] = CC.EditDate
            FROM ChgOver_Check CC
            LEFT JOIN ChgOverCheckList CK WITH(NOLOCK) ON CC.ChgOverCheckListID = CK.ID
            LEFT join ChgOverCheckList_Detail CKD WITH(NOLOCK) ON  CKD.ID = CK.ID
            LEFT JOIN ChgOverCheckListBase Cb WITH(NOLOCK) ON CB.ID = CKD.ChgOverCheckListBaseID
            OUTER APPLY
            (
	            SELECT VAL = COUNT(1)
	            FROM Holiday 
	            WHERE FactoryID = 'FAC' and
                HolidayDate >= GETDATE() and 
                HolidayDate <= CC.Deadline
            )OverDay_Check_0
            OUTER APPLY
            (
	            SELECT VAL = COUNT(1)
	            FROM Holiday 
	            WHERE FactoryID = 'FAC' 
	            and HolidayDate >= CC.ActualDate 
	            and HolidayDate <= CC.Deadline
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

            this.SetGrid(chgOverChkList);
            return Ict.Result.True;
        }

        /// <summary>
        /// Save -- Append/Revise/Delete按鈕要隱藏
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }
    }
}
