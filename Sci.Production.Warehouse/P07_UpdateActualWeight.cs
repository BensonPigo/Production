﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci;
using Sci.Data;
using Ict;
using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P07_UpdateActualWeight : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable selectDataTable1;
      //  List<DataRow> datas;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();

        public P07_UpdateActualWeight(DataRow data)
        {
            InitializeComponent();
            dr = data;
            string selectCommand1 = string.Format(@"select * from receiving_detail  WITH (NOLOCK) where id='{0}'", dr["id"].ToString());

            DualResult selectResult1 = DBProxy.Current.Select(null, selectCommand1, out selectDataTable1);
            if (selectResult1 == false)
            { ShowErr(selectCommand1, selectResult1); }
            else
            {
                //object inqty = selectDataTable1.Compute("sum(inqty)", null);
                //object outqty = selectDataTable1.Compute("sum(outqty)", null);
                //object adjust = selectDataTable1.Compute("sum(adjust)", null);
                //this.numericBox1.Value = !MyUtility.Check.Empty(inqty) ? decimal.Parse(inqty.ToString()) : 0m;
                //this.numericBox2.Value = !MyUtility.Check.Empty(outqty) ? decimal.Parse(outqty.ToString()) : 0m;
                //this.numericBox3.Value = !MyUtility.Check.Empty(adjust) ? decimal.Parse(adjust.ToString()) : 0m;
            }
        }
        public P07_UpdateActualWeight(object data,string data2)
        {
            InitializeComponent();
            selectDataTable1 = (DataTable)data;
            this.Text+= " - " + data2;
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            listControlBindingSource1.DataSource = selectDataTable1;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            
            //設定Grid1的顯示欄位
            this.gridUpdateAct.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridUpdateAct.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridUpdateAct)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13),iseditingreadonly:true)  //0
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
                .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), iseditable: false).Get(out cbb_fabrictype)  //3
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)    //4
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true) //5
                .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6
                .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)    //7
                .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7, iseditingreadonly: true)    //8
                .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7)    //9
                ;

            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";

            gridUpdateAct.Columns["actualweight"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool rtn;
            gridUpdateAct.ValidateControl();

            if (!(rtn = MyUtility.Tool.CursorUpdateTable(selectDataTable1,"dbo.receiving_detail",null)))
            {
                MyUtility.Msg.WarningBox("Save failed!!");
            }
            MyUtility.Msg.InfoBox("Save successful!!");
        }
    }
}
