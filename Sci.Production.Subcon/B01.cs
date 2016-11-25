using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;


namespace Sci.Production.Subcon
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");

            DataRow dr = dt.NewRow();
            dr[0] = "Inch";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "MM";
            dt.Rows.Add(dr);

            this.comboBox1.DataSource = dt;
            this.comboBox1.DisplayMember = "Value";
            this.comboBox1.ValueMember = "Value";

        }

        //
        protected override void ClickNewAfter()
        {
            
            base.ClickNewAfter();
        }
        
        //copy前清空id
        protected override void ClickCopyAfter()
        {
            CurrentMaintain["Refno"] = DBNull.Value;
            CurrentMaintain["localsuppid"] = DBNull.Value;
            CurrentMaintain["price"] = DBNull.Value;
            CurrentMaintain["quotdate"] = DBNull.Value;
            CurrentMaintain["currencyid"] = DBNull.Value;
            textBox1.Focus();
            txtartworktype_fty1.ValidateControl();
        }

        //編輯狀態限制
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["refno"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Refno > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Category > can not be empty!");
                this.txtartworktype_fty1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Unitid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Unit > can not be empty!");
                this.txtunit_fty1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (CurrentMaintain["Category"].ToString().IndexOf("CARTON")>=0)
            {
                if (CurrentMaintain["CtnLength"]==DBNull.Value || (decimal)CurrentMaintain["CtnLength"]==0)
                {
                    MyUtility.Msg.WarningBox("< CtnLength > can not be empty!");
                    this.numericBox1.Focus();
                    return false;
                }
                if (CurrentMaintain["CtnWidth"]==DBNull.Value || (decimal)CurrentMaintain["CtnWidth"] ==0)
                {
                    MyUtility.Msg.WarningBox("< CtnWidth > can not be empty!");
                    this.numericBox2.Focus();
                    return false;
                }
                if (CurrentMaintain["CtnHeight"]==DBNull.Value || (decimal)CurrentMaintain["CtnHeight"] ==0)
                {
                    MyUtility.Msg.WarningBox("< CtnHeight > can not be empty!");
                    this.numericBox7.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(CurrentMaintain["CtnUnit"].ToString()))
                {
                    MyUtility.Msg.WarningBox("< CtnUnit > can not be empty!");
                    this.comboBox1.Focus();
                    return false;
                }
                if (CurrentMaintain["CBM"]==DBNull.Value || (decimal)CurrentMaintain["CBM"] ==0)
                {
                    MyUtility.Msg.WarningBox("< CBM > can not be empty!");
                    this.numericBox3.Focus();
                    return false;
                }
            }

            if (CurrentMaintain["Category"].ToString().IndexOf("THREAD") >=0)
            {
                if (CurrentMaintain["MeterToCone"]==DBNull.Value || (decimal)CurrentMaintain["MeterToCone"] ==0)
                {
                    MyUtility.Msg.WarningBox("< MeterToCone > can not be empty!");
                    this.numericBox8.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(CurrentMaintain["ThreadTypeID"].ToString()))
                {
                    MyUtility.Msg.WarningBox("< Thread Item > can not be empty!");
                    this.textBox8.Focus();
                    return false;
                }
                if (CurrentMaintain["ThreadTex"]==DBNull.Value || (decimal)CurrentMaintain["ThreadTex"] ==0)
                {
                    MyUtility.Msg.WarningBox("< ThreadTex > can not be empty!");
                    this.numericBox5.Focus();
                    return false;
                }
                if (CurrentMaintain["Weight"]==DBNull.Value || (decimal)CurrentMaintain["Weight"]  ==0)
                {
                    MyUtility.Msg.WarningBox("< Weight > can not be empty!");
                    this.numericBox4.Focus();
                    return false;
                }
                if (CurrentMaintain["AxleWeight"]==DBNull.Value || (decimal)CurrentMaintain["AxleWeight"] == 0)
                {
                    MyUtility.Msg.WarningBox("< AxleWeight > can not be empty!");
                    this.numericBox6.Focus();
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        //算CBM
        private void getCBM()
        {
            if (CurrentMaintain == null) return;
            if (CurrentMaintain["Category"].ToString().IndexOf("THREAD") >= 0) return;
            if (string.IsNullOrWhiteSpace(CurrentMaintain["CtnLength"].ToString()) 
                || string.IsNullOrWhiteSpace(CurrentMaintain["CtnWidth"].ToString())
                || string.IsNullOrWhiteSpace(CurrentMaintain["CtnHeight"].ToString()))
            {
                return;
            }
            if (this.comboBox1.SelectedValue.ToString() == "Inch")
            {
                double i = double.Parse(CurrentMaintain["CtnLength"].ToString()) *
                    double.Parse(CurrentMaintain["CtnWidth"].ToString()) *
                    double.Parse(CurrentMaintain["CtnHeight"].ToString()) /  1728;
                CurrentMaintain["cbm"] = i;
                //this.numericBox3.Text = Math.Round(i, 4).ToString();
            }
            else
            {
                double i = double.Parse(CurrentMaintain["CtnLength"].ToString()) *
                    double.Parse(CurrentMaintain["CtnWidth"].ToString()) *
                    double.Parse(CurrentMaintain["CtnHeight"].ToString()) / 1000000000;
                this.numericBox3.Text = MyUtility.Math.Round(i, 4).ToString();
            }
            
        }

        //改變artworktype時，控制可輸入的欄位
        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
            
        }

        //計算cbm相關欄位的valid事件
        private void textBox3_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.numericBox1.Text)
               || string.IsNullOrWhiteSpace(numericBox2.Text)
               || string.IsNullOrWhiteSpace(numericBox7.Text))
            {
                return;
            }
            getCBM();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.B01_Quotation( this.IsSupportEdit, dr);
            frm.ShowDialog(this);
            this.RenewData();

        }

        //按鈕變色
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            
            string sqlcmd = "select refno from localitem_quot where refno = '" + CurrentMaintain["refno"].ToString() + "';";
            if (MyUtility.Check.Seek(sqlcmd, "Production"))
            {
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }

             sqlcmd = "select refno from localap_detail where refno = '" + CurrentMaintain["refno"].ToString() + "';";
             if (MyUtility.Check.Seek(sqlcmd, "Production"))
            {
                this.button2.ForeColor = Color.Blue;
            }
            else
            {
                this.button2.ForeColor = Color.Black;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.B01_History(dr);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void txtartworktype_fty1_Validating(object sender, CancelEventArgs e)
        {
            CurrentMaintain["category"] = txtartworktype_fty1.Text;
            switch (this.txtartworktype_fty1.Text.Trim())
            {
                case "CARTON":
                    this.groupBox1.Enabled = true;
                    this.groupBox2.Enabled = false;
                    CurrentMaintain["MeterToCone"] = DBNull.Value;
                    CurrentMaintain["ThreadTypeID"] = DBNull.Value;
                    CurrentMaintain["ThreadTex"] = DBNull.Value;
                    CurrentMaintain["Weight"] = DBNull.Value;
                    CurrentMaintain["AxleWeight"] = DBNull.Value;
                    if (string.IsNullOrWhiteSpace(comboBox1.SelectedText.ToString()))
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    break;
                case "EMB_THREAD":
                case "SP_THREAD":
                    this.groupBox1.Enabled = false;
                    this.groupBox2.Enabled = true;
                    CurrentMaintain["CtnLength"] = DBNull.Value;
                    CurrentMaintain["CtnWidth"] = DBNull.Value;
                    CurrentMaintain["CtnHeight"] = DBNull.Value;
                    CurrentMaintain["CtnUnit"] = DBNull.Value;
                    CurrentMaintain["CBM"] = DBNull.Value;
                    CurrentMaintain["CTNWeight"] = DBNull.Value;
                    break;
                default:
                    this.groupBox1.Enabled = false;
                    this.groupBox2.Enabled = false;
                    CurrentMaintain["MeterToCone"] = DBNull.Value;
                    CurrentMaintain["ThreadTypeID"] = DBNull.Value;
                    CurrentMaintain["ThreadTex"] = DBNull.Value;
                    CurrentMaintain["Weight"] = DBNull.Value;
                    CurrentMaintain["AxleWeight"] = DBNull.Value;
                    CurrentMaintain["CtnLength"] = DBNull.Value;
                    CurrentMaintain["CtnWidth"] = DBNull.Value;
                    CurrentMaintain["CtnHeight"] = DBNull.Value;
                    CurrentMaintain["CtnUnit"] = DBNull.Value;
                    CurrentMaintain["CBM"] = DBNull.Value;
                    break;

            }
        }
    }
}
