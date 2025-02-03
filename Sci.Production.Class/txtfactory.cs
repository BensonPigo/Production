using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;
using System.Data.SqlClient;
using Ict;
using System.Linq;
using Sci.Data;
using System.Data;

namespace Sci.Production.Class
{
    /// <summary>
    /// Txtfactory
    /// </summary>
    public partial class Txtfactory : Win.UI.TextBox
    {
        /// <summary>
        /// 是否要顯示 Junk 的資料
        /// </summary>
        [Description("是否要顯示 Junk 的資料")]
        public bool IssupportJunk { get; set; } = false;

        /// <summary>
        /// 是否需要排除非同 MDivition 的資料
        /// </summary>
        [Description("是否需要排除非同 MDivition 的資料")]
        public bool FilteMDivision { get; set; } = false;

        /// <summary>
        /// 是否只顯示 IsProduceFty 的資料
        /// </summary>
        [Description("是否只顯示 IsProduceFty 的資料")]
        public bool IsProduceFty { get; set; } = false;

        /// <summary>
        /// MDivision
        /// </summary>
        [Description("M元件")]
        public object MDivision { get; set; }

        /// <summary>
        /// boolFtyGroupList
        /// 預設值 True
        /// True：替換單字 FtyGroup
        /// False：替換單字 ID
        /// </summary>
        public bool BoolFtyGroupList { get; set; } = true;

        /// <summary>
        /// 多選
        /// </summary>
        public bool IsMultiselect { get; set; } = false;

        /// <summary>
        /// IE走這
        /// </summary>
        public bool IsIE { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Txtfactory"/> class.
        /// </summary>
        public Txtfactory()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            #endregion
            #region SQL Filte
            List<string> listFilte = new List<string>();
            if (!this.IssupportJunk)
            {
                listFilte.Add("Junk = 0");
            }

            if (this.IsProduceFty)
            {
                listFilte.Add("IsProduceFty = 1");
            }

            if (this.FilteMDivision)
            {
                listSqlPar.Add(new SqlParameter("@MDivision", Env.User.Keyword));
                listFilte.Add("MDivisionID = @MDivision");
            }

            if (this.MDivision != null && !MyUtility.Check.Empty(((Win.UI.TextBox)this.MDivision).Text))
            {
                listSqlPar.Add(new SqlParameter("@MDivision", ((Win.UI.TextBox)this.MDivision).Text));
                listFilte.Add("MDivisionID = @MDivision");
            }
            #endregion
            #region SQL CMD

            string strShowColumn = string.Empty;
            string strOrderBy = string.Empty;
            string sqlcmd = string.Empty;
            if (!this.IsIE)
            {
                // 依boolFtyGroupList=true 顯示FtyGroup 反之顯示ID
                if (this.BoolFtyGroupList)
                {
                    strShowColumn = "DISTINCT FtyGroup";
                }
                else
                {
                    strShowColumn = "ID";
                    strOrderBy = "order by FtyGroup";
                }

                sqlcmd = $@"Select {strShowColumn} as Factory from Production.dbo.Factory WITH (NOLOCK) {((listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : string.Empty)} {strOrderBy}";
            }
            else
            {
                sqlcmd = "SELECT DISTINCT FactoryID FROM Employee";
            }

            #endregion
            if (this.IsMultiselect)
            {
                DualResult dualResult = DBProxy.Current.Select(null, sqlcmd, listSqlPar, out DataTable dt);
                if (!dualResult)
                {
                    MyUtility.Msg.ErrorBox(dualResult.ToString());
                    return;
                }

                Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(dt, "Factory", "Factory", "8", this.Text);
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
                this.ValidateText();

            }
            else
            {
                Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, listSqlPar, "8", this.Text, false, ",");
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.Text = item.GetSelectedString();
                this.ValidateText();
            }
        }

        /// <inheritdoc/>
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            listSqlPar.Add(new SqlParameter("@str", this.Text));
            #endregion
            #region SQL Filte
            List<string> listFilte = new List<string>();

            if (!this.IssupportJunk)
            {
                listFilte.Add("Junk = 0");
            }

            if (this.IsProduceFty)
            {
                listFilte.Add("IsProduceFty = 1");
            }

            if (this.FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision");
            }
            #endregion

            // 依boolFtyGroupList=true 顯示FtyGroup 反之顯示ID
            string strShowColumn = string.Empty;
            if (this.BoolFtyGroupList)
            {
                strShowColumn = "DISTINCT FtyGroup";
                listFilte.Add("FtyGroup = @str");
            }
            else
            {
                strShowColumn = "ID";
                listFilte.Add("id = @str");
            }

            string where = (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : string.Empty;
            string sqlcmd = $"Select {strShowColumn} from Production.dbo.Factory WITH (NOLOCK) {where}";

            if (IsIE)
            {
                sqlcmd = $@"SELECT DISTINCT FactoryID FROM Employee where FactoryID = @str";
            }
            if (this.IsMultiselect)
            {
                string[] factorys = this.Text.Split(',');
                List<string> listnotexist = new List<string>();
                foreach (string factory in factorys)
                {
                    if (!MyUtility.Check.Empty(factory))
                    {
                        listSqlPar.Clear();
                        listSqlPar.Add(new SqlParameter("@MDivision", Env.User.Keyword));
                        listSqlPar.Add(new SqlParameter("@str", factory));
                        if (!MyUtility.Check.Seek(sqlcmd, listSqlPar))
                        {
                            listnotexist.Add(factory);
                        }
                    }
                }

                if (listnotexist.Count > 0)
                {
                    this.Text = this.OldValue;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"< Factory : {listnotexist.JoinToString(",")} > not found!!!");
                    return;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
                {
                    if (!MyUtility.Check.Seek(sqlcmd, listSqlPar))
                    {
                        this.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                        return;
                    }
                }
            }
        }
    }
}
