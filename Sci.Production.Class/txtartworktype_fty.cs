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

namespace Sci.Production.Class
{
    public partial class txtartworktype_fty : Sci.Win.UI.TextBox
    {
        private string m_type = string.Empty;
        private string m_subprocess = string.Empty;

        [Category("Custom Properties")]
        //[Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string cClassify
        {
            set { this.m_type = value; }
            get { return this.m_type; }
        }
       [Category("Custom Properties")]
       // [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string cSubprocess
        {
            set { this.m_subprocess = value; }
            get { return this.m_subprocess; }
        }

        public txtartworktype_fty()
        {
            #region PopUp
            this.PopUp += (s, e) =>
            {
                //Sci.Win.UI.TextBox textBox = (Sci.Win.UI.TextBox) s;
                string sqlwhere = "Where 1=1";
                string sqlcmd = string.Empty;

                if (!string.IsNullOrWhiteSpace(cClassify))
                {
                    sqlwhere = sqlwhere + " And Classify in (" + this.cClassify + ")";
                };

                if (!string.IsNullOrWhiteSpace(cSubprocess))
                {
                    if (this.cSubprocess == "Y")
                    { sqlwhere = sqlwhere + " And IsSubprocess =1 "; }
                    else
                    { sqlwhere = sqlwhere + " And IsSubprocess =0 "; };
                };
                sqlcmd = "select id, Abbreviation from artworktype " + sqlwhere + " order by seq";
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("artworktype.id,Abbreviation", "20,10", this.Text, false, ",");
                // SELECT Id,sid FROM ArtworkType ORDER BY Code WHERE &csWhere INTO CURSOR ART_Class
                DialogResult result = item.ShowDialog();
                if (result == DialogResult.Cancel) { return; }
                this.Text = item.GetSelectedString();
            };
            #endregion

            #region Validating
            this.Validating += (s, e) =>
            {
                string str = this.Text;
                if (!string.IsNullOrWhiteSpace(str) && str != this.OldValue)
                {
                    string tmp = myUtility.Lookup("id", str, "Artworktype", "id");
                    if (string.IsNullOrWhiteSpace(tmp))
                    {
                        MessageBox.Show(string.Format("< Artworktype : {0} > not found!!!", str));
                        this.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.Text = tmp;
                    }

                }
            };
            #endregion
        }
    }
}
