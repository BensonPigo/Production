using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;


namespace Sci.Production.IE
{
    public partial class P01_History : Sci.Win.Subs.Base
    {
        private DataTable gridData;
        IList<string> comboBox1_RowSource = new List<string>();
        DataRow masterData;

        public P01_History(DataRow MasterData)
        {
            InitializeComponent();
            masterData = MasterData;
            this.Text = "History -- " + masterData["StyleID"].ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            displayBox1.Value = masterData["StyleID"].ToString();
            displayBox2.Value = masterData["SeasonID"].ToString();
            displayBox4.Value = masterData["ComboType"].ToString();
            numericBox1.Value = MyUtility.Convert.GetInt(masterData["TotalSewingTime"]);
            numericBox2.Value = MyUtility.Convert.GetInt(masterData["NumberSewer"]);

            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@styleid";
            sp1.Value = masterData["StyleID"].ToString();
            sp2.ParameterName = "@seasonid";
            sp2.Value = masterData["SeasonID"].ToString();
            sp3.ParameterName = "@brandid";
            sp3.Value = masterData["BrandID"].ToString();
            sp4.ParameterName = "@combotype";
            sp4.Value = masterData["ComboType"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);

            //撈CD Code
            DataTable cdCode;
            string sqlCmd = "select CdCodeID from Style where ID = @styleid and SeasonID = @seasonid and BrandID = @brandid";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out cdCode);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!\r\n" + result.ToString());
                displayBox3.Value = "";
            }
            else
            {
                displayBox3.Value = cdCode.Rows.Count > 0 ? cdCode.Rows[0]["CdCodeID"].ToString() : "";
            }

            //撈Grid資料
            sqlCmd = @"select th.ID,th.TotalSewingTime,th.NumberSewer,thd.Seq,thd.OperationID,o.DescEN,
thd.Annotation,thd.Frequency,o.MtlFactorID,thd.SMV,thd.MachineTypeID,thd.Mold,
thd.PcsPerHour,thd.Sewer,thd.IETMSSMV,
LEFT(th.Phase+REPLICATE(' ',10),10)+' VER-'+th.Version as Status,
IIF(Phase = 'Initial',1,iif(Phase = 'Prelim',2,iif(Phase = 'Estimate',3,4))) as sort
from TimeStudyHistory th
left join TimeStudyHistory_Detail thd on th.ID = thd.ID
left join Operation o on thd.OperationID = o.ID
where StyleID = @styleid and SeasonID = @seasonid and BrandID = @brandid and ComboType = @combotype
order by IIF(Phase = 'Initial',1,iif(Phase = 'Prelim',2,iif(Phase = 'Estimate',3,4))),th.Version,thd.Seq";
            result = DBProxy.Current.Select(null, sqlCmd, cmds, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!\r\n" + result.ToString());
            }
            else
            {
                foreach (DataRow dr in gridData.Rows)
                {
                    if (comboBox1_RowSource.Contains(dr["Status"].ToString()) == false)
                    {
                        comboBox1_RowSource.Add(dr["Status"].ToString());
                    }
                }
                comboBox1.DataSource = comboBox1_RowSource;
            }
            listControlBindingSource1.DataSource = gridData;

            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("OperationID", header: "Operation code", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .EditText("DescEN", header: "Operation Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("Frequency", header: "Frequency", decimal_places: 2, iseditingreadonly: true)
                .Text("OperationMtlFactorID", header: "Factor", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Numeric("SMV", header: "SMV (sec)",  decimal_places: 4, iseditingreadonly: true)
                .Text("MachineTypeID", header: "M/C", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Mold", header: "Attachment", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("PcsPerHour", header: "Pcs/hr", decimal_places: 1, iseditingreadonly: true)
                .Numeric("Sewer", header: "Sewer", decimal_places: 1, iseditingreadonly: true)
                .Numeric("IETMSSMV", header: "Std. SMV", decimal_places: 4, iseditingreadonly: true);
        }

        //Status
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                gridData.DefaultView.RowFilter = "Status = '" + comboBox1.SelectedValue.ToString() + "'";

                numericBox1.Value = MyUtility.Convert.GetInt(gridData.DefaultView[0]["TotalSewingTime"]);
                numericBox2.Value = MyUtility.Convert.GetInt(gridData.DefaultView[0]["NumberSewer"]);
            }
            else
            {
                gridData.DefaultView.RowFilter = "Status = ''";
                numericBox1.Value = 0;
                numericBox2.Value = 0;
            }
        }

        //Summary
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_ArtworkSummary callNextForm;
            if (gridData.DefaultView.Count > 0)
            {
                callNextForm = new Sci.Production.IE.P01_ArtworkSummary("TimeStudyHistory_Detail", MyUtility.Convert.GetLong(gridData.DefaultView[0]["ID"]));
            }
            else
            {
                callNextForm = new Sci.Production.IE.P01_ArtworkSummary("TimeStudyHistory_Detail", 0);
            }
            DialogResult result = callNextForm.ShowDialog(this);

        }

        //To Excel
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_History_Print callNextForm = new Sci.Production.IE.P01_History_Print(masterData, MyUtility.Convert.GetString(comboBox1.SelectedValue), MyUtility.Convert.GetString(displayBox3.Value), MyUtility.Convert.GetInt(numericBox1.Value), MyUtility.Convert.GetInt(numericBox2.Value));
            DialogResult result = callNextForm.ShowDialog(this);
        }
    }
}
