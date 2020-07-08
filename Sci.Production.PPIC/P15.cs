using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P15
    /// </summary>
    public partial class P15 : Sci.Win.Tems.QueryForm
    {
        private Color red = Color.FromArgb(255, 179, 179);
        private Color yelow = Color.FromArgb(255, 255, 184);
        private Color green = Color.FromArgb(204, 255, 204);
        private Color gray = Color.FromArgb(224, 224, 224);
        private DataTable dt_result = new DataTable();

        /// <summary>
        /// P15
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            MyUtility.Tool.SetupCombox(this.comboBoxShift, 2, 1, "All,All,D,Day,N,Night");
            MyUtility.Tool.SetupCombox(this.comboBoxFbType, 2, 1, "All,All,F,Fabric ,A,Accessory ");
            MyUtility.Tool.SetupCombox(this.comboBoxStatus, 2, 1, "All,All,Waiting,Waiting,Preparing,Preparing,Ready,Ready,Finished,Finished,WPR,Waiting+Preparing+Ready");
            MyUtility.Tool.SetupCombox(this.comboBoxRotate, 2, 1, "15,15/sec,30,30/sec,60,60/sec");
            this.comboBoxShift.SelectedIndex = 0;
            this.comboBoxFbType.SelectedIndex = 0;
            this.comboBoxStatus.SelectedIndex = 5;
            this.comboBoxRotate.SelectedIndex = 2;
            this.QueryData();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.displayUseStock.BackColor = this.red;
            this.displayBoxPrepairing.BackColor = this.yelow;
            this.displayBoxReady.BackColor = this.green;
            this.displayBoxFinish.BackColor = this.gray;

            #region NO. 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts_id = new DataGridViewGeneratorTextColumnSettings();
            ts_id.CellMouseDoubleClick += (s, e) =>
            {
                if (this.checkBoxRotate.Checked)
                {
                    return;
                }

                var dr = this.gridDetail.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!this.checkBoxRotate.Checked)
                {
                    if (dr["FabricType"].Equals("Fabric"))
                    {
                        P10 frm = new P10(this.MenuItem, dr["ID"].ToString());
                        frm.ShowDialog(this);
                    }
                    else
                    {
                        var frm = new P11(this.MenuItem, dr["ID"].ToString());
                        frm.ShowDialog(this);
                    }
                }
            };
            #endregion

            #region issueLackID開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts_issueid = new DataGridViewGeneratorTextColumnSettings();
            ts_issueid.CellMouseDoubleClick += (s, e) =>
            {
                if (this.checkBoxRotate.Checked)
                {
                    return;
                }

                var dr = this.gridDetail.GetDataRow<DataRow>(e.RowIndex);
                if (dr == null)
                {
                    return;
                }

                if (!this.checkBoxRotate.Checked)
                {
                    string fullpath = System.Windows.Forms.Application.StartupPath + ".\\Sci.Production.Warehouse.dll";
                    var assemblys = Assembly.LoadFile(fullpath);
                    var types = assemblys.GetTypes().ToList();
                    var myClass = types.Where(x => x.FullName == (dr["FabricType"].Equals("Fabric") ? "Sci.Production.Warehouse.P16" : "Sci.Production.Warehouse.P15")).First();

                    if (myClass != null)
                    {
                        var callMethod = myClass.GetMethod("Call");
                        callMethod.Invoke(null, new object[] { dr["issueLackID"].ToString(), this.MdiParent });
                    }
                }
            };
            #endregion

            this.gridDetail.DataSource = this.detailbs;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("MDivisionID", header: "M", width: Widths.AnsiChars(4), iseditingreadonly: true)
                .Text("FactoryID", header: "FTY", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("ID", header: "NO.", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: ts_id)
                .DateTime("ApvDate", header: "Approved" + Environment.NewLine + "Date", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("FabricType", header: "FabricType", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("POID", header: "Master SP#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("SewingLineID", header: "Sewing" + Environment.NewLine + "Line", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("issueLackID", header: "Issue No.", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: ts_issueid);
            this.ChangeRowColor();
        }

        private void ChangeRowColor()
        {
            DataTable tmp_dt = (DataTable)this.detailbs.DataSource;
            for (int index = 0; index < tmp_dt.Rows.Count; index++)
            {
                switch (tmp_dt.Rows[index]["Status"].ToString())
                {
                    case "Waiting":
                        this.gridDetail.Rows[index].DefaultCellStyle.BackColor = this.red;
                        this.gridDetail.Rows[index].DefaultCellStyle.SelectionBackColor = this.red;
                        break;
                    case "Preparing":
                        this.gridDetail.Rows[index].DefaultCellStyle.BackColor = this.yelow;
                        this.gridDetail.Rows[index].DefaultCellStyle.SelectionBackColor = this.yelow;
                        break;
                    case "Ready":
                        this.gridDetail.Rows[index].DefaultCellStyle.BackColor = this.green;
                        this.gridDetail.Rows[index].DefaultCellStyle.SelectionBackColor = this.green;
                        break;
                    case "Finished":
                        this.gridDetail.Rows[index].DefaultCellStyle.BackColor = this.gray;
                        this.gridDetail.Rows[index].DefaultCellStyle.SelectionBackColor = this.gray;
                        break;
                    default:
                        break;
                }
            }
        }

        private void QueryData()
        {
            string query_sql = $@"select 
l.MDivisionID,
l.FactoryID,
l.ID,
l.ApvDate,
[Type] = IIF(l.Type='R','Replacement','Lacking'),
[FabricType] = IIF(l.FabricType='F','Fabric','Accessory'),
l.OrderID,
l.POID,
l.SewingLineID,
l.issueLackID,
[Status] = IIF(l.Status='Received','Finished',IIF(l.issueLackID='','Waiting', IIF(il.status='Confirmed','Preparing','Ready')))
from Lack l WITH (NOLOCK)
left join IssueLack il WITH (NOLOCK) on l.issueLackID=il.ID 
where l.status <> 'New' ";

            if (!MyUtility.Check.Empty(this.dateRangeDate.DateBox1.Value))
            {
                query_sql += $" and Convert(date, l.apvdate) >= '{this.dateRangeDate.DateBox1.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateRangeDate.DateBox2.Value))
            {
                query_sql += $" and Convert(date, l.apvdate) <= '{this.dateRangeDate.DateBox2.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                query_sql += $" and l.MDivisionID = '{this.txtMdivision.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                query_sql += $" and l.FactoryID  = '{this.txtfactory.Text}'";
            }

            if (!this.comboBoxShift.SelectedValue.Equals("All"))
            {
                query_sql += $" and l.Shift   = '{this.comboBoxShift.SelectedValue}'";
            }

            if (!this.comboBoxFbType.SelectedValue.Equals("All"))
            {
                query_sql += $" and l.FabricType    = '{this.comboBoxFbType.SelectedValue}'";
            }

            query_sql = $" select * from ({query_sql}) a ";

            if (!this.comboBoxStatus.SelectedValue.Equals("All"))
            {
                string status_where = this.comboBoxStatus.SelectedValue.Equals("WPR") ? "'Waiting','Preparing','Ready'" : $"'{this.comboBoxStatus.SelectedValue}'";
                query_sql += $" where status  in ({status_where})";
            }

            query_sql += " order by apvdate ";

            DualResult result = DBProxy.Current.Select(null, query_sql, out this.dt_result);
            if (result)
            {
                this.detailbs.DataSource = null;
                this.detailbs.DataSource = this.dt_result;
            }
            else
            {
                this.ShowErr(result);
                return;
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.QueryData();
            this.ChangeRowColor();
        }

        private void GridDetail_Sorted(object sender, EventArgs e)
        {
            this.ChangeRowColor();
        }

        private void CheckBoxRotate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxRotate.Checked)
            {
                this.dateRangeDate.ReadOnly = true;
                this.txtMdivision.ReadOnly = true;
                this.txtfactory.ReadOnly = true;
                this.comboBoxFbType.ReadOnly = true;
                this.comboBoxShift.ReadOnly = true;
                this.comboBoxStatus.ReadOnly = true;
                this.comboBoxRotate.ReadOnly = true;
                this.btnQuery.SetReadOnly(true);
                this.timerRotate.Interval = Convert.ToInt16(this.comboBoxRotate.SelectedValue) * 1000;
                this.timerRotate.Start();
            }
            else
            {
                this.dateRangeDate.ReadOnly = false;
                this.txtMdivision.ReadOnly = false;
                this.txtfactory.ReadOnly = false;
                this.comboBoxFbType.ReadOnly = false;
                this.comboBoxShift.ReadOnly = false;
                this.comboBoxRotate.ReadOnly = false;
                this.comboBoxStatus.ReadOnly = false;
                this.btnQuery.SetReadOnly(false);
                this.timerRotate.Stop();
            }
        }

        private void TimerRotate_Tick(object sender, EventArgs e)
        {
            this.timerRotate.Interval = Convert.ToInt16(this.comboBoxRotate.SelectedValue) * 1000;
            this.QueryData();
            this.ChangeRowColor();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
