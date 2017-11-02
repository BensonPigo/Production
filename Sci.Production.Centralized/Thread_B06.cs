using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Centralized
{ 
    public partial class Thread_B06 : Sci.Win.Tems.QueryForm
    {
        public Thread_B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            resetGridData();
            this.grid.IsEditingReadOnly = true;
            this.btnAppend.Visible = false;
            this.btnDelete.Visible = false;
            this.btnSave.Visible = false;
            this.btnUndo.Visible = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Allowance = null;

            Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "Seq", iseditingreadonly: true)
                .Numeric("LowerBound", header: "LowerBound", iseditingreadonly: false)
                .Numeric("UpperBound", header: "UpperBound", iseditingreadonly: false)
                .Numeric("Allowance", header: "Allowance", decimal_places: 2, iseditingreadonly: false).Get(out col_Allowance)
                .EditText("Remark", header: "Remark", iseditingreadonly: false);

            col_Allowance.DecimalZeroize = Ict.Win.UI.NumericBoxDecimalZeroize.Default;
            col_Allowance.Maximum = (decimal)999.99;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            DataRow newRow = ((DataTable)bindingSource1.DataSource).NewRow();
            int newID;
            if (((DataTable)bindingSource1.DataSource).Rows.Count == 0)
                newID = 0;
            else
                newID = Convert.ToInt32(((DataTable)bindingSource1.DataSource).Compute("Max(ID)", string.Empty));

            if (newID >= 999)
                MyUtility.Msg.WarningBox("ID can't more then 999.");
            else
            {
                newRow["ID"] = (newID + 1).ToString("000");
                ((DataTable)bindingSource1.DataSource).Rows.Add(newRow);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable gridDt = ((DataTable)bindingSource1.DataSource);
            if (gridDt != null && gridDt.Rows.Count > 0)
            {
                gridDt.Rows.RemoveAt(this.grid.GetSelectedRowIndex());

                int index = 1;
                foreach (DataRow dr in gridDt.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dr["ID"] = (index++).ToString("000");
                        dr.EndEdit();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region Save Before
            #region 刪除 LowerBound、UpperBound、Allowance、Remark 皆為空的項目
            if (((DataTable)bindingSource1.DataSource).AsEnumerable().Any(row => row["LowerBound"].Empty() == false
                                                                                 || row["UpperBound"].Empty() == false
                                                                                 || row["Allowance"].Empty() == false
                                                                                 || row["Remark"].Empty() == false))
            {
                bindingSource1.DataSource = ((DataTable)bindingSource1.DataSource).AsEnumerable().Where(row => row["LowerBound"].Empty() == false
                                                                                                               || row["UpperBound"].Empty() == false
                                                                                                               || row["Allowance"].Empty() == false
                                                                                                               || row["Remark"].Empty() == false
                                                                                                        ).CopyToDataTable();
            }
            else
            {
                bindingSource1.DataSource = ((DataTable)bindingSource1.DataSource).Clone();
            }
            #endregion
            #region 若 LowerBound、UpperBound、Allowance 若有任一筆資料任一值為空，則不能存檔
            if (((DataTable)bindingSource1.DataSource).AsEnumerable().Any(row => row["LowerBound"].Empty()
                                                                                 || row["UpperBound"].Empty()
                                                                                 || row["Allowance"].EqualString("")))
            {
                var errDt = ((DataTable)bindingSource1.DataSource).AsEnumerable().Where(row => row["LowerBound"].Empty()
                                                                                               || row["UpperBound"].Empty()
                                                                                               || row["Allowance"].EqualString(""));
                StringBuilder errStr = new StringBuilder();
                foreach (DataRow errDr in errDt)
                {
                    errStr.Append($"Seq:{errDr["ID"]}, LowerBound、UpperBound、Allowanc can't be empty.{Environment.NewLine}");
                }
                MyUtility.Msg.WarningBox(errStr.ToString());
                return;
            }
            #endregion

            if (((DataTable)bindingSource1.DataSource).Rows.Count > 0)
            {
                #region 第一筆資料 LowerBound 一定要為 1
                if (((DataTable)bindingSource1.DataSource).Rows[0]["LowerBound"].EqualDecimal(1) == false)
                {
                    MyUtility.Msg.WarningBox("Seq:001, LowerBound must be 1.");
                    return;
                }
                #endregion
                #region 檢查所有資料列 LowerBound 一定要小於 UpperBound
                if (((DataTable)bindingSource1.DataSource).AsEnumerable().Any(row => Convert.ToInt32(row["LowerBound"]) >= Convert.ToInt32(row["UpperBound"])))
                {
                    var errDt = ((DataTable)bindingSource1.DataSource).AsEnumerable().Where(row => Convert.ToInt32(row["LowerBound"]) >= Convert.ToInt32(row["UpperBound"]));
                    StringBuilder errStr = new StringBuilder();
                    foreach (DataRow errDr in errDt)
                    {
                        errStr.Append($"Seq:{errDr["ID"]}, LowerBound must less than UpperBound.{Environment.NewLine}");
                    }
                    MyUtility.Msg.WarningBox(errStr.ToString());
                    return;
                }
                #endregion
                #region UpperBound一定要小於下一行資料的LowerBound
                if (((DataTable)bindingSource1.DataSource).Rows.Count > 1)
                {
                    List<int> errList = new List<int>();
                    for (int i = 0; i < ((DataTable)bindingSource1.DataSource).Rows.Count - 1; i++)
                    {
                        if (Convert.ToInt32(((DataTable)bindingSource1.DataSource).Rows[i]["UpperBound"]) >= Convert.ToInt32(((DataTable)bindingSource1.DataSource).Rows[i + 1]["LowerBound"]))
                        {
                            errList.Add(i);
                        }
                    }

                    if (errList.Count > 0)
                    {
                        StringBuilder errStr = new StringBuilder();
                        foreach (int errDr in errList)
                        {
                            errStr.Append($"Seq:{((DataTable)bindingSource1.DataSource).Rows[errDr]["ID"]}, UpperBound must less than Seq：{((DataTable)bindingSource1.DataSource).Rows[errDr + 1]["ID"]} LowerBound.{Environment.NewLine}");
                        }
                        MyUtility.Msg.WarningBox(errStr.ToString());
                        return;
                    }
                }
                #endregion
                #region UpperBound + 1 一定要等於下一行資料的 LowerBound
                if (((DataTable)bindingSource1.DataSource).Rows.Count > 1)
                {
                    List<int> errList = new List<int>();
                    for (int i = 0; i < ((DataTable)bindingSource1.DataSource).Rows.Count - 1; i++)
                    {
                        if (Convert.ToInt32(((DataTable)bindingSource1.DataSource).Rows[i]["UpperBound"]) + 1 != Convert.ToInt32(((DataTable)bindingSource1.DataSource).Rows[i + 1]["LowerBound"]))
                        {
                            errList.Add(i);
                        }
                    }

                    if (errList.Count > 0)
                    {
                        StringBuilder errStr = new StringBuilder();
                        foreach (int errDr in errList)
                        {
                            errStr.Append($"Seq:{((DataTable)bindingSource1.DataSource).Rows[errDr]["ID"]}, UpperBound plus 1 must be equal to Seq：{((DataTable)bindingSource1.DataSource).Rows[errDr + 1]["ID"]} LowerBound.{Environment.NewLine}");
                        }
                        MyUtility.Msg.WarningBox(errStr.ToString() + "otherwise there will be undefined digits between two seq.");
                        return;
                    }
                }
                #endregion
            }
            #endregion
            #region Save
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@UserName", Sci.Env.User.UserName));

            string strUpdateTable = @"
delete tas
from ThreadAllowanceScale tas
where not exists (select *
				  from #tmp
				  where tas.ID = #tmp.ID)

merge ThreadAllowanceScale as t
using #tmp as s
on t.ID = s.ID
when matched then 
	update set 
		t.LowerBound = s.LowerBound
		, t.UpperBound = s.UpperBound
		, t.Allowance = s.Allowance
		, t.Remark = s.Remark
		, t.EditName = @UserName
		, t.EditDate = GetDate()
when not matched then 
	insert (
		ID         , LowerBound  , UpperBound  , Allowance  , Remark
		, AddName  , AddDate
	) values (
		s.ID       , s.LowerBound, s.UpperBound, s.Allowance, s.Remark
		, @UserName, GetDate()
	);";
            DataTable dtResult;
            SqlConnection sqlConn = new SqlConnection();
            DBProxy.Current.OpenConnection(this.ConnectionName, out sqlConn);
            DualResult result = MyUtility.Tool.ProcessWithDatatable(((DataTable)bindingSource1.DataSource)
                                                                        , null
                                                                        , strUpdateTable
                                                                        , out dtResult
                                                                        , paramters: listSqlParameter
                                                                        , conn: sqlConn);
                if (result == false)
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }
            #endregion
            #region Save After
            this.grid.IsEditingReadOnly = true;
            this.btnAppend.Visible = false;
            this.btnDelete.Visible = false;
            this.btnSave.Visible = false;
            this.btnUndo.Visible = false;
            this.btnEdit.Visible = true;
            this.btnClose.Visible = true;
            resetGridData();
            #endregion
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            resetGridData();
            #region Undo After
            this.grid.IsEditingReadOnly = true;
            this.btnAppend.Visible = false;
            this.btnDelete.Visible = false;
            this.btnSave.Visible = false;
            this.btnUndo.Visible = false;
            this.btnEdit.Visible = true;
            this.btnClose.Visible = true;
            #endregion 
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.grid.IsEditingReadOnly = false;
            this.btnAppend.Visible = true;
            this.btnDelete.Visible = true;
            this.btnSave.Visible = true;
            this.btnUndo.Visible = true;
            this.btnEdit.Visible = false;
            this.btnClose.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetGridData()
        {
            DataTable gridDt;
            DualResult result = DBProxy.Current.Select(this.ConnectionName, "select * from ThreadAllowanceScale", out gridDt);
            if (result)
            {
                this.bindingSource1.DataSource = gridDt;
            }
            else
            {
                MyUtility.Msg.ErrorBox(result.ToString());
            }
        }
    }
}
