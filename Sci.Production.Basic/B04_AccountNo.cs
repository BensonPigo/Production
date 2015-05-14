using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using Ict;

namespace Sci.Production.Basic
{
    public partial class B04_AccountNo : Sci.Win.Subs.Input4
    {
        public B04_AccountNo(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.displayBox1.Text = this.KeyValue1;
            this.displayBox2.Text = myUtility.Lookup("Abb", this.KeyValue1, "LocalSupp", "ID");
        }

        protected override DualResult OnRequery()
        {
            string selectCommand = string.Format("select '{0}' as ID,a.ID as ArtworkTypeID, a.Seq, b.AccountNo, b.AddName,b.AddDate, b.EditName, b.EditDate from (select ID,Seq from ArtworkType where IsSubprocess = 1 or Classify = 'P' or Seq BETWEEN 'A' and 'Z') a left join (select ArtworkTypeID,AccountNo,AddName,AddDate,EditName,EditDate from LocalSupp_AccountNo where ID = '{0}') b on a.ID = b.ArtworkTypeID order by a.Seq", this.KeyValue1);
            Ict.DualResult returnResult;
            DataTable ArtworkTable = new DataTable();
            returnResult = DBProxy.Current.Select(null, selectCommand, out ArtworkTable);
            if (!returnResult)
            {
                return returnResult;
            }
            SetGrid(ArtworkTable);
            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("ArtworkTypeID", header: "Type", width: Widths.AnsiChars(20),iseditable:false)
                .Text("AccountNo", header: "Account No", width: Widths.AnsiChars(10));

            return true;
        }

        protected override bool OnSave()
        {
            ITableSchema ts;
            DualResult rs = DBProxy.Current.GetTableSchema(null, "LocalSupp_AccountNo", out ts);
            IList<DataRow> ds = Datas;
            
            foreach (DataRow dr in ds)
            {
                if (dr.RowState == DataRowState.Modified)
                {
                    if (String.IsNullOrWhiteSpace(dr["AccountNo"].ToString()))
                    {
                        rs = DBProxy.Current.Delete(null, ts, dr);
                        if (rs != Result.True)
                        {
                            MessageBox.Show(rs.ToString());
                            return false;
                        }
                    }
                    else
                    {
                        string selectCommand = string.Format("select ID from LocalSupp_AccountNo where ID = '{0}' and ArtworkTypeID = '{1}'", dr["ID"].ToString(),dr["ArtworkTypeID"].ToString());
                        if (!myUtility.Seek(selectCommand, null))
                        {
                            rs = DBProxy.Current.Insert(null, ts, dr);
                            if (rs != Result.True)
                            {
                                MessageBox.Show(rs.ToString());
                                return false;
                            }
                        }
                        else
                        {
                            bool different;
                            rs = DBProxy.Current.UpdateByChanged(null, ts, dr, out different);
                            if (rs != Result.True)
                            {
                                MessageBox.Show(rs.ToString());
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
