﻿using System;
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

            this.comboCartonDimension.DataSource = dt;
            this.comboCartonDimension.DisplayMember = "Value";
            this.comboCartonDimension.ValueMember = "Value";

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
            txtRefno.Focus();
            txtartworktype_ftyCategory.ValidateControl();
        }

        //編輯狀態限制
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtRefno.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.ReadOnly=true;
        }

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["refno"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Refno > can not be empty!");
                this.txtRefno.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Category"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Category > can not be empty!");
                this.txtartworktype_ftyCategory.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Unitid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Unit > can not be empty!");
                this.txtunit_ftyUnit.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.txtDescription.Focus();
                return false;
            }

            if (CurrentMaintain["Category"].ToString().IndexOf("CARTON")>=0)
            {
                if (CurrentMaintain["CtnLength"]==DBNull.Value || (decimal)CurrentMaintain["CtnLength"]==0)
                {
                    MyUtility.Msg.WarningBox("< CtnLength > can not be empty!");
                    this.numL.Focus();
                    return false;
                }
                if (CurrentMaintain["CtnWidth"]==DBNull.Value || (decimal)CurrentMaintain["CtnWidth"] ==0)
                {
                    MyUtility.Msg.WarningBox("< CtnWidth > can not be empty!");
                    this.numW.Focus();
                    return false;
                }
                if (CurrentMaintain["CtnHeight"]==DBNull.Value || (decimal)CurrentMaintain["CtnHeight"] ==0)
                {
                    MyUtility.Msg.WarningBox("< CtnHeight > can not be empty!");
                    this.numH.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(CurrentMaintain["CtnUnit"].ToString()))
                {
                    MyUtility.Msg.WarningBox("< CtnUnit > can not be empty!");
                    this.comboCartonDimension.Focus();
                    return false;
                }
                if (CurrentMaintain["CBM"]==DBNull.Value || (decimal)CurrentMaintain["CBM"] ==0)
                {
                    MyUtility.Msg.WarningBox("< CBM > can not be empty!");
                    this.numCBM.Focus();
                    return false;
                }
            }

            if (CurrentMaintain["Category"].ToString().IndexOf("THREAD") >=0)
            {
                if (CurrentMaintain["MeterToCone"]==DBNull.Value || (decimal)CurrentMaintain["MeterToCone"] ==0)
                {
                    MyUtility.Msg.WarningBox("< MeterToCone > can not be empty!");
                    this.numCone.Focus();
                    return false;
                }
                if (string.IsNullOrWhiteSpace(CurrentMaintain["ThreadTypeID"].ToString()))
                {
                    MyUtility.Msg.WarningBox("< Thread Type > can not be empty!");
                    this.txtThreadType.Focus();
                    return false;
                }
                if (CurrentMaintain["ThreadTex"]==DBNull.Value || (decimal)CurrentMaintain["ThreadTex"] ==0)
                {
                    MyUtility.Msg.WarningBox("< ThreadTex > can not be empty!");
                    this.numThreadTex.Focus();
                    return false;
                }
                if (CurrentMaintain["Weight"]==DBNull.Value || (decimal)CurrentMaintain["Weight"]  ==0)
                {
                    MyUtility.Msg.WarningBox("< Weight > can not be empty!");
                    this.numWeightGW.Focus();
                    return false;
                }
                if (CurrentMaintain["AxleWeight"]==DBNull.Value || (decimal)CurrentMaintain["AxleWeight"] == 0)
                {
                    MyUtility.Msg.WarningBox("< AxleWeight > can not be empty!");
                    this.numWeightofAxle.Focus();
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
            if (this.comboCartonDimension.SelectedValue.ToString() == "Inch")
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
                this.numCBM.Text = MyUtility.Math.Round(i, 4).ToString();
            }
            
        }

        //改變artworktype時，控制可輸入的欄位
        private void txtartworktype_ftyCategory_Validated(object sender, EventArgs e)
        {
            
        }

        //計算cbm相關欄位的valid事件
        private void comboCartonDimension_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.numL.Text)
               || string.IsNullOrWhiteSpace(numW.Text)
               || string.IsNullOrWhiteSpace(numH.Text))
            {
                return;
            }
            getCBM();
        }

        private void btnQuotationRecord_Click(object sender, EventArgs e)
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

            string sqlcmd = "select refno from localitem_quot WITH (NOLOCK) where refno = '" + CurrentMaintain["refno"].ToString() + "';";
            if (MyUtility.Check.Seek(sqlcmd, "Production"))
            {
                this.btnQuotationRecord.ForeColor = Color.Blue;
            }
            else
            {
                this.btnQuotationRecord.ForeColor = Color.Black;
            }

            sqlcmd = "select refno from localap_detail WITH (NOLOCK) where refno = '" + CurrentMaintain["refno"].ToString() + "';";
             if (MyUtility.Check.Seek(sqlcmd, "Production"))
            {
                this.btnPaymentHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnPaymentHistory.ForeColor = Color.Black;
            }
        }

        private void btnPaymentHistory_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.B01_History(dr);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void txtartworktype_ftyCategory_Validating(object sender, CancelEventArgs e)
        {
            CurrentMaintain["category"] = txtartworktype_ftyCategory.Text;
            switch (this.txtartworktype_ftyCategory.Text.Trim())
            {
                case "CARTON":
                    this.groupBox1.Enabled = true;
                    this.groupBox2.Enabled = false;
                    CurrentMaintain["MeterToCone"] = DBNull.Value;
                    CurrentMaintain["ThreadTypeID"] = DBNull.Value;
                    CurrentMaintain["ThreadTex"] = DBNull.Value;
                    CurrentMaintain["Weight"] = DBNull.Value;
                    CurrentMaintain["AxleWeight"] = DBNull.Value;
                    if (string.IsNullOrWhiteSpace(comboCartonDimension.SelectedText.ToString()))
                    {
                        comboCartonDimension.SelectedIndex = 0;
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

        //[Thread Type]右鍵開窗
        private void txtThreadType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || txtThreadType.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID from ThreadType WITH (NOLOCK) WHERE Junk=0", "20", this.txtThreadType.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.txtThreadType.Text = item.GetSelectedString();
        }

        //[Thread Type]檢核
        private void txtThreadType_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtThreadType.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtThreadType.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"select ID from ThreadType WITH (NOLOCK) WHERE Junk=0 and id = '{0}'", textValue)))
                {
                    MyUtility.Msg.WarningBox(string.Format("< Thread Type: {0} > not found !!", textValue));
                    this.txtThreadType.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }


    }
}
