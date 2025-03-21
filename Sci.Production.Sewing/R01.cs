using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Prg;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R01
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        private DateTime? _date;
        private string _factory;
        private string _team;
        private string _factoryName;
        private DataTable _printData;
        private DataTable _ttlData;
        private DataTable _subprocessData;
        private List<APIData> dataMode = new List<APIData>();

        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.dateDate.Value = DateTime.Today.AddDays(-1);
            this.comboFactory.SetDataSource();
            this.comboSewingTeam1.SetDataSource();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (this.comboFactory.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Factory can't empty!!");
                return false;
            }

            string errMsg = string.Empty;
            string sql = string.Format(
            @"select OutputDate
	                ,FactoryID
	                ,SewingLineID
	                ,Team
	                ,Shift
	                ,SubconOutFty 
	                ,SubConOutContractNumber 
                from SewingOutput
                where 1=1
                    and OutputDate = cast('{0}' as date)
                    and Status in('','NEW')
                    and FactoryID = '{1}'
            ",
            Convert.ToDateTime(this.dateDate.Value).ToString("yyyy/MM/dd"),
            this.comboFactory.Text);
            DualResult result = DBProxy.Current.Select(null, sql, out DataTable dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail\r\n" + result.ToString());
                return false;
            }

            foreach (DataRow dr in dt.Rows)
            {
                errMsg += MyUtility.Check.Empty(errMsg) ? "Please lock data first! \r\n" : "\r\n";
                errMsg += string.Format(
                    "Date:{0},Factory: {1}, Line#: {2}, Team:{3}, Shift:{4}, SubconOut-Fty:{5}, SubconOut_Contract#:{6}.",
                    Convert.ToDateTime(dr["OutputDate"].ToString()).ToString("yyyy/MM/dd"),
                    dr["FactoryID"],
                    dr["SewingLineID"],
                    dr["Team"],
                    dr["Shift"],
                    dr["SubconOutFty"],
                    dr["SubConOutContractNumber"]);
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                MyUtility.Msg.WarningBox(errMsg);
                return false;
            }

            this._date = this.dateDate.Value;
            this._factory = this.comboFactory.Text;
            this._team = this.comboSewingTeam1.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DataTable[] dtR01Array;

            List<SqlParameter> listParR01 = new List<SqlParameter>()
            {
                new SqlParameter("@Factory", Env.User.Factory),
                new SqlParameter("@OutputDate",  Convert.ToDateTime(this._date).ToString("yyyy/MM/dd")),
                new SqlParameter("@Team", this._team),
            };
            string sqlGetSewing_R01List = @"exec GetSewing_R01  @Factory
	                                            ,@OutputDate
                                                ,@Team";

            DualResult result = DBProxy.Current.Select(null, sqlGetSewing_R01List, listParR01, out dtR01Array);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this._printData = dtR01Array[0];
            this._ttlData = dtR01Array[1];
            this._subprocessData = dtR01Array[2];

            this._factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", this._factory));
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData.Rows.Count);

            if (this._printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string excelName = string.Empty;
            R01_ToExcel.ToExcel(this._factoryName, this._factory, this._date, (DateTime)this.dateDate.Value, this._printData, this._ttlData, this._subprocessData, this.dataMode, "Sewing_R01_DailyCMPReport", ref excelName);
            this.HideWaitMessage();
            return true;
        }
    }
}