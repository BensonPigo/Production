using System;
using System.ComponentModel;
using System.Data;

using Ict;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Subcon
{
    public partial class P01_ModifyPoQty : Win.Subs.Base
    {
        protected DataRow dr;
        protected int sum_order_qty = 0;

        public P01_ModifyPoQty(DataRow data)
        {
            this.InitializeComponent();
            this.dr = data;
            this.displaySPNo.Text = this.dr["orderid"].ToString();
            this.displayCutpartID.Text = this.dr["patterncode"].ToString();
            this.displayArtwork.Text = this.dr["artworkid"].ToString();
            this.displayCutpartName.Text = this.dr["patterndesc"].ToString();
            this.displayFarmOut.Text = this.dr["farmout"].ToString();
            this.displayFarmIn.Text = this.dr["farmin"].ToString();
            this.displayAPQty.Text = this.dr["apqty"].ToString();
            this.displayExceedQty.Text = this.dr["exceedqty"].ToString();
            this.numPOQty.Text = this.dr["poqty"].ToString();

            DataTable tmpdt;
            Sci.Data.DBProxy.Current.Select(null, string.Format("select sum(qty) from order_qty WITH (NOLOCK) where id = '{0}'", data["orderid"]), out tmpdt);
            if (MyUtility.Check.Empty(tmpdt.Rows[0][0]))
            {
                this.displayCurrentOrderQty.Text = string.Empty;
            }
            else
            {
                this.sum_order_qty = int.Parse(tmpdt.Rows[0][0].ToString());
                this.displayCurrentOrderQty.Text = tmpdt.Rows[0][0].ToString();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.dr.RejectChanges();
            base.OnClosed(e);
        }

        private void numPOQty_Validated(object sender, EventArgs e)
        {
            this.dr["poqty"] = ((Win.UI.NumericBox)sender).Value;
            if (((Win.UI.NumericBox)sender).Value > this.sum_order_qty)
            {
                this.dr["exceedqty"] = ((Win.UI.NumericBox)sender).Value - this.sum_order_qty;
                this.displayExceedQty.Text = this.dr["exceedqty"].ToString();
            }
            else
            {
                this.dr["exceedqty"] = 0;
                this.displayExceedQty.Text = this.dr["exceedqty"].ToString();
            }

            this.dr["amount"] = Convert.ToDecimal(this.dr["poqty"]) * Convert.ToDecimal(this.dr["price"]);
        }

        private void numPOQty_Validating(object sender, CancelEventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!(this.dr["apqty"] == DBNull.Value) && (this.numPOQty.Value < int.Parse(this.dr["apqty"].ToString())))
            {
                this.ShowErr("PO Qty can't less than AP Qty");
                return;
            }

            DualResult result, result2;
            DataTable dt_out;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    string sqlcmd = string.Format(
                        @"
select id from artworkpo_detail WITH (NOLOCK) 
where exceedqty > 0 and id = '{0}'
and orderid != '{1}'
and artworktypeid != '{2}'
and artworkid !='{3}'
and patterncode != '{4}'",
                        this.dr["poqty"], this.dr["exceedqty"], this.dr["id"], this.dr["orderid"], this.dr["artworktypeid"], this.dr["artworkid"], this.dr["patterncode"]);
                    if (!(result = DBProxy.Current.Select(null, sqlcmd, out dt_out)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlcmd, result);
                        return;
                    }

                    int flag = 0;
                    if (!(dt_out.Rows.Count == 0 && this.dr["exceedqty"].ToString() == "0"))
                    {
                        flag = 1;
                    }

                    string sqlcmd2 = string.Format(
                        @"
                                update artworkpo
                                set exceed = {0}
                                    ,editdate = getdate()
                                    ,editname = '{1}'
                                where id = '{2}'", flag, Env.User.UserID, this.dr["id"].ToString());
                    result = DBProxy.Current.Execute(null, sqlcmd2);

                    sqlcmd = string.Format(
                        @" update artworkpo_detail 
                                set poqty = {0}, exceedqty = {1}
                                ,amount={7}
                                where id = '{2}'
                                and orderid = '{3}'
                                and artworktypeid = '{4}'
                                and artworkid ='{5}'
                                and patterncode = '{6}'", this.dr["poqty"], this.dr["exceedqty"], this.dr["id"], this.dr["orderid"], this.dr["artworktypeid"], this.dr["artworkid"], this.dr["patterncode"], this.dr["amount"]);
                    result2 = DBProxy.Current.Execute(null, sqlcmd);
                    if (result && result2)
                    {
                        _transactionscope.Complete();
                        _transactionscope.Dispose();
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
                    this.ShowErr("Save Error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.dr.RejectChanges();
            this.Close();
        }
    }
}
