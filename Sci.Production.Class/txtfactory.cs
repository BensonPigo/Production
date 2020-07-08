using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sci.Win.UI;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txtfactory : Win.UI.TextBox
    {
        private bool _IssupportJunk = false;
        private bool _FilteMDivision = false;
        private bool _boolFtyGroupList = true;
        private bool _IsProduceFty = false;
        private Object _MDivision;

        [Description("是否要顯示 Junk 的資料")]
        public bool IssupportJunk
        {
            get { return this._IssupportJunk; }
            set { this._IssupportJunk = value; }
        }

        [Description("是否需要排除非同 MDivition 的資料")]
        public bool FilteMDivision
        {
            get { return this._FilteMDivision; }
            set { this._FilteMDivision = value; }
        }

        [Description("是否只顯示 IsProduceFty 的資料")]
        public bool IsProduceFty
        {
            get { return this._IsProduceFty; }
            set { this._IsProduceFty = value; }
        }

        [Description("M元件")]
        public Object MDivision
        {
            get { return this._MDivision; }
            set { this._MDivision = value; }
        }

        /// <summary>
        /// boolFtyGroupList
        /// 預設值 True
        /// True：替換單字 FtyGroup
        /// False：替換單字 ID
        /// </summary>
        public bool boolFtyGroupList
        {
            get
            {
                return this._boolFtyGroupList;
            }

            set
            {
                this._boolFtyGroupList = value;
            }
        }

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
                listSqlPar.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
                listFilte.Add("MDivisionID = @MDivision");
            }

            if (this._MDivision != null && !MyUtility.Check.Empty(((Win.UI.TextBox)this._MDivision).Text))
            {
                listSqlPar.Add(new SqlParameter("@MDivision", ((Win.UI.TextBox)this._MDivision).Text));
                listFilte.Add("MDivisionID = @MDivision");
            }
            #endregion
            #region SQL CMD

            // 依boolFtyGroupList=true 顯示FtyGroup 反之顯示ID
            string strShowColumn = string.Empty;
            if (this.boolFtyGroupList)
            {
                strShowColumn = "DISTINCT FtyGroup";
            }
            else
            {
                strShowColumn = "ID";
            }

            string sqlcmd = string.Format(
                @"
Select {1} as Factory 
from Production.dbo.Factory WITH (NOLOCK) 
{0}
order by FtyGroup", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : string.Empty, strShowColumn);
            #endregion
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, listSqlPar, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
            this.ValidateText();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string str = this.Text;
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
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
            #region SQL CMD

            // 依boolFtyGroupList=true 顯示FtyGroup 反之顯示ID
            string strShowColumn = string.Empty;
            if (this.boolFtyGroupList)
            {
                strShowColumn = "DISTINCT FtyGroup";
                listFilte.Add("FtyGroup = @str");
            }
            else
            {
                strShowColumn = "ID";
                listFilte.Add("id = @str");
            }

            string sqlcmd = string.Format(
                @"
Select {1}
from Production.dbo.Factory WITH (NOLOCK) 
{0}", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : string.Empty, strShowColumn);
            #endregion
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd, listSqlPar) == false)
                {
                    this.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Factory : {0} > not found!!!", str));
                    return;
                }
            }
        }

        public txtfactory()
        {
            this.Size = new System.Drawing.Size(66, 23);
        }
    }
}
