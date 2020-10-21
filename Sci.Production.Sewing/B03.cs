using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class B03 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("FactoryID = '{0}'", Env.User.Factory);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterFactoryID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["FactoryID"]);
            string masterProductionDate = (e.Master == null) ? string.Empty : MyUtility.Convert.GetDate(e.Master["ProductionDate"]).Value.ToString("yyyy/MM/dd");
            this.DetailSelectCommand = string.Format(
                @"
select a.*,[LineLocationName] = b.Name
from ProductionLineAllocation_Detail a
left join LineLocation b on a.FactoryID = b.FactoryID and a.LineLocationID = b.ID
where a.FactoryID = '{0}'
and a.ProductionDate  = '{1}'",
                masterFactoryID,
                masterProductionDate);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            string sqlCmd;
            DataRow dr;
            DataTable dt;
            DualResult result;

            DataGridViewGeneratorTextColumnSettings lineLocationID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings sewingLineID = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings team = new DataGridViewGeneratorTextColumnSettings();

            #region lineLocationID Right Click & Validating
            lineLocationID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right &&
                    this.EditMode &&
                    e.RowIndex != -1)
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    sqlCmd = string.Format("select ID, Name from LineLocation where FactoryID = '{0}'", Env.User.Factory);
                    result = DBProxy.Current.Select(null, sqlCmd, out dt);
                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "10,20", MyUtility.Convert.GetString(dr["LineLocationID"]), headercaptions: "ID,Name");
                    DialogResult returnResult = item.ShowDialog();
                    if (returnResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    string selectedID = MyUtility.Convert.GetString(item.GetSelecteds().First()["ID"]);
                    DataRow[] lineLocationDataRow = dt.Select($"ID='{selectedID}'");
                    if (lineLocationDataRow.Count() > 0)
                    {
                        dr["LineLocationID"] = lineLocationDataRow[0]["ID"].ToString();
                        dr["LineLocationName"] = lineLocationDataRow[0]["Name"].ToString();
                    }
                    else
                    {
                        dr["LineLocationID"] = string.Empty;
                        dr["LineLocationName"] = string.Empty;
                    }

                    dr.EndEdit();
                }
            };

            lineLocationID.CellValidating += (s, e) =>
            {
                if (this.EditMode && !string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Convert.GetString(e.FormattedValue).Equals(dr["LineLocationID"]))
                    {
                        sqlCmd = string.Format("select ID, Name from LineLocation where FactoryID = '{0}' and ID = {1}", Env.User.Factory, e.FormattedValue);
                        result = DBProxy.Current.Select(null, sqlCmd, out dt);
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            dr["LineLocationID"] = string.Empty;
                            dr["LineLocationName"] = string.Empty;
                            MyUtility.Msg.WarningBox(string.Format("Line Location ID : {0} may be not exists", e.FormattedValue));
                            return;
                        }

                        dr["LineLocationID"] = MyUtility.Convert.GetString(e.FormattedValue);
                        dr["LineLocationName"] = MyUtility.Convert.GetString(dt.Rows[0]["Name"]);
                        dr.EndEdit();
                    }
                }
            };
            #endregion

            #region sewingLineID Validating
            sewingLineID.CellValidating += (s, e) =>
            {
                if (this.EditMode && !string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Convert.GetString(e.FormattedValue).Equals(dr["SewingLineID"]))
                    {
                        sqlCmd = string.Format("select ID from SewingLine where ID = '{0}' and FactoryID = '{1}' and Junk = 0", e.FormattedValue, Env.User.Factory);
                        if (!MyUtility.Check.Seek(sqlCmd))
                        {
                            dr["SewingLineID"] = string.Empty;
                            MyUtility.Msg.WarningBox(string.Format("Line# {0} may be not exists or junk.", e.FormattedValue));
                            return;
                        }

                        dr["SewingLineID"] = MyUtility.Convert.GetString(e.FormattedValue);
                        dr.EndEdit();
                    }
                }
            };
            #endregion

            #region team Left Click
            team.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left &&
                    this.EditMode &&
                    e.RowIndex != -1)
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    switch (MyUtility.Convert.GetString(dr["Team"]))
                    {
                        case "A":
                            dr["Team"] = "B";
                            break;
                        case "B":
                        default:
                            dr["Team"] = "A";
                            break;
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("LineLocationID", header: "Line Location ID", width: Widths.AnsiChars(2), settings: lineLocationID)
                .Text("LineLocationName", header: "Location Name", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("SewingLineID", header: "Line #", width: Widths.AnsiChars(2), settings: sewingLineID)
                .Text("Team", header: "Team", width: Widths.AnsiChars(1), settings: team, iseditingreadonly: true);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsertClick()
        {
            base.OnDetailGridInsertClick();
            this.CurrentDetailData["Team"] = "A";
        }

        /// <inheritdoc/>
        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();
            this.CurrentDetailData["Team"] = "A";
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentDetailData["Team"] = "A";
        }

        private void BtnCopyDate_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                string msg = this.CheckEmpty();
                if (!string.IsNullOrEmpty(msg))
                {
                    MyUtility.Msg.WarningBox(msg);
                    return;
                }

                string sqlCmd =
                    @"select b.*,[LineLocationName] = c.Name
                      from ProductionLineAllocation a
                      inner join ProductionLineAllocation_Detail b on a.FactoryID = b.FactoryID and a.ProductionDate = b.ProductionDate
                      left join LineLocation c on b.FactoryID = c.FactoryID and b.LineLocationID = c.ID
                      where a.FactoryID = @FactoryID
                      and a.ProductionDate = (select max(ProductionDate) ProductionDate from ProductionLineAllocation where ProductionDate < @ProductionDate)";

                System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@FactoryID", MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]));
                System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@ProductionDate", MyUtility.Convert.GetDate(this.CurrentMaintain["ProductionDate"]).Value.ToString("yyyyMMdd"));
                IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                cmds.Add(sp1);
                cmds.Add(sp2);
                DataTable dt;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out dt);
                this.detailgridbs.DataSource = dt;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            string msg = this.CheckEmpty();
            if (!string.IsNullOrEmpty(msg))
            {
                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            foreach (DataRow row in this.DetailDatas.Where(x => MyUtility.Check.Empty(x["LineLocationID"].ToString())))
            {
                MyUtility.Msg.WarningBox("LineLocationID cannot be empty.");
                return false;
            }

            foreach (DataRow row in this.DetailDatas.Where(x => MyUtility.Check.Empty(x["SewingLineID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SewingLineID cannot be empty.");
                return false;
            }

            foreach (DataRow row in this.DetailDatas.Where(x => MyUtility.Check.Empty(x["Team"].ToString())))
            {
                MyUtility.Msg.WarningBox("Team cannot be empty.");
                return false;
            }

            var query = from a in this.DetailDatas.AsEnumerable()
                        group a by new { LineLocationID = a["LineLocationID"], SewingLineID = a["SewingLineID"], Team = a["Team"] } into g
                        select new
                        {
                            g.Key.LineLocationID,
                            g.Key.SewingLineID,
                            g.Key.Team,
                            count = g.Count(),
                        };

            foreach (var item in query)
            {
                if (item.count > 1)
                {
                    MyUtility.Msg.WarningBox(string.Format("Line Location ID:{0}, Line#:{1}, Team:{2} already existed, please use another", item.LineLocationID, item.SewingLineID, item.Team));
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        private string CheckEmpty()
        {
            string rtnStr = string.Empty;

            if (MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]))
            {
                this.txtFactoryID.Focus();
                rtnStr = "Factory can't empty!!";
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ProductionDate"]))
            {
                this.dateProductionDate.Focus();
                rtnStr = "Date can't empty!!";
            }

            return rtnStr;
        }
    }
}
