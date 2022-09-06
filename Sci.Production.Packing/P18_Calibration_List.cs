using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    public partial class P18_Calibration_List : Sci.Win.Subs.Input4
    {
        public P18_Calibration_List(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
             : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();

            this.comboMDMachineID.DataSource = new List<string>();
        }

        protected override DualResult OnRequery()
        {
            if (this.comboMDMachineID.SelectedValue == null)
            {
                return Ict.Result.True;
            }

            string selectCommand = $@"
select 
[Selected] = 0
,CalibrationTime
,Point1
,Point2
,Point3
,Point4
,Point5
,Point6
,Point7
,Point8
,Point9
,ID
,CalibrationDate
,Operator
,MachineID
from MDCalibrationList
where 1=1
and MachineID = '{this.comboMDMachineID.Text}'
and CalibrationDate = CONVERT(date,getdate())


";
            DualResult returnResult = DBProxy.Current.Select(null, selectCommand, out DataTable dtDtail);
            if (!returnResult)
            {
                return returnResult;
            }

            this.SetGrid(dtDtail);
            return Ict.Result.True;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // combo Data Source設定
            DataTable dtMDCalibrationList;
            Ict.DualResult result;
            if (result = DBProxy.Current.Select(null, @"
select row = 0, MachineID = '',operator = ''
union all
select row = ROW_NUMBER () over(order by MachineID), MachineID , operator
from MDMachineBasic
Where Junk = 0
", out dtMDCalibrationList))
            {
                this.comboMDMachineID.DataSource = dtMDCalibrationList;
                this.comboMDMachineID.DisplayMember = "MachineID";
                this.comboMDMachineID.ValueMember = "MachineID";
            }
            else
            {
                this.ShowErr(result);
            }

            // 根據userid來自動帶出MachineID
            DataRow[] drUserRowNB = dtMDCalibrationList.Select($@" operator = '{Env.User.UserID}'");
            if (drUserRowNB.Length > 0)
            {
                int row = MyUtility.Convert.GetInt(drUserRowNB[0]["row"]);
                this.comboMDMachineID.SelectedIndex = row;
            }
            else
            {
                this.comboMDMachineID.SelectedIndex = 0;
            }

            this.EditMode = CanEdit;
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings mask_CalibrationTime = new DataGridViewGeneratorMaskedTextColumnSettings();
            mask_CalibrationTime.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);

                string strTime = e.FormattedValue.ToString().Substring(0, 2) + ":" + e.FormattedValue.ToString().Substring(2, 2);//e.FormattedValue.ToString();
                if (this.IsDateTimeFormat(strTime))
                {
                    dr["CalibrationTime"] = strTime;
                    dr.EndEdit();
                }
                else
                {
                    e.Cancel = true;
                }
            };
            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .MaskedText("CalibrationTime", "00:00", header: "Calibration Time", width: Widths.AnsiChars(8), iseditingreadonly: false, settings: mask_CalibrationTime)
               .CheckBox("Point1", header: "1", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point2", header: "2", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point3", header: "3", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point4", header: "4", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point5", header: "5", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point6", header: "6", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point7", header: "7", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point8", header: "8", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .CheckBox("Point9", header: "9", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               ;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["CalibrationDate"] = DateTime.Now;
            selectDr["Operator"] = Env.User.UserID;
        }

        private bool IsDateTimeFormat(string value)
        {
            try
            {
                DateTime.Parse(value);
                return true;
            }
            catch
            {
                MyUtility.Msg.WarningBox("Date format error");
                return false;
            }
        }

        private void comboMDMachineID_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboMDMachineID.SelectedValue == null)
            {
                return;
            }

            this.OnRequery();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.gridbs == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.gridbs.DataSource;

            if (dt.Rows.Count <= 0)
            {
                return;
            }

            var upd_list = dt.AsEnumerable().Where(x => x["Selected"].EqualDecimal(1)).ToList();
            if (upd_list.Count == 0)
            {
                return;
            }

            string sqlMerge = $@"
merge MDCalibrationList as t
using #tmp as s
on t.MachineID = s.MachineID and t.CalibrationDate = convert(date,getDate())
when matched then 
update set 
	t.CalibrationTime  = s.CalibrationTime
	,t.[Point1]     = iif(s.[Point1] = 'True' , 1,0)
    ,t.[Point2]		= iif(s.[Point2]= 'True' , 1,0)
    ,t.[Point3]		= iif(s.[Point3]= 'True' , 1,0)
    ,t.[Point4]		= iif(s.[Point4]= 'True' , 1,0)
    ,t.[Point5]		= iif(s.[Point5]= 'True' , 1,0)
    ,t.[Point6]		= iif(s.[Point6]= 'True' , 1,0)
    ,t.[Point7]		= iif(s.[Point7]= 'True' , 1,0)
    ,t.[Point8]		= iif(s.[Point8]= 'True' , 1,0)
    ,t.[Point9]		= iif(s.[Point9]= 'True' , 1,0)
    ,t.[SubmitDate] = getdate()
    ,t.Operator     = '{Env.User.UserID}'
when not matched by target then
insert([MachineID]
      ,[CalibrationTime]
      ,[Point1]
      ,[Point2]
      ,[Point3]
      ,[Point4]
      ,[Point5]
      ,[Point6]
      ,[Point7]
      ,[Point8]
      ,[Point9]
      ,[CalibrationDate]
      ,[SubmitDate]
      ,[Operator])
values(s.[MachineID]
      ,s.[CalibrationTime]
      ,s.[Point1]
      ,s.[Point2]
      ,s.[Point3]
      ,s.[Point4]
      ,s.[Point5]
      ,s.[Point6]
      ,s.[Point7]
      ,s.[Point8]
      ,s.[Point9]
      ,convert(date,getdate())
      ,getdate()
      ,'{Env.User.UserID}'
);
";

            string sqlchk = "select 1 from #tmp";
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlchk, out DataTable dtresult)))
            {
                this.ShowErr(result);
                return;
            }
        }
    }
}
