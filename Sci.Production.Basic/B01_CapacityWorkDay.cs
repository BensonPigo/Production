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
            //設定ComboBox顯示資料
            //comboBox1
            string selectCommand = string.Format("select distinct Year from Factory_TMS where ID = '{0}'", this.motherData["ID"].ToString());
            Ict.DualResult returnResult;
            DataTable yearTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out yearTable))
            {
                this.comboBox1.DataSource = yearTable;
                this.comboBox1.DisplayMember = "Year";
                this.comboBox1.ValueMember = "Year";
            }

            //comboBox2
            selectCommand = string.Format("select distinct ArtworkTypeID from Factory_TMS where ID = '{0}'", this.motherData["ID"].ToString());
            DataTable artworkTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out artworkTable))
            {
                this.comboBox2.DataSource = artworkTable;
                this.comboBox2.DisplayMember = "ArtworkTypeID";
                this.comboBox2.ValueMember = "ArtworkTypeID";
            }

            //comboBox3
            selectCommand = string.Format("select distinct Year from Factory_WorkHour where ID = '{0}'", this.motherData["ID"].ToString());
            DataTable yearTable2 = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out yearTable2))
            {
                this.comboBox3.DataSource = yearTable2;
                this.comboBox3.DisplayMember = "Year";
                this.comboBox3.ValueMember = "Year";
            }

            //設定ComboBox預設值
            this.comboBox1.SelectedValue = DateTime.Now.Year.ToString();
            this.comboBox2.SelectedValue = "SEWING";
            this.comboBox3.SelectedValue = DateTime.Now.Year.ToString();
            
            //呼叫撈Grid資料Method
            this.SelectGridData();

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = bindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                 .Text("Year", header: "Year", width: Widths.AnsiChars(4))
                 .Text("Month", header: "Month", width: Widths.AnsiChars(2))
                 .Text("ArtworkTypeID", header: "Artwork", width: Widths.AnsiChars(20))
                 .Numeric("TMS", header: "GSD", width: Widths.AnsiChars(8));

            //設定Grid2的顯示欄位
            this.grid2.IsEditingReadOnly = false;
            this.grid2.DataSource = bindingSource2;
            Helper.Controls.Grid.Generator(this.grid2)
                 .Text("Year", header: "Year", width: Widths.AnsiChars(4))
                 .Text("Month", header: "Month", width: Widths.AnsiChars(2))
                 .Numeric("HalfMonth1", header: "1st half month work day", width: Widths.AnsiChars(3))
                 .Numeric("HalfMonth2", header: "2nd half month work day", width: Widths.AnsiChars(3));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            this.SelectGridData();
        }

        private void comboBox2_Validated(object sender, EventArgs e)
        {
            this.SelectGridData();
        }

        private void comboBox3_Validated(object sender, EventArgs e)
        {
            this.SelectGridData();
        }

        // 撈取Grid資料
        private void SelectGridData()
        {
            string selectCommand1 = string.Format("select * from Factory_TMS where ID = '{0}' and Year = '{1}'  and ArtworkTypeID = '{2}'", this.motherData["ID"].ToString(), this.comboBox1.SelectedValue, this.comboBox2.SelectedValue);
            DataTable selectDataTable1;
            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            bindingSource1.DataSource = selectDataTable1;

            selectCommand1 = string.Format("select * from Factory_WorkHour where ID = '{0}'  and Year = '{1}'  ", this.motherData["ID"].ToString(), this.comboBox3.SelectedValue);
            DataTable selectDataTable2;
            DualResult selectResult2 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable2);
            bindingSource2.DataSource = selectDataTable2;
        }
    }
}
