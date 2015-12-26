using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Class;

namespace Sci.Production.Thread
{
    public partial class P21 : Sci.Win.Tems.Input6
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_color;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_cons;


        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            dateBox3.Text = MyUtility.GetValue.Lookup("SciDelivery", textBox1.Text, "Orders", "ID");
            dateBox4.Text = MyUtility.GetValue.Lookup("SewInLine", textBox1.Text, "Orders", "ID");
            change_record();
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
            @"SELECT a.*, b.description, b.MetertoCone ,c.description as colordesc,d.newCone,d.useCOne
            FROM ThreadRequisition_Detail a
            Left Join Localitem b on a.refno = b.id
            Left join ThreadColor c on c.id = a.ThreadColorid
            Left join ThreadStock d on d.refno = a.refno and d.Threadcolorid = a,threadcolorid
            WHERE a.ID = '{0}'", masterID);
            
            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread");
            DataGridViewGeneratorNumericColumnSettings cons = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings usestock = new DataGridViewGeneratorNumericColumnSettings();
            usestock.CellValidating += (s, e) =>
            {
                decimal oldvalue =(decimal)CurrentDetailData["UseStockQty"];
                decimal newvalue = (decimal)e.FormattedValue;
                if (!this.EditMode || oldvalue == newvalue) return;
                CurrentDetailData["PurchaseQty"] = (decimal)CurrentDetailData["TotalQty"] + (decimal)CurrentDetailData["AllowanceQty"] - newvalue;
                CurrentDetailData.EndEdit();
            };

            Helper.Controls.Grid.Generator(this.grid)
           .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20)).Get(out col_refno)
           .Text("description", header: "Thread Desc", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .CellThreadColor("ThreadColorid", header: "Thread Color", width: Widths.AnsiChars(15), iseditingreadonly: true).Get(out col_color)
           .Text("Colordesc", header: "Thread Color Desc", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Numeric("ConsumptionQty", header: "Total Consumptions", width: Widths.AnsiChars(5), integer_places: 6).Get(out col_cons)
           .Numeric("MeterToCone", header: "No. of Meters Per Cons", width: Widths.AnsiChars(5), integer_places: 7, decimal_places:1, iseditingreadonly: true)
           .Numeric("TotalQty", header: "No. of Cones", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("AllowanceQty", header: "20% allowance", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("UseCone", header: "Use Cone", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("UseStockQty", header: "Use Stock", width: Widths.AnsiChars(5), integer_places: 6)
           .Numeric("PurchaseQty", header: "PO Qty", width: Widths.AnsiChars(5), integer_places: 6)
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
           .Text("POID", header: "PO ID", width: Widths.AnsiChars(10), iseditingreadonly: true);



            this.detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink;
        }
        private void change_record()
        {
            if (CurrentMaintain["autoCreate"].ToString()== "True")
            {
                col_color.IsEditingReadOnly = true;
                col_cons.IsEditingReadOnly = true;
                col_refno.IsEditingReadOnly = true;
                this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Empty;
                this.detailgrid.Columns[2].DefaultCellStyle.BackColor = Color.Empty;
                this.detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Empty;

            }
            else
            {
                col_color.IsEditingReadOnly = false;
                col_cons.IsEditingReadOnly = false;
                col_refno.IsEditingReadOnly = false;
                this.detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
                this.detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            }
        }
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already Confirmed, you can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already Confirmed, you can't modify", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string id = textBox1.Text;
            DataRow drOrder;
            if (!MyUtility.Check.Seek(string.Format("Select * from Order where id='{0}'",id),out drOrder ))
            { 
                MyUtility.Msg.WarningBox("<SP No> data not found");
                e.Cancel = true;
                return;
            }
            
            dateBox3.Value = (DateTime)drOrder["SciDelivery"];
            dateBox4.Value = (DateTime)drOrder["SewInLine"];
            displayBox1.Text = drOrder["Styleid"].ToString();
            displayBox2.Text = drOrder["Seasonid"].ToString();
            displayBox3.Text = drOrder["Brandid"].ToString();
            displayBox4.Text = drOrder["factoryid"].ToString();
            string sql = string.Format(@"Select a.*,b. from ThreadColorComb a,ThreadColorComb_Detail b 
                            Where a.id = b.id ");

        }

    }
}
