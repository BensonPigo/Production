using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;

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


            DataTable dt2 = new DataTable();
            DBProxy.Current.Select(null, @"
SELECT  [Text]=Name, [Value]= CASE WHEN Name = 'Manual' THEN 0
                                   WHEN Name = 'Auto' THEN 1
                              ELSE 0  END
FROM DropDownList
WHERE Type='Pms_LocalItem_UnPack'
", out dt2);

            this.dropDownUnpack.DataSource = dt2;
            this.dropDownUnpack.DisplayMember = "Text";
            this.dropDownUnpack.ValueMember = "Value";

        }

        //
        protected override void ClickNewAfter()
        {
            
            base.ClickNewAfter();
            groupbox_status_control();
        }
        
        //copy前清空id
        protected override void ClickCopyAfter()
        {
            CurrentMaintain["Refno"] = DBNull.Value;
            CurrentMaintain["localsuppid"] = DBNull.Value;
            CurrentMaintain["price"] = DBNull.Value;
            CurrentMaintain["quotdate"] = DBNull.Value;
            CurrentMaintain["currencyid"] = DBNull.Value;
            CurrentMaintain["Status"] = "New";
            txtRefno.Focus();
            txtartworktype_ftyCategory.ValidateControl();
        }

        //編輯狀態限制
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtRefno.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.ReadOnly=true;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Locked"))
            {
                this.txtDescription.ReadOnly = true;
                this.txtunit_ftyUnit.ReadOnly = true;
                this.txtAccountNo.TextBox1.ReadOnly = true;
            }
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

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            #region ISP20200757 資料交換 - Sunrise
            if (this.CurrentMaintain["Category"].ToString().ToUpper() == "CARTON")
            {
                Task.Run(() => new Sunrise_FinishingProcesses().SentLocalItemToFinishingProcesses(this.CurrentMaintain["RefNo"].ToString()))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
        }

        protected override void ClickClose()
        {
            string updatesql = $@"update  LocalItem set Status = 'Locked' where Refno = '{this.CurrentMaintain["Refno"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            base.ClickClose();
        }

        protected override void ClickUnclose()
        {
            string updatesql = $@"update  LocalItem set Status = 'New' where Refno = '{this.CurrentMaintain["Refno"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            base.ClickUnclose();
        }

        //算CBM
        private void getCBM()
        {
            if (CurrentMaintain == null) return;
            if (CurrentMaintain["Category"].ToString().IndexOf("THREAD") >= 0) return;
            //if (string.IsNullOrWhiteSpace(CurrentMaintain["CtnLength"].ToString()) 
            //    || string.IsNullOrWhiteSpace(CurrentMaintain["CtnWidth"].ToString())
            //    || string.IsNullOrWhiteSpace(CurrentMaintain["CtnHeight"].ToString()))
            //{
            //    return;
            //}
            if (comboCartonDimension.SelectedIndex != -1)
            {
                if (this.comboCartonDimension.SelectedValue.ToString() == "Inch")
                {
                    double i = double.Parse(numL.Text.ToString()) *
                        double.Parse(numW.Text.ToString()) *
                        double.Parse(numH.Text.ToString()) * 0.00001639;
                   // numCBM.Text = MyUtility.Math.Round(i, 4).ToString();
                   CurrentMaintain["CBM"] = MyUtility.Math.Round(i, 4).ToString();
                    //this.numericBox3.Text = Math.Round(i, 4).ToString();
                }
                else
                {
                    double i = double.Parse(numL.Text.ToString()) *
                        double.Parse(numW.Text.ToString()) *
                        double.Parse(numH.Text.ToString()) / 1000000000;
                    CurrentMaintain["CBM"] = MyUtility.Math.Round(i, 4).ToString();
                   // numCBM.Text = MyUtility.Math.Round(i, 4).ToString();

                }
            }
        }

        //改變artworktype時，控制可輸入的欄位
        private void txtartworktype_ftyCategory_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["category"]).EqualString("Carton"))
            {
                this.chkIsCarton.Enabled = true;
                this.dropDownUnpack.ReadOnly = false;
            }
            else
            {
                this.chkIsCarton.Enabled = false;
                this.dropDownUnpack.ReadOnly = true;
            }
            this.dropDownUnpack.SelectedValue = 0;
            this.CurrentMaintain["Unpack"] = 0;
        }

        private void W_H_L_Validated(object sender, EventArgs e)
        {
            
            getCBM();
        }

        private void btnQuotationRecord_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.B01_Quotation( this.IsSupportEdit, dr, this.Perm.Confirm);
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

            if (CurrentMaintain["Category"].ToString().ToUpper()== "SP_THREAD" || CurrentMaintain["Category"].ToString().ToUpper() == "EMB_THREAD")
            {
                this.btnThread.Visible = true;
            }
            else
            {
                this.btnThread.Visible = false;
            }

            if (this.EditMode)
            {
                this.btnThread.Enabled = false;

                if (this.txtartworktype_ftyCategory.Text.ToUpper() == "CARTON")
                {
                    // Category = Caron 並且為編輯模式下才可以編輯
                    this.dropDownUnpack.ReadOnly = false;
                }
                else
                {
                    this.dropDownUnpack.ReadOnly = true;
                }
            }
            else
            {
                this.groupBox1.Enabled = true;
                this.groupBox2.Enabled = true;
                this.btnThread.Enabled = true;

                this.dropDownUnpack.ReadOnly = true;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["category"]).EqualString("Carton"))
            {
                this.btnSetCardboardPads.Visible = true;
                this.chkIsCarton.Enabled = true;
            }
            else
            {
                this.btnSetCardboardPads.Visible = false;
                this.chkIsCarton.Enabled = false;
            }

        }

        private void btnPaymentHistory_Click(object sender, EventArgs e)
        {
            var dr = CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Subcon.B01_History(dr);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void groupbox_status_control() {
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
                    CurrentMaintain["Status"] = "New";

                    CurrentMaintain["CtnLength"] = CurrentMaintain["CtnLength"] == DBNull.Value ? 0 : CurrentMaintain["CtnLength"];
                    CurrentMaintain["CtnWidth"] = CurrentMaintain["CtnWidth"] == DBNull.Value ? 0 : CurrentMaintain["CtnWidth"];
                    CurrentMaintain["CtnHeight"] = CurrentMaintain["CtnHeight"] == DBNull.Value ? 0 : CurrentMaintain["CtnHeight"];

                    if (comboCartonDimension.SelectedIndex == -1)
                    {
                        comboCartonDimension.SelectedIndex = 0;
                        CurrentMaintain["CtnUnit"] = comboCartonDimension.SelectedValue;
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

        private void txtartworktype_ftyCategory_Validating(object sender, CancelEventArgs e)
        {
            groupbox_status_control();
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
                    this.txtThreadType.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Thread Type: {0} > not found !!", textValue));
                    return;
                }
            }
        }

        // 計算cbm相關欄位的valid事件
        
        private void comboCartonDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (CurrentMaintain != null && comboCartonDimension.SelectedValue != null) {
                CurrentMaintain["CtnUnit"] = comboCartonDimension.SelectedValue.ToString();
            }
            if (string.IsNullOrWhiteSpace(this.numL.Text)
           || string.IsNullOrWhiteSpace(numW.Text)
           || string.IsNullOrWhiteSpace(numH.Text))
            {
                return;
            }
            getCBM();
        }

        private void btnThread_Click(object sender, EventArgs e)
        {
            var callfrm = new B01_ThreadColorPrice(true, CurrentMaintain["Refno"].ToString(),string.Empty,string.Empty);
            callfrm.ShowDialog();
        }
        Form batchapprove;
        private void btnBatchApprove_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Confirm)
            {
                MyUtility.Msg.WarningBox("You don't have permission to confirm.");
                return;
            }

            if (batchapprove == null || batchapprove.IsDisposed)
            {
                batchapprove = new Sci.Production.Subcon.B01_BatchApprove(reload);
                batchapprove.Show();
            }
            else
            {
                batchapprove.Activate();
            }

        }

        private void btnSetCardboardPads_Click(object sender, EventArgs e)
        {
            Form form = new Sci.Production.Subcon.B01_SetCardBoarsPads(this.CurrentMaintain);
            form.ShowDialog(this);
            this.OnDetailEntered();
            this.RenewData();
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (EditMode)
            {
                if (this.btnSetCardboardPads != null)
                    this.btnSetCardboardPads.Enabled = false;
            }
            else
            {
                if (this.btnSetCardboardPads != null)
                    this.btnSetCardboardPads.Enabled = true;
            }
        }

        public void reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }

        private void B01_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (batchapprove != null)
            {
                batchapprove.Dispose();
            }
        }

        /// <summary>
        /// B01_FormLoaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B01_FormLoaded(object sender, EventArgs e)
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = "JUNK = 0";
                        break;
                    case "1":
                        this.DefaultWhere = "JUNK = 1";
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }
                this.ReloadDatas();
            };
        }
    }
}
