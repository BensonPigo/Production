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
            //comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            //comboBox1.DataSource = comboxbs1;
            this.Text = "History -- " + masterData["StyleID"].ToString();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            displayBox1.Value = masterData["StyleID"].ToString();
            displayBox2.Value = masterData["SeasonID"].ToString();
            displayBox3.Value = MyUtility.GetValue.Lookup(string.Format("select CdCodeID from Style where ID = '{0}' and SeasonID = '{1}' and BrandID = '{2}'", masterData["StyleID"].ToString(), masterData["SeasonID"].ToString(), masterData["BrandID"].ToString()));
            displayBox4.Value = masterData["ComboType"].ToString();
            numericBox1.Value = Convert.ToInt32(masterData["TotalSewingTime"]);
            numericBox2.Value = Convert.ToInt32(masterData["NumberSewer"]);

            string sqlCmd = string.Format(@"select th.ID,th.TotalSewingTime,th.NumberSewer,thd.Seq,thd.OperationID,o.DescEN,
thd.Annotation,thd.Frequency,o.MtlFactorID,thd.SMV,thd.MachineTypeID,thd.Mold,
thd.PcsPerHour,thd.Sewer,thd.IETMSSMV,
LEFT(th.Phase+REPLICATE(' ',10),10)+' VER-'+th.Version as Status,
IIF(Phase = 'Initial',1,iif(Phase = 'Prelim',2,iif(Phase = 'Estimate',3,4))) as sort
from TimeStudyHistory th
left join TimeStudyHistory_Detail thd on th.ID = thd.ID
left join Operation o on thd.OperationID = o.ID
where StyleID = '{0}' and SeasonID = '{1}' and ComboType = '{2}'
order by IIF(Phase = 'Initial',1,iif(Phase = 'Prelim',2,iif(Phase = 'Estimate',3,4))),th.Version,thd.Seq", masterData["StyleID"].ToString(), masterData["SeasonID"].ToString(), masterData["ComboType"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
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
                .EditText("OperationDescEN", header: "Operation Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
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

                numericBox1.Value = Convert.ToInt32(gridData.DefaultView[0]["TotalSewingTime"]);
                numericBox2.Value = Convert.ToInt32(gridData.DefaultView[0]["NumberSewer"]);
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
                callNextForm = new Sci.Production.IE.P01_ArtworkSummary("TimeStudyHistory_Detail", Convert.ToInt64(gridData.DefaultView[0]["ID"]));
            }
            else
            {
                callNextForm = new Sci.Production.IE.P01_ArtworkSummary("TimeStudyHistory_Detail", 0);
            }
            DialogResult result = callNextForm.ShowDialog(this);

        }
    }
}
