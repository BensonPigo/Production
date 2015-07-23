using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Ict;
using Ict.Win;
using Sci.Win;
using Sci.Data;

namespace Sci.Production.Tools
{
    public partial class AuthorityByPosition_History : Sci.Win.Subs.Input4
    {
        private DataTable dtPass1 = null;
        private DualResult result = null;
        private DataRow findedData = null;

        public AuthorityByPosition_History(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();

            //if (!(result = DBProxy.Current.Select(null, "SELECT * FROM Pass1", out dtPass1)))
            //{
            //    MyUtility.Msg.ErrorBox(result.ToString());
            //    this.Close();
            //}
            //dtPass1.PrimaryKey = new DataColumn[] { dtPass1.Columns["ID"] };
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
               .Text("Modifier", header: "Modifier", width: Widths.AnsiChars(20), iseditingreadonly: true)
               .DateTime("AddDate", header: "Modify Date", width: Widths.AnsiChars(20), iseditingreadonly: true, format:DataGridViewDateTimeFormat.yyyyMMddHHmmss)
               .EditText("Remark", header: "Remark", width: Widths.AnsiChars(30), iseditingreadonly: false)
               .Text("Editby", header: "Edit by", width: Widths.AnsiChars(40), iseditingreadonly: true);

            return true;
        }

        protected override void OnRequeryPost(DataTable datas)
        {

            base.OnRequeryPost(datas);

            datas.Columns.Add("Modifier");
            datas.Columns.Add("Editby");
            foreach (DataRow dr in datas.Rows)
            {
                dr["Modifier"] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(dr["AddName"], format: (int)
Sci.Production.PublicPrg.Prgs.Pass1Format.NameExt);

               // DateTime dd = (DateTime)dr["EditDate"];

                dr["Editby"] = Sci.Production.PublicPrg.Prgs.GetAddOrEditBy(dr["EditName"], dateColumn: dr["EditDate"], format: (int)
Sci.Production.PublicPrg.Prgs.Pass1Format.IDNameExtDateTime);

                //findedData = dtPass1.Rows.Find(dr["AddName"].ToString());
                //dr["Modifier"] = (findedData != null) ? 
                //                     (findedData["Name"].ToString() + "   Ext. " + findedData["ExtNo"].ToString()) :
                //                     dr["AddName"].ToString();

                //findedData = dtPass1.Rows.Find(dr["EditName"].ToString());
                //dr["Editby"] = (findedData != null) ? 
                //                   (dr["EditName"].ToString() + " - " + findedData["Name"].ToString() + "  Ext." + findedData["ExtNo"].ToString() + "  " + ((DateTime)dr["EditDate"]).ToAppDateTimeFormatString()) :
                //                   dr["EditName"].ToString();
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            this.append.Visible = false;
            this.revise.Visible = false;
            this.delete.Visible = false;
        }
    }
}
