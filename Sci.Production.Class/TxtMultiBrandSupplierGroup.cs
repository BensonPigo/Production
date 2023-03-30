using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Ict;

using Sci.Win.UI;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public class TxtMultiBrandSupplierGroup : Sci.Win.UI.TextBox
    {
        /// <inheritdoc/>
        public TxtMultiBrandSupplierGroup()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public DataTable suppData = null;

        private Control myBrand;    // 欄位.存入要取值的<控制項>

        /// <inheritdoc />
        [Category("Custom Properties")]
        [Description("選擇畫面上[Brand]的控制項名稱，僅篩選出對應該Brand的資料")]
        public Control myBrandName
        {
            get
            {
                return this.myBrand;
            }

            set
            {
                this.myBrand = value;
            }
        }

        /// <inheritdoc/>
        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            // if (!this.IsSupportEditMode) { return; }
            string selecteds = this.Text;
            string sqlCMD = $@"Select RTRIM(s1.ID) as ID, RTRIM(s2.ID) as SuppGroupID,s1.AbbEN as Name,s1.CountryID as Country From Supp s1                              
                              Inner Join dbo.BrandRelation as bs WITH (NOLOCK) ON bs.BrandID = '{this.myBrand.Text}' and bs.SuppID = s1.ID
                              inner join Supp s2 on bs.SuppGroup = s2.ID
                              Where s2.junk =0 
                              GROUP BY s1.ID,s2.ID,s1.AbbEN,s1.CountryID
                              Order By s1.ID";
            Sci.Win.Tools.SelectItem2 item = this.suppData == null
                ? new Sci.Win.Tools.SelectItem2(sqlCMD, "ID,Supp Group,Name,COUNTRY", "6,6,20,4", selecteds)
                : new Sci.Win.Tools.SelectItem2(this.suppData, "ID,Supp Group,Name,Country", "ID,Name,Country", "6,6,20,4", selecteds);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            // MessageBox.Show(item.GetSelectedString());
            IList<DataRow> list = item.GetSelecteds();

            if (list.Count == 0)
            {
                this.Text = string.Empty;
            }
            else
            {
                this.Text = string.Join(
                    ",", list.Select(row => row["SuppGroupID"].ToString()).Distinct()
                              .OrderBy(id => id));
            }

            this.ValidateControl();
            /*
            for (int i = 0; i < list.Count; i++)
            {


                if (i == 0) this.Text = (String)list[i].GetValue("ID");
                else this.Text += "," + list[i].GetValue("ID");
            }*/
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Size = new System.Drawing.Size(200, 22);
            this.IsSupportEditMode = false;
            this.PopUpMode = TextBoxPopUpMode.EditModeAndReadOnly;
            this.ReadOnly = true;
            this.ResumeLayout(false);
        }
    }
}