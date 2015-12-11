using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Subcon
{
    public partial class P01_ModifyPoQty : Sci.Win.Subs.Base
    {
        protected DataRow dr;
        protected int sum_order_qty = 0;
        public P01_ModifyPoQty(DataRow data)
        {
            InitializeComponent();
            dr = data;
            this.displayBox1.Text = dr["orderid"].ToString();
            this.displayBox2.Text = dr["patterncode"].ToString();
            this.displayBox3.Text = dr["artworkid"].ToString();
            this.displayBox4.Text = dr["patterndesc"].ToString();
            this.displayBox5.Text = dr["farmout"].ToString();
            this.displayBox7.Text = dr["farmin"].ToString();
            this.displayBox6.Text = dr["apqty"].ToString();
            this.displayBox9.Text = dr["exceedqty"].ToString();
            this.numericBox1.Text = dr["poqty"].ToString();

            DataTable tmpdt;
            Sci.Data.DBProxy.Current.Select(null, string.Format("select sum(qty) from order_qty where id = '{0}'", data["orderid"]), out tmpdt);
            if (MyUtility.Check.Empty(tmpdt.Rows[0][0]))
            { this.displayBox8.Text = ""; }
            else
            { sum_order_qty = int.Parse(tmpdt.Rows[0][0].ToString()); this.displayBox8.Text = tmpdt.Rows[0][0].ToString(); }

        }

        private void numericBox1_Validated(object sender, EventArgs e)
        {
            dr["poqty"] = ((Sci.Win.UI.NumericBox)sender).Value;
            if (((Sci.Win.UI.NumericBox)sender).Value > sum_order_qty)
            {
                dr["exceedqty"] = ((Sci.Win.UI.NumericBox)sender).Value - sum_order_qty;
                this.displayBox9.Text = dr["exceedqty"].ToString();
            }
            else
            {
                dr["exceedqty"] = 0;
                this.displayBox9.Text = dr["exceedqty"].ToString();
            }
        }

        private void numericBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!(dr["apqty"] == DBNull.Value) && ((Sci.Win.UI.NumericBox)sender).Value < int.Parse(dr["apqty"].ToString()))
            {
                MyUtility.Msg.WarningBox("PO Qty can't less than AP Qty", "Wanring");
                e.Cancel = true;
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DualResult result, result2;
            DataTable dt_out;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    string sqlcmd = string.Format(@" select id from artworkpo_detail
                                where exceedqty > 0 and id = '{0}'
                                and orderid != '{1}'
                                and artworktypeid != '{2}'
                                and artworkid !='{3}'
                                and patterncode != '{4}'", dr["poqty"]
                                                                 , dr["exceedqty"]
                                                                 , dr["id"]
                                                                 , dr["orderid"]
                                                                 , dr["artworktypeid"]
                                                                 , dr["artworkid"]
                                                                 , dr["patterncode"]);
                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out dt_out)))
                    {
                        ShowErr(sqlcmd, result);
                        return;
                    }

                    int flag = 0;
                    if (!(dt_out.Rows.Count == 0 && dr["exceedqty"].ToString()=="0"))
                    {
                        flag = 1;
                    }

                    string sqlcmd2 = string.Format(@"
                                update artworkpo
                                set exceed = {0}
                                    ,editdate = getdate()
                                    ,editname = '{1}'
                                where id = '{2}'", flag, Env.User.UserID, dr["id"].ToString());
                    result = DBProxy.Current.Execute(null, sqlcmd2);

                    sqlcmd = string.Format(@" update artworkpo_detail 
                                set poqty = {0}, exceedqty = {1}
                                where id = '{2}'
                                and orderid = '{3}'
                                and artworktypeid = '{4}'
                                and artworkid ='{5}'
                                and patterncode = '{6}'", dr["poqty"], dr["exceedqty"], dr["id"], dr["orderid"], dr["artworktypeid"], dr["artworkid"], dr["patterncode"]);
                    result2 = DBProxy.Current.Execute(null, sqlcmd);
                    if (result && result2)
                    {
                        _transactionscope.Complete();
                        MyUtility.Msg.WarningBox("Save sucessful");
                    }
                    else
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Save Error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
