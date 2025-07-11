using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using static Sci.Production.Cutting.CuttingWorkOrder;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// Cutting P02、P09匯自動生成Seq 和 CutNo的表單
    /// </summary>
    public partial class AutoSeq_CutNo : Sci.Win.Tems.QueryForm
    {
        private IList<DataRow> DetailDatas;
        private DataTable dtSource;
        private CuttingForm _Form; // Cutting Form
        private string _functionCode; // Default function code

        /// <summary>
        /// Cutting P02、P09匯自動生成Seq 和 CutNo的表單
        /// </summary>
        /// <param name="form">P02 / P09</param>
        /// <param name="detailData"> detail Data</param>
        /// /// <param name="dtDataSource"> dtDataSource</param>
        public AutoSeq_CutNo(CuttingForm form, IList<DataRow> detailData, DataTable dtDataSource)
        {
            this.InitializeComponent();
            this.Text = form == CuttingForm.P02 ? "P02 Auto Seq By" : "P09 Auto CutNo";
            this._Form = form;
            this._functionCode = form == CuttingForm.P02 ? "Cutting.P02" : "Cutting.P09";
            this.DetailDatas = detailData;
            this.dtSource = dtDataSource;
            this.EditMode = true;
        }

        /// <inheritdoc />
        protected override void OnFormLoaded()
        {
            // Grid setup
            this.ui_grid.IsEditingReadOnly = false;
            if (this._Form == CuttingForm.P02)
            {
                this.Helper.Controls.Grid.Generator(this.ui_grid)
                .CheckBox("IsSelect", header: string.Empty, width: Widths.AnsiChars(8), iseditable: true, trueValue: true, falseValue: false)
                .Text("SpecColumn", header: "Auto Seq By", width: Widths.AnsiChars(32), iseditingreadonly: true);
            }
            else
            {
                this.Helper.Controls.Grid.Generator(this.ui_grid)
                .CheckBox("IsSelect", header: string.Empty, width: Widths.AnsiChars(8), iseditable: true, trueValue: true, falseValue: false)
                .Text("SpecColumn", header: "Auto CutNo By", width: Widths.AnsiChars(32), iseditingreadonly: true);
            }

            var gridDataSource = new DataTable();
            this.ui_grid.DataSource = gridDataSource;

            // Query data
            this.Query();
        }

        private void Query()
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@empid", Env.User.UserID),
                new SqlParameter("@factoryID", Env.User.Factory),
                new SqlParameter("@functionCode", this._functionCode),
            };

            string sqlcmd = @"
if not exists (select 1 from Userdata 
    where empid = @empid and factoryID = @factoryID and functionCode = @functionCode
    and SpecColumn in ('IsPatternPanel','IsFabricCombo')
)
begin 
    Select [IsSelect] = 1 ,[SpecColumn] = 'Pattern Panel',[SpecColumn_Ori] = 'IsPatternPanel'
    union all
    Select [IsSelect] = 0 ,[SpecColumn] = 'Fabric Combo',[SpecColumn_Ori] = 'IsFabricCombo'
end
else
begin
     Select [IsSelect] = case when SpecValue = 1 then 1
     when SpecValue = 0 then 0
     else 0 end
     ,[SpecColumn] = case 	   
	    when SpecColumn = 'IsPatternPanel' then 'Pattern Panel'
	    when SpecColumn = 'IsFabricCombo' then 'Fabric Combo'
	    else '' end
    ,[SpecColumn_Ori] = SpecColumn
    from Userdata 
    where empid = @empid and factoryID = @factoryID and functionCode = @functionCode
    and SpecColumn in ('IsPatternPanel','IsFabricCombo')
end
";
            DualResult result = null;
            if (!(result = DBProxy.Current.Select(string.Empty, sqlcmd, parameters, out DataTable dt)))
            {
                this.ShowErr(result);
                return;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                this.ShowErr("Data not found!");
                return;
            }

            this.ui_grid.DataSource = dt;
        }

        private void Ui_btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnAutoSeq_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.ui_grid.DataSource;
            if (!dt.AsEnumerable().Any())
            {
                return;
            }

            DataRow[] selectedRows = dt.Select("IsSelect = 1");
            if (selectedRows.Length <= 0)
            {
                this.ShowErr("Please select at least one option to proceed.");
                return;
            }

            int maxSeq;
            int maxCutNo;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (this._Form == CuttingForm.P02)
                {
                    if (MyUtility.Check.Empty(dr["Seq"]) && !MyUtility.Check.Empty(dr["estcutdate"]))
                    {
                        DataTable wk = this.dtSource;
                        string pattern = dr["PatternPanel_CONCAT"].ToString();

                        // 動態組條件
                        List<string> conditions = new List<string>();
                        foreach (var item in selectedRows)
                        {
                            string column = item["SpecColumn"].ToString();
                            switch (column)
                            {
                                case "Pattern Panel":
                                    conditions.Add(
                                        $"(PatternPanel_CONCAT = '{pattern}' OR ('{pattern}' IN ('FA+FC','FC+FA') AND PatternPanel_CONCAT IN ('FA+FC','FC+FA')))");
                                    break;
                                case "Fabric Combo":
                                    conditions.Add($"FabricCombo = '{dr["FabricCombo"]}'");
                                    break;
                            }
                        }

                        string strFilter = string.Join(" AND ", conditions);
                        string maxValue = wk.Compute("Max(Seq)", strFilter)?.ToString();

                        if (int.TryParse(maxValue, out int maxNo))
                        {
                            maxSeq = maxNo + 1;
                        }
                        else
                        {
                            maxSeq = 1;
                        }

                        dr["Seq"] = maxSeq;
                    }
                }
                else
                {
                    if (MyUtility.Check.Empty(dr["CutNo"]) && !MyUtility.Check.Empty(dr["estcutdate"]))
                    {
                        DataTable wk = this.dtSource;

                        // 動態組條件
                        List<string> conditions = new List<string>();
                        foreach (var item in selectedRows)
                        {
                            string column = item["SpecColumn"].ToString();
                            switch (column)
                            {
                                case "Pattern Panel":
                                    conditions.Add(
                                        $"PatternPanel_CONCAT = '{dr["PatternPanel_CONCAT"]}'");
                                    break;
                                case "Fabric Combo":
                                    conditions.Add($"FabricCombo = '{dr["FabricCombo"]}'");
                                    break;
                            }
                        }

                        string strFilter = string.Join(" AND ", conditions);
                        string maxValue = wk.Compute("Max(CutNo)", strFilter)?.ToString();

                        if (int.TryParse(maxValue, out int maxNo))
                        {
                            maxCutNo = maxNo + 1;
                        }
                        else
                        {
                            maxCutNo = 1;
                        }

                        dr["CutNo"] = maxCutNo;
                    }
                }
            }

            // Save Userdata
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@empid", Env.User.UserID),
                        new SqlParameter("@factoryID", Env.User.Factory),
                        new SqlParameter("@functionCode", this._functionCode),
                    };
                    string sqlUpd = $@"
if not exists (
	select 1 from Userdata 
	where empid = @empid and factoryID = @factoryID and functionCode = @functionCode
	and SpecColumn in ('IsPatternPanel','IsFabricCombo')
)
begin
	insert into UserData([EmpID],[FactoryID],[FunctionCode],[SpecColumn],[SpecValue],[Editdate])
	select @empid, @factoryID, @functionCode,[SpecColumn_Ori],[IsSelect],GETDATE()
	from #tmp	
end
else
begin
	update t
	set t.SpecValue = case 
			when s.IsSelect = 1 then 1
			when s.IsSelect = 0 then 0
			else s.IsSelect end
	from Userdata t
	inner join #tmp s on s.SpecColumn_Ori = t.SpecColumn
	where empid = @empid and factoryID = @factoryID and functionCode = @functionCode
	and t.SpecColumn in ('IsPatternPanel','IsFabricCombo') 
end
";
                    DualResult upResult;
                    if (!(upResult = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, sqlUpd, out DataTable dtResult, paramters: parameters)))
                    {
                        scope.Dispose();
                        this.ShowErr(upResult);
                        return;
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex.Message);
                    return;
                }
            }

            this.Close();
        }
    }
}
