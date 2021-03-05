using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class B01 : Win.Tems.Input1
    {
        /// <inheritdoc/>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
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
            string cmd = @"
SELECT  [Text]=Name, [Value]= CASE WHEN Name = 'Manual' THEN 0
                                   WHEN Name = 'Auto' THEN 1
                              ELSE 0  END
FROM DropDownList
WHERE Type='Pms_LocalItem_UnPack'
";
            DBProxy.Current.Select(null, cmd, out dt2);

            this.dropDownUnpack.DataSource = dt2;
            this.dropDownUnpack.DisplayMember = "Text";
            this.dropDownUnpack.ValueMember = "Value";
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.Groupbox_status_control();
        }

        // copy前清空id

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            this.CurrentMaintain["Refno"] = DBNull.Value;
            this.CurrentMaintain["localsuppid"] = DBNull.Value;
            this.CurrentMaintain["price"] = DBNull.Value;
            this.CurrentMaintain["quotdate"] = DBNull.Value;
            this.CurrentMaintain["currencyid"] = DBNull.Value;
            this.CurrentMaintain["Status"] = "New";
            this.txtRefno.Focus();
            this.txtartworktype_ftyCategory.ValidateControl();
        }

        // 編輯狀態限制

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtRefno.ReadOnly = true;
            this.txtSubconSupplier.TextBox1.ReadOnly = true;
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Locked"))
            {
                this.txtDescription.ReadOnly = true;
                this.txtunit_ftyUnit.ReadOnly = true;
                this.txtAccountNo.TextBox1.ReadOnly = true;
            }
        }

        // 存檔前檢查

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["refno"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Refno > can not be empty!");
                this.txtRefno.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Category"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Category > can not be empty!");
                this.txtartworktype_ftyCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Unitid"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Unit > can not be empty!");
                this.txtunit_ftyUnit.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.txtDescription.Focus();
                return false;
            }

            if (this.CurrentMaintain["Category"].ToString().IndexOf("CARTON") >= 0)
            {
                if (this.CurrentMaintain["CtnLength"] == DBNull.Value || (decimal)this.CurrentMaintain["CtnLength"] == 0)
                {
                    MyUtility.Msg.WarningBox("< CtnLength > can not be empty!");
                    this.numL.Focus();
                    return false;
                }

                if (this.CurrentMaintain["CtnWidth"] == DBNull.Value || (decimal)this.CurrentMaintain["CtnWidth"] == 0)
                {
                    MyUtility.Msg.WarningBox("< CtnWidth > can not be empty!");
                    this.numW.Focus();
                    return false;
                }

                if (this.CurrentMaintain["CtnHeight"] == DBNull.Value || (decimal)this.CurrentMaintain["CtnHeight"] == 0)
                {
                    MyUtility.Msg.WarningBox("< CtnHeight > can not be empty!");
                    this.numH.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(this.CurrentMaintain["CtnUnit"].ToString()))
                {
                    MyUtility.Msg.WarningBox("< CtnUnit > can not be empty!");
                    this.comboCartonDimension.Focus();
                    return false;
                }

                if (this.CurrentMaintain["CBM"] == DBNull.Value || (decimal)this.CurrentMaintain["CBM"] == 0)
                {
                    MyUtility.Msg.WarningBox("< CBM > can not be empty!");
                    this.numCBM.Focus();
                    return false;
                }
            }

            if (this.CurrentMaintain["Category"].ToString().IndexOf("THREAD") >= 0)
            {
                if (this.CurrentMaintain["MeterToCone"] == DBNull.Value || (decimal)this.CurrentMaintain["MeterToCone"] == 0)
                {
                    MyUtility.Msg.WarningBox("< MeterToCone > can not be empty!");
                    this.numCone.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ThreadTypeID"].ToString()))
                {
                    MyUtility.Msg.WarningBox("< Thread Type > can not be empty!");
                    this.txtThreadType.Focus();
                    return false;
                }

                if (this.CurrentMaintain["ThreadTex"] == DBNull.Value || (decimal)this.CurrentMaintain["ThreadTex"] == 0)
                {
                    MyUtility.Msg.WarningBox("< ThreadTex > can not be empty!");
                    this.numThreadTex.Focus();
                    return false;
                }

                if (this.CurrentMaintain["Weight"] == DBNull.Value || (decimal)this.CurrentMaintain["Weight"] == 0)
                {
                    MyUtility.Msg.WarningBox("< Weight > can not be empty!");
                    this.numWeightGW.Focus();
                    return false;
                }

                if (this.CurrentMaintain["AxleWeight"] == DBNull.Value || (decimal)this.CurrentMaintain["AxleWeight"] == 0)
                {
                    MyUtility.Msg.WarningBox("< AxleWeight > can not be empty!");
                    this.numWeightofAxle.Focus();
                    return false;
                }
            }

            string chk = $@"select Ctnheight from LocalItem where RefNo='{this.CurrentMaintain["Refno"]}' AND CtnHeight = {this.CurrentMaintain["CtnHeight"]} AND Ctnwidth = {this.CurrentMaintain["Ctnwidth"]} ";

            if (!MyUtility.Check.Seek(chk))
            {
                string cmd = "select * from MailTo where ID='102' AND ToAddress != '' AND ToAddress IS NOT NULL";

                if (MyUtility.Check.Seek(cmd))
                {
                    Prgs.LocalItem_RunningChange(this.CurrentMaintain);
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            if (this.CurrentMaintain["Category"].ToString().ToUpper() == "CARTON")
            {
                string ctnUnit = this.CurrentMaintain["CtnUnit"].ToString().ToUpper();

                decimal ctnheight = MyUtility.Convert.GetDecimal(this.CurrentMaintain["CtnHeight"]);
                if (ctnUnit == "INCH")
                {
                    ctnheight = ctnheight * MyUtility.Convert.GetDecimal(25.4);
                }

                string cmd = $@"
SELECT  DISTINCT  a.UKey
	  ,b.ShippingMarkTypeUkey
	  ,b.IsHorizontal
	  ,b.FromBottom
	  ,b.FromRight
	  ,sticker.Width
	  ,sticker.Length
	  ,[RealCtnHeight] = {ctnheight}
INTO #tmp
FROM ShippingMarkPicture a
INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey
INNER JOIN StickerSize sticker ON b.StickerSizeID = sticker.ID
WHERE a.CTNRefno = '{this.CurrentMaintain["Refno"]}'

UPDATE t
SET t.CtnHeight = s.RealCtnHeight
FROM ShippingMarkPicture t
INNER JOIN #tmp s ON  t.Ukey = s.Ukey

UPDATE t
SET t.IsOverCtnHt = 
				(		
					CASE WHEN s.IsHorizontal = 1 THEN IIF( s.FromBottom + s.Width > s.RealCtnHeight , 1, 0)
						 WHEN s.IsHorizontal = 0 THEN IIF( s.FromBottom + s.Length > s.RealCtnHeight , 1, 0)
						 ELSE 0
					END
				)
	,t.NotAutomate = 
				(		
					CASE WHEN s.IsHorizontal = 1 THEN IIF( s.FromBottom + s.Width > s.RealCtnHeight , 1, 0)
						 WHEN s.IsHorizontal = 0 THEN IIF( s.FromBottom + s.Length > s.RealCtnHeight , 1, 0)
						 ELSE 0
					END
				)
FROM ShippingMarkPicture_Detail t 
INNER JOIN #tmp s ON t.ShippingMarkPictureUkey = s.Ukey AND t.ShippingMarkTypeUkey = s.ShippingMarkTypeUkey

DROP TABLE #tmp
";

                DBProxy.Current.Execute(null, cmd);
            }

            #region ISP20200757 資料交換 - Sunrise
            if (this.CurrentMaintain["Category"].ToString().ToUpper() == "CARTON")
            {
                Task.Run(() => new Sunrise_FinishingProcesses().SentLocalItemToFinishingProcesses(this.CurrentMaintain["RefNo"].ToString()))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        // 算CBM
        private void GetCBM()
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            if (this.CurrentMaintain["Category"].ToString().IndexOf("THREAD") >= 0)
            {
                return;
            }

            // if (string.IsNullOrWhiteSpace(CurrentMaintain["CtnLength"].ToString())
            //    || string.IsNullOrWhiteSpace(CurrentMaintain["CtnWidth"].ToString())
            //    || string.IsNullOrWhiteSpace(CurrentMaintain["CtnHeight"].ToString()))
            // {
            //    return;
            // }
            if (this.comboCartonDimension.SelectedIndex != -1)
            {
                if (this.comboCartonDimension.SelectedValue.ToString() == "Inch")
                {
                    double i = double.Parse(this.numL.Text.ToString()) *
                        double.Parse(this.numW.Text.ToString()) *
                        double.Parse(this.numH.Text.ToString()) * 0.00001639;

                   // numCBM.Text = MyUtility.Math.Round(i, 4).ToString();
                    this.CurrentMaintain["CBM"] = MyUtility.Math.Round(i, 4).ToString();

                    // this.numericBox3.Text = Math.Round(i, 4).ToString();
                }
                else
                {
                    double i = double.Parse(this.numL.Text.ToString()) *
                        double.Parse(this.numW.Text.ToString()) *
                        double.Parse(this.numH.Text.ToString()) / 1000000000;
                    this.CurrentMaintain["CBM"] = MyUtility.Math.Round(i, 4).ToString();

                   // numCBM.Text = MyUtility.Math.Round(i, 4).ToString();
                }
            }
        }

        // 改變artworktype時，控制可輸入的欄位
        private void Txtartworktype_ftyCategory_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["category"]).EqualString("Carton"))
            {
                this.chkIsCarton.Enabled = true;
                this.dropDownUnpack.ReadOnly = false;
                this.txtCartonType.ReadOnly = false;
                this.txtCartonType.IsSupportEditMode = true;
            }
            else
            {
                this.chkIsCarton.Enabled = false;
                this.dropDownUnpack.ReadOnly = true;
                this.txtCartonType.ReadOnly = true;
                this.txtCartonType.IsSupportEditMode = false;
                this.CurrentMaintain["CartonType"] = string.Empty;
            }

            this.dropDownUnpack.SelectedValue = 0;
            this.CurrentMaintain["Unpack"] = 0;
        }

        private void W_H_L_Validated(object sender, EventArgs e)
        {
            this.GetCBM();
        }

        private void BtnQuotationRecord_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new B01_Quotation(this.IsSupportEdit, dr, this.Perm.Confirm);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // 按鈕變色

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            string sqlcmd = "select refno from localitem_quot WITH (NOLOCK) where refno = '" + this.CurrentMaintain["refno"].ToString() + "';";
            if (MyUtility.Check.Seek(sqlcmd, "Production"))
            {
                this.btnQuotationRecord.ForeColor = Color.Blue;
            }
            else
            {
                this.btnQuotationRecord.ForeColor = Color.Black;
            }

            sqlcmd = "select refno from localap_detail WITH (NOLOCK) where refno = '" + this.CurrentMaintain["refno"].ToString() + "';";
            if (MyUtility.Check.Seek(sqlcmd, "Production"))
            {
                this.btnPaymentHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnPaymentHistory.ForeColor = Color.Black;
            }

            if (this.CurrentMaintain["Category"].ToString().ToUpper() == "SP_THREAD" || this.CurrentMaintain["Category"].ToString().ToUpper() == "EMB_THREAD")
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

                if (this.EditMode)
                {
                    this.txtCartonType.ReadOnly = false;
                    this.txtCartonType.IsSupportEditMode = true;
                }
                else
                {
                    this.txtCartonType.ReadOnly = true;
                    this.txtCartonType.IsSupportEditMode = false;
                }
            }
            else
            {
                this.btnSetCardboardPads.Visible = false;
                this.chkIsCarton.Enabled = false;
                this.txtCartonType.ReadOnly = true;
                this.txtCartonType.IsSupportEditMode = false;
            }
        }

        private void BtnPaymentHistory_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            var frm = new B01_History(dr);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void Groupbox_status_control()
        {
            this.CurrentMaintain["category"] = this.txtartworktype_ftyCategory.Text;
            switch (this.txtartworktype_ftyCategory.Text.Trim())
            {
                case "CARTON":
                    this.groupBox1.Enabled = true;
                    this.groupBox2.Enabled = false;
                    this.CurrentMaintain["MeterToCone"] = DBNull.Value;
                    this.CurrentMaintain["ThreadTypeID"] = DBNull.Value;
                    this.CurrentMaintain["ThreadTex"] = DBNull.Value;
                    this.CurrentMaintain["Weight"] = DBNull.Value;
                    this.CurrentMaintain["AxleWeight"] = DBNull.Value;
                    this.CurrentMaintain["Status"] = "New";

                    this.CurrentMaintain["CtnLength"] = this.CurrentMaintain["CtnLength"] == DBNull.Value ? 0 : this.CurrentMaintain["CtnLength"];
                    this.CurrentMaintain["CtnWidth"] = this.CurrentMaintain["CtnWidth"] == DBNull.Value ? 0 : this.CurrentMaintain["CtnWidth"];
                    this.CurrentMaintain["CtnHeight"] = this.CurrentMaintain["CtnHeight"] == DBNull.Value ? 0 : this.CurrentMaintain["CtnHeight"];

                    if (this.comboCartonDimension.SelectedIndex == -1)
                    {
                        this.comboCartonDimension.SelectedIndex = 0;
                        this.CurrentMaintain["CtnUnit"] = this.comboCartonDimension.SelectedValue;
                    }

                    break;
                case "EMB_THREAD":
                case "SP_THREAD":
                    this.groupBox1.Enabled = false;
                    this.groupBox2.Enabled = true;
                    this.CurrentMaintain["CtnLength"] = DBNull.Value;
                    this.CurrentMaintain["CtnWidth"] = DBNull.Value;
                    this.CurrentMaintain["CtnHeight"] = DBNull.Value;
                    this.CurrentMaintain["CtnUnit"] = DBNull.Value;
                    this.CurrentMaintain["CBM"] = DBNull.Value;
                    this.CurrentMaintain["CTNWeight"] = DBNull.Value;
                    break;
                default:
                    this.groupBox1.Enabled = false;
                    this.groupBox2.Enabled = false;
                    this.CurrentMaintain["MeterToCone"] = DBNull.Value;
                    this.CurrentMaintain["ThreadTypeID"] = DBNull.Value;
                    this.CurrentMaintain["ThreadTex"] = DBNull.Value;
                    this.CurrentMaintain["Weight"] = DBNull.Value;
                    this.CurrentMaintain["AxleWeight"] = DBNull.Value;
                    this.CurrentMaintain["CtnLength"] = DBNull.Value;
                    this.CurrentMaintain["CtnWidth"] = DBNull.Value;
                    this.CurrentMaintain["CtnHeight"] = DBNull.Value;
                    this.CurrentMaintain["CtnUnit"] = DBNull.Value;
                    this.CurrentMaintain["CBM"] = DBNull.Value;
                    break;
            }
        }

        private void Txtartworktype_ftyCategory_Validating(object sender, CancelEventArgs e)
        {
            this.Groupbox_status_control();
        }

        // [Thread Type]右鍵開窗
        private void TxtThreadType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.txtThreadType.ReadOnly == true)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID from ThreadType WITH (NOLOCK) WHERE Junk=0", "20", this.txtThreadType.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtThreadType.Text = item.GetSelectedString();
        }

        // [Thread Type]檢核
        private void TxtThreadType_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtThreadType.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtThreadType.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"select ID from ThreadType WITH (NOLOCK) WHERE Junk=0 and id = '{0}'", textValue)))
                {
                    this.txtThreadType.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Thread Type: {0} > not found !!", textValue));
                    return;
                }
            }
        }

        // 計算cbm相關欄位的valid事件
        private void ComboCartonDimension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CurrentMaintain != null && this.comboCartonDimension.SelectedValue != null)
            {
                this.CurrentMaintain["CtnUnit"] = this.comboCartonDimension.SelectedValue.ToString();
            }

            if (string.IsNullOrWhiteSpace(this.numL.Text)
           || string.IsNullOrWhiteSpace(this.numW.Text)
           || string.IsNullOrWhiteSpace(this.numH.Text))
            {
                return;
            }

            this.GetCBM();
        }

        private void BtnThread_Click(object sender, EventArgs e)
        {
            var callfrm = new B01_ThreadColorPrice(true, this.CurrentMaintain["Refno"].ToString(), string.Empty, string.Empty);
            callfrm.ShowDialog();
        }

        private Form batchapprove;

        private void BtnBatchApprove_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Confirm)
            {
                MyUtility.Msg.WarningBox("You don't have permission to confirm.");
                return;
            }

            if (this.batchapprove == null || this.batchapprove.IsDisposed)
            {
                this.batchapprove = new B01_BatchApprove(this.Reload);
                this.batchapprove.Show();
            }
            else
            {
                this.batchapprove.Activate();
            }
        }

        private void BtnSetCardboardPads_Click(object sender, EventArgs e)
        {
            Form form = new B01_SetCardBoarsPads(this.CurrentMaintain);
            form.ShowDialog(this);
            this.OnDetailEntered();
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.EditMode)
            {
                if (this.btnSetCardboardPads != null)
                {
                    this.btnSetCardboardPads.Enabled = false;
                }
            }
            else
            {
                if (this.btnSetCardboardPads != null)
                {
                    this.btnSetCardboardPads.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Reload
        /// </summary>
        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }

        private void B01_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.batchapprove != null)
            {
                this.batchapprove.Dispose();
            }
        }

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
