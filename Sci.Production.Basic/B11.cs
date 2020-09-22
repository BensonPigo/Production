using Ict;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B11
    /// </summary>
    public partial class B11 : Sci.Win.Tems.Input1
    {
        private string oldStickerComb;
        private string oldStickerCombMix;

        /// <summary>
        /// B11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 帶出Group/Quota的全名
            this.displayGroupQuota2.Text = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'QuotaRegion' and ID = '{0}'", this.CurrentMaintain["QuotaArea"].ToString()));

            // 在編輯模式下，下列這些欄位都不可以被修改
            if (this.EditMode)
            {
                this.txtCountry.TextBox1.ReadOnly = true;
                this.editLabel.ReadOnly = true;
                this.txtPaytermarBulk.TextBox1.ReadOnly = true;
                this.txtPaytermarSample.TextBox1.ReadOnly = true;
                this.checkJunk.ReadOnly = true;
                this.checkScanPack.ReadOnly = true;
                this.checkVASSHAS.ReadOnly = true;
                this.checkSpecialCustomer.ReadOnly = true;
            }

            // 按鈕Shipping Mark變色
            if (!string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkFront"].ToString()) ||
                !string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkBack"].ToString()) ||
                !string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkLeft"].ToString()) ||
                !string.IsNullOrWhiteSpace(this.CurrentMaintain["MarkRight"].ToString()))
            {
                this.btnShippingMark.ForeColor = Color.Blue;
            }
            else
            {
                this.btnShippingMark.ForeColor = Color.Black;
            }

            this.txtStickerComb.Text = MyUtility.GetValue.Lookup($@"

SELECT ID
FROM ShippingMarkCombination
WHERE BrandID = '{this.CurrentMaintain["BrandID"]}'
AND Ukey = '{this.CurrentMaintain["StickerCombinationUkey"]}'
AND IsMixPack = 0
AND Category = 'PIC'
AND Junk = 0
");
            this.oldStickerComb = this.txtStickerComb.Text;
            this.txtStickerCombMix.Text = MyUtility.GetValue.Lookup($@"

SELECT ID
FROM ShippingMarkCombination
WHERE BrandID = '{this.CurrentMaintain["BrandID"]}'
AND Ukey = '{this.CurrentMaintain["StickerCombinationUkey_MixPack"]}'
AND IsMixPack = 1
AND Category = 'PIC'
AND Junk = 0
");
            this.oldStickerCombMix = this.txtStickerCombMix.Text;
        }

        private void BtnShippingMark_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B11_ShippingMark callNextForm = new Sci.Production.Basic.B11_ShippingMark(false, this.CurrentMaintain);
            callNextForm.ShowDialog(this);
        }

        private void TxtStickerComb_Validating(object sender, CancelEventArgs e)
        {
            string newStickerComb = this.txtStickerComb.Text;

            if (this.oldStickerComb != newStickerComb)
            {
                if (MyUtility.Check.Empty(newStickerComb))
                {
                    this.CurrentMaintain["StickerCombinationUkey"] = DBNull.Value;
                    this.txtStickerComb.Text = string.Empty;
                    return;
                }

                string cmd = $@"
SELECT  [Shipping Mark Combination ID]=ID ,Ukey 
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND ID = @ID
AND IsMixPack = 0
AND Category='PIC'
AND Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newStickerComb));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["StickerCombinationUkey"] = DBNull.Value;
                    this.txtStickerComb.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Data not found !!");
                }
                else
                {
                    this.CurrentMaintain["StickerCombinationUkey"] = MyUtility.Convert.GetInt(dt.Rows[0]["Ukey"]);
                    this.txtStickerComb.Text = newStickerComb;
                }
            }
        }

        private void TxtStickerComb_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"
SELECT  [Shipping Mark Combination ID]=ID
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND IsMixPack = 0
AND Category='PIC'
AND Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "Shipping Mark Combination ID", "10", string.Empty, "Shipping Mark Combination ID");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            cmd = $@"
SELECT  Ukey
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND IsMixPack = 0
AND Category='PIC'
AND Junk=0
AND ID = '{item.GetSelectedString()}'
";
            IList<DataRow> selectedData = item.GetSelecteds();
            this.txtStickerComb.Text = item.GetSelectedString();
            this.CurrentMaintain["StickerCombinationUkey"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(cmd));
        }

        private void TxtStickerCombMix_Validating(object sender, CancelEventArgs e)
        {
            string newStickerCombMix = this.txtStickerCombMix.Text;

            if (this.oldStickerComb != newStickerCombMix)
            {
                if (MyUtility.Check.Empty(newStickerCombMix))
                {
                    this.CurrentMaintain["StickerCombinationUkey_MixPack"] = DBNull.Value;
                    this.txtStickerCombMix.Text = string.Empty;
                    return;
                }

                string cmd = $@"
SELECT  [Shipping Mark Combination ID]=ID ,Ukey 
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND ID = @ID
AND IsMixPack = 1
AND Category='PIC'
AND Junk=0";
                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@ID", newStickerCombMix));

                DataTable dt;
                DualResult r = DBProxy.Current.Select(null, cmd, paras, out dt);

                if (!r)
                {
                    this.ShowErr(r);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    this.CurrentMaintain["StickerCombinationUkey_MixPack"] = DBNull.Value;
                    this.txtStickerCombMix.Text = string.Empty;
                    MyUtility.Msg.WarningBox("Data not found !!");
                }
                else
                {
                    this.CurrentMaintain["StickerCombinationUkey_MixPack"] = MyUtility.Convert.GetInt(dt.Rows[0]["Ukey"]);
                    this.txtStickerCombMix.Text = newStickerCombMix;
                }
            }
        }

        private void TxtStickerCombMix_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string cmd = $@"
SELECT  [Shipping Mark Combination ID]=ID
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND IsMixPack = 1
AND Category='PIC'
AND Junk=0";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);
            SelectItem item = new SelectItem(cmd, "Shipping Mark Combination ID", "10", string.Empty, "Shipping Mark Combination ID");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            cmd = $@"
SELECT  Ukey
FROM ShippingMarkCombination
WHERE BrandID='{this.CurrentMaintain["BrandID"]}'
AND IsMixPack = 1
AND Category='PIC'
AND Junk=0
AND ID = '{item.GetSelectedString()}'
";
            IList<DataRow> selectedData = item.GetSelecteds();
            this.txtStickerCombMix.Text = item.GetSelectedString();
            this.CurrentMaintain["StickerCombinationUkey_MixPack"] = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(cmd));
        }
    }
}
