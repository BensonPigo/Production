using Sci.Win.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtsubprocess
    /// </summary>
    public partial class Txtsubprocess : Win.UI.TextBox
    {
        /// <summary>
        /// MultiSelect
        /// </summary>
        [Description("是否要多選")]
        public bool MultiSelect { get; set; } = true;

        /// <summary>
        /// IsRFIDProcess
        /// </summary>
        public bool IsRFIDProcess { get; set; } = true;

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            string sqlWhere = "Select ID From Subprocess WITH (NOLOCK) where junk=0 ";

            if (this.IsRFIDProcess)
            {
                sqlWhere += " and IsRFIDProcess= 1";
            }

            if (this.MultiSelect)
            {
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.Text, defaultValueColumn: "ID");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
            }
            else
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlWhere, headercaptions: "Subprocess ID", columnwidths: "30", defaults: this.Text, defaultValueColumn: "ID");
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
            if (this.MultiSelect)
            {
                string[] ids = this.Text.Split(',');
                List<string> listnotexist = new List<string>();
                foreach (string id in ids)
                {
                    if (!MyUtility.Check.Empty(id))
                    {
                        string sqlCheck = $"select 1 from Subprocess WITH (NOLOCK) where junk=0 and ID = '{id}'";
                        if (this.IsRFIDProcess)
                        {
                            sqlCheck += " and IsRFIDProcess= 1";
                        }

                        if (!MyUtility.Check.Seek(sqlCheck))
                        {
                            listnotexist.Add(id);
                        }
                    }
                }

                if (listnotexist.Count > 0)
                {
                    this.Text = this.OldValue;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"<Subprocess> : {listnotexist.JoinToString(",")}  not found!!!");
                    return;
                }
            }
            else
            {
                string sqlCheck = $"select 1 from Subprocess WITH (NOLOCK) where junk=0 and ID = '{this.Text}'";
                if (this.IsRFIDProcess)
                {
                    sqlCheck += " and IsRFIDProcess= 1";
                }

                if (!MyUtility.Check.Empty(this.Text) && !MyUtility.Check.Seek(sqlCheck))
                {
                    MyUtility.Msg.WarningBox($"<Subprocess> {this.Text} not found");
                    e.Cancel = true;
                    return;
                }
            }

            base.OnValidating(e);
        }
    }
}
