using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Ict.Win;
using Ict;

namespace Sci.Production.Class
{
    /// <summary>
    /// TxtSubconReason
    /// </summary>
    public partial class TxtSubconReason : Win.UI._UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtSubconReason"/> class.
        /// </summary>
        public TxtSubconReason()
        {
            this.InitializeComponent();
        }

        private bool isPopUp = false;

        /// <summary>
        /// 填入Reason Type。例如：RR
        /// </summary>
        [Category("Custom Properties")]
        [Description("填入Reason Type。例如：RR")]
        public string Type { get; set; }

        /// <summary>
        /// 是否可以多選
        /// </summary>
        [Category("Custom Properties")]
        [Description("是否可以多選")]
        public bool MutiSelect { get; set; } = false;

        /// <inheritdoc/>
        public Win.UI.TextBox TextBox1 { get; private set; }

        /// <inheritdoc/>
        public Win.UI.DisplayBox DisplayBox1 { get; private set; }

        /// <inheritdoc/>
        [Bindable(true)]
        public string TextBox1Binding
        {
            get { return this.TextBox1.Text; }
            set { this.TextBox1.Text = value; }
        }

        /// <inheritdoc/>
        [Bindable(true)]
        public string DisplayBox1Binding
        {
            get { return this.DisplayBox1.Text; }
            set { this.DisplayBox1.Text = value; }
        }

        /// <summary>
        /// Where Muti String, [,]區隔
        /// </summary>
        /// <returns>string</returns>
        public string WhereString()
        {
            if (this.MutiSelect)
            {
                return this.TextBox1.Text.Split(',').Select(s => "'" + s + "'").JoinToString(",");
            }
            else
            {
                return "'" + this.TextBox1.Text + "'";
            }
        }

        private void TextBox1_Validating(object sender, CancelEventArgs e)
        {
            // base.OnValidating(e);
            string[] reasonList = this.TextBox1.Text.Split(',');

            // if (!string.IsNullOrWhiteSpace(str) && str != this.textBox1.OldValue)
            if (reasonList.Length > 0 && !MyUtility.Check.Empty(this.TextBox1.Text) && !this.isPopUp)
            {
                var checkReasonResult = reasonList.Select(reasonID =>
                {
                    DataRow dr;
                    bool isExists = MyUtility.Check.Seek($"select ID,Reason from SubconReason with (nolock) where Type = '{this.Type}' and ID = '{reasonID}'", out dr);
                    if (isExists)
                    {
                        return new
                        {
                            ID = dr["ID"].ToString(),
                            Reason = dr["Reason"].ToString(),
                            isExists = true,
                        };
                    }
                    else
                    {
                        return new
                        {
                            ID = reasonID,
                            Reason = string.Empty,
                            isExists = false,
                        };
                    }
                });

                var reasonNotExistsList = checkReasonResult.Where(s => s.isExists == false);

                if (reasonNotExistsList.Any())
                {
                    this.DisplayBox1.Text = string.Empty;
                    this.TextBox1.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Reason: {0} > not found!!!", reasonNotExistsList.Select(s => s.ID).JoinToString(",")));
                    this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                    return;
                }

                this.DisplayBox1.Text = checkReasonResult.Select(s => s.Reason).JoinToString(",");
                this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }

            if (string.IsNullOrWhiteSpace(this.TextBox1.Text))
            {
                this.DisplayBox1.Text = string.Empty;
                return;
            }

            if (e.Cancel)
            {
                return;
            }

            this.isPopUp = false;
            this.OnValidating(e);
        }

        private void TextBox1_Validated(object sender, EventArgs e)
        {
            this.OnValidated(e);
        }

        private void TextBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlGetSubconReason = string.Format("Select Id, Reason from SubconReason WITH (NOLOCK) where type='{0}' order by id", this.Type);

            if (this.MutiSelect)
            {
                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(
                    sqlGetSubconReason, "ID,Reason", this.TextBox1.Text);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.TextBox1.Text = item.GetSelecteds().Select(s => s["Id"].ToString()).JoinToString(",");
                this.DisplayBox1.Text = item.GetSelecteds().Select(s => s["Reason"].ToString()).JoinToString(",");
            }
            else
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(
                    sqlGetSubconReason, "ID,Reason", this.TextBox1.Text);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.TextBox1.Text = item.GetSelectedString();
                this.DisplayBox1.Text = item.GetSelecteds()[0][1].ToString();
            }

            this.isPopUp = true;

            // this.Validate();
            this.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
        }

        /// <inheritdoc/>
        public class CellSubconReason : DataGridViewGeneratorTextColumnSettings
        {
            // public static DataGridViewGeneratorComboBoxColumnSettings GetGridCell(string Type)
            //        {
            //            cellSubconReason cellcombo = new cellSubconReason();
            //            if (!Env.DesignTime)
            //            {
            //                string sqlcmd = $@"
            // Select ID as SubconReasonID, Reason
            // from SubconReason WITH (NOLOCK)
            // where type='{Type}'
            // order by id
            // ";
            //                Ict.DualResult result;
            //                DataTable dt = new DataTable();
            //                Dictionary<string, string> dict_SubReason = new Dictionary<string, string>();
            //                if (result = DBProxy.Current.Select(null, sqlcmd, out dt))
            //                {
            //                    cellcombo.DataSource = dt;
            //                    cellcombo.DisplayMember = "SubconReasonID";
            //                    cellcombo.ValueMember = "SubconReasonID";
            //                }
            //            }
            //            return cellcombo;
            //        }

            /// <summary>
            /// GetGridtxtCell
            /// </summary>
            /// <param name="type">SubconReason.Type</param>
            /// <returns>DataGridViewGeneratorTextColumnSettings</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridtxtCell(string type)
            {
                CellSubconReason celltextbox = new CellSubconReason();

                celltextbox.EditingMouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                        if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                        {
                            return;
                        }

                        DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                        string sqlcmd = $@"select id,Reason from SubconReason where Type='{type}'";
                        Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "10,20", string.Empty, "id,SubConReason");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }
                        else
                        {
                            row["SubconReasonID"] = item.GetSelectedString();
                            row["ReasonDesc"] = item.GetSelecteds()[0]["Reason"].ToString();
                            row.EndEdit();
                        }
                    }
                };

                celltextbox.CellValidating += (s, e) =>
                {
                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;

                    // Parent form 若是非編輯狀態就 return
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    // 右鍵彈出功能
                    DataRow row = grid.GetDataRow<DataRow>(e.RowIndex);
                    string oldValue = row["SubconReasonID"].ToString();
                    string newValue = e.FormattedValue.ToString(); // user 編輯當下的value , 此值尚未存入DataRow
                    string sql;
                    DataRow dr;
                    sql = $@"select id,Reason from SubconReason where Type='{type}' and id ='{newValue}' and junk=0";
                    if (!MyUtility.Check.Empty(newValue) && oldValue != newValue)
                    {
                        if (!MyUtility.Check.Seek(sql, out dr))
                        {
                            row["SubconReasonID"] = string.Empty;
                            row.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox($"< Reason# : {newValue}> not found.");
                            return;
                        }
                        else
                        {
                            row["SubconReasonID"] = dr["id"].ToString();
                            row["ReasonDesc"] = dr["Reason"].ToString();
                            row.EndEdit();
                        }
                    }
                };

                return celltextbox;
            }
        }
    }
}
