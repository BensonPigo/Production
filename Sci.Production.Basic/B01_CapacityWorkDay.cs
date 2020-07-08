using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B01_CapacityWorkDay
    /// </summary>
    public partial class B01_CapacityWorkDay : Win.Subs.Base
    {
        private DataRow motherData;

        /// <summary>
        /// B01_CapacityWorkDay
        /// </summary>
        /// <param name="data"> data </param>
        public B01_CapacityWorkDay(DataRow data)
        {
            this.InitializeComponent();
            this.motherData = data;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            // 呼叫撈Grid資料Method
            this.SelectGridData();

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.motherData["ID"].ToString();
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            // 設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                 .Text("Year", header: "Year", width: Widths.AnsiChars(4))
                 .Text("Month", header: "Month", width: Widths.AnsiChars(2))
                 .Text("Artwork", header: "Artwork", width: Widths.AnsiChars(30))
                 .Numeric("TMS", header: "GSD", width: Widths.AnsiChars(8));

            // 設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.grid2)
                 .Text("Year", header: "Year", width: Widths.AnsiChars(4))
                 .Text("Month", header: "Month", width: Widths.AnsiChars(2))
                 .Numeric("HalfMonth1", header: "1st half month work day", width: Widths.AnsiChars(3))
                 .Numeric("HalfMonth2", header: "2nd half month work day", width: Widths.AnsiChars(3));

            // 設定ComboBox顯示資料
            // comboBox1
            string selectCommand = "select distinct Year from Factory_TMS WITH (NOLOCK) where ID = @id";

            DualResult returnResult;
            DataTable yearTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out yearTable))
            {
                MyUtility.Tool.SetupCombox(this.comboCapacityYear, 1, yearTable);
            }

            // comboBox2
            selectCommand = "select distinct ArtworkTypeID from Factory_TMS WITH (NOLOCK) where ID = @id";
            DataTable artworkTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out artworkTable))
            {
                MyUtility.Tool.SetupCombox(this.comboArtwork, 1, artworkTable);
            }

            // comboBox3
            selectCommand = "select distinct Year from Factory_WorkHour WITH (NOLOCK) where ID = @id";
            DataTable yearTable2 = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out yearTable2))
            {
                MyUtility.Tool.SetupCombox(this.comboWorkdayYear, 1, yearTable2);
            }

            // 設定ComboBox預設值
            this.comboCapacityYear.SelectedValue = DateTime.Now.Year.ToString();
            this.comboArtwork.SelectedValue = "SEWING";
            this.comboWorkdayYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        // 撈取Grid資料
        private void SelectGridData()
        {
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = this.motherData["ID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string selectCommand1 = @"select FT.*,Artwork=FT.ArtworkTypeID +' (' + iif(ArtworkUnit = 'PCS','PCS', iif(ArtworkUnit = 'STITCH', 'STITCH in thousands','TMS/Min'))+')'
                                                                        from Factory_TMS FT WITH (NOLOCK)
                                                                        inner join ArtworkType AT WITH (NOLOCK) on FT.ArtworkTypeID=AT.ID 
                                                                        where FT.ID = @id";
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, cmds, out selectDataTable1);
            this.listControlBindingSource1.DataSource = selectDataTable1;

            selectCommand1 = "select * from Factory_WorkHour WITH (NOLOCK) where ID = @id";
            DataTable selectDataTable2;
            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand1, cmds, out selectDataTable2);
            this.listControlBindingSource2.DataSource = selectDataTable2;
        }

        private void ComboCapacityYear_SelectedIndexChanged(object sender, EventArgs e)
        {
                this.CapacityFilter();
        }

        private void ComboArtwork_SelectedIndexChanged(object sender, EventArgs e)
        {
                this.CapacityFilter();
        }

        private void ComboWorkdayYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboWorkdayYear.SelectedIndex != -1)
            {
                ((DataTable)this.listControlBindingSource2.DataSource).DefaultView.RowFilter = "Year = " + this.comboWorkdayYear.SelectedValue.ToString();
            }
        }

        private void CapacityFilter()
        {
            ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = "Year = " + (this.comboCapacityYear.SelectedIndex != -1 ? this.comboCapacityYear.SelectedValue.ToString() : "0") + " AND ArtworkTypeID = '" + (this.comboArtwork.SelectedIndex != -1 ? this.comboArtwork.SelectedValue.ToString() : string.Empty) + "'";
        }
    }
}
