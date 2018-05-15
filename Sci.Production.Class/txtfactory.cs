using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using System.Data.SqlClient;

namespace Sci.Production.Class
{
    public partial class txtfactory : Sci.Win.UI.TextBox
    {
        private bool _IssupportJunk = false;
        private bool _FilteMDivision = false;

        [Description("是否要顯示 Junk 的資料")]
        public bool IssupportJunk
        {
            get { return _IssupportJunk; }
            set { _IssupportJunk = value; }
        }
        
        [Description("是否需要排除非同 MDivition 的資料")]
        public bool FilteMDivision
        {
            get { return _FilteMDivision; }
            set { _FilteMDivision = value; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            #region SQL Parameter
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            #endregion
            #region SQL Filte
            List<string> listFilte = new List<string>();
            if (!IssupportJunk)
            {
                listFilte.Add("Junk = 0");
            }
            if (FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision");
            }
            #endregion
            #region SQL CMD
            string sqlcmd = string.Format(@"
Select DISTINCT FtyGroup as Factory 
from Production.dbo.Factory WITH (NOLOCK) 
{0}
order by FtyGroup", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : "");
            #endregion
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlcmd, listSqlPar, "8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
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
            listFilte.Add("FtyGroup = @str");
            if (!IssupportJunk)
            {
                listFilte.Add("Junk = 0");
            }
            if (FilteMDivision)
            {
                listFilte.Add("MDivisionID = @MDivision");
            }
            #endregion
            #region SQL CMD
            string sqlcmd = string.Format(@"
Select DISTINCT FtyGroup
from Production.dbo.Factory WITH (NOLOCK) 
{0}", (listFilte.Count > 0) ? "where " + listFilte.JoinToString("\n\rand ") : "");
            #endregion
            if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
            {
                if (MyUtility.Check.Seek(sqlcmd, listSqlPar) == false)
                {
                    this.Text = "";
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

