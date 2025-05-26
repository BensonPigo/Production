using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using Sci.Production.Prg;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class P50 : Sci.Win.Tems.QueryForm
    {
        private DataTable dtMain;
        private DataRow drBasic;
        private Ict.Win.UI.DataGridViewTextBoxColumn colColorID;
        private Ict.Win.UI.DataGridViewTextBoxColumn colColorDesc;
        static string CHARs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private Ict.Win.UI.DataGridViewTextBoxColumn col_TestSeason;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_DueSeason;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_TestDate;
        private Ict.Win.UI.DataGridViewDateBoxColumn col_DueDate;

        /// <inheritdoc/>
        public P50(ToolStripMenuItem menuitem)
           : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.GetMaterialDocument();
        }

        private void GetMaterialDocument()
        {
            DataTable dt;

            string sql = $"Select distinct DocumentName From MaterialDocument WHERE FileRule in (1,2) and junk = 0";
            var result = DBProxy.Current.Select(string.Empty, sql, out dt);
            if (result && dt.Rows.Count > 0)
            {
                this.cboDocumentname.DataSource = dt;
                this.cboDocumentname.ValueMember = "DocumentName";
                this.cboDocumentname.DisplayMember = "DocumentName";
                this.cboDocumentname.SelectedIndex = 0;
            }
            else
            {
                this.ShowWarning("[Material Document Platform] is not exists!");
                return;
            }
        }

        private void GridSetup()
        {
            this.UI_grid.IsEditingReadOnly = false;
            this.UI_grid.AutoGenerateColumns = false;
            this.Helper.Controls.Grid.Generator(this.UI_grid)
               .CheckBox("sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: true, falseValue: false)
               .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("BrandRefNo", header: "Brand RefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("ColorID", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out this.colColorID)
               .Text("ColorDesc", header: "Color Desc", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out this.colColorDesc)
               .Button("...", header: "File", width: Widths.AnsiChars(8), onclick: this.ClickClip)
               .Date("TestReport", header: "Upload date", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Date("TestReportTestDate", header: "Test Date", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: this.GetTestDateCell()).Get(out this.col_TestDate)
               .Date("DueDate", header: "Due Date", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: this.GetDueDateCell()).Get(out this.col_DueDate)
               .Text("TestSeasonID", header: "Test Season", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: this.GetSeasonCell()).Get(out this.col_TestSeason)
               .Text("DueSeason", header: "Due Season", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: this.GetDueSeasonCell()).Get(out this.col_DueSeason)
               .Text("AddName", header: "Add Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Date("AddDate", header: "Add Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("EditName", header: "Edit Name ", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Date("EditDate", header: "Edit Date", width: Widths.AnsiChars(13), iseditingreadonly: true)
               ;

            this.UI_grid.DataSource = this.gridBS;
            this.UI_grid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.UI_grid_RowPrePaint);
            this.UI_grid.RowEnter += this.UI_Grid_RowEnter;
        }

        private void UI_Grid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.EditMode == false)
            {
                return;
            }

            var data = ((DataRowView)this.UI_grid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null)
            {
                return;
            }

            if (MyUtility.Check.Empty(data["CanModify"]))
            {
                this.col_DueDate.IsEditingReadOnly = true;
                this.col_DueSeason.IsEditingReadOnly = true;
                this.col_TestDate.IsEditingReadOnly = true;
                this.col_TestSeason.IsEditingReadOnly = true;
            }
            else
            {
                this.col_DueDate.IsEditingReadOnly = false;
                this.col_DueSeason.IsEditingReadOnly = false;
                this.col_TestDate.IsEditingReadOnly = false;
                this.col_TestSeason.IsEditingReadOnly = false;
            }
        }

        private void UI_grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataRow drMainGrid = this.UI_grid.GetDataRow(e.RowIndex);
            DataGridViewRow dgvMainGrid = this.UI_grid.Rows[e.RowIndex];
            if (drMainGrid == null || dgvMainGrid == null)
            {
                return;
            }

            dgvMainGrid.Cells["TestReportTestDate"].Style.BackColor = VFPColor.Blue_183_227_255;
            dgvMainGrid.Cells["TestSeasonID"].Style.BackColor = VFPColor.Blue_183_227_255;
            dgvMainGrid.Cells["DueSeason"].Style.BackColor = VFPColor.Blue_183_227_255;
            dgvMainGrid.Cells["DueDate"].Style.BackColor = VFPColor.Blue_183_227_255;
        }

        private DataGridViewGeneratorTextColumnSettings GetSeasonCell()
        {
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();

            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    if (row["UniqueKey"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please upload file first!", "Error");
                        row["SeasonRow"] = DBNull.Value;
                        return;
                    }

                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        row["SeasonRow"] = DBNull.Value;
                        return;
                    }

                    string sql = $@"Select ID From Season Where BrandID = '{this.txtBrand1.Text}'";

                    Sci.Win.Tools.SelectItem item =
                    new Sci.Win.Tools.SelectItem(sql, "20", row["TestSeasonID"].ToString(), "Season");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    string newValue = item.GetSelectedString().TrimEnd().ToUpper();
                    decimal expiration = MyUtility.Convert.GetDecimal(this.drBasic["Expiration"]);
                    if (!this.drBasic["Expiration"].Empty() && expiration > 0)
                    {
                        DataRow dueSeason = DBProxy.Current.SeekEx(
                        $@"
                        SELECT
	                        RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                            ,ID 
                        INTO #probablySeasonList
                        FROM Season Where BrandID = @BrandID

                        SELECT ID,RowNo 
                        FROM #probablySeasonList
                        WHERE RowNo = (SELECT RowNo FROM #probablySeasonList WHERE ID = @SeasonID) + {expiration - 1}",
                        "SeasonID",
                        newValue,
                        "BrandID",
                        this.drBasic["BrandID"]).ExtendedData;

                        if (dueSeason != null)
                        {
                            row["DueSeason"] = dueSeason["ID"];
                            row["SeasonRow"] = dueSeason["RowNo"];
                        }
                    }

                    row["TestSeasonID"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                    row.EndEdit();

                    sql = $@"
                    Update UASentReport
                    SET 
                    TestSeasonID = isnull(@SeasonID,''),
                    DueSeason = isnull(@DueSeason,''),
                    EditName = isnull(@UserID,'') ,
                    EditDate = getdate() 
                    WHERE 
                    BrandRefno = @BrandRefno and
                    ColorID = @ColorID and
                    SuppID = @SuppID and
                    DocumentName = @DocumentName and
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("SuppID", row["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("SeasonID", newValue),
                        new SqlParameter("DueSeason", row["DueSeason"]),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DualResult result1 = DBProxy.Current.Execute(null, sql, plis);

                    if (!result1)
                    {
                        this.ShowErr(result1.ToString());
                        return;
                    }
                }
            };

            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                var newValue = e.FormattedValue;
                var oldValue = row["TestSeasonID"].ToString();

                if (!MyUtility.Check.Empty(newValue) && !newValue.EqualString(oldValue))
                {
                    if (row["UniqueKey"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please upload file first!", "Error");
                        row["DueSeason"] = DBNull.Value;
                        return;
                    }

                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        row["DueSeason"] = DBNull.Value;
                        return;
                    }

                    string sql = $@"Select ID From Season Where BrandID = '{this.txtBrand1.Text}' and ID = @SeasonID";

                    if (DBProxy.Current.SeekEx(sql, "SeasonID", newValue).ExtendedData == null)
                    {
                        MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > not found!", newValue), "Error");
                        e.Cancel = true;
                        return;
                    }

                    decimal expiration = MyUtility.Convert.GetDecimal(this.drBasic["Expiration"]);
                    if (!this.drBasic["Expiration"].Empty() && expiration > 0)
                    {
                        DataRow dueSeason = DBProxy.Current.SeekEx(
                        $@"
                        SELECT
	                        RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                            ,ID 
                        INTO #probablySeasonList
                        FROM Season Where BrandID = @BrandID

                        SELECT ID,RowNo FROM #probablySeasonList
                        WHERE RowNo = (SELECT RowNo FROM #probablySeasonList WHERE ID =@SeasonID)+{expiration - 1}",
                        "SeasonID",
                        newValue,
                        "BrandID",
                        this.drBasic["BrandID"]).ExtendedData;

                        if (dueSeason != null)
                        {
                            row["DueSeason"] = dueSeason["ID"];
                            row["SeasonRow"] = dueSeason["RowNo"];
                        }
                    }

                    row["TestSeasonID"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                    row.EndEdit();

                    sql = $@"
                    Update 
                    UASentReport 
                    SET 
                    TestSeasonID = isnull(@SeasonID,''),
                    DueSeason = isnull(@DueSeason,''),
                    EditName = isnull(@UserID,'') ,
                    EditDate = getdate() 
                    WHERE 
                    BrandRefno = @BrandRefno and 
                    ColorID = @ColorID and 
                    SuppID = @SuppID and 
                    DocumentName = @DocumentName and 
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("SuppID", row["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("SeasonID", newValue),
                        new SqlParameter("DueSeason", row["DueSeason"]),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DualResult result1 = DBProxy.Current.Execute(null, sql, plis);

                    if (!result1)
                    {
                        this.ShowErr(result1.ToString());
                        return;
                    }
                }
            };

            return ts;
        }

        private DataGridViewGeneratorTextColumnSettings GetDueSeasonCell()
        {
            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    if (row["UniqueKey"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please upload file first!", "Error");
                        row["SeasonRow"] = DBNull.Value;
                        return;
                    }

                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        row["SeasonRow"] = DBNull.Value;
                        return;
                    }

                    if (row["TestSeasonID"].Empty())
                    {
                        MyUtility.Msg.ErrorBox("Please input season first!");
                        return;
                    }

                    string sql = $@"Select ID From Season Where BrandID = '{this.txtBrand1.Text}'";

                    Sci.Win.Tools.SelectItem item =
                    new Sci.Win.Tools.SelectItem(sql, "20", row["DueSeason"].ToString(), "Season");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    string newValue = item.GetSelectedString().TrimEnd().ToUpper();

                    sql = $@"
                    SELECT
	                    RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                        ,ID 
                    INTO #probablySeasonList
                    FROM dbo.Season Where BrandID = @BrandID

                    SELECT main.ID FROM #probablySeasonList main
                    Outer Apply (
                      SELECT RowNo FROM #probablySeasonList WHERE ID = @SeasonID
                    )chkSeason
                    WHERE main.ID = @DueSeason and main.RowNo >= chkSeason.RowNo";
                    if (DBProxy.Current.SeekEx(sql, "DueSeason", newValue, "SeasonID", row["TestSeasonID"], "BrandID", this.drBasic["BrandID"]).ExtendedData == null)
                    {
                        MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > can't less than {1}!", newValue, row["TestSeasonID"].ToString()), "Error");
                        return;
                    }

                    row["DueSeason"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                    row.EndEdit();

                    sql = $@"
                    Update 
                    UASentReport 
                    SET  
                    DueSeason = isnull(@DueSeason ,'')
                    WHERE 
                    BrandRefno = @BrandRefno and
                    ColorID = @ColorID and 
                    SuppID = @SuppID and
                    DocumentName = @DocumentName and 
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("SuppID", row["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("DueSeason", newValue),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DualResult result1 = DBProxy.Current.Execute(null, sql, plis);

                    if (!result1)
                    {
                        this.ShowErr(result1.ToString());
                        return;
                    }
                }
            };

            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                var newValue = e.FormattedValue;
                var oldValue = row["DueSeason"].ToString();

                if (!MyUtility.Check.Empty(newValue) && !newValue.EqualString(oldValue))
                {
                    if (row["UniqueKey"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please upload file first!", "Error");
                        e.Cancel = true;
                        row["DueSeason"] = DBNull.Value;
                        return;
                    }

                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        e.Cancel = true;
                        row["DueSeason"] = DBNull.Value;
                        return;
                    }

                    string sql = string.Empty;

                    if (!row["TestSeasonID"].Empty())
                    {
                        sql = $@"Select ID From Season Where BrandID = '{this.txtBrand1.Text}' and ID = @SeasonID";

                        if (DBProxy.Current.SeekEx(sql, "SeasonID", newValue).ExtendedData == null)
                        {
                            MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > not found!", newValue), "Error");
                            e.Cancel = true;
                            return;
                        }

                        string dueSeason = string.Empty;

                        sql = $@"
                        SELECT
	                        RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                            ,ID 
                        INTO #probablySeasonList                     
                        FROM Season Where BrandID = @BrandID

                        SELECT main.ID FROM #probablySeasonList main
                        Outer Apply (
                          SELECT RowNo FROM #probablySeasonList WHERE ID = @SeasonID
                        )chkSeason
                        WHERE main.ID = @DueSeason and main.RowNo >= chkSeason.RowNo";
                        if (DBProxy.Current.SeekEx(sql, "DueSeason", newValue, "SeasonID", row["TestSeasonID"], "BrandID", this.drBasic["BrandID"]).ExtendedData == null)
                        {
                            MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > can't less than {1}!", newValue, row["TestSeasonID"].ToString()), "Error");
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        MyUtility.Msg.ErrorBox("Please input season first!");
                        row["DueSeason"] = DBNull.Value;
                        e.Cancel = true;
                        return;
                    }

                    row["DueSeason"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                    row.EndEdit();
                    sql = $@"
                    Update 
                    UASentReport
                    SET  
                    DueSeason = isnull(@DueSeason,''), 
                    EditName = isnull(@UserID,'') ,
                    EditDate = getdate() 
                    WHERE 
                    BrandRefno = @BrandRefno and 
                    ColorID = @ColorID and 
                    SuppID = @SuppID and 
                    DocumentName = @DocumentName and 
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("SuppID", row["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("DueSeason", newValue),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DualResult result1 = DBProxy.Current.Execute(null, sql, plis);

                    if (!result1)
                    {
                        this.ShowErr(result1.ToString());
                        return;
                    }
                }
            };

            return ts;
        }

        private DataGridViewGeneratorDateColumnSettings GetTestDateCell()
        {
            DataGridViewGeneratorDateColumnSettings ts = new DataGridViewGeneratorDateColumnSettings();
            string whereStr = string.Empty;

            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                var newValue = e.FormattedValue;
                var oldValue = MyUtility.Convert.GetDate(row["TestReportTestDate"]);

                if (!MyUtility.Check.Empty(newValue) &&
                    (MyUtility.Convert.GetDate(newValue) != MyUtility.Convert.GetDate(oldValue)))
                {
                    if (row["UniqueKey"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please upload file first!", "Error");
                        row["TestReportTestDate"] = DBNull.Value;
                        return;
                    }

                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        e.Cancel = true;
                        row["TestReportTestDate"] = DBNull.Value;
                        return;
                    }

                    if (MyUtility.Convert.GetDate(newValue) > DateTime.Now)
                    {
                        MyUtility.Msg.ErrorBox("[Test Date] can't bigger than Today!", "Error");
                        row["TestReportTestDate"] = DBNull.Value;
                        return;
                    }

                    newValue = MyUtility.Check.Empty(newValue) ? DBNull.Value : newValue;
                    row["TestReportTestDate"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                    row.EndEdit();

                    string sql = $@"
                    Update 
                    UASentReport
                    SET 
                    TestReportTestDate = @TestDate,
                    EditName = @UserID,
                    EditDate = getdate() 
                    WHERE 
                    BrandRefno = @BrandRefno and
                    ColorID = @ColorID and 
                    SuppID = @SuppID and 
                    DocumentName = @DocumentName and 
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("SuppID", row["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("TestDate", newValue),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DualResult result1 = DBProxy.Current.Execute(null, sql, plis);

                    if (!result1)
                    {
                        this.ShowErr(result1.ToString());
                        return;
                    }
                }
            };

            return ts;
        }

        private DataGridViewGeneratorDateColumnSettings GetDueDateCell()
        {
            DataGridViewGeneratorDateColumnSettings ts = new DataGridViewGeneratorDateColumnSettings();
            string whereStr = string.Empty;

            ts.CellValidating += (s, e) =>
            {
                DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                if (!((Sci.Win.Forms.Base)grid.FindForm()).EditMode)
                {
                    return;
                }

                DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);

                var newValue = e.FormattedValue;
                var oldValue = MyUtility.Convert.GetDate(row["DueDate"]);

                if (!MyUtility.Check.Empty(newValue) &&
                    (MyUtility.Convert.GetDate(newValue) != MyUtility.Convert.GetDate(oldValue)))
                {
                    if (row["UniqueKey"] == DBNull.Value)
                    {
                        MyUtility.Msg.ErrorBox("Please upload file first!", "Error");
                        e.Cancel = true;
                        row["DueDate"] = DBNull.Value;
                        return;
                    }

                    if (!MyUtility.Convert.GetBool(row["CanModify"]))
                    {
                        MyUtility.Msg.ErrorBox("Can't modify!", "Error");
                        e.Cancel = true;
                        row["DueDate"] = DBNull.Value;
                        return;
                    }

                    if (!row["TestReportTestDate"].Empty())
                    {
                        DateTime tm;
                        if (!DateTime.TryParse(newValue.ToString(), out tm))
                        {
                            MyUtility.Msg.ErrorBox(string.Format("< Due Date: {0} > isn't a valid date value!", newValue), "Error");
                            e.Cancel = true;
                            return;
                        }

                        if (tm < MyUtility.Convert.GetDate(row["TestReportTestDate"]))
                        {
                            MyUtility.Msg.ErrorBox("Due Date should bigger than Test Date!", "Error");
                            e.Cancel = true;
                            return;
                        }
                    }
                    else
                    {
                        MyUtility.Msg.ErrorBox("Please input [Test Date] first!");
                        e.Cancel = true;
                        return;
                    }

                    row["DueDate"] = newValue;
                    row["EditDate"] = DateTime.Now;
                    row["EditName"] = Env.User.UserID;
                    row.EndEdit();

                    string sql = $@"
                    Update
                    UASentReport
                    SET
                    DueDate = @DueDate, 
                    EditName = @UserID, 
                    EditDate = getdate()
                    WHERE
                    BrandRefno = @BrandRefno and
                    ColorID = @ColorID and 
                    SuppID = @SuppID and
                    DocumentName = @DocumentName and
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", row["BrandRefno"]),
                        new SqlParameter("ColorID", row["ColorID"]),
                        new SqlParameter("SuppID", row["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("DueDate", newValue),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DualResult result1 = DBProxy.Current.Execute(null, sql, plis);

                    if (!result1)
                    {
                        this.ShowErr(result1.ToString());
                        return;
                    }
                }
            };

            return ts;
        }

        /// <inheritdoc/>
        private void ClickClip(object sender, EventArgs e)
        {
            var row = this.UI_grid.GetCurrentDataRow();
            if (row == null)
            {
                return;
            }

            var id = row["UniqueKey"].ToString();
            if (id.IsNullOrWhiteSpace())
            {
                return;
            }

            string sqlcmd = $@"select 
            [FileName] = TableName + PKey,
             SourceFile,
            AddDate
            from GASAClip
            where TableName = 'UASentReport' and 
            UniqueKey = '{id}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
            }

            List<string> list = new List<string>();
            string filePath = MyUtility.GetValue.Lookup("select [path] from CustomizedClipPath where TableName = 'UASentReport'");

            // 組ClipPath
            string clippath = MyUtility.GetValue.Lookup($"select ClipPath from System");
            foreach (DataRow dataRow in dt.Rows)
            {
                string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                string saveFilePath = Path.Combine(clippath, yyyyMM);
                string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                lock (FileDownload_UpData.DownloadFileAsync($"{PmsWebAPI.PMSAPApiUri}/api/FileDownload/GetFile", filePath + "\\" + yyyyMM, fileName, saveFilePath))
                {
                }
            }

            #region 傳遞額外的參數
            if (!row.Table.Columns.Contains("BasicBrandID"))
            {
                row.Table.Columns.Add("BasicBrandID", typeof(string));
            }

            if (!row.Table.Columns.Contains("BasicDocumentName"))
            {
                row.Table.Columns.Add("BasicDocumentName", typeof(string));
            }

            if (!row.Table.Columns.Contains("updateCol_Where"))
            {
                row.Table.Columns.Add("updateCol_Where", typeof(string));
            }

            if (!this.txtTestSeason.Text.Empty())
            {
                row["TestSeasonID"] = this.txtTestSeason.Text;
                row["dueSeason"] = this.txtDueSeason.Text;
            }

            if (this.dateTestDate.Value.HasValue)
            {
                row["TestReportTestDate"] = MyUtility.Convert.GetDate(this.dateTestDate.Value).ToYYYYMMDD();
                if (this.DueDate.Value.HasValue)
                {
                    row["dueDate"] = MyUtility.Convert.GetDate(this.DueDate.Value).ToYYYYMMDD();
                }
            }

            var now = DateTime.Now;

            if (row["UniqueKey"].Empty())
            {
                row["AddName"] = Env.User.UserID;
                row["AddDate"] = now;
            }
            else
            {
                row["EditName"] = Env.User.UserID;
                row["EditDate"] = now;
            }

            row["BasicBrandID"] = this.drBasic["BrandID"];
            row["BasicDocumentName"] = this.drBasic["DocumentName"];
            if (this.DueDate.Value.HasValue)
            {
                row["dueDate"] = MyUtility.Convert.GetDate(this.DueDate.Value).ToYYYYMMDD();
            }

            string updateCol = string.Empty;
            if (!this.txtTestSeason.Text.Empty())
            {
                updateCol += $",TestSeasonID = '{this.txtTestSeason.Text}',DueSeason = '{this.txtDueSeason.Text}' ";
            }

            if (this.dateTestDate.Value.HasValue)
            {
                updateCol += $",TestReportTestDate = '{this.dateTestDate.Text}'";
                if (this.dateTestDate.Value > DateTime.Now)
                {
                    MyUtility.Msg.ErrorBox("[Test Date] can't bigger than Today!", "Error");
                    return;
                }

                if (this.DueDate.Value.HasValue)
                {
                    updateCol += $",DueDate = '{this.DueDate.Text}'";
                }
            }

            row["updateCol_Where"] = updateCol;
            #endregion

            bool isEnable = MyUtility.Check.Empty(row["canModify"]) ? false : true;
            using (var dlg = new PublicForm.ClipGASA("UASentReport", id, isEnable, row, apiUrlFile: $"{PmsWebAPI.PMSAPApiUri}/api/FileDelete/RemoveFile"))
            {
                dlg.ShowDialog();

                foreach (DataRow dataRow in dt.Rows)
                {
                    string yyyyMM = ((DateTime)dataRow["AddDate"]).ToString("yyyyMM");
                    string saveFilePath = Path.Combine(clippath, yyyyMM);
                    string fileName = dataRow["FileName"].ToString() + Path.GetExtension(dataRow["SourceFile"].ToString());
                    string deleteFile = Path.Combine(saveFilePath, fileName);
                    if (File.Exists(deleteFile))
                    {
                        File.Delete(deleteFile);
                        Console.WriteLine($"刪除 {filePath} 成功！");
                    }
                }
            }
        }

        private void Btn_NewSearch_Click(object sender, EventArgs e)
        {
            this.txtBrand1.Text = string.Empty;
            this.txtBrandRefno.Text = string.Empty;
            this.txtMultiSupplier1.Text = string.Empty;
            this.txtRefno.Text = string.Empty;
            this.txtStyle1.Text = string.Empty;
            this.txtColor.Text = string.Empty;
            this.txtUser1.TextBox1.Text = string.Empty;
            this.chkUploadRecord.Checked = false;
            this.chkNonValidDoc.Checked = false;
        }

        private void Btn_Find_Click(object sender, EventArgs e)
        {
            var conditions = new StringBuilder(@" 1=1");
            List<SqlParameter> parmes = new List<SqlParameter>();
            if (this.cboDocumentname.Text == string.Empty || this.txtBrand1.Text == string.Empty)
            {
                this.ShowWarning("[Document Name] and [Brand] can't be empty!");
                return;
            }

            string cmd = $@"
Select m.* 
, MtlType = MtlType.value
, WeaveType = WeaveType.value
, supplier = supp.value
, MergedBrand = MergedBrand.value
From MaterialDocument m
Outer apply ( SELECT
		value = STUFF((SELECT
				CONCAT(',', MtlType.MtltypeId)
			FROM MaterialDocument_MtlType MtlType WITH (NOLOCK)
			WHERE MtlType.DocumentName = m.DocumentName and MtlType.BrandID = m.BrandID
			FOR XML PATH (''))
		, 1, 1, '')
) MtlType
Outer apply ( SELECT
		value = STUFF((SELECT
				CONCAT(',', WeaveType.WeaveTypeId)
			FROM MaterialDocument_WeaveType WeaveType WITH (NOLOCK)
			WHERE WeaveType.DocumentName = m.DocumentName and WeaveType.BrandID = m.BrandID
			FOR XML PATH (''))
		, 1, 1, '')
) WeaveType
Outer apply ( SELECT
		value = STUFF((SELECT
				CONCAT(',', supp.SuppID)
			FROM MaterialDocument_Supplier supp WITH (NOLOCK)
			WHERE supp.DocumentName = m.DocumentName
			AND supp.BrandID = m.BrandID
			FOR XML PATH (''))
		, 1, 1, '')
) supp
Outer apply (
    SELECT value = STUFF((
        SELECT CONCAT(',', MergedBrand)
		FROM MaterialDocument_Brand WITH (NOLOCK)
		WHERE DocumentName = m.DocumentName
		AND BrandID = m.BrandID
		FOR XML PATH (''))
	, 1, 1, '')
) MergedBrand
Where documentName = @documentName 
and brandID = @brandID 
and m.FileRule in ('1','2') 
and m.junk = 0
";

            var res = DBProxy.Current.SeekEx(cmd, "documentName", this.cboDocumentname.Text, "brandID", this.txtBrand1.Text);
            if (!res)
            {
                this.ShowErr(res.InnerResult);
                return;
            }

            if (res.ExtendedData == null)
            {
                MyUtility.Msg.ErrorBox("Basic setting not found!");
                return;
            }

            this.drBasic = res.ExtendedData;
            this.colColorID.Visible = this.drBasic["FileRule"].ToString() != "1";
            this.colColorDesc.Visible = this.drBasic["FileRule"].ToString() != "1";

            #region 基本檔設定
            if (MyUtility.Convert.GetBool(this.drBasic["ExcludeRibItem"]))
            {
                conditions.AppendLine($" and f.ribitem = 0");
            }

            conditions.AppendLine($" and f.type='{this.drBasic["FabricType"]}'");
            conditions.AppendLine($" and f.BrandRefNo <> ''");
            conditions.AppendLine($" and Season.Month >= (select month From Season where id ='{this.drBasic["ActiveSeason"]}' and BrandID = '{this.drBasic["brandID"]}')");
            if (!this.drBasic["EndSeason"].Empty())
            {
                conditions.AppendLine($" and Season.Month <= (select month From Season where id ='{this.drBasic["EndSeason"]}' and BrandID = '{this.drBasic["brandID"]}')");
            }

            if (!this.drBasic["ExcludeProgram"].Empty())
            {
                conditions.AppendLine($" and o.ProgramID not in (select data from splitstring('{this.drBasic["ExcludeProgram"]}',','))");
            }

            if (MyUtility.Convert.GetBool(this.drBasic["ExcludeReplace"]))
            {
                conditions.AppendLine($" and (po3.seq1 < '50' or po3.seq1 > '69')");
            }

            if (MyUtility.Convert.GetBool(this.drBasic["ExcludeStock"]))
            {
                conditions.AppendLine($" and po3.seq1 < '70'");
            }

            if (this.drBasic["FabricType"].ToString() == "A" && !this.drBasic["MtlTypeClude"].Empty() && !this.drBasic["MtlType"].Empty())
            {
                string not = this.drBasic["MtlTypeClude"].ToString() == "E" ? "not" : string.Empty;
                conditions.AppendLine($" and f.MtltypeId {not} in (select data from splitstring('{this.drBasic["MtlType"]}', ','))");
            }
            else if (this.drBasic["FabricType"].ToString() == "F" && !this.drBasic["MtlTypeClude"].Empty() && !this.drBasic["WeaveType"].Empty())
            {
                string not = this.drBasic["MtlTypeClude"].ToString() == "E" ? "not" : string.Empty;
                conditions.AppendLine($" and f.WeaveTypeId {not} in (select data from splitstring('{this.drBasic["WeaveType"]}', ','))");
            }

            if (!this.drBasic["SupplierClude"].Empty() && !this.drBasic["supplier"].Empty())
            {
                string not = this.drBasic["SupplierClude"].ToString() == "E" ? "not" : string.Empty;
                conditions.AppendLine($" and s2.ID {not} in (select data from splitstring('{this.drBasic["supplier"]}', ','))");
            }

            string category = DBProxy.Current.LookupEx<string>(
            @"
            SELECT
            value = STUFF((SELECT
			            CONCAT(',', ID)
		            FROM DropDownList WITH (NOLOCK)
		            WHERE type ='Category'
		            and NAME in(select data from splitstring(@Category,','))
		            FOR XML PATH (''))
	            , 1, 1, '')",
            "Category",
            this.drBasic["Category"]).ExtendedData;

            conditions.AppendLine($" and o.Category in (select data from splitstring('{category}',','))");

            parmes.Add(new SqlParameter() { ParameterName = "@documentName", SqlDbType = SqlDbType.VarChar, Size = 100, Value = this.cboDocumentname.Text });
            parmes.Add(new SqlParameter() { ParameterName = "@brandID", SqlDbType = SqlDbType.VarChar, Size = 8, Value = this.txtBrand1.Text });
            parmes.Add(new SqlParameter() { ParameterName = "@FileRule", SqlDbType = SqlDbType.Int, Value = this.drBasic["FileRule"] });
            #endregion

            #region 查詢條件
            if (this.txtRefno.Text != string.Empty)
            {
                conditions.AppendLine(" And f.Refno = @SearchRefno");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchRefno", SqlDbType = SqlDbType.VarChar, Size = 20, Value = this.txtRefno.Text });
            }

            if (this.txtSeason1.Text != string.Empty)
            {
                conditions.AppendLine(" And o.SeasonID = @SeasonID");
                parmes.Add(new SqlParameter() { ParameterName = "@SeasonID", SqlDbType = SqlDbType.VarChar, Size = 10, Value = this.txtSeason1.Text });
            }

            if (this.txtBrandRefno.Text != string.Empty)
            {
                conditions.AppendLine(" And f.BrandRefNo = @SearchBrandRefno");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchBrandRefno", SqlDbType = SqlDbType.VarChar, Size = 50, Value = this.txtBrandRefno.Text });
            }

            if (this.txtColor.Text != string.Empty)
            {
                conditions.AppendLine(" And Color.ID = @SearchColor");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchColor", SqlDbType = SqlDbType.VarChar, Size = 6, Value = this.txtColor.Text });
            }

            if (this.txtStyle1.Text != string.Empty)
            {
                conditions.AppendLine(" And s.ID = @SearchStyle");
                parmes.Add(new SqlParameter() { ParameterName = "@SearchStyle", SqlDbType = SqlDbType.VarChar, Size = 15, Value = this.txtStyle1.Text });
            }

            if (this.txtMultiSupplier1.Text != string.Empty)
            {
                conditions.AppendLine(" and s2.ID in (select data from splitstring(@supp,','))");
                parmes.Add(new SqlParameter("@supp", this.txtMultiSupplier1.Text));
            }

            if (this.txtUser1.TextBox1.Text != string.Empty)
            {
                conditions.AppendLine(" And po.PCHandle = @pchandle");
                parmes.Add(new SqlParameter() { ParameterName = "@pchandle", SqlDbType = SqlDbType.VarChar, Size = 10, Value = this.txtUser1.TextBox1.Text });
            }

            parmes.Add(new SqlParameter() { ParameterName = "@Responsibility", SqlDbType = SqlDbType.VarChar, Size = 50, Value = this.drBasic["Responsibility"] });
            #endregion

            // 品牌共用
            string sql = @"
SELECT MergedBrand
From MaterialDocument_Brand 
Where DocumentName = @documentName 
and BrandID = @BrandID
";
            DataTable dtBrand;
            var result = DBProxy.Current.Select(string.Empty, sql, parmes, out dtBrand);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 物料清單同一種料且同色只會出現一次,就算是品牌共用也只會有一筆
            string mainSql = $@"
select  distinct
    o.SeasonID
    ,SuppID = s2.ID
	,Supplier = IIF(Isnull(s2.AbbEN, '') = '', s2.ID, Concat(s2.ID, '-', s2.AbbEN))
    ,Refno = f.Refno
	,SCIRefNo = f.SCIRefNo
	,f.BrandRefNo
    ,ColorID = iif(@FileRule = 1, '', Color.ID)
    ,ColorDesc = iif(@FileRule = 1, '', Color.Name)
    ,canModify = CAST(iif((chkNoRes.value is null and @Responsibility = 'F') or chkNoRes.value = 'F', 1, 0) AS BIT)
    ,FactoryID = o.FtyGroup 
    ,[SeasonRow] = seasonList.RowNo
    from PO_Supp_Detail po3 WITH (NOLOCK)
    LEFT JOIN PO_Supp_Detail stockPO3 with (nolock) on iif(po3.StockPOID >'', 1, 0) = 0 AND stockPO3.ID =  po3.StockPOID
		and stockPO3.Seq1 = po3.StockSeq1
		and stockPO3.Seq2 = po3.StockSeq2	        
    INNER JOIN Orders o with(nolock) on o.ID = IIF(IsNull(po3.StockPOID, '') = '' , po3.ID, stockPO3.ID)
    inner join Fabric f WITH (NOLOCK) on f.SCIRefno =  IIF(IsNull(po3.StockPOID, '') = '' , po3.SCIRefno, stockPO3.SCIRefno)
    inner join PO_Supp po2 WITH (NOLOCK) on po2.ID = o.ID and po2.Seq1 = IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq1, stockPO3.Seq1)
    inner join PO WITH (NOLOCK) on po.ID = po2.ID 
    INNER JOIN Season WITH (NOLOCK) on o.SeasonID = Season.ID and o.BrandID = Season.BrandID
	INNER JOIN Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey		 
    Inner Join Supp WITH (NOLOCK) on po2.SuppID = Supp.ID
    INNER Join dbo.BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = o.BrandID and bs.SuppID = Supp.ID
    Inner Join Supp s2 WITH (NOLOCK) on s2.ID = bs.SuppGroup 
    Outer Apply(
        SELECT Color FROM GetPo3Spec(IIF(IsNull(po3.StockPOID, '') = '' , po3.ID, stockPO3.ID),IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq1, stockPO3.Seq1),IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq2, stockPO3.Seq2)) po3Spec
    )po3Spec
    LEFT JOIN  Color WITH (NOLOCK) ON Color.BrandId = o.BrandID AND Color.ID = po3Spec.Color
  
    Left join #probablySeasonList seasonList on seasonList.ID = Season.ID
    Outer apply(
        select top 1 value = Responsibility 
        FROM MaterialDocument_Responsbility 
        where DocumentName = @documentName 
        and BrandID = @brandID 
        and SuppID = s2.ID
    )chkNoRes
	where o.Junk = 0
    and o.Qty > 0
    and po2.SuppID <> 'FTY'
    and IIF(IsNull(po3.StockPOID, '') = '' , po3.Junk, stockPO3.Junk)  = 0        
	and IIF(IsNull(po3.StockPOID, '') = '' , po3.Qty, stockPO3.Qty) > 0
    and s.DevOption = 0
    and o.BrandID = @BrandID
    and {conditions}
";

            foreach (DataRow dr in dtBrand.Rows)
            {
                mainSql += $@"
UNION
select  distinct
     o.SeasonID
    ,SuppID = s2.ID
	,Supplier = IIF(Isnull(s2.AbbEN, '') = '', s2.ID, Concat(s2.ID, '-', s2.AbbEN))
    ,Refno = f.Refno
	,SCIRefNo = f.SCIRefNo
	,f.BrandRefNo
    ,ColorID = iif(@FileRule = 1, '', Color.ID)
    ,ColorDesc = iif(@FileRule = 1, '', Color.Name)
    ,canModify = CAST(iif((chkNoRes.value is null and @Responsibility = 'F') or chkNoRes.value = 'F', 1, 0) AS BIT)
    ,FactoryID = o.FtyGroup      
    ,[SeasonRow] = seasonList.RowNo
    from PO_Supp_Detail po3 WITH (NOLOCK)
    LEFT JOIN PO_Supp_Detail stockPO3 with (nolock) on iif(po3.StockPOID >'', 1, 0) = 0 AND stockPO3.ID =  po3.StockPOID
		and stockPO3.Seq1 = po3.StockSeq1
		and stockPO3.Seq2 = po3.StockSeq2	        
    INNER JOIN Orders o with(nolock) on o.ID = IIF(IsNull(po3.StockPOID, '') = '' , po3.ID, stockPO3.ID)
    inner join Fabric f WITH (NOLOCK) on f.SCIRefno =  IIF(IsNull(po3.StockPOID, '') = '' , po3.SCIRefno, stockPO3.SCIRefno)
    inner join PO_Supp po2 WITH (NOLOCK) on po2.ID = o.ID and po2.Seq1 = IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq1, stockPO3.Seq1)
    inner join PO WITH (NOLOCK) on po.ID = po2.ID 
    INNER JOIN Season WITH (NOLOCK) on o.SeasonID = Season.ID and o.BrandID = Season.BrandID
	INNER JOIN Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey		 
    Inner Join Supp WITH (NOLOCK) on po2.SuppID = Supp.ID
    INNER Join dbo.BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = o.BrandID and bs.SuppID = Supp.ID
    Inner Join Supp s2 WITH (NOLOCK) on s2.ID = bs.SuppGroup 
    Outer Apply(
        SELECT Color FROM GetPo3Spec(IIF(IsNull(po3.StockPOID, '') = '' , po3.ID, stockPO3.ID),IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq1, stockPO3.Seq1),IIF(IsNull(po3.StockPOID, '') = '' , po3.Seq2, stockPO3.Seq2)) po3Spec
    )po3Spec
    LEFT JOIN  Color WITH (NOLOCK) ON Color.BrandId = o.BrandID AND Color.ID = po3Spec.Color
  
    Left join #probablySeasonList seasonList on seasonList.ID = Season.ID
    Outer apply(
        select top 1 value = Responsibility 
        FROM MaterialDocument_Responsbility 
        where DocumentName = @documentName 
        and BrandID = @brandID 
        and SuppID = s2.ID
    )chkNoRes
	where o.Junk = 0
    and o.Qty > 0
    and po2.SuppID <> 'FTY'
    and IIF(IsNull(po3.StockPOID, '') = '' , po3.Junk, stockPO3.Junk)  = 0        
	and IIF(IsNull(po3.StockPOID, '') = '' , po3.Qty, stockPO3.Qty) > 0
    and s.DevOption = 0
    and o.BrandID = '{dr["MergedBrand"]}'
    and {conditions}
";
            }

            sql = $@"
--先把Season排序號並加上編號
if object_id('tempdb..#probablySeasonList') is not null 
Drop Table #probablySeasonList

if object_id('tempdb..#tmpBrand') is not null 
Drop Table #tmpBrand

SELECT
	RowNo = ROW_NUMBER() OVER (ORDER BY Month)
    ,ID 
INTO #probablySeasonList
FROM(
    Select DISTINCT Month, ID
    From Season 
    where BrandID = @brandID or BrandID in (select MergedBrand From MaterialDocument_Brand Where DocumentName = @documentName and BrandID = @BrandID)
)a

if object_id('tempdb..#tmpMain') is not null 
Drop Table #tmpMain

Select * 
into #tmpMain
From (
	Select *
	, rNo = Row_Number() Over(Partition by SuppID, BrandRefNo, Refno, ColorID  Order by SeasonRow desc)
    FROM (
        {mainSql}
    ) tmp
) main
WHERE rNo = 1

Select RowNo = ROW_NUMBER() OVER (ORDER BY BrandRefno,colorID)
, * 
from(
	SELECT DISTINCT
	    CAST(0 AS BIT) sel
	    ,TestReportTestDate = CONVERT(VARCHAR(10), sr.TestReportTestDate, 23)
	    ,TestReport = CONVERT(VARCHAR(10), sr.TestReport, 23)
        ,m.SuppID 
		,m.Supplier 
        ,Refno = min(Refno)
		,m.BrandRefNo
        ,m.ColorID 
        ,m.ColorDesc 
        ,sr.TestSeasonID
        ,sr.DueSeason
        ,sr.DueDate
        ,sr.AddName
        ,sr.AddDate
        ,sr.EditName
        ,sr.EditDate
        ,sr.Ukey
        ,sr.UniqueKey
        ,[SeasonRow]	
        ,canModify
        ,FactoryID
	FROM #tmpMain m
	 {(this.chkUploadRecord.Checked ? "Inner" : "LEFT")} JOIN UASentReport sr WITH (NOLOCK) on sr.SuppID = m.SuppID  
	 and sr.BrandRefno = m.BrandRefNo 
	 and (@FileRule = 1 or (@FileRule = 2 and sr.ColorID = m.ColorID)) 
	 and sr.BrandID = @BrandID 
	 and sr.DocumentName = @DocumentName
    GROUP BY sr.TestReportTestDate
	    ,sr.TestReport 
        ,m.SuppID   
		,m.Supplier 
		,m.BrandRefNo
        ,m.ColorID 
        ,m.ColorDesc 
        ,sr.TestSeasonID
        ,sr.DueSeason
        ,sr.DueDate
        ,sr.AddName
        ,sr.AddDate
        ,sr.EditName
        ,sr.EditDate
        ,sr.Ukey
        ,sr.UniqueKey
        ,[SeasonRow]	
        ,canModify
        ,FactoryID
)a
  Order By BrandRefNo,ColorID

";

            DataTable dt = new DataTable();
            result = DBProxy.Current.Select(string.Empty, sql, parmes, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.chkUploadRecord.Checked)
            {
                dt = MiscExtensions.TryCopyToDataTable(dt.Select("Ukey > 0"), dt);
            }

            if (this.chkNonValidDoc.Checked)
            {
                dt = MiscExtensions.TryCopyToDataTable(dt.Select("Ukey is null"), dt);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                this.ShowInfo("No Data");
            }

            this.gridBS.DataSource = dt;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridBS.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("no data!");
                return;
            }

            // 8=TestReportTestDate;9=TestSeasonID;10=Due; 如有異動需調整index序
            if (!this.UI_grid.CurrentCell.ColumnIndex.IsOneOfThe(8, 9, 10, 11))
            {
                MyUtility.Msg.WarningBox("Please focus on [Test Date]/[Due Date]/[Test Season]/[Due Season]!", "Error");
                return;
            }

            if (!dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel")).Any())
            {
                MyUtility.Msg.ErrorBox("No data selected!");
                return;
            }

            if (dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && w["UniqueKey"].Empty()).Any())
            {
                MyUtility.Msg.ErrorBox("Please upload files first!");
                return;
            }

            var canModifySupp = dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && !w.Field<bool>("canModify")).Select(row => row["SuppID"].ToString()).Distinct()
                                        .ToList()
                                        .JoinToString(",");
            if (!canModifySupp.Empty())
            {
                MyUtility.Msg.ErrorBox(canModifySupp + " can't modify!");
                return;
            }

            string updatecol = string.Empty;
            string updateData = this.txtUpdate.Text;
            string dueSeason = string.Empty;
            long seasonRow = 0;
            decimal expiration = MyUtility.Convert.GetDecimal(this.drBasic["Expiration"]);
            switch (this.UI_grid.CurrentCell.ColumnIndex)
            {
                case 8:
                    updatecol = "TestReportTestDate  = @updateData";
                    DateTime tm;
                    if (!DateTime.TryParse(updateData, out tm))
                    {
                        MyUtility.Msg.ErrorBox(string.Format(" {0} isn't a valid date value!", updateData), "Error");
                        return;
                    }

                    if (tm > DateTime.Now)
                    {
                        MyUtility.Msg.ErrorBox("[Test Date] can't bigger than Today!", "Error");
                        return;
                    }

                    break;
                case 9:
                    updatecol = "DueDate = @updateData";
                    if (dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && w["TestReportTestDate"].Empty()).Any())
                    {
                        MyUtility.Msg.ErrorBox("[Test Date] can't empty!");
                        return;
                    }

                    DateTime d;
                    if (!DateTime.TryParse(updateData, out d))
                    {
                        MyUtility.Msg.ErrorBox(string.Format("< Due : {0} > isn't a valid date value!", updateData), "Error");
                        return;
                    }

                    if (dt.ExtNotDeletedRows().AsEnumerable().Where(w => MyUtility.Convert.GetDate(w["TestReportTestDate"]) > d).Any())
                    {
                        MyUtility.Msg.ErrorBox("Due Date should bigger than Test Date!", "Error");
                        return;
                    }

                    break;
                case 10:
                    updatecol = "TestSeasonID  = @updateData";
                    string sql = $@"Select ID From Season Where BrandID = '{this.drBasic["BrandID"]}' and ID = @SeasonID";

                    if (DBProxy.Current.SeekEx(sql, "SeasonID", updateData).ExtendedData == null)
                    {
                        MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > not found!", updateData), "Error");
                        return;
                    }

                    if (!this.drBasic["Expiration"].Empty() && expiration > 0)
                    {
                        DataRow dr = DBProxy.Current.SeekEx(
                        $@"
                        SELECT
	                        RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                            ,ID INTO #probablySeasonList
                        FROM dbo.Season Where BrandID = @BrandID

                        SELECT ID,RowNo FROM #probablySeasonList
                        WHERE RowNo = (SELECT RowNo FROM #probablySeasonList WHERE ID =@SeasonID)+{expiration - 1}",
                        "SeasonID",
                        updateData,
                        "BrandID",
                        this.drBasic["BrandID"]).ExtendedData;

                        if (dr != null)
                        {
                            dueSeason = dr["ID"].ToString();
                            seasonRow = MyUtility.Convert.GetLong(dr["RowNo"]);
                        }
                    }

                    break;
                case 11:
                    updatecol = "DueSeason = @updateData";
                    if (dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && w["TestSeasonID"].Empty()).Any())
                    {
                        MyUtility.Msg.ErrorBox("[Test Season] can't empty!");
                        return;
                    }

                    sql = $@"Select ID From Season Where BrandID = '{this.drBasic["BrandID"]}' and ID = @SeasonID";

                    if (DBProxy.Current.SeekEx(sql, "SeasonID", updateData).ExtendedData == null)
                    {
                        MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > not found!", updateData), "Error");
                        return;
                    }

                    seasonRow = DBProxy.Current.LookupEx<long>(
                    $@"
                    SELECT
                    RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                    ,ID INTO #probablySeasonList
                    FROM dbo.Season Where BrandID = @BrandID

                    SELECT RowNo FROM #probablySeasonList
                    WHERE RowNo = (SELECT RowNo FROM #probablySeasonList WHERE ID =@SeasonID)+{expiration - 1}",
                    "SeasonID",
                    updateData,
                    "BrandID",
                    this.drBasic["BrandID"]).ExtendedData;

                    if (dt.ExtNotDeletedRows().AsEnumerable().Where(w => MyUtility.Convert.GetInt(w["SeasonRow"]) > seasonRow).Any())
                    {
                        MyUtility.Msg.ErrorBox("[Due Season] should bigger than [Test Season]!", "Error");
                        return;
                    }

                    break;
            }

            dt.ExtNotDeletedRowsForeach(r =>
            {
                if (MyUtility.Convert.GetBool(r["sel"]))
                {
                    string addUpdate = string.Empty;
                    if (!dueSeason.IsNullOrWhiteSpace())
                    {
                        addUpdate = ",DueSeason = @DueSeason";
                    }

                    string sql = $@"
                    Update 
                    UASentReport
                    SET  {updatecol} {addUpdate},
                    EditName = @UserID ,
                    EditDate = getdate() 
                    WHERE 
                    BrandRefno = @BrandRefno and
                    ColorID = @ColorID and 
                    SuppID = @SuppID and
                    DocumentName = @DocumentName and
                    BrandID = @BrandID";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("BrandRefno", r["BrandRefno"]),
                        new SqlParameter("ColorID", r["ColorID"]),
                        new SqlParameter("SuppID", r["SuppID"]),
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("updateData", updateData),
                        new SqlParameter("DueSeason", dueSeason),
                        new SqlParameter("DocumentName", this.drBasic["DocumentName"]),
                        new SqlParameter("BrandID", this.drBasic["BrandID"]),
                    };

                    DBProxy.Current.Execute(null, sql, plis);

                    switch (this.UI_grid.CurrentCell.ColumnIndex)
                    {
                        case 8:
                            r["TestReportTestDate"] = MyUtility.Convert.GetDate(updateData).ToYYYYMMDD();
                            break;
                        case 9:
                            r["dueDate"] = updateData;
                            break;
                        case 10:
                            r["TestSeasonID"] = updateData;
                            r["dueSeason"] = dueSeason;
                            r["SeasonRow"] = seasonRow;
                            break;
                        case 11:
                            r["dueSeason"] = updateData;
                            break;
                    }

                    r["EditDate"] = DateTime.Now;
                    r["EditName"] = Env.User.UserID;
                }
            });

            dt.AcceptChanges();
        }

        private void BtnFileUpload_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.gridBS.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("no data!");
                return;
            }

            if (!dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel")).Any())
            {
                MyUtility.Msg.ErrorBox("No data selected!");
                return;
            }

            var canModifySupp = dt.ExtNotDeletedRows().AsEnumerable().Where(w => w.Field<bool>("sel") && !w.Field<bool>("canModify")).Select(row => row["SuppID"].ToString()).Distinct()
                                        .ToList()
                                        .JoinToString(",");
            if (!canModifySupp.Empty())
            {
                MyUtility.Msg.ErrorBox(canModifySupp + " can't modify!");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog ofdFileName = ProductionEnv.GetOpenFileDialog();
            ofdFileName.Multiselect = true;
            if (ofdFileName.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(ofdFileName.FileName))
                {
                    this.ShowErr("Import File is not exist");
                    return;
                }
            }
            else
            {
                return;
            }

            string updateData = string.Empty;
            string updateCol = string.Empty;
            string dueSeason = string.Empty;
            string saveFilePath = DBProxy.Current.LookupEx<string>($@"select path From CustomizedClipPath WHERE TableName ='UASentReport'").ExtendedData;
            DateTime now = DateTime.Now;
            saveFilePath = Path.Combine(saveFilePath, now.ToString("yyyyMM"));
            decimal expiration = MyUtility.Convert.GetDecimal(this.drBasic["Expiration"]);
            List<string> pkeys = new List<string>();
            string[] files = ofdFileName.FileNames;
            int count = 0;
            foreach (string file in files)
            {
                string pkey = this.GetPKeyPre() + count.ToString().PadLeft(2, '0');
                string newFileName = "UASentReport" + pkey + Path.GetExtension(ofdFileName.FileName);
                string filenamme = Path.GetFileName(file);
                pkeys.Add(pkey + "-" + filenamme);

                count++;

                // 限制檔案大小
                var fileInfo = new FileInfo(ofdFileName.FileName);
                if (fileInfo.Length > 15 * 1024 * 1024)
                {
                    MyUtility.Msg.WarningBox("File size cannot exceed 15 MB limit!");
                    return;
                }

                // call API上傳檔案到Trade
                lock (FileDownload_UpData.UploadFile($"{PmsWebAPI.PMSAPApiUri}/api/FileUpload/PostFile", saveFilePath, newFileName, ofdFileName.FileName))
                {
                }
            }

            if (!this.txtTestSeason.Text.Empty())
            {
                updateData = this.txtTestSeason.Text;
                updateCol += $",TestSeasonID = '{updateData}',DueSeason = '{this.txtDueSeason.Text}' ";
            }

            if (this.dateTestDate.Value.HasValue)
            {
                updateCol += $",TestReportTestDate = '{this.dateTestDate.Text}'";
                if (this.dateTestDate.Value > DateTime.Now)
                {
                    MyUtility.Msg.ErrorBox("[Test Date] can't bigger than Today!", "Error");
                    return;
                }

                if (this.DueDate.Value.HasValue)
                {
                    updateCol += $",DueDate = '{this.DueDate.Text}'";
                }
            }

            dt.ExtNotDeletedRowsForeach(r =>
            {
                if (MyUtility.Convert.GetBool(r["sel"]))
                {
                    if (!this.txtTestSeason.Text.Empty())
                    {
                        r["TestSeasonID"] = this.txtTestSeason.Text;
                        r["dueSeason"] = this.txtDueSeason.Text;
                    }

                    if (this.dateTestDate.Value.HasValue)
                    {
                        r["TestReportTestDate"] = MyUtility.Convert.GetDate(this.dateTestDate.Value).ToYYYYMMDD();
                        if (this.DueDate.Value.HasValue)
                        {
                            r["dueDate"] = MyUtility.Convert.GetDate(this.DueDate.Value).ToYYYYMMDD();
                        }
                    }

                    if (r["UniqueKey"].Empty())
                    {
                        r["AddName"] = Env.User.UserID;
                        r["AddDate"] = now;
                    }
                    else
                    {
                        r["EditName"] = Env.User.UserID;
                        r["EditDate"] = now;
                    }

                    r["TestReport"] = MyUtility.Convert.GetDate(now).ToYYYYMMDD();

                    string addUpdate = string.Empty;

                    string sql = $@"


                    DECLARE @OutputTbl TABLE (ID bigint)
                    IF EXISTS(select 1 FROM dbo.UASentReport WHERE BrandRefno = '{r["BrandRefno"]}' and ColorID = '{r["ColorID"]}' and SuppID = '{r["SuppID"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}')
                    Begin
                        Update dbo.UASentReport SET  TestReport = getdate(), EditName = '{Env.User.UserID}' ,EditDate = getdate() {updateCol} 
                        output inserted.Ukey into @OutputTbl
                        WHERE BrandRefno = '{r["BrandRefno"]}' and ColorID = '{r["ColorID"]}' and SuppID = '{r["SuppID"]}' and DocumentName = '{this.drBasic["DocumentName"]}' and BrandID = '{this.drBasic["BrandID"]}'
                    End
                    Else
                     Begin
                        Insert Into dbo.[UASentReport]
                         (
                           [BrandRefno]
                          ,[ColorID]
                          ,[SuppID]
                          ,[TestReport]
                          ,[TestReportTestDate]
                          ,[AddDate]
                          ,[AddName]
                          ,[DocumentName]
                          ,[BrandID]
                          ,[TestSeasonID]
                          ,[DueSeason]
                          ,[DueDate]
                          ,[UniqueKey]
                        )
                        output inserted.Ukey into @OutputTbl
                        Values(
                          '{r["BrandRefno"]}'
                         ,'{r["ColorID"]}'
                         ,'{r["SuppID"]}'
                         ,getdate()
                         ,{(!this.dateTestDate.Value.HasValue ? "null" : $"'{this.dateTestDate.Text}'")}
                         ,getdate()
                         ,'{Env.User.UserID}'
                         ,'{this.drBasic["DocumentName"]}'
                         ,'{this.drBasic["BrandID"]}'
                         ,'{this.txtTestSeason.Text}'
                         ,'{this.txtDueSeason.Text}'
                         ,{(!this.DueDate.Value.HasValue ? "null" : $"'{this.DueDate.Text}'")}
                         ,'{r["BrandRefno"]}'+'_'+'{r["ColorID"]}'+'_'+'{r["SuppID"]}'+'_'+ '{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}'
                        )
                     End

                    INSERT INTO GASAClip (
                       [PKey]
                      ,[TableName]
                      ,[UniqueKey]
                      ,[SourceFile]
                      ,[Description]
                      ,[AddName]
                      ,[AddDate]
                      ,[FactoryID])
                    SELECT files.Pkey, 'UASentReport', '{r["BrandRefno"]}'+'_'+'{r["ColorID"]}'+'_'+'{r["SuppID"]}'+'_'+ '{this.drBasic["DocumentName"]}'+'_'+'{this.drBasic["BrandID"]}', files.FileName, 'File Upload', @UserID, getdate(), '{r["FactoryID"]}'
                     FROM @OutputTbl
                    Outer Apply(
                        select [Pkey] = SUBSTRING(Data,0,11),FileName = SUBSTRING(Data,12,len(Data)-11) from splitstring(@ClipPkey,'?')
                    )files

                    select UniqueKey from UASentReport
                    where ukey in (SELECT TOP 1 ID FROM @OutputTbl)
                    ";
                    List<SqlParameter> plis = new List<SqlParameter>()
                    {
                        new SqlParameter("UserID", Env.User.UserID),
                        new SqlParameter("ClipPkey", string.Join("?", pkeys)),
                        new SqlParameter("FileName", ofdFileName.SafeFileName),
                    };
                    DataTable dtUkey = new DataTable();
                    var result = DBProxy.Current.Select(null, sql, plis, out dtUkey);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    r["UniqueKey"] = dtUkey.Rows[0][0];
                }
            });

            dt.AcceptChanges();
            MyUtility.Msg.InfoBox("Update Success!");
        }

        /// <summary>
        /// Get PK string without prefix.
        /// </summary>
        /// <returns>string without prefix.</returns>
        private string GetPKeyPre()
        {
            var dtm_sys = DateTime.Now;

            string pkey = string.Empty;
            pkey += CHARs[dtm_sys.Year % 100].ToString() + CHARs[dtm_sys.Month].ToString() + CHARs[dtm_sys.Day].ToString();
            pkey += CHARs[dtm_sys.Hour].ToString();
            pkey += CHARs[dtm_sys.Minute / CHARs.Length].ToString() + CHARs[dtm_sys.Minute % CHARs.Length].ToString();
            pkey += CHARs[dtm_sys.Second / CHARs.Length].ToString() + CHARs[dtm_sys.Second % CHARs.Length].ToString();

            return pkey;
        }

        private void DateTestDate_Validated(object sender, EventArgs e)
        {
            if (this.dateTestDate.Value.HasValue && this.DueDate.Value.HasValue)
            {
                if (this.DueDate.Value < this.dateTestDate.Value)
                {
                    MyUtility.Msg.ErrorBox("Due Date should bigger than Test Date!", "Error");
                    this.DueDate.Value = null;
                    return;
                }
            }
            else if (!this.dateTestDate.Value.HasValue)
            {
                MyUtility.Msg.ErrorBox("Please input [Test Date] first!", "Error");
                this.DueDate.Value = null;
                return;
            }
        }

        private void DueDate_Validated(object sender, EventArgs e)
        {
            if (this.dateTestDate.Value.HasValue && this.DueDate.Value.HasValue)
            {
                if (this.DueDate.Value < this.dateTestDate.Value)
                {
                    MyUtility.Msg.ErrorBox("Due Date should bigger than Test Date!", "Error");
                    this.DueDate.Value = null;
                    return;
                }
            }
        }

        private void TxtDueSeason_Validated(object sender, EventArgs e)
        {
            if (!this.txtDueSeason.Text.Empty() && !this.txtTestSeason.Text.Empty())
            {
                string sql = $@"
                SELECT
	                RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                   ,ID INTO #probablySeasonList
                FROM dbo.Season Where BrandID = @BrandID

                SELECT main.ID FROM #probablySeasonList main
                Outer Apply (
                  SELECT RowNo FROM #probablySeasonList WHERE ID = @SeasonID
                )chkSeason
                WHERE main.ID = @DueSeason and main.RowNo >= chkSeason.RowNo";
                if (DBProxy.Current.SeekEx(sql, "DueSeason", this.txtDueSeason.Text, "SeasonID", this.txtTestSeason.Text, "BrandID", this.txtBrand1.Text).ExtendedData == null)
                {
                    MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > can't less than {1}!", this.txtDueSeason.Text, this.txtTestSeason.Text), "Error");
                    this.txtDueSeason.Text = string.Empty;
                    return;
                }
            }
            else if (this.txtTestSeason.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("Please input Season first!", "Error");
                this.txtDueSeason.Text = string.Empty;
                return;
            }
        }

        private void TxtTestSeason_Validated(object sender, EventArgs e)
        {
            if (!this.txtTestSeason.Text.Empty() && this.drBasic != null)
            {
                decimal expiration = MyUtility.Convert.GetDecimal(this.drBasic["Expiration"]);
                if (!this.drBasic["Expiration"].Empty() && expiration > 0)
                {
                    DataRow dr = DBProxy.Current.SeekEx(
                    $@"
                    SELECT
	                    RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                       ,ID INTO #probablySeasonList
                    FROM dbo.Season Where BrandID = @BrandID

                    SELECT ID,RowNo FROM #probablySeasonList
                    WHERE RowNo = (SELECT RowNo FROM #probablySeasonList WHERE ID =@SeasonID)+{expiration - 1}",
                    "SeasonID",
                    this.txtTestSeason.Text,
                    "BrandID",
                    this.drBasic["BrandID"]).ExtendedData;

                    if (dr != null)
                    {
                        this.txtDueSeason.Text = dr["ID"].ToString();
                    }
                }
            }

            if (!this.txtDueSeason.Text.Empty() && !this.txtTestSeason.Text.Empty())
            {
                string sql = $@"
                SELECT
	                RowNo = ROW_NUMBER() OVER (ORDER BY Month)
                   ,ID INTO #probablySeasonList
                FROM dbo.Season Where BrandID = @BrandID

                SELECT main.ID FROM #probablySeasonList main
                Outer Apply (
                  SELECT RowNo FROM #probablySeasonList WHERE ID = @SeasonID
                )chkSeason
                WHERE main.ID = @DueSeason and main.RowNo >= chkSeason.RowNo";
                if (DBProxy.Current.SeekEx(sql, "DueSeason", this.txtDueSeason.Text, "SeasonID", this.txtTestSeason.Text, "BrandID", this.txtBrand1.Text).ExtendedData == null)
                {
                    MyUtility.Msg.ErrorBox(string.Format("< Season : {0} > can't less than {1}!", this.txtDueSeason.Text, this.txtTestSeason.Text), "Error");
                    this.txtDueSeason.Text = string.Empty;
                    return;
                }
            }
        }

        private void CboDocumentname_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> parmes = new List<SqlParameter>();
            parmes.Add(new SqlParameter("@Documentname", this.cboDocumentname.Text));
            string sql = "select BrandID From MaterialDocument WHERE Documentname = @Documentname and FileRule in ('1','2') and junk = 0";
            var result = DBProxy.Current.Select(string.Empty, sql, parmes, out dt);
            if (result && dt.Rows.Count == 1)
            {
                this.txtBrand1.Text = dt.Rows[0]["BrandID"].ToString();
            }
            else
            {
                this.txtBrand1.Text = string.Empty;
            }
        }
    }
}
