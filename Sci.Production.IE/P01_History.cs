﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_History
    /// </summary>
    public partial class P01_History : Win.Subs.Base
    {
        private DataTable gridData;
        private IList<string> comboBox1_RowSource = new List<string>();
        private DataRow masterData;

        /// <summary>
        /// P01_History
        /// </summary>
        /// <param name="masterData">MasterData</param>
        public P01_History(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.Text = "History -- " + this.masterData["StyleID"].ToString();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.displayStyle.Value = this.masterData["StyleID"].ToString();
            this.displaySeason.Value = this.masterData["SeasonID"].ToString();
            this.displayStyle1.Value = this.masterData["ComboType"].ToString();
            this.numTotalSewingTimePc.Value = MyUtility.Convert.GetInt(this.masterData["TotalSewingTime"]);
            this.numNumOfSewer.Value = MyUtility.Convert.GetInt(this.masterData["NumberSewer"]);

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@styleid";
            sp1.Value = this.masterData["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = this.masterData["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = this.masterData["BrandID"].ToString();
            sp4.ParameterName = "@combotype";
            sp4.Value = this.masterData["ComboType"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);

            // 撈CD Code
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
                this.displayCD.Value = string.Empty;
                MyUtility.Msg.ErrorBox("Query data fail!\r\n" + result.ToString());
            }
            else
            {
                this.displayCD.Value = dt.Rows.Count > 0 ? dt.Rows[0]["CdCodeID"].ToString() : string.Empty;
                this.displayCDCodeNew.Value = dt.Rows.Count > 0 ? dt.Rows[0]["CDCodeNew"].ToString() : string.Empty;
                this.displayProductType.Value = dt.Rows.Count > 0 ? dt.Rows[0]["ProductType"].ToString() : string.Empty;
                this.displayFabricType.Value = dt.Rows.Count > 0 ? dt.Rows[0]["FabricType"].ToString() : string.Empty;
                this.displayLining.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Lining"].ToString() : string.Empty;
                this.displayGender.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Gender"].ToString() : string.Empty;
                this.displayConstruction.Value = dt.Rows.Count > 0 ? dt.Rows[0]["Construction"].ToString() : string.Empty;
            }

            // 撈Grid資料
            sqlCmd = @"select th.ID,th.TotalSewingTime,th.NumberSewer,thd.Seq,thd.OperationID,o.DescEN,
thd.Annotation,thd.Frequency,thd.MtlFactorID,thd.SMV,thd.MachineTypeID,thd.Mold,
thd.PcsPerHour,thd.Sewer,thd.IETMSSMV,
LEFT(th.Phase+REPLICATE(' ',10),10)+' VER-'+th.Version as Status,
IIF(Phase = 'Initial',1,iif(Phase = 'Prelim',2,iif(Phase = 'Estimate',3,4))) as sort ,
thd.IsSubprocess,
thd.IsNonSewingLine,
thd.PPA,
o.MasterPlusGroup,
thd.SewingMachineAttachmentID,
thd.Template,
thd.StdSMV,
thd.Thread_ComboID
from TimeStudyHistory th WITH (NOLOCK) 
left join TimeStudyHistory_Detail thd WITH (NOLOCK) on th.ID = thd.ID
left join Operation o WITH (NOLOCK) on thd.OperationID = o.ID
where StyleID = @styleid and SeasonID = @seasonid and BrandID = @brandid and ComboType = @combotype
order by IIF(Phase = 'Initial',1,iif(Phase = 'Prelim',2,iif(Phase = 'Estimate',3,4))),th.Version,thd.Seq";
            result = DBProxy.Current.Select(null, sqlCmd, cmds, out this.gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!\r\n" + result.ToString());
            }
            else
            {
                foreach (DataRow dr in this.gridData.Rows)
                {
                    if (this.comboBox1_RowSource.Contains(dr["Status"].ToString()) == false)
                    {
                        this.comboBox1_RowSource.Add(dr["Status"].ToString());
                    }
                }

                this.comboStatus.DataSource = this.comboBox1_RowSource;
            }

            this.listControlBindingSource1.DataSource = this.gridData;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("OperationID", header: "Operation code", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .EditText("DescEN", header: "Operation Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("Frequency", header: "Frequency", decimal_places: 2, iseditingreadonly: true)
                .Text("MtlFactorID", header: "Factor", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("StdSMV", header: "Std. SMV (sec)", integer_places: 4, decimal_places: 4, iseditingreadonly: true)
                .Numeric("SMV", header: "SMV (sec)", decimal_places: 4, iseditingreadonly: true)
                .CheckBox("IsSubprocess", header: "Subprocess", width: Widths.AnsiChars(7), iseditable: true)
                .CheckBox("IsNonSewingLine", header: "Non-Sewing line", width: Widths.AnsiChars(7), iseditable: true)
                .Text("PPAText", header: "PPA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("MachineTypeID", header: "M/C", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("MasterPlusGroup", header: "Machine Group", width: Widths.AnsiChars(8))
                .Text("Mold", header: "Attachment", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SewingMachineAttachmentID", header: "Part ID", width: Widths.AnsiChars(50), iseditingreadonly: true)
                .Text("Template", header: "Template", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Thread_ComboID", "Thread Combination", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PcsPerHour", header: "Pcs/hr", decimal_places: 1, iseditingreadonly: true)
                .Numeric("Sewer", header: "Sewer", decimal_places: 1, iseditingreadonly: true)
                .Numeric("IETMSSMV", header: "Std. SMV", decimal_places: 4, iseditingreadonly: true);
            this.gridDetail.AutoResizeColumns();
        }

        // Status
        private void ComboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboStatus.SelectedIndex != -1)
            {
                this.gridData.DefaultView.RowFilter = "Status = '" + this.comboStatus.SelectedValue.ToString() + "'";

                this.numTotalSewingTimePc.Value = MyUtility.Convert.GetInt(this.gridData.DefaultView[0]["TotalSewingTime"]);
                this.numNumOfSewer.Value = MyUtility.Convert.GetInt(this.gridData.DefaultView[0]["NumberSewer"]);
            }
            else
            {
                this.gridData.DefaultView.RowFilter = "Status = ''";
                this.numTotalSewingTimePc.Value = 0;
                this.numNumOfSewer.Value = 0;
            }
        }

        // Summary
        private void BtnSummary_Click(object sender, EventArgs e)
        {
            P01_ArtworkSummary callNextForm;
            if (this.gridData.DefaultView.Count > 0)
            {
                callNextForm = new P01_ArtworkSummary("TimeStudyHistory_Detail", MyUtility.Convert.GetLong(this.gridData.DefaultView[0]["ID"]));
            }
            else
            {
                callNextForm = new P01_ArtworkSummary("TimeStudyHistory_Detail", 0);
            }

            DialogResult result = callNextForm.ShowDialog(this);
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            P01_History_Print callNextForm = new P01_History_Print(this.masterData, MyUtility.Convert.GetString(this.comboStatus.SelectedValue), MyUtility.Convert.GetString(this.displayCD.Value), MyUtility.Convert.GetInt(this.numTotalSewingTimePc.Value), MyUtility.Convert.GetInt(this.numNumOfSewer.Value));
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
