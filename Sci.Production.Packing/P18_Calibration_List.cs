using Ict;
using Ict.Win;
using org.apache.pdfbox.io;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Sci.Production.Packing
{
    public partial class P18_Calibration_List : Sci.Win.Subs.Input4
    {
        public static string MachineID;

        public P18_Calibration_List(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
             : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();

            this.comboMDMachineID.DataSource = new List<string>();
        }

        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.revise.Visible = false;
            this.undo.Visible = false;
            this.save.Text = "Submit";
        }

        protected override DualResult OnRequery()
        {
            if (this.comboMDMachineID.SelectedValue == null)
            {
                return Ict.Result.True;
            }

            string selectCommand = $@"
select 
SelectAll = case when 
(
Point1 = 1 and 
Point2 = 1 and 
Point3 = 1 and 
Point4 = 1 and 
Point5 = 1 and 
Point6 = 1 and 
Point7 = 1 and 
Point8 = 1 and 
Point9 = 1
) then cast(1 as bit) else cast(0 as bit) end
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
,SubmitDate
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
                MachineID = this.comboMDMachineID.Text;
            }
            else
            {
                this.comboMDMachineID.SelectedIndex = 0;
            }
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings mask_CalibrationTime = new DataGridViewGeneratorMaskedTextColumnSettings();
            mask_CalibrationTime.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1 || this.EditMode == false)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                if (e.FormattedValue.ToString().Trim().Length < 4)
                {
                    MyUtility.Msg.WarningBox("[Calibration Time] length should be 4 !");
                    e.Cancel = true;
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                string strTime = string.Empty;
                if (e.FormattedValue.ToString().Contains(":"))
                {
                    strTime = e.FormattedValue.ToString().Substring(0, 2) + ":" + e.FormattedValue.ToString().Substring(3, 2);
                }
                else
                {
                    strTime = e.FormattedValue.ToString().Substring(0, 2) + ":" + e.FormattedValue.ToString().Substring(2, 2);
                }

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

            Ict.Win.DataGridViewGeneratorCheckBoxColumnSettings col_allselect = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_allselect.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1 || this.EditMode == false)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["Point1"] = 1;
                    dr["Point2"] = 1;
                    dr["Point3"] = 1;
                    dr["Point4"] = 1;
                    dr["Point5"] = 1;
                    dr["Point6"] = 1;
                    dr["Point7"] = 1;
                    dr["Point8"] = 1;
                    dr["Point9"] = 1;
                    dr["SelectAll"] = 1;
                }
                else
                {
                    dr["Point1"] = 0;
                    dr["Point2"] = 0;
                    dr["Point3"] = 0;
                    dr["Point4"] = 0;
                    dr["Point5"] = 0;
                    dr["Point6"] = 0;
                    dr["Point7"] = 0;
                    dr["Point8"] = 0;
                    dr["Point9"] = 0;
                    dr["SelectAll"] = 0;
                }

                dr.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("SelectAll", header: string.Empty, width: Widths.AnsiChars(4), iseditable: true, trueValue: true, falseValue: false, settings: col_allselect)
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

            selectDr["MachineID"] = this.comboMDMachineID.Text;
            selectDr["CalibrationDate"] = DateTime.Now;
            selectDr["SubmitDate"] = DateTime.Now;
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

            MachineID = this.comboMDMachineID.Text;
            this.OnRequery();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnSaveAfter()
        {
            base.OnSaveAfter();
            //if (this.save.Text.ToUpper() == "SAVE")
            if (this.save.Text.ToUpper() == "SUBMIT")
            {
                string selectCommand = $@"
select top 2
[Date] =　SUBSTRING(CONVERT(varchar, CalibrationDate)　+' '+　convert(varchar,CalibrationTime),0,17)
from MDCalibrationList
where 1=1
and MachineID = '{this.comboMDMachineID.Text}'
and CalibrationDate = CONVERT(date,getdate())
order by CalibrationTime desc
";
                DualResult returnResult = DBProxy.Current.Select(null, selectCommand, out DataTable dtDtail);
                if (!returnResult)
                {
                    this.ShowErr(returnResult);
                }

                if (dtDtail.Rows.Count == 2)
                {
                    P18_Calibration_History callForm = new P18_Calibration_History(dtDtail.Rows[0]["Date"].ToString(), dtDtail.Rows[1]["Date"].ToString());
                    callForm.ShowDialog(this);
                }

                P18.timer.Start();
            }
        }
    }
}
