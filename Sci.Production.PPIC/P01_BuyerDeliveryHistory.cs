using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ict.Win;
using Sci.Data;
using Ict;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P01_BuyerDeliveryHistory
    /// </summary>
    public partial class P01_BuyerDeliveryHistory : Win.Tems.QueryForm
    {
        private string _ModuleName;
        private string _TableName;
        private string _HisType;
        private string _SourceID;
        private string _SourceIDRange;

        /// <summary>
        /// P01_BuyerDeliveryHistory
        /// </summary>
        /// <param name="moduleName">ModuleName</param>
        /// <param name="tableName">TableName</param>
        /// <param name="hisType">HisType</param>
        /// <param name="sourceID">SourceID</param>
        /// <param name="sourceIDRange">SourceIDRange</param>
        public P01_BuyerDeliveryHistory(string moduleName, string tableName, string hisType, string sourceID, string sourceIDRange = "")
        {
            this.InitializeComponent();

            this._ModuleName = moduleName;
            this._TableName = tableName;
            this._HisType = hisType;
            this._SourceID = sourceID;
            this._SourceIDRange = sourceIDRange;
            this.Text = this.Text + $"({this._SourceID})";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("SourceID", header: "ID", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("NewValue", header: "New Value", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("OldValue", header: "Old Value", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("HisType", header: "Status", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(28), iseditingreadonly: true)
                .Text("ReasonID", header: "Reason ID", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Text("ResName", header: "Reason", width: Widths.AnsiChars(22), iseditingreadonly: true)
                .Text("editby", header: "Edit By", width: Widths.AnsiChars(22), iseditingreadonly: true)
                ;

            DataTable dt;
            string cmd = @"
select SourceID
       , HisType
	   , NewValue
	   , OldValue
	   , a.Remark
	   , a.ReasonID
	   , ResName = b.Name
	   , editby = dbo.getTPEPass1(a.AddName) + ' ' + CONVERT(CHAR(20), a.AddDate, 120) 
from TradeHIS_@Module@ a
left join Reason b on a.ReasonID = b.ID 
								and a.ReasonTypeID = b.ReasonTypeID
where TableName = @TableName";
            cmd = cmd.Replace("@Module@", this._ModuleName);
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@TableName", this._TableName));
            if (this._HisType != string.Empty)
            {
                cmd += " and HisType = @HisType";
                lis.Add(new SqlParameter("@HisType", this._HisType));
            }

            if (this._SourceID != string.Empty)
            {
                cmd += " and SourceID = @ID";
                lis.Add(new SqlParameter("@ID", this._SourceID));
            }

            if (this._SourceIDRange != string.Empty)
            {
                cmd += string.Format(" and SourceID in ( {0} )", this._SourceIDRange);
            }

            cmd += " order by a.AddDate ASC";
            DualResult res = DBProxy.Current.Select(string.Empty, cmd, lis, out dt);
            if (res)
            {
                if (this._HisType == "OrdersBuyerDelivery" || this._HisType == "OrdersSciDelivery")
                {
                    this.grid.Columns["HisType"].Visible = false;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["NewValue"].ToString() != string.Empty)
                        {
                            dt.Rows[i]["NewValue"] = Convert.ToDateTime(dt.Rows[i]["NewValue"].ToString()).ToShortDateString();
                        }

                        if (dt.Rows[i]["OldValue"].ToString() != string.Empty)
                        {
                            dt.Rows[i]["OldValue"] = Convert.ToDateTime(dt.Rows[i]["OldValue"].ToString()).ToShortDateString();
                        }
                    }
                }

                // Purchase P13
                if (this._TableName == "ShipPlan")
                {
                    this.grid.Columns["NewValue"].Visible = false;
                    this.grid.Columns["OldValue"].Visible = false;
                }

                this.grid_BS.DataSource = dt;
                this.grid.ColumnsAutoSize();
            }
        }
    }
}
