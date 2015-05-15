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
        protected override void OnNewAfter()
        {
            
            base.OnNewAfter();
        }
        
        //copy前清空id
        protected override void OnCopyAfter()
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
        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.textBox1.ReadOnly = true;
            txtartworktype_fty1.ReadOnly = true;
        }

        //存檔前檢查
        protected override bool OnSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["refno"].ToString()))
            {
                MessageBox.Show("< Refno > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                MessageBox.Show("< Category > can not be empty!");
                this.txtartworktype_fty1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Unitid"].ToString()))
            {
                MessageBox.Show("< Unit > can not be empty!");
                this.txtunit_fty1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["description"].ToString()))
            {
                MessageBox.Show("< Description > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (CurrentMaintain["Category"].ToString().IndexOf("CARTON")>=0)
            {
                if (String.IsNullOrWhiteSpace(CurrentMaintain["CtnLength"].ToString()) || CurrentMaintain["CtnLength"].ToString() == "0")
                {
                    MessageBox.Show("< CtnLength > can not be empty!");
                    this.textBox3.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["CtnWidth"].ToString()) || CurrentMaintain["CtnWidth"].ToString() == "0")
                {
                    MessageBox.Show("< CtnWidth > can not be empty!");
                    this.textBox4.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["CtnHeight"].ToString()) || CurrentMaintain["CtnHeight"].ToString() == "0")
                {
                    MessageBox.Show("< CtnHeight > can not be empty!");
                    this.textBox5.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["CtnUnit"].ToString()))
                {
                    MessageBox.Show("< CtnUnit > can not be empty!");
                    this.comboBox1.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["CBM"].ToString()) || CurrentMaintain["CBM"].ToString() == "0")
                {
                    MessageBox.Show("< CBM > can not be empty!");
                    this.textBox6.Focus();
                    return false;
                }
            }

            if (CurrentMaintain["Category"].ToString().IndexOf("THREAD") >=0)
            {
                if (String.IsNullOrWhiteSpace(CurrentMaintain["MeterToCone"].ToString()) || CurrentMaintain["MeterToCone"].ToString() == "0")
                {
                    MessageBox.Show("< MeterToCone > can not be empty!");
                    this.textBox7.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["ThreadTypeID"].ToString()))
                {
                    MessageBox.Show("< ThreadTypeID > can not be empty!");
                    this.textBox8.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["ThreadTex"].ToString()) || CurrentMaintain["ThreadTex"].ToString() == "0")
                {
                    MessageBox.Show("< ThreadTex > can not be empty!");
                    this.textBox9.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["Weight"].ToString()) || CurrentMaintain["Weight"].ToString() == "0")
                {
                    MessageBox.Show("< Weight > can not be empty!");
                    this.textBox10.Focus();
                    return false;
                }
                if (String.IsNullOrWhiteSpace(CurrentMaintain["AxleWeight"].ToString()) || CurrentMaintain["AxleWeight"].ToString() == "0")
                {
                    MessageBox.Show("< AxleWeight > can not be empty!");
                    this.textBox11.Focus();
                    return false;
                }
            }

            return base.OnSaveBefore();
        }

        //算CBM
        private void getCBM()
        {
            if (CurrentMaintain["Category"].ToString().IndexOf("THREAD") >= 0) return;
            if (CurrentMaintain == null) return;
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
	            this.textBox6.Text = Math.Round(i,4).ToString() ;
            }
            else
            {
                double i = double.Parse(CurrentMaintain["CtnLength"].ToString()) *
                    double.Parse(CurrentMaintain["CtnWidth"].ToString()) *
                    double.Parse(CurrentMaintain["CtnHeight"].ToString()) / 1000000000;
                this.textBox6.Text = Math.Round(i, 4).ToString();
            }
            
        }

        //改變artworktype時，控制可輸入的欄位
        private void txtartworktype_fty1_Validated(object sender, EventArgs e)
        {
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

        //計算cbm相關欄位的valid事件
        private void textBox3_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text)
               || string.IsNullOrWhiteSpace(textBox4.Text)
               || string.IsNullOrWhiteSpace(textBox5.Text))
            {
                return;
            }
            getCBM();
        }
    }
}
