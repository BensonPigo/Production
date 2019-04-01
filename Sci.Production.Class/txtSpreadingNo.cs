using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sci.Win.UI;
using Sci.Data;
using Sci;
using Ict;
using Sci.Win;


namespace Sci.Production.Class
{
  
    public partial class txtSpreadingNo : Sci.Win.UI.TextBox
    {
        //private string Where = "";   //" Where junk = 0";

        private string Where = "WHERE 1=1";

        [Category("Custom Properties")]
        private string _MDivisionID="";
        public string MDivision
        {
            get { return _MDivisionID; }
            set { _MDivisionID = value; }
        }


        [Category("Custom Properties")]
        private bool _IncludeJunk = true;
        public bool IncludeJunk
        {
            get { return _IncludeJunk ; }
            set { _IncludeJunk = value; }
        }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string sql;
            if (!string.IsNullOrWhiteSpace(MDivision))
            {
                Where += $"AND MDivisionID = '{MDivision}'";
            }

            //不包含Junk，因此去除Junk = 1 的資料
            if (!_IncludeJunk)
            {
                Where += $"AND junk = 0 ";
            }
            
            sql = "select distinct id,CutCell= CutCellID from SpreadingNo WITH (NOLOCK) " + Where;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, string.Empty, this.Text, false, ",");
            item.Width = 300;
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string newValue = this.Text;
            bool isJunked = false;
            if (!string.IsNullOrWhiteSpace(newValue) && newValue != this.OldValue)
            {
                string tmp = null;
                if (!string.IsNullOrWhiteSpace(MDivision) && _IncludeJunk)
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{MDivision}'");
                else if (string.IsNullOrWhiteSpace(MDivision) && _IncludeJunk)
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' ");
                else if (!string.IsNullOrWhiteSpace(MDivision) && !_IncludeJunk)
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{MDivision}' AND Junk=0");
                else if (string.IsNullOrWhiteSpace(MDivision) && !_IncludeJunk)
                    tmp = MyUtility.GetValue.Lookup($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{MDivision}' AND Junk=0");
               

                if (!string.IsNullOrWhiteSpace(MDivision))
                    isJunked = MyUtility.Check.Seek($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND MDivisionid='{MDivision}' AND Junk=1");
                else
                    isJunked = MyUtility.Check.Seek($"SELECT ID FROM SpreadingNo WHERE ID = '{newValue}' AND Junk=1");



                if (string.IsNullOrWhiteSpace(tmp))
                {
                    this.Text = "";
                    e.Cancel = true;

                    if (isJunked)
                        MyUtility.Msg.WarningBox(string.Format("Spreading No already junk, you can't choose!!"));
                    else
                        MyUtility.Msg.WarningBox(string.Format("< Spreading No > : {0} not found!!!", newValue));

                    return;
                }

            }
        }
        public txtSpreadingNo()
        {
            this.Width = 45;
        }

    }
}