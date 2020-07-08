using Ict;
using Sci.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// B03
    /// </summary>
    public partial class B03 : Win.Tems.Input1
    {
        private Hashtable ht = new Hashtable();
        private DataTable sizes;
        private DataTable sizesAll;

        /// <summary>
        /// B03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Resources\");
            if (this.ht.Count == 0)
            {
                this.ht.Add("Picture1", path + "CTN.jpg");
                this.pictureBox1.ImageLocation = this.ht["Picture1"].ToString();
            }

            #region ComboBox
            DualResult result;
            string cmd = $@"
SELECT [ID]='' ,[SIze]='' 
UNION
SELECT ID, SIze 
FROM StickerSize WITH (NOLOCK) 
where junk <> 1";
            result = DBProxy.Current.Select(null, cmd, out this.sizes);
            if (!result)
            {
                this.ShowErr(result);
            }

            cmd = $@"
SELECT [ID]='' ,[SIze]='' 
UNION
SELECT ID, SIze 
FROM StickerSize WITH (NOLOCK) 
";
            result = DBProxy.Current.Select(null, cmd, out this.sizesAll);
            if (!result)
            {
                this.ShowErr(result);
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.ComboPressing2DataSource();
        }

        private void ComboPressing2DataSource()
        {
            if (this.comboStickerSize != null && this.CurrentMaintain != null)
            {
                if (this.EditMode && this.sizes != null)
                {
                    MyUtility.Tool.SetupCombox(this.comboStickerSize, 1, this.sizes);
                    this.comboStickerSize.DisplayMember = "Size";
                }

                if (!this.EditMode && this.sizesAll != null)
                {
                    MyUtility.Tool.SetupCombox(this.comboStickerSize, 1, this.sizesAll);
                    this.comboStickerSize.DisplayMember = "Size";
                }

                this.comboStickerSize.SelectedValue = this.CurrentMaintain["StickerSizeID"];
            }
        }

        private void TxtCTNRefno_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select RefNo  from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' ", null, this.txtCTNRefno.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCTNRefno.Text = item.GetSelectedString();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Seq"] = 1;
        }

        private void TxtCTNRefno_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtCTNRefno.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtCTNRefno.OldValue)
            {
                if (!MyUtility.Check.Seek($"Select 1 from LocalItem WITH (NOLOCK) where Junk = 0 and Category='CARTON' and RefNo = '{textValue}'"))
                {
                    this.txtCTNRefno.Text = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< RefNo : {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CTNRefno"]))
            {
                MyUtility.Msg.WarningBox("CTNRefno can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Side"]))
            {
                MyUtility.Msg.WarningBox("Side can not empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Seq"]))
            {
                MyUtility.Msg.WarningBox("Seq can not empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.ComboPressing2DataSource();
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (!this.EditMode && this.CurrentMaintain != null && this.tabs.SelectedIndex == 1)
            {
                bool junk = MyUtility.Convert.GetBool(this.CurrentMaintain["junk"]);
                this.toolbar.cmdJunk.Enabled = !junk && this.Perm.Junk;
                this.toolbar.cmdUnJunk.Enabled = junk && this.Perm.Junk;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = false;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        protected override void ClickJunk()
        {
            base.ClickJunk();
            string sqlcmd = $@"update ShippingMarkPicture set junk = 1 
where BrandID = '{this.CurrentMaintain["BrandID"]}'
and CTNRefno = '{this.CurrentMaintain["CTNRefno"]}'
and Side = '{this.CurrentMaintain["Side"]}'
and Seq = '{this.CurrentMaintain["Seq"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            string sqlcmd = $@"update ShippingMarkPicture set junk = 0
where BrandID = '{this.CurrentMaintain["BrandID"]}'
and CTNRefno = '{this.CurrentMaintain["CTNRefno"]}'
and Side = '{this.CurrentMaintain["Side"]}'
and Seq = '{this.CurrentMaintain["Seq"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private void ComboStickerSize_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.comboStickerSize.SelectedIndex == -1)
            {
                return;
            }

            DataTable dt;
            DualResult result;
            Int64 id = Convert.ToInt64(this.comboStickerSize.SelectedValue);
            string cmd = "SELECT  Size ,Width,Length FROM StickerSize WITH(NOLOCK) WHERE ID=@ID";
            List<SqlParameter> paras = new List<SqlParameter>();

            paras.Add(new SqlParameter("@ID", id));

            result = DBProxy.Current.Select(null, cmd, paras, out dt);
            if (result)
            {
                if (dt.Rows != null && dt.Rows.Count > 0)
                {
                    // ISP20200158 移除[Production].[dbo].[ShippingMarkPicture].[PicLength] 及 [PicWidth]
                    // this.CurrentMaintain["PicLength"] = Convert.ToInt32(dt.Rows[0]["Length"]);
                    // this.CurrentMaintain["PicWidth"] = Convert.ToInt32(dt.Rows[0]["Width"]);
                }
            }
            else
            {
                this.ShowErr(result);
            }

            if (this.CurrentMaintain != null && this.EditMode)
            {
                this.CurrentMaintain["StickerSizeID"] = id;
            }
        }
    }
}
