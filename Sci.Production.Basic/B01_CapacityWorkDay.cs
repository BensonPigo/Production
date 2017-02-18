﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Basic
{
    public partial class B01_CapacityWorkDay : Sci.Win.Subs.Base
    {
        protected DataRow motherData;

        public B01_CapacityWorkDay(DataRow data)
        {
            InitializeComponent();
            this.motherData = data;
        }

        protected override void OnFormLoaded()
        {
            //呼叫撈Grid資料Method
            this.SelectGridData();

            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = motherData["ID"].ToString();
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = true;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("Year", header: "Year", width: Widths.AnsiChars(4))
                 .Text("Month", header: "Month", width: Widths.AnsiChars(2))
                 .Text("ArtworkTypeID", header: "Artwork", width: Widths.AnsiChars(20))
                 .Numeric("TMS", header: "GSD", width: Widths.AnsiChars(8));

            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = true;
            this.grid2.DataSource = listControlBindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                 .Text("Year", header: "Year", width: Widths.AnsiChars(4))
                 .Text("Month", header: "Month", width: Widths.AnsiChars(2))
                 .Numeric("HalfMonth1", header: "1st half month work day", width: Widths.AnsiChars(3))
                 .Numeric("HalfMonth2", header: "2nd half month work day", width: Widths.AnsiChars(3));


            //設定ComboBox顯示資料
            //comboBox1
            string selectCommand = "select distinct Year from Factory_TMS WITH (NOLOCK) where ID = @id";

            Ict.DualResult returnResult;
            DataTable yearTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out yearTable))
            {
                MyUtility.Tool.SetupCombox(comboBox1, 1, yearTable);
            }

            //comboBox2
            selectCommand = "select distinct ArtworkTypeID from Factory_TMS WITH (NOLOCK) where ID = @id";
            DataTable artworkTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out artworkTable))
            {
                MyUtility.Tool.SetupCombox(comboBox2, 1, artworkTable);
            }

            //comboBox3
            selectCommand = "select distinct Year from Factory_WorkHour WITH (NOLOCK) where ID = @id";
            DataTable yearTable2 = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out yearTable2))
            {
                MyUtility.Tool.SetupCombox(comboBox3, 1, yearTable2);
            }

            //設定ComboBox預設值
            this.comboBox1.SelectedValue = DateTime.Now.Year.ToString();
            this.comboBox2.SelectedValue = "SEWING";
            this.comboBox3.SelectedValue = DateTime.Now.Year.ToString();
        }

        // 撈取Grid資料
        private void SelectGridData()
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id";
            sp1.Value = motherData["ID"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string selectCommand1 = "select * from Factory_TMS WITH (NOLOCK) where ID = @id";
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, cmds, out selectDataTable1);
            listControlBindingSource1.DataSource = selectDataTable1;

            selectCommand1 = "select * from Factory_WorkHour WITH (NOLOCK) where ID = @id";
            DataTable selectDataTable2;
            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand1, cmds, out selectDataTable2);
            listControlBindingSource2.DataSource = selectDataTable2;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                CapacityFilter();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
                CapacityFilter();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex != -1)
            {
                ((DataTable)listControlBindingSource2.DataSource).DefaultView.RowFilter = "Year = " + comboBox3.SelectedValue.ToString();
            }
        }

        private void CapacityFilter()
        {
            ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = "Year = " + (comboBox1.SelectedIndex != -1 ? comboBox1.SelectedValue.ToString() : "0") + " AND ArtworkTypeID = '" + (comboBox2.SelectedIndex != -1 ? comboBox2.SelectedValue.ToString() : "") + "'";
        }
    }
}
