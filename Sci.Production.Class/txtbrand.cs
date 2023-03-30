﻿using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtbrand
    /// </summary>
    public partial class Txtbrand : Win.UI.TextBox
    {
        private bool multi_select = false;

        private Control MyDocument { get; set; } = null;   // 欄位.存入要取值的<控制項>

        /// <inheritdoc />
        //[Category("Custom Properties")]
        //[Description("選擇畫面上[Document]的控制項名稱，僅篩選出對應該Brand的資料")]
        public Control MyDocumentdName
        {
            get
            {
                return this.MyDocument;
            }

            set
            {
                this.MyDocument = value;
            }
        }

        /// <summary>
        /// Multi Select
        /// </summary>
        public bool MultiSelect
        {
            set
            {
               this.multi_select = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtbrand"/> class.
        /// </summary>
        public Txtbrand()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "SELECT Id,NameCH,NameEN FROM Production.dbo.Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
            if (this.multi_select)
            {
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, string.Empty, "10,29,35", string.Empty, null, null, null)
                {
                    Size = new System.Drawing.Size(810, 666),
                };
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }
            else
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, "10,29,35", this.Text, false, ",")
                {
                    Size = new System.Drawing.Size(777, 666),
                };
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }

            this.ValidateText();
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                string[] str_multi = str.Split(',');
                if (str_multi.Length > 1)
                {
                    string err_brand = string.Empty;
                    foreach (string chk_str in str_multi)
                    {
                        if (MyUtility.Check.Seek(chk_str, "Brand", "id", "Production") == false)
                        {
                            err_brand += "," + chk_str;
                        }
                    }

                    if (!err_brand.Equals(string.Empty))
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Brand : {0} > not found!!!", err_brand.Substring(1)));
                        return;
                    }
                }
                else
                {
                    if (MyUtility.Check.Seek(str, "Brand", "id", "Production") == false)
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Brand : {0} > not found!!!", str));
                        return;
                    }
                }
            }
        }
    }
}
